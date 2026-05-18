/**************************************************************************
 * *
 * File:        IPlayerState.cs                                          *
 * Copyright:   (c) 2026, Loghin Elisei                                  *
 * E-mail:      elisei.loghin2@student.tuiasi.ro                         *
 * Website:                                                              *
 * Description: Interface defining the State Pattern for player control. *
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
    /// Definește comportamentele specifice fiecărei stări a player-ului media
    /// conform șablonului de proiectare State.
    /// </summary>
    /// <remarks>
    /// Interfața permite separarea logicii de control a player-ului în funcție
    /// de starea curentă (ex.: Play, Pause, Stop), oferind implementări diferite
    /// pentru acțiunile disponibile utilizatorului.
    /// </remarks>
    public interface IPlayerState
    {
        /// <summary>
        /// Gestionează acțiunea butonului Play/Pause în funcție de starea curentă a player-ului.
        /// </summary>
        /// <param name="context">Fereastra principală care conține instanța MediaPlayer.</param>
        IPlayerState PlayPause(MediaPlayer mediaPlayer);

        /// <summary>
        /// Gestionează acțiunea butonului de Stop.
        /// </summary>
        /// <param name="context">Fereastra principală care conține instanța MediaPlayer.</param>
        IPlayerState Stop(MediaPlayer mediaPlayer);

        /// <summary>
        /// Oferă iconița (caracterul vizual) potrivită pentru butonul de Play/Pause conform stării curente.
        /// </summary>
        /// <returns>Un string reprezentând iconița (ex. "▶" sau "❚❚").</returns>
        string GetButtonIcon();
    }
}
