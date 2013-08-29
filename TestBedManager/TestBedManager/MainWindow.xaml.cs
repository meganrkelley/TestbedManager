using System.Windows;
using System.Windows.Controls;
using TestBedManager.Properties;

namespace TestBedManager
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			SetUpStaticReferences();
			ApplyUserSettings();
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
			Master.databaseManager = new DatabaseManager();
			DBConnectionManager.Connect();
			Master.activeTestbed = new Testbed{ ID = -1 };
		}

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
		#endregion

		#region Save/load testbeds
		private void MenuItemSaveCurrentList_Click(object sender, RoutedEventArgs e)
		{
			new SaveListWindow();
		}

		private void MenuItemLoadSavedList_Click(object sender, RoutedEventArgs e)
		{
			Master.browser.Show();
		}
		#endregion

		#region Remote commands

		private void MenuItemBeep_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				RemoteTaskManager remoteTaskManager = new RemoteTaskManager(computer);
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
			//rundll?
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
			new EventCodeWindow();
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

		#endregion

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

		#endregion

		#region Local commands
		private void MenuItemRemoteDesktop_Click(object sender, RoutedEventArgs e)
		{
			foreach (RemoteComputer computer in Master.table.selectedItems) {
				LocalProcessExecutor executor = new LocalProcessExecutor();
				executor.StartRemoteDesktop(computer.hostname);
			}
		}
		#endregion

        // copied from TestbedTable.xaml.cs
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Testbed list;
            if (Settings.Default.MostRecentList == -1)
            {
                list = Master.databaseManager.GetAllComputers();
            }
            else
            {
                list = Master.databaseManager.GetListContents(Settings.Default.MostRecentList.ToString());
            }
            foreach (RemoteComputer item in list.items)
            {
                Master.activeTestbed.Add(item);
                Master.logManager.Add(item);
            }
        }
	}
}