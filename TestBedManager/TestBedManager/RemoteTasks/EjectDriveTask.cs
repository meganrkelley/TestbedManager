using System;
using System.IO;

namespace TestBedManager
{
	class EjectDriveTask : RemoteTask
	{
		public EjectDriveTask(RemoteComputer computer) : base(computer)
		{
			SetUpWmiConnection(WmiClass.Process);
		}

		public override void Run()
		{
			// copy ejectCD.bat over and run it
			string path = @"\\" + remoteComputer.connectionInfo.hostname + @"\C$\ejectCD.bat";
			try {
				File.Copy(Path.Combine(
						Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "ejectCD.bat"),
						path, true);

				CreateProcessTask createProc = new CreateProcessTask(remoteComputer);
				createProc.Run(path);
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(string.Format("Error when executing WMI query/method on {0}: {1}", 
					remoteComputer.ipAddressStr, ex));
				remoteComputer.Log("Error: " + ex.Message);
			}

			remoteComputer.Log("Attempted to eject CD drive.");
		}
	}
}
