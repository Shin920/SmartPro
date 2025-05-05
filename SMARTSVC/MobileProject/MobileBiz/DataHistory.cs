using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMART;
using System.Data;
using System.Collections;

namespace MobileBiz
{
    public class DataHistory : DBBase
    {
        public DataHistory(string strClientID)
        {
            connection.ConnectDB(strClientID);
        }

        //     public ArrayList LoadDataHistory(object[] GDObj, string strFormID)
        //     {
        //         // Global Data
        //         GData GD = new GData(GDObj);

        //         string strQuery = @"
        //select FieldName from FormKeyList
        //where LangID = " + CFL.Q(GD.LangID) + " and FormID = " + CFL.Q(strFormID);

        //         DataTableReader dr;

        //         try
        //         {
        //             dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
        //         }
        //         catch (Exception e)
        //         {
        //             return CFL.EncodeData("01", CFL.RS("01", this, GD.LangID), e);
        //         }

        //         return CFL.EncodeData("00", "", dr, null);
        //     }

        //        public DataSet Grid(object[] GDObj, string strWhereClause)
        //        {
        //            // Global Data
        //            GData GD = new GData(GDObj);
        //            string strQuery = "";

        //            strQuery += @"
        //Select Case A.ManipulateType when 'I' then 'Insert'
        //                             when 'U' then 'Update'
        //                             when 'D' then 'Delete' 
        //       End As ManipulateType
        //     , A.ManipulateUser +N'(' + B.UserName + N')' as ManipulateUser
        //     , A.ManipulateTime
        //     , A.ManipulateHost
        //From DataHistory A, xErpUser B";

        //            if (strWhereClause != null && 0 != strWhereClause.Length)
        //            {
        //                strQuery += @"
        //Where " + strWhereClause;
        //                strQuery += @" 
        //  And A.ManipulateUser = B.UserID ";
        //            }
        //            else
        //                strQuery += @" 
        //Where A.ManipulateUser = B.UserID ";

        //            strQuery += @" 
        //Order by SerNo";

        //            DataSet ds = new DataSet();

        //            try
        //            {
        //                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
        //            }
        //            catch (Exception e)
        //            {
        //                return ds;
        //            }
        //            return ds;
        //        }

        //     public ArrayList Grid(bool bStartQuery, string strWhereClause, string strCondition, object[] Conditions, object[] GDObj)
        //     {
        //         // Global Data
        //         GData GD = new GData(GDObj);

        //         string strErrorCode = "00", strErrorMsg = "";

        //         if (!bStartQuery)
        //             return CFL.EncodeData(strErrorCode, strErrorMsg, null);

        //         string strQuery = @"
        //select case A.ManipulateType
        //when 'I' then 'Insert'
        //when 'U' then 'Update'
        //when 'D' then 'Delete'
        //end, 
        //A.ManipulateUser +N'(' + B.UserName + N')' as ManipulateUser, A.ManipulateTime, A.ManipulateHost
        //from DataHistory A, xErpUser B";

        //         if (strWhereClause != null && 0 != strWhereClause.Length)
        //         {
        //             strQuery += " where " + strWhereClause;
        //             strQuery += " and A.ManipulateUser = B.UserID ";
        //         }
        //         else
        //             strQuery += " where A.ManipulateUser = B.UserID ";

        //         strQuery += " order by SerNo";

        //         DataTableReader dr;
        //         try
        //         {
        //             dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
        //         }
        //         catch (Exception e)
        //         {
        //             return CFL.EncodeData("01", CFL.RS("01", this, GD.LangID), e);
        //         }

        //         return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        //     }

        public CFL GetCurTime(string strClientID)
        {
            ArrayList alTemp;
            CFL data;
            DataSet ds = new DataSet();
            /*
			// Db Connect
			if ( !m_bInTrans )
			{
				if ( "00" != alData[0].ToString() )
				{
					// Errors that can continue
					alTemp = new ArrayList();
					alTemp.Add ( alData[0] );
					alTemp.Add ( alData[1] );
					alTemp.Add ( 1 );
					alTemp.Add ( 1 );
					alTemp.Add ( CFL.GetSysDateTime() );
					return alTemp;
				}
			}
            */
            string strErrorCode = "00", strErrorMsg = "";

            // Load EmpMaster Information
            string strQuery = @"select Convert ( char(19), getdate(), 120 ) as CurDateTime";

            try
            {
                ds = SQLDataSet(strQuery);

            }
            catch (Exception ex)
            {
                ds.Tables.Add();
                ds.Tables[0].Columns.Add("CurDateTime");

                DataRow dr = ds.Tables[0].NewRow();
                dr["CurDateTime"] = GetSysDateTime();
                ds.Tables[0].Rows.Add(dr);
                data = EncodeData(ds, 0, "");
                return data;
            }


            DataSet ds2 = new DataSet();
            ds2.Tables.Add();
            ds2.Tables[0].Columns.Add("CurDateTime");

            DataRow dr2 = ds2.Tables[0].NewRow();
            string strTemp = ds.Tables[0].Rows[0]["CurDateTime"].ToString();
            dr2["CurDateTime"] = strTemp.Substring(0, 4) + strTemp.Substring(5, 2) + strTemp.Substring(8, 2) + strTemp.Substring(11, 2) + strTemp.Substring(14, 2) + strTemp.Substring(17, 2);
            ds2.Tables[0].Rows.Add(dr2);
            data = EncodeData(ds2, 0, "");

            //DataTableReader dr;
            //try
            //{
            //    dr = CFL.ExecuteDataTableReader(strClientID, strQuery, CommandType.Text);
            //}
            //catch (Exception e)
            //{
            //    // Errors that can continue
            //    strErrorCode = "01";
            //    strErrorMsg = "Can't get the Time of Database Server. Use the Time of Application Server Instead.";
            //    alTemp = new ArrayList();
            //    alTemp.Add(strErrorCode);
            //    alTemp.Add(strErrorMsg);
            //    alTemp.Add(1);
            //    alTemp.Add(1);
            //    alTemp.Add(CFL.GetSysDateTime());
            //    return alTemp;
            //}

            // manipulate date format
            //alTemp = CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            //string strTemp = alTemp[4].ToString();
            //alTemp[4] = strTemp.Substring(0, 4) + strTemp.Substring(5, 2) + strTemp.Substring(8, 2) + strTemp.Substring(11, 2) + strTemp.Substring(14, 2) + strTemp.Substring(17, 2);

            return data;
        }


        public CFL SaveDataHistory(CFLUserInfo G, string strFormID, ArrayList alKeyValue, Type enManipulateType)
        {

            string strQuery = "";
            CFL data = new CFL();
            DataSet ds = new DataSet();

            if (strFormID == null || strFormID.Trim() == "")
            {
                //if (m_bInTrans)
                //    m_tr.Rollback();
                //return CFL.EncodeData("01", CFL.RS("02", this, GD.LangID), null);
            }

            if (alKeyValue == null || alKeyValue.Count == 0)
            {
                //if (m_bInTrans)
                //    m_tr.Rollback();
                //return CFL.EncodeData("02", CFL.RS("03", this, GD.LangID), null);
            }

            string strKeyValue = EncodeKeyValues(alKeyValue);

            string strManipulateType = "";
            switch (enManipulateType)
            {
                case Type.Insert:
                    strManipulateType = "I";
                    break;
                case Type.Update:
                    strManipulateType = "U";
                    break;
                case Type.Delete:
                    strManipulateType = "D";
                    break;
                default:
                    data = EncodeData(ds, -1, "DataHistory 저장 중 오류가 발생 하였습니다.");
                    return data;
            }

            CFL alTime = GetCurTime(G.ClientID);
            if (alTime.errCode.ToString() != "0")
            {
                //if (m_bInTrans)
                //    m_tr.Rollback();
                //return CFL.EncodeData("05", alTime[1].ToString(), null);
                data = EncodeData(ds, -1, "DataHistory 저장 중 오류가 발생 하였습니다.");
                return data;
            }
            string strManipulateTime = alTime.ds.Tables[0].Rows[0][0].ToString();

            strQuery = @"
			select count(*)+1 As Cnt from DataHistory where FormID = " + CFL.Q(strFormID) + " and KeyValue = " + CFL.Q(strKeyValue);

            //if (!m_bInTrans)
            //    CFL.BeginTran(GD, ref m_tr, ref m_db);

            //DataTableReader dr;
            try
            {
                //dr = CFL.ExecuteDataTableReaderTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db);
                ds = SQLDataSet(strQuery);
            }
            catch (Exception e)
            {
                //m_tr.Rollback();
                //return CFL.EncodeData("06", CFL.RS("11", this, GD.LangID), e);
                data = EncodeData(ds, -1, "DataHistory 저장 중 오류가 발생 하였습니다." + e.Message);
            }
            //dr.Read();

            string strCnt = ds.Tables[0].Rows[0][0].ToString();

            strQuery = @"
			insert into DataHistory
			values ( " + CFL.Q(strFormID) + ", " + CFL.Q(strKeyValue) + ", " + strCnt + ", " + CFL.Q(strManipulateType) + ", " + CFL.Q(G.UserID)
            + ", " + CFL.Q(strManipulateTime) + ", " + CFL.Q(G.UserIP) + " )";

            //dr.Close();

            try
            {
                //CFL.ExecuteTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db);
                ds = SQLDataSet(strQuery);
            }
            catch (Exception e)
            {
                //m_tr.Rollback();
                //return CFL.EncodeData("04", CFL.RS("05", this, GD.LangID), e);
                data = EncodeData(ds, -1, "DataHistory 저장 중 오류가 발생 하였습니다." + e.Message);
            }

            //// Transaction Commit
            //if (!m_bInTrans)
            //    m_tr.Commit();

            //return CFL.EncodeData("00", null, null);
            data = EncodeData(ds, 0, "");
            return data;
        }


        //public ArrayList DynamicGrid(bool bStartQuery, string strWhereClause, string strCondition, object[] Conditions, object[] GDObj)
        //{
        //	// Global Data
        //	GData GD = new GData(GDObj);

        //	SMART.Advanced.GridViewColumn col = new SMART.Advanced.GridViewColumn();

        //	// Generate Column Information. Insert when FixedColumn = False	
        //	DataTableReader dr = null;

        //	ArrayList alColumns = new ArrayList();
        //	ArrayList alWidth = new ArrayList();

        //	int iTotWidth = 0;
        //	int iLastWidth = 420;
        //	int iCount = 0;

        //	string strQuery = "";

        //	// KeyValues
        //	if (!bStartQuery)
        //	{
        //		for (int i = 1; i <= 3; i++)
        //		{
        //			col.Name = "KeyValue" + i;
        //			col.Text = CFL.RS("06", this, GD.LangID) + i;
        //			col.Width = 140;
        //			col.ExportData(alColumns);
        //			col = new SMART.Advanced.GridViewColumn();

        //			//  alColumns.Add ( "AllowSorting = True|" );
        //		}
        //	}
        //	else
        //	{
        //		try
        //		{
        //			strQuery = @"
        //			select Sum(MaxLength), Count(MaxLength)
        //			from FormKeyList
        //			where FormID = " + CFL.Q(strCondition) + " and LangID = " + CFL.Q(GD.LangID);

        //			dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
        //			dr.Read();

        //			iTotWidth = dr.GetInt32(0);
        //			iCount = dr.GetInt32(1);

        //			dr.Close();

        //			strQuery = @"
        //			select SerNo, FieldName, MaxLength
        //			from FormKeyList
        //			where FormID = " + CFL.Q(strCondition) + " and LangID = " + CFL.Q(GD.LangID);

        //			dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

        //			while (dr.Read())
        //			{
        //				col.Name = "KeyValue" + dr[0].ToString().Trim();
        //				col.Text = dr[1].ToString().Trim();
        //				col.Width = (short)((iCount == Convert.ToInt32(dr[0].ToString().Trim())) ? iLastWidth : (int)Math.Floor(420 * Convert.ToDouble(dr[2].ToString().Trim()) / iTotWidth));
        //				col.ExportData(alColumns);
        //				col = new SMART.Advanced.GridViewColumn();

        //				/*						
        //				alColumns.Add ( "KeyValue" + dr[0].ToString().Trim() );
        //				alColumns.Add ( dr[1].ToString().Trim() );
        //				alColumns.Add ( ( iCount == Convert.ToInt32 (dr[0].ToString().Trim() ) ) ? iLastWidth : (int)Math.Floor ( 420 * Convert.ToDouble ( dr[2].ToString().Trim() ) / iTotWidth ) );
        //				alColumns.Add ( "AllowSorting = True|" );
        //				alColumns.Add ( "" );
        //				alColumns.Add ( "" );
        //				alColumns.Add ( "" );
        //				alColumns.Add ( "" );
        //                      */

        //				iLastWidth -= (int)Math.Floor(420 * Convert.ToDouble(dr[2].ToString().Trim()) / iTotWidth);
        //			}

        //			dr.Close();
        //		}
        //		catch (Exception e)
        //		{
        //			dr.Close();

        //			for (int i = 1; i <= 3; i++)
        //			{
        //				col.Name = "KeyValue" + i;
        //				col.Text = CFL.RS("06", this, GD.LangID) + i;
        //				col.Width = 140;
        //				col.ExportData(alColumns);
        //				col = new SMART.Advanced.GridViewColumn();

        //				/*
        //				alColumns.Add ( "KeyValue" + i );
        //				alColumns.Add ( CFL.RS ( "06", this,  GD.LangID ) + i );
        //				alColumns.Add ( 140 );
        //				alColumns.Add ( "AllowSorting = True|" );
        //				alColumns.Add ( "" );
        //				alColumns.Add ( "" );
        //				alColumns.Add ( "" );
        //				alColumns.Add ( "" );
        //                      */
        //			}
        //		}
        //	}

        //	/*
        //	// SerNo		
        //	alColumns.Add ( "SerNo" );
        //	alColumns.Add ( "No." );
        //	alColumns.Add ( 50 );
        //	alColumns.Add ( "AllowSorting = True|" );
        //	alColumns.Add ( "" );
        //	alColumns.Add ( "" );
        //	alColumns.Add ( "" );
        //	alColumns.Add ( "" );
        //          */

        //	col.Name = "SerNo";
        //	col.Text = "No.";
        //	col.Width = 50;
        //	col.ExportData(alColumns);
        //	col = new SMART.Advanced.GridViewColumn();


        //	/*
        //	// ManipulateType			
        //	alColumns.Add ( "ManipulateType" );
        //	alColumns.Add ( CFL.RS ( "07", this,  GD.LangID ) );
        //	alColumns.Add ( 80 );
        //	alColumns.Add ("AllowSorting = True|" );
        //	alColumns.Add ( "" );
        //	alColumns.Add ( "" );
        //	alColumns.Add ( "" );
        //	alColumns.Add ( "" );
        //          */

        //	col.Name = "ManipulateType";
        //	col.Text = CFL.RS("07", this, GD.LangID);
        //	col.Width = 80;
        //	col.ExportData(alColumns);
        //	col = new SMART.Advanced.GridViewColumn();


        //	/*
        //	// ManipulateUser		
        //	alColumns.Add ( "ManipulateUser" );
        //	alColumns.Add ( CFL.RS ( "08", this,  GD.LangID ) );
        //	alColumns.Add ( 80 );
        //	alColumns.Add ( "AllowSorting = True|" );
        //	alColumns.Add ( "" );
        //	alColumns.Add ( "" );
        //	alColumns.Add ( "" );
        //	alColumns.Add ( "" );
        //          */


        //	col.Name = "ManipulateUser";
        //	col.Text = CFL.RS("08", this, GD.LangID);
        //	col.Width = 120;
        //	col.ExportData(alColumns);
        //	col = new SMART.Advanced.GridViewColumn();

        //	/*
        //	// ManipulateTime
        //	string strTemp = "";
        //	CFL.SetEncodedProperty ( "Type", "Date", ref strTemp );
        //	CFL.SetEncodedProperty ( "DateType", "YYYYMMDDHHMMSS", ref strTemp );
        //	CFL.SetEncodedProperty ( "AllowSorting", "True",ref strTemp );

        //	alColumns.Add ( "ManipulateTime" );
        //	alColumns.Add ( CFL.RS ( "09", this,  GD.LangID ) );
        //	alColumns.Add ( 130 );
        //	alColumns.Add ( strTemp );
        //	alColumns.Add ( "" );
        //	alColumns.Add ( "" );
        //	alColumns.Add ( "" );
        //	alColumns.Add ( "" );
        //          */

        //	col.Name = "ManipulateTime";
        //	col.Text = CFL.RS("09", this, GD.LangID);
        //	col.Width = 130;
        //	col.DataType = SMART.Advanced.eGridDataType.DateTime;
        //	col.DateFormat = SMART.Advanced.eDateFormatType.YYYYMMDD;
        //	col.ExportData(alColumns);
        //	col = new SMART.Advanced.GridViewColumn();


        //	/*
        //	// ManipulateHost			
        //	alColumns.Add ( "ManipulateHost" );
        //	alColumns.Add ( CFL.RS ( "10", this,  GD.LangID ) );
        //	alColumns.Add ( 100 );
        //	alColumns.Add ( "AllowSorting = True|" );
        //	alColumns.Add ( "" );
        //	alColumns.Add ( "" );
        //	alColumns.Add ( "" );
        //	alColumns.Add ( "" );
        //          */

        //	col.Name = "ManipulateHost";
        //	col.Text = CFL.RS("10", this, GD.LangID);
        //	col.Width = 110;
        //	col.ExportData(alColumns);
        //	col = new SMART.Advanced.GridViewColumn();

        //	if (!bStartQuery)
        //		return CFL.EncodeData("00", "", alColumns, null);

        //	strQuery = @"
        //	select KeyValue, SerNo, 
        //	case ManipulateType
        //	when 'I' then 'Insert'
        //	when 'U' then 'Update'
        //	when 'D' then 'Delete'
        //	end,
        //	ManipulateUser , ManipulateTime, ManipulateHost  
        //	from DataHistory ";

        //	if (strWhereClause != null && 0 != strWhereClause.Length)
        //	{
        //		strQuery += " where " + strWhereClause;
        //	}

        //	strQuery += " order by ManipulateTime Desc, KeyValue, SerNo";


        //	try
        //	{
        //		dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
        //	}
        //	catch (Exception e)
        //	{
        //		return CFL.EncodeData("03", null, alColumns, e);
        //	}

        //	return EncodeHistoryData("00", "", dr, alColumns, null);
        //}



        private ArrayList EncodeHistoryData(string strErrorCode, string strErrorMsg, DataTableReader dr, ArrayList alColumns, Exception e)
        {
            ArrayList alTemp = new ArrayList();

            // ErrorCode and Error Msg

            // when error occurred
            if (strErrorCode != "00" || dr.IsClosed)
            {
                alTemp.Add(strErrorCode);
                // db ErrorMsg only in Debug Mode
#if (DEBUG)
                if (null != e)
                    alTemp.Add(strErrorMsg + "\n" + e.ToString());
                else
                    alTemp.Add(strErrorMsg);
#else
					alTemp.Add ( strErrorMsg );
#endif
                alTemp.Add(0);
                alTemp.Add(0);
                return alTemp;
            }
            else
            {
                alTemp.Add(strErrorCode);
                alTemp.Add(strErrorMsg);
            }

            // FieldCnt
            alTemp.Add((int)(alColumns.Count / 8));
            // RecordCnt - this time 0 and set real value later
            alTemp.Add(0);

            int i, j;
            for (i = 0; i < alColumns.Count; i++)
            {
                alTemp.Add(alColumns[i]);
            }

            char[] chSpace = { ' ' };

            // insert data into ArrayList
            if (!dr.Read())
            {
                // set error Message
                alTemp[1] = "There's no such data";
                alTemp[3] = 0;
                return alTemp;
            }

            bool bRowExists = true;
            for (i = 0; bRowExists; i++)
            {
                ArrayList alSource = Separator(dr.GetValue(0).ToString());

                foreach (string strTemp in alSource)
                {
                    alTemp.Add(strTemp);
                }
                for (j = 1; j < dr.FieldCount; j++)
                    alTemp.Add(F(dr.GetValue(j)));
                bRowExists = dr.Read();
            }
            dr.Close();

            // set real data Value
            alTemp[3] = i;
            return alTemp;
        }

        private object F(object obj)
        {
            if (null == obj || "System.DBNull" == obj.GetType().ToString())
                return "";
            else
                return T(obj.ToString());
        }

        private string T(string str)
        {
            char[] chSpace = { ' ' };
            if (str == null || str == "")
                return str;
            else
                return str.TrimEnd(chSpace);
        }

        private ArrayList Separator(string strSource)
        {
            ArrayList alSource = new ArrayList();
            int Index = 0;

            while (true)
            {
                if (strSource.IndexOf("#$", Index) < 0)
                {
                    alSource.Add(strSource.Substring(Index));
                    return alSource;
                }
                else
                {
                    alSource.Add(strSource.Substring(Index, strSource.IndexOf("#$", Index) - Index));
                    Index = strSource.IndexOf("#$", Index) + 2;
                }
            }
        }

        private string EncodeKeyValues(ArrayList alKeyValues)
        {
            string strTemp = "";

            for (int i = 0; i < alKeyValues.Count; i++)
            {
                strTemp += alKeyValues[i];

                if (i != alKeyValues.Count - 1)
                {
                    strTemp += "#$";
                }
            }

            return strTemp;
        }

        public enum Type { Insert, Update, Delete };

        public static string GetSysDateTime()
        {
            // Year
            string strTemp = DateTime.Now.Year.ToString();
            strTemp = "000" + strTemp;
            strTemp = strTemp.Substring(strTemp.Length - 4, 4);
            string strResult = strTemp;
            // Month
            strTemp = DateTime.Now.Month.ToString();
            strTemp = "0" + strTemp;
            strTemp = strTemp.Substring(strTemp.Length - 2, 2);
            strResult += strTemp;
            // Date
            strTemp = DateTime.Now.Day.ToString();
            strTemp = "0" + strTemp;
            strTemp = strTemp.Substring(strTemp.Length - 2, 2);
            strResult += strTemp;
            // Hour
            strTemp = DateTime.Now.Hour.ToString();
            strTemp = "0" + strTemp;
            strTemp = strTemp.Substring(strTemp.Length - 2, 2);
            strResult += strTemp;
            // Minute
            strTemp = DateTime.Now.Minute.ToString();
            strTemp = "0" + strTemp;
            strTemp = strTemp.Substring(strTemp.Length - 2, 2);
            strResult += strTemp;
            // Second
            strTemp = DateTime.Now.Second.ToString();
            strTemp = "0" + strTemp;
            strTemp = strTemp.Substring(strTemp.Length - 2, 2);
            strResult += strTemp;

            return strResult;
        }
    }
}
