<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Webforms/AdminSidePanel.Master" CodeBehind="AdminHomePage_AddAdminAccount.aspx.vb" Inherits="OnlineViewingOfGrades.AdminHomePage_AddAdminAccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="AdminControlPlaceHolder" runat="server">
    <asp:Label ID="LabelTitle" runat="server" Font-Bold="True" Font-Size="Large" Text="Add Admin Account"></asp:Label>
    <br />
    <br />
    <asp:Label ID="UsernameLabel" runat="server" Text="Username:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="UsernameField" runat="server" Width="128px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:RequiredFieldValidator ID="UsernameRequiredValidator" runat="server" ErrorMessage="Field is required" Font-Bold="False" Font-Size="Small" ForeColor="Red" ControlToValidate="UsernameField" ValidationGroup="AddAdmin"></asp:RequiredFieldValidator>
    <br />
    <br />
    <asp:Label ID="PasswordLabel" runat="server" Text="Password:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="PasswordField" runat="server" TextMode="Password"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:RequiredFieldValidator ID="PasswordRequiredValidator" runat="server" ErrorMessage="Field is required" Font-Bold="False" Font-Size="Small" ForeColor="Red" ControlToValidate="PasswordField" ValidationGroup="AddAdmin"></asp:RequiredFieldValidator>
    <br />
    <asp:Label ID="ConfirmPasswordLabel" runat="server" Text="Confirm Password:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="ConfirmPasswordField" runat="server" TextMode="Password"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:RequiredFieldValidator ID="ConfirmPasswordRequiredValidator" runat="server" ErrorMessage="Field is required" Font-Bold="False" Font-Size="Small" ForeColor="Red" ControlToValidate="ConfirmPasswordField" ValidationGroup="AddAdmin"></asp:RequiredFieldValidator>
    <br />
    <asp:CheckBox ID="GeneratePasswordCheckbox" runat="server" Text="Generate Password Instead" />
    <br />
    <br />
    <asp:Label ID="PrivilageLabel" runat="server" Text="Privilage Mode:"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:DropDownList ID="PrivilageModeDropDownList" runat="server" Height="16px" Width="134px">
    </asp:DropDownList>
&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:RequiredFieldValidator ID="PrivilageModeRequiredValidator" runat="server" ErrorMessage="Field is required" Font-Bold="False" Font-Size="Small" ForeColor="Red" ControlToValidate="PrivilageModeDropDownList" ValidationGroup="AddAdmin"></asp:RequiredFieldValidator>
    <br />
    <br />
    <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red" Visible="False"></asp:Label>
    <br />
    <br />
    <asp:Button ID="AddAdminButton" runat="server" Text="Add Admin" ValidationGroup="AddAdmin" />
</asp:Content>
