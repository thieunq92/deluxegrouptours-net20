<%@ Page Title="" Language="C#" MasterPageFile="SailsMaster.Master" AutoEventWireup="true" CodeBehind="EventList.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.EventList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <fieldset>
        <legend>
            <img alt="Room" src="../Images/sails.gif" align="absMiddle" />
            <%= base.GetText("textBookingList") %>
        </legend>
        <div class="settinglist">
            <div class="basicinfo">
                <table>
                    <tr>
                    <td>
                        From</td>
                    <td>
                        <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
                        <ajax:CalendarExtender ID="calendarFrom" runat="server" TargetControlID="txtFrom"
                            Format="dd/MM/yyyy">
                        </ajax:CalendarExtender>
                    </td>
                    <td>
                        To</td>
                    <td>
                        <asp:TextBox ID="txtTo" runat="server"></asp:TextBox>
                        <ajax:CalendarExtender ID="calendarTo" runat="server" TargetControlID="txtTo" Format="dd/MM/yyyy">
                        </ajax:CalendarExtender>
                    </td>
                    </tr>
                </table>
            </div>
            <asp:Button ID="buttonSearch" runat="server" OnClick="buttonSearch_Click" ValidationGroup="date"
                CssClass="button" />
        </div>
        <div class="data_table">
            <div class="data_grid">
                <table cellspacing="1">
                    <asp:Repeater ID="rptEvents" runat="server" OnItemDataBound="rptEvents_ItemDataBound">
                        <HeaderTemplate>
                            <tr>                                
                                <th rowspan="2">Event code</th>
                                <th rowspan="2">Date</th>
                                <th rowspan="2">Trip</th>
                                <th rowspan="2">Number of pax</th>
                                <th colspan="3">Revenue</th>
                                <th colspan="3">Expenses</th>
                                <th rowspan="2">Profit</th>
                            </tr>
                            <tr>
                                <th>Paid</th>
                                <th>Receivable</th>
                                <th>Total</th>
                                <th>Paid</th>
                                <th>Payable</th>                                
                                <th>Total</th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Repeater ID="rptGroups" runat="server" OnItemDataBound="rptGroups_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td><asp:HyperLink ID="hplEventCode" runat="server"></asp:HyperLink></td>
                                        <td><asp:HyperLink ID="hplDate" runat="server"></asp:HyperLink></td>
                                        <td><asp:HyperLink ID="hplTrip" runat="server"></asp:HyperLink></td>
                                        <td style="text-align: right;"><asp:Literal ID="litPax" runat="server"></asp:Literal></td>
                                        <td style="text-align: right;"><asp:Literal ID="litRevenuePaid" runat="server"></asp:Literal></td>
                                        <td style="text-align: right;"><asp:Literal ID="litReceivable" runat="server"></asp:Literal></td>
                                        <td style="text-align: right;"><asp:Literal ID="litRevenue" runat="server"></asp:Literal></td>
                                        <td style="text-align: right;"><asp:Literal ID="litExpensePaid" runat="server"></asp:Literal></td>
                                        <td style="text-align: right;"><asp:Literal ID="litPayable" runat="server"></asp:Literal></td>
                                        <td style="text-align: right;"><asp:Literal ID="litExpense" runat="server"></asp:Literal></td>
                                        <td style="text-align: right;"><asp:Literal ID="litProfit" runat="server"></asp:Literal></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <div class="pager">
                <svc:Pager ID="pagerEvents" runat="server" HideWhenOnePage="true" ControlToPage="rptEvents" PageSize="100" PagerLinkMode="HyperLinkQueryString" />
            </div>
        </div>
    </fieldset>
</asp:Content>

