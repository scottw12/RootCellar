<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--<script src="http://code.jquery.com/jquery-1.6.4.js" type="text/javascript"></script>--%>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            // Get Products
          GetProducts();
            
        });
        function GetProducts() {
            
            urlToHandler = 'https://www.salesvu.com/townvu/api/index.php';
            jsonData = '{"api_key":"6991d5c4211be3c66e69913b721250ac","action":"product","store_id":"34798","category":"C2"}';
            $.ajax({
                url: urlToHandler,
                crossDomain: true,
                data: {request: jsonData, callback: "handleJSON1"},

                dataType: 'jsonp',
                type: 'POST',
                contentType: 'application/json',
                success: function (response) {},
                error: function (msg) {}
            });
            
            urlToHandler = 'https://www.salesvu.com/townvu/api/index.php';
            jsonData = '{"api_key":"6991d5c4211be3c66e69913b721250ac","action":"product","store_id":"34800","category":"C2"}';
            $.ajax({
                url: urlToHandler,
                crossDomain: true,
                data: { request: jsonData, callback: "handleJSON2" },

                dataType: 'jsonp',
                type: 'POST',
                contentType: 'application/json',
                success: function (response) { },
                error: function (msg) { }
            });
        }
        function handleJSON1(info) {
            document.getElementById('Label1').innerHTML += "Downtown Columbia<br />";
            jQuery.each(info.data, function (i, val) {
                document.getElementById('Label1').innerHTML += (val.product_id + " " + val.product_name + " " + val.description + " " + val.unit_id + " " + val.selling_price) + "<br />"
            });
            }
        function handleJSON2(info) {
            document.getElementById('Label1').innerHTML += "<br />Jefferson City<br />";
            jQuery.each(info.data, function (i, val) {
                document.getElementById('Label1').innerHTML += (val.product_id + " " + val.product_name + " " + val.description + " " + val.unit_id + " " + val.selling_price) + "<br />"
            });
        }
        //{"status":"ok","data":[{"category_id":"2","category_name":"Subscriptions"}]}
    </script>
   

</head>
<body>
    <form id="form1" runat="server">
        <asp:Label runat="server" ID="Label1"></asp:Label><br />
        <br />
        <br />
        <asp:Label runat="server" ID="Label2"></asp:Label><br />
        <hr />
        <input type="text" value="" id="Bounty" name="Bounty" />
        <input type="text" value="" id="Barnyard" name="Barnyard" />
        <input type="text" value="" id="Ploughman" name="Ploughman" />
        <div id="post"></div>
    </form>
</body>
</html>
