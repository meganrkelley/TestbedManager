using System;
using System.Management;

namespace TestBedManager
{
	public class ComputerSystemQueryTask : RemoteTask
	{
		public ComputerSystemQueryTask(RemoteComputer computer)
		{
			remoteComputer = computer;
			SetUpWmiConnection(WmiClass.ComputerSystem);
		}

		public override void Run(string parameter)
		{
			ObjectQuery query = new ObjectQuery(String.Format("select * from {0}", WmiClass.OS));

			try {
				using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
					foreach (var item in wmiObjectSearcher.Get()) {
						string name = (string)item["Caption"];
						string arch = (string)item["OSArchitecture"];
						string installDate = (string)item["InstallDate"];
						DateTime date = ManagementDateTimeConverter.ToDateTime(installDate);
						remoteComputer.Log(name + " " + arch + " (Installed " + date + ")");
					}
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Error when executing WMI query/method on " + remoteComputer.ipAddressStr + ": " + ex);
				remoteComputer.Log("Error: " + ex.Message);
			}
		}
	}
}