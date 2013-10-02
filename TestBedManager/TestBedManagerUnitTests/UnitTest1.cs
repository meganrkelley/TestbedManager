using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using TestBedManager;
using TestBedManagerDB;

namespace TestBedManagerUnitTests
{
	[TestClass]
	public class TestBedManagerUnitTests
	{
		/* Add x computers
		 * Save x computers to testbed A
		 * Save some or all of same computers to testbed B
		 * Load testbeds A and B
		 * Delete x computers
		 *
		 * Try add invalid hostname
		 * Try add invalid testbed name
		 * Try establish WMI connection on unresponding computer
		 *
		 * Try all tasks
		 * Try delete SDF file
		 *
		 * Verify Settings changes
		 * Verify change account info
		 */

		[TestMethod]
		public void EncryptDecrypt()
		{
			string password, encrypted, decrypted;

			password = "abcdefg";
			encrypted = Encryption.Encrypt(password);
			decrypted = Encryption.Decrypt(encrypted);
			Assert.AreEqual(password, decrypted);
			encrypted = Encryption.Encrypt(decrypted);
			decrypted = Encryption.Decrypt(encrypted);
			Assert.AreEqual(password, decrypted);

			password = "a b c d e f g";
			encrypted = Encryption.Encrypt(password);
			decrypted = Encryption.Decrypt(encrypted);
			Assert.AreEqual(password, decrypted);

			password = "~!@#$%^&*()-+";
			encrypted = Encryption.Encrypt(password);
			decrypted = Encryption.Decrypt(encrypted);
			Assert.AreEqual(password, decrypted);
		}

		[TestMethod]
		public void AddRemoveTest()
		{
			// init database connection
			ConnectionManager.Connect();
			Computers computersInterface = new Computers();

			// add localhost/127.0.0.1 to db
			computersInterface.Insert("dummy", "127.0.0.1", "", "");

			// check the db to see if it's there
			bool found = false;
			DataTable all = computersInterface.SelectAll();
			Assert.AreNotEqual(0, all.Rows.Count);
			foreach (DataRow row in all.Rows) {
				if (row["Hostname"].ToString().Equals("dummy"))
					found = true;
			}

			Assert.IsTrue(found);

			// delete it from the db
			computersInterface.Delete("dummy", "127.0.0.1");

			// check the db to see if it's there (it shouldn't be)
			found = false;
			all = computersInterface.SelectAll();
			foreach (DataRow row in all.Rows) {
				if (row["Hostname"].ToString().Equals("dummy"))
					found = true;
			}

			Assert.IsFalse(found);
		}

		[TestMethod]
		public void SaveLoadTest()
		{
			// init database connection
			ConnectionManager.Connect();
			Computers computersInterface = new Computers();

			// add localhost/127.0.0.1 to db
			int computerID = computersInterface.Insert("dummy", "127.0.0.1", "", "");

			// check the db to see if it's there
			bool found = false;
			DataTable all = computersInterface.SelectAll();
			Assert.AreNotEqual(0, all.Rows.Count);
			foreach (DataRow row in all.Rows) {
				if (row["Hostname"].ToString().Equals("dummy"))
					found = true;
			}

			Assert.IsTrue(found);

			// save it to a new testbed
			int testbedID = new Testbeds().Insert("SaveLoadTest01");
			new TestbedRelations().Insert(computerID, testbedID);

			bool foundInTestbed = false;
			DataTable testbedsResults = new TestbedRelations().FindByTestbedID(testbedID);
			foreach (DataRow row in testbedsResults.Rows) {
				if (int.Parse(row["ComputerID"].ToString()) == computerID &&
					int.Parse(row["TestbedID"].ToString()) == testbedID)
					foundInTestbed = true;
			}

			Assert.IsTrue(foundInTestbed);

			new TestbedRelations().DeleteTestbed(testbedID);
			new Testbeds().Delete(testbedID);
		}

		//[TestMethod]
		//public void TryBadHostnameAndTestbedName()
		//{
		//}

		//[TestMethod]
		//public void TryConnectWmi()
		//{
		//}

		//[TestMethod]
		//public void TryTask_()
		//{
		//}

		//[TestMethod]
		//public void VerifySettingsTest()
		//{
		//}

		//[TestMethod]
		//public void VerifyAccountInfoChangeTest()
		//{
		//}

		//[TestMethod]
		//public void DatabaseStressTest()
		//{
		//	MainWindow m = new MainWindow();
		//	m.Show();
		//	for (int i = 0; i < 100000; i++) {
		//		Random random = new Random();
		//		string r1 = random.Next(1, 255).ToString();
		//		string r2 = random.Next(1, 255).ToString();
		//		string r3 = random.Next(1, 255).ToString();
		//		string r4 = random.Next(1, 255).ToString();
		//		RemoteComputer item = new RemoteComputer("stuff", IPAddress.Parse(String.Format("{0}.{1}.{2}.{3}", r1, r2, r3, r4)), new NetworkCredential());
		//		ActiveTestbed.Add(item);
		//		ActiveTestbed.Remove(item);
		//	}
		//	m.Close();
		//}
	}
}