<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin/StoreAdminMaster.Master" CodeBehind="SecurityManager.aspx.vb" Inherits="sselStore.Admin.SecurityManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .secitems th,
        .secitems td {
            padding: 6px;
            vertical-align: middle;
        }

        .itemid {
            border-collapse: collapse;
        }

            .itemid td {
                vertical-align: middle;
                padding: 3px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="placeholderlayout">
        <h1>Security Manager</h1>
        <div style="padding-bottom: 5px; margin-bottom: 5px; border-bottom: 1px solid #DCDCDC;">
            <table class="itemid">
                <tr>
                    <td>Item ID:</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtItemID" Width="60"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnAdd" Text="Add" OnClick="btnAdd_Click" />
                    </td>
                    <td style="color: #ff0000;">
                        <asp:Literal runat="server" ID="litAddErrorMessage"></asp:Literal>
                    </td>
                </tr>
            </table>
        </div>
        <div style="margin-top: 10px;">
            <div style="padding-bottom: 5px;">
                <asp:Button runat="server" ID="btnSave" Text="Save Changes" OnClick="btnSave_Click" />
            </div>
            <asp:GridView runat="server" ID="gvSecurityItems" AutoGenerateColumns="false" CssClass="secitems store-grid">
                <RowStyle CssClass="row" />
                <AlternatingRowStyle CssClass="altrow" />
                <HeaderStyle CssClass="header" />
                <FooterStyle CssClass="footer" />
                <Columns>
                    <asp:BoundField DataField="ItemID" HeaderText="ItemID" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:TemplateField HeaderText="Active" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="chkActive" Checked='<%#DataBinder.Eval(Container.DataItem, "Active")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div style="font-style: italic; width: 300px;">
                        No items were found.
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
