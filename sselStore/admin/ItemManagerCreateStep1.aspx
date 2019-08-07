<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin/StoreAdminMaster.Master" CodeBehind="ItemManagerCreateStep1.aspx.vb" Inherits="sselStore.Admin.ItemManagerCreateStep1" %>

<%@ Import Namespace="sselStore.AppCode" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-table {
            border-collapse: separate;
            border-spacing: 2px;
        }

            .form-table tbody th {
                padding: 5px;
                border-bottom: solid 1px #ddd;
                background-color: #efefef;
                text-align: left;
            }

            .form-table tbody td {
                padding: 5px;
                border-bottom: solid 1px #eaeaea;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="placeholderlayout">
        <div class="detail_form_row">
            <h1>Create New Item</h1>
        </div>
        <div class="detail_form_row">
            <h2><b>Step 1: New Item's General Information</b></h2>
            <br />
            <h2 class="shaded">Step 2: Upload Item Image (Optional)</h2>
            <asp:Literal runat="server" ID="litSaveError"></asp:Literal>
        </div>
        <table class="form-table">
            <tbody>
                <tr>
                    <th>Part #</th>
                    <td>
                        <asp:TextBox ID="txtManufacturerPN" runat="server" Width='<%#GlobalUtility.GetTextBoxWidth("ManufacturerPN")%>' MaxLength='<%#GlobalUtility.GetTextBoxMaxLength("ManufacturerPN")%>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>Description</th>
                    <td>
                        <asp:TextBox ID="txtDescription" runat="server" Width='<%#GlobalUtility.GetTextBoxWidth("Description")%>' MaxLength='<%#GlobalUtility.GetTextBoxMaxLength("Description")%>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>Category</th>
                    <td>
                        <asp:DropDownList ID="ddlCat0" runat="server" DataSourceID="odsCat0" DataValueField="CatID" DataTextField="CatName" Width="200" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>Sub Category</th>
                    <td>
                        <asp:DropDownList ID="ddlCat1" DataValueField="CatID" DataTextField="CatName" Width="200" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>Store Display</th>
                    <td>
                        <asp:CheckBox ID="chkStoreDisplay" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>Cross Charge</th>
                    <td>
                        <asp:CheckBox ID="chkCrossCharge" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>Search Key Words</th>
                    <td>
                        <asp:TextBox ID="txtKeyWords" Width="250" MaxLength="100" runat="server"></asp:TextBox>
                        <span style="font-style: italic; color: #808080;">e.g. Key1, Key2, Key3</span>
                    </td>
                </tr>
                <tr>
                    <th>Active</th>
                    <td>
                        <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                    </td>
                </tr>
                <tr>
                    <th>Initial Stock Quantity</th>
                    <td>
                        <asp:TextBox ID="txtStockQty" Text="0" runat="server" MaxLength='<%#GlobalUtility.GetTextBoxMaxLength("StockQuantity")%>' Width='<%#GlobalUtility.GetTextBoxWidth("StockQuantity")%>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>Minimum Quantity</th>
                    <td>
                        <asp:TextBox ID="txtMin" Text="0" runat="server" MaxLength='<%#GlobalUtility.GetTextBoxMaxLength("MinStockQuantity")%>' Width='<%#GlobalUtility.GetTextBoxWidth("MinStockQuantity")%>'></asp:TextBox>
                    </td>
                </tr>
            </tbody>
        </table>

        <%--
        <div class="detail_form_row">
            
            <asp:RequiredFieldValidator ValidationGroup="AddNew" ID="rfvStockQty" runat="server" ControlToValidate="txtStockQty" ErrorMessage="Stock quantity box cannot be empty" Display="none"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ValidationGroup="AddNew" ID="rfvMin" runat="server" ControlToValidate="txtMin" ErrorMessage="Minimum stock quantity box cannot be empty" Display="none"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ValidationGroup="AddNew" ID="revStockQty" runat="server" ControlToValidate="txtStockQty" ValidationExpression="\b[0-9]{0,4}\b" ErrorMessage="Stock quantity incorrect format" Display="None"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ValidationGroup="AddNew" ID="revMin" runat="server" ControlToValidate="txtMin" ValidationExpression="\b[0-9]{0,4}\b" ErrorMessage="Minimun stock quantity incorrect format" Display="None"></asp:RegularExpressionValidator>
        </div>
        <div class="detail_form_row">
            <asp:ValidationSummary ID="sum" runat="server" ShowSummary="true" ValidationGroup="AddNew" />
        </div>
        --%>

        <div class="detail_form_row" style="margin-top: 20px;">
            <asp:Button ID="btnSave" runat="server" Text="Save and Go to Step 2" OnClick="BtnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="BtnCancel_Click" />
        </div>

        <asp:ObjectDataSource ID="odsCat0" runat="server" TypeName="sselStore.AppCode.BLL.CatalogManager" SelectMethod="GetCategoryListByHierarchy">
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Name="catId" Type="Int32" />
                <asp:Parameter DefaultValue="0" Name="level" Type="Int32" />
                <asp:Parameter DefaultValue="False" Name="webOnly" Type="Boolean" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
