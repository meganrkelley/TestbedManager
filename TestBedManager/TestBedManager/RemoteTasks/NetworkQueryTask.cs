using System;
using System.Management;

namespace TestBedManager
{
	public class NetworkQueryTask : RemoteTask
	{
		public NetworkQueryTask(RemoteComputer computer)
		{
			this.remoteComputer = computer;
			base.SetUpWmiConnection(WmiClass.NetworkConfig);
		}

		public override void Run(string parameter)
		{
			ObjectQuery query = new ObjectQuery(String.Format("select * from {0} where MACAddress is not null",
				WmiClass.NetworkConfig));

			using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
				foreach (var item in wmiObjectSearcher.Get()) {

					string caption = (string)item["Caption"];
					string serviceName = (string)item["ServiceName"];
					string dnsHostname = (string)item["DNSHostName"];
					string mac = (string)item["MACAddress"];

					string ipAddressesStr = "";
					string[] ipAddresses = (string[])item["IpAddress"];
					if (ipAddresses != null)
						for (int i = 0; i < ipAddresses.Length; i++)
							ipAddressesStr += ipAddresses[i] + Environment.NewLine;

					remoteComputer.Log("Device: " + caption);
					remoteComputer.Log("Service: " + serviceName);
					remoteComputer.Log("Hostname: " + dnsHostname);
					remoteComputer.Log("IP Address(es): " + ipAddressesStr);
					remoteComputer.Log("MAC: " + mac + Environment.NewLine);
				}
			}
		}
	}
}
