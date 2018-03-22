<%@ Page Language="C#" MasterPageFile="SailsMaster.Master" AutoEventWireup="true"
    Codebehind="AgencyList.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.AgencyList"
    Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <fieldset>
        <legend>
            <img alt="Room" src="../Images/sails.gif" align="absMiddle" />
            <%= base.GetText("titleAgencyList") %>
        </legend>
        <div class="settinglist">
            <div class="search_panel">
                <table>
                    <tr>
                        <td>
                            <%= base.GetText("textName") %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
                        <td>
                            <%= base.GetText("textRole") %>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRoles" runat="server">
                            </asp:DropDownList></td>
                        <td>
                            <%= base.GetText("textSaleInCharge")%>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSales" runat="server">
                            </asp:DropDownList></td>
                    </tr>
                </table>
            </div>
            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                CssClass="button" />
        </div>
        <div class="data_table">
            <div class="data_grid">
                <table cellspacing="1">
                    <asp:Repeater ID="rptAgencies" runat="server" OnItemDataBound="rptAgencies_ItemDataBound"
                        OnItemCommand="rptAgencies_ItemCommand">
                        <HeaderTemplate>
                            <tr class="header">
                                <th colspan="2">
                                    <%= base.GetText("textAgencyName") %>
                                </th>
                                <th>
                                    <%= base.GetText("textPhone") %>
                                </th>
                                <th>
                                    <%= base.GetText("textFax") %>
                                </th>
                                <th>
                                    <%= base.GetText("textEmail") %>
                                </th>
                                <th>
                                    <%= base.GetText("textContract") %>
                                </th>
                                <th>
                                    <%= base.GetText("textSaleInCharge") %>
                                </th>
                                <th>
                                    <%= base.GetText("textRole") %>
                                </th>
                                <th>
                                    <%= base.GetText("textLastBooking") %>
                                </th>
                                <th>
                                    Price
                                </th>
                                <th>
                                    <%= base.GetText("textAction") %>
                                </th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr id="trItem" runat="server" class="item">
                                <td>
                                    <asp:Literal ID="litIndex" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:HyperLink ID="hplName" runat="server"></asp:HyperLink>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem,"Phone") %>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem,"Fax") %>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem,"Email") %>
                                </td>
                                <td>
                                    <asp:Literal ID="litContract" runat="server"></asp:Literal>
                                    <asp:HyperLink ID="hplContract" runat="server"></asp:HyperLink>
                                </td>
                                <td>
                                    <asp:Literal ID="litSale" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="litRole" runat="server"></asp:Literal></td>
                                <td id="tdLastBooking" runat="server">
                                </td>
                                <td>
                                    <asp:HyperLink ID="hplPriceSetting" runat="server">Setting
                                    </asp:HyperLink>
                                </td>
                                <td>
                                    <asp:HyperLink ID="hplEdit" runat="server">
                                        <asp:Image ID="imageEdit" runat="server" ImageAlign="AbsMiddle" AlternateText="Edit"
                                            CssClass="image_button16" ImageUrl="../Images/edit.gif" />
                                    </asp:HyperLink>
                                    <asp:ImageButton runat="server" ID="imageButtonDelete" ToolTip='Delete' ImageUrl="../Images/delete_file.gif"
                                        AlternateText='Delete' ImageAlign="AbsMiddle" CssClass="image_button16" CommandName="Delete"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                                    <ajax:ConfirmButtonExtender ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="imageButtonDelete"
                                        ConfirmText='<%# base.GetText("messageConfirmDelete") %>'>
                                    </ajax:ConfirmButtonExtender>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <div class="pager">
                <svc:Pager ID="pagerBookings" runat="server" HideWhenOnePage="true" ControlToPage="rptAgencies"
                    OnPageChanged="pagerBookings_PageChanged" PageSize="20" />
            </div>
        </div>
        <asp:Button ID="btnExportAgencyList" runat="server" Text="Export" OnClick="btnExport_Click"
            CssClass="button" />
    </fieldset>
</asp:Content>
