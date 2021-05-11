<%@ Page Title="" Language="vb" MasterPageFile="~/copyFrom/Site1.Master" AutoEventWireup="true" CodeBehind="Edit Information.aspx.cs" Inherits="Online_grades.webpages.WebForm4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style2 {
            width: 201px;
        }
        .auto-style3 {
            width: 255px;
        }
        .auto-style4 {
            width: 33%;
            height: 158px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    








    <table class="auto-style4" style="color:orange;">
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style3">&nbsp;</td>
            
        </tr>
        <tr>
            <td class="auto-style2">Name</td>
            <td class="auto-style3">
                <asp:TextBox ID="TextBox1" runat="server" Width="173px"></asp:TextBox>
            </td>
            
        </tr>
        <tr>
            <td class="auto-style2">new Password</td>
            <td class="auto-style3">
                <asp:TextBox ID="TextBox2" runat="server" Width="172px"></asp:TextBox>
            </td>
            
        </tr>
        <tr>
            <td class="auto-style2">Confirm password</td>
            <td class="auto-style3">
                <asp:TextBox ID="TextBox3" runat="server" Width="170px"></asp:TextBox>
            </td>
            
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style3">&nbsp;</td>
            
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style3">
                <asp:Button ID="Button1" runat="server" Height="32px" Text="Confirm" Width="118px" />
            </td>
           
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style3">&nbsp;</td>
            

        </tr>
    </table>


    








</asp:Content>
