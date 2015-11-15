<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Summary.aspx.cs" EnableEventValidation="false" Inherits="admin_Summary" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .GridViewClass {
            font-weight: 700;
            color: #000000;
            background-color:white;
        }
    </style>
    <script type="text/javascript" lang="javascript">
        //function Error() {
        //    alert("You have no permission to access this. Please Contact to admin");
        //    window.location = "../Admin/Default.aspx";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <asp:Literal runat="server" ID="Literal1"></asp:Literal>
    <div class="threediv">
        <div>
            <asp:LinkButton runat="server" Text="Download" ID="DownloadButton" OnClick="DownloadButton_Click"></asp:LinkButton>
        </div>
        <div>
            <asp:DropDownList runat="server" ID="WeekList" AutoPostBack="True" OnSelectedIndexChanged="WeekList_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div>
            
        </div>
    </div>
    <br />
    <div class="center">
    <telerik:RadProgressManager ID="RadProgressManager1" runat="server" />
        <telerik:RadProgressArea ID="RadProgressArea1" runat="server" />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" HeaderStyle-Font-Bold="true" HeaderStyle-ForeColor="black" CssClass="GridViewClass" AllowSorting="True" DataSourceID="SqlDataSource1" HeaderStyle-BackColor="white" OnRowDataBound="GridView1_RowDataBound">
        <Columns>
            <asp:BoundField DataField="Store" HeaderText="" SortExpression="Store" ItemStyle-HorizontalAlign="Left"  />
            <asp:BoundField DataField="Bounty" HeaderText="Bounty" SortExpression="Bounty" ItemStyle-BackColor="#acb237" />
            <asp:BoundField DataField="Barnyard" HeaderText="Barnyard" SortExpression="Barnyard" ItemStyle-BackColor="#d5642a"/>
            <asp:BoundField DataField="Ploughman" HeaderText="Ploughman" SortExpression="Ploughman" ItemStyle-BackColor="#8f4836"/>
            <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" ItemStyle-BackColor="LightGreen" />
        </Columns>
    </asp:GridView>
    
</div>
    
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT [SId], [Store], [Bounty], [Barnyard], [Ploughman], [Total] FROM [Summary] order by [SId]"></asp:SqlDataSource>
</asp:Content>

