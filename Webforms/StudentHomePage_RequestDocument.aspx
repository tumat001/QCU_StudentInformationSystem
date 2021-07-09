<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Webforms/StudentSidePanel.Master" CodeBehind="StudentHomePage_RequestDocument.aspx.vb" Inherits="OnlineViewingOfGrades.StudentHomePage_RequestGradeSlip" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="StudentControlPlaceHolder" runat="server">
    <br />
    <asp:Label ID="LabelTitle01" runat="server" Font-Size="X-Large" Text="Request For Document"></asp:Label>
    <br />
    <br />
&nbsp;&nbsp;&nbsp;
    <asp:Label ID="LabelDocument" runat="server" Text="Document:"></asp:Label>
&nbsp;&nbsp;&nbsp;
    <asp:DropDownList ID="DocumentDropDownList" runat="server" Height="23px" Width="257px">
    </asp:DropDownList>
    <br />
    <br />
&nbsp;&nbsp;
    <asp:Label ID="EmailLabel" runat="server" Text="Email:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="EmailTextbox" runat="server" Enabled="False" Width="269px"></asp:TextBox>
    <br />
    <br />
&nbsp;&nbsp;
    <asp:Label ID="PurposeLabel" runat="server" Text="Purpose:"></asp:Label>
&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="TextBox1" runat="server" Rows="3" TextMode="MultiLine" Width="341px"></asp:TextBox>
    <br />
    <br />
    <br />
&nbsp;&nbsp;
    <asp:Button ID="SendRequestButton" runat="server" Text="Send Request Button" />
    <br />
&nbsp;&nbsp;&nbsp;
    <asp:Label ID="LabelResultStatus" runat="server" ForeColor="Red" Text="Label" Visible="False"></asp:Label>
    <br />
    <br />
</asp:Content>
