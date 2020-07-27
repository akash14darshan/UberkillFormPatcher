using System.Diagnostics;
using System.IO;
using System.Threading;
class GamePath
{
    public static string Path
    {
        get
        {
            int count = 0;
            while(true)
            {
                if(count>1)
                    System.Console.WriteLine(System.Environment.NewLine + "Attempting again. Trial number {0} : ", count + 1 + System.Environment.NewLine);
                if (count == 5)
                {
                    System.Console.ForegroundColor = System.ConsoleColor.Black;
                    System.Console.BackgroundColor = System.ConsoleColor.Red;
                    System.Console.WriteLine(System.Environment.NewLine + "Couldnt find game directory. \nMake sure steam and game are installed. Contact staffs for help if the problem persists."+System.Environment.NewLine);
                    System.Console.ReadLine();
                    System.Environment.Exit(0);                   
                }
                LaunchGame();
                string path = GetPath();
                if (path.Contains(":"))
                    return path;
                else count++;
            }
        }
    }
    static void LaunchGame()
    {
        Process process = Process.Start("steam://rungameid/291210");
        process.WaitForExit();
    }
    static string GetPath()
    {
        string path = "Not Found";
        bool closed = false;
        while(true)
        {
            Process[] processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                if (process.ProcessName.ToLower().Contains("uberstrike"))
                {
                    path = process.MainModule.FileName.Substring(0, process.MainModule.FileName.LastIndexOf('\\'));
                    closed = true;
                    process.Kill();
                    break;
                }
            }
            if(closed)
            break;
            Thread.Sleep(500);
        }
        return path;
    }
}

