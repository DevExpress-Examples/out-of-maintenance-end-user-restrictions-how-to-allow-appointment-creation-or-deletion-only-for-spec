Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Web.UI.WebControls
Imports DevExpress.Web.ASPxClasses
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.Web.ASPxScheduler.Internal
Imports DevExpress.XtraScheduler

Partial Public Class _Default
	Inherits System.Web.UI.Page
	Private lastInsertedAppointmentId As Integer

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		If (Not IsPostBack) Then
			ASPxScheduler1.Start = New DateTime(2008, 7, 12)
			Session("currentUser") = ASPxComboBox1.SelectedItem.Text
		End If
	End Sub

	Protected Sub ASPxScheduler1_AppointmentRowInserting(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxScheduler.ASPxSchedulerDataInsertingEventArgs)
		e.NewValues.Remove("ID")
	End Sub

	Protected Sub SqlDataSourceAppointments_Inserted(ByVal sender As Object, ByVal e As SqlDataSourceStatusEventArgs)
		Dim connection As SqlConnection = CType(e.Command.Connection, SqlConnection)

		Using command As New SqlCommand("SELECT IDENT_CURRENT('CarScheduling')", connection)
			lastInsertedAppointmentId = Convert.ToInt32(command.ExecuteScalar())
		End Using
	End Sub

	Protected Sub ASPxScheduler1_AppointmentRowInserted(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxScheduler.ASPxSchedulerDataInsertedEventArgs)
		e.KeyFieldValue = lastInsertedAppointmentId
	End Sub

	Protected Sub ASPxScheduler1_AppointmentsInserted(ByVal sender As Object, ByVal e As DevExpress.XtraScheduler.PersistentObjectsEventArgs)
		Dim apt As Appointment = CType(e.Objects(0), Appointment)
		Dim storage As ASPxSchedulerStorage = CType(sender, ASPxSchedulerStorage)
		storage.SetAppointmentId(apt, lastInsertedAppointmentId)
	End Sub

	Protected Sub ASPxScheduler1_BeforeExecuteCallbackCommand(ByVal sender As Object, ByVal e As SchedulerCallbackCommandEventArgs)
		Dim allowFlag As Boolean = Session("currentUser").ToString() = "Admin"

		If e.CommandId = SchedulerCallbackCommandId.MenuView Then
			e.Command = New CustomMenuViewCallbackCommand(CType(sender, ASPxScheduler), allowFlag)
		ElseIf e.CommandId = SchedulerCallbackCommandId.MenuAppointment Then
			e.Command = New CustomMenuAppointmentCallbackCommand(CType(sender, ASPxScheduler), allowFlag)
		ElseIf e.CommandId = SchedulerCallbackCommandId.AppointmentDelete Then
			e.Command = New CustomAppointmentDeleteCallbackCommand(CType(sender, ASPxScheduler), allowFlag)
		End If
	End Sub

	Protected Sub ASPxCallback1_Callback(ByVal source As Object, ByVal e As DevExpress.Web.ASPxCallback.CallbackEventArgs)
		Session("currentUser") = ASPxComboBox1.SelectedItem.Text
	End Sub
End Class

Public Class CustomMenuViewCallbackCommand
	Inherits MenuViewCallbackCommand
	Private allowAppointmentCreate As Boolean
	Private currentCommandProhibited As Boolean

	Public Sub New(ByVal control As ASPxScheduler, ByVal allowAppointmentCreate As Boolean)
		MyBase.New(control)
		Me.allowAppointmentCreate = allowAppointmentCreate
	End Sub

	Protected Overrides Sub ParseParameters(ByVal parameters As String)
		Dim isNewAppointmentCommand As Boolean = (parameters = "NewAppointment" OrElse parameters = "NewAllDayEvent" OrElse parameters = "NewRecurringAppointment" OrElse parameters = "NewRecurringEvent")

		currentCommandProhibited = isNewAppointmentCommand AndAlso Not allowAppointmentCreate

		MyBase.ParseParameters(parameters)
	End Sub

	Protected Overrides Sub ExecuteCore()
		If currentCommandProhibited Then
			AddHandler Control.CustomJSProperties, AddressOf ASPxScheduler1_CustomJSProperties
		Else
			MyBase.ExecuteCore()
		End If
	End Sub

	Private Sub ASPxScheduler1_CustomJSProperties(ByVal sender As Object, ByVal e As CustomJSPropertiesEventArgs)
		e.Properties.Add("cpWarning", "The currently logged user cannot create appointments.")
	End Sub
End Class

Public Class CustomMenuAppointmentCallbackCommand
	Inherits MenuAppointmentCallbackCommand
	Private allowAppointmentDelete As Boolean
	Private currentCommandProhibited As Boolean

	Public Sub New(ByVal control As ASPxScheduler, ByVal allowAppointmentDelete As Boolean)
		MyBase.New(control)
		Me.allowAppointmentDelete = allowAppointmentDelete
	End Sub

	Protected Overrides Sub ParseParameters(ByVal parameters As String)
		Dim isDeleteAppointmentCommand As Boolean = (parameters = "DeleteAppointment")

		currentCommandProhibited = isDeleteAppointmentCommand AndAlso Not allowAppointmentDelete

		MyBase.ParseParameters(parameters)
	End Sub

	Protected Overrides Sub ExecuteCore()
		If currentCommandProhibited Then
			AddHandler Control.CustomJSProperties, AddressOf ASPxScheduler1_CustomJSProperties
		Else
			MyBase.ExecuteCore()
		End If
	End Sub

	Private Sub ASPxScheduler1_CustomJSProperties(ByVal sender As Object, ByVal e As CustomJSPropertiesEventArgs)
		e.Properties.Add("cpWarning", "The currently logged user cannot delete appointments.")
	End Sub
End Class

Public Class CustomAppointmentDeleteCallbackCommand
	Inherits AppointmentDeleteCallbackCommand
	Private allowAppointmentDelete As Boolean

	Public Sub New(ByVal control As ASPxScheduler, ByVal allowAppointmentDelete As Boolean)
		MyBase.New(control)
		Me.allowAppointmentDelete = allowAppointmentDelete
	End Sub

	Protected Overrides Sub ExecuteCore()
		If (Not allowAppointmentDelete) Then
			MyBase.CanCloseForm = False
			AddHandler Control.CustomJSProperties, AddressOf ASPxScheduler1_CustomJSProperties
		Else
			MyBase.ExecuteCore()
		End If
	End Sub

	Private Sub ASPxScheduler1_CustomJSProperties(ByVal sender As Object, ByVal e As CustomJSPropertiesEventArgs)
		e.Properties.Add("cpWarning", "The currently logged user cannot delete appointments.")
	End Sub
End Class