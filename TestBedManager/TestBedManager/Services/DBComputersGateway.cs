using System.Data;
using System.Data.SqlServerCe;

namespace TestBedManager
{
	/// <summary>
	/// Acts as an interface to query, update, insert into, or delete from 
	/// the table containing RemoteComputer information.
	/// </summary>
	class DBComputersGateway
	{
		public DataTable Find(int ID)
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select * from ComputerInfo where ID = " + ID, DBConnectionManager.connection);
			adapter.Fill(queryResultTable);
			return queryResultTable;
		}

		public DataTable Find(string IpAddress)
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select * from ComputerInfo where ipAddress = '" + IpAddress + "'", DBConnectionManager.connection);
			adapter.Fill(queryResultTable);
			return queryResultTable;
		}

		public void Update(int ID, string hostname, string ipAddress, string username, string password)
		{
			var command = DBConnectionManager.connection.CreateCommand();
			command.CommandText = "update ComputerInfo set hostname = '" + hostname + "', ipAddress = '" + ipAddress + "', username = '" + username + "', password = '" + password + "' where ID = " + ID;
			command.ExecuteNonQueryAsync();
			command.Dispose();
		}

		public void Insert(string hostname, string ipAddress, string username, string password)
		{
			var command = DBConnectionManager.connection.CreateCommand();
			command.CommandText = "insert into ComputerInfo (hostname, ipAddress, username, password) values ('" + hostname + "', '" + ipAddress + "', '" + username + "', '" + password + "')";
			command.ExecuteNonQueryAsync();
			command.Dispose();
		}

		public void Delete(int ID)
		{
			var command = DBConnectionManager.connection.CreateCommand();
			command.CommandText = "delete from ComputerInfo where ID = " + ID;
			command.ExecuteNonQueryAsync();
			command.Dispose();
		}
	}
}
