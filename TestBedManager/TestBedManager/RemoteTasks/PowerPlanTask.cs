using System;
using System.Management;

namespace TestBedManager
{
	internal class PowerPlanTask : RemoteTask
	{
		public PowerPlanTask(RemoteComputer computer)
		{
			remoteComputer = computer;
			SetUpWmiConnection(WmiClass.PowerPlan);
		}

		public override void Run(string powerPlanName)
		{
			ObjectQuery query = new ObjectQuery(String.Format("select * from {0} where upper(ElementName)='{1}'", WmiClass.PowerPlan, powerPlanName.ToUpper()));

			try {
				using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
					foreach (ManagementObject item in wmiObjectSearcher.Get()) {
						if (!(bool)item["IsActive"])
							item.InvokeMethod("Activate", null);
					}
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Error when executing WMI query/method on " + remoteComputer.ipAddressStr + ": " + ex);
				remoteComputer.Log("Error: " + ex.Message);
			}

			query = new ObjectQuery(String.Format("select * from {0} where IsActive=True", WmiClass.PowerPlan));

			try {
				using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
					foreach (ManagementObject item in wmiObjectSearcher.Get()) {
						string name = (string)item["ElementName"];

						if (name.Equals(powerPlanName, StringComparison.InvariantCultureIgnoreCase))
							remoteComputer.Log("Successfully changed power plan to " + name + ".");
						else
							remoteComputer.Log("Failed to change power plan to " + powerPlanName + ". The currently active power plan is " + name + ".");
					}
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Error when executing WMI query/method on " + remoteComputer.ipAddressStr + ": " + ex);
				remoteComputer.Log("Error: " + ex.Message);
			}
		}
	}
}