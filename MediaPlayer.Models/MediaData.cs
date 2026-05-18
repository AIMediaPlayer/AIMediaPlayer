/**************************************************************************
 * *
 * File:        MediaData.cs                                             *
 * Copyright:   (c) 2026, Loghin Elisei                                  *
 * E-mail:      elisei.loghin2@student.tuiasi.ro                         *
 * Website:                                                              *
 * Description: Manages the collection of Media objects and LibVLC.      *
 * *
 * This program is free software; you can redistribute it and/or modify  *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation. This program is distributed in the      *
 * hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 * the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 * PURPOSE. See the GNU General Public License for more details.         *
 * *
 **************************************************************************/
using LibVLCSharp.Shared;
using System.Collections.Generic;

namespace AIMediaPlayer.Models
{
    /// <summary>
    /// Clasa ce gestioneaza colectia de obiecte Media utilizate in aplicatie
    /// </summary>
    public class MediaData
    {
        private List<Media> _mediaItems;
        private LibVLC vlc;

        /// <summary>
        /// Lista de obiecte Media incarcate in aplicatie
        /// </summary>
        public List<Media> MediaItems
        {
            get { return _mediaItems; }
        }

        /// <summary>
        /// Constructor ce initializeaza lista de fisiere media si instanta LibVLC
        /// </summary>
        public MediaData()
        {
            _mediaItems = new List<Media>();
            vlc = new LibVLC();
        }
    }
}