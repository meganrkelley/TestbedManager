using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Controls;

namespace TestBedManager
{
	internal class AppStats
	{
		private BackgroundWorker worker = new BackgroundWorker();
		private TextBlock statusText;

		public AppStats(TextBlock statusTextObj)
		{
			statusText = statusTextObj;

			worker.WorkerReportsProgress = true;
			worker.DoWork += worker_DoWork;
			worker.ProgressChanged += worker_ProgressChanged;

			worker.RunWorkerAsync();
		}

		private void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			while (true) {
				List<object> userStateObj = new List<object>(2);
				userStateObj.Add(Process.GetCurrentProcess().PrivateMemorySize64 / 1048576);
				userStateObj.Add(0);

				worker.ReportProgress(0, userStateObj);
				Thread.Sleep(1000);
			}
		}

		private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			List<object> userStateObj = (List<object>)e.UserState;
			long memorySize = (long)userStateObj[0];

			statusText.Text = "Memory usage: " + memorySize + " MB";
		}
	}
}