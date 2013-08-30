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
				var textRange = new TextRange(RichTextBoxCommand.Document.ContentStart, 
					RichTextBoxCommand.Document.ContentEnd);

				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.CreateProcess(textRange.Text + " > C:\\Users\\Megan\\Desktop\\output.txt");
			}
			Close();
		}
	}
}
