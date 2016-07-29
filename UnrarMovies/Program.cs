using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Email;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace UnrarMovies
{
    class Program
    {
        static void Main(string[] args)
        {

            string destinationPath = (@"/volume1/Media/Filmer/");
            string path = @"/volume1/Download/Film/";
            //string path = @"C:\test\";

            //string[] allFiles = System.IO.Directory.GetFiles(@"C:\test\", "*.rar", System.IO.SearchOption.AllDirectories);
            string[] allFiles = System.IO.Directory.GetFiles(@"/volume1/Download/Film/", "*.rar", System.IO.SearchOption.AllDirectories);

            FileInfoHandler files = new FileInfoHandler();
            UnrarArchives test = new UnrarArchives();
            NameHandler sortName = new NameHandler();
            List<string> MovieNames = new List<string>();
            Gmail OutMail = new Gmail()
            {
                SendFrom = "",
                PassWord = "",
                SendTo = "",
                TextHead = "Nya Filmer !"

            };
            // you have to set the pass and so on beforehand

            foreach (var rar in allFiles)
            {
                test.extractArchive(path, rar);
                files.ReadFileSize(rar);

                if (files.IsItAMovie)
                {

                    string dir = sortName.GetDirName(rar);
                    MovieNames.Add(dir);

                    files.IsItAMovie = false;
                }


            }

            // Unpack all subs since alot of subs.rar comes with an extra rar in them.

            //string[] subsRar = Directory.GetFiles(@"C:\test\", "*.rar");
            string[] subsRar = Directory.GetFiles(@"/volume1/Download/Film/", "*.rar");
            foreach (var subsRars in subsRar)
            {
                //Console.WriteLine(subsRars);
                test.extractArchive(path, subsRars);
                CleanUp.CleanUpSubRars(subsRars);

            }




            for (int i = 0; i < MovieNames.Count; i++)
            {
                //Names are bad change fast
                sortName.StartNameSorting(MovieNames[i]);
                //Console.WriteLine(sortName.MovieNameOMDB);
                var json = new WebClient().DownloadString("http://www.omdbapi.com/?t=" + sortName.MovieNameOMDB + "&y=&plot=short&r=json");
                JsonDeterialize movie = JsonConvert.DeserializeObject<JsonDeterialize>(json);
                //Console.WriteLine(movie.Title + " " + movie.Genre);
              
                OutMail.TextBody += movie.Title + ". Rating : " + movie.imdbRating + " by " +movie.imdbVotes +
                " voters ."+"Genre : "+movie.Genre+ ". Runtime : " + movie.Runtime;



                OutMail.TextBody += "<tr>";
                OutMail.TextBody += "<tr>";
            }
            //string[] filesToMove = Directory.GetFiles(@"c:\test\", "*.*");
            string[] filesToMove = Directory.GetFiles(@"/volume1/Download/Film/", "*.*");
            CleanUp.MoveTheFiles(filesToMove, destinationPath);

            // Detta var nödvändigt för att inte få ssl error på certificate när du kör den via mono....
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            OutMail.GmailSend();
         
            
        }
    }
}
