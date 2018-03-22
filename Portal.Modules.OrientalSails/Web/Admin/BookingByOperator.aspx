<%@  Language="C#" MasterPageFile="SailsMaster.Master" AutoEventWireup="true" CodeBehind="BookingByOperator.aspx.cs"
    Inherits="Portal.Modules.OrientalSails.Web.Admin.BookingByOperator" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <fieldset>
        <legend>
            <img alt="Room" src="../Images/sails.gif" align="absMiddle" />
            Report by operators </legend>
        <div>
            <div>
                <table>
                    <tr>
                        <td>From</td>
                        <td><asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
                        <ajax:CalendarExtender ID="calendarFrom" runat="server" TargetControlID="txtFrom"
                            Format="dd/MM/yyyy">
                        </ajax:CalendarExtender></td>
                        <td>To</td>
                        <td><asp:TextBox ID="txtTo" runat="server"></asp:TextBox>
                        <ajax:CalendarExtender ID="calenderTo" runat="server" TargetControlID="txtTo"
                            Format="dd/MM/yyyy">
                        </ajax:CalendarExtender></td>
                        <td><asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Report" /></td>
                    </tr>
                </table>
            </div>
            <div class="data_table">
                <div class="data_grid">
                    <table>
                        <asp:Repeater ID="rptDates" runat="server" OnItemDataBound="rptDates_ItemDataBound">
                            <HeaderTemplate>
                                <tr>
                                    <th></th>
                                    <asp:Repeater ID="rptOperators" runat="server" OnItemDataBound="rptOperators_ItemDataBound">
                                        <ItemTemplate>
                                            <th>
                                                <asp:Literal ID="litName" runat="server"></asp:Literal>
                                            </th>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <th><asp:Literal ID="litDate" runat="server"></asp:Literal></th>
                                    <asp:Repeater ID="rptOperators" runat="server" OnItemDataBound="rptOperators_ItemDataBound">
                                        <ItemTemplate>
                                            <td style="text-align: right;">
                                                <asp:Literal ID="litCount" runat="server"></asp:Literal>
                                            </td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr>
                                    <th>Total</th>
                                    <asp:Repeater ID="rptOperators" runat="server" OnItemDataBound="rptOperators_ItemDataBound">
                                        <ItemTemplate>
                                            <td style="text-align: right;">
                                                <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                            </td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                            </FooterTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
        </div>
    </fieldset>
</asp:Content>
