<%@ Page Language="C#" MasterPageFile="SailsMaster.Master" AutoEventWireup="true" CodeBehind="GuideCollects.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.GuideCollects" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">

    <script type="text/javascript">
        function toggleVisible(id) {
            item = document.getElementById(id);
            if (item.style.display == "") {
                item.style.display = "none";
            }
            else {
                item.style.display = "";
            }
        }
    </script>

    <fieldset>
        <legend>
            <img alt="Room" src="../Images/sails.gif" align="absMiddle" />
            Guide collect </legend>
        <div class="search_panel">
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
                    <td>
                        Agency</td>
                    <td>
                        <asp:TextBox ID="txtAgency" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        Booking Code</td>
                    <td>
                        <asp:TextBox ID="txtBookingCode" runat="server"></asp:TextBox></td>
                    <td>
                        <%= base.GetText("textSaleInCharge")%>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSales" runat="server">
                        </asp:DropDownList></td>
                    <td>
                    Services</td>
                    <td>
                        <asp:DropDownList ID="ddlTrips" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Paid on</td>
                    <td><asp:TextBox ID="txtPaidOn" runat="server"></asp:TextBox>
                    <ajax:CalendarExtender ID="calenderPaidOn" runat="server" TargetControlID="txtPaidOn"
                            Format="dd/MM/yyyy">
                        </ajax:CalendarExtender>
                    </td>
                </tr>
            </table>
        </div>

        <asp:Button ID="btnDisplay" runat="server" Text="Display" OnClick="btnDisplay_Click"
            CssClass="button" />
        <div>
                <ul style="list-style: none; padding: 0px; padding-top: 5px; margin: 0px; margin-top: 10px;"
                    class="tabbutton">
                    <asp:Repeater ID="rptOrganization" runat="server" OnItemDataBound="rptOrganization_ItemDataBound">
                        <HeaderTemplate>
                            <li id="liMenu" runat="server">
                                <asp:HyperLink ID="hplOrganization" runat="server" Text="Your regions"></asp:HyperLink>
                            </li>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li id="liMenu" runat="server">
                                <asp:HyperLink ID="hplOrganization" runat="server"></asp:HyperLink>                                
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
        </div>
        <div style="clear:both;"></div>
        <svc:Popup ID="popupManager" runat="server">
        </svc:Popup>
        <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click"
            CssClass="button" />
        <asp:Button ID="btnExportNoAgency" runat="server" Text="Export sales data" OnClick="btnExportNoAgency_Click"
            CssClass="button" />
        <asp:Button ID="btnExportRevenue" runat="server" Text="Export revenue" OnClick="btnExportRevenue_Click"
            CssClass="button" />
        <asp:Button ID="btnExportRevenueBySale" runat="server" Text="Export revenue by sale"
            OnClick="btnExportRevenueBySale_Click" CssClass="button" />
        <div class="data_table">
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
    </fieldset>
</asp:Content>