<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Webforms/StudentSidePanel.Master" CodeBehind="StudentHomePage_EditProfileContent.aspx.vb" Inherits="OnlineViewingOfGrades.StudentHomePage_EditProfileContent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="StudentControlPlaceHolder" runat="server">

    <div>
        <asp:Label ID="InsLabel01" runat="server" Font-Bold="True" Font-Size="Large" Text="Change Password" Width="640px"></asp:Label>
        <br />
        <br />
        <asp:Label ID="CurrentPasswordLabel" runat="server" Text="Current Password" Width="180px"></asp:Label>
        <br />
        <asp:TextBox ID="CurrentPasswordField" runat="server" TextMode="Password"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:RequiredFieldValidator ID="RequiredOldPasswordValidator" runat="server" ErrorMessage="This field is required" ForeColor="Red" Font-Size="Small" ControlToValidate="CurrentPasswordField" ValidationGroup="ChangePassword"></asp:RequiredFieldValidator>
        <br />
        <br />
        <asp:Label ID="NewPasswordLabel" runat="server" Text="New Password" Width="180px"></asp:Label>
        <br />
        <asp:TextBox ID="NewPasswordField" runat="server" TextMode="Password"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:RequiredFieldValidator ID="RequiredNewPasswordValidator" runat="server" ErrorMessage="This field is required" ForeColor="Red" Font-Size="Small" ControlToValidate="NewPasswordField" ValidationGroup="ChangePassword"></asp:RequiredFieldValidator>
        <br />
        <asp:Label ID="ConfirmPasswordLabel" runat="server" Text="Confirm Password" Width="180px"></asp:Label>
        <br />
        <asp:TextBox ID="ConfirmPasswordField" runat="server" TextMode="Password"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:RequiredFieldValidator ID="RequiredConfirmPasswordValidator" runat="server" ErrorMessage="This field is required" ForeColor="Red" Font-Size="Small" ControlToValidate="ConfirmPasswordField" ValidationGroup="ChangePassword"></asp:RequiredFieldValidator>
        <br />
        <br />
        <asp:CheckBox ID="ShowNewPasswordCheckBox" runat="server" Text="Show New Passwords" Width="180px" />
        <br />
        <br />
        <asp:Label ID="CustomErrorLabel" runat="server" Font-Size="Small" ForeColor="Red" Visible="False" Width="240px"></asp:Label>
        <br />
        <br />
        <asp:Button ID="ChangePasswordButton" runat="server" Text="Change Password" ValidationGroup="ChangePassword" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="CancelButton" runat="server" Text="Cancel" />
        <br />
        <br />
    </div>

</asp:Content>
