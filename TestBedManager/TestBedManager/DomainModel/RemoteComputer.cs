using System.Collections.Generic;
using System.Net;
using TestBedManager.Properties;

namespace TestBedManager
{
	public class RemoteComputer
	{
		#region Instance Variables
		
		private int _ID;
		private RemoteConnectionInfo _connectionInfo;
		private List<IComputerObserver> observers = new List<IComputerObserver>();

		private int _tabIndex;

		#endregion

		#region Constructors/Destructor

		public RemoteComputer(string hostname, IPAddress ipAddress, NetworkCredential credentials)
		{
			_connectionInfo = new RemoteConnectionInfo(hostname, ipAddress, credentials);
			Attach(Master.activeTestbed);
		}

		public RemoteComputer(string hostname, IPAddress ipAddress, string username, string password)
		{
			_connectionInfo = new RemoteConnectionInfo(hostname, ipAddress, new NetworkCredential(username, password));
			Attach(Master.activeTestbed);
		}

		public RemoteComputer(string hostname, string ipAddress, string username, string password)
		{
			_connectionInfo = new RemoteConnectionInfo(hostname, IPAddress.Parse(ipAddress), new NetworkCredential(username, password));
			Attach(Master.activeTestbed);
		}

		public RemoteComputer()
		{
			_connectionInfo = new RemoteConnectionInfo(Resources.DefaultHostname, IPAddress.None, new NetworkCredential());
			Attach(Master.activeTestbed);
		}

		~RemoteComputer()
		{
			List<IComputerObserver> copy = new List<IComputerObserver>(observers);
			foreach (IComputerObserver observer in copy)
				Detach(observer);
		}

		#endregion

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
		public RemoteConnectionInfo connectionInfo
		{
			get { return _connectionInfo; }
			set {
				if (value != _connectionInfo)
					Notify();
				_connectionInfo = value; 
			}
		}
		
		public NetworkCredential credentials
		{
			get { return _connectionInfo.credentials; }
			set {
				if (value != _connectionInfo.credentials)
					Notify();
				_connectionInfo.credentials = value; 
			}
		}

		public string hostname
		{
			get { return _connectionInfo.hostname; }
			set {
				if (value != _connectionInfo.hostname)
					Notify();
				_connectionInfo.hostname = value; 
			}
		}

		public IPAddress ipAddress
		{
			get { return _connectionInfo.ipAddress; }
			set {
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
			set {
				if (value != _connectionInfo.status)
					Notify(); 
				_connectionInfo.status = value; 
				
			}
		}

		public int tabIndex { get { return _tabIndex; } set { _tabIndex = value; } }

		#endregion

		#region Properties (bound to GUI)
		
		public string statusImage
		{
			get {
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
			get {
				switch (status) {
					case NetworkStatus.Unknown:
						return "Unknown";
					case NetworkStatus.Disconnected:
						return "Unable to ping";
					case NetworkStatus.WmiConnected:
						return "WMI is connected";
					default:
						return "Responding to ping only";
				}
			}
		}

		#endregion

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

		#endregion

		#region Logging

		public void Log(string text, bool printTimestamp = true)
		{
			if (text == null)
				text = "";
			Master.logManager.WriteToComputerTab(this, text, printTimestamp);
		}

		#endregion
	}
}