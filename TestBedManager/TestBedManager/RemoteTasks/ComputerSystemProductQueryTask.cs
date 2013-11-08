﻿using System;
using System.Management;

namespace TestBedManager
{
	public class ComputerSystemProductQueryTask : RemoteTask
	{
		public ComputerSystemProductQueryTask(RemoteComputer computer) : base(computer)
		{
			SetUpWmiConnection(WmiClass.ComputerSystemProduct);
		}

		public override void Run(string parameter)
		{
			ObjectQuery query = new ObjectQuery(String.Format("select * from {0}", WmiClass.ComputerSystemProduct));

			try {
				using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
					foreach (var item in wmiObjectSearcher.Get()) {
						string name = (string)item["Name"];
						string vendor = (string)item["Vendor"];
						remoteComputer.Log(name + " " + vendor);
					}
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(string.Format("Error when executing WMI query/method on {0}: {1}", 
					remoteComputer.ipAddressStr, ex));
				remoteComputer.Log("Error: " + ex.Message);
			}
		}
	}
}