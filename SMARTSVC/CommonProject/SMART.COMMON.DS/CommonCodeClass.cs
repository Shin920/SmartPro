using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Web;

namespace SMART.COMMON.DS
{
    public class CommonCodeClass
    {
        COMMON.DS.MessageMaster Message = new COMMON.DS.MessageMaster();
        //
        public DataSet GetCodeMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("CodeCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.CodeCode) AS CodeCode ";
            }
            if (0 <= strCondition.IndexOf("CodeName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.CodeName) AS CodeName ";
            }
            if (0 <= strCondition.IndexOf("TypeCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.TypeCode) AS TypeCode ";
            }
            if (0 <= strCondition.IndexOf("Factor1/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.Factor1) AS Factor1 ";
            }

            strQuery += @"
FROM CodeMaster A
WHERE A.ComCode=" + CFL.Q(GD.ComCode) + " AND A.CodeUse = 'Y' ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY A.SortKey ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }


        public DataSet GetCodeIDMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("CodeID/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.CodeID) AS CodeID ";
            }

            if (0 <= strCondition.IndexOf("CodeIDName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.CodeIDName) AS CodeIDName ";
            }

            strQuery += @"
FROM CodeIDMaster A
JOIN CodeIDGubun B ON A.CodeID = B.CodeID 
WHERE A.CodeIDUse = 'Y'
  And A.LangID = " + CFL.Q(GD.LangID);

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY A.CodeID DESC ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }

        public DataSet GetCoaMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("CoaCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.CoaCode) AS CoaCode ";
            }

            if (0 <= strCondition.IndexOf("CoaName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.CoaName) AS CoaName ";
            }

            strQuery += @"
FROM CoaMaster A
WHERE A.CoaUse = 'Y'";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY A.CoaName ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }

        //
        public DataSet GetAccMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("AccCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(AccCode) AS AccCode ";
            }
            if (0 <= strCondition.IndexOf("AccName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(AccName) AS AccName ";
            }

            strQuery += @"
FROM accMaster
WHERE AccUse = 'Y'";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY AccCode ";


            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }


        //부서조회
        public DataSet GetDeptMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("DeptCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.DeptCode) AS DeptCode ";
            }
            if (0 <= strCondition.IndexOf("DeptName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.DeptName) AS DeptName ";
            }

            strQuery += @"
FROM DeptMaster A
WHERE ComCode = " + CFL.Q(GD.ComCode);

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY DeptCode ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }


        public DataSet GetCsMaster(object[] GDObj, string strWhereClause, string strCondition, ASPxGridView Grid = null)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strMsg = "L01243;L01452;L01442;L00399;L00400;L01478;L02396;L01479;L03099;L03100;L03196";

            DataSet mds = Message.MsgSet(GDObj, GD.LangID, strMsg);

            if (Grid != null)
            {
                Grid.Columns.Clear();
            }

            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("CsCode/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L01243");
                    col.FieldName = "CsCode";
                    col.Width = new System.Web.UI.WebControls.Unit(100, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " rtrim(isnull(a.CsCode,'')) as CsCode ";
            }

            if (0 <= strCondition.IndexOf("CardCode/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L03099"); //신용카드코드
                    col.FieldName = "CardCode";
                    col.Width = new System.Web.UI.WebControls.Unit(100, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " rtrim(isnull(a.CsCode,'')) as CardCode ";
            }

            if (0 <= strCondition.IndexOf("CardName/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L03100"); //신용카드명
                    col.FieldName = "CardName";
                    col.Width = new System.Web.UI.WebControls.Unit(100, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " rtrim(isnull(a.CsName,'')) as CardName ";
            }

            if (0 <= strCondition.IndexOf("CardRegNo/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L03196"); //신용카드정보
                    col.FieldName = "CardRegNo";
                    col.Width = new System.Web.UI.WebControls.Unit(100, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " RTRIM(isnull(a.RegNo, '')) AS CardRegNo ";
            }

            if (0 <= strCondition.IndexOf("CsNameFull/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L01452");
                    col.FieldName = "CsNameFull";
                    col.Width = new System.Web.UI.WebControls.Unit(100, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " rtrim(isnull(a.CsNameFull,'')) as CsNameFull ";
            }

            if (0 <= strCondition.IndexOf("CsName/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L01442");
                    col.FieldName = "CsName";
                    col.Width = new System.Web.UI.WebControls.Unit(100, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " rtrim(isnull(a.CsName,'')) as CsName ";
            }

            if (0 <= strCondition.IndexOf("CurrCode/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L00399");
                    col.FieldName = "CurrCode";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " rtrim(isnull(a.CurrCode,'')) as CurrCode ";
            }

            if (0 <= strCondition.IndexOf("BaseExchRatio/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L00400");
                    col.FieldName = "BaseExchRatio";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " RTRIM(B.BaseExchRatio) AS BaseExchRatio";
            }

            if (0 <= strCondition.IndexOf("TaxCodeAr/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L01478");
                    col.FieldName = "TaxCodeAr";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " RTRIM(A.TaxCodeAr) AS TaxCodeAr ";
            }

            if (0 <= strCondition.IndexOf("CsItems/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L02396");
                    col.FieldName = "CsItems";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " IsNull(A.CsItems, '') AS CsItems ";
            }

            if (0 <= strCondition.IndexOf("TaxCodeAp/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L01479");
                    col.FieldName = "TaxCodeAp";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " RTRIM(A.TaxCodeAp) AS TaxCodeAp ";
            }

            if (0 <= strCondition.IndexOf("CsInitial/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = "거래처이니셜";
                    col.FieldName = "CsInitial";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " RTRIM(A.CsInitial) AS CsInitial ";
            }

            strQuery += @"
FROM CsMaster A
    LEFT JOIN CurrencyMaster B on b.ComCode =" + CFL.Q(GD.ComCode) + @" and B.CurrCode = A.CurrCode ";

            strQuery += @"
WHERE A.ComCode = " + CFL.Q(GD.ComCode) + @"
  AND A.CsUse = 'Y' ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY A.CsCode ";


            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }


        // 추가조건 1개
        public DataSet GetCsMaster(object[] GDObj, string strWhereClause, string strCondition, string strCond1, ASPxGridView Grid = null)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strMsg = "L01243;L01452;L01442;L00399;L00400;L01478;L02396;L01479;L03099;L03100;L03196;L04263";

            DataSet mds = Message.MsgSet(GDObj, GD.LangID, strMsg);

            if (Grid != null)
            {
                Grid.Columns.Clear();
            }

            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("CsCode/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L01243");
                    col.FieldName = "CsCode";
                    col.Width = new System.Web.UI.WebControls.Unit(100, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " rtrim(isnull(a.CsCode,'')) as CsCode ";
            }

            if (0 <= strCondition.IndexOf("CardCode/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L03099"); //신용카드코드
                    col.FieldName = "CardCode";
                    col.Width = new System.Web.UI.WebControls.Unit(100, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " rtrim(isnull(a.CsCode,'')) as CardCode ";
            }

            if (0 <= strCondition.IndexOf("CardName/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L03100"); //신용카드명
                    col.FieldName = "CardName";
                    col.Width = new System.Web.UI.WebControls.Unit(100, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " rtrim(isnull(a.CsName,'')) as CardName ";
            }

            if (0 <= strCondition.IndexOf("CardRegNo/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L03196"); //신용카드정보
                    col.FieldName = "CardRegNo";
                    col.Width = new System.Web.UI.WebControls.Unit(100, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " RTRIM(isnull(a.RegNo, '')) AS CardRegNo ";
            }

            if (0 <= strCondition.IndexOf("CsNameFull/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L01452");
                    col.FieldName = "CsNameFull";
                    col.Width = new System.Web.UI.WebControls.Unit(100, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " rtrim(isnull(a.CsNameFull,'')) as CsNameFull ";
            }

            if (0 <= strCondition.IndexOf("CsName/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L01442");
                    col.FieldName = "CsName";
                    col.Width = new System.Web.UI.WebControls.Unit(100, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " rtrim(isnull(a.CsName,'')) as CsName ";
            }

            if (0 <= strCondition.IndexOf("CurrCode/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L00399");
                    col.FieldName = "CurrCode";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " rtrim(isnull(a.CurrCode,'')) as CurrCode ";
            }

            if (0 <= strCondition.IndexOf("BaseExchRatio/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L00400");
                    col.FieldName = "BaseExchRatio";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " RTRIM(B.BaseExchRatio) AS BaseExchRatio";
            }

            if (0 <= strCondition.IndexOf("TaxCodeAr/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L01478");
                    col.FieldName = "TaxCodeAr";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " RTRIM(A.TaxCodeAr) AS TaxCodeAr ";
            }

            if (0 <= strCondition.IndexOf("CsItems/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L02396");
                    col.FieldName = "CsItems";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " IsNull(A.CsItems, '') AS CsItems ";
            }

            if (0 <= strCondition.IndexOf("TaxCodeAp/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L01479");
                    col.FieldName = "TaxCodeAp";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";
                strQuery += " RTRIM(A.TaxCodeAp) AS TaxCodeAp ";
            }

            if (0 <= strCondition.IndexOf("ItemVndPrice/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L04263");  // 구매단가
                    col.FieldName = "ItemVndPrice";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " RTRIM( isnull( isnull(FORMAT(K.ItemPrice, '0.00'), FORMAT(C.StdPurPrice, '0.00')), 0) ) AS ItemVndPrice ";
            }
            if (0 <= strCondition.IndexOf("CsAddress/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = "주소";
                    col.FieldName = "CsAddress";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " IsNull(A.CsAddress, '') AS CsAddress ";
            }
            if (0 <= strCondition.IndexOf("CsChief/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = "대표";
                    col.FieldName = "CsChief";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " IsNull(A.CsChief, '') AS CsChief ";
            }

            if (0 <= strCondition.IndexOf("RegNo/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = "사업자번호";
                    col.FieldName = "RegNo";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " IsNull(A.RegNo, '') AS RegNo ";
            }

            if (0 <= strCondition.IndexOf("CsInitial/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = "거래처이니셜";
                    col.FieldName = "CsInitial";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += @", ";

                strQuery += " IsNull(A.CsInitial, '') AS CsInitial ";
            }
            strQuery += @"
FROM CsMaster A
    LEFT JOIN CurrencyMaster B on b.ComCode =" + CFL.Q(GD.ComCode) + @" and B.CurrCode = A.CurrCode ";


            if (0 <= strCondition.IndexOf("ItemVndPrice/"))
            {
                strQuery += @"    
	Left Outer Join ItemVndMaster K on A.CsCode = K.CsCode and K.SiteCode = " + CFL.Q(GD.SiteCode) + @" 
                                   and K.ItemCode = " + CFL.Q(strCond1) + @" 
                                   and K.CurrCode = " + CFL.Q(GD.CurrCode) + @"  
                                   and convert(char(8), getdate(), 112) between K.BDate and K.EDate 
	Left Outer Join ItemSiteMaster C on C.SiteCode = " + CFL.Q(GD.SiteCode) + " and C.ItemCode  =  " + CFL.Q(strCond1);
            }


            strQuery += @"
WHERE A.ComCode = " + CFL.Q(GD.ComCode) + @"
  AND A.CsUse = 'Y' ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY A.CsCode ";


            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }


        public DataSet GetParentCsMaster(object[] GDObj, string strWhereClause, string strCondition, ASPxGridView Grid = null)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strMsg = "L03198;L03197";
            DataSet mds = Message.MsgSet(GDObj, GD.LangID, strMsg);
            if (Grid != null)
            {
                Grid.Columns.Clear();
            }
            string strQuery = "SELECT distinct";

            if (0 <= strCondition.IndexOf("ParentCs/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L03197"); //카드사코드
                    col.FieldName = "ParentCs";
                    col.Width = new System.Web.UI.WebControls.Unit(100, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (16 < strQuery.Length)
                    strQuery += @",";
                strQuery += " rtrim(isnull(a.ParentCs,'')) as ParentCs ";
            }
            if (0 <= strCondition.IndexOf("ParentCsName/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L03198"); //신용카드명
                    col.FieldName = "ParentCsName";
                    col.Width = new System.Web.UI.WebControls.Unit(100, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += @",";
                strQuery += " rtrim(isnull(b.CsNameFull,'')) as ParentCsName ";
            }

            strQuery += @"
From CsMaster A
Left join CsMaster B on  B.ComCode = N'COM1' and B.CsCode = A.ParentCs ";

            strQuery += @"
WHERE A.ComCode = " + CFL.Q(GD.ComCode) + @"
  AND A.CsUse = 'Y' ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY ParentCs ";


            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }

        public DataSet GetNationMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("NationCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.NationCode) AS NationCode ";
            }
            if (0 <= strCondition.IndexOf("NationName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.NationName) AS NationName ";
            }
            if (0 <= strCondition.IndexOf("CurrCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.CurrCode) AS CurrCode ";
            }
            if (0 <= strCondition.IndexOf("RegionCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.RegionCode) AS RegionCode ";
            }

            strQuery += @"
FROM NationMaster A
WHERE A.NationUse = 'Y' AND A.ComCode = " + CFL.Q(GD.ComCode);

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY CAST(A.SortKey as int) ASC ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }

        public DataSet GetRegionMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("RegionCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " rtrim(RegionCode) as RegionCode  ";
            }
            if (0 <= strCondition.IndexOf("RegionName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " rtrim(RegionName) as RegionName ";
            }

            strQuery += @"
FROM RegionMaster
WHERE RegionUse = 'Y' ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY SortKey ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }

        public DataSet GetTransRegionMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("RegionCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " rtrim(RegionCode) as RegionCode  ";
            }
            if (0 <= strCondition.IndexOf("RegionName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " rtrim(RegionName) as RegionName ";
            }

            strQuery += @"
FROM sdTransRegionMaster";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += @"
WHERE " + strWhereClause;

            strQuery += @"
ORDER BY RegionCode ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }

        public DataSet GetHsMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("HSCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " HSCode ";
            }
            if (0 <= strCondition.IndexOf("HSName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " HSName ";
            }

            strQuery += @"
FROM HsMaster A
WHERE A.HsUse = 'Y'
  AND A.ComCode = " + CFL.Q(GD.ComCode) + @"";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY A.HSCode ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }

        public DataSet GetUserMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("UserID/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ", ";
                strQuery += " RTRIM(UserID) AS UserID";
            }

            if (0 <= strCondition.IndexOf("UserName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ", ";
                strQuery += " RTRIM(UserName) AS UserName";
            }

            strQuery += @"
FROM xErpUser
WHERE ComCode = " + CFL.Q(GD.ComCode) + @"
 ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += strWhereClause;
            }

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }


        public DataSet GetItemSiteMasterLoad(object[] GDObj, string strWhereClause, string strCondition, string strCond1, string strCond2, string strCond3)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("ItemCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " a.ItemCode ";
            }
            if (0 <= strCondition.IndexOf("ItemName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " a.ItemName ";
            }
            if (0 <= strCondition.IndexOf("ItemSpec/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " a.ItemSpec ";
            }
            if (0 <= strCondition.IndexOf("UnitPP/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(a.UnitPP,'')) AS UnitPP ";
            }
            if (0 <= strCondition.IndexOf("UnitPP_Combo/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(a.UnitPP,'')) AS UnitPP_Combo ";
            }
            if (0 <= strCondition.IndexOf("UnitMM/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(a.UnitMM,'')) AS UnitMM ";
            }
            if (0 <= strCondition.IndexOf("ItemUnit/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitPO AS ItemUnit ";
            }
            if (0 <= strCondition.IndexOf("UnitCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitPP AS UnitCode ";
            }
            if (0 <= strCondition.IndexOf("WhCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " rtrim(a.WhCode )AS WhCode ";
            }
            if (0 <= strCondition.IndexOf("WhName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(ISNULL((SELECT WhName from WhMaster WHERE ComCode=N'COM4' AND WhCode= a.WhCode ),'')) AS WhName  ";
            }
            if (0 <= strCondition.IndexOf("StdSalesPrice/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" ISNULL( RTRIM ( CASE WHEN ( ( SELECT ContractPrice
                                                             FROM ItemCsMaster
                                                             WHERE ComCode = " + CFL.Q(GD.ComCode) + @"
                                                               AND CsCode = " + CFL.Q(strCond1) + @"
                                                               AND ItemCode = A.ItemCode 
                                                               AND " + CFL.Q(strCond2.Replace("-", "")) + @" BETWEEN ContractBdate AND ContractEdate
                                                               AND CurrCode = " + CFL.Q(strCond3) + @" ) IS NULL ) 
                                                         OR 
                                                         ( ( SELECT ContractPrice 
                                                             FROM ItemCsMaster
                                                             WHERE ComCode = " + CFL.Q(GD.ComCode) + @"
                                                               AND CsCode = " + CFL.Q(strCond1) + @"
                                                               AND ItemCode = A.ItemCode 
                                                               AND " + CFL.Q(strCond2.Replace("-", "")) + @" BETWEEN ContractBdate AND ContractEdate
                                                               AND CurrCode = " + CFL.Q(strCond3) + @" ) = 0 ) 
                                                         THEN FORMAT(A.StdSalesPrice, '0.00')
                                                    ELSE ( SELECT FORMAT(ContractPrice, '0.00')   
                                                           FROM ItemCsMaster
                                                           WHERE ComCode = " + CFL.Q(GD.ComCode) + @"
                                                             AND CsCode = " + CFL.Q(strCond1) + @"
                                                             AND ItemCode = A.ItemCode 
                                                             AND " + CFL.Q(strCond2.Replace("-", "")) + @" BETWEEN ContractBdate AND ContractEdate
                                                             AND CurrCode = " + CFL.Q(strCond3) + @" )
                                               END ), 0) As StdSalesPrice ";
            }
            if (0 <= strCondition.IndexOf("ItemType/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" IsNull(A.ItemType, '') as ItemType";
            }
            if (0 <= strCondition.IndexOf("LotCheck/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" IsNull(A.LotCheck, '') as LotCheck";
            }
            if (0 <= strCondition.IndexOf("InspCheck/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" IsNull(A.InspCheck, '') as InspCheck";
            }
            //판매단가
            if (0 <= strCondition.IndexOf("ItemSalesPrice/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" rtrim(isnull(FORMAT(b.ContractPrice , '0.00'), 0))  As ItemSalesPrice  ";
            }
            //고객품번
            if (0 <= strCondition.IndexOf("ItemCsCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" rtrim(isnull(ItemCsCode, ''))  As ItemCsCode  ";
            }
            if (0 <= strCondition.IndexOf("ItemGroup/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" rtrim(isnull(A.ItemGroup, ''))  As ItemGroup  ";
            }
            if (0 <= strCondition.IndexOf("GroupName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" rtrim(isnull(C.GroupName, ''))  As GroupName  ";
            }
            if (0 <= strCondition.IndexOf("StdPurPrice"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" RTRIM(IsNull(Vnd.ItemPrice, IsNull(StdPurPrice, '0.00'))) As StdPurPrice";
            }
            if (0 <= strCondition.IndexOf("OhQty"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @"  rtrim(isnull( CAST(K.OhQty As numeric(18,2)), 0) ) As OhQty";
            }
            if (0 <= strCondition.IndexOf("ExtraField2/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" rtrim(isnull(D.ExtraField2, ''))  As ExtraField2";
            }
            if (0 <= strCondition.IndexOf("ExtraField3/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" rtrim(isnull(E.CodeName, ''))  As ExtraField3";
            }
            if (0 <= strCondition.IndexOf("MakeType/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" rtrim(isnull(a.MakeType, ''))  As MakeType";
            }

            strQuery += @"
FROM ItemSiteMaster A ";
            if (0 <= strCondition.IndexOf("ItemSalesPrice/") || 0 <= strCondition.IndexOf("ItemCsCode/"))
            {
                strQuery += @"
join ItemCsMaster B on b.ComCode = " + CFL.Q(GD.ComCode) + @" and a.ItemCode = b.ItemCode  and getdate() between b.ContractBdate and b.ContractEdate
";
            }
            if (0 <= strCondition.IndexOf("StdPurPrice"))
            {
                strQuery += @"
    Left Outer Join ItemVndMaster Vnd On A.SiteCode = Vnd.SiteCode And A.ItemCode = Vnd.ItemCode 
		                           and " + CFL.Q(strCond2.Replace("-", "")) + @" Between Vnd.BDate And Vnd.EDate -- 구매일기준
                                   and Vnd.CsCode = " + CFL.Q(strCond1) + @"
                                   and Vnd.CurrCode = " + CFL.Q(strCond3);
            }
            // 총재고
            if (0 <= strCondition.IndexOf("OhQty/"))
            {
                strQuery += @"
    Left Outer Join ( Select ItemCode, WhCode, Sum(OhQty) As OhQty 
                      From mmInventory
                      Where SiteCode = " + CFL.Q(GD.SiteCode) + @" 
                        and OhQty > 0 
                      Group By ItemCode, WhCode) K On K.ItemCode = A.ItemCode
";
            }

            if (0 <= strCondition.IndexOf("GroupName/"))
            {
                strQuery += @"
    Left Outer Join ItemGroup C On C.ComCode = " + CFL.Q(GD.ComCode) + @" And A.ItemGroup = C.ItemGroup
";
            }

            //DaeAn
            if (GD.SiteNo == "DaeAn")
            {
                if (0 <= strCondition.IndexOf("ExtraField2/") || 0 <= strCondition.IndexOf("ExtraField3/"))
                {
                    strQuery += @"
    Left Outer Join ItemMaster D On D.ComCode = " + CFL.Q(GD.ComCode) + @" And A.ItemCode = D.ItemCode
";
                }
                if (0 <= strCondition.IndexOf("ExtraField3/"))
                {
                    strQuery += @"
    Left Outer Join CodeMaster E On E.ComCode = " + CFL.Q(GD.ComCode) + @" And E.CodeID = 'ItemMaster_Maker' And D.ExtraField3 = E.CodeCode
";
                }
            }

            strQuery += @"
WHERE A.ItemUse = 'Y'
  AND A.SiteCode = " + CFL.Q(GD.SiteCode) + @"";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY A.ItemCode ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }

        public DataSet GetItemSiteMasterLoad(object[] GDObj, string strWhereClause, string strCondition, ASPxGridView Grid = null)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strMsg = "L00291;L00292;L00394;L00395;L02354;L00416;L00388;L02567;L00544;L04711;L04712;L04713;L04714";
            DataSet mds = Message.MsgSet(GDObj, GD.LangID, strMsg);
            if (Grid != null)
            {
                Grid.Columns.Clear();
            }

            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("ItemCode/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "품목코드";
                    col.Caption = Message.MsgText(mds, "L00291");
                    col.FieldName = "ItemCode";
                    col.Width = new System.Web.UI.WebControls.Unit(150, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += @",";
                strQuery += " rtrim(isnull(a.ItemCode,'')) as ItemCode ";
            }

            if (0 <= strCondition.IndexOf("ItemName/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "품목명";
                    col.Caption = Message.MsgText(mds, "L00292");
                    col.FieldName = "ItemName";
                    col.Width = new System.Web.UI.WebControls.Unit(230, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " rtrim(isnull(a.ItemName,'')) as ItemName";
            }

            if (0 <= strCondition.IndexOf("ItemSpec/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "규격";
                    col.Caption = Message.MsgText(mds, "L00394");
                    col.FieldName = "ItemSpec";
                    col.Width = new System.Web.UI.WebControls.Unit(200, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " rtrim(isnull(a.ItemSpec,'')) as ItemSpec  ";
            }

            if (0 <= strCondition.IndexOf("UnitPP/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "단위";
                    col.Caption = Message.MsgText(mds, "L00395");
                    col.FieldName = "UnitPP";
                    col.Width = new System.Web.UI.WebControls.Unit(30, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(a.UnitPP,'')) AS UnitPP ";
            }

            if (0 <= strCondition.IndexOf("UnitPP_Combo/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "단위";
                    col.Caption = Message.MsgText(mds, "L00395");
                    col.FieldName = "UnitPP_Combo";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(UnitPP,'')) AS UnitPP_Combo ";
            }

            if (0 <= strCondition.IndexOf("UnitMM/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "단위";
                    col.Caption = Message.MsgText(mds, "L00395");
                    col.FieldName = "UnitMM";
                    col.Width = new System.Web.UI.WebControls.Unit(30, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(a.UnitMM,'')) AS UnitMM ";
            }

            if (0 <= strCondition.IndexOf("UnitPO/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "단위";
                    col.Caption = Message.MsgText(mds, "L00395");
                    col.FieldName = "UnitPO";
                    col.Width = new System.Web.UI.WebControls.Unit(30, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(a.UnitPO,'')) AS UnitPO ";
            }

            if (0 <= strCondition.IndexOf("UnitSD/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "단위";
                    col.Caption = Message.MsgText(mds, "L00395");
                    col.FieldName = "UnitSD";
                    col.Width = new System.Web.UI.WebControls.Unit(30, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(a.UnitSD,'')) AS UnitSD ";
            }

            if (0 <= strCondition.IndexOf("UnitEXIM/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "단위";
                    col.Caption = Message.MsgText(mds, "L00395");
                    col.FieldName = "UnitEXIM";
                    col.Width = new System.Web.UI.WebControls.Unit(30, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(a.UnitEXIM,'')) AS UnitEXIM ";
            }

            if (0 <= strCondition.IndexOf("ItemAccCode/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "품목계정코드";
                    col.Caption = Message.MsgText(mds, "L02354");
                    col.FieldName = "ItemAccCode";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " rtrim(isnull(A.ItemAccCode,'')) as ItemAccCode ";
            }

            if (0 <= strCondition.IndexOf("ItemUnit/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "단위";
                    col.Caption = Message.MsgText(mds, "L00395");
                    col.FieldName = "ItemUnit";
                    col.Width = new System.Web.UI.WebControls.Unit(30, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(a.UnitPO,'')) AS ItemUnit ";
            }

            if (0 <= strCondition.IndexOf("WhCode/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "단위";
                    col.Caption = Message.MsgText(mds, "L00388");
                    col.FieldName = "WhCode";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(a.WhCode,'')) AS WhCode ";
            }

            if (0 <= strCondition.IndexOf("StdQty/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L04711"); //"기준량";
                    col.FieldName = "StdQty";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" '1' as StdQty ";
            }

            if (0 <= strCondition.IndexOf("QtyPerGubunName/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = "QtyPerGubunName";
                    col.FieldName = "QtyPerGubunName";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" Case When A.QtyPerGubun = 'A'  Then '절대값' Else '비율' End QtyPerGubunName ";
            }

            if (0 <= strCondition.IndexOf("QtyPerGubun/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = "QtyPerGubun";
                    col.FieldName = "QtyPerGubun";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" isnull(A.QtyPerGubun, 'A') as QtyPerGubun ";
            }

            if (0 <= strCondition.IndexOf("UnitCode/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "단위";
                    col.Caption = Message.MsgText(mds, "L00395");
                    col.FieldName = "UnitCode";
                    col.Width = new System.Web.UI.WebControls.Unit(30, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(a.UnitPP,'')) AS UnitCode ";
            }

            // ItemType
            if (0 <= strCondition.IndexOf("ItemType/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();

                    col.Caption = Message.MsgText(mds, "L00416");
                    col.FieldName = "ItemType";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" IsNull(A.ItemType, '') as ItemType";
            }


            //표준판매단가
            if (0 <= strCondition.IndexOf("StdSalesPrice/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L04712");// "표준판매단가";
                    col.FieldName = "StdSalesPrice";
                    col.Width = new System.Web.UI.WebControls.Unit(90, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" rtrim(isnull(FORMAT(A.StdSalesPrice, '0.00'), 0))  As StdSalesPrice  ";
            }

            // MinOrderSize
            if (0 <= strCondition.IndexOf("MinOrderSize/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();

                    col.Caption = Message.MsgText(mds, "L00416");
                    col.FieldName = "MinOrderSize";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" CAST(A.MinOrderSize AS NUMERIC(18,0)) AS MinOrderSize";
            }

            //HSCode
            if (0 <= strCondition.IndexOf("HSCode/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();

                    col.Caption = Message.MsgText(mds, "L02567");
                    col.FieldName = "HSCode";
                    col.Width = new System.Web.UI.WebControls.Unit(40, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" A.HSCode";
            }

            if (0 <= strCondition.IndexOf("LotCheck/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();

                    col.Caption = Message.MsgText(mds, "L00544");
                    col.FieldName = "LotCheck";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" A.LotCheck";
            }

            //표준구매단가
            if (0 <= strCondition.IndexOf("StdPurPrice/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L04713");// "표준구매단가";
                    col.FieldName = "StdPurPrice";
                    col.Width = new System.Web.UI.WebControls.Unit(90, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" Rtrim(isnull(FORMAT(A.StdPurPrice, '0.00'), 0))  As StdPurPrice  ";
            }
            //판매단가
            if (0 <= strCondition.IndexOf("ItemSalesPrice/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L04714");// "판매단가";
                    col.FieldName = "ItemSalesPrice";
                    col.Width = new System.Web.UI.WebControls.Unit(90, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" rtrim(isnull(FORMAT(b.ContractPrice , '0.00'), 0))  As ItemSalesPrice  ";
            }
            strQuery += @"
FROM ItemSiteMaster A ";
            if (0 <= strCondition.IndexOf("ItemSalesPrice/"))
            {
                strQuery += @"
join ItemCsMaster B on b.ComCode = " + CFL.Q(GD.ComCode) + @" and a.ItemCode = b.ItemCode  and getdate() between b.ContractBdate and b.ContractEdate
";
            }
            strQuery += @"
WHERE A.ItemUse = 'Y'
  AND A.SiteCode = " + CFL.Q(GD.SiteCode) + @"";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY A.ItemCode ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }


        // 추가 Condition 가져오는 Item Popup
        public DataSet GetItemSiteMasterLoad(object[] GDObj, string strWhereClause, string strCondition, string strCond1, string strCond2, string strCond3, ASPxGridView Grid = null)
        {

            // Global Data
            GData GD = new GData(GDObj);

            string strMsg = "L00291;L00292;L00394;L00395;L02354;L00416;L00388;L02567;L00544;L04711;L04712;L04713;L04714;L04700;L00398;L00393;L00309;L00378";

            DataSet mds = Message.MsgSet(GDObj, GD.LangID, strMsg);

            if (Grid != null)
            {
                Grid.Columns.Clear();
            }

            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("ItemCode/"))
            {

                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "품목코드";
                    col.Caption = Message.MsgText(mds, "L00291");
                    col.FieldName = "ItemCode";
                    col.Width = new System.Web.UI.WebControls.Unit(150, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += @",";
                strQuery += " rtrim(isnull(a.ItemCode,'')) as ItemCode ";
            }

            if (0 <= strCondition.IndexOf("ItemName/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "품목명";
                    col.Caption = Message.MsgText(mds, "L00292");
                    col.FieldName = "ItemName";
                    col.Width = new System.Web.UI.WebControls.Unit(230, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " rtrim(isnull(a.ItemName,'')) as ItemName";
            }

            if (0 <= strCondition.IndexOf("ItemSpec/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "규격";
                    col.Caption = Message.MsgText(mds, "L00394");
                    col.FieldName = "ItemSpec";
                    col.Width = new System.Web.UI.WebControls.Unit(200, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " rtrim(isnull(a.ItemSpec,'')) as ItemSpec  ";
            }

            if (0 <= strCondition.IndexOf("UnitPP/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "단위";
                    col.Caption = Message.MsgText(mds, "L00395");
                    col.FieldName = "UnitPP";
                    col.Width = new System.Web.UI.WebControls.Unit(30, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(a.UnitPP,'')) AS UnitPP ";
            }

            if (0 <= strCondition.IndexOf("UnitPP_Combo/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "단위";
                    col.Caption = Message.MsgText(mds, "L00395");
                    col.FieldName = "UnitPP_Combo";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(A.UnitPP,'')) AS UnitPP_Combo ";
            }

            if (0 <= strCondition.IndexOf("UnitMM/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "단위";
                    col.Caption = Message.MsgText(mds, "L00395");
                    col.FieldName = "UnitMM";
                    col.Width = new System.Web.UI.WebControls.Unit(30, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(a.UnitMM,'')) AS UnitMM ";
            }

            if (0 <= strCondition.IndexOf("UnitPO/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "단위";
                    col.Caption = Message.MsgText(mds, "L00395");
                    col.FieldName = "UnitPO";
                    col.Width = new System.Web.UI.WebControls.Unit(30, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(a.UnitPO,'')) AS UnitPO ";
            }

            if (0 <= strCondition.IndexOf("UnitSD/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "단위";
                    col.Caption = Message.MsgText(mds, "L00395");
                    col.FieldName = "UnitSD";
                    col.Width = new System.Web.UI.WebControls.Unit(30, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(a.UnitSD,'')) AS UnitSD ";
            }

            if (0 <= strCondition.IndexOf("UnitEXIM/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "단위";
                    col.Caption = Message.MsgText(mds, "L00395");
                    col.FieldName = "UnitEXIM";
                    col.Width = new System.Web.UI.WebControls.Unit(30, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(a.UnitEXIM,'')) AS UnitEXIM ";
            }

            if (0 <= strCondition.IndexOf("ItemAccCode/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "품목계정코드";
                    col.Caption = Message.MsgText(mds, "L02354");
                    col.FieldName = "ItemAccCode";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " rtrim(isnull(A.ItemAccCode,'')) as ItemAccCode ";
            }

            if (0 <= strCondition.IndexOf("ItemAccName/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "품목계정";
                    col.Caption = Message.MsgText(mds, "L00378");
                    col.FieldName = "ItemAccName";
                    col.Width = new System.Web.UI.WebControls.Unit(60, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " rtrim(isnull(Acc.ItemAccName, '')) as ItemAccName ";
            }

            if (0 <= strCondition.IndexOf("ItemUnit/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "단위";
                    col.Caption = Message.MsgText(mds, "L00395");
                    col.FieldName = "ItemUnit";
                    col.Width = new System.Web.UI.WebControls.Unit(30, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(a.UnitPO,'')) AS ItemUnit ";
            }

            if (0 <= strCondition.IndexOf("WhCode/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "단위";
                    col.Caption = Message.MsgText(mds, "L00388");
                    col.FieldName = "WhCode";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(a.WhCode,'')) AS WhCode ";
            }

            if (0 <= strCondition.IndexOf("WhName/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L00309");
                    col.FieldName = "WhName";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(ISNULL((SELECT WhName from WhMaster WHERE ComCode=" + CFL.Q(GD.ComCode) + @" AND WhCode= a.WhCode ),'')) AS WhName ";
            }

            if (0 <= strCondition.IndexOf("StdQty/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L04711"); //"기준량";
                    col.FieldName = "StdQty";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" '1' as StdQty ";
            }

            if (0 <= strCondition.IndexOf("QtyPerGubunName/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = "QtyPerGubunName";
                    col.FieldName = "QtyPerGubunName";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" Case When A.QtyPerGubun = 'A'  Then '절대값' Else '비율' End QtyPerGubunName ";
            }

            if (0 <= strCondition.IndexOf("QtyPerGubun/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = "QtyPerGubun";
                    col.FieldName = "QtyPerGubun";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" isnull(A.QtyPerGubun, 'A') as QtyPerGubun ";
            }

            if (0 <= strCondition.IndexOf("UnitCode/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    //col.Caption = "단위";
                    col.Caption = Message.MsgText(mds, "L00395");
                    col.FieldName = "UnitCode";
                    col.Width = new System.Web.UI.WebControls.Unit(30, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(isnull(a.UnitPP,'')) AS UnitCode ";
            }

            // ItemType
            if (0 <= strCondition.IndexOf("ItemType/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();

                    col.Caption = Message.MsgText(mds, "L00416");
                    col.FieldName = "ItemType";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" IsNull(A.ItemType, '') as ItemType";
            }


            // 표준판매단가
            if (0 <= strCondition.IndexOf("StdSalesPrice/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L04712");// "표준판매단가";
                    col.FieldName = "StdSalesPrice";
                    col.Width = new System.Web.UI.WebControls.Unit(90, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += ", ";

                //  strQuery += @" rtrim(isnull(FORMAT(A.StdSalesPrice, '0.00'), 0))  As StdSalesPrice  ";
                strQuery += @" ISNULL( RTRIM ( CASE WHEN ( ( SELECT ContractPrice
                                                             FROM ItemCsMaster
                                                             WHERE ComCode = " + CFL.Q(GD.ComCode) + @"
                                                               AND CsCode = " + CFL.Q(strCond1) + @"
                                                               AND ItemCode = A.ItemCode 
                                                               AND " + CFL.Q(strCond2.Replace("-", "")) + @" BETWEEN ContractBdate AND ContractEdate
                                                               AND CurrCode = " + CFL.Q(strCond3) + @" ) IS NULL ) 
                                                         OR 
                                                         ( ( SELECT ContractPrice 
                                                             FROM ItemCsMaster
                                                             WHERE ComCode = " + CFL.Q(GD.ComCode) + @"
                                                               AND CsCode = " + CFL.Q(strCond1) + @"
                                                               AND ItemCode = A.ItemCode 
                                                               AND " + CFL.Q(strCond2.Replace("-", "")) + @" BETWEEN ContractBdate AND ContractEdate
                                                               AND CurrCode = " + CFL.Q(strCond3) + @" ) = 0 ) 
                                                         THEN FORMAT(A.StdSalesPrice, '0.00')
                                                    ELSE ( SELECT FORMAT(ContractPrice, '0.00')   
                                                           FROM ItemCsMaster
                                                           WHERE ComCode = " + CFL.Q(GD.ComCode) + @"
                                                             AND CsCode = " + CFL.Q(strCond1) + @"
                                                             AND ItemCode = A.ItemCode 
                                                             AND " + CFL.Q(strCond2.Replace("-", "")) + @" BETWEEN ContractBdate AND ContractEdate
                                                             AND CurrCode = " + CFL.Q(strCond3) + @" )
                                               END ), 0) As StdSalesPrice ";
            }

            // MinOrderSize
            if (0 <= strCondition.IndexOf("MinOrderSize/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();

                    col.Caption = Message.MsgText(mds, "L00416");
                    col.FieldName = "MinOrderSize";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" CAST(A.MinOrderSize AS NUMERIC(18,0)) AS MinOrderSize";
            }

            //HSCode
            if (0 <= strCondition.IndexOf("HSCode/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();

                    col.Caption = Message.MsgText(mds, "L02567");
                    col.FieldName = "HSCode";
                    col.Width = new System.Web.UI.WebControls.Unit(40, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" isnull(A.HSCode, '') HSCode";
            }

            if (0 <= strCondition.IndexOf("LotCheck/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();

                    col.Caption = Message.MsgText(mds, "L00544");
                    col.FieldName = "LotCheck";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" A.LotCheck";
            }

            if (0 <= strCondition.IndexOf("InspCheck/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();

                    col.Caption = Message.MsgText(mds, "L00544");
                    col.FieldName = "InspCheck";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" A.InspCheck";
            }

            //표준구매단가
            if (0 <= strCondition.IndexOf("StdPurPrice/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L04713");// "표준구매단가";
                    col.FieldName = "StdPurPrice";
                    col.Width = new System.Web.UI.WebControls.Unit(90, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                //strQuery += @" Rtrim(isnull(FORMAT(A.StdPurPrice, '0.00'), 0))  As StdPurPrice  "; ItemVndMaster 조인해서 가져오도록 수정
                strQuery += @" Rtrim(isnull(Vnd.ItemPrice, IsNull(A.StdPurPrice, '0.00')))  As StdPurPrice  ";
            }
            //판매단가
            if (0 <= strCondition.IndexOf("ItemSalesPrice/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L04714");// "판매단가";
                    col.FieldName = "ItemSalesPrice";
                    col.Width = new System.Web.UI.WebControls.Unit(90, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" rtrim(isnull(FORMAT(b.ContractPrice , '0.00'), 0))  As ItemSalesPrice  ";
            }
            //고객품번
            if (0 <= strCondition.IndexOf("ItemCsCode/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L04700");// "고객품번";
                    col.FieldName = "ItemCsCode";
                    col.Width = new System.Web.UI.WebControls.Unit(90, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" rtrim(isnull(b.ItemCsCode , ''))  As ItemCsCode ";
            }

            if (0 <= strCondition.IndexOf("ItemGroup/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L00393");// "품목군";
                    col.FieldName = "ItemGroup";
                    col.Width = new System.Web.UI.WebControls.Unit(90, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" rtrim(isnull(A.ItemGroup , ''))  As ItemGroup ";
            }

            if (0 <= strCondition.IndexOf("GroupName/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L00393");// "품목군";
                    col.FieldName = "GroupName";
                    col.Width = new System.Web.UI.WebControls.Unit(90, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += @" rtrim(isnull(N.GroupName, ''))  As GroupName ";
            }

            // 수입offer단가
            if (0 <= strCondition.IndexOf("IMPrice/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = Message.MsgText(mds, "L00398");// "단가";
                    col.FieldName = "IMPrice";
                    col.Width = new System.Web.UI.WebControls.Unit(90, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += ", ";

                //  strQuery += @" rtrim(isnull(FORMAT(A.StdSalesPrice, '0.00'), 0))  As StdSalesPrice  ";
                strQuery += @" ISNULL( RTRIM ( case when
				                                    ((select ItemPrice from ItemvndMaster where SiteCode=" + CFL.Q(GD.SiteCode) + @"
                                                            and CurrCode=" + CFL.Q(strCond1) + @"
                                                            and CsCode=" + CFL.Q(strCond2) + @" 
                                                            and ItemCode=A.ItemCode
				                                            and " + CFL.Q(GD.CurDate) + @" between Bdate and Edate) is null)			
                                                    or
				                                    ((select ItemPrice from ItemvndMaster where SiteCode=" + CFL.Q(GD.SiteCode) + @"
                                                            and CurrCode=" + CFL.Q(strCond1) + @"
                                                            and CsCode=" + CFL.Q(strCond2) + @"
                                                            and ItemCode=A.ItemCode
				                                            and " + CFL.Q(GD.CurDate) + @" between Bdate and Edate) = 0)     then  StdPurPrice 
				                                    else
				                                    (select ItemPrice from ItemvndMaster where SiteCode=" + CFL.Q(GD.SiteCode) + @"
                                                            and CurrCode=" + CFL.Q(strCond1) + @"
                                                            and CsCode=" + CFL.Q(strCond2) + @"
                                                            and ItemCode=A.ItemCode
				                                            and " + CFL.Q(GD.CurDate) + @" between Bdate and Edate) end ), 0.00) As IMPrice ";
            }

            // 재고량            
            if (0 <= strCondition.IndexOf("OhQty/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = "총 재고";
                    col.FieldName = "OhQty";
                    col.Width = new System.Web.UI.WebControls.Unit(90, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += ",";

                strQuery += @" rtrim(isnull( CAST(K.OhQty As numeric(18,2)), 0) ) As OhQty";
            }

            // 대안일레컴 SMD/DIP            
            if (0 <= strCondition.IndexOf("ExtraField2/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = "SMD/DIP";
                    col.FieldName = "ExtraField2";
                    col.Width = new System.Web.UI.WebControls.Unit(80, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += ",";

                strQuery += @" rtrim(isnull(L.ExtraField2, '')) As ExtraField2";
            }
            // 대안일레컴 SMD/DIP            
            if (0 <= strCondition.IndexOf("ExtraField3/"))
            {
                if (GD.SiteNo == "DaeAn")
                {
                    if (Grid != null)
                    {
                        GridViewDataColumn col = new GridViewDataColumn();
                        col.Caption = "제조사";
                        col.FieldName = "ExtraField3";
                        col.Width = new System.Web.UI.WebControls.Unit(100, System.Web.UI.WebControls.UnitType.Percentage);
                        Grid.Columns.Add(col);
                    }

                    if (7 < strQuery.Length)
                        strQuery += ",";

                    strQuery += @" rtrim(isnull(M.CodeName, '')) As ExtraField3";
                }
            }

            if (0 <= strCondition.IndexOf("MakeType/"))
            {
                if (Grid != null)
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Caption = "";
                    col.FieldName = "MakeType";
                    col.Width = new System.Web.UI.WebControls.Unit(0, System.Web.UI.WebControls.UnitType.Percentage);
                    Grid.Columns.Add(col);
                }

                if (7 < strQuery.Length)
                    strQuery += ",";

                strQuery += @" rtrim(isnull(a.MakeType, '')) As MakeType";
            }

            strQuery += @"
FROM ItemSiteMaster A ";

            if (0 <= strCondition.IndexOf("ItemSalesPrice/") || 0 <= strCondition.IndexOf("ItemCsCode/"))
            {
                strQuery += @"
    Join ItemCsMaster B On B.ComCode = " + CFL.Q(GD.ComCode) + @" and A.ItemCode = B.ItemCode And Convert(nchar(8), GetDate(), 112) Between B.ContractBdate And B.ContractEdate
";
            }

            if (0 <= strCondition.IndexOf("StdPurPrice"))
            {
                strQuery += @"
    Left Outer Join ItemVndMaster Vnd On A.SiteCode = Vnd.SiteCode And A.ItemCode = Vnd.ItemCode 
		                           and " + CFL.Q(strCond2.Replace("-", "")) + @" Between Vnd.BDate And Vnd.EDate -- 구매일기준
                                   and Vnd.CsCode = " + CFL.Q(strCond1) + @"
                                   and Vnd.CurrCode = " + CFL.Q(strCond3);
            }

            // 총재고
            if (0 <= strCondition.IndexOf("OhQty/"))
            {
                strQuery += @"
    Left Outer Join ( Select ItemCode, WhCode, Sum(OhQty) As OhQty 
                      From mmInventory
                      Where SiteCode = " + CFL.Q(GD.SiteCode) + @" 
                        and OhQty > 0 
                      Group By ItemCode, WhCode) K On K.ItemCode = A.ItemCode
";
            }


            if (0 <= strCondition.IndexOf("GroupName/"))
            {
                strQuery += @"
	Left Outer Join ItemGroup N On N.ComCode = " + CFL.Q(GD.ComCode) + @" And A.ItemGroup = N.ItemGroup
";
            }

            if (0 <= strCondition.IndexOf("ItemAccName/"))
            {
                strQuery += @"
    Join ItemAcc Acc On Acc.ComCode = " + CFL.Q(GD.ComCode) + @" and A.ItemAccCode = Acc.ItemAccCode
";
            }

            if (GD.SiteNo == "DaeAn")
            {
                if (0 <= strCondition.IndexOf("ExtraField2/") || 0 <= strCondition.IndexOf("ExtraField3/"))
                {
                    strQuery += @"
	Left Outer Join ItemMaster L On L.ComCode = " + CFL.Q(GD.ComCode) + @" And A.ItemCode = L.ItemCode
";
                }
                if (0 <= strCondition.IndexOf("ExtraField3/"))
                {
                    strQuery += @"
    Left Outer Join CodeMaster M On M.ComCode = " + CFL.Q(GD.ComCode) + @" And M.CodeID = 'ItemMaster_Maker' And L.ExtraField3 = M.CodeCode
";
                }
            }

            strQuery += @"
WHERE A.ItemUse = 'Y'
  AND A.SiteCode = " + CFL.Q(GD.SiteCode) + @"";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY A.ItemCode ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }


       

        public DataSet GetItemGroupMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "SELECT";

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

            strQuery += @"
FROM ItemGroup A
WHERE ComCode=" + CFL.Q(GD.ComCode) + @" 
AND GroupUse = 'Y'";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY ItemGroup ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }


        public DataSet GetUserGroupMaster(object[] GDObj, string WhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("UserGroup/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UserGroup ";
            }
            if (0 <= strCondition.IndexOf("GroupName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " GroupName ";
            }

            strQuery += @"
FROM UserGroup ";

            if (WhereClause != "")
                strQuery += WhereClause;


            strQuery += @"
ORDER BY UserGroup ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }


        public DataSet GetEmpMasterPopup(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("EmpCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.EmpCode) AS EmpCode ";
            }
            if (0 <= strCondition.IndexOf("EmpName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.EmpName) AS EmpName ";
            }

            strQuery += @"
FROM EmpMaster A
LEFT JOIN DeptMaster B ON A.ComCode = B.ComCode AND A.SiteCode = B.SiteCode AND A.DefaultDept = B.DeptCode
WHERE A.ComCode = " + CFL.Q(GD.ComCode) + @"
  AND A.SiteCode = " + CFL.Q(GD.SiteCode) + @" ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY A.EmpName, A.EmpCode ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }

       

        public DataSet TreePopup(object[] GDObj, string strWhereClause, string strSiteCode, string strChildDeptCode, string strChildDeptName, string strParentDeptCode, string strParentDeptName)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = @" 
Select Convert(int, DeptLevel)+1 as ObjLevel, ParentDept as ParentObj, DeptCode as ObjID
, case when (select COUNT(ParentDept) from DeptMaster B where a.ComCode = b.ComCode and a.DeptCode = b.ParentDept) >0 then 'N' else 'Y' end as LeafCheck
, DeptCode as ObjID2, DeptName as ObjName	
From DeptMaster a ";

            bool bWhereWritten = false;

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += " where " + strWhereClause;
                bWhereWritten = true;
            }

            string strDeptCode = strParentDeptCode;
            string strDeptName = strParentDeptName;
            if (strDeptCode != "")
            {
                strQuery += (bWhereWritten ? " and " : " where ") + " DeptCode  like " + CFL.Q(strDeptCode + "%");

                bWhereWritten = true;
            }
            if (strDeptName != "")
            {
                strQuery += (bWhereWritten ? " and " : " where ") + " DeptName  like " + CFL.Q(strDeptName + "%");
                bWhereWritten = true;
            }

            string strDeptCode1 = strChildDeptCode;
            string strDeptName1 = strChildDeptName;
            if (strDeptCode1 != "")
            {
                strQuery += (bWhereWritten ? " and " : " where ") + " DeptCode  like " + CFL.Q(strDeptCode1 + "%");
                bWhereWritten = true;
            }
            if (strDeptName1 != "")
            {
                strQuery += (bWhereWritten ? " and " : " where ") + " DeptName  like " + CFL.Q(strDeptName1 + "%");
                bWhereWritten = true;
            }

            // Restrict to Values starts with User Key-In
            string strTemp = strSiteCode;
            if (strTemp != "")
                strQuery += (bWhereWritten ? " and " : " where ") + "SiteCode like " + CFL.Q(strTemp + "%");

            strQuery += " order by ObjLevel, ParentObj, ObjID";


            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }

        

      
        public DataSet GetItemAccMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("ItemAccCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(ItemAccCode) As ItemAccCode ";
            }
            if (0 <= strCondition.IndexOf("ItemAccName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemAccName ";
            }
            if (0 <= strCondition.IndexOf("ItemType/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemType ";
            }

            strQuery += @"
FROM ItemAcc
WHERE ItemAccUse = 'Y' ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY SortKey ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }



        public DataSet Popup_CcMaster(object[] GDObj, string WhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("CcCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " rtrim(CcCode) as CcCode ";
            }
            if (0 <= strCondition.IndexOf("CcName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " rtrim(CcName) as CcName ";
            }

            strQuery += @"
FROM CcMaster ";

            if (WhereClause != "")
                strQuery += @"
Where " + WhereClause;


            strQuery += @"
ORDER BY CcCode ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }

        public DataSet Popup_PcMaster(object[] GDObj, string WhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "SELECT";

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
FROM PcMaster
Where ComCode = " + CFL.Q(GD.ComCode);

            if (WhereClause != "")
                strQuery += WhereClause;

            strQuery += @"
ORDER BY PcCode ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }

        public DataSet GetLangMaster(object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = @"
Select LangID, LangName	from LangMaster with (NOLOCK) where LangUse = " + CFL.Q("Y") + @"	order by	SortKey";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }
            finally
            {
                strQuery = null;
            }

            return ds;
        }

        //사업장 정보 데이터셋으로 출력
        public DataSet GetSiteMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("SiteCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.SiteCode) AS SiteCode ";
            }
            if (0 <= strCondition.IndexOf("SiteName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.SiteName) AS SiteName ";
            }
            if (0 <= strCondition.IndexOf("sdArPostingPoint/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.sdArPostingPoint) AS sdArPostingPoint ";
            }

            strQuery += @"
From SiteMaster A, CompanyMaster B 
Where A.ComCode = B.ComCode AND A.ComCode = " + CFL.Q(GD.ComCode);

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += strWhereClause;
            }

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }

        //시스템유형
        public DataSet GetCaseMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("CaseID/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.CaseID) AS CaseID ";
            }
            if (0 <= strCondition.IndexOf("SysCase/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.SysCase) AS SysCase ";
            }
            if (0 <= strCondition.IndexOf("CaseSys/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " rtrim(CaseID) + '|' + rtrim(SysCase) as CaseSys ";
            }
            if (0 <= strCondition.IndexOf("SysCaseName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.SysCaseName) AS SysCaseName ";
            }

            strQuery += @"
From CaseMaster A
Where A.CaseUse='Y' ";


            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += strWhereClause;
            }

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }

        public DataSet GetTypeMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("TypeCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(TypeCode) AS TypeCode ";
            }
            if (0 <= strCondition.IndexOf("TypeCode2/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(TypeCode) AS TypeCode2 ";
            }
            if (0 <= strCondition.IndexOf("TypeName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(TypeName) AS TypeName ";
            }
            if (0 <= strCondition.IndexOf("TypeName2/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(TypeName) AS TypeName2 ";
            }

            strQuery += @"
FROM TypeMaster A
WHERE A.TypeUse = 'Y' ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY A.SortKey ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }
        

        public DataSet GetCompanyMaster(object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = @"
SELECT ComCode, ComName, ComNameEng
FROM CompanyMaster
WHERE ComUse = 'Y'
  and ComCode = " + CFL.Q(GD.ComCode);

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }

        public DataSet GetTaxMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("TaxCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.TaxCode) AS TaxCode ";
            }
            if (0 <= strCondition.IndexOf("TaxName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.TaxName) AS TaxName ";
            }
            // 2021.07.08 노재호 추가
            if (0 <= strCondition.IndexOf("TaxType/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.TaxType) AS TaxType ";
            }
            if (0 <= strCondition.IndexOf("TaxRatio/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.TaxRatio) AS TaxRatio ";
            }
            strQuery += @"
FROM TaxMaster A
WHERE A.TaxUse = 'Y' AND A.ComCode=" + CFL.Q(GD.ComCode);

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY A.SortKey ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }

        public DataSet GetCurrencyMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("CurrName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " CurrName ";
            }
            if (0 <= strCondition.IndexOf("BaseExchRatio/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " BaseExchRatio ";
            }
            if (0 <= strCondition.IndexOf("CurrCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " CurrCode ";
            }

            strQuery += @"
FROM CurrencyMaster A 
WHERE CurrUse = 'Y' AND A.ComCode=" + CFL.Q(GD.ComCode);

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY A.SortKey ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }

        public DataSet GetExchangeRateForDate(object[] GDObj, string strDate, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("CurrName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.CurrName) As CurrName ";
            }
            if (0 <= strCondition.IndexOf("BaseExchRatio/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " IsNull(B.ExchRate1, A.BaseExchRatio) As BaseExchRatio ";
            }
            if (0 <= strCondition.IndexOf("CurrCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.CurrCode) As CurrCode ";
            }

            strQuery += @"
From CurrencyMaster A 
	Left Join ExchangeRate B On A.ComCode = B.ComCode And A.CurrCode = B.CurrCode And B.ExchDate = IsNull(" + CFL.Q(strDate) + @", Convert(nchar(8), GetDate(), 112))
Where A.ComCode = " + CFL.Q(GD.ComCode) + @"
  And A.CurrUse = 'Y'";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY A.SortKey ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }

        public DataSet GetPaymentMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "SELECT";

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
            if (0 <= strCondition.IndexOf("PayType/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " A.PayType ";
            }

            strQuery += @"
From PaymentMaster A 
    Left Outer Join AccMaster B on A.ArAcc = B.AccCode and B.CoaCode = " + CFL.Q(GD.CoaCode) + @"
    Left Outer Join AccMaster C on A.ApAcc = C.AccCode and C.CoaCode = " + CFL.Q(GD.CoaCode) + @"
Where A.PayUse = N'Y' 
and A.ComCode = " + CFL.Q(GD.ComCode);

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }


        public DataSet GetrpPayCondition(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("PayCondCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(PayCondCode) AS PayCondCode ";
            }
            if (0 <= strCondition.IndexOf("PayCondName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.PayCondName) AS PayCondName ";
            }

            strQuery += @"
FROM rpPayCondition A 
WHERE A.ComCode = " + CFL.Q(GD.ComCode) + @"
  AND A.PayCondUse = 'Y' ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY A.PayCondCode ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }


      
        public DataSet GetUnitMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("UnitCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " TRIM(UnitCode) AS UnitCode ";
            }

            if (0 <= strCondition.IndexOf("UnitCode1/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(UnitCode) AS UnitCode1";
            }

            if (0 <= strCondition.IndexOf("UnitName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " TRIM(UnitName) AS UnitName ";
            }

            strQuery += @"
FROM UnitMaster
WHERE UnitUse = 'Y' AND ComCode=" + CFL.Q(GD.ComCode);

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY SortKey ";


            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }


        public DataSet GetWhMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("WhCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(WhCode) AS WhCode ";
            }
            if (0 <= strCondition.IndexOf("WhName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(WhName) AS WhName ";
            }

            strQuery += @"
FROM WhMaster
WHERE WhUse = 'Y'
And SiteCode = " + CFL.Q(GD.SiteCode) + @"
And ComCode = " + CFL.Q(GD.ComCode) + " ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY WhCode ";


            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }

        public DataSet GetNoMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            GData GD = new GData(GDObj);

            // Generate Column Information
            string strQuery = "SELECT";

            // First Column
            if (0 <= strCondition.IndexOf("NoType/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " NoType ";
            }
            if (0 <= strCondition.IndexOf("NoName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " NoName ";
            }
            if (0 <= strCondition.IndexOf("ModuleID/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ModuleID ";
            }


            strQuery += @"	
FROM NoMaster ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += " where " + strWhereClause;

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }

       
        public DataSet GetCaseAcc(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("CaseCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(CaseCode) AS CaseCode ";
            }

            if (0 <= strCondition.IndexOf("CaseName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(CaseName) AS CaseName ";
            }

            if (0 <= strCondition.IndexOf("SysCase/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(SysCase) AS SysCase ";
            }
            if (0 <= strCondition.IndexOf("CaseID/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(CaseID) AS CaseID ";
            }


            strQuery += @"
FROM CaseAcc
WHERE ComCode = " + CFL.Q(GD.ComCode) + @"
  AND CaseUse = 'Y'";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY SortKey ";


            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }

       

        public DataSet GetCaseOrigin(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("OriginCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(OriginCode) AS OriginCode ";
            }

            if (0 <= strCondition.IndexOf("OriginName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(OriginName) AS OriginName ";
            }


            strQuery += @"
FROM CaseOrigin
WHERE LangID = " + CFL.Q(GD.LangID) + @" ";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY OriginCode ";


            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }

        public DataSet GetCaseID(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("CaseID/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                //strQuery += " CaseID + '|' + RTRIM(CaseID) AS CaseID ";
                strQuery += " RTRIM(CaseID) AS CaseID ";
            }

            if (0 <= strCondition.IndexOf("IDName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(IDName) AS IDName ";
            }


            strQuery += @"
FROM CaseID
WHERE IDUse = 'Y'";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY CaseID ";


            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }

       

        public DataSet GetCodeSiteMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("CodeCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " rtrim(CodeCode) as CodeCode ";
            }
            if (0 <= strCondition.IndexOf("CodeName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " rtrim(CodeName) as CodeName ";
            }

            strQuery += @"
FROM CodeSiteMaster";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += @" 
Where " + strWhereClause;

            strQuery += @" 
  and CodeUse='Y' ";

            strQuery += @" 
Order By SortKey ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }


        public DataSet GetEmpDeptPopup(object[] GDObj, string strToday, string strWhere)
        {
            // Global Data
            GData GD = new GData(GDObj);


            string strQuery = @"
select  EmpCode ,  EmpName ,  DefaultDept ,  DeptName  
from EmpMaster A
left join DeptMaster B on A.DefaultDept = B.DeptCode and A.ComCode = B.ComCode
left join CodeMaster C on A.ComCode = C.ComCode and A.JobPosition = C.CodeCode  and C.CodeID = 'hrJobPosition'
left join CodeMaster D on A.ComCode = D.ComCode and A.JobDuty = D.CodeCode  and D.CodeID = 'hrJobDuty'
Where A.ComCode = " + CFL.Q(GD.ComCode) + @" 
    and A.SiteCode = " + CFL.Q(GD.SiteCode) + @" 
    and A.EmpBDate <= " + CFL.Q(strToday) + @" 
    and case when isnull(A.EmpEDate, '') ='' then '20501231' else A.EmpEDate end  >= " + CFL.Q(strToday);

            if (strWhere != "")
            {
                strQuery += strWhere;
            }

            strQuery += "Order By DefaultDept, EmpName";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }



        public DataSet GetMngEmpMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "SELECT";

            if (0 <= strCondition.IndexOf("MngGroup/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.MngGroup) as MngGroup ";
            }
            if (0 <= strCondition.IndexOf("MngName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(B.MngName) AS MngName ";
            }
            if (0 <= strCondition.IndexOf("EmpCode/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(A.EmpCode) AS EmpCode ";
            }
            if (0 <= strCondition.IndexOf("EmpName/"))
            {
                if (7 < strQuery.Length)
                    strQuery += ",";
                strQuery += " RTRIM(C.EmpName) AS EmpName ";
            }

            strQuery += @"
from MngEmpMapping A, ManageGroup B, EmpMaster C 
where  A.SiteCode = " + CFL.Q(GD.SiteCode) + @"
and A.MngGroup = B.MngGroup 
and A.SiteCode = B.SiteCode 
and A.MngType = B.MngType
and A.EmpCode = C.EmpCode";


            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            strQuery += @"
ORDER BY A.SortKey ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }
        public DataSet GetItemMaster(object[] GDObj, string strWhereClause, string strCondition)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "Select ";

            if (0 <= strCondition.IndexOf("ItemCode/"))
            {
                if (16 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemCode ";
            }

            if (0 <= strCondition.IndexOf("ItemName/"))
            {
                if (16 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemName ";
            }

            if (0 <= strCondition.IndexOf("ItemSpec/"))
            {
                if (16 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemSpec ";
            }

            if (0 <= strCondition.IndexOf("UnitSD/"))
            {
                if (16 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitSD ";
            }

            if (0 <= strCondition.IndexOf("UnitPP/"))
            {
                if (16 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitPP ";
            }

            if (0 <= strCondition.IndexOf("UnitPO/"))
            {
                if (16 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitPO ";
            }

            if (0 <= strCondition.IndexOf("UnitMM/"))
            {
                if (16 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitMM ";
            }

            if (0 <= strCondition.IndexOf("UnitCO/"))
            {
                if (16 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitCO ";
            }

            if (0 <= strCondition.IndexOf("UnitQM/"))
            {
                if (16 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitQM ";
            }

            if (0 <= strCondition.IndexOf("UnitEXIM/"))
            {
                if (16 < strQuery.Length)
                    strQuery += ",";
                strQuery += " UnitEXIM ";
            }

            if (0 <= strCondition.IndexOf("ItemAccCode/"))
            {
                if (16 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemAccCode ";
            }

            if (0 <= strCondition.IndexOf("ItemType/"))
            {
                if (16 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemType ";
            }

            if (0 <= strCondition.IndexOf("ItemGroup/"))
            {
                if (16 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ItemGroup ";
            }

            // Query 문 생성
            strQuery += @"
From ItemMaster
Where ItemUse = 'Y'";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;


            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }




        public DataSet GetExchangeRate(object[] GDObj, string strCondition, string strCurrCode, string strExchDate)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "Select ";

            if (0 <= strCondition.IndexOf("CurrCode/"))
            {
                if (16 < strQuery.Length)
                    strQuery += ",";
                strQuery += " CurrCode ";
            }

            if (0 <= strCondition.IndexOf("ExchDate/"))
            {
                if (16 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ExchDate ";
            }

            if (0 <= strCondition.IndexOf("ExchRate1/"))
            {
                if (16 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ExchRate1 ";
            }

            if (0 <= strCondition.IndexOf("ExchRate2/"))
            {
                if (16 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ExchRate2 ";
            }

            if (0 <= strCondition.IndexOf("ExchRate3/"))
            {
                if (16 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ExchRate3 ";
            }

            if (0 <= strCondition.IndexOf("ExchRate4/"))
            {
                if (16 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ExchRate4 ";
            }

            if (0 <= strCondition.IndexOf("ExchRate5/"))
            {
                if (16 < strQuery.Length)
                    strQuery += ",";
                strQuery += " ExchRate5 ";
            }

            strQuery += " from ExchangeRate";

            bool bWhere = false;
            if (strCurrCode != null && 0 != strCurrCode.Length)
            {
                strQuery += bWhere ? " And " : " Where ";
                strQuery += "CurrCode = " + CFL.Q(strCurrCode);
                bWhere = true;
            }

            if (strExchDate != null && 0 != strExchDate.Length)
            {
                strQuery += bWhere ? " And " : " Where ";
                strQuery += " ExchDate = " + CFL.Q(strExchDate);
                bWhere = true;
            }


            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }


    }
}
