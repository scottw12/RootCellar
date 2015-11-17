<%@ Application Language="VB" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.Net.Mail" %>

<script runat="server">

    Protected Sub Application_Start(sender As Object, e As EventArgs)

    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If Not Request.ServerVariables("REMOTE_HOST").ToString.Contains("66.249.") Then
                Dim ex As Exception = Server.GetLastError
                Dim oMail As MailMessage = New MailMessage()
                oMail.From = New MailAddress("website@rootcellarboxes.com")
                oMail.To.Add(New MailAddress("scottw@jkmcomm.com"))
                oMail.Subject = "Root Cellar Application Error"
                oMail.Priority = MailPriority.High
                oMail.IsBodyHtml = True
                oMail.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
                oMail.Body &= "<head><title></title></head>"
                oMail.Body &= "<body>"
                oMail.Body &= "URL: " + Request.Url.ToString + "<br />"
                oMail.Body &= "Referer: " + Request.ServerVariables("HTTP_REFERER").ToString + "<br />"
                oMail.Body &= "IP: " + Request.ServerVariables("REMOTE_HOST").ToString + "<br />"
                oMail.Body &= "Error Message: " + ex.ToString() + "<br />"
                If User.Identity.IsAuthenticated Then
                    oMail.Body &= "User: " + User.Identity.Name + "<br />"
                Else
                    oMail.Body &= "User: Not loged in"
                End If
                oMail.Body &= "Form Values: " + "<br />"
                For Each s As String In Request.Form.AllKeys
                    If s <> "__VIEWSTATE" Then
                        oMail.Body &= (s & ":") + Request.Form(s) + "<br />"
                    End If
                Next
                oMail.Body &= "Session Values: " + "<br />"
                For Each s As String In Session.Keys
                    oMail.Body &= (s & ":") + Session(s) + "<br />"
                Next
                oMail.Body &= "Error Stack: " + ex.StackTrace + "<br />"
                oMail.Body &= "</body>"
                oMail.Body &= "</html>"
                Dim htmlView As AlternateView = AlternateView.CreateAlternateViewFromString(oMail.Body, Nothing, "text/html")
                oMail.AlternateViews.Add(htmlView)
                Dim smtpmail2 As New SmtpClient("mail.rootcellarboxes.com")
                smtpmail2.EnableSsl = False
                smtpmail2.Credentials = New NetworkCredential("website@rootcellarboxes.com", "mZo4g4#3")
                smtpmail2.Send(oMail)
                oMail = Nothing
            End If
        Catch ex2 As Exception

        End Try

    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub

</script>