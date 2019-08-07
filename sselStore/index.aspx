<%@ Page Title="LNF Store" Language="vb" AutoEventWireup="false" MasterPageFile="~/StoreMaster.Master" CodeBehind="index.aspx.vb" Inherits="sselStore.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="placeholderlayout">
        <h1>Welcome to the LNF Store</h1>
        <div style="padding-top: 10px; padding-bottom: 20px;">
            You are logged in as
            <asp:Label ID="lblUserName" runat="server"></asp:Label>
            - Please logout if this is not your identity.
        </div>
        <div class="store-news">
            <table>
                <tr>
                    <td style="vertical-align: middle;">
                        <h2 class="block">LNF News</h2>
                    </td>
                    <td style="vertical-align: middle;" class="news-edit">
                        <asp:PlaceHolder runat="server" ID="phAdminEdit">(<a href="#" class="edit-link">edit</a><a href="." class="cancel-link" style="display: none;">cancel</a>)
                        </asp:PlaceHolder>
                    </td>
                </tr>
            </table>
            <div class="store-news-editor" style="display: none;">
                <textarea class="store-news-textarea" style="width: 100%; height: 450px" cols="5" rows="5"></textarea>
            </div>
            <div class="store-news-content" style="min-height: 80px;">
                <span class="nodata">Loading...</span>
            </div>
        </div>
        <div class="store-map">
            <img src="images/store-location.jpg" alt="Store Location Map" />
        </div>
        <div class="debug">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
    <script type="text/javascript">
        var ed = tinymce.init({
            selector: ".store-news-textarea",
            plugins: "save",
            toolbar: "save",
            save_onsavecallback: function (e) {
                var content = tinymce.DOM.encode(e.getContent());

                $.ajax({
                    "url": "ajax/news.ashx",
                    "type": "POST",
                    "data": { "news": content }
                }).done(function (data, textStatus, jqXHR) {
                    $('.cancel-link').hide();
                    $('.edit-link').show();
                    $('.store-news-editor').hide();
                    $('.store-news-content').show();
                    $(".store-news-content").html(data);
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    var err = $(jqXHR.responseText).filter("title").text() || errorThrown;
                    alert(err);
                });
            }
        }).then(function (editors) {
            var ed = editors[0];

            $('.cancel-link').click(function (e) {
                e.preventDefault();
                $(this).hide();
                $('.edit-link').show();
                $('.store-news-editor').hide();
                $('.store-news-content').show();
            });

            $('.edit-link').click(function (e) {
                e.preventDefault();
                $(this).hide();
                $('.cancel-link').show();
                $('.store-news-content').hide();
                $.ajax("ajax/news.ashx").done(function (data, textStatus, jqXHR) {
                    ed.setContent(data);
                    $('.store-news-editor').show();
                });
            });

            $(".store-news-content").load("ajax/news.ashx");
        });
    </script>
</asp:Content>
