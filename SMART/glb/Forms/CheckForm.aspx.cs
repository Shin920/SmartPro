using SMART;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class glb_Forms_CheckForm : PageBase
{
    public string m_strScriptTarget;
    public string m_strCstLogout;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();

        m_strScriptTarget = @"
<script>
	this.location = """ + Request["ReturnUrl"] + @""";
</script>";

    }
}