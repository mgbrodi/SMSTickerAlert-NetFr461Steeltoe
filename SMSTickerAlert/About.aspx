<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="SMSTickerAlert.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<p></p>
    <h3>Ticker Alert via SMS.</h3>
    <p> 
    <ul class="list-group">             
      <li class="list-group-item d-flex justify-content-between align-items-center">
        1. Pick your ticker from the list      
      </li>
      <li class="list-group-item d-flex justify-content-between align-items-center">
        2. Set the thersholds and provide your SMS number
      </li>
      <li class="list-group-item d-flex justify-content-between align-items-center">
        3. Wait for the SMS to come in as soon as one of the thersholds is hit
      </li>
    </ul>
</asp:Content>
