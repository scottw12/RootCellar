<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="details.aspx.cs" Inherits="admin_details" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <h1>Subscriber Account</h1>
    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    <hr />
    <cc1:Accordion ID="AccordionCtrl" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
        ContentCssClass="accordionContent" AutoSize="None" FadeTransitions="true">
        <Panes>
            <cc1:AccordionPane ID="SubscriptionsPane" runat="server">
                <Header>
<asp:Literal runat="server" ID="HeaderLiteral"></asp:Literal> Subscriptions</Header>
                <Content>
                    Food Box
                                            <asp:CheckBoxList runat="server" ID="BoxButton">
                                                <asp:ListItem Text="Barnyard - $35.00"></asp:ListItem>
                                                <asp:ListItem Text="Bounty - $35.00"></asp:ListItem>
                                                <asp:ListItem Text="Ploughman - $35.00"></asp:ListItem>
                                            </asp:CheckBoxList><br />
                    <br />
                    <asp:UpdatePanel runat="server" ID="tableUPanel">
                        <ContentTemplate>
                            Pickup Day:
            <asp:DropDownList runat="server" ID="PickupDayList" AutoPostBack="true" OnSelectedIndexChanged="PickupDayList_SelectedIndexChanged"></asp:DropDownList><span style="color: red;"><asp:Literal runat="server" ID="PUDLiteral"></asp:Literal></span><br />
                            Store:
            <asp:DropDownList runat="server" ID="StoreList" AutoPostBack="true" OnSelectedIndexChanged="StoreList_SelectedIndexChanged"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="PickupDayList" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="storelist" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <br />
                    <center><asp:Button ID="Button3" runat="server" Text="Update" class="submit" OnClick="Button3_Click1"  /></center>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccountPane" runat="server">
                <Header>Subscriber Info</Header>
                <Content>
                    <h3>Subscriber 1</h3>
                    First Name:<br />
                    <asp:TextBox ID="firstname1" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter your First Name" ControlToValidate="firstname1"></asp:RequiredFieldValidator><br />
                    Last Name:<br />
                    <asp:TextBox ID="lastname1" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your Last Name" ControlToValidate="lastname1"></asp:RequiredFieldValidator><br />
                    Email:<br />
                    <asp:TextBox ID="email1" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter your Email Address" ControlToValidate="email1"></asp:RequiredFieldValidator><br />
                    Phone:<br />
                    <asp:TextBox ID="phone1" runat="server" class="text-input" Width="500px"></asp:TextBox><br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please enter your phone number" ControlToValidate="phone1"></asp:RequiredFieldValidator><br />
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
                    Allergies:<br />
                    <asp:TextBox ID="allergies" runat="server" class="text-input" TextMode="multiline" Width="500px" Rows="4"></asp:TextBox><br />
                    <br />
                    Newsletter's<br />
                    <asp:CheckBox ID="BountyNL" runat="server" Text="Bounty Box Summer 2015" /><br />
                    <asp:CheckBox ID="BarnyardNL" runat="server" Text="Barnyard Box Summer 2015" /><br />
                    <asp:CheckBox ID="PloughmanNL" runat="server" Text="Ploughman Box Summer 2015" /><br />
                    <br />
                    <h3>
                        <asp:Literal ID="litDelivery" runat="server" /></h3>
                    <br />
                    <center><asp:Button ID="Button1" runat="server" OnClick="Button1_Click1" Text="Update" class="submit" /></center>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="NotesPane" runat="server">
                <Header>Notes</Header>
                <Content>
                    <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="PaymentsPane" runat="server">
                <Header>Payments</Header>
                <Content>
                    <center>
                        <table>
                            <tr>
                                <td>
                         <h3>Additional Products</h3>
                            <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" CssClass="GridViewClass" ShowHeaderWhenEmpty="true" EmptyDataText="No Record To Display" DataKeyNames="SubscriberID"  Visible="false">
                                <Columns>
                                    <asp:BoundField DataField="SubscriberID" HeaderText="SubscriberID" InsertVisible="False" ReadOnly="True" SortExpression="SubscriberID" Visible="false" />
                                    <asp:BoundField DataField="Week" HeaderText="Week" SortExpression="Week" />
                                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" SortExpression="ProductName" />
                                    <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" SortExpression="Price" />
                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" HeaderStyle-Width="100px">
                                        <HeaderStyle Width="100px"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="IsPaid" HeaderText="Status" SortExpression="Price" />
                                </Columns>
                            </asp:GridView>
                                    </td>
                                <td>
                  <%--  <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="GridViewClass" AllowSorting="False" DataKeyNames="SubId">
                    <Columns>
                        <asp:BoundField DataField="SubId" HeaderText="SubId" InsertVisible="False" ReadOnly="True" SortExpression="SubId" Visible="false" />
                        <asp:BoundField DataField="Week" HeaderText="Week" SortExpression="Week" />
                        <asp:TemplateField HeaderText="Bounty">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="BountyPaidCheck" Checked='<%#Convert.ToBoolean(Eval("PaidBounty"))%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Barnyard">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="BarnyardPaidCheck" Checked='<%# Convert.ToBoolean(Eval("PaidBarnyard"))%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ploughman">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="PloughmanPaidCheck" Checked='<%# Convert.ToBoolean(Eval("PaidPloughman"))%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>--%>
                                    
                                    
                             <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="GridViewClass" AllowSorting="False" DataKeyNames="SubId" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="SubId" HeaderText="SubId" InsertVisible="False" ReadOnly="True" SortExpression="SubId" Visible="false" />
                                <asp:BoundField DataField="Week" HeaderText="Week" SortExpression="Week" />
                                <asp:TemplateField HeaderText="Bounty">
                                    <ItemTemplate>
                                        <%--<asp:CheckBox runat="server" ID="BountyPaidCheck" Checked='<%# Eval("PaidBounty").ToString().Equals("1")%>' Enabled='<%#Convert.ToBoolean(Eval("PaidBounty"))%>' AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />--%>
                                        <asp:CheckBox runat="server" ID="BountyPaidCheck" Checked='<%# Eval("PaidBounty").ToString().Equals("True")%>' AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Barnyard">
                                    <ItemTemplate>
                                        <%--<asp:CheckBox runat="server" ID="BarnyardPaidCheck" Checked='<%# Eval("PaidBarnyard").ToString().Equals("1")%>' Enabled='<%# Convert.ToBoolean(Eval("PaidBarnyard"))%>' AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />--%>
                                        <asp:CheckBox runat="server" ID="BarnyardPaidCheck" Checked='<%# Eval("PaidBarnyard").ToString().Equals("True")%>' AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ploughman">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="PloughmanPaidCheck" Checked='<%# Eval("PaidPloughman").ToString().Equals("True")%>' AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />
                                        <%--<asp:CheckBox runat="server" ID="CheckBox1" Checked='<%# Eval("PaidPloughman").ToString() == "1" ? true:false %>' AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />--%>
                                        <%--<asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" Checked='<%# Eval("PaidPloughman").ToString() == "1" ? true:false %>' OnCheckedChanged="OnCheckedChanged" />--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Additional Products">
                                    <ItemTemplate>
                                        <asp:Button ID="btnAddtionalProduct" Text="View" runat="server" CommandName="AddtionalProduct" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Paid">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="cbPaid"  Enabled="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Home Delivery"  ItemStyle-Width="40px" >
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="cbHomedelivery" Checked='<%# Eval("HomeDelivery").ToString().Equals("True")%>' Enabled="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                
                            </Columns>
                        </asp:GridView>         
                                    
                                    
                                    <br />
                                    </td>
                                </tr>
                            </table>
                    <span style="color:Red;">CAUTION: This cannot be undone!</span>
                    <asp:Button ID="Button2" runat="server" Text="Update Payments" OnClick="Button2_Click1" class="submit" /></center>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane1" runat="server">
                <Header>Username-Password Reset</Header>
                <Content>
                    New Password:
                    <asp:TextBox ID="TextBox3" runat="server" TextMode="Password" class="text-input"></asp:TextBox><br />
                    <asp:CheckBox ID="CheckBox1" runat="server" />Send Email notification
                        <br />
                    <asp:Button ID="PassResetButton" runat="server" Text="Reset Password" CssClass="submit" OnClick="PassResetButton_Click" />
                    <br />
                    <br />
                    Current Username:
                    <asp:Literal runat="server" ID="UsernameLiteral"></asp:Literal><br />
                    New Username:
                    <asp:TextBox ID="TextBox1" runat="server" class="text-input"></asp:TextBox><br />
                    <asp:CheckBox ID="CheckBox2" runat="server" />Send Email notification
                        <br />
                    <asp:Button ID="Button4" runat="server" Text="Reset Username" CssClass="submit" OnClick="Button4_Click" />
                </Content>
            </cc1:AccordionPane>
        </Panes>
    </cc1:Accordion>
</asp:Content>

