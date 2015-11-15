<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProductPay.aspx.cs" Inherits="customer_ProductPay" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:DropDownList ID="ddlWeeks" OnSelectedIndexChanged="ddlWeeks_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
    <br />
    <asp:GridView ID="gvSelectedProduct" runat="server" AllowPaging="True"
        class="table table-striped table-hover table-bordered" data-ride="datatables"
        AutoGenerateColumns="False" CellPadding="4" DataKeyNames=" ProductId"
        GridLines="Both" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
        ForeColor="#333333" OnRowDataBound="gvSelectedProduct_RowDataBound" OnRowCommand="gvSelectedProduct_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="ProductId" ShowHeader="False" Visible="False" SortExpression="ProductId">
                <ItemTemplate>
                    <asp:Label ID="ProductId" runat="server" Text='<%#Eval("ProductId")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Sr.No
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:HiddenField ID="hfId" runat="server" Value='<%#Eval("Id")%>'></asp:HiddenField>
                    <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ProductName" HeaderText="ProductName" SortExpression="ProductName" />

            <asp:BoundField DataField="ProductPrice" DataFormatString="${0:f}" HeaderText="Product Price" SortExpression="ProductPrice" />
            <%-- <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />--%>
            <asp:TemplateField HeaderText="Quantity" ShowHeader="False">
                <ItemTemplate>
                    <asp:Label ID="lblQuantity" runat="server" Text='<%#Eval("Quantity")%>' Visible="false"></asp:Label>
                    <asp:TextBox ID="txtQuantity" runat="server" Text='<%#Eval("Quantity")%>' ></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Total Price" ShowHeader="False">
                <ItemTemplate>
                    <asp:Label ID="Label3" runat="server" Text="$"></asp:Label>
                    <asp:Label ID="lblTotalPrice" runat="server" Text=""></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Week" HeaderText="Week" SortExpression="Week" />
            <asp:TemplateField HeaderText="Delete">
                <ItemTemplate>
                    <asp:ImageButton ID="ImgDelete" runat="server" ImageUrl="~/images/appfalse.gif" OnClientClick="return ConfirmOnDelete();"
                        CommandArgument='<%#Container.DataItemIndex+1 %>' CausesValidation="false" CommandName="Delete1" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Edit" ShowHeader="False" Visible="false">
                <ItemTemplate>
                    <asp:LinkButton ID="lbTotalProduct" runat="server" Text="Edit" CommandName="Edit1" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Update" ShowHeader="False">
                <ItemTemplate>
                    <asp:ImageButton ID="Edit" runat="server" ImageUrl="~/images/apptrue.gif" CausesValidation="false" ToolTip='<%#Eval("Id")%>'
                        CommandArgument='<%#Container.DataItemIndex %>' CommandName="Update1" />
                </ItemTemplate>
            </asp:TemplateField>

            <%--<asp:TemplateField HeaderText="Edit" ShowHeader="False">
                                <ItemTemplate>                                    
                                    <asp:ImageButton ID="Edit" runat="server" ImageUrl="~/images/apptrue.gif" CausesValidation="false"
                                        CommandArgument='<%#Eval(" ProductId")%>' CommandName="Edit1" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>                                    
                                    <asp:ImageButton ID="ImgDelete" runat="server" ImageUrl="~/images/appfalse.gif" OnClientClick="return ConfirmOnDelete();"
                                        CommandArgument='<%#Eval(" ProductId")%>' CausesValidation="false" CommandName="Delete1" />
                                </ItemTemplate>
                            </asp:TemplateField>--%>
        </Columns>
    </asp:GridView>
    <asp:Label ID="Label2" runat="server"></asp:Label>
    <asp:Label ID="Label1" runat="server" Visible="false" Text="You have to pay $"></asp:Label><asp:Label ID="lblTotal" Visible="false" runat="server"></asp:Label>
    <br />
    <br />
    <asp:Button ID="btnPayment" Text="Proceed to Payment" runat="server" OnClick="btnPayment_Click" />
    <asp:Button ID="btnAddMoreItems" runat="server" Text="Make Additional Orders" OnClick="btnAddMoreItems_Click" />
    <asp:Button ID="btnPayInStore" runat="server" Text="Pay Products In Store" OnClick="btnPayInStore_Click" />

    <asp:Button ID="btnInvoice" runat="server" Visible="false" OnClick="btnInvoice_Click" Text="Invoice" />

</asp:Content>

