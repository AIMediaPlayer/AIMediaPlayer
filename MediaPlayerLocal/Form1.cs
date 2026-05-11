using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using LibVLCSharp.WinForms;
using LibVLCSharp.Shared;
using MediaModel;
using MediaPresenter;
using System.Threading.Tasks;
using MediaPlayerLocal;
using LibVLCSharp.Shared.Structures;
using System.Drawing;
using System.Security.Cryptography;

namespace local
{
    /// <summary>
    /// Reprezintă fereastra principală a aplicației de redare media locală.
    /// Gestionează interfața utilizator cu ajutorul LibVLCSharp pentru redare video și audio.
    /// </summary>
    public partial class Form1 : Form
    {
        private IPlaylistManager _playlistManager;
        private IPresenter _presenter;

        private LibVLC _vlc;
        private MediaPlayer _player;
        private Media _currentMedia;
        private MediaProgressBar _mediaProgressBar;

        private string _currentPlaylistPath;
        private bool _isUserSeeking;

        /// <summary>
        /// Obține sau setează obiectul <see cref="Media"/> curent pregătit pentru redare.
        /// </summary>
        /// <value>Instanța media care va fi încărcată în player.</value>
        public Media CurrentMedia
        {
            get { return _currentMedia; }
            set
            {
                _currentMedia = value;
            }
        }

        /// <summary>
        /// Setează sursa media a player-ului și actualizează afișarea timpului în interfață.
        /// </summary>
        /// <value>Obiectul media ce urmează a fi redat imediat de către <see cref="_player"/>.</value>
        public Media SetPlayer
        {
            set
            {
                _player.Media = value;
                UpdateTimeSpan();
            }
        }

        /// <summary>
        /// Inițializează o nouă instanță a clasei <see cref="Form1"/>.
        /// Configurează motorul VLC, player-ul, managerul de playlist și evenimentele de actualizare UI.
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            Core.Initialize();

            _vlc = new LibVLC();
            _player = new MediaPlayer(_vlc);
            _playlistManager = new PlaylistManager(_vlc);
            _mediaProgressBar = new MediaProgressBar();
            _presenter = new Presenter(_playlistManager);

            videoView.MediaPlayer = _player;

            _player.EndReached += (s, e) =>
            {
                var next = _presenter.Next();

                if (next != null)
                {
                    CurrentMedia = next;
                    BeginInvoke(new Action(StartMedia));
                }
            };

            timerUpdateUI.Tick += (sender, args) =>
            {
                // La fiecare tick, se modifica timpul de redare in mod dinamic
                if (_player.Media != null)
                {
                    mediaProgressBar.Value = (float)_player.Time / _player.Media.Duration;
                    mediaProgressBar.Invalidate();
                    UpdateTimeSpan();
                }
            };


            // Adaugare eveniment de actualizare a listei de subtitrari disponibile
            _player.Playing += (sender, args) =>
            {
                UpdateSubtitleList();
            };

        }

        /// <summary>
        /// Gestionează evenimentul de click pentru deschiderea și redarea imediată a unui fișier.
        /// </summary>
        /// <remarks>
        /// Utilizează parsarea asincronă pentru a evita blocarea thread-ului de UI în timpul procesării metadatelor fișierului.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Media Files|*.mp4;*.mp3;*.avi;*.wav;*.mkv;*.m4a";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Uri uri = new Uri(dlg.FileName);
                    Media media = new Media(_vlc, uri);

                    // Se dizabiliteaza TEMPORAR butonul de incarcare a fisierelor,
                    // deoarece parsarea presupune o operatie mai costisitoare,
                    // iar controlul nu trebuie folosit pana la finalizarea parsarii
                    buttonOpenFile.Enabled = false;

                    MediaParsedStatus parsedResult = await media.Parse(MediaParseOptions.ParseLocal);

                    buttonOpenFile.Enabled = true;

                    if (parsedResult == MediaParsedStatus.Done)
                    {
                        CurrentMedia = media;
                        StartMedia();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Gestionează adăugarea unui fișier media în lista de redare (Playlist).
        /// </summary>
        /// <remarks>
        /// Operația este asincronă deoarece adăugarea implică parsarea fișierului de către <see cref="_playlistManager"/>.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Media Files|*.mp4;*.mp3;*.avi;*.wav;*.mkv;*.m4a";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Uri uri = new Uri(dlg.FileName);

                // Se dizabiliteaza TEMPORAR butonul de incarcare a fisierelor,
                // deoarece parsarea presupune o operatie mai costisitoare,
                // iar controlul nu trebuie folosit pana la finalizarea parsarii
                buttonLoadFile.Enabled = false;

                bool addResult = await _playlistManager.Add(uri);

                buttonLoadFile.Enabled = true;

                if (addResult == true)
                {
                    UpdateMediaList();
                }
                else
                {
                    MessageBox.Show("Cannot add selected file");
                }
            }
        }

        /// <summary>
        /// Actualizează elementele vizuale din <c>listBoxTitles</c> cu titlurile disponibile în playlist.
        /// </summary>
        private void UpdateMediaList()
        {
            // se apeleaza metoda de listare specifica din modelul playlistului
            List<string> titleList = _playlistManager.ListAll();

            // afiseaza fisierele disponibile in playlist
            listBoxTitles.Items.Clear();
            foreach (string title in titleList)
            {
                listBoxTitles.Items.Add(title);
            }
        }

        /// <summary>
        /// Controlează starea de redare (Play/Pause) a player-ului.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPlayMedia_Click(object sender, EventArgs e)
        {
            // se verifica daca player-ul are atasat un obiect Media
            if (!_player.WillPlay)
            {
                // daca nu are niciun fisier media atasat 
                StartMedia();
            }
            else
            {
                // daca are un fisier media atasat
                if (!_player.IsPlaying)
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
        }

        /// <summary>
        /// Încarcă media selectată din listă ca fiind obiectul media curent.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxTitles_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBoxTitles.SelectedIndex;

            if (index != -1)
            {
                _playlistManager.SetCurrentIndex(index);

                string title = listBoxTitles.GetItemText(listBoxTitles.Items[index]);

                Media selectedMedia = _playlistManager.GetMedia(title);

                if (selectedMedia != null)
                {
                    CurrentMedia = selectedMedia;
                }
            }
        }

        }

        /// <summary>
        /// Gestionează dublu-click-ul pe un element din listă pentru redare imediată.
        /// </summary>
        private void listBoxTitles_DoubleClick(object sender, MouseEventArgs e)
        {
            int index = listBoxTitles.IndexFromPoint(e.Location);

            if (index != ListBox.NoMatches)
            {
                string title = listBoxTitles.GetItemText(listBoxTitles.Items[index]);

                Media selectedMedia = _playlistManager.GetMedia(title);

                if (selectedMedia != null)
                {
                    CurrentMedia = selectedMedia;
                    StartMedia();
                }
            }
        }

        /// <summary>
        /// Pornește redarea obiectului <see cref="CurrentMedia"/> și actualizează interfața (titlu, butoane).
        /// </summary>
        /// <param name="media"></param>
        private void StartMedia()
        {

            SetPlayer = CurrentMedia;
            _player.Play();
            
            buttonPlayMedia.Text = "PAUSE";
            textBoxCurrentMediaTitle.Text = CurrentMedia.Meta(MetadataType.Title);
            subtitleAddToolStripMenuItem.Enabled = true;
        }

        private void SyncListBoxToCurrent()
        {
            int index = listBoxTitles.SelectedIndex;

            Media current = _playlistManager.GetCurrent();

            for (int i = 0; i < listBoxTitles.Items.Count; i++)
            {
                if (listBoxTitles.Items[i].ToString() == current.Meta(MetadataType.Title))
                {
                    listBoxTitles.SelectedIndex = i;
                    break;
                }
            }
        }

        /// <summary>
        /// Permite căutarea în timp real prin tragerea cursorului pe bara de progres (Drag and Seek).
        /// </summary>
        private void mediaProgressBar_MouseDown(object sender, MouseEventArgs e) { }

        private void mediaProgressBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (_player.WillPlay)
            {
                _player.SeekTo(TimeSpan.FromMilliseconds((long)(mediaProgressBar.Value * _player.Media.Duration)));
            }
        }

        /// <summary>
        /// Actualizează volumul audio la eliberarea cursorului de pe bara de volum.
        /// </summary>
        private void mediaProgressBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (mediaProgressBar.IsDragging)
            {
                if (_player.WillPlay)
                {
                    _player.SeekTo(TimeSpan.FromMilliseconds((long)(mediaProgressBar.Value * _player.Media.Duration)));
                }
            }
        }

        /// <summary>
        /// Actualizează volumul audio în timp real în timpul tragerii cursorului de volum.
        /// </summary>
        private void mediaProgressBarAudio_MouseUp(object sender, MouseEventArgs e)
        {
            if (_player.WillPlay)
            {
                _player.Volume = (int)(mediaProgressBarAudio.Value * 100);
            }
        }

        /// <summary>
        /// Actualizează eticheta text care afișează timpul curent și durata totală a fișierului media.
        /// </summary>
        private void mediaProgressBarAudio_MouseMove(object sender, MouseEventArgs e)
        {
            if (mediaProgressBarAudio.IsDragging)
            {
                if (_player.WillPlay)
                {
                    _player.Volume = (int)(mediaProgressBarAudio.Value * 100);
                }
            }
        }

        /// <summary>
        /// Populează meniul de subtitrări cu pistele disponibile în fișierul media curent.
        /// </summary>
        /// <remarks>
        /// Include logica de thread-safety folosind <c>Invoke</c> pentru a interacționa cu controalele UI din evenimente asincrone VLC.
        /// </remarks>
        private void UpdateTimeSpan()
        {
            TimeSpan currentTimeSpan = TimeSpan.FromMilliseconds(_player.Time);
            TimeSpan totalTimeSpan = TimeSpan.FromMilliseconds(_player.Media.Duration);
            labelMediaTimeSpan.Text = $"{currentTimeSpan:hh\\:mm\\:ss} / {totalTimeSpan:hh\\:mm\\:ss}";
        }

        /// <summary>
        /// Populează meniul de subtitrări cu pistele disponibile în fișierul media curent.
        /// </summary>
        /// <remarks>
        /// Include logica de thread-safety folosind <c>Invoke</c> pentru a interacționa cu controalele UI din evenimente asincrone VLC.
        /// </remarks>
        private void UpdateSubtitleList()
        {
            // se revine in thread-ul UI principal
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new Action(UpdateSubtitleList));
                return;
            }


            subtitleListToolStripMenuItem.DropDownItems.Clear();

            // obiect ce contine informatii despre subtritrarile disponibile fisierului media
            TrackDescription[] descriptions = _player.SpuDescription;
            

            if (descriptions != null && descriptions.Length > 0)
            {
                subtitleListToolStripMenuItem.Enabled = true;

                int currentSubtitleID = _player.Spu;

                // se itereaza fiecare subtitrare
                foreach (TrackDescription item in descriptions)
                {
                    string subtitleName = item.Name;
                    int subtitleID = item.Id;

                    ToolStripMenuItem subtitleItem = new ToolStripMenuItem(subtitleName);
                    subtitleItem.Tag = subtitleID;

                    if(subtitleID == currentSubtitleID)
                    {
                        subtitleItem.Checked = true;
                    }

                    // Acest handler realizează două acțiuni principale:
                    // 1. Instruiește player-ul să schimbe pista de subtitrare activă folosind ID-ul specificat.
                    // 2. Actualizează interfața grafică prin debifarea tuturor celorlalte opțiuni din meniu 
                    //    și bifarea elementului curent (comportament tip "Radio Button").
                    subtitleItem.Click += (sender, args) =>
                    {
                        _player.SetSpu(subtitleID);

                        foreach (ToolStripItem subtitleSiblingItem in subtitleListToolStripMenuItem.DropDownItems)
                        {
                            if (subtitleSiblingItem is ToolStripMenuItem menuItem)
                            {
                                menuItem.Checked = false;
                            }
                        }

                        ((ToolStripMenuItem)sender).Checked = true;
                    };

                    subtitleListToolStripMenuItem.DropDownItems.Add(subtitleItem);
                }
            }
        }

        /// <summary>
        /// Oprește redarea media și resetează starea butoanelor din interfață.
        /// </summary>
        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (_player.IsPlaying)
            {
                _player.Stop();
                buttonPlayMedia.Text = "PLAY";

                subtitleAddToolStripMenuItem.Enabled = false;
                subtitleListToolStripMenuItem.Enabled = false;
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            var media = _presenter.Next();

            if (media != null)
            {
                CurrentMedia = media;
                StartMedia();
                SyncListBoxToCurrent();
            }
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            CurrentMedia = _presenter.Previous();
            StartMedia();
        }

        private void buttonShuffle_Click(object sender, EventArgs e)
        {
            CurrentMedia = _presenter.Shuffle();
            StartMedia();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Playlist (*.json)|*.json";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _currentPlaylistPath = dlg.FileName;

                _playlistManager.Save(dlg.FileName);

                MessageBox.Show("Media curentă salvată în playlist!");
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Playlist (*.json)|*.json";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _currentPlaylistPath = dlg.FileName;

                _playlistManager.Load(dlg.FileName);

                UpdateMediaList();

                var media = _playlistManager.GetCurrent();

                if (media != null)
                {
                    CurrentMedia = media;
                    StartMedia();
                }
            }
        }

        private void buttonSavePlaylist_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "JSON Playlist|*.json";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _currentPlaylistPath = dlg.FileName;

                _playlistManager.SavePlaylist(dlg.FileName);

                MessageBox.Show("Playlist saved!");
            }
        }

        private void buttonRepeatPlaylist_Click(object sender, EventArgs e)
        {
            _playlistManager.Repeat();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listBoxTitles.SelectedIndex == -1)
                return;

            int index = listBoxTitles.SelectedIndex;

            _playlistManager.Remove(index, _currentPlaylistPath);

            UpdateMediaList();
        }

        /// <summary>
        /// Permite utilizatorului să încarce un fișier extern de subtitrare (SRT, ASS etc.) și să îl atașeze redării curente.
        /// </summary>
        private void addSubtitleFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Subtitle Files|*.srt;*.ass;*.ssa;*.sub";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string subtitleName = dlg.SafeFileName;

                    Uri uri = new Uri(dlg.FileName);
                    _player.AddSlave(MediaSlaveType.Subtitle, uri.AbsoluteUri, true);

                    foreach (ToolStripItem subtitleSiblingItem in subtitleListToolStripMenuItem.DropDownItems)
                    {
                        if (subtitleSiblingItem is ToolStripMenuItem menuItem)
                        {
                            menuItem.Checked = false;
                        }
                    }

                    ToolStripMenuItem subtitleItem = new ToolStripMenuItem(subtitleName);
                    subtitleItem.Checked = true;

                    subtitleItem.Click += (menuSender, args) =>
                    {
                        _player.AddSlave(MediaSlaveType.Subtitle, uri.AbsoluteUri, true);
                        foreach (ToolStripItem subtitleSiblingItem in subtitleListToolStripMenuItem.DropDownItems)
                        {
                            if (subtitleSiblingItem is ToolStripMenuItem menuItem)
                            {
                                menuItem.Checked = false;
                            }
                        }
                        ((ToolStripMenuItem)menuSender).Checked = true;
                    };

                    subtitleListToolStripMenuItem.DropDownItems.Add(subtitleItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}