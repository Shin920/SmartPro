using SMART;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class glb_SiteBoard_Announce_AnnounceMng : PageBase
{
    GObj G = new GObj();

    string strOutFlag = "";
    string strOutMsg = "";
    string strJob = "";

    SMART.COMMON.DA.Announce ObjA = new SMART.COMMON.DA.Announce();
    SMART.COMMON.DS.Announce ObjS = new SMART.COMMON.DS.Announce();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();

        G = (GObj)Session["G"];

        if (!IsPostBack)
        {
            if (Request["Action"] == "Load")
            {
                txtBoard_ID.Text = CFL.GetStr(Request["ID"]);
                txtNum.Text = CFL.GetStr(Request["NUM"]);
            }

            // 메인화면에서는 저장 & 삭제 버튼 감추기
            if (Request["From"] == "Main")
            {
                btnSave.ClientVisible = false;
                btnDelete.ClientVisible = false;

                dotImage.Width = 0;
            }

            txtUserID.Text = G.UserID;
        }
    }

    protected void callBackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
        string job = hidField["job"].ToString();

        switch (job)
        {
            case "SELECT":
                JobSelect();
                break;
            case "SAVE":
                JobSave();
                break;
            case "DELETE":
                JobDelete();
                break;
            default:
                break;
        }
    }

    void JobSelect()
    {
        DataSet ds = new DataSet();

        if (txtBoard_ID.Text.Trim() != "")
        {
            ds = ObjS.GetAnounceMngLoad(G.D, txtBoard_ID.Text.Trim(), txtNum.Text.Trim());
        }
        else
        {
            ds = ObjS.GetAnounceMngNewDataLoad(G.D, txtUserID.Text.Trim());
        }

        hidField.Clear();
        hidField["job"] = "SELECT";

        if (ds.Tables[0].Rows.Count > 0)
        {
            hidField["status"] = "Y";
            hidField["msg"] = "정상 조회 되었습니다.";

            //값넣기
            CFL.setDataSetToDevHiddenField(ds, hidField);
        }
        else
        {
            hidField["status"] = "N";
            hidField["msg"] = "조회 정보가 없습니다.";
        }
    }

    void JobSave()
    {
        hidField.Clear();
        hidField["job"] = "SAVE";

        if (txtTitle.Text.Trim() == "")
        {
            hidField["status"] = "N";
            hidField["msg"] = "타이틀 정보가 없습니다.";
            return;
        }
        if (txtPw.Text.Trim().Length < 4)
        {
            hidField["status"] = "N";
            hidField["msg"] = "비번은 4자리 이상으로 등록하여 주십시오.";
            return;
        }
        if (txtPw.Text.Trim().Length > 10)
        {
            hidField["status"] = "N";
            hidField["msg"] = "비번은 10자리 이하로 등록하여 주십시오.";
            return;
        }
        if (txtEmail.Text.Trim() == "")
        {
            hidField["status"] = "N";
            hidField["msg"] = "작성자의 이메일 정보가 없습니다.";
            return;
        }

        string strBoard_ID = txtBoard_ID.Text.Trim();
        string strNum = txtNum.Text.Trim();
        string strTitle = txtTitle.Text.Trim();
        string strContent = memoContent.Text;
        string strWriter = G.UserName;
        string strWriter_ID = G.UserID;
        string strEmail = txtEmail.Text.Trim();
        string strPWD = txtPw.Text.Trim();
        string strImportance = CFL.GetStr(rbImportance.Value);

        ArrayList alSave = ObjA.Save(G.D, G.SiteCode, strBoard_ID, strNum, strTitle, strContent, strWriter, strWriter_ID, strEmail, strPWD, strImportance);
        if (alSave[0].ToString() != "00")
        {
            hidField["status"] = "N";
            hidField["msg"] = alSave[1].ToString();
        }
        else
        {
            hidField["status"] = "Y";
            hidField["msg"] = "등록 되었습니다.";
        }
    }

    void JobDelete()
    {
        string strBoard_ID = txtBoard_ID.Text.Trim();
        string strNum = txtNum.Text.Trim();

        ArrayList alSave = ObjA.Delete(G.D, strBoard_ID, strNum);
        if (alSave[0].ToString() != "00")
        {
            hidField["status"] = "N";
            hidField["msg"] = alSave[1].ToString();
        }
        else
        {
            hidField["status"] = "Y";
            hidField["msg"] = "삭제 되었습니다.";
        }
    }
}