<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Reports.aspx.cs" Inherits="admin_Reports" EnableEventValidation="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }

        .auto-style2 {
            width: 100%;
        }

        .auto-style3 {
            width: 276px;
        }

        .auto-style4 {
            width: 276px;
        }

        .auto-style6 {
            width: 276px;
        }

        .auto-style7 {
            width: 276px;
        }

        .auto-style8 {
            width: 276px;
        }

        .auto-style9 {
            width: 276px;
        }

        .auto-style10 {
            width: 276px;
        }

        .auto-style11 { 
            width: 276px;
        }
    </style>
    <script type="text/javascript" lang="javascript">
        //function Error() {
        //    alert("You have no permission to access this. Please Contact to admin");
        //    window.location = "../Admin/Default.aspx";
        }
    </script>

    <script type="text/javascript">
        function openRadWindow() {
            var radwindow = $find('<%=RadWindow1.ClientID %>');
            radwindow.show();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <h1>Reports</h1>
    <br />
    <table id="table1" class="auto-style1">
        <tr>
            <td class="auto-style2">
                <table id="table2" class="auto-style1">
                    <tr>
                        <td class="auto-style3">Select report:</td>
                        <td>
                            <asp:DropDownList ID="ddlReport" runat="server" OnSelectedIndexChanged="ddlReport_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem>Allergies</asp:ListItem>
                                <asp:ListItem>Notes</asp:ListItem>
                                <asp:ListItem Text="Online Payment Tracking" Value="Time Tracking"></asp:ListItem>
                                <asp:ListItem Value="Home delivery">Delivery</asp:ListItem>
                                <asp:ListItem>Store</asp:ListItem>
                                <asp:ListItem>NPU</asp:ListItem>
                                <asp:ListItem>Vacation</asp:ListItem>
                                <asp:ListItem>Pickup Time Tracking</asp:ListItem>
                                <asp:ListItem Value="Last Week Report">Week End Report</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">Select Week:</td>
                        <td>
                            <asp:DropDownList ID="WeekList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="WeekList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">Select Store:</td>
                        <td>
                            <asp:DropDownList ID="ddlStore" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td class="auto-style3">Select Pickup Day:</td>
                        <td>
                            <asp:DropDownList ID="ddlPickupDay" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPickupDay_SelectedIndexChanged">
                                <asp:ListItem Value="Select Pickup Day">Select Pickup Day</asp:ListItem>
                                <asp:ListItem>Thursday</asp:ListItem>
                                <asp:ListItem>Friday</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
        <tr>
            <td>
                <div id="divAllergies" runat="server">
                    <table id="table3" class="auto-style1">
                        <tr>
                            <td class="auto-style4">
                                <asp:Label ID="lblCustomerName" runat="server" Text="User Name:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfv1" runat="server" ValidationGroup="a" ControlToValidate="txtCustomerName" ErrorMessage="Please Enter Name"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style4">
                                <asp:Button ID="btnSearchCustomer" runat="server" Text="Search" OnClick="btnSearchCustomer_Click" ValidationGroup="a" />
                            </td>
                            <td>
                                <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style4">
                                <asp:Button ID="btnView" runat="server" Text="Download Report" OnClick="btnView_Click" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                    <br />

                    <asp:GridView ID="gvAllergies" runat="server"
                        class="table table-striped table-hover table-bordered" data-ride="datatables"
                        AutoGenerateColumns="False" CellPadding="4" DataKeyNames="SubId"
                        GridLines="Both" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
                        ForeColor="#333333" OnPageIndexChanging="gvAllergies_PageIndexChanging" OnRowDataBound="gvAllergies_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="SubId" ShowHeader="False" Visible="False" SortExpression="SubId">
                                <ItemTemplate>
                                    <asp:Label ID="SubId" runat="server" Text='<%#Eval("SubId")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    Sr.No
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Username" HeaderText="User Name" SortExpression="Username" />
                            <asp:BoundField DataField="Allergies" HeaderText="Allergies" SortExpression="Allergies" />
                        </Columns>
                    </asp:GridView>
                </div>

                <div id="divNotes" runat="server">
                    <table id="table4" class="auto-style1">
                        <tr>
                            <td class="auto-style3">
                                <asp:Label ID="lblUserNameNotes" runat="server" Text="User Name:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUserNameNotes" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCustomerName" ErrorMessage="Please Enter Name" ValidationGroup="b"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style3">
                                <asp:Button ID="btnSearchNotes" runat="server" Text="Search" ValidationGroup="b" OnClick="btnSearchNotes_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnClearNotes" runat="server" Text="Clear" OnClick="btnClearNotes_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style3">
                                <asp:Button ID="btnView0" runat="server" Text="Download Report" OnClick="btnView_Click" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                    <br />
                    <asp:GridView ID="gvNotes" runat="server"
                        class="table table-striped table-hover table-bordered" data-ride="datatables"
                        AutoGenerateColumns="False" CellPadding="4" DataKeyNames="SubId"
                        GridLines="Both" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
                        ForeColor="#333333" OnPageIndexChanging="gvNotes_PageIndexChanging" OnRowDataBound="gvNotes_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="SubId" ShowHeader="False" Visible="False" SortExpression="SubId">
                                <ItemTemplate>
                                    <asp:Label ID="SubId" runat="server" Text='<%#Eval("SubId")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    Sr.No
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Username" HeaderText="User Name" SortExpression="Username" />
                            <asp:BoundField DataField="PermanentNotes" HeaderText="Permanent Notes" SortExpression="PermanentNotes" />
                            <asp:BoundField DataField="WeeklyNotes" HeaderText="Weekly Notes" SortExpression="WeeklyNotes" />
                        </Columns>
                    </asp:GridView>
                </div>


                <div id="divTimeTracking" runat="server">
                    <table id="table5" class="auto-style1">
                        <tr>
                            <td class="auto-style6">
                                <asp:Label ID="lblTTUserName" runat="server" Text="User Name:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTT" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtTT" ErrorMessage="Please Enter Name" ValidationGroup="c"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style6">
                                <asp:Button ID="btnSearchTT" runat="server" Text="Search" OnClick="btnSearchTT_Click" ValidationGroup="c" />
                            </td>
                            <td>
                                <asp:Button ID="btnCancelTT" runat="server" Text="Clear" CausesValidation="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style6">
                                <asp:Button ID="btnView1" runat="server" Text="Download Report" OnClick="btnView_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnLastWeekTT" runat="server" Text="Last Week Record" OnClick="btnLastWeekTT_Click" Visible="False" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:GridView ID="gvTimeTracking" runat="server"
                        class="table table-striped table-hover table-bordered" data-ride="datatables"
                        AutoGenerateColumns="False" CellPadding="4" DataKeyNames="SubId" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
                        ForeColor="#333333" OnPageIndexChanging="gvTimeTracking_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="SubId" ShowHeader="False" Visible="False" SortExpression="SubId">
                                <ItemTemplate>
                                    <asp:Label ID="SubId" runat="server" Text='<%#Eval("SubId")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    Sr.No
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Username" HeaderText="User Name" SortExpression="Username" />
                            <asp:BoundField DataField="PurchaseDate" HeaderText="Purchase Date" SortExpression="PurchaseDate" DataFormatString="{0:d}" />
                        </Columns>
                    </asp:GridView>
                </div>

                <div id="divHomeDelivery" runat="server">
                    <table id="table6" class="auto-style1">
                        <tr>
                            <td class="auto-style7">
                                <asp:Label ID="lblUserNameHD" runat="server" Text="User Name:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtHD" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtHD" ErrorMessage="Please Enter Name" ValidationGroup="d"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style7">Date: </td>
                            <td>
                                <telerik:RadDatePicker ID="dpHD" runat="server">
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style7">
                                <asp:Button ID="btnSearchHD" runat="server" Text="Search" ValidationGroup="d" OnClick="btnSearchHD_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnCancelHD" runat="server" Text="Clear" CausesValidation="false" OnClick="btnCancelHD_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style7">
                                <asp:Button ID="btnView2" runat="server" Text="Download Report" OnClick="btnView_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnLastWeekHD" runat="server" Text="Last Week Record" OnClick="btnLastWeekHD_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />

                    <asp:UpdatePanel ID="upGrdView" runat="server" UpdateMode="Conditional">
                        <%--<Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvHomeDelivery" EventName="OnRowCommand" />
                        </Triggers>--%>
                        <ContentTemplate>
                            <asp:GridView ID="gvHomeDelivery" runat="server" CssClass="GridViewClass"
                                data-ride="datatables"
                                AutoGenerateColumns="False" CellPadding="4" DataKeyNames="SubId"
                                GridLines="Both" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
                                ForeColor="#333333" OnPageIndexChanging="gvHomeDelivery_PageIndexChanging" OnRowDataBound="gvHomeDelivery_RowDataBound" OnRowCommand="gvHomeDelivery_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="SubId" ShowHeader="False" Visible="False" SortExpression="SubId">
                                        <ItemTemplate>
                                            <asp:Label ID="SubId" runat="server" Text='<%#Eval("SubId")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <HeaderTemplate>
                                            Sr.No
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Username" HeaderText="User Name" SortExpression="Username" />
                                    <asp:TemplateField HeaderText="Bounty">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfBounty" runat="server" Value='<%#Eval("Bounty")%>' />
                                            <asp:ImageButton ID="ImgBounty" runat="server" ImageUrl="~/images/appfalse.gif" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Barnyard">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfBarnYard" runat="server" Value='<%#Eval("BarnYard")%>' />
                                            <asp:ImageButton ID="ImgBarnYard" runat="server" ImageUrl="~/images/appfalse.gif" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ploughman">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfPloughman" runat="server" Value='<%#Eval("Ploughman")%>' />
                                            <asp:ImageButton ID="ImgPloughman" runat="server" ImageUrl="~/images/appfalse.gif" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:BoundField DataField="Bounty" HeaderText="Bounty" />
                                    <asp:BoundField DataField="BarnYard" HeaderText="BarnYard" />
                                    <asp:BoundField DataField="Ploughman" HeaderText="Ploughman" />--%>
                                    <asp:BoundField DataField="BestTime" HeaderText="Delivery Time Range" />
                                    <asp:TemplateField HeaderText="Details">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgDelete" runat="server" ImageUrl="~/images/Help.png"
                                                CommandArgument='<%#Eval("SubId")%>' CausesValidation="false" CommandName="Download" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>


                            <telerik:RadWindow ID="RadWindow1" runat="server" Width="450px" Height="450px" NavigateUrl="http://www.telerik.com">
                                <ContentTemplate>
                                    <asp:GridView ID="gvViewProducts" runat="server" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                        </Columns>

                                    </asp:GridView>
                                </ContentTemplate>
                            </telerik:RadWindow>

                        </ContentTemplate>
                    </asp:UpdatePanel>



                </div>


                <div id="divStore" runat="server">
                    <table id="table7" class="auto-style1">
                        <tr>
                            <td class="auto-style8">
                                <asp:Label ID="lblStoreUN" runat="server" Text="User Name:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtStore" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtStore" ErrorMessage="Please Enter Name" ValidationGroup="f"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">Date: &nbsp;&nbsp;</td>
                            <td>
                                <telerik:RadDatePicker ID="dpStore" runat="server">
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">
                                <asp:Button ID="btnStoreFind" runat="server" Text="Search" OnClick="btnStoreFind_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnStoreCancel" runat="server" Text="Clear" CausesValidation="false" OnClick="btnStoreCancel_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">
                                <asp:Button ID="btnView3" runat="server" Text="Download Report" OnClick="btnView_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnLastWeekStore" runat="server" Text="Last Week Record" OnClick="btnLastWeekStore_Click" />
                            </td>
                        </tr>
                    </table>

                    <br />
                    <asp:GridView ID="gvStore" runat="server"
                        class="table table-striped table-hover table-bordered" data-ride="datatables"
                        AutoGenerateColumns="False" CellPadding="4" DataKeyNames="SubId"
                        GridLines="Both" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
                        ForeColor="#333333" OnPageIndexChanging="gvStore_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="SubId" ShowHeader="False" Visible="False" SortExpression="SubId">
                                <ItemTemplate>
                                    <asp:Label ID="SubId" runat="server" Text='<%#Eval("SubId")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    Sr.No
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
                            <asp:BoundField DataField="Store" HeaderText="Store" SortExpression="Store" />
                            <asp:BoundField DataField="PurchaseDate" DataFormatString="{0:d}" HeaderText="Order Date" SortExpression="PurchaseDate" />
                        </Columns>
                    </asp:GridView>
                </div>

                <!--NPU-->
                <div id="divNPU" runat="server">
                    <table id="table8" class="auto-style1">
                        <tr>
                            <td class="auto-style9">
                                <asp:Label ID="lblNPUUser" runat="server" Text="User Name:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNPU" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtNPU" ErrorMessage="Please Enter Name" ValidationGroup="NPU"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style9">Date:</td>
                            <td>
                                <asp:DropDownList ID="ddlWeeks" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style9">
                                <asp:Button ID="btnNPUSearch" runat="server" Text="Search" OnClick="btnNPUSearch_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnNPUCancel" runat="server" Text="Clear" CausesValidation="false" OnClick="btnNPUCancel_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style9">
                                <asp:Button ID="btnView4" runat="server" Text="Download Report" OnClick="btnView_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnLastWeekNPU" runat="server" Text="Last Week Record" OnClick="btnLastWeekNPU_Click" />
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="lblSelectstore" runat="server" Text="Select Store:" Visible="false" /><asp:DropDownList ID="ddlStoreNPU" runat="server" Visible="false">
                    </asp:DropDownList>
                    &nbsp;
                    &nbsp;
                    
                    <br />
                    <asp:GridView ID="gvNPU" runat="server"
                        class="table table-striped table-hover table-bordered" data-ride="datatables"
                        AutoGenerateColumns="False" CellPadding="4" DataKeyNames="SubId"
                        GridLines="Both" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
                        ForeColor="#333333" OnPageIndexChanging="gvNPU_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="SubId" ShowHeader="False" Visible="False" SortExpression="SubId">
                                <ItemTemplate>
                                    <asp:Label ID="SubId" runat="server" Text='<%#Eval("SubId")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    Sr.No
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Username" HeaderText="User Name" SortExpression="Username" />
                            <asp:BoundField DataField="Bounty" HeaderText="Bounty" SortExpression="Bounty" />
                            <asp:BoundField DataField="BarnYard" HeaderText="Barnyard" SortExpression="BarnYard" />
                            <asp:BoundField DataField="Ploughman" HeaderText="Ploughman" SortExpression="Ploughman" />
                            <asp:BoundField DataField="PickupDay" HeaderText="Pickup Day" SortExpression="PickupDay" />
                        </Columns>
                    </asp:GridView>
                </div>

                <div id="divWeeklyReports" runat="server">
                    <table id="table9" class="auto-style1">
                        <tr>
                            <td class="auto-style10">Select Box</td>
                            <td>
                                <asp:DropDownList ID="ddlBoxes" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBoxes_SelectedIndexChanged">
                                    <asp:ListItem>Bounty</asp:ListItem>
                                    <asp:ListItem>Barnyard</asp:ListItem>
                                    <asp:ListItem>Ploughman</asp:ListItem>
                                </asp:DropDownList>

                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style10">
                                <asp:Button ID="btnView5" runat="server" Text="Download Report" OnClick="btnView_Click" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                    <br />
                    <asp:GridView ID="gvWeeklyReports" runat="server"
                        class="table table-striped table-hover table-bordered" data-ride="datatables"
                        AutoGenerateColumns="False" CellPadding="4"
                        GridLines="Both" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
                        ForeColor="#333333" OnPageIndexChanging="gvNPU_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate >
                                    Sr.No
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TotalBoxes" HeaderText="Picked Up" SortExpression="TotalBoxes" />
                            <asp:BoundField DataField="Vacation" HeaderText="Vacation" SortExpression="Vacation" />
                            <asp:BoundField DataField="NPU" HeaderText="NPU" SortExpression="NPU" />
                        </Columns>
                    </asp:GridView>
                </div>

                <div id="divVacation" runat="server">
                    <table id="table10" class="auto-style1">
                        <tr>
                            <td class="auto-style11">
                                <asp:Label ID="lblUserVacation" runat="server" Text="User Name:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVacation" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style11">
                                <asp:Button ID="btnVactionSearch" runat="server" Text="Search" OnClick="btnVactionSearch_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnVacationClear" runat="server" Text="Clear" CausesValidation="false" OnClick="btnVacationClear_Click" />

                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:GridView ID="gvVacation" runat="server"
                        class="table table-striped table-hover table-bordered" data-ride="datatables"
                        AutoGenerateColumns="False" CellPadding="4" DataKeyNames="SubId"
                        GridLines="Both" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
                        ForeColor="#333333" OnPageIndexChanging="gvVacation_PageIndexChanging" OnRowDataBound="gvVacation_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="SubId" ShowHeader="False" Visible="False" SortExpression="SubId">
                                <ItemTemplate>
                                    <asp:Label ID="SubId" runat="server" Text='<%#Eval("SubId")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    Sr.No
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FirstName1" HeaderText="First Name" SortExpression="FirstName1" />
                            <asp:BoundField DataField="LastName1" HeaderText="Last Name" SortExpression="LastName1" />
                            <asp:TemplateField HeaderText="Bounty">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfBounty" runat="server" Value='<%#Eval("Bounty")%>' />
                                    <asp:ImageButton ID="ImgBounty" runat="server" ImageUrl="~/images/appfalse.gif" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Barnyard">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfBarnYard" runat="server" Value='<%#Eval("BarnYard")%>' />
                                    <asp:ImageButton ID="ImgBarnYard" runat="server" ImageUrl="~/images/appfalse.gif" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ploughman">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfPloughman" runat="server" Value='<%#Eval("Ploughman")%>' />
                                    <asp:ImageButton ID="ImgPloughman" runat="server" ImageUrl="~/images/appfalse.gif" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="Bounty" HeaderText="Bounty" />
                            <asp:BoundField DataField="BarnYard" HeaderText="BarnYard" />
                            <asp:BoundField DataField="Ploughman" HeaderText="Ploughman" />--%>
                            <asp:BoundField DataField="PickupDay" HeaderText="Pickup Day" />
                            <asp:BoundField DataField="VacationAddedBy" HeaderText="Scheduled By" />
                            <asp:BoundField DataField="VacationAddedDate" DataFormatString="{0:d}" HeaderText="Scheduled On" />
                        </Columns>
                    </asp:GridView>
                </div>

                <div id="divPickupTT" runat="server">
                    <table id="table11" class="auto-style1">
                        <tr>
                            <td>
                                <asp:Label ID="lblPTT" runat="server" Text="User Name:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPikupTT" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnPTTSearch" runat="server" Text="Search" OnClick="btnPTTSearch_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnPTTClear" runat="server" Text="Clear" CausesValidation="false" OnClick="btnPTTClear_Click" />

                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:GridView ID="gvPickupTT" runat="server"
                        class="table table-striped table-hover table-bordered" data-ride="datatables"
                        AutoGenerateColumns="False" CellPadding="4" DataKeyNames="TTID"
                        GridLines="Both" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
                        ForeColor="#333333" OnPageIndexChanging="gvPickupTT_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="TTID" ShowHeader="False" Visible="False" SortExpression="TTID">
                                <ItemTemplate>
                                    <asp:Label ID="SubId" runat="server" Text='<%#Eval("TTID")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false"> 
                                <HeaderTemplate>
                                    Sr.No
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PickupTime" HeaderText="Pickup Time" SortExpression="PickupTime" />
                            <asp:BoundField DataField="Employee" HeaderText="Employee Logged In" SortExpression="Employee" />
                            <asp:BoundField DataField="Customer" HeaderText="Customer Name" />
                        </Columns>
                    </asp:GridView>
                </div>

            </td>
        </tr>
    </table>

    <asp:Panel ID="pnlHomeDelvieryRecords" runat="server">
        <div id="divHomeDeliveryRecord" runat="server">
          <%--  <asp:GridView ID="gvDELIVERYINFORMATION" runat="server" AutoGenerateColumns="False" data-ride="datatables" class="table table-striped table-hover table-bordered"  ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
                ForeColor="#333333" CellPadding="4" Width="100%">
                <Columns>
                   class="table table-striped table-hover table-bordered"
                   data-ride="datatables"
                    <asp:BoundField DataField="Username" HeaderText="Username" >
                         <HeaderStyle Width="15%" />
                        
                    <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Bounty"  HeaderText="Bounty" ApplyFormatInEditMode="True" >
                    
                    <HeaderStyle Width="15%" />
                    <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="BarnYard" HeaderText="Barnyard" ApplyFormatInEditMode="True" >
                    
                    <HeaderStyle Width="15%" />
                    <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Ploughman" HeaderText="Ploughman" >
                    
                    <HeaderStyle Width="15%" Wrap="True" />
                    <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DeliveryAddress" HeaderText="Delivery Address" >

                    <HeaderStyle Width="30%" />
                        
                    <ItemStyle Width="30%" />
                    </asp:BoundField>

                    <asp:BoundField DataField="BestTime" HeaderText="Best Time" >
                         <HeaderStyle Width="15%" />
                        
                    <ItemStyle Width="15%" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>--%>
            
            <asp:GridView ID="gvDELIVERYINFORMATION" runat="server" AutoGenerateColumns="False" data-ride="datatables" class="table table-striped table-hover table-bordered"  ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display." Width="100%" OnRowDataBound="gvDELIVERYINFORMATION_RowDataBound">
                <Columns>
                   <%-- class="table table-striped table-hover table-bordered"--%>
                    <%--data-ride="datatables"--%>
                    <asp:BoundField DataField="Username" HeaderText="Username" >
                       
                    </asp:BoundField>
                    <asp:BoundField DataField="Bounty"  HeaderText="Bounty" ApplyFormatInEditMode="True"   >
                    
                  
                    </asp:BoundField>
                    <asp:BoundField DataField="BarnYard" HeaderText="Barnyard" ApplyFormatInEditMode="True"   >
                    
                 
                    </asp:BoundField>
                    <asp:BoundField DataField="Ploughman" HeaderText="Ploughman"   >
                    
                  
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Subscription">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                                   
                                                </ItemTemplate>
                                            </asp:TemplateField>
                    <asp:BoundField DataField="DeliveryAddress" HeaderText="Delivery Address" >

                    <HeaderStyle Width="30%" />
                        
                    <ItemStyle Width="30%" />
                    </asp:BoundField>

                    <asp:BoundField DataField="BestTime" HeaderText="Best Time" >
                         <HeaderStyle Width="15%" />
                        
                    <ItemStyle Width="15%" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="gvSUBSCRIPTIONINFORMATION" runat="server" AutoGenerateColumns="False" class="table table-striped table-hover table-bordered" data-ride="datatables"
                GridLines="Both" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display." OnRowDataBound="gvSUBSCRIPTIONINFORMATION_RowDataBound"
                ForeColor="#333333" CellPadding="4">
                <Columns>
                    <asp:BoundField DataField="SpecialInstruction" HeaderText="Delivery Notes" />
                    <asp:BoundField DataField="Allergies" HeaderText="Allergies" />
                </Columns>
            </asp:GridView>
            <asp:GridView ID="gvThirdTable" runat="server" AutoGenerateColumns="False" class="table table-striped table-hover table-bordered" data-ride="datatables"
                GridLines="Both" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display." OnRowDataBound="gvThirdTable_RowDataBound"
                ForeColor="#333333" CellPadding="4">
                <Columns>
                    <asp:BoundField DataField="Notes" HeaderText="Notes" />
                </Columns>
            </asp:GridView>
            <asp:GridView ID="gvADDITIONALITEMS" runat="server" AutoGenerateColumns="False" class="table table-striped table-hover table-bordered" data-ride="datatables"
                GridLines="Both" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
                ForeColor="#333333" CellPadding="4">
                <Columns>
                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                    <asp:BoundField DataField="Price" HeaderText="Price" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>
</asp:Content>

