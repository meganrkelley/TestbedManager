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
			adapter.Fill(queryResultTable);
			return queryResultTable;
		}

		public DataTable FindByComputerID(int ID)
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select * from TestbedRelations where ComputerID = "
				+ ID, ConnectionManager.connection);
			adapter.Fill(queryResultTable);
			return queryResultTable;
		}

		public void Update(int ID, int ComputerID, int TestbedID)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "update TestbedRelations set ComputerID = " + ComputerID +
				", TestbedID = " + TestbedID + " where ID = " + ID;
			command.ExecuteNonQuery();
			command.Dispose();
		}

		public void Insert(int ComputerID, int TestbedID)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "insert into TestbedRelations (ComputerID, TestbedID) values ("
				+ ComputerID + ",  " + TestbedID + ")";
			command.ExecuteNonQuery();
			command.Dispose();
		}

		public void DeleteTestbed(int TestbedID)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "delete from TestbedRelations where TestbedID = " + TestbedID;
			command.ExecuteNonQuery();
			command.Dispose();
		}

		public void DeleteComputer(int ComputerID)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "delete from TestbedRelations where ComputerID = " + ComputerID;
			command.ExecuteNonQuery();
			command.Dispose();
		}

		public void DeleteComputerFromTestbed(int ComputerID, int TestbedID)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "delete from TestbedRelations where TestbedID = " + TestbedID +
				" and ComputerID = " + ComputerID;
			command.ExecuteNonQuery();
			command.Dispose();
		}
	}
}