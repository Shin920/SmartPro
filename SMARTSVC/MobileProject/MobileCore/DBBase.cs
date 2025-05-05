using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART
{

    public class DBBase
    {
        public Connection connection = new Connection();


        //SQL 조회.
        public DataSet SQLDataSet(string sql)
        {
            DataSet ds = new DataSet();

            SqlDataAdapter sda = new SqlDataAdapter("", connection.con);
            sda.SelectCommand.CommandText = sql;
            sda.SelectCommand.CommandType = CommandType.Text;

            if (connection.tranFlag == true && connection.tran != null)
            {
                //Transaction
                sda.SelectCommand.Transaction = connection.tran;
            }

            sda.Fill(ds, "table1");

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                foreach (DataColumn dc in ds.Tables[i].Columns) // trim column names
                {
                    dc.ColumnName = dc.ColumnName.Trim();
                }

                foreach (DataRow dr in ds.Tables[i].Rows) // trim string data
                {
                    foreach (DataColumn dc in ds.Tables[i].Columns)
                    {
                        if (dc.DataType == typeof(string))
                        {
                            object o = dr[dc];
                            if (!Convert.IsDBNull(o) && o != null)
                            {
                                dr[dc] = o.ToString().Trim();
                            }
                        }
                    }
                }
            }

            return ds;
        }

        //SQL INSERT UPDATE
        public int SQLInsertUpdate(string sql)
        {
            int ret = 0;
            SqlCommand cmd = new SqlCommand("", connection.con);
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            if (connection.tranFlag == true && connection.tran != null)
            {
                //Transaction
                cmd.Transaction = connection.tran;
            }
            ret = cmd.ExecuteNonQuery();

            return ret;
        }

        //쿼리 후 데이터 가져오는 형식 만들어 주는 함수
        public CFL EncodeData(DataSet ds, int errCode, string msg)
        {
            CFL ret = new CFL();
            ret.SetData(setDataSetNullToBlank(ds), errCode, msg);
            return ret;
        }

        //쿼리 후 데이터 가져오는 형식 만들어 주는 함수
        public CFL EncodeData(DataSet ds, int errCode, string msg, string retData)
        {
            CFL ret = new CFL();
            ret.SetData(setDataSetNullToBlank(ds), errCode, msg, retData);
            return ret;
        }

        /*
         * Dataset 컬럼값이 null 이면 공백(Blank) 변환함
         * */
        public DataSet setDataSetNullToBlank(DataSet ds)
        {
            //
            if (ds == null)
            {
                return ds;
            }

            //
            if (ds.Tables.Count <= 0)
            {
                return ds;
            }

            for (int j = 0; j < ds.Tables[0].Columns.Count; j++)        // 열
            {
                if (ds.Tables[0].Columns[j].DataType.ToString() == "System.String")
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)       // 행
                    {
                        if (DBNull.Value.Equals(ds.Tables[0].Rows[i][j]))
                        {
                            ds.Tables[0].Rows[i][j] = "";
                        }
                    }
                }
            }

            return ds;
        }


    }
}
