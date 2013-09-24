﻿using System;
using System.Data;
using System.Data.SqlServerCe;
using System.Diagnostics;
using TestBedManagerDB.Properties;

namespace TestBedManagerDB
{
	public class ConnectionManager
	{
		public static SqlCeConnection connection;

		public static SqlCeConnection Connect()
		{
			try {
				if (connection != null && connection.State == ConnectionState.Open)
					return connection;
				DebugLog.DebugLog.Log("Opening a connection to " + Settings.Default.dbConnectionString + ".");
				connection = new SqlCeConnection(Settings.Default.dbConnectionString);
				connection.Open();
			} catch (Exception ex) {
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