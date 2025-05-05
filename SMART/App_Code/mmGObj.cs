using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace SMART
{
    /// <summary>
    /// Summary description for mmGObj.
    /// </summary>
    public class mmGObj
    {
        public mmGObj()
        {
        }

        /// <summary>
        ///  Properties
        /// </summary>
        public short DigitNo_mmOhPrice { get { return m_sDigitNo_mmOhPrice; } }
        protected short m_sDigitNo_mmOhPrice;
        public short DigitType_mmOhPrice { get { return m_sDigitType_mmOhPrice; } }
        protected short m_sDigitType_mmOhPrice;
        public decimal mmMrPlusTolerance { get { return m_dmmMrPlusTolerance; } }
        protected decimal m_dmmMrPlusTolerance;
        public string mmPiLockCheck { get { return m_strmmPiLockCheck; } }
        protected string m_strmmPiLockCheck;

        /// <summary>
        /// Service Object for this
        /// </summary>
        public object[] LoadInfo(string strSiteCode, object[] GDObj)
        {

            ArrayList alData = MMGLoadInfo(strSiteCode, GDObj);
            if ("00" == alData[0].ToString())
            {
                // set Properties
                m_sDigitNo_mmOhPrice = CFL.Tos(alData[4]);
                m_sDigitType_mmOhPrice = CFL.Tos(alData[5]);
                m_dmmMrPlusTolerance = CFL.Tod(alData[6]);
                m_strmmPiLockCheck = alData[7].ToString();
            }

            object[] alTemp = new object[4];
            alTemp[0] = alData[0];
            alTemp[1] = alData[1];
            alTemp[2] = alData[2];
            alTemp[3] = alData[3];

            return alTemp;
        }

        public ArrayList MMGLoadInfo(string strSiteCode, object[] GDObj)
        {
            GData GD = new GData(GDObj);



            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
-- 4, 5, 6, 7
				select DigitNo_mmOhPrice, DigitType_mmOhPrice, mmMrPlusTolerance, mmPiLockCheck
				from SiteMaster 
				where SiteCode = " + CFL.Q(strSiteCode);


            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = "품목공용 정보를 가져올 수 없습니다.";
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);

        }
    }
}
