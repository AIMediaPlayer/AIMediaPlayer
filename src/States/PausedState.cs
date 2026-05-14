using AIMediaPlayer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMediaPlayer.States
{
    public class PausedState : IPlayerState
    {
        public void PlayPause(MainWindow context)
        {
            context.MediaPlayer.Play();
            context.SetState(new PlayingState());
        }

        public void Stop(MainWindow context)
        {
            context.MediaPlayer.Stop();
            context.SetState(new StoppedState());
        }

        public string GetButtonIcon() => "▶";
    }
}
