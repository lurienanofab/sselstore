<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/StoreMaster.Master" CodeBehind="Search.aspx.vb" Inherits="sselStore.Search" %>

<%@ Register Src="~/Controls/CatalogList.ascx" TagName="CatalogList" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="placeholderlayout">
        <h1>Search Result</h1>
        <h2>
            <asp:Label ID="lblSubTitle" runat="server"></asp:Label></h2>
        <uc:CatalogList ID="CatalogList1" runat="server" Search="true" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
