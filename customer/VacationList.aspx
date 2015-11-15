<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="VacationList.aspx.cs" Inherits="customer_VacationList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="gvVacation" runat="server" AllowPaging="True" AllowSorting="false"
                        class="table table-striped table-hover table-bordered" data-ride="datatables"
                        AutoGenerateColumns="False" CellPadding="4" DataKeyNames="VID"
                        GridLines="Both" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
                        ForeColor="#333333" OnRowCommand="gvVacation_RowCommand" OnPageIndexChanging="gvVacation_PageIndexChanging">                      
                        <Columns>
                            <asp:TemplateField HeaderText="VID" ShowHeader="False" Visible="False" SortExpression="VID">
                                <ItemTemplate>
                                    <asp:Label ID="VID" runat="server" Text='<%#Eval("VID")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Sr.No</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Vacation" HeaderText="Vacation" SortExpression="Vacation" />
                            <asp:BoundField DataField="VacationDate" HeaderText="Vacation Date"  dataformatstring="{0:MM/dd/yyyy}" SortExpression="VacationDate" />
                            <asp:BoundField DataField="DeliveryBoy" HeaderText="Delivery Boy" SortExpression="DeliveryBoy" />
                            <asp:BoundField DataField="ContactNumber" HeaderText="Contact Number" SortExpression="ContactNumber" />
                            <asp:TemplateField HeaderText="Edit" ShowHeader="False">
                                <ItemTemplate>                                    
                                    <asp:ImageButton ID="Edit" runat="server" ImageUrl="~/images/apptrue.gif" CausesValidation="false"
                                        CommandArgument='<%#Eval("VID")%>' CommandName="Edit1" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>                                    
                                    <asp:ImageButton ID="ImgDelete" runat="server" ImageUrl="~/images/appfalse.gif" OnClientClick="return ConfirmOnDelete();"
                                        CommandArgument='<%#Eval("VID")%>' CausesValidation="false" CommandName="Delete1" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
</asp:Content>

