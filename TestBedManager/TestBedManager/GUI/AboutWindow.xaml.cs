using System.Windows;

namespace TestBedManager
{
	/// <summary>
	/// Interaction logic for AboutWindow.xaml
	/// </summary>
	public partial class AboutWindow : Window
	{
		public AboutWindow()
		{
			InitializeComponent();
			Show();
			TextBlockAbout.Width = this.Width;
			TextBlockAbout.Height = this.Height;
		}
	}
}