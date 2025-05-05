using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
using SMART;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class glb_MainContents_MainUserSchedule : PageBase
{
    GObj G = new GObj();
    SMART.COMMON.DS.MainContents ObjS = new SMART.COMMON.DS.MainContents();
    SMART.COMMON.DA.MainContents ObjA = new SMART.COMMON.DA.MainContents();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();

        G = (GObj)Session["G"];

        if (!IsPostBack)
        {
            schedule.DataBind();

            if (Request["Action"] == "Load")
            {
                schedule.Start = DateTime.Parse(Request["SDate"]);
            }
        }
    }

    bool isLocked = true;

    protected void schedule_DataBinding(object sender, EventArgs e)
    {
        G = (GObj)Session["G"];
        
        string strWhere = @"
 AND EmpCode = " + CFL.Q(G.EmpCode);

        DataSet appDs = ObjS.GetAppointmentsLoad(G.D, strWhere);
        schedule.AppointmentDataSource = appDs;

        DataSet resourcesDs = ObjS.GetResourcesLoad(G.D, strWhere);
        schedule.ResourceDataSource = resourcesDs;
    }

    protected void schedule_AppointmentInserting(object sender, DevExpress.XtraScheduler.PersistentObjectCancelEventArgs e)
    {
        if (CFL.GetStr(G.EmpCode) == "")
        {
            schedule.JSProperties["cpCallbackError"] = "현재 로그인 ID에는 사원번호 정보가 없습니다.";
            e.Cancel = false;
            return;
        }

        ASPxSchedulerStorage storage = sender as ASPxSchedulerStorage;
        Appointment newAppointment = e.Object as Appointment;

        string UniqueID = CFL.GetStr(newAppointment.Id);
        string EmpCode = CFL.GetStr(G.EmpCode);
        DateTime StartDate = DateTime.Parse(CFL.GetStr(newAppointment.Start));
        DateTime EndDate = DateTime.Parse(CFL.GetStr(newAppointment.End));
        string AllDay = CFL.GetStr(newAppointment.AllDay);
        string Subject = CFL.GetStr(newAppointment.Subject);
        string Description = CFL.GetStr(newAppointment.Description);
        string Label = CFL.GetStr(newAppointment.LabelKey);
        string Status = CFL.GetStr(newAppointment.StatusKey);
        string Location = CFL.GetStr(newAppointment.Location);

        storage.BeginUpdate();

        ArrayList aSave = ObjA.SetSave(G.D, UniqueID, EmpCode, StartDate, EndDate, AllDay, Subject, Description, Label, Status, Location);

        storage.EndUpdate();

        if (aSave[0].ToString() != "00")
        {
            schedule.JSProperties["cpCallbackError"] = "스케줄 등록 중 오류가 발생 하였습니다.";
            e.Cancel = false;
        }
        else
        {
            e.Cancel = true;
            schedule.DataBind();
        }
    }

    protected void schedule_AppointmentRowUpdating(object sender, ASPxSchedulerDataUpdatingEventArgs e)
    {
        if (CFL.GetStr(G.EmpCode) == "")
        {
            schedule.JSProperties["cpCallbackError"] = "현재 로그인 ID에는 사원번호 정보가 없습니다.";
            e.Cancel = false;
            return;
        }

        ASPxSchedulerStorage storage = sender as ASPxSchedulerStorage;

        string UniqueID = CFL.GetStr(e.Keys[0]);
        string EmpCode = CFL.GetStr(G.EmpCode);
        DateTime StartDate = DateTime.Parse(CFL.GetStr(e.NewValues["StartDate"]));
        DateTime EndDate = DateTime.Parse(CFL.GetStr(e.NewValues["EndDate"]));
        string AllDay = CFL.GetStr(e.NewValues["AllDay"]);
        string Subject = CFL.GetStr(e.NewValues["Subject"]);
        string Description = CFL.GetStr(e.NewValues["Description"]);
        string Label = CFL.GetStr(e.NewValues["Label"]);
        string Status = CFL.GetStr(e.NewValues["Status"]);
        string Location = CFL.GetStr(e.NewValues["Location"]);

        storage.BeginUpdate();

        ArrayList aSave = ObjA.SetSave(G.D, UniqueID, EmpCode, StartDate, EndDate, AllDay, Subject, Description, Label, Status, Location);

        storage.EndUpdate();

        if (aSave[0].ToString() != "00")
        {
            schedule.JSProperties["cpCallbackError"] = "스케줄 등록 중 오류가 발생 하였습니다.";
            e.Cancel = false;
        }
        else
        {
            e.Cancel = true;
            schedule.DataBind();
        }
    }

    protected void schedule_AppointmentDeleting(object sender, PersistentObjectCancelEventArgs e)
    {
        ASPxSchedulerStorage storage = sender as ASPxSchedulerStorage;
        string UniqueID = e.Object.Id.ToString();
        
        storage.BeginUpdate();

        ArrayList aDelete = ObjA.SetDelete(G.D, UniqueID);

        storage.EndUpdate();

        if (aDelete[0].ToString() != "00")
        {
            schedule.JSProperties["cpCallbackError"] = "스케줄 삭제 중 오류가 발생 하였습니다.";
            e.Cancel = false;
        }
        else
        {
            e.Cancel = true;
            schedule.DataBind();
        }
    }



}