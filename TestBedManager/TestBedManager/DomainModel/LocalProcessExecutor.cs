using System.Diagnostics;
using System.IO;

namespace TestBedManager
{
	public class LocalProcessExecutor
	{
		public void StartRemoteDesktop(string ip)
		{
			Process proc = new Process();
			proc.StartInfo.FileName = "mstsc.exe";
			proc.StartInfo.Arguments = "/v:" + ip;
			proc.Start();
		}

		public void OpenDocumentationFile()
		{
			string documentationFilepath = Path.Combine(Directory.GetCurrentDirectory(), "Documentation.txt");
			Process proc = new Process();
			proc.StartInfo.FileName = documentationFilepath;
			try {
				proc.Start();
			} catch (System.Exception ex) {
				DebugLog.DebugLog.Log("Couldn't open the documentation file at " + documentationFilepath + ": " + ex.Message);
			}
		}
	}
}