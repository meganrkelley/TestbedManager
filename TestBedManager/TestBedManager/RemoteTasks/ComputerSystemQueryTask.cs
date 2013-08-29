using System;
using System.Management;

namespace TestBedManager
{
	public class ComputerSystemQueryTask : RemoteTask
	{
		public ComputerSystemQueryTask(RemoteComputer computer)
		{
			this.remoteComputer = computer;
			base.SetUpWmiConnection(WmiClass.ComputerSystem);
		}

		public override void Run(string parameter)
		{
			ObjectQuery query = new ObjectQuery(String.Format("select * from {0}", WmiClass.OS));

			using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
				foreach (var item in wmiObjectSearcher.Get()) {
					string name = (string)item["Caption"];
					string arch = (string)item["OSArchitecture"];
					string installDate = (string)item["InstallDate"];
					DateTime date = ManagementDateTimeConverter.ToDateTime(installDate);
					remoteComputer.Log(name + " " + arch + " (Installed " + date + ")");
				}
			}
		}
	}
}
