﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Workouts.aspx.cs" Inherits="WebApplication5.Workouts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
            <div style="height:50px;">

    </div>        
    <div style="margin-left:530px;">
  
    
        <table >
            <tr >
                <td>
                    <asp:GridView ID="GridView1" CssClass="Exercises" runat="server" ></asp:GridView>
                </td>
            </tr>
        </table>
  
        
 </div> 
</asp:Content>
