<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Webforms/AdminSidePanel.Master" CodeBehind="AdminHomePage_ViewStudentTableContent.aspx.vb" Inherits="OnlineViewingOfGrades.AdminHomePage_ViewStudentTableContent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="AdminControlPlaceHolder" runat="server">

    <div style="float:right">
        <asp:Label ID="Label1" runat="server" Text="search:"></asp:Label>
        &nbsp;&nbsp;&nbsp;
        <asp:Panel runat="server" DefaultButton="SearchNameButton">
            <asp:TextBox ID="SearchField" runat="server"></asp:TextBox>
            <asp:Button ID="SearchNameButton" runat="server" style="display:none" OnClick="SearchNameButton_Click" />
        </asp:Panel>
    </div>
    <br /><br /><br />
    <div>
        <asp:GridView ID="StudentGridView" runat="server" AutoGenerateColumns="False" Width="600px" AllowPaging="True" OnPageIndexChanged="StudentGridView_PageIndexChanged" OnPageIndexChanging="StudentGridView_PageIndexChanging">
            <Columns>
                <asp:BoundField HeaderText="Username" DataField="Username" />
                <asp:BoundField HeaderText="Email Address" DataField="EmailAddress" />
                
                <asp:CheckBoxField DataField="Selected" HeaderText="Selected"/>
                
            </Columns>
            <PagerSettings Mode="NumericFirstLast" />
        </asp:GridView>
        <br />
    </div>
    <div>

        <asp:Button ID="UnselectAllButton" runat="server" Text="Unselect All" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="SelectAllButton" runat="server" Text="Select All" />

        <br />

        <br />
        <asp:Button ID="CreateStudentButton" runat="server" Text="Create Student Account" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="SyncAccountsButton" runat="server" Text="Sync Accounts From MIS" />
        <br />
        <br />
        <asp:Label ID="LabelIns02" runat="server" Text="Perform Action to Selected:"></asp:Label>
        <br />
        <br />
        <asp:Button ID="EditPropertiesButton" runat="server" Text="Edit Properties" />

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <br />

    </div>
    

</asp:Content>
