using System;
using System.Management;

namespace TestBedManager
{
	public class BatteryInfoTask : RemoteTask
	{
		public BatteryInfoTask(RemoteComputer computer)
			: base(computer)
		{
			SetUpWmiConnection(WmiClass.Battery);
		}

		public override void Run()
		{
			ObjectQuery query = new ObjectQuery(String.Format("select EstimatedChargeRemaining, Status, BatteryStatus from {0}", WmiClass.Battery));

			try {
				using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
					foreach (var item in wmiObjectSearcher.Get()) {
						UInt16 statusCode = (UInt16)item["BatteryStatus"];
						remoteComputer.Log("Battery: " +
							((UInt16)item["EstimatedChargeRemaining"]).ToString() +
							"% remaining. Status: " + GetBatteryStatusMessage(statusCode) + Environment.NewLine);
					}
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(string.Format("Error when executing WMI query/method on {0}: {1}",
					remoteComputer.ipAddressStr, ex));
				remoteComputer.Log("Error: " + ex.Message);
				WmiConnectionHandler.AttemptReconnect(mgmtClass.Scope);
			}
		}

		private string GetBatteryStatusMessage(UInt16 code)
		{
			switch (code) {
				case 1:
					return "The battery is discharging.";

				case 2:
					return "The system has access to AC so no battery is being discharged. However, the battery is not necessarily charging.";

				case 3:
					return "Fully charged";

				case 4:
					return "Low";

				case 5:
					return "Critical";

				case 6:
					return "Charging";

				case 7:
					return "Charging and high";

				case 8:
					return "Charging and low";

				case 9:
					return "Charging and critical";

				case 10:
					return "Undefined";

				case 11:
					return "Partially charged";

				default:
					return "Unknown";
			}
		}
	}
}