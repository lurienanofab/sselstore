<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin/StoreAdminMaster.Master" CodeBehind="ItemManager.aspx.vb" Inherits="sselStore.Admin.ItemManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="placeholderlayout">
        <h1>Items Management</h1>

        <div class="detail_form_row">
            Search
            <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="searchbox"></asp:TextBox>
            <asp:Button ID="btnSearch" Text="Search" runat="server" ValidationGroup="searchgroup" />&nbsp;&nbsp;&nbsp;
            <asp:CheckBox ID="chkWebOnly" runat="server" Text="Web Store Items Only" AutoPostBack="true" Checked="true" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnCreateNew" runat="server" Text="Create New Item" />&nbsp;&nbsp;
            <div>
                <asp:RequiredFieldValidator ID="rfvSearch" ValidationGroup="searchgroup" runat="server" ErrorMessage="Search query cannot be empty" ControlToValidate="txtSearch" ForeColor="Red"></asp:RequiredFieldValidator>&nbsp;&nbsp;
            </div>
        </div>

        <div class="detail_form_row">
            <asp:Literal runat="server" ID="litSearchMessage"></asp:Literal>
            <asp:GridView ID="gvItem" runat="server" DataSourceID="odsItem" DataKeyNames="ItemID" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" PageSize="15" CssClass="store-grid">
                <RowStyle CssClass="row" />
                <AlternatingRowStyle CssClass="altrow" />
                <HeaderStyle CssClass="header" />
                <FooterStyle CssClass="footer" />
                <Columns>
                    <asp:TemplateField>
                        <ItemStyle Width="30" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnDel" runat="Server" OnClientClick="return confirm('Are you sure you want to delete this record?');" ImageUrl="~/images/im_delete.gif" CommandName="Delete" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Part NO" DataField="ManufacturerPN">
                        <ItemStyle HorizontalAlign="Center" Width="110px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Description" DataField="Description" SortExpression="Description">
                        <ItemStyle Width="250px" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Cross Charge" SortExpression="CrossCharge">
                        <ItemStyle Width="70" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:CheckBox Enabled="false" ID="chkCrossCharge" runat="server" Checked='<%# Eval("CrossCharge") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:ButtonField ButtonType="Image" ImageUrl="~/images/im_edit.gif" CommandName="detail" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30" />
                </Columns>
            </asp:GridView>

            <asp:ObjectDataSource ID="odsItem" runat="server" TypeName="sselStore.AppCode.BLL.ItemManager" SelectMethod="GetAllItems" DeleteMethod="DeleteItem">
                <DeleteParameters>
                    <asp:Parameter Name="itemID" Type="Int32" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="chkWebOnly" DefaultValue="True" Name="webitem" PropertyName="Checked" Type="Boolean" />
                    <asp:Parameter DefaultValue="False" Name="lookup" Type="Boolean" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:ObjectDataSource ID="odsItemSearch" runat="server" TypeName="sselStore.AppCode.BLL.ItemManager" SelectMethod="Search" DeleteMethod="DeleteItem">
                <SelectParameters>
                    <asp:ControlParameter DefaultValue="" ControlID="txtSearch" Name="query" PropertyName="Text" Type="String" />
                    <asp:Parameter DefaultValue="All" Name="action" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
