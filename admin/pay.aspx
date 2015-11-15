<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="pay.aspx.cs" Inherits="admin_pay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 500px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel runat="server" ID="tableUPanel" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tr>
                    <td style="vertical-align: top; width: 420px;">
                        <h3>
                            <asp:Literal runat="server" ID="Literal1"></asp:Literal></h3>
                        <asp:Literal runat="server" ID="Literal2"></asp:Literal>
                        <br />
                        <asp:Panel runat="server" ID="step1">
                            Payment Due: <span style="color: red; font-weight: bold;">
                                <asp:Literal runat="server" ID="Price"></asp:Literal></span>

                            <br />
                            <asp:Button ID="Button1" runat="server" Text="In Store Payment Made" OnClick="Button1_Click" class="submit" />
                            <h3>Additional Products</h3>
                            <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" CssClass="GridViewClass" ShowHeaderWhenEmpty="true" EmptyDataText="No Record To Display" DataKeyNames="SubscriberID" Visible="false">
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
                        </asp:Panel>
                    </td>
                    <td style="text-align: right;">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="GridViewClass" AllowSorting="False" DataKeyNames="SubId" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="SubId" HeaderText="SubId" InsertVisible="False" ReadOnly="True" SortExpression="SubId" Visible="false" />
                                <asp:BoundField DataField="Week" HeaderText="Week" SortExpression="Week" />
                                <asp:TemplateField HeaderText="Bounty">
                                    <ItemTemplate>
                                        <%--<asp:CheckBox runat="server" ID="BountyPaidCheck" Checked='<%# Eval("PaidBounty").ToString().Equals("1")%>' Enabled='<%#Convert.ToBoolean(Eval("PaidBounty"))%>' AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />--%>
                                        <asp:CheckBox runat="server" ID="BountyPaidCheck" Checked='<%# Eval("PaidBounty").ToString().Equals("True")%>'  AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Barnyard">
                                    <ItemTemplate>
                                        <%--<asp:CheckBox runat="server" ID="BarnyardPaidCheck" Checked='<%# Eval("PaidBarnyard").ToString().Equals("1")%>' Enabled='<%# Convert.ToBoolean(Eval("PaidBarnyard"))%>' AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />--%>
                                        <asp:CheckBox runat="server" ID="BarnyardPaidCheck" Checked='<%# Eval("PaidBarnyard").ToString().Equals("True")%>'  AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ploughman">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="PloughmanPaidCheck" Checked='<%# Eval("PaidPloughman").ToString().Equals("True")%>'  AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />
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
                                <asp:TemplateField HeaderText="Home Delivery">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="cbHomedelivery" Checked='<%# Eval("HomeDelivery").ToString().Equals("True")%>' Enabled="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                
                            </Columns>
                        </asp:GridView>
                    </td>

                    <td style="text-align: right;">
                        <asp:GridView ID="gvProduct" runat="server" AutoGenerateColumns="False" CssClass="GridViewClass" OnRowDataBound="gvProduct_RowDataBound" Visible="false">
                            <Columns>
                                <asp:BoundField DataField="Week" HeaderText="Week" SortExpression="Week" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

