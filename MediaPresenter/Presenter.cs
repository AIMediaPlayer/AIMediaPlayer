using MediaModel;
using LibVLCSharp.Shared;


namespace MediaPresenter
{
    public class Presenter : IPresenter
    {
        private IPlaylistManager _playlistManager;

        public Presenter(IPlaylistManager playlistManager)
        {
            _playlistManager = playlistManager;
        }

        public Media Next()
        {
            _playlistManager.Next();
            return _playlistManager.GetCurrent();
        }

        public Media Previous()
        {
            _playlistManager.Previous();
            return _playlistManager.GetCurrent();
        }

        public Media Shuffle()
        {
            _playlistManager.Shuffle();
            return _playlistManager.GetCurrent();
        }
    }
}
