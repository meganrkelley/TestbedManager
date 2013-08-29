using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestBedManager
{
	public class RemoteTaskManager
	{
		private RemoteComputer remoteComputer;

		public RemoteTaskManager(RemoteComputer computer)
		{
			this.remoteComputer = computer;
		}

		private void RunRemoteTask(RemoteTask task, string parameter)
		{
			try {
				Task.Factory.StartNew(() => task.Run(parameter));
			} catch (Exception ex) {
				Trace.WriteLine(ex);
			}
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
			RunRemoteTask(task, "");
		}

		public void QueryComputerSystem()
		{
			RemoteTask task = new ComputerSystemQueryTask(remoteComputer);
			RunRemoteTask(task, "");
		}

		public void QueryLocalTime()
		{
			RemoteTask task = new LocalTimeQueryTask(remoteComputer);
			RunRemoteTask(task, "");
		}

		public void QueryScheduledJobs()
		{
			RemoteTask task = new ScheduledJobsQueryTask(remoteComputer);
			RunRemoteTask(task, "");
		}

		public void QueryRunningProcesses()
		{
			RemoteTask task = new RunningProcessesQueryTask(remoteComputer);
			RunRemoteTask(task, "");
		}

		public void QueryInstalledPrograms()
		{
			RemoteTask task = new ProgramsQueryTask(remoteComputer);
			RunRemoteTask(task, "");
		}

		public void QueryEventViewer(string eventID)
		{
			RemoteTask task = new EventViewerQueryTask(remoteComputer);
			RunRemoteTask(task, eventID);
		}

		public void QueryNetworkData()
		{
			RemoteTask task = new NetworkQueryTask(remoteComputer);
			RunRemoteTask(task, "");
		}

		public void RenameComputer(string newHostname)
		{
			RemoteTask task = new RenameComputerTask(remoteComputer);
			RunRemoteTask(task, newHostname);
		}
	}
}