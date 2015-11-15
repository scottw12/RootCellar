<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AssignRole.aspx.cs" Inherits="admin_AssignRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
    </style>
    <script type="text/javascript" lang="javascript">
        function Save() {
            alert("Role Assign Successfully");
            window.location = "../Admin/Default.aspx";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <p>
        <br />
        <table id="table1" class="auto-style1">
            <tr>
                <td>Role</td>
                <td>Allow Access</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCurrentSubscribers" runat="server" Text="1. Current Subscribers"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="cbCS" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCreateNewSubscriber" runat="server" Text="2. Create New Subscriber"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="cbCNS" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSubscriberNotes" runat="server" Text="3. Subscriber Notes"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="cbSN" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblWeeklyPickups" runat="server" Text="4. Weekly Pickups"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="cbWP" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblWeeklySummary" runat="server" Text="5. Weekly Summary"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="cbWS" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblAdmin" runat="server" Text="6. Admin"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="cbAdmin" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    7. View Report</td>
                <td>
                    <asp:CheckBox ID="cbVR" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    8. Add Products</td>
                <td>
                    <asp:CheckBox ID="cbAddProduct" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    9. Delivery</td>
                <td>
                    <asp:CheckBox ID="cbDelivery" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    10. Delivery Time</td>
                <td>
                    <asp:CheckBox ID="cbDeliveryTime" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" />
                </td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </p>

</asp:Content>

