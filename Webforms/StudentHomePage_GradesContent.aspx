<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Webforms/StudentSidePanel.Master" CodeBehind="StudentHomePage_GradesContent.aspx.vb" Inherits="OnlineViewingOfGrades.StudentHomePage_GradesContent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="StudentControlPlaceHolder" runat="server">

    

    <table style="width:90%; background-color:lightblue; margin:auto;">
        <tr>
            <td>
                <table class="studNameDiv">
                    <tr>
                        <td colspan="3">
                            <h2>
                                <asp:Label ID="StudentNameLabel" runat="server"></asp:Label>
                            </h2>
                        </td>
                        <td>&nbsp</td>
                        <td>
                            <asp:DropDownList ID="YearDropDownList" runat="server">
                            </asp:DropDownList>
                        </td>
                    <tr>
                        <td>
                            <h3>
                                <asp:Label ID="CampusLabel" runat="server"></asp:Label>
                            </h3>
                        </td>
                        <td>
                            <h3>
                                <asp:Label ID="SchoolYearLabel" runat="server"></asp:Label>
                            </h3>
                        </td>            
                        <td>
                            <h3>
                                <asp:Label ID="TermLabel" runat="server" Font-Bold="True"></asp:Label>
                            </h3>
                        </td>    
                    </tr>
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
                    <asp:GridView ID="StudentGradeInfoGridView" runat="server" AutoGenerateColumns="False" Width="90%" BackColor="White">
                        <Columns>
                            <asp:BoundField DataField="SubCode" HeaderText="Sub Code">
                            </asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="Description">
                            </asp:BoundField>
                            <asp:BoundField DataField="Units" HeaderText="Units">
                            </asp:BoundField>
                            <asp:BoundField DataField="Grade" HeaderText="Grade">
                            </asp:BoundField>
                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                            <asp:BoundField DataField="Professor" HeaderText="Professor">
                            </asp:BoundField>
                            <asp:BoundField DataField="Class" HeaderText="Class" />
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <br />
                <asp:LinkButton ID="DownloadGradesButton" runat="server">Download grades as image</asp:LinkButton>
            <br /></td>
        </tr>
    </table>

    

</asp:Content>
