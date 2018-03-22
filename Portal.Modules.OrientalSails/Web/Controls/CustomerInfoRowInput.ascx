<%@ Control Language="C#" AutoEventWireup="true" Codebehind="CustomerInfoRowInput.ascx.cs"
    Inherits="Portal.Modules.OrientalSails.Web.Controls.CustomerInfoRowInput" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<td>
<asp:HiddenField ID="hiddenId" runat="server" />
<asp:TextBox ID="txtName" runat="server" Width="160"></asp:TextBox>
</td>
<td>
<asp:DropDownList ID="ddlGender" runat="server" Width="100">
    <asp:ListItem>-- Unknown --</asp:ListItem>
    <asp:ListItem>Male</asp:ListItem>
    <asp:ListItem>Female</asp:ListItem>
</asp:DropDownList>
</td>
<td>
<asp:DropDownList ID="ddlCustomerType" runat="server"  Width="50">
    <asp:ListItem>Adult</asp:ListItem>
    <asp:ListItem>Child</asp:ListItem>
    <asp:ListItem>Baby</asp:ListItem>
</asp:DropDownList>
</td>
<td>
<asp:TextBox ID="txtBirthDay" runat="server" Width="120"></asp:TextBox>
<ajax:CalendarExtender ID="calendarBirthday" runat="server" TargetControlID="txtBirthDay"
    Format="dd/MM/yyyy">
</ajax:CalendarExtender>
</td>
<td>
<asp:TextBox ID="txtNationality" runat="server" Width="120" Visible="false"></asp:TextBox>
</td>
<td>
<asp:DropDownList ID="ddlNationalities" runat="server" Width="120"></asp:DropDownList>
</td>
<td>
<asp:TextBox ID="txtVisaNo" runat="server" Width="120"></asp:TextBox>
</td>
<td>
<asp:TextBox ID="txtPassport" runat="server" Width="120"></asp:TextBox>
</td>
<td>
<asp:TextBox ID="txtVisaExpired" runat="server" Width="120"></asp:TextBox>
</td>
<td>
<asp:PlaceHolder ID="plhChild" runat="server">
    <asp:CheckBox ID="chkChild" runat="server" CssClass="checkbox" Text="Child"/>
</asp:PlaceHolder>
<asp:CheckBox ID="chkVietKieu" runat="server" CssClass="checkbox" Text="Viet Kieu" />
</td>
<td>
<ajax:CalendarExtender ID="calendarVisa" runat="server" TargetControlID="txtVisaExpired"
    Format="dd/MM/yyyy">
</ajax:CalendarExtender>
<asp:TextBox ID="txtCode" Visible="false" runat="server" Width="30"></asp:TextBox>
</td>
<td>
<asp:TextBox ID="txtTotal" runat="server" Width="60" style="text-align:right;"></asp:TextBox>
</td>