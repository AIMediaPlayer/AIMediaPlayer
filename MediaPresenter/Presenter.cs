using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaModel;


namespace MediaPresenter
{
    public class Presenter : IPresenter
    {
        private IPlaylistManager _playlistManager;

        public Presenter(IPlaylistManager playlistManager)
        {
            _playlistManager = playlistManager;
        }

        
    }
}
