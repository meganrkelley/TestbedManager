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
		// 4. Add to the database and set its ID
		// 5. Start monitoring the network connection
		public static void Add(RemoteComputer computer)
		{
			if (Master.table.TableContainsHostname(computer.hostname) ||
				Master.table.TableContainsIp(computer.ipAddress)) {
					Master.main.ChangeStatusBarText("A computer with that name or IP address already exists.");
					return;
			}

			Master.table.dataGrid.Items.Add(computer);

			Master.logManager.Add(computer);

			if (computersTable.Find(computer.hostname).Rows.Count == 0) {
				computer.ID = computersTable.Insert(
					computer.hostname,
					computer.ipAddressStr,
					computer.credentials.UserName,
					computer.credentials.Password);

				DebugLog.DebugLog.Log("Added new computer (" + computer.ID + ", " + 
					computer.hostname + ", " + computer.ipAddressStr + ") to the database.");
			}

			new ConnectionMonitor(computer);

			Master.main.ChangeStatusBarText(computer.ipAddressStr + " added.");
		}

		// 1. Update the GUI table
		// 2. Update the database
		public static void Update(RemoteComputer computer)
		{
			List<RemoteComputer> items = new List<RemoteComputer>();
			foreach (RemoteComputer item in Master.table.dataGrid.Items)
				items.Add(item); // Copy so we don't modify the collection

			foreach (RemoteComputer item in items) {
				if (item.hostname == computer.hostname &&
					item.ipAddressStr == computer.ipAddressStr) {
					Master.table.dataGrid.Items.Remove(item);
					Master.table.dataGrid.Items.Add(computer);
				}
			}

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
			return Master.table.dataGrid.Items.IsEmpty;
		}
	}
}