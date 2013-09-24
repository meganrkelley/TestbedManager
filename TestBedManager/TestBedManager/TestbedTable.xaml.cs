using System.Collections.Generic;
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
			if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)) // Don't try to populate the list in design mode.
			    return;

			Testbed testbed = new Testbed();
			if (Settings.Default.MostRecentList == -1) {
				DataTable table = new Computers().SelectAll();
				foreach (DataRow row in table.Rows) {
					RemoteComputer computer = new RemoteComputer(row);
					testbed.Add(computer);
				}
			} else {
				DataTable table = new TestbedRelations().FindByTestbedID(Settings.Default.MostRecentList);
				foreach (DataRow row in table.Rows) {
					RemoteComputer computer = new RemoteComputer(row);
					testbed.Add(computer);
				}
			}

			foreach (RemoteComputer item in testbed.items) {
				ActiveTestbed.Add(item);
			}
		}

		public bool TableContains(string hostname)
		{
			foreach (RemoteComputer item in dataGrid.Items)
				if (item.hostname.Equals(hostname, System.StringComparison.InvariantCultureIgnoreCase))
					return true;
			return false;
		}

		/// <summary>
		/// When a Testbed changes its state, update the GUI.
		/// </summary>
		/// <param name="computer"></param>
		public void Update(Testbed testbed)
		{
			//dataGrid.Dispatcher.Invoke((Action)(() => {
			//	//? This task can be made more efficient.
			//	foreach (var computer in items) {
			//		if (!testbed.items.Contains(computer)) {
			//		//	dataGrid.Items.Remove(computer);
			//			ActiveTestbed.Remove(computer);
			//		//	Master.activeTestbed.Remove(computer);
			//		}
			//	}
			//	foreach (var computer in testbed.items) {
			//		if (!dataGrid.Items.Contains(computer)) {
			//		//	dataGrid.Items.Add(computer);
			//			ActiveTestbed.Add(computer);
			//			ConnectionMonitor connMon = new ConnectionMonitor(computer);
			//		}
			//	}
			//	dataGrid.Items.Refresh();
			//}));
		}

		#region Removing computers

		private void MenuItemRemove_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in selectedItems) {
				ActiveTestbed.Remove(computer);
			}
		}

		private void MenuItemRemoveAll_Click(object sender, RoutedEventArgs e)
		{
			Testbed selected = Testbed.ToTestbed(selectedItems);
			foreach (RemoteComputer computer in selected.items) {
				ActiveTestbed.Remove(computer);
			}
		}

		private void MenuItemRemoveFromDb_Click(object sender, RoutedEventArgs e)
		{
			Testbed selected = Testbed.ToTestbed(selectedItems);
			foreach (RemoteComputer computer in selected.items) {

				ActiveTestbed.Remove(computer);

				new TestbedRelations().DeleteComputer(computer.ID);

				new Computers().Delete(computer.ID);
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
			foreach (var item in selectedItems) {
				AccountInfoView accountInfo = new AccountInfoView();
				accountInfo.InitializeInfo(item);
			}
		}
	}
}