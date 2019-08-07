<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="OrderDetail.ascx.vb" Inherits="sselStore.Controls.OrderDetail" %>

<%@ Import Namespace="sselStore.AppCode" %>
<%@ Register Src="~/Controls/CartView.ascx" TagName="CartView" TagPrefix="uc" %>
<asp:Literal runat="server" ID="litError"></asp:Literal>
<div runat="server" id="divMain">
    <asp:HiddenField runat="server" ID="hidSOID" />
    <asp:HiddenField runat="server" ID="hidClientID" />

    <asp:LinkButton ID="lbtnBack" Visible="false" runat="server" ForeColor="blue" Text="&larr; Back to store order list"></asp:LinkButton>

    <h1>Order Detail - ID:
        <asp:Label ID="lblSOID" runat="server"></asp:Label></h1>

    <div runat="server" id="divOrderHeader" class="order_detail_container">
        <table class="order_header">
            <tr>
                <td>Creation Date</td>
                <td>
                    <asp:TextBox runat="server" ID="txtCreationDate" Width="80"></asp:TextBox>
                </td>
                <td>Order Status</td>
                <td>
                    <span runat="server" id="spnStatusDDL">
                        <asp:DropDownList runat="server" ID="ddlStatus" DataValueField="Status" DataTextField="Status" />
                    </span>
                    <span runat="server" id="spnStatusText">
                        <asp:Label runat="server" ID="lblStatus"></asp:Label>
                    </span>
                </td>
            </tr>
            <tr>
                <td>Client</td>
                <td>
                    <asp:TextBox runat="server" ID="txtClient"></asp:TextBox>
                </td>
                <td>Contact</td>
                <td>
                    <asp:TextBox runat="server" ID="txtContact" TextMode="MultiLine" Width="235"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Status Change Date</td>
                <td>
                    <asp:TextBox runat="server" ID="txtStatusChangeDate" Width="80"></asp:TextBox>
                </td>
                <td>Account</td>
                <td>
                    <asp:TextBox runat="server" ID="txtAccount" ReadOnly="true" Width="235"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Pickup Location</td>
                <td>
                    <asp:TextBox runat="server" ID="txtLocation" Width="150" Enabled="false"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </div>

    <hr />

    <div class="detail_form_row">
        <uc:CartView runat="server" ID="CartView1" Locked="true" SessionKey="OrderDetail" OnCheckoutClick="CartView1_CheckoutClick" OnCartLoading="CartView1_CartLoading" />
        <asp:Label ID="lblSaveMessage" runat="server" CssClass="warningtext"></asp:Label>
        <div id="divAddNewItem" runat="server">
            <h3>Add new item in current order</h3>
            <a href="#" class="HyperLinkNormal item-lookup-link" style="margin-right: 20px;">Item Lookup</a>
            ItemID
            <asp:TextBox ID="txtNewItemID" CssClass="new-item-id" MaxLength='<%#GlobalUtility.GetTextBoxMaxLength("ItemID")%>' Width='<%#GlobalUtility.GetTextBoxWidth("ItemID")%>' runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ValidationGroup="NewItem" ID="rfvNewItemID" runat="server" ControlToValidate="txtNewItemID" ErrorMessage="Item ID cannot be empty" Display="None"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ValidationGroup="NewItem" ID="revNewItemID" runat="server" ControlToValidate="txtNewItemID" ValidationExpression="\b[1-9][0-9]{0,2}\b" ErrorMessage="Item ID incorrect format" Display="None"></asp:RegularExpressionValidator>
            Quantity
            <asp:TextBox ID="txtNewQty" CssClass="new-item-qty" MaxLength='<%#GlobalUtility.GetTextBoxMaxLength("StockQuantity")%>' Width='<%#GlobalUtility.GetTextBoxWidth("StockQuantity")%>' runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ValidationGroup="NewItem" ID="rfvNewQty" runat="server" ControlToValidate="txtNewQty" ErrorMessage="New quantity cannot be empty" Display="None"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ValidationGroup="NewItem" ID="revNewQty" runat="server" ControlToValidate="txtNewQty" ValidationExpression="\b[1-9][0-9]{0,2}\b" ErrorMessage="New quantity incorrect format" Display="None"></asp:RegularExpressionValidator>
            <div style="display: inline-block; margin-left: 20px;">
                <asp:Button ValidationGroup="NewItem" ID="btnCreateNewOrderDetail" runat="server" Text="Add" OnClick="btnCreateNewOrderDetail_Click" />
            </div>
            <asp:ValidationSummary ID="sum2" ValidationGroup="NewItem" runat="server" ShowSummary="true" />
            <div style="margin-top: 20px; font-style: italic; color: #808080;">
                Note: You must click the Save button after adding/removing an item, or modifying the quantity.
            </div>
        </div>
    </div>

    <div class="item-lookup-dialog" style="display: none;" title="Item Lookup">
        <label style="display: block; margin-bottom: 10px;">
            <input type="checkbox" class="item-option" checked />
            Web Store Items Only
        </label>
        <table class="item-lookup-table striped" style="width: 100%;">
            <thead>
                <tr>
                    <th>ItemID</th>
                    <th>Part #</th>
                    <th>Description</th>
                    <th>Total</th>
                    <th>Avail.</th>
                    <th>Reserve</th>
                </tr>
            </thead>
            <tbody></tbody>
        </table>
    </div>

    <asp:Panel runat="server" Visible="false" ID="panSecurityItems">
        <h3>Security Items</h3>
        <asp:Repeater runat="server" ID="rptSecItems">
            <HeaderTemplate>
                <table class="repeater" style="width: 100%;">
                    <tr>
                        <th>Item ID</th>
                        <th>Description</th>
                        <th>Card Number</th>
                        <th>Date Emailed</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td class="item">
                        <asp:HiddenField runat="server" ID="hidSecurityItemOrderID" Value='<%# DataBinder.Eval(Container.DataItem, "SecurityItemOrderID") %>' />
                        <asp:Label runat="server" ID="lblItemID" Text='<%# DataBinder.Eval(Container.DataItem, "ItemID") %>'></asp:Label>
                    </td>
                    <td class="item">
                        <asp:Label runat="server" ID="lblItemDescription" Text='<%# DataBinder.Eval(Container.DataItem, "ItemDescription")%>'></asp:Label>
                    </td>
                    <td class="item">
                        <asp:TextBox runat="server" ID="txtCardNumber" Text='<%# DataBinder.Eval(Container.DataItem, "ItemCardNumber")%>' Enabled='<%# IIf(DataBinder.Eval(Container.DataItem, "SecurityItemOrderID") Is DBNull.Value, "true", "false") %>' CssClass="watermark"></asp:TextBox>
                    </td>
                    <td class="item" style="text-align: center; width: 150px;">
                        <asp:Label runat="server" ID="lblDateEmailed" Text='<%# DbDate(DataBinder.Eval(Container.DataItem, "EmailSentDate")) %>'></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr>
                    <td class="alt_item">
                        <asp:HiddenField runat="server" ID="hidSecurityItemOrderID" Value='<%# DataBinder.Eval(Container.DataItem, "SecurityItemOrderID") %>' />
                        <asp:Label runat="server" ID="lblItemID" Text='<%# DataBinder.Eval(Container.DataItem, "ItemID") %>'></asp:Label>
                    </td>
                    <td class="alt_item">
                        <asp:Label runat="server" ID="lblItemDescription" Text='<%# DataBinder.Eval(Container.DataItem, "ItemDescription")%>'></asp:Label>
                    </td>
                    <td class="alt_item">
                        <asp:TextBox runat="server" ID="txtCardNumber" Text='<%# DataBinder.Eval(Container.DataItem, "ItemCardNumber")%>' Enabled='<%# IIf(DataBinder.Eval(Container.DataItem, "SecurityItemOrderID") Is DBNull.Value, "true", "false") %>' CssClass="watermark"></asp:TextBox>
                    </td>
                    <td class="alt_item" style="text-align: center; width: 150px;">
                        <asp:Label runat="server" ID="lblDateEmailed" Text='<%# DbDate(DataBinder.Eval(Container.DataItem, "EmailSentDate")) %>'></asp:Label>
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                <tr>
                    <td colspan="4">
                        <asp:Button runat="server" ID="btnSendEmail" Text="Send Email" OnClick="btnSendEmail_Click" />
                        <asp:Label runat="server" ID="lblSendEmailError" ForeColor="#FF0000"></asp:Label>
                    </td>
                </tr>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </asp:Panel>

    <hr />

    <div class="detail_form_row" style="margin-top: 20px;">
        <span id="spnPrint" runat="server">
            <asp:Image runat="server" ID="imgPrint" ImageUrl="~/images/printer.gif" AlternateText="Print" />
            <asp:HyperLink runat="server" ID="lnkPrinter" CssClass="HyperLinkNormal" Text="Printer-friendly"></asp:HyperLink></span>
        <div style="margin-top: 10px;">
            <%--<asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="SaveGroup" />--%>
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
        </div>
    </div>
    <asp:Label ID="lblLastStatus" Visible="false" runat="server"></asp:Label>
</div>
