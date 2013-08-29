using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TestBedManager
{
	public partial class Browser : Window
	{
		public Browser()
		{
			InitializeComponent();
		}

		private void WindowListBrowserWindow_Loaded(object sender, RoutedEventArgs e)
		{
			List<Testbed> lists = Master.databaseManager.GetAllSavedLists();
			foreach (Testbed testbed in lists) {

				Testbed listContents = Master.databaseManager.GetListContents(testbed.ID.ToString());
				if (listContents.items.Count == 1 && listContents.items[0] == null)
					continue;
				listTree.AddList(testbed.title, listContents); //? dispatcher invoke?
			}

            Testbed master = Master.databaseManager.GetAllComputers();
            foreach (RemoteComputer computer in master.items) {
                masterList.Items.Add(computer.hostname); //? dispatcher invoke?
            }
		}

		private void ButtonLoad_Click(object sender, RoutedEventArgs e)
		{
			TreeViewItem item = (TreeViewItem)listTree.treeview.SelectedItem;

			if (item == null || !item.HasItems)
				return;

            TestbedEditor.Load((string)item.Header);
		//	TestbedEditor.Load(Master.databaseManager.GetListID(item.Header.ToString()));

			Close();
		}

		private void ButtonDeleteTestbed_Click(object sender, RoutedEventArgs e)
		{
			TreeViewItem item = (TreeViewItem)listTree.treeview.SelectedItem;

			if (item == null)
				return;

			listTree.treeview.Items.Remove(listTree.treeview.SelectedItem);
		//	TestbedEditor.Delete(Master.databaseManager.GetListID(item.Header.ToString()));
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

        #endregion
    }
}