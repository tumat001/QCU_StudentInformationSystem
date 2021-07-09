<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Webforms/AdminSidePanel.Master" CodeBehind="AdminHomePage_RequestableDocumentEditPage.aspx.vb" Inherits="OnlineViewingOfGrades.AdminHomePage_RequestableDocumentEditPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="AdminControlPlaceHolder" runat="server">
    <p>
&nbsp;&nbsp;&nbsp; &nbsp;<asp:Label ID="LabelTitle" runat="server" Font-Size="X-Large" Text="Requestable Document List"></asp:Label>
    </p>
&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:ListBox ID="ReqDocumentListBox" runat="server" Height="102px" Width="186px"></asp:ListBox>
    <br />
    <br />
&nbsp;&nbsp;&nbsp;
    <asp:Label ID="AddDocumentLabel" runat="server" Text="Add Document:"></asp:Label>
&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="AddDocumentField" runat="server" AutoCompleteType="Disabled" Width="194px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;
    <asp:Button ID="AddDocumentButton" runat="server" Text="Add Document" />
    <br />
    <br />
&nbsp;&nbsp;&nbsp;
    <asp:Label ID="Label1" runat="server" Text="Update Selected Document:"></asp:Label>
&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="UpdateSelectedDocumentField" runat="server" AutoCompleteType="Disabled" Width="202px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;
    <asp:Button ID="UpdateSelectedDocumentButton" runat="server" Text="Update Selected Docu" />
    <br />
    <br />
&nbsp;&nbsp;&nbsp;
    <asp:Button ID="DeleteSelectedDocument" runat="server" Text="Delete Selected Document" />
    <br />
    <br />
&nbsp;&nbsp;&nbsp; -----------------<br />
    <br />
&nbsp;&nbsp;&nbsp;
    <asp:Button ID="FinalizeChangesButton" runat="server" Text="Finalize Changes" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="CancelButton" runat="server" Text="Cancel" />
    <br />
</asp:Content>
