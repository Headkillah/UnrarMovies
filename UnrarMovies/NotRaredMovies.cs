using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrarMovies
{
    class NotRaredMovies
    {

        public static List<string> GetNotRaredMovies(string destPath, string downloadPath)
        {
            CleanUp cleaner = new CleanUp();
            List<string> notRared = new List<string>();
          
            string[] allTheFiles = Directory.GetFiles(downloadPath, "*.*", SearchOption.AllDirectories);
            string[] moviesOnNas = Directory.GetFiles(destPath, "*.*", SearchOption.AllDirectories);

            foreach (var file in allTheFiles)
            {

                var fileInfo = new FileInfo(file);
                var ext = Path.GetExtension(file);
                bool amIASub = AmIASubtitle(ext);
                long size = fileInfo.Length;
                if (size > 700000000 || amIASub)
                {
                    var fileName = Path.GetFileName(file);
                    string realPath = cleaner.GetRealMovieName(file);
                    
                    if (!moviesOnNas.Contains(destPath + realPath + ext, StringComparer.OrdinalIgnoreCase))
                    {
                        File.Move(file, destPath + realPath + ext);
                        if (!amIASub)
                            notRared.Add(realPath);
                    }


                }
            }
            return notRared;
        }

        private static bool AmIASubtitle(string ext)
        {
            if (ext == ".srt" || ext == ".sub" || ext == ".idx")
            {
                return true;
            }
            else
                return false;
        }
    }
}
