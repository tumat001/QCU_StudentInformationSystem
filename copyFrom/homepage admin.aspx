<%@ Page Title="" Language="C#" MasterPageFile="~/copyFrom/Site1.Master" AutoEventWireup="true" CodeBehind="homepage admin.aspx.cs" Inherits="Online_grades.webpages.WebForm3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
       
       
        .auto-style1 {
            width: 635px;
            background-color: azure;
        }
        .auto-style3 {
            width: 10px;
            height: 93px;
        }
        .auto-style4 {
            width: 215px;
            height: 192px;
            background-color: azure;
            border: 3px solid gray;
        }
        .auto-style5 {
            width: 54px;
        }
        .auto-style6 {
            width: 215px;
            height: 104px;
            background-color:beige;
            border: 3px solid gray;
        }
       
       
        .auto-style7 {
            width: 292px;
        }
       
       
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">





    <div  class="centralize">

    <table style="width:100%">
        <tr>
            <td colspan="3" class="auto-style3"></td>
            
           
        </tr>
        <tr>
            <td class="auto-style4">
               <h4>COURSES</h4>
               <ul>
                   <li> <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">Information Technology</asp:LinkButton> </li>
                   <li> <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click">Industrial Engineering</asp:LinkButton></li>
                   <li> <asp:LinkButton ID="LinkButton3" runat="server" OnClick="LinkButton3_Click">Accounting</asp:LinkButton> </li>
                   <li> <asp:LinkButton ID="LinkButton4" runat="server" OnClick="LinkButton4_Click">Electronics Engineering</asp:LinkButton>  </li>
               </ul>


            </td>
            <td rowspan="4" class="auto-style5"></td>
            <td rowspan="4" class="auto-style1" >


                <div> <table class="auto-style7"><tr>
                    <td> 
                        <asp:Button ID="Button1" runat="server" Text="1st year" Visible="False" />
                    </td>
                    <td> 
                        <asp:Button ID="Button2" runat="server" Text="2nd year" Visible="False" />
                    </td>
                    <td> 
                        <asp:Button ID="Button3" runat="server" Text="3rd year" Visible="False" />
                    </td>
                    <td> 
                        <asp:Button ID="Button4" runat="server" Text="4th year" Visible="False" />
                    </td>
                             </tr>
                    
                      </table> </div>

                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Search by ID" Visible="False"></asp:Label> </td>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server" Visible="False" Width="144px"></asp:TextBox>
                            <asp:Button ID="Button5" runat="server" Text="search" Visible="False" />
                        </td>
                    </tr>
                </table>



                
                <div style="text-align:center;"><asp:Label ID="Label1" runat="server" Text=" "  ForeColor="#993333" Font-Size="20pt"></asp:Label></div>

                <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" Height="114px" Width="632px">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField />
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" />
                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <RowStyle BackColor="#F7F7DE" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                    <SortedAscendingHeaderStyle BackColor="#848384" />
                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                    <SortedDescendingHeaderStyle BackColor="#575357" />
                </asp:GridView>



            </td>
            
        </tr>
        <tr>
            <td class="auto-style6">
                <h4>ACCOOUNT INFORMATION</h4>
                <ul>
                    <li>  <asp:LinkButton ID="LinkButton5" runat="server" OnClick="LinkButton5_Click">View Admin Information Table</asp:LinkButton> </li>
                    <li> <asp:LinkButton ID="LinkButton6" runat="server" OnClick="LinkButton6_Click">View Student Information Table</asp:LinkButton> </li>
                </ul>
            </td>
        </tr>
        <tr>
            <td style="background-color:cadetblue"> <ul>
                <li> Edit account information(another page)</li>
                <li>Logout</li>
                                                    </ul>
            </td>
        </tr>


       
    </table>
        </div>






</asp:Content>
