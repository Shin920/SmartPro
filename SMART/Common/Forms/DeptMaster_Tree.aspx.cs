using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMART;
using System.Data;
using SMART.OM.DA;
using DevExpress.Web;

public partial class Common_Forms_DeptMaster_Tree : PageBase
{
    GObj G = new GObj();

    string strOutMessage = "";
    string strOutFlag = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();
        G = ((GObj)Session["G"]);
        if (!IsPostBack)
        {
            if (Request["Action"] == "Load")
            {
                lblMenuTitle.Text = G.SiteName;

                txtDeptCode.Text = CFL.GetStr(Request["Code"]).ToUpper().Replace("NULL", "");
                txtDeptName.Text = CFL.GetStr(Request["Name"]).ToUpper().Replace("NULL", "");
                txtParentDeptCode.Text = CFL.GetStr(Request["PrtCode"]).ToUpper().Replace("NULL", "");
                txtParentDeptName.Text = CFL.GetStr(Request["PrtName"]).ToUpper().Replace("NULL", "");

                //Data조회
                DataTreeBind();
            }
        }
    }

    protected void callbackTree_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
        DataTreeBind();
    }

    void DataTreeBind()
    {
        SMART.COMMON.DS.CommonCodeClass mpc = new SMART.COMMON.DS.CommonCodeClass();
        DataSet ds = mpc.TreePopup(G.D, "", G.SiteCode, txtDeptCode.Text, txtDeptName.Text, txtParentDeptCode.Text, txtParentDeptName.Text);
        if (ds.Tables[0].Rows.Count > 0)
        {
            this.tree.Nodes.Clear();
            CreateTreeViewNodesRecursive(ds.Tables[0], this.tree.Nodes, ds.Tables[0].Rows[0]["ParentObj"].ToString().Trim());
        }
    }

    private void CreateTreeViewNodesRecursive(DataTable table, TreeViewNodeCollection nodesCollection, string parentID)
    {
        for (int i = 0; i < table.Rows.Count; i++)
        {
            if (table.Rows[i]["ParentObj"].ToString().Trim() == parentID)
            {
                TreeViewNode node = new TreeViewNode(table.Rows[i]["ObjName"].ToString().Trim(), table.Rows[i]["ObjID"].ToString().Trim());

                if (table.Rows[i]["LeafCheck"].ToString().Trim() == "N")
                {
                    node.Image.Url = "/SmartPro/Contents/Main/images/menutree_icon/icon_folder_blue.png";
                }
                else
                {
                    node.Image.Url = "/SmartPro/Contents/Main/images/menutree_icon/icon_page_2.png";
                }

                nodesCollection.Add(node);

                CreateTreeViewNodesRecursive(table, node.Nodes, node.Name);
            }
        }
    }
}