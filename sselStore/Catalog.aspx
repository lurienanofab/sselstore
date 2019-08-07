<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/StoreMaster.Master" CodeBehind="Catalog.aspx.vb" Inherits="sselStore.Catalog" %>

<%@ Register Src="~/Controls/CatalogList.ascx" TagName="CatalogList" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Literal runat="server" ID="litError"></asp:Literal>
    <div class="placeholderlayout" id="divItem" runat="server">
        <h1>
            <asp:Label ID="lblTitle" runat="server" Text="Items"></asp:Label>
        </h1>
        <uc:CatalogList ID="cl" runat="server" />
    </div>
    <div id="divCat" runat="server" class="placeholderlayout">
        <h1>
            <asp:Label ID="lblTitleCat" runat="server" Text="Sub categories" />
        </h1>
        <asp:DataList ID="dlCategory" runat="server" DataKeyField="CatID" DataSourceID="odsSubCatList">
            <ItemTemplate>
                <table>
                    <tr>
                        <td>
                            <a href="Catalog.aspx?self=true&tabid=0&h1=1&cid=<%# Eval("CatID") %>">
                                <img class='<%# GetImageStyle(Eval("CatID").ToString()) %>' alt='Category - <%# Eval("Description") %>' src='<%# SubCatImageURL(Eval("CatID").ToString()) %>' />
                            </a>
                        </td>
                        <td>
                            <div class="plainbox">
                                <asp:HyperLink runat="server" ID="hypCategory" NavigateUrl='<%#String.Format("{0}{1}", VirtualPathUtility.ToAbsolute("~/Catalog.aspx?self=true&tabid=0&h1=1&cid="), Eval("CatID"))%>' Text='<%#String.Format("[{0}]<br />{1}", Eval("CatName"), Eval("Description"))%>'></asp:HyperLink>
                            </div>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:DataList>

        <asp:ObjectDataSource ID="odsSubCatList" runat="server" TypeName="sselStore.AppCode.BLL.CatalogManager" SelectMethod="GetAllSubCategory">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="14" Name="catId" QueryStringField="cid" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
