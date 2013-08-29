//using System;
//using System.Diagnostics;
//using System.IO;
//using System.Management;
//using System.Net;
//using System.Net.NetworkInformation;
//using System.Runtime.InteropServices;
//using System.Threading;

//namespace RemoteControl
//{
//	public class WmiTools
//	{
//		private readonly string hostname;
//		private readonly string password;
//		private readonly string username;

//		public WmiTools()
//		{
//		}

//		public WmiTools(string hostname, string username, string password)
//		{
//			this.hostname = hostname;
//			this.username = username;
//			this.password = password;
//		}

//		public MainWindow mainWindow { get; set; }

//		public void Write(string text)
//		{
//			Console.WriteLine(text);
//		}

//		public void WmiExceptionMessage(Exception ex)
//		{
//			string message = "";
//			if (ex == null)
//			{
//				message = "";
//			}
//			else
//			{
//				message = "Exception: " + ex.Message;
//			}
//			string text = "Unable to access remote machine. " + message + Environment.NewLine +
//						  "Possible solutions: " + Environment.NewLine +
//						  "\t - Verify the username and password are correct." + Environment.NewLine +
//						  "\t - Ensure that the user has Administrator privileges." + Environment.NewLine +
//						  "\t - Make sure that WMI control is allowed on the remote machine." + Environment.NewLine +
//						  "\t - Minimum OS requirement is Windows 2000.";

//			Write(text + Environment.NewLine);
//		}

//		#region Process/ITE

//		/// <summary>
//		///     Runs a cmd process on a remote machine and gets its output.
//		///     This requires write access to the C$ administrative share on the remote machine.
//		/// </summary>
//		/// <param name="command">The command or program to run</param>
//		public void RunCustomCommand(string command)
//		{
//			string outputFilePath = "\\\\" + hostname + "\\C$\\WiseUtilities_" + Path.GetRandomFileName();
//			string fullCommand = "cmd /c \"" + command + " 1> " + outputFilePath + " 2>&1\"";

//			try
//			{
//				ManagementScope mgmtScope = CreateRemoteScope();
//				mgmtScope.Connect();

//				var managementPath = new ManagementPath("Win32_Process");
//				var processClass = new ManagementClass(mgmtScope, managementPath, new ObjectGetOptions());

//				object[] methodArgs = { fullCommand, null, null, 0 };

//				processClass.InvokeMethod("Create", methodArgs);
//			}
//			catch (Exception ex)
//			{
//				if (ex is UnauthorizedAccessException ||
//					ex is COMException ||
//					ex is ManagementException)
//				{
//					WmiExceptionMessage(ex);
//					return;
//				}
//			}

//			Write("Reading output file from your command...");
//			ReadPrintDelete(outputFilePath);
//		}

//		public void ClearIteTestContent(string IP)
//		{
//			if (!IsITEInstalled())
//			{
//				Write("ITE is not installed.");
//				return;
//			}
//			KillAllITEProcesses();
//			Thread.Sleep(1000);
//			RunClearContentBatchFile();
//			StartITETestExecutor();
//		}

//		public void KillAllITEProcesses()
//		{
//			try
//			{
//				ManagementScope mgmtScope = CreateRemoteScope();
//				mgmtScope.Connect();
//				var managementPath = new ManagementPath("Win32_Process");
//				var processClass = new ManagementClass(mgmtScope, managementPath, new ObjectGetOptions());

//				string killITE_command = "cmd /c \"taskkill /F /IM ITE*\"";
//				object[] killITE_methodArgs = { killITE_command, null, null, 0 };
//				object killITE_result = null;

//				killITE_result = processClass.InvokeMethod("Create", killITE_methodArgs);
//			}
//			catch (Exception ex)
//			{
//				if (ex is UnauthorizedAccessException ||
//					ex is COMException ||
//					ex is ManagementException)
//				{
//					WmiExceptionMessage(ex);
//					return;
//				}
//			}

//			DateTime start = DateTime.Now;
//			int timeoutInSeconds = 30;
//			while (IsITETestExecutorRunning())
//			{
//				if (start.AddSeconds(timeoutInSeconds) < DateTime.Now)
//				{
//					Write("ITE Test Executor was not killed after " + timeoutInSeconds + " seconds.");
//					return;
//				}
//				Thread.Sleep(1000);
//			}
//			Write("Killed all ITE processes successfully.");
//		}

//		public void RunClearContentBatchFile()
//		{
//			try
//			{
//				ManagementScope mgmtScope = CreateRemoteScope();
//				mgmtScope.Connect();
//				var managementPath = new ManagementPath("Win32_Process");
//				var processClass = new ManagementClass(mgmtScope, managementPath, new ObjectGetOptions());

//				string command = "cmd /c \""
//								 +
//								 "rd /S /Q \"C:\\ProgramData\\ITE\\Test Content\\Test Libraries\" \"C:\\ProgramData\\ITE\\Test Content\\Test Sets\" "
//								 +
//								 " && md \"C:\\ProgramData\\ITE\\Test Content\\Test Libraries\" \"C:\\ProgramData\\ITE\\Test Content\\Test Sets\" "
//								 + " && del /F /Q \"C:\\ProgramData\\ITE\\Settings\\localtestcontent.itereg\" "
//								 + "\"";
//				object[] methodArgs = { command, null, null, 0 };
//				object result = null;

//				result = processClass.InvokeMethod("Create", methodArgs);
//			}
//			catch (Exception ex)
//			{
//				if (ex is UnauthorizedAccessException ||
//					ex is COMException ||
//					ex is ManagementException)
//				{
//					WmiExceptionMessage(ex);
//					return;
//				}
//			}

//			Write("Attempted to delete ITE Test Content. Verifying...");

//			Thread.Sleep(3000);
//			ValidateTestContentCleared();
//		}

//		public void ValidateTestContentCleared()
//		{
//			ManagementScope mgmtScope = null;
//			var libsQuery =
//				new ObjectQuery(
//					@"SELECT * FROM Win32_Directory WHERE Drive='C:' AND Path='\\programdata\\ite\\test content\\test libraries\\'");
//			var setsQuery =
//				new ObjectQuery(
//					@"SELECT * FROM Win32_Directory WHERE Drive='C:' AND Path='\\programdata\\ite\\test content\\test sets\\'");

//			Write("Verifying Test Libraries is empty...");
//			try
//			{
//				mgmtScope = CreateRemoteScope();
//				mgmtScope.Connect();

//				ManagementObjectCollection libsFiles = new ManagementObjectSearcher(mgmtScope, libsQuery).Get();
//				if (libsFiles.Count > 0)
//				{
//					Write("Failed to clear Test Libraries: " + libsFiles.Count + " folder(s) found:");
//					foreach (ManagementObject file in libsFiles)
//						Write("    " + file.GetPropertyValue("Name") + " " + file.GetPropertyValue("LastModified"));
//				}
//				else
//					Write("Successfully cleared Test Libraries.");
//			}
//			catch (Exception ex)
//			{
//				if (ex is UnauthorizedAccessException ||
//					ex is COMException ||
//					ex is ManagementException)
//				{
//					WmiExceptionMessage(ex);
//				}
//			}

//			Write("Verifying Test Sets is empty...");
//			try
//			{
//				ManagementObjectCollection setsFiles = new ManagementObjectSearcher(mgmtScope, setsQuery).Get();
//				if (setsFiles.Count > 0)
//				{
//					Write("Failed to clear Test Sets: " + setsFiles.Count + " folder(s) found.");
//					foreach (ManagementObject file in setsFiles)
//						Write("    " + file.GetPropertyValue("Name"));
//				}
//				else
//					Write("Successfully cleared Test Sets.");
//			}
//			catch (Exception ex)
//			{
//				if (ex is UnauthorizedAccessException ||
//					ex is COMException ||
//					ex is ManagementException)
//				{
//					WmiExceptionMessage(ex);
//				}
//			}
//		}

//		public void StartITETestExecutor()
//		{
//			if (!IsITEInstalled())
//			{
//				Write("ITE is not installed.");
//				return;
//			}

//			Write("Starting ITE Test Executor...");
//			string RemoteITEProgramFilesPath = "";
//			if (GetArchitecture().Contains("64"))
//				RemoteITEProgramFilesPath = @"C:\Program Files (x86)\ITE\Integrated Testing Environment\ITETestExecutor.exe";
//			else
//				RemoteITEProgramFilesPath = @"C:\Program Files\ITE\Integrated Testing Environment\ITETestExecutor.exe";

//			var proc = new Process();
//			proc.StartInfo.FileName = "schtasks";
//			proc.StartInfo.Arguments = " /create /s \\\\" + hostname + " /u " + username + " /p " + password +
//									   " /tn \"Start ITE Test Executor\" /sc once /tr \"'" + RemoteITEProgramFilesPath +
//									   "'\" /f /st 00:00";

//			//st " + ConvertDateForSchTasks(GetLocalTime().AddMinutes(1)) + " /Z /F";
//			proc.StartInfo.CreateNoWindow = true;
//			proc.StartInfo.UseShellExecute = false;
//			proc.StartInfo.RedirectStandardError = true;
//			proc.StartInfo.RedirectStandardOutput = true;
//			proc.Start();
//			proc.WaitForExit();

//			proc = new Process();
//			proc.StartInfo.FileName = "schtasks";
//			proc.StartInfo.Arguments = " /run /s \\\\" + hostname + " /u " + username + " /p " + password +
//									   " /i /tn \"Start ITE Test Executor\"";
//			proc.StartInfo.CreateNoWindow = true;
//			proc.StartInfo.UseShellExecute = false;
//			proc.StartInfo.RedirectStandardError = true;
//			proc.StartInfo.RedirectStandardOutput = true;
//			proc.Start();
//			proc.WaitForExit();

//			DateTime start = DateTime.Now;
//			int timeoutInSeconds = 30;
//			while (!IsITETestExecutorRunning())
//			{
//				if (start.AddSeconds(timeoutInSeconds) < DateTime.Now)
//				{
//					Write("ITE Test Executor did not start after " + timeoutInSeconds + " seconds.");
//					return;
//				}
//				Thread.Sleep(1000);
//			}
//			Write("ITE Test Executor started successfully.");
//		}

//		public bool IsITETestExecutorRunning()
//		{
//			try
//			{
//				ManagementScope mgmtScope = CreateRemoteScope();
//				mgmtScope.Connect();
//				var query = new ObjectQuery("SELECT * FROM Win32_Process WHERE Name='ITETestExecutor.exe'");
//				ManagementObjectCollection queryCollection = new ManagementObjectSearcher(mgmtScope, query).Get();
//				if (queryCollection.Count > 0)
//					return true;
//			}
//			catch (Exception ex)
//			{
//				if (ex is UnauthorizedAccessException ||
//					ex is COMException ||
//					ex is ManagementException)
//				{
//					WmiExceptionMessage(ex);
//				}
//			}
//			return false;
//		}

//		public bool IsITEInstalled()
//		{
//			string RemoteITEProgramFilesPath = string.Empty;
//			if (GetArchitecture().Contains("64"))
//				RemoteITEProgramFilesPath = "\\\\program files (x86)\\\\ite\\\\";
//			else
//				RemoteITEProgramFilesPath = "\\\\program files\\\\ite\\\\";

//			try
//			{
//				ManagementScope mgmtScope = CreateRemoteScope();
//				mgmtScope.Connect();

//				var iteProgramFilesQuery =
//					new ObjectQuery(@"SELECT * FROM Win32_Directory WHERE Drive='C:' AND Path='" + RemoteITEProgramFilesPath + "'");
//				ManagementObjectCollection iteProgramFilesResults = new ManagementObjectSearcher(mgmtScope, iteProgramFilesQuery).Get();

//				if (iteProgramFilesResults.Count > 0)
//					return true;
//			}
//			catch (Exception ex)
//			{
//				if (ex is UnauthorizedAccessException ||
//					ex is COMException ||
//					ex is ManagementException)
//				{
//					WmiExceptionMessage(ex);
//				}
//			}

//			return false;
//		}

//		#endregion Process/ITE

//		#region Drivers

//		public void GetDriversByClass(string selectedClass)
//		{
//			try
//			{
//				ManagementScope mgmtScope = CreateRemoteScope();
//				mgmtScope.Connect();

//				Write("Searching for \"" + selectedClass + "\" drivers..." + Environment.NewLine);

//				ObjectQuery query =
//					query = new ObjectQuery("SELECT * FROM Win32_PnPSignedDriver WHERE DeviceClass='" + selectedClass.ToUpper() +
//											"' AND DeviceName IS NOT NULL AND DriverVersion IS NOT NULL");
//				ManagementObjectCollection queryCollection = new ManagementObjectSearcher(mgmtScope, query).Get();
//				if (queryCollection.Count == 0)
//				{
//					Write("No \"" + selectedClass + "\" drivers found.");
//					return;
//				}
//				foreach (ManagementObject queryResult in queryCollection)
//				{
//					string deviceName = queryResult.GetPropertyValue("DeviceName").ToString();
//					string driverVersion = queryResult.GetPropertyValue("DriverVersion").ToString();
//					Write(deviceName + " | " + driverVersion);
//				}
//				Write(Environment.NewLine + "End of \"" + selectedClass + "\" drivers.");
//			}
//			catch (Exception ex)
//			{
//				if (ex is UnauthorizedAccessException ||
//					ex is COMException ||
//					ex is ManagementException)
//				{
//					WmiExceptionMessage(ex);
//				}
//			}
//		}

//		public void GetDriversByName(string name)
//		{
//			string searchName = "", className = "";
//			if (name.ToUpper() == "INTEL WIRELESS-N XXXX")
//			{
//				searchName = "-N"; // Could be "Wireless-N" or "Advanced-N".
//				className = "NET";
//			}
//			else if (name.ToUpper() == "INTEL BLUETOOTH + HS")
//			{
//				searchName = "Wireless Bluetooth";
//				className = "BLUETOOTH";
//			}
//			else
//				return;

//			try
//			{
//				ManagementScope mgmtScope = CreateRemoteScope();
//				mgmtScope.Connect();

//				Write("Searching for " + name + " driver...");

//				var query = new ObjectQuery("SELECT * FROM Win32_PnPSignedDriver WHERE DeviceClass='" + className +
//											"' AND DeviceName IS NOT NULL AND DriverVersion IS NOT NULL AND DeviceName LIKE \"%Intel%\"");

//				ManagementObjectCollection queryCollection = new ManagementObjectSearcher(mgmtScope, query).Get();
//				if (queryCollection.Count == 0)
//				{
//					Write("No \"" + name + "\" drivers found.");
//					return;
//				}
//				foreach (ManagementObject queryResult in queryCollection)
//				{
//					string deviceName = queryResult.GetPropertyValue("DeviceName").ToString();
//					string driverVersion = queryResult.GetPropertyValue("DriverVersion").ToString();

//					if (deviceName.Contains(searchName))
//					{
//						Write(deviceName + " | " + driverVersion);
//						return;
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				if (ex is UnauthorizedAccessException ||
//					ex is COMException ||
//					ex is ManagementException)
//				{
//					WmiExceptionMessage(ex);
//					return;
//				}
//			}
//		}

//		// Search the registry for Intel's WiFi TIC. Works on 15.x, but path could change with 16.x+
//		public string GetWiFiTic()
//		{
//			string result = string.Empty;

//			try
//			{
//				ManagementScope mgmtScope = CreateRemoteScope();
//				mgmtScope.Connect();
//				var registryClass = new ManagementClass(mgmtScope, new ManagementPath("StdRegProv"), null);
//				ManagementBaseObject inParams = registryClass.GetMethodParameters("GetStringValue");
//				inParams["sSubKeyName"] = @"SOFTWARE\Intel\Wireless";
//				inParams["sValueName"] = "TIC";

//				ManagementBaseObject outParams = registryClass.InvokeMethod("GetStringValue", inParams, null);

//				if (!string.IsNullOrEmpty(result))
//					result = outParams["sValue"].ToString();
//				else
//				{
//					inParams["sSubKeyName"] = @"SOFTWARE\Intel\Wireless\Install";
//					inParams["sValueName"] = "TIC";

//					outParams = registryClass.InvokeMethod("GetStringValue", inParams, null);
//					result = outParams["sValue"].ToString();
//				}
//			}
//			catch (Exception ex)
//			{
//				if (!(ex is NullReferenceException))
//					WmiExceptionMessage(ex);
//				return "No WiFi TIC found.";
//			}

//			return "WiFi TIC: " + result;
//		}

//		#endregion Drivers

//		#region Local Time

//		public DateTime GetLocalTime()
//		{
//			string mon = "", day = "", year = "", hour = "", min = "", sec = "";

//			try
//			{
//				ManagementScope mgmtScope = CreateRemoteScope();
//				mgmtScope.Connect();

//				var query = new ObjectQuery("SELECT * FROM Win32_LocalTime");
//				ManagementObjectCollection queryCollection = new ManagementObjectSearcher(mgmtScope, query).Get();
//				foreach (ManagementObject result in queryCollection)
//				{
//					mon = result.GetPropertyValue("Month").ToString();
//					day = result.GetPropertyValue("Day").ToString();
//					year = result.GetPropertyValue("Year").ToString();
//					hour = result.GetPropertyValue("Hour").ToString();
//					min = result.GetPropertyValue("Minute").ToString();
//					sec = result.GetPropertyValue("Second").ToString();
//				}
//			}
//			catch (Exception ex)
//			{
//				if (ex is UnauthorizedAccessException ||
//					ex is COMException ||
//					ex is ManagementException)
//				{
//					WmiExceptionMessage(ex);
//					return DateTime.MinValue;
//				}
//			}

//			string timeString = mon + "/" + day + "/" + year + " " + hour + ":" + min + ":" + sec;
//			DateTime time = DateTime.Parse(timeString);

//			return time;
//		}

//		public bool SetTime(string serverName)
//		{
//			try
//			{
//				string fullCommand = @"net time \\" + serverName + " /set /yes";
//				ManagementScope mgmtScope = CreateRemoteScope();
//				mgmtScope.Connect();

//				// Create WMI path and class
//				var managementPath = new ManagementPath("Win32_Process");
//				var processClass = new ManagementClass(mgmtScope, managementPath, new ObjectGetOptions());

//				// Call Process "Create" method with given parameters
//				object[] methodArgs = { fullCommand, null, null, 0 };
//				object result = null;
//				result = processClass.InvokeMethod("Create", methodArgs);
//			}
//			catch (Exception ex)
//			{
//				if (ex is UnauthorizedAccessException ||
//					ex is COMException ||
//					ex is ManagementException)
//				{
//					WmiExceptionMessage(ex);
//					return false;
//				}
//			}
//			return true;
//		}

//		#endregion Local Time

//		#region Operating System

//		public void RestartMachine(string IP)
//		{
//			try
//			{
//				ManagementScope mgmtScope = CreateRemoteScope();
//				mgmtScope.Connect();
//				var osQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
//				ManagementObjectCollection queryCollection = new ManagementObjectSearcher(mgmtScope, osQuery).Get();
//				foreach (ManagementObject result in queryCollection)
//					result.InvokeMethod("Reboot", null);
//			}
//			catch (Exception ex)
//			{
//				if (ex is UnauthorizedAccessException ||
//					ex is COMException ||
//					ex is ManagementException)
//				{
//					WmiExceptionMessage(ex);
//					return;
//				}
//			}

//			Write("Restarting...");

//			ThreadStart starter = () => PingUntilSuccess(IP, 120);
//			var pingerThread = new Thread(starter) { IsBackground = true };
//			pingerThread.Start();
//		}

//		/// <summary>
//		///     Get the OS architecture of a remote machine.
//		/// </summary>
//		/// <returns>"32-bit" or "64-bit"</returns>
//		public string GetArchitecture()
//		{
//			try
//			{
//				ManagementScope mgmtScope = CreateRemoteScope();
//				mgmtScope.Connect();
//				var query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
//				ManagementObjectCollection queryCollection = new ManagementObjectSearcher(mgmtScope, query).Get();
//				foreach (ManagementObject queryResult in queryCollection)
//					return queryResult.GetPropertyValue("OSArchitecture").ToString();
//			}
//			catch (Exception ex)
//			{
//				if (ex is UnauthorizedAccessException ||
//					ex is COMException ||
//					ex is ManagementException)
//				{
//					WmiExceptionMessage(ex);
//				}
//			}
//			return "Unknown";
//		}

//		public string GetOSInfo()
//		{
//			string result = "";
//			try
//			{
//				ManagementScope mgmtScope = CreateRemoteScope();
//				mgmtScope.Connect();
//				var query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
//				ManagementObjectCollection queryCollection = new ManagementObjectSearcher(mgmtScope, query).Get();
//				foreach (ManagementObject queryResult in queryCollection)
//					result += queryResult.GetPropertyValue("Caption") + " (" +
//							  queryResult.GetPropertyValue("OSArchitecture") + ")";
//			}
//			catch (Exception ex)
//			{
//				if (ex is UnauthorizedAccessException ||
//					ex is COMException ||
//					ex is ManagementException)
//				{
//					WmiExceptionMessage(ex);
//				}
//			}
//			return result;
//		}

//		#endregion Operating System

//		#region Computer System

//		public void RenameRemoteMachine(string IP, string newHostname)
//		{
//			string currentHostname = hostname;

//			var connectionOptions = new ConnectionOptions
//				{
//					Authentication = AuthenticationLevel.PacketPrivacy,
//					Impersonation = ImpersonationLevel.Impersonate,
//					EnablePrivileges = true,
//					Username = username,
//					Password = password
//				};

//			var computerSystemPath = new ManagementPath
//				{
//					ClassName = "Win32_ComputerSystem",
//					Server = currentHostname,
//					Path = currentHostname + "\\root\\cimv2:Win32_ComputerSystem.Name='" + currentHostname + "'",
//					NamespacePath = "\\\\" + currentHostname + "\\root\\cimv2"
//				};

//			var remoteScope = new ManagementScope(computerSystemPath, connectionOptions);
//			var remoteSystem = new ManagementObject(remoteScope, computerSystemPath, null);

//			try
//			{
//				ManagementBaseObject newRemoteSystemName = remoteSystem.GetMethodParameters("Rename");

//				TimeSpan methodOptions = new InvokeMethodOptions().Timeout = new TimeSpan(0, 10, 0);

//				newRemoteSystemName.SetPropertyValue("Name", newHostname);
//				newRemoteSystemName.SetPropertyValue("UserName", username);
//				newRemoteSystemName.SetPropertyValue("Password", password);

//				ManagementBaseObject outParams = remoteSystem.InvokeMethod("Rename", newRemoteSystemName, null);
//				if (!outParams.GetPropertyValue("ReturnValue").ToString().Equals("0"))
//				{
//					Write("Rename failed.");
//					return;
//				}
//			}
//			catch (Exception ex)
//			{
//				if (ex is UnauthorizedAccessException ||
//					ex is COMException ||
//					ex is ManagementException)
//				{
//					WmiExceptionMessage(ex);
//					return;
//				}
//			}
//			Write("Rename to " + newHostname + " successful.");

//			RestartMachine(IP);
//		}

//		public string GetComputerModel()
//		{
//			try
//			{
//				ManagementScope mgmtScope = CreateRemoteScope();
//				mgmtScope.Connect();
//				var query = new ObjectQuery("SELECT * FROM Win32_ComputerSystemProduct");
//				ManagementObjectCollection queryCollection = new ManagementObjectSearcher(mgmtScope, query).Get();
//				foreach (ManagementObject queryResult in queryCollection)
//					return queryResult.GetPropertyValue("Vendor") + " " +
//						   queryResult.GetPropertyValue("Name");
//			}
//			catch (Exception ex)
//			{
//				if (ex is UnauthorizedAccessException ||
//					ex is COMException ||
//					ex is ManagementException)
//				{
//					WmiExceptionMessage(ex);
//				}
//			}
//			return "Could not find computer model.";
//		}

//		public string GetBiosVersion()
//		{
//			try
//			{
//				ManagementScope mgmtScope = CreateRemoteScope();
//				mgmtScope.Connect();
//				var query = new ObjectQuery("SELECT * FROM Win32_BIOS");
//				ManagementObjectCollection queryCollection = new ManagementObjectSearcher(mgmtScope, query).Get();
//				foreach (ManagementObject queryResult in queryCollection)
//					return "Version: " + queryResult.GetPropertyValue("Version") + Environment.NewLine +
//						   "SMBIOS version: " + queryResult.GetPropertyValue("SMBIOSBIOSVersion");
//			}
//			catch (Exception ex)
//			{
//				if (ex is UnauthorizedAccessException ||
//					ex is COMException ||
//					ex is ManagementException)
//				{
//					WmiExceptionMessage(ex);
//				}
//			}
//			return "Could not find a BIOS version.";
//		}

//		#endregion Computer System

//		#region Helper functions

//		/// <summary>
//		///     Pings device until it goes down, then pings until it comes back up.
//		///     Waits timeoutInSeconds for machine to go down, then waits the same amount of time for it to come back up.
//		/// </summary>
//		/// <param name="_IP"></param>
//		/// <param name="_timeoutInSeconds"></param>
//		public void PingUntilSuccess(object _IP, object _timeoutInSeconds)
//		{
//			var IP = _IP as string;
//			int timeoutInSeconds = int.Parse(_timeoutInSeconds.ToString());

//			IPAddress outAddress;
//			if (!IPAddress.TryParse(IP, out outAddress))
//			{
//				Write(IP + " is not a valid IP address.");
//				return;
//			}

//			// Wait until the computer shuts off.
//			DateTime totalRebootTimeStart = DateTime.Now;
//			var ping = new PingRemoteComputer();
//			DateTime start = DateTime.Now;
//			while (start.AddSeconds(timeoutInSeconds) > DateTime.Now)
//			{
//				try
//				{
//					PingReply reply = ping.Send(IP, 500);
//					IPStatus pingStatus = reply.Status;
//					if (pingStatus != IPStatus.Success)
//					{
//						Write("Stopped responding to pings.");
//						break;
//					}
//					Thread.Sleep(500);
//				}
//				catch (PingException ex)
//				{
//					Write(ex.Message);
//				}
//			}

//			// Wait for the machine to come back up.
//			ping = new PingRemoteComputer();
//			start = DateTime.Now;
//			while (start.AddSeconds(timeoutInSeconds) > DateTime.Now)
//			{
//				try
//				{
//					PingReply reply = ping.Send(IP, 500);
//					IPStatus pingStatus = reply.Status;
//					if (pingStatus == IPStatus.Success)
//					{
//						string totalRebootTime = DateTime.Now.Subtract(totalRebootTimeStart).TotalSeconds.ToString();
//						Write("Successfully pinged. Total time to restart: " + totalRebootTime + " seconds.");
//						return;
//					}

//					//Write(".", false);
//					Thread.Sleep(500);
//				}
//				catch (PingException ex)
//				{
//					Write(ex.Message);
//				}
//			}
//			Write("Failed to respond to pings after " + timeoutInSeconds + " seconds.");
//		}

//		/// <summary>
//		///     Reads a file and prints its contents to the console window, then deletes it.
//		/// </summary>
//		/// <param name="filepath">Full path to file to read/delete.</param>
//		/// <param name="timeoutInSeconds">Seconds to wait for file to be created and unlocked.</param>
//		private void ReadPrintDelete(string filepath, int timeoutInSeconds = 10, bool delete = true)
//		{
//			// Wait for filepath to exist before trying to read it.
//			DateTime start = DateTime.Now;
//			while (!File.Exists(filepath))
//			{
//				if (start.AddSeconds(timeoutInSeconds) < DateTime.Now)
//				{
//					Write("File was not created after " + timeoutInSeconds + " seconds.");
//					return;
//				}
//				Thread.Sleep(1000);
//			}

//			// Wait for file to be unlocked before trying to read it.
//			start = DateTime.Now;
//			while (IsFileLocked(new FileInfo(filepath)))
//			{
//				if (start.AddSeconds(timeoutInSeconds) < DateTime.Now)
//				{
//					Write("File was not unlocked after " + timeoutInSeconds + " seconds.");
//					return;
//				}
//				Thread.Sleep(1000);
//			}

//			// Finally read the file and print to GUI.
//			try
//			{
//				Write(File.ReadAllText(filepath));
//				Write("End of output.");
//			}
//			catch (Exception ex)
//			{
//				if (ex is FileNotFoundException ||
//					ex is IOException)
//					Write("Error when attempting to read file: " + ex.Message);
//			}

//			if (!delete) return;

//			// Wait for it to unlock again.
//			while (IsFileLocked(new FileInfo(filepath)))
//			{
//				if (start.AddSeconds(timeoutInSeconds) < DateTime.Now)
//				{
//					Write("File was not unlocked after " + timeoutInSeconds + " seconds.");
//					return;
//				}
//				Thread.Sleep(1000);
//			}

//			// Delete the file.
//			try
//			{
//				File.Delete(filepath);
//			}
//			catch (Exception ex)
//			{
//				if (ex is FileNotFoundException ||
//					ex is IOException)
//					Write("Error when attempting to delete file: " + ex.Message);
//			}
//		}

//		protected virtual bool IsFileLocked(FileInfo file)
//		{
//			FileStream stream = null;

//			try
//			{
//				stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
//			}
//			catch (IOException)
//			{
//				return true;
//			}
//			finally
//			{
//				if (stream != null)
//					stream.Close();
//			}

//			return false;
//		}

//		public ManagementScope CreateRemoteScope(string wmiNamespace = "\\root\\cimv2")
//		{
//			var options = new ConnectionOptions
//				{
//					Username = username,
//					Password = password,
//					Impersonation = ImpersonationLevel.Impersonate,
//					EnablePrivileges = true,
//					Authentication = AuthenticationLevel.PacketPrivacy
//				};
//			return new ManagementScope("\\\\" + hostname + wmiNamespace, options);
//		}

//		#endregion Helper functions
//	}
//}