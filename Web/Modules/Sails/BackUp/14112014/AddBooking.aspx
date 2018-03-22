<%@ Page Language="C#" MasterPageFile="SailsMaster.Master" AutoEventWireup="True"
    Codebehind="AddBooking.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.AddBooking"
    Title="Untitled Page" %>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="Portal.Modules.OrientalSails" Namespace="Portal.Modules.OrientalSails.Web.Controls" TagPrefix="orc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <script type="text/javascript">
    function toggleVisible(id)
    {
        item = document.getElementById(id);
        if (item.style.display=="")
        {
            item.style.display="none";
        }
        else
        {
            item.style.display = "";
        }
    }    
    
    function setVisible(id, visible)
    {
        control = document.getElementById(id);
        if (visible)
        {control.style.display = "";}
        else
        {
        control.style.display = "none";
        }
        
    }
    
    function ddltype_changed(id, optionid, vids)
    {
        ddltype = document.getElementById(id);
        if (vids.indexOf('#' + ddltype.options[ddltype.selectedIndex].value + '#') >= 0)
        {
            setVisible(optionid, true);
        }
        else
        {
            setVisible(optionid, false);
        }
    }
    
    function ddlagency_changed(id, codeid)
    {
        ddltype = document.getElementById(id);
        switch (ddltype.selectedIndex)
        {
            case 0:
                setVisible(codeid, false);
            break;
            default:
                setVisible(codeid, true);
            break;
        }
    }
    </script>

    <fieldset>
        <legend>
            <img alt="Room" src="../Images/sails.gif"/>
            <%= base.GetText("textAddBooking") %>            
        </legend>
        <div>
            <div class="basicinfo">
                <table>
                    <tr>
                        <asp:PlaceHolder ID="plhTrip" runat="server">
                        <td>
                            <%= base.GetText("textTrip") %>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlOrgs" runat="server" Width="80"></asp:DropDownList>
                            <svc:CascadingDropDown runat="server" ID="cddlTrips" Width="110"/>
                            <asp:DropDownList ID="ddlTrips" runat="server" Width="110" Visible="false">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlOptions" runat="server" Width="80" Style="display: none;">
                                <asp:ListItem>Option 1</asp:ListItem>
                                <asp:ListItem>Option 2</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        </asp:PlaceHolder>
                        <td>
                            <%= base.GetText("textStartDate") %>
                            *</td>
                        <td>
                            <asp:TextBox ID="txtDate" runat="server"></asp:TextBox><ajax:CalendarExtender ID="calendarDate"
                                runat="server" TargetControlID="txtDate" Format="dd/MM/yyyy">
                            </ajax:CalendarExtender>
                        </td>
                        <asp:PlaceHolder ID="plhEndDate" runat="server">
                        <td>
                            End date
                            *</td>
                        <td>
                            <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox><ajax:CalendarExtender ID="calenderEnd"
                                runat="server" TargetControlID="txtEndDate" Format="dd/MM/yyyy">
                            </ajax:CalendarExtender>
                        </td>
                        </asp:PlaceHolder>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>Number of pax</td>
                        <td colspan= "3">
                            Adult<asp:DropDownList ID="ddlAdult" runat="server" Width="80" style="text-align:right;"></asp:DropDownList>
                            Child<asp:DropDownList ID="ddlChild" runat="server" Width="80" style="text-align:right;"></asp:DropDownList>
                            Baby<asp:DropDownList ID="ddlBaby" runat="server" Width="80" style="text-align:right;"></asp:DropDownList>
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                            <%= base.GetText("textAgency") %>
                        </td>
                        <td>                       
                            <orc:AgencySelector ID="agencySelector" runat="server" />
                            <asp:TextBox ID="txtAgencyCode" runat="server" Width="75"></asp:TextBox>
                            <ajax:TextBoxWatermarkExtender ID="waterAgency" runat="server" TargetControlID="txtAgencyCode"
                                WatermarkText="TA Code">
                            </ajax:TextBoxWatermarkExtender>
                        </td>
                        <td colspan="2">
                            <asp:Repeater ID="rptExtraServices" runat="server" OnItemDataBound="rptExtraServices_ItemDataBound">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "Name") %>
                                    <asp:CheckBox ID="chkService" runat="server" CssClass="checkbox" />
                                    <asp:HiddenField ID="hiddenId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "Id") %>' />
                                    <asp:HiddenField ID="hiddenValue" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "Price") %>' />
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="button" />
        </div>
    </fieldset>
</asp:Content>
