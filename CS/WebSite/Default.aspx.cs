using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Internal;
using DevExpress.XtraScheduler;

public partial class _Default : System.Web.UI.Page {
    private int lastInsertedAppointmentId;

    protected void Page_Load(object sender, EventArgs e) {
        if(!IsPostBack) {
            ASPxScheduler1.Start = new DateTime(2008, 7, 12);
            Session["currentUser"] = ASPxComboBox1.SelectedItem.Text;
        }
    }

    protected void ASPxScheduler1_AppointmentRowInserting(object sender, DevExpress.Web.ASPxScheduler.ASPxSchedulerDataInsertingEventArgs e) {
        e.NewValues.Remove("ID");
    }

    protected void SqlDataSourceAppointments_Inserted(object sender, SqlDataSourceStatusEventArgs e) {
        SqlConnection connection = (SqlConnection)e.Command.Connection;

        using(SqlCommand command = new SqlCommand("SELECT IDENT_CURRENT('CarScheduling')", connection)) {
            lastInsertedAppointmentId = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    protected void ASPxScheduler1_AppointmentRowInserted(object sender, DevExpress.Web.ASPxScheduler.ASPxSchedulerDataInsertedEventArgs e) {
        e.KeyFieldValue = lastInsertedAppointmentId;
    }

    protected void ASPxScheduler1_AppointmentsInserted(object sender, DevExpress.XtraScheduler.PersistentObjectsEventArgs e) {
        Appointment apt = (Appointment)e.Objects[0];
        ASPxSchedulerStorage storage = (ASPxSchedulerStorage)sender;
        storage.SetAppointmentId(apt, lastInsertedAppointmentId);
    }

    protected void ASPxScheduler1_BeforeExecuteCallbackCommand(object sender, SchedulerCallbackCommandEventArgs e) {
        bool allowFlag = Session["currentUser"].ToString() == "Admin";
        
        if (e.CommandId == SchedulerCallbackCommandId.MenuView)
            e.Command = new CustomMenuViewCallbackCommand((ASPxScheduler)sender, allowFlag);
        else if (e.CommandId == SchedulerCallbackCommandId.MenuAppointment)
            e.Command = new CustomMenuAppointmentCallbackCommand((ASPxScheduler)sender, allowFlag);
        else if (e.CommandId == SchedulerCallbackCommandId.AppointmentDelete)
            e.Command = new CustomAppointmentDeleteCallbackCommand((ASPxScheduler)sender, allowFlag);
    }
 
    protected void ASPxCallback1_Callback(object source, DevExpress.Web.ASPxCallback.CallbackEventArgs e) {
        Session["currentUser"] = ASPxComboBox1.SelectedItem.Text;
    }
}

public class CustomMenuViewCallbackCommand : MenuViewCallbackCommand {
    bool allowAppointmentCreate;
    bool currentCommandProhibited;

    public CustomMenuViewCallbackCommand(ASPxScheduler control, bool allowAppointmentCreate)
        : base(control) {
        this.allowAppointmentCreate = allowAppointmentCreate;
    }

    protected override void ParseParameters(string parameters) {
        bool isNewAppointmentCommand = (parameters == "NewAppointment" ||
            parameters == "NewAllDayEvent" ||
            parameters == "NewRecurringAppointment" ||
            parameters == "NewRecurringEvent");

        currentCommandProhibited = isNewAppointmentCommand && !allowAppointmentCreate;

        base.ParseParameters(parameters);
    }

    protected override void ExecuteCore() {
        if (currentCommandProhibited) {
            Control.CustomJSProperties += new CustomJSPropertiesEventHandler(ASPxScheduler1_CustomJSProperties);
        }
        else {
            base.ExecuteCore();
        }
    }

    void ASPxScheduler1_CustomJSProperties(object sender, CustomJSPropertiesEventArgs e) {
        e.Properties.Add("cpWarning", "The currently logged user cannot create appointments.");
    }
}

public class CustomMenuAppointmentCallbackCommand : MenuAppointmentCallbackCommand {
    private bool allowAppointmentDelete;
    private bool currentCommandProhibited;

    public CustomMenuAppointmentCallbackCommand(ASPxScheduler control, bool allowAppointmentDelete)
        : base(control) {
        this.allowAppointmentDelete = allowAppointmentDelete;
    }

    protected override void ParseParameters(string parameters) {
        bool isDeleteAppointmentCommand = (parameters == "DeleteAppointment");

        currentCommandProhibited = isDeleteAppointmentCommand && !allowAppointmentDelete;

        base.ParseParameters(parameters);
    }

    protected override void ExecuteCore() {
        if (currentCommandProhibited) {
            Control.CustomJSProperties += new CustomJSPropertiesEventHandler(ASPxScheduler1_CustomJSProperties);
        }
        else {
            base.ExecuteCore();
        }
    }

    private void ASPxScheduler1_CustomJSProperties(object sender, CustomJSPropertiesEventArgs e) {
        e.Properties.Add("cpWarning", "The currently logged user cannot delete appointments.");
    }
}

public class CustomAppointmentDeleteCallbackCommand : AppointmentDeleteCallbackCommand {
    private bool allowAppointmentDelete;

    public CustomAppointmentDeleteCallbackCommand(ASPxScheduler control, bool allowAppointmentDelete)
        : base(control) {
        this.allowAppointmentDelete = allowAppointmentDelete;
    }

    protected override void ExecuteCore() {
        if (!allowAppointmentDelete) {
            base.CanCloseForm = false;
            Control.CustomJSProperties += new CustomJSPropertiesEventHandler(ASPxScheduler1_CustomJSProperties);
        }
        else {
            base.ExecuteCore();
        }
    }

    private void ASPxScheduler1_CustomJSProperties(object sender, CustomJSPropertiesEventArgs e) {
        e.Properties.Add("cpWarning", "The currently logged user cannot delete appointments.");
    }
}