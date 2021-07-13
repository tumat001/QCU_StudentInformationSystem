<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Webforms/AdminSidePanel.Master" CodeBehind="AdminHomePage_EditAdminProfile.aspx.vb" Inherits="OnlineViewingOfGrades.AdminHomePage_EditAdminProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style4 {
            width: 241px;
        }
    </style>
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
                <asp:Label ID="PrivilageModeLabel" runat="server" Text="Set Privilage Mode:"></asp:Label>
                <br />
                <asp:DropDownList ID="PrivilageModeDropDownList" runat="server" Height="16px" Width="165px">
                </asp:DropDownList>
                <br />
                <br />
                --------------------<br />
                <br />
                <asp:Label ID="SetPasswordLabel" runat="server" Text="Set Password:"></asp:Label>
                <br />
                <asp:RadioButton ID="NoChangeRadioButton" runat="server" GroupName="SetPasswordMode" Text="No Change" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:RadioButton ID="SetManuallyRadioButton" runat="server" GroupName="SetPasswordMode" Text="Set Manually" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:RadioButton ID="AutoGenerateRadioButton" runat="server" GroupName="SetPasswordMode" Text="Auto Generate" />
                <br />
                <br />
                <asp:Label ID="ManualSetPasswordLabel" runat="server" Text="Manual Set Password:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="ManualSetPasswordField" runat="server" TextMode="Password"></asp:TextBox>
                &nbsp;
                <br />
                <asp:Label ID="ConfirmPasswordLabel" runat="server" Text="Confirm Password:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="ConfirmPasswordField" runat="server" TextMode="Password"></asp:TextBox>
                &nbsp;
                <br />
                <br />
                --------------------<br />
                <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red" Visible="False"></asp:Label>
    <asp:Label ID="SuccessLabel" runat="server" ForeColor="#006600" Text="Account(s) edited successfully" Visible="False"></asp:Label>
                <br />
                <br />
                <asp:Button ID="ConfirmChangesButton" runat="server" Text="Confirm Changes" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="CancelButton" runat="server" Text="Cancel" />
            </td>
        </tr>
    </table>

</asp:Content>
