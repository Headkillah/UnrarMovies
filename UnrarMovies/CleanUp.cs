using System;
using System.Collections.Generic;
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
            //string[] moviesOnNas = Directory.GetFiles(destinationPath, "*.*");
            string[] moviesOnNas = Directory.GetFiles(destinationPath, "*.*", SearchOption.AllDirectories);


            

            foreach (var file in filesToMove)
            {
                //Console.WriteLine(file);
                var fileName = Path.GetFileName(file);
                string realPath = GetRealMovieName(file);
                var movieName = Path.GetFileNameWithoutExtension(file);
                var ext = Path.GetExtension(file);
               
                //Console.WriteLine(fileName);
                
                if (!moviesOnNas.Contains(destinationPath+realPath+ext))
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
            //Array.ForEach(Directory.GetFiles(@"/volume1/Download/Film/"), File.Delete);
            Directory.Delete(extractFolder, true);
            //Array.ForEach(Directory.GetFiles(@"c:\test\", SearchOption.AllDirectories), File.Delete);
        }


    }
}
