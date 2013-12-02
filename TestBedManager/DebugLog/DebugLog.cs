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

			Settings.Default.LogDir = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
			Settings.Default.Save();

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
			try {
				File.AppendAllText(logFilePath, text.ToString() + Environment.NewLine);
			} catch (Exception ex) {
				Trace.WriteLine("Failed to append text to log file: " + ex.Message);
			}
		}

		public static void ClearLogs()
		{
			Initialize();

			try {
				Directory.Delete(Settings.Default.LogDir, true);
				Directory.CreateDirectory(Settings.Default.LogDir);
			} catch (Exception ex) {
				Trace.WriteLine("Failed to delete and recreate log directory: " + ex.Message);
			}
		}

		public static void OpenLogsFolderInExplorer()
		{
			Initialize();

			Process proc = new Process();
			proc.StartInfo.FileName = "explorer.exe";
			proc.StartInfo.Arguments = Settings.Default.LogDir;
			try {
				proc.Start();
			} catch (Exception ex) {
				Trace.WriteLine(ex);
			}
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