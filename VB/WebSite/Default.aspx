﻿<%@ Page Language="vb" AutoEventWireup="true"  CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register assembly="DevExpress.Web.v15.2, Version=15.2.20.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.ASPxScheduler.v15.2, Version=15.2.20.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxScheduler" tagprefix="dxwschs" %>
<%@ Register assembly="DevExpress.XtraScheduler.v15.2.Core, Version=15.2.20.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraScheduler" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="You are logged in as:" />

        <dx:ASPxComboBox ID="ASPxComboBox1" runat="server" SelectedIndex="0" ValueType="System.String">
            <Items>
                <dx:ListEditItem Selected="True" Text="Novice" Value="Novice" />
                <dx:ListEditItem Text="Admin" Value="Admin" />
            </Items>
            <ClientSideEvents SelectedIndexChanged="function(s, e) {
                callback.PerformCallback(s.GetSelectedItem().text);
            }" />
        </dx:ASPxComboBox>

        <dx:ASPxCallback ID="ASPxCallback1" runat="server" 
            ClientInstanceName="callback" oncallback="ASPxCallback1_Callback" />

        <dxwschs:ASPxScheduler ID="ASPxScheduler1" runat="server"
            AppointmentDataSourceID="SqlDataSourceAppointments" 
            ResourceDataSourceID="SqlDataSourceResources"
            onappointmentrowinserted="ASPxScheduler1_AppointmentRowInserted" 
            onappointmentrowinserting="ASPxScheduler1_AppointmentRowInserting" 
            onappointmentsinserted="ASPxScheduler1_AppointmentsInserted" 
            onbeforeexecutecallbackcommand="ASPxScheduler1_BeforeExecuteCallbackCommand">
            <ClientSideEvents EndCallback="function(s, e) {
                if (s.cpWarning) {
                    alert(s.cpWarning);
                    delete s.cpWarning;
                }
            }" />
            <Storage>
                <Appointments ResourceSharing="True">
                    <Mappings 
                        AppointmentId="ID" 
                        AllDay="AllDay" 
                        Description="Description" 
                        End="EndTime" 
                        Label="Label" 
                        Location="Location" 
                        RecurrenceInfo="RecurrenceInfo" 
                        ReminderInfo="ReminderInfo" 
                        ResourceId="CarId" 
                        Start="StartTime" 
                        Status="Status" 
                        Subject="Subject" 
                        Type="EventType" />
                </Appointments>
                <Resources>
                    <Mappings 
                        ResourceId="ID"  
                        Caption="Model" />
                </Resources>
            </Storage>
        </dxwschs:ASPxScheduler>

        <asp:SqlDataSource ID="SqlDataSourceResources" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CarsXtraSchedulingConnectionString %>" 
            SelectCommand="SELECT [ID], [Model] FROM [Cars]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceAppointments" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CarsXtraSchedulingConnectionString %>" 
            DeleteCommand="DELETE FROM [CarScheduling] WHERE [ID] = @ID" 
            InsertCommand="INSERT INTO [CarScheduling] ([CarId], [UserId], [Status], [Subject], [Description], [Label], [StartTime], [EndTime], [Location], [AllDay], [EventType], [RecurrenceInfo], [ReminderInfo], [Price], [ContactInfo]) VALUES (@CarId, @UserId, @Status, @Subject, @Description, @Label, @StartTime, @EndTime, @Location, @AllDay, @EventType, @RecurrenceInfo, @ReminderInfo, @Price, @ContactInfo)" 
            SelectCommand="SELECT * FROM [CarScheduling]" 
            UpdateCommand="UPDATE [CarScheduling] SET [CarId] = @CarId, [UserId] = @UserId, [Status] = @Status, [Subject] = @Subject, [Description] = @Description, [Label] = @Label, [StartTime] = @StartTime, [EndTime] = @EndTime, [Location] = @Location, [AllDay] = @AllDay, [EventType] = @EventType, [RecurrenceInfo] = @RecurrenceInfo, [ReminderInfo] = @ReminderInfo, [Price] = @Price, [ContactInfo] = @ContactInfo WHERE [ID] = @ID" 
            oninserted="SqlDataSourceAppointments_Inserted">
            <DeleteParameters>
                <asp:Parameter Name="ID" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="ID" Type="Int32" />
                <asp:Parameter Name="CarId" Type="String" />
                <asp:Parameter Name="UserId" Type="Int32" />
                <asp:Parameter Name="Status" Type="Int32" />
                <asp:Parameter Name="Subject" Type="String" />
                <asp:Parameter Name="Description" Type="String" />
                <asp:Parameter Name="Label" Type="Int32" />
                <asp:Parameter Name="StartTime" Type="DateTime" />
                <asp:Parameter Name="EndTime" Type="DateTime" />
                <asp:Parameter Name="Location" Type="String" />
                <asp:Parameter Name="AllDay" Type="Boolean" />
                <asp:Parameter Name="EventType" Type="Int32" />
                <asp:Parameter Name="RecurrenceInfo" Type="String" />
                <asp:Parameter Name="ReminderInfo" Type="String" />
                <asp:Parameter Name="Price" Type="Decimal" />
                <asp:Parameter Name="ContactInfo" Type="String" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="CarId" Type="String" />
                <asp:Parameter Name="UserId" Type="Int32" />
                <asp:Parameter Name="Status" Type="Int32" />
                <asp:Parameter Name="Subject" Type="String" />
                <asp:Parameter Name="Description" Type="String" />
                <asp:Parameter Name="Label" Type="Int32" />
                <asp:Parameter Name="StartTime" Type="DateTime" />
                <asp:Parameter Name="EndTime" Type="DateTime" />
                <asp:Parameter Name="Location" Type="String" />
                <asp:Parameter Name="AllDay" Type="Boolean" />
                <asp:Parameter Name="EventType" Type="Int32" />
                <asp:Parameter Name="RecurrenceInfo" Type="String" />
                <asp:Parameter Name="ReminderInfo" Type="String" />
                <asp:Parameter Name="Price" Type="Decimal" />
                <asp:Parameter Name="ContactInfo" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>

    </div>
    </form>
</body>
</html>