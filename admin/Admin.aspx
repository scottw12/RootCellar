<%@ Page Title="" Language="c#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Admin.aspx.cs" Inherits="admin_Admin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .SGridViewClass {
            font-weight: 700;
            color: #000000;
            margin-right: -35px;
            margin-left: -15px;
        }
    </style>
    <script type="text/javascript" lang="javascript">
        function Save() {
            alert("Created User Successfully");
            window.location = "../Admin/Default.aspx";
        }
        function Deleted() {
            alert("Deleted User Successfully");
            window.location = "../Admin/Default.aspx";
        }
        //function Error() {
        //    alert("You have no permission to access this. Please Contact to admin");
        //    window.location = "../Admin/Default.aspx";
        //}
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager><asp:Literal ID="Literal4" runat="server"></asp:Literal>
    <cc1:Accordion ID="AccordionCtrl" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
        ContentCssClass="accordionContent" AutoSize="None" FadeTransitions="true">
        <Panes>
            <cc1:AccordionPane ID="AccordionPane0" runat="server">
                <Header>
                            Create New User</Header>
                <Content>
                    <asp:Label ID="Label1" runat="server"></asp:Label><br />
                    First Name:<br />
                    <asp:TextBox ID="firstname" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                    Last Name:<br />
                    <asp:TextBox ID="lastname" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                    Email:<br />
                    <asp:TextBox ID="email" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                    Password:<br />
                    <asp:TextBox ID="password" runat="server" TextMode="Password" class="text-input" Width="500px"></asp:TextBox><br />
                    Role:<br />
                    <asp:CheckBox ID="Standard" runat="server" />Employee
                            <asp:CheckBox ID="Admin" runat="server" />Admin
                            <cc1:MutuallyExclusiveCheckBoxExtender ID="mecbe1" runat="server" TargetControlID="Standard"
                                Key="YesNo" />
                    <cc1:MutuallyExclusiveCheckBoxExtender ID="mecbe3" runat="server" TargetControlID="Admin"
                        Key="YesNo" />
                    <br />
                    <br />

                    <center><asp:Button ID="Button6" runat="server" Text="Submit" class="submit" OnClick="Button6_Click" /></center>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane1" runat="server">
                <Header>
                            Assign User Roles</Header>
                <Content>
                    <asp:Label ID="Label2" runat="server"></asp:Label>
                    <asp:GridView ID="GridView2" runat="server" DataSourceID="SqlDataSource2" OnSelectedIndexChanged="edit_SelectedIndexChanged"
                        AutoGenerateColumns="False" HeaderStyle-HorizontalAlign="Left" Visible="false">
                        <Columns>
                            <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                            <asp:BoundField DataField="Email" HeaderText="E-Mail" SortExpression="Email" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                            <asp:BoundField DataField="Role" HeaderText="Role" SortExpression="Role" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                            <asp:CommandField ShowSelectButton="True" SelectText="Edit" ControlStyle-ForeColor="Black" HeaderStyle-Width="190px" ItemStyle-Width="190px" />                           
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:GridView ID="gvAssignRole" runat="server" OnSelectedIndexChanged="edit_SelectedIndexChanged"
                        AutoGenerateColumns="False" HeaderStyle-HorizontalAlign="Left">
                        <Columns>
                            <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                            <asp:BoundField DataField="Email" HeaderText="E-Mail" SortExpression="Email" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                            <asp:BoundField DataField="Role" HeaderText="Role" SortExpression="Role" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("UserId", "~/admin/AssignRole.aspx?Id={0}") %>' Text=" Assign User Roles" />                                    
                                </ItemTemplate>
                            </asp:TemplateField>                          
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Panel ID="Panel1" runat="server" Visible="false">
                        First Name:<br />
                        <asp:TextBox ID="TextBox1" runat="server" class="text-input"></asp:TextBox>
                        <br />
                        Last Name:<br />
                        <asp:TextBox ID="TextBox2" runat="server" class="text-input"></asp:TextBox>
                        <br />
                        <br />
                        Username:&nbsp;
                                <asp:Label ID="Label21" runat="server"></asp:Label>
                        <br />
                        <br />
                        Email:<br />
                        <asp:TextBox ID="TextBox4" runat="server" class="text-input"></asp:TextBox>
                        <br />
                        <br />
                        Role:<asp:CheckBox ID="CheckBox1" runat="server" />Employee
                                <asp:CheckBox ID="CheckBox2" runat="server" />Admin
                                <cc1:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender1" runat="server"
                                    TargetControlID="Checkbox1" Key="YesNo" />
                        <cc1:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender2" runat="server"
                            TargetControlID="Checkbox2" Key="YesNo" />
                        <br />
                        Reset Password:<br />
                        <asp:TextBox ID="TextBox3" runat="server" TextMode="Password" class="text-input"></asp:TextBox><br />
                        <br />
                        <asp:Button ID="Button7" runat="server" Text="Submit" CssClass="submit" />
                    </asp:Panel>

                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane2" runat="server">
                <Header>
                            Freeze User</Header>
                <Content>
                    <asp:Label ID="Label4" runat="server"></asp:Label>
                    <asp:GridView ID="GridView4" runat="server" DataSourceID="SqlDataSource3" OnSelectedIndexChanged="freeze_SelectedIndexChanged"
                        AutoGenerateColumns="False" HeaderStyle-HorizontalAlign="Left">
                        <Columns>
                            <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                            <asp:BoundField DataField="Email" HeaderText="E-Mail" SortExpression="Email" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                            <asp:BoundField DataField="Role" HeaderText="Role" SortExpression="Role" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                            <asp:CommandField ShowSelectButton="True" SelectText="Freeze" ControlStyle-ForeColor="Black" />
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Label ID="Label5" runat="server"></asp:Label>
                    <asp:GridView ID="GridView5" runat="server" DataSourceID="SqlDataSource4" OnSelectedIndexChanged="unfreeze_SelectedIndexChanged"
                        AutoGenerateColumns="False" HeaderStyle-HorizontalAlign="Left">
                        <Columns>
                            <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                            <asp:BoundField DataField="Email" HeaderText="E-Mail" SortExpression="Email" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                            <asp:BoundField DataField="Role" HeaderText="Role" SortExpression="Role" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                            <asp:CommandField ShowSelectButton="True" SelectText="Un-Freeze" ControlStyle-ForeColor="Black" />
                        </Columns>
                    </asp:GridView>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane3" runat="server">
                <Header>
                            Delete User</Header>
                <Content>
                    CAUTION - This can not be undone!<br />
                    <asp:Label ID="Label3" runat="server"></asp:Label>
                    <asp:GridView ID="GridView3" runat="server" DataSourceID="SqlDataSource2" OnSelectedIndexChanged="delete_SelectedIndexChanged"
                        AutoGenerateColumns="False" HeaderStyle-HorizontalAlign="Left">
                        <Columns>
                            <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                            <asp:BoundField DataField="Email" HeaderText="E-Mail" SortExpression="Email" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                            <asp:BoundField DataField="Role" HeaderText="Role" SortExpression="Role" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                            <asp:CommandField ShowSelectButton="True" SelectText="Delete" ControlStyle-ForeColor="Black" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                        </Columns>
                    </asp:GridView>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane4" runat="server">
                <Header>
                            Stores</Header>
                <Content>
                    <asp:Label ID="Label6" runat="server"></asp:Label>
                    <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" OnSelectedIndexChanged="Storedelete_SelectedIndexChanged"
                        AutoGenerateColumns="False" HeaderStyle-HorizontalAlign="Left">
                        <Columns>
                            <asp:BoundField DataField="Store" HeaderText="Store" SortExpression="Store" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                            <asp:CommandField ShowSelectButton="True" SelectText="Delete" ControlStyle-ForeColor="Black" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                        </Columns>
                    </asp:GridView>
                    <br />
                    Add a new Store:<br />
                    <asp:TextBox ID="NewStore" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                    <br />

                    <center><asp:Button ID="Button1" runat="server" Text="Submit" class="submit" /></center>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane5" runat="server">
                <Header>
                            Pickup Days</Header>
                <Content>
                    <asp:Label ID="Label7" runat="server"></asp:Label>
                    <asp:GridView ID="GridView6" runat="server" DataSourceID="SqlDataSource5" OnSelectedIndexChanged="PickupDaydelete_SelectedIndexChanged"
                        AutoGenerateColumns="False" HeaderStyle-HorizontalAlign="Left">
                        <Columns>
                            <asp:BoundField DataField="PickupDay" HeaderText="Pickup Day" SortExpression="PickupDay" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                            <asp:CommandField ShowSelectButton="True" SelectText="Delete" ControlStyle-ForeColor="Black" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                        </Columns>
                    </asp:GridView>
                    <br />
                    Add a new Pickup Day:<br />
                    <asp:TextBox ID="NewPickupDay" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                    <br />

                    <center><asp:Button ID="Button2" runat="server" Text="Submit" class="submit" /></center>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane6" runat="server">
                <Header>Seasons</Header>
                <Content>
                    <h2>Seasons</h2>
                    <h3>
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal></h3>
                    <asp:Label ID="Label8" runat="server"></asp:Label>
                    One and only one Season must be marked as "Current"
                    <asp:GridView ID="GridView7" runat="server" AutoGenerateColumns="False" HeaderStyle-HorizontalAlign="Left" EmptyDataText="No Seasons" ShowFooter="true" OnRowCancelingEdit="GridViewSample_RowCancelingEdit" OnRowEditing="GridViewSample_RowEditing" DataKeyNames="SID"
                        OnRowUpdating="GridViewSample_RowUpdating" CssClass="SGridViewClass"
                        OnRowDeleting="GridViewSample_RowDeleting" OnRowCommand="GridViewSample_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="SID" Visible="false" />
                            <asp:TemplateField HeaderText="Season">
                                <ItemTemplate>
                                    <asp:Literal ID="SIDLit" runat="server" Text='<%#Eval("sid")%>' Visible="false" />
                                    <asp:Literal ID="Snamelit" runat="server" Text='<%#Eval("name") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Literal ID="SIDLit" runat="server" Text='<%#Eval("sid")%>' Visible="false" />
                                    <asp:TextBox ID="SeasonName" runat="server" Text='<%#Eval("name") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="NewSeasonName" runat="server"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Current">
                                <ItemTemplate>
                                    <asp:CheckBox ID="SeasonCurrent1" runat="server" Checked='<%# bool.Parse(Eval("CurrentS").ToString()) %>' Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="SeasonCurrent2" runat="server" Checked='<%# bool.Parse(Eval("CurrentS").ToString()) %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="NewSeasonCurrent" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Enroll">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Seasonenroll1" runat="server" Checked='<%# bool.Parse(Eval("enroll").ToString()) %>' Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="Seasonenroll2" runat="server" Checked='<%# bool.Parse(Eval("enroll").ToString()) %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="NewSeasonenroll" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date's">
                                <ItemTemplate>
                                    Start:
                                    <asp:Literal ID="slit" runat="server" Text='<%#Eval("SStart") %>' /><br />
                                    End: 
                                    <asp:Literal ID="elit" runat="server" Text='<%#Eval("SEnd")%>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                        <ContentTemplate>
                                            Starts<br />
                                            <asp:Calendar ID="Calendar1" runat="server" OnDayRender="DaySelect"></asp:Calendar>
                                            <br />
                                             <%--SelectedDate ='<%#Eval("SStart") %>'--%>
                                            Ends<br />
                                            <asp:Calendar ID="Calendar2" runat="server"  OnDayRender="DaySelect"></asp:Calendar>
                                            <%--SelectedDate='<%#Eval("SEnd")%>'--%>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                        <ContentTemplate>
                                            Starts<br />
                                            <asp:Calendar ID="NewCalendar1" runat="server" OnDayRender="DaySelect"></asp:Calendar>
                                            <br />
                                            Ends<br />
                                            <asp:Calendar ID="NewCalendar2" runat="server" OnDayRender="DaySelect"></asp:Calendar>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit/Delete" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" />
                                    <span onclick="return confirm('Are you sure want to delete this season?')">
                                        <asp:LinkButton ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" />
                                    </span>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="btnUpdate" Text="Update" runat="server" CommandName="Update" />
                                    <asp:LinkButton ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Button ID="btnInsertRecord" runat="server" Text="Add" ValidationGroup="ValgrpCust" CommandName="Insert" />
                                </FooterTemplate>
                                <HeaderStyle Width="15%"></HeaderStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <h2>Excluded Weeks</h2>
                    <h3>
                        <asp:Literal ID="Literal2" runat="server"></asp:Literal></h3>
                    <asp:GridView ID="GridView8" runat="server" AutoGenerateColumns="False" HeaderStyle-HorizontalAlign="Left" EmptyDataText="No Excluded Dates" ShowFooter="true" CssClass="SGridViewClass"
                        OnRowDeleting="GridView8_RowDeleting" OnRowCommand="GridView8_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Date">
                                <ItemTemplate>
                                    <asp:Literal ID="EIDLit" runat="server" Text='<%#Eval("eid")%>' Visible="false" />
                                    <asp:Literal ID="EDatelit" runat="server" Text='<%#Eval("EDate")%>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <%--<asp:Calendar ID="NewEDate" runat="server" SelectedDate='<%#Eval("SStart") %>' OnDayRender="DaySelect"></asp:Calendar>--%>
                                    <asp:Calendar ID="NewEDate" runat="server" OnDayRender="DaySelect"></asp:Calendar>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <span onclick="return confirm('Are you sure want to delete this date?')">
                                        <asp:LinkButton ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" />
                                    </span>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Button ID="btnInsertRecord" runat="server" Text="Add" ValidationGroup="ValgrpCust" CommandName="Insert" />
                                </FooterTemplate>
                                <HeaderStyle Width="15%"></HeaderStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane7" runat="server">
                <Header>Active Box's</Header>
                <Content>
                    <asp:Literal ID="Literal3" runat="server"></asp:Literal><br />
                    To exclude a box from new subscribers in the current season, uncheck it's box<br />
                    <br />
                    <asp:CheckBox ID="BountyActive" runat="server" />
                    Bounty
                    <asp:CheckBox ID="BarnyardActive" runat="server" />
                    Barnyard
                    <asp:CheckBox ID="PloughmanActive" runat="server" />
                    Ploughman
                    <center><asp:Button ID="ActiveBoxButton" runat="server" Text="Submit" class="submit" /></center>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane8" runat="server">
                <Header>Unpaid Deposits</Header>
                <Content>
                    <asp:GridView ID="GridView9" runat="server" EmptyDataText="There are no subscribers with an unpaid deposit" CssClass="SGridViewClass" HeaderStyle-HorizontalAlign="Left" AutoGenerateColumns="False" DataSourceID="SqlDataSource6">
                        <Columns>
                            <asp:BoundField DataField="FirstName1" HeaderText="First Name" SortExpression="FirstName1" />
                            <asp:BoundField DataField="LastName1" HeaderText="Last Name" SortExpression="LastName1" />
                            <%--<asp:BoundField DataField="Email1" HeaderText="Email1" SortExpression="Email1" />--%>
                            <asp:BoundField DataField="phone1" HeaderText="Phone" SortExpression="phone1" />
                            <asp:CheckBoxField DataField="Bounty" HeaderText="Bounty" SortExpression="Bounty" />
                            <asp:CheckBoxField DataField="PaidBounty" HeaderText="Dep Paid" SortExpression="PaidBounty" />
                            <asp:CheckBoxField DataField="BarnYard" HeaderText="BarnYard" SortExpression="BarnYard" />
                            <asp:CheckBoxField DataField="PaidBarnyard" HeaderText="Dep Paid" SortExpression="PaidBarnyard" />
                            <asp:CheckBoxField DataField="Ploughman" HeaderText="Ploughman" SortExpression="Ploughman" />
                            <asp:CheckBoxField DataField="PaidPloughman" HeaderText="Dep Paid" SortExpression="PaidPloughman" />
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    </asp:GridView>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane9" runat="server">
                <Header>Multiple Subscriptions</Header>
                <Content>
                    <asp:GridView ID="GridView10" runat="server" EmptyDataText="There are no subscribers with multiple subscriptions" CssClass="SGridViewClass" HeaderStyle-HorizontalAlign="Left" AutoGenerateColumns="False" OnRowDataBound="GridView10_RowDataBound" DataKeyNames="SubId">
                        <Columns>
                            <asp:BoundField DataField="FirstName1" HeaderText="First Name" SortExpression="FirstName1" />
                            <asp:BoundField DataField="LastName1" HeaderText="Last Name" SortExpression="LastName1" />
                            <asp:TemplateField HeaderText="Subscriptions">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="Count" Text='<%# Eval("Count")%>'></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" ID="detailslink" NavigateUrl='<%# Eval("SubID")%>'>Details</asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle Width="75px" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    </asp:GridView>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane10" runat="server" Visible="false" >
                <Header>
                            Assign Role</Header>
                <Content>
                    <asp:Label ID="Label9" runat="server"></asp:Label>
                    <%--<asp:GridView ID="gvAssignRole" runat="server" OnSelectedIndexChanged="edit_SelectedIndexChanged"
                        AutoGenerateColumns="False" HeaderStyle-HorizontalAlign="Left">
                        <Columns>
                            <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                            <asp:BoundField DataField="Email" HeaderText="E-Mail" SortExpression="Email" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                            <asp:BoundField DataField="Role" HeaderText="Role" SortExpression="Role" HeaderStyle-Width="190px" ItemStyle-Width="190px" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("UserId", "~/admin/AssignRole.aspx?Id={0}") %>' Text=" Assign User Roles" />                                    
                                </ItemTemplate>
                            </asp:TemplateField>                          
                        </Columns>
                    </asp:GridView>--%>
                    <br />
                </Content>
            </cc1:AccordionPane>
        </Panes>
    </cc1:Accordion>
    <br />

    <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="Select
  Subscribers.FirstName1,
  Subscribers.LastName1,
  Subscribers.Email1,
  Subscribers.phone1,
  Subscribers.Bounty,
  weekly.PaidBounty,
  Subscribers.BarnYard,
  weekly.PaidBarnyard,
  Subscribers.Ploughman,
  weekly.PaidPloughman
From
  Subscribers Inner Join
  weekly On Subscribers.SubId = weekly.SubId
Where
  ((Subscribers.Bounty = 'true' And
      weekly.PaidBounty = 'false') Or
    (Subscribers.BarnYard = 'true' And
      weekly.PaidBarnyard = 'false') Or
    (Subscribers.Ploughman = 'true' And
      weekly.PaidPloughman = 'false')) And
  weekly.Week = '1/1/1900' And
  Subscribers.Active = 'true'"></asp:SqlDataSource>
    <br />
    <br />

    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>"
        SelectCommand="SELECT [email], [role], [Username], [LastName], [FirstName] FROM [UserInfo] ORDER BY [LastName], [FirstName]"></asp:SqlDataSource>
    <br />
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>"
        SelectCommand="SELECT email, role, LastName, FirstName, Username FROM Userinfo where isapproved='true' ORDER BY Username"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>"
        SelectCommand="SELECT email, role, LastName, FirstName, Username FROM Userinfo where isapproved='false' ORDER BY Username"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>"
        SelectCommand="SELECT [store] FROM [stores]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>"
        SelectCommand="SELECT [pickupday] FROM [pickupdays]"></asp:SqlDataSource>
    <br />

    <br />
</asp:Content>

