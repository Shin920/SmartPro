using System;
using System.Collections;
using System.Data.OleDb;
using System.IO;

namespace SMART
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class ClientCore
	{
		public ClientCore()
		{

		}

		public ArrayList Ddl()
		{
			string[] sarIDList = ExtractClientIDList();
			ArrayList alClientList = new ArrayList();
			foreach (string strID in sarIDList)
				alClientList.Add(GetClientInfo(strID));

			// Make Ddl
			ArrayList alData = new ArrayList();
			alData.Add("00");
			alData.Add("");
			alData.Add(2);
			alData.Add(alClientList.Count);

			for (int i = 0; i < alClientList.Count; i++)
			{
				alData.Add("ID = " + ((ClientInfo)alClientList[i]).ID + "|");
				alData.Add(((ClientInfo)alClientList[i]).Name);
			}

			return alData;
		}

		public struct ClientInfo
		{
			public ClientInfo(string strID, string strName, string strDBServer, string strUserID, string strPassword, string strDataBase, string strDBKind, bool bUse, string strDomainName)
			{
				ID = strID;
				Name = strName;
				DBServer = strDBServer;
				UserID = strUserID;
				Password = strPassword;
				DataBase = strDataBase;
				DBKind = strDBKind;
				Use = bUse;
				DomainName = strDomainName;
			}

			public string ID;
			public string Name;
			public string DBServer;
			public string UserID;
			public string Password;
			public string DataBase;
			public string DBKind;
			public bool Use;
			public string DomainName;
		}

		public string[] ExtractClientIDList()
		{
			TextReader txtReader = null;
			try
			{
				txtReader = File.OpenText(CFL.GetIniPath() + "\\" + CFL.formRoot + "Clients.ini");
			}
			catch
			{
				return new string[0];
			}

			string strTemp;
			ArrayList alTemp = new ArrayList();
			bool bContinue = true;
			while (bContinue)
			{
				do
				{
					strTemp = txtReader.ReadLine();
				} while (-1 != txtReader.Peek() && !strTemp.StartsWith("["));

				if (!strTemp.StartsWith("["))
				{
					bContinue = false;
				}
				else
				{
					alTemp.Add(strTemp.Substring(1, strTemp.IndexOf("]") - 1));
				}
			}

			txtReader.Close();

			string[] sarResult = new string[alTemp.Count];
			for (int i = 0; i < alTemp.Count; i++)
				sarResult[i] = alTemp[i].ToString();
			return sarResult;
		}

		public static ClientInfo GetClientInfo(string strID)
		{
			TextReader txtReader = null;
			try
			{
				txtReader = File.OpenText(CFL.GetIniPath() + "\\" + CFL.formRoot + "Clients.ini");
			}
			catch
			{
				throw (new Exception(CFL.GetIniPath()));
			}

			string strTemp;
			do
			{
				strTemp = txtReader.ReadLine();
			} while (-1 != txtReader.Peek() && !strTemp.StartsWith("[" + strID));

			ClientInfo cliInfo = new ClientInfo();
			if (strTemp.StartsWith("[" + strID))
			{
				// ID
				cliInfo.ID = strID;
				// Name
				strTemp = txtReader.ReadLine();
				cliInfo.Name = strTemp.Substring(7, strTemp.Length - 7);
				// DBServer
				strTemp = txtReader.ReadLine();
				cliInfo.DBServer = strTemp.Substring(11, strTemp.Length - 11);
				// UserID
				strTemp = txtReader.ReadLine();
				cliInfo.UserID = strTemp.Substring(9, strTemp.Length - 9);
				// Password
				strTemp = txtReader.ReadLine();
				cliInfo.Password = strTemp.Substring(11, strTemp.Length - 11);
				// DataBase
				strTemp = txtReader.ReadLine();
				cliInfo.DataBase = strTemp.Substring(11, strTemp.Length - 11);
				// DBKind
				strTemp = txtReader.ReadLine();
				cliInfo.DBKind = strTemp.Substring(9, strTemp.Length - 9);
				// Use
				strTemp = txtReader.ReadLine();
				cliInfo.Use = strTemp.Substring(6, strTemp.Length - 6) == "Y";
				//DomainName
				strTemp = txtReader.ReadLine();
				cliInfo.DomainName = strTemp.Substring(13, strTemp.Length - 13);
			}

			txtReader.Close();
			return cliInfo;
		}

		public static string GetConnectionString(GData GD)
		{
			// Security Check
			if (null == GD.UserID || "" == GD.UserID)
				return "E-Login first, please.";

			return GetConnectionString(GD.ClientID);
		}

		public static string GetConnectionString(GObj G)
		{
			// Security Check
			if (null == G.UserID || "" == G.UserID)
				return "E-Login first, please.";

			return GetConnectionString(G.ClientID);
		}

		public static string GetConnectionString(string strClientID)
		{

			ClientInfo cliInfo = GetClientInfo(strClientID);
			if (null == cliInfo.ID || "" == cliInfo.ID)
			{
				return "E-Incorrent Client Data!";
			}

			// return "Provider=" + cliInfo.DBKind + ";Data Source=" + cliInfo.DBServer + ";User ID=" + cliInfo.UserID + ";PassWord=" + cliInfo.Password + ";Initial Catalog=" + cliInfo.DataBase + ";Connect Timeout=3600";
			return "Data Source=" + cliInfo.DBServer + ";User ID=" + cliInfo.UserID + ";PassWord=" + cliInfo.Password + ";Initial Catalog=" + cliInfo.DataBase + ";Connect Timeout=3600";
		}

		public static ArrayList SetDB(GData GD, ref OleDbConnection db)
		{
			// Security Check
			if (null == GD.UserID || "" == GD.UserID)
				return CFL.EncodeData("01", "Login first, please.", null);

			return SetDB(GD.ClientID, ref db);
		}

		public static ArrayList SetDB(string strClientID, ref OleDbConnection db)
		{
			/*
						if ( null == strClientID || "" == strClientID )
							strClientID = "Default";
			*/

			ClientInfo cliInfo = GetClientInfo(strClientID);
			if (null == cliInfo.ID || "" == cliInfo.ID)
			{
				return CFL.EncodeData("01", "Incorrent Client Data!", null);
			}

			db = new OleDbConnection("Provider=" + cliInfo.DBKind + ";Data Source=" + cliInfo.DBServer + ";User ID=" + cliInfo.UserID + ";PassWord=" + cliInfo.Password + ";Initial Catalog=" + cliInfo.DataBase + ";Connect Timeout=3600");
			try
			{
				db.Open();
			}
			catch (Exception e)
			{
				return CFL.EncodeData("01", "Cannot connect Database.", e);
			}

			return CFL.EncodeData("00", "", null);
		}

		public static bool SetDB(GData GD, bool bInTrans, ref OleDbConnection db, ref ArrayList alResult)
		{
			if (bInTrans)
			{
				alResult = CFL.EncodeData("00", "", null);
				return true;
			}

			alResult = SetDB(GD, ref db);
			if (alResult[0].ToString() != "00")
				return false;
			return true;
		}

		public string GetDomainName(string strClientID)
		{
			ClientInfo cliInfo = GetClientInfo(strClientID);

			return cliInfo.DomainName;
		}

	}
}
