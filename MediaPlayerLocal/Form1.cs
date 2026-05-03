using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using LibVLCSharp.WinForms;
using LibVLCSharp.Shared;
using MediaModel;
using MediaPresenter;
using System.Threading.Tasks;


namespace local
{
    public partial class Form1 : Form
    {
        private IPlaylistManager _playlistManager;
        private IPresenter _presenter;

        private LibVLC _vlc;
        private MediaPlayer _player;

        private bool _isUserSeeking;

        /// <summary>
        /// Proprietate ce controleaza obiectul Media care este redat de MediaPlayer
        /// </summary>
        /// <remarks>
        /// Proprietatea seteaza si valoarea maxima pentru trackBar ca fiind
        /// durata fisierului in milisecunde
        /// </remarks>
        public Media CurrentMedia
        {
            get { return _player.Media; }
            set 
            { 
                _player.Media = value;
                trackBarMediaSeek.Maximum = (int)_player.Media.Duration;
            }
        }

        public Form1()
        {
            InitializeComponent();

            Core.Initialize();

            // Creare componente
            _vlc = new LibVLC();
            _player = new MediaPlayer(_vlc);

            Panel videoPanel = new Panel()
            {
                Dock = DockStyle.Fill,
            };
            
            _playlistManager = new PlaylistManager(_vlc);


            // Atasare MediaPlayer la un control de tip VideoView specific din WinForms
            videoView.MediaPlayer = _player;

            // Adaugare eveniment la timer.Tick
            timerUpdateUI.Tick += (sender, args) =>
            {
                // La fiecare tick, se modifica bara de redare cu timpul de redare
                if (_player.IsPlaying)
                {
                    trackBarMediaSeek.Value = (int)_player.Time;
                }
            };

        }

        /// <summary>
        /// Functie asincrona de deschidere a unui fisier si creare continut de tip Media pentru redare
        /// </summary>
        /// <remarks>
        /// Se opteaza pentru un control ce lucreaza asincron, astfel incat parsarea fisierelor Media sa fie cat mai fluida
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Media Files|*.mp4;*.mp3;*.avi;*.wav;*.mkv;*.m4a" ;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Creare continut media
                    Uri uri = new Uri(dlg.FileName);
                    Media media = new Media(_vlc, uri);

                    // Dizabilitare buton pentru deschiderea unui alt fisier
                    buttonOpenFile.Enabled = false;

                    // Se asteapta parsarea noului media object
                    MediaParsedStatus parsedResult = await media.Parse(MediaParseOptions.ParseLocal);

                    // Reabilitare buton pentru deschiderea unui alt fisier
                    buttonOpenFile.Enabled = true;

                    // Daca parsarea a avut succes, se incepe redarea noului media
                    if (parsedResult == MediaParsedStatus.Done)
                    {
                        StartMedia(media);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Functie asincrona de deschidere a unui fisier si creare continut de tip Media
        /// care va fi adaugat unei Liste de obiecte Media (controlat de modelul playlistManager) 
        /// </summary>
        /// <remarks>
        /// Se opteaza si aici pentru un control ce lucreaza asincron
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Media Files|*.mp4;*.mp3;*.avi;*.wav;*.mkv;*.m4a";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                // Se creaza resursa ce identifica fisierul selectat
                Uri uri = new Uri(dlg.FileName);

            // Se dizabiliteaza TEMPORAR butonul de incarcare a fisierelor,
            // deoarece parsarea presupune o operatie mai costisitoare,
            // iar controlul nu trebuie folosit pana la finalizarea parsarii
                buttonLoadFile.Enabled = false;

                // Se trimite resursa catre modelul playlist-ului care va crea si parsa noul obiect Media
                bool addResult = await _playlistManager.Add(uri);

                buttonLoadFile.Enabled = true;

                // Daca operatia s-a finalizat fara erori
                if (addResult == true)
                {
                    // se actualizeaza controlul de tip lista cu noul media
                    UpdateMediaList();
                }
                else
                {
                    MessageBox.Show("Cannot add selected file");
                }
                //presenter.AddMedia(media);
            }
        }

        /// <summary>
        /// Cere modelului o lista cu toate titlurile fisierelor media adaugate, dupa care le incarca in listBox
        /// </summary>
        private void UpdateMediaList()
        {
            List<string> titleList = _playlistManager.ListAll();
            listBoxTitles.Items.Clear();

            foreach (string title in titleList)
            {
                listBoxTitles.Items.Add(title);
            }
        }

        /// <summary>
        /// Interschimba starea de play/pause a fisierului ce este in redare
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPlayMedia_Click(object sender, EventArgs e)
        {
            if(!_player.IsPlaying)
            {
                _player.Play();
                buttonPlayMedia.Text = "PAUSE";
            }
            else
            {
                _player.Pause();
                buttonPlayMedia.Text = "PLAY";
            }


        }


        /// <summary>
        /// Selecteaza o valoare din listBox pentru care se doreste a reda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxTitles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string title = listBoxTitles.Text;

            // Cautarea se face prin modelul playlist-ului
            Media selectedMedia = _playlistManager.GetMedia(title);

            if (selectedMedia != null)
            {
                StartMedia(selectedMedia);
            }

        }

        /// <summary>
        /// Seteaza obiectul media curent, incepe redarea si afiseaza informatiile despre media
        /// </summary>
        /// <param name="media"></param>
        private void StartMedia(Media media)
        {
            CurrentMedia = media;
            _player.Play();
            buttonPlayMedia.Text = "PAUSE";
            textBoxCurrentMediaTitle.Text = CurrentMedia.Meta(MetadataType.Title);
        }

        /// <summary>
        /// Seteaza flag ce indica ca utilizatorul a inceput functia de Seek
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBarMediaSeek_MouseDown(object sender, MouseEventArgs e)
        {
            _isUserSeeking = true;
        }

        /// <summary>
        /// Reseteaza flag ce indica ca utilizatorul a inceput functia de Seek si modifica timpul redarii cu cel cautat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBarMediaSeek_MouseUp(object sender, MouseEventArgs e)
        {
            _isUserSeeking = false;

            // apply final position
            _player.Time = trackBarMediaSeek.Value;
        }

        /// <summary>
        /// Cat timp se modifica trackBar, se cauta si un nou timp de redare
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBarMediaSeek_Scroll(object sender, EventArgs e)
        {
            if (_isUserSeeking)
            {
                // optional: live seeking while dragging
                _player.Time = trackBarMediaSeek.Value;
            }
        }
    }
}
