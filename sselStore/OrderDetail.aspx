<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/StoreMaster.Master" CodeBehind="OrderDetail.aspx.vb" Inherits="sselStore.OrderDetail" %>

<%@ Register Src="~/Controls/OrderDetail.ascx" TagName="OrderDetail" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="placeholderlayout">
        <uc:OrderDetail ID="OrderDetail1" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
