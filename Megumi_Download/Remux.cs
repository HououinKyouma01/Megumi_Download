using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Megumi_Download
{
    class Remux
    {
        bool mkvmerge = (System.IO.File.Exists(Directory.GetCurrentDirectory() + "\\" + "mkvmerge.exe") ? true : false);
        bool mkvextract = (System.IO.File.Exists(Directory.GetCurrentDirectory() + "\\" + "mkvextract.exe") ? true : false);
        string configfile;
        string muxfile;
        List<String> regexx = new List<String>();
        List<String> oldnames = new List<String>();
        List<String> newnames = new List<String>();
        public Remux(string temppath, string movelocalonly)
        {



            if (movelocalonly == "OFF")
            {
                configfile = File.ReadAllText(temppath + "MegumiDownloadTemp" + "\\" + "temppath.txt");
                muxfile = File.ReadAllText(temppath + "MegumiDownloadTemp" + "\\" + "temppathfile.txt");

            }
            if (movelocalonly == "ON")
            {
                configfile = File.ReadAllText(temppath + "temppath.txt");
                muxfile = File.ReadAllText(temppath + "temppathfile.txt");

            }


            
        }


        public void LoadConfig()
        {

            if (mkvmerge == true && mkvextract == true)
            {
                string mkvextract = Directory.GetCurrentDirectory() + "\\" + "mkvextract.exe";
                string mkvmerge = Directory.GetCurrentDirectory() + "\\" + "mkvmerge.exe";
                if (File.Exists(configfile + "\\subs.ass"))
                {
                    File.Delete(configfile + "\\subs.ass");
                }
                // Make sure that only eng tracks left
                var remsub = Process.Start(mkvmerge, " -o " + "\"" + configfile + "out.mkv" + "\"" + " --subtitle-tracks eng" + " \"" + muxfile + "\"");
                
                remsub.WaitForExit();
                var exitCode = remsub.ExitCode;

                // remove file after leaving only EN
                File.Delete(muxfile);

                var process = Process.Start(mkvextract, " \"" + configfile + "out.mkv"+ "\"" + " tracks 2:" + "\"" + configfile + "\\subs.ass" + "\"");
                process.WaitForExit();
                var exitCode2 = process.ExitCode;

                string[] lines = File.ReadAllLines(configfile + "replace.txt");
                

                int i = 0;
            foreach (string line in lines)
            {

                string[] configsplit = line.Split(new char[] { '|' });

                oldnames.Add(configsplit[0]);
                newnames.Add(configsplit[1]);
                i++;

            }
            }

        }

        public void Start()
        {
            string text = File.ReadAllText(configfile + "\\subs.ass");
            text = text.Replace("Wh-wh", "W-Wh");
            text = text.Replace("Wh-Wh", "W-Wh");
            text = text.Replace("Th-th", "T-Th");
            text = text.Replace("Th-Th", "T-Th");
            text = text.Replace("A-a", "A-A");
            text = text.Replace("B-b", "B-B");
            text = text.Replace("C-c", "C-C");
            text = text.Replace("D-d", "D-D");
            text = text.Replace("E-e", "E-E");
            text = text.Replace("F-f", "F-F");
            text = text.Replace("G-g", "G-G");
            text = text.Replace("H-h", "H-H");
            text = text.Replace("I-i", "I-I");
            text = text.Replace("J-j", "J-J");
            text = text.Replace("K-k", "K-K");
            text = text.Replace("L-l", "L-L");
            text = text.Replace("M-m", "M-m");
            text = text.Replace("N-n", "N-N");
            text = text.Replace("O-o", "O-O");
            text = text.Replace("P-p", "P-P");
            text = text.Replace("Q-q", "Q-Q");
            text = text.Replace("R-r", "R-r");
            text = text.Replace("S-s", "S-s");
            text = text.Replace("T-t", "T-T");
            text = text.Replace("U-u", "U-U");
            text = text.Replace("W-w", "W-W");
            text = text.Replace("Y-y", "Y-Y");
            text = text.Replace("Z-z", "Z-Z");
            File.WriteAllText(configfile + "\\subs.ass", text);

            string[] subs = File.ReadAllLines(configfile + "subs.ass");
            File.Delete(configfile + "subs.ass");
            System.IO.File.WriteAllText(configfile + "subs.ass", "", Encoding.UTF8);
            foreach (string line in subs)
            {
                string repl = Regex.Replace(line, "-$", "—");
                

                for (int i = 0; i < oldnames.Count; i++)
                {
                    repl = Regex.Replace(repl, @"\b" + oldnames[i] + @"\b", newnames[i]);
                }

                File.AppendAllText(configfile + "\\subs.ass", repl + "\n");
            }



        }

        public void Mux()
        {
            string mkvextract = Directory.GetCurrentDirectory() + "\\" + "mkvextract.exe";
            string mkvmerge = Directory.GetCurrentDirectory() + "\\" + "mkvmerge.exe";
            var remsub = Process.Start(mkvmerge, " -o " + "\"" + muxfile + "\"" + " --no-subtitles" + " \"" + configfile + "out.mkv" + "\"" + " --language \"0:eng\"" + " --track-name \"0:MegumiDownloadFixed\"" + " \"" + configfile + "subs.ass" + "\"");
            remsub.WaitForExit();
            var exitCode3 = remsub.ExitCode;
            File.Delete(configfile + "out.mkv");
            File.Delete(configfile + "subs.ass");
           File.Delete(Path.GetTempPath() + "MegumiDownloadTemp" + "\\" + "temppath.txt");
           File.Delete(Path.GetTempPath() + "MegumiDownloadTemp" + "\\" + "temppathfile.txt");
        }

    }


}