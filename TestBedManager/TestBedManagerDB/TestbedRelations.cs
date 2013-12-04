using System;
using System.Data;
using System.Data.SqlServerCe;

namespace TestBedManagerDB
{
	public class TestbedRelations
	{
		public DataTable FindByTestbedID(int ID)
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select * from TestbedRelations where TestbedID = "
				+ ID, ConnectionManager.connection);
			try {
				adapter.Fill(queryResultTable);
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("TestbedRelations.FindByTestbedID failed: " + ex);
			}
			return queryResultTable;
		}

		public DataTable FindByComputerID(int ID)
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select * from TestbedRelations where ComputerID = "
				+ ID, ConnectionManager.connection);
			try {
				adapter.Fill(queryResultTable);
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("TestbedRelations.FindByTestbedID failed: " + ex);
			}
			return queryResultTable;
		}

		public void Update(int ID, int ComputerID, int TestbedID)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "update TestbedRelations set ComputerID = " + ComputerID +
				", TestbedID = " + TestbedID + " where ID = " + ID;
			try {
				command.ExecuteNonQuery();
				command.Dispose();
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("TestbedRelations.Update failed: " + ex);
			}
		}

		public void Insert(int ComputerID, int TestbedID)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "insert into TestbedRelations (ComputerID, TestbedID) values ("
				+ ComputerID + ",  " + TestbedID + ")";
			try {
				command.ExecuteNonQuery();
				command.Dispose();
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("TestbedRelations.Insert failed: " + ex);
			}
		}

		public void DeleteTestbed(int TestbedID)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "delete from TestbedRelations where TestbedID = " + TestbedID;
			try {
				command.ExecuteNonQuery();
				command.Dispose();
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("TestbedRelations.DeleteTestbed failed: " + ex);
			}
		}

		public void DeleteComputer(int ComputerID)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "delete from TestbedRelations where ComputerID = " + ComputerID;
			try {
				command.ExecuteNonQuery();
				command.Dispose();
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("TestbedRelations.DeleteComputer failed: " + ex);
			}
		}

		public void DeleteComputerFromTestbed(int ComputerID, int TestbedID)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "delete from TestbedRelations where TestbedID = " + TestbedID +
				" and ComputerID = " + ComputerID;
			try {
				command.ExecuteNonQuery();
				command.Dispose();
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("TestbedRelations.DeleteComputerFromTestbed failed: " + ex);
			}
		}
	}
}