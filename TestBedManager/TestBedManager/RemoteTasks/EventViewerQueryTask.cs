using System;
using System.Management;

namespace TestBedManager
{
	public class EventViewerQueryTask : RemoteTask
	{
		public EventViewerQueryTask(RemoteComputer computer)
		{
			remoteComputer = computer;
			SetUpWmiConnection(WmiClass.EventViewer);
		}

		public override void Run(string[] parameters)
		{
			ObjectQuery query = new ObjectQuery(BuildQueryString(parameters));

			remoteComputer.Log("Querying Event Viewer...");

			try {
				using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
					foreach (var item in wmiObjectSearcher.Get()) {

						UInt16 eventCode = (UInt16)item["EventCode"];
						string msg = (string)item["Message"];
						string timeGenerated = (string)item["TimeGenerated"];

						DateTime date = ManagementDateTimeConverter.ToDateTime(timeGenerated);

						remoteComputer.Log("Event ID: " + eventCode + Environment.NewLine +
							"Time Generated: " + date + Environment.NewLine +
							"Message: " + msg + Environment.NewLine, false);
					}
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Error when executing WMI query/method on " + remoteComputer.ipAddressStr + ": " + ex);
				remoteComputer.Log("Error: " + ex.Message);
			}

			remoteComputer.Log("End events.");
		}

		private static string BuildQueryString(string[] parameters)
		{
			string queryStr = "select * from " + WmiClass.EventViewer + " where LogFile='System' ";

			if (parameters[0] != "")
				queryStr += "and EventCode=" + parameters[0] + " ";
			if (parameters[1] != "")
				queryStr += "and SourceName like '%" + parameters[1] + "%' ";
			if (parameters[2] != "")
				queryStr += "and Type='" + parameters[2] + "'";

			return queryStr;
		}
	}
}