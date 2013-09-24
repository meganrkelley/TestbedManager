using System.Data;
using TestBedManager.Properties;
using TestBedManagerDB;

namespace TestBedManager
{
	public class TestbedEditor
	{
	//	private static DBTestbedRelationsHandler testbedRelationsHandler = new DBTestbedRelationsHandler();
	//	private static DBTestbedHandler testbedHandler = new DBTestbedHandler();

		private static TestbedRelations relationsTable = new TestbedRelations();
		private static Testbeds testbedsTable = new Testbeds();

		// 1. Input validation
		// 2. Generate a new testbed ID
		// 3. Add the items in the GUI to the relations table with this new ID
		public static void Save(string name = "")
		{
			if (ActiveTestbed.IsEmpty())
				return;

			if (name == "")
				name = Resources.DefaultTestbedName;

			int nextTestbedID = testbedsTable.GetNextID();
			foreach (RemoteComputer item in Master.table.dataGrid.Items) {
				relationsTable.Insert(item.ID, nextTestbedID);
			}
		//	testbedHandler.Add(name);

		//	int newTestbedID = testbedHandler.Find(name);
		//	Testbed newTestbed = new Testbed(newTestbedID, name);

		//	foreach (var item in ActiveTestbed.items)
		//		testbedRelationsHandler.AddComputerToTestbed(item, newTestbed);
		}

		// 1. Get a list of all the computers with this ID in the relations table
		// 2. Add each of these to the activetestbed
		// 3. Save the "most recent list" setting.
		public static void Load(int id)
		{
			DataTable table = relationsTable.FindByTestbedID(id);
			foreach (DataRow row in table.Rows) {
				RemoteComputer computer = new RemoteComputer(row);
				ActiveTestbed.Add(computer);
			}
		//	Testbed testbed = testbedRelationsHandler.GetTestbedByID(id);
		//	testbed.ID = id;

		//	foreach (RemoteComputer computer in testbed.items) {
		//		ActiveTestbed.Add(computer);
		//		Master.logManager.Add(computer);
		//	}

			Settings.Default.MostRecentList = id;
			Settings.Default.Save();
		}

		public static void Load(string title)
		{
		//	int testbedID = testbedHandler.Find(title);
		//	Load(testbedID);
		}

		// 1. Remove this testbed from the relations table and the testbed table
		public static void Delete(int id)
		{
			testbedsTable.Delete(id);
			relationsTable.DeleteTestbed(id);
		//	testbedHandler.Remove(id);
		//	testbedRelationsHandler.RemoveEntireTestbed(id);
		}

		public static void Delete(string title)
		{
		//	int testbedID = testbedHandler.Find(title);
		//	Delete(testbedID);
		}

	}
}