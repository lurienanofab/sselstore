<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AdminSideMenu.ascx.vb" Inherits="sselStore.Controls.AdminSideMenu" %>

<div id="menutext">
    <ul>
        <asp:Repeater ID="rptMenu" DataSourceID="odsSideMenuList" runat="server" OnItemDataBound="rptMenu_ItemDataBound">
            <ItemTemplate>
                <li>
                    <asp:HyperLink runat="server" ID="hypMenuItem" NavigateUrl='<%#String.Format("~/admin/{0}?tab={1}&menu={2}", Eval("MenuURL"), Eval("TabID"), Eval("MenuID"))%>' Text='<%#Eval("MenuName")%>'></asp:HyperLink>
                    <%--<a style="<%# GetDisplayStyle(Eval("TabID").ToString(),Eval("MenuID").ToString()) %>" href="<%#Request.ApplicationPath%>/Admin/<%# Eval("MenuURL") %>?tab=<%# Eval("TabID")%>&menu=<%# Eval("MenuID")%>">
                        <%#Eval("MenuName").ToString()%></a>--%>
                    <br />
                    <br />
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</div>

<asp:ObjectDataSource ID="odsSideMenuList" runat="server" SelectMethod="GetSideMenuList" TypeName="sselStore.AppCode.BLL.AdminManager">
    <SelectParameters>
        <asp:QueryStringParameter DefaultValue="0" Name="tabid" QueryStringField="tab" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
