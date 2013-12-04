using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using TestBedManager.Properties;
using TestBedManagerDB;
// Delete logs after certain amount of time

namespace TestBedManager
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			SetUpStaticReferences();
			ConnectionManager.Connect();
			AppStats appStatus = new AppStats(StatusBarText);

			ApplyUserSettings();
		}

		private void SetUpStaticReferences()
		{
			Master.main = this;
			Master.table = TestbedTable;
			Master.logManager = new OutputLogManager();
		}

		#region Settings

		private void ApplyUserSettings()
		{
			MenuItemUseIpOverHostname.IsChecked = Settings.Default.UseIp;
		}

		private void MenuItemUseIpOverHostname_Click(object sender, RoutedEventArgs e)
		{
			Settings.Default.UseIp = ((MenuItem)sender).IsChecked;
			Settings.Default.Save();
		}

		private void MenuItemResetAllSettings_Click(object sender, RoutedEventArgs e)
		{
			Settings.Default.Reset();
			Settings.Default.Save();

			// Make immediate cosmetic changes
			ColorWindow.ChangeTextBoxBg(System.Windows.Media.Colors.White);
		}

		private void MenuItemClearDatabase_Click(object sender, RoutedEventArgs e)
		{
			new TestBedManagerDB.GeneralUtils().ClearAllTables();
			Master.table.ClearDataGrid();
			Master.logManager.tabs.Items.Clear();
		}

		#endregion Settings

		#region Remote commands

		private void MenuItemEjectDrive_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.EjectDrive();
			}
		}

		private void MenuItemRestart_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.CreateProcess("shutdown -r -t 0 -f");
			}
		}

		private void MenuItemShutdown_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.CreateProcess("shutdown -s -t 0 -f");
			}
		}

		private void MenuItemHibernate_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.CreateProcess("shutdown -h -t 0 -f");
			}
		}

		private void MenuItemSleep_Click(object sender, RoutedEventArgs e)
		{
			MenuItemHibernate_Click(sender, e); //TODO Sleep
		}

		private void MenuItemComputerProduct_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryComputerProduct();
			}
		}

		private void MenuItemComputerSystem_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryComputerSystem();
			}
		}

		private void MenuItemInstalledPrograms_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryInstalledPrograms();
			}
		}

		private void MenuItemLocalTime_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryLocalTime();
			}
		}

		private void MenuItemNetworkData_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryNetworkData();
			}
		}

		private void MenuItemPowerSettings_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				MenuItem selectedMenuItem = e.Source as MenuItem;
				remoteTaskManager.PowerPlan(selectedMenuItem.Header.ToString());
			}
		}

		private void MenuItemRunningPrograms_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryRunningProcesses();
			}
		}

		private void MenuItemScheduledTasks_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryScheduledJobs();
			}
		}

		private void MenuItemBios_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryBiosVersion();
			}
		}

		private void MenuItemBatteryInfo_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryBatteryInfo();
			}
		}

		private void MenuItemDriveInfo_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryDriveInfo();
			}
		}

		private void MenuItemRename_Click(object sender, RoutedEventArgs e)
		{
			new RenameComputerWindow();
		}

		private void MenuItemRun_Click(object sender, RoutedEventArgs e)
		{
			new RunCmdWindow();
		}

		private void MenuItemEventViewer_Click(object sender, RoutedEventArgs e)
		{
			new EventViewerWindow();
		}


		#endregion Remote commands

		#region Driver Classes

		private void MenuItemDrivers_amppal_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryDrivers("amppal");
			}
		}

		private void MenuItemDrivers_bluetooth_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryDrivers("bluetooth");
			}
		}

		private void MenuItemDrivers_display_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryDrivers("display");
			}
		}

		private void MenuItemDrivers_media_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryDrivers("media");
			}
		}

		private void MenuItemDrivers_net_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryDrivers("net");
			}
		}

		private void MenuItemDrivers_processor_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryDrivers("processor");
			}
		}

		private void MenuItemDrivers_system_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryDrivers("system");
			}
		}

		private void MenuItemDrivers_usb_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.QueryDrivers("usb");
			}
		}

		#endregion Driver Classes

		#region Local commands/windows

		private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
		{
			new AboutWindow();
		}

		private void MenuItemAddNewComputer_Click(object sender, RoutedEventArgs e)
		{
			new AddComputerWindow();
		}

		private void MenuItemExit_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void MenuItemRestartApp_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Forms.Application.Restart();
			Application.Current.Shutdown();
		}

		private void MenuItemSaveCurrentList_Click(object sender, RoutedEventArgs e)
		{
			new SaveListWindow();
		}

		private void MenuItemLoadSavedList_Click(object sender, RoutedEventArgs e)
		{
			Master.browser.Show();
		}

		private void MenuItemRemoteDesktop_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				LocalProcessExecutor executor = new LocalProcessExecutor();
				executor.StartRemoteDesktop(computer.hostname);
			}
		}

		private void MenuItemOpenLogs_Click(object sender, RoutedEventArgs e)
		{
			DebugLog.DebugLog.OpenLogsFolderInExplorer();
		}

		private void MenuItemClearLogs_Click(object sender, RoutedEventArgs e)
		{
			DebugLog.DebugLog.ClearLogs();
		}

		private void WindowMainWindow_Closed(object sender, System.EventArgs e)
		{
			ConnectionManager.Disconnect();
			App.Current.Shutdown();
		}

		private void MenuItemDocs_Click(object sender, RoutedEventArgs e)
		{
			LocalProcessExecutor executor = new LocalProcessExecutor();
			executor.OpenDocumentationFile();
		}

		#endregion Local commands/windows

		public void ChangeStatusBarText(string newText)
		{
			Dispatcher.Invoke((Action)(() => {
				StatusBarMain.Text = newText;
			}));
		}
		
	}
}