﻿using System.Collections.Generic;
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

		public RemoteTask(RemoteComputer computer)
		{
			remoteComputer = computer;
		}

		public virtual void Run() { }
		public virtual void Run(string parameter) { }
		public virtual void Run(string[] parameters) { }

		protected void SetUpWmiConnection(string wmiClass)
		{
			if (remoteComputer.credentials == null || 
				remoteComputer.hostname == null)
				return;

			if (remoteComputer.credentials.UserName == "" ||
				remoteComputer.credentials.Password == "") {
					string msg = string.Format("Username or password was empty for {0}; cannot create connection.", 
						remoteComputer.ipAddressStr);
				DebugLog.DebugLog.Log(msg);
				remoteComputer.Log(msg);
			}

			ManagementPath mgmtPath = new ManagementPath(wmiClass);
			ManagementScope scope = WmiConnectionHandler.SetUpScope(remoteComputer, wmiClass);
			mgmtClass = new ManagementClass(scope, mgmtPath, new ObjectGetOptions());

			Thread connectionThread = new Thread(new ParameterizedThreadStart(WmiConnectionHandler.ConnectToScope));
			connectionThread.Start(new List<object> { scope, remoteComputer });
		}
	}
}