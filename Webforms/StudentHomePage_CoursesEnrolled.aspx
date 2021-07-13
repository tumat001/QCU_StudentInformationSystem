<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Webforms/StudentSidePanel.Master" CodeBehind="StudentHomePage_CoursesEnrolled.aspx.vb" Inherits="OnlineViewingOfGrades.StudentHomePage_CoursesEnrolled" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="StudentControlPlaceHolder" runat="server">

    

    <table style="width:90%; background-color:lightblue; margin:auto;">
        <tr>
            <td>
                <table class="studNameDiv">
                    <tr>
                        <td>
                            <h2>
                                <asp:Label ID="StudentNameLabel" runat="server"></asp:Label>
                            </h2>
                        </td>
                        <td>&nbsp</td>
                        <td>
                            &nbsp;</td>
                    </table>
            </td>
        </tr>
        <tr>
            <td>
                <br />
                <br /></td>
        </tr>
        <tr>
            <td>
                <div>
                    <asp:GridView ID="StudentCoursesGridView" runat="server" AutoGenerateColumns="False" Width="90%" BackColor="White">
                        <Columns>
                            <asp:BoundField DataField="Course" HeaderText="Course">
                            </asp:BoundField>
                            <asp:BoundField DataField="School Year" HeaderText="School Year" />
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <br />
                <asp:Label ID="NoCoursesToDisplayLabel" runat="server" Text="No Courses To Display" Visible="False"></asp:Label>
            <br /></td>
        </tr>
    </table>

    

</asp:Content>
