using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Net;
using System;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Threading;


namespace GUIPatch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        String gamelocation = "";
        int win64 = 0;

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/UvFM98d");
        }

        public void Download()
        {

            if (File.Exists(Directory.GetCurrentDirectory() + @"\patchfiles.zip"))
                File.Delete((Directory.GetCurrentDirectory() + @"\patchfiles.zip"));
            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                wc.DownloadFileAsync(new System.Uri("https://uberkill.cc/patcher/patchfiles.zip"), Directory.GetCurrentDirectory() + @"\patchfiles.zip");
            }

        }
        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            UnZip();
        }

        private void DeleteGameDir()
        {
            string currentdir = Directory.GetCurrentDirectory();
            string patchdir = currentdir + @"\Uberstrike";
            if (Directory.Exists(patchdir))
                Directory.Delete(patchdir, true);

        }

        public void UnZip()
        {
            textBox1.AppendText(Environment.NewLine + "Downloaded" + Environment.NewLine + "Extracting");
            string zipPath = "";
            Task t = Task.Run(() =>
            {
                DeleteGameDir();

                string currentdir = Directory.GetCurrentDirectory();

                zipPath = currentdir + @"\patchfiles.zip";
                string extractPath = currentdir;
                ZipFile.ExtractToDirectory(zipPath, extractPath);
            });
            t.Wait();
            string level12 = gamelocation+@"\Uberstrike_Data\level12";
            string level13 = gamelocation+@"\Uberstrike_Data\level13";
            if (File.Exists(zipPath))
                File.Delete(zipPath);
            if (File.Exists(level12))
                File.Delete(level12);
            if (File.Exists(level13))
                File.Delete(level13);
            if(win64==1)
            {
                if (File.Exists(Directory.GetCurrentDirectory() + @"\Uberstrike\UberEye32.exe"))
                {
                    File.Delete(Directory.GetCurrentDirectory() + @"\Uberstrike\UberEye32.exe");
                    System.IO.File.Move(Directory.GetCurrentDirectory() + @"\Uberstrike\UberEye64.exe", Directory.GetCurrentDirectory() + @"\Uberstrike\UberEye.exe");
                }
            }
            if (win64 == 0)
            {
                if (File.Exists(Directory.GetCurrentDirectory() + @"\Uberstrike\UberEye64.exe"))
                {
                    File.Delete(Directory.GetCurrentDirectory() + @"\Uberstrike\UberEye64.exe");
                    System.IO.File.Move(Directory.GetCurrentDirectory() + @"\Uberstrike\UberEye32.exe", Directory.GetCurrentDirectory() + @"\Uberstrike\UberEye.exe");
                }
            }
            string test = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            test = test.Substring(0, test.Length - 7) + @"LocalLow\Unity";
            if (Directory.Exists(test))
                Directory.Delete(test, true);
            Copy(gamelocation);

        }




        public string Copy(string gamelocation)
        {
            textBox1.AppendText(Environment.NewLine + "Patching Files");
            string copyout = "";
            string src = Directory.GetCurrentDirectory() + @"\Uberstrike";
            string dest = gamelocation + @"\";
            string qargs = @"/s/h/e/k/f/c/y";
            string cmdquery = "/C xcopy " + "\"" + src + "\"" + " " + "\"" + dest + "\"" + " " + qargs;

            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = cmdquery,
                    WorkingDirectory = Directory.GetCurrentDirectory(),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            proc.WaitForExit();

            DeleteGameDir();
            textBox1.AppendText(Environment.NewLine + "Successfully Patched");
            MessageBox.Show("You can run the game without Patcher");
            return copyout;
        }

        public string getLocation(string steamlocation)
        {
            string liblocation = steamlocation + "\\steamapps\\libraryfolders.vdf";
            var gamelocations = new List<string>();
            int counter = 0;
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(liblocation);
            while ((line = file.ReadLine()) != null)
            {
                int posi = line.IndexOf(':');
                if (posi > 0)
                {
                    posi--;
                    int posf = line.Length - 1;
                    string outtemp = line.Substring(posi, posf - posi) + @"\steamapps";
                    gamelocations.Add(outtemp);
                }
                counter++;
            }
            file.Close();
            gamelocations.Add(steamlocation + @"\steamapps");
            for (int i = 0; i < gamelocations.Count; i++)
            {
                string temppath = Convert.ToString(gamelocations[i]);
                if (File.Exists(temppath + @"\appmanifest_291210.acf"))
                    return ((temppath + @"\common\Uberstrike"));
            }
            return "notfound";

        }

        public string SteamPath()
        {
            string regpath;
            if (File.Exists("C:\\Windows\\SysWOW64\\calc.exe"))
            {
                regpath = @"SOFTWARE\WOW6432Node\Valve\Steam";
                win64 = 1;
            }
            else
                regpath = @"SOFTWARE\Valve\Steam";
 
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regpath))
            {
                if (key != null)
                {
                    Object o = key.GetValue("InstallPath");
                    if (o != null)
                    {
                        return (Convert.ToString(o));
                    }
                }
                return "notfound";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            gamelocation = getLocation(SteamPath());
            textBox1.AppendText("Detected Game Location:      " + gamelocation + Environment.NewLine + "Downloading");
            Download();
        }





        private void Button2_Click(object sender, EventArgs e)
        {

        }

        public void ProgressBar_Click(object sender, EventArgs e)
        {

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button2_Click_1(object sender, EventArgs e)
        {

        }
    }
}