<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin/StoreAdminMaster.Master" CodeBehind="InventoryManager.aspx.vb" Inherits="sselStore.Admin.InventoryManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="placeholderlayout">
        <h1>Inventory Management</h1>

        <div class="detail_form_row">
            Search
            <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="searchbox"></asp:TextBox>
            <asp:Button ID="btnSearch" Text="Search" runat="server" ValidationGroup="searchgroup" />&nbsp;&nbsp;&nbsp;
            <asp:CheckBox ID="chkWebOnly" runat="server" Text="Web Store Items Only" AutoPostBack="true" Checked="true" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <span id="spnPrint" runat="server" visible="true">
                <img src="../images/printer.gif" alt="print" />
                <a href="PrintInventory.aspx" class="HyperLinkNormal" target="_blank">Print inventory</a>
            </span>
            <div>
                <asp:RequiredFieldValidator ID="rfvSearch" ValidationGroup="searchgroup" runat="server" ErrorMessage="Search query cannot be empty" ControlToValidate="txtSearch" ForeColor="Red"></asp:RequiredFieldValidator>
            </div>
        </div>

        <div>
            <asp:GridView ID="gvInv" runat="server" DataSourceID="odsInv" DataKeyNames="ItemID" AutoGenerateColumns="False" AllowPaging="true" PageSize="20" AllowSorting="true" CssClass="store-grid">
                <RowStyle CssClass="row" />
                <AlternatingRowStyle CssClass="altrow" />
                <HeaderStyle CssClass="header" />
                <FooterStyle CssClass="footer" />
                <Columns>
                    <asp:BoundField HeaderText="MPN" DataField="ManufacturerPN" ReadOnly="true" ItemStyle-Width="100">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Flag">
                        <ItemTemplate>
                            <asp:Image runat="server" ID="imgStockStatus" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Description" DataField="Description" ReadOnly="true" ItemStyle-Width="230" SortExpression="Description" />
                    <asp:BoundField HeaderText="Min" DataField="MinStockQuantity" ReadOnly="true" SortExpression="MinStockQuantity" />
                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:Label ID="lblTotalStock" Text='<%# Eval("StockQuantity") %>' runat="server"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Width="32" MaxLength="4" ID="txtTotalStock" runat="server" Text='<%# Bind("StockQuantity") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvTotalStock" runat="server" ControlToValidate="txtTotalStock" ErrorMessage="Total stock cannot be empty" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revTotalStock" runat="server" ControlToValidate="txtTotalStock" ValidationExpression="\b[0-9]{0,4}\b" ErrorMessage="Total stock incorrect format" Display="None"></asp:RegularExpressionValidator>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qty" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:Label ID="lblOnOrderQty" Text='<%# Eval("StockOnOrder") %>' runat="server"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Width="32" MaxLength="4" ID="txtOnOrderQty" runat="server" Text='<%# Bind("StockOnOrder") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvOnOrderQty" runat="server" ControlToValidate="txtOnOrderQty" ErrorMessage="On order stock cannot be empty" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revOnOrderQty" runat="server" ControlToValidate="txtOnOrderQty" ValidationExpression="\b[0-9]{0,4}\b" ErrorMessage="On order stock incorrect format" Display="None"></asp:RegularExpressionValidator>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Date" DataField="OrderDate" ItemStyle-Width="80" SortExpression="OrderDate" ItemStyle-HorizontalAlign="center" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" />
                    <asp:BoundField HeaderText="Est. Date" DataField="EstimatedArrivalDate" SortExpression="EstimatedArrivalDate" ItemStyle-Width="80" ItemStyle-HorizontalAlign="center" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" />
                    <asp:BoundField HeaderText="Avail" NullDisplayText="0" DataField="AvailableStock" ItemStyle-HorizontalAlign="center" ReadOnly="True" />
                    <asp:BoundField HeaderText="Res" DataField="StockReserve" NullDisplayText="0" ItemStyle-HorizontalAlign="center" ReadOnly="True" />
                    <asp:CommandField ShowEditButton="true" ButtonType="Image" CancelImageUrl="~/images/im_cancel.gif" UpdateImageUrl="~/images/im_update.gif" EditImageUrl="~/images/im_edit.gif" />
                </Columns>
            </asp:GridView>
            <asp:ValidationSummary ID="sum" runat="server" ShowSummary="true" />
            <div style="padding-left: 20px;">
                <asp:Label ID="lblErrorDateFormat" runat="server" CssClass="warningtext"></asp:Label>
            </div>
        </div>

        <asp:ObjectDataSource ID="odsInv" runat="server" TypeName="sselStore.AppCode.BLL.InventoryManager" SelectMethod="GetAllInventory" UpdateMethod="UpdateInventoryItem">
            <SelectParameters>
                <asp:ControlParameter ControlID="chkWebOnly" DefaultValue="True" Name="webOnly" PropertyName="Checked" Type="Boolean" />
            </SelectParameters>
        </asp:ObjectDataSource>

        <asp:ObjectDataSource ID="odsInvSearch" runat="server" TypeName="sselStore.AppCode.BLL.InventoryManager" SelectMethod="Search" UpdateMethod="UpdateInventoryItem">
            <SelectParameters>
                <asp:ControlParameter DefaultValue="" ControlID="txtSearch" Name="query" PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
    <script>
        function windowPrint() {
            window.open('PrintInventory.aspx', 'PrintInventory', 'status=yes,toolbar=yes,menubar=yes,location=no,scrollbars=yes')
        }
    </script>
</asp:Content>
