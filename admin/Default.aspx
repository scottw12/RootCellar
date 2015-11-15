<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="admin_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 300px;
            margin-right: auto;
            margin-left: auto;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Root Cellar Admin</h1>
    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    <table>
        <tr>
            <td class="auto-style1"><a href="Subscribers.aspx">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/subscribers.png" /><br />
                Current Subscribers</a></td>
            <td class="auto-style1"><a href="New-Subscriber.aspx">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/images/new.png" /><br />
                Create New Subscriber</a></td>
            <td class="auto-style1"><a href="notes.aspx">
                <asp:Image ID="Image3" runat="server" ImageUrl="~/images/notes.png" /><br />
                Subscriber Notes</a></td>

        </tr>

        <tr>
            <td class="auto-style1"><a href="Pickups.aspx">
                <asp:Image ID="Image4" runat="server" ImageUrl="~/images/pickup.png" /><br />
                Weekly Pickups</a></td>
            <td class="auto-style1"><a href="Summary.aspx">
                <asp:Image ID="Image5" runat="server" ImageUrl="~/images/summary.png" /><br />
                Weekly Summary</a></td>
            <td class="auto-style1"><a runat="server" id="AdminLink" href="../Admin/Admin.aspx">
                <asp:Image ID="Image6" runat="server" ImageUrl="~/images/admin.png" /><br />
                Admin</a></td>
        </tr>

        <tr>
            <td class="auto-style1"><a href="../Admin/Reports.aspx">
                <asp:Image ID="Image7" runat="server" ImageUrl="~/images/vacation.png" /><br />
                View Reports</a></td>
           <td class="auto-style1"><a href="../Admin/ProductsNew.aspx">
                <asp:Image ID="Image8" runat="server" ImageUrl="~/images/Fruits.jpg" Height="75px" Width="86px"/><br />
                Add Products</a></td>
             <td class="auto-style1"><a runat="server" id="A1" href="~/admin/HomeDeliveryPendingRequest.aspx">
                <asp:Image ID="Image9" runat="server" ImageUrl="~/images/pickup.png"  Height="75px" Width="86px"/><br />
                Delivery Report</a></td>
        </tr>

        <tr>
            <td class="auto-style1"><a href="../Admin/HomeDelivery.aspx">
                <asp:Image ID="Image10" runat="server" ImageUrl="~/images/new.png" /><br />
                Delivery Time</a></td>
          <%-- <td class="auto-style1"><a href="../Admin/ProductsNew.aspx">
                <asp:Image ID="Image11" runat="server" ImageUrl="~/images/Fruits.jpg" Height="75px" Width="86px"/><br />
                Add Products</a></td>
             <td class="auto-style1"><a runat="server" id="A2" href="Admin.aspx">
                <asp:Image ID="Image12" runat="server" ImageUrl="~/images/Fruits.jpg" /><br />
                Admin</a></td>--%>
        </tr>
    </table>


    <br />
    <br />
    <br />
    <br />

</asp:Content>

