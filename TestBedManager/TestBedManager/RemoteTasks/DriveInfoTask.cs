using System;
using System.Management;

namespace TestBedManager
{
	public class DriveInfoTask : RemoteTask
	{
		public DriveInfoTask(RemoteComputer computer)
			: base(computer)
		{
			SetUpWmiConnection(WmiClass.Disk);
		}

		public override void Run()
		{
			remoteComputer.Log("Querying logical drives...");
			ObjectQuery query = new ObjectQuery(String.Format("select * from {0}", WmiClass.Disk));

			try {
				using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
					foreach (var item in wmiObjectSearcher.Get()) {
						remoteComputer.Log(FormatString(item), false);
					}
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(string.Format(
					"Error when executing WMI query/method on {0}: {1}",
					remoteComputer.ipAddressStr, ex));
				remoteComputer.Log("Error: " + ex.Message);
				WmiConnectionHandler.AttemptReconnect(mgmtClass.Scope);
			}

			remoteComputer.Log("End of logical drives." + Environment.NewLine);
		}

		private string FormatString(ManagementBaseObject item)
		{
			string result = "";

			if (item["Caption"] != null)
				result += "Volume: " + item["Caption"].ToString() + Environment.NewLine;
			if (item["VolumeName"] != null)
				result += "Name: " + item["VolumeName"].ToString() + Environment.NewLine;
			if (item["FileSystem"] != null)
				result += "File system: " + item["FileSystem"].ToString() + Environment.NewLine;
			if (item["FreeSpace"] != null)
				result += "Free space (GB): " + ((ulong)item["FreeSpace"] / 1073741824).ToString() + Environment.NewLine;
			if (item["Size"] != null)
				result += "Total size (GB): " + ((ulong)item["Size"] / 1073741824).ToString() + Environment.NewLine;

			return result;
		}
	}
}