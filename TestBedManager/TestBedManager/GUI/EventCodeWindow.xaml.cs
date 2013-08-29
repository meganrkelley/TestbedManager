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
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryEventViewer(TextBoxEventID.Text);
			}
		}
	}
}
