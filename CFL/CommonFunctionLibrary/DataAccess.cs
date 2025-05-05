using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Collections;

namespace SMART
{
    /// <summary>
    /// DataAccess에 대한 요약 설명입니다.
    /// </summary>
    public class DataAccess
    {
        public DataAccess()
        {
            //
            // TODO: 여기에 생성자 논리를 추가합니다.
            //
        }

        public static void CloseTran(ref SqlTransaction tran, ref SqlConnection con)
        {
            if (con != null)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con.Dispose();
            }

            if (tran != null)
                tran.Dispose();
        }

        public static void BeginTran(string clientID, ref SqlTransaction tran, ref SqlConnection con)
        {
            con = new SqlConnection(ClientCore.GetConnectionString(clientID));
            con.Open();
            // tranaction
            tran = con.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
        }

        public static SqlConnection GetDBConnection(GData GD)
        {
            return new SqlConnection(ClientCore.GetConnectionString(GD));
        }


        public static SqlConnection GetDBConnection(GObj G)
        {
            return new SqlConnection(ClientCore.GetConnectionString(G));
        }

        public static DataSet GetDataSet(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string SQLTraceMode, string SQLTracePath)
        {
            return GetDataSet(GD.ClientID, commandText, sqlParameters, commandType, SQLTraceMode, SQLTracePath);
        }

        public static DataSet GetDataSet(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string SQLTraceMode, string SQLTracePath)
        {
            return GetDataSet(G.ClientID, commandText, sqlParameters, commandType, SQLTraceMode, SQLTracePath);
        }

        public static DataSet GetDataSet(string clientID, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string SQLTraceMode, string SQLTracePath)
        {
            SqlConnection con = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataSet dsReturn = null;

            try
            {
                cmd = new SqlCommand();
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;
                //cmd.CommandTimeout = Constant.CommandTimeout;
                cmd.CommandTimeout = 3600;
                if (sqlParameters != null)
                {
                    foreach (SqlParameter param in sqlParameters)
                    {
                        AddParameter(cmd, param);
                    }
                }

                if (SQLTraceMode.Equals("on")) SQLTrace(SQLTracePath, cmd, true);

                con = new SqlConnection(ClientCore.GetConnectionString(clientID));
                con.Open();

                cmd.Connection = con;

                dsReturn = new DataSet();
                da = new SqlDataAdapter(cmd);
                da.Fill(dsReturn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                    con.Dispose();
                }
                if (cmd != null) cmd.Dispose();
                if (da != null) da.Dispose();
            }

            return dsReturn;
        }

        public static DataSet GetDataSet(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string strTableName, int pageIndex, int pageSize, string SQLTraceMode, string SQLTracePath)
        {
            return GetDataSet(GD.ClientID, commandText, sqlParameters, commandType, strTableName, pageIndex, pageSize, SQLTraceMode, SQLTracePath);
        }
        public static DataSet GetDataSet(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string strTableName, int pageIndex, int pageSize, string SQLTraceMode, string SQLTracePath)
        {
            return GetDataSet(G.ClientID, commandText, sqlParameters, commandType, strTableName, pageIndex, pageSize, SQLTraceMode, SQLTracePath);
        }
        public static DataSet GetDataSet(string clientID, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string strTableName, int pageIndex, int pageSize, string SQLTraceMode, string SQLTracePath)
        {
            SqlConnection con = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataSet dsReturn = null;

            try
            {
                cmd = new SqlCommand();
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;
                //  cmd.CommandTimeout = Constant.CommandTimeout;
                cmd.CommandTimeout = 3600;
                if (sqlParameters != null)
                {
                    foreach (SqlParameter param in sqlParameters)
                    {
                        AddParameter(cmd, param);
                    }
                }

                if (SQLTraceMode.Equals("on")) SQLTrace(SQLTracePath, cmd, true);

                con = new SqlConnection(ClientCore.GetConnectionString(clientID));
                con.Open();

                cmd.Connection = con;

                dsReturn = new DataSet();
                da = new SqlDataAdapter(cmd);
                //da.Fill(dsReturn);
                int startRecord = pageIndex * pageSize;
                int maxRecords = pageSize;
                da.Fill(dsReturn, startRecord, maxRecords, strTableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                    con.Dispose();
                }
                if (cmd != null) cmd.Dispose();
                if (da != null) da.Dispose();
            }

            return dsReturn;
        }

        public static DataSet GetDataSetTran(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start, string SQLTraceMode, string SQLTracePath)
        {
            return GetDataSetTran(GD.ClientID, commandText, sqlParameters, commandType, ref tran, ref con, start, SQLTraceMode, SQLTracePath);
        }

        public static DataSet GetDataSetTran(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start, string SQLTraceMode, string SQLTracePath)
        {
            return GetDataSetTran(G.ClientID, commandText, sqlParameters, commandType, ref tran, ref con, start, SQLTraceMode, SQLTracePath);
        }

        public static DataSet GetDataSetTran(string clientID, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start, string SQLTraceMode, string SQLTracePath)
        {
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataSet dsReturn = null;

            try
            {
                cmd = new SqlCommand();
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;
                //cmd.CommandTimeout = Constant.CommandTimeout;
                cmd.CommandTimeout = 3600;
                if (sqlParameters != null)
                {
                    foreach (SqlParameter param in sqlParameters)
                    {
                        AddParameter(cmd, param);
                    }
                }

                if (SQLTraceMode.Equals("on")) SQLTrace(SQLTracePath, cmd, true);

                if (start == true)
                {
                    con = new SqlConnection(ClientCore.GetConnectionString(clientID));
                    con.Open();
                    // tranaction
                    tran = con.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                }

                cmd.Connection = con;
                cmd.Transaction = tran;
                cmd.Prepare();

                dsReturn = new DataSet();
                da = new SqlDataAdapter(cmd);
                da.Fill(dsReturn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (da != null) da.Dispose();
            }

            return dsReturn;
        }

        public static DataSet GetDataSetTran(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string strTableName, int pageIndex, int pageSize, ref SqlTransaction tran, ref SqlConnection con, bool start, string SQLTraceMode, string SQLTracePath)
        {
            return GetDataSetTran(GD.ClientID, commandText, sqlParameters, commandType, strTableName, pageIndex, pageSize, ref tran, ref con, start, SQLTraceMode, SQLTracePath);
        }
        public static DataSet GetDataSetTran(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string strTableName, int pageIndex, int pageSize, ref SqlTransaction tran, ref SqlConnection con, bool start, string SQLTraceMode, string SQLTracePath)
        {
            return GetDataSetTran(G.ClientID, commandText, sqlParameters, commandType, strTableName, pageIndex, pageSize, ref tran, ref con, start, SQLTraceMode, SQLTracePath);
        }
        public static DataSet GetDataSetTran(string clientID, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string strTableName, int pageIndex, int pageSize, ref SqlTransaction tran, ref SqlConnection con, bool start, string SQLTraceMode, string SQLTracePath)
        {
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataSet dsReturn = null;

            try
            {
                cmd = new SqlCommand();
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;
                //  cmd.CommandTimeout = Constant.CommandTimeout;
                cmd.CommandTimeout = 3600;
                if (sqlParameters != null)
                {
                    foreach (SqlParameter param in sqlParameters)
                    {
                        AddParameter(cmd, param);
                    }
                }

                if (SQLTraceMode.Equals("on")) SQLTrace(SQLTracePath, cmd, true);

                if (start == true)
                {
                    con = new SqlConnection(ClientCore.GetConnectionString(clientID));
                    con.Open();
                    // tranaction
                    tran = con.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                }

                cmd.Connection = con;
                cmd.Transaction = tran;
                cmd.Prepare();

                dsReturn = new DataSet();
                da = new SqlDataAdapter(cmd);
                //da.Fill(dsReturn);
                int startRecord = pageIndex * pageSize;
                int maxRecords = pageSize;
                da.Fill(dsReturn, startRecord, maxRecords, strTableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (da != null) da.Dispose();
            }

            return dsReturn;
        }

        public static int ExecuteNonQuery(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string SQLTraceMode, string SQLTracePath)
        {
            return ExecuteNonQuery(GD.ClientID, commandText, sqlParameters, commandType, SQLTraceMode, SQLTracePath);
        }
        public static int ExecuteNonQuery(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string SQLTraceMode, string SQLTracePath)
        {
            return ExecuteNonQuery(G.ClientID, commandText, sqlParameters, commandType, SQLTraceMode, SQLTracePath);
        }
        public static int ExecuteNonQuery(string clientID, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string SQLTraceMode, string SQLTracePath)
        {
            int iReturn = 0;

            SqlConnection con = null;
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand();
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;
                //cmd.CommandTimeout = Constant.CommandTimeout;
                cmd.CommandTimeout = 3600;

                if (sqlParameters != null)
                {
                    foreach (SqlParameter param in sqlParameters)
                    {
                        AddParameter(cmd, param);
                    }
                }

                if (SQLTraceMode.Equals("on")) SQLTrace(SQLTracePath, cmd, true);

                con = new SqlConnection(ClientCore.GetConnectionString(clientID));
                con.Open();

                cmd.Connection = con;
                iReturn = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                    con.Dispose();
                }
                if (cmd != null) cmd.Dispose();
            }

            return iReturn;
        }


        public static SqlDataReader ExecuteReader(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string SQLTraceMode, string SQLTracePath)
        {
            return ExecuteReader(GD.ClientID, commandText, sqlParameters, commandType, SQLTraceMode, SQLTracePath);
        }

        public static SqlDataReader ExecuteReader(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string SQLTraceMode, string SQLTracePath)
        {
            return ExecuteReader(G.ClientID, commandText, sqlParameters, commandType, SQLTraceMode, SQLTracePath);
        }

        public static SqlDataReader ExecuteReader(string clientID, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string SQLTraceMode, string SQLTracePath)
        {
            SqlConnection con = null;
            SqlCommand cmd = null;
            SqlDataReader dataReader = null;
            try
            {
                cmd = new SqlCommand();
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;

                cmd.CommandTimeout = 3600;
                if (sqlParameters != null)
                {
                    foreach (SqlParameter param in sqlParameters)
                    {
                        AddParameter(cmd, param);
                    }
                }

                if (SQLTraceMode.Equals("on")) SQLTrace(SQLTracePath, cmd, true);

                con = new SqlConnection(ClientCore.GetConnectionString(clientID));
                con.Open();

                cmd.Connection = con;
                dataReader = cmd.ExecuteReader();

            }
            catch (Exception ex)
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                    con.Dispose();
                }
                if (cmd != null)
                    cmd.Dispose();
                throw ex;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
            }

            return dataReader;
        }

        public static SqlDataReader ExecuteReaderTran(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start, string SQLTraceMode, string SQLTracePath)
        {
            return ExecuteReaderTran(GD.ClientID, commandText, sqlParameters, commandType, ref tran, ref con, start, SQLTraceMode, SQLTracePath);
        }

        public static SqlDataReader ExecuteReaderTran(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start, string SQLTraceMode, string SQLTracePath)
        {
            return ExecuteReaderTran(G.ClientID, commandText, sqlParameters, commandType, ref tran, ref con, start, SQLTraceMode, SQLTracePath);
        }

        public static SqlDataReader ExecuteReaderTran(string clientID, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start, string SQLTraceMode, string SQLTracePath)
        {
            SqlDataReader dataReader = null;

            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand();
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;

                cmd.CommandTimeout = 3600;
                if (sqlParameters != null)
                {
                    foreach (SqlParameter param in sqlParameters)
                    {
                        AddParameter(cmd, param);
                    }
                }

                if (SQLTraceMode.Equals("on")) SQLTrace(SQLTracePath, cmd, true);

                if (start == true)
                {
                    con = new SqlConnection(ClientCore.GetConnectionString(clientID));
                    con.Open();
                    // tranaction
                    tran = con.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                }

                cmd.Connection = con;
                cmd.Transaction = tran;
                cmd.Prepare();

                dataReader = cmd.ExecuteReader();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();

            }

            return dataReader;
        }

        public static DataTableReader ExecuteDataTableReader(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string SQLTraceMode, string SQLTracePath)
        {
            return ExecuteDataTableReader(GD.ClientID, commandText, sqlParameters, commandType, SQLTraceMode, SQLTracePath);
        }

        public static DataTableReader ExecuteDataTableReader(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string SQLTraceMode, string SQLTracePath)
        {
            return ExecuteDataTableReader(G.ClientID, commandText, sqlParameters, commandType, SQLTraceMode, SQLTracePath);
        }

        public static DataTableReader ExecuteDataTableReader(string clientID, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string SQLTraceMode, string SQLTracePath)
        {
            return ExecuteDataTableReader(GetDataSet(clientID, commandText, sqlParameters, commandType, SQLTraceMode, SQLTracePath));
        }

        public static DataTableReader ExecuteDataTableReaderTran(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start, string SQLTraceMode, string SQLTracePath)
        {
            return ExecuteDataTableReaderTran(GD.ClientID, commandText, sqlParameters, commandType, ref tran, ref con, start, SQLTraceMode, SQLTracePath);
        }

        public static DataTableReader ExecuteDataTableReaderTran(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start, string SQLTraceMode, string SQLTracePath)
        {
            return ExecuteDataTableReaderTran(G.ClientID, commandText, sqlParameters, commandType, ref tran, ref con, start, SQLTraceMode, SQLTracePath);
        }

        public static DataTableReader ExecuteDataTableReaderTran(string clientID, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start, string SQLTraceMode, string SQLTracePath)
        {
            return ExecuteDataTableReader(GetDataSetTran(clientID, commandText, sqlParameters, commandType, ref tran, ref con, start, SQLTraceMode, SQLTracePath));
        }

        public static int ExecuteTran(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start, string SQLTraceMode, string SQLTracePath)
        {
            return ExecuteTran(GD.ClientID, commandText, sqlParameters, commandType, ref tran, ref con, start, SQLTraceMode, SQLTracePath);
        }

        public static int ExecuteTran(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start, string SQLTraceMode, string SQLTracePath)
        {
            return ExecuteTran(G.ClientID, commandText, sqlParameters, commandType, ref tran, ref con, start, SQLTraceMode, SQLTracePath);
        }

        private static DataTableReader ExecuteDataTableReader(DataSet dataSet)
        {
            if (dataSet.Tables.Count == 0)
                return null;
            return dataSet.CreateDataReader();
        }

        public static int ExecuteTran(string clientID, string commandText, SqlParameter[] sqlParameters, CommandType commandType, ref SqlTransaction tran, ref SqlConnection con, bool start, string SQLTraceMode, string SQLTracePath)
        {
            int iReturn = 0;

            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand();
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;
                cmd.CommandTimeout = 3600;
                if (sqlParameters != null)
                {
                    foreach (SqlParameter param in sqlParameters)
                    {
                        AddParameter(cmd, param);
                    }
                }

                if (SQLTraceMode.Equals("on")) SQLTrace(SQLTracePath, cmd, true);

                if (start == true)
                {
                    con = new SqlConnection(ClientCore.GetConnectionString(clientID));
                    con.Open();
                    // tranaction
                    tran = con.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                }

                cmd.Connection = con;
                cmd.Transaction = tran;
                cmd.Prepare();

                iReturn = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
            }

            return iReturn;
        }

        public static int ExecuteMultiple(GData GD, string commandText, SqlParameter[] sqlParameters, string[,] paramValues, CommandType commandType, string SQLTraceMode, string SQLTracePath)
        {
            return ExecuteMultiple(GD.ClientID, commandText, sqlParameters, paramValues, commandType, SQLTraceMode, SQLTracePath);
        }
        public static int ExecuteMultiple(GObj G, string commandText, SqlParameter[] sqlParameters, string[,] paramValues, CommandType commandType, string SQLTraceMode, string SQLTracePath)
        {
            return ExecuteMultiple(G.ClientID, commandText, sqlParameters, paramValues, commandType, SQLTraceMode, SQLTracePath);
        }
        public static int ExecuteMultiple(string clientID, string commandText, SqlParameter[] sqlParameters, string[,] paramValues, CommandType commandType, string SQLTraceMode, string SQLTracePath)
        {
            if (sqlParameters == null) throw new Exception("ExecuteMultiple() 서비스의 sqlParameters 파라미터는 null 일 수 없습니다.");

            int iReturn = 0;

            SqlConnection con = null;
            SqlTransaction ts = null;
            SqlCommand cmd = null;

            int iColumnCount = paramValues.GetUpperBound(1) + 1;
            int iRowCount = paramValues.GetUpperBound(0) + 1;
            int iCol = 0;
            int iRow = 0;

            string[] paramVals = new string[iColumnCount];
            for (iCol = 0; iCol < iColumnCount; iCol++)
                paramVals[iCol] = paramValues[iRow, iCol];

            try
            {
                cmd = new SqlCommand();
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;
                //cmd.CommandTimeout = Constant.CommandTimeout;
                cmd.CommandTimeout = 3600;
                foreach (SqlParameter param in sqlParameters)
                {
                    AddParameter(cmd, param);
                }

                con = new SqlConnection(ClientCore.GetConnectionString(clientID));
                con.Open();
                ts = con.BeginTransaction();

                cmd.Connection = con;
                cmd.Transaction = ts;
                cmd.Prepare();

                for (iRow = 0; iRow < iRowCount; iRow++)
                {
                    for (iCol = 0; iCol < iColumnCount; iCol++)
                    {
                        string strValue = paramValues[iRow, iCol];
                        if ((strValue == null) || (strValue.Length == 0)) cmd.Parameters[iCol].Value = System.DBNull.Value;
                        else cmd.Parameters[iCol].Value = strValue;
                    }
                    if (SQLTraceMode.Equals("on"))
                    {
                        if (iRow == (iRowCount - 1)) SQLTrace(SQLTracePath, cmd, true);
                        else if (SQLTraceMode.Equals("on")) SQLTrace(SQLTracePath, cmd, false);
                    }
                    iReturn += cmd.ExecuteNonQuery();
                }

                ts.Commit();
            }
            catch (Exception ex)
            {
                if (ts != null) ts.Rollback();
                throw ex;
            }
            finally
            {
                if (ts != null) ts.Dispose();
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                    con.Dispose();
                }
                if (cmd != null) cmd.Dispose();
            }

            return iReturn;
        }


        public static object ExecuteScalar(GData GD, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string SQLTraceMode, string SQLTracePath)
        {
            return ExecuteScalar(GD.ClientID, commandText, sqlParameters, commandType, SQLTraceMode, SQLTracePath);
        }

        public static object ExecuteScalar(GObj G, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string SQLTraceMode, string SQLTracePath)
        {
            return ExecuteScalar(G.ClientID, commandText, sqlParameters, commandType, SQLTraceMode, SQLTracePath);
        }
        public static object ExecuteScalar(string clientID, string commandText, SqlParameter[] sqlParameters, CommandType commandType, string SQLTraceMode, string SQLTracePath)
        {
            SqlConnection con = null;
            SqlCommand cmd = null;
            object oReturn = null;

            try
            {
                cmd = new SqlCommand();
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;
                //cmd.CommandTimeout = Constant.CommandTimeout;
                cmd.CommandTimeout = 3600;
                if (sqlParameters != null)
                {
                    foreach (SqlParameter param in sqlParameters)
                    {
                        AddParameter(cmd, param);
                    }
                }

                if (SQLTraceMode.Equals("on")) SQLTrace(SQLTracePath, cmd, true);

                con = new SqlConnection(ClientCore.GetConnectionString(clientID));
                con.Open();

                cmd.Connection = con;
                oReturn = cmd.ExecuteScalar();
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                    con.Dispose();
                }
                if (cmd != null) cmd.Dispose();
            }
            return oReturn;
        }

        private static void AddParameter(SqlCommand cmd, SqlParameter param)
        {
            if ((param.Value == null) || ((param.Value.GetType().ToString().Equals("System.String")) && ((string)param.Value).Length == 0))
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add(param);
        }

        private static void SQLTrace(string SQLTracePath, System.Data.SqlClient.SqlCommand cmd, bool markEndLine)
        {
            System.Text.StringBuilder oBuilder = new System.Text.StringBuilder();

            oBuilder.Append(System.DateTime.Now.ToString());
            oBuilder.Append('\t');

            if (cmd.Parameters.Count == 0)
            {
                oBuilder.Append(cmd.CommandText);
            }
            else
            {
                oBuilder.Append(cmd.CommandText);
                for (int iElemCnt = 0; iElemCnt < cmd.Parameters.Count; iElemCnt++)
                {
                    object oValue = cmd.Parameters[iElemCnt].Value;
                    if (oValue != null)
                    {
                        string strValue = null;
                        if (oValue == System.DBNull.Value)
                            strValue = "NULL";
                        else
                        {
                            switch (cmd.Parameters[iElemCnt].SqlDbType)
                            {
                                case SqlDbType.Char:
                                case SqlDbType.VarChar:
                                case SqlDbType.NChar:
                                case SqlDbType.NVarChar:
                                    strValue = string.Concat("'", (string)oValue, "'");
                                    break;
                                case SqlDbType.Image:
                                    oBuilder.Append("<Blob>");
                                    break;
                                case SqlDbType.Binary:
                                    oBuilder.Append("<Binary>");
                                    break;
                                case SqlDbType.VarBinary:
                                    oBuilder.Append("<VarBinary>");
                                    break;
                                default:
                                    strValue = oValue.ToString();
                                    break;
                            }
                        }
                        oBuilder.Replace(cmd.Parameters[iElemCnt].ParameterName, strValue);
                    }
                }
            }
            if (markEndLine) oBuilder.Append("\r\n-------------------------------------------------------------");
            string sContents = oBuilder.ToString();

            string sFileName = string.Format("{0}_SQLQueryTrc_{1}.log", System.Net.Dns.GetHostName().ToLower(),
                System.DateTime.Now.ToShortDateString());

            CFL.LogToFile(SQLTracePath, sFileName, sContents);
        }
    }
}
