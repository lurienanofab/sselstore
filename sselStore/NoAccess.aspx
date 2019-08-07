<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NoAccess.aspx.vb" Inherits="sselStore.NoAccess" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>No Access</title>
    <style>
        body {
            font-family: Arial;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top: 10px; padding: 10px;">
            You do not have access to the store at this time.
            <asp:Literal runat="server" ID="litContact"></asp:Literal>
        </div>
    </form>
</body>
</html>
