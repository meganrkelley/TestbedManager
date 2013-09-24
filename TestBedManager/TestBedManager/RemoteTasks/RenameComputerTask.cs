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
			remoteComputer.Log("Attempting to rename computer from " + remoteComputer.hostname + " to " + newHostname + " and restart.");

			var inParams = mgmtClass.GetMethodParameters("Rename");
			inParams.SetPropertyValue("Name", newHostname);
			inParams.SetPropertyValue("UserName", remoteComputer.credentials.UserName);
			inParams.SetPropertyValue("Password", remoteComputer.credentials.Password);
			var outParams = mgmtClass.InvokeMethod("Rename", inParams, null);

			RemoteTask task = new CreateProcessTask(remoteComputer);
			task.Run("shutdown -r");
		}
	}
}