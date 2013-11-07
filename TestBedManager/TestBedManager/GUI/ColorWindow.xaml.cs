using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TestBedManager.Properties;
using Xceed.Wpf.Toolkit;

namespace TestBedManager
{
	public partial class ColorWindow : Window
	{
		public ColorWindow()
		{
			InitializeComponent();

			ColorPicker colorPicker = new ColorPicker();
			colorPicker.IsOpen = true;
			colorPicker.SelectedColor = Colors.White;
			grid.Children.Add(colorPicker);
			colorPicker.SelectedColorChanged += colorPicker_SelectedColorChanged;

			Show();
		}

		private void colorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
		{
			Color newColor = e.NewValue;
			newColor = ChangeTextBoxBg(newColor);
			Settings.Default.BackgroundColor = e.NewValue;
			Settings.Default.Save();
		}

		public static Color ChangeTextBoxBg(Color newColor)
		{
			foreach (TabItem tab in Master.logManager.tabs.Items) {
				System.Windows.Controls.RichTextBox textBox = (System.Windows.Controls.RichTextBox)tab.Content;
				textBox.Background = new SolidColorBrush(newColor);
			}
			return newColor;
		}
	}
}