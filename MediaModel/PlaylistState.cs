using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaModel
{
    /// <summary>
    /// Clasa folosita pentru serializarea si deserializarea starii unui playlist
    /// </summary>
    /// <remarks>
    /// Este utilizata pentru salvarea si incarcarea playlist-ului din fisiere JSON
    /// </remarks>
    public class PlaylistState
    {
        /// <summary>
        /// Indexul curent al elementului redat din playlist
        /// </summary>
        public int CurrentIndex { get; set; }

        /// <summary>
        /// Lista de fisiere media salvate in playlist (sub forma de string URI)
        /// </summary>
        public List<string> Items { get; set; }
    }
}