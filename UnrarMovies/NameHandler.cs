using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UnrarMovies
{
    class NameHandler
    {
        public NameHandler()
        {
           
        }

        public void StartNameSorting(string inFile)
        {
            /// du måste göra en koll på filnamnet via Directoryinfo.Name för att få rätt mapp namn som du sen kastar ut
            startName = inFile;
            SortFileName();
        }

        public string GetDirName(string fullPath)
        {

            
            string[] directories = fullPath.Split(Path.DirectorySeparatorChar);
            string Dir = directories[directories.Length - 2];
            return Dir;
        }



        private string startName;

        public string StartName
        {
            get { return startName; }
            set { startName = value; }
        }

        private string year;

        public string Year
        {
            get { return year; }
            set { year = value; }
        }

       

        private string movieName;

        public string MovieName
        {
            get { return movieName; }
            set { movieName = value; }
        }

        private string movieNameOMDB;

        public string MovieNameOMDB
        {
            get
            {
                string outName = movieName.Replace(" ", "+");
                return outName;
            }
            set { movieNameOMDB = value; }
        }





        public void SortFileName()
        {

            string[] movieNameArray = Regex.Split(startName, @"\d{4}");
            string movieNameReplace = movieNameArray[0].Replace(".", " ");
            movieName = movieNameReplace;
        }
    }
}
