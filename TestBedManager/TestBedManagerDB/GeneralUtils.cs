namespace TestBedManagerDB
{
	public class GeneralUtils
	{
		public void ClearAllTables()
		{
			var command = ConnectionManager.connection.CreateCommand();

			command.CommandText = "delete from Computers";
			command.ExecuteNonQuery();
			ResetIdentitySeed("Computers");

			command.CommandText = "delete from Testbeds";
			command.ExecuteNonQuery();
			ResetIdentitySeed("Testbeds");

			command.CommandText = "delete from TestbedRelations";
			command.ExecuteNonQuery();
			ResetIdentitySeed("TestbedRelations");

			command.Dispose();

			DebugLog.DebugLog.Log("All tables have been cleared and their identity seeds reset.");
		}

		public void DeleteComputerFromAllTables(int ID)
		{
			var command = ConnectionManager.connection.CreateCommand();

			command.CommandText = string.Format("delete from Computers where ID={0}", ID);
			command.ExecuteNonQuery();

			command.CommandText = string.Format("delete from TestbedRelations where ComputerID={0}", ID);
			command.ExecuteNonQuery();

			command.Dispose();
		}

		private void ResetIdentitySeed(string tableName, string identityColumnName = "ID")
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = string.Format("alter table {0} alter column {1} identity (0,1)", 
				tableName, identityColumnName);
			command.ExecuteNonQuery();
			command.Dispose();
		}
	}
}
