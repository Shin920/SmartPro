using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WorkFlow;
using SMART;

namespace SMART.Common.Forms.WorkFlow.WfDocument
{
    /// <summary>
    /// Summary description for Approve.
    /// </summary>
    public partial class WfDocument : System.Web.UI.Page   // SMART.Advanced.WebForm
    {

        protected string strRefCnt1;
        protected string strRefCnt2;
        protected string strApprove;
        protected string strApproveAll;
        protected string strRead;
        protected string strHold;
        protected string strBack;
        protected string strHTitle;
        protected string strScript;
        protected string strPlan;
        protected string strCancel;

        public WfDocument()
        {
            Page.Init += new System.EventHandler(Page_Init);
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here
        }

        private void Page_Init(object sender, EventArgs e)
        {

            G = ((GObj)Session["G"]);

            // Render StyleSheet ( After Permission Check )
            Response.Write(G.StyleSheet);

            strRefCnt1 = CFL.RS("P37", "WorkFlow", Server, ((GObj)Session["G"]).LangID);
            strRefCnt2 = CFL.RS("P38", "WorkFlow", Server, ((GObj)Session["G"]).LangID);
            lblWfRoute.Text = CFL.RS("P35", "WorkFlow", Server, ((GObj)Session["G"]).LangID);
            lblDescr.Text = CFL.RS("P36", "WorkFlow", Server, ((GObj)Session["G"]).LangID);


        //  btnOK.Text = CFL.RS("P04", "WorkFlow", Server, ((GObj)Session["G"]).LangID);
            strHTitle = CFL.RS("P46", "WorkFlow", Server, ((GObj)Session["G"]).LangID);
            strScript = CFL.RS("P47", "WorkFlow", Server, ((GObj)Session["G"]).LangID);  // 결재경로를 선택하여 주십시오
            strPlan = CFL.RS("P48", "WorkFlow", Server, ((GObj)Session["G"]).LangID);   // 기안
            strCancel = CFL.RS("P49", "WorkFlow", Server, ((GObj)Session["G"]).LangID);

 
            
            /* inux 20021126 condition의 userid가 route item에 포함된 route는 제외 */
            
            // 2013.11.26 : 각각의 결재를 사용하는 그룹권한 체크 막음
            /*
            popRoute.WhereClause += @" 
    and not exists  ( Select userid 
                        From WfRouteItem
						Where WfRoute.SiteCode = WfRouteItem.SiteCode
						and WfRoute.RouteCode = WfRouteItem.RouteCode
						and WfRouteItem.UserID = " + CFL.Q(G.UserID) + @" ) 
--  and ( UserGroup = " + CFL.Q(G.UserGroup) + @" )
            ";
            */

            /***********************************************************/
            strApprove = CFL.RS("P39", "WorkFlow", Server, ((GObj)Session["G"]).LangID);
            strApproveAll = CFL.RS("P40", "WorkFlow", Server, ((GObj)Session["G"]).LangID);
            strRead = CFL.RS("P43", "WorkFlow", Server, ((GObj)Session["G"]).LangID);
            strHold = CFL.RS("P41", "WorkFlow", Server, ((GObj)Session["G"]).LangID);
            strBack = CFL.RS("P42", "WorkFlow", Server, ((GObj)Session["G"]).LangID);
        }

        GObj G;

        public string m_strScriptTarget = "";
        public string m_strStampRender = "&nbsp;";


        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>

        private void InitializeComponent()
        {

            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);        
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);        
            this.btnAction.Click += new System.EventHandler(this.btnAction_Click);        
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion

        protected void btnLoad_Click(object sender, System.EventArgs e)
        {
            // Set Loaded Flag
            txtLoaded.Text = "Y";

            string strViewState = txtViewState.Text;

            // WfInformation Load from ViewState
            int iCurStep = CFL.Toi(CFL.GetEncodedProperty("CurStep", strViewState));
            
            if (iCurStep == 0)
                iCurStep = 1;
            
            string strRouteCode = txtRouteCode.Text;
            string strRouteName = "";
            
            int iStepCnt = 0;
            
            string[] sarRouteUserID = new String[0];
                        

            // Make Stamp Cells
            string strErrorMsg = "";
            string strStampRender = WfCtrl.MakeStamp( G.D, ref strErrorMsg, false, strViewState
                                                    , null, G.SiteCode, G.LangID, Request["FormID"], G.UserID
                                                    , ref strRouteCode, ref strRouteName, ref iStepCnt, ref sarRouteUserID
                                                    , CFL.GetEncodedProperty("OriginNo", strViewState)

                                                //  , CFL.GetEncodedProperty("OriginSerNo", strViewState)
                                                    , CFL.GetEncodedProperty("OriginSer", strViewState)

                                                    , CFL.GetEncodedProperty("MoreKey", strViewState) );

            if ("" != strErrorMsg)
            {
                m_strScriptTarget = CFL.GetMsg(strErrorMsg);
                return;
            }

            // Get Route Information
            txtRouteCode.Text = strRouteCode;
            txtRouteName.Text = strRouteName;
            txtStepCnt.Text = iStepCnt.ToString();
            txtDocuDescr.Text = CFL.GetEncodedProperty("DocuDescr", strViewState);

            string[] sarUserIDRef = CFL.GetEncodedProperties("UserIDRef", strViewState);
            txtRefCnt.Text = sarUserIDRef.Length.ToString();
            string[] sarUserID = CFL.GetEncodedProperties("UserID", strViewState);

            // Determine Possible Action
            string strDocuStatus = CFL.GetEncodedProperty("DocuStatus", strViewState);


            // 해당유저ID의 RouteStep구하기.            
            SMART.WorkFlow obj = new SMART.WorkFlow();

            object[] alData = CFL.ArrayListToObject(obj.WfRouteStep(G.D, G.UserID, CFL.GetEncodedProperty("RouteCode", strViewState)));
            
            if (alData[0].ToString() != "00")
            {
                m_strScriptTarget = CFL.GetMsg(alData[1].ToString());
                return;
            }

            string strRouteStep;
            if (alData.Length < 5)
            {
                strRouteStep = "1";
            }
            else
            {
                strRouteStep = alData[4].ToString();
            }

            //20021212 inux 현문서 결제여부 check

            alData = CFL.ArrayListToObject(obj.hasApproved(G.D, Request["FormID"], CFL.GetEncodedProperty("OriginNo", strViewState)
                                                     //  , CFL.GetEncodedProperty("OriginSerNo", strViewState)
                                                         , CFL.GetEncodedProperty("OriginSer", strViewState)
                                                         , G.UserID
                                                         , CFL.GetEncodedProperty("MoreKey", strViewState) ) );
           
            if (alData[0].ToString() != "00")
            {
                m_strScriptTarget = CFL.GetMsg(alData[1].ToString());
                return;
            }

            if (CFL.Toi(alData[4]) > 0)
                hasApproved.Text = "Y"; //결제중일경우 표시..
            else
                hasApproved.Text = "N";


            // Running Document
            if ("X" == strDocuStatus || "Y" == strDocuStatus)	//x기안 y결제중
            {
                // CurStep Approve
                //if ( sarRouteUserID[iCurStep-2].ToLower() == G.UserID.ToLower() ) 
                //iCurStep 결제문서load한 상태 예) 1(기안),2(검토),3(결제), strRouteStep은 현재유저의 필드상태(2.0,2.1,3,.)
                if (iCurStep == CFL.Toi(strRouteStep.Substring(0, 1)) && hasApproved.Text != "Y")	//결제해야하는 상태에서 결제않했을경우

                    txtAction.Text = "Approve";
                // PrevStep Cancel
                //else if ( iCurStep > 1 && sarUserID[iCurStep-2].ToLower() == G.UserID.ToLower() )
                else if (iCurStep == CFL.Toi(strRouteStep.Substring(0, 1)))	//합의중 자기가 결제했을경우
                    txtAction.Text = "Cancel";
                else if ((iCurStep - 1) == CFL.Toi(strRouteStep.Substring(0, 1))) //합의없는 일반결제의경우

                    txtAction.Text = "Cancel";
                else
                    txtAction.Text = "Disable";
            }
            // Closed Document 결제완료
            else if ("Z" == strDocuStatus)
            {
                // Cancel Possible
                if (sarUserID[sarUserID.Length - 1].ToLower() == G.UserID.ToLower())
                    txtAction.Text = "Cancel";
                // Disable
                else
                    txtAction.Text = "Disable";
            }

            // Backed Document
            else if ("B" == strDocuStatus)
            {
                if (sarUserID[sarUserID.Length - 1].ToLower() == G.UserID.ToLower())
                    txtAction.Text = "Approve";
                // Disable
                else
                    txtAction.Text = "Cancel";
                //txtAction.Text = "Disable";
            }
            // Document on Hold
            else if ("H" == strDocuStatus)
            {
                if (sarUserID[iCurStep - 1].ToLower() == G.UserID.ToLower())	//자기가 보류한 문서일경우

                    txtAction.Text = "Hold";
                else if (iCurStep == CFL.Toi(strRouteStep.Substring(0, 1)) && hasApproved.Text != "Y")	//결제해야하는 상태에서 결제않했을경우

                    txtAction.Text = "Approve";
                else if (iCurStep == CFL.Toi(strRouteStep.Substring(0, 1)))	//합의중 자기가 결제했을경우
                    txtAction.Text = "Cancel";
                else if ((iCurStep - 1) == CFL.Toi(strRouteStep.Substring(0, 1))) //합의없는 일반결제의경우

                    txtAction.Text = "Cancel";
                // Disable
                else
                    txtAction.Text = "Disable";
            }
            // FirstTime
            else
            {
                // Anyone Possible
                txtAction.Text = "Approve";
            }


            //결제경로를 선택하지않았을경우 disable
            if (txtRouteCode.Text == "")
            {
                txtAction.Text = "Disable";
            }

            // 테스트 막음
            btnOK.Enabled = !(txtAction.Text == "Disable");

            // Route Enabling                        
            txtRouteCode.ReadOnly = !("" == strDocuStatus);
            txtRouteName.ReadOnly = !("" == strDocuStatus);


            /*
            popRoute.Enabled = ("" == strDocuStatus);
            */

            m_strStampRender = strStampRender;

            //isWfPwd 결제암호 필요여부
            alData = CFL.ArrayListToObject(obj.WfPSLoad(G.D, G.UserID));

            if (alData[0].ToString() != "00")
            {
                m_strScriptTarget = CFL.GetMsg(alData[1].ToString());
                return;
            }

            txtIsWfPwd.Text = alData[8].ToString();

        }


        protected void popRoute_OK(object source, System.EventArgs e)
        {
            btnLoad_Click(null, null);
        }


        protected void btnAction_Click(object sender, System.EventArgs e)
        {
            
            // WfInformation Load from ViewState
            int iCurStep = CFL.Toi(CFL.GetEncodedProperty("CurStep", txtViewState.Text));
            if (iCurStep == 0)
                iCurStep = 1;

            string strViewState = txtViewState.Text;
            string strDocuStatus = CFL.GetEncodedProperty("DocuStatus", strViewState);
            int iStepCnt = CFL.Toi(txtStepCnt.Text);
            bool bHold = false, bBack = false, approve = false;


            // 유저의 routeStep 구하기

            /*
            SVC.WorkFlowSoap obj = new SVC.WorkFlowSoap();
            SMART.OM.DA.WorkFlow obj = new SMART.OM.DA.WorkFlow();
            */ 
            SMART.WorkFlow obj = new SMART.WorkFlow();

            object[] alData = CFL.ArrayListToObject(obj.WfRouteStep(G.D, G.UserID, CFL.GetEncodedProperty("RouteCode", strViewState)));
            
            if (alData[0].ToString() != "00")
            {
                m_strScriptTarget = CFL.GetMsg(alData[1].ToString());
                return;
            }


            string strRouteStep;
            
            if (alData.Length < 5)
            {
                strRouteStep = "1.0";
                approve = false;	//	기안
            }
            else
            {
                strRouteStep = alData[4].ToString();
                approve = true;	//	결제
            }

            string[] tempRoute = strRouteStep.Split('.');

            // 결제후 iCurStep++수정..복수결제의 경우 formID, OriginNo, OriginSerNo을 구해야함.
            string OriginNo = CFL.GetEncodedProperty("OriginNo", strViewState);

        //  string OriginSerNo = CFL.GetEncodedProperty("OriginSerNo", strViewState);
            string OriginSerNo = CFL.GetEncodedProperty("OriginSer", strViewState);

            // 추가 (2013.08.26)
            string MoreKey = CFL.GetEncodedProperty("MoreKey", strViewState);

            
            alData = CFL.ArrayListToObject(obj.CanBeApprove(G.D, CFL.GetEncodedProperty("RouteCode", strViewState), iCurStep, Request["FormID"], OriginNo, OriginSerNo, MoreKey));
            
            if (alData[0].ToString() != "00")
            {
                m_strScriptTarget = CFL.GetMsg(alData[1].ToString());
                return;
            }

            string canBeApproved = alData[4].ToString();	// 다음 결제단계로 넘어갈수 있는지(TRUE/FALSE)
            string isAgreed = alData[5].ToString();	        // 다 합의 되었는지 여부(TRUE/FALSE)
            string hasHold = alData[6].ToString();	        // 해당되는 단계군에서 자기를 제외한 WfDocuRoute/WfStatus가 holding으로 있는지(TRUE/FALSE)


            string[] dumy = CFL.GetEncodedProperties("UserID", strViewState);

            int tempStep = dumy.Length;			// 현재 결재에 참가한 사람수..

            // Approve Button
            if ("Approve" == txtActionType.Text)
            {
                
                if ("Approve" == txtAction.Text || "Hold" == txtAction.Text)
                {
                    // $$$$ 무조건 tempStep을 사용하면 안됨.. H경우에서 -> Approve의 경우는

                    // 원래자기 encode property num를 이용해서 가져와야함.. tempstep을 대체
                    
                    int curStep = getStepNum(G.UserID, strViewState);

                    if (curStep > 0)
                        tempStep = curStep;

                    CFL.SetEncodedProperty("UserID" + (tempStep), G.UserID, ref strViewState);
                    CFL.SetEncodedProperty("EmpName" + (tempStep), G.UserName, ref strViewState);
                    CFL.SetEncodedProperty("WfStatus" + (tempStep), "A", ref strViewState);

                //  CFL.SetEncodedProperty("WfDateTime" + (tempStep), G.CurTime, ref strViewState);
                    CFL.SetEncodedProperty("WfDateTime" + (tempStep), DateTime.Now.ToString("yyyyMMddHHmmss"), ref strViewState);

                    CFL.SetEncodedProperty("RouteStep" + (tempStep), strRouteStep, ref strViewState);

                    //해당되는 단계군에서 자기를 제외한 WfDocuRoute/WfStatus가 holding으로 있는지
                    if (hasHold == "TRUE")
                    {
                        bHold = true;
                    }
                    //20021205 inux 결제후 iCurStep++수정..복수결제의 경우 
                    else if (canBeApproved == "TRUE" || iCurStep == 1)
                    {
                        iCurStep++;
                    }

                    hasApproved.Text = "Y"; //결제됨 정보 표기
                }
                else if ("Cancel" == txtAction.Text)
                {
                    // $$$ 원래 자기 encode property num를 이용해서 가져와야함.. tempstep을 대체

                    int curStep = getStepNum(G.UserID, strViewState);
                    CFL.SetEncodedProperty("UserID" + (curStep), "", ref strViewState, true);
                    CFL.SetEncodedProperty("EmpName" + (curStep), "", ref strViewState, true);
                    CFL.SetEncodedProperty("WfStatus" + (curStep), "", ref strViewState, true);
                    CFL.SetEncodedProperty("WfDateTime" + (curStep), "", ref strViewState, true);
                    CFL.SetEncodedProperty("RouteStep" + (curStep), "", ref strViewState);

                    int i = iCurStep - 1;
                    
                    // Remove D Items(전결 취소)
                    for (; "D" == CFL.GetEncodedProperty("WfStatus" + (tempStep - 1), strViewState); --tempStep)
                    {
                        CFL.SetEncodedProperty("UserID" + (tempStep - 1), "", ref strViewState, true);
                        CFL.SetEncodedProperty("EmpName" + (tempStep - 1), "", ref strViewState, true);
                        CFL.SetEncodedProperty("WfStatus" + (tempStep - 1), "", ref strViewState, true);
                        CFL.SetEncodedProperty("WfDateTime" + (tempStep - 1), "", ref strViewState, true);
                        CFL.SetEncodedProperty("RouteStep" + (tempStep - 1), "", ref strViewState);
                    }

                    //전결을 취소할 경우 iCurStep :100 ->현재스텝에서의 최대값.
                    if (iCurStep == 100)
                    {
                        string tempStep2 = CFL.GetEncodedProperty("RouteStep" + (tempStep - 1), strViewState);
                        tempRoute = tempStep2.Split('.');
                        iCurStep = CFL.Toi(tempRoute[0]);
                    }
                    //현재 결제 단계(결제상신: 현재단계-1, 합의중일경우0)
                    else if (isAgreed == "TRUE")
                    {
                        iCurStep = iCurStep - 1;
                    }
                    
                    hasApproved.Text = "";  //결제취소 정보 표기
                }
            }
            // Approve All the Items 전결
            else if ("ApproveAll" == txtActionType.Text)
            {
                if ("Approve" == txtAction.Text || "Hold" == txtAction.Text)
                {

                //  string strCurTime = G.CurTime;
                    string strCurTime = DateTime.Now.ToString("yyyyMMddHHmmss");

                    int i = iCurStep;

                    //전결 가능한 UserID, UserName, RouteStep 가져오기..(현재 RouteStep, UserID)..
                    alData = CFL.ArrayListToObject(obj.getApproveAll(G.D, CFL.GetEncodedProperty("RouteCode", strViewState), iCurStep, Request["FormID"]
                                                                   , OriginNo, OriginSerNo, G.UserID
                                                                   , MoreKey));
                    
                    if (alData[0].ToString() != "00")
                    {
                        m_strScriptTarget = CFL.GetMsg(alData[1].ToString());
                        return;
                    }

                    int count = CFL.Toi(alData[3]);
                    for (i = 0; i < CFL.Toi(count); i++)
                    {
                        CFL.SetEncodedProperty("UserID" + (i + tempStep), G.UserID, ref strViewState);
                        CFL.SetEncodedProperty("EmpName" + (i + tempStep), G.UserName, ref strViewState);

                        if (G.UserID == alData[4 + i * 2].ToString())
                            CFL.SetEncodedProperty("WfStatus" + (i + tempStep), "A", ref strViewState);	//결제
                        else
                            CFL.SetEncodedProperty("WfStatus" + (i + tempStep), "D", ref strViewState);	//전결

                        CFL.SetEncodedProperty("WfDateTime" + (i + tempStep), strCurTime, ref strViewState);                        

                        CFL.SetEncodedProperty("RouteStep" + (i + tempStep), alData[5 + i * 2].ToString(), ref strViewState);
                    }
                    iCurStep = 100;	//결제의 max값..
                }
            }
            // Hold Approving for a while
            else if ("Hold" == txtActionType.Text)
            {
                if ("Approve" == txtAction.Text)
                {
                    CFL.SetEncodedProperty("UserID" + (tempStep), G.UserID, ref strViewState);
                    CFL.SetEncodedProperty("EmpName" + (tempStep), G.UserName, ref strViewState);
                    CFL.SetEncodedProperty("WfStatus" + (tempStep), "C", ref strViewState);

                //  CFL.SetEncodedProperty("WfDateTime" + (tempStep), G.CurTime, ref strViewState);
                    CFL.SetEncodedProperty("WfDateTime" + (tempStep), DateTime.Now.ToString("yyyyMMddHHmmss"), ref strViewState);

                    CFL.SetEncodedProperty("RouteStep" + (tempStep), strRouteStep, ref strViewState);
                    bHold = true;
                }
                // Hold Cancel
                else if ("Hold" == txtAction.Text)
                {// $$$ 원래 자기 encode property num를 이용해서 가져와야함.. tempstep을 대체

                    int curStep = getStepNum(G.UserID, strViewState);
                    CFL.SetEncodedProperty("UserID" + (curStep), "", ref strViewState, true);
                    CFL.SetEncodedProperty("EmpName" + (curStep), "", ref strViewState, true);
                    CFL.SetEncodedProperty("WfStatus" + (curStep), "", ref strViewState, true);
                    CFL.SetEncodedProperty("WfDateTime" + (curStep), "", ref strViewState, true);
                    CFL.SetEncodedProperty("RouteStep" + (curStep), "", ref strViewState);
                }
            }
            // Back to the Submitee 반려...
            else if ("Back" == txtActionType.Text)
            {
                if ("Approve" == txtAction.Text || "Hold" == txtAction.Text)
                {
                    CFL.SetEncodedProperty("UserID" + (tempStep), G.UserID, ref strViewState);
                    CFL.SetEncodedProperty("EmpName" + (tempStep), G.UserName, ref strViewState);
                    CFL.SetEncodedProperty("WfStatus" + (tempStep), "B", ref strViewState);

                //  CFL.SetEncodedProperty("WfDateTime" + (tempStep), G.CurTime, ref strViewState);
                    CFL.SetEncodedProperty("WfDateTime" + (tempStep), DateTime.Now.ToString("yyyyMMddHHmmss"), ref strViewState);

                    CFL.SetEncodedProperty("RouteStep" + (tempStep), strRouteStep, ref strViewState);
                    iCurStep = iCurStep - 1;
                    bBack = true;
                }
            }
            // Nothing Selected
            else
            {
                m_strScriptTarget = CFL.GetMsg(CFL.RS("P47", "WorkFlow", Server, ((GObj)Session["G"]).LangID));
                return;
            }

            // Adjust CurStep
            CFL.SetEncodedProperty("CurStep", iCurStep.ToString(), ref strViewState);

            if (bHold)
                CFL.SetEncodedProperty("DocuStatus", "H", ref strViewState);
            else if (bBack)
                CFL.SetEncodedProperty("DocuStatus", "B", ref strViewState);
            else if (iCurStep == 1)
                CFL.SetEncodedProperty("DocuStatus", "", ref strViewState);
            else if (iCurStep == 2 && !approve)
                CFL.SetEncodedProperty("DocuStatus", "X", ref strViewState);//기안
            else if (iCurStep > CFL.Toi(txtStepCnt.Text))
                CFL.SetEncodedProperty("DocuStatus", "Z", ref strViewState);//결제완료
            else
                CFL.SetEncodedProperty("DocuStatus", "Y", ref strViewState);//결제중


            txtViewState.Text = strViewState;

            //inux수정 11.26 자동close(); //btnLoad_Click( null, null );
            CFL.SetEncodedProperty("RouteCode", txtRouteCode.Text, ref strViewState);
            // DocuDescr
            CFL.SetEncodedProperty("DocuDescr", txtDocuDescr.Text, ref strViewState);
            // SendMail
            CFL.SetEncodedProperty("SendMail", txtSendMail.Text, ref strViewState);


            txtViewState.Text = strViewState;

            m_strScriptTarget = @"
            <script>
                window.opener.document.all['" + Request["ViewStateName"] + @"'].value = document.all['txtViewState'].value;
                window.opener." + Request["OkFunctionName"] + @"();
                self.close();
            </script> ";
        }


        protected void btnOK_Click(object sender, System.EventArgs e)
        {
            string strViewState = txtViewState.Text;

            // RouteCode
            CFL.SetEncodedProperty("RouteCode", txtRouteCode.Text, ref strViewState);
            // DocuDescr
            CFL.SetEncodedProperty("DocuDescr", txtDocuDescr.Text, ref strViewState);
            // SendMail
            CFL.SetEncodedProperty("SendMail", txtSendMail.Text, ref strViewState);

            txtViewState.Text = strViewState;

            m_strScriptTarget = @"
			<script>
				self.close();
			</script>";
            //inux 변경 20021213 
            /*
            m_strScriptTarget = @"
<script>
    window.opener.document.all['" + Request["ViewStateName"] + @"'].value = document.all['txtViewState'].value;
    window.opener." + Request["OkFunctionName"] + @"();
self.close();
</script>
";
*/
        }

        protected void btn09_Click(object sender, System.EventArgs e)
        {

        }
        protected void btn07_Click(object sender, System.EventArgs e)
        {

        }

        protected void btn03_Click(object sender, System.EventArgs e)
        {

        }


        private int getStepNum(string userid, string encodeString)
        {
            string[] tempArray = encodeString.Split('|');
            int m, n = 0;

            for (int i = 0; i < tempArray.Length; i++)
            {
                m = tempArray[i].IndexOf("UserID");
                if (m > -1)
                {
                    n = tempArray[i].IndexOf(userid);
                    if (n > 0)
                    {
                        n = tempArray[i].IndexOf('=');
                        return CFL.Toi(tempArray[i].Substring(6, n - 6).Trim());
                    }

                }
            }
            return 0;
        }
    }
}