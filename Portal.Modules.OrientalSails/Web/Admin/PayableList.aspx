<%@ Page Language="C#" MasterPageFile="SailsMaster.Master" AutoEventWireup="true"
    CodeBehind="PayableList.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.PayableList"
    Title="Untitled Page" %>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <script>
        $(function () {
            $("#tabs").tabs({ 
                activate: function() {
                    var selectedTab = $('#tabs').tabs('option', 'active');
                    $("#<%= hdnSelectedTab.ClientID %>").val(selectedTab);
                },
                active: <%= hdnSelectedTab.Value %>
                });
        });
    </script>
    <fieldset>
        <legend>
            <img alt="Room" src="../Images/sails.gif" align="absMiddle" />
            Payable report </legend>
        <div class="search_panel">
            <table>
                <tr>
                    <td>From</td>
                    <td>
                        <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
                        <ajax:CalendarExtender ID="calendarFrom" runat="server" TargetControlID="txtFrom"
                            Format="dd/MM/yyyy">
                        </ajax:CalendarExtender>
                    </td>
                    <td>To</td>
                    <td>
                        <asp:TextBox ID="txtTo" runat="server"></asp:TextBox>
                        <ajax:CalendarExtender ID="calendarTo" runat="server" TargetControlID="txtTo" Format="dd/MM/yyyy">
                        </ajax:CalendarExtender>
                    </td>
                    <td>Supplier
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSupplier" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>Service</td>
                    <td>
                        <asp:DropDownList ID="ddlCostTypes" runat="server">
                        </asp:DropDownList></td>
                    <td>Trip Code</td>
                    <td>
                        <asp:TextBox ID="txtTripCode" runat="server">
                        </asp:TextBox></td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnDisplay" runat="server" Text="Display" OnClick="btnDisplay_Click"
            CssClass="button" />
        <div style="margin-bottom: -10px;">
            <ul style="list-style: none; padding: 0"
                class="tabbutton">
                <asp:Repeater ID="rptOrganization" runat="server" OnItemDataBound="rptOrganization_ItemDataBound">
                    <HeaderTemplate>
                        <li id="liMenu" runat="server" style="position: static; float: none; display: inline-block; width: -moz-max-content; width: -webkit-max-content; width: intrinsic; margin: 0;">
                            <asp:HyperLink ID="hplOrganization" runat="server" Text="All regions"></asp:HyperLink>
                        </li>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li id="liMenu" runat="server" style="position: static; float: none; display: inline-block; width: -moz-max-content; width: -webkit-max-content; width: intrinsic; margin: 0;">
                            <asp:HyperLink ID="hplOrganization" runat="server"></asp:HyperLink>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <div style="display: inline-block; margin-bottom: 15px;">
            <ul style="list-style: none; padding: 0px"
                class="tabbutton">
                <asp:Repeater ID="rptServices" runat="server" OnItemDataBound="rptServices_ItemDataBound">
                    <HeaderTemplate>
                        <li id="liMenu" runat="server" style="position: static; float: none; display: inline-block; width: -moz-max-content; width: -webkit-max-content; width: intrinsic; margin: 0;">
                            <asp:HyperLink ID="hplCostType" runat="server" Text="All"></asp:HyperLink>
                        </li>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li id="liMenu" runat="server" style="position: static; float: none; display: inline-block; width: -moz-max-content; width: -webkit-max-content; width: intrinsic; margin: 0;">
                            <asp:HyperLink ID="hplCostType" runat="server"></asp:HyperLink>
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        <li id="liMenu" runat="server" style="position: static; float: none; display: inline-block; width: -moz-max-content; width: -webkit-max-content; width: intrinsic; margin: 0;">
                            <asp:HyperLink ID="hplCostType" runat="server" Text="Theo số khách"></asp:HyperLink>
                        </li>
                    </FooterTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <div style="display: inline-block; float: right;">
            Payment status: 
            <ul style="list-style: none; padding: 0px"
                class="tabbutton">
                <li id="liAllPaid" runat="server" style="position: static; float: none; display: inline-block; width: -moz-max-content; width: -webkit-max-content; width: intrinsic; margin: 0;">
                    <asp:HyperLink ID="hplAllPaid" runat="server">All</asp:HyperLink></li>
                <li id="liNotPaid" runat="server" style="position: static; float: none; display: inline-block; width: -moz-max-content; width: -webkit-max-content; width: intrinsic; margin: 0;">
                    <asp:HyperLink ID="hplNotPaid" runat="server">Not paid</asp:HyperLink></li>
                <li id="liPaid" runat="server" style="position: static; float: none; display: inline-block; width: -moz-max-content; width: -webkit-max-content; width: intrinsic; margin: 0;">
                    <asp:HyperLink ID="hplPaid" runat="server">Paid</asp:HyperLink></li>
            </ul>
        </div>
        <div id="tabs" style="margin-bottom: 10px">
            <ul>
                <li><a href="#tabs-1">Deluxe Group Tour</a></li>
                <li><a href="#tabs-2">Hue Street Food</a></li>
            </ul>
            <div id="tabs-1">
                <div class="data_table">
                    <%--<h4>Tạm thời không hiển thị data để tối ưu hệ thống, vẫn có thể xuất sang excel bình thường</h4>--%>
                    <div class="data_grid">
                        <table>
                            <tr>
                                <th>Date</th>
                                <th>Trip code
                                </th>
                                <th>Partner</th>
                                <th>Service</th>
                                <th>Total</th>
                                <th>Paid</th>
                                <th>Payable</th>
                                <th style="width: 180px;"></th>
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
                                        <td colspan="4">Subtotal</td>
                                        <td>
                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal></td>
                                        <td>
                                            <asp:Literal ID="litPaid" runat="server"></asp:Literal></td>
                                        <td>
                                            <asp:Literal ID="litPayable" runat="server"></asp:Literal></td>
                                        <td>
                                            <asp:Button ID="btnPay" runat="server" Text="Pay all" CssClass="button" />
                                            <ajax:ConfirmButtonExtender ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="btnPay"
                                                ConfirmText='Are you sure you have pay all payable in the list? This action can not be undone.'>
                                            </ajax:ConfirmButtonExtender>
                                        </td>
                                    </tr>
                                </FooterTemplate>
                            </asp:Repeater>
                            <%--<tr>
                        <th>
                            GRAND TOTAL</th>
                        <th>
                            <asp:Literal ID="litGrandTotal" runat="server"></asp:Literal></th>
                        <td>
                            <asp:Literal ID="litGrandPaid" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="litGrandPayable" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Button ID="btnPay" runat="server" Text="Pay all" CssClass="button" />
                            <ajax:ConfirmButtonExtender ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="btnPay"
                                ConfirmText='Are you sure you have pay all payable in the list (and all pages)? This action can not be undone.'>
                            </ajax:ConfirmButtonExtender>
                        </td>
                    </tr>--%>
                        </table>
                    </div>
                    <div class="pager">
                        <svc:Pager ID="pagerServices" PagerLinkMode="HyperLinkQueryString" runat="server"
                            HideWhenOnePage="true" ControlToPage="rptExpenseServices" PageSize="50" />
                    </div>
                </div>
            </div>
            <div id="tabs-2">
                <div class="data_table">
                    <%--<h4>Tạm thời không hiển thị data để tối ưu hệ thống, vẫn có thể xuất sang excel bình thường</h4>--%>
                    <div class="data_grid">
                        <table>
                            <tr>
                                <th>Date</th>
                                <th>Trip code
                                </th>
                                <th>Partner</th>
                                <th>Service</th>
                                <th>Total</th>
                                <th>Paid</th>
                                <th>Payable</th>
                                <th style="width: 180px;"></th>
                                <th>Paid on</th>
                            </tr>
                            <asp:Repeater ID="rptExpenseServicesHsf" runat="server" OnItemDataBound="rptExpenseServicesHsf_ItemDataBound"
                                OnItemCommand="rptExpenseServicesHsf_ItemCommand">
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
                                        <td colspan="4">Subtotal</td>
                                        <td>
                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal></td>
                                        <td>
                                            <asp:Literal ID="litPaid" runat="server"></asp:Literal></td>
                                        <td>
                                            <asp:Literal ID="litPayable" runat="server"></asp:Literal></td>
                                        <td>
                                            <asp:Button ID="btnPay" runat="server" Text="Pay all" CssClass="button" />
                                            <ajax:ConfirmButtonExtender ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="btnPay"
                                                ConfirmText='Are you sure you have pay all payable in the list? This action can not be undone.'>
                                            </ajax:ConfirmButtonExtender>
                                        </td>
                                    </tr>
                                </FooterTemplate>
                            </asp:Repeater>
                            <%--<tr>
                        <th>
                            GRAND TOTAL</th>
                        <th>
                            <asp:Literal ID="litGrandTotal" runat="server"></asp:Literal></th>
                        <td>
                            <asp:Literal ID="litGrandPaid" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="litGrandPayable" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Button ID="btnPay" runat="server" Text="Pay all" CssClass="button" />
                            <ajax:ConfirmButtonExtender ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="btnPay"
                                ConfirmText='Are you sure you have pay all payable in the list (and all pages)? This action can not be undone.'>
                            </ajax:ConfirmButtonExtender>
                        </td>
                    </tr>--%>
                        </table>
                    </div>
                    <div class="pager">
                        <svc:Pager ID="pagerServicesHsf" PagerLinkMode="HyperLinkQueryString" runat="server"
                            HideWhenOnePage="true" ControlToPage="rptExpenseServicesHsf" PageSize="50" />
                    </div>
                </div>
            </div>
        </div>
        <asp:Button ID="btnExportPayables" runat="server" Text="Export all payables" CssClass="button" OnClick="btnExportPayables_Click" />
        <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" OnClick="btnExport_Click" />
        <asp:Button ID="btnExportOneSheet" runat="server" Text="Xuất 1 sheet" CssClass="button" OnClick="btnExportOneSheet_Click" />
        <asp:Button ID="btnExportGuide" runat="server" Text="Xuất báo cáo guide" CssClass="button" OnClick="btnExportGuide_Click" />
    </fieldset>
    <asp:HiddenField ID="hdnSelectedTab" runat="server" Value="0" />
</asp:Content>
