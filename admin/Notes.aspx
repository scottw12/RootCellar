<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Notes.aspx.cs" Inherits="admin_Notes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" lang="javascript">
        //function Error() {
        //    alert("You have no permission to access this. Please Contact to admin");
        //    window.location = "../Admin/Default.aspx";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Literal runat="server" ID="Literal1"></asp:Literal><br />
    Subscriber:
    <asp:DropDownList runat="server" ID="SubscriberList" AutoPostBack="True" OnSelectedIndexChanged="SubscriberList_SelectedIndexChanged"></asp:DropDownList>

    <asp:Panel runat="server" ID="NotesPanel">
        <br />
        Note Type:
        <asp:RadioButtonList runat="server" ID="NoteType" AutoPostBack="True" OnSelectedIndexChanged="NoteType_SelectedIndexChanged">
            <asp:ListItem Text="Weekly"></asp:ListItem>
            <asp:ListItem Text="Permanent Record"></asp:ListItem>
        </asp:RadioButtonList><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please select a Note Type" ForeColor="red" ControlToValidate="NoteType"></asp:RequiredFieldValidator>
        <asp:DropDownList runat="server" ID="WeekList" AutoPostBack="True" OnSelectedIndexChanged="WeekList_SelectedIndexChanged"></asp:DropDownList>
        <br />
        <asp:TextBox runat="server" ID="NoteBox" TextMode="MultiLine" Rows="10" Width="800px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter a Note" ForeColor="red" ControlToValidate="NoteBox"></asp:RequiredFieldValidator>
        <br />
        <asp:Button runat="server" ID="submit" Text="Submit" OnClick="submit_Click" />
        <br />
        <h2>Current Notes</h2>
        <asp:Literal runat="server" ID="CurrNotesLiteral"></asp:Literal>
    </asp:Panel>
    <br />
    <br />
    
</asp:Content>

