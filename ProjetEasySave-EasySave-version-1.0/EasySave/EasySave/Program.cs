using System;

namespace EasySave
{
    internal class Program
    {
        static void Main()
        {   
            string ProjectName = "EEpy";
            string logDirectory = "C:\\Users\\acer\\OneDrive\\Bureau\\ici";
            Log myLog=new Log(ProjectName, logDirectory);
            myLog.Create_log();

            Console.WriteLine("File created in {0}", logDirectory);
        }
    }
}
