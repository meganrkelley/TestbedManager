using System.Data;
using System.Data.SqlServerCe;

namespace TestBedManager
{
	class DBTestbedRelationsGateway
	{
		public DataTable Find(int ID)
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select * from ListRelations where ID = " + ID, 
                DBConnectionManager.connection);
			adapter.Fill(queryResultTable);
			return queryResultTable;
		}

		public void Update(int ID, int ComputerID, int TestbedID)
		{
			var command = DBConnectionManager.connection.CreateCommand();
			command.CommandText = "update ListRelations set ComputerID = " + ComputerID + 
                ", ListID = " + TestbedID + " where ID = " + ID;
			command.ExecuteNonQueryAsync();
			command.Dispose();
		}

		public void Insert(int ComputerID, int TestbedID)
		{
			var command = DBConnectionManager.connection.CreateCommand();
			command.CommandText = "insert into ListRelations (ComputerID, ListID) values (" 
                + ComputerID + ",  " + TestbedID + ")";
			command.ExecuteNonQueryAsync();
			command.Dispose();
		}

        public void DeleteTestbed(int TestbedID)
		{
			var command = DBConnectionManager.connection.CreateCommand();
            command.CommandText = "delete from ListRelations where ListID = " + TestbedID;
			command.ExecuteNonQueryAsync();
			command.Dispose();
		}

        public void DeleteComputer(int ComputerID)
        {
            var command = DBConnectionManager.connection.CreateCommand();
            command.CommandText = "delete from ListRelations where ComputerID = " + ComputerID;
            command.ExecuteNonQueryAsync();
            command.Dispose();
        }

        public void DeleteComputerFromTestbed(int ComputerID, int TestbedID)
        {
            var command = DBConnectionManager.connection.CreateCommand();
            command.CommandText = "delete from ListRelations where ListID = " + TestbedID + 
                " and ComputerID = " + ComputerID;
            command.ExecuteNonQueryAsync();
            command.Dispose();
        }
	}
}
