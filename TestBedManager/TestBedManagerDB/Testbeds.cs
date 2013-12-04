using System;
using System.Data;
using System.Data.SqlServerCe;

namespace TestBedManagerDB
{
	public class Testbeds
	{
		public DataTable Find(int ID)
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select * from Testbeds where ID = " + ID,
				ConnectionManager.connection);
			try {
				adapter.Fill(queryResultTable);
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Testbeds.Find failed: " + ex);
			}
			return queryResultTable;
		}

		public DataTable Find(string title)
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select * from Testbeds where Title = '"
				+ title + "'", ConnectionManager.connection);
			try {
				adapter.Fill(queryResultTable);
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Testbeds.Find failed: " + ex);
			}
			return queryResultTable;
		}

		public DataTable SelectAll()
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select * from Testbeds", ConnectionManager.connection);
			try {
				adapter.Fill(queryResultTable);
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Testbeds.SelectAll failed: " + ex);
			}
			return queryResultTable;
		}

		public void Update(int ID, string title)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "update Testbeds set Title = '" + title + "' where ID = " +
				ID;
			try {
				command.ExecuteNonQuery();
				command.Dispose();
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Testbeds.Update failed: " + ex);
			}
		}

		public int Insert(string title)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "insert into Testbeds (Title) values ('" + title + "')";
			try {
				command.ExecuteNonQuery();
				command.Dispose();
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Testbeds.Insert failed: " + ex);
			}

			DataTable findResults = Find(title);
			foreach (DataRow row in findResults.Rows) {
				if (row["ID"] is DBNull)
					return -1;
				return (int)row["ID"];
			}

			return -1;
		}

		public void Delete(int ID)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "delete from Testbeds where ID = " + ID;
			try {
				command.ExecuteNonQuery();
				command.Dispose();
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Testbeds.Delete failed: " + ex);
			} 
		}

		// alter table Testbeds alter column ID identity (0,1)
		// There must be one entry in the Testbeds table or the ID will start over at 0.
		public int GetNextID()
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select max(ID) as maxID from Testbeds", ConnectionManager.connection);
			try {
				adapter.Fill(queryResultTable);
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Testbeds.GetNextID failed: " + ex);
			}
			foreach (DataRow row in queryResultTable.Rows) {
				if (row["maxID"] is DBNull) {
					ResetIDSeed();
					return 0;
				}
				return (int)row["maxID"] + 1;
			}
			return -1;
		}

		private void ResetIDSeed()
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "alter table Testbeds alter column ID identity (0,1)";
			try {
				command.ExecuteNonQuery();
				command.Dispose();
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Testbeds.ResetIDSeed failed: " + ex);
			} 
		}
	}
}