<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="excel.aspx.cs" Inherits="admin_excel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h3>
                    <asp:Literal runat="server" ID="Literal1"></asp:Literal></h3>
    <asp:FileUpload ID="FileUpload1" runat="server" />
<asp:Button ID="btnUpload" runat="server" Text="Upload"
            OnClick="btnUpload_Click" />
<br />
<asp:Label ID="Label1" runat="server" Text="Has Header ?" />
<asp:RadioButtonList ID="rbHDR" runat="server">
    <asp:ListItem Text = "Yes" Value = "Yes" Selected = "True" >
    </asp:ListItem>
    <asp:ListItem Text = "No" Value = "No"></asp:ListItem>
</asp:RadioButtonList>
<asp:GridView ID="GridView1" runat="server"
OnPageIndexChanging = "PageIndexChanging" AllowPaging = "false">
</asp:GridView>
</asp:Content>

