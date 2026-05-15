/**************************************************************************
 * *
 * File:        PausedState.cs                                           *
 * Copyright:   (c) 2026, Loghin Elisei                                  *
 * E-mail:      elisei.loghin2@student.tuiasi.ro                         *
 * Website:                                                              *
 * Description: State implementation representing the player's paused UI.*
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMediaPlayer.States
{
    public class PausedState : IPlayerState
    {
        /// <summary>
        /// Gestionează acțiunea butonului Play/Pause în funcție de starea curentă a player-ului.
        /// </summary>
        /// <param name="context">Fereastra principală care conține instanța MediaPlayer.</param>
        public void PlayPause(MainWindow context)
        {
            context.MediaPlayer.Play();
            context.SetState(new PlayingState());
        }

        /// <summary>
        /// Gestionează acțiunea butonului de Stop.
        /// </summary>
        /// <param name="context">Fereastra principală care conține instanța MediaPlayer.</param>
        public void Stop(MainWindow context)
        {
            context.MediaPlayer.Stop();
            context.SetState(new StoppedState());
        }

        /// <summary>
        /// Oferă iconița (caracterul vizual) potrivită pentru butonul de Play/Pause conform stării curente.
        /// </summary>
        /// <returns>Un string reprezentând iconița (ex. "▶" sau "❚❚").</returns>
        public string GetButtonIcon() => "▶";
    }
}
