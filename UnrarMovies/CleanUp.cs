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


        public static bool MoveTheFiles(string[] filesToMove, string destinationPath)
        {
            string[] moviesOnNas = Directory.GetFiles(@"/volume1/Media/Filmer/", "*.*");


            //string destinationPath = (@"C:\temp\");
            foreach (var file in filesToMove)
            {
                //Console.WriteLine(file);
                var fileName = Path.GetFileName(file);
                //Console.WriteLine(fileName);
                if (!moviesOnNas.Contains(@"/volume1/Media/Filmer/"+fileName))
                {
                    File.Move(file, destinationPath + fileName);
                }
                
                
                
            }
            return true;

        }

        
    }
}
