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
	/// gwWorkFlow에 대한 요약 설명입니다.
	/// </summary>
	[WebService(Name="gwWorkFlow",Namespace="http://gwWorkFlow.UsgKorea.com")]
	public class gwWorkFlow : System.Web.Services.WebService
	{
		public gwWorkFlow()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();
		}

		#region Component Designer generated code
		
		//웹 서비스 디자이너에 필요합니다. 
		private IContainer components = null;
				
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		
		[WebMethod]
		public ArrayList WfGateWay ( object[] GDObj, string strFormID, string strOriginNo, string strOriginSerNo )
		{
			string strWfErrorCode = "00";
			string strWfErrorMsg = "";

			if ( strFormID == "GL001" )
			{
			}
			else if ( strFormID == "PO033" )
			{
			}

			return CFL.EncodeData ( strWfErrorCode, strWfErrorMsg, null );
		}
	}
}
