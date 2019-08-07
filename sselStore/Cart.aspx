<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/StoreMaster.Master" CodeBehind="Cart.aspx.vb" Inherits="sselStore.Cart" %>

<%@ Register Src="~/Controls/CartView.ascx" TagName="CartView" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="placeholderlayout">
        <h1>Your Shopping Cart</h1>
        <uc:CartView runat="server" ID="CartView1" OnItemDetailClick="CartView1_ItemDetailClick" OnCheckoutClick="CartView1_CheckoutClick" OnContinueShoppingClick="CartView1_ContinueShoppingClick" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
