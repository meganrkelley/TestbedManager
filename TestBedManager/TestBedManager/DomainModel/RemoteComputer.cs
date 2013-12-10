using System.Collections.Generic;
using System.Net;
using TestBedManager.Properties;

namespace TestBedManager
{
	public class RemoteComputer
	{
		public static RemoteComputer Dummy()
		{
			return new RemoteComputer("dummy", "123.123.123.123", "tester", "tester");
		}

		#region Instance Variables

		private int _ID;
		private ConnectionInfo _connectionInfo;
		private List<IComputerObserver> observers = new List<IComputerObserver>();

		#endregion Instance Variables

		#region Constructors/Destructor

		public RemoteComputer(string hostname, IPAddress ipAddress, string username, string password)
		{
			_connectionInfo = new ConnectionInfo(hostname, ipAddress, new NetworkCredential(username, password));
		}

		public RemoteComputer(string hostname, string ipAddress, string username, string password)
		{
			_connectionInfo = new ConnectionInfo(hostname, IPAddress.Parse(ipAddress), new NetworkCredential(username, password));
		}

		public RemoteComputer(System.Data.DataRow row)
		{
			ID = (int)row["ID"];
			_connectionInfo = new ConnectionInfo(
				(string)row["Hostname"],
				System.Net.IPAddress.Parse((string)row["Address"]),
				new System.Net.NetworkCredential(
					(string)row["Username"],
					(string)row["Password"]
			));
		}

		~RemoteComputer()
		{
			List<IComputerObserver> copy = new List<IComputerObserver>(observers);
			foreach (IComputerObserver observer in copy)
				Detach(observer);
		}

		#endregion Constructors/Destructor

		#region Accessors

		/// <summary>
		/// Represents the index of the computer in the TabControl.
		/// </summary>
		public int ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

		/// <summary>
		/// Holds network info for the computer like IP, hostname, and credentials.
		/// </summary>
		public ConnectionInfo connectionInfo
		{
			get { return _connectionInfo; }
			set
			{
				if (value != _connectionInfo)
					Notify();
				_connectionInfo = value;
			}
		}

		public NetworkCredential credentials
		{
			get { return _connectionInfo.credentials; }
			set
			{
				if (value != _connectionInfo.credentials)
					Notify();
				_connectionInfo.credentials = value;
			}
		}

		public string hostname
		{
			get { return _connectionInfo.hostname; }
			set
			{
				if (value != _connectionInfo.hostname)
					Notify();
				_connectionInfo.hostname = value;
			}
		}

		public IPAddress ipAddress
		{
			get { return _connectionInfo.ipAddress; }
			set
			{
				if (value != _connectionInfo.ipAddress)
					Notify();
				_connectionInfo.ipAddress = value;
			}
		}

		public string ipAddressStr
		{
			get { return _connectionInfo.ipAddress.ToString(); }
		}

		public NetworkStatus status
		{
			get { return _connectionInfo.status; }
			set
			{
				if (value != _connectionInfo.status)
					Notify();
				_connectionInfo.status = value;
			}
		}

		#endregion Accessors

		#region Properties (bound to GUI)

		public string statusImage
		{
			get
			{
				switch (status) {
					case NetworkStatus.Unknown:
						return Resources.Unknown;

					case NetworkStatus.Disconnected:
						return Resources.Disconnected;

					case NetworkStatus.WmiConnected:
						return Resources.WmiConnected;

					default:
						return Resources.Connected;
				}
			}
		}

		public string statusToolTip
		{
			get
			{
				switch (status) {
					case NetworkStatus.Unknown:
						return "Unknown status";

					case NetworkStatus.Disconnected:
						return "Unable to ping";

					case NetworkStatus.WmiConnected:
						return "WMI connection established";

					default:
						return "Responding to ping";
				}
			}
		}

		#endregion Properties (bound to GUI)

		#region Observing

		public void Attach(IComputerObserver observer)
		{
			observers.Add(observer);
		}

		public void Detach(IComputerObserver observer)
		{
			observers.Remove(observer);
		}

		public void Notify()
		{
			foreach (IComputerObserver observer in observers)
				observer.Update(this);
		}

		#endregion Observing

		#region Logging

		public void Log(string text, bool printTimestamp = true)
		{
			if (text == null)
				text = "";
			Master.logManager.WriteToComputerTab(this, text, printTimestamp);
		}

		#endregion Logging
	}
}