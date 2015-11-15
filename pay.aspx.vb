
Partial Class pay
    Inherits System.Web.UI.Page
    Protected Sub Button1_Click(sender As Object, e As EventArgs)
        Session("totalShoppingAmt") = TextBox1.Text
        Session("orderID") = "SubID" + DateTime.Now.Ticks.ToString
        Response.Redirect("account/sendpayment.aspx")
    End Sub
End Class
