using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;

namespace SMART
{
	/// <summary>
	/// Summary description for CGW.
	/// </summary>
	[WebService(Name="CGW",Namespace="http://CGW.UsgKorea.com")]
	public class CGW : System.Web.Services.WebService
	{
		public CGW()
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
		protected override void Dispose( bool disposing )
		{
		}

		[WebMethod]
		public ArrayList GridGateWay (bool bStartQuery, string strModule, string strClassName, string strMethodName, string strWhereClause, string strCondition, object[] Conditions, object[] GDObj )
		{
			CFL.ModuleIni Module = CFL.StrToModuleIni ( strModule );
			
            switch ( Module )
			{
                case CFL.ModuleIni.OM:
                    CommGW.gwOM gwOM = new CommGW.gwOM();
                    return gwOM.GridGateWay(bStartQuery, strClassName, strMethodName, strWhereClause, strCondition, Conditions, GDObj);
                
                default:
                    CommGW.gwOM Obj = new CommGW.gwOM();
                    return Obj.GridGateWay(bStartQuery, strClassName, strMethodName, strWhereClause, strCondition, Conditions, GDObj);
			}
		}

		/// <summary>
		/// GateWay for Tree Control
		/// return definition for Tree Control
        /// 1st Column : Level
		/// 2nd Column : Parent Key
		/// 3rd Column : Child Key
		/// 4th Column : ItemName
		/// 5th Column : URL
		/// </summary>
		[WebMethod]
		public ArrayList TreeGateWay (bool bStartQuery, string strModule, string strClassName, string strMethodName, string strWhereClause, string strCondition, object[] Conditions, object[] GDObj )
		{
			CFL.ModuleIni Module = CFL.StrToModuleIni ( strModule );
			
            switch ( Module )
			{
                case CFL.ModuleIni.OM:
                    CommGW.gwOM gwOM = new CommGW.gwOM();
                    return gwOM.TreeGateWay(bStartQuery, strClassName, strMethodName, strWhereClause, strCondition, Conditions, GDObj);
                
                default:
                    CommGW.gwOM Obj = new CommGW.gwOM();
                    return Obj.TreeGateWay(bStartQuery, strClassName, strMethodName, strWhereClause, strCondition, Conditions, GDObj);
			}
		}



	}
}