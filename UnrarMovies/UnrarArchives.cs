using SharpCompress.Archive;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrarMovies
{
    class UnrarArchives
    {
        public void extractArchive(string dir, string file)
        {
            var compressed = ArchiveFactory.Open(@file);

            foreach (var entry in compressed.Entries)
            {
                if (!entry.IsDirectory)
                {
                    //Change settings extractoptions to overwrite if needed
                    entry.WriteToDirectory(@dir, ExtractOptions.ExtractFullPath | ExtractOptions.None);
                }
            }
        }
    }
}
