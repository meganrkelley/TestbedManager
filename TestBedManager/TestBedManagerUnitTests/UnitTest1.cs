using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using TestBedManager;

namespace TestBedManagerUnitTests
{
	[TestClass]
	public class UnitTest1
	{
		//[TestMethod]
		//public void DatabaseStressTest()
		//{
		//	MainWindow m = new MainWindow();
		//	for (int i = 0; i < 100000; i++) {
		//		Random random = new Random();
		//		string r1 = random.Next(35, 35).ToString();
		//		string r2 = random.Next(1, 255).ToString();
		//		string r3 = random.Next(1, 255).ToString();
		//		string r4 = random.Next(1, 255).ToString();
		//		RemoteComputer item = new RemoteComputer("stuff", IPAddress.Parse(String.Format("{0}.{1}.{2}.{3}", r1, r2, r3, r4)), new NetworkCredential());
		//		RemoteComputerList.Add(item);
		//	}
		//	RemoteComputerList.Clear();
		//}

		/*
		 * Add x computers
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
		public void AddRemoveTest()
		{

		}

		[TestMethod]
		public void SaveLoadTest()
		{

		}

		[TestMethod]
		public void TryBadHostnameAndTestbedName()
		{

		}

		[TestMethod]
		public void TryConnectWmi()
		{

		}

		[TestMethod]
		public void TryTask_()
		{

		}

		[TestMethod]
		public void TryDeleteSDF()
		{

		}

		[TestMethod]
		public void VerifySettingsTest()
		{

		}

		[TestMethod]
		public void VerifyAccountInfoChangeTest()
		{

		}
	}
}