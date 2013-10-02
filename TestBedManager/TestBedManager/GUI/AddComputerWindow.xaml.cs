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
			string encryptedPassword = Encryption.Encrypt(PasswordBoxPassword.Password);

			ConnectionInfoChecker checker = new ConnectionInfoChecker();
			RemoteComputer computer = checker.GetValidRemoteComputer(
				TextBoxHostnameIp.Text,
				TextBoxUsername.Text,
				encryptedPassword);

			if (computer != null)
				ActiveTestbed.Add(computer);

			Close();
		}
	}
}