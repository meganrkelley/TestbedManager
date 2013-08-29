using TestBedManager.Properties;

namespace TestBedManager
{
	public class TestbedEditor
	{
        static DBTestbedRelationsHandler testbedRelationsHandler = new DBTestbedRelationsHandler();
        static DBTestbedHandler testbedHandler = new DBTestbedHandler();

		public static void Save(string name = "")
		{
			if (Master.activeTestbed.items.Count == 0)
				return;
			name = (name == "" ? Resources.DefaultTestbedName : name);

            testbedHandler.Add(name);

            int newTestbedID = testbedHandler.Find(name);
            Testbed newTestbed = new Testbed { ID = newTestbedID, title = name };

            foreach (var item in Master.activeTestbed.items)
                testbedRelationsHandler.AddComputerToTestbed(item, newTestbed);
		}



        // fix this duplication----------
        public static void Load(string title)
        {
            int testbedID = testbedHandler.Find(title);
            Testbed testbed = testbedRelationsHandler.GetTestbedByID(testbedID);
            foreach (RemoteComputer computer in testbed.items) {
                Master.activeTestbed.Add(computer);
                Master.logManager.Add(computer);
            }

            Settings.Default.MostRecentList = testbedID;
            Settings.Default.Save();
        }

		public static void Load(int id)
		{
            Testbed testbed = testbedRelationsHandler.GetTestbedByID(id);
            foreach (RemoteComputer computer in testbed.items) {
				Master.activeTestbed.Add(computer);
				Master.logManager.Add(computer);
			}

			Settings.Default.MostRecentList = id;
			Settings.Default.Save();
		}


		
		public static void Delete(int id)
		{
            testbedHandler.Remove(id);
            testbedRelationsHandler.RemoveEntireTestbed(id);
		}

        public static void Delete(string title)
        {
            int testbedID = testbedHandler.Find(title);
            testbedHandler.Remove(testbedID);
            testbedRelationsHandler.RemoveEntireTestbed(testbedID);
        }


        // --------------------------------
	}
}