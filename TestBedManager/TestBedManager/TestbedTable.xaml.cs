using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using TestBedManager.Properties;

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
				foreach (RemoteComputer item in dataGrid.SelectedItems)
					selected.Add(item);
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
				foreach (RemoteComputer item in dataGrid.Items)
					items.Add(item);
				return items;
			}
		}
		#endregion

		/// <summary>
		/// On load, initialize the testbed table with the most recently used testbed.
		/// </summary>
		private void TestbedTableDataGrid_Loaded(object sender, RoutedEventArgs e)
		{
            //if (DesignerProperties.GetIsInDesignMode(this)) // Don't try to populate the list in design mode.
            //    return;

            //Testbed list;
            //if (Settings.Default.MostRecentList == -1) {
            //    list = Master.databaseManager.GetAllComputers();
            //} else {
            //    list = Master.databaseManager.GetListContents(Settings.Default.MostRecentList.ToString());
            //}
            //foreach (RemoteComputer item in list.items) {
            //    Master.activeTestbed.Add(item);
            //    Master.logManager.Add(item); // noo bad
            //}
		}

		/// <summary>
		/// When a Testbed changes its state, update the GUI.
		/// </summary>
		/// <param name="computer"></param>
		public void Update(Testbed testbed)
		{
			dataGrid.Dispatcher.Invoke((Action)(() => {
				//? This task can be made more efficient.
				foreach (var computer in items) {
					if (!testbed.items.Contains(computer)) {
						dataGrid.Items.Remove(computer);
						Master.activeTestbed.Remove(computer);
					}
				}
				foreach (var computer in testbed.items) {
					if (!dataGrid.Items.Contains(computer)) {
						dataGrid.Items.Add(computer);
						ConnectionMonitor connMon = new ConnectionMonitor(computer);
					}
				}
				dataGrid.Items.Refresh();
			}));
		}

		#region Removing computers
		private void MenuItemRemove_Click(object sender, RoutedEventArgs e)
		{
			foreach (var item in selectedItems) {
				Master.activeTestbed.Remove(item);
				Master.logManager.Remove(item);
			}
		}

		private void MenuItemRemoveAll_Click(object sender, RoutedEventArgs e)
		{
			Testbed selected = Testbed.ToTestbed(selectedItems);
			foreach (RemoteComputer computer in selected.items) {
				Master.activeTestbed.Remove(computer);
				Master.logManager.Remove(computer);
			}
		}

		private void MenuItemRemoveFromDb_Click(object sender, RoutedEventArgs e)
		{
			Testbed selected = Testbed.ToTestbed(selectedItems);
			foreach (RemoteComputer computer in selected.items) {
			//	Master.databaseManager.RemoveComputer(computer);

                new DBComputerHandler().Remove(computer.ID);
                new DBTestbedRelationsHandler().RemoveComputerFromAllTestbeds(computer.ID);

				Master.activeTestbed.Remove(computer);
				Master.logManager.Remove(computer);
			}
		}
		#endregion

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
		#endregion

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
