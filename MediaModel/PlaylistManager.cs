using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibVLCSharp.Forms;
using LibVLCSharp.Forms.Shared;
using LibVLCSharp.Shared;
using System.Security.Policy;
using MediaCommons;
using System.Threading;

namespace MediaModel
{
    /// <summary>
    /// Clasa de tip model pentru manipulare playlist de fisiere media
    /// </summary>
    public class PlaylistManager : IPlaylistManager
    {
        private List<Media> _mediaList;
        private LibVLC _vlc;

        public PlaylistManager(LibVLC vlc)
        {
            _vlc = vlc;
            _mediaList = new List<Media>();
        }

        /// <summary>
        /// Functie asincrona ce asteapta parsarea fisierului media si-l adauaga in lista de obiecte Media 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<bool> Add(Uri uri)
        {
            try
            {
                Media media = new Media(_vlc, uri);

                var status = await media.Parse(MediaParseOptions.ParseLocal);

                if(status == MediaParsedStatus.Done)
                {
                    _mediaList.Add(media);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Cauta si returneaza un obiect Media prezent in lista
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Media GetMedia(string title)
        {
            foreach(Media media in _mediaList)
            {
                string mediaTitle = media.Meta(MetadataType.Title);
                if (mediaTitle == title)
                    return media;
            }
            return null;
        }

        /// <summary>
        /// Furnizeaza titlul fiecarui obiect Media prezent in lista
        /// </summary>
        /// <returns></returns>
        public List<string> ListAll()
        {
            List<string> titleList = new List<string>();

            foreach (Media media in _mediaList)
            {
                string title = media.Meta(MetadataType.Title);
                titleList.Add(title);
            }

            return titleList;
        }

    }
}
