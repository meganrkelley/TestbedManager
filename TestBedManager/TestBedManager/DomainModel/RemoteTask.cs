using System.Collections.Generic;
using System.Management;
using System.Threading;

namespace TestBedManager
{
	public abstract class RemoteTask
	{
		private ManagementClass _mgmtClass;
		private RemoteComputer _remoteComputer;

		public ManagementClass mgmtClass
		{
			get { return _mgmtClass; }
			set { _mgmtClass = value; }
		}

		public RemoteComputer remoteComputer
		{
			get { return _remoteComputer; }
			set { _remoteComputer = value; }
		}

		public abstract void Run(string parameter);

		protected void SetUpWmiConnection(string wmiClass)
		{
			if (remoteComputer.credentials == null || remoteComputer.hostname == null)
				return;

			ManagementPath mgmtPath = new ManagementPath(wmiClass);
			ManagementScope scope = WmiConnectionHandler.SetUpScope(remoteComputer);
			mgmtClass = new ManagementClass(scope, mgmtPath, new ObjectGetOptions());

            Thread connectionThread = new Thread(new ParameterizedThreadStart(WmiConnectionHandler.ConnectToScope));
            connectionThread.Start(new List<object> { scope, remoteComputer });
		}
	}

	//PowerStateTask // sleep states
	//PowerSettingsTask // powerplan
	//BeepTask or EjectCdRomTask
}