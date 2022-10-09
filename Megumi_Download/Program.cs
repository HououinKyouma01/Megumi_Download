using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Megumi_Download
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var mutex = new Mutex(false, "Megumi Download"))
            {
                // return immediately without blocking
                bool isAnotherInstanceOpen = !mutex.WaitOne(TimeSpan.Zero);
                if (isAnotherInstanceOpen)
                {
                    Console.WriteLine("Only one instance of this app is allowed.");
                    Console.ReadKey();
                    return;
                }

                var configdic = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\config.megumi")

              .Select(l => l.Split(new[] { '=' }))
              .ToDictionary(s => s[0].Trim(), s => s[1].Trim());
                

                string host = configdic["HOST"];
                string username = configdic["USER"];
                string password = configdic["PASSWORD"];
                string remoteDirectory = configdic["REMOTEPATCH"];
                string animepath = configdic["LOCALPATCH"];
                string kodiswitch = configdic["KODISWITCH"];
                string saveinfo = configdic["SAVEINFO"];


                string tempdir = Path.GetTempPath();


                SFTPControl filessftp = new SFTPControl();
                filessftp.LoadConfig();
                filessftp.Listdir(host, username, password, remoteDirectory, tempdir);

                if (kodiswitch == "ON")
                {
                    Debug.WriteLine("ON");
                    string kodiaddress = configdic["KODIADDRESS"];
                    string kodiport = configdic["KODIPORT"];
                    string kodiuser = configdic["KODIUSER"];
                    string kodipass = configdic["KODIPASS"];
                    MoveFiles move = new MoveFiles();
                    move.LoadConfig();
                    move.Start(tempdir, animepath, kodiaddress, kodiuser, kodipass, kodiport, kodiswitch, saveinfo);
                }
                else
                {
                    MoveFiles move = new MoveFiles();
                    move.LoadConfig();
                    move.Start(tempdir, animepath, kodiswitch, saveinfo);
                }


            }
        }
    }
}
