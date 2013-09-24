namespace TestBedManager
{
	public class CreateProcessTask : RemoteTask
	{
		public CreateProcessTask(RemoteComputer computer)
		{
			this.remoteComputer = computer;
			base.SetUpWmiConnection(WmiClass.Process);
		}

		public override void Run(string command)
		{
			var inParams = mgmtClass.GetMethodParameters("Create");
			inParams["CommandLine"] = command;
			mgmtClass.InvokeMethod("Create", inParams, null);
		}
	}
}