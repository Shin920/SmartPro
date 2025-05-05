using SMART;
using SMART.OM.DA;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class glb_MainContents_MainUserInfo : PageBase
{
    GObj G = new GObj();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();

        G = (GObj)Session["G"];

        if (!IsPostBack)
        {
            MainDataSetting();
        }
    }

    void MainDataSetting()
    {
        // 스탬프 유무 확인후 가져오기 
        string strFileName = Server.MapPath("/SmartPro/UserImages/EmpPhoto/" + G.ClientID + "/" + G.ComCode + "_" + G.EmpCode + ".bmp");

        if (File.Exists(strFileName))
            imgUser.ImageUrl = "/SmartPro/UserImages/EmpPhoto/" + G.ClientID + "/" + G.ComCode + "_" + G.EmpCode + ".bmp";
        else
            imgUser.ImageUrl = "";

        lblEmpNameTitle.InnerText = "사원정보";
        lblEmpName.InnerText = "(" + G.EmpCode + ") " + G.EmpName;

        lblSIteNameTitle.InnerText = "사업장정보";
    //  lblSIteName.InnerText = "(" + G.SiteCode + ") " + G.SiteName;
        lblSIteName.InnerText = G.SiteName;

        SMART.COMMON.DS.MainContents Obj = new SMART.COMMON.DS.MainContents();
        DataSet ds = Obj.GetEmpMasterInfo(G.D);

        if (ds.Tables[0].Rows.Count > 0)
        {
            lblDeptNameTitle.InnerText = "부서정보";
        //  lblDeptName.InnerText = "(" + CFL.GetStr(ds.Tables[0].Rows[0]["DeptCode"]) + ") " + CFL.GetStr(ds.Tables[0].Rows[0]["DeptName"]);
            lblDeptName.InnerText = CFL.GetStr(ds.Tables[0].Rows[0]["DeptName"]);

            lblMobileNoTitle.InnerText = "전화번호";
            lblMobileNo.InnerText = CFL.GetStr(ds.Tables[0].Rows[0]["MobileNo"]);

            lblEmailTitle.InnerText = "E-mail";
            lblEmail.InnerText = CFL.GetStr(ds.Tables[0].Rows[0]["Email"]);

            lblIpsaDayTitle.InnerText = "입사일자";
            lblIpsaDay.InnerText = CFL.GetStr(ds.Tables[0].Rows[0]["IpsaDay"]);

            lblBirtyDayTitle.InnerText = "생일";
            lblBirtyDay.InnerText = CFL.GetStr(ds.Tables[0].Rows[0]["BirthDay"]);

        }
    }
}