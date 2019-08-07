<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PrintInventory.aspx.vb" Inherits="sselStore.Admin.PrintInventory" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Print Inventory</title>
    <link rel="stylesheet" href="../styles/default.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div style="padding: 20px;">
            <asp:Repeater runat="server" ID="rptInventory">
                <HeaderTemplate>
                    <table class="store-grid" style="border-collapse: collapse;">
                        <thead>
                            <tr class="header">
                                <th colspan="2">Generic</th>
                                <th colspan="3">Stock</th>
                                <th colspan="2">New IOF Order</th>
                                <th colspan="2">Store</th>
                            </tr>
                            <tr class="header">
                                <th>MPN</th>
                                <th>Description</th>
                                <th>Min</th>
                                <th>Total</th>
                                <th>Qty</th>
                                <th>Date</th>
                                <th>Est Date</th>
                                <th>Avail</th>
                                <th>Res</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="row">
                        <td style="text-align: center;"><%#Eval("ManufacturerPN")%></td>
                        <td><%#Eval("Description")%></td>
                        <td style="text-align: center;"><%#Eval("MinStockQuantity")%></td>
                        <td style="text-align: center;"><%#Eval("StockQuantity")%></td>
                        <td style="text-align: center;"><%#Eval("StockOnOrder")%></td>
                        <td style="text-align: center;"><%#Eval("OrderDate")%></td>
                        <td style="text-align: center;"><%#Eval("EstimatedArrivalDate")%></td>
                        <td style="text-align: center;"><%#Eval("AvailableStock")%></td>
                        <td style="text-align: center;"><%#Eval("StockReserve")%></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="alt-row">
                        <td style="text-align: center;"><%#Eval("ManufacturerPN")%></td>
                        <td><%#Eval("Description")%></td>
                        <td style="text-align: center;"><%#Eval("MinStockQuantity")%></td>
                        <td style="text-align: center;"><%#Eval("StockQuantity")%></td>
                        <td style="text-align: center;"><%#Eval("StockOnOrder")%></td>
                        <td style="text-align: center;"><%#Eval("OrderDate")%></td>
                        <td style="text-align: center;"><%#Eval("EstimatedArrivalDate")%></td>
                        <td style="text-align: center;"><%#Eval("AvailableStock")%></td>
                        <td style="text-align: center;"><%#Eval("StockReserve")%></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </form>
</body>
</html>
