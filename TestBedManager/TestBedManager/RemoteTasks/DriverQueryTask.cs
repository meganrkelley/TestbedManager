using System;
using System.Management;

namespace TestBedManager
{
	public class DriverQueryTask : RemoteTask
	{
		public DriverQueryTask(RemoteComputer computer) : base(computer)
		{
			SetUpWmiConnection(WmiClass.PnPSignedDriver);
		}

		public override void Run(string deviceClass)
		{
			ObjectQuery query = new ObjectQuery(String.Format(
				"select * from {0} where DeviceClass='{1}' and DeviceName is not null and DriverVersion is not null", 
				WmiClass.PnPSignedDriver, deviceClass.ToUpper()));

			remoteComputer.Log("Querying " + deviceClass + " drivers...");

			try {
				using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
					foreach (var item in wmiObjectSearcher.Get()) {
						remoteComputer.Log((string)item["DeviceName"] + " " + 
							(string)item["DriverVersion"], false);
					}
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Error when executing WMI query/method on " + 
					remoteComputer.ipAddressStr + ": " + ex);
				remoteComputer.Log("Error: " + ex.Message);
				WmiConnectionHandler.AttemptReconnect(mgmtClass.Scope);
			}

			remoteComputer.Log("End of " + deviceClass + " drivers." + Environment.NewLine);
		}
	}
}