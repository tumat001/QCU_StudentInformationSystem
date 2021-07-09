<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Webforms/AdminSidePanel.Master" CodeBehind="AdminHomePage_BackupPage.aspx.vb" Inherits="OnlineViewingOfGrades.AdminHomePage_BackupPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="AdminControlPlaceHolder" runat="server">
&nbsp;
    <asp:Label ID="LabelBackup" runat="server" Font-Size="X-Large" Text="Accounts Database Backup"></asp:Label>
    <br />
    <br />
&nbsp;&nbsp;&nbsp;
    <asp:Button ID="CreateBackupButton" runat="server" Text="Create Backup File" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="LoadBackupButton" runat="server" Text="Load Backup File" />
    <br />
    <br />
</asp:Content>
