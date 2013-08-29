using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Net;
using TestBedManager.Properties;

namespace TestBedManager
{
	public class DatabaseManager : ITestbedObserver
	{
		public void Update(Testbed testbed)
		{
			//// Check if each computer in this testbed is already in the database. If it is, update its information.
			//foreach (RemoteComputer computer in testbed.items) {
			//	DataTable matches = Select(String.Format("select id from {0} where id = {1}", 
			//		Resources.TableName_Computers,
			//		computer.ID));
			//	if (matches.Rows.Count == 0) {
			//		computer.ID = Add(computer);
			//	} else {
			//		Edit(String.Format("update {0} set hostname = '{1}', ipAddress = '{2}', username = '{3}', password = '{4}' where id = {5}", 
			//			Resources.TableName_Computers, 
			//			computer.hostname, 
			//			computer.ipAddressStr, 
			//			computer.credentials.UserName, 
			//			computer.credentials.Password,
			//			computer.ID));
			//	}
			//}
		}

        public void CreateListWithItems(string title, Testbed testbed)
        {
            CreateList(title);
            foreach (var item in testbed.items)
                AddToList(item, title);
        }

        public List<Testbed> GetAllSavedLists()
        {
            List<Testbed> allLists = new List<Testbed>();
            DataTable table = Select("select * from " + Resources.TableName_Testbeds);
            foreach (DataRow row in table.Rows)
            {
                allLists.Add(new Testbed
                {
                    ID = (int)row["ID"],
                    title = row["title"].ToString()
                });
            }
            return allLists;
        }

        public Testbed GetAllComputers()
        {
            Testbed all = new Testbed();
            DataTable table = Select("select * from " + Resources.TableName_Computers);
            foreach (DataRow row in table.Rows)
            {
                all.items.Add(new RemoteComputer
                {
                    ID = int.Parse(row["ID"].ToString()),
                    hostname = row["hostname"].ToString(),
                    ipAddress = IPAddress.Parse(row["ipAddress"].ToString()),
                    credentials = new NetworkCredential(
                        row["username"].ToString(),
                        row["password"].ToString()
                    )
                });
            }
            return all;
        }

        private string GetComputerID(RemoteComputer computer)
        {
            DataTable computerIDTable = Select(String.Format("select ID from {0} where hostname = '{1}' and ipAddress = '{2}'",
                Resources.TableName_Computers, computer.hostname, computer.ipAddressStr));

            if (computerIDTable.Rows.Count == 0)
                return null;

            return computerIDTable.Rows[0]["ID"].ToString();
        }

        public Testbed GetListContents(string titleOrId)
        {
            Testbed computersInList = new Testbed();
            string listID = GetListID(titleOrId);

            DataTable computerIDTable = Select(String.Format("select ComputerID from {0} where ListID = {1}",
                Resources.TableName_TestbedRelations, listID));

            foreach (DataRow row in computerIDTable.Rows)
                computersInList.items.Add(GetRemoteComputer(row["ComputerID"].ToString()));

            return computersInList;
        }

        public string GetListID(string title)
        {
            int outID;
            if (int.TryParse(title, out outID))
                return title;

            DataTable listIDTable = Select(String.Format("select ID from {0} where title = '{1}'",
                Resources.TableName_Testbeds, title));

            if (listIDTable.Rows.Count == 0)
                return null;

            return listIDTable.Rows[0]["ID"].ToString();
        }

        public void RemoveComputer(RemoteComputer computer)
        {
            Edit(String.Format("delete from {0} where ID = {1}",
                Resources.TableName_Computers, computer.ID));

            Edit(String.Format("delete from {0} where ComputerID = {1}",
                Resources.TableName_TestbedRelations, computer.ID));
        }

        public void RemoveList(string titleOrId)
        {
            string listID = GetListID(titleOrId);

            Edit(String.Format("delete from {0} where ID = {1}",
                Resources.TableName_Testbeds, listID));

            Edit(String.Format("delete from {0} where listID = {1}",
                Resources.TableName_TestbedRelations, listID));
        }




		private int Add(RemoteComputer computer)
		{
			if (ComputerExists(computer))
				return int.Parse(GetComputerID(computer));

			Edit(String.Format("insert into {0} (hostname, ipAddress, username, password) values ('{1}','{2}','{3}','{4}')", 
				Resources.TableName_Computers, computer.hostname, computer.ipAddress, 
				computer.credentials.UserName, computer.credentials.Password));

			return int.Parse(GetComputerID(computer));
		}

		private void AddToList(RemoteComputer computer, string title)
		{
			if (ListContains(computer, title))
				return;
			string listID = GetListID(title);

			Edit(String.Format("insert into {0} (ComputerID, ListID) values ({1}, {2})", 
				Resources.TableName_TestbedRelations, computer.ID, listID));
		}

		private void CreateList(string title)
		{
			if (ListTitleExists(title))
				return;
			Edit(String.Format("insert into {0} (title) values ('{1}')", 
				Resources.TableName_Testbeds, title));
		}

		private RemoteComputer GetRemoteComputer(string computerID)
		{
			DataTable table = Select(String.Format("select * from {0} where ID = {1}", 
				Resources.TableName_Computers, computerID));
			foreach (DataRow row in table.Rows) {
				return new RemoteComputer(
					row["hostname"].ToString(),
					IPAddress.Parse(row["ipAddress"].ToString()),
					row["username"].ToString(),
					row["password"].ToString()
				);
			}
			return null;
		}

		private bool ListContains(RemoteComputer computer, string titleOrId)
		{
			string listID = GetListID(titleOrId);

			DataTable results = Select(
				String.Format("select ComputerID from {0} where ComputerID = '{1}' and ListID = '{2}'", 
				Resources.TableName_TestbedRelations, computer.ID, listID)
			);
			return results.Rows.Count > 0;
		}

		private bool ComputerExists(RemoteComputer computer)
		{
			DataTable matchingComputerTable = Select(
				String.Format("select ID from {0} where hostname = '{1}' and ipAddress = '{2}'", 
					Resources.TableName_Computers, computer.hostname, computer.ipAddressStr)
			);

			return matchingComputerTable.Rows.Count > 0;
		}

		private bool ListTitleExists(string title)
		{
			DataTable table = Select(String.Format("select ID from {0} where title = '{1}'", 
				Resources.TableName_Testbeds, title));

			return table.Rows.Count > 0;
		}

		private void Edit(string query)
		{
			try {
				var command = DBConnectionManager.connection.CreateCommand();
				command.CommandText = query;
				command.ExecuteNonQuery();
				command.Dispose();
			} catch (Exception ex) {
				Trace.WriteLine(ex);
			}
		}

		private DataTable Select(string query)
		{
			var resultTable = new DataTable();

			try {
                var adapter = new SqlCeDataAdapter(query, DBConnectionManager.connection);
				adapter.Fill(resultTable); // Fill is slow, but straightforward.
			} catch (Exception ex) {
				Trace.WriteLine(ex);
			}

			return resultTable;
		}
	}
}