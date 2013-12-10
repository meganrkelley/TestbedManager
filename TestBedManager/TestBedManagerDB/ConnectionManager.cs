using System;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Windows.Forms;
using TestBedManagerDB.Properties;

namespace TestBedManagerDB
{
	public class ConnectionManager
	{
		public static SqlCeConnection connection;

		public static SqlCeConnection Connect()
		{
			AppDomain.CurrentDomain.SetData("DataDirectory",
				Path.Combine(Directory.GetCurrentDirectory(), "Database"));

			try {
				if (connection != null && connection.State == ConnectionState.Open)
					return connection;
				DebugLog.DebugLog.Log("Opening a connection to " + Settings.Default.dbConnectionString + ".");
				connection = new SqlCeConnection(Settings.Default.dbConnectionString);
				connection.Open();
			} catch (Exception ex) {
				MessageBox.Show("Couldn't connect to the database: " + ex.Message);
				DebugLog.DebugLog.Log(ex);
			}

			return connection;
		}

		public static void Disconnect()
		{
			try {
				DebugLog.DebugLog.Log("Closing the connection to " + Settings.Default.dbConnectionString + ".");
				connection.Close();
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(ex);
			}
		}
	}
}