using System.Threading.Tasks;

namespace TestBedManager
{
	public class RemoteTaskManager
	{
		private RemoteComputer remoteComputer;

		public RemoteTaskManager(RemoteComputer computer)
		{
			remoteComputer = computer;
		}

		private void RunRemoteTask(RemoteTask task)
		{
			if (task.mgmtClass == null)
				return;
			Task.Factory.StartNew(() => task.Run());
		}

		private void RunRemoteTask(RemoteTask task, string parameter = "")
		{
			if (task.mgmtClass == null)
				return;
			Task.Factory.StartNew(() => task.Run(parameter));
		}

		private void RunRemoteTask(RemoteTask task, string[] parameters)
		{
			if (task.mgmtClass == null)
				return;
			Task.Factory.StartNew(() => task.Run(parameters));
		}

		public void CreateProcess(string command)
		{
			RemoteTask task = new CreateProcessTask(remoteComputer);
			RunRemoteTask(task, command);
		}

		public void QueryDrivers(string deviceClass)
		{
			RemoteTask task = new DriverQueryTask(remoteComputer);
			RunRemoteTask(task, deviceClass);
		}

		public void QueryComputerProduct()
		{
			RemoteTask task = new ComputerSystemProductQueryTask(remoteComputer);
			RunRemoteTask(task);
		}

		public void QueryComputerSystem()
		{
			RemoteTask task = new ComputerSystemQueryTask(remoteComputer);
			RunRemoteTask(task);
		}

		public void QueryLocalTime()
		{
			RemoteTask task = new LocalTimeQueryTask(remoteComputer);
			RunRemoteTask(task);
		}

		public void QueryScheduledJobs()
		{
			RemoteTask task = new ScheduledJobsQueryTask(remoteComputer);
			RunRemoteTask(task);
		}

		public void QueryRunningProcesses()
		{
			RemoteTask task = new RunningProcessesQueryTask(remoteComputer);
			RunRemoteTask(task);
		}

		public void QueryInstalledPrograms()
		{
			RemoteTask task = new ProgramsQueryTask(remoteComputer);
			RunRemoteTask(task);
		}

		public void QueryEventViewer(string eventID, string source, string level)
		{
			RemoteTask task = new EventViewerQueryTask(remoteComputer);
			RunRemoteTask(task, new string[] { eventID, source, level });
		}

		public void QueryNetworkData()
		{
			RemoteTask task = new NetworkQueryTask(remoteComputer);
			RunRemoteTask(task);
		}

		public void RenameComputer(string newHostname)
		{
			RemoteTask task = new RenameComputerTask(remoteComputer);
			RunRemoteTask(task, newHostname);
		}

		public void PowerPlan(string planName)
		{
			RemoteTask task = new PowerPlanTask(remoteComputer);
			RunRemoteTask(task, planName);
		}

		public void EjectDrive()
		{
			RemoteTask task = new EjectDriveTask(remoteComputer);
			RunRemoteTask(task);
		}

		internal void QueryBiosVersion()
		{
			RemoteTask task = new BiosVersionTask(remoteComputer);
			RunRemoteTask(task);
		}

		internal void QueryBatteryInfo()
		{
			RemoteTask task = new BatteryInfoTask(remoteComputer);
			RunRemoteTask(task);
		}

		internal void QueryDriveInfo()
		{
			RemoteTask task = new DriveInfoTask(remoteComputer);
			RunRemoteTask(task);
		}
	}
}