using System;
using System.IO;
using System.Threading;

namespace TestBedManager
{
	public class CreateProcessTask : RemoteTask
	{
		public CreateProcessTask(RemoteComputer computer) : base(computer)
		{
			SetUpWmiConnection(WmiClass.Process);
		}

		public override void Run(string command)
		{
			string outputFilePath = @"\\" + remoteComputer.ipAddressStr + @"\C$\" + Path.GetRandomFileName();
			string fullCommand = "cmd /c \"" + command + " 1> " + outputFilePath + " 2>&1\"";

			try {
				var inParams = mgmtClass.GetMethodParameters("Create");
				inParams["CommandLine"] = fullCommand;
				mgmtClass.InvokeMethod("Create", inParams, null);
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(string.Format("Error when executing WMI query/method on {0}: {1}", 
					remoteComputer.ipAddressStr, ex));
				remoteComputer.Log("Error: " + ex.Message);
			}
			ReadPrintDelete(outputFilePath, 60);
		}

		private void ReadPrintDelete(string filepath, int timeoutInSeconds = 10, bool delete = true)
		{
			WaitForFileExist(filepath, timeoutInSeconds);
			WaitForFileUnlock(filepath, timeoutInSeconds);

			ReadFileAndPrint(filepath);

			if (!delete) 
				return;

			WaitForFileUnlock(filepath, timeoutInSeconds);

			DeleteFile(filepath);
		}

		private bool WaitForFileUnlock(string filepath, int timeoutInSeconds) 
		{
			DateTime start = DateTime.Now;
			while (IsFileLocked(new FileInfo(filepath))) {
				if (TimeoutExceeded(timeoutInSeconds, start)) {
					remoteComputer.Log("File was not unlocked after " + timeoutInSeconds + " seconds.");
					return false;
				}
				Thread.Sleep(1000);
			}
			return true;
		}

		private bool WaitForFileExist(string filepath, int timeoutInSeconds)
		{
			DateTime start = DateTime.Now;
			while (!File.Exists(filepath)) {
				if (TimeoutExceeded(timeoutInSeconds, start)) {
					remoteComputer.Log("File was not created after " + timeoutInSeconds + " seconds.");
					return false;
				}
				Thread.Sleep(1000);
			}
			return true;
		}

		private static bool TimeoutExceeded(int timeoutInSeconds, DateTime start)
		{
			return start.AddSeconds(timeoutInSeconds) < DateTime.Now;
		}

		private void ReadFileAndPrint(string filepath)
		{
			try {
				remoteComputer.Log(File.ReadAllText(filepath));
				remoteComputer.Log("End of output.");
			} catch (Exception ex) {
				if (ex is FileNotFoundException ||
					ex is IOException)
					remoteComputer.Log("Error when attempting to read file: " + ex.Message);
			}
		}

		private void DeleteFile(string filepath)
		{
			try {
				File.Delete(filepath);
			} catch (Exception ex) {
				if (ex is FileNotFoundException ||
					ex is IOException)
					remoteComputer.Log("Error when attempting to delete file: " + ex.Message);
			}
		}

		protected virtual bool IsFileLocked(FileInfo file)
		{
			FileStream stream = null;

			try {
				stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
			} catch (IOException) {
				return true;
			} finally {
				if (stream != null)
					stream.Close();
			}

			return false;
		}
	}
}