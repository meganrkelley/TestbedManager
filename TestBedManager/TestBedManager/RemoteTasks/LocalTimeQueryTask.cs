using System;
using System.Management;

namespace TestBedManager
{
	public class LocalTimeQueryTask : RemoteTask
	{
		public LocalTimeQueryTask(RemoteComputer computer) : base(computer)
		{
			SetUpWmiConnection(WmiClass.LocalTime);
		}

		public override void Run()
		{
			ObjectQuery query = new ObjectQuery(String.Format("select * from {0}", WmiClass.LocalTime));

			try {
				using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
					foreach (var item in wmiObjectSearcher.Get())
						remoteComputer.Log(FormatDateTime(wmiObjectSearcher, query));
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(string.Format("Error when executing WMI query/method on {0}: {1}", 
					remoteComputer.ipAddressStr, ex));
				remoteComputer.Log("Error: " + ex.Message);
				WmiConnectionHandler.AttemptReconnect(mgmtClass.Scope);
			}
		}

		private string FormatDateTime(ManagementObjectSearcher searcher, ObjectQuery query)
		{
			using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
				foreach (var item in wmiObjectSearcher.Get()) {
					return (uint)item["Month"] + "/" + 
						(uint)item["Day"] + "/" + 
						(uint)item["Year"] + " " + 
						Pad((uint)item["Hour"]) + ":" + 
						Pad((uint)item["Minute"]) + ":" + 
						Pad((uint)item["Second"]);
				}
			}

			return "ERROR";
		}

		private static string Pad(uint unit)
		{
			string unitStr = unit.ToString();
			if (unitStr.Length == 1)
				unitStr = "0" + unitStr;
			return unitStr;
		}
	}
}