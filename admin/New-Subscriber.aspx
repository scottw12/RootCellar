<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="New-Subscriber.aspx.cs" Inherits="admin_New_Subscriber" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            font-size: small;
        }
    </style>
    <script type="text/javascript" lang="javascript">
        function Error() {
            alert("You have no permission to access this. Please Contact to admin");
            window.location = "../Admin/Default.aspx";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Literal runat="server" ID="Literal0"></asp:Literal>
            <asp:Panel runat="server" ID="Panel1">
                <h1>Create New Account</h1>
                <hr />
                <table>
                    <tr>
                        <td style="vertical-align: top;">
                            <asp:Literal runat="server" ID="Literal1"></asp:Literal><br />
                            <asp:CheckBox runat="server" ID="NextYear" Text="Create user for next season" OnCheckedChanged="NextChanged" AutoPostBack="true"></asp:CheckBox><br />
                            Subscription
    <asp:CheckBox runat="server" ID="BarnyardBox" Text="Barnyard - $35.00" OnCheckedChanged="OnBoxChanged" AutoPostBack="true"></asp:CheckBox>
                            <asp:CheckBox runat="server" ID="BountyBox" Text="Bounty - $35.00" OnCheckedChanged="OnBoxChanged" AutoPostBack="true"></asp:CheckBox>
                            <asp:CheckBox runat="server" ID="PloughmanBox" Text="Ploughman - $35.00" OnCheckedChanged="OnBoxChanged" AutoPostBack="true"></asp:CheckBox>
                            <br />
                            Allergies:<br />
                            <asp:TextBox ID="allergies" runat="server" class="text-input" TextMode="multiline" Width="500px" Rows="4"></asp:TextBox>
                            <br />
                            <br />
                            <asp:UpdatePanel runat="server" ID="tableUPanel">
                                <ContentTemplate>
                                    Store:
            <asp:DropDownList runat="server" ID="StoreList" AutoPostBack="True" OnSelectedIndexChanged="StoreList_SelectedIndexChanged1"></asp:DropDownList>
                                    <span style="color: red;">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please select a Store" ControlToValidate="StoreList"></asp:RequiredFieldValidator></span><br />
                                    Pickup Day:
            <asp:DropDownList runat="server" ID="PickupDayList" AutoPostBack="True" OnSelectedIndexChanged="PickupDayList_SelectedIndexChanged1"></asp:DropDownList>
                                    <span style="color: red;">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please select a Pickup Day" ControlToValidate="PickupDayList"></asp:RequiredFieldValidator>
                                        <br />
                                        <span style="color: red;">
                                            <asp:Literal runat="server" ID="PUDLiteral"></asp:Literal></span>
                                    </span>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="PickupDayList" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="storelist" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <h3>Subscriber 1</h3>
                            First Name:<br />
                            <asp:TextBox ID="firstname1" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                            <span style="color: red;">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter your First Name" ControlToValidate="firstname1"></asp:RequiredFieldValidator></span><br />
                            Last Name:<br />
                            <asp:TextBox ID="lastname1" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                            <span style="color: red;">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your Last Name" ControlToValidate="lastname1"></asp:RequiredFieldValidator></span><br />
                            Email:<br />
                            <asp:TextBox ID="email1" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                            <span style="color: red;">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter your Email Address" ControlToValidate="email1"></asp:RequiredFieldValidator></span><br />
                            Phone:<br />
                            <asp:TextBox ID="phone1" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                            <span style="color: red;">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please enter your phone number" ControlToValidate="phone1"></asp:RequiredFieldValidator></span><br />
                            <br />
                            <h3>Subscriber 2</h3>
                            First Name:<br />
                            <asp:TextBox ID="firstname2" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                            <br />
                            Last Name:<br />
                            <asp:TextBox ID="lastname2" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                            <br />
                            Email:<br />
                            <asp:TextBox ID="email2" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                            <br />
                            Phone:<br />
                            <asp:TextBox ID="phone2" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                            <br />
                            <br />
                            Address:<br />
                            <asp:TextBox ID="address" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                            <span style="color: red;">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Please enter your address" ControlToValidate="address"></asp:RequiredFieldValidator></span><br />
                            City: 
                                            <asp:TextBox ID="city" runat="server" class="text-input" Width="172px"></asp:TextBox>
                            &nbsp;&nbsp;State: 
                                            <asp:TextBox ID="state" runat="server" class="text-input" Width="70px"></asp:TextBox>
                            &nbsp;&nbsp;Zip: 
                                            <asp:TextBox ID="zip" runat="server" class="text-input" Width="100px"></asp:TextBox><br />
                            <span style="color: red;">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Please enter your city" ControlToValidate="city"></asp:RequiredFieldValidator></span>&nbsp;&nbsp;
                                            <span style="color: red;">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Please enter your state" ControlToValidate="state"></asp:RequiredFieldValidator></span>&nbsp;&nbsp;
                                            <span style="color: red;">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Please enter your zip code" ControlToValidate="zip"></asp:RequiredFieldValidator></span><br />
                            <h3>Subscribe to our Newsletter's</h3>
                            <asp:CheckBox ID="BountyNL" runat="server" />Bounty Box Summer 2015<br />
                            <asp:CheckBox ID="BarnyardNL" runat="server" />Barnyard Box Summer 2015<br />
                            <asp:CheckBox ID="PloughmanNL" runat="server" />Ploughman Box Summer 2015<br />

                            <br />
                            <br />
                            Payment Due: <span style="color: red; font-weight: bold;">
                                <asp:Literal runat="server" ID="Price"></asp:Literal></span>
                            <asp:Button ID="Button1" runat="server" Text="Create Account" class="submit" OnClick="Button1_Click" />
                        </td>
                        <td style="vertical-align: top">Weeks to pay for
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="GridViewClass" AllowSorting="False">
                    <Columns>
                        <asp:BoundField DataField="Week" HeaderText="Week" SortExpression="Week" />
                        <asp:TemplateField HeaderText="Bounty">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="BountyPaidCheck" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Barnyard">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="BarnyardPaidCheck" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ploughman">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="PloughmanPaidCheck" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>




                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

