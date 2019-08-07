<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/StoreMaster.Master" CodeBehind="OrderReceipt.aspx.vb" Inherits="sselStore.OrderReceipt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .receipt-message {
            margin-bottom: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="placeholderlayout">
        <h2 class="shaded">Confirm Order &gt;&gt;&gt;</h2>
        <h2><b>Step2: Done</b></h2>
        <br />
        <br />
        <h3>Order #
            <asp:Label ID="lblSOID" runat="server"></asp:Label></h3>
        <div class="plainbox">
            <div class="receipt-message">
                <asp:Literal runat="server" ID="litOrderReceiptMessage"></asp:Literal>
            </div>
            You can view your open orders
            <asp:HyperLink runat="server" ID="hypOrders" NavigateUrl="~/Orders.aspx?tab=2" Text="here"></asp:HyperLink><br />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
