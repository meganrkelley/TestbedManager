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
			File.Copy(Path.Combine(
				Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "ejectCD.bat"), 
				path, true);

			CreateProcessTask createProc = new CreateProcessTask(remoteComputer);
			createProc.Run(path);

			remoteComputer.Log("Attempted to eject CD drive.");
		}
	}
}
