using System;
using System.ComponentModel;
using System.Net;
using System.Threading;

class Downloader
{
    private static volatile bool completed=false;
    public static void DownloadFile()
    {
        Console.WriteLine("Downloading Patcher..." + Environment.NewLine);
        string address = "https://raw.githubusercontent.com/akashdarshan99/Updates/master/patchfiles.zip";
        string location = System.IO.Path.GetTempPath()+"patchfiles.zip";
        WebClient client = new WebClient();
        Uri Uri = new Uri(address);
        client.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
        client.DownloadFileAsync(Uri, location);
        while (!DownloadCompleted)
            Thread.Sleep(500);
    }
    public static bool DownloadCompleted { get { return completed; } }
    private static void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
    {
        Console.Write("\r{0}    Downloaded {1} of {2} KB. {3} % Completed...",
        (string)e.UserState,
        (int)e.BytesReceived/1024,
        (int)e.TotalBytesToReceive/1024,
        (int)e.ProgressPercentage);
    }
    private static void Completed(object sender, AsyncCompletedEventArgs e)
    {
        Console.WriteLine(Environment.NewLine);
        if (e.Cancelled == true)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Download has been canceled.");
            Console.ReadKey();
            Environment.Exit(0);
        }
        else if(e.Error!=null)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Error.ToString());
            Console.ReadKey();
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("Download completed!");
        }
        completed = true;
    }
}
