using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.COMMON.DA
{
    public class ValidationClass
    {
        public ValidationClass() { }

        public ValidationClass(SqlConnection db, SqlTransaction tr)
        {

            m_db = db;
            m_tr = tr;
            m_bInTrans = true;
        }

        SqlConnection m_db;
        SqlTransaction m_tr;
        bool m_bInTrans = false;

        public ArrayList GetItemAccUseCheck(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = @"SELECT";

            if (0 <= strCondition.IndexOf("ItemAccCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemAccCode ";
            }
            if (0 <= strCondition.IndexOf("ItemAccName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemAccName ";
            }

            strQuery += @"
FROM ItemAcc
WHERE ItemAccUse = 'Y'";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY ItemAccCode ";


            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
            finally
            {
                strQuery = null;
            }
        }

        public ArrayList GetAccMasterUseCheck(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = @"SELECT";

            if (0 <= strCondition.IndexOf("AccCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " AccCode ";
            }
            if (0 <= strCondition.IndexOf("AccName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " AccName ";
            }

            strQuery += @"
FROM accMaster
WHERE AccUse = 'Y'";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY AccCode ";


            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
        }

        public ArrayList GetCustomerUseCheck(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"SELECT";

            if (0 <= strCondition.IndexOf("CsCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " CsCode ";
            }
            if (0 <= strCondition.IndexOf("CsName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " CsName ";
            }

            strQuery += @"
FROM CsMaster A ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += @" 
Where " + strWhereClause;
            }

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
        }

        public ArrayList GetPayCheck(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"SELECT";

            if (0 <= strCondition.IndexOf("PayCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " PayCode ";
            }
            if (0 <= strCondition.IndexOf("PayName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " PayName ";
            }

            strQuery += @"
FROM PaymentMaster A ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += @" 
Where " + strWhereClause;
            }

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
        }

        public ArrayList GetUserGoupCheck(object[] GDObj, string strWhereClause)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
SELECT UserGroup, GroupName FROM UserGroup ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += @" 
Where " + strWhereClause;
            }

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
            finally
            {
                strQuery = null;
            }
        }

        public ArrayList GetUserCheck(object[] GDObj, string strWhereClause)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
SELECT UserID, UserName
FROM xErpUser ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += @" 
Where " + strWhereClause;
            }

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
            finally
            {
                strQuery = null;
            }
        }

        public ArrayList GetItemGroupCheck(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"SELECT";

            if (0 <= strCondition.IndexOf("ItemGroup/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemGroup ";
            }
            if (0 <= strCondition.IndexOf("GroupName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " GroupName ";
            }
            if (0 <= strCondition.IndexOf("ItemAccCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemAccCode ";
            }
            if (0 <= strCondition.IndexOf("ItemType/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemType ";
            }

            strQuery += @"
FROM ItemGroup A
WHERE GroupUse = 'Y'";

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += strWhereClause;
            }

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
        }

        public ArrayList GetItemMasterUseCheck(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("ItemCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemCode ";
            }
            if (0 <= strCondition.IndexOf("ItemName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemName ";
            }
            if (0 <= strCondition.IndexOf("ItemSpec/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemSpec ";
            }
            if (0 <= strCondition.IndexOf("UnitSD/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitSD ";
            }
            if (0 <= strCondition.IndexOf("UnitPP/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += "  ";
            }
            if (0 <= strCondition.IndexOf("UnitPP/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitPP ";
            }
            if (0 <= strCondition.IndexOf("UnitPO/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitPO ";
            }
            if (0 <= strCondition.IndexOf("UnitMM/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitMM ";
            }
            if (0 <= strCondition.IndexOf("UnitCO/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitCO ";
            }
            if (0 <= strCondition.IndexOf("UnitQM/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitQM ";
            }
            if (0 <= strCondition.IndexOf("UnitEXIM/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitEXIM ";
            }
            if (0 <= strCondition.IndexOf("ItemAccCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemAccCode ";
            }
            if (0 <= strCondition.IndexOf("ItemType/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemType ";
            }
            if (0 <= strCondition.IndexOf("ItemGroup/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemGroup ";
            }

            strQuery += @"
FROM ItemMaster A
WHERE A.ItemUse = 'Y' ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY A.ItemCode ";


            DataTableReader dr = null;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG01", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
            finally
            {
                strQuery = null;
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        public ArrayList GetItemSiteMasterUseCheck(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("ItemCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemCode ";
            }
            if (0 <= strCondition.IndexOf("ItemName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemName ";
            }
            if (0 <= strCondition.IndexOf("ItemSpec/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemSpec ";
            }
            if (0 <= strCondition.IndexOf("UnitSD/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitSD ";
            }
            if (0 <= strCondition.IndexOf("UnitPP/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += "  ";
            }
            if (0 <= strCondition.IndexOf("UnitPP/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitPP ";
            }
            if (0 <= strCondition.IndexOf("UnitPO/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitPO ";
            }
            if (0 <= strCondition.IndexOf("UnitMM/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitMM ";
            }
            if (0 <= strCondition.IndexOf("UnitCO/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitCO ";
            }
            if (0 <= strCondition.IndexOf("UnitQM/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitQM ";
            }
            if (0 <= strCondition.IndexOf("UnitEXIM/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitEXIM ";
            }
            if (0 <= strCondition.IndexOf("ItemAccCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemAccCode ";
            }
            if (0 <= strCondition.IndexOf("ItemType/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemType ";
            }
            if (0 <= strCondition.IndexOf("ItemGroup/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemGroup ";
            }

            strQuery += @"
FROM ItemSiteMaster A
WHERE A.ItemUse = 'Y' ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY A.ItemCode ";


            DataTableReader dr = null;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG01", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
            finally
            {
                strQuery = null;
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        public ArrayList GetCaseMasterUseCheck(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = @"SELECT";

            if (0 <= strCondition.IndexOf("CaseCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " CaseCode ";
            }
            if (0 <= strCondition.IndexOf("CaseID/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " CaseID ";
            }
            if (0 <= strCondition.IndexOf("SysCase/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " SysCase ";
            }
            if (0 <= strCondition.IndexOf("CaseName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " CaseName ";
            }

            strQuery += @"
FROM CaseAcc
WHERE CaseUse = 'Y'";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY SortKey ";


            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
        }

        public ArrayList GetCCMasterUseCheck(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = @"SELECT";

            if (0 <= strCondition.IndexOf("CcCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " CcCode ";
            }
            if (0 <= strCondition.IndexOf("CcName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " CcName ";
            }

            strQuery += @"
FROM CcMaster
WHERE ComCode = " + CFL.Q(GD.ComCode) + @"
AND SiteCode = " + CFL.Q(GD.SiteCode) + @"
AND CcUse = 'Y'";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY CcCode ";


            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
        }

        public ArrayList GetWhMasterUseCheck(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"SELECT";

            if (0 <= strCondition.IndexOf("WhCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " WhCode ";
            }
            if (0 <= strCondition.IndexOf("WhName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " WhName ";
            }

            strQuery += @"
FROM WhMaster A ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += @" 
Where " + strWhereClause;
            }

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
        }

        public ArrayList GetCodeMasterUseCheck(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"SELECT";

            if (0 <= strCondition.IndexOf("CodeCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " CodeCode ";
            }
            if (0 <= strCondition.IndexOf("CodeName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " CodeName ";
            }

            strQuery += @"
FROM CodeMaster A ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += @" 
Where " + strWhereClause;
            }

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
        }

        public ArrayList GetManageGroup(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
SELECT MngGroup, MngName 
FROM ManageGroup ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += @" 
Where " + strWhereClause;
            }

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
            finally
            {
                strQuery = null;
            }
        }

        public ArrayList GetLedCode(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"SELECT";

            if (0 <= strCondition.IndexOf("DepCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " DepCode ";
            }
            if (0 <= strCondition.IndexOf("LastBalanceAmnt/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " LastBalanceAmnt ";
            }
            if (0 <= strCondition.IndexOf("DepNo/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " DepNo ";
            }
            if (0 <= strCondition.IndexOf("DepName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " DepName ";
            }
            if (0 <= strCondition.IndexOf("DepCode2/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " DepCode2 ";
            }

            strQuery += @"
FROM trDeposit ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += @" 
Where " + strWhereClause;
            }

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
        }

        public ArrayList GetExpense(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"SELECT";

            if (0 <= strCondition.IndexOf("ExpenseCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ExpenseCode ";
            }
            if (0 <= strCondition.IndexOf("ExpenseName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ExpenseName ";
            }

            strQuery += @"
from ExpenseMaster ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += @" 
Where " + strWhereClause;
            }

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
        }

        public ArrayList GetEmpMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"SELECT";

            if (0 <= strCondition.IndexOf("EmpCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " EmpCode ";
            }
            if (0 <= strCondition.IndexOf("EmpName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " EmpName ";
            }

            strQuery += @"
FROM EmpMaster A ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += @" 
Where " + strWhereClause;
            }

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
        }

        public ArrayList GetDeptMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"SELECT";

            if (0 <= strCondition.IndexOf("DeptCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " DeptCode ";
            }
            if (0 <= strCondition.IndexOf("DeptName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " DeptName ";
            }

            strQuery += @"
FROM DeptMaster A ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += @" 
Where " + strWhereClause;
            }

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
        }

        public ArrayList GetItemSegMasterUseCheck(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("ItemSegCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemSegCode ";
            }
            if (0 <= strCondition.IndexOf("ItemSegName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemSegName ";
            }
            strQuery += @"
FROM ItemSegMaster A
WHERE A.ItemSegUse = 'Y' ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY A.ItemSegCode ";


            DataTableReader dr = null;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG01", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
            finally
            {
                strQuery = null;
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        public ArrayList GetPcMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"SELECT";

            if (0 <= strCondition.IndexOf("PcCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " PcCode ";
            }
            if (0 <= strCondition.IndexOf("PcName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " PcName ";
            }

            strQuery += @"
FROM PcMaster A";

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += @" 
Where " + strWhereClause;
            }

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
        }

        public ArrayList GetcoCeGroup(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"SELECT";

            if (0 <= strCondition.IndexOf("CeGroupCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " CeGroupCode ";
            }
            if (0 <= strCondition.IndexOf("CeGroupName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " CeGroupName ";
            }

            strQuery += @"
from coCeGroup ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += @" 
Where " + strWhereClause;
            }

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
        }

        public ArrayList GetcoCeMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"SELECT";

            if (0 <= strCondition.IndexOf("CeCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " CeCode ";
            }
            if (0 <= strCondition.IndexOf("CeName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " CeName ";
            }

            strQuery += @"
from coCeMaster ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += @" 
Where " + strWhereClause;
            }

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
        }

        public ArrayList GetAeMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"SELECT";

            if (0 <= strCondition.IndexOf("AeCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " AeCode ";
            }
            if (0 <= strCondition.IndexOf("AeName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " AeName ";
            }

            strQuery += @"
from coAeMaster ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += @" 
Where " + strWhereClause;
            }

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
        }

        public ArrayList GetCoAeMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"SELECT";

            if (0 <= strCondition.IndexOf("AeCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " AeCode ";
            }
            if (0 <= strCondition.IndexOf("AeName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " AeName ";
            }

            strQuery += @"
from coPcAeMaster ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += @" 
Where " + strWhereClause;
            }

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);

                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG14", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
        }

    }
}
