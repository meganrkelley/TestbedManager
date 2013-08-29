using System.Windows;

namespace TestBedManager
{
	public partial class AddComputerWindow : Window
	{
		public AddComputerWindow()
		{
			InitializeComponent();

			Show();
			TextBoxHostnameIp.Focus();
		}

		private void ButtonAdd_Click(object sender, RoutedEventArgs e)
		{
			ConnectionInfoChecker checker = new ConnectionInfoChecker();
			RemoteComputer computer = checker.GetValidRemoteComputer(
				TextBoxHostnameIp.Text,
				TextBoxUsername.Text,
				PasswordBoxPassword.Password
			);

			if (computer != null) {
				Master.activeTestbed.Add(computer);
				Master.logManager.Add(computer); // also called when loading a list... need to fix logmanager update()
			}

			Close();
		}
	}
}