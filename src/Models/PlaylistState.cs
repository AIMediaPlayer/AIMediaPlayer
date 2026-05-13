using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMediaPlayer.Models
{
    public class PlaylistItemState
    {
        public string Mrl { get; set; }
        public string Title { get; set; }
        public string ThumbnailPath { get; set; } 
    }

    public class PlaylistState
    {
        public int CurrentIndex { get; set; }
        public List<PlaylistItemState> Items { get; set; }
    }
}