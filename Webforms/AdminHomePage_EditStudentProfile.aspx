<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Webforms/AdminSidePanel.Master" CodeBehind="AdminHomePage_EditStudentProfile.aspx.vb" Inherits="OnlineViewingOfGrades.AdminHomePage_EditStudentProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="AdminControlPlaceHolder" runat="server">
    
    <table style="width:100%;">
        <tr>
            <td class="auto-style4">
                <asp:Label ID="SelectedAccountsLabel" runat="server" Text="Selected Accounts:"></asp:Label>
                <br />
                <asp:ListBox ID="SelectedAccountsListBox" runat="server" Height="126px" Width="234px"></asp:ListBox>
            </td>
            <td>
                <asp:Label ID="SetEmailLabel" runat="server" Text="Set Email:"></asp:Label>
                <br />
                <asp:RadioButton ID="NoChangeEmailRadioButton" runat="server" GroupName="SetEmailMode" Text="No Change" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:RadioButton ID="SetManuallyEmailRadioButton" runat="server" GroupName="SetEmailMode" Text="Set Manually" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <br />
                <br />
                <asp:Label ID="ManualSetEmailLabel" runat="server" Text="Manual Set Email:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="ManualSetEmailField" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <br />
                <asp:Label ID="ErrorInEmailLabel" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                <br />
                --------------------<br />
                <br />
                <asp:Label ID="SetPasswordLabel" runat="server" Text="Set Password:"></asp:Label>
                <br />
                <asp:RadioButton ID="NoChangePasswordRadioButton" runat="server" GroupName="SetPasswordMode" Text="No Change" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:RadioButton ID="SetManuallyPasswordRadioButton" runat="server" GroupName="SetPasswordMode" Text="Set Manually" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:RadioButton ID="AutoGeneratePasswordRadioButton" runat="server" GroupName="SetPasswordMode" Text="Auto Generate" />
                <br />
                <br />
                <asp:Label ID="ManualSetPasswordLabel" runat="server" Text="Manual Set Password:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="ManualSetPasswordField" runat="server" TextMode="Password"></asp:TextBox>
                <br />
                <asp:Label ID="ConfirmPasswordLabel" runat="server" Text="Confirm Password:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="ConfirmPasswordField" runat="server" TextMode="Password"></asp:TextBox>
                <br />
                <asp:Label ID="ErrorInPasswordLabel" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                <br />
                --------------------<br />
                <br />
                <asp:Button ID="ConfirmChangesButton" runat="server" Text="Confirm Changes" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="CancelButton" runat="server" Text="Cancel" />
            </td>
        </tr>
    </table>
</asp:Content>
