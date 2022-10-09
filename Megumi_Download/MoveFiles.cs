using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Megumi_Download
{
    class MoveFiles
    {

        List<String> series = new List<String>();
        List<String> seriesdirs = new List<String>();
        List<String> season = new List<String>();

        public void LoadConfig()
        {

            string configfile = Directory.GetCurrentDirectory() + "\\serieslist.megumi";

            string[] lines = File.ReadAllLines(configfile);

            int i = 0;
            foreach (string line in lines)
            {

                string[] configsplit = line.Split(new char[] { '|' });

                series.Add(configsplit[0]);
                seriesdirs.Add(configsplit[1]);
                season.Add(configsplit[2]);
                i++;

            }

        }


        public void Start(string temppath, string animepath, string kodiswtitch, string saveinfo, string kodiaddress = "0", string kodiuser = "0", string kodipass = "0", string kodiport = "0")
        {
            temppath = temppath + "MegumiDownloadTemp" + "\\";
            Directory.CreateDirectory(temppath);
            string[] filelist = Directory.GetFiles(temppath)
                             .Select(Path.GetFileName)
                             .ToArray();

            foreach (string file in filelist)
            {

                for (int i = 0; i < series.Count; i++)
                {

                    if (file.Contains(series[i]))
                    {
                        /* Match epnum_ext = Regex.Match(file, @"\s([0-9][0-9])\s(\[.*\]).*(\..*)");*/
                        /*Match epnum_ext = Regex.Match(file, @"\s([0-9][0-9])\s((\(.*\))|(\[.*\])).*(\..*)"); */
                        Match epnum_ext = Regex.Match(file, @"\s([0-9][0-9])(\s((\(.*\))|(\[.*\]))).*(\..*)|\s([0-9][0-9])(\..*)");
                        if (epnum_ext.Groups[4].Success || epnum_ext.Groups[5].Success)
                        {
                            if (epnum_ext.Success)
                        {
                            Console.WriteLine("New episode of " + series[i] + " found");
                            Console.WriteLine("Moving " + epnum_ext.Groups[1].Value + " episode of " + series[i] + " to " + animepath + seriesdirs[i] + "\\" + "Season " + season[i]);
                            string nameafter = "S0" + season[i] + "E" + epnum_ext.Groups[1].Value + epnum_ext.Groups[6].Value;
                            Directory.CreateDirectory(animepath + seriesdirs[i] + "\\" + "Season " + season[i]);        

                            if (File.Exists(animepath + seriesdirs[i] + "\\" + "Season " + season[i] + "\\" + nameafter))
                            {
                                File.Delete(animepath + seriesdirs[i] + "\\" + "Season " + season[i] + "\\" + nameafter);
                            }
                                //check if file for replacement exis ! CHECK BELOW TOO
                            System.IO.File.Move(temppath + file, animepath + seriesdirs[i] + "\\" + "Season " + season[i] + "\\" + nameafter);
                                // save original ep name to info.txt in series folder
                                if (saveinfo == "ON") { 
                                    System.IO.File.AppendAllText(animepath + seriesdirs[i] + "\\" + "Season " + season[i] + "\\" + "info.txt", file + " (" + nameafter + ")" + "\n");
                                }
                                //check if file for replacement exis ! CHECK BELOW TOO
                                bool replacenames = (System.IO.File.Exists(animepath + seriesdirs[i] + "\\" + "Season " + season[i] + "\\" + "replace.txt") ? true : false);
                                if (replacenames == true)
                                {
                                    System.IO.File.Delete(temppath + "temppath.txt");
                                    System.IO.File.Delete(temppath + "temppathfile.txt");
                                    System.IO.File.AppendAllText(temppath + "temppath.txt", animepath + seriesdirs[i] + "\\" + "Season " + season[i] + "\\");
                                    System.IO.File.AppendAllText(temppath + "temppathfile.txt", animepath + seriesdirs[i] + "\\" + "Season " + season[i] + "\\" + nameafter);
                                }

                                if (kodiswtitch == "ON")
                            {
                                using (var webClient = new WebClient())
                                {
                                    webClient.Credentials = new NetworkCredential(kodiuser, kodipass);
                                    string json = "{\"jsonrpc\":\"2.0\",\"method\":\"GUI.ShowNotification\",\"params\":{\"title\":\"Download complete\",\"message\":\"" + series[i] + "\"},\"id\":1}";
                                    string jsonaddress = "http://" + kodiaddress + ":" + kodiport + "/" + "jsonrpc";
                                    var response = webClient.UploadString(jsonaddress, "POST", json);
                                    Debug.WriteLine(response);
                                }
                            }

                            

                        }
                        }
                        if (epnum_ext.Groups[7].Success)
                        {
                            if (epnum_ext.Success)
                            {
                                Console.WriteLine("New episode of " + series[i] + "found");
                                Console.WriteLine("Moving " + epnum_ext.Groups[7].Value + " episode of " + series[i] + " to " + animepath + seriesdirs[i] + "\\" + "Season " + season[i]);
                                string nameafter = "S0" + season[i] + "E" + epnum_ext.Groups[7].Value + epnum_ext.Groups[8].Value;
                                Directory.CreateDirectory(animepath + seriesdirs[i] + "\\" + "Season " + season[i]);

                                if (File.Exists(animepath + seriesdirs[i] + "\\" + "Season " + season[i] + "\\" + nameafter))
                                {
                                    File.Delete(animepath + seriesdirs[i] + "\\" + "Season " + season[i] + "\\" + nameafter);
                                }
                                //check if file for replacement exis ! CHECK ABOVE TOO
                                System.IO.File.Move(temppath + file, animepath + seriesdirs[i] + "\\" + "Season " + season[i] + "\\" + nameafter);

                                //check if file for replacement exis ! CHECK ABOVE TOO
                                bool replacenames = (System.IO.File.Exists(animepath + seriesdirs[i] + "\\" + "Season " + season[i] + "\\" + "replace.txt") ? true : false);
                               
                                // save original ep name to info.txt in series folder
                                if (saveinfo == "ON")
                                {
                                    System.IO.File.AppendAllText(animepath + seriesdirs[i] + "\\" + "Season " + season[i] + "\\" + "info.txt", file + " (" + nameafter + ")" + "\n");
                                }
                                if (replacenames == true)
                                {
                                    System.IO.File.Delete(temppath + "temppath.txt");
                                    System.IO.File.Delete(temppath + "temppathfile.txt");
                                    System.IO.File.AppendAllText(temppath + "temppath.txt", animepath + seriesdirs[i] + "\\" + "Season " + season[i] + "\\");
                                    System.IO.File.AppendAllText(temppath + "temppathfile.txt", animepath + seriesdirs[i] + "\\" + "Season " + season[i] + "\\" + nameafter);
                                }

                                if (kodiswtitch == "ON")
                                {
                                    using (var webClient = new WebClient())
                                    {
                                        webClient.Credentials = new NetworkCredential(kodiuser, kodipass);
                                        string json = "{\"jsonrpc\":\"2.0\",\"method\":\"GUI.ShowNotification\",\"params\":{\"title\":\"Download complete\",\"message\":\"" + series[i] + "\"},\"id\":1}";
                                        string jsonaddress = "http://" + kodiaddress + ":" + kodiport + "/" + "jsonrpc";
                                        var response = webClient.UploadString(jsonaddress, "POST", json);
                                        Debug.WriteLine(response);
                                    }
                                }



                            }
                        }
                    }

                    bool configfileexist = (System.IO.File.Exists(Path.GetTempPath() + "MegumiDownloadTemp" + "\\" + "temppath.txt") ? true : false);
                    bool muxfileexist = (System.IO.File.Exists(Path.GetTempPath() + "MegumiDownloadTemp" + "\\" + "temppathfile.txt") ? true : false);
                    if (configfileexist == true && muxfileexist == true)
                    {
                        Remux mux = new Remux();
                        mux.LoadConfig();
                        mux.Start();
                        mux.Mux();
                    }

                }

            }
        }
    }
}
