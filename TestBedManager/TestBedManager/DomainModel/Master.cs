namespace TestBedManager
{
	/// <summary>
	/// Holds references to the MainWindow and other GUI elements needed by classes that access the GUI.
	/// </summary>
	public class Master
	{
		public static MainWindow main { get; set; }

		public static TestbedTable table { get; set; }

		public static OutputLogManager logManager { get; set; }

		private static Browser _browser;

		public static Browser browser
		{
			// A window cannot be Shown again after it's been closed,
			// so just recreate the window if it's not visible.
			get
			{
				if (_browser == null || !_browser.IsVisible)
					_browser = new Browser();
				return _browser;
			}
			set { }
		}
	}
}