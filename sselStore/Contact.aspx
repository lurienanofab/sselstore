<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/StoreMaster.Master" CodeBehind="Contact.aspx.vb" Inherits="sselStore.Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        h2.block{
            font-size: 10pt;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="placeholderlayout">
        <h1>Contact Information</h1>
        <h2 class="block">If you have any questions or suggestions please email:</h2>
        <strong style="margin-left: 20px;">
            <asp:HyperLink runat="server" ID="lnkStoreAdminEmail"></asp:HyperLink></strong>
        <h2 class="block">To report bugs or give suggestions concerning the store website please email:</h2>
        <strong style="margin-left: 20px;">
            <asp:HyperLink runat="server" ID="lnkWebAdminEmail"></asp:HyperLink></strong>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
