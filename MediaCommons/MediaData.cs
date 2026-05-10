using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCommons
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