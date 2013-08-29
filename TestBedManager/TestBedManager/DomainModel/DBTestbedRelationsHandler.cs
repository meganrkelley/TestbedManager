using System.Data;
namespace TestBedManager
{
    class DBTestbedRelationsHandler
    {
        DBTestbedRelationsGateway testbedRelationsGateway = new DBTestbedRelationsGateway();

        public void AddComputerToTestbed(RemoteComputer computer, Testbed testbed) 
        {
            testbedRelationsGateway.Insert(computer.ID, testbed.ID);
        }

        public void RemoveComputerFromTestbed(RemoteComputer computer, Testbed testbed)
        {
            testbedRelationsGateway.DeleteComputerFromTestbed(computer.ID, testbed.ID);
        }

        public void RemoveEntireTestbed(int testbedID)
        {
            testbedRelationsGateway.DeleteTestbed(testbedID);
        }

        public void RemoveComputerFromAllTestbeds(int computerID)
        {
            testbedRelationsGateway.DeleteComputer(computerID);
        }

        public Testbed GetTestbedByID(int id)
        {
            Testbed testbed = new Testbed();

            DataTable findResults = testbedRelationsGateway.Find(id);
            foreach (DataRow row in findResults.Rows) {
                testbed.Add(new RemoteComputer {
					ID = (int)row["ID"],
					hostname = row["hostname"].ToString(),
					ipAddress = System.Net.IPAddress.Parse(row["ipAddress"].ToString()),
					credentials = new System.Net.NetworkCredential(row["username"].ToString(), 
                        row["password"].ToString())
				});
            }

            return testbed;
        }
    }
}
