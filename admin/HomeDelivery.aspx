<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="HomeDelivery.aspx.cs" Inherits="admin_HomeDelivery" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }

        .auto-style2 {
            width: 109px;
        }

        .auto-style3 {
            width: 383px;
        }

        .auto-style4 {
            width: 83px;
        }
    </style>
     <script type="text/javascript" lang="javascript">
         function Error() {
             //alert("You have no permission to access this. Please Contact to admin");
             //window.location = "../Admin/Default.aspx";
         }
    </script>
    <script type="text/javascript" lang="javascript">
        function Save() {
            alert("Record Added Successfully");
            window.location = "HomeDelivery.aspx";
        }
    </script>
    <script type="text/javascript" lang="javascript">
        function Update() {
            alert("Record Updated Successfully");
            window.location = "HomeDelivery.aspx";
        }
    </script>
    <script type="text/javascript" lang="javascript">
        function ConfirmOnDelete() {
            var res = confirm("Are you sure to delete?");
            if (res == true) {
                alert('Record Deleted Successfully');
                return true;
            }
            else
                return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <h1>Create Delivery Time Range</h1>
    <table id="table1" class="auto-style1">
        <tr>
            <td class="auto-style2">Delivery Time Name:</td>
            <td class="auto-style3">
                <asp:TextBox ID="txtLocation" runat="server" MaxLength="100" Height="60px" TextMode="MultiLine" Width="241px"></asp:TextBox>
                <br />
                <asp:RequiredFieldValidator ID="rfvLoc" runat="server" ControlToValidate="txtLocation" ErrorMessage="Please Insert Location"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="auto-style2">Day:</td>
            <td class="auto-style3">
                <asp:DropDownList ID="ddlDays" runat="server">
                    <asp:ListItem>Sunday</asp:ListItem>
                    <asp:ListItem>Monday</asp:ListItem>
                    <asp:ListItem>Tuesday</asp:ListItem>
                    <asp:ListItem>Wednesday</asp:ListItem>
                    <asp:ListItem>Thursday</asp:ListItem>
                    <asp:ListItem>Friday</asp:ListItem>
                    <asp:ListItem>Saturday</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="auto-style2">Start Time:</td>
            <td class="auto-style3">
                <telerik:RadTimePicker ID="rtpStartTime" runat="server"  Culture="English (United States)" >
                </telerik:RadTimePicker>
                <asp:RequiredFieldValidator ID="rfvST" runat="server" ControlToValidate="rtpStartTime" ErrorMessage="Please Insert Start Time"></asp:RequiredFieldValidator>
            </td>
            <td class="auto-style4">End Time:</td>
            <td>
                <telerik:RadTimePicker ID="rtpEndTime" runat="server"  Culture="English (United States)">
                </telerik:RadTimePicker>
                <asp:RequiredFieldValidator ID="rfvEnd" runat="server" ControlToValidate="rtpEndTime" ErrorMessage="Please Insert End Time"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="auto-style2"><span data-dobid="hdw">Available Stores</span></td>
            <td class="auto-style3">
                <asp:CheckBoxList ID="cblStores" runat="server">
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td class="auto-style2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save " />
            </td>
            <td class="auto-style3">&nbsp;</td>
        </tr>
    </table>
    <asp:GridView ID="gvHomeDelivery" runat="server"
        class="table table-striped table-hover table-bordered" data-ride="datatables"
        AutoGenerateColumns="False" CellPadding="4" DataKeyNames="HomeDeliveryId"
        GridLines="Both" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
        ForeColor="#333333" OnRowCommand="gvHomeDelivery_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="HomeDeliveryId" ShowHeader="False" Visible="False" SortExpression="HomeDeliveryId">
                <ItemTemplate>
                    <asp:Label ID="HomeDeliveryId" runat="server" Text='<%#Eval("HomeDeliveryId")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Sr.No
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location" />
            <asp:BoundField DataField="Day" HeaderText="Day" SortExpression="Day" />
            <asp:TemplateField HeaderText="Edit" ShowHeader="False">
                <ItemTemplate>
                    <asp:ImageButton ID="Edit" runat="server" ImageUrl="~/images/apptrue.gif" CausesValidation="false"
                        CommandArgument='<%#Eval("HomeDeliveryId")%>' CommandName="Edit1" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Delete">
                <ItemTemplate>
                    <asp:ImageButton ID="ImgDelete" runat="server" ImageUrl="~/images/appfalse.gif" OnClientClick="return ConfirmOnDelete();"
                        CommandArgument='<%#Eval("HomeDeliveryId")%>' CausesValidation="false" CommandName="Delete1" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>


</asp:Content>

