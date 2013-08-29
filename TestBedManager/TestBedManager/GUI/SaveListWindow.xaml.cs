using System.Windows;

namespace TestBedManager
{
	public partial class SaveListWindow : Window
	{
		public SaveListWindow()
		{
			InitializeComponent();

			TextBoxListName.Focus();
			TextBoxListName.SelectAll();
			Show();
		}

		private void ButtonSave_Click(object sender, RoutedEventArgs e)
		{
			string listName = TextBoxListName.Text.Trim();

			if (string.IsNullOrEmpty(listName))
				listName = Properties.Resources.DefaultTestbedName;

			TestbedEditor.Save(listName);

			Close();
		}
	}
}