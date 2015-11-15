<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="pickup.aspx.cs" Inherits="admin_pickup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:DropDownList runat="server" ID="WeekList" Visible="false" AutoPostBack="True"></asp:DropDownList><br />
    <asp:Literal runat="server" ID="literal1"></asp:Literal><br />
    <br />
    <asp:Panel runat="server" ID="step0">
        Subscriber has set this week as their vacation week!
    </asp:Panel>
    <br />
    <br />
   <h3>Additional Products</h3>
    <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" CssClass="GridViewClass" ShowHeaderWhenEmpty="true" EmptyDataText="No Record To Display" DataKeyNames="SubscriberID">
        <Columns>
            <asp:BoundField DataField="SubscriberID" HeaderText="SubscriberID" InsertVisible="False" ReadOnly="True" SortExpression="SubscriberID" Visible="false" />
            <asp:BoundField DataField="Week" Visible="false" HeaderText="Week" SortExpression="Week" />
            <asp:BoundField DataField="ProductName" HeaderText="Product Name" SortExpression="ProductName" />
            <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" SortExpression="Price" />
            <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" HeaderStyle-Width="100px">
                <HeaderStyle Width="100px"></HeaderStyle>
            </asp:BoundField>
            <asp:BoundField DataField="IsPaid" HeaderText="Status" SortExpression="Price" />
        </Columns>
    </asp:GridView>
    <br />
    <br />
    <asp:Panel runat="server" ID="step1" >
       
        <asp:Literal runat="server" ID="literal2"></asp:Literal>
        <br />
        <br />
        <b>Current Notes:</b><br />
        <asp:Literal runat="server" ID="literal3"></asp:Literal><br />
         <h4>Have you double checked the box contents?</h4><br />
        <div class="center">
            <asp:Button ID="Yes1" runat="server" Text="Yes" OnClick="Yes1_Click" class="submit" />
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="step2">
        <asp:Literal runat="server" ID="literal4"></asp:Literal>
        <br />
        <asp:Panel runat="server" ID="step2b">
            <asp:Button ID="PaidButton" runat="server" Text="Paid in Store" OnClick="PaidButton_Click" class="submit" />
        </asp:Panel>

    </asp:Panel>
    <asp:Panel runat="server" ID="step3">
        <asp:Literal runat="server" ID="literal5"></asp:Literal><br />
        <div class="center">
            <asp:Button ID="Complete" runat="server" Text="Pickup Complete" OnClick="Complete_Click" class="submit" />
        </div>
    </asp:Panel>
</asp:Content>

