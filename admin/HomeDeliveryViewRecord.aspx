<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="HomeDeliveryViewRecord.aspx.cs" Inherits="admin_HomeDeliveryViewRecord" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
    </style>
    <script type="text/javascript" lang="javascript">
        function Save() {
            alert("Request Approved Successfully");
            window.location = "HomeDeliveryPendingRequest.aspx";
        }
    </script>
    <script type="text/javascript" lang="javascript">
        function Update() {
            alert("Request Denied Successfully");
            window.location = "HomeDeliveryPendingRequest.aspx";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Home Delivery Status</h1>
    <asp:DropDownList ID="ddlBestTime" runat="server" Visible="false"></asp:DropDownList><br />
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <table id="table1" class="auto-style1">
        <tr>
            <td>Home Delivery Request Status:</td>
            <td>
                <asp:Label ID="lblRequest" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Delivery Address:</td>
            <td>
                <asp:TextBox ID="txtDeliveryAddress" runat="server" Enabled="False" Height="46px" MaxLength="250" TextMode="MultiLine" Width="239px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Best Time:</td>
            <td>
                <asp:Label ID="lblBestTime" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Selected Location:</td>
            <td>
                <asp:Label ID="lblSelectdLocation" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Special Instruction:</td>
            <td>
                <asp:TextBox ID="txtSpeInstr" runat="server" Enabled="False" Height="46px" MaxLength="250" TextMode="MultiLine" Width="239px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <%--<td>Last Point:</td>
            <td>
                <asp:DropDownList ID="ddlStartDay" runat="server">
                    <asp:ListItem>Saunday</asp:ListItem>
                    <asp:ListItem>Monday</asp:ListItem>
                    <asp:ListItem>Tuesday</asp:ListItem>
                    <asp:ListItem>Wednesday</asp:ListItem>
                    <asp:ListItem>Thursday</asp:ListItem>
                    <asp:ListItem>Friday</asp:ListItem>
                    <asp:ListItem>Saturday</asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp; Start Time:
                <telerik:RadTimePicker ID="rtpStartTime" runat="server">
                </telerik:RadTimePicker>
                &nbsp;&nbsp;&nbsp; End Time:
                <telerik:RadTimePicker ID="rtpEndTime" runat="server">
                </telerik:RadTimePicker>
            </td>--%>
        </tr>
        <tr>
            <td>Home Delivery Charges($):</td>
            <td>
                <asp:TextBox ID="txtCharges" runat="server"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="FilTxtExtCode" runat="server" FilterType="Numbers,Custom" ValidChars="."
                    TargetControlID="txtCharges">
                </cc1:FilteredTextBoxExtender>
                &nbsp;
                <asp:RequiredFieldValidator ID="rfvCha" ControlToValidate="txtCharges" runat="server" ErrorMessage="Please Insert Charges" ValidationGroup="Mail"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>Mail:</td>
            <td>
                <asp:TextBox TextMode="MultiLine" ID="txtBody" runat="server" MaxLength="1000" Height="69px" Width="241px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvBo" runat="server" ControlToValidate="txtBody" ValidationGroup="Mail" ErrorMessage="Please Insert Mail Body"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnApprove" runat="server" OnClick="btnApprove_Click" Text="Approve &amp; Send Mail" ValidationGroup="Mail" />
            </td>
            <td>
                <asp:Button ID="btnDeny" runat="server" ValidationGroup="Mail" Text="Deny &amp; Send Mail" OnClick="btnDeny_Click" />
                &nbsp;
                <asp:Button ID="btnCancel" runat="server" PostBackUrl="~/admin/HomeDeliveryPendingRequest.aspx" Text="Cancel" />
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
</asp:Content>

