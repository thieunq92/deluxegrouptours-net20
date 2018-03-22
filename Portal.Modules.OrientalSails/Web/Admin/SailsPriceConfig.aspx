<%@ Page Language="C#" MasterPageFile="SailsMaster.Master" AutoEventWireup="true"
    CodeBehind="SailsPriceConfig.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.SailsPriceConfig"
    Title="Untitled Page" %>

<%@ Import Namespace="Portal.Modules.OrientalSails.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <fieldset>
        <legend>
            <img alt="Room" src="../Images/sails.gif" align="absMiddle" />
            <asp:Label ID="titleSailsPriceConfig" runat="server"></asp:Label>
        </legend>
        <asp:Panel ID="panelContent" runat="server">
            <div class="settinglist">
                <div class="advancedinfo">
                    <table style="width: 33%">
                        <tr>
                            <th>
                                Valid From
                            </th>
                            <th style="width: 70px">
                                &nbsp;
                            </th>
                            <th>
                            </th>
                            <%-- <th>
                                &nbsp;
                            </th>
                            <th>
                            </th>--%>
                        </tr>
                        <%--<asp:Repeater ID="rptPriceTables" runat="server" OnItemDataBound="rptPriceTables_ItemDataBound"
                            OnItemCommand="rptPriceTables_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Label ID="labelCruise" runat="server"></asp:Label>
                                    </td>--%>
                        <%--<td>
                                        <asp:Label ID="labelValidFrom" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="labelValidTo" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:LinkButton ID="hplEdit" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'>Edit</asp:LinkButton></td>--%>
                        <%--         </tr>
                            </ItemTemplate>
                        </asp:Repeater>--%>
                        <asp:Repeater runat="server" ID="rptValidFrom" OnItemDataBound="rptValidFrom_OnItemDataBound"
                            OnItemCommand="rptValidFrom_OnItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblValidFrom" Text='<%#((DateTime)Eval("ValidFrom")).ToString("dd/MM/yyyy")%>' />
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lbtnEdit" CommandName="Edit"></asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lbtnDelete" CommandName="Delete" CommandArgument='<%#((DateTime)Eval("ValidFrom")).ToString("dd/MM/yyyy")%>'></asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                    <div class="pager" style="text-align: left; margin-top: 9px; margin-bottom: -20px;">
                        <svc:Pager runat="server" ID="pgValidFrom" ControlToPage="rptValidFrom" HideWhenOnePage="true"
                            OnPageChanged="pgValidFrom_OnPageChanged" PageSize="5"></svc:Pager>
                    </div>
                </div>
                <%--<asp:Button ID="btnAddNew" runat="server" CssClass="button" Text="Add new table"
                    OnClick="btnAddNew_Click" />--%>
                <div class="advancedinfo">
                    <table style="margin-top:7px">
                        <tr>
                            <td>
                                <span style="font-weight: bold">Valid From</span>
                                <asp:TextBox ID="textBoxStartDate" runat="server"></asp:TextBox>
                                <ajax:CalendarExtender ID="calenderFrom" runat="server" TargetControlID="textBoxStartDate"
                                    OnClientShown="colorValidFrom" OnClientDateSelectionChanged="dateChanged" Format="dd/MM/yyyy">
                                </ajax:CalendarExtender>
                                <%--     <span style = "font-weight: bold">Valid To</span>
                                <asp:TextBox ID="textBoxEndDate" runat="server"></asp:TextBox>
                                <ajax:CalendarExtender ID="calenderTo" runat="server" TargetControlID="textBoxEndDate"
                                    Format="dd/MM/yyyy">
                                </ajax:CalendarExtender>--%>
                            </td>
                            <%-- <td>Apply for
                            <asp:DropDownList ID="ddlCruises" runat="server"></asp:DropDownList>
                            </td> --%>
                        </tr>
                    </table>
                    <script>
                        function dateChanged(sender, args) {
                            var date = sender.get_selectedDate().getDate();
                            var month = sender.get_selectedDate().getMonth();
                            month += 1;
                            if (date < 10)
                                date = "0" + date;
                            if (month < 10)
                                month = "0" + month;

                            window.location.href = '<%= Url %>&validfrom=' + date + month + sender.get_selectedDate().getFullYear();
                        }

                        function colorValidFrom(sender, args) {

                            sender._prevArrow.addEventListener("click",colorDate,false);
                            sender._nextArrow.addEventListener("click",colorDate,false);
                            sender._title.addEventListener("click",colorDate,false);
                            colorDate();
                        }

                        function colorDate() {
                            <%
                               String selector = "";
                               foreach (SailsPriceConfig sailsPriceConfig in Module.SailsPriceConfigGetAllBySailsTripAndOption(_trip, Option))
                               {
                                    selector = selector+"div[title='"+sailsPriceConfig.ValidFrom.ToLongDateString()+"'],";
                               }
                               if(!String.IsNullOrEmpty(selector))
                               selector = selector.Remove(selector.Length - 1);
                            %> 
                            
                            $("<%= selector%>").css("background-color", "gray");
                               
                            $(":not(<%= selector%>)").css("background-color", "white");

                        }
                    </script>
                </div>
                <div class="advancedinfo">
                    <%--<table>
                        <asp:Repeater runat="server" ID="rptRoomClass" OnItemDataBound="rptRoomClass_ItemDataBound">
                            <HeaderTemplate>
                                <tr>
                                    <th>
                                    </th>
                                    <asp:Repeater runat="server" ID="rptRoomTypeHeader">
                                        <ItemTemplate>
                                            <th>
                                                <%# DataBinder.Eval(Container.DataItem,"Name") %>
                                            </th>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <th>Single supplement (+)</th>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <th align="left">
                                        <asp:Label runat="server" ID="labelRoomClassId" Style="display: none;"></asp:Label>
                                        <%# DataBinder.Eval(Container.DataItem,"Name") %>
                                    </th>
                                    <asp:Repeater runat="server" ID="rptRoomTypeCell">
                                        <ItemTemplate>
                                            <td>
                                                <asp:Label runat="server" ID="labelRoomTypeId" Text='<%# DataBinder.Eval(Container.DataItem,"Id") %>'
                                                    Style="display: none;"></asp:Label>
                                                <asp:Label runat="server" ID="labelSailsPriceConfigId" Style="display: none;"></asp:Label>
                                                <b>USD</b> <asp:TextBox runat="server" ID="textBoxPrice" Width="80"></asp:TextBox> &nbsp;&nbsp;
                                                <b>VND</b> <asp:TextBox runat="server" ID="txtPriceVND" Width="80"></asp:TextBox>
                                            </td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <td><asp:TextBox ID="txtSingle" runat="server"></asp:TextBox></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>--%>
                    <script>
                        $(document).ready(function () {

                            var txtPriceAdultUSD = $("#<%= txtPriceAdultUSD.ClientID%>");
                            var hidPriceAdultUSD = $("#<%= hidPriceAdultUSD.ClientID%>");
                            var txtPriceAdultVND = $("#<%= txtPriceAdultVND.ClientID%>");
                            var hidPriceAdultVND = $("#<%= hidPriceAdultVND.ClientID%>");

                            var txtPriceBabyUSD = $("#<%= txtPriceBabyUSD.ClientID%>");
                            var hidPriceBabyUSD = $("#<%= hidPriceBabyUSD.ClientID%>");
                            var txtPriceBabyVND = $("#<%= txtPriceBabyVND.ClientID%>");
                            var hidPriceBabyVND = $("#<%= hidPriceBabyVND.ClientID%>");

                            var txtPriceChildUSD = $("#<%= txtPriceChildUSD.ClientID%>");
                            var hidPriceChildUSD = $("#<%= hidPriceChildUSD.ClientID%>");
                            var txtPriceChildVND = $("#<%= txtPriceChildVND.ClientID%>");
                            var hidPriceChildVND = $("#<%= hidPriceChildVND.ClientID%>");

                            hidPriceAdultUSD.val(txtPriceAdultUSD.val());
                            hidPriceChildUSD.val(txtPriceChildUSD.val());
                            hidPriceBabyUSD.val(txtPriceBabyUSD.val());
                            hidPriceAdultVND.val(txtPriceAdultVND.val());
                            hidPriceChildVND.val(txtPriceChildVND.val());
                            hidPriceBabyVND.val(txtPriceBabyVND.val());

                            txtPriceAdultUSD.blur(function () {
                                hidPriceAdultUSD.val(txtPriceAdultUSD.val());
                            });

                            txtPriceChildUSD.blur(function () {
                                hidPriceChildUSD.val(txtPriceChildUSD.val());
                            });

                            txtPriceBabyUSD.blur(function () {
                                hidPriceBabyUSD.val(txtPriceBabyUSD.val());
                            });

                            txtPriceAdultVND.blur(function () {
                                hidPriceAdultVND.val(txtPriceAdultVND.val());
                            });

                            txtPriceChildVND.blur(function () {
                                hidPriceChildVND.val(txtPriceChildVND.val());
                            });

                            txtPriceBabyVND.blur(function () {
                                hidPriceBabyVND.val(txtPriceBabyVND.val());
                            });
                        });

                        function assignValueToHiddenField(txt, hid) {
                            $("#" + hid).val($("#" + txt).val());
                        }
                    </script>
                    <table>
                        <tr>
                            <th width="10%">
                                Public Price
                            </th>
                            <th width="10%">
                                USD
                            </th>
                            <th width="10%">
                                VND
                            </th>
                            <th>
                            </th>
                        </tr>
                        <tr>
                            <td>
                                Adult
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPriceAdultUSD" />
                                <asp:HiddenField runat="server" ID="hidPriceAdultUSD" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtPriceAdultVND" runat="server" />
                                <asp:HiddenField runat="server" ID="hidPriceAdultVND" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Child
                            </td>
                            <td>
                                <asp:TextBox ID="txtPriceChildUSD" runat="server" />
                                <asp:HiddenField runat="server" ID="hidPriceChildUSD" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtPriceChildVND" runat="server" />
                                <asp:HiddenField runat="server" ID="hidPriceChildVND" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Baby
                            </td>
                            <td>
                                <asp:TextBox ID="txtPriceBabyUSD" runat="server" />
                                <asp:HiddenField runat="server" ID="hidPriceBabyUSD" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtPriceBabyVND" runat="server" />
                                <asp:HiddenField runat="server" ID="hidPriceBabyVND" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="advancedinfo">
                    <table style="width: 66%">
                        <tr>
                            <th rowspan="2" style="width: 12%">
                                Comission
                            </th>
                            <th colspan="2" style="text-align: center">
                                Adult
                            </th>
                            <th colspan="2" style="text-align: center">
                                Child
                            </th>
                            <th colspan="2" style="text-align: center">
                                Baby
                            </th>
                        </tr>
                        <tr>
                            <th style="text-align: center">
                                USD
                            </th>
                            <th style="text-align: center">
                                VND
                            </th>
                            <th style="text-align: center">
                                USD
                            </th>
                            <th style="text-align: center">
                                VND
                            </th>
                            <th style="text-align: center">
                                USD
                            </th>
                            <th style="text-align: center">
                                VND
                            </th>
                        </tr>
                        <asp:Repeater runat="server" ID="rptCommission" OnItemDataBound="rptCommission_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <asp:Label runat="server" ID="AgencyLevelId" Text='<%# DataBinder.Eval(Container.DataItem,"Id") %>'
                                        Style="display: none;"></asp:Label>
                                    <asp:Label runat="server" ID="AgencyCommissionId" Style="display: none;"></asp:Label>
                                    <td>
                                        <asp:Label runat="server" ID="AgencyLevelName" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtAdultCommissionUSD"></asp:TextBox>
                                        <asp:HiddenField runat="server" ID="hidAdultCommissionUSD"></asp:HiddenField>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtAdultCommissionVND"></asp:TextBox>
                                        <asp:HiddenField runat="server" ID="hidAdultCommissionVND"></asp:HiddenField>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtChildCommissionUSD"></asp:TextBox>
                                        <asp:HiddenField runat="server" ID="hidChildCommissionUSD"></asp:HiddenField>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtChildCommissionVND"></asp:TextBox>
                                        <asp:HiddenField runat="server" ID="hidChildCommissionVND"></asp:HiddenField>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtBabyCommissionUSD"></asp:TextBox>
                                        <asp:HiddenField runat="server" ID="hidBabyCommissionUSD"></asp:HiddenField>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtBabyCommissionVND"></asp:TextBox>
                                        <asp:HiddenField runat="server" ID="hidBabyCommissionVND"></asp:HiddenField>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
                <asp:Button ID="buttonSubmit" runat="server" CssClass="button" OnClick="buttonSubmit_Click" />
                <asp:Button ID="buttonCancel" runat="server" CssClass="button" OnClick="buttonCancel_Click" />
            </div>
        </asp:Panel>
    </fieldset>
</asp:Content>
