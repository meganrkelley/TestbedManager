using System;
using System.Data;
using System.Data.SqlServerCe;
using System.Diagnostics;
using TestBedManager.Properties;

namespace TestBedManager
{
	static class DBConnectionManager
	{
		public static SqlCeConnection connection;

		public static SqlCeConnection Connect()
		{
			try {
				if (connection != null && connection.State == ConnectionState.Open)
					return connection;
				DebugLog.Log("Opening a connection...");
				connection = new SqlCeConnection(Settings.Default.TestBedManagerDBConnectionString);
				connection.Open();
			} catch (Exception ex) {
				DebugLog.Log(ex);
			}

			return connection;
		}

		public static void Disconnect()
		{
			try {
				connection.Close();
			} catch (Exception ex) {
				DebugLog.Log(ex);
			}
		}
	}
}
