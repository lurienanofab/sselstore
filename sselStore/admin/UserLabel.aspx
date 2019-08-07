<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin/StoreAdminMaster.Master" CodeBehind="UserLabel.aspx.vb" Inherits="sselStore.Admin.UserLabel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .label-form-table {
            border-spacing: 3px;
            border-collapse: separate;
        }

            .label-form-table td {
                padding: 3px;
            }

        .label-table td {
            padding: 20px;
        }

        .tblCustomLabel,
        .tblDetails {
            border-collapse: separate;
            border-spacing: 3px;
            width: 400px;
        }

            .tblCustomLabel td,
            .tblDetails td {
                padding: 3px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="placeholderlayout">
        <h2 class="block">Label Data</h2>

        <div style="border: 1px; border-width: 50px; border-left: 50px;">
            <table class="label-form-table">
                <tr class="trCustom">
                    <td style="width: 160px;">Custom Label</td>
                    <td>
                        <input type="checkbox" id="chkCustomLabel" class="chkCustomLabel" />
                    </td>
                </tr>
            </table>
            <table id="tblCustomLabel" class="tblCustomLabel" style="display: none;">
                <tr class="trCustomText1">
                    <td style="width: 160px;">Top Text</td>
                    <td>
                        <input type="text" id="txtTextTop" class="txtTextTop" />
                    </td>
                </tr>
                <tr class="trCustomText2">
                    <td>Bottom Text  </td>
                    <td>
                        <input type="text" id="txtTextBottom" class="txtTextBottom" />
                    </td>
                </tr>
            </table>
            <table id="tblDetails" class="tblDetails">
                <tr>
                    <td style="width: 160px;">* Select User Name:</td>
                    <td>
                        <asp:DropDownList ID="ddlUsers" runat="server" AutoPostBack="True" DataTextField="Text" DataValueField="Value" CssClass="ddlUsers">
                            <asp:ListItem>Select User Name</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr style="display: none">
                    <td>First Name:</td>
                    <td>
                        <asp:Label ID="lblUserFirstName" class="lblUserFirstName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr style="display: none">
                    <td>Last Name:</td>
                    <td>
                        <asp:Label ID="lblUserLastName" class="lblUserLastName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr style="display: none">
                    <td>User Account</td>
                    <td>
                        <asp:DropDownList ID="ddlAccounts" runat="server" AutoPostBack="True" DataTextField="AccountID" DataValueField="AccountID" AppendDataBoundItems="True">
                            <asp:ListItem>Select Account</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr style="display: none">
                    <td>Start Date</td>
                    <td>
                        <asp:Label ID="lblStartDate" runat="server" class="startDate"></asp:Label>
                    </td>
                </tr>
                <tr style="display: none">
                    <td>External/Internal</td>
                    <td>
                        <asp:Label ID="lblExternalInternal" class="lblExternalInternal" runat="server"></asp:Label>
                    </td>
                </tr>

                <tr style="display: none">
                    <td>Staff</td>
                    <td>
                        <asp:Label ID="lblStaff" runat="server" class="lblStaff"></asp:Label>
                    </td>
                </tr>
                <tr class="trmanagers">
                    <td>* Select Manager:</td>
                    <td>
                        <asp:DropDownList ID="ddlAllManagers" runat="server" DataTextField="LName" DataValueField="ClientID" class="ddlAllManagers" />
                    </td>
                </tr>
                <tr class="trmngprof" style="display: none">
                    <td>Manager is Prof</td>
                    <td>
                        <input type="checkbox" id="chkProf" class="chkProf" />
                    </td>
                </tr>
                <tr class="trorg">
                    <td>User Organization</td>
                    <td>
                        <asp:DropDownList ID="ddlOrgs" class="ddlOrgs" runat="server" DataTextField="OrgName" DataValueField="OrgID" />
                    </td>
                </tr>
                <tr class="trintern">
                    <td>LNF Staff</td>
                    <td>
                        <input type="checkbox" id="chkLNFStaff" class="chkLNFStaff" />
                    </td>
                </tr>
                <tr class="trintern">
                    <td>LNF Intern</td>
                    <td>
                        <input type="checkbox" id="chkLNFIntern" class="chkLNFIntern" />
                    </td>
                </tr>
            </table>

            <br />
        </div>
        <table class="label-table" border="1">
            <tr>
                <th>Name Tag Label </th>
                <th>Mini Backside Label</th>
            </tr>
            <tr>
                <td style="background-color: #f0f0f0;">
                    <div id="labelImageDiv">
                        <img id="labelImage" src="//ssel-apps.eecs.umich.edu/static/images/onepixel.png" alt="Label Preview" />
                    </div>
                </td>
                <td style="background-color: #f0f0f0;">
                    <div id="labelMiniImageDiv">
                        <img id="labelMiniImage" src="//ssel-apps.eecs.umich.edu/static/images/onepixel.png" alt="Label Preview" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <input type="button" id="btnMainPrint" value="Print Complete Label" />
                </td>
                <td style="text-align: center;">
                    <input type="button" id="btnMiniPrint" value="Print Mini Label" />
                </td>
            </tr>
        </table>

        <br />
        <div id="printersDiv">
            <label for="printersSelect">Printer:</label>
            <select id="printersSelect"></select>
        </div>
        <br />

        <%--<asp:Button runat="server" ID="btnTest2" Text="Testing only for PDF Generation" OnClick="BtnTest2_Click" />--%>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
    <script src="../scripts/LabelUtils.js"></script>
    <script src="../scripts/DYMO.Label.Framework.latest.js"></script>
</asp:Content>
