﻿using System.Net;

namespace TestBedManager
{
	/// <summary>
	/// Holds state information about the network connection to a computer.
	/// </summary>
	public class RemoteConnectionInfo
	{
		#region Instance Variables

		private string _hostname;
		private IPAddress _ipAddress;
		private NetworkCredential _credentials;
		private NetworkStatus _status;

		#endregion Instance Variables

		#region Constructors

		public RemoteConnectionInfo(string hostname, IPAddress ipAddress)
		{
			_hostname = hostname;
			_ipAddress = ipAddress;
			_credentials = new NetworkCredential();
			_status = NetworkStatus.Unknown;
		}

		public RemoteConnectionInfo(string hostname, IPAddress ipAddress, NetworkCredential credentials)
		{
			_hostname = hostname;
			_ipAddress = ipAddress;
			_credentials = credentials;
			_status = NetworkStatus.Unknown;
		}

		#endregion Constructors

		#region Accessors

		public string hostname
		{
			get { return _hostname; }
			set { _hostname = value; }
		}

		public IPAddress ipAddress
		{
			get { return _ipAddress; }
			set { _ipAddress = value; }
		}

		public NetworkCredential credentials
		{
			get { return _credentials; }
			set { _credentials = value; }
		}

		public NetworkStatus status
		{
			get { return _status; }
			set { _status = value; }
		}

		#endregion Accessors
	}
}