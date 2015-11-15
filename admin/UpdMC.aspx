<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UpdMC.aspx.cs" Inherits="admin_UpdMC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Literal runat="server" ID="Literal0"></asp:Literal>
    <asp:GridView runat="server" ID="gv1" AutoGenerateColumns="False" DataSourceID="SqlDataSource1">
        <Columns>
            <asp:BoundField DataField="FirstName1" HeaderText="FirstName1" SortExpression="FirstName1"></asp:BoundField>
            <asp:BoundField DataField="LastName1" HeaderText="LastName1" SortExpression="LastName1"></asp:BoundField>
            <asp:BoundField DataField="email1" HeaderText="email1" SortExpression="email1"></asp:BoundField>
            <asp:BoundField DataField="PickupDay" HeaderText="PickupDay" SortExpression="PickupDay"></asp:BoundField>
            <asp:CheckBoxField DataField="BountyNL" HeaderText="BountyNL" SortExpression="BountyNL"></asp:CheckBoxField>
            <asp:CheckBoxField DataField="BarnyardNL" HeaderText="BarnyardNL" SortExpression="BarnyardNL"></asp:CheckBoxField>
            <asp:CheckBoxField DataField="PloughmanNL" HeaderText="PloughmanNL" SortExpression="PloughmanNL"></asp:CheckBoxField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:DefaultConnection %>' SelectCommand="SELECT [FirstName1], [LastName1], [email1], [PickupDay], [BountyNL], [BarnyardNL], [PloughmanNL] FROM [Subscribers]"></asp:SqlDataSource>
    <asp:GridView runat="server" ID="gv2" AutoGenerateColumns="False" DataSourceID="SqlDataSource2">
        <Columns>
            <asp:BoundField DataField="FirstName2" HeaderText="FirstName2" SortExpression="FirstName2"></asp:BoundField>
            <asp:BoundField DataField="LastName2" HeaderText="LastName2" SortExpression="LastName2"></asp:BoundField>
            <asp:BoundField DataField="email2" HeaderText="email2" SortExpression="email2"></asp:BoundField>
            <asp:BoundField DataField="PickupDay" HeaderText="PickupDay" SortExpression="PickupDay"></asp:BoundField>
            <asp:CheckBoxField DataField="BountyNL" HeaderText="BountyNL" SortExpression="BountyNL"></asp:CheckBoxField>
            <asp:CheckBoxField DataField="BarnyardNL" HeaderText="BarnyardNL" SortExpression="BarnyardNL"></asp:CheckBoxField>
            <asp:CheckBoxField DataField="PloughmanNL" HeaderText="PloughmanNL" SortExpression="PloughmanNL"></asp:CheckBoxField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:DefaultConnection %>' SelectCommand="SELECT [FirstName2], [LastName2], [email2], [PickupDay], [BountyNL], [BarnyardNL], [PloughmanNL] FROM [Subscribers] where email2 <> ''"></asp:SqlDataSource>
</asp:Content>

