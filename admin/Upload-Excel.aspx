<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Upload-Excel.aspx.vb" Inherits="admin_Upload_Excel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="SM1"></asp:ScriptManager>
    <h1>Upload new search</h1>
    <asp:FileUpload ID="File1" runat="server" /><br />
    <asp:Literal ID="Literal3" runat="server"></asp:Literal><br />
    <asp:Button ID="Button1" runat="server" Text="Upload" />
    <br />
    <br />
    <asp:UpdatePanel runat="server" ID="UPanel1" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Literal runat="server" ID="Literal0"></asp:Literal>
            <asp:GridView ID="Gridview1" runat="server" OnRowDataBound="Gridview1_RowDataBound"></asp:GridView>
            <asp:Literal ID="Literal2" runat="server"></asp:Literal>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="Button2" runat="server" Text="Import" />
            SubID's <asp:Literal ID="Literal5" runat="server"></asp:Literal>
    <br /><asp:Button ID="Button3" runat="server" Text="Email" />
</asp:Content>

