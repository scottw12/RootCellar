<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="products.aspx.cs" Inherits="products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="center">
            <h1>LOCAL FOOD BOXES</h1>
            <h3>Eating locally, one week at a time</h3>
        </div>

        <div class="threediv">
            <asp:LinkButton runat="server" ID="Barnyard" OnClick="Barnyard_click">
            <div class="col">
                <img src="images/barnyard.jpg" /><p>Missouri Barnyard Box</p><div class="price">$35.00</div>
            </div></asp:LinkButton>
            <asp:LinkButton runat="server" ID="Bounty" OnClick="Bounty_click">
            <div class="col">
                <img src="images/bounty.jpg" /><p>Missouri Bounty Box</p><div class="price">$35.00</div>
            </div></asp:LinkButton>
                <asp:LinkButton runat="server" ID="Ploughman" OnClick="Ploughman_click">
            <div class="col">
                <img src="images/ploughmans.jpg" /><p>Missouri Ploughman's Box</p><div class="price">$35.00</div>
            </div></asp:LinkButton>
        </div>
</asp:Content>

