<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserFilter.aspx.cs" Inherits="WebApplication5.UserFilter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="height:30px;"></div>

    <div style="width: 50%; margin: 0 auto;">
    <p class="EquipTB1">This page filters which users do a workout in certain time</p>
    <asp:TextBox ID="tbstart" CssClass="EquipTB1" runat="server"></asp:TextBox>
    <asp:TextBox ID="tbend" CssClass="EquipTB2" runat="server"></asp:TextBox>
           <br />
       <br />
       <div class="EquipBtn">
             <asp:Button OnClick="Button1_Click" CssClass="btn btn-primary" ID="Button1" runat="server" Text="Filter" />

       </div>
       <asp:Label ID="Label1" runat="server" Text="Please check the times in textboxes.">
       </asp:Label>
        <div style="height:30px;"></div>
       <table>
           <tr>
               <td>
                   <asp:GridView ID="GridView1" runat="server" CssClass="Exercises"></asp:GridView>
               </td>
           </tr>
       </table>
       </div>
</asp:Content>
