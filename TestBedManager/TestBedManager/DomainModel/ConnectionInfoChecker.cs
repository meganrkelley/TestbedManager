using System.Net;
using System.Windows.Forms;

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
			if (string.IsNullOrEmpty(username.Trim()) ||
				string.IsNullOrEmpty(password.Trim())) {
				if (!(hostnameOrIp == "localhost" ||
					hostnameOrIp == "127.0.0.1")) {
					MessageBox.Show("Username and password are required for remote connections.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return null;
				}
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