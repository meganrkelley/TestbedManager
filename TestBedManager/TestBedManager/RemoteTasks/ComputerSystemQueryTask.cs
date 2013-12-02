using System;
using System.Management;

namespace TestBedManager
{
	public class ComputerSystemQueryTask : RemoteTask
	{
		public ComputerSystemQueryTask(RemoteComputer computer) : base(computer)
		{
			SetUpWmiConnection(WmiClass.ComputerSystem);
		}

		public override void Run()
		{
			ObjectQuery query = new ObjectQuery(String.Format("select * from {0}", WmiClass.OS));

			try {
				using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
					foreach (var item in wmiObjectSearcher.Get()) {
						DateTime date = ManagementDateTimeConverter.ToDateTime((string)item["InstallDate"]);
						remoteComputer.Log((string)item["Caption"] + " " + 
							(string)item["OSArchitecture"] + 
							" (Installed " + date + ")");
					}
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(string.Format("Error when executing WMI query/method on {0}: {1}", 
					remoteComputer.ipAddressStr, ex));
				remoteComputer.Log("Error: " + ex.Message);
				WmiConnectionHandler.AttemptReconnect(mgmtClass.Scope);
			}
		}
	}
}