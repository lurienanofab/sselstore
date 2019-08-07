<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin/StoreAdminMaster.Master" CodeBehind="OrderManager.aspx.vb" Inherits="sselStore.Admin.OrderManager" %>

<%@ Register Src="~/Controls/OrderList.ascx" TagName="OrderList" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .kftbl {
            border-collapse: collapse;
        }

            .kftbl th, .kftbl td {
                border: 1px solid #DCDCDC;
                padding: 4px;
            }

            .kftbl th {
                background-color: #DCDCDC;
            }

        .fobnum {
            width: 120px;
        }

        .watermark {
            color: #999999;
            font-style: italic;
            font-family: Arial;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="placeholderlayout">
        <div style="display: none;">
            <h1>Key Fob Orders</h1>
            <div class="keyfob">
                <table class="kftbl">
                    <tr>
                        <th>Fob#</th>
                        <th>Client</th>
                        <th>Type</th>
                        <th>&nbsp;</th>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ID="txtFobNumber" CssClass="fobnum" />
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlFobClient" DataSourceID="odsClients" DataValueField="ClientID" DataTextField="DisplayName" OnDataBound="ddlFobClient_DataBound">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlFobType">
                                <asp:ListItem Text="Card" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Fob" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnFobOrder" Text="Order" />
                        </td>
                    </tr>
                </table>
                <asp:ObjectDataSource runat="server" ID="odsClients" TypeName="sselStore.AppCode.BLL.SselData" SelectMethod="GetClients"></asp:ObjectDataSource>
            </div>
        </div>
        <h1>Orders Management</h1>
        <uc:OrderList ID="OrderList1" runat="server" AdminMode="true" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('.fobnum').watermark('Scan or Type Card#', { className: 'watermark' });
        });
    </script>
</asp:Content>
