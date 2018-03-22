<%@ Page Language="C#" MasterPageFile="SailsMaster.Master" AutoEventWireup="true"
    Codebehind="BookingReportPeriodAll.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.BookingReportPeriodAll"
    Title="Untitled Page" %>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <fieldset>
        <legend>
            <img alt="Room" src="../Images/sails.gif" align="absMiddle" />
            <%= base.GetText("textBookingByPeriod") %>
        </legend>
        <div class="search_panel">
            <table>
                <tr>
                    <td>
                        <%= base.GetText("textDateFrom") %>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
                        <ajax:CalendarExtender ID="calendarFrom" runat="server" TargetControlID="txtFrom"
                            Format="dd/MM/yyyy">
                        </ajax:CalendarExtender>
                    </td>
                    <td>
                        <%= base.GetText("textDateTo") %>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTo" runat="server"></asp:TextBox>
                        <ajax:CalendarExtender ID="calendarTo" runat="server" TargetControlID="txtTo" Format="dd/MM/yyyy">
                        </ajax:CalendarExtender>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnDisplay" runat="server" Text="Display" OnClick="btnDisplay_Click"
            CssClass="button" />
        <div class="data_table">
            <div class="data_grid">
                <table>
                    <asp:Repeater ID="rptBookingList" runat="server" OnItemDataBound="rptBookingList_ItemDataBound"
                        OnItemCommand="rptBookingList_ItemCommand">
                        <HeaderTemplate>
                            <tr class="header">
                                <th style="width: 80px;">
                                    <%= base.GetText("textDate") %>
                                </th>
                                <asp:Repeater ID="rptTripRow" runat="server" OnItemDataBound="rptTripRow_ItemDataBound">
                                    <ItemTemplate>
                                        <th id="thTrip" runat="server" colspan="2">
                                        </th>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>                            
                            <asp:Literal ID="litTr" runat="server"></asp:Literal>
                            <td>
                                <asp:HyperLink ID="hplDate" runat="server"></asp:HyperLink>
                            </td>
                            <asp:Repeater ID="rptTripCustomer" runat="server" OnItemDataBound="rptTripCustomer_ItemDataBound">
                                <ItemTemplate>
                                    <td><asp:Literal ID="litGroup" runat="server"></asp:Literal></td>
                                    <td><asp:Literal ID="litCustomer" runat="server"></asp:Literal></td>
                                </ItemTemplate>
                            </asp:Repeater>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
        <asp:Button ID="btnExcel" runat="server" Text="Export" OnClick="btnExcel_Click" CssClass="button" />
    </fieldset>
</asp:Content>
