<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Subscribers.aspx.cs" Inherits="admin_Subscribers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" lang="javascript">
        //function Error() {
        //    alert("You have no permission to access this. Please Contact to admin");
        //    window.location = "../Admin/Default.aspx";
        //}
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:Literal runat="server" ID="lit1"></asp:Literal><br />
    <asp:UpdateProgress runat="server" id="PageUpdateProgress">
            <ProgressTemplate>
               <strong> Please Wait your request is Loading...</strong>
                <img src="../images/ajax-loader.gif" />
            </ProgressTemplate>
        </asp:UpdateProgress>
    <asp:UpdatePanel runat="server" ID="tableUPanel" UpdateMode="Conditional"  >
    <ContentTemplate>
    <div class="threediv">
        <div>
            <asp:DropDownList runat="server" ID="WeekList" AutoPostBack="True" OnSelectedIndexChanged="WeekList_SelectedIndexChanged1" ></asp:DropDownList><br />
            Show inactive: <asp:Checkbox runat="server" ID="Showinactive"  OnCheckedChanged="Showinactive_CheckedChanged" AutoPostBack="True" ></asp:Checkbox>
        </div>
        <div>
            Store:
            <asp:DropDownList runat="server" ID="StoreList" AutoPostBack="True" OnSelectedIndexChanged="StoreList_SelectedIndexChanged1" ></asp:DropDownList>
       </div>
        <div>
            Pickup Day:
            <asp:DropDownList runat="server" ID="PickupDayList" AutoPostBack="true" OnSelectedIndexChanged="PickupDayList_SelectedIndexChanged"></asp:DropDownList>
        </div>
    </div>
    <span style="color:red;"><asp:Literal runat="server" ID="PUDLiteral"></asp:Literal></span>
    
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="SubGridViewClass" AllowSorting="True" OnRowCommand="GridView1_RowCommand" DataKeyNames="SubId" OnRowDataBound="GridView1_RowDataBound">
        <Columns>
            <asp:BoundField DataField="SubId" HeaderText="SubId" InsertVisible="False" ReadOnly="True" SortExpression="SubId" Visible="false" />
            <asp:BoundField DataField="LastName1" HeaderText="Last Name 1" SortExpression="LastName1" />
            <asp:BoundField DataField="FirstName1" HeaderText="First Name 1" SortExpression="FirstName1" />
            <asp:BoundField DataField="LastName2" HeaderText="Last Name 2" SortExpression="LastName2" />
            <asp:BoundField DataField="FirstName2" HeaderText="First Name 2" SortExpression="FirstName2" />
            <asp:TemplateField Headertext="Pay">
                <ItemTemplate>
                    <asp:ImageButton ID="btnpay" runat="server" CommandName="Pay" ImageUrl="~/images/Pay.png"></asp:ImageButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField Headertext="Picked up">
                <ItemTemplate>
                    <asp:ImageButton ID="pickedup" runat="server" ImageUrl="~/images/apptrue.gif"></asp:ImageButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Vacation">
                <ItemTemplate>
                    <asp:ImageButton ID="btnvac" runat="server" CommandName="Vacation" ImageUrl="~/images/Vacation.png"></asp:ImageButton>
                    <%--<asp:Button runat="server" ID="ForceVac" CommandName="ForceVacation" Text="Extra Vacation" />--%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Details">
                <ItemTemplate>
                    <asp:ImageButton ID="btndet" runat="server" CommandName="Details" ImageUrl="~/images/details.png"></asp:ImageButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
        </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="PickupDayList" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="storelist" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="Showinactive" EventName="CheckedChanged" />
    </Triggers>
</asp:UpdatePanel>
</asp:Content>

