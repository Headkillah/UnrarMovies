using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrarMovies
{
    class FileInfoHandler
    {
        /// <summary>
        /// Gonna return a long to get correct fileszie to compare with.
        /// </summary>
        public long GetFileSize { get; set; }

        public bool IsItAMovie = false;


        /// <summary>
        /// Goinna read the rarfile so it can check if its for a movie or if its something like a sub.rar
        /// </summary>
        /// <param name="rarfile">insert the rarfile with complete path as a string</param>
        public void ReadFileSize(string rarfile)
        {
            FileInfo rarFile = new FileInfo(rarfile);
            long rarSize = rarFile.Length;
            GetFileSize = rarSize;
            CheckWhatItIS(rarSize);
        }

      

        internal void CheckWhatItIS(long rarSize)
        {
            if (rarSize < 40000000)
                IsItAMovie = false;
            else
                IsItAMovie = true;
        }
    }
}
