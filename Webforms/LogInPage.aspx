<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LogInPage.aspx.vb" Inherits="OnlineViewingOfGrades.LogInPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        body{
            background-image:url("../images/photo_backgrounds_textures_app05.jpg");
            background-size:100%;
            background-repeat: no-repeat;
            
        }
        .navbar{
            flex-wrap: wrap;        
            width:auto;  
            background-color: saddlebrown;
            text-align:center;
            opacity:0.8;
        }
        .navbar-right{
            text-align-last:right;
            display: flex; 
            justify-content: flex-end
        }
        .content{
            display:flex;
            flex-wrap:wrap;
            align-items:center;
            height:100%;
        }
        .centralize{
            width:90%;
            margin:auto;
        }       
        .table1{
            
            background-color: azure;      
            flex-wrap:wrap;
            width:100%;
            align-self:center;
            align-content:center;
            font-size:20px;
            border: 2px solid black; 
           
        }
        .table1 tr:nth-child(even) td {
            background-color: beige;
        }
        .gradetable1:nth-child(even) {background: #CCC}
        .studNameDiv{
            background-color:azure;
            flex-wrap:wrap;
            align-content:center;
            width:100%;

        }
        .auto-style1 {
            width: 185px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div class="navbar">
            <div ><br /><h1>QCU Student Information System</h1>

            </div>
        </div>

        <div class="content, centralize">
        <table style="margin-left:auto; margin-right:auto ; margin-top:10%; margin-bottom:auto" >
            <tr><td>
                    <img src="../images/QCU_Logo_2019.png" height="300" width="300"/>
                </td><td>&nbsp&nbsp&nbsp&nbsp</td><td>&nbsp&nbsp&nbsp&nbsp</td>
                <td>
                    <table>
                        <tr>
                            <td> Username:</td><td><asp:TextBox ID="UsernameField" runat="server" Width="190px"></asp:TextBox> </td><td class="auto-style1">
                            <asp:RequiredFieldValidator ID="RequiredUsernameValidator" runat="server" ControlToValidate="UsernameField" ErrorMessage="Username is required" Font-Size="Small" ForeColor="Red" Visible="False"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td> Password:</td><td><asp:TextBox ID="PasswordField" runat="server" Width="190px" TextMode="Password"></asp:TextBox> </td><td class="auto-style1">
                            <asp:RequiredFieldValidator ID="RequiredPasswordValidator" runat="server" ControlToValidate="PasswordField" ErrorMessage="Password  is required" Font-Size="Small" ForeColor="Red" Visible="False"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td></td><td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td></td>
                        </tr>


                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Label ID="IncorrectInputLabel" runat="server" Font-Size="Small" ForeColor="Red" Text="Incorrect username or password" Visible="False"></asp:Label>
                            </td>
                        </tr>


                        <tr>
                            <td></td><td><asp:Button ID="LogInButton" runat="server" Text="Log In" Width="143px" /></td>
                        </tr>
                        <tr>
                            <td></td><td>  
                                <asp:LinkButton ID="ForgotPasswordButton" runat="server">Forgot Password?</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td></tr>
        </table>
        
    </div>
    </form>
</body>
</html>
