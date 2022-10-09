using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Megumi_Download
{
    class SFTPControl
    {

        List<String> groups = new List<String>();


        public void LoadConfig()
        {
            string configfile = Directory.GetCurrentDirectory() + "\\groups.megumi";


            string[] lines = File.ReadAllLines(configfile);

            foreach (string line in lines)
            {

                groups.Add(line);

            }

        }


        public void Listdir(string host, string username, string password, string remoteDirectory, string tempdir)
        {

            using (SftpClient sftp = new SftpClient(host, username, password))
            {
                try
                {
                    sftp.Connect();
                    tempdir = tempdir + "MegumiDownloadTemp" + "\\";
                    var files = sftp.ListDirectory(remoteDirectory);
                    var tasks = new List<Task>();


                    foreach (var file in files)
                    {
                        for (int i = 0; i < groups.Count; i++)
                        {
                            if (file.Name.Contains("[" + groups[i] + "]"))
                            {
                                if (File.Exists(tempdir + file.Name))
                                {
                                    File.Delete(tempdir + file.Name);
                                }

                                using (Stream fileStream = File.OpenWrite(tempdir + file.Name))
                                {
                                    //sftp.BufferSize = 20 * 512;
                                    Console.WriteLine("Downloading: " + file.Name);
                                    sftp.DownloadFile(remoteDirectory + file.Name, fileStream);
                                    sftp.DeleteFile(remoteDirectory + file.Name);
                                }
                            }
                        }
                    }
                    Console.WriteLine("Finished");
                    System.Threading.Thread.Sleep(1000);
                    sftp.Disconnect();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("An exception has been caught " + e.ToString());
                }
            }
        }
    }
}

