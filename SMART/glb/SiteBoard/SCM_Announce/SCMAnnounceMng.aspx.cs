using SMART;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class glb_SiteBoard_SCM_Announce_SCMAnnounceMng : PageBase
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

            txtUserID.Text = G.UserID;

            Grid.DataBind();
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
        DataSet ds2 = new DataSet();

        // 수정
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


    // 현재는 이부분을 안쓰고... Grid_BatchUpdate 부분에서 처리
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


        // SCM 정보 저장   ---------------------------------------------------------

        string[] RowItem = txtRow.Text.Split(',');

        string[] sarUserID = new string[RowItem.Length / 2];
        string[] sarUserName = new string[RowItem.Length / 2];


        if (RowItem.Length > 0)
        {
            // 값세팅
            for (int i = 0, j = 0; i < RowItem.Length / 2; i++, j += 2)
            {
                sarUserID[i] = RowItem[j];
                sarUserName[i] = RowItem[j + 1];
            }          
        }
                

        ArrayList alSave = ObjA.Save_SCM(G.D, G.SiteCode, strBoard_ID, strNum, strTitle, strContent, strWriter, strWriter_ID, strEmail, strPWD, strImportance, sarUserID, sarUserName);
                        

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


    // ***********************************************************************


    protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
    {
        if (e.Parameters == "NEW")
        {
            strJob = "NEW";
            Grid.ClearSort();
            Grid.DataBind();
        }      
        else if (e.Parameters == "UPDATE")
        {
            Grid.UpdateEdit();
        }
        else
        {         
            Grid.ClearSort();
            Grid.DataBind();
        }
    }


    public void Grid_DataBinding(object sender, EventArgs e)
    {
        if (strJob == "NEW")
        {
            strJob = "";
            return;
        }

        DataSet ds = new DataSet();

        // 수정모드
        if (txtBoard_ID.Text.Trim() != "")
        {
            ds = ObjS.getSCMID_Saved(G.D, txtBoard_ID.Text.Trim(), txtNum.Text.Trim());
        }
        // 신규모드
        else
        {
            ds = ObjS.getSCMID(G.D);
        }
                       

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("No");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Rows[i]["No"] = (i + 1).ToString();
                }
            }
        }

        Grid.DataSource = ds;        
    }


    protected void Grid_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
    {
        DevExpress.Web.ASPxGridView gv = (DevExpress.Web.ASPxGridView)sender;
        gv.JSProperties.Add("cp_ret_message", strOutMsg);
        gv.JSProperties.Add("cp_ret_flag", strOutFlag);
        gv.JSProperties.Add("cp_ret_job", strJob);
    }

    protected void Grid_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
    {
               
    }


    protected void Grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
    {
        e.Handled = false;

        strJob = "SAVE";
        strOutFlag = "N";

        DataTable dt = CFL.GetDataCols(e, Grid);
        DataRow[] dtr = dt.Select("UseYn = 'Y'");

        if (txtTitle.Text.Trim() == "")
        {
            strOutMsg = "타이틀 정보가 없습니다.";
            return;
        }

        if (txtPw.Text.Trim().Length < 4)
        {
            strOutMsg = "비번은 4자리 이상으로 등록하여 주십시오.";
            return;
        }

        if (txtPw.Text.Trim().Length > 10)
        {
            strOutMsg = "비번은 10자리 이하로 등록하여 주십시오.";
            return;
        }

        if (txtEmail.Text.Trim() == "")
        {
            strOutMsg = "작성자의 이메일 정보가 없습니다.";
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


        // SCM 정보 저장   --------------------------------------
        int rCnt = dtr.Length;

        string[] sarUserID = new string[rCnt];
        string[] sarUserName = new string[rCnt];

        for (int i = 0; i < rCnt; i++)
        {
            sarUserID[i] = dtr[i]["UserID"].ToString();
            sarUserName[i] = dtr[i]["UserName"].ToString();
        }
        // SCM 정보 저장 : 끝  ----------------------------------


        ArrayList alSave = ObjA.Save_SCM(G.D, G.SiteCode, strBoard_ID, strNum, strTitle, strContent, strWriter, strWriter_ID, strEmail, strPWD, strImportance, sarUserID, sarUserName);


        if (alSave[0].ToString() != "00")
        {
            strOutMsg = alSave[1].ToString();
        }
        else
        {
            strOutFlag = "Y";

            e.Handled = true;

            strOutMsg = "등록 되었습니다.";
        }
    }

}