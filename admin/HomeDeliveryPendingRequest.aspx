<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="HomeDeliveryPendingRequest.aspx.cs" Inherits="admin_HomeDeliveryPendingRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script type="text/javascript" lang="javascript">
         function Error() {
             //alert("You have no permission to access this. Please Contact to admin");
             //window.location = "../Admin/Default.aspx";
         }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h3>Home Delivery Pending Requests</h3>
    <asp:GridView ID="gvHomeDelivery" runat="server"
        class="table table-striped table-hover table-bordered" data-ride="datatables"
        AutoGenerateColumns="False" CellPadding="4" DataKeyNames="HomeDeliveryRecordID"
        GridLines="Both" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
        ForeColor="#333333" OnRowCommand="gvHomeDelivery_RowCommand" >
        <Columns>
            <asp:TemplateField HeaderText="HomeDeliveryRecordID" ShowHeader="False" Visible="False" SortExpression="HomeDeliveryRecordID">
                <ItemTemplate>
                    <asp:Label ID="HomeDeliveryRecordID" runat="server" Text='<%#Eval("HomeDeliveryRecordID")%>' />
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
            <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
            <asp:BoundField DataField="Request" HeaderText="Request" SortExpression="Request" />
            <asp:TemplateField HeaderText="View/Approve/Deny" ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="Edit" runat="server"  CausesValidation="false" CommandArgument='<%#Eval("HomeDeliveryRecordID")%>' CommandName="Edit1">Edit</asp:LinkButton>
                    <%--<asp:ImageButton ID="Edit" runat="server" ImageUrl="~/images/apptrue.gif" CausesValidation="false"
                        CommandArgument='<%#Eval("HomeDeliveryRecordID")%>' CommandName="Edit1" />--%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>

