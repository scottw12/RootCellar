<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ViewProductDetails.aspx.cs" Inherits="admin_ViewProductDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" CssClass="GridViewClass" ShowHeaderWhenEmpty="true" EmptyDataText="No Record To Display" DataKeyNames="SubscriberID">
                <Columns>
                    <asp:BoundField DataField="SubscriberID" HeaderText="SubscriberID" InsertVisible="False" ReadOnly="True" SortExpression="SubscriberID" Visible="false" />
                    <asp:BoundField DataField="Week" HeaderText="Week" SortExpression="Week" />
                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" SortExpression="ProductName" />
                    <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" HeaderStyle-Width="100px" >
<HeaderStyle Width="100px"></HeaderStyle>
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
    <br />
    <br />
    <asp:Button ID="btnBack" Text="Back" runat="server" PostBackUrl="~/admin/Pickups.aspx" />
</asp:Content>

