﻿using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Email;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Linq;
using System;

namespace UnrarMovies
{
    class Program
    {
        static void Main(string[] args)
        {

            string destinationPath = (@"/volume1/Media/Filmer/");
            //string destinationPath = @"C:\temp\";
            string path = @"/volume1/Download/Film/extracting/";
            //string path = @"C:\test\Download\Film\extracting\";
            string downloadPath = @"/volume1/Download/Film/";
            //string downloadPath = @"C:\test\Download\Film\";
            string kodiIp = "192.168.1.10";
            string kodiPort = "9002";
            JsonSerializeKodi Kodi = new JsonSerializeKodi();
            Kodi.id = 1;
            Kodi.jsonrpc = "2.0";
            Kodi.method = "VideoLibrary.Scan";
            // kolla efter fler metoder och se vad du behöver mer för propertys
            string httpBase = "http://" + kodiIp + ":" + kodiPort + "/jsonrpc?request=";
            string jsonKodiCall = JsonConvert.SerializeObject(Kodi);
            bool sendKodiUpdate = true;

            string[] allFiles = Directory.GetFiles(downloadPath, "*.rar", SearchOption.AllDirectories);
       
            bool NoRarsThisTime = false;
            FileInfoHandler files = new FileInfoHandler();
            UnrarArchives test = new UnrarArchives();
            NameHandler sortName = new NameHandler();
            List<string> MovieNames = new List<string>();
            CleanUp Cleaner = new CleanUp();
            Gmail OutMail = new Gmail()
            {
                SendFrom = "",
                PassWord = "",
                SendTo = "",
                TextHead = "Nya Filmer !"

            };
            // hämta logininfo från fil eller lägg det i assembly. Fil känns som det mest stabila
            
            if (allFiles.Count() < 1)
                NoRarsThisTime = true;
            foreach (var rar in allFiles)
            {
                test.extractArchive(path, rar);
                // här har du en bugg som kommer märkas tydligare när man använder sig mer av xvid filmer med rars på 15 mb
                // kolla om du inte kan ta och ändra värdet mer. Kolla över storleken på subs rar och se om dom är över 15mb. Detta gäller ju bara den
                // första raren och inte den som blir uppackad. Kolla om du inte kan ta och path.contains(subs) för då borde den kolla filnamnet
                // också och någon av dom borde innehålla subs, kanske.
                files.ReadFileSize(rar);

                if (files.IsItAMovie)
                {

                    string dir = sortName.GetDirName(rar);
                    MovieNames.Add(dir);

                    files.IsItAMovie = false;
                }
             }

            // Unpack all subs since alot of subs.rar comes with an extra rar in them.
            if (!NoRarsThisTime)
            {
             
                string[] subsRar = Directory.GetFiles(path, "*.rar", System.IO.SearchOption.AllDirectories);
                foreach (var subsRars in subsRar)
                {
                    test.extractArchive(path, subsRars);
                 }
               
                string[] filesToMove = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
             
                Cleaner.MoveTheFiles(filesToMove, destinationPath);
                
            }
            
            for (int i = 0; i < MovieNames.Count; i++)
            {
                if (Cleaner.CheckIfIShouldMail(MovieNames[i]))
               {
                    //Names are bad change fast
                    sortName.StartNameSorting(MovieNames[i]);
                 
                    var json = new WebClient().DownloadString("http://www.omdbapi.com/?t=" + sortName.MovieNameOMDB + "&y=&plot=short&r=json");
                    JsonDeterialize movie = JsonConvert.DeserializeObject<JsonDeterialize>(json);
         
                    bool checkMovie = string.IsNullOrWhiteSpace(movie.Title);
                    if (!checkMovie)
                    {
                        OutMail.TextBody += "<br />";
                        OutMail.TextBody += movie.Title + ". Rating : " + movie.imdbRating + " by " + movie.imdbVotes +
                         " voters ." + "Genre : " + movie.Genre + ". Runtime : " + movie.Runtime+ "\n";
                        OutMail.TextBody += "<br />";
                    }
                    else
                    {
                        OutMail.TextBody += "<br />";
                        OutMail.TextBody += MovieNames[i];
                        OutMail.TextBody += "<br />";
                    }
                }
            
            }
         
            if (!NoRarsThisTime)
            {
                CleanUp.CleanUpTheLeftovers(path);
            }
            
            List<string> notRaredMovies = new List<string>();
            notRaredMovies = NotRaredMovies.GetNotRaredMovies(destinationPath, downloadPath);
            foreach (var item in notRaredMovies)
            {
                sortName.StartNameSorting(item);
            
                var json = new WebClient().DownloadString("http://www.omdbapi.com/?t=" + sortName.MovieNameOMDB + "&y=&plot=short&r=json");
                JsonDeterialize movie = JsonConvert.DeserializeObject<JsonDeterialize>(json);
      
                bool checkMovie = string.IsNullOrWhiteSpace(movie.Title);
                if (!checkMovie)
                {
                    OutMail.TextBody += "<br />";
                    OutMail.TextBody += movie.Title + ". Rating : " + movie.imdbRating + " by " + movie.imdbVotes +
                     " voters ." + "Genre : " + movie.Genre + ". Runtime : " + movie.Runtime+ "\n";
                    OutMail.TextBody += "<br />";
                }
                else
                {
                    OutMail.TextBody += "<br />";
                    OutMail.TextBody += item;
                    OutMail.TextBody += "<br />";

                }
                
                CleanUp.CleanUpTheLeftovers(downloadPath + item);
                
            }

            // Detta var nödvändigt för att inte få ssl error på certificate när du kör den via mono....
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            
            if (OutMail.TextBody != null)
                OutMail.GmailSend();

            if (sendKodiUpdate)
            {
                using (var webClient = new WebClient())
                {
                    webClient.Headers.Set("Content-Type", "application/json");

                    var response = webClient.UploadString(httpBase, "POST", jsonKodiCall);

                }

            }




        }
    }
}
