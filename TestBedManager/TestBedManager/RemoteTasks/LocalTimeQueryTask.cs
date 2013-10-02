using System;
using System.Management;

namespace TestBedManager
{
	public class LocalTimeQueryTask : RemoteTask
	{
		public LocalTimeQueryTask(RemoteComputer computer)
		{
			remoteComputer = computer;
			SetUpWmiConnection(WmiClass.LocalTime);
		}

		public override void Run(string parameter)
		{
			ObjectQuery query = new ObjectQuery(String.Format("select * from {0}", WmiClass.LocalTime));

			try {
				using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
					foreach (var item in wmiObjectSearcher.Get()) {
						uint mon = (uint)item["Month"];
						uint day = (uint)item["Day"];
						uint year = (uint)item["Year"];
						uint hour = (uint)item["Hour"];
						uint min = (uint)item["Minute"];
						uint sec = (uint)item["Second"];
						remoteComputer.Log(mon + "/" + day + "/" + year + " " + hour + ":" + min + ":" + sec);
					}
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Error when executing WMI query/method on " + remoteComputer.ipAddressStr + ": " + ex);
				remoteComputer.Log("Error: " + ex.Message);
			}
		}
	}
}