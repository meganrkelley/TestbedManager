using System;
using System.Diagnostics;
using System.IO;
using DebugLog.Properties;

namespace DebugLog
{
	public class DebugLog
	{
		private static string logFilePath;
		private static bool isInitialized = false;

		public static void Initialize()
		{
			if (isInitialized)
				return;

			if (!Directory.Exists(Settings.Default.LogDir)) {
				Trace.WriteLine("Creating log files directory " + Settings.Default.LogDir + ".");
				Directory.CreateDirectory(Settings.Default.LogDir);
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

			Directory.Delete(Settings.Default.LogDir, true);
			Directory.CreateDirectory(Settings.Default.LogDir);
		}

		public static void OpenLogsFolderInExplorer()
		{
			Initialize();

			Process proc = new Process();
			proc.StartInfo.FileName = "explorer.exe";
			proc.StartInfo.Arguments = Settings.Default.LogDir;
			proc.Start();
		}

		private static void SetLogFileUri()
		{
			logFilePath = Path.Combine(Settings.Default.LogDir, GetDateTimeForFilename() + ".txt");
		}

		private static string GetDateTimeForFilename()
		{
			return DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Year +
				"_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second +
				"-" + DateTime.Now.Millisecond;
		}
	}
}