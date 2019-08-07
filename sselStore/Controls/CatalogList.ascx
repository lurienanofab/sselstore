<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CatalogList.ascx.vb" Inherits="sselStore.Controls.CatalogList" %>
<div class="error">
    <asp:Literal runat="server" ID="litSecItemMessage"></asp:Literal>
</div>
<asp:DataList ID="dlItem" runat="server" DataKeyField="ItemID" DataSourceID="odsRootCategoryList">
    <ItemTemplate>
        <table>
            <tr>
                <td>
                    <asp:HyperLink runat="server" ID="hypItemDetailImage" NavigateUrl='<%#String.Format("~/ItemDetail.aspx?item={0}&avail={1}", Eval("ItemID"), Eval("AvailableStock"))%>'>
                        <asp:Image runat="server" ID="imgItem" AlternateText='<%#Eval("Description", "Product Image - {0}")%>' ImageUrl='<%#String.Format("~/images/items/{0}", GetItemImageName(Convert.ToInt32(Eval("ItemID"))))%>' CssClass='<%#GetImageStyle(Convert.ToInt32(Eval("ItemID")))%>' />
                    </asp:HyperLink>
                </td>
                <td>
                    <div class="plainbox">
                        <div style="color: Blue; font-weight: bold;">
                            <asp:HyperLink runat="server" ID="hypItemDetail1" NavigateUrl='<%#String.Format("~/ItemDetail.aspx?item={0}&avail={1}", Eval("ItemID"), Eval("AvailableStock"))%>' Text='<%#Eval("Description")%>' ForeColor="Blue"></asp:HyperLink>
                        </div>
                        <asp:Literal runat="server" ID="litNotes" Text='<%#GetNotes(Eval("Notes"))%>'></asp:Literal>
                        <table>
                            <tbody>
                                <tr>
                                    <td style="width: 125px;">Price:
                                        <%#GetItemPrice(Convert.ToInt32(Eval("ItemID")))%></td>
                                    <td>Stock:
                                        <%#Eval("AvailableStock")%></td>
                                </tr>
                            </tbody>
                        </table>
                        <asp:Label ID="lblStockWarning" runat="server" ForeColor="Red" Text='<%#GetWarningMessage(Integer.Parse(Eval("AvailableStock").ToString()))%>'></asp:Label>
                        <br />
                        <asp:HyperLink runat="server" ID="hypItemDetail2" NavigateUrl='<%#String.Format("~/ItemDetail.aspx?item={0}&avail={1}", Eval("ItemID"), Eval("AvailableStock"))%>'>
                            More<img alt="Detail" src="images/morearrow.gif"></asp:HyperLink>
                        &nbsp;&nbsp;<asp:TextBox ID="txtBuyQ" runat="server" Width="20" Text="1" MaxLength="2" ValidationGroup="check"></asp:TextBox>&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkAddToCart" runat="server" CssClass="addtocartlink" CommandArgument='<%# CombineString(Eval("ItemID").ToString(), GetItemPrice(Int32.Parse(Eval("ItemID").ToString())), Eval("Description").ToString(), Eval("ManufacturerPN").ToString())  %>' CommandName="AddToCart" Visible='<%# GetVisible(Int32.Parse(Eval("AvailableStock").ToString())) %>' ValidationGroup="check">Add To Cart</asp:LinkButton>
                        <br />
                        <asp:RangeValidator ID="rv1" runat="server" ErrorMessage="Incorrect format (Quantiy must be 1 to 99)" ControlToValidate="txtBuyQ" MaximumValue="99" MinimumValue="1" Type="Integer" Display="static" ValidationGroup="check"></asp:RangeValidator>
                    </div>
                </td>
            </tr>
        </table>
    </ItemTemplate>
</asp:DataList>

<asp:ObjectDataSource ID="odsRootCategoryList" runat="server" TypeName="sselStore.AppCode.BLL.CatalogManager" SelectMethod="GetItemsByCategory">
    <SelectParameters>
        <asp:QueryStringParameter DefaultValue="14" Name="catId" QueryStringField="cid" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>

<asp:ObjectDataSource ID="odsSearch" runat="server" TypeName="sselStore.AppCode.BLL.ItemManager" SelectMethod="Search">
    <SelectParameters>
        <asp:QueryStringParameter Name="query" QueryStringField="q" Type="String" />
        <asp:Parameter DefaultValue="None" Name="action" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
