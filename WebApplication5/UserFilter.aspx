<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserFilter.aspx.cs" Inherits="WebApplication5.UserFilter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="height:70px;"></div>

    <div style="width: 50%; margin: 0 auto;">

        <div style="text-align:center;">
    <asp:Button OnClick="Button3_Click" CssClass="btn btn-info" ID="Button3" runat="server" Text="Pick A Date" />
         </div>
    
        <div style="height:30px;"></div>
        <div style="width:50%; margin: 0 auto;">
    <asp:Calendar ID="Calendar1" runat="server" Visible="false" ></asp:Calendar>
            </div>
        
           <br />
       <br />
       <div style="text-align:center;">
             <asp:Button OnClick="Button1_Click" CssClass="btn btn-primary" ID="Button1" runat="server" Text="Filter" />
                    <br />
       <br />
       <asp:Label ID="Label1" runat="server" Text="Please check the times in textboxes.">
       </asp:Label> 
           <asp:Label ID="Label2" runat="server" Visible ="false"></asp:Label>
           <table style="margin: 0 auto;">
           <tr>
               <td>
                   <asp:GridView ID="GridView1" runat="server" CssClass="Exercises">
                   </asp:GridView>
               </td>
           </tr>
       </table>
       </div>
        </div>
        
</asp:Content>
