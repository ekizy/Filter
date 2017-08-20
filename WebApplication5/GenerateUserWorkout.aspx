<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GenerateUserWorkout.aspx.cs" Inherits="WebApplication5.GenerateUserWorkout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="height:50px;">

    </div>        
    <div style="margin-left:500px;">
  
    
        <table >
            <tr >
                <td>
                    <asp:Label ID="Label1" runat="server" Text="User Name"></asp:Label>
                </td>
                <td>
                     <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                </td>
            </tr>

                        <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Workout Name"></asp:Label>
                </td>

                <td>  
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>

                </td>
                   
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" Text="Generate User Workout" OnClick="Button1_Click" />
                    </td>
            </tr>
        
        </table>
  
        
 </div>  
</asp:Content>