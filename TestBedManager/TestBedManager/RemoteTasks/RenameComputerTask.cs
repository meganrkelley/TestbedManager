using System;
using System.Management;

namespace TestBedManager
{
	public class RenameComputerTask : RemoteTask
	{
		public RenameComputerTask(RemoteComputer computer)
			: base(computer)
		{
			SetUpWmiConnection(WmiClass.ComputerSystem);
		}

		public override void Run(string newHostname)
		{
			remoteComputer.Log("Attempting to rename computer from " + remoteComputer.hostname + " to " + newHostname + ".");

			try {
				using (var cs = new ManagementObject("Win32_ComputerSystem.Name='"
						+ remoteComputer.hostname + "'")) {
					cs.Get();
					var inParams = cs.GetMethodParameters("Rename");
					inParams.SetPropertyValue("Name", newHostname);
					var outParams = cs.InvokeMethod("Rename", inParams, null);
					remoteComputer.Log("You must restart this machine for the change to take effect.");
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(string.Format("Error when executing WMI query/method on {0}: {1}",
					remoteComputer.ipAddressStr, ex));
				remoteComputer.Log("Error: You must be able to ping '" + remoteComputer.hostname + "'. Message: " + ex.Message);
				WmiConnectionHandler.AttemptReconnect(mgmtClass.Scope);
			}
		}
	}
}