using System;
using System.Management;

namespace TestBedManager
{
	public class ScheduledJobsQueryTask : RemoteTask
	{
		public ScheduledJobsQueryTask(RemoteComputer computer)
		{
			remoteComputer = computer;
			SetUpWmiConnection(WmiClass.ScheduledJob);
		}

		public override void Run(string parameter)
		{
			ObjectQuery query = new ObjectQuery(String.Format("select * from {0}", WmiClass.ScheduledJob));

			remoteComputer.Log("Querying scheduled tasks...");

			try {
				using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
					foreach (var item in wmiObjectSearcher.Get()) {
						remoteComputer.Log((string)item["Name"], false);
					}
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Error when executing WMI query/method on " + remoteComputer.ipAddressStr + ": " + ex);
				remoteComputer.Log("Error: " + ex.Message);
			}

			remoteComputer.Log("End of scheduled tasks.");
		}
	}
}