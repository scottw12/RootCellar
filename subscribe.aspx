<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="subscribe.aspx.vb" Inherits="subscribe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tr>
                    <td style="vertical-align: top;">
                        <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                        <asp:Literal runat="server" ID="Literal0"></asp:Literal>
                        <asp:Panel runat="server" ID="AppPanel">
                        <h2>Select a Subscription</h2>
                        <asp:Panel runat="server" ID="CurrentSeason">
                        <asp:CheckBox runat="server" ID="BarnyardBox" Text="Barnyard - $35.00" OnCheckedChanged="OnBoxChanged" AutoPostBack="true"></asp:CheckBox><asp:Literal ID="BarnyardActive" runat="server"></asp:Literal><br />
                        <asp:CheckBox runat="server" ID="BountyBox" Text="Bounty - $35.00" OnCheckedChanged="OnBoxChanged" AutoPostBack="true"></asp:CheckBox><asp:Literal ID="BountyActive" runat="server"></asp:Literal><br />
                        <asp:CheckBox runat="server" ID="PloughmanBox" Text="Ploughman - $35.00" OnCheckedChanged="OnBoxChanged" AutoPostBack="true"></asp:CheckBox><asp:Literal ID="PloughmanActive" runat="server"></asp:Literal>
                            </asp:Panel>
                        <asp:Panel runat="server" ID="NextSeason" Visible="false">
                            <h3>Subscribe to the <asp:Literal ID="Literal3" runat="server"></asp:Literal> season</h3>
                        <asp:CheckBox runat="server" ID="NewBarnyardBox" Text="Barnyard - $35.00" OnCheckedChanged="OnBoxChanged" AutoPostBack="true"></asp:CheckBox><br />
                        <asp:CheckBox runat="server" ID="NewBountyBox" Text="Bounty - $35.00" OnCheckedChanged="OnBoxChanged" AutoPostBack="true"></asp:CheckBox><br />
                        <asp:CheckBox runat="server" ID="NewPloughmanBox" Text="Ploughman - $35.00" OnCheckedChanged="OnBoxChanged" AutoPostBack="true"></asp:CheckBox>
                        </asp:Panel>
                        <h3>Subscriber 1</h3>
                        First Name:<br />
                        <asp:TextBox ID="firstname1" runat="server" class="text-input" Width="500px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter your First Name" ForeColor="red" ControlToValidate="firstname1"></asp:RequiredFieldValidator><br />
                        Last Name:<br />
                        <asp:TextBox ID="lastname1" runat="server" class="text-input" Width="500px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your Last Name" ForeColor="red" ControlToValidate="lastname1"></asp:RequiredFieldValidator><br />
                        Email:<br />
                        <asp:TextBox ID="email1" runat="server" class="text-input" Width="500px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter your Email Address" ForeColor="red" ControlToValidate="email1"></asp:RequiredFieldValidator><br />
                        Phone:<br />
                        <asp:TextBox ID="phone1" runat="server" class="text-input" Width="500px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please enter your Phone Number" ForeColor="red" ControlToValidate="phone1"></asp:RequiredFieldValidator><br />
                        <br />
                        <h3>Subscriber 2</h3>
                        First Name:<br />
                        <asp:TextBox ID="firstname2" runat="server" class="text-input" Width="500px"></asp:TextBox>
                        <br />
                        Last Name:<br />
                        <asp:TextBox ID="lastname2" runat="server" class="text-input" Width="500px"></asp:TextBox>
                        <br />
                        Email:<br />
                        <asp:TextBox ID="email2" runat="server" class="text-input" Width="500px"></asp:TextBox>
                        <br />
                        Phone:<br />
                        <asp:TextBox ID="phone2" runat="server" class="text-input" Width="500px"></asp:TextBox>
                        <br />
                        <br />
                        Address:<br />
                        <asp:TextBox ID="address" runat="server" class="text-input" Width="500px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Please enter your Address" ForeColor="red" ControlToValidate="address"></asp:RequiredFieldValidator><br />
                        City: 
                                            <asp:TextBox ID="city" runat="server" class="text-input" Width="172px"></asp:TextBox>
                        &nbsp;&nbsp;State: 
                                            <asp:TextBox ID="state" runat="server" class="text-input" Width="70px"></asp:TextBox>
                        &nbsp;&nbsp;Zip: 
                                            <asp:TextBox ID="zip" runat="server" class="text-input" Width="100px"></asp:TextBox><br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Please enter your City" ForeColor="red" ControlToValidate="city"></asp:RequiredFieldValidator>&nbsp;&nbsp;
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Please enter your State" ForeColor="red" ControlToValidate="state"></asp:RequiredFieldValidator>&nbsp;&nbsp;
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Please enter your Zip Code" ForeColor="red" ControlToValidate="zip"></asp:RequiredFieldValidator><br />
                        Allergies:<br />
                        <asp:TextBox ID="allergies" runat="server" class="text-input" TextMode="multiline" Width="500px" Rows="4"></asp:TextBox>
                        <br />
                        <asp:UpdatePanel runat="server" ID="tableUPanel">
                            <ContentTemplate>
                                Store:
            <asp:DropDownList runat="server" ID="StoreList" AutoPostBack="True"></asp:DropDownList>
                                <span style="color: red;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please select a Store" ControlToValidate="StoreList"></asp:RequiredFieldValidator></span><br />
                                Pickup Day:
            <asp:DropDownList runat="server" ID="PickupDayList" AutoPostBack="True"></asp:DropDownList>
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
                        <br />
                        Refered By:<br />
                        <asp:TextBox ID="Refered" runat="server" class="text-input" Width="500px"></asp:TextBox>
                        <br />
                        <br />
                        Subscribe to our Newsletter's<br />
                        <asp:CheckBox ID="BountyNL" runat="server" Text="Bounty Box Spring 2015" /><br />
                        <asp:CheckBox ID="BarnyardNL" runat="server" Text="Barnyard Box Spring 2015" /><br />
                        <asp:CheckBox ID="PloughmanNL" runat="server" Text="Ploughman Box Spring 2015" /><br />
                        <br />
                        <br />
                        <%--</td>
            <td style="vertical-align:top">--%>
                        <%--Weeks to pay for
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="GridViewClass" AllowSorting="False" Visible="false">
                    <Columns>
                        <asp:BoundField DataField="Week" HeaderText="Week" SortExpression="Week" ControlStyle-Width="200px" />
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
                </asp:GridView>--%>

                        <%--<br /><br />Payment Due: <span style ="color:red; font-weight:bold;"><asp:Literal runat="server" ID="Price"></asp:Literal></span> + tax<br /> --%><asp:Button ID="Button1" runat="server" Text="Subscribe" class="submit" />
                        </div>
                            </asp:Panel></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

