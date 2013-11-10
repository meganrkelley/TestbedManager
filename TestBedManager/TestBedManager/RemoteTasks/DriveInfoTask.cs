using System;
using System.Management;

namespace TestBedManager
{
	public class DriveInfoTask : RemoteTask
	{
		public DriveInfoTask(RemoteComputer computer) : base(computer)
		{
			SetUpWmiConnection(WmiClass.Disk);
		}

		public override void Run()
		{
			ObjectQuery query = new ObjectQuery(String.Format("select * from {0}", WmiClass.Disk));

			try {
				using (var wmiObjectSearcher = new ManagementObjectSearcher(mgmtClass.Scope, query)) {
					foreach (var item in wmiObjectSearcher.Get()) {
						remoteComputer.Log(
							"Volume: " + item["Caption"].ToString() + Environment.NewLine +
							"Name: " + item["VolumeName"].ToString() + Environment.NewLine +
							"File system: " + item["FileSystem"].ToString() + Environment.NewLine +
							"Free space (GB): " + ((ulong)item["FreeSpace"] / 1073741824).ToString() + Environment.NewLine +
							"Total size (GB): " + ((ulong)item["Size"] / 1073741824).ToString());
					}
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(string.Format("Error when executing WMI query/method on {0}: {1}",
					remoteComputer.ipAddressStr, ex));
				remoteComputer.Log("Error: " + ex.Message);
			}
		}
	}
}