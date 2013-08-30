using System;
using System.Management;

namespace TestBedManager
{
	public class RunningProcessesQueryTask : RemoteTask
	{
		public RunningProcessesQueryTask(RemoteComputer computer)
		{
			this.remoteComputer = computer;
			base.SetUpWmiConnection(WmiClass.Process);
		}

		public override void Run(string parameter)
		{
			ObjectQuery query = new ObjectQuery(String.Format("select * from {0}", WmiClass.Process));

			remoteComputer.Log("Querying running processes...");

			using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
				foreach (var item in wmiObjectSearcher.Get()) {
					remoteComputer.Log(item["Caption"].ToString(), false);
				}
			}

			remoteComputer.Log("End of running processes.");
		}
	}
}
