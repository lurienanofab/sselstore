<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin/StoreAdminMaster.Master" CodeBehind="ItemManagerDetail.aspx.vb" Inherits="sselStore.Admin.ItemManagerDetail" %>

<%@ Import Namespace="sselStore.AppCode" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .detail_form_row {
            padding-bottom: 10px;
        }

        .detail_form_column {
            padding-top: 3px;
            padding-right: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Literal runat="server" ID="litError"></asp:Literal>
    <div runat="server" id="divMain" class="placeholderlayout">
        <div>
            <asp:LinkButton ID="lbtnBack" runat="server" PostBackUrl="~/admin/ItemManager.aspx?tab=1&menu=0" ForeColor="blue" Text="&larr; Back to item list" Visible="false"></asp:LinkButton>
        </div>

        <div class="detail_form_row">
            <h1>Item Detail - ID:
                <asp:Label ID="lblItemID" runat="server"></asp:Label></h1>
            <asp:Literal ID="litSaveError" runat="server"></asp:Literal>
        </div>

        <table>
            <tr>
                <td class="detail_form_row detail_form_column" style="vertical-align: top;">Part #</td>
                <td class="detail_form_row" style="vertical-align: top;">
                    <asp:TextBox runat="server" ID="txtManufacturerPN" Width="400" MaxLength='<%#GlobalUtility.GetTextBoxMaxLength("ManufacturerPN")%>'></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="detail_form_row detail_form_column" style="vertical-align: top;">Description</td>
                <td class="detail_form_row" style="vertical-align: top;">
                    <asp:TextBox ID="txtDescription" Width="400" MaxLength='<%#GlobalUtility.GetTextBoxMaxLength("Description")%>' runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="detail_form_row detail_form_column" style="vertical-align: top;">Notes</td>
                <td class="detail_form_row" style="vertical-align: top;">
                    <asp:TextBox ID="txtNotes" TextMode="MultiLine" runat="server" Width="400" Height="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="detail_form_row detail_form_column" style="vertical-align: top;">Category</td>
                <td class="detail_form_row" style="vertical-align: top;">
                    <asp:DropDownList runat="server" ID="ddlCat0" DataSourceID="odsCat0" DataValueField="CatID" DataTextField="CatName" AutoPostBack="true" Width="150">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="detail_form_row detail_form_column" style="vertical-align: top;">Sub Category</td>
                <td class="detail_form_row" style="vertical-align: top;">
                    <asp:DropDownList runat="server" ID="ddlCat1" DataValueField="CatID" DataTextField="CatName" Width="150">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="detail_form_row detail_form_column" style="vertical-align: top;">Search Key Words</td>
                <td class="detail_form_row" style="vertical-align: top;">
                    <asp:TextBox runat="server" ID="txtKeyWords" Width="250" MaxLength="100"></asp:TextBox>&nbsp;&nbsp;e.g. Key1, Key2, Key3
                </td>
            </tr>
            <tr>
                <td class="detail_form_row" style="vertical-align: top;" colspan="2">
                    <asp:CheckBox runat="server" ID="chkStoreDisplay" Text="Store Display" />
                    &nbsp;&nbsp;&nbsp;<asp:CheckBox runat="server" ID="chkCrossCharge" Text="Cross Charge" />
                    &nbsp;&nbsp;&nbsp; Price $<asp:TextBox runat="server" ID="txtPrice" MaxLength="6" Width="40" ReadOnly="true"></asp:TextBox>
                    <asp:Button runat="server" ID="btnPricing" Text="Pricing" />
                </td>
            </tr>
            <tr>
                <td class="detail_form_row" style="vertical-align: top;" colspan="2">
                    <asp:CheckBox ID="chkActive" runat="server" Checked="true" Text="Active" />
                    &nbsp;&nbsp;&nbsp;Stock Quantity
                    <asp:TextBox ID="txtStockQty" runat="server" Text="0" MaxLength='<%#GlobalUtility.GetTextBoxMaxLength("StockQuantity")%>' Width='<%#GlobalUtility.GetTextBoxWidth("StockQuantity")%>'></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;Minimum Quantity
                    <asp:TextBox ID="txtMin" runat="server" Text="10" MaxLength='<%#GlobalUtility.GetTextBoxMaxLength("MinStockQuantity")%>' Width='<%#GlobalUtility.GetTextBoxWidth("MinStockQuantity")%>'></asp:TextBox>
                </td>
            </tr>
        </table>

        <div class="detail_form_row">
            <asp:Button ID="btnSave" runat="server" Text="Save" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
        </div>

        <hr />

        <h3 style="margin-top: 20px;">Item Image Upload</h3>
        <div class="detail_form_row">
            File Path:
            <asp:FileUpload ID="FileUpload1" runat="server" />
            <asp:Button ID="btnUpload" runat="server" Text="Upload" />

        </div>
        <div class="detail_form_row">
            <asp:Image runat="server" ID="imgItem" Width="100" Height="100" />
        </div>

    </div>

    <asp:ObjectDataSource ID="odsCat0" runat="server" TypeName="sselStore.AppCode.BLL.CatalogManager" SelectMethod="GetCategoryListByHierarchy">
        <SelectParameters>
            <asp:Parameter DefaultValue="0" Name="catId" Type="Int32" />
            <asp:Parameter DefaultValue="0" Name="level" Type="Int32" />
            <asp:Parameter DefaultValue="False" Name="webOnly" Type="Boolean" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
