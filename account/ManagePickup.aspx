<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ManagePickup.aspx.cs" Inherits="account_ManagePickup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }

        .auto-style2 {
            width: 223px;
        }
    </style>
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
    </script>6
    <script type="text/javascript" lang="javascript">
        function Save() {
            alert("Store Added Successfully");
            window.location = "ManagePickup.aspx";
        }
    </script>
    <script type="text/javascript" lang="javascript">
        function Update() {
            alert("Store Updated Successfully");
            window.location = "ManagePickup.aspx";
        }
    </script>
    <script type="text/javascript" lang="javascript">
        function DisableWeekends(sender, args) {
            for (var i = 0; i < sender._days.all.length; i++) {
                for (var j = 0; j < 6; j++) {
                    if (sender._days.all[i].id == "DatePicker_CalendarExtender_day_" + j + "_5") {
                        sender._days.all[i].disabled = true;
                        sender._days.all[i].innerHTML = "<div>" + sender._days.all[i].innerText + "</div>";
                    }

                    if (sender._days.all[i].id == "DatePicker_CalendarExtender_day_" + j + "_6") {
                        sender._days.all[i].disabled = true;
                        sender._days.all[i].innerHTML = "<div>" + sender._days.all[i].innerText + "</div>";
                    }
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <br />
    <table id="table1" class="auto-style1">
        <tr>
            <td class="auto-style2">Select Day:</td>
            <td>
                <asp:DropDownList ID="ddlWeek" runat="server" >
                </asp:DropDownList>
                <%--                <telerik:RadDatePicker ID="DatePicker" Width="50%" DateInput-ReadOnly="true" runat="server">
                   
                </telerik:RadDatePicker>--%>
                <%--                 <asp:TextBox ID="DatePicker" runat="server" />
<ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="DatePicker" Format="dd/MM/yyyy" runat="server">
</ajax:CalendarExtender>  --%>

                <%--<asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="DatePicker" ErrorMessage="Please Select Date"></asp:RequiredFieldValidator>--%></td>
        </tr>
        <tr>
            <td class="auto-style2">Select New Pickup Point:</td>
            <td>
                <asp:DropDownList ID="ddlStore" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="auto-style2">Select Pickup Day:</td>
            <td>
                <asp:DropDownList runat="server" ID="PickupDayList" ></asp:DropDownList></td>
        </tr>
        <tr>
            <td class="auto-style2">Do you want to update <span data-dobid="hdw">permanently</span>?</td>
            <td>
                <asp:CheckBox ID="cbPermanent" runat="server" Text="Yes" />
            </td>
        </tr>
        <tr>
            <td class="auto-style2">
                <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update Pickup" />
                <asp:Button ID="btnCancel" PostBackUrl="~/account/Default.aspx" runat="server" CausesValidation="false" Text="Back" />
            </td>
            <td>&nbsp;</td>
        </tr>
    </table>

    <p>
        <asp:GridView ID="gvPickupChange" runat="server"
            class="table table-striped table-hover table-bordered" data-ride="datatables"
            AutoGenerateColumns="False" CellPadding="4" DataKeyNames="PickupID" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
            ForeColor="#333333" OnRowCommand="gvPickupChange_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="PickupID" ShowHeader="False" Visible="False" SortExpression="PickupID">
                    <ItemTemplate>
                        <asp:Label ID="PickupID" runat="server" Text='<%#Eval("PickupID")%>' />
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
                <asp:BoundField DataField="PickupDate" HeaderText="Pickup Week" SortExpression="PickupDate" />
                <asp:BoundField DataField="PickupPoint" HeaderText="Pickup Point" SortExpression="PickupPoint" />
                <asp:BoundField DataField="PickupDay" HeaderText="Pickup Day" SortExpression="PickupDay" />
                <asp:TemplateField HeaderText="Delete">
                    <ItemTemplate>
                        <asp:ImageButton ID="ImgDelete" runat="server" ImageUrl="~/images/appfalse.gif" OnClientClick="return ConfirmOnDelete();"
                            CommandArgument='<%#Eval("PickupID")%>' CausesValidation="false" CommandName="Delete1" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>

    </p>

</asp:Content>

