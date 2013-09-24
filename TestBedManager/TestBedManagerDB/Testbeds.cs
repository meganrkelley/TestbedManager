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
			adapter.Fill(queryResultTable);
			return queryResultTable;
		}

		public DataTable Find(string title)
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select * from Testbeds where Title = '" 
				+ title + "'", ConnectionManager.connection);
			adapter.Fill(queryResultTable);
			return queryResultTable;
		}

		public DataTable SelectAll()
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select * from Testbeds", ConnectionManager.connection);
			adapter.Fill(queryResultTable);
			return queryResultTable;
		}

		public void Update(int ID, string title)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "update Testbeds set Title = '" + title + "' where ID = " + 
				ID;
			command.ExecuteNonQuery();
			command.Dispose();
		}

		public int Insert(string title)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "insert into Testbeds (Title) values ('" + title + "')";
			command.ExecuteNonQuery();
			command.Dispose();

			DataTable findResults = Find(title);
			if (findResults.Rows.Count == 0)
				return -1;
			foreach (DataRow row in findResults.Rows)
				return int.Parse(row["ID"].ToString());
			return -1;
		}

		public void Delete(int ID)
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = "delete from Testbeds where ID = " + ID;
			command.ExecuteNonQuery();
			command.Dispose();
		}

		public int GetNextID()
		{
			DataTable queryResultTable = new DataTable();
			var adapter = new SqlCeDataAdapter("select max(ID) as maxID from Testbeds", ConnectionManager.connection);
			adapter.Fill(queryResultTable);
			foreach (DataRow row in queryResultTable.Rows) {
				if (string.IsNullOrEmpty(row["maxID"].ToString()))
					return 0;
				return int.Parse(row["maxID"].ToString());
			}
			return -1;
		}
	}
}
