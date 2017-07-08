<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EquipmentFilter.aspx.cs" Inherits="WebApplication5.EquipmentFilter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="height:30px;">

    </div>
   <div style="width: 50%; margin: 0 auto;">

   
     <p> This page is created for showing which user is working with which equipment in certain time.</p>
    <asp:DropDownList CssClass="Eqfilter" ID="DropDownList1" runat="server"></asp:DropDownList>
      <br />
       <br />
    <asp:TextBox CssClass="EquipTB1" placeholder="start time" ID="tbstart" runat="server">

    </asp:TextBox>
    <asp:TextBox CssClass="EquipTB2"  placeholder="end time" ID="tbend" runat="server">

    </asp:TextBox>
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
