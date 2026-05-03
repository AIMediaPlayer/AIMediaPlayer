using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using LibVLCSharp.Forms;
using LibVLCSharp.Forms.Shared;
using LibVLCSharp.Shared;


namespace MediaModel
{
    public interface IPlaylistManager
    {
        Task<bool> Add(Uri uri);

        Media GetMedia(string title);
        List<string> ListAll();

    }
}
