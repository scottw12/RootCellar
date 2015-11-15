<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="vacation.aspx.cs" Inherits="admin_vacation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    <h3>
        <asp:Literal runat="server" ID="name"></asp:Literal></h3>
    <br />
    <asp:Panel runat="server" ID="calpanel">

        <div>
            <asp:Label ID="Label2" Text="Select Vacation Week:" runat="server"></asp:Label>
            <asp:DropDownList runat="server" ID="WeekList"></asp:DropDownList><br />
            <br />
            <asp:Button ID="btnAddNewVacation" runat="server" Text="Add New Vacation" OnClick="btnAddNewVacation_Click" />
        </div>
        <%--<asp:Literal runat="server" ID="Literal1"></asp:Literal><br />
        <asp:DropDownList runat="server" ID="WeekList" AutoPostBack="True"></asp:DropDownList>
        <br />
        <br />
        <br />
        <br />
        --%>
        <asp:GridView ID="gvVacation" runat="server" AllowPaging="false" AllowSorting="false"
            class="table table-striped table-hover table-bordered" data-ride="datatables"
            AutoGenerateColumns="False" CellPadding="4" DataKeyNames="VID" OnRowCommand="gvVacation_RowCommand"
            GridLines="Both" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
            ForeColor="#333333">
            <Columns>
                <asp:TemplateField HeaderText="VID" ShowHeader="False" Visible="False" SortExpression="VID">
                    <ItemTemplate>
                        <asp:Label ID="VID" runat="server" Text='<%#Eval("VID")%>' />
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
                <asp:BoundField DataField="VacationWeek" HeaderText="Vacation Week" SortExpression="VacationWeek" />
                <asp:TemplateField HeaderText="Edit" ShowHeader="False" Visible="false">
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
        <asp:Button runat="server" ID="Submit" Text="Back" PostBackUrl="~/admin/Subscribers.aspx" />
    </asp:Panel>
</asp:Content>

