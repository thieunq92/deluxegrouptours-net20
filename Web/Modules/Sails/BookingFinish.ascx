<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BookingFinish.ascx.cs" Inherits="Portal.Modules.OrientalSails.Web.BookingFinish" %>
<table class="price_table" cellspacing="1">
    <tr>
        <th>Room</th>
        <th>People</th>
<%--        <th>Price</th>
        <th>Extra service</th>
        <th>Total</th>--%>
    </tr>
<asp:Repeater ID="rptRooms" runat="server" OnItemDataBound="rptRooms_ItemDataBound">
    <ItemTemplate>
        <tr>
            <th style="text-align:left"><asp:Literal ID="litRoom" runat="server"></asp:Literal></th>
            <td><asp:Literal ID="litPeople" runat="server"></asp:Literal></td>            
<%--            <td>$<asp:Literal ID="litPrice" runat="server"></asp:Literal></td>
            <td>$<asp:Literal ID="litService" runat="server"></asp:Literal></td>
            <td>$<asp:Literal ID="litTotal" runat="server"></asp:Literal></td>--%>
        </tr>
    </ItemTemplate>
</asp:Repeater>
    <tr>
        <th colspan="4" style="text-align:left">GRAND TOTAL</th>
        <th style="text-align:right"><strong>$<asp:Label ID="labelTotal" runat="server"></asp:Label></strong></th>
    </tr>
</table>
<asp:Button ID="btnConfirm" runat="server" Text="Confirm" OnClick="btnConfirm_Click" />