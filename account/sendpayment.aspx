<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sendpayment.aspx.cs" Inherits="account_sendpayment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form action="https://www.sandbox.paypal.com/cgi-bin/webscr" method="post" id="form1"
        name="form1">
        <input type="hidden" name="cmd" value="_xclick">
        <input type="hidden" name="business" value="sales@rdollc.com"><!--Paypal or sandbox Merchant account -->
        <input type="hidden" name="item_name" value="Root Cellar Subscription Payment">
        <input type="hidden" name="item_number" value="1">
        <input type="hidden" name="amount" value="34.99">
        <input type="hidden" name="return" value="http://www.rdollc.com/rootcellar/thankyou.aspx "><!--this page will be your redirection page -->
        <input type="hidden" name="cancel_return" value="http://www.rdollc.com/rootcellar/ ">
        <input type="hidden" name="currency_code" value="USD">
        <input type="hidden" name="notify_url" value="http://www.rdollc.com/rootcellar/paypal.aspx "><!--this should be your domain web page where you going to receive all your transaction variables -->
    </form>

    <script language="javascript">
        document.form1.submit();
    </script>
</body>
</html>
