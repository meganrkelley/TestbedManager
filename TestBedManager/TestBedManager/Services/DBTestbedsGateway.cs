using System.Data;
using System.Data.SqlServerCe;

namespace TestBedManager
{
	class DBTestbedsGateway
	{
		public DataTable Find(int ID)
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select * from Lists where ID = " + ID, DBConnectionManager.connection);
			adapter.Fill(queryResultTable);
			return queryResultTable;
		}

		public DataTable Find(string title)
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select * from Lists where title = '" + title + "'", DBConnectionManager.connection);
			adapter.Fill(queryResultTable);
			return queryResultTable;
		}

        public DataTable SelectAll()
        {
            DataTable queryResultTable = new DataTable();
            var adapter = new SqlCeDataAdapter("select * from Lists", DBConnectionManager.connection);
            adapter.Fill(queryResultTable);
            return queryResultTable;
        }

		public void Update(int ID, string title)
		{
			var command = DBConnectionManager.connection.CreateCommand();
			command.CommandText = "update Lists set title = '" + title + "' where ID = " + ID;
			command.ExecuteNonQueryAsync();
			command.Dispose();
		}

		public void Insert(string title)
		{
			var command = DBConnectionManager.connection.CreateCommand();
			command.CommandText = "insert into Lists (title) values ('" + title + "')";
			command.ExecuteNonQueryAsync();
			command.Dispose();
		}

		public void Delete(int ID)
		{
			var command = DBConnectionManager.connection.CreateCommand();
			command.CommandText = "delete from Lists where ID = " + ID;
			command.ExecuteNonQueryAsync();
			command.Dispose();
		}
	}
}
