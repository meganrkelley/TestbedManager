using System;
using System.Management;

namespace TestBedManager
{
	public class BiosVersionTask : RemoteTask
	{
		public BiosVersionTask(RemoteComputer computer) : base(computer)
		{
			SetUpWmiConnection(WmiClass.BIOS);
		}

		public override void Run()
		{
			ObjectQuery query = new ObjectQuery(String.Format("select Version from {0}", WmiClass.BIOS));

			try {
				using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
					foreach (var item in wmiObjectSearcher.Get()) {
						remoteComputer.Log("BIOS version: " + item["Version"].ToString());
					}
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(string.Format("Error when executing WMI query/method on {0}: {1}",
					remoteComputer.ipAddressStr, ex));
				remoteComputer.Log("Error: " + ex.Message);
			}
		}
	}
}