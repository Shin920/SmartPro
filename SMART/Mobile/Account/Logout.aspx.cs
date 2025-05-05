using SMART;
using System;

public partial class Account_Logout : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Cookies.Clear();
    }
}