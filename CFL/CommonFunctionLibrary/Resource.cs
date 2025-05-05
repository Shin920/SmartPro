using System;

namespace SMART
{
	/// <summary>
	/// Resource에 대한 요약 설명입니다.
	/// </summary>
	public class Resource
	{
		public Resource()
		{
		}

		public static string GetString(string strLangID, string strID)
		{
			strLangID = strLangID.ToLower();
			string strResult = "";
			switch (strID)
			{
				case "01":
					switch (strLangID)
					{
						case "kor":
							strResult = "사용자 ID를 입력해 주십시오.";
							break;
						case "jpn":
							strResult = "ユーザーIDを入力してください。";
							break;
						case "chn":
							strResult = "请输入使用者ID。";
							break;
						default:
							strResult = "Please Input USER ID";
							break;
					}
					break;
				case "02":
					switch (strLangID)
					{
						case "kor":
							strResult = "사용자 정보를 가져올 수 없습니다.";
							break;
						case "jpn":
							strResult = "ユーザー情報を呼び出せません。";
							break;
						case "chn":
							strResult = "无法读取使用者信息。";
							break;
						default:
							strResult = "Failed to Load User Information";
							break;
					}
					break;
				case "03":
					switch (strLangID)
					{
						case "kor":
							strResult = "잘못된 사용자 ID입니다.";
							break;
						case "jpn":
							strResult = "誤ったユーザーIDです。";
							break;
						case "chn":
							strResult = "使用者ID错误";
							break;
						default:
							strResult = "Wrong User ID.";
							break;
					}
					break;
				case "04":
					switch (strLangID)
					{
						case "kor":
							strResult = "사용자";
							break;
						case "jpn":
							strResult = "ユーザー";
							break;
						case "chn":
							strResult = "使用者";
							break;
						default:
							strResult = "The available period of the User";
							break;
					}
					break;
				case "05":
					switch (strLangID)
					{
						case "kor":
							strResult = "의 유효기간이 만료되었습니다. 시스템 관리자에게 문의하여 주십시오.";
							break;
						case "jpn":
							strResult = "的已过使用有效期。请向系统管理者咨询。";
							break;
						case "chn":
							strResult = "";
							break;
						default:
							strResult = "has expired. Please contact your system administrator.";
							break;
					}
					break;
				case "06":
					switch (strLangID)
					{
						case "kor":
							strResult = "암호가 잘못되었습니다.";
							break;
						case "jpn":
							strResult = "パスワードが違います。";
							break;
						case "chn":
							strResult = "密码错误。";
							break;
						default:
							strResult = "Wrong Password.";
							break;
					}
					break;
				case "07":
					switch (strLangID)
					{
						case "kor":
							strResult = "결재용 암호가 잘못되었습니다.";
							break;
						case "jpn":
							strResult = "決裁用パスワードが誤りました。";
							break;
						case "chn":
							strResult = "决裁密码错误";
							break;
						default:
							strResult = "Wrong Password for Workflow.";
							break;
					}
					break;
				case "08":
					switch (strLangID)
					{
						case "kor":
							strResult = "인증서가 잘못되었습니다. USB Key를 확인해 주십시오.";
							break;
						case "jpn":
							strResult = "認証書が誤りました。USB Keyをご確認ください。";
							break;
						case "chn":
							strResult = "认证书错误。请确认USB Key。";
							break;
						default:
							strResult = "Wrong Certificate. Please check your USB Key.";
							break;
					}
					break;
				case "09":
					switch (strLangID)
					{
						case "kor":
							strResult = "인증서가 잘못되었습니다. 해당 PC에 인증서가 올바르게 설치되었는지 확인해 주십시오.";
							break;
						case "jpn":
							strResult = "認証書が誤りました。該当PCに認証書が正しく設置されているのか確認してください。";
							break;
						case "chn":
							strResult = "认证书错误。请确认认证书是否正确设置在相应PC上。";
							break;
						default:
							strResult = "Wrong Certificate. Please make sure your Certificate has Correctly installed in your PC.";
							break;
					}
					break;
			}
			return strResult;
		}
	}
}
