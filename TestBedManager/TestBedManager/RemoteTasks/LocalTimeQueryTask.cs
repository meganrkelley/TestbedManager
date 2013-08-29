using System;
using System.Management;

namespace TestBedManager
{
	public class LocalTimeQueryTask : RemoteTask
	{
		public LocalTimeQueryTask(RemoteComputer computer)
		{
			this.remoteComputer = computer;
			base.SetUpWmiConnection(WmiClass.LocalTime);
		}

		public override void Run(string parameter)
		{
			ObjectQuery query = new ObjectQuery(String.Format("select * from {0}", WmiClass.LocalTime));

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
		}
	}
}
