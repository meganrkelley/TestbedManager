using System;
using System.Management;

namespace TestBedManager
{
	public class EventViewerQueryTask : RemoteTask
	{
		public EventViewerQueryTask(RemoteComputer computer) : base(computer)
		{
			SetUpWmiConnection(WmiClass.EventViewer);
		}

		public override void Run(string[] parameters)
		{
			ObjectQuery query = new ObjectQuery(BuildQueryString(parameters));

			remoteComputer.Log("Querying Event Viewer...");

			try {
				using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
					foreach (var item in wmiObjectSearcher.Get()) {

						DateTime date = ManagementDateTimeConverter.ToDateTime(
							item["TimeGenerated"].ToString());

						remoteComputer.Log(
							"Event ID: " + (UInt16)item["EventCode"] + Environment.NewLine +
							"Time Generated: " + date + Environment.NewLine +
							"Message: " + (string)item["Message"] + Environment.NewLine, 
							false);
					}
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(string.Format("Error when executing WMI query/method on {0}: {1}",
					remoteComputer.ipAddressStr, ex));
				remoteComputer.Log("Error: " + ex.Message);
				WmiConnectionHandler.AttemptReconnect(mgmtClass.Scope);
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