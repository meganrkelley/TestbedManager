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
			ObjectQuery query = new ObjectQuery(String.Format("select * from {0}", WmiClass.Disk));

			try {
				using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
					foreach (var item in wmiObjectSearcher.Get()) {
						if (item == null)
							continue;
						remoteComputer.Log(FormatString(item));
					}
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(string.Format(
					"Error when executing WMI query/method on {0}: {1}",
					remoteComputer.ipAddressStr, ex));
				remoteComputer.Log("Error: " + ex.Message);
				WmiConnectionHandler.AttemptReconnect(mgmtClass.Scope);
			}
		}

		private string FormatString(ManagementBaseObject item)
		{
			string result = "";

			if (!string.IsNullOrEmpty(item["Caption"].ToString()))
				result += "Volume: " + item["Caption"].ToString() + Environment.NewLine;
			if (!string.IsNullOrEmpty(item["VolumeName"].ToString()))
				result += "Name: " + item["VolumeName"].ToString() + Environment.NewLine;
			if (!string.IsNullOrEmpty(item["FileSystem"].ToString()))
				result += "File system: " + item["FileSystem"].ToString() + Environment.NewLine;
			if (!string.IsNullOrEmpty(item["FreeSpace"].ToString()))
				result += "Free space (GB): " + ((ulong)item["FreeSpace"] / 1073741824).ToString() + Environment.NewLine;
			if (!string.IsNullOrEmpty(item["Size"].ToString()))
				result += "Total size (GB): " + ((ulong)item["Size"] / 1073741824).ToString() + Environment.NewLine;

			return result;
		}
	}
}