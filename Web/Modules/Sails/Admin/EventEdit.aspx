<%@ Page Title="" Language="C#" MasterPageFile="SailsMaster.Master" AutoEventWireup="true" CodeBehind="EventEdit.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.EventEdit" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
<fieldset>
        <legend>
            <img alt="Room" src="../Images/sails.gif" align="absMiddle" />
            <asp:Label ID="titleBookingView" runat="server"></asp:Label>
        </legend>
        <div class="settinglist">            
            <div class="basicinfo">
                <h4>Summary</h4>
                <table cellspacing="1" style="background-color: black;">
                    <tbody style="background-color: white;">
                    <tr>
                        <th rowspan="2" style="text-align: center;">Date</th>
                        <th rowspan="2" style="text-align: center;">Trip</th>
                        <th rowspan="2" style="text-align: center;">Pax</th>
                        <th colspan="3" style="text-align: center;">Revenue</th>
                        <th colspan="3" style="text-align: center;">Expense</th>
                        <th colspan="2" style="text-align: center;">Profit</th>
                    </tr>
                    <tr>
                        <th style="text-align: center;">Total</th>
                        <th style="text-align: center;">Paid</th>
                        <th style="text-align: center;">Receivable</th>
                        <th style="text-align: center;">Total</th>
                        <th style="text-align: center;">Paid</th>
                        <th style="text-align: center;">Payable</th>
                        <th style="text-align: center;">Real Cash</th>
                        <th style="text-align: center;">Expected</th>
                    </tr>
                    <tr>
                        <td><asp:Literal ID="litDate" runat="server"></asp:Literal></td>
                        <td><asp:Literal ID="litTrip" runat="server"></asp:Literal></td>
                        <td><asp:Literal ID="litPax" runat="server"></asp:Literal></td>
                        <td style="text-align: right;"><asp:Literal ID="litRevenueTotal" runat="server"></asp:Literal></td>
                        <td style="text-align: right;"><asp:Literal ID="litRevenuePaid" runat="server"></asp:Literal></td>
                        <td style="text-align: right;"><asp:Literal ID="litReceivable" runat="server"></asp:Literal></td>
                        <td style="text-align: right;"><asp:Literal ID="litPayableTotal" runat="server"></asp:Literal></td>
                        <td style="text-align: right;"><asp:Literal ID="litPayablePaid" runat="server"></asp:Literal></td>
                        <td style="text-align: right;"><asp:Literal ID="litPayable" runat="server"></asp:Literal></td>
                        <td style="text-align: right;"><asp:Literal ID="litRealCash" runat="server"></asp:Literal></td>
                        <td style="text-align: right;"><asp:Literal ID="litExpected" runat="server"></asp:Literal></td>
                    </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="data_table">
            <h4>Bookings</h4>
            <div class="data_grid">
                <table>
                    <asp:Repeater ID="rptBookingList" runat="server" OnItemDataBound="rptBookingList_ItemDataBound">
                        <HeaderTemplate>
                            <tr class="header">
                                <th rowspan="2">
                                    Booking code
                                </th>
                                <th rowspan="2">
                                    TA Code
                                </th>
                                <th rowspan="2">
                                    Sale in charge
                                </th>
                                <th rowspan="2" style="width: 130px;">
                                    Partner
                                </th>
<%--                                <th rowspan="2">
                                    Cruise
                                </th>--%>
                                <th rowspan="2">
                                    Service
                                </th>
                                <th rowspan="2">
                                    Date
                                </th>
                                <th colspan="3">
                                    No of pax
                                </th>           
                                <th rowspan="2">Guide Collect</th>                     
                                <th rowspan="2">
                                    Total
                                </th>
                                <th colspan="2">
                                    Paid</th>
                                <th rowspan="2">
                                    Applied rate</th>
                                <th rowspan="2">
                                    Receivables</th>
                                <th rowspan="2">
                                    Action</th>
                                <th rowspan="2">
                                    Paid on
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    Adult</th>
                                <th>
                                    Child</th>
                                <th>
                                    Infant</th>
                                <th>
                                    USD</th>
                                <th>
                                    VND</th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr id="trItem" runat="server" class="item">
                                <td>
                                    <asp:HyperLink ID="hplCode" runat="server"></asp:HyperLink>
                                </td>
                                <td>
                                    <asp:Literal ID="litTACode" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="litSaleInCharge" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:HyperLink ID="hyperLink_Partner" runat="server"></asp:HyperLink>
                                </td>
<%--                                <td>
                                    <asp:HyperLink ID="hplCruise" runat="server"></asp:HyperLink>
                                </td>--%>
                                <td>
                                    <asp:Literal ID="litService" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:Literal ID="litDate" runat="server"></asp:Literal><asp:HiddenField ID="hiddenId"
                                        runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label ID="label_NoOfAdult" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label ID="label_NoOfChild" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label ID="label_NoOfBaby" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    <asp:Literal ID="litGuideCollect" runat="server"></asp:Literal>
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label ID="label_TotalPrice" runat="server">
                                        <asp:TextBox ID="txtTotal" runat="server" Style="display: none;"></asp:TextBox></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    <asp:Literal ID="litPaid" runat="server"></asp:Literal></td>
                                <td style="text-align: right;">
                                    <asp:Literal ID="litPaidBase" runat="server"></asp:Literal></td>
                                <td style="text-align: right;">
                                    <asp:Literal ID="litCurrentRate" runat="server"></asp:Literal></td>
                                <td style="text-align: right;">
                                    <asp:Literal ID="litReceivable" runat="server"></asp:Literal></td>
                                <td style="text-align: right;">
                                    <a id="aPayment" runat="server" style="cursor: pointer;">Payment</a>
                                </td>
                                <td>
                                    <asp:Literal ID="litPaidOn" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr class="item">
                                <td colspan="7">
                                    GRAND TOTAL
                                </td>
                                <td style="text-align: right;">
                                    <strong>
                                        <asp:Label ID="label_NoOfAdult" runat="server"></asp:Label></strong>
                                </td>
                                <td style="text-align: right;">
                                    <strong>
                                        <asp:Label ID="label_NoOfChild" runat="server"></asp:Label></strong>
                                </td>
                                <td style="text-align: right;">
                                    <strong>
                                        <asp:Label ID="label_NoOfBaby" runat="server"></asp:Label></strong>
                                </td>
                                <td style="text-align: right;">
                                    <strong>
                                        <asp:Label ID="label_TotalPrice" runat="server"></asp:Label></strong>
                                </td>
                                <td style="text-align: right;">
                                    <strong>
                                        <asp:Literal ID="litPaid" runat="server"></asp:Literal></strong></td>
                                <td style="text-align: right;">
                                    <strong>
                                        <asp:Literal ID="litPaidBase" runat="server"></asp:Literal></strong></td>
                                <td>
                                </td>
                                <td style="text-align: right;">
                                    <strong>
                                        <asp:Literal ID="litReceivable" runat="server"></asp:Literal></strong></td>
                                <td style="text-align: right;">
                                    <a id="aPayment" runat="server" style="cursor: pointer;">Pay all</a></td>
                            </tr>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>

        <div class="data_table">
            <h4>Expenses</h4>
            <div class="data_grid">
                <table>
                    <tr>
                        <th>
                            Date</th>
                        <th>
                            Trip code
                        </th>
                        <th>
                            Partner</th>
                        <th>
                            Service</th>
                        <th>
                            Total</th>
                        <th>
                            Paid</th>
                        <th>
                            Payable</th>
                        <th style="width: 160px;">
                        </th>
                        <th>Paid on</th>
                    </tr>
                    <asp:Repeater ID="rptExpenseServices" runat="server" OnItemDataBound="rptExpenseServices_ItemDataBound"
                        OnItemCommand="rptExpenseServices_ItemCommand">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:HyperLink ID="hplDate" runat="server"></asp:HyperLink></td>
                                <td>
                                    <asp:HyperLink ID="hplTripCode" runat="server"></asp:HyperLink>
                                </td>
                                <td>
                                    <asp:HyperLink ID="hplPartner" runat="server"></asp:HyperLink></td>
                                <td>
                                    <asp:HyperLink ID="hplService" runat="server"></asp:HyperLink></td>
                                <td>
                                    <asp:Literal ID="litTotal" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:Literal ID="litPaid" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:Literal ID="litPayable" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:TextBox ID="txtPay" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnPay" runat="server" Text="Pay" CssClass="button" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' /></td>
                                <td>
                                    <asp:Literal ID="litPaidOn" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td colspan="4">
                                    Subtotal</td>
                                <td>
                                    <asp:Literal ID="litTotal" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:Literal ID="litPaid" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:Literal ID="litPayable" runat="server"></asp:Literal></td>
                                <td>
                                </td>
                            </tr>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </fieldset>
</asp:Content>
