using AIMediaPlayer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMediaPlayer.States
{
    public class StoppedState : IPlayerState
    {
        public void PlayPause(MainWindow context)
        {
            if (context.MediaPlayer.Media != null)
            {
                context.MediaPlayer.Play();
                context.SetState(new PlayingState());
            }
        }

        public void Stop(MainWindow context) {  }

        public string GetButtonIcon() => "▶";
    }
}
