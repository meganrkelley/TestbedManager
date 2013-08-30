namespace TestBedManager
{
	public class RenameComputerTask : RemoteTask
	{
		public RenameComputerTask(RemoteComputer computer)
		{
			this.remoteComputer = computer;
			base.SetUpWmiConnection(WmiClass.ComputerSystem);
		}

		public override void Run(string newHostname)
		{
			var inParams = mgmtClass.GetMethodParameters("Rename");
			inParams["Name"] = newHostname;
			inParams["UserName"] = remoteComputer.credentials.UserName;
			inParams["Password"] = remoteComputer.credentials.Password;
			var outParams = mgmtClass.InvokeMethod("Rename", inParams, null);

			//TODO: Reboot: invoke "Reboot" from Win32_OperatingSystem (or just run shutdown -r)
		}
	}
}
