<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSEdit.aspx.cs" 
    Inherits="SMSTickerAlert.TickerAlert.SMSEdit" MasterPageFile="~/Site.Master" %>

<asp:Content ID="form1" ContentPlaceHolderID="MainContent" runat="server">

    <script lang="javascript" type="text/javascript" >
function SetButtonStatus(sender, target)
{

    if (document.getElementById('MainContent_txtLow').value.length > 0 &&
        document.getElementById('MainContent_txtHigh').value.length > 0 &&
        document.getElementById('MainContent_txtMobile').value.length > 0) {
        document.getElementById("MainContent_"+target).disabled = false;
    }
    else {
        document.getElementById("MainContent_"+target).disabled = true;
    }
}
    </script>


    <div class="row">
        <div class="col-md-8">
            <section id="loginForm">
                <div class="form-horizontal">
                    <h3>
                    <asp:Label ID="lblTicker" runat="server" Text="Ticker"></asp:Label> 
                    currently at <asp:Label ID="lblCurrent" runat="server" Text="Current" DataFormatString="{0:c}"></asp:Label> <br />  
                    <small class="text-muted">last read on <asp:Label ID="lblLastRead" runat="server" Text="Last Read"></asp:Label></small>
                    </h3>
                </div>
                <p class="text-danger">
                    <asp:Label ID="lblError" runat="server" Text="Fill in all the fields to proceed!" Visible="False"></asp:Label>
                </p>

                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtHigh" CssClass="col-md-2 control-label">High*</asp:Label>
                    <div class="col-md-10">
                        <div class="form-group">
                        <div class="input-group mb-3">
                            <div class="input-group-prepend">
                            <span class="input-group-text">$</span>
                            </div>
                        <asp:TextBox class="form-control" ID="txtHigh" runat="server"  DataFormatString="{0:c}" onkeyup="SetButtonStatus(this, 'Subscribe')"></asp:TextBox>
                        </div>
                        </div>
                    </div>
                </div>
                    
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtLow" CssClass="col-md-2 control-label">Low*</asp:Label>
                    <div class="col-md-10">
                        <div class="form-group">
                        <div class="input-group mb-3">
                            <div class="input-group-prepend">
                            <span class="input-group-text">$</span>
                            </div>
                        <asp:TextBox class="form-control" ID="txtLow" runat="server"  onkeyup="SetButtonStatus(this, 'Subscribe')" DataFormatString="{0:c}"></asp:TextBox>
                        </div>
                        </div>                    
                    </div>
                </div>       
      
                 <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtMobile" CssClass="col-md-2 control-label">Mobile*</asp:Label>
                    <div class="col-md-10">
                        <div class="form-group">
                        <div class="input-group mb-3">
                            <div class="input-group-prepend">
                            <span class="input-group-text">+1</span>
                            </div>
                        <asp:TextBox class="form-control" ID="txtMobile" runat="server"  onkeyup="SetButtonStatus(this, 'Subscribe')"></asp:TextBox>
                        </div>
                        </div> 
                    </div>
                </div>  
                
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <asp:Button CssClass="btn btn-primary"  ID="Subscribe" runat="server" Text="Subscribe" OnClick="Subscribe_Click" Enabled="False" />
                    </div>
                </div>
                                 
            </section>
        </div>
    </div>   
</asp:Content>