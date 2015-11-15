<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Vacation.aspx.cs" Inherits="customer_Vacation" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" lang="javascript">
        function Save() {
            alert("Vacation Added Successfully");
            window.location = "VacationList.aspx";
        }
    </script>
    <script type="text/javascript" lang="javascript">
        function Update() {
            alert("Vacation Updated Successfully");
            window.location = "VacationList.aspx";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <br />
        <br />
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:Label ID="Label1" Text="Vacation:" runat="server"></asp:Label><br />       
        <asp:TextBox ID="txtVacation" runat="server" class="text-input" MaxLength="50" Width="500px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtVacation" ErrorMessage="Please Enter Vacation"></asp:RequiredFieldValidator>
         <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Custom" ValidChars=" "
    TargetControlID="txtVacation" />
        <br />

        <asp:Label ID="Label2" runat="server" Text="Vacation Date:"></asp:Label><br />        
        <%--<asp:TextBox ID="txtVacationDate" runat="server" class="text-input" MaxLength="50" Width="500px"></asp:TextBox><br />--%>
        <telerik:RadDatePicker ID="RadDatePicker1" Width="500px" runat="server" DateInput-ReadOnly="true">
        </telerik:RadDatePicker>
        <asp:RequiredFieldValidator runat="server" ID="RFV1" ControlToValidate="RadDatePicker1" ErrorMessage="Please Select Date"></asp:RequiredFieldValidator>
        <br />
        <asp:Label ID="Label3" runat="server" Text="DeliveryBoy:"></asp:Label><br />
        
        <asp:TextBox ID="txtDeliveryBoy" runat="server" class="text-input" MaxLength="50" Width="500px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDeliveryBoy" ErrorMessage="Please Enter Name"></asp:RequiredFieldValidator>
        <br />
        <asp:Label ID="Label4" runat="server" Text="Address:"></asp:Label><br />
        
        <asp:DropDownList ID="ddlAddress" runat="server">
            <asp:ListItem>Downtown Columbia</asp:ListItem>
            <asp:ListItem>Jefferson City</asp:ListItem>
            <asp:ListItem>DHSS</asp:ListItem>
            <asp:ListItem>Mizzou North</asp:ListItem>
            <asp:ListItem>QUARTERDECK </asp:ListItem>
            <asp:ListItem>University Hospital/School of Medicine </asp:ListItem>
            <asp:ListItem>UM Heinkel </asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="Label5" runat="server" Text="Contact Number:"></asp:Label><br />
        
        <asp:TextBox ID="txtContactNumber" runat="server" class="text-input" MaxLength="50" Width="500px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtContactNumber" ErrorMessage="Please Enter Contact Number"></asp:RequiredFieldValidator>
         <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
    TargetControlID="txtContactNumber" />
        <br />
        <asp:Label ID="Label6" runat="server" Text="Email:"></asp:Label><br />
        
        <asp:TextBox ID="txtEmail" runat="server" class="text-input" MaxLength="50" Width="500px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtEmail" ErrorMessage="Please Enter Email Address"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Enter Valid Email Address" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>

        <br />

        <asp:Button ID="btnAddVacation" runat="server" Text="Save" OnClick="btnAddVacation_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"/>
    </div>
</asp:Content>

