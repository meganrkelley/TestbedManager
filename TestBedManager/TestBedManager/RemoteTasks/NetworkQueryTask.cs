using System;
using System.Management;

namespace TestBedManager
{
	public class NetworkQueryTask : RemoteTask
	{
		public NetworkQueryTask(RemoteComputer computer)
			: base(computer)
		{
			SetUpWmiConnection(WmiClass.NetworkConfig);
		}

		public override void Run()
		{
			ObjectQuery query = new ObjectQuery(
				String.Format("select * from {0} where MACAddress is not null",
				WmiClass.NetworkConfig));

			remoteComputer.Log("Querying network information...");

			try {
				using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
					foreach (var item in wmiObjectSearcher.Get()) {
						remoteComputer.Log(BuildDataString(item), false);
					}
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(string.Format("Error when executing WMI query/method on {0}: {1}",
					remoteComputer.ipAddressStr, ex));
				remoteComputer.Log("Error: " + ex.Message);
				WmiConnectionHandler.AttemptReconnect(mgmtClass.Scope);
			}

			remoteComputer.Log("End of network information." + Environment.NewLine);
		}

		private static string BuildDataString(ManagementBaseObject item)
		{
			string ipAddressesStr = BuildIPAddressString(item);

			string result = "Device: " + (string)item["Caption"] + Environment.NewLine +
				"Service: " + (string)item["ServiceName"] + Environment.NewLine;

			if (!string.IsNullOrEmpty((string)item["DNSHostName"]))
				result += "Hostname: " + (string)item["DNSHostName"] + Environment.NewLine;
			if (!string.IsNullOrEmpty(ipAddressesStr))
				result += "IP Address(es): " + ipAddressesStr + Environment.NewLine;
			if (!string.IsNullOrEmpty((string)item["MACAddress"]))
				result += "MAC: " + (string)item["MACAddress"] + Environment.NewLine;

			return result;
		}

		private static string BuildIPAddressString(ManagementBaseObject item)
		{
			string result = "";

			string[] ipAddresses = (string[])item["IpAddress"];
			if (ipAddresses != null) {
				for (int i = 0; i < ipAddresses.Length; i++) {
					result += ipAddresses[i];
					if (i < ipAddresses.Length)
						result += Environment.NewLine;
				}
			}

			return result;
		}
	}
}