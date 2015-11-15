<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="BuyProduct.aspx.cs" Inherits="customer_BuyProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Repeater ID="rcProducts" runat="server" OnItemCommand="rcProducts_ItemCommand" >
        <ItemTemplate>
            <div>
                <asp:Image ID="imgProductImage" runat="server" ImageUrl='<%# "../admin/ProductsImage/"+Eval("ProductImage")%>' Height="150px" Width="150px" />
                <br />                
                <asp:Label ID="ProductID" Text='<%#Eval("ProductID") %>' Visible="false" runat="server"></asp:Label>
                <asp:Label ID="lblProductName" Text='<%#Eval("ProductName") %>' runat="server"></asp:Label>
                <br />
                <asp:Label ID="lblPrice" Text='<%#Eval("ProductPrice") %>' runat="server"></asp:Label>
                <br />                
                Enter Quantity:<asp:TextBox ID="txtQuantity" runat="server"></asp:TextBox>
                <br />
                <asp:CheckBox ID="cbAddToCart" Text="Add to Cart" runat="server"/>
                <br />
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Button ID="btnAddCart" runat="server" Text="Add To Cart" OnClick="btnAddCart_Click" />
    <br />

</asp:Content>

