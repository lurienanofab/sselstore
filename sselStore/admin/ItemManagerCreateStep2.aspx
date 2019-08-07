<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin/StoreAdminMaster.Master" CodeBehind="ItemManagerCreateStep2.aspx.vb" Inherits="sselStore.Admin.ItemManagerCreateStep2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="placeholderlayout">
        <div class="detail_form_row">
            <h1>Create New Item - ID:
            <asp:Label ID="lblItemID" runat="server" Text="Auto"></asp:Label></h1>
        </div>
        <div class="detail_form_row">
            <h2 class="shaded">Step 1: New Item's General Information</h2>
            <br />
            <h2><b>Step 2: Upload Item Image (Optional)</b></h2>
            <asp:Literal ID="litSaveError" runat="server"></asp:Literal>
        </div>
        <div class="detail_form_row">
            The new item has been created successfully into database. You can now add new image or go back to item list.
        </div>
        <div class="detail_form_row">
            File Path:
            <asp:FileUpload ID="FileUpload1" runat="server" />
        </div>
        <div class="detail_form_row">
            <asp:Image runat="server" ID="imgItem" Visible="false" />
        </div>
        <div class="detail_form_row">
            <asp:Button ID="btnUpload" runat="server" Text="Upload" ValidationGroup="image" OnClick="btnUpload_Click" />
            <asp:Button ID="btnDone" runat="server" Text="&larr; Back to Item List" OnClick="btnDone_Click" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
