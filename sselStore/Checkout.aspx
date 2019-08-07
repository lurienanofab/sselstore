<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/StoreMaster.Master" CodeBehind="Checkout.aspx.vb" Inherits="sselStore.Checkout" %>

<%@ Register Src="~/Controls/CartView.ascx" TagName="CartView" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="placeholderlayout">
        <div class="detail_form_row">
            <h2><b>Step1: Confirm and Payment</b> &gt;&gt;&gt;</h2>
            <h2 class="shaded">Done</h2>
        </div>

        <div class="detail_form_row">
            <h3>You are about to place a new order</h3>
            <div class="detail_form_row" id="divReviewMsg" runat="server">
                Please review the order details to make sure everything is correct or click
                <asp:HyperLink ID="hylCart" Text="here to go back to your cart" runat="server" NavigateUrl="~/Cart.aspx?tab=1"></asp:HyperLink>
                <div style="margin-top: 20px;">
                    Please note: The external non-academic multiplier also applies to store purchases. Your total amount may change if you change the account that you select.
                </div>
            </div>

            <h3>Payment Information</h3>
            Project Name:
            <asp:DropDownList ID="ddlAccount" runat="server" DataTextField="Text" DataValueField="Value" AutoPostBack="true" OnSelectedIndexChanged="ddlAccount_SelectedIndexChanged"></asp:DropDownList>

            <div class="detail_form_row" style="margin-top: 20px;">
                <uc:CartView runat="server" ID="CartView1" Locked="true" OnCartLoading="CartView1_CartLoading" />
            </div>
        </div>

        <div runat="server" id="divLocation" style="margin-top: 20px;">
            <div class="detail_form_row">
                <h3>Additional Information</h3>
                Chemical Pickup Location:
                <asp:DropDownList ID="ddlLocation" runat="server" DataTextField="LocationName" DataValueField="InventoryLocationID"></asp:DropDownList>
            </div>
        </div>

        <div id="divClient" runat="server" class="detail_form_row" visible="false">
            <h3>Kit Recipient</h3>
            Client Name:
            <asp:DropDownList ID="ddlClient" runat="server" DataTextField="DisplayName" DataValueField="ClientID"></asp:DropDownList>
        </div>

        <div class="detail_form_row">
            <asp:Label ID="lblMessage" CssClass="warningtext" runat="server"></asp:Label>
        </div>

        <div class="detail_form_row" style="text-align: right; margin-top: 20px; padding-right: 10px;">
            <span class="warningtext">Your ordering process is not finished until you click this button &rarr;</span>
            <asp:Button ID="btnPlaceOrder" runat="server" Text="Place Order" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
