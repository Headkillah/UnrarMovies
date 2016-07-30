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
            string[] moviesOnNas = Directory.GetFiles(@"/volume1/Media/Filmer/", "*.*");
            
            
            //string destinationPath = (@"C:\temp\");

            foreach (var file in filesToMove)
            {
                //Console.WriteLine(file);
                var fileName = Path.GetFileName(file);
                var movieName = Path.GetFileNameWithoutExtension(file);
                //Console.WriteLine(fileName);
                if (!moviesOnNas.Contains(@"/volume1/Media/Filmer/"+fileName))
                {
                    
                    File.Move(file, destinationPath + fileName);
                }
                else
                {
                    MoviesNotToMail.Add(movieName);
                }
            }
            
        }

        public static void CleanUpTheLeftovers()
        {
            Array.ForEach(Directory.GetFiles(@"/volume1/Download/Film/"), File.Delete);
        }


    }
}
