using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TestBedManager.Properties;


namespace TestBedManager
{
	/// <summary>
	/// Responsible for adding, writing to, and removing output tabs for each computer.
	/// Also handles output options such as font, saving to a file, copying to clipboard, etc.
	/// </summary>
	public class OutputLogManager
	{
		#region Private fields

		private System.Windows.Forms.FontDialog fontDialog;
		private SaveFileDialog saveOutputDialog;
		private TabControl tabControl = Master.main.mainTabControl;

		private TabItem selectedTab
		{
			get
			{
				TabItem tab = new TabItem();
				tabControl.Dispatcher.Invoke((Action)(() => {
					tab = (TabItem)tabControl.SelectedItem;
				}));
				return tab;
			}
			set { tabControl.SelectedItem = value; }
		}

		#endregion Private fields

		public TabControl tabs
		{
			get { return tabControl; }
			set { tabControl = value; }
		}

		public void Add(RemoteComputer computer)
		{
			if (TabControlContains(computer.hostname))
				return;
			TabItem newTab = CreateTabItemForComputer(computer);
			tabControl.Items.Add(newTab);
			selectedTab = newTab; // Bring the new tab into focus.
		}

		public void Remove(RemoteComputer computer)
		{
			try {
				tabControl.Items.Remove(GetTabItem(computer));
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(ex);
			}
		}

		public void WriteToComputerTab(RemoteComputer computer, string text, bool printTimestamp = true)
		{
			try {
				tabControl.Dispatcher.Invoke((Action)(() => {
					TabItem thisTab = GetTabItem(computer);
					tabControl.SelectedItem = thisTab;
					RichTextBox textBox = ((RichTextBox)(((TabItem)thisTab).Content));
					if (textBox != null) {
						if (printTimestamp)
							textBox.AppendText("[" + DateTime.Now + "] " + text + Environment.NewLine);
						else
							textBox.AppendText(text + Environment.NewLine);
						textBox.ScrollToEnd();
					}
				}));
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(ex);
			}
		}

		public bool TabControlContains(string hostname)
		{
			foreach (TabItem item in tabControl.Items) {
				if (item.Header.ToString().Equals(hostname, StringComparison.InvariantCultureIgnoreCase))
					return true;
			}
			return false;
		}

		private TabItem GetTabItem(RemoteComputer computer)
		{
			foreach (TabItem item in tabControl.Items) {
				if (item.Header.ToString().Equals(computer.hostname, StringComparison.InvariantCultureIgnoreCase) ||
					item.Header.ToString().Equals(computer.ipAddressStr, StringComparison.InvariantCultureIgnoreCase))
					return item;
			}
			return null;
		}

		private void close_Click(object sender, RoutedEventArgs e)
		{
			tabControl.Items.Remove(selectedTab);
		}

		private void closeAll_Click(object sender, RoutedEventArgs e)
		{
			tabControl.Items.Clear();
		}

		#region Create custom GUI elements

		private TabItem CreateTabItemForComputer(RemoteComputer computer)
		{
			return new TabItem {
				Header = computer.hostname,
				Content = CreateTextBox(),
				ContextMenu = CreateTabItemMenu()
			};
		}

		private RichTextBox CreateTextBox()
		{
			RichTextBox rtb = new RichTextBox {
				IsReadOnly = true,
				VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
				ContextMenu = CreateTextboxMenu(),
				FontFamily = new FontFamily("Segoe UI"),
				Background = new SolidColorBrush(Settings.Default.BackgroundColor)
			};
			rtb.SetValue(Paragraph.LineHeightProperty, 1.0);
			return rtb;
		}

		private MenuItem CreateMenuItem(string header, string iconPath)
		{
			return new MenuItem {
				Header = header,
				Icon = new Image {
					Source = new BitmapImage(new Uri(iconPath, UriKind.Relative))
				}
			};
		}

		private ContextMenu CreateTabItemMenu()
		{
			MenuItem
				close = CreateMenuItem("Close Active Tab", "Icons/remove_small.png"),
				closeAll = CreateMenuItem("Close All Tabs", "Icons/remove.png");

			close.Click += close_Click;
			closeAll.Click += closeAll_Click;

			ContextMenu menu = new ContextMenu();
			menu.Items.Add(close);
			menu.Items.Add(new Separator());
			menu.Items.Add(closeAll);

			return menu;
		}

		private ContextMenu CreateTextboxMenu()
		{
			MenuItem
				copy = CreateMenuItem("Copy All Text to Clipboard", "Icons/clipboard.png"),
				save = CreateMenuItem("Save Output to File", "Icons/disk.png"),
				clear = CreateMenuItem("Clear Output", "Icons/clean.png"),
				font = CreateMenuItem("Change Font", "Icons/font1.png"),
				paint = CreateMenuItem("Change Background Color", "Icons/background.png");

			copy.Click += copy_Click;
			save.Click += save_Click;
			clear.Click += clear_Click;
			font.Click += font_Click;
			paint.Click += paint_Click;

			ContextMenu menu = new ContextMenu();
			menu.Items.Add(copy);
			menu.Items.Add(save);
			menu.Items.Add(clear);
			menu.Items.Add(new Separator());
			menu.Items.Add(font);
			menu.Items.Add(paint);

			return menu;
		}

		#endregion Create custom GUI elements

		#region Textbox formatting options

		private void clear_Click(object sender, RoutedEventArgs e)
		{
			((RichTextBox)selectedTab.Content).Document.Blocks.Clear();
		}

		private void copy_Click(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText(new TextRange(((RichTextBox)selectedTab.Content).Document.ContentStart,
				((RichTextBox)selectedTab.Content).Document.ContentEnd).Text);
		}

		private void font_Click(object sender, RoutedEventArgs e)
		{
			fontDialog = new System.Windows.Forms.FontDialog {
				ShowApply = true,
				ShowColor = true
			};
			fontDialog.Apply += fontDialog_Apply;
			if (fontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				fontDialog_Apply(sender, e);
			}
		}

		private void fontDialog_Apply(object sender, EventArgs e)
		{
			foreach (TabItem tab in tabControl.Items) {
				var range = new TextRange(((RichTextBox)tab.Content).Document.ContentStart,
					((RichTextBox)tab.Content).Document.ContentEnd);
				range.ApplyPropertyValue(TextElement.FontSizeProperty, (double)fontDialog.Font.Size * 1.5);
				range.ApplyPropertyValue(TextElement.FontFamilyProperty, new FontFamily(fontDialog.Font.FontFamily.Name));
				((RichTextBox)tab.Content).ScrollToEnd();
			}
		}

		private void paint_Click(object sender, RoutedEventArgs e)
		{
			new ColorWindow();
		}

		private void save_Click(object sender, RoutedEventArgs e)
		{
			saveOutputDialog = new SaveFileDialog {
				DefaultExt = ".txt",
				Filter = "Plain Text Files|*.txt|All Files|*.*",
				Title = "Save Output Log As..."
			};
			saveOutputDialog.FileOk += saveOutputDialog_FileOk;
			saveOutputDialog.ShowDialog();
		}

		private void saveOutputDialog_FileOk(object sender, CancelEventArgs e)
		{
			using (FileStream fs = File.OpenWrite(saveOutputDialog.FileName)) {
				TextRange text = new TextRange(((RichTextBox)selectedTab.Content).Document.ContentStart,
					((RichTextBox)selectedTab.Content).Document.ContentEnd);
				text.Save(fs, DataFormats.Text);
			}
		}

		#endregion Textbox formatting options
	}
}