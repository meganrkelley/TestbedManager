using System.Windows;
using System.Windows.Documents;

namespace TestBedManager
{
	public partial class RunCmdWindow : Window
	{
		public RunCmdWindow()
		{
			InitializeComponent();
			RichTextBoxCommand.Focus();
			Show();
		}

		private void ButtonRun_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				var textRange = new TextRange(RichTextBoxCommand.Document.ContentStart, RichTextBoxCommand.Document.ContentEnd);
				remoteTaskManager.CreateProcess(textRange.Text);
			}
			Close();
		}
	}
}
