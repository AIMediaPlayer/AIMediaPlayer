/**************************************************************************
 * *
 * File:        PlaylistState.cs                                         *
 * Copyright:   (c) 2026, Loghin Elisei                                  *
 * E-mail:      elisei.loghin2@student.tuiasi.ro                         *
 * Website:                                                              *
 * Description: Data models for serializing and loading playlist state.  *
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
using System.Text;
using System.Threading.Tasks;

namespace AIMediaPlayer.Models
{
    /// <summary>
    /// Reprezintă starea salvată a unui singur element din playlist.
    /// Este utilizată pentru a stoca și încărca rapid detaliile fișierului media fără a-l parsa din nou.
    /// </summary>
    public class PlaylistItemState
    {
        /// <summary>
        /// Obține sau setează Media Resource Locator-ul (MRL). 
        /// Reprezintă calea sau adresa (locală sau de rețea) a fișierului media.
        /// </summary>
        public string Mrl { get; set; }

        /// <summary>
        /// Obține sau setează titlul elementului media.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Obține sau setează calea către fișierul imagine utilizat ca thumbnail (previzualizare).
        /// </summary>
        public string ThumbnailPath { get; set; }
    }

    /// <summary>
    /// Reprezintă starea globală a playlist-ului, utilizată pentru serializare (salvare/încărcare din fișierul JSON).
    /// </summary>
    public class PlaylistState
    {
        /// <summary>
        /// Obține sau setează indexul fișierului media care era redat sau selectat curent la momentul salvării.
        /// </summary>
        public int CurrentIndex { get; set; }

        /// <summary>
        /// Obține sau setează lista completă a elementelor din playlist, sub formă de obiecte <see cref="PlaylistItemState"/>.
        /// </summary>
        public List<PlaylistItemState> Items { get; set; }
    }
}