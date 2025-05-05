using DevExpress.XtraReports.UI;
using SMART;
using SMART.COMMON.DS;
using SMART.REPORT;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MASTER_MasterPage : System.Web.UI.MasterPage
{
    GObj G = new GObj();

    protected void Page_Load(object sender, EventArgs e)
    {
        G = (GObj)Session["G"];

        txtLoginUserID.Text = G.UserID.ToUpper();
    }

    protected void mastCallback_Callback(object source, DevExpress.Web.CallbackEventArgs e)
    {
        string strParam = e.Parameter;
        string[] sarParam = strParam.Split('&');
        string strRet = "";

        string strGubun = "";

        for (int i = 0; i < sarParam.Length; i++)
        {
            if (sarParam[i].ToUpper().IndexOf("GUBUN") >= 0)
            {

                strGubun = CFL.GetStr(sarParam[i].Split('=')[1]).ToUpper();

                if (strGubun == "CODE")
                {
                    strRet = this.GetCodeCode(strParam);
                }
                if (strGubun == "CS")
                {
                    strRet = this.GetCsMaster(strParam);
                }
                if (strGubun == "CC")
                {
                    strRet = this.GetCCMaster(strParam);
                }           
                
               
                if (strGubun == "DEPT")
                {
                    strRet = this.GetDeptMaster(strParam);
                }
                if (strGubun == "EMP")
                {
                    strRet = this.GetEmpCode(strParam);
                }
                if (strGubun == "IGM")
                {
                    strRet = this.GetItemGroupMaster(strParam);
                }
                if (strGubun == "ITEM")
                {
                    strRet = this.GetItemMaster(strParam);
                }
                if (strGubun == "UGM")
                {
                    strRet = this.GetUserGroupMaster(strParam);
                }
                if (strGubun == "USER")
                {
                    strRet = this.GetUserMaster(strParam);
                }
                if (strGubun == "ACC")
                {
                    strRet = this.GetAccMaster(strParam);
                }
                if (strGubun == "ACCMASTER")
                {
                    strRet = this.GetAccMasterVer2(strParam);
                }
                if (strGubun == "HS")
                {
                    strRet = this.GetHsMaster(strParam);
                }
                if (strGubun == "NA")
                {
                    strRet = this.GetNationMaster(strParam);
                }
                if (strGubun == "REG")
                {
                    strRet = this.GetRegionMaster(strParam);
                }
                if (strGubun == "WH")
                {
                    strRet = this.GetWhMaster(strParam);
                }

                break;
            }
        }

        e.Result = strRet;
    }

    #region 공통코드
    string GetCodeCode(string strParam)
    {
        string strRet = "";

        string[] sarParam = strParam.Split('&');
        string strGubun = "";
        string strName = "";
        string strCode = "";
        string strCodeID = "";
        string strWhere = "";
        string strCondition = "";

        for (int i = 0; i < sarParam.Length; i++)
        {
            if (sarParam[i].ToUpper().IndexOf("GUBUN=") >= 0)
            {
                strGubun = CFL.GetStr(sarParam[i].Split('=')[1]).ToUpper();
            }
            else if (sarParam[i].ToUpper().IndexOf("CODE=") >= 0)
            {
                strCode = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("NAME=") >= 0)
            {
                strName = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("CODEID=") >= 0)
            {
                strCodeID = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("W=") >= 0)
            {
                strWhere = sarParam[i].Replace("W=", "");
            }
            else if (sarParam[i].ToUpper().IndexOf("CONDITION=") >= 0)
            {
                strCondition = sarParam[i].Split('=')[1];
            }
        }

        string strWhereClause = "";
        if (strCode != "")
        {
            strWhereClause += @"
AND A.CodeCode LIKE " + CFL.Q(strCode + "%") + @" ";
        }
        if (strName != "")
        {
            strWhereClause += @"
AND A.CodeName LIKE " + CFL.Q("%" + strName + "%") + @" ";
        }
        if (strCodeID != "")
        {
            strWhereClause += @"
AND A.CodeID = " + CFL.Q(strCodeID) + @" ";
        }
        if (strWhere != "")
        {
            strWhereClause += strWhere;
        }

        CommonCodeClass Obj = new CommonCodeClass();
        DataSet ds = Obj.GetCodeMaster(G.D, strWhereClause, strCondition);

        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            if(strCondition.Contains("Factor1"))
            {
                strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["CodeCode"])
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["CodeName"])
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["Factor1"]);
            }
            else
            {
                strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["CodeCode"])
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["CodeName"]);
            }
        }
        else
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + strCode
                + "|" + strName;
        }

        return strRet;
    }


    #endregion

    #region 사원정보
    string GetEmpCode(string strParam)
    {
        string strRet = "";

        string[] sarParam = strParam.Split('&');
        string strGubun = "";
        string strName = "";
        string strCode = "";
        string strWhere = "";
        string strCondition = "";

        for (int i = 0; i < sarParam.Length; i++)
        {
            if (sarParam[i].ToUpper().IndexOf("GUBUN=") >= 0)
            {
                strGubun = CFL.GetStr(sarParam[i].Split('=')[1]).ToUpper();
            }
            else if (sarParam[i].ToUpper().IndexOf("CODE=") >= 0)
            {
                strCode = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("NAME=") >= 0)
            {
                strName = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("W=") >= 0)
            {
                strWhere = sarParam[i].ToUpper().Replace("W=", "");
            }
            else if (sarParam[i].ToUpper().IndexOf("CONDITION=") >= 0)
            {
                strCondition = sarParam[i].Split('=')[1];
            }
        }
        if(strCondition =="")
        {
            strCondition = "EmpCode/EmpName/";
        }

        string strWhereClause = "";
        if (strCode != "")
        {
            strWhereClause += @"
AND A.EmpCode LIKE " + CFL.Q(strCode + "%") + @" ";
        }
        if (strName != "")
        {
            strWhereClause += @"
AND A.EmpName LIKE " + CFL.Q("%" + strName + "%") + @" ";
        }
        if (strWhere != "")
        {
            strWhereClause += strWhere;
        }


        CommonCodeClass Obj = new CommonCodeClass();
        DataSet ds = Obj.GetEmpMasterPopup(G.D, strWhereClause, strCondition);

        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["EmpCode"])
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["EmpName"]);
        }
        else
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + strCode
                + "|" + strName;
        }

        return strRet;
    }
    #endregion

    #region 거래처코드
    string GetCsMaster(string strParam)
    {
        string strRet = "";

        string[] sarParam = strParam.Split('&');
        string strGubun = "";
        string strName = "";
        string strCode = "";
        string strCondition = "CsCode/CsName/";
        string strWhereClause = "";
        for (int i = 0; i < sarParam.Length; i++)
        {
            if (sarParam[i].ToUpper().IndexOf("GUBUN") >= 0 && strGubun == "")
            {
                strGubun = CFL.GetStr(sarParam[i].Split('=')[1]).ToUpper();

            }
            else if (sarParam[i].ToUpper().IndexOf("CONDITION") >= 0)
            {
                strCondition = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("CODE") >= 0)
            {
                strCode = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("NAME") >= 0)
            {
                strName = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("W") >= 0)
            {
                strWhereClause = sarParam[i].Replace("W=", "");
            }
        }

        
        if (strCode != "")
        {
            strWhereClause += @"
AND A.CsCode LIKE " + CFL.Q(strCode + "%") + @" ";
        }
        if (strName != "")
        {
            strWhereClause += @"
AND A.CsName LIKE " + CFL.Q("%" + strName + "%") + @" ";
        }

        CommonCodeClass Obj = new CommonCodeClass();
        DataSet ds = Obj.GetCsMaster(G.D, strWhereClause, strCondition);

        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            //strRet = ds.Tables[0].Rows.Count.ToString()
            //    + "|" + strParam
            //    + "|" + strGubun
            //    + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["CsCode"])
            //    + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["CsName"]);
            strRet = ds.Tables[0].Rows.Count.ToString() + "|" + strParam + "|" + strGubun;
            string[] sarCondition = strCondition.Split('/');
            for (int i = 0; i < sarCondition.Length; i++)
            {
                switch (sarCondition[i].Trim())
                {
                    case "CsCode":
                        strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["CsCode"]);
                        break;
                    case "CsNameFull":
                        strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["CsNameFull"]);
                        break;
                    case "CsName":
                        strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["CsName"]);
                        break;
                    case "CurrCode":
                        strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["CurrCode"]);
                        break;
                    case "BaseExchRatio":
                        strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["BaseExchRatio"]);
                        break;
                    case "TaxCodeAr":
                        strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["TaxCodeAr"]);
                        break;
                    case "TaxCodeAp":
                        strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["TaxCodeAp"]);
                        break;
                    case "CsInitial":
                        strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["CsInitial"]);
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + strCode
                + "|" + strName;
        }

        return strRet;
    }
    #endregion

    #region 코스트센터
    string GetCCMaster(string strParam)
    {
        string strRet = "";

        string[] sarParam = strParam.Split('&');
        string strGubun = "";
        string strName = "";
        string strCode = "";
        string strWhere = "";
        string strCondition = "";

        for (int i = 0; i < sarParam.Length; i++)
        {
            if (sarParam[i].ToUpper().IndexOf("GUBUN=") >= 0)
            {
                strGubun = CFL.GetStr(sarParam[i].Split('=')[1]).ToUpper();
            }
            else if (sarParam[i].ToUpper().IndexOf("W=") >= 0)
            {
                strWhere = sarParam[i].Replace("W=", "");
            }
            else if (sarParam[i].ToUpper().IndexOf("CODE=") >= 0)
            {
                strCode = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("NAME=") >= 0)
            {
                strName = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("CONDITION=") >= 0)
            {
                strCondition = sarParam[i].Split('=')[1];
            }
        }

        string strWhereClause = "1=1 ";
        if (strCode != "")
        {
            strWhereClause += @"
AND CcCode LIKE " + CFL.Q(strCode + "%") + @" ";
        }
        if (strName != "")
        {
            strWhereClause += @"
AND CcName LIKE " + CFL.Q("%" + strName + "%") + @" ";
        }
        if (strWhere != "")
        {
            strWhereClause += strWhere;
        }

        CommonCodeClass Obj = new CommonCodeClass();
        DataSet ds = Obj.Popup_CcMaster(G.D, strWhereClause, strCondition);

        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["CcCode"])
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["CcName"]);
        }
        else
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + strCode
                + "|" + strName;
        }

        return strRet;              
        
    }
    #endregion

    #region 부서코드
    string GetDeptMaster(string strParam)
    {
        string strRet = "";

        string[] sarParam = strParam.Split('&');
        string strGubun = "";
        string strName = "";
        string strCode = "";
        string strWhere = "";
        string strCondition = "";

        for (int i = 0; i < sarParam.Length; i++)
        {
            if (sarParam[i].ToUpper().IndexOf("GUBUN=") >= 0)
            {
                strGubun = CFL.GetStr(sarParam[i].Split('=')[1]).ToUpper();
            }
            else if (sarParam[i].ToUpper().IndexOf("CODE=") >= 0)
            {
                strCode = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("NAME=") >= 0)
            {
                strName = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("W=") >= 0)
            {
                strWhere = sarParam[i].Replace("W=", "");
            }
            else if (sarParam[i].ToUpper().IndexOf("CONDITION=") >= 0)
            {
                strCondition = sarParam[i].Split('=')[1];
            }
        }

        string strWhereClause = "";
        if (strCode != "")
        {
            strWhereClause += @"
AND A.DeptCode LIKE " + CFL.Q(strCode + "%") + @" ";
        }
        if (strName != "")
        {
            strWhereClause += @"
AND A.DeptName LIKE " + CFL.Q("%" + strName + "%") + @" ";
        }
        if (strWhere != "")
        {
            strWhereClause += strWhere;
        }

        CommonCodeClass Obj = new CommonCodeClass();
        DataSet ds = Obj.GetDeptMaster(G.D, strWhereClause, strCondition);

        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["DeptCode"])
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["DeptName"]);
        }
        else
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + strCode
                + "|" + strName;
        }

        return strRet;
    }
    #endregion

    #region 품목그룹코드
    string GetItemGroupMaster(string strParam)
    {
        string strRet = "";

        //string[] sarParam = strParam.Split('&');
        //string strGubun = "";
        //string strName = "";
        //string strCode = "";

        //for (int i = 0; i < sarParam.Length; i++)
        //{
        //    if (sarParam[i].ToUpper().IndexOf("GUBUN") >= 0)
        //    {
        //        strGubun = CFL.GetStr(sarParam[i].Split('=')[1]).ToUpper();
        //    }
        //    else if (sarParam[i].ToUpper().IndexOf("CODE") >= 0)
        //    {
        //        strCode = sarParam[i].Split('=')[1];
        //    }
        //    else if (sarParam[i].ToUpper().IndexOf("NAME") >= 0)
        //    {
        //        strName = sarParam[i].Split('=')[1];
        //    }
        //}
        string[] sarParam = strParam.Split('&');
        string strGubun = "";
        string strName = "";
        string strCode = "";
        string strWhere = "";
        string strCondition = "";

        for (int i = 0; i < sarParam.Length; i++)
        {
            if (sarParam[i].ToUpper().IndexOf("GUBUN=") >= 0)
            {
                strGubun = CFL.GetStr(sarParam[i].Split('=')[1]).ToUpper();
            }
            else if (sarParam[i].ToUpper().IndexOf("CODE=") >= 0)
            {
                strCode = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("NAME=") >= 0)
            {
                strName = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("W=") >= 0)
            {
                strWhere = sarParam[i].Replace("W=", "");
            }
            else if (sarParam[i].ToUpper().IndexOf("CONDITION=") >= 0)
            {
                strCondition = sarParam[i].Split('=')[1];
            }
        }


        string strWhereClause = "";
        if (strCode != "")
        {
            strWhereClause += @"
AND A.ItemGroup LIKE " + CFL.Q(strCode + "%") + @" ";
        }
        if (strName != "")
        {
            strWhereClause += @"
AND A.GroupName LIKE " + CFL.Q("%" + strName + "%") + @" ";
        }
        if (strWhere != "")
        {
            strWhereClause += strWhere;
        }

        CommonCodeClass Obj = new CommonCodeClass();
        DataSet ds = Obj.GetItemGroupMaster(G.D, strWhereClause, strCondition);

        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["ItemGroup"])
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["GroupName"]);
        }
        else
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + strCode
                + "|" + strName;
        }

        return strRet;
    }
    #endregion

    #region 품목코드
    string GetItemMaster(string strParam)
    {
        string strRet = "";

        string[] sarParam = strParam.Split('&');
        string strGubun = "";
        string strName = "";
        string strCode = "";
        string strWhere = "";
        //string strCondition = "ItemCode/ItemName/ItemSpec/UnitPP/"; //default condition
        string strCondition = ""; //default condition
        string strCond1 = "";
        string strCond2 = "";
        string strCond3 = "";

        for (int i = 0; i < sarParam.Length; i++)
        {
            if (sarParam[i].ToUpper().IndexOf("CONDITION=") >= 0)
            {
                strCondition += sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("GUBUN=") >= 0)
            {
                strGubun = CFL.GetStr(sarParam[i].Split('=')[1]).ToUpper();
            }
            else if (sarParam[i].ToUpper().IndexOf("CODE=") >= 0)
            {
                strCode = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("NAME=") >= 0)
            {
                strName = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("UNIT=") >= 0)
            {
                strCondition = "ItemCode/ItemName/ItemSpec/";
                strCondition += sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("W=") >= 0)
            {
                strWhere = sarParam[i].Replace("W=", "");
            }
            else if (sarParam[i].ToUpper().IndexOf("TYPE=") >= 0)
            {
                if (sarParam[i].Split('=')[1] == "NPPROUT")
                {
                    strWhere += " and ItemCode in ( select distinct ItemCode from nppSubstituteMaster where SiteCode = " + CFL.Q(G.SiteCode) + @" )  and ItemType = 'MAT'";
                }
                if (sarParam[i].Split('=')[1] == "BOMCHK")
                {
                    strWhere += " and ItemCode in (select ItemCode from nppBomHeader)";
                }
            }
            else if(sarParam[i].ToUpper().IndexOf("COND1") >= 0)
            {
                strCond1 = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("COND2") >= 0)
            {
                strCond2 = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("COND3") >= 0)
            {
                strCond3 = sarParam[i].Split('=')[1];
            }
        }
        if (strCondition == "")
            strCondition = "ItemCode/ItemName/ItemSpec/ItemUnit/LotCheck/";
        string strWhereClause = "";
        if (strCode != "")
        {
            strWhereClause += @"
AND a.ItemCode LIKE " + CFL.Q(strCode + "%") + @" ";
        }
        if (strName != "")
        {
            strWhereClause += @"
AND a.ItemName LIKE " + CFL.Q("%" + strName + "%") + @" ";
        }
        if (strWhere != "")
        {
            strWhereClause += strWhere;
        }

        CommonCodeClass Obj = new CommonCodeClass();
        DataSet ds = Obj.GetItemSiteMasterLoad(G.D, strWhereClause, strCondition, strCond1, strCond2, strCond3);

        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["ItemCode"])
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["ItemName"]);
            //+ "|" + CFL.GetStr(ds.Tables[0].Rows[0]["ItemSpec"]);
            //+ "|" + CFL.GetStr(ds.Tables[0].Rows[0]["ItemUnit"]);
            if (strCondition.IndexOf("ItemSpec/") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["ItemSpec"]);
            }
            if (strCondition.IndexOf("UnitPP/") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["UnitPP"]);
            }
            if (strCondition.IndexOf("UnitPP_Combo/") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["UnitPP_Combo"]);
            }
            if (strCondition.IndexOf("UnitMM/") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["UnitMM"]);
            }
            if (strCondition.IndexOf("ItemAccCode/") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["ItemAccCode"]);
            }
            if (strCondition.IndexOf("ItemAccCode1/") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["ItemAccCode1"]);
            }
            if (strCondition.IndexOf("StdQty/") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["StdQty"]);
            }
            if (strCondition.IndexOf("QtyPerGubunName/") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["QtyPerGubunName"]);
            }
            if (strCondition.IndexOf("QtyPerGubun/") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["QtyPerGubun"]);
            }
            if (strCondition.IndexOf("UnitCode/") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["UnitCode"]);
            }
            if (strCondition.IndexOf("ItemUnit/") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["ItemUnit"]);
            }
            
            if (strCondition.IndexOf("ItemType/") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["ItemType"]);
            }
            if (strCondition.IndexOf("LotCheck/") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["LotCheck"]);
            }
            if (strCondition.IndexOf("OhQty") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["OhQty"]);
            }

            if (strCondition.IndexOf("ItemSalesPrice/") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["ItemSalesPrice"]);
            }
            if (strCondition.IndexOf("WhCode/") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["WhCode"]);
            }
            if (strCondition.IndexOf("WhName/") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["WhName"]);
            }
            if (strCondition.IndexOf("StdSalesPrice/") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["StdSalesPrice"]);
            }
            if (strCondition.IndexOf("ItemGroup/") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["ItemGroup"]);
            }
            if(strCondition.IndexOf("StdPurPrice") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["StdPurPrice"]);
            }
            if (strCondition.IndexOf("GroupName") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["GroupName"]);
            }
            if (strCondition.IndexOf("ExtraField2") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["ExtraField2"]);
            }
            if (strCondition.IndexOf("ExtraField3") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["ExtraField3"]);
            }
            if (strCondition.IndexOf("MakeType") >= 0)
            {
                strRet += "|" + CFL.GetStr(ds.Tables[0].Rows[0]["MakeType"]);
            }
        }
        else
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + strCode
                + "|" + strName;
        }

        return strRet;
    }
    #endregion

    #region 유저그룹코드
    string GetUserGroupMaster(string strParam)
    {
        string strRet = "";

        string[] sarParam = strParam.Split('&');
        string strGubun = "";
        string strName = "";
        string strCode = "";

        for (int i = 0; i < sarParam.Length; i++)
        {
            if (sarParam[i].ToUpper().IndexOf("GUBUN") >= 0)
            {
                strGubun = CFL.GetStr(sarParam[i].Split('=')[1]).ToUpper();
            }
            else if (sarParam[i].ToUpper().IndexOf("CODE") >= 0)
            {
                strCode = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("NAME") >= 0)
            {
                strName = sarParam[i].Split('=')[1];
            }
        }

        string strWhereClause = "";
        if (strCode != "")
        {
            strWhereClause += @"
AND A.UserGroup LIKE " + CFL.Q(strCode + "%") + @" ";
        }
        if (strName != "")
        {
            strWhereClause += @"
AND A.GroupName LIKE " + CFL.Q("%" + strName + "%") + @" ";
        }

        CommonCodeClass Obj = new CommonCodeClass();
        DataSet ds = Obj.GetUserGroupMaster(G.D, strWhereClause, "UserGroup/GroupName/");

        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["UserGroup"])
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["GroupName"]);
        }
        else
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + strCode
                + "|" + strName;
        }

        return strRet;
    }
    #endregion

    #region 유저코드 (로그인)
    string GetUserMaster(string strParam)
    {
        string strRet = "";

        string[] sarParam = strParam.Split('&');
        string strGubun = "";
        string strName = "";
        string strCode = "";
        string strWhere = "";
        string strCondition = "";

        for (int i = 0; i < sarParam.Length; i++)
        {
            if (sarParam[i].ToUpper().IndexOf("GUBUN=") >= 0)
            {
                strGubun = CFL.GetStr(sarParam[i].Split('=')[1]).ToUpper();
            }
            else if (sarParam[i].ToUpper().IndexOf("CODE=") >= 0)
            {
                strCode = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("NAME=") >= 0)
            {
                strName = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("W=") >= 0)
            {
                strWhere = sarParam[i].Replace("W=", "");
            }
            else if (sarParam[i].ToUpper().IndexOf("CONDITION=") >= 0)
            {
                strCondition = sarParam[i].Split('=')[1];
            }
        }

        string strWhereClause = "";
        if (strCode != "")
        {
            strWhereClause += @"
AND UserID LIKE " + CFL.Q(strCode + "%") + @" ";
        }
        if (strName != "")
        {
            strWhereClause += @"
AND UserName LIKE " + CFL.Q("%" + strName + "%") + @" ";
        }
        if (strWhere != "")
        {
            strWhereClause += strWhere;
        }

        CommonCodeClass Obj = new CommonCodeClass();
        DataSet ds = Obj.GetUserMaster(G.D, strWhereClause, strCondition);

        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["UserID"])
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["UserName"]);
        }
        else
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + strCode
                + "|" + strName;
        }

        return strRet;
    }
    #endregion

    #region AccMaster
    string GetAccMaster(string strParam)
    {
        string strRet = "";

        string[] sarParam = strParam.Split('&');
        string strGubun = "";
        string strName = "";
        string strCHKYN = "Y";
        string strCode = "";
        string strWhere = "";
        string strCondition = "";
        for (int i = 0; i < sarParam.Length; i++)
        {
            if (sarParam[i].ToUpper().IndexOf("GUBUN=") >= 0)
            {
                strGubun = CFL.GetStr(sarParam[i].Split('=')[1]).ToUpper();
            }
            else if (sarParam[i].ToUpper().IndexOf("W=") >= 0)
            {
                strWhere = sarParam[i].Replace("W=", "");
            }
            else if (sarParam[i].ToUpper().IndexOf("CONDITION=") >= 0)
            {
                strCondition = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("CODE=") >= 0)
            {
                strCode = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("NAME=") >= 0)
            {
                strName = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("CHKYN=") >= 0)
            {
                strCHKYN = sarParam[i].Split('=')[1];
            }            
        }

        string strWhereClause = "";
        if (strCHKYN == "Y")
        {
            strWhereClause = " AND LEFT(AccType,1) != RIGHT(AccType,1) AND CoaCode = " + CFL.Q(G.CoaCode);
        }
        if (strCode != "")
        {
            strWhereClause += @"
And AccCode like  " + CFL.Q(strCode + "%");
        }
        if (strName != "")
        {
            strWhereClause += @"
And AccName like " + CFL.Q("%"+ strName + "%");
        }
        if (strWhere != "")
        {
            strWhereClause += strWhere;
        }
        if(strCondition =="")
        {
            strCondition = "AccCode/AccName/";
        }
        CommonCodeClass Obj = new CommonCodeClass();
        DataSet ds = Obj.GetAccMaster(G.D, strWhereClause, strCondition);

        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["AccCode"])
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["AccName"]);
        }
        else
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + strCode
                + "|" + strName;
        }

        return strRet;
    }
    #endregion
    #region AccMaster
    string GetAccMasterVer2(string strParam)
    {
        string strRet = "";

        string[] sarParam = strParam.Split('&');
        string strGubun = "";
        string strName = "";
        string strCode = "";
        string strWhere = "";
        string strCondition = "";
        for (int i = 0; i < sarParam.Length; i++)
        {
            if (sarParam[i].ToUpper().IndexOf("GUBUN=") >= 0)
            {
                strGubun = CFL.GetStr(sarParam[i].Split('=')[1]).ToUpper();
            }
            else if (sarParam[i].ToUpper().IndexOf("W=") >= 0)
            {
                strWhere = sarParam[i].Replace("W=", "");
            }
            else if (sarParam[i].ToUpper().IndexOf("CONDITION=") >= 0)
            {
                strCondition = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("CODE=") >= 0)
            {
                strCode = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("NAME=") >= 0)
            {
                strName = sarParam[i].Split('=')[1];
            }
        }

        string strWhereClause = "";
        if (strCode != "")
        {
            strWhereClause += @"
And AccCode like  " + CFL.Q(strCode + "%");
        }
        if (strName != "")
        {
            strWhereClause += @"
And AccName like " + CFL.Q(strName + "%");
        }
        if (strWhere != "")
        {
            strWhereClause += strWhere;
        }
        if (strCondition == "")
        {
            strCondition = "AccCode/AccName/";
        }
        CommonCodeClass Obj = new CommonCodeClass();
        DataSet ds = Obj.GetAccMaster(G.D, strWhereClause, strCondition);

        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["AccCode"])
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["AccName"]);
        }
        else
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + strCode
                + "|" + strName;
        }

        return strRet;
    }
    #endregion

    #region HsMaster
    string GetHsMaster(string strParam)
    {
        string strRet = "";

        string[] sarParam = strParam.Split('&');
        string strGubun = "";
        string strName = "";
        string strCode = "";

        for (int i = 0; i < sarParam.Length; i++)
        {
            if (sarParam[i].ToUpper().IndexOf("GUBUN") >= 0)
            {
                strGubun = CFL.GetStr(sarParam[i].Split('=')[1]).ToUpper();
            }
            else if (sarParam[i].ToUpper().IndexOf("CODE") >= 0)
            {
                strCode = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("NAME") >= 0)
            {
                strName = sarParam[i].Split('=')[1];
            }
        }

        string strWhereClause = "";
        if (strCode != "")
        {
            strWhereClause += @"
And A.HSCode = " + CFL.Q(strCode);
        }
        if (strName != "")
        {
            strWhereClause += @"
And A.HSName = " + CFL.Q(strName);
        }

        CommonCodeClass Obj = new CommonCodeClass();
        DataSet ds = Obj.GetHsMaster(G.D, strWhereClause, "HSCode/HSName/");

        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["HSCode"])
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["HSName"]);
        }
        else
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + strCode
                + "|" + strName;
        }

        return strRet;
    }
    #endregion

    #region NationMaster
    string GetNationMaster(string strParam)
    {
        string strRet = "";

        string[] sarParam = strParam.Split('&');
        string strGubun = "";
        string strName = "";
        string strCode = "";

        for (int i = 0; i < sarParam.Length; i++)
        {
            if (sarParam[i].ToUpper().IndexOf("GUBUN") >= 0)
            {
                strGubun = CFL.GetStr(sarParam[i].Split('=')[1]).ToUpper();
            }
            else if (sarParam[i].ToUpper().IndexOf("CODE") >= 0)
            {
                strCode = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("NAME") >= 0)
            {
                strName = sarParam[i].Split('=')[1];
            }
        }

        string strWhereClause = "";
        if (strCode != "")
        {
            strWhereClause += @"
AND A.NationCode LIKE " + CFL.Q(strCode + "%") + @" ";
        }
        if (strName != "")
        {
            strWhereClause += @"
AND A.NationName LIKE " + CFL.Q("%" + strName + "%") + @" ";
        }

        CommonCodeClass Obj = new CommonCodeClass();
        DataSet ds = Obj.GetNationMaster(G.D, strWhereClause, "NationCode/NationName/");

        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["NationCode"])
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["NationName"]);
        }
        else
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + strCode
                + "|" + strName;
        }

        return strRet;
    }
    #endregion

    #region RegionMaster
    string GetRegionMaster(string strParam)
    {
        string strRet = "";

        string[] sarParam = strParam.Split('&');
        string strGubun = "";
        string strName = "";
        string strCode = "";

        for (int i = 0; i < sarParam.Length; i++)
        {
            if (sarParam[i].ToUpper().IndexOf("GUBUN") >= 0)
            {
                strGubun = CFL.GetStr(sarParam[i].Split('=')[1]).ToUpper();
            }
            else if (sarParam[i].ToUpper().IndexOf("CODE") >= 0)
            {
                strCode = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("NAME") >= 0)
            {
                strName = sarParam[i].Split('=')[1];
            }
        }

        string strWhereClause = "";
        if (strCode != "")
        {
            strWhereClause += @"
AND RegionCode LIKE " + CFL.Q(strCode + "%") + @" ";
        }
        if (strName != "")
        {
            strWhereClause += @"
AND RegionName LIKE " + CFL.Q("%" + strName + "%") + @" ";
        }

        CommonCodeClass Obj = new CommonCodeClass();
        DataSet ds = Obj.GetRegionMaster(G.D, strWhereClause, "RegionCode/RegionName/");

        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["RegionCode"])
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["RegionName"]);
        }
        else
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + strCode
                + "|" + strName;
        }

        return strRet;
    }
    #endregion

    #region WhMaster
    string GetWhMaster(string strParam)
    {
        string strRet = "";

        string[] sarParam = strParam.Split('&');
        string strGubun = "";
        string strName = "";
        string strCode = "";

        for (int i = 0; i < sarParam.Length; i++)
        {
            if (sarParam[i].ToUpper().IndexOf("GUBUN") >= 0)
            {
                strGubun = CFL.GetStr(sarParam[i].Split('=')[1]).ToUpper();
            }
            else if (sarParam[i].ToUpper().IndexOf("CODE") >= 0)
            {
                strCode = sarParam[i].Split('=')[1];
            }
            else if (sarParam[i].ToUpper().IndexOf("NAME") >= 0)
            {
                strName = sarParam[i].Split('=')[1];
            }
        }

        string strWhereClause = "";
        if (strCode != "")
        {
            strWhereClause += @"
And WhCode = " + CFL.Q(strCode);
        }
        if (strName != "")
        {
            strWhereClause += @"
And WhName = " + CFL.Q(strName);
        }

        CommonCodeClass Obj = new CommonCodeClass();
        DataSet ds = Obj.GetWhMaster(G.D, strWhereClause, "WhCode/WhName/");

        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["WhCode"])
                + "|" + CFL.GetStr(ds.Tables[0].Rows[0]["WhName"]);
        }
        else
        {
            strRet = ds.Tables[0].Rows.Count.ToString()
                + "|" + strParam
                + "|" + strGubun
                + "|" + strCode
                + "|" + strName;
        }

        return strRet;
    }
    #endregion

    protected void btnExcelFormDown_Click(object sender, EventArgs e)
    {
        string sReportID = txtMasterRID.Text.Trim();
        string sParam = txtPrintParam.Text.Trim();
        string sFileNm = txtDownFileName.Text.Trim();

        if (sReportID == "")
        {
            return;
        }
        //sParam = sParam + "|" + txtMasterData1.Text.Trim()
        //    + "|" + txtMasterData2.Text.Trim()
        //    + "|" + txtMasterData3.Text.Trim();

        if (txtMasterData1.Text.Trim() != "") sParam += "|" + txtMasterData1.Text.Trim();
        if (txtMasterData2.Text.Trim() != "") sParam += "|" + txtMasterData2.Text.Trim();
        if (txtMasterData3.Text.Trim() != "") sParam += "|" + txtMasterData3.Text.Trim();
        if (txtMasterData4.Text.Trim() != "") sParam += "|" + txtMasterData4.Text.Trim();
        if (txtMasterData5.Text.Trim() != "") sParam += "|" + txtMasterData5.Text.Trim();
        if (txtMasterData6.Text.Trim() != "") sParam += "|" + txtMasterData6.Text.Trim();
        if (txtMasterData7.Text.Trim() != "") sParam += "|" + txtMasterData7.Text.Trim();
        if (txtMasterData8.Text.Trim() != "") sParam += "|" + txtMasterData8.Text.Trim();
        if (txtMasterData9.Text.Trim() != "") sParam += "|" + txtMasterData9.Text.Trim();
        if (txtMasterData10.Text.Trim() != "") sParam += "|" + txtMasterData10.Text.Trim();
        if (txtMasterData11.Text.Trim() != "") sParam += "|" + txtMasterData11.Text.Trim();
        if (txtMasterData12.Text.Trim() != "") sParam += "|" + txtMasterData12.Text.Trim();
        if (txtMasterData13.Text.Trim() != "") sParam += "|" + txtMasterData13.Text.Trim();
        if (txtMasterData14.Text.Trim() != "") sParam += "|" + txtMasterData14.Text.Trim();
        if (txtMasterData15.Text.Trim() != "") sParam += "|" + txtMasterData15.Text.Trim();

        REPORT_LINK LINK = new REPORT_LINK(G.D, sReportID, sParam);
        XtraReport RPT = LINK.Classification();

        // REPORT_LINK에서 DataSorce가 아닌 리포트로 여러 PAGE를 받아와도 1장으로 바뀌기에 수정.. 
        if (RPT.Pages.Count <= 1)
        {
            RPT.CreateDocument();
        }

        using (MemoryStream ms = new MemoryStream())
        {
            Page.Response.Clear();

            if (txtSheetDiv.Text == "Y")
            {
                RPT.PrintingSystem.ContinuousPageNumbering = true;
                RPT.ExportOptions.Xlsx.ExportMode = DevExpress.XtraPrinting.XlsxExportMode.SingleFilePageByPage;
            }

            RPT.ExportToXlsx(ms);

            byte[] bR = ms.ToArray();
            if (sFileNm == "")
                Page.Response.AddHeader("Content-Disposition", "inline; filename=" + sReportID + ".xlsx");
            else
                Page.Response.AddHeader("Content-Disposition", "inline; filename=" + DateTime.Now.ToString("yyyy년MM월dd일") + sFileNm + ".xlsx");
            Page.Response.ContentType = "application/xlsx";
            Page.Response.OutputStream.Write(bR, 0, bR.Length);
            Page.Response.End();
        }
    }
}
