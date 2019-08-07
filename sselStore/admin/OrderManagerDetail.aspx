<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin/StoreAdminMaster.Master" CodeBehind="OrderManagerDetail.aspx.vb" Inherits="sselStore.Admin.OrderManagerDetail" %>

<%@ Register Src="~/Controls/OrderDetail.ascx" TagName="OrderDetail" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ui-dialog {
            font-size: 10pt;
        }

        .item-lookup-table tbody tr {
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="placeholderlayout">
        <uc:OrderDetail runat="server" ID="odAdmin" AdminMode="true" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
    <script>
        $(document).ready(function () {
            $('.watermark').watermark('Scan here', { className: 'watermark' })

            var getItemOption = function () {
                return $(".item-option").is(":checked") ? "web-store-items" : "all-items";
            }

            $(".item-lookup-link").on("click", function (e) {
                e.preventDefault();

                var lookupDialog = $(".item-lookup-dialog").dialog({ "width": 700 });

                var table = $(".item-lookup-table").dataTable({
                    "retrieve": true,
                    "ajax": "../ajax/items.ashx?item-option=" + getItemOption(),
                    "columns": [
                        { "data": "ItemID", "width": "70px" },
                        { "data": "ManufacturerPN" },
                        { "data": "Description" },
                        { "data": "StockQuantity" },
                        { "data": "StockAvailable" },
                        { "data": "StockReserve" }
                    ]
                });

                $(".item-lookup-table tbody").on("click", "tr", function (e) {
                    var itemId = $(this).find("td").eq(0).text();
                    lookupDialog.dialog("close");
                    $(".new-item-id").val(itemId);
                    $(".new-item-qty").val("1").focus();
                });

                $(".item-option").on("change", function (e) {
                    table.api().ajax.url("../ajax/items.ashx?item-option=" + getItemOption()).load();
                });
            });
        });
    </script>
</asp:Content>
