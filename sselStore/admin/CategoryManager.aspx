<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin/StoreAdminMaster.Master" CodeBehind="CategoryManager.aspx.vb" Inherits="sselStore.Admin.CategoryManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .repeater th, .repeater td {
            vertical-align: middle;
        }

        .cat_image_container {
            position: relative;
            width: 100px;
        }

        .cat_image {
            width: 100px;
            height: 100px;
        }

        .img_button {
            position: static;
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divOpenImage">
        <asp:HiddenField runat="server" ID="hidOpenImage" />
    </div>
    <div class="placeholderlayout">
        <h1>Category Manager</h1>
        <div style="margin-bottom: 5px;">
            Parent:
            <asp:DropDownList runat="server" ID="ddlParent" DataValueField="CatID" DataTextField="CatName" AutoPostBack="true" OnSelectedIndexChanged="ddlParent_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div class="error">
            <asp:Literal runat="server" ID="litErrorMessageTop"></asp:Literal>
        </div>
        <asp:Repeater runat="server" ID="rptCategories">
            <HeaderTemplate>
                <table class="repeater">
                    <tr>
                        <th style="width: 100px; padding: 0px;">Image</th>
                        <th>CatID</th>
                        <th>ParentID</th>
                        <th>Hierarchy</th>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Active</th>
                        <th>Display</th>
                        <th style="width: 40px;">&nbsp;</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td class="item" style="text-align: center; width: 100px; padding: 0px;">
                        <div class="cat_image_container">
                            <img class="img_button" id='<%# "link" + DataBinder.Eval(Container.DataItem, "CatID").ToString() %>' onclick='<%# "javascript:toggleImage(" + DataBinder.Eval(Container.DataItem, "CatID").ToString() + ")" %>' src="//ssel-apps.eecs.umich.edu/static/images/view.png" border="0" alt="View" />
                            <div class="img_container" id='<%# "img" + DataBinder.Eval(Container.DataItem, "CatID").ToString() %>' style="display: none;">
                                <asp:Image runat="server" ID="imgCat" ImageUrl='<%# CatImgURL(DataBinder.Eval(Container.DataItem, "CatID").ToString()) %>' CssClass="cat_image" />
                            </div>
                        </div>
                    </td>
                    <td class="item">
                        <asp:Label runat="server" ID="lblCatID" Text='<%# DataBinder.Eval(Container.DataItem, "CatID") %>'></asp:Label>
                    </td>
                    <td class="item">
                        <asp:Label runat="server" ID="lblParentID" Text='<%# DataBinder.Eval(Container.DataItem, "ParentID") %>'></asp:Label>
                        <asp:TextBox runat="server" ID="txtEditParentID" Text='<%# DataBinder.Eval(Container.DataItem, "ParentID") %>' Width="50" Visible="false"></asp:TextBox>
                    </td>
                    <td class="item">
                        <asp:Label runat="server" ID="lblHierarchy" Text='<%# DataBinder.Eval(Container.DataItem, "HierarchyLevel") %>'></asp:Label>
                        <asp:TextBox runat="server" ID="txtEditHierarchy" Text='<%# DataBinder.Eval(Container.DataItem, "HierarchyLevel") %>' Width="60" Visible="false"></asp:TextBox>
                    </td>
                    <td class="item">
                        <asp:Label runat="server" ID="lblName" Text='<%# DataBinder.Eval(Container.DataItem, "CatName") %>'></asp:Label>
                        <asp:TextBox runat="server" ID="txtEditName" Text='<%# DataBinder.Eval(Container.DataItem, "CatName") %>' Width="120" Visible="false"></asp:TextBox>
                    </td>
                    <td class="item">
                        <asp:Label runat="server" ID="lblDescription" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'></asp:Label>
                        <asp:TextBox runat="server" ID="txtEditDescription" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>' Width="200" Visible="false"></asp:TextBox>
                    </td>
                    <td class="item" style="text-align: center;">
                        <asp:CheckBox runat="server" ID="chkActive" Checked='<%# DataBinder.Eval(Container.DataItem, "Active") %>' Enabled="false"></asp:CheckBox>
                    </td>
                    <td class="item" style="text-align: center;">
                        <asp:CheckBox runat="server" ID="chkDisplay" Checked='<%# DataBinder.Eval(Container.DataItem, "StoreDisplay") %>' Enabled="false"></asp:CheckBox>
                    </td>
                    <td class="item" style="text-align: center; vertical-align: middle;">
                        <div runat="server" id="divEditButton">
                            <asp:ImageButton runat="server" ID="btnEdit" ImageUrl="//ssel-apps.eecs.umich.edu/static/images/edit.png" ToolTip="Edit" AlternateText="Edit" OnCommand="CategoryItem_Command" CommandName="edit" CommandArgument='<%# Container.ItemIndex %>' />
                        </div>
                        <div runat="server" id="divSaveCancelButton" visible="false">
                            <asp:ImageButton runat="server" ID="btnSave" ImageUrl="//ssel-apps.eecs.umich.edu/static/images/ok.png" ToolTip="Save" AlternateText="Save" OnCommand="CategoryItem_Command" CommandName="save" CommandArgument='<%# Container.ItemIndex %>' />
                            <asp:ImageButton runat="server" ID="btnCancel" ImageUrl="//ssel-apps.eecs.umich.edu/static/images/cancel.png" ToolTip="Cancel" AlternateText="Cancel" OnCommand="CategoryItem_Command" CommandName="cancel" CommandArgument='<%# Container.ItemIndex %>' />
                        </div>
                    </td>
                </tr>
                <tr runat="server" id="trUploadImage" visible="false">
                    <td colspan="9" class="item">Upload Image:&nbsp;
                        <asp:FileUpload runat="server" ID="fupCatImage" />
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr>
                    <td class="alt_item" style="text-align: center; width: 100px; padding: 0px;">
                        <div class="cat_image_container">
                            <img class="img_button" id='<%# "link" + DataBinder.Eval(Container.DataItem, "CatID").ToString() %>' onclick='<%# "javascript:toggleImage(" + DataBinder.Eval(Container.DataItem, "CatID").ToString() + ")" %>' src="//ssel-apps.eecs.umich.edu/static/images/view.png" border="0" alt="View" />
                            <div class="img_container" id='<%# "img" + DataBinder.Eval(Container.DataItem, "CatID").ToString() %>' style="display: none;">
                                <asp:Image runat="server" ID="imgCat" ImageUrl='<%# CatImgURL(DataBinder.Eval(Container.DataItem, "CatID").ToString()) %>' CssClass="cat_image" />
                            </div>
                        </div>
                    </td>
                    <td class="alt_item">
                        <asp:Label runat="server" ID="lblCatID" Text='<%# DataBinder.Eval(Container.DataItem, "CatID") %>'></asp:Label>
                    </td>
                    <td class="alt_item">
                        <asp:Label runat="server" ID="lblParentID" Text='<%# DataBinder.Eval(Container.DataItem, "ParentID") %>'></asp:Label>
                        <asp:TextBox runat="server" ID="txtEditParentID" Text='<%# DataBinder.Eval(Container.DataItem, "ParentID") %>' Width="50" Visible="false"></asp:TextBox>
                    </td>
                    <td class="alt_item">
                        <asp:Label runat="server" ID="lblHierarchy" Text='<%# DataBinder.Eval(Container.DataItem, "HierarchyLevel") %>'></asp:Label>
                        <asp:TextBox runat="server" ID="txtEditHierarchy" Text='<%# DataBinder.Eval(Container.DataItem, "HierarchyLevel") %>' Width="60" Visible="false"></asp:TextBox>
                    </td>
                    <td class="alt_item">
                        <asp:Label runat="server" ID="lblName" Text='<%# DataBinder.Eval(Container.DataItem, "CatName") %>'></asp:Label>
                        <asp:TextBox runat="server" ID="txtEditName" Text='<%# DataBinder.Eval(Container.DataItem, "CatName") %>' Width="120" Visible="false"></asp:TextBox>
                    </td>
                    <td class="alt_item">
                        <asp:Label runat="server" ID="lblDescription" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'></asp:Label>
                        <asp:TextBox runat="server" ID="txtEditDescription" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>' Width="200" Visible="false"></asp:TextBox>
                    </td>
                    <td class="alt_item" style="text-align: center;">
                        <asp:CheckBox runat="server" ID="chkActive" Checked='<%# DataBinder.Eval(Container.DataItem, "Active") %>' Enabled="false"></asp:CheckBox>
                    </td>
                    <td class="alt_item" style="text-align: center;">
                        <asp:CheckBox runat="server" ID="chkDisplay" Checked='<%# DataBinder.Eval(Container.DataItem, "StoreDisplay") %>' Enabled="false"></asp:CheckBox>
                    </td>
                    <td class="alt_item" style="width: 40px; text-align: center; vertical-align: middle;">
                        <div runat="server" id="divEditButton">
                            <asp:ImageButton runat="server" ID="btnEdit" ImageUrl="//ssel-apps.eecs.umich.edu/static/images/edit.png" ToolTip="Edit" AlternateText="Edit" OnCommand="CategoryItem_Command" CommandName="edit" CommandArgument='<%# Container.ItemIndex %>' />
                        </div>
                        <div runat="server" id="divSaveCancelButton" visible="false">
                            <asp:ImageButton runat="server" ID="btnSave" ImageUrl="//ssel-apps.eecs.umich.edu/static/images/ok.png" ToolTip="Save" AlternateText="Save" OnCommand="CategoryItem_Command" CommandName="save" CommandArgument='<%# Container.ItemIndex %>' />
                            <asp:ImageButton runat="server" ID="btnCancel" ImageUrl="//ssel-apps.eecs.umich.edu/static/images/cancel.png" ToolTip="Cancel" AlternateText="Cancel" OnCommand="CategoryItem_Command" CommandName="cancel" CommandArgument='<%# Container.ItemIndex %>' />
                        </div>
                    </td>
                </tr>
                <tr runat="server" id="trUploadImage" visible="false">
                    <td colspan="9" class="alt_item">Upload Image:&nbsp;
                        <asp:FileUpload runat="server" ID="fupCatImage" />
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtAddParentID" Width="50" Visible="false"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtAddHierarchy" Width="60" Enabled="false" Text="0"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtAddName" Width="120"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtAddDescription" Width="200"></asp:TextBox>
                    </td>
                    <td colspan="3">
                        <asp:ImageButton runat="server" ID="btnAddCategory" ImageUrl="//ssel-apps.eecs.umich.edu/static/images/add.png" OnClick="btnAddCategory_Click" />
                    </td>
                </tr>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <div class="error">
            <asp:Literal runat="server" ID="litErrorMessageBottom"></asp:Literal>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
    <script>
        function toggleImage(catID) {
            var img = $('#img' + catID);
            var link = $('#link' + catID);
            var hid = $('#divOpenImage input[type="hidden"]');

            var display = img.css('display');

            $('.img_container').css('display', 'none');
            $('.img_button').css({ 'position': 'static' });
            $('.img_button').attr('src', '//ssel-apps.eecs.umich.edu/static/images/view.png');

            if (display == 'none') {
                img.css('display', 'block');
                link.css({ 'position': 'absolute', 'top': '0', 'right': '0' });
                link.attr('src', '//ssel-apps.eecs.umich.edu/static/images/close.png');
                hid.val(catID);
            }
            else {
                hid.val('');
            }
        }
    </script>
</asp:Content>
