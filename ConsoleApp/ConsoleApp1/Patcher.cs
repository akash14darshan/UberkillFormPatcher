using System.IO;
using System.IO.Compression;
using System;
using System.Diagnostics;
using System.Threading;

class Patcher
{
    public static void Extract()
    {
        Console.WriteLine(Environment.NewLine + "Extracting Files."+Environment.NewLine);
        string pathToZip = Path.GetTempPath() + "patchfiles.zip";
        string destination = Path.GetTempPath() + "Patch";
        try
        {
            if (Directory.Exists(destination))
                Directory.Delete(destination,true);
        }
        catch(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Red; 
            Console.WriteLine("Error Occured. Send the screenshot to uberkill staffs.\n"+e.ToString()); Console.ReadLine(); Environment.Exit(0); 
        }
        using (ZipArchive archive = ZipFile.OpenRead(pathToZip))
        {
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                string extractFilePath = Path.Combine(destination, entry.FullName).Replace("/","\\");
                if (entry.FullName.Contains("."))
                {
                    string directorypath = extractFilePath.Substring(0, extractFilePath.LastIndexOf('\\'));
                    Directory.CreateDirectory(directorypath);
                    Console.WriteLine(extractFilePath);
                    entry.ExtractToFile(extractFilePath);
                }                    
            }
            Console.WriteLine(Environment.NewLine + "Extracted Successfully.");
        }
    }
    static bool _Exited = false;
    public static void CopyFiles(string gamelocation)
    {
        Console.WriteLine(Environment.NewLine + "Copying files to game folder"+Environment.NewLine);
        BeginCopyFiles(gamelocation);
        while (!_Exited)
            Thread.Sleep(500);
        Cleanup();
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.Green;
        Console.WriteLine(Environment.NewLine + "Patcher has been installed Successfully. You can run Uberstrike now.");
    }
    static void BeginCopyFiles(string gamelocation)
    {
        Process proc = new Process();
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.CreateNoWindow = true;
        proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        proc.StartInfo.FileName = "cmd.exe";
        proc.StartInfo.Arguments = "/cxcopy \"" + Path.GetTempPath() + "Patch\\Uberstrike\"" + " \"" + gamelocation + "\" /s/h/e/k/f/c/y";
        proc.StartInfo.RedirectStandardOutput = true;
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.CreateNoWindow = true;
        proc.EnableRaisingEvents = true;
        proc.OutputDataReceived += (sender, args) => Display(args.Data);
        proc.ErrorDataReceived += (sender, args) => Display(args.Data);
        proc.Exited += (sender, args) => _Exited = true;
        proc.Start();
        proc.BeginOutputReadLine();
    }
    static void Display(string line)
    {
        Console.WriteLine(line);
    }
    public static void Cleanup()
    {
        string pathToZip = System.IO.Path.GetTempPath() + "patchfiles.zip";
        string destination = Path.Combine(Path.GetTempPath() , "Patch");
        try
        {
            if (Directory.Exists(destination))
                Directory.Delete(destination, true);
            if (File.Exists(pathToZip))
                File.Delete(pathToZip);
        }
        catch { }
    }
}

