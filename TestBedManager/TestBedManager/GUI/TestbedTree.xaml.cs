using System.Collections.Generic;
using System.Windows.Controls;

namespace TestBedManager
{
	public partial class TestbedTree : UserControl
	{
		public TestbedTree()
		{
			InitializeComponent();
		}

		public void AddList(string title, Testbed contents)
		{
			TreeViewItem listItem = new TreeViewItem {
				Header = title,
				IsExpanded = true
			};
			treeview.Items.Add(listItem);
			foreach (var item in contents.items) {
				TreeViewItem computerItem = new TreeViewItem {
					Header = item.hostname
				};
				listItem.Items.Add(computerItem);
			}
		}
	}
}