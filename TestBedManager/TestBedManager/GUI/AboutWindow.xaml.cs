using System.Windows;

namespace TestBedManager
{
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