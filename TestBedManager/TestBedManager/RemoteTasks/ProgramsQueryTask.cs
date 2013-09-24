using System;
using System.Management;

namespace TestBedManager
{
	public class ProgramsQueryTask : RemoteTask
	{
		public ProgramsQueryTask(RemoteComputer computer)
		{
			this.remoteComputer = computer;
			base.SetUpWmiConnection(WmiClass.Product);
		}

		public override void Run(string parameter)
		{
			ObjectQuery query = new ObjectQuery(String.Format("select * from {0}", WmiClass.Product));

			remoteComputer.Log("Querying installed programs...");

			using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
				foreach (var item in wmiObjectSearcher.Get()) {
					string productName = (string)item["Caption"];
					if (productName != null)
						remoteComputer.Log(productName, false);
				}
			}

			remoteComputer.Log("End of installed programs.");
		}
	}
}