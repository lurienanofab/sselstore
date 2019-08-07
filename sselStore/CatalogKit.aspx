<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/StoreMaster.Master" CodeBehind="CatalogKit.aspx.vb" Inherits="sselStore.CatalogKit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="placeholderlayout">
        <h1>Kits</h1>
        <asp:DataList ID="dlKit" runat="server" DataKeyField="KitID" DataSourceID="odsKitList">
            <ItemTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:Image runat="server" ID="imgKit" AlternateText='<%#Eval("Description", "Kit - {0}")%>' ImageUrl='<%#String.Format("{0}{1}", VirtualPathUtility.ToAbsolute("~/images/items/"), GetKitImageName())%>' />
                        </td>
                        <td>
                            <div class="plainbox">
                                <div style="color: Blue; font-weight: bold;">
                                    <%#Eval("KitName")%>
                                </div>
                                <div style="padding-left: 20px; margin-bottom: 10px;">
                                    <%#GetItemList(Convert.ToInt32(Eval("KitID")))%>
                                </div>
                                <div style="padding-left: 40px; margin-bottom: 10px;">
                                    <asp:LinkButton ID="lnkAddToCart" runat="server" CssClass="addtocartlink" CommandArgument='<%# Eval("KitID") %>' CommandName="AddToCart">Add To Cart</asp:LinkButton>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:DataList>
    </div>
    <asp:ObjectDataSource ID="odsKitList" runat="server" TypeName="sselStore.AppCode.BLL.KitManager" SelectMethod="GetAllKits"></asp:ObjectDataSource>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
