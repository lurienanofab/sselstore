<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin/StoreAdminMaster.Master" CodeBehind="SearchOrder.aspx.vb" Inherits="sselStore.Admin.SearchOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ui-widget {
            font-size: 10pt;
        }

        .datepicker {
            width: 100px;
        }

        .order-table td {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="placeholderlayout">
        <h1 style="margin-bottom: 30px;">Advanced Search for Orders</h1>
        <div class="detail_form_row">
            <div style="margin-bottom: 10px;">
                User:
                <asp:DropDownList runat="server" ID="ddlUser" DataTextField="DisplayName" DataValueField="ClientID" Width="200"></asp:DropDownList>
                <asp:RadioButtonList runat="server" ID="rblUserType" AutoPostBack="true" OnSelectedIndexChanged="rblUserType_SelectedIndexChanged" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Text="External" Value="external" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Internal" Value="internal"></asp:ListItem>
                    <asp:ListItem Text="Staff" Value="staff"></asp:ListItem>
                    <asp:ListItem Text="All" Value="all"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div style="margin-bottom: 10px;">
                Order Date:
                <span style="margin-left: 20px;">From</span>
                <asp:TextBox runat="server" ID="txtStartDate" CssClass="datepicker"></asp:TextBox>
                <img src="../images/calendar.gif" alt="" />
                <span style="margin-left: 20px;">To</span>
                <asp:TextBox runat="server" ID="txtEndDate" CssClass="datepicker"></asp:TextBox>
                <img src="../images/calendar.gif" alt="" />
            </div>
            <div style="margin-bottom: 10px;">
                <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
            </div>
        </div>
        <asp:Repeater runat="server" ID="rptOrders">
            <HeaderTemplate>
                <hr />
                <div style="margin-top: 10px;">
                    <table class="order-table striped" style="width: 100%;">
                        <thead>
                            <tr>
                                <th>Order ID</th>
                                <th>Created</th>
                                <th>Status</th>
                                <th>Status Changed</th>
                            </tr>
                        </thead>
                        <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:HyperLink runat="server" ID="hypDetail" Text='<%#Eval("SOID")%>' NavigateUrl='<%#Eval("SOID","~/admin/OrderManagerDetail.aspx?tab=0&menu=2&status=Search&so={0}")%>'></asp:HyperLink>
                    </td>
                    <td><%#Eval("CreationDate", "{0:MM/dd/yyyy}")%></td>
                    <td><%#Eval("Status")%></td>
                    <td><%#Eval("StatusChangeDate", "{0:MM/dd/yyyy}")%></td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr>
                    <td>
                        <asp:HyperLink runat="server" ID="hypDetail" Text='<%#Eval("SOID")%>' NavigateUrl='<%#Eval("SOID","~/admin/OrderManagerDetail.aspx?tab=0&menu=2&status=Search&so={0}")%>'></asp:HyperLink>
                    </td>
                    <td><%#Eval("CreationDate", "{0:MM/dd/yyyy}")%></td>
                    <td><%#Eval("Status")%></td>
                    <td><%#Eval("StatusChangeDate", "{0:MM/dd/yyyy}")%></td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </tbody>
                </table>
                </div>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
    <script>
        $(document).ready(function () {
            $(".datepicker").datepicker();
            $(".order-table").dataTable({
                "lengthChange": false,
                "paging": false,
                "searching": false,
                "info": false
            });
        });
    </script>
</asp:Content>
