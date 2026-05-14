using AIMediaPlayer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMediaPlayer.States
{
    public interface IPlayerState
    {
        void PlayPause(MainWindow context);
        void Stop(MainWindow context);
        string GetButtonIcon();
    }
}
