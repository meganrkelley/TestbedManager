using System;
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
				computer.Log("There was a problem connecting to WMI. Error message: " + ex.Message);
			}
		}

		public static ManagementScope SetUpScope(RemoteComputer computer, string wmiClass = "none")
		{
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
				System.Diagnostics.Trace.WriteLine(ex);
			}
		}
	}
}