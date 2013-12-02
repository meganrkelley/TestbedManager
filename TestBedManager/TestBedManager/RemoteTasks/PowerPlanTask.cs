using System;
using System.Management;

namespace TestBedManager
{
	internal class PowerPlanTask : RemoteTask
	{
		public PowerPlanTask(RemoteComputer computer) : base(computer)
		{
			SetUpWmiConnection(WmiClass.PowerPlan);
		}

		public override void Run(string powerPlanName)
		{
			try {
				ActivatePlan(powerPlanName);
				ValidateActivePlan(powerPlanName);
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(string.Format("Error when executing WMI query/method on {0}: {1}", 
					remoteComputer.ipAddressStr, ex));
				remoteComputer.Log("Error: " + ex.Message);
				WmiConnectionHandler.AttemptReconnect(mgmtClass.Scope);
			}
		}

		private void ValidateActivePlan(string powerPlanName)
		{
			ObjectQuery selectActivePlanQuery = new ObjectQuery(
						String.Format("select * from {0} where IsActive=True",
						WmiClass.PowerPlan));

			using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, selectActivePlanQuery)) {
				foreach (ManagementObject item in wmiObjectSearcher.Get()) {
					string name = (string)item["ElementName"];

					if (name.Equals(powerPlanName, StringComparison.InvariantCultureIgnoreCase))
						remoteComputer.Log("Successfully changed power plan to " + name + ".");
					else
						remoteComputer.Log("Failed to change power plan to " + powerPlanName + ". The currently active power plan is " + name + ".");
				}
			}
		}

		private void ActivatePlan(string powerPlanName)
		{
			ObjectQuery selectGivenPlanQuery = new ObjectQuery(
						String.Format("select * from {0} where ElementName='{1}'",
						WmiClass.PowerPlan, powerPlanName));

			using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, selectGivenPlanQuery)) {
				foreach (ManagementObject item in wmiObjectSearcher.Get()) {
					if (!(bool)item["IsActive"])
						item.InvokeMethod("Activate", null);
				}
			}
		}
	}
}