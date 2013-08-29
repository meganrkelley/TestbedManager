using System;
using System.Data;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using TestBedManager.Properties;

namespace TestBedManager
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
			WindowStartupLocation = WindowStartupLocation.CenterScreen;

			//if (Settings.Default.ShowStartDialog)
			//    new StartDialog();
		}

		~MainWindow()
		{
		}


		#region File

		private void MenuItemRestartApplication_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
			System.Windows.Forms.Application.Restart();
		}

		private void MenuItemExit_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		#endregion


		#region List

		private void MenuItem_SaveList_Click(object sender, RoutedEventArgs e)
		{
		}

		private void MenuItem_DeleteLists_Click(object sender, RoutedEventArgs e)
		{
			var result = MessageBox.Show("Are you sure you want to delete all your saved lists?",
				"Confirm Delete", 
				MessageBoxButton.YesNo, 
				MessageBoxImage.Warning,
				MessageBoxResult.Cancel);

		}

		private void MenuItem_LoadList_Click(object sender, RoutedEventArgs e)
		{
		}

		private void MenuItem_Add_Click(object sender, RoutedEventArgs e)
		{
		}

		private void MenuItem_ScanLAN_Click(object sender, RoutedEventArgs e)
		{
		}

		private void MenuItem_Clear_Click(object sender, RoutedEventArgs e)
		{
		}

		#endregion


		#region Actions

		private void MenuItemGetOsInfo_Click(object sender, RoutedEventArgs e)
		{
		}

		#endregion


		#region Help

		private void MenuItem_About_Click(object sender, RoutedEventArgs e)
		{
		}

		#endregion
	}
}