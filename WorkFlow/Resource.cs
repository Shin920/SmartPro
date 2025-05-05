using System;

namespace WorkFlow
{
	/// <summary>
	/// Summary description for Resource.
	/// </summary>
	public class Resource
	{
		public Resource()
		{
            this.m_strLangID = SMART.CFL.GetProfileInfo("CtrlLang");
		}

		public string m_strLangID;

		public string GetResString (string strCode)
		{
			string strLang = "";
			
			if (m_strLangID.ToUpper()=="KOR")
			{
				switch(strCode)
				{
					case "P01":
						strLang="결재진행중";
						break;
					case "P02":
						strLang="결재완료";
						break;
					case "P03":
						strLang="결재문서 기안";
						break;
					case "P04":
						strLang="결재문서가 도착하였습니다.";
						break;
					case "P05":
						strLang="로 접속하시기 바랍니다.";
						break;
					case "P06":
						strLang="메일을 전송하는데 실패하였습니다.";
						break;	
					case "P07":
						strLang="기안";
						break;
					case "P08":
						strLang="검토";
						break;
					case "P09":
						strLang="승인";
						break;
					case "P10":
						strLang="결재시각";
						break;
					case "P11":
						strLang="상태";
						break;
					case "P12":
						strLang="반송";
						break;
					case "P13":
						strLang="보류";
						break;
					case "P14":
						strLang="전결";
						break;
					case "P15":
						strLang="결재자";
						break;
					case "P16":
						strLang="개인설정화면에서 메일 주소를 등록하십시오.";
						break;
					default:
						strLang="";
						break;
				}
			}
			if (m_strLangID.ToUpper()=="ENG")
			{
				switch(strCode)
				{
					case "P01":
						strLang="Approval in progress";
						break;
					case "P02":
						strLang="Approval Completed";
						break;
					case "P03":
						strLang="Draft for approval document";
						break;
					case "P04":
						strLang="A approval document is arrived.";
						break;
					case "P05":
						strLang=" access, please.";
						break;
					case "P06":
						strLang="Failed to send the mail.";
						break;	
					case "P07":
						strLang="Draft";
						break;
					case "P08":
						strLang="Review";
						break;
					case "P09":
						strLang="Approve";
						break;
					case "P10":
						strLang="Approval Time";
						break;
					case "P11":
						strLang="State";
						break;
					case "P12":
						strLang="Reject";
						break;
					case "P13":
						strLang="Hold";
						break;
					case "P14":
						strLang="Arbitrary Decision";
						break;
					case "P15":
						strLang="Approver";
						break;
					case "P16":
						strLang="Register your mail address in Personal Setting screen.";
						break;
					default:
						strLang="";
						break;
				}
			}
			if (m_strLangID.ToUpper()=="JPN")
			{
				switch(strCode)
				{
					case "P01":
						strLang="決裁進行中";
						break;
					case "P02":
						strLang="決裁完了";
						break;
					case "P03":
						strLang="決裁文書起案";
						break;
					case "P04":
						strLang="決裁文書が到着しました。";
						break;
					case "P05":
						strLang="に接続してください。";
						break;
					case "P06":
						strLang="メール発送に失敗しました。";
						break;	
					case "P07":
						strLang="起案";
						break;
					case "P08":
						strLang="検討";
						break;
					case "P09":
						strLang="承認";
						break;
					case "P10":
						strLang="決裁時刻";
						break;
					case "P11":
						strLang="状態";
						break;
					case "P12":
						strLang="返送";
						break;
					case "P13":
						strLang="保留";
						break;
					case "P14":
						strLang="専決";
						break;
					case "P15":
						strLang="決裁者";
						break;
					case "P16":
						strLang="個人設定画面でメールアドレスを登録してください。";
						break;
					default:
						strLang="";
						break;
				}
			}
			if (m_strLangID.ToUpper()=="CHN")
			{
				switch(strCode)
				{
					case "P01":	
						strLang="决裁进行中";
						break;
					case "P02":	
						strLang="决裁完成";
						break;
					case "P03":	
						strLang="决裁文件 起案";
						break;
					case "P04":	
						strLang="有需要决裁的文件";
						break;
					case "P05":	
						strLang="请连接...";
						break;
					case "P06":	
						strLang="MAIL发送失败";
						break;
					case "P07":	
						strLang="起案";
						break;
					case "P08":	
						strLang="检讨";
						break;
					case "P09":	
						strLang="承认";
						break;
					case "P10":	
						strLang="决裁时间";
						break;
					case "P11":	
						strLang="状态";
						break;
					case "P12":	
						strLang="返送";
						break;
					case "P13":	
						strLang="保留";
						break;
					case "P14":	
						strLang="专决";
						break;
					case "P15":	
						strLang="决裁者";
						break;
					case "P16":	
						strLang="请在个人设置画面里登陆MAIL地址。";
						break;
					default:	
						strLang="";
						break;
				}
			}
			
			return strLang;
		}
	}
}
