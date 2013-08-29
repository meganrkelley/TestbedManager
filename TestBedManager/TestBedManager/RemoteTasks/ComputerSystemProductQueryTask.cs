using System;
using System.Management;

namespace TestBedManager
{
	public class ComputerSystemProductQueryTask : RemoteTask
	{
		public ComputerSystemProductQueryTask(RemoteComputer computer)
		{
			this.remoteComputer = computer;
			base.SetUpWmiConnection(WmiClass.ComputerSystemProduct);
		}

		public override void Run(string parameter)
		{
			ObjectQuery query = new ObjectQuery(String.Format("select * from {0}", WmiClass.ComputerSystemProduct));

			using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
				foreach (var item in wmiObjectSearcher.Get()) {
					string name = (string)item["Name"];
					string vendor = (string)item["Vendor"];
					remoteComputer.Log(name + " " + vendor);
				}
			}
		}
	}
}
