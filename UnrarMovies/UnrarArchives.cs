using SharpCompress.Archive;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UnrarMovies
{
    class UnrarArchives
    {
        public void extractArchive(string dir, string file)
        {
            var compressed = ArchiveFactory.Open(@file);
            string folder = FolderName(file);
           

            foreach (var entry in compressed.Entries)
            {
                if (!entry.IsDirectory)
                {
                    
                    //Change settings extractoptions to overwrite if needed
                    entry.WriteToDirectory(@dir+folder, ExtractOptions.ExtractFullPath | ExtractOptions.None);

                }
               
            }
            compressed.Dispose();
        }

        private string FolderName(string filePath)
        {
            string[] directories = filePath.Split(Path.DirectorySeparatorChar);
            string pathen = directories[directories.Length - 1];
            if (pathen.Contains("subs"))
                return directories[directories.Length - 3];
            else
                return directories[directories.Length-2];
        }
    }
}
