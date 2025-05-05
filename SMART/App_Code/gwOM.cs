using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using SMART;

namespace SMART.CommGW
{
    /// <summary>
    /// Summary description for gwGL.
    /// </summary>
    [WebService(Name = "gwOM", Namespace = "http://gwOM.UsgKorea.com")]
    public class gwOM : System.Web.Services.WebService
    {
        public gwOM()
        {
            //CODEGEN: This call is required by the ASP.NET Web Services Designer
            InitializeComponent();
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
        }


        /// <summary>
        /// Gateway for Controls
        /// Do not modify the definition of each function
        /// </summary>

        
        public ArrayList GridGateWay(bool bStartQuery, string strClassName, string strMethodName, string strWhereClause, string strCondition, object[] Conditions, object[] GDObj)
        {
                        
            if (strClassName == "WorkFlow")
            {
                SMART.COMMON.DA.WorkFlow Obj = new SMART.COMMON.DA.WorkFlow();
                switch (strMethodName)
                {
                    case "WfSMGrid":
                        return Obj.WfSMGrid(bStartQuery, strWhereClause, strCondition, Conditions, GDObj);
                    case "WfRGrid":
                        return Obj.WfRGrid(bStartQuery, strWhereClause, strCondition, Conditions, GDObj);
                    case "WfRItemGrid":
                        return Obj.WfRItemGrid(bStartQuery, strWhereClause, strCondition, Conditions, GDObj);
                    case "WfRPopup":
                        return Obj.WfRPopup(bStartQuery, strWhereClause, strCondition, Conditions, GDObj);
                    case "WfPSGrid":
                        return Obj.WfPSGrid(bStartQuery, strWhereClause, strCondition, Conditions, GDObj);
                    case "WfPSGrid1":
                        return Obj.WfPSGrid1(bStartQuery, strWhereClause, strCondition, Conditions, GDObj);
                }
            }
           

            return CFL.NoService();
        }


        // ************************************************************************************************************************************************************************
        
        // Tree 부분

        public ArrayList TreeGateWay(bool bStartQuery, string strClassName, string strMethodName, string strWhereClause, string strCondition, object[] Conditions, object[] GDObj)
        {
                        

            return CFL.NoService();
        }

    }
}
