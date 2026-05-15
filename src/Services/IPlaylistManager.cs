/**************************************************************************
 * *
 * File:        IPlaylistManager.cs                                      *
 * Copyright:   (c) 2026, Loghin Elisei                                  *
 * E-mail:      elisei.loghin2@student.tuiasi.ro                         *
 * Website:                                                              *
 * Description: Interface defining the contract for playlist management. *
 * *
 * This program is free software; you can redistribute it and/or modify  *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation. This program is distributed in the      *
 * hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 * the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 * PURPOSE. See the GNU General Public License for more details.         *
 * *
 **************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using LibVLCSharp.Shared;

namespace AIMediaPlayer.Services
{
    /// <summary>
    /// Interfață ce definește contractul pentru gestionarea unui playlist de fișiere media.
    /// </summary>
    public interface IPlaylistManager
    {
        /// <summary>
        /// Eveniment declanșat atunci când structura sau starea playlist-ului a fost modificată (adăugare, ștergere, amestecare etc.).
        /// </summary>
        event Action PlaylistUpdated;

        /// <summary>
        /// Adaugă un nou fișier media în playlist în mod asincron.
        /// </summary>
        /// <param name="uri">Calea (URI) către fișierul media.</param>
        /// <returns>Returnează true dacă fișierul a fost adăugat cu succes.</returns>
        Task<bool> Add(Uri uri);

        /// <summary>
        /// Obține un obiect Media din playlist pe baza titlului său.
        /// </summary>
        /// <param name="title">Titlul fișierului media căutat.</param>
        /// <returns>Obiectul Media corespunzător, sau null dacă nu a fost găsit.</returns>
        Media GetMedia(string title);

        /// <summary>
        /// Returnează o listă cu titlurile tuturor elementelor media din playlist.
        /// </summary>
        /// <returns>O listă de șiruri de caractere (titluri).</returns>
        List<string> ListAll();

        /// <summary>
        /// Setează indexul elementului media care trebuie redat curent.
        /// </summary>
        /// <param name="index">Poziția în playlist (index de la 0).</param>
        void SetCurrentIndex(int index);

        /// <summary>
        /// Trece la următorul element din playlist (avansare).
        /// </summary>
        void Next();

        /// <summary>
        /// Trece la elementul anterior din playlist.
        /// </summary>
        void Previous();

        /// <summary>
        /// Amestecă (shuffle) aleatoriu ordinea elementelor din playlist.
        /// </summary>
        void Shuffle();

        /// <summary>
        /// Comută opțiunea de repetare a playlist-ului (On/Off).
        /// </summary>
        void Repeat();

        /// <summary>
        /// Elimină un element media din playlist de la un index specificat și actualizează fișierul salvat pe disc.
        /// </summary>
        /// <param name="index">Poziția elementului ce trebuie eliminat.</param>
        /// <param name="playlistPath">Calea către fișierul playlist-ului pentru a persista modificarea.</param>
        void Remove(int index, string playlistPath);

        /// <summary>
        /// Salvează starea detaliată curentă a playlist-ului pe disc.
        /// </summary>
        /// <param name="path">Calea unde va fi salvat fișierul (ex. .json).</param>
        void SavePlaylist(string path);

        /// <summary>
        /// Metodă alternativă de salvare care stochează elementele playlist-ului pe disc sub formă de listă simplă.
        /// </summary>
        /// <param name="path">Calea unde va fi salvat fișierul.</param>
        void Save(string path);

        /// <summary>
        /// Încarcă în mod asincron un playlist salvat anterior de pe disc.
        /// </summary>
        /// <param name="path">Calea către fișierul playlist-ului ce trebuie încărcat.</param>
        /// <returns>Task asincron ce reprezintă operațiunea de încărcare.</returns>
        Task Load(string path);

        /// <summary>
        /// Returnează obiectul Media care este setat ca fiind curent în playlist.
        /// </summary>
        /// <returns>Obiectul Media curent, sau null dacă playlist-ul este gol.</returns>
        Media GetCurrent();

        /// <summary>
        /// Obține o listă de obiecte cu starea (metadatele vizuale și rutele) elementelor din playlist, utilă pentru afișarea în interfața grafică.
        /// </summary>
        /// <returns>O listă cu informațiile elementelor, de tip PlaylistItemState.</returns>
        List<AIMediaPlayer.Models.PlaylistItemState> GetPlaylistInfo();
    }
}