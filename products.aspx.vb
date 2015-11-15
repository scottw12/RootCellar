
Partial Class products
    Inherits System.Web.UI.Page

    Protected Sub Barnyard_Click(sender As Object, e As EventArgs) Handles Barnyard.Click
        Response.Redirect("subscribe?B=Barnyard")
    End Sub

    Protected Sub Bounty_Click(sender As Object, e As EventArgs) Handles Bounty.Click
        Response.Redirect("subscribe?B=Bounty")
    End Sub

    Protected Sub Ploughman_Click(sender As Object, e As EventArgs) Handles Ploughman.Click
        Response.Redirect("subscribe?B=Ploughman")
    End Sub
End Class
