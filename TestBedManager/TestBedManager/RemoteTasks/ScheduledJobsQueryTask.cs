using System;
using System.Management;

namespace TestBedManager
{
	public class ScheduledJobsQueryTask : RemoteTask
	{
		public ScheduledJobsQueryTask(RemoteComputer computer)
		{
			this.remoteComputer = computer;
			base.SetUpWmiConnection(WmiClass.ScheduledJob);
		}

		public override void Run(string parameter)
		{
			ObjectQuery query = new ObjectQuery(String.Format("select * from {0}", WmiClass.ScheduledJob));

			remoteComputer.Log("Querying scheduled tasks...");

			using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
				foreach (var item in wmiObjectSearcher.Get()) {
					remoteComputer.Log((string)item["Name"], false);
				}
			}

			remoteComputer.Log("End of scheduled tasks.");
		}
	}
}
