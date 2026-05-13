using AIMediaPlayer.Models;
using LibVLCSharp.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AIMediaPlayer.Services
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

                // Așteptăm să încerce extragerea metadatelor (dar nu ne blocăm de statusul returnat)
                await media.Parse(MediaParseOptions.ParseLocal);

                // Adăugăm fișierul în lista internă indiferent dacă parsarea a returnat Done, Skipped etc.
                _mediaList.Add(media);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la adăugarea media: {ex.Message}");
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
            foreach (Media media in _mediaList)
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
            int count = 0;
            List<string> titleList = new List<string>();

            foreach (Media media in _mediaList)
            {
                string title = media.Meta(MetadataType.Title);

                // Adăugăm fallback-ul către numele fișierului
                if (string.IsNullOrEmpty(title))
                {
                    title = Path.GetFileName(Uri.UnescapeDataString(media.Mrl));
                }

                titleList.Add(title);
                count++;
            }
            if (count == 0)
                return null;

            return titleList;
        }

        public void SetCurrentIndex(int index)
        {
            if (index >= 0 && index < _mediaList.Count)
                _currentIndex = index;
        }

        public void Next()
        {
            if (_mediaList.Count == 0)
                return;

            if (_currentIndex < _mediaList.Count - 1)
            {
                _currentIndex++;
            }
            else
            {
                if (_repeat)
                    _currentIndex = 0;
                else
                    _currentIndex = _mediaList.Count - 1;
            }
        }

        public void Previous()
        {
            if (_mediaList.Count == 0)
                return;

            if (_currentIndex > 0)
            {
                _currentIndex--;
            }
            else
            {
                if (_repeat)
                    _currentIndex = _mediaList.Count - 1;
                else
                    _currentIndex = 0;
            }
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

        public void Remove(int index, string playlistPath)
        {
            if (index >= 0 && index < _mediaList.Count)
            {
                _mediaList.RemoveAt(index);

                if (_currentIndex >= _mediaList.Count && _mediaList.Count > 0)
                    _currentIndex = _mediaList.Count - 1;
                else if (_currentIndex > index)
                    _currentIndex--;

                SavePlaylist(playlistPath);
            }
        }
        public List<PlaylistItemState> GetPlaylistInfo()
        {
            var list = new List<PlaylistItemState>();

            foreach (var m in _mediaList)
            {
                string artworkUrl = m.Meta(MetadataType.ArtworkURL);
                string thumbnailLocalPath = null;

                if (!string.IsNullOrEmpty(artworkUrl))
                {
                    if (artworkUrl.StartsWith("file:///"))
                    {
                        try { thumbnailLocalPath = Uri.UnescapeDataString(new Uri(artworkUrl).LocalPath); }
                        catch { thumbnailLocalPath = artworkUrl; }
                    }
                    else
                    {
                        thumbnailLocalPath = artworkUrl;
                    }
                }

                string cleanTitle = m.Meta(MetadataType.Title);
                if (string.IsNullOrEmpty(cleanTitle))
                {
                    cleanTitle = Path.GetFileName(Uri.UnescapeDataString(m.Mrl));
                }

                list.Add(new PlaylistItemState
                {
                    Mrl = m.Mrl,
                    Title = cleanTitle,
                    ThumbnailPath = thumbnailLocalPath
                });
            }

            return list;
        }
        public void SavePlaylist(string path)
        {
            var state = new PlaylistState
            {
                CurrentIndex = _currentIndex,
                Items = _mediaList.Select(m =>
                {
                    // 1. Extragem Artwork-ul (coperta încorporată în fișier, descărcată de VLC în cache)
                    string artworkUrl = m.Meta(MetadataType.ArtworkURL);
                    string thumbnailLocalPath = null;

                    if (!string.IsNullOrEmpty(artworkUrl))
                    {
                        // LibVLC returnează de obicei un format URI (ex: "file:///C:/Users/.../art.jpg")
                        // Trebuie să-l convertim într-o cale locală normală ca să poată fi citit de Avalonia ulterior
                        if (artworkUrl.StartsWith("file:///"))
                        {
                            try
                            {
                                thumbnailLocalPath = Uri.UnescapeDataString(new Uri(artworkUrl).LocalPath);
                            }
                            catch
                            {
                                thumbnailLocalPath = artworkUrl; // Fallback în caz de eroare la conversie
                            }
                        }
                        else
                        {
                            // Poate fi un link web sau un "attachment://"
                            thumbnailLocalPath = artworkUrl;
                        }
                    }

                    // 2. Extragem Titlul (cu fallback la numele fișierului, dacă nu are metadate)
                    string title = m.Meta(MetadataType.Title);
                    if (string.IsNullOrEmpty(title))
                    {
                        title = Path.GetFileName(Uri.UnescapeDataString(m.Mrl));
                    }

                    return new PlaylistItemState
                    {
                        Mrl = m.Mrl,
                        Title = title,
                        ThumbnailPath = thumbnailLocalPath // Calea preluată automat (va fi null dacă nu există copertă)
                    };
                }).ToList()
            };

            File.WriteAllText(path, JsonConvert.SerializeObject(state, Formatting.Indented));
        }

        public void Save(string path)
        {
            List<string> items = new List<string>();

            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                dynamic state = JsonConvert.DeserializeObject(json);

                foreach (var item in state.Items)
                {
                    items.Add(item.ToString());
                }
            }

            if (_mediaList.Count > 0 && _currentIndex >= 0)
            {
                string current = _mediaList[_currentIndex].Mrl;

                if (!items.Contains(current))
                    items.Add(current);
            }

            var obj = new
            {
                CurrentIndex = items.Count - 1,
                Items = items
            };

            File.WriteAllText(path, JsonConvert.SerializeObject(obj, Formatting.Indented));
        }

        public async Task Load(string path)
        {
            if (!File.Exists(path)) return;

            try
            {
                var json = File.ReadAllText(path);
                var state = JsonConvert.DeserializeObject<PlaylistState>(json);

                _mediaList.Clear();
                if (state?.Items != null)
                {
                    foreach (var item in state.Items)
                    {
                        // Creăm obiectul media
                        var media = new Media(_vlc, item.Mrl, FromType.FromLocation);

                        // IMPORTANT: Trebuie să așteptăm parsarea pentru a avea acces la titlu/metadate
                        await media.Parse(MediaParseOptions.ParseLocal);

                        _mediaList.Add(media);
                    }
                    _currentIndex = state.CurrentIndex;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la încărcarea playlist-ului: {ex.Message}");
            }
        }

        public Media GetCurrent()
        {
            if (_mediaList.Count == 0) return null;
            return _mediaList[_currentIndex];
        }
    }
}