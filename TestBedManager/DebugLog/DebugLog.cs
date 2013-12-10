using DebugLog.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

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

			CreateLogDirectory();
			CreateLogFile();

			isInitialized = true;
		}

		private static void CreateLogDirectory()
		{
			if (!Directory.Exists(Settings.Default.LogDir)) {
				Trace.WriteLine("Creating log files directory " + Settings.Default.LogDir + ".");
				try {
					Directory.CreateDirectory(Settings.Default.LogDir);
				} catch (Exception ex) {
					Trace.WriteLine("Couldn't create the logs folder at " + Settings.Default.LogDir + ": " + ex);
				}
			}
		}

		private static void CreateLogFile()
		{
			try {
				CreateNewLogFilePath();
				File.Create(logFilePath);
			} catch (Exception ex) {
				Trace.WriteLine("Couldn't create the log file " + logFilePath + ": " + ex);
			}
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
				if (ex is IOException)
					MessageBox.Show("The logs directory is in use. Please check if " +
						Settings.Default.LogDir + " is open.");
				Trace.WriteLine("Failed to delete and recreate log directory: " + ex.Message);
			}
		}

		public static void OpenLogsFolderInExplorer()
		{
			Initialize();
			CreateLogDirectory();

			Process proc = new Process();
			proc.StartInfo.FileName = "explorer.exe";
			proc.StartInfo.Arguments = Settings.Default.LogDir;
			try {
				proc.Start();
			} catch (Exception ex) {
				Trace.WriteLine(ex);
			}
		}

		private static void CreateNewLogFilePath()
		{
			logFilePath = Path.Combine(Settings.Default.LogDir,
				CreateNewLogFileName() + ".txt");
		}

		private static string CreateNewLogFileName()
		{
			return DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Year +
				"_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second +
				"-" + DateTime.Now.Millisecond;
		}

		public static string GetLogDirSetting()
		{
			return Settings.Default.LogDir;
		}
	}
}