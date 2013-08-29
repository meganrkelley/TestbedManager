using System.Collections.Generic;

namespace TestBedManager
{
	/// <summary>
	/// A Collection of RemoteComputers.
	/// </summary>
	public class Testbed : IComputerObserver
	{
		#region Private members
		
		private List<ITestbedObserver> observers = new List<ITestbedObserver>();
		private List<RemoteComputer> _items = new List<RemoteComputer>();
		private int _ID;
		private string _title;

		#endregion

		#region Accessors

		public List<RemoteComputer> items
		{
			get { return _items; }
			set { _items = value; }
		}

		public int ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

		public string title
		{
			get { return _title; }
			set { _title = value; }
		}

		#endregion

		#region Constructor/Destructor
		
		public Testbed()
		{
			Attach(Master.table);
			Attach(Master.databaseManager);
		}

		public Testbed(string title)
		{
			this.title = title;
			Attach(Master.table);
			Attach(Master.databaseManager);
		}

		~Testbed()
		{
			Detach(Master.table);
			Detach(Master.databaseManager);
		}

		#endregion

		#region Observing

		public void Update(RemoteComputer item)
		{
			Notify();
		}

		public void Attach(ITestbedObserver observer)
		{
			observers.Add(observer);
		}

		public void Detach(ITestbedObserver observer)
		{
			observers.Remove(observer);
		}

		public void Notify()
		{
			foreach (ITestbedObserver observer in observers)
				observer.Update(this);
		}

		#endregion

		/// <summary>
		/// Add a computer to this testbed. Attach this testbed as an observer of the computer.
		/// </summary>
		/// <param name="item"></param>
		public void Add(RemoteComputer item)
		{
			if (!items.Contains(item)) {
				items.Add(item);
				item.Attach(this);
				Notify();
			}
		}

		public void Remove(RemoteComputer item)
		{
			items.Remove(item);
			item.Detach(this);
			Notify();
		}

		public void Clear()
		{
			items.Clear();
			foreach (var item in items)
				item.Detach(this);
			Notify();
		}

		public static Testbed ToTestbed(List<RemoteComputer> list)
		{
			Testbed testbed = new Testbed();
			foreach (var item in list)
				testbed.items.Add(item);
			return testbed;
		}
	}
}
