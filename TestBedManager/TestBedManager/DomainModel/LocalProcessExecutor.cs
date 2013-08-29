using System.Diagnostics;

namespace TestBedManager
{
	public class LocalProcessExecutor
	{
		public void StartRemoteDesktop(string hostname)
		{
			Process proc = new Process();
			proc.StartInfo.FileName = "mstsc.exe";
			proc.StartInfo.Arguments = "/v:" + hostname;
			proc.Start();
		}
	}
}
