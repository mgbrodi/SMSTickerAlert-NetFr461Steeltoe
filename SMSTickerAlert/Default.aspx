<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SMSTickerAlert._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row">
        <div class="col-md-4">
            <br />
            <h2>Available Tickers:</h2>
            <p/>
            <div id="CategoryMenu" style="text-align: left">       
            <asp:ListView ID="tickerList"  
                ItemType="ModelsCommons.Model.Ticker" 
                runat="server"
                SelectMethod="GetTickers" >
                
                <ItemTemplate>
                    <b style="font-size: large; font-style: normal">
                        <ul class="list-group">
                          <li class="list-group-item d-flex justify-content-between align-items-center">
                            <a href="/TickerAlert/SMSEdit.aspx?id=<%#: Item.TickerName %>">
                            <%#: Item.TickerName %> (<%#: Item.Description %>)  
                            </a>
                            <span class="badge badge-primary badge-pill"><%#: string.Format("$ {0:#,##0.00}", double.Parse(Item.Current.ToString())) %></span>
                          </li>
                        </ul>
                        </b>
                </ItemTemplate>
               <ItemSeparatorTemplate>  
               </ItemSeparatorTemplate>
            </asp:ListView>
        </div>
      </div>
    </div>
    <p />
    <br />
    <hr />
    <br />
    <h2>Currently available SMS Alerts</h2>
    <div >
        <asp:GridView ID="SMSTickerAlertGrid" runat="server" AutoGenerateColumns="False" 
            ShowFooter="False" CellPadding="4" CellSpacing="4" 
        ItemType="ModelsCommons.Model.TickerSMSSettings" SelectMethod="SelectTickers" CssClass="table table-hover" DeleteMethod="SMSTickerAlertGrid_DeleteItem" DataKeyNames="Mobile">   
        <Columns>
            <asp:BoundField DataField="Mobile" HeaderText="Mobile" SortExpression="Mobile" />        
            <asp:BoundField DataField="TickerName" HeaderText="Ticker" />        
            <asp:BoundField DataField="TickerDate" HeaderText="Added On" />        
            <asp:BoundField DataField="High" HeaderText="High Threshold" DataFormatString="{0:c}" />        
            <asp:BoundField DataField="Low" HeaderText="Low Threshold" DataFormatString="{0:c}" /> 
            <asp:CommandField ShowDeleteButton="true" />
        </Columns>
            <RowStyle CssClass="table-light" />
            <HeaderStyle CssClass="table-primary" />
            <AlternatingRowStyle CssClass="table-default" /> 
        </asp:GridView>
    </div>
</asp:Content>
