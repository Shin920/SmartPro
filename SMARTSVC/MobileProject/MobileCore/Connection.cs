using System.Data.SqlClient;

namespace SMART
{
    public class Connection
    {

        public SqlConnection con = null;
        public SqlTransaction tran = null;
        public bool tranFlag { get; set; }

        public void ConnectDB(string strClientID)
        {
            ReadINI INI = new ReadINI();

            string strDBServer = CFL.GetStr(INI.GetEntryValue(strClientID, "DBServer"));
            string strDataBase = CFL.GetStr(INI.GetEntryValue(strClientID, "DataBase"));
            string strUserID = CFL.GetStr(INI.GetEntryValue(strClientID, "UserID"));
            string strPassword = CFL.GetStr(INI.GetEntryValue(strClientID, "Password"));


            //접속 커넥션
            string connectionString = "Data Source=" + strDBServer + ";Initial Catalog=" + strDataBase + ";User ID=" + strUserID + ";Password=" + strPassword;

            con = new SqlConnection(connectionString);
            con.Open();
            tranFlag = false;
        }


        public void BeginTran()
        {
            tran = con.BeginTransaction();
            tranFlag = true;
        }

        public void CommitTran()
        {
            tran.Commit();
            tranFlag = false;
        }

        public void RollbackTran()
        {
            tran.Rollback();
            tranFlag = false;
        }

        public void DisConnectDB()
        {
            if (tranFlag == true && tran != null)
            {
                tran.Rollback();
            }

            if (con != null)
            {
                con.Close();
                con.Dispose();
                con = null;
            }
        }
    }


}
