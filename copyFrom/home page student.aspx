<%@ Page Title="" Language="C#" MasterPageFile="~/webpages/Site1.Master" AutoEventWireup="true" CodeBehind="home page student.aspx.cs" Inherits="Online_grades.webpages.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <br /><br />


    <div style="background-color:lightblue;", class="centralize">
        <h2>Select Sy code &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Select Term</h2>
    </div>
    

    <div>
        <table style="width:90%; background-color:lightblue; margin:auto;">
            <tr> 
                <td rowspan="2">
                    <p>Home</p>
                    <p>Edit Information</p>
                    <p>Create Copy of grade slip</p>
                    <p>Logout</p>
                </td><td>
                                        <table class="studNameDiv">
            <tr> <td colspan="3"><h2>Sample student  Surname, First name , Middle name</h2></td> <td>&nbsp</td><td>
                <asp:DropDownList ID="DropDownList2" runat="server"></asp:DropDownList>
                                                                                              </td>       
            <tr>
                <td> <h3>&nbsp&nbsp&nbsp(Section/ Year)</h3></td><td> <h3> (Campus)</h3></td><td> <h3>(School year)</h3></td>
            </tr>
            </table>
                                     </td>

            </tr>

             <tr><td><br /><br /></td></tr>


            <tr><td></td>
                <td>
                    <table class="table1">
            <tr>
                <th>Subject</th><th>Description</th><th>Units</th><th>Grade</th><th>Instructor</th>
            </tr>
            <tr>
                <td>CC101</td><td>Data Structure Algorithms</td><td>2</td><td>1.0</td><td>instructor name1</td>
            </tr>
            <tr>
                <td>NEt102</td><td>Networking</td><td>2</td><td>1.25</td><td>instructor name2</td>
            </tr>
            <tr>
                <td>GS50</td><td>Information System</td><td>2</td><td>1.50</td><td>instructor name3</td>
            </tr>

        </table>


                </td>
            </tr>
        </table>



    </div>





   



</asp:Content>
