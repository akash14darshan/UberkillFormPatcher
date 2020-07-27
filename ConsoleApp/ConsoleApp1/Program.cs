using System;
class Program
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Finding Game Location........" + Environment.NewLine);
            string Gamepath = GamePath.Path;
            Console.WriteLine("Game Location Found: " + Gamepath);
            Downloader.DownloadFile();
            Patcher.Extract();
            Patcher.CopyFiles(Gamepath);
            Console.ReadLine();
        }
        catch(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Kindly send the screenshot to any developer/admin");
            Console.WriteLine(Environment.NewLine + e.ToString() + Environment.NewLine);
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}

