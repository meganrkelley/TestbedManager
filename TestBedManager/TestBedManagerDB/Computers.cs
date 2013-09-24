using System.Data;
using System.Data.SqlServerCe;
using System.Net;

namespace TestBedManagerDB
{
	public class Computers
	{
		//public DataTable Find(int ID)
		//{
		//	DataTable queryResultTable = new DataTable();
		//	var adapter = new SqlCeDataAdapter("select * from Computers where ID = " + ID,
		//		ConnectionManager.connection);
		//	adapter.Fill(queryResultTable);
		//	return queryResultTable;
		//}

		public DataTable Find(IPAddress ipAddress)
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select * from Computers where Address = '" 
				+ ipAddress.ToString() + "'", ConnectionManager.connection);
			adapter.Fill(queryResultTable);
			return queryResultTable;
		}

		public DataTable Find(string hostname)
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select * from Computers where Hostname = '"
				+ hostname + "'", ConnectionManager.connection);
			adapter.Fill(queryResultTable);
			return queryResultTable;
		}

		public DataTable SelectAll()
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select * from Computers", ConnectionManager.connection);
			adapter.Fill(queryResultTable);
			return queryResultTable;
		}

		public void Update(int ID, string hostname, string ipAddress, string username, string password)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "update Computers set Hostname = '" + hostname + 
				"', Address = '" + ipAddress + "', Username = '" + username + "', Password = '" 
				+ password + "' where ID = " + ID;
			command.ExecuteNonQuery();
			command.Dispose();
		}

		public int Insert(string hostname, string ipAddress, string username, string password)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "insert into Computers (Hostname, Address, Username, Password) values ('" 
				+ hostname + "', '" + ipAddress + "', '" + username + "', '" + password + "')";
			command.ExecuteNonQuery();
			command.Dispose();

			DataTable findResults = Find(hostname);
			if (findResults.Rows.Count == 0)
				return -1;
			foreach (DataRow row in findResults.Rows)
				return int.Parse(row["ID"].ToString());
			return -1;
		}

		public void Delete(int ID)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "delete from Computers where ID = " + ID;
			command.ExecuteNonQuery();
			command.Dispose();
		}

		// Both address and hostname must match
		public void Delete(string hostname, string address)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "delete from Computers where Address = '" + address +
				"' and Hostname = '" + hostname + "'";
			command.ExecuteNonQuery();
			command.Dispose();
		}
	}
}
