<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CartView.ascx.vb" Inherits="sselStore.Controls.CartView" %>
<table class="cart">
    <tr>
        <td>
            <asp:Label ID="lblTopMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False" DataKeyNames="ItemID" CssClass="store-grid" GridLines="Vertical">
                <RowStyle CssClass="row" />
                <AlternatingRowStyle CssClass="altrow" />
                <HeaderStyle CssClass="header" />
                <FooterStyle CssClass="footer" />
                <EmptyDataTemplate>
                    <% If AdminMode Then%>
                    -- No items in order --
                    <% Else%>
                    -- No items in your cart --
                    <%End If%>
                </EmptyDataTemplate>
                <Columns>
                    <asp:BoundField HeaderText="Item ID" Visible="true" DataField="ItemID" ItemStyle-CssClass="centered" />
                    <asp:TemplateField HeaderText="Description" ItemStyle-Width="250" ItemStyle-CssClass="centered">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="lbtnItemDetail" CommandArgument='<%#Eval("ItemID")%>' Visible='<%#Not Locked AndAlso Not AdminMode%>' OnCommand="lbtnItemDetail_Command" ForeColor="Blue" Text='<%#Eval("Description")%>'></asp:LinkButton>
                            <asp:Literal runat="server" ID="litItemDetail" Text='<%#Eval("Description")%>' Visible='<%#Locked OrElse AdminMode%>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Part #" DataField="ManufacturerPN" ItemStyle-Width="110" ItemStyle-CssClass="centered"></asp:BoundField>
                    <asp:BoundField HeaderText="Price" DataField="UnitPrice" DataFormatString="{0:C}" HtmlEncode="False" ItemStyle-CssClass="centered"></asp:BoundField>
                    <asp:TemplateField HeaderText="Quantity" ItemStyle-CssClass="centered">
                        <ItemTemplate>
                            <asp:TextBox ID="txtQuantity" runat="server" Enabled='<%# IsEnabled() %>' Text='<%# Eval("Quantity")%>' Visible='<%#Not Locked%>' Width="40" CausesValidation="True" MaxLength="3" CssClass="numeric-text"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="rfv1" runat="server" ErrorMessage="Box cannot be empty" ControlToValidate="txtQuantity" Display="Static" ValidationGroup="check">*</asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="Incorrect format (or qty must be from 1 to 999)" ControlToValidate="txtQuantity" MaximumValue="999" MinimumValue="0" Type="Integer" Display="Static" ValidationGroup="check">*</asp:RangeValidator>--%>
                            <asp:Literal runat="server" ID="litQuantity" Text='<%#Eval("Quantity")%>' Visible='<%#Locked%>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-CssClass="centered" HeaderText="Line Total" DataField="LineTotal" DataFormatString="{0:c}" HtmlEncode="False"></asp:BoundField>
                    <asp:TemplateField>
                        <ItemStyle CssClass="centered" />
                        <ItemTemplate>
                            <asp:ImageButton ID="ibtnItemDelete" runat="Server" Visible="<%# IsRegularShopping() %>" ImageUrl="//ssel-apps.eecs.umich.edu/static/images/delete.png" ToolTip='<%#Eval("Description","Delete {0}") %>' AlternateText="Delete" CommandArgument='<%# Bind("ItemID") %>' CommandName="Delete" OnCommand="ibtnItemDelete_Command" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
    <tr>
        <td style="padding-top: 10px; text-align: right;">
            <asp:Button ID="btnAdjustQ" runat="server" Text="Adjust Quantity" Width="110" ValidationGroup="check" OnClick="btnAdjustQ_Click" />
        </td>
    </tr>
    <tr>
        <td style="padding-top: 10px; text-align: right;">
            <div id="divCartOperation" runat="server">
                <div>
                    <div style="font-weight: bold; padding-right: 10px;">
                        Total:
                        <asp:Label ID="lblTotal" runat="Server"></asp:Label>
                    </div>
                </div>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <div style="width: 590px;">
                <asp:Label ID="lblStockError" runat="server" CssClass="warningtext"></asp:Label>
            </div>
        </td>
    </tr>
    <tr>
        <td style="text-align: right; padding-top: 20px;">
            <asp:Button ID="btnContinueShopping" Text="Continue Shopping" runat="server" OnClick="btnContinueShopping_Click" />
            <asp:Button ID="btnCheckout" runat="server" Text="Checkout &rarr;" Width="110" ValidationGroup="check" OnClick="btnCheckout_Click" />
        </td>
    </tr>
</table>
