using System.Windows;

namespace TestBedManager
{
	public partial class RunCmdWindow : Window
	{
		public RunCmdWindow()
		{
			InitializeComponent();
			TextBoxCommand.Focus();
			Show();
		}

		private void ButtonRun_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.CreateProcess(TextBoxCommand.Text);
			}
		//	Close();
		}

		private void TextBoxCommand_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Enter)
				ButtonRun_Click(sender, e);
		}
	}
}