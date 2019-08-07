<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin/StoreAdminMaster.Master" CodeBehind="ItemManagerPricing.aspx.vb" Inherits="sselStore.Admin.ItemManagerPricing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .boxtitle {
            padding: 5px;
            background-color: #CCCCFF;
            border-bottom: 1px solid #808080;
            font-weight: bold;
        }

        .pricing_table {
            border-collapse: collapse;
        }

            .pricing_table th, .pricing_table td {
                padding: 5px;
                border: 1px solid #AAAAAA;
                vertical-align: middle;
            }

            .pricing_table th {
                background-color: #CCCCCC;
            }

            .pricing_table td {
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Literal runat="server" ID="litDebug"></asp:Literal>
    <asp:HiddenField runat="server" ID="hidCurrentPackageID" />
    <asp:HiddenField runat="server" ID="hidCurrentVendorPackageID" />
    <div style="padding-left: 20px; padding-top: 20px;">
        <h1>Item Pricing</h1>
        <div style="padding: 0px 0px 15px 10px; font-weight: bold;">
            Current Price:&nbsp;
            <asp:Literal runat="server" ID="litCurrentPrice"></asp:Literal>
        </div>
        <div style="padding: 10px; border: 1px solid #DCDCDC;">
            <div style="border: 1px solid #808080; margin-bottom: 10px;">
                <div class="boxtitle">
                    Package - Item ID:&nbsp;
                    <asp:Literal runat="server" ID="litItemID"></asp:Literal>
                </div>
                <div style="padding: 5px;">
                    <asp:Repeater runat="server" ID="rptPackage">
                        <HeaderTemplate>
                            <table class="pricing_table">
                                <tr>
                                    <th>ID</th>
                                    <th>Description</th>
                                    <th>Base Qty</th>
                                    <th>Active</th>
                                    <th>Vendor Package</th>
                                    <th>&nbsp;</th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr runat="server" id="trPackageItem">
                                <td>
                                    <asp:Label runat="server" ID="lblPackageID" Text='<%# DataBinder.Eval(Container.DataItem, "PackageID") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblPackageDescription" Text='<%# DataBinder.Eval(Container.DataItem, "Descriptor") %>'></asp:Label>
                                    <asp:TextBox runat="server" ID="txtPackageDescriptionEdit" Text='<%# DataBinder.Eval(Container.DataItem, "Descriptor") %>' Visible="false" Width="180"></asp:TextBox>
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label runat="server" ID="lblPackageBaseQty" Text='<%# DataBinder.Eval(Container.DataItem, "BaseQMultiplier")%>'></asp:Label>
                                    <asp:TextBox runat="server" ID="txtPackageBaseQtyEdit" Text='<%# DataBinder.Eval(Container.DataItem, "BaseQMultiplier")%>' Visible="false" Width="55"></asp:TextBox>
                                </td>
                                <td style="text-align: center;">
                                    <asp:CheckBox runat="server" ID="chkPackageActive" Checked='<%# DataBinder.Eval(Container.DataItem, "Active")%>' Enabled="false" />
                                </td>
                                <td style="text-align: center;">
                                    <asp:LinkButton runat="server" ID="lnkVendorPackage" OnCommand="lnkVendorPackage_Command" CommandName="view_vendor_package" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PackageID") %>' Text="View"></asp:LinkButton>
                                </td>
                                <td style="text-align: center;">
                                    <div runat="server" id="divPackageEditButton">
                                        <asp:ImageButton runat="server" ID="btnPackageEdit" ImageUrl="//ssel-apps.eecs.umich.edu/static/images/edit.png" AlternateText="Edit" ToolTip="Edit" CommandName="edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PackageID")%>' OnCommand="btnPackageEdit_Command" />
                                    </div>
                                    <div runat="server" id="divPackageSaveCancelButton" visible="false">
                                        <asp:ImageButton runat="server" ID="btnPackageEditSave" ImageUrl="//ssel-apps.eecs.umich.edu/static/images/ok.png" AlternateText="Save" ToolTip="Save" CommandName="save" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PackageID")%>' OnCommand="btnPackageEdit_Command" />
                                        <asp:ImageButton runat="server" ID="btnPackageEditCancel" ImageUrl="//ssel-apps.eecs.umich.edu/static/images/cancel.png" AlternateText="Cancel" ToolTip="Cancel" CommandName="cancel" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PackageID")%>' OnCommand="btnPackageEdit_Command" />
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtAddPackageDescription" Width="180"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtAddPackageBaseQty" Width="55"></asp:TextBox>
                                </td>
                                <td colspan="3" style="vertical-align: middle;">
                                    <table style="border-collapse: collapse; margin: 0px;">
                                        <tr>
                                            <td style="vertical-align: middle; border: none; padding: 0px;">
                                                <asp:ImageButton runat="server" ID="btnPackageAdd" ImageUrl="//ssel-apps.eecs.umich.edu/static/images/add.png" AlternateText="Add" ToolTip="Add" OnClick="btnPackageAdd_Click" />
                                            </td>
                                            <td style="vertical-align: middle; border: none; padding: 0px 0px 0px 5px; color: #FF0000;">
                                                <asp:Literal runat="server" ID="litPackageAddError"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <div runat="server" id="divVendorPackage" visible="false" style="border: 1px solid #808080; margin-bottom: 10px;">
                <div class="boxtitle">
                    Vendor Package - Package ID:&nbsp;
                    <asp:Literal runat="server" ID="litPackageID"></asp:Literal>
                </div>
                <div style="padding: 5px;">
                    <asp:Literal runat="server" ID="litVendorPackageUpdateError"></asp:Literal>
                    <asp:Repeater runat="server" ID="rptVendorPackage">
                        <HeaderTemplate>
                            <table class="pricing_table">
                                <tr>
                                    <th>ID</th>
                                    <th>Vendor</th>
                                    <th>Vendor Package #</th>
                                    <th>Active</th>
                                    <th>Price</th>
                                    <th>&nbsp;</th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr runat="server" id="trVendorPackageItem">
                                <td>
                                    <asp:Label runat="server" ID="lblVendorPackageID" Text='<%# DataBinder.Eval(Container.DataItem, "VendorPackageID") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblVendorPackageVendor" Text='<%# DataBinder.Eval(Container.DataItem, "VendorID") %>'></asp:Label>
                                    <asp:TextBox runat="server" ID="txtVendorPackageVendorEdit" Text='<%# DataBinder.Eval(Container.DataItem, "VendorID") %>' Visible="false" Width="180"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblVendorPackageNumber" Text='<%# DataBinder.Eval(Container.DataItem, "VendorPN")%>'></asp:Label></div>
                                    <asp:TextBox runat="server" ID="txtVendorPackageNumberEdit" Text='<%# DataBinder.Eval(Container.DataItem, "VendorPN")%>' Visible="false" Width="120"></asp:TextBox>
                                </td>
                                <td style="text-align: center;">
                                    <asp:CheckBox runat="server" ID="chkVendorPackageActive" Checked='<%# DataBinder.Eval(Container.DataItem, "Active")%>' Enabled="false" />
                                </td>
                                <td style="text-align: center; width: 70px;">
                                    <asp:LinkButton runat="server" ID="lnkCost" OnCommand="lnkCost_Command" CommandName="view_cost" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "VendorPackageID") %>' Text="View"></asp:LinkButton>
                                </td>
                                <td style="text-align: center;">
                                    <div runat="server" id="divVendorPackageEditButton">
                                        <asp:ImageButton runat="server" ID="btnVendorPackageEdit" ImageUrl="//ssel-apps.eecs.umich.edu/static/images/edit.png" AlternateText="Edit" ToolTip="Edit" CommandName="edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "VendorPackageID")%>' OnCommand="btnVendorPackageEdit_Command" />
                                    </div>
                                    <div runat="server" id="divVendorPackageSaveCancelButton" visible="false">
                                        <asp:ImageButton runat="server" ID="btnVendorPackageEditSave" ImageUrl="//ssel-apps.eecs.umich.edu/static/images/ok.png" AlternateText="Save" ToolTip="Save" CommandName="save" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "VendorPackageID")%>' OnCommand="btnVendorPackageEdit_Command" />
                                        <asp:ImageButton runat="server" ID="btnVendorPackageEditCancel" ImageUrl="//ssel-apps.eecs.umich.edu/static/images/cancel.png" AlternateText="Cancel" ToolTip="Cancel" CommandName="cancel" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "VendorPackageID")%>' OnCommand="btnVendorPackageEdit_Command" />
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtAddVendorPackageVendor" Width="180"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtAddVendorPackageNumber" Width="120"></asp:TextBox>
                                </td>
                                <td colspan="3" style="vertical-align: middle;">
                                    <table style="border-collapse: collapse; margin: 0px;">
                                        <tr>
                                            <td style="vertical-align: middle; border: none; padding: 0px;">
                                                <asp:ImageButton runat="server" ID="btnVendorPackageAdd" ImageUrl="//ssel-apps.eecs.umich.edu/static/images/add.png" AlternateText="Add" ToolTip="Add" OnClick="btnVendorPackageAdd_Click" />
                                            </td>
                                            <td style="vertical-align: middle; border: none; padding: 0px 0px 0px 5px; color: #FF0000;">
                                                <asp:Literal runat="server" ID="litVendorPackageAddError"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <div runat="server" id="divPrice" visible="false" style="border: 1px solid #808080; margin-bottom: 10px;">
                <div class="boxtitle">
                    Price - Vendor Package ID:&nbsp;
                    <asp:Literal runat="server" ID="litPriceVendorPackageID"></asp:Literal>
                </div>
                <div style="padding: 5px;">
                    <asp:Repeater runat="server" ID="rptPrice" OnItemDataBound="rptPrice_ItemDataBound">
                        <HeaderTemplate>
                            <table class="pricing_table">
                                <tr>
                                    <th>ID</th>
                                    <th>Break Qty</th>
                                    <th>Package Cost</th>
                                    <th>Package Markup</th>
                                    <th>Package Price</th>
                                    <th>Date Active</th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr runat="server" id="trPriceItem">
                                <td>
                                    <asp:Label runat="server" ID="lblPriceID" Text='<%# DataBinder.Eval(Container.DataItem, "PriceID") %>'></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label runat="server" ID="lblPriceBreakQty" Text='<%# DataBinder.Eval(Container.DataItem, "PriceBreakQuantity") %>'></asp:Label>
                                    <asp:TextBox runat="server" ID="txtPriceBreakQtyEdit" Text='<%# DataBinder.Eval(Container.DataItem, "PriceBreakQuantity") %>' Visible="false" Width="180"></asp:TextBox>
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label runat="server" ID="lblPricePackageCost" Text='<%# DataBinder.Eval(Container.DataItem, "PackageCost")%>'></asp:Label>
                                    <asp:TextBox runat="server" ID="txtPricePackageCostEdit" Text='<%# DataBinder.Eval(Container.DataItem, "PackageCost")%>' Visible="false" Width="120"></asp:TextBox>
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label runat="server" ID="lblPricePackageMarkup" Text='<%# DataBinder.Eval(Container.DataItem, "PackageMarkup")%>'></asp:Label>
                                    <asp:TextBox runat="server" ID="txtPricePackageMarkupEdit" Text='<%# DataBinder.Eval(Container.DataItem, "PackageMarkup")%>' Visible="false" Width="120"></asp:TextBox>
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label runat="server" ID="lblPricePackagePrice" Text='<%# DataBinder.Eval(Container.DataItem, "PackagePrice")%>'></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label runat="server" ID="lblPriceDateActive" Text='<%# DataBinder.Eval(Container.DataItem, "DateActive")%>'></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtAddPriceBreakQty" Width="60"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtAddPricePackageCost" Width="120"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtAddPricePackageMarkup" Width="120"></asp:TextBox>
                                </td>
                                <td colspan="2" style="vertical-align: middle;">
                                    <table style="border-collapse: collapse; margin: 0px;">
                                        <tr>
                                            <td style="vertical-align: middle; border: none; padding: 0px;">
                                                <asp:ImageButton runat="server" ID="btnPriceAdd" ImageUrl="//ssel-apps.eecs.umich.edu/static/images/add.png" AlternateText="Add" ToolTip="Add" OnClick="btnPriceAdd_Click" />
                                            </td>
                                            <td style="vertical-align: middle; border: none; padding: 0px 0px 0px 5px; color: #FF0000;">
                                                <asp:Literal runat="server" ID="litPriceAddError"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <div style="float: right; margin-top: 10px;">
                <asp:Button runat="server" ID="btnReturn" Text="Return To Item Detail" OnClick="btnReturn_Click" />
            </div>
            <div style="clear: both;">
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
