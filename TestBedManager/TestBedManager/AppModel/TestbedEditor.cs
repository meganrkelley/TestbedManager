using System.Data;
using TestBedManager.Properties;
using TestBedManagerDB;

namespace TestBedManager
{
	public class TestbedEditor
	{
		private static TestbedRelations relationsTable = new TestbedRelations();
		private static Testbeds testbedsTable = new Testbeds();
		private static Computers computersTable = new Computers();

		// 1. Input validation
		// 2. Generate a new testbed ID
		// 3. Insert the new testbed
		// 4. Add the items in the GUI to the relations table with this new ID
		public static void Save(string name = "")
		{
			if (ActiveTestbed.IsEmpty())
				return;

			if (name == "")
				name = Resources.DefaultTestbedName;

			int nextTestbedID = testbedsTable.GetNextID();

			testbedsTable.Insert(name);

			foreach (RemoteComputer item in Master.table.dataGrid.Items)
				relationsTable.Insert(item.ID, nextTestbedID);
		}

		// 1. Get a list of all the computers with this ID in the relations table
		// 2. Add each of these to the activetestbed
		// 3. Save the "most recent list" setting.
		public static void Load(int id)
		{
			DataTable table = relationsTable.FindByTestbedID(id);
			foreach (DataRow row in table.Rows) {
				DataTable table_computer = computersTable.Find((int)row["ComputerID"]);
				foreach (DataRow row_computer in table_computer.Rows) {
					ActiveTestbed.Add(new RemoteComputer(row_computer));
				}
			}

			Settings.Default.MostRecentList = id;
			Settings.Default.Save();
		}

		// Just calls the method for id
		public static void Load(string name)
		{
			DataTable table = testbedsTable.Find(name);
			foreach (DataRow row in table.Rows) {
				Load((int)row["ID"]);
				return;
			}
			DebugLog.DebugLog.Log("Could not find a matching testbed ID in table Testbeds for name " + name);
		}

		// 1. Remove this testbed from the relations table 
		//		and the testbed table
		public static void Delete(int id)
		{
			testbedsTable.Delete(id);
			relationsTable.DeleteTestbed(id);
		}

		// Just calls the method for id
		public static void Delete(string name)
		{
			DataTable table = testbedsTable.Find(name);
			foreach (DataRow row in table.Rows) {
				Delete((int)row["ID"]);
				return;
			}
			DebugLog.DebugLog.Log("Could not find a matching testbed ID in table Testbeds for name " + name);
		}
	}
}