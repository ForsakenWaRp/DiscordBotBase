using System;
using System.Threading.Tasks;

namespace ForsakenNet.Logging
{
    //The LogType's
    public enum LogType
    {
        Error,
        Log,
        Warning,
        Command
    }


    public class Log
    {
        //Not beautifull but works and easy..
        public static Task WriteLog(string log, LogType logType = LogType.Log)
        { //Switch it no Overhead no extra Lib or System just plain and simple Logging.
            switch(logType)
            {
                case LogType.Log:
                    Console.WriteLine($"[{getTime()} - Log] - {log}.");
                    break;
                case LogType.Command:
                    Console.WriteLine($"[{getTime()} - Command] - {log}.");
                    break;
                case LogType.Error:
                    Console.WriteLine($"[{getTime()} - Error] - {log}.");
                    break;
                case LogType.Warning:
                    Console.WriteLine($"[{getTime()} - Warning] - {log}.");
                    break;
            }

            return Task.CompletedTask;
        }
        //Get's the Time 
        private static string getTime()
        {
            return DateTime.Now.ToString("hh:mm:ss");
        }
    }


}
