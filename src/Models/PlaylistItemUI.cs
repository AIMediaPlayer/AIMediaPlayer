/**************************************************************************
 * *
 * File:        PlaylistItemUI.cs                                        *
 * Copyright:   (c) 2026, Loghin Elisei                                  *
 * E-mail:      elisei.loghin2@student.tuiasi.ro                         *
 * Website:                                                              *
 * Description: UI data model for displaying playlist items visually.    *
 * *
 * This program is free software; you can redistribute it and/or modify  *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation. This program is distributed in the      *
 * hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 * the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 * PURPOSE. See the GNU General Public License for more details.         *
 * *
 **************************************************************************/
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMediaPlayer.Models
{
    /// <summary>
    /// Reprezintă modelul de date utilizat exclusiv pentru interfața grafică (UI).
    /// Conține informațiile necesare pentru afișarea unui element în lista vizuală a playlist-ului.
    /// </summary>
    public class PlaylistItemUI
    {
        /// <summary>
        /// Obține sau setează titlul fișierului media, care va fi afișat în listă.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Obține sau setează imaginea de previzualizare (thumbnail-ul) asociată fișierului media.
        /// Poate fi null dacă fișierul nu are artwork sau dacă încărcarea imaginii a eșuat.
        /// </summary>
        public Bitmap? Thumbnail { get; set; }

        /// <summary>
        /// Obține sau setează o descriere text a tipului de fișier (ex. "Video File", "Audio File").
        /// Implicit este setat pe "Media File".
        /// </summary>
        public string MediaType { get; set; } = "Media File";
    }
}
