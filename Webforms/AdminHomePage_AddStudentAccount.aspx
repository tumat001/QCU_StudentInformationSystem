<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Webforms/AdminSidePanel.Master" CodeBehind="AdminHomePage_AddStudentAccount.aspx.vb" Inherits="OnlineViewingOfGrades.AdminHomePage_AddStudentAccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="AdminControlPlaceHolder" runat="server">
    <asp:Label ID="LabelTitle" runat="server" Font-Bold="True" Font-Size="Large" Text="Add Student Account"></asp:Label>
    <br />
    <br />
    <asp:Label ID="UsernameLabel" runat="server" Text="Username:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="UsernameField" runat="server" Width="128px" AutoCompleteType="Disabled"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:RequiredFieldValidator ID="UsernameRequiredValidator" runat="server" ErrorMessage="Field is required" Font-Bold="False" Font-Size="Small" ForeColor="Red" ControlToValidate="UsernameField" ValidationGroup="AddStudent"></asp:RequiredFieldValidator>
    <br />
    <br />
    <asp:Label ID="PasswordLabel" runat="server" Text="Password:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="PasswordField" runat="server" TextMode="Password"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;
    <br />
    <asp:Label ID="ConfirmPasswordLabel" runat="server" Text="Confirm Password:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="ConfirmPasswordField" runat="server" TextMode="Password"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;
    <br />
    <asp:CheckBox ID="GeneratePasswordCheckbox" runat="server" Text="Generate Password Instead" />
    <br />
    <br />
    <asp:Label ID="EmailLabel" runat="server" Text="Email:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="EmailField" runat="server" Width="250px" AutoCompleteType="Disabled"></asp:TextBox>
&nbsp;&nbsp;&nbsp;
    <asp:RequiredFieldValidator ID="EmailRequiredValidator" runat="server" ErrorMessage="Field is required" Font-Bold="False" Font-Size="Small" ForeColor="Red" ControlToValidate="EmailField" ValidationGroup="AddStudent"></asp:RequiredFieldValidator>
    <br />
    <br />
    <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red" Visible="False"></asp:Label>
    <asp:Label ID="SuccessLabel" runat="server" ForeColor="#006600" Text="Account added successfully" Visible="False"></asp:Label>
    <br />
    <br />
    <asp:Button ID="AddStudentButton" runat="server" Text="Add Student" ValidationGroup="AddStudent" />
    <br />
</asp:Content>
