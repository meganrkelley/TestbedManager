namespace TestBedManager
{
	/// <summary>
	/// Constant strings containing WMI class names.
	/// </summary>
	public static class WmiClass
	{
		public const string
			Account = "Win32_Account",
			Battery = "Win32_Battery",
			BIOS = "Win32_BIOS",
			ComputerSystem = "Win32_ComputerSystem",
			ComputerSystemProduct = "Win32_ComputerSystemProduct",
			Disk = "Win32_LogicalDisk",
			LocalTime = "Win32_LocalTime",
			NetworkAdapter = "Win32_NetworkAdapter",
			NetworkConfig = "Win32_NetworkAdapterConfiguration",
			NetworkConnection = "Win32_NetworkConnection",
			EventViewer = "Win32_NTLogEvent",
			OS = "Win32_OperatingSystem",
			PnPSignedDriver = "Win32_PnPSignedDriver",
			PowerPlan = "Win32_PowerPlan",
			Process = "Win32_Process",
			Product = "Win32_Product",
			ScheduledJob = "Win32_ScheduledJob";
	}
}