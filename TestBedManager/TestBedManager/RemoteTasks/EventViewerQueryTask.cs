using System;
using System.Management;

namespace TestBedManager
{
	public class EventViewerQueryTask : RemoteTask
	{
		public EventViewerQueryTask(RemoteComputer computer)
		{
			this.remoteComputer = computer;
			base.SetUpWmiConnection(WmiClass.EventViewer);
		}

		public override void Run(string parameter)
		{
			ObjectQuery query = new ObjectQuery(String.Format("select * from {0} where Logfile='System' and EventCode={1}", WmiClass.EventViewer, parameter));

			using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
				foreach (var item in wmiObjectSearcher.Get()) {
					// Byte[] data = (Byte[])item.GetPropertyValue("Data");
					string msg = (string)item["Message"];
					string timeGenerated = (string)item["TimeGenerated"];
					DateTime date = ManagementDateTimeConverter.ToDateTime(timeGenerated);
					remoteComputer.Log("Event ID: " + parameter);
					remoteComputer.Log("Time Generated: " + date);
					remoteComputer.Log("Message: " + msg);
				}
			}
		}
	}
}
