<%@ Page Language="C#" MasterPageFile="SailsMaster.Master" AutoEventWireup="true"
    Codebehind="BookingList.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.BookingList"
    Title="Untitled Page" %>

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
                            <%= base.GetText("textBookingCode") %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBookingId" runat="server"></asp:TextBox></td>
                        <td>
                            <%= base.GetText("textCustomerName") %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="labelTripName" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlOrgs" runat="server" Width="80"></asp:DropDownList>
                            <svc:CascadingDropDown runat="server" ID="cddlTrips" Width="110"/>
                        </td>
                        <td>
                            <asp:Label ID="labelStartDate" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="textBoxStartDate" runat="server"></asp:TextBox>
                            <asp:Image ID="imagecalenderStartDate" ImageUrl="/Images/calendar.gif" runat="server"
                                CssClass="image_button16" ImageAlign="AbsMiddle" />
                            <asp:RegularExpressionValidator ValidationGroup="date" ID="revStartDate" runat="server"
                                ControlToValidate="textBoxStartDate" EnableClientScript="false" ErrorMessage="Date is not valid"
                                ValidationExpression="(?n:^(?=\d)((?<day>31(?!(.0?[2469]|11))|30(?!.0?2)|29(?(.0?2)(?=.{3,4}(1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(16|[2468][048]|[3579][26])00))|0?[1-9]|1\d|2[0-8])(?<sep>[/.-])(?<month>0?[1-9]|1[012])\2(?<year>(1[6-9]|[2-9]\d)\d{2})(?:(?=\x20\d)\x20|$))?(?<time>((0?[1-9]|1[012])(:[0-5]\d){0,2}(?i:\ [AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$)"></asp:RegularExpressionValidator>
                            <ajax:CalendarExtender ID="calenderStartDate" runat="server" Format="dd/MM/yyyy"
                                TargetControlID="textBoxStartDate" PopupButtonID="imagecalenderStartDate">
                            </ajax:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <!--<tr>
                        <td><%= base.GetText("textBookingType") %></td>
                        <td><asp:DropDownList ID="ddlBookingTypes" runat="server">
                            <asp:ListItem>-- None --</asp:ListItem>
                            <asp:ListItem>Normal</asp:ListItem>
                            <asp:ListItem>Charter</asp:ListItem>
                        </asp:DropDownList></td>
                        <td></td>
                        <td></td>
                    </tr>-->
                    <tr>
                        <td>
                            <%= base.GetText("textStatus") %>
                        </td>
                        <td>
                            <input type="button" class="button" value="All" id="btnAll" runat="server" />
                            <input type="button" class="button" value="Approved" style="background-color: #92D050;
                                background-image: none" id="btnApproved" runat="server" />
                            <input type="button" class="button" value="Rejected" style="background-color: #FF7F7F;
                                background-image: none" id="btnRejected" runat="server" />
                            <input type="button" class="button" value="Cancelled" style="background-color: #FFFFFF;
                                background-image: none" id="btnCancelled" runat="server" />
                            <input type="button" class="button" value="Pending" style="background-color: #FFFF00;
                                background-image: none" id="btnPending" runat="server" />
                            <asp:PlaceHolder ID="plhCharter" runat="server">
                                <input type="button" class="button" value="Charter" style="background-color: #FF00FF;
                                    background-image: none" id="btnCharter" runat="server" />
                                <input type="button" class="button" value="On charter day" style="background-color: #AAAAAA;
                                    background-image: none" id="btnChartered" runat="server" />
                                <input type="button" class="button" value="Cần chuyển" style="background-color: #9ABAFF;
                                    background-image: none" id="btnNotTransferred" runat="server" />
                            </asp:PlaceHolder>
                        </td>
                        <asp:PlaceHolder ID="plhAccountStatus" runat="server">
                            <td>
                                <%= base.GetText("textAccountingStatus") %>
                            </td>
                            <td>
                                <input type="button" class="button" value="All" id="btnAccountingAll" runat="server" />
                                <input type="button" class="button" value="New" id="btnAccountingNew" runat="server" />
                                <input type="button" class="button" value="Not updated" id="btnAccountingNotUpdated"
                                    runat="server" />
                                <input type="button" class="button" value="Updated" id="btnAccountingUpdated" runat="server" />
                            </td>
                        </asp:PlaceHolder>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <em>Lưu ý: Số lượng ghi trong dấu "()" là số lượng booking khởi hành trong tương lai
                                (ngày xuất phát lớn hơn ngày hiện tại), không bao gồm các điều kiện lọc khác (theo
                                tên, theo ngày khởi hành...)</em>
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
                    <asp:Repeater ID="rptBookingList" runat="server" OnItemDataBound="rptBookingList_ItemDataBound"
                        OnItemCommand="rptBookingList_ItemCommand">
                        <HeaderTemplate>
                            <tr class="header">
                                <th>
                                    <%#base.GetText("textBookingCode")%>
                                </th>
                                <th>
                                    <%#base.GetText("labelTripName")%>
                                </th>
                                <th>
                                    <%#base.GetText("textNumberOfPax")%>
                                </th>
                                <th id="columnCustomerName" runat="server">
                                    <%#base.GetText("textCustomerName")%>
                                </th>
                                <th>
                                    <%#base.GetText("labelPartner") %>
                                </th>
                                <th>
                                    <%#base.GetText("textAgencyCode") %>
                                </th>
                                <th>
                                    <%#base.GetText("labelStatus") %>
                                </th>
                                <th>
                                </th>
                                <th>
                                    <%#base.GetText("textConfirmBy") %>
                                </th>
                                <th>
                                    <%#base.GetText("labelStartDate") %>
                                </th>
                                <asp:PlaceHolder ID="plhAccounting" runat="server">
                                    <th>
                                        <%#base.GetText("textAccounting") %>
                                    </th>
                                </asp:PlaceHolder>
                                <th>
                                    <%#base.GetText("labelAction") %>
                                </th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr id="trItem" runat="server" class="item">
                                <td>
                                    <asp:Panel CssClass="hover_content" ID="PopupMenu" runat="server">
                                        <asp:Literal ID="litNote" runat="server"></asp:Literal>
                                    </asp:Panel>
                                    <asp:HyperLink ID="hplCode" runat="server"></asp:HyperLink>
                                </td>
                                <td>
                                    <asp:HyperLink ID="hyperLink_Trip" runat="server"></asp:HyperLink>
                                </td>
                                <td>
                                    <asp:Literal ID="litPax" runat="server"></asp:Literal></td>
                                <td id="columnCustomerName" runat="server">
                                    <asp:Label ID="labelName" runat="server"></asp:Label></td>
                                <td>
                                    <asp:HyperLink ID="hplAgency" runat="server"></asp:HyperLink>
                                </td>
                                <td>
                                    <asp:Literal ID="litAgencyCode" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Label ID="label_Status" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Literal ID="litAmend" runat="server" Text="Amended"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Label ID="labelConfirmBy" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="label_startDate" runat="server"></asp:Label>
                                </td>
                                <td id="plhAccounting">
                                    <asp:Literal ID="litAccounting" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:HyperLink ID="hyperLinkView" runat="server">
                                        <asp:Image ID="imageView" runat="server" ImageAlign="AbsMiddle" AlternateText="View"
                                            CssClass="image_button16" ImageUrl="../Images/edit.gif" />
                                    </asp:HyperLink>
                                    <asp:ImageButton runat="server" ID="imageButtonDelete" ToolTip='Delete' ImageUrl="../Images/delete_file.gif"
                                        AlternateText='Delete' ImageAlign="AbsMiddle" CssClass="image_button16" CommandName="Delete"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' Visible="false" />
                                    <ajax:ConfirmButtonExtender ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="imageButtonDelete"
                                        ConfirmText='<%# base.GetText("messageConfirmDelete") %>'>
                                    </ajax:ConfirmButtonExtender>
                                    <asp:Image ImageUrl="../Images/info.png" ID="imgNote" runat="server" CssClass="image_button16" />
                                    <ajax:HoverMenuExtender ID="hmeNote" runat="Server" HoverCssClass="popupHover" PopupControlID="PopupMenu"
                                        PopupPosition="Left" TargetControlID="imgNote" PopDelay="25" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <div class="pager">
                <svc:Pager ID="pagerBookings" runat="server" HideWhenOnePage="true" ControlToPage="rptBookingList"
                    OnPageChanged="pagerBookings_PageChanged" PageSize="20" />
            </div>
        </div>
    </fieldset>
</asp:Content>
