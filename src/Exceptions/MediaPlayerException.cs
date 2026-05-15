
/**************************************************************************
 * *
 * File:        MediaPlayerException.cs                                  *
 * Copyright:   (c) 2026, Loghin Elisei                                  *
 * E-mail:      elisei.loghin2@student.tuiasi.ro                         *
 * Website:                                                              *
 * Description: Custom exceptions for media player and playlist errors.  *
 * *
 * This program is free software; you can redistribute it and/or modify  *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation. This program is distributed in the      *
 * hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 * the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 * PURPOSE. See the GNU General Public License for more details.         *
 * *
 **************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMediaPlayer.Exceptions
{
    /// <summary>
    /// Excepție personalizată pentru erorile apărute în funcționarea player-ului media sau a playlist-ului.
    /// </summary>
    public class MediaPlayerException : Exception
    {
        // proprietate pentru a ști ce operațiune a eșuat
        public string OperationName { get; }

        /// <summary>
        /// Constructor implicit pentru excepția MediaPlayerException.
        /// </summary>
        public MediaPlayerException() : base() { }

        /// <summary>
        /// Constructor care inițializează excepția cu un mesaj specific.
        /// </summary>
        /// <param name="message">Mesajul care descrie eroarea.</param>
        public MediaPlayerException(string message) : base(message) { }

        /// <summary>
        /// Constructor care inițializează excepția cu un mesaj și o excepție internă (inner exception).
        /// </summary>
        /// <param name="message">Mesajul care descrie eroarea.</param>
        /// <param name="innerException">Excepția care a cauzat excepția curentă.</param>
        public MediaPlayerException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Constructor care include mesajul, numele operațiunii care a eșuat și excepția internă.
        /// </summary>
        /// <param name="message">Mesajul care descrie eroarea.</param>
        /// <param name="operationName">Numele operațiunii în timpul căreia a apărut eroarea.</param>
        /// <param name="innerException">Excepția care a cauzat eroarea curentă (opțional).</param>
        public MediaPlayerException(string message, string operationName, Exception innerException = null)
            : base(message, innerException)
        {
            OperationName = operationName;
        }
    }
}
