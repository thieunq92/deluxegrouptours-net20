<%@ Page Language="C#" MasterPageFile="SailsMaster.Master" AutoEventWireup="true"
    Codebehind="AgencyEdit.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.AgencyEdit"
    Title="Untitled Page" %>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls.FileUpload"
    TagPrefix="svc" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Src="/Admin/Controls/UserSelector.ascx" TagPrefix="asp" TagName="UserSelector" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <fieldset>
        <legend>
            <img alt="Room" src="../Images/sails.gif" align="absMiddle" />
            <%= base.GetText("textAgencyEdit")%></legend>
        <div class="settinglist">
            <svc:Popup ID="popupManager" runat="server">
            </svc:Popup>
            <div class="basicinfo">
                <table>
                    <tr>
                        <td>                            
                            <asp:Label ID="labelName" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="textBoxName" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ValidationGroup="valid" ControlToValidate="textBoxName"
                                ErrorMessage="Requied Field"></asp:RequiredFieldValidator>
                        </td>
                        <td rowspan="3">
                            <%= base.GetText("textAddress")%></td>
                        <td rowspan="3">
                            <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <%= base.GetText("textPhone")%></td>
                        <td>
                            <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <%= base.GetText("textFax")%></td>
                        <td>
                            <asp:TextBox ID="txtFax" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <%= base.GetText("textEmail")%></td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td>
                        <td>
                            <%= base.GetText("textRole")%></td>
                        <td>
                            <asp:DropDownList ID="ddlRoles" runat="server">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>
                            <%= base.GetText("textTaxCode")%></td>
                        <td>
                            <asp:TextBox ID="txtTaxCode" runat="server"></asp:TextBox></td>
                        <td>
                            <%= base.GetText("textContractStatus")%></td>
                        <td>
                            <asp:DropDownList ID="ddlContractStatus" runat="server">
                                <asp:ListItem Value="0">No</asp:ListItem>
                                <asp:ListItem Value="1">Pending</asp:ListItem>
                                <asp:ListItem Value="2">Yes</asp:ListItem>
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>
                            <%= base.GetText("textSaleInCharge")%></td>
                        <td>
                            <asp:DropDownList ID="ddlSales" runat="server">
                            </asp:DropDownList></td>
                        <td>
                            <%= base.GetText("textAccountant")%></td>
                        <td>
                            <asp:TextBox ID="txtAccountant" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            Region
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlOrganizations" runat="server"></asp:DropDownList>
                        </td>
                        <td>
                            <%= base.GetText("textPaymentPeriod")%></td>
                        <td>
                            <asp:DropDownList ID="ddlPaymentPeriod" runat="server">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td colspan="2"><asp:Literal ID="litCreated" runat="server"></asp:Literal></td>
                        <td colspan="2"><asp:Literal ID="litModified" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td>
                            <%= base.GetText("textOthers")%></td>
                        <td colspan="3">
                            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Width="98%"></asp:TextBox></td>
                    </tr>
                </table>
            </div>
            <asp:Button ID="buttonSave" runat="server" OnClick="buttonSave_Click" CssClass="button"
                ValidationGroup="valid" />
            <asp:PlaceHolder ID="plhContracts" runat="server" Visible="false">
                <div class="basicinfo">
                    <table>
                        <asp:Repeater ID="rptContracts" runat="server" OnItemDataBound="rptContracts_ItemDataBound"
                            OnItemCommand="rptContracts_ItemCommand">
                            <HeaderTemplate>
                                <tr>
                                    <td>
                                        <%= base.GetText("textContractName")%></td>
                                    <td>
                                        <%= base.GetText("textExpiredDate")%></td>
                                    <td>
                                    </td>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "ContractName") %>
                                    </td>
                                    <td>
                                        <asp:Literal ID="litExpiredDate" runat="server"></asp:Literal></td>
                                    <td>
                                        <asp:ImageButton ID="imageView" runat="server" ImageAlign="AbsMiddle" AlternateText="View"
                                            CssClass="image_button16" ImageUrl="../Images/edit.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                                            CommandName="edit" Width="16" />
                                        <asp:LinkButton ID="lbtDownload" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                                            CommandName='download' Text='<%# DataBinder.Eval(Container.DataItem, "FileName") %>'></asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
                <asp:HiddenField ID="hiddenContractId" runat="server" />
                <div class="basicinfo">
                    <table>
                        <tr>
                            <td>
                                <%= base.GetText("textContractName")%></td>
                            <td>
                                <asp:TextBox ID="txtContractName" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <%= base.GetText("textExpiredDate")%></td>
                            <td>
                                <asp:TextBox ID="txtExpiredDate" runat="server"></asp:TextBox><ajax:CalendarExtender
                                    ID="calendarDate" runat="server" TargetControlID="txtExpiredDate" Format="dd/MM/yyyy">
                                </ajax:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= base.GetText("textContractUpload")%></td>
                            <td>
                                <asp:FileUpload ID="fileUploadContract" runat="server" /></td>
                        </tr>
                    </table>
                </div>
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="button" />
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="plhHidden" runat="server" Visible="false">
            <div class="basicinfo">
                <h4>
                    Contract file</h4>
                <asp:HyperLink ID="hplOldContract" runat="server">View current contract</asp:HyperLink><br />
                <svc:FileUploaderAJAX ID="uploadContract" runat="server"></svc:FileUploaderAJAX>
                <asp:TextBox ID="txtPath" runat="server" Style="display: none;"></asp:TextBox>
            </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="plhAssignedUser" runat="server" Visible="false">
                <div class="data_table">
                    <h4>
                        <%= base.GetText("textAssignedUser")%></h4>
                    <div class="data_grid">
                        <table cellpadding="2'" cellspacing="0">
                            <asp:Repeater ID="rptUsers" runat="server" OnItemCommand="rptUsers_ItemCommand">
                                <HeaderTemplate>
                                    <tr>
                                        <th>
                                            Username</th>
                                        <th>
                                            Full name</th>
                                        <th>
                                            Email</th>
                                        <th>
                                        </th>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "Username") %>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "Fullname") %>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "Email") %>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imageButtonDelete" ToolTip='Delete' ImageUrl="../Images/delete_file.gif"
                                                AlternateText='Delete' ImageAlign="AbsMiddle" CssClass="image_button16" CommandName="Delete"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' /></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>
                <asp:UserSelector ID="userSelector" runat="server">
                </asp:UserSelector>
                <asp:Button ID="btnAdd" runat="server" CssClass="button" Text="Assign" OnClick="btnAdd_Click" />
            </asp:PlaceHolder>
            <div class="data_table">
                <h4>
                    Booker (contacts) list</h4>
                <div class="data_grid">
                    <table>
                        <tr>
                            <th>
                                Name</th>
                            <th>
                                Phone</th>
                            <th>
                                Email</th>
                            <th>
                                Enabled</th>
                            <th>
                                Action</th>
                        </tr>
                        <asp:Repeater ID="rptContacts" runat="server" OnItemDataBound="rptContacts_ItemDataBound"
                            OnItemCommand="rptContacts_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "Name") %>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "Phone") %>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "Email") %>
                                    </td>
                                    <td>
                                        <asp:Literal ID="litEnabled" runat="server"></asp:Literal></td>
                                    <td>
                                        <input id="btnEdit" runat="server" type="button" value="Edit" class="button" />
                                        <asp:Button ID="btnEnabled" runat="server" CssClass="button" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
                <input id="btnAddContact" runat="server" type="button" value="Add contact" class="button" />
            </div>
        </div>
    </fieldset>
</asp:Content>
