using System.Data;
using System.Net;

namespace TestBedManager
{
    // alter table ComputerInfo alter column ID identity (0,1) <-- Reset identity seed (if at column index 0) to 1
	class DBComputerHandler
	{
		DBComputersGateway computerTableGateway = new DBComputersGateway();

		/// <summary>
		/// Find a RemoteComputer in the database by its ID.
		/// </summary>
		/// <returns>A new RemoteComputer with the matching information.</returns>
		public RemoteComputer Find(int ID)
		{
			DataTable findResults = computerTableGateway.Find(ID);
			if (findResults.Rows.Count == 0)
				return null;
			foreach (DataRow row in findResults.Rows) {
				return new RemoteComputer {
					ID = (int)row["ID"],
					hostname = row["hostname"].ToString(),
					ipAddress = IPAddress.Parse(row["ipAddress"].ToString()),
					credentials = new NetworkCredential(row["username"].ToString(), row["password"].ToString())
				};
			}
			return null;
		}

		/// <summary>
		/// Find a RemoteComputer in the database by its IP address.
		/// </summary>
		/// <returns>A new RemoteComputer with the matching information.</returns>
		public RemoteComputer Find(string IpAddress)
		{
			DataTable findResults = computerTableGateway.Find(IpAddress);
			if (findResults.Rows.Count == 0)
				return null;
			foreach (DataRow row in findResults.Rows) {
				return new RemoteComputer {
					ID = (int)row["ID"],
					hostname = row["hostname"].ToString(),
					ipAddress = IPAddress.Parse(row["ipAddress"].ToString()),
					credentials = new NetworkCredential(row["username"].ToString(), row["password"].ToString())
				};
			}
			return null;
		}

		public void Add(RemoteComputer computer)
		{
			// Check if computer is already in the database
			if (Find(computer.ipAddressStr) != null)
				return;
			computerTableGateway.Insert(computer.hostname, computer.ipAddressStr, computer.credentials.UserName, computer.credentials.Password);
			computer.ID = Find(computer.ipAddressStr).ID;
		}

		public void Update(RemoteComputer computer)
		{
			computerTableGateway.Update(computer.ID, computer.hostname, computer.ipAddressStr, computer.credentials.UserName, computer.credentials.Password);
		}

		public void Remove(int ID)
		{
			computerTableGateway.Delete(ID);
		}
	}
}
