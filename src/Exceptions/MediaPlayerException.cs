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
        // O proprietate extra pentru a ști ce operațiune a eșuat
        public string OperationName { get; }

        public MediaPlayerException() : base() { }

        public MediaPlayerException(string message) : base(message) { }

        public MediaPlayerException(string message, Exception innerException)
            : base(message, innerException) { }

        public MediaPlayerException(string message, string operationName, Exception innerException = null)
            : base(message, innerException)
        {
            OperationName = operationName;
        }
    }
}
