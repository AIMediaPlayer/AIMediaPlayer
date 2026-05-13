using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMediaPlayer.Models
{
    public class PlaylistItemUI
    {
        public string Title { get; set; } = string.Empty;
        public Bitmap? Thumbnail { get; set; }

        public string MediaType { get; set; } = "Media File";
    }
}
