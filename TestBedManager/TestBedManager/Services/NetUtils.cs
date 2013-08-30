using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using TestBedManager.Properties;

namespace TestBedManager
{
	/// <summary>
	/// Responsible for retrieving and parsing the hostname and the IP address(es) of a given computer (asynchronously)
	/// </summary>
	public class NetUtils
	{
		private BackgroundWorker bgWorker;

		public NetUtils()
		{
			SetUpBackgroundWorker();
		}

		private void SetUpBackgroundWorker()
		{
			bgWorker = new BackgroundWorker {
				WorkerReportsProgress = true,
				WorkerSupportsCancellation = true
			};
			bgWorker.DoWork += worker_DoWork;
			bgWorker.RunWorkerCompleted += worker_RunWorkerCompleted;
		}

		public void GetHostEntryAsync(RemoteComputer computer)
		{
			if (!bgWorker.IsBusy)
				bgWorker.RunWorkerAsync(computer);
		}

		private string GetHostname(string ip)
		{
			if (Settings.Default.UseIp)
				return ip;

			string returnValue = Resources.DefaultHostname;

			try {
				return RemoveDnsSuffix(Dns.GetHostEntry(ip).HostName);
			} catch (Exception ex) {
				DebugLog.Log(ex);
			}

			return returnValue;
		}

		private List<IPAddress> GetIPv4Addresses(string hostname)
		{
			try {
				return RemoveIPv6Addresses(Dns.GetHostEntry(hostname).AddressList.ToList());
			} catch (Exception ex) {
				DebugLog.Log(ex);
			}

			return new List<IPAddress>(new IPAddress[] { IPAddress.None });
		}

		private IPAddress GetIPv4FromHostname(string hostname)
		{
			return GetIPv4Addresses(hostname)[0];
		}

		//public IEnumerable<string> GetNetViewHostnames()
		//{
		//	var hostnames = new List<string>();

		//	var proc = new Process {
		//		StartInfo = {
		//			FileName = "net.exe",
		//			Arguments = "view /all",
		//			CreateNoWindow = true,
		//			UseShellExecute = false,
		//			RedirectStandardOutput = true,
		//			RedirectStandardError = true
		//		}
		//	};
		//	proc.Start();

		//	var streamReader = new StreamReader(proc.StandardOutput.BaseStream,
		//										proc.StandardOutput.CurrentEncoding);

		//	string line;
		//	while ((line = streamReader.ReadLine()) != null)
		//		if (line.StartsWith("\\"))
		//			hostnames.Add(line.Substring(2).Substring(0,
		//				line.Substring(2).IndexOf(" ", StringComparison.Ordinal)).ToUpper());

		//	streamReader.Close();
		//	proc.WaitForExit(1000);

		//	return hostnames;
		//}

		private string RemoveDnsSuffix(string hostname)
		{
			if (hostname.Contains('.'))
				return hostname.Substring(0, hostname.IndexOf('.'));
			return hostname;
		}

		private List<IPAddress> RemoveIPv6Addresses(IEnumerable<IPAddress> addresses)
		{
			List<IPAddress> returnValue = new List<IPAddress>();
			foreach (IPAddress address in addresses)
				if (address.AddressFamily != AddressFamily.InterNetworkV6)
					returnValue.Add(address);
			return returnValue;
		}

		private void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			RemoteComputer computer = (RemoteComputer)e.Argument;

			if (computer.ipAddress == IPAddress.None)
				computer.ipAddress = GetIPv4FromHostname(computer.hostname);
			else
				computer.hostname = GetHostname(computer.ipAddressStr);

			e.Result = computer;
		}

		private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Master.activeTestbed.Add((RemoteComputer)e.Result);
		}
	}
}