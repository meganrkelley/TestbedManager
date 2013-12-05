using System.Windows;

namespace TestBedManager
{
	public partial class RenameComputerWindow : Window
	{
		public RenameComputerWindow()
		{
			InitializeComponent();
			TextBoxNewHostname.Focus();
			Show();
		}

		private void ButtonRename_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.RenameComputer(TextBoxNewHostname.Text);
			}
			Close();
		}
	}
}