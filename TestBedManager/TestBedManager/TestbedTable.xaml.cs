using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using TestBedManager.Properties;
using TestBedManagerDB;

namespace TestBedManager
{
	public partial class TestbedTable : UserControl, ITestbedObserver
	{
		public TestbedTable()
		{
			InitializeComponent();
			Master.table = this;
		}

		#region Properties and accessors

		/// <summary>
		/// Return a copy of the currently selected items in the data grid.
		/// </summary>
		public List<RemoteComputer> selectedItems
		{
			get
			{
				if (dataGrid.SelectedItems.Count == 0)
					Master.main.MenuItemTasks.IsEnabled = false;
				else
					Master.main.MenuItemTasks.IsEnabled = true;

				List<RemoteComputer> selected = new List<RemoteComputer>();
				foreach (RemoteComputer computer in dataGrid.SelectedItems)
					selected.Add(computer);

				return selected;
			}
		}

		/// <summary>
		/// Return a copy of all the items in the data grid.
		/// </summary>
		public List<RemoteComputer> items
		{
			get
			{
				List<RemoteComputer> items = new List<RemoteComputer>();
				foreach (RemoteComputer computer in dataGrid.Items)
					items.Add(computer);
				return items;
			}
		}

		#endregion Properties and accessors

		/// <summary>
		/// On load, initialize the testbed table with the most recently used testbed.
		/// </summary>
		private void TestbedTableDataGrid_Loaded(object sender, RoutedEventArgs e)
		{
			// Don't try to populate the list in design mode.
			if (DesignerProperties.GetIsInDesignMode(this))
				return;

			if (Settings.Default.MostRecentList < 0 ||
				new Testbeds().NumberOfTestbeds() == 0)
				AddAllComputersTo();
			else
				AddMostRecentListTo();
		}

		private static void AddMostRecentListTo()
		{
			// Get all the computers in the most recent list.
			DataTable table = new TestbedRelations().FindByTestbedID(
				Settings.Default.MostRecentList);

			// Get the information for each computer in the most recent list.
			foreach (DataRow row in table.Rows) {
				DataTable table_computer = new Computers().Find(
					(int)row["ComputerID"]);

				// Add each computer's information to the testbed object.
				foreach (DataRow row_computer in table_computer.Rows)
					ActiveTestbed.Add(new RemoteComputer(row_computer));
			}

			Master.main.ChangeStatusBarText("Loaded most recent testbed.");
		}

		private static void AddAllComputersTo()
		{
			DataTable table = new Computers().SelectAll();
			foreach (DataRow row in table.Rows)
				ActiveTestbed.Add(new RemoteComputer(row));
		}

		public bool TableContainsHostname(string hostname)
		{
			foreach (RemoteComputer item in dataGrid.Items)
				if (item.hostname.Equals(
					hostname, StringComparison.InvariantCultureIgnoreCase))
					return true;
			return false;
		}

		public bool TableContainsIp(System.Net.IPAddress ip)
		{
			foreach (RemoteComputer item in dataGrid.Items)
				if (item.ipAddress.Equals(ip))
					return true;
			return false;
		}

		public void RefreshItems()
		{
			dataGrid.Dispatcher.Invoke((Action)(() => {
				dataGrid.Items.Refresh();
			}));
		}

		/// <summary>
		/// When a Testbed changes its state, update the GUI.
		/// </summary>
		/// <param name="computer"></param>
		public void Update(Testbed testbed)
		{
		}

		#region Removing computers

		private void MenuItemRemove_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in selectedItems)
				ActiveTestbed.Remove(computer);
		}

		public void ClearDataGrid()
		{
			dataGrid.Items.Clear();
		}

		public void MenuItemRemoveFromDb_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in selectedItems) {
				ActiveTestbed.Remove(computer);
				new GeneralUtils().DeleteComputerFromAllTables(computer.ID);
			}
		}

		#endregion Removing computers

		#region Menu items that just open windows

		private void MenuItemAdd_Click(object sender, RoutedEventArgs e)
		{
			new AddComputerWindow();
		}

		private void MenuItemSave_Click(object sender, RoutedEventArgs e)
		{
			new SaveListWindow();
		}

		private void MenuItemLoad_Click(object sender, RoutedEventArgs e)
		{
			Master.browser.Show();
		}

		#endregion Menu items that just open windows

		private void MenuItemSelectAll_Click(object sender, RoutedEventArgs e)
		{
			dataGrid.SelectAll();
		}

		private void MenuItemAccountInfo_Click(object sender, RoutedEventArgs e)
		{
			foreach (var item in selectedItems)
				new AccountInfoView().InitializeInfo(item);
		}

		// Enable the TASKS menu when an item is selected.
		private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			int count = dataGrid.SelectedItems.Count;

			if (count > 0)
				Master.main.MenuItemTasks.IsEnabled = true;

			if (count == 1) {
				Master.main.ChangeStatusBarText("1 item selected.");
				return;
			}

			Master.main.ChangeStatusBarText(count + " items selected.");
		}
	}
}