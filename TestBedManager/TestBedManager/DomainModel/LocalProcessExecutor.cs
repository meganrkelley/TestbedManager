using System.Diagnostics;
using System.IO;

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

		public void OpenDocumentationFile()
		{
			Process proc = new Process();
			proc.StartInfo.FileName = Path.Combine(
				Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,
				"Documentation.txt");
			proc.Start();
		}
	}
}