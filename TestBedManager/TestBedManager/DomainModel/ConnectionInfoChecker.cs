using System.Net;

namespace TestBedManager
{
	/// <summary>
	/// Checks that the given connection information for a new computer is valid.
	/// </summary>
	public class ConnectionInfoChecker
	{
		/// <summary>
		/// Tests whether the "hostnameOrIp" input is a hostname or IP address and resolves accordingly.
		/// </summary>
		/// <returns>Null if the input was invalid.</returns>
		public RemoteComputer GetValidRemoteComputer(string hostnameOrIp, string username, string password)
		{
			if (string.IsNullOrEmpty(hostnameOrIp.Trim()) ||
				string.IsNullOrEmpty(username.Trim()) ||
				string.IsNullOrEmpty(password.Trim())) {
					Master.main.ChangeStatusBarText("Username and password are required.");
					return null;
			}

			RemoteComputer computer = new RemoteComputer(hostnameOrIp, IPAddress.None, username, password);

			if (!IsValidIpAddress(hostnameOrIp))
				computer.hostname = hostnameOrIp;
			else
				computer.ipAddress = IPAddress.Parse(hostnameOrIp);

			new NetUtils().GetHostEntryAsync(computer);

			return computer;
		}

		public bool IsValidIpAddress(string ip)
		{
			if (ip == "0.0.0.0")
				return false;

			IPAddress outIp;
			return IPAddress.TryParse(ip, out outIp);
		}
	}
}