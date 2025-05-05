using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.OM.DS
{
    public class WorkFlow
    {
        public WorkFlow()
        {
        }
        public WorkFlow(SqlConnection db, SqlTransaction tr)
        {

            m_db = db;
            m_tr = tr;
            m_bInTrans = true;
        }

        SqlConnection m_db;
        SqlTransaction m_tr;
        bool m_bInTrans = false;


        public DataSet RouteList(object[] GDObj, string strViewState)
        {
            // Global Data
            GData GD = new GData(GDObj);

            DataSet ds = new DataSet("RouteInfo");

            string strQuery = @"
Select RTRIM(RouteCode) As RouteCode
     , RTRIM(RouteName) As RouteName
     , RTRIM(StepCnt) As StepCnt
From WfRoute
Where RouteUse = 'Y' 
  and SiteCode = " + CFL.Q(GD.SiteCode) + @"
  and not exists ( Select userid 
                   From WfRouteItem
				   Where WfRoute.SiteCode = WfRouteItem.SiteCode
					 and WfRoute.RouteCode = WfRouteItem.RouteCode
					 and WfRouteItem.UserID = " + CFL.Q(GD.UserID) + @" ) 
--  and ( UserGroup = N'G001' )  ";

            // 관리번호 선택 안했으면..
  //          if (!strViewState.Contains("OriginNo"))
  //          {
  //              strQuery += @"
  //and RouteCode = 'XXXXX' ";
  //          }

            strQuery += @"
Order by RouteCode
";

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);

            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }


        public DataSet WfIBCountNew(object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);

            DataSet ds = new DataSet("RouteInfo");

            string strQuery = @"
Select count(*)
From WfDocument A, WfRouteItem B
Where A.SiteCode = " + CFL.Q(GD.SiteCode) + @"
  and A.RouteCode = B.RouteCode 
  and A.SiteCode = B.SiteCode 
  and FLOOR(A.CurStep) = FLOOR(B.RouteStep)	
  and B.UserID = " + CFL.Q(GD.UserID) + @"
  and DocuStatus IN ('X', 'Y', 'H') ";

            // X:기안 ->Y:결재중 ->Z:결재완료 & B:반송 & H : 보류;

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);

            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }


    }
}