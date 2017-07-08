<%@ Page Title="Exercises" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Exercises.aspx.cs" Inherits="WebApplication5.Exercises" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

               <div style="height:30px;">

    </div>     
        <div style="margin-left:200px;">
  
    
        <table >
            <tr >
                <td>
                    <asp:GridView ID="GridView1" CssClass="Exercises" runat="server" ></asp:GridView>
                </td>
            </tr>
        </table>
  
        
 </div>  
</asp:Content>
