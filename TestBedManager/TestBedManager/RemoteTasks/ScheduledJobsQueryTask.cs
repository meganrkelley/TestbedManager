using System;
using System.Management;

namespace TestBedManager
{
	public class ScheduledJobsQueryTask : RemoteTask
	{
		public ScheduledJobsQueryTask(RemoteComputer computer)
			: base(computer)
		{
			SetUpWmiConnection(WmiClass.ScheduledJob);
		}

		public override void Run()
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
				DebugLog.DebugLog.Log(string.Format("Error when executing WMI query/method on {0}: {1}",
					remoteComputer.ipAddressStr, ex));
				remoteComputer.Log("Error: " + ex.Message);
				WmiConnectionHandler.AttemptReconnect(mgmtClass.Scope);
			}

			remoteComputer.Log("End of scheduled tasks." + Environment.NewLine);
		}
	}
}