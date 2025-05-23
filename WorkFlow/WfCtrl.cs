using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.Mail;
using System.Collections;
using System.IO;
using SMART;

namespace WorkFlow
{
	/// <summary>
	/// Summary description for WorkFlow.
	/// </summary>
	[DefaultProperty("ID"),
	DefaultEvent("OK"),
	ToolboxData("<{0}:WfCtrl runat=server></{0}:WfCtrl>")]
	public class WfCtrl : System.Web.UI.WebControls.WebControl, IPostBackEventHandler
	{
        public WfCtrl()
            : base()
        {
            this.FormID = string.Empty;
            this.ShowStamp = false;
            this.WfAmount = string.Empty;
            this.OriginNo = string.Empty;
            this.OriginSerNo = string.Empty;

            this.MoreKey = string.Empty;  // 2013.08.26
        }

		#region Constants

		private const int c_WindowWidth = 480;	  // 475
		private const int c_WindowHeight = 280;   // 270
		private const string c_TableHeight = @"""82""";
		private const string c_TableHeightS = @"""66""";
		private const string c_CellWidth = @"""60""";
		private const string c_CellWidthS = @"""52""";
		private const string c_CellHeight = @"""50""";
		private const string c_HeaderHeight = @"""16""";
		private const string c_ImageWidth = @"""50""";
		private const string c_ImageHeight = @"""50""";
		private const string c_LangID = "kor";

		#endregion

		#region Variables

		private bool m_bClear = false;
		private string m_strViewState = string.Empty;

		#endregion

		#region Properties

		[Bindable(false),
		Category("General")]
		public string FormID
		{
            get
            {
                return (string)ViewState["WfCtrlFormID"];
            }
            set
            {
                ViewState["WfCtrlFormID"] = value;
            }
		}

		[Bindable(false),
		Category("General")]
		public bool ShowStamp
		{
            get
            {
                return (bool)ViewState["WfCtrlShowStamp"];
            }
            set
            {
                ViewState["WfCtrlShowStamp"] = value;
            }
		}

		[Bindable(false),
		Browsable(false),
		DefaultValue("")]
		public string WfAmount
		{
            get
            {
                return (string)ViewState["WfCtrlWfAmount"];
            }
            set
            {
                ViewState["WfCtrlWfAmount"] = value;
            }
		}

		[Bindable(false),
		Browsable(false),
		DefaultValue("")]
		public string OriginNo
		{
            get
            {
                return (string)ViewState["WfCtrlOriginNo"];
            }
            set
            {
                ViewState["WfCtrlOriginNo"] = value;
            }
		}

		[Bindable(false),
		Browsable(false),
		DefaultValue("")]
		public string OriginSerNo
		{
            get
            {
                return (string)ViewState["WfCtrlOriginSerNo"];
            }
            set
            {
                ViewState["WfCtrlOriginSerNo"] = value;
            }
		}

		[Bindable(false),
		Browsable(false),
		DefaultValue("")]
		public string DocuStatus
		{
			get
			{	
				string strViewState = (m_strViewState == string.Empty) ? Parent.Page.Request["WorkFlow_" + UniqueID + "_VIEWSTATE"] : m_strViewState;
                return SMART.CFL.GetEncodedProperty("DocuStatus", strViewState);
			}
			set
			{
			}
		}

		[Bindable(false),
		Browsable(false),
		DefaultValue("")]
		public string LoadedDocuStatus
		{
			get		
			{	
				string strViewState = (m_strViewState == string.Empty) ? Parent.Page.Request["WorkFlow_" + UniqueID + "_VIEWSTATE"] : m_strViewState;
                return SMART.CFL.GetEncodedProperty("LoadedStatus", strViewState);
			}
			set
			{
			}
		}


        // 2013.08.26
        [Bindable(false),
        Browsable(false),
        DefaultValue("")]
        public string MoreKey
        {
            get
            {
                return (string)ViewState["WfCtrlMoreKey"];
            }
            set
            {
                ViewState["WfCtrlMoreKey"] = value;
            }
        }


		#endregion

		#region Initialization

		protected override void OnInit ( EventArgs e )
		{
            if (Parent.Page.Session["G"] != null && ((SMART.GObj)Parent.Page.Session["G"]).UserID != null && ((SMART.GObj)Parent.Page.Session["G"]).UserID != string.Empty)
			{
                m_GDObj = ((SMART.GObj)Parent.Page.Session["G"]).D;
                m_strLangID = ((SMART.GObj)Parent.Page.Session["G"]).LangID;
                m_strUserID = ((SMART.GObj)Parent.Page.Session["G"]).UserID;
                m_strSiteCode = ((SMART.GObj)Parent.Page.Session["G"]).SiteCode;
                m_strCliID = ((SMART.GObj)Parent.Page.Session["G"]).ClientID;
                Visible = ((SMART.GObj)Parent.Page.Session["G"]).WfUse;                
			}
			else
				m_GDObj = null;

			if ( Parent.Page.IsPostBack )
			{
				if ( null != Parent.Page.Request["WorkFlow_" + UniqueID + "_VIEWSTATE"] )
					m_strViewState = Parent.Page.Request["WorkFlow_" + UniqueID + "_VIEWSTATE"].ToString();
			}
		}

		private object[] m_GDObj;
		private string m_strLangID;
		private string m_strUserID;
		private string m_strSiteCode;
		private string m_strCliID;

		#endregion

		#region Event Handling

		public delegate void WfEventHandler (object source, EventArgs e);
		protected static readonly object EventOK = new object();

		/// <summary>
		/// OKEvent Definition
		/// </summary>
		[Category("Action")]
		public event WfEventHandler OK
		{
			add
			{
				Events.AddHandler(EventOK, value);
			}
			remove 
			{
				Events.RemoveHandler(EventOK, value);
			}
		}

		/// <summary>
		///	Event Handler
		/// </summary>
		/// <param name="e"></param>
		public virtual void OnOK(EventArgs e) 
		{
			// Toss Event to Appropriate Event Handler
			WfEventHandler handler = (WfEventHandler)Events[EventOK];
			if (handler != null) handler(this, e);
		}

		// Event Distribution
		public virtual void RaisePostBackEvent(string eventArgument) 
		{
			// Only OK Event
			OnOK (null);
		}

		#endregion

		# region Render

		/// <summary>
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void RenderContents(HtmlTextWriter output)
		{
			Resource Res=new Resource();
			Res.m_strLangID = (m_GDObj == null) ? c_LangID : m_strLangID;
	
			StringBuilder sbRender = new StringBuilder();

			// Design Mode
			if (Site != null && Site.DesignMode)
			{
				output.Write(@"
<table border=""1"" cellspacing=""0"" cellpadding=""0"" height=" + c_TableHeightS + @" width=""162"">
  <tr>
		<td width=" + c_CellWidthS + @" height=" + c_HeaderHeight + @">
			&nbsp;
		</td>
		<td width=" + c_CellWidthS + @" height=" + c_HeaderHeight + @">
			&nbsp;
		</td>
		<td width=" + c_CellWidthS + @" height=" + c_HeaderHeight + @">
			&nbsp;
		</td>
  </tr>
  <tr>
		<td width=" + c_CellWidthS + @" height=" + c_CellHeight + @">
			&nbsp;
		</td>
		<td width=" + c_CellWidthS + @" height=" + c_CellHeight + @">
			&nbsp;
		</td>
		<td width=" + c_CellWidthS + @" height=" + c_CellHeight + @">
			&nbsp;
		</td>
  </tr>
</table>");
				return;
			}

			// Load ViewState Data
			string strViewState = m_strViewState;

			// Origin
			/*
			if (m_bClear)
				strViewState = string.Empty;
			*/

			if (m_bClear)
			{
				strViewState = string.Empty;

				// 추가 (2021.08)
			//	OriginNo = null;
			}

			m_bClear = false;
			m_strViewState = string.Empty;

            string strDocuStatus = SMART.CFL.GetEncodedProperty("DocuStatus", strViewState);
            string strRouteCode = SMART.CFL.GetEncodedProperty("RouteCode", strViewState);
            string strOriginNo = SMART.CFL.GetEncodedProperty("OriginNo", strViewState);
            string strOriginSerNo = SMART.CFL.GetEncodedProperty("OriginSerNo", strViewState);

            string strMoreKey = SMART.CFL.GetEncodedProperty("MoreKey", strViewState);  // 2013.08.26

            /*
			sbRender.Append(@"
<div onmouseover = ""this.style.cursor='hand'"">");
			*/

            // (2021.07.09)
            sbRender.Append(@"
<div onmouseover = ""style='cursor:pointer'"">");

            // Make Image for Wysiwyg
            string strErrorMsg = string.Empty;
			string strRouteName = string.Empty;
			int iStepCnt = 0;
			string[] sarRouteUserID = new string[0];

			sbRender.Append ( MakeStamp (m_GDObj, ref strErrorMsg, true, strViewState, UniqueID
				, m_strSiteCode, m_strLangID, FormID, m_strUserID, ref strRouteCode, ref strRouteName
				, ref iStepCnt, ref sarRouteUserID, OriginNo, OriginSerNo
                , MoreKey) );  // 2013.08.26

			sbRender.Append(@"
</div>");

			// Hidden View State
            // 2015.07.27 : 화면에 추가했을 때.. 컨트롤 밑에 빈공간 생기는 현상 수정
            /*
			sbRender.Append(@"
<textarea style=""width:1px;height:1px"" name=""WorkFlow_" + UniqueID + @"_VIEWSTATE"">" + strViewState + @"</textarea>");
            */
            sbRender.Append(@"
<div style=""display:none"">
    <textarea style=""width:1px;height:1px"" name=""WorkFlow_" + UniqueID + @"_VIEWSTATE"">" + strViewState + @"</textarea>
</div> ");


			if (Enabled)
			{
				// JavaScript function that load Document Insert window
				sbRender.Append(@"
<script language=""javascript"">
varWindow = null;

function " + UniqueID + @"_ChildOpen()
{
	varWindow = window.open(""" + System.Configuration.ConfigurationManager.AppSettings["CommonFormsUrl"] 
					+ @"WorkFlow/WfDocument.aspx?ViewStateName="
                    + HttpUtility.UrlEncode("WorkFlow_" + UniqueID + "_VIEWSTATE", System.Text.Encoding.GetEncoding("euc-kr"))
					+ "&OkFunctionName="
                    + HttpUtility.UrlEncode("WorkFlow" + UniqueID + "_OK", System.Text.Encoding.GetEncoding("euc-kr"))
                    + "&FormID=" + HttpUtility.UrlEncode(FormID, System.Text.Encoding.GetEncoding("euc-kr"))
					+ @""", """", ""width=" + c_WindowWidth 
					+ ", height=" + c_WindowHeight
					+ @", left="" + (window.event.screenX - " + c_WindowWidth 
					+ @") + "", top="" + (window.event.screenY) + "", menubar=no, toolbar=no, location=no, directories=no, status=no, scrollbars=no, resizable=no"");
	self.document.body.onfocus=" + UniqueID + @"_Focus;
	self.document.body.onactivate=" + UniqueID + @"_Focus;
}

function " + UniqueID + @"_Focus()
{
	varWindow.focus();
}

function WorkFlow" + UniqueID + @"_OK()
{
	" + Page.GetPostBackEventReference(this) + @";
}
</script>");
			}
			else
				sbRender.Append(@"
<script language=""javascript"">
    function " + UniqueID + @"_ChildOpen() {}
</script>");

			output.Write(sbRender);
		}

		# endregion

		#region Public Methods
		
		/// <summary>
		/// GerPrintInfo()
		/// </summary>
		public WfPrintInfo GetPrintInfo() 
		{
			WfPrintInfo wfPI = new WfPrintInfo(Visible);
            wfPI.StampHeight = SMART.CFL.Toi(c_ImageHeight.Replace(@"""", ""));
            wfPI.StampWidth = SMART.CFL.Toi(c_ImageHeight.Replace(@"""", ""));
            wfPI.StepHeight = SMART.CFL.Toi(c_HeaderHeight.Replace(@"""", ""));

			ArrayList stepsList = new ArrayList();
			SVC.WorkFlow Obj = new SVC.WorkFlow();
			Object[] alData2 = null, alData = null;
 
			string strViewState = m_strViewState;

            string strRouteCode = SMART.CFL.GetEncodedProperty("RouteCode", strViewState);
            string[] sarUserID = SMART.CFL.GetEncodedProperties("UserID", strViewState);
            string[] sarEmpName = SMART.CFL.GetEncodedProperties("EmpName", strViewState);
            string[] sarWfStatus = SMART.CFL.GetEncodedProperties("WfStatus", strViewState);
            string[] sarWfDateTime = SMART.CFL.GetEncodedProperties("WfDateTime", strViewState);

			// Default RouteCode
			if (strRouteCode == string.Empty)
			{
				object[] alData3 = Obj.GetDefaultRoute(m_GDObj, m_strSiteCode, FormID, m_strUserID);
				if (alData3[0].ToString() != "00")
				{
					string strErrorMsg = alData3[1].ToString();
                    Obj.Dispose();
					return wfPI;
				}
				if ((int)alData3[3] > 0)
					strRouteCode = alData3[4].ToString();
			}

			string strStepTitle = string.Empty;
			string strRouteName = string.Empty;
			int iStepCnt = 3;
 
			if (strRouteCode != string.Empty)
			{
				alData = Obj.WfRItemGrid (true, "A.RouteCode = " + CFL.Q(strRouteCode) + " and A.SiteCode = " + CFL.Q (m_strSiteCode), string.Empty, null, m_GDObj);
				if (alData[0].ToString() != "00" || (int)alData[3] < 1)
				{
					string strErrorMsg = alData[1].ToString();
					wfPI.Steps = stepsList;
                    Obj.Dispose();
					return wfPI;
				}
				iStepCnt = (int)alData[3] + 1;
			}

			// Routing Information Load
			if (strRouteCode != string.Empty)
			{
				alData2 = Obj.WfRLoad (  m_GDObj, strRouteCode );
				if (alData2[0].ToString() != "00" || (int)alData2[3] < 1)
				{
					string strErrorMsg = alData2[1].ToString();
                    Obj.Dispose();
					return wfPI;
				}
 
				// Get Route Information
				strRouteName = alData2[4].ToString();
				iStepCnt = CFL.Toi(alData2[8]);
				strStepTitle = alData2[5].ToString();
			}
			else
			{
				GetDefaultRoute(m_strLangID, 1, ref strStepTitle);
			}

			// Extract Route Data
			string[] sarStepTitle = new string[iStepCnt];
			string[] sarRouteUserID = new string[iStepCnt];
			string[] sarRouteEmpName = new string[iStepCnt];
   
			/*
			 * 기안자 정보 입력
			 * */
			string imgurl1 = string.Empty;
			if (sarUserID.Length > 0)
			{
				if (sarWfStatus[0] == "A")
                    imgurl1 = "/" + CFL.formRoot + "/UserImages/UserStamps/" + m_strCliID + "/" + ((SMART.GObj)Parent.Page.Session["G"]).SiteInitial + "_" + sarUserID[0] + ".bmp";
				else if (sarWfStatus[0] == "B")
                    imgurl1 = "/" + CFL.formRoot + "/Common/Image/Stamps/BackStamp.gif";
				else if (sarWfStatus[0] == "C")
                    imgurl1 = "/" + CFL.formRoot + "/Common/Image/Stamps/HoldStamp.gif";
				else if (sarWfStatus[0] == "D")
                    imgurl1 = "/" + CFL.formRoot + "/Common/Image/Stamps/SlashStamp.gif";
				else
					imgurl1 = string.Empty;
			}

			WfPrintInfoStep wfPIS1 = new WfPrintInfoStep(strStepTitle, imgurl1);
			stepsList.Add(wfPIS1);
 
			/* 그 다음 결제 정보 */
			for (int i = 1; i < iStepCnt; i++)
			{
				if (strRouteCode != string.Empty)
					strStepTitle = alData[4+(int)alData[2]*(i-1)].ToString();
				else
				{
					GetDefaultRoute(m_strLangID, i+1, ref strStepTitle);
				}
 
				string imgurl ="&nbsp;";
 
				if ( i < sarUserID.Length)
				{
					string strUserID = sarUserID[i];
					string strWfStatus = sarWfStatus[i];
     
					if (strWfStatus == "A")
                        imgurl = "/" + CFL.formRoot + "/UserImages/UserStamps/" + m_strCliID + "/" + m_strSiteCode.Substring(0, 1) + "_" + strUserID + ".bmp";
					else if (strWfStatus == "B")
                        imgurl = "/" + CFL.formRoot + "/Common/Image/Stamps/BackStamp.gif";
					else if (strWfStatus == "C")
                        imgurl = "/" + CFL.formRoot + "/Common/Image/Stamps/HoldStamp.gif";
					else if (strWfStatus == "D")
                        imgurl = "/" + CFL.formRoot + "/Common/Image/Stamps/SlashStamp.gif";
					else
						imgurl = string.Empty;
				}
				WfPrintInfoStep wfPIS = new WfPrintInfoStep(strStepTitle,imgurl);
				stepsList.Add(wfPIS);
			}
   
			wfPI.Steps = stepsList;
            Obj.Dispose();
			return wfPI;
		}
 
		public bool OnTheFlow
		{
			get
			{
				SVC.WorkFlow Obj = new SVC.WorkFlow();
				
                object[] alData = Obj.WfDLoad1(m_GDObj, m_strSiteCode, FormID, OriginNo, OriginSerNo, MoreKey);  // 2013.08.26
                
                Obj.Dispose();
				
                if (alData[0].ToString() == "00" && (int)alData[3] > 0)
					return true;
				else
					return false;
			}
		}

		/// <summary>
		/// query Document data from Database
		/// </summary>
		public void Query ()
		{
			string strViewState = string.Empty;

			// WfDocument Load
            SMART.CFL.SetEncodedProperty("OriginNo", OriginNo, ref strViewState);
            SMART.CFL.SetEncodedProperty("OriginSer", OriginSerNo, ref strViewState);

            SMART.CFL.SetEncodedProperty("MoreKey", MoreKey, ref strViewState); // 2013.08.26

			
			SVC.WorkFlow Obj = new SVC.WorkFlow();

			object[] alData = Obj.WfDLoad1(m_GDObj, m_strSiteCode, FormID, OriginNo, OriginSerNo, MoreKey);  // 2013.08.26
			
            if (alData[0].ToString() == "00" && (int)alData[3] > 0)
			{
                SMART.CFL.SetEncodedProperty("RouteCode", alData[4].ToString(), ref strViewState);
                SMART.CFL.SetEncodedProperty("DocuDescr", alData[5].ToString(), ref strViewState);
                SMART.CFL.SetEncodedProperty("CurStep", alData[6].ToString(), ref strViewState);
                SMART.CFL.SetEncodedProperty("DocuStatus", alData[7].ToString(), ref strViewState);
                SMART.CFL.SetEncodedProperty("LoadedStatus", alData[7].ToString(), ref strViewState);
			}

            alData = Obj.WfDLoad2(m_GDObj, m_strSiteCode, FormID, OriginNo, OriginSerNo, MoreKey);  // 2013.08.26
			
            if (alData[0].ToString() == "00")
			{
				for (int i = 0; i < (int)alData[3]; i++)
				{
                    SMART.CFL.SetEncodedProperty("UserID" + i, alData[4 + i * (int)alData[2]].ToString(), ref strViewState);
                    SMART.CFL.SetEncodedProperty("EmpName" + i, alData[4 + i * (int)alData[2] + 1].ToString(), ref strViewState);
                    SMART.CFL.SetEncodedProperty("WfStatus" + i, alData[4 + i * (int)alData[2] + 2].ToString(), ref strViewState);
                    SMART.CFL.SetEncodedProperty("WfDateTime" + i, alData[4 + i * (int)alData[2] + 3].ToString(), ref strViewState);
                    SMART.CFL.SetEncodedProperty("RouteStep" + i, alData[4 + i * (int)alData[2] + 4].ToString(), ref strViewState);
				}
			}

            alData = Obj.WfDLoad3(m_GDObj, m_strSiteCode, FormID, OriginNo, OriginSerNo, MoreKey);  // 2013.08.26
			
            if (alData[0].ToString() == "00")
			{
				for (int i = 0; i < (int)alData[3]; i++)
				{
                    SMART.CFL.SetEncodedProperty("UserIDRef" + i, alData[4 + i * (int)alData[2]].ToString(), ref strViewState);
                    SMART.CFL.SetEncodedProperty("EmpNameRef" + i, alData[4 + i * (int)alData[2] + 1].ToString(), ref strViewState, false);
                    SMART.CFL.SetEncodedProperty("ReadCheck" + i, alData[4 + i * (int)alData[2] + 2].ToString(), ref strViewState);
                    SMART.CFL.SetEncodedProperty("ReadDateTime" + i, alData[4 + i * (int)alData[2] + 3].ToString(), ref strViewState, false);
				}
			}

            Obj.Dispose();
			
            m_strViewState = strViewState;
			
            m_bClear = false;
		}


		/// <summary>
		/// Clear Document Data
		/// </summary>
		public void Clear ()
		{
			m_bClear = true;

            // 추가 (2021.08)
            /*
			StringWriter stringwriter = new StringWriter();
			
			HtmlTextWriter stringBuilder = new HtmlTextWriter(stringwriter);
			
			RenderContents(stringBuilder);
			*/
        }


        /// <summary>
        /// Save Document Files
        /// </summary>
        public object[] Save()
		{
			Resource Res = new Resource();
			Res.m_strLangID = m_strLangID;

			string strViewState = Parent.Page.Request["WorkFlow_" + UniqueID + "_VIEWSTATE"];
			SVC.WorkFlow Obj = new SVC.WorkFlow();

            if (SMART.CFL.GetEncodedProperty("SendMail", strViewState) == "Y" && SMART.CFL.GetEncodedProperty("DocuStatus", strViewState) != "Z")
			{
                object[] alData = Obj.WfRGetMail(m_GDObj, SMART.CFL.GetEncodedProperty("RouteCode", strViewState), SMART.CFL.GetEncodedProperty("CurStep", strViewState));
                if (alData[0].ToString() != "00")
                {
                    Obj.Dispose();
                    return alData;
                }
                else if ((int)alData[3] == 0)
                {
                    alData = new object[4];
                    alData[0] = "02";
                    alData[1] = Res.GetResString("P16");
                    alData[2] = 0;
                    alData[3] = 0;
                    Obj.Dispose();
                    return alData;
                }
				
				string strMailAddress = alData[4].ToString();
				MailMessage thismsg = new MailMessage();
				thismsg.To = strMailAddress;
				thismsg.From = "Eagle-Workflow";
				thismsg.Subject = Res.GetResString("P04");
                thismsg.Body = "<br>" + Res.GetResString("P04") + "<br><a href=\"http://" + Obj.GetDomainName(m_strCliID) + "/" + CFL.formRoot + "/Login.aspx\">EAGLE</a> " + Res.GetResString("P05");
				thismsg.BodyFormat = MailFormat.Html;

				try
				{
					SmtpMail.Send(thismsg);
				}
				catch
				{
					alData = new object[4];
					alData[0] = "01";
					alData[1] = Res.GetResString("P06");
					alData[2] = 0;
					alData[3] = 0;
                    Obj.Dispose();
					return alData;
				}

                SMART.CFL.SetEncodedProperty("SendMail", "", ref strViewState, true);
				m_strViewState = strViewState;
			}

            object[] o = Obj.WfDSave2(m_GDObj, m_strLangID, m_strSiteCode, FormID, OriginNo, OriginSerNo
                , SMART.CFL.GetEncodedProperty("RouteCode", strViewState)
                , SMART.CFL.GetEncodedProperty("DocuDescr", strViewState)
                , SMART.CFL.GetEncodedProperty("CurStep", strViewState)
                , SMART.CFL.GetEncodedProperty("DocuStatus", strViewState)
                , SMART.CFL.GetEncodedProperties("UserID", strViewState)
                , SMART.CFL.GetEncodedProperties("WfStatus", strViewState)
                , SMART.CFL.GetEncodedProperties("WfDateTime", strViewState)
                , SMART.CFL.GetEncodedProperties("UserIDRef", strViewState)
                , SMART.CFL.GetEncodedProperties("ReadCheck", strViewState)
                , SMART.CFL.GetEncodedProperties("ReadDateTime", strViewState)
                , SMART.CFL.GetEncodedProperties("RouteStep", strViewState), WfAmount
                , MoreKey);  // 2013.08.26

            Obj.Dispose();
            
            return o;
		}



		/// <summary>
		/// Delete Document Files
		/// </summary>
		public object[] Delete()
		{
			string strViewState = Parent.Page.Request["WorkFlow_" + UniqueID + "_VIEWSTATE"];

			SVC.WorkFlow Obj = new SVC.WorkFlow();

			object[] o = Obj.WfDDelete(m_GDObj, m_strSiteCode, FormID, OriginNo, OriginSerNo, MoreKey);
            
            Obj.Dispose();
            
            return o;
		}

		/// <summary>
		/// Change Styles of Controls that Affected by the Visibility of this Control
		/// </summary>
		public string StyleChange(string strStyle, string strChangePart)
		{
			if (!Visible)
			{
				string strTemp = strStyle.ToLower();
				int iStart = strTemp.IndexOf(strChangePart.ToLower());
				if (iStart > -1)
				{
					// Extract Left Coordinate
					// 4 means count of 'left'
					int iEnd = strTemp.IndexOf(";", iStart);
					for (int i = 1; i < iEnd - iStart - 4; i++)
					{
						if (strTemp.Substring(iStart + 4 + i, 1) != " " && strTemp.Substring(iStart + 4 + i, 1) != ":")
						{
							iStart += i + 4;
							break;
						}
					}
					for (int i = 1; i < iEnd - iStart; i++)
					{
						if (strTemp.Substring(iStart + i, 1).ToLower() == "p")
						{
							iEnd = iStart + i;
							break;
						}
					}
					strTemp = strTemp.Substring(iStart, iEnd - iStart);
					int iTemp = CFL.Toi(strTemp);
					iTemp = iTemp + CFL.Toi(Width.Value);
					strStyle = strStyle.Substring(0, iStart) + iTemp + strStyle.Substring(iEnd, strStyle.Length - iEnd);
				}
			}
			return strStyle;
		}

		#endregion

		#region Static Functions

		public static string MakeStamp(object[] GDObjTemp, ref string strErrorMsg, bool bCtrl, string strViewState
			, string strID, string strSiteCode, string strLangID, string strFormID, string p_strUserID
			, ref string strRouteCode, ref string strRouteName, ref int iStepCnt, ref string[] sarRouteUserID)
		{
			return MakeStamp(GDObjTemp, ref strErrorMsg,bCtrl, strViewState, strID, strSiteCode, strLangID, strFormID
				, p_strUserID, ref strRouteCode, ref strRouteName, ref iStepCnt, ref sarRouteUserID, null, null
                , null);
		}

		public static string MakeStamp(object[] GDObjTemp, ref string strErrorMsg, bool bCtrl, string strViewState
			, string strID, string strSiteCode, string strLangID, string strFormID, string p_strUserID
			, ref string strRouteCode, ref string strRouteName, ref int iStepGrade, ref string[] sarRouteUserID 
			, string strOriginNo, string strOriginSerNo
            , string strMoreKey)  // 2013.08.26
		{
			
            SVC.WorkFlow Obj = new SVC.WorkFlow();
            
            SMART.GData GD = new SMART.GData(GDObjTemp);
			
            object[] alData;
			
            int iStepCnt;
			
            // Saved RouteCode
			if (strRouteCode == string.Empty)
				strRouteCode = CFL.GetEncodedProperty("RouteCode", strViewState);

			// when First Insert then DefaultRoute
			if (strRouteCode == string.Empty)
			{
				alData = Obj.GetDefaultRoute(GDObjTemp, strSiteCode, strFormID, p_strUserID);
				if (alData[0].ToString() != "00")
				{
					strErrorMsg = alData[1].ToString();
                    Obj.Dispose();
					return string.Empty;
				}
				if ((int)alData[3] > 0)
					strRouteCode = alData[4].ToString();
			}

            string[] sarUserID = SMART.CFL.GetEncodedProperties("UserID", strViewState);
            string[] sarEmpName = SMART.CFL.GetEncodedProperties("EmpName", strViewState);
            string[] sarWfStatus = SMART.CFL.GetEncodedProperties("WfStatus", strViewState);
            string[] sarWfDateTime = SMART.CFL.GetEncodedProperties("WfDateTime", strViewState);
			
			// Get the FirstStep Signee
			string strUserID = string.Empty;
			string strEmpName = string.Empty;
			string strRouteUserID = string.Empty;
			string strRouteEmpName = string.Empty;
			string strWfStatus = string.Empty;
			string strWfDateTime = string.Empty;
			string strStepTitle = string.Empty;

			if ( sarUserID.Length > 0 )
			{
				strUserID = sarUserID[0];
				strEmpName = sarEmpName[0];
				strWfStatus = sarWfStatus[0];
				strWfDateTime = sarWfDateTime[0];
			}

			// Routing Information Load
			if (strRouteCode != string.Empty)
			{
				alData = Obj.WfRLoad(GDObjTemp, strRouteCode);
				if (alData[0].ToString() != "00" || (int)alData[3] < 1)
				{
					strErrorMsg = alData[1].ToString();
                    Obj.Dispose();
					return string.Empty;
				}

				// Get Route Information
				strRouteName = alData[4].ToString();
				iStepCnt = CFL.Toi(alData[8]);
				strStepTitle = alData[5].ToString();
				iStepGrade = CFL.Toi(alData[10]);
			}
			else
			{
				iStepCnt = 3;
				iStepGrade = 3;
				GetDefaultRoute(strLangID, 1, ref strStepTitle);
				strUserID = string.Empty;
				strEmpName = string.Empty;
				strWfStatus = string.Empty;
				strWfDateTime = string.Empty;
				strRouteUserID = string.Empty;
				strRouteEmpName= string.Empty;
			}

			string strLine1 = string.Empty;
			string strLine2 = string.Empty;
			string strLine3 = string.Empty;
			MakeStampCell(bCtrl, strStepTitle, strUserID, strEmpName, strRouteUserID, strRouteEmpName
				, strWfStatus, strWfDateTime, ref strLine1, ref strLine2, ref strLine3, strLangID
				, GD.ClientID, GD.SiteInitial, GD.CultureID);

			sarRouteUserID = new string[iStepCnt-1];
			string[] sarStepTitle = new string[iStepCnt-1];
			string[] sarEmpName2 = new string[iStepCnt-1];
			string[] sarRouteEmp = new string[iStepCnt-1];
			string[] sarRouteEmpName = new string[iStepCnt-1];
			string[] sarWfStatus2 = new string[iStepCnt-1];
			string[] sarWfDateTime2 = new string[iStepCnt-1];

			// Make Step after 2
			if (strRouteCode != string.Empty)
			{
				// inux SerOriginNo있는경우 이전method, 없는 경우 net Method
				bool isEmptyWorkflow = true;
				
                if (strOriginNo == null || strOriginNo == string.Empty)
				{
					alData = Obj.WfRItemGrid(true, "A.RouteCode = " + CFL.Q(strRouteCode) + " and A.SiteCode = " + CFL.Q(strSiteCode), string.Empty, null, GDObjTemp);
					isEmptyWorkflow = true;
				}
				else
				{
					alData = Obj.WfRItemGrid2(true, strFormID, strOriginNo, strOriginSerNo, strRouteCode, GDObjTemp, strMoreKey);  // 2013.08.26
					isEmptyWorkflow = false;
				}

				if (alData[0].ToString() != "00" || (int)alData[3] < 1)
				{
					strErrorMsg = alData[1].ToString();
                    Obj.Dispose();
					return string.Empty;
				}

				// Extract Route Data
				for (int i = 0; i < (int)alData[3]; i++)
				{
					sarStepTitle[i] = alData[4+(int)alData[2]*i].ToString();
					sarRouteUserID[i] = alData[4+(int)alData[2]*i+1].ToString();
					sarEmpName2[i] = alData[4+(int)alData[2]*i+2].ToString();
					if(!isEmptyWorkflow)
					{
						sarWfStatus2[i] = alData[4+(int)alData[2]*i+3].ToString();
						sarWfDateTime2[i] = alData[4+(int)alData[2]*i+4].ToString();
						sarRouteEmp[i] = alData[4+(int)alData[2]*i+5].ToString();
						sarRouteEmpName[i] = alData[4+(int)alData[2]*i+6].ToString();
					}
					else
					{
						sarWfStatus2[i] = string.Empty;
						sarWfDateTime2[i] = string.Empty;
						sarRouteEmp[i] = alData[4+(int)alData[2]*i+3].ToString();
						sarRouteEmpName[i] = string.Empty;
					}
				}
			}
			else
			{
				strStepTitle = string.Empty;
				GetDefaultRoute(strLangID, 2, ref strStepTitle);
				sarStepTitle[0] = strStepTitle;
				sarRouteUserID[0] = string.Empty;
				sarEmpName2[0] = string.Empty;
				sarWfStatus2[0] = string.Empty;			
				sarWfDateTime2[0] = string.Empty;
				sarRouteEmp[0] = string.Empty;
				sarRouteEmpName[0] = string.Empty;

				if (iStepCnt > 2)
				{
					strStepTitle = string.Empty;
					GetDefaultRoute(strLangID, 3, ref strStepTitle);
					sarStepTitle[1] = strStepTitle;
					sarRouteUserID[1] = string.Empty;
					sarEmpName2[1] = string.Empty;
					sarWfStatus2[1] = string.Empty;
					sarWfDateTime2[1] = string.Empty;
					sarRouteEmp[1] = string.Empty;
					sarRouteEmpName[1] = string.Empty;
				}
			}

			if (bCtrl)
			{
				// Show the Last two Step Cells in Control Mode
				int iStartIdx = sarStepTitle.Length > 1 ? sarStepTitle.Length-2 : 0;
				for (int i = iStartIdx; i < sarStepTitle.Length; i++)
					MakeStampCell(bCtrl, sarStepTitle[i], sarRouteUserID[i], sarEmpName2[i], sarRouteEmp[i]
						, sarRouteEmpName[i], sarWfStatus2[i], sarWfDateTime2[i], ref strLine1, ref strLine2
						, ref strLine3, strLangID, GD.ClientID, GD.SiteInitial, GD.CultureID);
			}
			else
				MakeStampCell(bCtrl, sarStepTitle,  sarRouteUserID, sarEmpName2, sarWfStatus2, sarWfDateTime2
					, sarRouteEmp, sarRouteEmpName, ref strLine1, ref strLine2, ref strLine3, strLangID
					, GD.ClientID, GD.SiteInitial, GD.CultureID );
            Obj.Dispose();
			return @"
<table border=""1"" cellspacing=""0"" cellpadding=""0"" bordercolor=""#6a7281"" height="
				+ ( bCtrl ? c_TableHeightS : c_TableHeight ) 
				+ ( bCtrl ? @" onclick=""" + strID + @"_ChildOpen()""" : string.Empty ) + @" >
	<tr>
		" + strLine1 + @"
	</tr>
	<tr>
		" + strLine2 + @"
	</tr>" + (!bCtrl ? @"
	<tr>
		" + strLine3 + @"
	</tr>" : "" ) + @"
</table>
";
		}

		private static void GetDefaultRoute(string strLangID, int iStep
			, ref string strStepTitle)
		{
			Resource res = new Resource();
			res.m_strLangID = strLangID;

			switch (iStep)
			{
				case 1:
					strStepTitle = res.GetResString("P07");
					break;
				case 2:
					strStepTitle = res.GetResString("P08");
					break;
				case 3:
					strStepTitle = res.GetResString("P09");
					break;
			}
		}

		private static void MakeStampCell(bool bCtrl, string strStepTitle, string strUserID, string strEmpName
			, string strRouteUserID, string strRouteEmpName, string strWfStatus, string strWfDateTime
			, ref string strLine1, ref string strLine2, ref string strLine3, string strLangID
			, string strClientID, string strSiteInitial, string strCultureID)
		{
			Resource res = new Resource();
			res.m_strLangID = strLangID;
			
			// Charater Manipulation for Table Image
			if (strStepTitle == string.Empty)
				strStepTitle = "&nbsp;";
			if (strEmpName == string.Empty)
				strEmpName = "&nbsp;";
			if (strRouteEmpName == string.Empty)
				strRouteEmpName = "&nbsp;";

			string strLine1Temp, strLine2Temp, strLine3Temp;

			// Make Header            
			strLine1Temp = @"
		<td class=""TD02"" style=""background-color: #2c58a9; text-align: center;"" width=" + (bCtrl ? c_CellWidthS : c_CellWidth) + @" valign=""middle"" align=""center"" height=" + c_HeaderHeight + @">
			<font size=""2"" color=""#ffffff""><b>" + strStepTitle + @"</b></font>
		</td>";
                        

			// Make Stamp Cell
			strLine2Temp = @"
		<td width=" + (bCtrl ? c_CellWidthS : c_CellWidth) + @" style=""text-align: center;"" valign=""middle"" align=""center"" height=" + c_CellHeight + @">
";
			
			if (strWfStatus == "A")
				strLine2Temp += @"
			<img height=" + c_ImageHeight + @" width=" + c_ImageWidth + @" src=""/" + CFL.formRoot + @"/UserImages/UserStamps/" + strClientID + "/" + strSiteInitial + "_" + strUserID + ".bmp\" title=\"[" + res.GetResString("P11") + "] " + res.GetResString("P09") + "\n[" + res.GetResString("P15") + "] " + strEmpName + " ( " + strUserID + " )\n[" + res.GetResString("P10") + "] " + SMART.CFL.DateConvert(strWfDateTime, SMART.CFL.DCType.DTSAddSlash) + "\" />";
			else if (strWfStatus == "B")
				strLine2Temp += @"
			<img height=" + c_ImageHeight + @" width=" + c_ImageWidth + @" src=""/" + CFL.formRoot + @"/Common/Image/Stamps/BackStamp.gif"" title=""[" + res.GetResString("P11") + "] " + res.GetResString("P12") + @"
[" + res.GetResString("P15") + "] " + strEmpName + "(" + strUserID + @")
[" + res.GetResString("P10") + "] " + SMART.CFL.DateConvert(strWfDateTime, CFL.DCType.DTSAddSlash) + @""" />";
			else if (strWfStatus == "C")
				strLine2Temp += @"
			<img height=" + c_ImageHeight + @" width=" + c_ImageWidth + @" src=""/" + CFL.formRoot + @"/Common/Image/Stamps/HoldStamp.gif"" title=""[" + res.GetResString("P11") + "] " + res.GetResString("P13") + @"
[" + res.GetResString("P15") + "] " + strEmpName + "(" + strUserID + @")
[" + res.GetResString("P10") + "] " + SMART.CFL.DateConvert(strWfDateTime, CFL.DCType.DTSAddSlash) + @""" />";
			else if (strWfStatus == "D")
				strLine2Temp += @"
			<img height=" + c_ImageHeight + @" width=" + c_ImageWidth + @""" src=""/" + CFL.formRoot + @"/Common/Image/Stamps/SlashStamp.gif"" title=""[" + res.GetResString("P11") + "] " + res.GetResString("P14") + @"
[" + res.GetResString ("P15") + "] " + strEmpName + "(" + strUserID + @")
[" + res.GetResString("P10") + "] " + SMART.CFL.DateConvert(strWfDateTime, CFL.DCType.DTSAddSlash) + @""" />";
			else
				strLine2Temp += "			&nbsp;";
			
			strLine2Temp += @"
		</td>";

			// Make Tail            
			strLine3Temp = @"
		<td class=""TD02"" bgcolor=""#F1F1F1"" width=" + c_CellWidth + @" style=""text-align: center;"" valign=""middle"" align=""center"" height=" + c_HeaderHeight + @">
      		<font size=""2"" color=""#000080"">";
                        

			if (strWfStatus == "A" && strWfDateTime.Length > 8)
                strLine3Temp += SMART.CFL.DateConvert(strWfDateTime.Substring(0, 8), SMART.CFL.DCType.DAddSlash).Substring(2, 8);
			else if (strWfStatus == "B")
				strLine3Temp += strEmpName;
			else if (strWfStatus == "C")
				strLine3Temp += strEmpName;
			else if (strWfStatus =="D")
				strLine3Temp += "/";
			else
				strLine3Temp += strRouteEmpName;


			strLine3Temp += @"</font>
		</td>
";


			switch (strCultureID.ToLower())
			{
				case "jp":
					strLine1 = strLine1Temp + strLine1;
					strLine2 = strLine2Temp + strLine2;
					strLine3 = strLine3Temp + strLine3;
					break;
				default:
					strLine1 += strLine1Temp;
					strLine2 += strLine2Temp;
					strLine3 += strLine3Temp;
					break;
			}

		}

		//20021210 inux 
		private static void MakeStampCell(bool bCtrl, string[] sarStepTitle, string[] sarUserID, string[] sarEmpName
			, string[] sarWfStatus, string[] sarWfDateTime, string[] sarRouteEmp,  string[] sarRouteEmpName
			, ref string strLine1, ref string strLine2, ref string strLine3, string strLangID
			, string strClientID, string strSiteInitial, string strCultureID )
		{
			Resource res = new Resource();
			res.m_strLangID = strLangID;

			// inux 20021218 합의의 경우 셀병합...
			string isConcord  = "N";  // S:합의시작, M:합의중, E:합의마지막필드
			int    concordCnt = 0;
	
			for (int i=0; i< sarUserID.Length; i++)
			{
				string strLine1Temp, strLine2Temp, strLine3Temp, strUserName, strRouteUserName;

				strUserName = sarEmpName[i].Trim()+"["+sarUserID[i]+"]";
				strRouteUserName = sarRouteEmpName[i].Trim();

				// Charater Manipulation for Table Image
				if (sarStepTitle[i] == null || sarStepTitle[i] == string.Empty)
					sarStepTitle[i] = "&nbsp;";
				if (sarEmpName[i] == null || sarEmpName[i] == string.Empty)
					strUserName = "&nbsp;";
				if (sarRouteEmpName[i] == null || sarRouteEmpName[i] == string.Empty)
					strRouteUserName = "&nbsp;";
				if (sarWfStatus[i] == null || sarWfStatus[i] == string.Empty)
					sarWfStatus[i] = "&nbsp;";
				if (sarWfDateTime[i] == null || sarWfDateTime[i] == string.Empty)
					sarWfDateTime[i] = "&nbsp;";
							
				// inux 20021218 합의의 경우 셀병합...
				string nextTitle = string.Empty;
				string prevTitle = string.Empty;
				if (i > 0)
					prevTitle = sarStepTitle[i-1];

				try
				{
					nextTitle = sarStepTitle[i+1];
				}
				catch
				{
					nextTitle = string.Empty;
				}

				if (sarStepTitle[i] == nextTitle && sarStepTitle[i] != prevTitle)
				{
					isConcord = "S";
					concordCnt = 1;
				}
				else if (sarStepTitle[i] == prevTitle && sarStepTitle[i] != nextTitle)
				{
					isConcord = "E";
					++concordCnt;
				}
				else if (sarStepTitle[i] == prevTitle && sarStepTitle[i] == nextTitle)
				{
					isConcord = "M";
					++concordCnt;
				}
				else
				{
					isConcord = "N";
				}

				// Make Header
				if(isConcord == "E")
				{
					strLine1Temp = @"
		<td class=""TD02"" style=""background-color: #2c58a9; text-align: center;"" width=" + (bCtrl ? CFL.Toi(c_CellWidthS) * concordCnt : CFL.Toi(c_CellWidth) * concordCnt) + @" valign=""middle"" align=""center"" height=" + c_HeaderHeight + @" colspan="""+concordCnt+@""">
			< font size=""2"" color=""#ffffff""><b>" + sarStepTitle[i] + @"</b></font>
		</td>";
				}
				else if(isConcord == "S" || isConcord == "M")
				{
					strLine1Temp = string.Empty;
				}
				else
				{
					strLine1Temp = @"
		<td class=""TD02"" style=""background-color: #2c58a9; text-align: center;"" width=" + (bCtrl ? c_CellWidthS : c_CellWidth) + @" valign=""middle"" align=""center"" height=" + c_HeaderHeight + @" >
			<font size=""2"" color=""#ffffff""><b>" + sarStepTitle[i] + @"</b></font>
		</td>";
				}

				// Make Stamp Cell
				strLine2Temp = @"
		<td width=" + (bCtrl ? c_CellWidthS : c_CellWidth) + @" style=""text-align: center;"" valign=""middle"" align=""center"" height=" + c_CellHeight + @" >";
				if (sarWfStatus[i] == "A")
					strLine2Temp += @"
			<img height=" + c_ImageHeight + " width=" + c_ImageWidth + @" src=""/" + CFL.formRoot + @"/UserImages/UserStamps/" + strClientID + "/" + strSiteInitial + "_" + sarUserID[i] + @".bmp"" title=""[" + res.GetResString("P11") + "] " + res.GetResString("P09") + @"
[" + res.GetResString("P15") + "] " + sarEmpName[i] + "(" + sarUserID[i] + @")
[" + res.GetResString("P10") + "] " + CFL.DateConvert(sarWfDateTime[i], CFL.DCType.DTSAddSlash) + @""" />";
				else if (sarWfStatus[i] == "B")
					strLine2Temp += @"
			<img height=" + c_ImageHeight + " width=" + c_ImageWidth + @" src=""/" + CFL.formRoot + @"/Common/Image/Stamps/BackStamp.gif"" title=""[" + res.GetResString("P11") + "] " + res.GetResString("P12") + @"
[" + res.GetResString("P15") + "] " + sarEmpName[i] + "(" + sarUserID[i] + @")
[" + res.GetResString("P10") + "] " + CFL.DateConvert(sarWfDateTime[i], CFL.DCType.DTSAddSlash) + @""" />";
				else if (sarWfStatus[i] == "C")
					strLine2Temp += @"
			<img height=" + c_ImageHeight + " width=" + c_ImageWidth + @" src=""/" + CFL.formRoot + @"/Common/Image/Stamps/HoldStamp.gif"" title=""[" + res.GetResString("P11") + "] " + res.GetResString("P13") + @"
[" + res.GetResString("P15") + "] " + sarEmpName[i] + "(" + sarUserID[i] + @")
[" + res.GetResString("P10") + "] " + CFL.DateConvert(sarWfDateTime[i], SMART.CFL.DCType.DTSAddSlash) + @""" />";
				else if (sarWfStatus[i] == "D")
					strLine2Temp += @"
			<img height=" + c_ImageHeight + " width=" + c_ImageWidth + @" src=""/" + CFL.formRoot + @"/Common/Image/Stamps/SlashStamp.gif""  title=""[" + res.GetResString("P11") + "] " + res.GetResString("P14") + @"
[" + res.GetResString("P15") + "] " + sarEmpName[i] + "(" + sarUserID[i]+ @")
[" + res.GetResString("P10") + "] " + CFL.DateConvert(sarWfDateTime[i], SMART.CFL.DCType.DTSAddSlash) + @""" />";
				else
					strLine2Temp += @"&nbsp;";
						
				strLine2Temp += @"
		</td>";

				// Make Tail
				strLine3Temp = @"
		<td class=""TD02"" bgcolor=""#F1F1F1"" width=" + c_CellWidth + @" style=""text-align: center;"" valign=""middle"" align=""center"" " + c_HeaderHeight + @" >
      		<font size=""2"" color=""#000080"">";

				if (sarWfStatus[i] == "A" && sarWfDateTime[i].Length > 8)
					strLine3Temp += CFL.DateConvert(sarWfDateTime[i].Substring(0, 8), CFL.DCType.DAddSlash).Substring(2, 8);
				else if (sarWfStatus[i] == "B")
					strLine3Temp += strUserName;
				else if (sarWfStatus[i] == "C")
					strLine3Temp += strUserName;
				else if (sarWfStatus[i] == "D")
					strLine3Temp += "/";
				else
					strLine3Temp += strUserName;

				strLine3Temp += @"
			</font>
		</td>";

				switch (strCultureID.ToLower())
				{
					case "jp":
						strLine1 = strLine1Temp + strLine1;
						strLine2 = strLine2Temp + strLine2;
						strLine3 = strLine3Temp + strLine3;
						break;
					default:
						strLine1 += strLine1Temp;
						strLine2 += strLine2Temp;
						strLine3 += strLine3Temp;
						break;
				}
			}
		}
		
	#endregion
	}

	#region Struct for WfPrintInfoStep

	public struct WfPrintInfoStep
	{
		private string m_stepName;
		private string m_stampURL;
  
		public WfPrintInfoStep(string m_stepName, string m_stampURL)
		{
			this.m_stepName = m_stepName;
			this.m_stampURL = m_stampURL;
		}
 
		public string StepName
		{
			get
			{
				return m_stepName;
			}
			set
			{
				m_stepName = value;
			}
		}
 

		public string StampURL
		{
			get
			{
				return m_stampURL;
			}
			set
			{
				m_stampURL = value;
			}
		}
	}

	#endregion
 
	#region Struct for WfPrintInfo

	public struct WfPrintInfo
	{
		private ArrayList m_steps;
		private int m_stepHeight;
		private int m_stampHeight;
		private int m_stampWidth;
		private bool m_visible;
  
		public WfPrintInfo ( bool bVisible )
		{
			m_visible = bVisible;
			m_steps = new ArrayList();
			m_stepHeight = 0;
			m_stampHeight = 0;
			m_stampWidth = 0;
		}

		public bool Visible
		{
			get
			{
				return m_visible;
			}
			set
			{
				m_visible = value;
			}
		}
		public  ArrayList Steps
		{
			get
			{
				return m_steps;
			}
			set
			{
				m_steps = value;
			}
		}
 
		public int StepHeight
		{
			get
			{
				return m_stepHeight;
			}
			set
			{
				m_stepHeight = value;
			}
		}
		public int  StampHeight
		{
			get
			{
				return m_stampHeight;
			}
			set
			{
				m_stampHeight = value;
			}
		}
		public int  StampWidth
		{
			get
			{
				return m_stampWidth;
			}
			set
			{
				m_stampWidth = value;
			}
		}
 
	}

	#endregion
}
