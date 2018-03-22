<%@ Page Language="C#" MasterPageFile="SailsMaster.Master" AutoEventWireup="true"
    CodeBehind="BookingReport.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.BookingReport"
    Title="Untitled Page" %>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <script type="text/javascript">
        function addCommas(nStr) {
            nStr = nStr.replace(',', '');
            nStr += '';
            x = nStr.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }
            return x1 + x2;
        }
    </script>
    <fieldset>
        <svc:Popup ID="popupManager" runat="server">
        </svc:Popup>
        <legend>
            <img alt="Room" src="../Images/sails.gif" align="absMiddle" />
            <%= base.GetText("textBookingByDate") %>
        </legend>
        <div>
            <%--<em>
                <%= base.GetText("textShowCustomerOnboardNote") %>
            </em>
            <br />
            <em>
                <%= base.GetText("textShowCustomerTransferNote") %>
            </em>--%>
            <table>
                <tr>
                    <td>
                        <%= base.GetText("textDateToView") %>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                        <ajax:CalendarExtender ID="calendarDate" runat="server" TargetControlID="txtDate"
                            Format="dd/MM/yyyy">
                        </ajax:CalendarExtender>
                    </td>
                    <td>
                         <asp:Button ID="btnDisplay" runat="server" Text="Display" OnClick="btnDisplay_Click"
                CssClass="button" />
                    </td>
                </tr>
            </table>           
            <div>
                <ul style="list-style: none; padding: 0px; padding-top: 5px; margin: 0px; margin-top: 10px;"
                    class="tabbutton">
                    <asp:Repeater ID="rptOrganization" runat="server" OnItemDataBound="rptOrganization_ItemDataBound">
                        <HeaderTemplate>
                            <li id="liMenu" runat="server">
                                <asp:HyperLink ID="hplOrganization" runat="server" Text="All regions"></asp:HyperLink>
                            </li>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li id="liMenu" runat="server">
                                <asp:HyperLink ID="hplOrganization" runat="server"></asp:HyperLink>
                                <ul>
                                    <asp:Repeater ID="rptTrips" runat="server" OnItemDataBound="rptTrips_ItemDataBound">
                                        <ItemTemplate>
                                            <li id="liMenu" runat="server">
                                                <asp:HyperLink ID="hplTrip" runat="server"></asp:HyperLink>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
            <div>
                <ul style="list-style: none; padding: 0px; padding-top: 5px; margin: 0px; margin-top: 10px;"
                    class="tabbutton">
                    <asp:Repeater ID="rptTrips" runat="server" OnItemDataBound="rptTrips_ItemDataBound">
                        <HeaderTemplate>
                            <li id="liMenu" runat="server">
                                <asp:HyperLink ID="hplTrip" runat="server" Text="All"></asp:HyperLink>
                            </li>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li id="liMenu" runat="server">
                                <asp:HyperLink ID="hplTrip" runat="server"></asp:HyperLink>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>
        <input type="button" id="btnComment" runat="server" value="View Comment" class="button"
            visible="false" />
        <div class="data_table">
            <div class="data_grid">
                <table>
                    <asp:Repeater ID="rptBookingList" runat="server" OnItemDataBound="rptBookingList_ItemDataBound"
                        OnItemCommand="rptBookingList_ItemCommand">
                        <HeaderTemplate>
                            <tr class="header">
                                <th rowspan="2">
                                    <%# base.GetText("textIndex") %>
                                </th>
                                <th rowspan="2">
                                    <%# base.GetText("textPax") %>
                                </th>
                                <th rowspan="2">
                                    Group
                                </th>
                                <th colspan="3">
                                    <%# base.GetText("textNumberOfPax") %>
                                </th>
                                <th rowspan="2">
                                    <%# base.GetText("textTrip") %>
                                </th>
                                <th rowspan="2">
                                    <%# base.GetText("textPickupAddress") %>
                                </th>
                                <th rowspan="2">
                                    <%# base.GetText("textSpecialRequest") %>
                                </th>
                                <asp:PlaceHolder ID="plhHidden" runat="server" Visible="false">
                                    <th colspan="2">
                                        <%# base.GetText("textNumberOfCabin") %>
                                    </th>
                                    <th colspan="2">
                                    </th>
                                </asp:PlaceHolder>
                                <th rowspan="2">
                                    <%# base.GetText("textAgency") %>
                                </th>
                                <th rowspan="2">
                                    <%# base.GetText("textBookingCode") %>
                                </th>
                                <th rowspan="2">
                                    <%# base.GetText("textTotal") %>
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    <%# base.GetText("textAdult") %>
                                </th>
                                <th>
                                    <%# base.GetText("textChild") %>
                                </th>
                                <th>
                                    <%# base.GetText("textBaby") %>
                                </th>
                                <asp:PlaceHolder ID="plhHidden2" runat="server" Visible="false">
                                    <th>
                                        Double
                                    </th>
                                    <th>
                                        Twin
                                    </th>
                                    <th>
                                        Adult
                                    </th>
                                    <th>
                                        Child
                                    </th>
                                </asp:PlaceHolder>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="item" id="trItem" runat="server">
                                <td>
                                    <asp:Literal ID="litIndex" runat="server"></asp:Literal>
                                    <asp:HiddenField ID="hiddenId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="label_NameOfPax" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlGroup" runat="server">
                                    </asp:DropDownList>
                                    <asp:Literal ID="litGroup" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Label ID="label_NoOfAdult" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="label_NoOfChild" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="label_NoOfBaby" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labelItinerary" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labelPuAddress" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labelSpecialRequest" runat="server"></asp:Label>
                                </td>
                                <td id="hidden1" runat="server" visible="false">
                                    <asp:Label ID="label_NoOfDoubleCabin" runat="server"></asp:Label>
                                </td>
                                <td id="hidden2" runat="server" visible="false">
                                    <asp:Label ID="label_NoOfTwinCabin" runat="server"></asp:Label>
                                </td>
                                <td id="hidden3" runat="server" visible="false">
                                    <asp:Label ID="label_NoOfTransferAdult" runat="server"></asp:Label>
                                </td>
                                <td id="hidden4" runat="server" visible="false">
                                    <asp:Label ID="label_NoOfTransferChild" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:HyperLink ID="hyperLink_Partner" runat="server"></asp:HyperLink>
                                </td>
                                <td>
                                    <asp:HyperLink ID="hplCode" runat="server"></asp:HyperLink>
                                </td>
                                <td>
                                    <asp:Label ID="label_TotalPrice" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <a id="anchorFeedback" runat="server" style="cursor: pointer;">Feedback</a>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr class="item">
                                <td colspan="3">
                                    <%# base.GetText("textGrandTotal") %>
                                </td>
                                <td>
                                    <strong>
                                        <asp:Label ID="label_NoOfAdult" runat="server"></asp:Label></strong>
                                </td>
                                <td>
                                    <strong>
                                        <asp:Label ID="label_NoOfChild" runat="server"></asp:Label></strong>
                                </td>
                                <td>
                                    <strong>
                                        <asp:Label ID="label_NoOfBaby" runat="server"></asp:Label></strong>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <asp:PlaceHolder ID="plhHidden" runat="server" Visible="false">
                                    <td>
                                        <strong>
                                            <asp:Label ID="label_NoOfDoubleCabin" runat="server"></asp:Label></strong>
                                    </td>
                                    <td>
                                        <strong>
                                            <asp:Label ID="label_NoOfTwinCabin" runat="server"></asp:Label></strong>
                                    </td>
                                    <td>
                                        <strong>
                                            <asp:Label ID="label_NoOfTransferAdult" runat="server"></asp:Label></strong>
                                    </td>
                                    <td>
                                        <strong>
                                            <asp:Label ID="label_NoOfTransferChild" runat="server"></asp:Label></strong>
                                    </td>
                                </asp:PlaceHolder>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <strong>
                                        <asp:Label ID="label_TotalPrice" runat="server"></asp:Label></strong>
                                </td>
                            </tr>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
        <div style="float: right;">
            <asp:Button ID="btnLockIncome" runat="server" CssClass="button" OnClick="btnLockIncome_Click"
                Text="Lock income" Visible="false" />
        </div>
        <div>
            <asp:Button ID="btnLock" runat="server" CssClass="button" OnClick="btnLock_Click"
                Visible="false" />
        </div>
        <div class="basicinfo">
            <asp:PlaceHolder ID="plhDailyExpenses" runat="server">
                <table>
                    <asp:PlaceHolder ID="plhOperator" runat="server">
                    <tr>
                        <td colspan="2">
                            Điều hành
                        </td>
                        <td colspan="5">
                            Tên
                            <asp:DropDownList ID="ddlOperators" runat="server" Width="100"></asp:DropDownList>
                            <asp:TextBox ID="txtOperator" runat="server" Width="100" ReadOnly="true"></asp:TextBox>
                            Điện thoại
                            <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                            Sale in charge
                            <asp:DropDownList ID="ddlSaleInCharge" runat="server">
                            </asp:DropDownList>
                            Tel. No.
                            <asp:TextBox ID="txtSalePhone" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    </asp:PlaceHolder>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <%= base.GetText("textSupplier") %>
                        </td>
                        <td>
                            <%= base.GetText("textName") %>
                        </td>
                        <td>
                            <%= base.GetText("textPhone") %>
                        </td>
                        <td>
                            <%= base.GetText("textCost") %>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <asp:Repeater ID="rptCruiseExpense" runat="server" OnItemDataBound="rptCruiseExpense_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td colspan="8" align="center">
                                    <asp:HiddenField ID="hiddenId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                                    <h4><%# DataBinder.Eval(Container.DataItem,"Name") %></h4>
                                </td>
                            </tr>
                            <asp:Repeater ID="rptServices" runat="server" OnItemDataBound="rptServices_ItemDataBound">
                                <ItemTemplate>
                                    <tr id="seperator" runat="server" class="seperator" visible="false">
                                        <td colspan="8" style="border-bottom: solid 1px black;">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="hiddenId" runat="server" />
                                            <asp:HiddenField ID="hiddenType" runat="server" />
                                            <asp:Literal ID="litType" runat="server"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSuppliers" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                            <asp:DropDownList ID="ddlGuides" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCost" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlGroups" runat="server" Visible="true" Width="50">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnRemove" runat="server" Text='<%# base.GetText("btnRemove") %>'
                                                CssClass="button" OnClick="btnRemove_Click" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr>
                                <td colspan="7">
                                    <asp:Button ID="btnAddServiceBlock" Text="Add block" CssClass="button" runat="server"
                                        OnClick="btnAddServiceBlock_Click" />
                                    <asp:Repeater ID="rptAddServices" runat="server" OnItemDataBound="rtpAddServices_ItemDataBound"
                                        Visible="true">
                                        <ItemTemplate>
                                            <asp:Button ID="btnAddService" runat="server" OnClick="btnAddService_Click" CssClass="button"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td colspan="5">
                                    Tổng
                                </td>
                                <td>
                                    <strong>
                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal></strong>
                                </td>
                            </tr>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="plhBarRevenue" runat="server">
                <table>
                    <tr>
                        <td>
                            Doanh thu quầy bar
                        </td>
                        <td>
                            <asp:TextBox ID="txtBarRevenue" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:PlaceHolder>
        </div>
        <asp:PlaceHolder ID="plhExpenses" runat="server">
            <div class="data_grid">
                <h4>
                    Dự toán chi phí</h4>
                <div class="data_table">
                    <table>
                        <tr>
                            <th>
                                Chi phí theo số khách
                            </th>
                            <th>
                                Chi phí theo chuyến
                            </th>
                            <th>
                                Tổng chi phí theo số khách và theo chuyến
                            </th>
                            <th>
                                Chi phí nhập ở trên
                            </th>
                            <th>
                                Tổng cộng
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="litCustomerCost" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litRunningCost" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litSubTotal" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litManual" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <strong>
                                    <asp:Literal ID="litTotal" runat="server"></asp:Literal></strong>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="plhNote" runat="server" Visible="false">
            <div class="basicinfo">
                <h4>
                    <%= base.GetText("textNote") %>
                </h4>
                <FCKeditorV2:FCKeditor ID="fckNote" runat="server" Width="100%" Height="200" BasePath="~/support/fckeditor/"
                    ToolbarSet="Basic">
                </FCKeditorV2:FCKeditor>
            </div>
        </asp:PlaceHolder>
        <asp:Button ID="btnSaveExpenses" runat="server" Text="Save without export" OnClick="btnSaveExpenses_Click"
            CssClass="button" />
        <asp:Button ID="btnExport" runat="server" Text="Export tour" OnClick="btnExport_Click"
            CssClass="button" />
        <asp:Button ID="btnIncomeDate" runat="server" Text="Xuất doanh thu" OnClick="btnIncomeDate_Click"
            Visible="false" CssClass="button" />
        <asp:Button ID="btnExportRoom" runat="server" Text="Export room list" OnClick="btnExportRoom_Click"
            Visible="false" CssClass="button" />
        <asp:Button ID="btnExportWelcome" runat="server" Text="Export Welcome board" OnClick="btnExportWelcome_Click"
            CssClass="button" />
        <asp:Button ID="btnExcel" runat="server" Text="Export customer data" OnClick="btnExcel_Click"
            CssClass="button" />
        <asp:Button ID="btnProvisional" runat="server" Text="Export provisional register"
            Visible="false" OnClick="btnProvisional_Click" CssClass="button" />
        <asp:Button ID="btnFaxHotel" runat="server" Text="Fax Hotel" OnClick="btnFaxHotel_Click"
            CssClass="button" />
    </fieldset>
</asp:Content>
