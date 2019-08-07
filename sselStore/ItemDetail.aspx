<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/StoreMaster.Master" CodeBehind="ItemDetail.aspx.vb" Inherits="sselStore.ItemDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Literal runat="server" ID="litError"></asp:Literal>
    <div runat="server" id="divMain" class="placeholderlayout">
        <div style="color: #ff0000; font-weight: bold; margin-bottom: 10px;">
            <asp:Literal runat="server" ID="litSecItemMessage"></asp:Literal>
        </div>
        <div style="color: #0000ff; font-weight: bold;">
            <asp:Label ID="lblDescription" runat="server"></asp:Label>
        </div>
        <div class="item-notes" style="margin-bottom: 20px;">
            <asp:Label runat="server" ID="lblNotes"></asp:Label>
        </div>
        <div style="margin-bottom: 10px;">
            <table>
                <tr>
                    <td>
                        <asp:Image ID="imgItem" runat="server" />
                    </td>
                    <td>
                        <table>
                            <tbody>
                                <tr>
                                    <td style="width: 140px;">Price:
                                        <asp:Label ID="lblUnitCost" runat="server" Font-Bold="True"></asp:Label></td>
                                    <td>
                                        <span>Cross Charge:</span>
                                        <asp:Label ID="lblCrossCharge" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="plainbox">
                                    <td>Part NO:
                                        <asp:Label ID="lblManufacturerPN" runat="server"></asp:Label></td>
                                    <td>Stock:
                                        <asp:Label ID="lblStockQ" runat="server"></asp:Label>
                                        <asp:Label ID="lblStockWarning" runat="server" ForeColor="Red"></asp:Label></td>
                                </tr>
                            </tbody>
                        </table>
                        <div class="plainbox">
                            <asp:Panel ID="pnlAdd" runat="server">
                                <table style="width: 100%; border-collapse: collapse;">
                                    <tr id="trAddToCart" runat="server">
                                        <td style="padding: 0;">
                                            <asp:TextBox ID="txtBuyQ" runat="server" Width="20" Text="1" MaxLength="2" ValidationGroup="check"></asp:TextBox>&nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkAddToCart" runat="server" CssClass="addtocartlink" OnClick="lnkAddToCart_Click" ValidationGroup="check">Add To Cart</asp:LinkButton>
                                            <asp:Label ID="lblOutOfStock" runat="server" ForeColor="Gray" Visible="False">[Currently Out of Stock]</asp:Label>
                                            <br />
                                            <asp:RangeValidator ID="rv1" runat="server" ErrorMessage="Incorrect format (Quantiy must be 1 to 99)" ControlToValidate="txtBuyQ" MaximumValue="99" MinimumValue="1" Type="Integer" Display="static" ValidationGroup="check"></asp:RangeValidator>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                        <asp:Label ID="lblItemID" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:HyperLink runat="server" ID="hypReturn">&larr; Return</asp:HyperLink>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
