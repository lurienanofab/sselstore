<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="OrderList.ascx.vb" Inherits="sselStore.Controls.OrderList" %>

Order Status:
<asp:DropDownList ID="ddlStatus" DataSourceID="odsStatus" DataTextField="Status" AutoPostBack="true" DataValueField="Status" runat="server">
</asp:DropDownList>

<asp:ObjectDataSource ID="odsStatus" runat="server" TypeName="sselStore.AppCode.BLL.StoreOrderManager" SelectMethod="GetStoreOrderStatus">
    <SelectParameters>
        <asp:Parameter DefaultValue="true" Name="showAll" Type="boolean" />
        <asp:Parameter DefaultValue="false" Name="showCart" Type="Boolean" />
        <asp:Parameter DefaultValue="Open" Name="currentStatus" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>

<span id="spnPrint" runat="server" visible="false">
    <img src="../images/printer.gif" alt="print" />
    <a href="PrintOrder.aspx?status=open" class="HyperLinkNormal" target="_blank">Printer-friendly form for all open orders</a></span>
<br />
<br />
<asp:GridView ID="gvStoreOrder" runat="server" DataSourceID="odsOrders" DataKeyNames="SOID" AllowPaging="true" PageSize="15" AutoGenerateColumns="False" AllowSorting="true" CssClass="store-grid">
    <RowStyle CssClass="row" />
    <AlternatingRowStyle CssClass="altrow" />
    <HeaderStyle CssClass="header" />
    <FooterStyle CssClass="footer" />
    <Columns>
        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <asp:ImageButton ID="ibtnDelete" AlternateText="Delete this order" runat="Server" OnClientClick="return confirm('Are you sure you want to delete this record?');" ImageUrl="~/images/im_delete.gif" CommandName="Delete" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:HyperLinkField HeaderText="Order ID" SortExpression="SOID" DataTextField="SOID" ControlStyle-CssClass="HyperLinkNormal" ItemStyle-HorizontalAlign="center" />
        <asp:BoundField Visible="false" HeaderText="Client" DataField="DisplayName" SortExpression="DisplayName" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField HeaderText="Created" DataField="CreationDate" SortExpression="CreationDate" ItemStyle-Width="140" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False" />
        <asp:BoundField HeaderText="Status" DataField="Status" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField HeaderText="Status Changed" DataField="StatusChangeDate" SortExpression="StatusChangeDate" ItemStyle-Width="120" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" />
        <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <asp:ImageButton ID="ibtnEdit" runat="Server" OnClientClick="return confirm('To edit an order, all items currently in your shopping cart will be lost.  Do you want to continue?');" ImageUrl="~/images/im_edit.gif" AlternateText="Edit" CommandName="MoveToCart" CommandArgument='<%#Eval("SOID")%>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:ButtonField HeaderText="View" ButtonType="image" Text="Detail" ImageUrl="~/images/im_viewdetail.gif" CommandName="Detail" Visible="false" />
    </Columns>
</asp:GridView>

<asp:ObjectDataSource runat="server" ID="odsOrders" TypeName="sselStore.AppCode.BLL.StoreOrderManager" SelectMethod="GetStoreOrderList" DeleteMethod="DeleteOrder">
    <SelectParameters>
        <asp:SessionParameter DefaultValue="1" Name="clientId" SessionField="ClientID" Type="Int32" />
        <asp:ControlParameter DefaultValue="Open" Name="orderStatus" ControlID="ddlStatus" Type="String" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="SOID" Type="Int32" />
    </DeleteParameters>
</asp:ObjectDataSource>

<asp:ObjectDataSource runat="server" ID="odsOrdersAdmin" TypeName="sselStore.AppCode.BLL.StoreOrderManager" SelectMethod="GetStoreOrderList" DeleteMethod="DeleteOrder">
    <SelectParameters>
        <asp:ControlParameter DefaultValue="Open" ControlID="ddlStatus" Name="orderStatus" Type="String" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="SOID" Type="Int32" />
    </DeleteParameters>
</asp:ObjectDataSource>
