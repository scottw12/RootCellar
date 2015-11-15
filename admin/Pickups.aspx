<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Pickups.aspx.cs" Inherits="admin_Pickups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .GridViewClass {
             width: 1000px;
            font-weight: 700;
            color: #000000;
            margin-right: -70px;
            margin-left: -50px;
        }
    </style>
    <script type="text/javascript" lang="javascript">
        //function Error() {
        //    alert("You have no permission to access this. Please Contact to admin");
        //    window.location = "../Admin/Default.aspx";
        //}
    </script>
</asp:Content> 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
     <asp:UpdateProgress runat="server" id="PageUpdateProgress" DisplayAfter="0">
            <ProgressTemplate>
               <strong> Please Wait your request is Loading...</strong>
                <img src="../images/ajax-loader.gif" />
            </ProgressTemplate>
        </asp:UpdateProgress>
    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
            <div style="float: right;">
                <asp:Button ID="ReminderButton" runat="server" Text="Send Reminder E-Mail" Visible="false" OnClick="ReminderButton_Click" />
            </div>
            <br />
            <div class="threediv">
                <div>
                    <asp:DropDownList runat="server" ID="WeekList" AutoPostBack="True" OnSelectedIndexChanged="WeekList_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div>
                    Store:
            <asp:DropDownList runat="server" ID="StoreList" AutoPostBack="True" OnSelectedIndexChanged="StoreList_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div>
                    Pickup Day:
            <asp:DropDownList runat="server" ID="PickupDayList" AutoPostBack="true" OnSelectedIndexChanged="PickupDay_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>
            <span style="color: red;">
                <asp:Literal runat="server" ID="PUDLiteral"></asp:Literal></span>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="PickupDayList" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="storelist" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <asp:CheckBox runat="server" ID="NPUCheck" Text="Show NPU only" AutoPostBack="true" OnCheckedChanged="NPUCheck_CheckedChanged"/>
    <asp:UpdatePanel runat="server" ID="tableUPanel">
        <ContentTemplate>
            <asp:Literal runat="server" ID="Literal1"></asp:Literal>
            <div>
                <%--<table class="GridViewClass" border="1" id="ContentPlaceHolder1_GridView1" style="border-collapse: collapse;" >
                    <tr>
                        <th scope="col" style="width: 120px;">Last Name</th>
                        <th scope="col" style="width: 120px;">First Name</th>
                        <th scope="col" style="width: 75px;">Bounty</th>
                        <th scope="col" style="width: 75px;">Barnyard</th>
                        <th scope="col" style="width: 75px;">Ploughman</th>
                        <th scope="col" style="width: 75px;">Bounty Paid</th>
                        <th scope="col" style="width: 75px;">Barnyard Paid</th>
                        <th scope="col" style="width: 75px;">Ploughman Paid</th>
                        <th scope="col" style="width: 75px;">Picked up</th>
                        <th scope="col" style="width: 100px;">Phone #</th>
                    </tr>
                </table>--%>
            </div>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="GridViewClass" AllowSorting="True" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand" DataKeyNames="SubId"  ShowHeader="True">
                <Columns>
                    <asp:BoundField DataField="SubId" HeaderText="SubId" InsertVisible="False" ReadOnly="True" SortExpression="SubId" Visible="false" />
                    <asp:BoundField DataField="LastName1" HeaderText="Last Name" SortExpression="LastName1" ItemStyle-Width="120px"/>
                    <asp:BoundField DataField="FirstName1" HeaderText="First Name" SortExpression="FirstName1" ItemStyle-Width="120px"/>
                    <%--<asp:BoundField DataField="Allergies" HeaderText="Allergies" SortExpression="Allergies" Visible="false" />--%>
                    <asp:TemplateField HeaderText="Bounty">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="BountySub" Text="Subscribed" Visible='<%# Convert.ToBoolean(Eval("Bounty"))%>' />
                            <asp:Literal runat="server" ID="BountyVac" Text="Vacation" Visible='<%# Convert.ToBoolean(Eval("BountyVac"))%>' />
                        </ItemTemplate>
                         <ItemStyle Width="75px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Barnyard">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="BarnyardSub" Text="Subscribed" Visible='<%# Convert.ToBoolean(Eval("Barnyard"))%>' />
                            <asp:Literal runat="server" ID="BarnyardVac" Text="Vacation" Visible='<%# Convert.ToBoolean(Eval("BarnyardVac"))%>' />
                        </ItemTemplate>
                         <ItemStyle Width="75px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ploughman">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="PloughmanSub" Text="Subscribed" Visible='<%# Convert.ToBoolean(Eval("Ploughman"))%>' />
                            <asp:Literal runat="server" ID="PloughmanVac" Text="Vacation" Visible='<%# Convert.ToBoolean(Eval("PloughmanVac"))%>' />
                        </ItemTemplate>
                         <ItemStyle Width="75px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Bounty Paid">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnpayBounty" runat="server" CommandName="PayBounty" ImageUrl="~/images/appfalse.gif" Enabled="false" Visible='<%# Convert.ToBoolean(Eval("paidBounty"))%>'></asp:ImageButton>
                            <%--<asp:ImageButton ID="btnpayBounty" runat="server" CommandName="PayBounty" ImageUrl="~/images/appfalse.gif" Enabled="false"></asp:ImageButton>
                        <asp:ImageButton ID="btnpaidBounty" runat="server" ImageUrl="~/images/apptrue.gif" Enabled="false"></asp:ImageButton>--%>
                        </ItemTemplate>
                         <ItemStyle Width="75px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Barnyard Paid">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnpayBarnyard" runat="server" CommandName="PayBarnyard" ImageUrl="~/images/appfalse.gif" Enabled="false" Visible='<%# Convert.ToBoolean(Eval("paidBarnyard"))%>'></asp:ImageButton>
                            <%--<asp:ImageButton ID="btnpayBarnyard" runat="server" CommandName="PayBarnyard" ImageUrl="~/images/appfalse.gif" Enabled="false"></asp:ImageButton>
                            <asp:ImageButton ID="btnpaidBarnyard" runat="server" ImageUrl="~/images/apptrue.gif" Enabled="false"></asp:ImageButton>--%>
                        </ItemTemplate>
                         <ItemStyle Width="75px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ploughman Paid">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnpayPloughman" runat="server" CommandName="PayPloughman" ImageUrl="~/images/appfalse.gif" Enabled="false" Visible='<%# Convert.ToBoolean(Eval("paidPloughman"))%>'></asp:ImageButton>
                            <%--<asp:ImageButton ID="btnpayPloughman" runat="server" CommandName="PayPloughman" ImageUrl="~/images/appfalse.gif" Enabled="false"></asp:ImageButton>
                            <asp:ImageButton ID="btnpaidPloughman" runat="server" ImageUrl="~/images/apptrue.gif" Enabled="false"></asp:ImageButton>--%>
                        </ItemTemplate>
                         <ItemStyle Width="75px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Picked up">
                        <ItemTemplate>
                            <asp:ImageButton ID="pickup" runat="server" CommandName="Pickup" ImageUrl="~/images/appfalse.gif" Visible='<%# Convert.ToBoolean(Eval("pickedup"))%>'></asp:ImageButton>
                            <%--<asp:ImageButton ID="pickup" runat="server" CommandName="Pickup" ImageUrl="~/images/appfalse.gif"></asp:ImageButton>
                            <asp:ImageButton ID="pickedup" runat="server" ImageUrl="~/images/apptrue.gif"></asp:ImageButton>--%>
                        </ItemTemplate>
                         <ItemStyle Width="75px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="phone" HeaderText="Phone #" SortExpression="Phone"  ItemStyle-Width="100px" HeaderStyle-Width="100px" />
                    <asp:TemplateField HeaderText="Product Details" Visible="false">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnProduct" runat="server" CommandName="ViewProduct" ImageUrl="~/images/Help.png" CommandArgument='<%# Eval("SubId")%>' ></asp:ImageButton>
                            <%--<asp:Button ID="btnProduct" runat="server" CommandName="ViewProduct" Text="View" CommandArgument='<%# Eval("SubId")%>' />  --%>
                        </ItemTemplate>
                         <ItemStyle Width="75px" />
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Weeklist" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="storelist" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="pickupdaylist" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="NPUCheck" EventName="CheckedChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="updateProgress" runat="server">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.7;">
                <span style="border-width: 0px; position: fixed; padding: 50px; color: white; font-size: 36px; left: 40%; top: 40%;">Loading ...</span>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>

