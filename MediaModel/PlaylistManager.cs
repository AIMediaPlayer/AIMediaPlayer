using LibVLCSharp.Forms;
using LibVLCSharp.Forms.Shared;
using LibVLCSharp.Shared;
using MediaCommons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediaModel
{
    /// <summary>
    /// Clasa de tip model pentru manipulare playlist de fisiere media
    /// </summary>
    public class PlaylistManager : IPlaylistManager
    {
        private int _currentIndex = 0;
        private bool _repeat = false;
        private List<Media> _mediaList;
        private LibVLC _vlc;

        public PlaylistManager(LibVLC vlc)
        {
            _vlc = vlc;
            _mediaList = new List<Media>();
        }

        /// <summary>
        /// Functie asincrona ce asteapta parsarea fisierului media si-l adauaga in lista de obiecte Media 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<bool> Add(Uri uri)
        {
            try
            {
                Media media = new Media(_vlc, uri);

                var status = await media.Parse(MediaParseOptions.ParseLocal);

                if(status == MediaParsedStatus.Done)
                {
                    _mediaList.Add(media);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Cauta si returneaza un obiect Media prezent in lista
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Media GetMedia(string title)
        {
            foreach(Media media in _mediaList)
            {
                string mediaTitle = media.Meta(MetadataType.Title);
                if (mediaTitle == title)
                    return media;
            }
            return null;
        }

        /// <summary>
        /// Furnizeaza titlul fiecarui obiect Media prezent in lista
        /// </summary>
        /// <returns></returns>
        public List<string> ListAll()
        {
            List<string> titleList = new List<string>();

            foreach (Media media in _mediaList)
            {
                string title = media.Meta(MetadataType.Title);
                titleList.Add(title);
            }

            return titleList;
        }

        public void SetCurrentIndex(int index)
        {
            if (index >= 0 && index < _mediaList.Count)
                _currentIndex = index;
        }

        public void Next()
        {
            if (_mediaList.Count == 0) return;

            _currentIndex++;

            if (_currentIndex >= _mediaList.Count)
                _currentIndex = _repeat ? 0 : _mediaList.Count - 1;
        }

        public void Previous()
        {
            if (_mediaList.Count == 0) return;

            _currentIndex--;

            if (_currentIndex < 0)
                _currentIndex = 0;
        }

        public void Shuffle()
        {
            _mediaList = _mediaList.OrderBy(x => Guid.NewGuid()).ToList();
            _currentIndex = 0;
        }

        public void Repeat()
        {
            _repeat = !_repeat;
        }

        public void Remove(string title)
        {
            var media = _mediaList.FirstOrDefault(m => m.Meta(MetadataType.Title) == title);

            if (media != null)
            {
                int index = _mediaList.IndexOf(media);
                _mediaList.Remove(media);

                if (index <= _currentIndex && _currentIndex > 0)
                    _currentIndex--;
            }
        }

        public void Save(string path)
        {
            List<string> items = new List<string>();

            // 1. dacă fișierul există, îl citim
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                dynamic state = JsonConvert.DeserializeObject(json);

                foreach (var item in state.Items)
                {
                    items.Add(item.ToString());
                }
            }

            // 2. adăugăm media curentă
            if (_mediaList.Count > 0 && _currentIndex >= 0)
            {
                string current = _mediaList[_currentIndex].Mrl;

                if (!items.Contains(current)) // evităm duplicate
                    items.Add(current);
            }

            // 3. salvăm înapoi
            var obj = new
            {
                CurrentIndex = items.Count - 1,
                Items = items
            };

            File.WriteAllText(path, JsonConvert.SerializeObject(obj, Formatting.Indented));
        }

        public void Load(string path)
        {
            if (!File.Exists(path))
                return;

            var json = File.ReadAllText(path);
            dynamic state = JsonConvert.DeserializeObject(json);

            _mediaList.Clear();

            foreach (var item in state.Items)
            {
                string url = item.ToString();
                var media = new Media(_vlc, new Uri(url));
                _mediaList.Add(media);
            }

            _currentIndex = (int)state.CurrentIndex;

            if (_currentIndex < 0 || _currentIndex >= _mediaList.Count)
                _currentIndex = 0;
        }

        public Media GetCurrent()
        {
            if (_mediaList.Count == 0) return null;
            return _mediaList[_currentIndex];
        }

    }
}
