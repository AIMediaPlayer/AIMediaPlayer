using MediaModel;
using LibVLCSharp.Shared;
using System;

namespace MediaPresenter
{
    /// <summary>
    /// Clasa Presenter care actioneaza ca intermediar intre UI si PlaylistManager
    /// </summary>
    public class Presenter : IPresenter
    {
        private IPlaylistManager _playlistManager;

        /// <summary>
        /// Constructor ce initializeaza Presenter-ul cu un manager de playlist
        /// </summary>
        /// <param name="playlistManager">Instanta de IPlaylistManager folosita pentru controlul playlist-ului</param>
        public Presenter(IPlaylistManager playlistManager)
        {
            _playlistManager = playlistManager;
        }

        /// <summary>
        /// Trece la urmatorul element din playlist si il returneaza pentru redare
        /// </summary>
        /// <returns>Media urmatoare din playlist</returns>
        public Media Next()
        {
            _playlistManager.Next();
            return _playlistManager.GetCurrent();
        }

        /// <summary>
        /// Trece la elementul anterior din playlist si il returneaza pentru redare
        /// </summary>
        /// <returns>Media anterioara din playlist</returns>
        public Media Previous()
        {
            _playlistManager.Previous();
            return _playlistManager.GetCurrent();
        }

        /// <summary>
        /// Amesteca ordinea elementelor din playlist si returneaza primul element
        /// </summary>
        /// <returns>Prima media dupa shuffle</returns>
        public Media Shuffle()
        {
            _playlistManager.Shuffle();
            return _playlistManager.GetCurrent();
        }
    }
}