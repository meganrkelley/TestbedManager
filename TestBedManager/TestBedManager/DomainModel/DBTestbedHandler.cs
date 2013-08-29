using System.Collections.Generic;
using System.Data;

namespace TestBedManager
{
	class DBTestbedHandler
	{
		DBTestbedsGateway testbedTableGateway = new DBTestbedsGateway();
        DBTestbedRelationsGateway testbedRelationsGateway = new DBTestbedRelationsGateway();

		public string Find(int ID)
		{
			DataTable findResults = testbedTableGateway.Find(ID);
			foreach (DataRow row in findResults.Rows)
				return row["title"].ToString();
			return null;
		}

		public int Find(string title)
		{
			DataTable findResults = testbedTableGateway.Find(title);
			foreach (DataRow row in findResults.Rows)
				return (int)row["ID"];
			return -1;
		}

		public void Add(string title)
		{
			testbedTableGateway.Insert(title);
		}

		public void Update(int ID, string title)
		{
			testbedTableGateway.Update(ID, title);
		}

		public void Remove(int ID)
		{
			testbedTableGateway.Delete(ID);
		}
	}
}
