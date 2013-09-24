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
				if (scope.IsConnected)
					computer.status = NetworkStatus.WmiConnected;
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(ex);
			}
		}

		public static ManagementScope SetUpScope(RemoteComputer computer)
		{
			string serverPath = String.Format(@"\\{0}\root\cimv2", computer.hostname);
			ConnectionOptions connectionOptions;

			if (computer.hostname.Equals("localhost", StringComparison.InvariantCultureIgnoreCase) ||
				computer.ipAddressStr.Equals("127.0.0.1", StringComparison.InvariantCultureIgnoreCase)) {
				connectionOptions = new ConnectionOptions {
					EnablePrivileges = true,
					Impersonation = ImpersonationLevel.Impersonate
				};
			} else {
				connectionOptions = new ConnectionOptions {
					EnablePrivileges = true,
					Authentication = AuthenticationLevel.PacketPrivacy,
					Impersonation = ImpersonationLevel.Impersonate,
					Username = computer.credentials.UserName,
					Password = computer.credentials.Password
				};
			}

			return new ManagementScope(serverPath, connectionOptions);
		}
	}
}