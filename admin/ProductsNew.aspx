<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProductsNew.aspx.cs" Inherits="admin_ProductsNew" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" lang="javascript">
        function Save() {
            alert("Product Added Successfully");
            window.location = "ProductsNew.aspx";
        }
    </script>
    <script type="text/javascript" lang="javascript">
        function Update() {
            alert("Product Updated Successfully");
            window.location = "ProductsNew.aspx";
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
    <script type="text/javascript" lang="javascript">
        //function Error() {
        //    alert("You have no permission to access this. Please Contact to admin");
        //    window.location = "../Admin/Default.aspx";
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
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
    <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Custom" ValidChars=" "
        TargetControlID="txtProducts" />--%>

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
    Select Week:<br />
    <%--<asp:DropDownList ID="ddlWeek" runat="server">
    </asp:DropDownList>--%>
    <telerik:RadComboBox ID="ddlWeek" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
        Width="500px" >
    </telerik:RadComboBox>


   <%-- <asp:RequiredFieldValidator InitialValue=" - Select a Week - " ID="Req_ID" Display="Dynamic"
        runat="server" ControlToValidate="ddlWeek"
        ErrorMessage="Please Select Week"></asp:RequiredFieldValidator>--%>


    <br />
    Quantity:<br />
    <asp:TextBox ID="txtQuantity" runat="server" MaxLength="8" Width="500px" class="text-input"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtQuantity" ErrorMessage="Please Insert Quantity"></asp:RequiredFieldValidator>
    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers" TargetControlID="txtQuantity" />

    <br />
    <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <h3>Select Stores</h3>

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Repeater ID="rpStoreInfo" runat="server">
        <HeaderTemplate>
            <table id="table1" class="auto-style1">
                <tr>
                    <td hidden="hidden">Store ID</td>
                    <td>Store Name</td>
                    <td>Start Day</td>
                    <td>Start Time</td>
                    <td>End Day</td>

                    <td>End Time</td>
                    <%-- <td>Available Day</td>--%>
                    <%-- <td>Home Delivery</td>--%>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td hidden="hidden">
                    <asp:Label ID="lblStoreID" runat="server" Text='<%#Eval("StoreID")%>'></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="cbStoreName" Text='<%#Eval("Store")%>' runat="server" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlStartDay" runat="server">
                        <asp:ListItem Value="1">Sunday</asp:ListItem>
                        <asp:ListItem Value="2">Monday</asp:ListItem>
                        <asp:ListItem Value="3">Tuesday</asp:ListItem>
                        <asp:ListItem Value="4">Wednesday</asp:ListItem>
                        <asp:ListItem Value="5">Thursday</asp:ListItem>
                        <asp:ListItem Value="6">Friday</asp:ListItem>
                        <asp:ListItem Value="7">Saturday</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <telerik:RadTimePicker ID="rtpStartTime" runat="server">
                    </telerik:RadTimePicker>
                </td>
                <td>
                    <asp:DropDownList ID="ddlEndDay" runat="server">
                        <asp:ListItem Value="1">Sunday</asp:ListItem>
                        <asp:ListItem Value="2">Monday</asp:ListItem>
                        <asp:ListItem Value="3">Tuesday</asp:ListItem>
                        <asp:ListItem Value="4">Wednesday</asp:ListItem>
                        <asp:ListItem Value="5">Thursday</asp:ListItem>
                        <asp:ListItem Value="6">Friday</asp:ListItem>
                        <asp:ListItem Value="7">Saturday</asp:ListItem>
                    </asp:DropDownList>
                </td>

                <td>
                    <telerik:RadTimePicker ID="rtpEndTime" runat="server">
                    </telerik:RadTimePicker>
                </td>
                <%-- <td>
                <asp:CheckBox ID="cbThu" runat="server" Text="Thursday" />
                <br />
                <asp:CheckBox ID="cbFri" runat="server" Text="Friday" />
            </td>--%>
                <%--<td>
                <asp:CheckBox ID="cbHomeDelivery" runat="server" />
            </td>--%>
            </tr>

        </ItemTemplate>
        <FooterTemplate></table></FooterTemplate>
    </asp:Repeater>

    <asp:Repeater ID="rpStoreInfoEdit" runat="server" Visible="false" OnItemDataBound="rpStoreInfoEdit_ItemDataBound">
        <HeaderTemplate>
            <table id="table1" class="auto-style1">
                <tr>
                    <td hidden="hidden">Store ID</td>
                    <td>Store Name</td>
                    <td>Start Day</td>
                    <td>Start Time</td>
                    <td>End Day</td>

                    <td>End Time</td>
                    <%-- <td>Available Day</td>
            <td>Home Delivery</td>--%>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td hidden="hidden">
                    <asp:Label ID="lblStoreID" runat="server" Text='<%#Eval("StoreID")%>'></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="cbStoreName" Text='<%#Eval("Store")%>' runat="server" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlStartDay" runat="server">
                        <asp:ListItem Value="1">Sunday</asp:ListItem>
                        <asp:ListItem Value="2">Monday</asp:ListItem>
                        <asp:ListItem Value="3">Tuesday</asp:ListItem>
                        <asp:ListItem Value="4">Wednesday</asp:ListItem>
                        <asp:ListItem Value="5">Thursday</asp:ListItem>
                        <asp:ListItem Value="6">Friday</asp:ListItem>
                        <asp:ListItem Value="7">Saturday</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <telerik:RadTimePicker ID="rtpStartTime" runat="server">
                    </telerik:RadTimePicker>
                </td>
                <td>
                    <asp:DropDownList ID="ddlEndDay" runat="server">
                        <asp:ListItem Value="1">Sunday</asp:ListItem>
                        <asp:ListItem Value="2">Monday</asp:ListItem>
                        <asp:ListItem Value="3">Tuesday</asp:ListItem>
                        <asp:ListItem Value="4">Wednesday</asp:ListItem>
                        <asp:ListItem Value="5">Thursday</asp:ListItem>
                        <asp:ListItem Value="6">Friday</asp:ListItem>
                        <asp:ListItem Value="7">Saturday</asp:ListItem>
                    </asp:DropDownList>
                </td>

                <td>
                    <telerik:RadTimePicker ID="rtpEndTime" runat="server">
                    </telerik:RadTimePicker>
                </td>
                <%--  <td>
                <asp:CheckBox ID="cbThu" runat="server" Text="Thursday" Checked='<%#Convert.ToInt32(Eval("ADThu"))==1?true:false%>'/>
                <br />
                <asp:CheckBox ID="cbFri" runat="server" Text="Friday" Checked='<%#Convert.ToInt32(Eval("ADFri"))==1?true:false%>'/>
            </td>
            <td>
                <asp:CheckBox ID="cbHomeDelivery" runat="server" Checked='<%#Convert.ToInt32(Eval("HomeDelivery"))==1?true:false%>'/>
            </td>--%>
            </tr>

        </ItemTemplate>
        <FooterTemplate></table></FooterTemplate>
    </asp:Repeater>
    <br />
    <h3>Payment Type: </h3>
    <asp:CheckBox ID="cbPayOnline" Text="Pay Online" runat="server" Checked='<%#Convert.ToInt32(Eval("PayOnline"))==1?true:false%>' />
    <asp:CheckBox ID="cbInShop" Text="In Store" runat="server" Checked='<%#Convert.ToInt32(Eval("PayInShop"))==1?true:false%>' />

    <br />

    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />


    <br />
    <br />
    <h3>Product Details</h3>
       <%--  class="table table-striped table-hover table-bordered" data-ride="datatables"--%>
   <%-- <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>--%>
   <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
             <asp:GridView ID="gvProduct" runat="server" AllowPaging="false" AllowSorting="True"
   
        AutoGenerateColumns="False" CellPadding="4" DataKeyNames="ProductID" ShowHeaderWhenEmpty="True" EmptyDataText="No Records to Display."
        ForeColor="#333333" OnRowCommand="gvProduct_RowCommand"  OnSorting="gvProduct_Sorting" AllowCustomPaging="True">
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
            <asp:BoundField DataField="ProductPrice" HeaderText="Product Price" SortExpression="ProductPrice" />
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
      <%--  </ContentTemplate>
    </asp:UpdatePanel>--%>
   
</asp:Content>

