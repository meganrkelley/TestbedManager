using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Threading;
using TestBedManager.Properties;

namespace TestBedManager
{
	/// <summary>
	/// Every {interval} seconds, update the status of the given RemoteComputer by pinging it.
	/// </summary>
	public class ConnectionMonitor
	{
		private RemoteComputer remoteComputer;
		private int interval;
		private BackgroundWorker bgWorker;

		public ConnectionMonitor(RemoteComputer computer)
		{
			this.interval = Settings.Default.PingInterval;
			CommonInitialize(computer);
		}

		public ConnectionMonitor(RemoteComputer computer, int seconds)
		{
			this.interval = seconds;
			CommonInitialize(computer);
		}

		private void CommonInitialize(RemoteComputer computer)
		{
			remoteComputer = computer;
			InitializeBackgroundWorker();
			Start();
		}

		~ConnectionMonitor()
		{
			Stop();
		}

		private void InitializeBackgroundWorker()
		{
			bgWorker = new BackgroundWorker {
				WorkerSupportsCancellation = true,
				WorkerReportsProgress = true
			};
			bgWorker.DoWork += bgWorker_DoWork;
			bgWorker.ProgressChanged += bgWorker_ProgressChanged;
		}

		public void Start()
		{
			bgWorker.RunWorkerAsync();
		}

		public void Stop()
		{
			bgWorker.CancelAsync();
		}

		private NetworkStatus ToNetworkStatus(IPStatus ipStatus)
		{
			return ipStatus == IPStatus.Success ?
				NetworkStatus.PingOnly : NetworkStatus.Disconnected;
		}

		private void PingRemoteComputer()
		{
			PingReply reply = new Ping().Send(remoteComputer.ipAddress);
			bgWorker.ReportProgress(100, ToNetworkStatus(reply.Status));
		}

		private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			while (!bgWorker.CancellationPending) {
				try {
					PingRemoteComputer();
				} catch (System.Exception ex) {
					DebugLog.DebugLog.Log(ex);
				}
				Thread.Sleep(interval * 1000);
			}
		}

		private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			remoteComputer.status = (NetworkStatus)e.UserState;
		}
	}
}