using DevExpress.Web;
using SMART;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MASTER_MasterMain : System.Web.UI.MasterPage
{
    GObj G;

    public string strMenu01 = "";
    public string strMenu02 = "";
    public string strMenu03 = "";
    public string strMenu04 = "";
    public string strMenu05 = "";
    public string strMenu06 = "";
    public string strMenu07 = "";
    public string strMenu08 = "";
    public string strMenu09 = "";
    public string strMenu10 = "";
    public string strMenu11 = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        G = (GObj)Session["G"];

        if (!IsPostBack)
        {
            txtLoginUserID.Text = G.UserID.ToUpper();

            LangLoad();

            // (2022.7.8) 권한에 따른 메뉴 Visible 처리를 위해, aspx 부분에서 이 곳으로 이동.
            callbackTree_Callback(null, null);
        }
    }


    void LangLoad()
    {

        string[] sarCode = new string[]
        {
            "L01443", "L01444", "L01445", "L01446", "L01447"
            , "L01448", "L01449", "L01450", "L01451", "L02875"
        };

        SMART.COMMON.DS.MessageMaster mm = new SMART.COMMON.DS.MessageMaster();
        DataSet ds = mm.GetFormLangDataLoad(G.D, G.LangID, sarCode);
        
        //초기화 후 히든필드에 담기
        hidLang2.Clear();
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            hidLang2[CFL.GetStr(ds.Tables[0].Rows[i]["MCode"])] = CFL.GetStr(ds.Tables[0].Rows[i]["Message"]);
        }
        //가져온 다국어 정보 라벨 세팅하기.
        SettingLang();

    }

    void SettingLang()
    {

        //초기화 후 히든필드에 담기
        strMenu01 = "MY";

        /*
        strMenu03 = CFL.GetStr(hidLang2["L01443"]); //계약관리   <---   현장관리
        strMenu04 = CFL.GetStr(hidLang2["L01444"]); //경영관리   <---   운영관리
        strMenu05 = CFL.GetStr(hidLang2["L01445"]); //인사급여
        strMenu06 = CFL.GetStr(hidLang2["L02875"]); //재무회계
        strMenu07 = CFL.GetStr(hidLang2["L01447"]); //자재관리   <---   대응관리
        strMenu08 = CFL.GetStr(hidLang2["L01448"]); //서비스관리 <---   정산관리
        strMenu09 = CFL.GetStr(hidLang2["L01449"]); //기준정보   <---   공통관리
        strMenu02 = CFL.GetStr(hidLang2["L01450"]); //시스템관리
        */

        strMenu03 = "영업관리";
        strMenu04 = "구매관리";
        strMenu05 = "생산관리";
        strMenu06 = "품질관리";
        strMenu07 = "설비관리";
        strMenu08 = "재무관리";
        strMenu09 = "인사급여";
        
        strMenu10 = "SCM관리";
        strMenu11 = "AI관리";

        strMenu02 = "운영관리";        
    }

    protected void callbackTree_Callback(object sender, CallbackEventArgsBase e)
    {
        SMART.COMMON.DS.ModuleObj obj = new SMART.COMMON.DS.ModuleObj();
                
        string[] sarUser = new string[] { "USER" };
        string[] sar1 = new string[] { "OM" };                          // 운영관리
        string[] sar2 = new string[] { "SD", "SM", "EX", "IM" };        // 영업관리
        string[] sar3 = new string[] { "PO", "MM" };                    // 구매관리
        string[] sar4 = new string[] { "NP", "OS" };                    // 생산관리
        string[] sar5 = new string[] { "QM" };                          // 품질관리
        string[] sar6 = new string[] { "PM" };                          // 설비관리
        string[] sar7 = new string[] { "GL", "AA", "TR", "CO", "BG" };  // 재무관리
        string[] sar8 = new string[] { "HR" };                          // 인사급여
        string[] sar10 = new string[] { "PP" };                         // SCM관리
        string[] sar11 = new string[] { "ES" };                         // AI

        DataSet dsMy = obj.Navigator_User(G.D, sarUser, "");
        DataSet ds2 = obj.Navigator(G.D, sar2, "");             // 영업관리
        DataSet ds3 = obj.Navigator(G.D, sar3, "");             // 구매관리
        DataSet ds4 = obj.Navigator(G.D, sar4, "");             // 생산관리
        DataSet ds5 = obj.Navigator(G.D, sar5, "");             // 품질관리
        DataSet ds6 = obj.Navigator_NoRoot(G.D, sar6, "");      // 설비관리
        DataSet ds7 = obj.Navigator(G.D, sar7, "");             // 재무관리
        DataSet ds8 = obj.Navigator(G.D, sar8, "");             // 인사급여

        DataSet ds10 = obj.Navigator_NoRoot(G.D, sar10, "");    // SCM관리
        DataSet ds11 = obj.Navigator_NoRoot(G.D, sar11, "");    // AI

        DataSet ds1 = obj.Navigator(G.D, sar1, "");             // 운영관리

        // My 메뉴
        if (dsMy.Tables[0].Rows.Count > 0)
        {
            CreateTreeViewNodesRecursive(dsMy.Tables[0], this.treeMY.Nodes, dsMy.Tables[0].Rows[0]["ParentObj"].ToString().Trim());
        }
                
        // 영업관리       
        if (ds2.Tables[0].Rows.Count > 0)
        {
            CreateTreeViewNodesRecursive(ds2.Tables[0], this.tree2.Nodes, ds2.Tables[0].Rows[0]["ParentObj"].ToString().Trim());
        }
        else
        {
            menu03.Visible = false;
        }
        
        // 구매관리        
        if (ds3.Tables[0].Rows.Count > 0)
        {
            CreateTreeViewNodesRecursive(ds3.Tables[0], this.tree3.Nodes, ds3.Tables[0].Rows[0]["ParentObj"].ToString().Trim());
        }
        else
        {
            menu04.Visible = false;
        }
        
        // 생산관리        
        if (ds4.Tables[0].Rows.Count > 0)
        {
            CreateTreeViewNodesRecursive(ds4.Tables[0], this.tree4.Nodes, ds4.Tables[0].Rows[0]["ParentObj"].ToString().Trim());
        }
        else
        {
            menu05.Visible = false;
        }        

        // 품질관리        
        if (ds5.Tables[0].Rows.Count > 0)
        {
            CreateTreeViewNodesRecursive(ds5.Tables[0], this.tree5.Nodes, ds5.Tables[0].Rows[0]["ParentObj"].ToString().Trim());
        }
        else
        {
            menu06.Visible = false;
        }        

        // 설비관리        
        if (ds6.Tables[0].Rows.Count > 0)
        {
            CreateTreeViewNodesRecursive(ds6.Tables[0], this.tree6.Nodes, ds6.Tables[0].Rows[0]["ParentObj"].ToString().Trim());
        }
        else
        {
            menu07.Visible = false;
        }
        
        // 재무관리
        if (ds7.Tables[0].Rows.Count > 0)
        {
            CreateTreeViewNodesRecursive(ds7.Tables[0], this.tree7.Nodes, ds7.Tables[0].Rows[0]["ParentObj"].ToString().Trim());
        }
        else
        {
            menu08.Visible = false;
        }

        // 인사급여
        if (ds8.Tables[0].Rows.Count > 0)
        {
            CreateTreeViewNodesRecursive(ds8.Tables[0], this.tree8.Nodes, ds8.Tables[0].Rows[0]["ParentObj"].ToString().Trim());
        }
        else
        {
            menu09.Visible = false;
        }


        // SCM관리
        if (ds10.Tables[0].Rows.Count > 0)
        {
            CreateTreeViewNodesRecursive(ds10.Tables[0], this.tree10.Nodes, ds10.Tables[0].Rows[0]["ParentObj"].ToString().Trim());
        }
        else
        {
            menu10.Visible = false;
        }


        // AI
        if (ds11.Tables[0].Rows.Count > 0)
        {
            CreateTreeViewNodesRecursive(ds11.Tables[0], this.tree11.Nodes, ds11.Tables[0].Rows[0]["ParentObj"].ToString().Trim());
        }
        else
        {
            menu11.Visible = false;
        }


        // 운영관리
        if (ds1.Tables[0].Rows.Count > 0)
        {
            CreateTreeViewNodesRecursive(ds1.Tables[0], this.tree1.Nodes, ds1.Tables[0].Rows[0]["ParentObj"].ToString().Trim());
        }
        else
        {
            menu02.Visible = false;
        }
    }

    private void CreateTreeViewNodesRecursive(DataTable table, TreeViewNodeCollection nodesCollection, string parentID)
    {
        for (int i = 0; i < table.Rows.Count; i++)
        {
            if (table.Rows[i]["ParentObj"].ToString().Trim() == parentID)
            {
                TreeViewNode node = new TreeViewNode(table.Rows[i]["ObjName"].ToString().Trim(), table.Rows[i]["ObjID"].ToString().Trim(), "", table.Rows[i]["FormUrl"].ToString().Trim());

                if (table.Rows[i]["FormUrl"].ToString().Trim() == "")
                {
                    node.Image.Url = "~/Contents/Main/images/menutree_icon/icon_folder_green.png";
                }
                else
                {
                    node.Image.Url = "~/Contents/Main/images/menutree_icon/icon_page_2.png";
                }

                nodesCollection.Add(node);

                CreateTreeViewNodesRecursive(table, node.Nodes, node.Name);
            }
        }
    }


}