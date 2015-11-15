<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="account_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

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
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <h1>My Account</h1>
    <asp:UpdatePanel runat="server" ID="MailPanel1" UpdateMode="Conditional">
        <ContentTemplate>
            <h2><span style="color: Green;">
                <asp:Literal runat="server" ID="Literal0"></asp:Literal></span></h2>
            <div style="border-left: thin; border-right: thin;">
                <telerik:RadTabStrip runat="server" ID="RadTabStrip1" MultiPageID="RadMultiPage1" SelectedIndex="0" OnTabClick="RadTabStrip1_TabClick">
                    <Tabs>
                        <telerik:RadTab Text="My Subscriptions" Width="20%"></telerik:RadTab>
                        <telerik:RadTab Text="Additional Product" Width="20%"></telerik:RadTab>
                        <telerik:RadTab Text="Make a Payment" Width="20%"></telerik:RadTab>
                        <telerik:RadTab Text="Vacation" Width="20%"></telerik:RadTab>
                        <telerik:RadTab Text="Account Info" Width="20%"></telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
                <hr />
                <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0" CssClass="outerMultiPage">
                    <telerik:RadPageView runat="server" ID="RadPageView1">
                        Subscription
                                            <asp:CheckBoxList runat="server" ID="BoxButton" Enabled="false">
                                                <asp:ListItem Text="Bounty - $35.00"></asp:ListItem>
                                                <asp:ListItem Text="Barnyard - $35.00"></asp:ListItem>
                                                <asp:ListItem Text="Ploughman - $35.00"></asp:ListItem>
                                            </asp:CheckBoxList><br />
                        Allergies:<br />
                        <asp:TextBox ID="allergies" runat="server" class="text-input" TextMode="multiline" Width="500px" Rows="4"></asp:TextBox>
                        <br />
                        <asp:Button ID="Button3" runat="server" Text="Update" CssClass="submit" OnClick="Button3_Click" />
                        <br />
                        <asp:UpdatePanel runat="server" ID="tableUPanel">
                            <ContentTemplate>
                                Store:
            <asp:DropDownList runat="server" ID="StoreList" AutoPostBack="True" OnSelectedIndexChanged="StoreList_SelectedIndexChanged" Enabled="false"></asp:DropDownList>
                                <span style="color: red;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please select a Store" ControlToValidate="StoreList"></asp:RequiredFieldValidator></span><br />
                                Pickup Day:
            <asp:DropDownList runat="server" ID="PickupDayList" AutoPostBack="True" OnSelectedIndexChanged="PickupDayList_SelectedIndexChanged" Enabled="false"></asp:DropDownList><span style="color: red;"><asp:Literal runat="server" ID="PUDLiteral"></asp:Literal></span>
                                <span style="color: red;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please select a Pickup Day" ControlToValidate="PickupDayList"></asp:RequiredFieldValidator></span>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="PickupDayList" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="storelist" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <br />


                        <asp:Button ID="btnChangePickup" runat="server" Text="Change Pickup" PostBackUrl="~/account/ManagePickup.aspx" CausesValidation="false" />

                        <br />
                        <br />
                        <br />
                        <hr />
                        <div class="threediv">
                            <div>
                                <asp:Button ID="CancelBounty" runat="server" Text="Cancel Bounty Subscription" OnClick="CancelBounty_Click"
                                    OnClientClick="return confirm('Are you Sure you want to Cancel your subscription to the Bounty Box? Doing so does result in the loss of your $35.00 deposit!');" />
                            </div>
                            <div>
                                <asp:Button ID="CancelBarnyard" runat="server" Text="Cancel Barnyard Subscription" OnClick="CancelBarnyard_Click"
                                    OnClientClick="return confirm('Are you Sure you want to Cancel your subscription to the Barnyard Box? Doing so does result in the loss of your $35.00 deposit!');" />
                            </div>
                            <div>
                                <asp:Button ID="CancelPloughman" runat="server" Text="Cancel Ploughman Subscription" OnClick="CancelPloughman_Click"
                                    OnClientClick="return confirm('Are you Sure you want to Cancel your subscription to the Ploughman Box? Doing so does result in the loss of your $35.00 deposit!');" />
                            </div>
                        </div>

                    </telerik:RadPageView>

                    <telerik:RadPageView runat="server" ID="RadPageView5">
                        <h1>Products</h1>

                        <div style="float: left; width: 980px;">
                            Select Week:
                            <asp:Button ID="btnCurrentOrders" runat="server" OnClick="btnCurrentOrders_Click" Text="View Current Orders" />
                            <br />
                            <asp:DropDownList ID="ddlweekProduct" runat="server" AutoPostBack="true" OnTextChanged="ddlweekProduct_TextChanged">
                            </asp:DropDownList>
                            <br />

                            <%--<asp:RequiredFieldValidator InitialValue=" - Select a Week - " ID="Req_ID" Display="Dynamic"
        runat="server" ControlToValidate="ddlWeek"
        ErrorMessage="Please Select Week"></asp:RequiredFieldValidator>--%>
                            <asp:Repeater ID="rcProducts" runat="server" OnItemCommand="rcProducts_ItemCommand" OnItemDataBound="rcProducts_ItemDataBound">
                                <ItemTemplate>
                                    <div style="float: left; margin: 0 10px 10px 0; width: 220px;">
                                        <div style="clear: both" runat="server" visible="<%# (Container.ItemIndex+1) % 1 == 0 %>">
                                            <asp:Image ID="imgProductImage" runat="server" ImageUrl='<%# "../admin/ProductsImage/"+Eval("ProductImage")%>' Height="150px" Width="150px" />
                                            <br />
                                            <asp:Label ID="ProductID" Text='<%#Eval("ProductID") %>' Visible="false" runat="server"></asp:Label>
                                            <asp:Label ID="lblProductName" Text='<%#Eval("ProductName") %>' runat="server"></asp:Label>
                                            <br />
                                            <asp:Label ID="lblPriceName" Text="Price: $" runat="server"></asp:Label>
                                            <asp:Label ID="lblPrice" Text='<%#Eval("ProductPrice") %>' runat="server"></asp:Label>
                                           
                                            <br />
                                            Enter Quantity:<asp:TextBox ID="txtQuantity" runat="server" Width="30%" MaxLength="5"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Numbers" TargetControlID="txtQuantity" runat="server"></cc1:FilteredTextBoxExtender>
                                            <br />
                                            <asp:CheckBox ID="cbAddToCart" Text="Add to Cart" runat="server" />
                                            <br />
                                            <asp:HiddenField ID="hfQuantity" runat="server" Value='<%#Eval("Quantity") %>' />
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <br />
                        <asp:Button ID="btnAddCart" runat="server" Text="Complete Order" OnClick="btnAddCart_Click" />
                        <br />
                    </telerik:RadPageView>
                    <telerik:RadPageView runat="server" ID="RadPageView2">
                        <asp:UpdatePanel runat="server" ID="PriceUPanel" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Literal runat="server" ID="PaymentsLiteral"></asp:Literal>
                                <h2>Make a Payment</h2>
                                <div>


                                    <asp:DropDownList ID="ddlWeek" runat="server" AutoPostBack="True" Visible="false">
                                    </asp:DropDownList>
                                    <br />

                                    <%--<asp:Label ID="lblSelectWeek" runat="server" Text="Select Week:"></asp:Label><asp:DropDownList runat="server" ID="WeeklistProduct"></asp:DropDownList><br />--%>
                                    <asp:Label ID="lblSelectProduct" runat="server" Text="Select the product:" Visible="false"></asp:Label>
                                    <br />
                                    <!--New Added-->
                                    <table>
                                        <asp:Repeater ID="rptSelectedProduct" runat="server" OnItemCommand="rptSelectedProduct_ItemCommand" Visible="false">
                                            <HeaderTemplate>
                                                <tr>
                                                    <th>Product</th>
                                                    <th>Total Price</th>
                                                </tr>

                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="cbProduct" Text='<%#Eval("ProductName") %>' OnCheckedChanged="cbProduct_CheckedChanged" AutoPostBack="true" ToolTip='<%#Eval("TotalPrice") %>' runat="server" Checked="true" /></td>
                                                    <td>
                                                        <asp:Label ID="lblPrice" Text='<%#Eval("TotalPrice") %>' runat="server"></asp:Label><asp:Label ID="Label1" Text="$" runat="server"></asp:Label></td>
                                                </tr>
                                            </ItemTemplate>

                                        </asp:Repeater>
                                    </table>
                                </div>

                                <%-- Select Payment Option:
                                <asp:RadioButtonList ID="rblPayment" runat="server" OnSelectedIndexChanged="rblPayment_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Text="Online" Value="Online" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Store" Value="Store"></asp:ListItem>
                                </asp:RadioButtonList>
                                <br />
                                <br />--%>

                                <%--Select the weeks you would like to pay for--%>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="GridViewClass" AllowSorting="False" DataKeyNames="SubId">
                                                <Columns>
                                                    <asp:BoundField DataField="SubId" HeaderText="SubId" InsertVisible="False" ReadOnly="True" SortExpression="SubId" Visible="false" />

                                                    <asp:BoundField DataField="Week" HeaderText="Week" SortExpression="Week" />
                                                    <asp:TemplateField HeaderText="Bounty">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="BountyPaidCheck" runat="server" AutoPostBack="true" Checked='<%# Eval("PaidBounty").ToString() == "1" ? true:false %>' OnCheckedChanged="OnCheckedChanged" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Barnyard">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="BarnyardPaidCheck" runat="server" AutoPostBack="true" Checked='<%# Eval("PaidBarnyard").ToString() == "1" ? true:false %>' OnCheckedChanged="OnCheckedChanged" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ploughman">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="PloughmanPaidCheck" runat="server" AutoPostBack="true" Checked='<%# Eval("PaidPloughman").ToString() == "1" ? true:false %>' OnCheckedChanged="OnCheckedChanged" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                        <td>
                                            <asp:GridView ID="gvProducts" runat="server" OnRowDataBound="gvProducts_RowDataBound" AutoGenerateColumns="False" CssClass="GridViewClass" AllowSorting="False" DataKeyNames="SubId">
                                                <Columns>
                                                    <asp:BoundField DataField="SubId" HeaderText="SubId" InsertVisible="False" ReadOnly="True" SortExpression="SubId" Visible="false" />
                                                    <asp:TemplateField HeaderText="Week" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblWeek" Text='<%# Eval("Week") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Week" HeaderText="Week" SortExpression="Week" Visible="false" />
                                                    <asp:TemplateField HeaderText="Products">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbProducts" AutoPostBack="true" OnCheckedChanged="cbProducts_CheckedChanged" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>  

                                        <td>
                                            <asp:GridView ID="gvDelivery" runat="server" OnRowDataBound="gvDelivery_RowDataBound" AutoGenerateColumns="False" CssClass="GridViewClass" AllowSorting="False" DataKeyNames="SubId">
                                                <Columns>
                                                    <asp:BoundField DataField="SubId" HeaderText="SubId" InsertVisible="False" ReadOnly="True" SortExpression="SubId" Visible="false" />
                                                    <asp:TemplateField HeaderText="Week" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblWeek" Text='<%# Eval("Week") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Week" HeaderText="Week" SortExpression="Week" Visible="false" />
                                                    <asp:TemplateField HeaderText="Delivery">
                                                        <ItemTemplate> 
                                                            <asp:CheckBox ID="cbDelivery" Enabled="true" OnCheckedChanged="cbDelivery_CheckedChanged" AutoPostBack="true" ToolTip='<%# Eval("Week") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                        <td style="vertical-align: top">
                                            <asp:DropDownList ID="ddlWeeks" OnSelectedIndexChanged="ddlWeeks_SelectedIndexChanged" Visible="false" AutoPostBack="true" runat="server"></asp:DropDownList>

                                            <asp:GridView ID="gvTotalProduct" Visible="false" runat="server" AutoGenerateColumns="False" CssClass="GridViewClass" AllowSorting="False" OnRowCommand="gvTotalProduct_RowCommand">
                                                <Columns>
                                                    <asp:BoundField DataField="Week" HeaderText="Week" />
                                                    <asp:TemplateField HeaderText="Additional Items">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbTotalProduct" runat="server" CommandName="chk" Text='<%#Eval("Price") %>' AutoPostBack="true" OnCheckedChanged="cbTotalProduct_CheckedChanged" Checked="true" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Additional Items Details">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbTotalProduct" runat="server" Text="Edit" CommandName="Edit1" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delivery" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbHomeDelivery" Text="Yes" AutoPostBack="true" OnCheckedChanged="cbHomeDelivery_CheckedChanged" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <br />
                                            <div>
                                                Payment Due: <span style="color: red; font-weight: bold;">$
                                                    <asp:Literal runat="server" ID="Price"></asp:Literal></span> + tax
                                            </div>
                                            <%--Do you want Home Delivery?<asp:CheckBox ID="cbHomeDelivery" Text="Yes" AutoPostBack="true" OnCheckedChanged="cbHomeDelivery_CheckedChanged" runat="server" /><br />--%>
                                            <asp:Button ID="Button2" runat="server" Text="Make a payment Now" OnClick="Button2_Click1" /><asp:Button ID="btnPayInStore" runat="server" Text="Pay Products In Store" OnClick="btnPayInStore_Click" Visible="false" />
                                            <%--OnClientClick="return confirm('Once your order is created, you will be redirected to Mercury Pay to complete your transaction.');"--%>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </telerik:RadPageView>
                    <telerik:RadPageView runat="server" ID="RadPageView3">
                        <asp:Literal runat="server" ID="weeksLiteral"></asp:Literal>
                        <table>
                            <tr>
                                <asp:Panel runat="server" ID="calpanel">
                                    <td style="width: 440px;">
                                        <br />
                                        <h3>Add a Vacation Week</h3>
                                        <asp:Literal runat="server" ID="Literal2"></asp:Literal><br />
                                        <asp:Label ID="Label2" Text="Select Vacation Week:" runat="server"></asp:Label>
                                        <asp:DropDownList runat="server" ID="WeekList"></asp:DropDownList>

                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <asp:Button runat="server" ID="Submit" Text="Submit Vacation Week" OnClick="btnAddVacation_Click" /></td>
                                </asp:Panel>
                                <td style="width: 440px;">
                                    <asp:Literal runat="server" ID="WeekDel"></asp:Literal><br />
                                    <h3>Current Vacation Weeks</h3>
                                    <asp:GridView ID="gvVacation" runat="server" AllowPaging="false" AllowSorting="false"
                                        class="table table-striped table-hover table-bordered" data-ride="datatables"
                                        AutoGenerateColumns="False" CellPadding="4" DataKeyNames="VID"
                                        GridLines="Both" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
                                        ForeColor="#333333" OnRowCommand="gvVacation_RowCommand">
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

                                </td>
                            </tr>
                        </table>


                    </telerik:RadPageView>
                    <telerik:RadPageView runat="server" ID="RadPageView4">
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                        <h3>Subscriber 1</h3>
                        First Name:
                    <asp:TextBox ID="firstname1" runat="server" class="text-input" Width="500px" Enabled="false"></asp:TextBox><br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter your First Name" ControlToValidate="firstname1"></asp:RequiredFieldValidator><br />
                        Last Name:
                    <asp:TextBox ID="lastname1" runat="server" class="text-input" Width="500px" Enabled="false"></asp:TextBox><br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your Last Name" ControlToValidate="lastname1"></asp:RequiredFieldValidator><br />
                        Email:
                    <asp:TextBox ID="email1" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter your Email Address" ControlToValidate="email1"></asp:RequiredFieldValidator><br />
                        Phone:
                    <asp:TextBox ID="phone1" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please enter your phone number" ControlToValidate="phone1"></asp:RequiredFieldValidator><br />
                        <br />
                        <h3>Subscriber 2</h3>
                        First Name:
                    <asp:TextBox ID="firstname2" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                        <br />
                        Last Name:
                    <asp:TextBox ID="lastname2" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                        <br />
                        Email:
                    <asp:TextBox ID="email2" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                        <br />
                        Phone:
                    <asp:TextBox ID="phone2" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                        <br />
                        <br />
                        Address:
                    <asp:TextBox ID="address" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Please enter your address" ControlToValidate="address"></asp:RequiredFieldValidator><br />
                        City: 
                                            <asp:TextBox ID="city" runat="server" class="text-input" Width="172px"></asp:TextBox>
                        &nbsp;&nbsp;State: 
                                            <asp:TextBox ID="state" runat="server" class="text-input" Width="70px"></asp:TextBox>
                        &nbsp;&nbsp;Zip: 
                                            <asp:TextBox ID="zip" runat="server" class="text-input" Width="100px"></asp:TextBox><br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Please enter your city" ControlToValidate="city"></asp:RequiredFieldValidator>&nbsp;&nbsp;
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Please enter your state" ControlToValidate="state"></asp:RequiredFieldValidator>&nbsp;&nbsp;
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Please enter your zip code" ControlToValidate="zip"></asp:RequiredFieldValidator><br />
                        <br />
                        <h3>Request Delivery</h3>
                        <table>
                            <tr>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="litMsg" runat="server"></asp:Literal><br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDeliveryAddress" runat="server" Text="Delivery Address"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtDeliveryAddress" TextMode="MultiLine" MaxLength="250" runat="server"></asp:TextBox></td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label ID="lblBestTime" runat="server" Text="Select Best Time"></asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="ddlBestTime" runat="server"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblLocation" runat="server" Text="Select Location"></asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="ddlLocation" runat="server">
                                        <asp:ListItem>Home</asp:ListItem>
                                        <asp:ListItem>Work</asp:ListItem>
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSpecialInstr" runat="server" Text="Special Instruction"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtSpecialinstr" TextMode="MultiLine" MaxLength="250" runat="server"></asp:TextBox></td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Button ID="btnRequest" runat="server" Text="Request/Update Delivery" OnClick="btnRequest_Click" /></td>
                            </tr>

                        </table>

                        <h3>Change Password</h3>
                        <asp:Literal ID="PassLiteral" runat="server"></asp:Literal><br />
                        <%--Current Password: <asp:TextBox ID="CurPassBox" TextMode="Password"  runat="server"></asp:TextBox><br />--%>
                New Password:
                        <asp:TextBox ID="NewPassBox1" TextMode="Password" runat="server"></asp:TextBox><br />
                        Confirm:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="NewPassBox2" TextMode="Password" runat="server"></asp:TextBox><br />
                        <asp:Button ID="Button4" runat="server" Text="Change Password" OnClick="Button4_Click" />
                        <br />
                        <br />
                        <h3>Subscribe to our Newsletter's</h3>
                        <asp:CheckBox ID="BountyNL" runat="server" />Bounty Box Summer 2015<br />
                        <asp:CheckBox ID="BarnyardNL" runat="server" />Barnyard Box Summer 2015<br />
                        <asp:CheckBox ID="PloughmanNL" runat="server" />Ploughman Box Summer 2015<br />

                        <asp:Button ID="Button1" runat="server" Text="Update" class="submit" OnClick="Button1_Click" />
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
        <li></li>
    </ul>

    <div runat="server" id="Invoice" visible="false">
        <asp:Panel ID="pnlInvoice" runat="server">
            <asp:GridView ID="gvSelectedProduct" runat="server" b
                class="table table-striped table-hover table-bordered" data-ride="datatables"
                AutoGenerateColumns="False" CellPadding="4" DataKeyNames=" ProductId"
                GridLines="Both" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
                ForeColor="#333333" OnRowDataBound="gvSelectedProduct_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="ProductId" ShowHeader="False" Visible="False" SortExpression="ProductId">
                        <ItemTemplate>
                            <asp:Label ID="ProductId" runat="server" Text='<%#Eval("ProductId")%>' />
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
                    <asp:BoundField DataField="ProductName" HeaderText="ProductName" SortExpression="ProductName" />
                    <asp:BoundField DataField="ProductPrice" HeaderText="Product Price" SortExpression="ProductPrice" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />
                    <asp:TemplateField HeaderText="Total Price" ShowHeader="False">
                        <ItemTemplate>
                            <asp:Label ID="lblTotalPrice" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>

            <asp:GridView ID="gvBox" runat="server" class="table table-striped table-hover table-bordered" data-ride="datatables"
                AutoGenerateColumns="False" CellPadding="4"
                GridLines="Both" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
                ForeColor="#333333">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Sr.No
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Week" HeaderText="Week" SortExpression="ProductName" />
                    <asp:BoundField DataField="Box" HeaderText="Box" SortExpression="Box" />
                </Columns>
            </asp:GridView>

            <asp:Label ID="Label1" runat="server" Text="You have to pay $"></asp:Label><asp:Label ID="lblTotal" runat="server"></asp:Label>
            <br />
            <br />
            <%--   <asp:Button ID="btnPayment" Text="Ok" runat="server" OnClick="btnPayment_Click" />--%>
        </asp:Panel>
    </div>


</asp:Content>

