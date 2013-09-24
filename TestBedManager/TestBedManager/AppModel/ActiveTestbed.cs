using System.Collections.Generic;
using TestBedManagerDB;

namespace TestBedManager
{
	/// <summary>
	/// The static collection of RemoteComputers currently in the GUI table in the main window.
	/// It behaves differently from a normal Testbed - if you add items to it, they will be
	/// inserted into the GUI table.
	/// It does not have an ID or a title.
	/// </summary>
	public static class ActiveTestbed
	{
		private static Computers computersTable = new Computers();

		// 1. Check if it already exists
		// 2. Add to the GUI table
		// 3. Add to the GUI tabs
		// 4. Add to the database and set its ID.
		public static void Add(RemoteComputer computer)
		{
			if (Master.table.TableContains(computer.hostname))
				return;
			Master.table.dataGrid.Items.Add(computer);


			DebugLog.DebugLog.Log("Added new computer (" + computer.hostname + ", " + computer.ipAddressStr + ") to the GUI table.");

			Master.logManager.Add(computer);

			DebugLog.DebugLog.Log("Added new computer (" + computer.hostname + ", " + computer.ipAddressStr + ") to the tab control.");

			if (computersTable.Find(computer.hostname).Rows.Count > 0)
				return;

			int id = computersTable.Insert(
				computer.hostname, 
				computer.ipAddressStr, 
				computer.credentials.UserName,
				computer.credentials.Password);

			computer.ID = id;

			DebugLog.DebugLog.Log("Added new computer (" + computer.ID + ", " + computer.hostname + ", " + computer.ipAddressStr + ") to the database.");
		}

		// 1. Update the GUI table
		// 2. Update the database
		public static void Update(RemoteComputer computer)
		{
			int index = Master.table.dataGrid.Items.IndexOf(computer);
			if (index < 0) {
				System.Diagnostics.Trace.WriteLine("Hit ActiveTestBed.Update and could not find the given computer in the GUI table. Hostname: " + computer.hostname);
				return;
			}
			Master.table.dataGrid.Items.RemoveAt(index);
			Master.table.dataGrid.Items.Insert(index, computer);

			computersTable.Update(computer.ID,
				computer.hostname,
				computer.ipAddressStr,
				computer.credentials.UserName,
				computer.credentials.Password);
		}

		// 1. Remove from the GUI table
		// 2. Remove from the GUI tabs
		public static void Remove(RemoteComputer computer)
		{
			Master.table.dataGrid.Items.Remove(computer);
			Master.logManager.Remove(computer);
		}

		public static bool IsEmpty()
		{
			return Master.table.dataGrid.Items.Count == 0;
		}
	}
}