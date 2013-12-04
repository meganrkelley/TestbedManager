using System;
using System.Management;

namespace TestBedManager
{
	public class RunningProcessesQueryTask : RemoteTask
	{
		public RunningProcessesQueryTask(RemoteComputer computer) : base(computer)
		{
			SetUpWmiConnection(WmiClass.Process);
		}

		public override void Run()
		{
			ObjectQuery query = new ObjectQuery(String.Format("select * from {0}", WmiClass.Process));

			remoteComputer.Log("Querying running processes...");

			try {
				using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
					foreach (var item in wmiObjectSearcher.Get()) {
						remoteComputer.Log(item["Caption"].ToString(), false);
					}
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(string.Format("Error when executing WMI query/method on {0}: {1}",
					remoteComputer.ipAddressStr, ex));
				remoteComputer.Log("Error: " + ex.Message);
				WmiConnectionHandler.AttemptReconnect(mgmtClass.Scope);
			}

			remoteComputer.Log("End of running processes." + Environment.NewLine);
		}
	}
}