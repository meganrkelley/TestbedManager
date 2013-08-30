using System;
using System.Diagnostics;
using System.IO;

namespace TestBedManager
{
    public class DebugLog
    {
        static string logDirectory = @"C:\Users\Megan\Desktop\LogFiles";
        static string logFilePath;
        static bool isInitialized = false;

        public static void Initialize()
        {
            if (isInitialized)
                return;

            if (!Directory.Exists(logDirectory)) {
                Trace.WriteLine("Creating log files directory " + logDirectory + ".");
                Directory.CreateDirectory(logDirectory);
            }

            SetLogFileUri();
            isInitialized = true;

            Log("Log file " + logFilePath + " created.");
        }

        public static void Log(object text) 
        {
            Initialize();

            Trace.WriteLine(text);
            File.AppendAllText(logFilePath, text.ToString() + Environment.NewLine);
        }

        public static void ClearLogs()
        {
            Initialize();

            Directory.Delete(logDirectory, true);
            Directory.CreateDirectory(logDirectory);
        }

        public static void OpenLogsFolderInExplorer()
        {
            Initialize();

            Process proc = new Process();
            proc.StartInfo.FileName = "explorer.exe";
            proc.StartInfo.Arguments = logDirectory;
            proc.Start();
        }

        private static void SetLogFileUri()
        {
            logFilePath = Path.Combine(logDirectory, GetDateTimeForFilename() + ".txt");
        }

        private static string GetDateTimeForFilename()
        {
            return DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Year +
                "_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + 
                "-" + DateTime.Now.Millisecond;
        }

    }
}
