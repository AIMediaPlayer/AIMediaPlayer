/**************************************************************************
 * *
 * File:        PlaylistManager.cs                                       *
 * Copyright:   (c) 2026, Loghin Elisei                                  *
 * E-mail:      elisei.loghin2@student.tuiasi.ro                         *
 * Website:                                                              *
 * Description: Concrete implementation of the playlist manager.         *
 * *
 * This program is free software; you can redistribute it and/or modify  *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation. This program is distributed in the      *
 * hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 * the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 * PURPOSE. See the GNU General Public License for more details.         *
 * *
 **************************************************************************/
using AIMediaPlayer.Models;
using LibVLCSharp.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AIMediaPlayer.Exceptions;

namespace AIMediaPlayer.Services
{
    /// <summary>
    /// Clasa de tip model pentru manipulare playlist de fisiere media
    /// </summary>
    public class PlaylistManager : IPlaylistManager
    {
        public event Action PlaylistUpdated;

        private int _currentIndex = 0;
        private bool _repeat = false;
        private List<Media> _mediaList;
        private LibVLC _vlc;

        /// <summary>
        /// Constructor ce inițializează managerul de playlist.
        /// </summary>
        /// <param name="vlc">Instanța de LibVLC utilizată pentru parsarea și gestionarea fișierelor media.</param>
        public PlaylistManager(LibVLC vlc)
        {
            _vlc = vlc;
            _mediaList = new List<Media>();
        }

        /// <summary>
        /// Declanșează evenimentul PlaylistUpdated pentru a notifica interfața grafică de schimbările apărute.
        /// </summary>
        private void NotifyPlaylistChanged() => PlaylistUpdated?.Invoke();


        /// <summary>
        /// Funcție asincronă ce așteaptă parsarea fișierului media și îl adaugă în lista de obiecte Media.
        /// </summary>
        /// <param name="uri">Calea (URI) către fișierul media ce trebuie adăugat.</param>
        /// <returns>Returnează true dacă fișierul a fost adăugat cu succes.</returns>
        /// <exception cref="MediaPlayerException">Aruncată atunci când fișierul nu poate fi adăugat.</exception>
        public async Task<bool> Add(Uri uri)
        {
            try
            {
                Media media = new Media(_vlc, uri);

                _mediaList.Add(media);
                NotifyPlaylistChanged();

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await media.Parse(MediaParseOptions.ParseLocal);
                        NotifyPlaylistChanged();
                    }
                    catch (Exception parseEx)
                    {
                        Console.WriteLine(new MediaPlayerException("Eroare în fundal la parsarea metadatelor.", "Parsare Media", parseEx).Message);
                    }
                });

                return true;
            }
            catch (Exception ex)
            {
                throw new MediaPlayerException($"Nu s-a putut adăuga fișierul: {uri}", "Adăugare Media", ex);
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
        /// Furnizează titlul fiecărui obiect Media prezent în listă.
        /// </summary>
        /// <returns>O listă de string-uri reprezentând titlurile fișierelor media, sau null dacă lista este goală.</returns>
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

        /// <summary>
        /// Setează indexul fișierului media curent din playlist.
        /// </summary>
        /// <param name="index">Indexul la care se dorește mutarea selecției.</param>
        public void SetCurrentIndex(int index)
        {
            if (index >= 0 && index < _mediaList.Count)
                _currentIndex = index;
        }

        /// <summary>
        /// Trece la următorul fișier media din playlist. Tratează logic și opțiunea de repetare (Repeat).
        /// </summary>
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

        /// <summary>
        /// Trece la fișierul media anterior din playlist. Tratează logic și opțiunea de repetare (Repeat).
        /// </summary>
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

        /// <summary>
        /// Amestecă (shuffle) aleatoriu ordinea fișierelor media din playlist și notifică UI-ul.
        /// </summary>
        public void Shuffle()
        {
            _mediaList = _mediaList.OrderBy(x => Guid.NewGuid()).ToList();
            _currentIndex = 0;
            NotifyPlaylistChanged();
        }

        /// <summary>
        /// Comută starea de repetare a playlist-ului (activat/dezactivat).
        /// </summary>
        public void Repeat()
        {
            _repeat = !_repeat;
        }

        /// <summary>
        /// Elimină un fișier media din playlist pe baza indexului său și salvează noua stare.
        /// </summary>
        /// <param name="index">Indexul elementului de eliminat.</param>
        /// <param name="playlistPath">Calea către fișierul unde se salvează starea playlist-ului.</param>
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
                NotifyPlaylistChanged();
            }
        }

        /// <summary>
        /// Obține o listă cu starea curentă a elementelor din playlist (Mrl, Titlu, Cale Thumbnail) pentru UI.
        /// </summary>
        /// <returns>O listă de obiecte PlaylistItemState.</returns>
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

        /// <summary>
        /// Salvează starea curentă a playlist-ului (elemente și index curent) într-un fișier JSON.
        /// </summary>
        /// <param name="path">Calea fișierului de salvare.</param>
        /// <exception cref="MediaPlayerException">Aruncată dacă salvarea pe disc eșuează.</exception>
        public void SavePlaylist(string path)
        {
            try
            {
                var state = new PlaylistState
                {
                    CurrentIndex = _currentIndex,
                    Items = _mediaList.Select(m =>
                    {
                        string artworkUrl = m.Meta(MetadataType.ArtworkURL);
                        string thumbnailLocalPath = null;

                        if (!string.IsNullOrEmpty(artworkUrl))
                        {
                            if (artworkUrl.StartsWith("file:///"))
                            {
                                try
                                {
                                    thumbnailLocalPath = Uri.UnescapeDataString(new Uri(artworkUrl).LocalPath);
                                }
                                catch
                                {
                                    thumbnailLocalPath = artworkUrl;
                                }
                            }
                            else
                            {
                                thumbnailLocalPath = artworkUrl;
                            }
                        }

                        string title = m.Meta(MetadataType.Title);
                        if (string.IsNullOrEmpty(title))
                        {
                            try
                            {
                                title = Path.GetFileName(Uri.UnescapeDataString(m.Mrl));
                            }
                            catch
                            {
                                title = "Unknown Media";
                            }
                        }

                        return new PlaylistItemState
                        {
                            Mrl = m.Mrl,
                            Title = title,
                            ThumbnailPath = thumbnailLocalPath
                        };
                    }).ToList()
                };

                File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(state, Newtonsoft.Json.Formatting.Indented));
            }
            catch (Exception ex)
            {
                throw new MediaPlayerException($"Nu s-a putut salva playlist-ul pe disc în locația: {path}", "Salvare Playlist", ex);
            }
        }

        /// <summary>
        /// Funcție alternativă de salvare a playlist-ului care stochează doar lista de referințe (MRL-uri).
        /// </summary>
        /// <param name="path">Calea fișierului JSON de destinație.</param>
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

        /// <summary>
        /// Încarcă un playlist salvat anterior dintr-un fișier JSON, refăcând obiectele Media.
        /// </summary>
        /// <param name="path">Calea fișierului de unde se face încărcarea.</param>
        /// <exception cref="MediaPlayerException">Aruncată dacă parsarea JSON-ului eșuează.</exception>
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
                        var media = new Media(_vlc, item.Mrl, FromType.FromLocation);

                        await media.Parse(MediaParseOptions.ParseLocal);

                        _mediaList.Add(media);
                    }
                    _currentIndex = state.CurrentIndex;
                    NotifyPlaylistChanged();
                }
            }
            catch (Exception ex)
            {
                throw new MediaPlayerException($"A eșuat încărcarea playlist-ului din {path}", "Încărcare Playlist", ex);
            }
        }

        /// <summary>
        /// Returnează obiectul Media selectat curent în playlist.
        /// </summary>
        /// <returns>Obiectul Media curent, sau null dacă lista este goală.</returns>
        public Media GetCurrent()
        {
            if (_mediaList.Count == 0) return null;
            return _mediaList[_currentIndex];
        }
    }
}