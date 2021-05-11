<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TestSendEmailToTargetPage.aspx.vb" Inherits="OnlineViewingOfGrades.TestSendEmailToTargetPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="EmailOfTargetLabel" runat="server" Text="Email of Target"></asp:Label>
&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="EmailOfTargetField" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="SendButton" runat="server" Text="Send" />
        </div>
    </form>
</body>
</html>
