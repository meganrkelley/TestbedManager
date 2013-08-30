using System.Windows;

namespace TestBedManager
{
	public partial class EventCodeWindow : Window
	{
		public EventCodeWindow()
		{
			InitializeComponent();
			TextBoxEventID.Focus();
			Show();
		}

		private void ButtonSearch_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(TextBoxEventID.Text.Trim()))
				return;

			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryEventViewer(TextBoxEventID.Text);
			}
			Close();
		}

		private void TextBoxEventID_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Enter)
				ButtonSearch_Click(sender, e);
		}
	}
}
