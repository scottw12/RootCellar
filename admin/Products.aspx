<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Products.aspx.cs" Inherits="admin_Products" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" lang="javascript">
        function Save() {
            alert("Product Added Successfully");
            window.location = "Products.aspx";
        }
    </script>
    <script type="text/javascript" lang="javascript">
        function Update() {
            alert("Product Updated Successfully");
            window.location = "Products.aspx";
        }
    </script>
    <script type="text/javascript" lang="javascript">
        function ConfirmOnDelete() {
            var res = confirm("Are you sure to delete?");
            if (res == true) {
                alert('Product Deleted Successfully');
                return true;
            }
            else
                return false;
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }

        .auto-style3 {
            width: 113px;
        }
        .auto-style8 {
            width: 270px;
        }
        .auto-style10 {
            width: 97px;
        }
        .auto-style12 {
            width: 175px;
        }
        .auto-style13 {
            width: 105px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <h1>Add Products</h1>
    <asp:Label ID="Label1" Text="Product Name:" runat="server"></asp:Label><br />
    <asp:TextBox ID="txtProducts" runat="server" class="text-input" MaxLength="50" Width="500px"></asp:TextBox>
    &nbsp;
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtProducts" ErrorMessage="Please Insert Product Name"></asp:RequiredFieldValidator>
    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Custom" ValidChars=" "
        TargetControlID="txtProducts" />

    <br />

    <asp:Label ID="Label2" Text="Product Description:" runat="server"></asp:Label><br />
    <asp:TextBox ID="txtPDescription" runat="server" class="text-input" MaxLength="250" Width="500px" TextMode="MultiLine" Height="61px"></asp:TextBox>

    &nbsp;
    <br />

    <asp:Label ID="Label3" Text="Select Product Image:" runat="server"></asp:Label><br />
    <asp:FileUpload ID="fuPImage" runat="server" />
            <asp:Button ID="btnUploadImage" runat="server" Text="Upload Image" OnClick="btnUploadImage_Click" CausesValidation="false" /><br />

    <asp:Label ID="Label4" Text="Product Price($):" runat="server"></asp:Label><br />
    <asp:TextBox ID="txtPPrice" runat="server" class="text-input" MaxLength="8" Width="500px"></asp:TextBox>
    &nbsp;
    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPPrice" ErrorMessage="Please Insert Price"></asp:RequiredFieldValidator>
    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers, Custom" ValidChars="."
        TargetControlID="txtPPrice" />
    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtPPrice" runat="server" ErrorMessage="Please Insert Valid Price" ValidationExpression="^\d+(?:[\.\,]\d+)?$"></asp:RegularExpressionValidator>
    <br />
    <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="cbDowntownColumbia" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="cbJeffersonCity" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="cbDHSS" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="cbMizzouNorth" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="cbQUARTERDECK" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="cbUniversityHospital" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="cbUMHeinkel" EventName="CheckedChanged" />
            
            <asp:AsyncPostBackTrigger ControlID="DCHomeDelivery" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="JCHomeDelivery" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="DHSSHomeDelivery" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="QHomeDelivery" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="UHHomeDelivery" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="UMHHomeDelivery" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="MNHomeDelivery" EventName="CheckedChanged" />
            
            <asp:AsyncPostBackTrigger ControlID="DCPaid" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="JCPaid" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="MNPaid" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="QPaid" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="DHSSPaid" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="UHPaid" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="UMHPaid" EventName="CheckedChanged" />   
            <%--<asp:AsyncPostBackTrigger ControlID="btnUploadImage" EventName="Click" /> --%>  
                     
        </Triggers>
        <ContentTemplate>
    <h3>Select Stores</h3>
    
            <table id="table1" class="auto-style1">
                <tr>
                    <td class="auto-style8">Store Name</td>
                    <td class="auto-style3">Day</td>
                    <td class="auto-style12">Home Delivery</td>
                    <td class="auto-style13">Paid</td>
                    <td>Charges</td>
                </tr>
                <tr>
                    <td class="auto-style8">

                        <asp:CheckBox ID="cbDowntownColumbia" runat="server" Text="Downtown Columbia" OnCheckedChanged="cbDowntownColumbia_CheckedChanged" AutoPostBack="True" />

                    </td>
                    <td class="auto-style3">
                        <asp:CheckBox ID="DCThu" runat="server" Enabled="False" Text="Thursday" />
                        <br />
                        <asp:CheckBox ID="DCFri" runat="server" Enabled="False" Text="Friday" />
                    </td>
                    <td class="auto-style12">
                        <asp:CheckBox ID="DCHomeDelivery" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="DCHomeDelivery_CheckedChanged" />
                        <br />
                        <asp:CheckBox ID="DCHomeDelivery0" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="DCHomeDelivery0_CheckedChanged" />
                    </td>
                    <td class="auto-style13">
                        <asp:CheckBox ID="DCPaid" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="DCPaid_CheckedChanged" />
                        <br />
                        <asp:CheckBox ID="DCPaid0" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="DCPaid0_CheckedChanged" />
                    </td>
                    <td>
                        <asp:TextBox ID="DCCharges" runat="server" Enabled="False"></asp:TextBox>
                        <br />
                        <asp:TextBox ID="DCCharges0" runat="server" Enabled="False"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style8">
                        <asp:CheckBox ID="cbJeffersonCity" runat="server" Text="Jefferson City" OnCheckedChanged="cbJeffersonCity_CheckedChanged" AutoPostBack="True" />
                    </td>
                    <td class="auto-style3">
                        <asp:CheckBox ID="JCThu" runat="server" Enabled="False" Text="Thursday" />
                        <br />
                        <asp:CheckBox ID="JSFri" runat="server" Enabled="False" Text="Friday" />
                    </td>
                    <td class="auto-style12">
                        <asp:CheckBox ID="JCHomeDelivery" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="JCHomeDelivery_CheckedChanged" />
                        <br />
                        <asp:CheckBox ID="JCHomeDelivery0" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="JCHomeDelivery0_CheckedChanged" />
                    </td>
                    <td class="auto-style13">
                        <asp:CheckBox ID="JCPaid" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="JCPaid_CheckedChanged" />
                        <br />
                        <asp:CheckBox ID="JCPaid0" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="JCPaid0_CheckedChanged" />
                    </td>
                    <td>
                        <asp:TextBox ID="JCCharges" runat="server" Enabled="False"></asp:TextBox>
                        <br />
                        <asp:TextBox ID="JCCharges0" runat="server" Enabled="False"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style8">
                        <asp:CheckBox ID="cbDHSS" runat="server" Text="DHSS" OnCheckedChanged="cbDHSS_CheckedChanged" AutoPostBack="True" />
                    </td>
                    <td class="auto-style3">
                        <asp:CheckBox ID="DHSSThu" runat="server" Enabled="False" Text="Thursday" />
                        <br />
                        <asp:CheckBox ID="DHSSFri" runat="server" Enabled="False" Text="Friday" />
                    </td>
                    <td class="auto-style12">
                        <asp:CheckBox ID="DHSSHomeDelivery" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="DHSSHomeDelivery_CheckedChanged" />
                        <br />
                        <asp:CheckBox ID="DHSSHomeDelivery0" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="DHSSHomeDelivery0_CheckedChanged" />
                    </td>
                    <td class="auto-style13">
                        <asp:CheckBox ID="DHSSPaid" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="DHSSPaid_CheckedChanged" />
                        <br />
                        <asp:CheckBox ID="DHSSPaid0" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="DHSSPaid0_CheckedChanged" />
                    </td>
                    <td>
                        <asp:TextBox ID="DHSSCharges" runat="server" Enabled="False"></asp:TextBox>
                        <br />
                        <asp:TextBox ID="DHSSCharges0" runat="server" Enabled="False"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style8">
                        <asp:CheckBox ID="cbMizzouNorth" runat="server" Text="Mizzou North" OnCheckedChanged="cbMizzouNorth_CheckedChanged" AutoPostBack="True" />
                    </td>
                    <td class="auto-style3">
                        <asp:CheckBox ID="MNThu" runat="server" Enabled="False" Text="Thursday" />
                        <br />
                        <asp:CheckBox ID="MNFri" runat="server" Enabled="False" Text="Friday" />
                    </td>
                    <td class="auto-style12">
                        <asp:CheckBox ID="MNHomeDelivery" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="MNHomeDelivery_CheckedChanged" />
                        <br />
                        <asp:CheckBox ID="MNHomeDelivery0" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="MNHomeDelivery0_CheckedChanged" />
                    </td>
                    <td class="auto-style13">
                        <asp:CheckBox ID="MNPaid" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="MNPaid_CheckedChanged" />
                        <br />
                        <asp:CheckBox ID="MNPaid0" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="MNPaid0_CheckedChanged" />
                    </td>
                    <td>
                        <asp:TextBox ID="MNCharges" runat="server" Enabled="False"></asp:TextBox>
                        <br />
                        <asp:TextBox ID="MNCharges0" runat="server" Enabled="False"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style8">
                        <asp:CheckBox ID="cbQUARTERDECK" runat="server" Text="QUARTERDECK" OnCheckedChanged="cbQUARTERDECK_CheckedChanged" AutoPostBack="True" />
                    </td>
                    <td class="auto-style3">
                        <asp:CheckBox ID="QThu" runat="server" Enabled="False" Text="Thursday" />
                        <br />
                        <asp:CheckBox ID="QFri" runat="server" Enabled="False" Text="Friday" />
                    </td>
                    <td class="auto-style12">
                        <asp:CheckBox ID="QHomeDelivery" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="QHomeDelivery_CheckedChanged" />
                        <br />
                        <asp:CheckBox ID="QHomeDelivery0" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="QHomeDelivery0_CheckedChanged" />
                    </td>
                    <td class="auto-style13">
                        <asp:CheckBox ID="QPaid" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="QPaid_CheckedChanged" />
                        <br />
                        <asp:CheckBox ID="QPaid0" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="QPaid0_CheckedChanged" />
                    </td>
                    <td>
                        <asp:TextBox ID="QCharges" runat="server" Enabled="False"></asp:TextBox>
                        <br />
                        <asp:TextBox ID="QCharges0" runat="server" Enabled="False"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style8">
                        <asp:CheckBox ID="cbUniversityHospital" runat="server" Text="University Hospital/School of Medicine" OnCheckedChanged="cbUniversityHospital_CheckedChanged" AutoPostBack="True" />
                    </td>
                    <td class="auto-style3">
                        <asp:CheckBox ID="UHThu" runat="server" Enabled="False" Text="Thursday" />
                        <br />
                        <asp:CheckBox ID="UHFri" runat="server" Enabled="False" Text="Friday" />
                    </td>
                    <td class="auto-style12">
                        <asp:CheckBox ID="UHHomeDelivery" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="UHHomeDelivery_CheckedChanged" />
                        <br />
                        <asp:CheckBox ID="UHHomeDelivery0" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="UHHomeDelivery0_CheckedChanged" />
                    </td>
                    <td class="auto-style13">
                        <asp:CheckBox ID="UHPaid" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="UHPaid_CheckedChanged" />
                        <br />
                        <asp:CheckBox ID="UHPaid0" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="UHPaid0_CheckedChanged" />
                    </td>
                    <td>
                        <asp:TextBox ID="UHCharges" runat="server" Enabled="False"></asp:TextBox>
                        <br />
                        <asp:TextBox ID="UHCharges0" runat="server" Enabled="False"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style8">
                        <asp:CheckBox ID="cbUMHeinkel" runat="server" Text="UM Heinkel" OnCheckedChanged="cbUMHeinkel_CheckedChanged" AutoPostBack="True" />
                    </td>
                    <td class="auto-style3">
                        <asp:CheckBox ID="UMHThu" runat="server" Enabled="False" Text="Thursday" />
                        <br />
                        <asp:CheckBox ID="UMHFri" runat="server" Enabled="False" Text="Friday" />
                    </td>
                    <td class="auto-style12">
                        <asp:CheckBox ID="UMHHomeDelivery" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="UMHHomeDelivery_CheckedChanged" />
                        <br />
                        <asp:CheckBox ID="UMHHomeDelivery0" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="UMHHomeDelivery0_CheckedChanged" />
                    </td>
                    <td class="auto-style13">
                        <asp:CheckBox ID="UMHPaid" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="UMHPaid_CheckedChanged" />
                        <br />
                        <asp:CheckBox ID="UMHPaid0" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="UMHPaid0_CheckedChanged" />
                    </td>
                    <td>
                        <asp:TextBox ID="UMHCharges" runat="server" Enabled="False"></asp:TextBox>
                        <br />
                        <asp:TextBox ID="UMHCharges0" runat="server" Enabled="False"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <h3>Payment Type: </h3>
    <asp:RadioButtonList ID="rblPayment" runat="server" RepeatDirection="Horizontal">
        <asp:ListItem>Pay Online</asp:ListItem>
        <asp:ListItem>In Shop</asp:ListItem>
    </asp:RadioButtonList>

    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />


    <br />
    <br />
    <h3>Product Details</h3>
    <asp:GridView ID="gvProduct" runat="server" AllowPaging="True" AllowSorting="false"
        class="table table-striped table-hover table-bordered" data-ride="datatables"
        AutoGenerateColumns="False" CellPadding="4" DataKeyNames="ProductID"
        GridLines="Both" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
        ForeColor="#333333" OnRowCommand="gvProduct_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="ProductID" ShowHeader="False" Visible="False" SortExpression="ProductID">
                <ItemTemplate>
                    <asp:Label ID="ProductID" runat="server" Text='<%#Eval("ProductID")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Sr.No
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ProductName" HeaderText="Product Name" SortExpression="ProductName" />
            <asp:BoundField DataField="ProductPrice" HeaderText="Product Price" SortExpression="Product Price" />
            <asp:TemplateField HeaderText="Edit" ShowHeader="False">
                <ItemTemplate>
                    <asp:ImageButton ID="Edit" runat="server" ImageUrl="~/images/apptrue.gif" CausesValidation="false"
                        CommandArgument='<%#Eval("ProductID")%>' CommandName="Edit1" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Delete">
                <ItemTemplate>
                    <asp:ImageButton ID="ImgDelete" runat="server" ImageUrl="~/images/appfalse.gif" OnClientClick="return ConfirmOnDelete();"
                        CommandArgument='<%#Eval("ProductID")%>' CausesValidation="false" CommandName="Delete1" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>

