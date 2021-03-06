﻿using System;
using System.Collections.Generic;
using System.Management;

namespace TestBedManager
{
	public class WmiConnectionHandler
	{
		public static void ConnectToScope(object listOfScopeAndComputer)
		{
			List<object> collection = (List<object>)listOfScopeAndComputer;
			var scope = collection[0] as ManagementScope;
			var computer = collection[1] as RemoteComputer;
			try {
				scope.Connect();
				if (scope.IsConnected) {
					computer.status = NetworkStatus.WmiConnected;
					Master.table.RefreshItems();
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(ex);
				computer.Log("There was a problem connecting to WMI. Error message: " + ex.Message +
					Environment.NewLine + "Will now attempt to reconnect.");
				AttemptReconnect(scope);
			}
		}

		public static ManagementScope SetUpScope(RemoteComputer computer, string wmiClass = "none")
		{
			// Since NMU's hostnames are messed up, I have to use the IP address to access the machine.
			// This creates an issue with RenameComputer.
			string serverPath = String.Format(@"\\{0}\root\cimv2", computer.ipAddressStr);

			if (wmiClass == WmiClass.PowerPlan) // PowerPlan is a special case.
				serverPath = String.Format(@"\\{0}\root\cimv2\power", computer.ipAddressStr);

			ConnectionOptions connectionOptions;

			// localhost case
			if (computer.hostname.Equals("localhost", StringComparison.InvariantCultureIgnoreCase) ||
				computer.ipAddressStr.Equals("127.0.0.1", StringComparison.InvariantCultureIgnoreCase)) {
				connectionOptions = new ConnectionOptions {
					EnablePrivileges = true,
					Impersonation = ImpersonationLevel.Impersonate
				};
			} else {
				string decryptedPassword = Encryption.Decrypt(computer.credentials.Password);

				connectionOptions = new ConnectionOptions {
					EnablePrivileges = true,
					Authentication = AuthenticationLevel.PacketPrivacy,
					Impersonation = ImpersonationLevel.Impersonate,
					Username = computer.credentials.UserName,
					Password = decryptedPassword
				};

				//	computer.Log("Connection created with password '" + decryptedPassword + "'.");
			}

			return new ManagementScope(serverPath, connectionOptions);
		}

		public static void AttemptReconnect(ManagementScope scope, int numberOfTries = 3)
		{
			try {
				for (int i = 0; i < numberOfTries; i++) {
					scope.Connect();
				}
			} catch (Exception ex) {
				System.Diagnostics.Trace.WriteLine("Tried to reconnect to scope on server " + scope.Path.Server + ". Error: " + ex);
			}
		}
	}
}