using System;
using System.IO;
using System.Threading;

namespace TestBedManager
{
	public class CreateProcessTask : RemoteTask
	{
		public CreateProcessTask(RemoteComputer computer)
		{
			remoteComputer = computer;
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
				DebugLog.DebugLog.Log("Error when executing WMI query/method on " + remoteComputer.ipAddressStr + ": " + ex);
				remoteComputer.Log("Error: " + ex.Message);
			}
			ReadPrintDelete(outputFilePath, 60);
		}

		private void ReadPrintDelete(string filepath, int timeoutInSeconds = 10, bool delete = true)
		{
			// Wait for filepath to exist before trying to read it.
			DateTime start = DateTime.Now;
			while (!File.Exists(filepath)) {
				if (TimeoutExceeded(timeoutInSeconds, start)) {
					remoteComputer.Log("File was not created after " + timeoutInSeconds + " seconds.");
					return;
				}
				Thread.Sleep(1000);
			}

			// Wait for file to be unlocked before trying to read it.
			start = DateTime.Now;
			while (IsFileLocked(new FileInfo(filepath))) {
				if (TimeoutExceeded(timeoutInSeconds, start)) {
					remoteComputer.Log("File was not unlocked after " + timeoutInSeconds + " seconds.");
					return;
				}
				Thread.Sleep(1000);
			}

			ReadFileAndPrint(filepath);

			if (!delete) 
				return;

			// Wait for it to unlock again.
			while (IsFileLocked(new FileInfo(filepath))) {
				if (TimeoutExceeded(timeoutInSeconds, start)) {
					remoteComputer.Log("File was not unlocked after " + timeoutInSeconds + " seconds.");
					return;
				}
				Thread.Sleep(1000);
			}

			DeleteFile(filepath);
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