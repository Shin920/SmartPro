using DevExpress.Web;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace SMART
{
    public class CFL
    {
        public const string c_CookieNameUserID = "xErpUserID";
        public const string c_CookieNameSiteCode = "xErpSiteCode";
        public const string c_CookieNameLangID = "xErpLangID";
        public const string c_CookieNameClientID = "xErpClientID";
        public const int c_CookieExpiryDay = 30;
        public const int c_KeySize = 10;

        static public string GetIP()
        {
            string strIP = "";

            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (NetworkInterface adapter in interfaces)
            {
                foreach (IPAddress ip in host.AddressList)
                {
                    if ((adapter.OperationalStatus.ToString() == "Up") && // I have a problem with this condition
                        (ip.AddressFamily == AddressFamily.InterNetwork))
                    {
                        strIP = ip.ToString();
                    }
                }
            }

            return strIP;
        }

        static public string GetStr(object str)
        {
            string temp = "";
            try
            {
                temp = str.ToString().Trim();
            }
            catch
            {
                temp = "";
            }

            return temp;
        }

        //object -> Query str 값
        static public string Q(object str)
        {
            string temp = "";
            try
            {
                if (str.ToString().Trim() == "")
                {
                    temp = "null";
                }
                else
                {
                    temp = "'";
                    temp += str.ToString().Trim();
                    temp += "'";
                }
            }
            catch
            {
                temp = "null";
            }

            return temp;
        }

        //object -> int
        static public int Toi(object str)
        {
            int temp = 0;
            try
            {
                temp = int.Parse(str.ToString().Trim().Replace(",", ""));
            }
            catch
            {
                temp = 0;
            }

            return temp;
        }

        //object -> decimal
        static public decimal Tod(object str)
        {
            decimal temp = 0;
            try
            {
                temp = decimal.Parse(str.ToString().Trim().Replace(",", ""));
            }
            catch
            {
                temp = 0;
            }

            return temp;
        }


        //object -> bool
        static public bool Tob(object str)
        {
            bool temp = true;
            try
            {
                temp = bool.Parse(str.ToString().Trim());
            }
            catch
            {
                temp = false;
            }

            return temp;
        }

        //object -> double
        static public double Todb(object str)
        {
            double temp = 0;
            try
            {
                temp = double.Parse(str.ToString().Trim().Replace(",", ""));
            }
            catch
            {
                temp = 0;
            }

            return temp;
        }

        /*
        * Data Row[0] 컬럼값을 DevExpress HiddenField 로 복사함
        * */
        public static void setDataSetToDevHiddenField(DataSet ds, DevExpress.Web.ASPxHiddenField hidField)
        {
            DataTable dt = ds.Tables[0];
            foreach (DataColumn dc in dt.Columns)
            {
                string col = dc.ColumnName;       //컬럼명을 대문자로 변환 해도됨(현재는 안되어 있음)
                if (dt.Rows.Count == 0)
                {
                    hidField[col] = "";
                }
                else
                {
                    hidField[col] = dt.Rows[0][col].ToString();
                }
            }
        }

        static public string GetMsg(string str)
        {
            string strTemp = "";

            strTemp = @"
<script type='text/javascript'>
    alert('" + str + @"');
</script>
                       ";

            return strTemp;
        }


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
                if (iKeyLength == sarKeyFieldName.Length)
                {
                    dt.Rows[dIndex].Delete();
                }
            }

            return dt;
        }
        #endregion

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

        public static string T(string str)
        {
            char[] chSpace = { ' ' };
            if (str == null || str == "")
                return str;
            else
                return str.TrimEnd(chSpace);
        }


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