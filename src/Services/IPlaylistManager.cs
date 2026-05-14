using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using LibVLCSharp.Shared;


namespace AIMediaPlayer.Services
{
    public interface IPlaylistManager
    {
        event Action PlaylistUpdated;
        Task<bool> Add(Uri uri);
        Media GetMedia(string title);
        List<string> ListAll();
        void SetCurrentIndex(int index);

        void Next();
        void Previous();
        void Shuffle();
        void Repeat();
        void Remove(int index, string playlistPath);
        void SavePlaylist(string path);
        void Save(string path);

        Task Load(string path);

        Media GetCurrent();

        List<AIMediaPlayer.Models.PlaylistItemState> GetPlaylistInfo();

    }
}
