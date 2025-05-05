using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Resources;
using Microsoft.Win32;
using System.Globalization;
using System.Text;
using System.Configuration;
using System.Drawing.Printing;
using DevExpress.Web;
using System.Collections.Generic;
using DevExpress.XtraPrintingLinks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Net.Mime;

namespace SMART
{
    public class CFL
    {

        #region Constants

        private const int c_ColInfoCnt = 8;
        private const string c_MobileComboSeperator = "\t";
        private static string c_ResourceRoot = System.Configuration.ConfigurationSettings.AppSettings["ResourceRoot"];
        public const string c_CookieNameClientID = "xErpClientID";
        public const string c_CookieNameSiteCode = "xErpSiteCode";
        public const string c_CookieNameLangID = "xErpLangID";
        public const int c_CookieExpiryDay = 30;

        public static string formRoot = "SmartPro";

        #endregion

        #region EncodeData Overloadings

        public static ArrayList ObjectsToArrayList(object[] os)
        {
            ArrayList arrayList = new ArrayList();
            foreach (object o in os)
                arrayList.Add(o);
            return arrayList;
        }

        public static object[] ArrayListToObject(ArrayList ar)
        {
            object[] o = new object[ar.Count];

            for (int i = 0; i < ar.Count; i++)
                o[i] = ar[i];

            return o;
        }

        public static string GridExceptionCode = "*!EXCEPTION!*";

        public static DataSet EncodeException(string message)
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            dataSet.Tables.Add(dataTable);

            dataTable.Columns.Add("code");
            dataTable.Columns.Add("message");

            DataRow newRow = dataTable.NewRow();
            newRow["code"] = CFL.GridExceptionCode;
            newRow["message"] = message;

            dataTable.Rows.Add(newRow);

            return dataSet;
        }

        public static ArrayList DynamicEncodeData(DataSet ds, ArrayList column)
        {
            ArrayList ar = new ArrayList();
            ar.Add("00");
            ar.Add("");
            ar.Add(ds.Tables[0].Columns.Count);
            ar.Add(ds.Tables[0].Rows.Count);

            for (int i = 0; i < column.Count; i++)
                ar.Add(column[i]);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    ar.Add(ds.Tables[0].Rows[i][j].ToString());

            return ar;
        }

        public static ArrayList DynamicEncodeException(string message)
        {
            ArrayList arList = new ArrayList();
            arList.Add("01");
            arList.Add(message);
            arList.Add(0);
            arList.Add(0);

            return arList;
        }


        /// <summary>
        /// Encode OleDbDataReader without column information
        /// use when 
        /// </summary>
        public static ArrayList EncodeData(string strErrorCode, string strErrorMsg, SqlDataReader dr, Exception e)
        {
            TimeChallenge("EncodeData Start");

            ArrayList alTemp = new ArrayList();

            // ErorCode and Error Msg

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
                alTemp.Add(strErrorMsg);
#endif
                alTemp.Add(0);
                alTemp.Add(0);
                return alTemp;
            }
            else
            {
                alTemp.Add("00");
                alTemp.Add("");
            }

            // FieldCnt
            alTemp.Add(dr.FieldCount);
            // RecordCnt - this time 0 and set real value later
            alTemp.Add(0);

            char[] chSpace = { ' ' };

            // insert data into ArrayList
            int i, j;
            if (!dr.Read())
            {
                // set error Message
                alTemp[1] = "There's No Such Data.";
                alTemp[3] = 0;
                return alTemp;
            }

            bool bRowExists = true;
            for (i = 0; bRowExists; i++)
            {
                for (j = 0; j < dr.FieldCount; j++)
                    alTemp.Add(F(dr.GetValue(j)));
                bRowExists = dr.Read();
            }
            dr.Close();

            // set real data Value
            alTemp[3] = i;

            TimeChallenge("EncodeData End");

            return alTemp;
        }

        /// <summary>
        /// Encode OleDbDataReader without column information
        /// use when 
        /// </summary>
        public static ArrayList EncodeData(string strErrorCode, string strErrorMsg, SqlDataReader dr, Exception e, SqlTransaction tr)
        {
            // if error then trnasaction rollback
            if ("00" != strErrorCode)
                tr.Rollback();

            ArrayList alTemp = new ArrayList();

            // ErorCode and Error Msg

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
                alTemp.Add(strErrorMsg);
#endif
                alTemp.Add(0);
                alTemp.Add(0);
                return alTemp;
            }
            else
            {
                alTemp.Add("00");
                alTemp.Add("");
            }

            // FieldCnt
            alTemp.Add(dr.FieldCount);
            // RecordCnt - this time 0 and set real value later
            alTemp.Add(0);

            char[] chSpace = { ' ' };

            // insert data into ArrayList
            int i, j;
            if (!dr.Read())
            {
                // set error Message
                alTemp[1] = "There's No Such Data.";
                alTemp[3] = 0;
                return alTemp;
            }

            bool bRowExists = true;
            for (i = 0; bRowExists; i++)
            {
                for (j = 0; j < dr.FieldCount; j++)
                    alTemp.Add(F(dr.GetValue(j)));
                bRowExists = dr.Read();
            }
            dr.Close();

            // if no error then transaction commit
            if ("00" == strErrorCode)
                tr.Commit();

            // set real data Value
            alTemp[3] = i;
            return alTemp;
        }

        /// <summary>
        /// Encode OleDbDataReader with column information
        /// </summary>
        public static ArrayList EncodeData(string strErrorCode, string strErrorMsg, SqlDataReader dr, ArrayList alColumns, Exception e)
        {
            TimeChallenge("EncodeData Start");

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
                alTemp.Add(strErrorMsg);
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
            alTemp.Add(dr.FieldCount);
            // RecordCnt - this time 0 and set real value later
            alTemp.Add(0);

            int i, j;
            for (i = 0; i < alColumns.Count; i++)
                alTemp.Add(alColumns[i]);

            // insert data into ArrayList
            if (!dr.Read())
            {
                // set error Message
                alTemp[1] = "There's No Such Data.";
                alTemp[3] = 0;
                return alTemp;
            }

            bool bRowExists = true;
            for (i = 0; bRowExists; i++)
            {
                for (j = 0; j < dr.FieldCount; j++)
                    alTemp.Add(F(dr.GetValue(j)));
                bRowExists = dr.Read();
            }
            dr.Close();

            // set real data Value
            alTemp[3] = i;

            TimeChallenge("EncodeData End ( Count - " + alTemp.Count.ToString() + " )");

            return alTemp;
        }

        /// <summary>
        /// Encode OleDbDataReader with column information
        /// </summary>
        public static ArrayList EncodeData(string strErrorCode, string strErrorMsg, SqlDataReader dr, ArrayList alColumns, Exception e, SqlTransaction tr)
        {
            // if error then trnasaction rollback
            if ("00" != strErrorCode)
                tr.Rollback();

            ArrayList alTemp = new ArrayList();

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
                alTemp.Add(strErrorMsg);
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
            alTemp.Add(dr.FieldCount);
            // RecordCnt - this time 0 and set real value later
            alTemp.Add(0);

            int i, j;
            for (i = 0; i < alColumns.Count; i++)
                alTemp.Add(alColumns[i]);

            char[] chSpace = { ' ' };

            // insert data into ArrayList
            if (!dr.Read())
            {
                // set error Message
                alTemp[1] = "There's No Such Data.";
                alTemp[3] = 0;
                return alTemp;
            }

            bool bRowExists = true;
            for (i = 0; bRowExists; i++)
            {
                for (j = 0; j < dr.FieldCount; j++)
                    alTemp.Add(F(dr.GetValue(j)));
                bRowExists = dr.Read();
            }
            dr.Close();

            // if no error then transaction commit
            if ("00" == strErrorCode)
                tr.Commit();

            // set real data Value
            alTemp[3] = i;
            return alTemp;
        }

        /// <summary>
        /// Encode only Column information
        /// </summary>
        public static ArrayList EncodeData(string strErrorCode, string strErrorMsg, ArrayList alColumns, Exception e)
        {
            ArrayList alTemp = new ArrayList();

            alTemp.Add(strErrorCode);
            // db ErrorMsg only in Debug Mode
#if (DEBUG)
            if (null != e)
                alTemp.Add(strErrorMsg + "\n" + e.ToString());
            else
                alTemp.Add(strErrorMsg);
#else
            alTemp.Add(strErrorMsg);
#endif

            // FieldCnt
            alTemp.Add(alColumns.Count / c_ColInfoCnt);
            // RecordCnt - this time 0 and set real value later
            alTemp.Add(0);

            for (int i = 0; i < alColumns.Count; i++)
            {
                alTemp.Add(alColumns[i]);
            }

            return alTemp;
        }

        /// <summary>
        /// Encode only Column information
        /// </summary>
        public static ArrayList EncodeData(string strErrorCode, string strErrorMsg, ArrayList alColumns, Exception e, SqlTransaction tr)
        {
            // if error then trnasaction rollback
            if ("00" != strErrorCode)
                tr.Rollback();

            ArrayList alTemp = new ArrayList();

            alTemp.Add(strErrorCode);
            // db ErrorMsg only in Debug Mode
#if (DEBUG)
            if (null != e)
                alTemp.Add(strErrorMsg + "\n" + e.ToString());
            else
                alTemp.Add(strErrorMsg);
#else
            alTemp.Add(strErrorMsg);
#endif

            // FieldCnt
            alTemp.Add(alColumns.Count / c_ColInfoCnt);
            // RecordCnt - this time 0 and set real value later
            alTemp.Add(0);

            for (int i = 0; i < alColumns.Count; i++)
            {
                alTemp.Add(alColumns[i]);
            }

            // if no error then transaction commit
            if ("00" == strErrorCode)
                tr.Commit();

            return alTemp;
        }


        public static ArrayList EncodeData(string strErrorCode, string strErrorMsg, Exception e)
        {
            ArrayList alTemp = new ArrayList();

            alTemp.Add(strErrorCode);
            // db ErrorMsg only in Debug Mode
#if (DEBUG)
            if (null != e)
                alTemp.Add(strErrorMsg + "\n" + e.Message.ToString());
            else
                alTemp.Add(strErrorMsg);
#else
            alTemp.Add(strErrorMsg);
#endif
            alTemp.Add(0);
            alTemp.Add(0);

            return alTemp;
        }

        public static ArrayList EncodeData(string strErrorCode, string strErrorMsg, Exception e, SqlTransaction tr)
        {
            // if error then trnasaction rollback
            if ("00" != strErrorCode)
                tr.Rollback();

            ArrayList alTemp = new ArrayList();

            alTemp.Add(strErrorCode);
            // db ErrorMsg only in Debug Mode
#if (DEBUG)
            if (null != e)
                alTemp.Add(strErrorMsg + "\n" + e.ToString());
            else
                alTemp.Add(strErrorMsg);
#else
            alTemp.Add(strErrorMsg);
#endif
            alTemp.Add(0);
            alTemp.Add(0);

            // if no error then transaction commit
            if ("00" == strErrorCode)
                tr.Commit();

            return alTemp;
        }

        /// <summary>
        /// Encode OleDataReader to DropDownList Format
        /// </summary>
        public static ArrayList EncodeDdlData(string strErrorCode, string strErrorMsg, SqlDataReader dr, Exception e)
        {
            return EncodeDdlData(strErrorCode, strErrorMsg, dr, e, true);
        }

        /// <summary>
        /// Encode OleDataReader to DropDownList Format
        /// </summary>
        public static ArrayList EncodeDdlData(string strErrorCode, string strErrorMsg, SqlDataReader dr, Exception e, bool bInsertBlank)
        {
            ArrayList alTemp = new ArrayList();

            // ErorCode and Error Msg

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
                alTemp.Add(strErrorMsg);
#endif
                alTemp.Add(0);
                alTemp.Add(0);
                return alTemp;
            }
            else
            {
                alTemp.Add("00");
                alTemp.Add("");
            }

            // FieldCnt
            alTemp.Add(2);
            // RecordCnt - this time 0 and set real value later
            alTemp.Add(0);

            string strTemp = "";

            if (bInsertBlank)
            {
                // select all Record for ddlData
                for (int k = 0; k < dr.FieldCount - 1; k++)
                {
                    SetEncodedProperty(dr.GetName(k), "", ref strTemp);
                }
                alTemp.Add(strTemp);
                alTemp.Add("");
            }

            // insert data into ArrayList
            int i, j;
            if (!dr.Read())
            {
                // set error Message
                alTemp[1] = "There's No Such Data.";
                return alTemp;
            }

            bool bRowExists = true;
            for (i = 0; bRowExists; i++)
            {
                strTemp = "";
                for (j = 0; j < dr.FieldCount - 1; j++)
                    SetEncodedProperty(dr.GetName(j), F(dr.GetValue(j)).ToString(), ref strTemp);
                alTemp.Add(strTemp);
                alTemp.Add(F(dr.GetValue(j)));
                bRowExists = dr.Read();
            }
            dr.Close();

            // set real data Value
            alTemp[3] = bInsertBlank ? i + 1 : i;
            return alTemp;
        }

        /// <summary>
        /// Encode OleDbDataReader to TreePopup Format
        /// use when 
        /// </summary>
        public static ArrayList EncodeTpData(string strErrorCode, string strErrorMsg, SqlDataReader dr, Exception e)
        {
            ArrayList alTemp = new ArrayList();

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
                alTemp.Add(strErrorMsg);
#endif
                alTemp.Add(0);
                alTemp.Add(0);
                return alTemp;
            }
            // FieldCnt
            if (dr.FieldCount < 5)
            {
                alTemp.Add("XX");
                alTemp.Add("TreePopup Requires at least 5 Fields");
                alTemp.Add(0);
                alTemp.Add(0);
                return alTemp;
            }

            alTemp.Add("00");
            alTemp.Add("");


            alTemp.Add(6);
            // RecordCnt - this time 0 and set real value later
            alTemp.Add(0);

            // insert data into ArrayList
            if (!dr.Read())
            {
                // set error Message
                alTemp[1] = "There's No Such Data.";
                return alTemp;
            }

            int i = 0;
            bool bRowExists = true;
            string strLink, strTitle;
            for (; bRowExists; i++)
            {
                for (int j = 0; j < 4; j++)
                    alTemp.Add(F(dr.GetValue(j)));
                // Make Link & Title String
                strLink = "\"";
                strTitle = "";
                for (int j = 0; j < dr.FieldCount - 4; j++)
                {
                    strLink += "C" + (j < 9 ? "0" : "") + (j + 1) + " = " + F(dr.GetValue(j + 4)) + "|";
                    strTitle += (j != 0 ? "\t" : "") + F(dr.GetValue(j + 4));
                }

                alTemp.Add(strLink + "\" )");
                alTemp.Add(strTitle);
                bRowExists = dr.Read();
            }
            dr.Close();

            // set real data Value
            alTemp[3] = i;
            return alTemp;
        }


        /// <summary>
        /// Encode OleDbDataReader without column information
        /// use when 
        /// </summary>
        public static ArrayList EncodeData(string strErrorCode, string strErrorMsg, DataTableReader dr, Exception e)
        {
            TimeChallenge("EncodeData Start");

            ArrayList alTemp = new ArrayList();

            // ErorCode and Error Msg

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
                alTemp.Add(strErrorMsg);
#endif
                alTemp.Add(0);
                alTemp.Add(0);
                return alTemp;
            }
            else
            {
                alTemp.Add("00");
                alTemp.Add("");
            }

            // FieldCnt
            alTemp.Add(dr.FieldCount);
            // RecordCnt - this time 0 and set real value later
            alTemp.Add(0);

            char[] chSpace = { ' ' };

            // insert data into ArrayList
            int i, j;
            if (!dr.Read())
            {
                // set error Message
                alTemp[1] = "There's No Such Data.";
                alTemp[3] = 0;
                return alTemp;
            }

            bool bRowExists = true;
            for (i = 0; bRowExists; i++)
            {
                for (j = 0; j < dr.FieldCount; j++)
                    alTemp.Add(F(dr.GetValue(j)));
                bRowExists = dr.Read();
            }
            dr.Close();

            // set real data Value
            alTemp[3] = i;

            TimeChallenge("EncodeData End");

            return alTemp;
        }

        /// <summary>
        /// Encode OleDbDataReader without column information
        /// use when 
        /// </summary>
        public static ArrayList EncodeData(string strErrorCode, string strErrorMsg, DataTableReader dr, Exception e, SqlTransaction tr)
        {
            // if error then trnasaction rollback
            if ("00" != strErrorCode)
                tr.Rollback();

            ArrayList alTemp = new ArrayList();

            // ErorCode and Error Msg

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
                alTemp.Add(strErrorMsg);
#endif
                alTemp.Add(0);
                alTemp.Add(0);
                return alTemp;
            }
            else
            {
                alTemp.Add("00");
                alTemp.Add("");
            }

            // FieldCnt
            alTemp.Add(dr.FieldCount);
            // RecordCnt - this time 0 and set real value later
            alTemp.Add(0);

            char[] chSpace = { ' ' };

            // insert data into ArrayList
            int i, j;
            if (!dr.Read())
            {
                // set error Message
                alTemp[1] = "There's No Such Data.";
                alTemp[3] = 0;
                return alTemp;
            }

            bool bRowExists = true;
            for (i = 0; bRowExists; i++)
            {
                for (j = 0; j < dr.FieldCount; j++)
                    alTemp.Add(F(dr.GetValue(j)));
                bRowExists = dr.Read();
            }
            dr.Close();

            // if no error then transaction commit
            if ("00" == strErrorCode)
                tr.Commit();

            // set real data Value
            alTemp[3] = i;
            return alTemp;
        }

        /// <summary>
        /// Encode OleDbDataReader with column information
        /// </summary>
        public static ArrayList EncodeData(string strErrorCode, string strErrorMsg, DataTableReader dr, ArrayList alColumns, Exception e)
        {
            TimeChallenge("EncodeData Start");

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
                alTemp.Add(strErrorMsg);
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
            alTemp.Add(dr.FieldCount);
            // RecordCnt - this time 0 and set real value later
            alTemp.Add(0);

            int i, j;
            for (i = 0; i < alColumns.Count; i++)
                alTemp.Add(alColumns[i]);

            // insert data into ArrayList
            if (!dr.Read())
            {
                // set error Message
                alTemp[1] = "There's No Such Data.";
                alTemp[3] = 0;
                return alTemp;
            }

            bool bRowExists = true;
            for (i = 0; bRowExists; i++)
            {
                for (j = 0; j < dr.FieldCount; j++)
                    alTemp.Add(F(dr.GetValue(j)));
                bRowExists = dr.Read();
            }
            dr.Close();

            // set real data Value
            alTemp[3] = i;

            TimeChallenge("EncodeData End ( Count - " + alTemp.Count.ToString() + " )");

            return alTemp;
        }

        /// <summary>
        /// Encode OleDbDataReader with column information
        /// </summary>
        public static ArrayList EncodeData(string strErrorCode, string strErrorMsg, DataTableReader dr, ArrayList alColumns, Exception e, SqlTransaction tr)
        {
            // if error then trnasaction rollback
            if ("00" != strErrorCode)
                tr.Rollback();

            ArrayList alTemp = new ArrayList();

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
                alTemp.Add(strErrorMsg);
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
            alTemp.Add(dr.FieldCount);
            // RecordCnt - this time 0 and set real value later
            alTemp.Add(0);

            int i, j;
            for (i = 0; i < alColumns.Count; i++)
                alTemp.Add(alColumns[i]);

            char[] chSpace = { ' ' };

            // insert data into ArrayList
            if (!dr.Read())
            {
                // set error Message
                alTemp[1] = "There's No Such Data.";
                alTemp[3] = 0;
                return alTemp;
            }

            bool bRowExists = true;
            for (i = 0; bRowExists; i++)
            {
                for (j = 0; j < dr.FieldCount; j++)
                    alTemp.Add(F(dr.GetValue(j)));
                bRowExists = dr.Read();
            }
            dr.Close();

            // if no error then transaction commit
            if ("00" == strErrorCode)
                tr.Commit();

            // set real data Value
            alTemp[3] = i;
            return alTemp;
        }

        /// <summary>
        /// Encode OleDataReader to DropDownList Format
        /// </summary>
        public static ArrayList EncodeDdlData(string strErrorCode, string strErrorMsg, DataTableReader dr, Exception e)
        {
            return EncodeDdlData(strErrorCode, strErrorMsg, dr, e, true);
        }

        /// <summary>
        /// Encode OleDataReader to DropDownList Format
        /// </summary>
        public static ArrayList EncodeDdlData(string strErrorCode, string strErrorMsg, DataTableReader dr, Exception e, bool bInsertBlank)
        {
            ArrayList alTemp = new ArrayList();

            // ErorCode and Error Msg

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
                alTemp.Add(strErrorMsg);
#endif
                alTemp.Add(0);
                alTemp.Add(0);
                return alTemp;
            }
            else
            {
                alTemp.Add("00");
                alTemp.Add("");
            }

            // FieldCnt
            alTemp.Add(2);
            // RecordCnt - this time 0 and set real value later
            alTemp.Add(0);

            string strTemp = "";

            if (bInsertBlank)
            {
                // select all Record for ddlData
                for (int k = 0; k < dr.FieldCount - 1; k++)
                {
                    SetEncodedProperty(dr.GetName(k), "", ref strTemp);
                }
                alTemp.Add(strTemp);
                alTemp.Add("");
            }

            // insert data into ArrayList
            int i, j;
            if (!dr.Read())
            {
                // set error Message
                alTemp[1] = "There's No Such Data.";
                return alTemp;
            }

            bool bRowExists = true;
            for (i = 0; bRowExists; i++)
            {
                strTemp = "";
                for (j = 0; j < dr.FieldCount - 1; j++)
                    SetEncodedProperty(dr.GetName(j), F(dr.GetValue(j)).ToString(), ref strTemp);
                alTemp.Add(strTemp);
                alTemp.Add(F(dr.GetValue(j)));
                bRowExists = dr.Read();
            }
            dr.Close();

            // set real data Value
            alTemp[3] = bInsertBlank ? i + 1 : i;
            return alTemp;
        }

        /// <summary>
        /// Encode OleDbDataReader to TreePopup Format
        /// use when 
        /// </summary>
        public static ArrayList EncodeTpData(string strErrorCode, string strErrorMsg, DataTableReader dr, Exception e)
        {
            ArrayList alTemp = new ArrayList();

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
                alTemp.Add(strErrorMsg);
#endif
                alTemp.Add(0);
                alTemp.Add(0);
                return alTemp;
            }
            // FieldCnt
            if (dr.FieldCount < 5)
            {
                alTemp.Add("XX");
                alTemp.Add("TreePopup Requires at least 5 Fields");
                alTemp.Add(0);
                alTemp.Add(0);
                return alTemp;
            }

            alTemp.Add("00");
            alTemp.Add("");


            alTemp.Add(6);
            // RecordCnt - this time 0 and set real value later
            alTemp.Add(0);

            // insert data into ArrayList
            if (!dr.Read())
            {
                // set error Message
                alTemp[1] = "There's No Such Data.";
                return alTemp;
            }

            int i = 0;
            bool bRowExists = true;
            string strLink, strTitle;
            for (; bRowExists; i++)
            {
                for (int j = 0; j < 4; j++)
                    alTemp.Add(F(dr.GetValue(j)));
                // Make Link & Title String
                strLink = "\"";
                strTitle = "";
                for (int j = 0; j < dr.FieldCount - 4; j++)
                {
                    strLink += "C" + (j < 9 ? "0" : "") + (j + 1) + " = " + F(dr.GetValue(j + 4)) + "|";
                    strTitle += (j != 0 ? "\t" : "") + F(dr.GetValue(j + 4));
                }

                alTemp.Add(strLink + "\" )");
                alTemp.Add(strTitle);
                bRowExists = dr.Read();
            }
            dr.Close();

            // set real data Value
            alTemp[3] = i;
            return alTemp;
        }


        /// <summary>
        /// return No Service Message
        /// </summary>
        /// <returns></returns>
        public static ArrayList NoService()
        {
            ArrayList alTemp = new ArrayList();
            alTemp.Add("XX");
            alTemp.Add("There's No Such Service.");
            alTemp.Add(0);
            alTemp.Add(0);
            return alTemp;
        }

        #endregion

        #region DBNull Filtering

        /// <summary>
        /// DB Null Filtering for String Data
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object F(object obj)
        {
            if (null == obj || "System.DBNull" == obj.GetType().ToString())
                return "";
            else
                return T(obj.ToString());
        }

        /// <summary>
        /// DB Null Filtering for Number Data
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object FD(object obj)
        {
            if (null == obj || "System.DBNull" == obj.GetType().ToString())
                return 0m;
            else
                return obj;
        }

        #endregion

        #region Validity Check

        public static bool RefCheck(string strItemName, string strLangID, ref ArrayList alResult)
        {
            return RefCheck(strItemName, strLangID, ref alResult, null);
        }

        public static bool RefCheck(string strItemName, string strLangID, ref ArrayList alResult, string strErrorCode)
        {
            string strCode = "00", strErrorMsg = "";
            // if there's error
            if ("00" != alResult[0].ToString())
                return false;
            // if there's no data then Error
            else if (0 == (int)alResult[3])
            {
                strCode = (null == strErrorCode || "" == strErrorCode) ? "Y1" : strErrorCode;
                // Message variation for Languages
                switch (strLangID.ToLower())
                {
                    case "eng":
                        strErrorMsg = "You have selected the item " + strItemName + " that does not exist.";
                        break;
                    case "chn":
                        strErrorMsg = "不存在的 " + strItemName + "项目被选择。";
                        break;
                    case "jpn":
                        strErrorMsg = "存在しない" + strItemName + "項目が選択されました。";
                        break;
                    // default Korean Language
                    default:
                        strErrorMsg = "존재하지 않는 " + strItemName + " 항목이 선택되었습니다.";
                        break;
                }
                alResult = EncodeData(strCode, strErrorMsg, null);
                return false;
            }
            return true;
        }

        public static bool NotNullCheck(string strItemName, string strItemValue, string strLangID, ref ArrayList alResult)
        {
            return NotNullCheck(strItemName, strItemValue, strLangID, ref alResult, null);
        }

        public static bool NotNullCheck(string strItemName, string strItemValue, string strLangID, ref ArrayList alResult, string strErrorCode)
        {
            string strCode = "00", strErrorMsg = "";
            if (null == strItemValue || "" == strItemValue)
            {
                strCode = (null == strErrorCode || "" == strErrorCode) ? "Y2" : strErrorCode;
                // Message variation for Languages
                switch (strLangID.ToLower())
                {
                    // default Korean Language
                    case "eng":
                        strErrorMsg = "Please enter the item " + strItemName;
                        break;
                    case "chn":
                        strErrorMsg = strItemName + "请输入项目。";
                        break;
                    case "jpn":
                        strErrorMsg = strItemName + "項目を入力してください。";
                        break;
                    default:
                        strErrorMsg = strItemName + "항목을 입력하여 주십시오.";
                        break;
                }
                alResult = EncodeData(strCode, strErrorMsg, null);
                return false;
            }
            return true;
        }

        #endregion

        #region String Handler

        /// <summary>
        /// Transform string to DB Entry word
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Q(string str)
        {
            if (str == null || str == "")
                return "null";
            else
            {
                // replace single Quotation - SQL Server Only
                str = str.Replace("'", "''");
                return "N'" + str + "'";
            }
        }
        public static string GetStr(object Obj)
        {
            string sRet = "";
            try
            {
                sRet = Obj.ToString().Trim();
            }
            catch
            {
                sRet = "";
            }

            return sRet;
        }

        public static string N(string str)
        {
            if (str == null || str == "")
                return "null";

            return str;
        }

        public static string T(string str)
        {
            char[] chSpace = { ' ' };
            if (str == null || str == "")
                return str;
            else
                return str.TrimEnd(chSpace);
        }

        private static bool EncodeCore(bool bGet, string strPropName, ref string strValue, ref string strSource, string strSeperator, bool bOnlyValid)
        {
            bool bExists = true;
            string strSource1, strPre, strTail;
            int i, j;

            if (bGet)
            {
                strSource1 = strSource;
                if (null == strSource1)
                {
                    strValue = "";
                    return false;
                }

                // the property will remain in the region by Property = value| format
                // There Can be any Meaning charactor in front of Property name
                // Check the character just in front of the PropName if it's an Alphabet or a Digit
                i = -1;
                do
                {
                    i = strSource1.IndexOf(strPropName + " = ", i + 1);
                } while (i > 0 && System.Char.IsLetterOrDigit(strSource1[i - 1]));

                if (i < 0)
                {
                    strValue = "";
                    bExists = false;
                }
                else
                {
                    j = strSource1.IndexOf(strSeperator, i + strPropName.Length + 3);
                    strValue = strSource1.Substring(i + strPropName.Length + 3, j - i - strPropName.Length - 3);
                }
            }
            else
            {
                string strValue1 = strValue;
                if (null == strValue1)
                    strValue1 = "";

                // the property will remain in the region by Property = value| format
                i = strSource.IndexOf(strPropName + " = ");
                if (i < 0)
                {
                    if ("" != strValue1 || !bOnlyValid)
                        strSource += strPropName + " = " + strValue1 + strSeperator;
                    bExists = false;
                }
                else
                {
                    // remove PrevData
                    if ("" == strValue1 && bOnlyValid)
                        strPre = strSource.Substring(0, i);
                    else
                        strPre = strSource.Substring(0, i + strPropName.Length + 3);
                    j = strSource.IndexOf(strSeperator, i + strPropName.Length + 3);
                    if ("" == strValue1 && bOnlyValid)
                        strTail = strSource.Substring(j + strSeperator.Length, strSource.Length - j - strSeperator.Length);
                    else
                        strTail = strSource.Substring(j, strSource.Length - j);
                    strSource = strPre + strValue1 + strTail;
                }
            }
            return bExists;
        }

        public static string GetEncodedProperty(string strPropName, string strSource)
        {
            string strValue = "";
            EncodeCore(true, strPropName, ref strValue, ref strSource, "|", false);
            return strValue;
        }

        public static string[] GetEncodedProperties(string strPropName, string strSource)
        {
            ArrayList alData = new ArrayList();
            string strValue = "";
            string strSource1 = strSource;
            // the property will remain in the region by Property1 = value| format
            for (int i = 0; EncodeCore(true, strPropName + i, ref strValue, ref strSource1, "|", false); i++)
            {
                alData.Add(strValue);
            }

            string[] sarValue = new string[alData.Count];
            for (int i = 0; i < alData.Count; i++)
                sarValue[i] = alData[i].ToString();

            return sarValue;
        }

        public static void SetEncodedProperty(string strPropName, string strValue, ref string strSource)
        {
            EncodeCore(false, strPropName, ref strValue, ref strSource, "|", true);
        }

        public static void SetEncodedProperty(string strPropName, string strValue, ref string strSource, bool bOnlyValid)
        {
            EncodeCore(false, strPropName, ref strValue, ref strSource, "|", bOnlyValid);
        }

        public static void SetEncodedProperties(string strPropName, ArrayList salValue, ref string strSource)
        {
            string[] sarValue = new string[salValue.Count];
            for (int i = 0; i < salValue.Count; i++)
                sarValue[i] = salValue[i].ToString();

            SetEncodedPropertiesCore(strPropName, sarValue, ref strSource, "|");
        }

        public static void SetEncodedProperties(string strPropName, string[] sarValue, ref string strSource)
        {
            SetEncodedPropertiesCore(strPropName, sarValue, ref strSource, "|");
        }

        public static void SetEncodedPropertiesCore(string strPropName, string[] sarValue, ref string strSource, string strSeperator)
        {
            int i = 0;
            string strTemp = "";
            foreach (string strValue in sarValue)
            {
                strTemp = strValue;
                EncodeCore(false, strPropName + i++, ref strTemp, ref strSource, strSeperator, false);
            }
        }

        public static string GetEncodedPropertyS(string strPropName, string strSource)
        {
            string strValue = "";
            string strSource1 = strSource;
            bool bTemp = EncodeCore(true, strPropName, ref strValue, ref strSource1, "#$", false);
            return strValue;
        }

        public static string[] GetEncodedPropertiesS(string strPropName, string strSource)
        {
            ArrayList alData = new ArrayList();
            string strValue = "";
            // the property will remain in the region by Property1 = value| format
            for (int i = 0; EncodeCore(true, strPropName + i, ref strValue, ref strSource, "#$", false); i++)
            {
                alData.Add(strValue);
            }
            string[] sarValue = new string[alData.Count];
            for (int i = 0; i < alData.Count; i++)
                sarValue[i] = alData[i].ToString();

            return sarValue;
        }

        public static void SetEncodedPropertyS(string strPropName, string strValue, ref string strSource)
        {
            EncodeCore(false, strPropName, ref strValue, ref strSource, "#$", true);
        }

        public static void SetEncodedPropertyS(string strPropName, string strValue, ref string strSource, bool bOnlyValid)
        {
            EncodeCore(false, strPropName, ref strValue, ref strSource, "#$", bOnlyValid);
        }

        public static void SetEncodedPropertiesS(string strPropName, ArrayList salValue, ref string strSource)
        {
            string[] sarValue = new string[salValue.Count];
            for (int i = 0; i < salValue.Count; i++)
                sarValue[i] = salValue[i].ToString();

            SetEncodedPropertiesCore(strPropName, sarValue, ref strSource, "#$");
        }

        public static void SetEncodedPropertiesS(string strPropName, string[] sarValue, ref string strSource)
        {
            SetEncodedPropertiesCore(strPropName, sarValue, ref strSource, "#$");
        }

        /// <summary>
        /// insert Comma for nemeric data ( specially for amount ) and Trim Digits
        /// </summary>
        /// <param name="iRoudingType">0:Rounding, 1:Flooring, 2:Ceiling</param>
        /// <returns></returns>
        public static string E(object objSource)
        {
            string strSource = objSource.ToString().ToLower();
            if (strSource.IndexOf("e") == -1)
                return strSource;
            bool bPositive = true;

            if (strSource.IndexOf("-") == 0)
            {
                bPositive = false;
                strSource = strSource.Substring(1);
            }

            string[] strTemp = strSource.Split('e');

            if (strTemp[1].Substring(0, 1) == "-")
            {
                if (strTemp[0].IndexOf(".") != -1)
                {
                    string[] strTemp1 = (strTemp[0]).Split('.');
                    strTemp[0] = strTemp1[0] + strTemp1[1];
                }

                for (int i = 1; i < Convert.ToInt32(strTemp[1].Substring(1)); i++)
                {
                    strTemp[0] = "0" + strTemp[0];
                }
                strTemp[0] = "0." + strTemp[0];
            }
            if (bPositive == false)
                strTemp[0] = "-" + strTemp[0];
            return strTemp[0];

        }

        public static string C(object objSource, int iDigit, int iRoudingType)
        {
            if (null == objSource || "" == objSource.ToString())
                return "";

            objSource = objSource.ToString().Replace(",", "");
            objSource = E(objSource);

            bool bNegative = false;
            decimal dTemp = Tod(objSource);
            if (dTemp < 0)
            {
                dTemp *= -1;
                bNegative = true;
            }

            switch (iRoudingType)
            {
                // Flooring
                case 1:
                    dTemp = decimal.Floor(dTemp * Power(10m, iDigit)) / Power(10m, iDigit);
                    break;
                // Ceiling
                case 2:
                    if ((decimal.Floor(dTemp * Power(10m, iDigit))) < dTemp * Power(10m, iDigit))
                        dTemp = (decimal.Floor(dTemp * Power(10m, iDigit)) + 1) / Power(10m, iDigit);
                    else
                        dTemp = decimal.Floor(dTemp * Power(10m, iDigit)) / Power(10m, iDigit);
                    break;
                // rounding
                default:
                    dTemp = decimal.Floor(dTemp * Power(10m, iDigit) + (0.5m)) / Power(10m, iDigit);
                    break;
            }

            if (bNegative)
                dTemp *= -1;

            string strInteger = (Decimal.Truncate(dTemp)).ToString();

            decimal dFraction = Decimal.Subtract(dTemp, Decimal.Truncate(dTemp));
            if (dFraction < 0m)
                dFraction = Decimal.Negate(dFraction);
            dFraction = dFraction * Power(10m, iDigit);

            string strFraction, strSource;
            strFraction = (Decimal.Truncate(dFraction)).ToString();
            if (strFraction.Length > iDigit)
                strFraction = strFraction.Substring(0, iDigit);

            while (strFraction.Length < iDigit)
                strFraction = "0" + strFraction;

            if (iDigit != 0)
            {
                if (bNegative && strInteger.Substring(0, 1) != "-")
                    strSource = "-";
                else strSource = "";
                strSource += strInteger + "." + strFraction;
            }
            else strSource = strInteger;
            //		string strSource = dTemp.ToString();


            // eliminate - / + notation
            string strHeader = "";
            if (strSource.Length > 0)
            {
                if ("-" == strSource.Substring(0, 1) || "+" == strSource.Substring(0, 1))
                {
                    strHeader = strSource.Substring(0, 1);
                    strSource = strSource.Substring(1, strSource.Length - 1);
                }
            }

            // split floating point
            string strTail = "";
            int iPoint = strSource.IndexOf(".");
            if (iPoint > -1)
            {
                if (iDigit > 0)
                    strTail = strSource.Substring(iPoint, (strSource.Length - iPoint > iDigit + 1) ? iDigit + 1 : strSource.Length - iPoint);

                strSource = strSource.Substring(0, iPoint);
            }
            else if (iDigit > 0)
                strTail = ".";

            if (iDigit > 0)
            {
                for (int i = strTail.Length; i < iDigit + 1; i++)
                    strTail += "0";
            }

            // insert Comma
            string strResult = (strSource.Length > 2) ? strSource.Substring(strSource.Length - 3, 3) : strSource;
            strSource = strSource.Length > 2 ? strSource.Substring(0, strSource.Length - 3) : "";
            while (strSource.Length > 2)
            {
                strResult = strSource.Substring(strSource.Length - 3, 3) + "," + strResult;
                strSource = strSource.Substring(0, strSource.Length - 3);
            }
            if (strSource.Length > 0)
                strResult = strSource + "," + strResult;
            return strHeader + strResult + strTail;
        }

        public static string CR(object objSource)
        {
            if (null == objSource || "" == objSource.ToString())
                return "";

            return objSource.ToString().Replace(",", "");
        }

        public static decimal Power(decimal dSource, long lExp)
        {
            decimal dResult = 1m;
            if (lExp >= 0)
            {
                for (long i = 0; i < lExp; i++)
                    dResult *= dSource;
            }
            else
            {
                for (long i = 0; i > lExp; i--)
                    dResult /= dSource;
            }
            return dResult;
        }

        #endregion

        #region Script Renderers

        /// <summary>
        /// GetError Msg URL
        /// </summary>
        /// <param name="strBtnType">ButtonType ( Back )</param>
        /// <param name="strTitle">MsgTitle</param>
        /// <param name="strMsg">MsgContent</param>
        /// <returns></returns>
        public static string GetErrorURL(string strBtnType, string strTitle, string strMsg)
        {
            return System.Configuration.ConfigurationSettings.AppSettings["CommonFormsUrl"] + "Msg.aspx?Btn=" + strBtnType
                + "&Title=" + HttpUtility.UrlEncode(strTitle, System.Text.Encoding.GetEncoding("euc-kr"))
                + "&Msg=" + HttpUtility.UrlEncode(strMsg, System.Text.Encoding.GetEncoding("euc-kr"));
        }

        /// <summary>
        /// Get Alert Script and Focus Given Edit
        /// </summary>
        /// <param name="strMsg"></param>
        /// <param name="strFocusID"></param>
        /// <returns></returns>
        public static string GetMsg(string strMsg, string strFocusID)
        {
            string strRender = @"<script language=""Javascript"">
alert ( """ + strMsg.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\r", "") + "\" );\n";
            if (null != strFocusID && "" != strFocusID)
                //strRender += "document.all['" + strFocusID + "'].focus();\n";
                strRender += strFocusID + ".Focus();" + "\n";
            strRender += "</script>\n";

            return strRender;
        }

        public static string GetMsg(string strMsg)
        {
            return GetMsg(strMsg, null);
        }

        /// <summary>
        /// Generate Status Bar Message
        /// </summary>
        public enum StatType { Insert, Update, Delete, New, Query, Load, Custom };
        public static string GetStatScript(StatType Type, bool bSuccess, string strLangID, string strDocName, bool bDocNameBadchim, string strDocNo)
        {
            string strScript = @"<script language=""Javascript"">window.status='";

            switch (strLangID.ToUpper())
            {
                case "ENG":
                    switch (Type)
                    {
                        case StatType.Insert:
                            if (bSuccess)
                                strScript += "Saved in [#" + strDocNo + "].";
                            else
                                strScript += "Failed to Save " + strDocName;
                            break;
                        case StatType.Update:
                            if (bSuccess)
                                strScript += strDocName + " of [#" + strDocNo + "] is modified.";
                            else
                                strScript += "Failed to modify " + strDocName + " of [#" + strDocNo + "].";
                            break;
                        case StatType.Delete:
                            if (bSuccess)
                                strScript += strDocName + " of [#" + strDocNo + "] is deleted.";
                            else
                                strScript += "Failed to delete " + strDocName + " of [#" + strDocNo + "].";
                            break;
                        case StatType.New:
                            strScript += "Create new " + strDocName;
                            break;
                        case StatType.Query:
                            if (bSuccess)
                                strScript += strDocName + " is found.";
                            else
                                strScript += "Search for " + strDocName + " has failed.";
                            break;
                        case StatType.Load:
                            if (bSuccess)
                                strScript += strDocName + " of [#" + strDocNo + "] is loaded.";
                            else
                                strScript += "Failed to load " + strDocName + " of [#" + strDocNo + "].";
                            break;
                        default:	// include Custom
                            strScript += strDocName;
                            break;
                    }
                    break;
                case "CHN":
                    switch (Type)
                    {
                        case StatType.Insert:
                            if (bSuccess)
                                strScript += "保存为 [" + strDocNo + "]号。";
                            else
                                strScript += strDocName + " 保存失败";
                            break;
                        case StatType.Update:
                            if (bSuccess)
                                strScript += "[" + strDocNo + "]号 " + strDocName + " 被修改。";
                            else
                                strScript += "[" + strDocNo + "]号 " + strDocName + " 修改失败。";
                            break;
                        case StatType.Delete:
                            if (bSuccess)
                                strScript += "[" + strDocNo + "]号 " + strDocName + " 被删除。";
                            else
                                strScript += "[" + strDocNo + "]号 " + strDocName + " 删除失败。";
                            break;
                        case StatType.New:
                            strScript += "编制新 " + strDocName + "。";
                            break;
                        case StatType.Query:
                            if (bSuccess)
                                strScript += strDocName + " 被检索。";
                            else
                                strScript += strDocName + " 检索失败。";
                            break;
                        case StatType.Load:
                            if (bSuccess)
                                strScript += "导出 " + "[" + strDocNo + "]号 " + strDocName + "。";
                            else
                                strScript += "[" + strDocNo + "]号 " + strDocName + " 导出失败。";
                            break;
                        default:	// include Custom
                            strScript += strDocName;
                            break;
                    }
                    break;
                case "JPN":
                    switch (Type)
                    {
                        case StatType.Insert:
                            if (bSuccess)
                                strScript += "[" + strDocNo + "] 番に保存されました。";
                            else
                                strScript += strDocName + " 保存に失敗しました。";
                            break;
                        case StatType.Update:
                            if (bSuccess)
                                strScript += "[" + strDocNo + "]番" + strDocName + "が" + "修正されました。";
                            else
                                strScript += "[" + strDocNo + "]番" + strDocName + "修正に失敗しました。";
                            break;
                        case StatType.Delete:
                            if (bSuccess)
                                strScript += "[" + strDocNo + "]番" + strDocName + "が" + "削除されました。";
                            else
                                strScript += "[" + strDocNo + "]番" + strDocName + "削除に失敗しました。";
                            break;
                        case StatType.New:
                            strScript += "新しい" + strDocName + "を" + "作成します。";
                            break;
                        case StatType.Query:
                            if (bSuccess)
                                strScript += strDocName + "が" + "検索されました。";
                            else
                                strScript += strDocName + "検索に失敗しました。";
                            break;
                        case StatType.Load:
                            if (bSuccess)
                                strScript += "[" + strDocNo + "]番" + strDocName + "が" + "ロードされました。";
                            else
                                strScript += "[" + strDocNo + "]番" + strDocName + "ロードに失敗しました。";
                            break;
                        default:	// include Custom
                            strScript += strDocName;
                            break;
                    }
                    break;
                default:	// include KOR
                    switch (Type)
                    {
                        case StatType.Insert:
                            if (bSuccess)
                                strScript += "[" + strDocNo + "] 번으로 저장되었습니다.";
                            else
                                strScript += strDocName + " 저장에 실패하였습니다.";
                            break;
                        case StatType.Update:
                            if (bSuccess)
                                strScript += "[" + strDocNo + "]번 " + strDocName + (bDocNameBadchim ? "이" : "가") + " 수정되었습니다.";
                            else
                                strScript += "[" + strDocNo + "]번 " + strDocName + " 수정에 실패하였습니다.";
                            break;
                        case StatType.Delete:
                            if (bSuccess)
                                strScript += "[" + strDocNo + "]번 " + strDocName + (bDocNameBadchim ? "이" : "가") + " 삭제되었습니다.";
                            else
                                strScript += "[" + strDocNo + "]번 " + strDocName + " 삭제에 실패하였습니다.";
                            break;
                        case StatType.New:
                            strScript += "새로운 " + strDocName + (bDocNameBadchim ? "을" : "를") + " 작성합니다.";
                            break;
                        case StatType.Query:
                            if (bSuccess)
                                strScript += strDocName + (bDocNameBadchim ? "이" : "가") + " 검색되었습니다.";
                            else
                                strScript += strDocName + " 검색에 실패하였습니다.";
                            break;
                        case StatType.Load:
                            if (bSuccess)
                                strScript += "[" + strDocNo + "]번 " + strDocName + (bDocNameBadchim ? "이" : "가") + " 로드되었습니다.";
                            else
                                strScript += "[" + strDocNo + "]번 " + strDocName + " 로드에 실패하였습니다.";
                            break;
                        default:	// include Custom
                            strScript += strDocName;
                            break;
                    }
                    break;
            }

            return strScript + @"';setTimeout( ""window.status = ''"", 5000 );</script>";
        }
        #endregion

        #region Handling Numbers

        public static decimal Tod(object objSrc)
        {
            try
            {
                return Convert.ToDecimal(objSrc.ToString());
            }
            catch
            {
                return 0m;
            }
        }

        public static double Todb(object objSrc)
        {
            try
            {
                return Convert.ToDouble(objSrc.ToString());
            }
            catch
            {
                return 0;
            }
        }

        public static int Toi(object objSrc)
        {
            try
            {
                return Convert.ToInt32(objSrc.ToString());
            }
            catch
            {
                return 0;
            }
        }

        public static short Tos(object objSrc)
        {
            try
            {
                return Convert.ToInt16(objSrc.ToString());

            }
            catch
            {
                return 0;
            }
        }

        public static decimal ArraySum(ArrayList arData)
        {
            return ArraySum(arData.ToArray());
        }

        public static decimal ArraySum(object[] arData)
        {
            decimal dSum = 0m;
            foreach (object obj in arData)
                dSum += Tod(obj);
            return dSum;
        }

        public static decimal R(object objSource, int iDigit, int iRoudingType)
        {
            return Tod(CR(C(objSource, iDigit, iRoudingType)));
        }

        #endregion

        #region ModuleIni Conversion

        public enum DivIni { ALL, MNG, LOG, FIN, HRM, ESM };
        /// <summary>
        /// Module Initial Definition
        /// </summary>

        public static ModuleIni[] arMNGDivIni = new ModuleIni[]{
            ModuleIni.OM};

        public static ModuleIni[] arLOGDivIni = new ModuleIni[]{
            ModuleIni.SD, ModuleIni.PP, ModuleIni.PO, ModuleIni.MM, ModuleIni.SM, ModuleIni.QM, ModuleIni.PM
            , ModuleIni.EX, ModuleIni.IM, ModuleIni.OS, ModuleIni.TM, ModuleIni.PR, ModuleIni.NP, ModuleIni.QC
        , ModuleIni.QE};

        public static ModuleIni[] arFINDivIni = new ModuleIni[]{
            ModuleIni.GL, ModuleIni.AA, ModuleIni.TR, ModuleIni.BG, ModuleIni.CO};

        public static ModuleIni[] arHRMDivIni = new ModuleIni[]{
            ModuleIni.HR, ModuleIni.HM};

        public static ModuleIni[] arESMDivIni = new ModuleIni[]{
            ModuleIni.ES};

        /// Sinbad Starts Modifying - 2003/12/11
        public enum ModuleIni { OM, SD, EX, SM, PP, MM, PO, IM, PM, QM, BG, AA, TR, CO, GL, TM, HR, PR, ES, OS, NP, QC, QE, HM };
        public static ModuleIni[] arModuleIni = new ModuleIni[] {
                                                                    ModuleIni.OM, ModuleIni.SD, ModuleIni.EX, ModuleIni.SM, ModuleIni.PP, ModuleIni.MM, ModuleIni.PO
                                                                    , ModuleIni.IM, ModuleIni.PM, ModuleIni.QM, ModuleIni.BG, ModuleIni.AA, ModuleIni.TR, ModuleIni.CO
                                                                    , ModuleIni.GL, ModuleIni.TM, ModuleIni.HR, ModuleIni.PR, ModuleIni.ES, ModuleIni.OS, ModuleIni.NP
                                                                    , ModuleIni.QC, ModuleIni.QE, ModuleIni.HM
                                                                };
        public static string c_CookieNameUserID;

        /// Sinbad Ends Modifying - 2003/12/11

        public static ModuleIni StrToModuleIni(string strModuleIni)
        {
            foreach (ModuleIni mi in arModuleIni)
            {
                if (mi.ToString() == strModuleIni)
                    return mi;
            }
            return arModuleIni[0];
        }
        public static int EnumModule(ModuleIni Module)
        {
            for (int i = 0; i < arModuleIni.Length; i++)
            {
                if (Module == arModuleIni[i])
                    return i;
            }
            return 0;
        }
        public static string[] GetLicensedModuleIni(DivIni divIni, string strModuleUseCheck)
        {
            ArrayList alLicensedModule = new ArrayList();

            for (int i = 0; i < arModuleIni.Length; i++)
            {
                if ("Y" == strModuleUseCheck.Substring(i, 1))
                {
                    switch (divIni)
                    {
                        case DivIni.ALL:
                            alLicensedModule.Add(arModuleIni[i].ToString());
                            break;
                        case DivIni.MNG:
                            foreach (ModuleIni mngDivIni in arMNGDivIni)
                                if (mngDivIni == arModuleIni[i])
                                {
                                    alLicensedModule.Add(arModuleIni[i].ToString());
                                    break;
                                }
                            break;
                        case DivIni.LOG:
                            foreach (ModuleIni logDivIni in arLOGDivIni)
                                if (logDivIni == arModuleIni[i])
                                {
                                    alLicensedModule.Add(arModuleIni[i].ToString());
                                    break;
                                }
                            break;
                        case DivIni.FIN:
                            foreach (ModuleIni finDivIni in arFINDivIni)
                                if (finDivIni == arModuleIni[i])
                                {
                                    alLicensedModule.Add(arModuleIni[i].ToString());
                                    break;
                                }
                            break;
                        case DivIni.HRM:
                            foreach (ModuleIni hrmDivIni in arHRMDivIni)
                                if (hrmDivIni == arModuleIni[i])
                                {
                                    alLicensedModule.Add(arModuleIni[i].ToString());
                                    break;
                                }
                            break;
                        case DivIni.ESM:
                            foreach (ModuleIni esmDivIni in arESMDivIni)
                                if (esmDivIni == arModuleIni[i])
                                {
                                    alLicensedModule.Add(arModuleIni[i].ToString());
                                    break;
                                }
                            break;
                        default:
                            alLicensedModule.Add(arModuleIni[i].ToString());
                            break;
                    }
                }
            }

            string[] sarLicensedModuleIni = new string[alLicensedModule.Count];
            for (int i = 0; i < alLicensedModule.Count; i++)
                sarLicensedModuleIni[i] = alLicensedModule[i].ToString();
            return sarLicensedModuleIni;
        }


        public static string MakeModuleIniFilter(string strModuleUseCheck, string strFieldName)
        {
            return MakeModuleIniFilter(DivIni.ALL, strModuleUseCheck, strFieldName);
        }

        public static string MakeModuleIniFilter(DivIni divIni, string strModuleUseCheck, string strFieldName)
        {
            string[] sarModuleIni = CFL.GetLicensedModuleIni(divIni, strModuleUseCheck);
            string strFilter = @" ( ";

            for (int i = 0; i < sarModuleIni.Length; i++)
            {
                if (i != 0)
                    strFilter += @"	or ";
                strFilter += @"
	" + strFieldName + @" like " + CFL.Q(sarModuleIni[i] + "%");
            }
            strFilter += @" )";
            return strFilter;
        }

        #endregion

        #region GetProfileInfo

        /// <summary>
        /// Get Profile information from SMART.ini file
        /// </summary>
        /// <param name="strPropertyName"></param>
        /// <returns></returns>
        public static string GetProfileInfo(string strPropertyName)
        {

            TextReader txtReader = File.OpenText(GetIniPath() + "\\" + CFL.formRoot + ".ini");

            // Windows 7 : 보안 때문  ( MES.ini )
            //  TextReader txtReader = File.OpenText(GetIniPath2() + "\\" + CFL.formRoot + ".ini");

            string strTemp;
            do
            {
                strTemp = txtReader.ReadLine();
            } while (-1 != txtReader.Peek() && !strTemp.StartsWith(strPropertyName + " = "));

            if (!strTemp.StartsWith(strPropertyName + " = "))
                return "";

            string strResult = strTemp.Substring(strPropertyName.Length + 3, strTemp.Length - strPropertyName.Length - 3);
            txtReader.Close();

            return strResult;
        }

        public static string GetIniPath()
        {
            string strPath;
            try
            {
                RegistryKey rgKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\UsgKorea\\SmartPro");
                strPath = rgKey.GetValue("IniPath").ToString();
            }
            catch
            {
                strPath = System.Environment.SystemDirectory;
            }
            return strPath;
        }

        // Windows 7 : 보안관련
        public static string GetIniPath2()
        {
            string strPath;

            strPath = (string)System.Configuration.ConfigurationManager.AppSettings["SMARTFilesBase"];


            return strPath;
        }

        #endregion

        #region about DateTime

        public static bool DateValidate(string strDateName, string strDate, string strLangID, ref ArrayList alResult)
        {
            return DateValidate(strDateName, strDate, strLangID, ref alResult, null);
        }

        // Input : 8 Byte Date ( yyyymmdd )
        public static bool DateValidate(string strDateName, string strDate, string strLangID, ref ArrayList alResult, string strErrorCode)
        {
            string strCode = "00", strErrorMsg = "";
            alResult = new ArrayList();
            if (null == strDate)
                strCode = "D0";
            else if (8 > strDate.Length)
                strCode = "D1";
            else if (8 < strDate.Length)
                strCode = "D2";

            int iYear = Toi(strDate.Substring(0, 4));
            int iMonth = Toi(strDate.Substring(4, 2));
            int iDay = Toi(strDate.Substring(6, 2));

            if (strCode == "00")
            {
                if (iYear < 1000)
                    strCode = "D3";
                else if (iMonth > 12 || iMonth < 1)
                    strCode = "D4";
                else if (iDay < 1)
                    strCode = "D5";
                else if (iDay > DateTime.DaysInMonth(iYear, iMonth))
                    strCode = "D6";
            }

            switch (strLangID.ToLower())
            {
                case "eng":
                    switch (strCode)
                    {
                        case "D0":
                            strErrorMsg = "Please enter the date for " + strDateName;
                            break;
                        case "D1":
                            strErrorMsg = "You have not entered the whole date for " + strDateName;
                            break;
                        case "D2":
                            strErrorMsg = "The data for " + strDateName + " is too long.";
                            break;
                        case "D3":
                            strErrorMsg = "You cannot enter the data for " + strDateName + " before year 1000.";
                            break;
                        case "D4":
                            strErrorMsg = "The month value of date " + strDateName + " is invalid.";
                            break;
                        case "D5":
                            strErrorMsg = "The date of " + strDateName + " should be larger than 0.";
                            break;
                        case "D6":
                            strErrorMsg = "The date value of date " + strDateName + " is invalid.";
                            break;
                    }
                    break;
                case "chn":
                    switch (strCode)
                    {
                        case "D0":
                            strErrorMsg = "请输入" + strDateName + "日期";
                            break;
                        case "D1":
                            strErrorMsg = strDateName + "日期没有全部输入。";
                            break;
                        case "D2":
                            strErrorMsg = strDateName + "日期太长。";
                            break;
                        case "D3":
                            strErrorMsg = "1000年以前的" + strDateName + "日期无法输入。";
                            break;
                        case "D4":
                            strErrorMsg = strDateName + "日期的月份错误。";
                            break;
                        case "D5":
                            strErrorMsg = strDateName + "日期的日应大于0。";
                            break;
                        case "D6":
                            strErrorMsg = strDateName + "日期的日有错误。";
                            break;
                    }
                    break;
                case "jpn":
                    switch (strCode)
                    {
                        case "D0":
                            strErrorMsg = strDateName + "日付を入力してください。";
                            break;
                        case "D1":
                            strErrorMsg = strDateName + "日付が全部入力出来ません。";
                            break;
                        case "D2":
                            strErrorMsg = strDateName + "日付の長さが長すぎます。";
                            break;
                        case "D3":
                            strErrorMsg = strDateName + "日付は1000年以前の入力が困難です。";
                            break;
                        case "D4":
                            strErrorMsg = strDateName + "日付の月が間違いました。";
                            break;
                        case "D5":
                            strErrorMsg = strDateName + "日付の日は0より大きくなければなりません。";
                            break;
                        case "D6":
                            strErrorMsg = strDateName + "日付の日が間違いました。";
                            break;
                    }
                    break;
                // default Korean
                default:
                    switch (strCode)
                    {
                        case "D0":
                            strErrorMsg = strDateName + " 일자를 입력해 주십시오.";
                            break;
                        case "D1":
                            strErrorMsg = strDateName + " 일자가 전부 입력되지 않았습니다.";
                            break;
                        case "D2":
                            strErrorMsg = strDateName + " 일자의 길이가 너무 깁니다.";
                            break;
                        case "D3":
                            strErrorMsg = strDateName + " 일자는 1000년 이전의 입력이 곤란합니다.";
                            break;
                        case "D4":
                            strErrorMsg = strDateName + " 일자의 월이 잘못되었습니다.";
                            break;
                        case "D5":
                            strErrorMsg = strDateName + " 일자의 일은 0보다 커야 합니다.";
                            break;
                        case "D6":
                            strErrorMsg = strDateName + " 일자의 일이 잘못되었습니다.";
                            break;
                    }
                    break;
            }

            alResult = EncodeData((null == strErrorCode || "" == strErrorCode) ? strCode : strErrorCode, strErrorMsg, null);
            if ("00" != strCode)
                return false;
            else
                return true;
        }

        // Input : 6 Byte Time ( hhmmss )
        public static bool TimeValidate(string strTimeName, string strTime, string strLangID, ref ArrayList alResult, string strErrorCode)
        {
            string strCode = "00", strErrorMsg = "";
            alResult = new ArrayList();
            if (null == strTime)
                strCode = "T0";
            else if (6 > strTime.Length)
                strCode = "T1";
            else if (6 < strTime.Length)
                strCode = "T2";

            if ("00" == strCode)
            {
                int iHour = Toi(strTime.Substring(0, 2));
                int iMinute = Toi(strTime.Substring(2, 2));
                int iSecond = Toi(strTime.Substring(4, 2));

                if (iHour > 23 || iHour < 0 || (iHour == 0 && strTime.Substring(0, 2) != "00"))
                    strCode = "T3";
                if (iMinute > 59 || iMinute < 0 || (iMinute == 0 && strTime.Substring(2, 2) != "00"))
                    strCode = "T4";
                if (iSecond > 59 || iSecond < 0 || (iSecond == 0 && strTime.Substring(4, 2) != "00"))
                    strCode = "T5";
            }

            switch (strLangID.ToLower())
            {
                case "eng":
                    {
                        switch (strCode)
                        {
                            case "T0":
                                strErrorMsg = "Please enter the time for " + strTimeName;
                                break;
                            case "T1":
                                strErrorMsg = "You have not entered the whole time for " + strTimeName;
                                break;
                            case "T2":
                                strErrorMsg = "The time for " + strTimeName + " is too long.";
                                break;
                            case "T3":
                                strErrorMsg = "The hour value of the time " + strTimeName + " is invalid.";
                                break;
                            case "T4":
                                strErrorMsg = "The minute value of the time " + strTimeName + " is invalid.";
                                break;
                            case "T5":
                                strErrorMsg = "The second value of the time " + strTimeName + " is invalid.";
                                break;
                        }
                        break;
                    }
                case "chn":
                    {
                        switch (strCode)
                        {
                            case "T0":
                                strErrorMsg = "请输入" + strTimeName + "时间";
                                break;
                            case "T1":
                                strErrorMsg = strTimeName + "时间没有全被输入。";
                                break;
                            case "T2":
                                strErrorMsg = strTimeName + "时间太长。";
                                break;
                            case "T3":
                                strErrorMsg = strTimeName + "时间的时错误。";
                                break;
                            case "T4":
                                strErrorMsg = strTimeName + "时间的分有误。";
                                break;
                            case "T5":
                                strErrorMsg = strTimeName + "时间的秒有误。";
                                break;
                        }
                        break;
                    }
                case "jpn":
                    {
                        switch (strCode)
                        {
                            case "T0":
                                strErrorMsg = strTimeName + " 時間を入力してください。";
                                break;
                            case "T1":
                                strErrorMsg = strTimeName + " 時間が全部入力になっていません。";
                                break;
                            case "T2":
                                strErrorMsg = strTimeName + " 時間の長さが長すぎます。";
                                break;
                            case "T3":
                                strErrorMsg = strTimeName + " 時間の時が誤りました。";
                                break;
                            case "T4":
                                strErrorMsg = strTimeName + " 時間の分が誤りました。";
                                break;
                            case "T5":
                                strErrorMsg = strTimeName + " 時間の秒が誤りました。";
                                break;
                        }
                        break;
                    }
                // default Korean
                default:
                    {
                        switch (strCode)
                        {
                            case "T0":
                                strErrorMsg = strTimeName + " 시간을 입력해 주십시오.";
                                break;
                            case "T1":
                                strErrorMsg = strTimeName + " 시간이 전부 입력되지 않았습니다.";
                                break;
                            case "T2":
                                strErrorMsg = strTimeName + " 시간의 길이가 너무 깁니다.";
                                break;
                            case "T3":
                                strErrorMsg = strTimeName + " 시간의 시가 잘못되었습니다.";
                                break;
                            case "T4":
                                strErrorMsg = strTimeName + " 시간의 분이 잘못되었습니다.";
                                break;
                            case "T5":
                                strErrorMsg = strTimeName + " 시간의 초가 잘못되었습니다.";
                                break;
                        }
                        break;
                    }
            }

            alResult = EncodeData((null == strErrorCode || "" == strErrorCode) ? strCode : strErrorCode, strErrorMsg, null);
            if ("00" != strCode)
                return false;
            else
                return true;
        }

        // Input : 14 Byte Time ( yyyymmddhhmmss )
        public static bool DateTimeValidate(string strDateTimeName, string strDateTime, string strLangID, ref ArrayList alResult, string strErrorCode)
        {
            string strErrorMsg = "";
            if (strDateTime == null || strDateTime.Length != 14)
            {
                switch (strLangID.ToLower())
                {
                    case "eng":
                        strErrorMsg = "You have not entered the whole time for " + strDateTimeName;
                        break;
                    case "chn":
                        strErrorMsg = strDateTimeName + " 时间没有全被输入。";
                        break;
                    case "jpn":
                        strErrorMsg = strDateTimeName + " 時間が全部入力になっていません。";
                        break;
                    default:
                        strErrorMsg = strDateTimeName + " 시간이 전부 입력되지 않았습니다.";
                        break;
                }
                alResult = EncodeData((null == strErrorCode || "" == strErrorCode) ? "T1" : strErrorCode, strErrorMsg, null);
                return false;
            }
            if (!DateValidate(strDateTimeName, strDateTime.Substring(0, 8), strLangID, ref alResult, strErrorCode))
                return false;
            if (!TimeValidate(strDateTimeName, strDateTime.Substring(8), strLangID, ref alResult, strErrorCode))
                return false;

            return true;
        }


        public static bool PeriodValidate(string strBdateName, string strBdate, string strEdateName, string strEdate, string strLangID, ref ArrayList alResult)
        {
            return PeriodValidate(strBdateName, strBdate, strEdateName, strEdate, strLangID, ref alResult, null);
        }

        public static bool PeriodValidate(string strBdateName, string strBdate, string strEdateName, string strEdate, string strLangID, ref ArrayList alResult, string strErrorCode)
        {
            string strCode = "00", strErrorMsg = "";
            alResult = new ArrayList();
            if (!DateValidate(strBdateName, strBdate, strLangID, ref alResult, strErrorCode))
                return false;
            if (!DateValidate(strEdateName, strEdate, strLangID, ref alResult, strErrorCode))
                return false;
            decimal dBdate = Convert.ToDecimal(strBdate), dEdate = Convert.ToDecimal(strEdate);
            if (dBdate > dEdate)
            {
                strCode = (null == strErrorCode || "" == strErrorCode) ? "P0" : strErrorCode;
                switch (strLangID.ToLower())
                {
                    // default Korean
                    case "eng":
                        strErrorMsg = "The date " + strEdateName + " is passed the date " + strBdateName;
                        break;
                    case "chn":
                        strErrorMsg = strBdateName + "日期 超出 " + strEdateName + "日期。";
                        break;
                    case "jpn":
                        strErrorMsg = strBdateName + "日付が" + strEdateName + "日付を過ぎました。";
                        break;
                    default:
                        strErrorMsg = strBdateName + "일자가" + strEdateName + "일자를 지났습니다.";
                        break;
                }
                alResult = EncodeData(strCode, strErrorMsg, null);
                return false;
            }
            return true;
        }

        public enum DCType { DAddSlash, DRemSlash, DTMAddSlash, DTMRemSlash, DTSAddSlash, DTSRemSlash, HMSAddColon, HMSRemColon, HMAddColon, HMRemColon };
        public static string DateConvert(string strDate, DCType Type)
        {
            if (null == strDate)
                return strDate;

            switch (Type)
            {
                case DCType.DAddSlash:
                    if (8 == strDate.Length)
                        strDate = strDate.Substring(0, 4) + "/" + strDate.Substring(4, 2) + "/" + strDate.Substring(6, 2);
                    break;
                case DCType.DRemSlash:
                    if (10 == strDate.Length)
                        strDate = strDate.Substring(0, 4) + strDate.Substring(5, 2) + strDate.Substring(8, 2);
                    break;
                case DCType.DTMAddSlash:
                    if (12 == strDate.Length)
                        strDate = strDate.Substring(0, 4) + "/" + strDate.Substring(4, 2) + "/" + strDate.Substring(6, 2) + " " + strDate.Substring(8, 2) + ":" + strDate.Substring(10, 2);
                    break;
                case DCType.DTMRemSlash:
                    if (16 == strDate.Length)
                        strDate = strDate.Substring(0, 4) + strDate.Substring(5, 2) + strDate.Substring(8, 2) + strDate.Substring(11, 2) + strDate.Substring(14, 2);
                    break;
                case DCType.DTSAddSlash:
                    if (14 == strDate.Length)
                        strDate = strDate.Substring(0, 4) + "/" + strDate.Substring(4, 2) + "/" + strDate.Substring(6, 2) + " " + strDate.Substring(8, 2) + ":" + strDate.Substring(10, 2) + ":" + strDate.Substring(12, 2);
                    break;
                case DCType.DTSRemSlash:
                    if (19 == strDate.Length)
                        strDate = strDate.Substring(0, 4) + strDate.Substring(5, 2) + strDate.Substring(8, 2) + strDate.Substring(11, 2) + strDate.Substring(14, 2) + strDate.Substring(17, 2);
                    break;
                case DCType.HMSAddColon:
                    if (6 == strDate.Length)
                        strDate = strDate.Substring(0, 2) + ":" + strDate.Substring(2, 2) + ":" + strDate.Substring(4, 2);
                    break;
                case DCType.HMSRemColon:
                    if (8 == strDate.Length)
                        strDate = strDate.Substring(0, 2) + strDate.Substring(3, 2) + strDate.Substring(6, 2);
                    break;
                case DCType.HMAddColon:
                    if (4 == strDate.Length)
                        strDate = strDate.Substring(0, 2) + ":" + strDate.Substring(2, 2);
                    break;
                case DCType.HMRemColon:
                    if (5 == strDate.Length)
                        strDate = strDate.Substring(0, 2) + strDate.Substring(3, 2);
                    break;
                default:
                    break;
            }
            return strDate;
        }

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

        public static string DateAdd(string strDate, int iYear, int iMonth, int iDay)
        {
            // if not Valid Date then use CurDate
            bool bCurDate = false;
            if (strDate == null || strDate.Length != 8)
                bCurDate = true;

            // Extract Year and Add
            int iYearTemp = bCurDate ? DateTime.Now.Year : Convert.ToInt32(strDate.Substring(0, 4));
            // Extract Month and Add
            int iMonthTemp = bCurDate ? DateTime.Now.Month : Convert.ToInt32(strDate.Substring(4, 2));
            // Extract Date
            DateTime dt = new DateTime(iYearTemp, iMonthTemp, bCurDate ? DateTime.Now.Day : Convert.ToInt32(strDate.Substring(6, 2)));
            dt = dt.AddDays(iDay);
            dt = dt.AddMonths(iMonth);
            dt = dt.AddYears(iYear);

            // Make Dates
            strDate = dt.Year.ToString();
            string strTemp = "0" + dt.Month.ToString();
            strDate += strTemp.Substring(strTemp.Length - 2, 2);
            strTemp = "0" + dt.Day.ToString();
            strDate += strTemp.Substring(strTemp.Length - 2, 2);
            return strDate;
        }

        public static int DateDiff(string strDateSrc, string strDateRem)
        {
            return DateDiff(strDateSrc, strDateRem, DDType.YYYYMMDD);
        }

        public enum DDType { YYYYMMDD, YYYYMM, YYYY };
        public static int DateDiff(string strDateSrc, string strDateRem, DDType type)
        {
            int iYearSrc, iYearRem, iMonthSrc, iMonthRem, iResult = 0, iYearResult, iMonthResult;
            switch (type)
            {
                case DDType.YYYYMMDD:
                    if (strDateSrc.Length != 8)
                        strDateSrc = DateConvert(strDateSrc, DCType.DRemSlash);
                    if (strDateRem.Length != 8)
                        strDateRem = DateConvert(strDateRem, DCType.DRemSlash);
                    // if not Valid Date then use CurDate
                    DateTime dtSrc = new DateTime(Toi(strDateSrc.Substring(0, 4)), Toi(strDateSrc.Substring(4, 2)), Toi(strDateSrc.Substring(6, 2)));
                    DateTime dtRem = new DateTime(Toi(strDateRem.Substring(0, 4)), Toi(strDateRem.Substring(4, 2)), Toi(strDateRem.Substring(6, 2)));
                    TimeSpan ts = dtSrc - dtRem;
                    iResult = ts.Days;
                    break;
                case DDType.YYYYMM:
                    if (strDateSrc.Length != 6)
                        strDateSrc = DateConvert(strDateSrc + "/01", DCType.DRemSlash);
                    if (strDateRem.Length != 6)
                        strDateRem = DateConvert(strDateRem + "/01", DCType.DRemSlash);
                    iYearSrc = Toi(strDateSrc.Substring(0, 4));
                    iYearRem = Toi(strDateRem.Substring(0, 4));
                    iMonthSrc = Toi(strDateSrc.Substring(4, 2));
                    iMonthRem = Toi(strDateRem.Substring(4, 2));
                    iYearResult = iYearSrc - iYearRem;
                    iMonthResult = iMonthSrc - iMonthRem;
                    iResult = iYearResult * 12 + iMonthResult;
                    break;
                case DDType.YYYY:
                    iYearSrc = Toi(strDateSrc.Substring(0, 4));
                    iYearRem = Toi(strDateRem.Substring(0, 4));
                    iResult = iYearSrc - iYearRem;
                    break;
            }
            return iResult;
        }

        #endregion

        #region Resource Handling

        // Common Resource
        public static string CRS(string strMsgCode, string strCategory, CultureInfo culture)
        {
            string strLangID = "";
            switch (culture.TwoLetterISOLanguageName)
            {
                case "ko":
                    strLangID = "kor";
                    break;
                case "ja":
                    strLangID = "jpn";
                    break;
                case "zh":
                    strLangID = "chn";
                    break;
                default:
                    strLangID = "eng";
                    break;
            }
            return CRS(strMsgCode, strCategory, strLangID);
        }

        public static string CRS(string strMsgCode, string strCategory, string strLangID)
        {
            return RS(strMsgCode, "Common/Resource/" + strCategory, null, strLangID, "ResourceRoot");
        }

        public static string RS(string strMsgCode, object FormClass, string strLangID)
        {
            string strName = FormClass.GetType().Name;
            try
            {
                if (FormClass.ToString().IndexOf(".DA.") > -1)
                {
                    string strTemp = FormClass.ToString().Substring(FormClass.ToString().LastIndexOf("."));
                    string strClass = FormClass.ToString().Replace(strTemp, "");
                    string strModule = strClass.Substring(strClass.Length - 5, 2);
                    string strDir = "SVC\\";
                    switch (strModule)
                    {
                        case "OM":
                            strDir += "MNGProject";
                            break;
                        case "EX":
                            strDir += "LOGProject";
                            break;
                        case "IM":
                            strDir += "LOGProject";
                            break;
                        case "MM":
                            strDir += "LOGProject";
                            break;
                        case "OS":
                            strDir += "LOGProject";
                            break;
                        case "PM":
                            strDir += "LOGProject";
                            break;
                        case "PO":
                            strDir += "LOGProject";
                            break;
                        case "PP":
                            strDir += "LOGProject";
                            break;
                        case "QM":
                            strDir += "LOGProject";
                            break;
                        case "SD":
                            strDir += "LOGProject";
                            break;
                        case "SM":
                            strDir += "LOGProject";
                            break;

                        // NP, NPP 추가 : 2014.04.10
                        case "NP":
                            strDir += "LOGProject";
                            break;
                        case "NPP":
                            strDir += "LOGProject";
                            break;

                        case "HR":
                            strDir += "HRMProject";
                            break;
                        case "BG":
                            strDir += "FINProject";
                            break;
                        case "AA":
                            strDir += "FINProject";
                            break;
                        case "CO":
                            strDir += "FINProject";
                            break;
                        case "GL":
                            strDir += "FINProject";
                            break;
                        case "TR":
                            strDir += "FINProject";
                            break;
                        case "ES":
                            strDir += "ESMProject";
                            break;
                        default:
                            strDir += "";
                            break;
                    }
                    string strFileName = c_ResourceRoot + strDir + "\\" + strClass + "\\" + strName + "_" + strLangID + ".resx";
                    ResXResourceSet rsSet = new ResXResourceSet(strFileName);
                    return rsSet.GetString(strMsgCode, true);
                }

                strName = strName.Replace(FormClass.GetType().BaseType.Name.ToLower() + "_aspx", "");
                strName = strName.Replace("_", "\\");
                return RS(strMsgCode, strName, FormClass.GetType().BaseType.Name, strLangID);
            }
            catch
            {
                return "";
            }
        }

        public static string RS(string strMsgCode, object FormClass, string strBaseType, string strLangID)
        {
            string strTemp = FormClass.GetType().Name;
            string strFileName = "";

            try
            {
                if (strBaseType != "" || null != strBaseType)
                    strTemp = strTemp.Replace(FormClass.GetType().BaseType.Name.ToLower() + "_aspx", "");
                else
                    strTemp = strTemp.Replace("_aspx", "");

                strTemp = strTemp.Replace("_", "\\");
                strFileName = c_ResourceRoot + "\\" + strTemp + strBaseType + "_" + strLangID + ".resx";
                ResXResourceSet rsSet = new ResXResourceSet(strFileName);
                return rsSet.GetString(strMsgCode, true);
            }
            catch
            {
                return "";
            }
        }

        public static string RS(string strMsgCode, string strFormClassName, string strBaseType, string strLangID)
        {
            string strFileName = "";
            string strProfileName = "ResourceRoot";
            strFileName = c_ResourceRoot + "\\" + strFormClassName + strBaseType + "_" + strLangID + ".resx";

            try
            {
                ResXResourceSet rsSet = new ResXResourceSet(strFileName);
                return rsSet.GetString(strMsgCode, true);
            }
            catch
            {
                return "";
            }
        }



        public static string RS(string strMsgCode, object FormClass, HttpServerUtility Server, string strLangID)
        {

            string strTemp = FormClass.GetType().Name;
            try
            {
                if (Server != null && File.Exists(Server.MapPath(strTemp)))
                    return RS(strMsgCode, strTemp, Server, strLangID, "");
                else
                {
                    // Eliminate SMART.
                    strTemp = FormClass.GetType().ToString();
                    if (strTemp.IndexOf(".") > -1)
                    {
                        strTemp = strTemp.Substring(strTemp.IndexOf("."));
                    }
                    strTemp = strTemp.Replace(".", "/");
                    if (Server != null && File.Exists(Server.MapPath(strTemp)))
                        return RS(strMsgCode, strTemp, Server, strLangID, "");
                    else
                    {
                        strTemp = strTemp.Replace("/", "\\");
                        return RS(strMsgCode, strTemp, Server, strLangID, "ResourceRoot");
                    }
                }
            }
            catch
            {
                return "";
            }
        }

        public static string RS(string strMsgCode, string strFormClassName, HttpServerUtility Server, string strLangID)
        {
            return RS(strMsgCode, strFormClassName, Server, strLangID, "");
        }

        private static string RS(string strMsgCode, string strFormClassName, HttpServerUtility Server, string strLangID, string strProfileName)
        {
            string strFileName = "";
            if (strProfileName != "")
                strFileName = System.Configuration.ConfigurationSettings.AppSettings[strProfileName] + strFormClassName + "_" + strLangID + ".resx";
            else
                strFileName = Server.MapPath(strFormClassName + "_" + strLangID + ".resx");

            try
            {
                ResXResourceSet rsSet = new ResXResourceSet(strFileName);
                return rsSet.GetString(strMsgCode, true);
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #region DataAccess

        public static void BeginTran(GData GD, ref SqlTransaction tran, ref SqlConnection con)
        {
            DataAccess.BeginTran(GD.ClientID, ref tran, ref con);
        }

        public static void CloseTran(ref SqlTransaction tran, ref SqlConnection con)
        {
            DataAccess.CloseTran(ref tran, ref con);
        }

        public static SqlConnection GetDBConnection(GData GD)
        {
            return DataAccess.GetDBConnection(GD);
        }

        public static int ExecuteTran(GData GD, string commandText, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con)
        {
            return ExecuteTran(GD, commandText, null, commandType, ref tran, ref con, (con == null));
        }

        public static int ExecuteTran(GData GD, string commandText, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return ExecuteTran(GD, commandText, null, commandType, ref tran, ref con, start);
        }

        public static int ExecuteTran(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return DataAccess.ExecuteTran(GD.ClientID, commandText, sqlParameters, commandType, ref tran, ref con, start, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static SqlDataReader ExecuteReader(string clientID, string commandText, CommandType commandType)
        {
            return DataAccess.ExecuteReader(clientID, commandText, null, commandType, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static SqlDataReader ExecuteReader(GData GD, string commandText, CommandType commandType)
        {
            return ExecuteReader(GD, commandText, null, commandType);
        }

        public static SqlDataReader ExecuteReader(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType)
        {
            return DataAccess.ExecuteReader(GD, commandText, sqlParameters, commandType, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static SqlDataReader ExecuteReaderTran(string clientID, string commandText, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con)
        {
            return DataAccess.ExecuteReaderTran(clientID, commandText, null, commandType, ref tran, ref con, (con == null), ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static SqlDataReader ExecuteReaderTran(string clientID, string commandText, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return DataAccess.ExecuteReaderTran(clientID, commandText, null, commandType, ref tran, ref con, start, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static SqlDataReader ExecuteReaderTran(GData GD, string commandText, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con)
        {
            return ExecuteReaderTran(GD, commandText, null, commandType, ref tran, ref con, (con == null));
        }

        public static SqlDataReader ExecuteReaderTran(GData GD, string commandText, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return ExecuteReaderTran(GD, commandText, null, commandType, ref tran, ref con, start);
        }

        public static SqlDataReader ExecuteReaderTran(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con)
        {
            return ExecuteReaderTran(GD, commandText, sqlParameters, commandType, ref tran, ref con, (con == null));
        }

        public static SqlDataReader ExecuteReaderTran(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return DataAccess.ExecuteReaderTran(GD, commandText, sqlParameters, commandType, ref tran, ref con, start, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataTableReader ExecuteDataTableReader(string clientID, string commandText, CommandType commandType)
        {
            return DataAccess.ExecuteDataTableReader(clientID, commandText, null, commandType, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataTableReader ExecuteDataTableReader(GData GD, string commandText, CommandType commandType)
        {
            return ExecuteDataTableReader(GD, commandText, null, commandType);
        }

        public static DataTableReader ExecuteDataTableReader(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType)
        {
            return DataAccess.ExecuteDataTableReader(GD, commandText, sqlParameters, commandType, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataTableReader ExecuteDataTableReaderTran(string clientID, string commandText, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con)
        {
            return DataAccess.ExecuteDataTableReaderTran(clientID, commandText, null, commandType, ref tran, ref con, (con == null), ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataTableReader ExecuteDataTableReaderTran(string clientID, string commandText, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return DataAccess.ExecuteDataTableReaderTran(clientID, commandText, null, commandType, ref tran, ref con, start, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataTableReader ExecuteDataTableReaderTran(GData GD, string commandText, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con)
        {
            return ExecuteDataTableReaderTran(GD, commandText, null, commandType, ref tran, ref con, (con == null));
        }

        public static DataTableReader ExecuteDataTableReaderTran(GData GD, string commandText, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return ExecuteDataTableReaderTran(GD, commandText, null, commandType, ref tran, ref con, start);
        }

        public static DataTableReader ExecuteDataTableReaderTran(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con)
        {
            return ExecuteDataTableReaderTran(GD, commandText, sqlParameters, commandType, ref tran, ref con, (con == null));
        }

        public static DataTableReader ExecuteDataTableReaderTran(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return DataAccess.ExecuteDataTableReaderTran(GD, commandText, sqlParameters, commandType, ref tran, ref con, start, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSet(GData GD, string commandText, CommandType commandType)
        {
            return DataAccess.GetDataSet(GD, commandText, null, commandType, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSet(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType)
        {
            return DataAccess.GetDataSet(GD, commandText, sqlParameters, commandType, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSet(GData GD, string commandText, CommandType commandType, string strTableName, int pageIndex, int pageSize)
        {
            return DataAccess.GetDataSet(GD, commandText, null, commandType, strTableName, pageIndex, pageSize, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSet(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string strTableName, int pageIndex, int pageSize)
        {
            return DataAccess.GetDataSet(GD, commandText, sqlParameters, commandType, strTableName, pageIndex, pageSize, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSetTran(GData GD, string commandText, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con)
        {
            return DataAccess.GetDataSetTran(GD, commandText, null, commandType, ref tran, ref con, (con == null), ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSetTran(GData GD, string commandText, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return DataAccess.GetDataSetTran(GD, commandText, null, commandType, ref tran, ref con, start, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSetTran(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con)
        {
            return DataAccess.GetDataSetTran(GD, commandText, sqlParameters, commandType, ref tran, ref con, (con == null), ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSetTran(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return DataAccess.GetDataSetTran(GD, commandText, sqlParameters, commandType, ref tran, ref con, start, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSetTran(GData GD, string commandText, CommandType commandType, string strTableName, int pageIndex, int pageSize, ref SqlTransaction tran, ref SqlConnection con)
        {
            return DataAccess.GetDataSetTran(GD, commandText, null, commandType, strTableName, pageIndex, pageSize, ref tran, ref con, (con == null), ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSetTran(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string strTableName, int pageIndex, int pageSize, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return DataAccess.GetDataSetTran(GD, commandText, sqlParameters, commandType, strTableName, pageIndex, pageSize, ref tran, ref con, start, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static object ExecuteScalar(GData GD, string commandText, CommandType commandType)
        {
            return DataAccess.ExecuteScalar(GD, commandText, null, commandType, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static object ExecuteScalar(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType)
        {
            return DataAccess.ExecuteScalar(GD, commandText, sqlParameters, commandType, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static int ExecuteRightCheck(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType)
        {
            return DataAccess.ExecuteNonQuery(GD, commandText, sqlParameters, commandType, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static int ExecuteNonQuery(GData GD, string commandText, CommandType commandType)
        {
            return ExecuteNonQuery(GD, commandText, null, commandType);
        }

        public static int ExecuteNonQuery(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType)
        {
            return DataAccess.ExecuteNonQuery(GD, commandText, sqlParameters, commandType, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static int ExecuteMultiple(GData GD, string commandText, SqlParameter[] sqlParameters, string[,] paramValues, CommandType commandType)
        {
            return DataAccess.ExecuteMultiple(GD, commandText, sqlParameters, paramValues, commandType, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static SqlConnection GetDBConnection(GObj G)
        {
            return DataAccess.GetDBConnection(G);
        }

        public static void BeginTran(GObj G, ref SqlTransaction tran, ref SqlConnection con)
        {
            DataAccess.BeginTran(G.ClientID, ref tran, ref con);
        }

        public static int ExecuteTran(GObj G, string commandText, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con)
        {
            return ExecuteTran(G, commandText, null, commandType, ref tran, ref con, con == null);
        }

        public static int ExecuteTran(GObj G, string commandText, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return ExecuteTran(G, commandText, null, commandType, ref tran, ref con, start);
        }

        public static int ExecuteTran(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return DataAccess.ExecuteTran(G.ClientID, commandText, sqlParameters, commandType, ref tran, ref con, start, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSet(GObj G, string commandText, CommandType commandType)
        {
            return DataAccess.GetDataSet(G, commandText, null, commandType, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSet(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType)
        {
            return DataAccess.GetDataSet(G, commandText, sqlParameters, commandType, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSet(GObj G, string commandText, CommandType commandType, string strTableName, int pageIndex, int pageSize)
        {
            return DataAccess.GetDataSet(G, commandText, null, commandType, strTableName, pageIndex, pageSize, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSet(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string strTableName, int pageIndex, int pageSize)
        {
            return DataAccess.GetDataSet(G, commandText, sqlParameters, commandType, strTableName, pageIndex, pageSize, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static SqlDataReader ExecuteReader(GObj G, string commandText, CommandType commandType)
        {
            return ExecuteReader(G, commandText, null, commandType);
        }

        public static SqlDataReader ExecuteReader(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType)
        {
            return DataAccess.ExecuteReader(G, commandText, sqlParameters, commandType, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static SqlDataReader ExecuteReaderTran(GObj G, string commandText, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con)
        {
            return ExecuteReaderTran(G, commandText, null, commandType, ref tran, ref con, (con == null));
        }

        public static SqlDataReader ExecuteReaderTran(GObj G, string commandText, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return ExecuteReaderTran(G, commandText, null, commandType, ref tran, ref con, start);
        }

        public static SqlDataReader ExecuteReaderTran(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con)
        {
            return ExecuteReaderTran(G, commandText, sqlParameters, commandType, ref tran, ref con, (con == null));
        }

        public static SqlDataReader ExecuteReaderTran(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return DataAccess.ExecuteReaderTran(G, commandText, sqlParameters, commandType, ref tran, ref con, start, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataTableReader ExecuteDataTableReader(GObj G, string commandText, CommandType commandType)
        {
            return ExecuteDataTableReader(G, commandText, null, commandType);
        }

        public static DataTableReader ExecuteDataTableReader(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType)
        {
            return DataAccess.ExecuteDataTableReader(G, commandText, sqlParameters, commandType, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataTableReader ExecuteDataTableReaderTran(GObj G, string commandText, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con)
        {
            return ExecuteDataTableReaderTran(G, commandText, null, commandType, ref tran, ref con, (con == null));
        }

        public static DataTableReader ExecuteDataTableReaderTran(GObj G, string commandText, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return ExecuteDataTableReaderTran(G, commandText, null, commandType, ref tran, ref con, start);
        }

        public static DataTableReader ExecuteDataTableReaderTran(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con)
        {
            return ExecuteDataTableReaderTran(G, commandText, sqlParameters, commandType, ref tran, ref con, (con == null));
        }

        public static DataTableReader ExecuteDataTableReaderTran(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return DataAccess.ExecuteDataTableReaderTran(G, commandText, sqlParameters, commandType, ref tran, ref con, start, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSetTran(GObj G, string commandText, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con)
        {
            return DataAccess.GetDataSetTran(G, commandText, null, commandType, ref tran, ref con, (con == null), ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSetTran(GObj G, string commandText, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return DataAccess.GetDataSetTran(G, commandText, null, commandType, ref tran, ref con, start, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSetTran(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con)
        {
            return DataAccess.GetDataSetTran(G, commandText, sqlParameters, commandType, ref tran, ref con, (con == null), ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSetTran(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return DataAccess.GetDataSetTran(G, commandText, sqlParameters, commandType, ref tran, ref con, start, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSetTran(GObj G, string commandText, CommandType commandType, string strTableName, int pageIndex, int pageSize, ref SqlTransaction tran, ref SqlConnection con)
        {
            return DataAccess.GetDataSetTran(G, commandText, null, commandType, strTableName, pageIndex, pageSize, ref tran, ref con, (con == null), ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSetTran(GObj G, string commandText, CommandType commandType, string strTableName, int pageIndex, int pageSize, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return DataAccess.GetDataSetTran(G, commandText, null, commandType, strTableName, pageIndex, pageSize, ref tran, ref con, start, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSetTran(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string strTableName, int pageIndex, int pageSize, ref SqlTransaction tran, ref SqlConnection con)
        {
            return DataAccess.GetDataSetTran(G, commandText, sqlParameters, commandType, strTableName, pageIndex, pageSize, ref tran, ref con, (con == null), ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static DataSet GetDataSetTran(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string strTableName, int pageIndex, int pageSize, ref SqlTransaction tran, ref SqlConnection con, bool start)
        {
            return DataAccess.GetDataSetTran(G, commandText, sqlParameters, commandType, strTableName, pageIndex, pageSize, ref tran, ref con, start, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static object ExecuteScalar(GObj G, string commandText, CommandType commandType)
        {
            return DataAccess.ExecuteScalar(G, commandText, null, commandType, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static object ExecuteScalar(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType)
        {
            return DataAccess.ExecuteScalar(G, commandText, sqlParameters, commandType, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static int ExecuteRightCheck(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType)
        {
            return DataAccess.ExecuteNonQuery(G, commandText, sqlParameters, commandType, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static int ExecuteNonQuery(GObj G, string commandText, CommandType commandType)
        {
            return ExecuteNonQuery(G, commandText, null, commandType);
        }

        public static int ExecuteNonQuery(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType)
        {
            return DataAccess.ExecuteNonQuery(G, commandText, sqlParameters, commandType, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        public static int ExecuteMultiple(GObj G, string commandText, SqlParameter[] sqlParameters, string[,] paramValues, CommandType commandType)
        {
            return DataAccess.ExecuteMultiple(G, commandText, sqlParameters, paramValues, commandType, ConfigurationManager.AppSettings["QueryLogMode"].Trim().ToLower(), ConfigurationManager.AppSettings["QueryLogPath"].Trim());
        }

        #endregion

        #region Log

        public static void LogToFile(string logFolder, string fileName, string contents)
        {
            if (!System.IO.Directory.Exists(logFolder))
                System.IO.Directory.CreateDirectory(logFolder);

            System.IO.FileStream oStream = new System.IO.FileStream(System.String.Concat(logFolder, "\\", fileName),
                System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);

            System.IO.StreamWriter oWriter = new System.IO.StreamWriter(oStream);

            oWriter.WriteLine(contents);

            oWriter.Flush();
            oWriter.Close();
        }

        public static void WriteEventLog(string eventLogSource, System.Diagnostics.EventLogEntryType logType, string contents)
        {
            System.Diagnostics.EventLog eLog = null;

            try
            {
                if (!(System.Diagnostics.EventLog.SourceExists(eventLogSource)))
                    System.Diagnostics.EventLog.CreateEventSource(eventLogSource, eventLogSource);

                eLog = new System.Diagnostics.EventLog(eventLogSource);
                eLog.Source = eventLogSource;
                eLog.WriteEntry(contents, logType);
            }
            finally
            {
                if (eLog != null) eLog.Dispose();
            }
        }

        #endregion

        # region Data Row[0] 컬럼값을 DevExpress HiddenField 로 복사함 조회된 데이터를 컬럼 명에 맞게 HashTable 형식인 HidenField 에 넣어준다(HashTable 구조 : Key, Value)
        public static void setDataSetToDevHiddenField(DataSet ds, DevExpress.Web.ASPxHiddenField hidField)
        {
            DataTable dt = ds.Tables[0];

            foreach (DataColumn dc in dt.Columns)
            {
                string col = dc.ColumnName.ToUpper();       //컬럼명을 대문자로 변환함!

                if (dt.Rows.Count == 0)
                {
                    hidField[col] = "";
                }
                else
                {
                    hidField[col] = dt.Rows[0][col].ToString().Trim();
                }
            }
        }
        #endregion

        # region Data Row[0] 컬럼값을 DevExpress HiddenField 로 복사함 조회된 데이터를 컬럼 명에 맞게 HashTable 형식인 HidenField 에 넣어준다(HashTable 구조 : Key, Value)
        public static void setDataTableToDevHiddenField(DataTable dt, DevExpress.Web.ASPxHiddenField hidField)
        {
            foreach (DataColumn dc in dt.Columns)
            {
                string col = dc.ColumnName.ToUpper();       //컬럼명을 대문자로 변환함!

                if (dt.Rows.Count == 0)
                {
                    hidField[col] = "";
                }
                else
                {
                    hidField[col] = dt.Rows[0][col].ToString().Trim();
                }
            }
        }
        #endregion

        #region TimeChallenge

        public static void TimeChallenge(string strMark)
        {
#if ( TIMECHALLENGE )
				StreamWriter sw = new StreamWriter ( "c:\\TimeChallenge.txt", true, System.Text.Encoding.UTF8 ) ;
				sw.Write ( strMark + " : " + DateTime.Now.ToString() + System.Environment.NewLine );
				sw.Close();
#endif
        }

        #endregion

        #region Order에서 쓰이는 용

        public DataSet ds { get; set; }
        public int errCode { get; set; }
        public string msg { get; set; }
        public string retData { get; set; }

        //Data 가져오는 함수
        public void SetData(DataSet v_ds, int v_errCode, string v_msg)
        {
            this.ds = v_ds;
            this.errCode = v_errCode;
            this.msg = v_msg;
        }

        //Data 가져오는 함수 (추가 데이터를 넘겨 받을 수 있는 함수)
        public void SetData(DataSet v_ds, int v_errCode, string v_msg, string v_retData)
        {
            this.ds = v_ds;
            this.errCode = v_errCode;
            this.msg = v_msg;
            this.retData = v_retData;
        }

        #endregion

        #region 수정된 그리드 데이터 배열로 가져오기
        public static DataTable GetDataCols(DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e, ASPxGridView Grid)
        {
            string[] sarKeyFieldName = Grid.KeyFieldName.Split(';');

            DataTable dt = new DataTable();

            for (int i = 0; i < Grid.DataColumns.Count; i++)
            {
                if (GetStr(Grid.DataColumns[i].FieldName) != "")
                {
                    dt.Columns.Add(Grid.DataColumns[i].FieldName);
                }
            }

            //반환용 데이터테이블
            DataTable rdt = new DataTable();
            for (int i = 0; i < Grid.DataColumns.Count; i++)
            {
                if (GetStr(Grid.DataColumns[i].FieldName) != "")
                {
                    rdt.Columns.Add(Grid.DataColumns[i].FieldName);
                }
            }

            List<DevExpress.Web.Data.ASPxDataInsertValues> dataInsert = e.InsertValues;
            List<DevExpress.Web.Data.ASPxDataUpdateValues> dataUpdate = e.UpdateValues;
            List<DevExpress.Web.Data.ASPxDataDeleteValues> dataDelete = e.DeleteValues;

            //추가된 내용이 없는 경우.
            int iCnt = (dataInsert.Count + dataUpdate.Count);
            if (iCnt == 0)
            {
                for (int i = 0; i < Grid.VisibleRowCount; i++)
                {
                    DataRow row = dt.NewRow();

                    for (int index = 0; index < dt.Columns.Count; index++)
                    {
                        row[dt.Columns[index].ColumnName] = CFL.GetStr(Grid.GetRowValues(i, dt.Columns[index].ColumnName));
                    }

                    dt.Rows.Add(row);
                }
            }
            else
            {
                //Update Part 1
                for (int i = 0; i < Grid.VisibleRowCount; i++)
                {
                    DataRow row = dt.NewRow();

                    for (int index = 0; index < dt.Columns.Count; index++)
                    {
                        row[dt.Columns[index].ColumnName] = CFL.GetStr(Grid.GetRowValues(i, dt.Columns[index].ColumnName));
                    }

                    dt.Rows.Add(row);
                }

                //Update Part 2
                for (int index = 0; index < dataUpdate.Count; index++)
                {
                    for (int dIndex = 0; dIndex < dt.Rows.Count; dIndex++)
                    {
                        int iKeyLength = 0;
                        for (int kIndex = 0; kIndex < sarKeyFieldName.Length; kIndex++)
                        {
                            if (CFL.GetStr(dt.Rows[dIndex][sarKeyFieldName[kIndex]]) == CFL.GetStr(dataUpdate[index].OldValues[sarKeyFieldName[kIndex]]))
                            {
                                iKeyLength++;
                            }
                        }

                        if (iKeyLength == sarKeyFieldName.Length)
                        {
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                dt.Rows[dIndex][dt.Columns[i].ColumnName] = CFL.GetStr(dataUpdate[index].NewValues[dt.Columns[i].ColumnName]);
                            }
                        }
                    }
                }

                //Insert
                for (int i = 0; i < dataInsert.Count; i++)
                {
                    DataRow row = dt.NewRow();

                    for (int index = 0; index < dt.Columns.Count; index++)
                    {
                        row[dt.Columns[index].ColumnName] = CFL.GetStr(dataInsert[i].NewValues[dt.Columns[index].ColumnName]);
                    }

                    dt.Rows.Add(row);
                }

            }

            //삭제필드가 있으면 키필드 비교 후 삭제.
            for (int dIndex = 0; dIndex < dt.Rows.Count; dIndex++)
            {
                /*
                int iKeyLength = 0;
                for (int index = 0; index < dataDelete.Count; index++)
                {
                    for (int kIndex = 0; kIndex < sarKeyFieldName.Length; kIndex++)
                    {
                        if (CFL.GetStr(dt.Rows[dIndex][sarKeyFieldName[kIndex]]) == CFL.GetStr(dataDelete[index].Keys[sarKeyFieldName[kIndex]]))
                        {
                            iKeyLength++;
                        }
                    }
                }

                //
                //if (iKeyLength == sarKeyFieldName.Length)
                //{
                //    dt.Rows[dIndex].Delete();
                //}
                //기준 테이블인 dt에서 삭제를 하게되면 dt.Rows.Count, Row의 Index가 변경되어 정상적인 값을 반환하지 못함.
                //삭제가 아니라 새로운 DataTable rdt에 넣고 rdt를 반환
                if (iKeyLength != sarKeyFieldName.Length)
                {
                    DataRow dRow = rdt.NewRow();
                    for (int i = 0; i < rdt.Columns.Count; i++)
                    {
                        dRow[rdt.Columns[i].ColumnName] = dt.Rows[dIndex][dt.Columns[i].ColumnName];
                    }
                    rdt.Rows.Add(dRow);
                }
                */
                //키필드 개별로 비교할때 서로 다른 키필드의 값에 동일 값이 있을때 정상 작동하지 않아 키필드 값을 전부 String화시켜 전체비교하는 방법으로 수정함. 2022.12.07 YSJ
                Boolean blchk = false;
                string strKeyFN = "";
                string strKeyFND = "";
                for (int kIndex = 0; kIndex < sarKeyFieldName.Length; kIndex++)
                {
                    strKeyFN += CFL.GetStr(dt.Rows[dIndex][sarKeyFieldName[kIndex]]) + "|";
                }
                for (int index = 0; index < dataDelete.Count; index++)
                {
                    strKeyFND = "";
                    for (int kIndex = 0; kIndex < sarKeyFieldName.Length; kIndex++)
                    {
                        strKeyFND += CFL.GetStr(dataDelete[index].Keys[sarKeyFieldName[kIndex]]) + "|";
                    }
                    if (strKeyFN == strKeyFND)
                    {
                        blchk = true;
                    }
                }

                if (blchk == false)
                {
                    DataRow dRow = rdt.NewRow();
                    for (int i = 0; i < rdt.Columns.Count; i++)
                    {
                        dRow[rdt.Columns[i].ColumnName] = dt.Rows[dIndex][dt.Columns[i].ColumnName];
                    }
                    rdt.Rows.Add(dRow);
                }
            }

            return rdt;
        }
        #endregion

        # region CreateTextFile

        public static string CreateTextFile(string strClientID, string strSessionID, string strContents)
        {
            if (strClientID == null || strClientID == string.Empty || strSessionID == null || strSessionID == string.Empty)
                return string.Empty;

            Random rd = new Random();
            string strDirectory = GetProfileInfo(CFL.formRoot + "FilesBase") + "Temp\\" + strClientID + "\\";
            string strFileName = strSessionID;
            strFileName += DateTime.Now.Year;
            strFileName += DateTime.Now.Month;
            strFileName += DateTime.Now.Day;
            strFileName += DateTime.Now.Hour;
            strFileName += DateTime.Now.Minute;
            strFileName += DateTime.Now.Second;
            strFileName += DateTime.Now.Millisecond;
            strFileName += rd.Next(1000);
            strFileName += ".txt";

            string strFileURL = "/" + CFL.formRoot + "/Temp/" + strClientID + "/" + strFileName;
            strFileName = strDirectory + strFileName;
            try
            {
                if (!Directory.Exists(strDirectory))
                    Directory.CreateDirectory(strDirectory);

                StreamWriter sw = new StreamWriter(strFileName, false, Encoding.Default);
                sw.Write(strContents);
                sw.Close();
            }
            catch
            {
                return string.Empty;
            }

            return strFileURL;
        }

        public static string CreateTextFileFromEncodedData(string strClientID, string strSessionID, string strSeperator, object[] EncodedData)
        {
            if (EncodedData == null || EncodedData.Length < 4)
                return string.Empty;
            int iColCnt = Toi(EncodedData[2]);
            int iRowCnt = Toi(EncodedData[3]);
            if (EncodedData.Length != 4 + iColCnt * iRowCnt)
                return string.Empty;

            if (strSeperator == null)
                strSeperator = "\t";

            System.Text.StringBuilder sbContents = new System.Text.StringBuilder();
            for (int i = 0; i < (int)iRowCnt; i++)
            {
                for (int j = 0; j < (int)iColCnt; j++)
                {
                    sbContents.Append(EncodedData[4 + iColCnt * i + j].ToString());

                    if (iColCnt != j + 1)
                        sbContents.Append(strSeperator);
                }
                sbContents.Append(System.Environment.NewLine);
            }

            return CreateTextFile(strClientID, strSessionID, sbContents.ToString());
        }

        public static string CreateTextFileFromEncodedData(string strClientID, string strSessionID, string strSeperator, ArrayList EncodedData)
        {
            return CreateTextFileFromEncodedData(strClientID, strSessionID, strSeperator, EncodedData.ToArray());
        }

        public static string CreateTextFileFromDataCols(string strClientID, string strSessionID, string strSeperator, ArrayList DataCols)
        {
            int iColumnCnt = DataCols.Count;
            if (iColumnCnt < 1)
                return string.Empty;
            int iRowCnt = ((string[])DataCols[0]).Length;

            // Checkout for the Validity
            // Syncronized Row Count Check
            for (int i = 1; i < DataCols.Count; i++)
            {
                if (iRowCnt != ((string[])DataCols[i]).Length)
                    return string.Empty;
            }

            object[] EncodedData = new object[4 + iColumnCnt * iRowCnt];
            EncodedData[0] = "00";
            EncodedData[1] = "";
            EncodedData[2] = iColumnCnt;
            EncodedData[3] = iRowCnt;

            // Insert data Columns in Encoded format
            for (int i = 0; i < iRowCnt; i++)
                for (int j = 0; j < iColumnCnt; j++)
                    EncodedData[4 + i * iColumnCnt + j] = ((string[])DataCols[j])[i];

            return CreateTextFileFromEncodedData(strClientID, strSessionID, strSeperator, EncodedData);
        }

        # endregion

        #region TextFileExport
        public static string TextExport(string strClientID, string strSessionID, string strDescr, string strContents)
        {
            string strFileURL = CreateTextFile(strClientID, strSessionID, strContents);
            if (strFileURL == null || strFileURL == string.Empty)
            {
                return "N|Cannot Create Text File.";
//                return @"
//<script language=""JavaScript"">
//	alert('Cannot Create Text File.');
//</script>
//";
            }

            return "Y|" + strFileURL;
            //            return @"
            //<script language=""JavaScript"">
            //	window.open ( """ + System.Configuration.ConfigurationSettings.AppSettings["CommonFormsUrl"] + "FileDownload.aspx?FileName=" + HttpUtility.UrlEncode(strFileURL, System.Text.Encoding.GetEncoding("euc-kr")) + @"&Descr=" + HttpUtility.UrlEncode(strDescr, System.Text.Encoding.GetEncoding("euc-kr")) + @""", """"
            //		, ""Width=100,Height=100,Left=2048,Top=2048,menubar=no, toolbar=no, location=no, directories=no, status=no, scrollbars=no, resizable=no"" );
            //</script>
            //";
        }
        public static string TextExportFromEncodedData(string strClientID, string strSessionID, string strDescr, ArrayList EncodedData, string strSeperator)
        {
            if (strClientID == null || strSessionID == null || EncodedData == null)
                return "N|Null in Parameter ( ClientID, SessionID, EncodedData )";
//                return @"
//<script language=""JavaScript"">
//	this.location = """ + CFL.GetErrorURL("Back", "Parameter Exception", "Null in Parameter ( ClientID, SessionID, EncodedData )") + @"""
//</script>
//";

            if (strDescr == null)
                strDescr = "";

            if (EncodedData[0].ToString() != "00")
                return GetMsg(EncodedData[1].ToString());

            if (strSeperator == null)
                strSeperator = "\t";

            System.Text.StringBuilder sbContents = new System.Text.StringBuilder();

            for (int i = 0; i < (int)EncodedData[3]; i++)
            {
                for (int j = 0; j < (int)EncodedData[2]; j++)
                {
                    sbContents.Append(EncodedData[4 + (int)EncodedData[2] * i + j].ToString());

                    if ((int)EncodedData[2] != j + 1)
                    {
                        sbContents.Append(strSeperator);
                    }
                }

                sbContents.Append(System.Environment.NewLine);
            }

            return TextExport(strClientID, strSessionID, strDescr, sbContents.ToString());
        }

        public static string TextExportFromEncodedData(string strClientID, string strSessionID, string strDescr, ArrayList EncodedData)
        {
            return TextExportFromEncodedData(strClientID, strSessionID, strDescr, EncodedData, null);
        }


        public static string TextExportFromDataCols(string strClientID, string strSessionID, string strDescr, DataSet ds, string strSeperator)
        {
            if (strClientID == null || strSessionID == null || ds == null)
                return "N|Null in Parameter ( ClientID, SessionID, DataCols )";
//                return @"
//<script language=""JavaScript"">
//	this.location=""" + CFL.GetErrorURL("Back", "Parameter Exception", "Null in Parameter ( ClientID, SessionID, DataCols )") + @"""
//</script>
//";


            int iColumnCnt = ds.Tables[0].Columns.Count;
            int iRowCnt = ds.Tables[0].Rows.Count;

            // Checkout for the Validity
            // Syncronized Row Count Check
//            for (int i = 1; i < DataCols.Count; i++)
//            {
//                if (iRowCnt != ((string[])DataCols[i]).Length)
//                {
//                    return "N|Error in Data Cols";
////                    return @"
////<script language=""JavaScript"">
////	this.location=""" + CFL.GetErrorURL("Back", "DataFormat Exception", "Error in Data Cols") + @"""
////</script>
////";
//                }
//            }

            ArrayList EncodedData = new ArrayList();
            EncodedData.Add("00");
            EncodedData.Add("");
            EncodedData.Add(iColumnCnt);
            EncodedData.Add(iRowCnt);

            // insert data Columns in Encoded format
            for (int i = 0; i < iRowCnt; i++)
                for (int j = 0; j < iColumnCnt; j++)
                    EncodedData.Add(ds.Tables[0].Rows[i][j]);

            return TextExportFromEncodedData(strClientID, strSessionID, strDescr, EncodedData, strSeperator);
        }

        public static string TextExportFromDataCols(string strClientID, string strSessionID, string strDescr, DataSet ds)
        {
            //수정사항
            //기존 프로젝트의 Postback, Download.aspx, Download 컨트롤등 사용할 수 없는 제약이 있어 수정..
            //Return 값은 status값, (실패시)message, (성공시)생성된 Text파일의 URL
            //Format(성공시) Y|/SmartPro/SMART/Files/....
            //Format(실패시) N|Error in Data Cols
            //return값으로 split('|')하여 callBackPanel의 status값, msg, hidField["URL"]에 대입해서 Hyperlink 컨트롤에 URL을 셋팅하여 임시 사용하도록..
            return TextExportFromDataCols(strClientID, strSessionID, strDescr, ds, null);
        }

        #endregion

        #region 출력물 (By 김태진)
        public static void Print(MenuItemEventArgs e, ASPxGridViewExporter GridExport, HttpResponse Response, ASPxGridView Grid, string strTitleName, bool bGubun)
        {
            for (int i = 0; i < Grid.DataColumns.Count; i++)
            {
                if (GetStr(Grid.DataColumns[i].FieldName) != "")
                {
                    if (Grid.DataColumns[i].Width.Value == 0)
                    {
                        Grid.Columns[GetStr(Grid.DataColumns[i].FieldName)].Visible = false;
                    }
                }
            }

            string strGubun = "";
            string strFileName = "";

            if (e.Item.Name.ToUpper() == "EXCEL")
            {
                //엑셀
                GridExport.FileName = strTitleName;
                GridExport.GridViewID = Grid.ID;
                GridExport.Styles.Default.Font.Name = "맑은 고딕";
                GridExport.Styles.Default.Font.Size = 15;
                GridExport.Landscape = true;

                GridExport.PageHeader.Center = strTitleName;
                GridExport.PageHeader.Font.Size = 20;
                GridExport.PageHeader.Font.Name = "맑은 고딕";

                strGubun = "application/xlsx";
                strFileName = strTitleName + ".xlsx";
            }

            if (e.Item.Name.ToUpper() == "PDF")
            {
                GridExport.FileName = strTitleName;
                GridExport.GridViewID = Grid.ID;
                GridExport.Styles.Default.Font.Name = "맑은 고딕";
                GridExport.Landscape = true;

                //PDF
                GridExport.PageHeader.Center = strTitleName;
                GridExport.PageHeader.Font.Name = "맑은 고딕";
                GridExport.PageHeader.Font.Size = 20;
                GridExport.PageFooter.Center = "[Page # of Pages #]";

                GridExport.PaperKind = System.Drawing.Printing.PaperKind.A4;

                strGubun = "application/pdf";
                strFileName = strTitleName + ".pdf";
            }

            if (bGubun == true)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    PrintableComponentLinkBase pcl = new PrintableComponentLinkBase(new PrintingSystemBase());
                    pcl.Component = GridExport;
                    pcl.Margins.Left = pcl.Margins.Right = 50;
                    pcl.Landscape = true;
                    pcl.CreateDocument(false);
                    pcl.PrintingSystemBase.Document.AutoFitToPagesWidth = 1;
                    if (e.Item.Name.ToUpper() == "EXCEL")
                    {
                        pcl.ExportToXlsx(ms);
                    }
                    else
                    {
                        pcl.ExportToPdf(ms);
                    }
                    WriteResponse(Response, ms.ToArray(), System.Net.Mime.DispositionTypeNames.Inline.ToString(), strGubun, strFileName);

                    PdfExportOptions options = new PdfExportOptions();
                    options.Compressed = false;
                    options.ShowPrintDialogOnOpen = true;
                    GridExport.WritePdfToResponse(options);
                }
            }
            else
            {
                if (e.Item.Name.ToUpper() == "EXCEL")
                {
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.FitToPrintedPageWidth = true;
                    options.ExportType = ExportType.WYSIWYG;
                    GridExport.WriteXlsxToResponse(options);
                }
                else
                {
                    PdfExportOptions options = new PdfExportOptions();
                    options.Compressed = false;
                    options.ShowPrintDialogOnOpen = true;
                    GridExport.WritePdfToResponse(options);
                }
            }

            for (int i = 0; i < Grid.DataColumns.Count; i++)
            {
                if (GetStr(Grid.DataColumns[i].FieldName) != "")
                {
                    if (Grid.DataColumns[i].Width.Value != 0)
                    {
                        Grid.Columns[GetStr(Grid.DataColumns[i].FieldName)].Visible = true;
                    }
                }
            }
        }

        public static void WriteResponse(HttpResponse response, byte[] filearray, string type, string strGubun, string strFileName)
        {
            response.ClearContent();
            response.Buffer = true;
            response.Cache.SetCacheability(HttpCacheability.Private);
            response.ContentType = strGubun;
            ContentDisposition contentDisposition = new ContentDisposition();
            contentDisposition.FileName = strFileName;
            contentDisposition.DispositionType = type;
            response.ContentType = "text/rtf; charset=UTF-8";
            response.AddHeader("Content-Disposition", contentDisposition.ToString());
            response.BinaryWrite(filearray);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            try
            {
                response.End();
            }
            catch (System.Threading.ThreadAbortException)
            {
            }

        }
        #endregion

        // SHA256 - 암호화 모듈        
        public static string EncryptSHA256(string text)
        {
            var sha = new System.Security.Cryptography.SHA256Managed();

            byte[] data = sha.ComputeHash(Encoding.ASCII.GetBytes(text));

            var sb = new StringBuilder();

            foreach (byte b in data)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

    }
}