using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCommons
{
    public class MediaData
    {
        private List<Media> _mediaItems;
        private LibVLC vlc;

        public List<Media> MediaItems 
        { 
            get { return _mediaItems; } 
        }

        public MediaData()
        {
            _mediaItems = new List<Media>();
            vlc = new LibVLC();
        }
    }
}
