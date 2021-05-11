<%@ Page Title="" Language="C#" MasterPageFile="~/copyFrom/Site1.Master" AutoEventWireup="true" CodeBehind="homepage.aspx.cs" Inherits="Online_grades.webpages.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <div class="content, centralize">
        <p></p>
        <table style="margin-left:auto; margin-right:auto ; margin-top:10%; margin-bottom:auto" >
            <tr><td>
                    <img src="../images/QCU_Logo_2019.png" height="300" width="300"/>
                </td><td>&nbsp&nbsp&nbsp&nbsp</td><td>&nbsp&nbsp&nbsp&nbsp</td>
                <td>
                    <table>
                        <tr>
                            <td> Email address: </td><td><asp:TextBox ID="TextBox1" runat="server" Width="190px"></asp:TextBox> </td>
                        </tr>
                        <tr>
                            <td> Password:</td><td><asp:TextBox ID="TextBox2" runat="server" Width="190px"></asp:TextBox> </td><td> <asp:CheckBox ID="CheckBox1" runat="server" /></td><td>show password</td>
                        </tr>
                        <tr>
                            <td></td><td>
                                <input id="Checkbox1" type="checkbox" /> Remember login</td>
                        </tr>
                        <tr>
                            <td>&nbsp</td>
                        </tr>


                        <tr>
                            <td></td><td><asp:Button ID="Button1" runat="server" Text="Login" Width="143px" PostBackUrl="~/webpages/home page login.aspx" /></td>
                        </tr>
                        <tr>
                            <td></td><td>  
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/webpages/home page login.aspx">Forgot password?</asp:HyperLink>
                                     </td>
                        </tr>
                    </table>
                </td></tr>
        </table>
        
    </div>









</asp:Content>
