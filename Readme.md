<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/134574076/10.1.12%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E3499)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [Default.aspx](./CS/WebSite/Default.aspx) (VB: [Default.aspx](./VB/WebSite/Default.aspx))
* [Default.aspx.cs](./CS/WebSite/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/WebSite/Default.aspx.vb))
<!-- default file list end -->
# End-User Restrictions - How to allow appointment creation or deletion only for specific users


<p>This example illustrates how to customize the <strong>MenuViewCallbackCommand</strong>, <strong>MenuAppointmentCallbackCommand</strong> and <strong>AppointmentDeleteCallbackCommand </strong>(see <a href="http://documentation.devexpress.com/#AspNet/CustomDocument5462"><u>Callback Commands</u></a>) to prevent some users from creating new appointments or deleting existing appointments. The current user in this example is selected via the <a href="http://documentation.devexpress.com/#AspNet/clsDevExpressWebASPxEditorsASPxComboBoxtopic"><u>ASPxComboBox</u></a> control. When a corresponding ASPxScheduler's callback occurs, handling is delegated to the custom callback command (see the <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebASPxSchedulerASPxScheduler_BeforeExecuteCallbackCommandtopic"><u>ASPxScheduler.BeforeExecuteCallbackCommand Event</u></a> handler). They determines if current operation should be denied. If so, base callback command logic is skipped and the <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebASPxSchedulerASPxScheduler_CustomJSPropertiestopic"><u>ASPxScheduler.CustomJSProperties Event</u></a> is handled in order to display a error message on the client-side (see the client-side <strong>EndCallback</strong> event handler in the Default.aspx file). This approach might be useful because, at present, there is no appropriate method to customize the popup menu dynamically (see the <a href="https://www.devexpress.com/Support/Center/p/Q346765">Context Menu Items Only for Appointments</a> thread).</p><p><strong>See Also:</strong><br />
<a href="https://www.devexpress.com/Support/Center/p/E3790">End-User Restrictions - How to allow appointment modification or deletion depending on custom field values</a></p>

<br/>


