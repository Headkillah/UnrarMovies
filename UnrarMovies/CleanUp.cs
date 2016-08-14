using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrarMovies
{
    class CleanUp
    {
        

        public static bool CleanUpSubRars(string subsRar)
        {
            try
            {
                File.Delete(subsRar);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        List<string> MoviesNotToMail = new List<string>();

        public bool CheckIfIShouldMail(string inMovieName)
        {
       
            if (MoviesNotToMail.Contains(inMovieName))
            {
                return false;
            }
            return true;
        }
        
        public void MoveTheFiles(string[] filesToMove, string destinationPath)
        {
            
            string[] moviesOnNas = Directory.GetFiles(destinationPath, "*.*", SearchOption.AllDirectories);
           

            

            foreach (var file in filesToMove)
            {
               
                var fileName = Path.GetFileName(file);
                string realPath = GetRealMovieName(file);
                var movieName = Path.GetFileNameWithoutExtension(file);
                var ext = Path.GetExtension(file);
 
                
                if (!moviesOnNas.Contains(destinationPath+realPath+ext,StringComparer.OrdinalIgnoreCase))
                {
                    if(ext != ".rar")
                    {
                        File.Move(file, destinationPath + realPath + ext);
                    }
                        

                }
                else
                {
                    MoviesNotToMail.Add(movieName);
                }

            }
            
        }

        public string GetRealMovieName(string filePath)
        {
            string[] directories = filePath.Split(Path.DirectorySeparatorChar);
            string pathen = directories[directories.Length - 2];
            return pathen;
        }

        public static void CleanUpTheLeftovers(string extractFolder)
        {
         
            Directory.Delete(extractFolder, true);
        
        }

       


    }
}
