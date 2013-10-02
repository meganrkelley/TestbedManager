using System;
using System.Management;

namespace TestBedManager
{
	public class ProgramsQueryTask : RemoteTask
	{
		public ProgramsQueryTask(RemoteComputer computer)
		{
			remoteComputer = computer;
			SetUpWmiConnection(WmiClass.Product);
		}

		public override void Run(string parameter)
		{
			ObjectQuery query = new ObjectQuery(String.Format("select * from {0}", WmiClass.Product));

			remoteComputer.Log("Querying installed programs...");

			try {
				using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
					foreach (var item in wmiObjectSearcher.Get()) {
						string productName = (string)item["Caption"];
						if (productName != null)
							remoteComputer.Log(productName, false);
					}
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Error when executing WMI query/method on " + remoteComputer.ipAddressStr + ": " + ex);
				remoteComputer.Log("Error: " + ex.Message);
			}

			remoteComputer.Log("End of installed programs.");
		}
	}
}