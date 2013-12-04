using System;
using System.Data.SqlServerCe;

namespace TestBedManagerDB
{
	public class GeneralUtils
	{
		public void ClearAllTables()
		{
			try {
				var command = ConnectionManager.connection.CreateCommand();
				ExecuteCommandAndReset(command, "Computers");
				ExecuteCommandAndReset(command, "Testbeds");
				ExecuteCommandAndReset(command, "TestbedRelations");
				command.Dispose();
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("GeneralUtils.ClearAllTables failed: " + ex);
			}

			DebugLog.DebugLog.Log("All tables have been cleared and their identity seeds reset.");
		}

		private void ExecuteCommandAndReset(SqlCeCommand command, string tableName)
		{
			command.CommandText = "delete from " + tableName;
			command.ExecuteNonQuery();
			ResetIdentitySeed(tableName);
		}

		public void DeleteComputerFromAllTables(int ID)
		{
			try {
				var command = ConnectionManager.connection.CreateCommand();

				command.CommandText = string.Format("delete from Computers where ID={0}", ID);
				command.ExecuteNonQuery();

				command.CommandText = string.Format("delete from TestbedRelations where ComputerID={0}", ID);
				command.ExecuteNonQuery();

				command.Dispose();
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("GeneralUtils.DeleteComputerFromAllTables failed: " + ex);
			}
		}

		private void ResetIdentitySeed(string tableName, string identityColumnName = "ID")
		{
			var command = ConnectionManager.connection.CreateCommand();
			command.CommandText = string.Format("alter table {0} alter column {1} identity (0,1)",
				tableName, identityColumnName);
			try {
				command.ExecuteNonQuery();
				command.Dispose();
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("GeneralUtils.ResetIdentitySeed failed: " + ex);
			}
		}
	}
}