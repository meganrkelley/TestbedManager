using System.Windows;
using System.Windows.Controls;
using TestBedManager.Properties;
using TestBedManagerDB;

// Delete logs after certain amount of time

// [x] System Events - give option to select level and search for source
// [x] Annoying ass tooltip on outputlog
// [x] Put more stuff under FILE tab
// [x] Run command - don't make it a scrolling textbox.
// [x] Rename + Reboot -- only rename option
// [x] Gray out tasks if a machine is not selected
// [x] Spacing in outputlog

namespace TestBedManager
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			SetUpStaticReferences();

			ConnectionManager.Connect();

			ApplyUserSettings();

			//if (Settings.Default.ShowStartupWarning)
			//	new StartupWindow();
		}

		private void ApplyUserSettings()
		{
			MenuItemShowStartupWarning.IsChecked = Settings.Default.ShowStartupWarning;
			MenuItemUseIpOverHostname.IsChecked = Settings.Default.UseIp;
		}

		private void SetUpStaticReferences()
		{
			Master.main = this;
			Master.table = TestbedTable;
			Master.logManager = new OutputLogManager();
		}

		#region Settings

		private void MenuItemShowStartupWarning_Click(object sender, RoutedEventArgs e)
		{
			Settings.Default.ShowStartupWarning = ((MenuItem)sender).IsChecked;
			Settings.Default.Save();
		}

		private void MenuItemUseIpOverHostname_Click(object sender, RoutedEventArgs e)
		{
			Settings.Default.UseIp = ((MenuItem)sender).IsChecked;
			Settings.Default.Save();
		}

		// Set Ping Interval

		#endregion Settings

		#region Remote commands

		private void MenuItemBeep_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
				remoteTaskManager.CreateProcess("echo \a"); // Trying to send a bell
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
			//TODO Sleep
			MenuItemHibernate_Click(sender, e);
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

		#endregion Local commands/windows

		private void WindowMainWindow_Closed(object sender, System.EventArgs e)
		{
			ConnectionManager.Disconnect();
			App.Current.Shutdown();
		}
	}
}