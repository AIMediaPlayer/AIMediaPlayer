/**************************************************************************
 * *
 * File:        StoppedState.cs                                          *
 * Copyright:   (c) 2026, Loghin Elisei                                  *
 * E-mail:      elisei.loghin2@student.tuiasi.ro                         *
 * Website:                                                              *
 * Description: State implementation representing the stopped player UI. *
 * *
 * This program is free software; you can redistribute it and/or modify  *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation. This program is distributed in the      *
 * hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 * the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 * PURPOSE. See the GNU General Public License for more details.         *
 * *
 **************************************************************************/
using AIMediaPlayer.Views;

namespace AIMediaPlayer.States
{
    /// <summary>
    /// Reprezintă starea de oprire a player-ului media în cadrul
    /// șablonului de proiectare State.
    /// </summary>
    public class StoppedState : IPlayerState
    {
        /// <summary>
        /// Gestionează acțiunea butonului Play/Pause în funcție de starea curentă a player-ului.
        /// </summary>
        /// <param name="context">Fereastra principală care conține instanța MediaPlayer.</param>
        public void PlayPause(MainWindow context)
        {
            if (context.MediaPlayer.Media != null)
            {
                context.MediaPlayer.Play();
                context.SetState(new PlayingState());
            }
        }

        /// <summary>
        /// Gestionează acțiunea butonului de Stop.
        /// </summary>
        /// <param name="context">Fereastra principală care conține instanța MediaPlayer.</param>
        public void Stop(MainWindow context) {  }

        /// <summary>
        /// Oferă iconița (caracterul vizual) potrivită pentru butonul de Play/Pause conform stării curente.
        /// </summary>
        /// <returns>Un string reprezentând iconița (ex. "▶" sau "❚❚").</returns>
        public string GetButtonIcon() => "▶";
    }
}
