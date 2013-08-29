using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Threading;

namespace TestBedManager
{
	public class WmiConnectionHandler
	{
        //public static void TestConnection(RemoteComputer computer)
        //{
        //    ManagementScope scope = SetUpScope(computer);
        //    Thread connectionThread = new Thread(new ParameterizedThreadStart(ConnectToScope));
        //    connectionThread.Start(new List<object> {scope, computer});
        //}

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
				Trace.WriteLine(ex);
			}
		}

		public static ManagementScope SetUpScope(RemoteComputer computer)
		{
			string serverPath = String.Format(@"\\{0}\root\cimv2", computer.hostname);
			ConnectionOptions connectionOptions = new ConnectionOptions {
				Impersonation = ImpersonationLevel.Impersonate,
				Username = computer.credentials.UserName,
				Password = computer.credentials.Password
			};
			return new ManagementScope(serverPath, connectionOptions);
		}
	}
}
