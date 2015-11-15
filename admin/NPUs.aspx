<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="NPUs.aspx.cs" Inherits="admin_NPUs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Literal runat="server" ID="Literal1"></asp:Literal>
    <div class="threediv">
        <div>
            <a href="#">Download</a>
        </div>
        <div>
            Store:
            <asp:DropDownList runat="server" ID="StoreList" AutoPostBack="true" >
                <asp:ListItem Text=""></asp:ListItem>
                <asp:ListItem Text="Downtown Columbia"></asp:ListItem>
                <asp:ListItem Text="Jefferson City"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div>
            Pickup Day:
            <asp:DropDownList runat="server" ID="PickupDay" AutoPostBack="true">
                <asp:ListItem Text=""></asp:ListItem>
                <asp:ListItem Text="Thursday"></asp:ListItem>
                <asp:ListItem Text="Friday"></asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <br />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="GridViewClass" AllowSorting="True" DataSourceID="SqlDataSource1" >
        <Columns>
            <asp:BoundField DataField="LastName1" HeaderText="LastName1" SortExpression="LastName1" />
            <asp:BoundField DataField="FirstName1" HeaderText="FirstName1" SortExpression="FirstName1" />
            <asp:BoundField DataField="phone1" HeaderText="phone1" SortExpression="phone1" />
            <asp:CheckBoxField DataField="Bounty" HeaderText="Bounty" SortExpression="Bounty" />
            <asp:CheckBoxField DataField="BarnYard" HeaderText="BarnYard" SortExpression="BarnYard" />
            <asp:CheckBoxField DataField="Ploughman" HeaderText="Ploughman" SortExpression="Ploughman" />
            <asp:BoundField DataField="PickupDay" HeaderText="PickupDay" SortExpression="PickupDay" />
            <asp:BoundField DataField="Store" HeaderText="Store" SortExpression="Store" />
       </Columns>
        </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>"> </asp:SqlDataSource>
</asp:Content>

