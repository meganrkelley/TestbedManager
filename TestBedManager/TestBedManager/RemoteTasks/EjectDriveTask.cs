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
			//string path = Path.Combine(@"\\" + remoteComputer.connectionInfo.hostname, 
			//	Directory.GetCurrentDirectory(), "ejectCD.bat");

			//try {
			//	try {
			//		File.Copy(Path.Combine(Directory.GetCurrentDirectory(), "ejectCD.bat"), path, true);
			//	} catch (Exception ex) {
			//		DebugLog.DebugLog.Log("Couldn't copy over ejectCD script: " + ex.Message);
			//	}

			//	CreateProcessTask createProc = new CreateProcessTask(remoteComputer);
			//	createProc.Run(path);
			//} catch (Exception ex) {
			//	DebugLog.DebugLog.Log(string.Format("Error when executing WMI query/method on {0}: {1}", 
			//		remoteComputer.ipAddressStr, ex));
			//	remoteComputer.Log("Error: " + ex.Message);
			//	WmiConnectionHandler.AttemptReconnect(mgmtClass.Scope);
			//}

			//remoteComputer.Log("Attempted to eject CD drive.");
		}
	}
}
