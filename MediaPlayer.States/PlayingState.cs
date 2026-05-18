/**************************************************************************
 * *
 * File:        PlayingState.cs                                          *
 * Copyright:   (c) 2026, Loghin Elisei                                  *
 * E-mail:      elisei.loghin2@student.tuiasi.ro                         *
 * Website:                                                              *
 * Description: State implementation representing the active playing UI. *
 * *
 * This program is free software; you can redistribute it and/or modify  *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation. This program is distributed in the      *
 * hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 * the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 * PURPOSE. See the GNU General Public License for more details.         *
 * *
 **************************************************************************/
using LibVLCSharp.Shared;

namespace AIMediaPlayer.States
{
    /// <summary>
    /// Reprezintă starea activă de redare a player-ului media în cadrul
    /// șablonului de proiectare State.
    /// </summary>
    public class PlayingState : IPlayerState
    {
        /// <summary>
        /// Gestionează acțiunea butonului Play/Pause în starea de redare, punând fluxul media pe pauză.
        /// </summary>
        /// <param name="mediaPlayer">Instanța de MediaPlayer pe care se execută acțiunea.</param>
        /// <returns>Noua stare a player-ului media după pauză (PausedState).</returns>
        public IPlayerState PlayPause(MediaPlayer mediaPlayer)
        {
            mediaPlayer.SetPause(true);
            return new PausedState();
        }

        /// <summary>
        /// Gestionează acțiunea butonului de Stop în starea de redare, oprind complet redarea fișierului.
        /// </summary>
        /// <param name="mediaPlayer">Instanța de MediaPlayer pe care se execută acțiunea.</param>
        /// <returns>Noua stare a player-ului media după oprire (StoppedState).</returns>
        public IPlayerState Stop(MediaPlayer mediaPlayer)
        {
            mediaPlayer.Stop();
            return new StoppedState();
        }

        /// <summary>
        /// Oferă iconița (caracterul vizual) potrivită pentru butonul de Play/Pause conform stării de redare.
        /// </summary>
        /// <returns>Un string reprezentând iconița de pauză ("❚❚").</returns>
        public string GetButtonIcon() => "❚❚";
    }
}
