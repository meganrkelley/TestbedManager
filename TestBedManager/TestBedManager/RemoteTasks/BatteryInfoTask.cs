using System;
using System.Management;

namespace TestBedManager
{
	public class BatteryInfoTask : RemoteTask
	{
		public BatteryInfoTask(RemoteComputer computer) : base(computer)
		{
			SetUpWmiConnection(WmiClass.Battery);
		}

		public override void Run()
		{
			ObjectQuery query = new ObjectQuery(String.Format("select EstimatedChargeRemaining, Status from {0}", WmiClass.Battery));

			try {
				using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
					foreach (var item in wmiObjectSearcher.Get()) {
						remoteComputer.Log("Battery: " + 
							((UInt16)item["EstimatedChargeRemaining"]).ToString() + 
							"% remaining. Status: " + item["Status"].ToString() + Environment.NewLine);
					}
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(string.Format("Error when executing WMI query/method on {0}: {1}",
					remoteComputer.ipAddressStr, ex));
				remoteComputer.Log("Error: " + ex.Message);
				WmiConnectionHandler.AttemptReconnect(mgmtClass.Scope);
			}
		}
	}
}