using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TestBedManagerDB;

namespace TestBedManager
{
	public partial class Browser : Window
	{
		public Browser()
		{
			InitializeComponent();
		}

		// 1. Get list of testbed titles
		// 2. Get list of computers for each testbed
		// 3. Add each list and its contents to the tree
		// 4. Add all of the computers in the DB to the master tree
		private void WindowListBrowserWindow_Loaded(object sender, RoutedEventArgs e)
		{
			DataTable allTestbeds = new Testbeds().SelectAll();
			foreach (DataRow row_testbed in allTestbeds.Rows) {
				// Loop through each testbed
				Testbed testbed = new Testbed((int)row_testbed["ID"], (string)row_testbed["Title"]);

				// Find all of the relations for this testbed ID
				DataTable testbedRelations = new TestbedRelations().FindByTestbedID((int)row_testbed["ID"]);

				foreach (DataRow row_relation in testbedRelations.Rows) {
					// Get the computer information for this ID
					DataTable table_computer = new Computers().Find((int)row_relation["ComputerID"]);

					foreach (DataRow row_computer in table_computer.Rows)
						testbed.Add(new RemoteComputer(row_computer));
				}

				listTree.AddList((string)row_testbed["Title"], testbed);
			}

			DisplayAllInMasterList();
		}

		// Get all the hostnames in the DB and display them on the right pane.
		private void DisplayAllInMasterList()
		{
			DataTable all = new Computers().SelectAll();
			foreach (DataRow row in all.Rows) {
				masterList.Items.Add(row["Hostname"]);
			}
		}

		private void ButtonLoad_Click(object sender, RoutedEventArgs e)
		{
			TreeViewItem item = (TreeViewItem)listTree.treeview.SelectedItem;
			if (item == null || !item.HasItems)
				return;

			TestbedEditor.Load((string)item.Header);

			Close();
		}

		private void ButtonDeleteTestbed_Click(object sender, RoutedEventArgs e)
		{
			TreeViewItem item = (TreeViewItem)listTree.treeview.SelectedItem;

			if (item == null)
				return;

			listTree.treeview.Items.Remove(listTree.treeview.SelectedItem);
			TestbedEditor.Delete((string)item.Header);
		}

		#region Expand/Collapse/Enter key

		private void expandImage_MouseDown(object sender, MouseButtonEventArgs e)
		{
			ExpandAll();
		}

		private void collapseImage_MouseDown(object sender, MouseButtonEventArgs e)
		{
			CollapseAll();
		}

		public void ExpandAll()
		{
			foreach (TreeViewItem node in listTree.treeview.Items)
				node.IsExpanded = true;
		}

		public void CollapseAll()
		{
			foreach (TreeViewItem node in listTree.treeview.Items)
				node.IsExpanded = false;
		}

		private void WindowListBrowserWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				ButtonLoad_Click(sender, e);
		}

		#endregion Expand/Collapse/Enter key

		private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
		{
		}

		private void MenuItemMoveTo_Click(object sender, RoutedEventArgs e)
		{
		}
	}
}