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

namespace local
{
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
        /// Proprietate ce controleaza obiectul Media care este redat de MediaPlayer
        /// </summary>
        /// <remarks>
        /// Proprietatea seteaza si valoarea maxima pentru trackBar ca fiind
        /// durata fisierului in milisecunde
        /// </remarks>
        public Media CurrentMedia
        {
            get { return _currentMedia; }
            set
            {
                _currentMedia = value;
            }
        }

        public Media SetPlayer
        {
            set
            {
                _player.Media = value;
                UpdateTimeSpan();
            }
        }

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
                if (_player.Media != null)
                {
                    mediaProgressBar.Value = (float)_player.Time / _player.Media.Duration;
                    mediaProgressBar.Invalidate();
                    UpdateTimeSpan();
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
            dlg.Filter = "Media Files|*.mp4;*.mp3;*.avi;*.wav;*.mkv;*.m4a";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Uri uri = new Uri(dlg.FileName);
                    Media media = new Media(_vlc, uri);

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
                Uri uri = new Uri(dlg.FileName);

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
            if (!_player.WillPlay)
            {
                StartMedia();
            }
            else
            {
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
        /// Selecteaza o valoare din listBox pentru care se doreste a reda
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
        /// Seteaza obiectul media curent, incepe redarea si afiseaza informatiile despre media
        /// </summary>
        /// <param name="media"></param>
        private void StartMedia()
        {
            SetPlayer = CurrentMedia;
            _player.Play();
            buttonPlayMedia.Text = "PAUSE";
            textBoxCurrentMediaTitle.Text = CurrentMedia.Meta(MetadataType.Title);
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

        private void mediaProgressBar_MouseDown(object sender, MouseEventArgs e) { }

        private void mediaProgressBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (_player.WillPlay)
            {
                _player.SeekTo(TimeSpan.FromMilliseconds((long)(mediaProgressBar.Value * _player.Media.Duration)));
            }
        }

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

        private void mediaProgressBarAudio_MouseUp(object sender, MouseEventArgs e)
        {
            if (_player.WillPlay)
            {
                _player.Volume = (int)(mediaProgressBarAudio.Value * 100);
            }
        }

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

        private void UpdateTimeSpan()
        {
            TimeSpan currentTimeSpan = TimeSpan.FromMilliseconds(_player.Time);
            TimeSpan totalTimeSpan = TimeSpan.FromMilliseconds(_player.Media.Duration);
            labelMediaTimeSpan.Text = $"{currentTimeSpan:hh\\:mm\\:ss} / {totalTimeSpan:hh\\:mm\\:ss}";
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (_player.IsPlaying)
            {
                _player.Stop();
                buttonPlayMedia.Text = "PLAY";
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
    }
}