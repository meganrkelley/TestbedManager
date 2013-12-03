using System;
using System.Data;
using System.Data.SqlServerCe;
using System.Net;

namespace TestBedManagerDB
{
	public class Computers
	{
		public DataTable Find(int computerID)
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select * from Computers where ID = " + computerID,
				ConnectionManager.connection);
			try {
				adapter.Fill(queryResultTable);
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Computers.Find failed: " + ex);
			}
			return queryResultTable;
		}

		public DataTable Find(IPAddress ipAddress)
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select * from Computers where Address = '"
				+ ipAddress.ToString() + "'", ConnectionManager.connection);
			try {
				adapter.Fill(queryResultTable);
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Computers.Find failed: " + ex);
			}
			return queryResultTable;
		}

		public DataTable Find(string hostname)
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select * from Computers where Hostname = '"
				+ hostname + "'", ConnectionManager.connection);
			try {
				adapter.Fill(queryResultTable);
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Computers.Find failed: " + ex);
			}
			return queryResultTable;
		}

		public DataTable SelectAll()
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select * from Computers", ConnectionManager.connection);
			try {
				adapter.Fill(queryResultTable);
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Computers.SelectAll failed: " + ex);
			}
			return queryResultTable;
		}

		public void Update(int ID, string hostname, string ipAddress, string username, string password)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "update Computers set Hostname = '" + hostname +
				"', Address = '" + ipAddress + "', Username = '" + username + "', Password = '"
				+ password + "' where ID = " + ID;
			try {
				command.ExecuteNonQuery();
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Computers.Update failed: " + ex);
			} 
			command.Dispose();
		}

		public int Insert(string hostname, string ipAddress, string username, string password)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "insert into Computers (Hostname, Address, Username, Password) values ('"
				+ hostname + "', '" + ipAddress + "', '" + username + "', '" + password + "')";
			try {
				command.ExecuteNonQuery();
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Computers.Insert failed: " + ex);
			} 
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
			try {
				command.ExecuteNonQuery();
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Computers.Delete failed: " + ex);
			} 
			command.Dispose();
		}

		// Both address and hostname must match
		public void Delete(string hostname, string address)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "delete from Computers where Address = '" + address +
				"' and Hostname = '" + hostname + "'";
			try {
				command.ExecuteNonQuery();
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Computers.Delete failed: " + ex);
			} 
			command.Dispose();
		}
	}
}