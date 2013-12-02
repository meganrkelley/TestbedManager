using System;

namespace TestBedManager
{
	public class RenameComputerTask : RemoteTask
	{
		public RenameComputerTask(RemoteComputer computer) : base(computer)
		{
			SetUpWmiConnection(WmiClass.ComputerSystem);
		}

		public override void Run(string newHostname)
		{
			remoteComputer.Log("Attempting to rename computer from " + remoteComputer.hostname + " to " + newHostname + ".");

			try {
				var inParams = mgmtClass.GetMethodParameters("Rename");
				inParams.SetPropertyValue("Name", newHostname);
				inParams.SetPropertyValue("UserName", remoteComputer.credentials.UserName);
				inParams.SetPropertyValue("Password", remoteComputer.credentials.Password);
				var outParams = mgmtClass.InvokeMethod("Rename", inParams, null);
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(string.Format("Error when executing WMI query/method on {0}: {1}",
					remoteComputer.ipAddressStr, ex));
				remoteComputer.Log("Error: " + ex.Message);
				WmiConnectionHandler.AttemptReconnect(mgmtClass.Scope);
			}

			remoteComputer.Log("You must restart this machine for the change to take effect.");
		}
	}
}