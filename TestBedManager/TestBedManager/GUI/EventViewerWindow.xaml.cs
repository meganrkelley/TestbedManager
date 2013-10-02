using System.Windows;

namespace TestBedManager
{
	public partial class EventViewerWindow : Window
	{
		public EventViewerWindow()
		{
			InitializeComponent();
			TextBoxEventID.Focus();
			Show();
		}

		private void ButtonSearch_Click(object sender, RoutedEventArgs e)
		{
			string ID = TextBoxEventID.Text.Trim();
			string source = TextBoxSource.Text.Trim();
			string level = ComboBoxLevel.Text;

			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryEventViewer(ID, source, level);
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