Imports System.Net
Imports System.IO

Partial Class admin_UpdMC
    Inherits System.Web.UI.Page
    Dim i As Integer = 0
    Private Sub UpdMailChimp(email As String, Bounty As Boolean, Barnyard As Boolean, Ploughman As Boolean, fname As String, lname As String, PUday As String)
        Dim webAddr As String = ""
        Try
            If Bounty = True Then
                Try
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=0a27dd543a&email[email]=" + email + "&merge_vars[FNAME]=" + fname + "&merge_vars[LNAME]=" + lname + "&merge_vars[MMERGE3]=" + PUday + "&double_optin=false&send_welcome=false"
                    Dim FwebAddr As New Uri(webAddr)
                    Dim httpWebRequest = DirectCast(WebRequest.Create(FwebAddr), HttpWebRequest)
                    httpWebRequest.ContentType = "application/json"
                    Dim httpResponse = DirectCast(httpWebRequest.GetResponse(), HttpWebResponse)
                    Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                        Dim val = streamReader.ReadToEnd()
                    End Using
                Catch ex As Exception
                End Try
            ElseIf Bounty = False Then
                Try
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/unsubscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=0a27dd543a&email[email]=" + email + "&merge_vars[FNAME]=" + fname + "&merge_vars[LNAME]=" + lname + "&merge_vars[MMERGE3]=" + PUday + "&double_optin=false&send_welcome=false"
                    Dim FwebAddr As New Uri(webAddr)
                    Dim httpWebRequest = DirectCast(WebRequest.Create(FwebAddr), HttpWebRequest)
                    httpWebRequest.ContentType = "application/json"
                    Dim httpResponse = DirectCast(httpWebRequest.GetResponse(), HttpWebResponse)
                    Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                        Dim val = streamReader.ReadToEnd()
                    End Using
                Catch ex As Exception
                End Try
            End If
            If Barnyard = True Then
                Try
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=2335ec6f51&email[email]=" + email + "&merge_vars[FNAME]=" + fname + "&merge_vars[LNAME]=" + lname + "&merge_vars[MMERGE3]=" + PUday + "&double_optin=false&send_welcome=false"
                    Dim FwebAddr As New Uri(webAddr)
                    Dim httpWebRequest = DirectCast(WebRequest.Create(FwebAddr), HttpWebRequest)
                    httpWebRequest.ContentType = "application/json"
                    Dim httpResponse = DirectCast(httpWebRequest.GetResponse(), HttpWebResponse)
                    Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                        Dim val = streamReader.ReadToEnd()
                    End Using
                Catch ex As Exception
                End Try
            ElseIf Barnyard = False Then
                Try
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/unsubscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=2335ec6f51&email[email]=" + email + "&merge_vars[FNAME]=" + fname + "&merge_vars[LNAME]=" + lname + "&merge_vars[MMERGE3]=" + PUday + "&double_optin=false&send_welcome=false"
                    Dim FwebAddr As New Uri(webAddr)
                    Dim httpWebRequest = DirectCast(WebRequest.Create(FwebAddr), HttpWebRequest)
                    httpWebRequest.ContentType = "application/json"
                    Dim httpResponse = DirectCast(httpWebRequest.GetResponse(), HttpWebResponse)
                    Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                        Dim val = streamReader.ReadToEnd()
                    End Using
                Catch ex As Exception
                End Try
            End If
            If Ploughman = True Then
                Try
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=4801343502&email[email]=" + email + "&merge_vars[FNAME]=" + fname + "&merge_vars[LNAME]=" + lname + "&merge_vars[MMERGE3]=" + PUday + "&double_optin=false&send_welcome=false"
                    Dim FwebAddr As New Uri(webAddr)
                    Dim httpWebRequest = DirectCast(WebRequest.Create(FwebAddr), HttpWebRequest)
                    httpWebRequest.ContentType = "application/json"
                    Dim httpResponse = DirectCast(httpWebRequest.GetResponse(), HttpWebResponse)
                    Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                        Dim val = streamReader.ReadToEnd()
                    End Using
                Catch ex As Exception
                End Try
            ElseIf Ploughman = False Then
                Try
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/unsubscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=4801343502&email[email]=" + email + "&merge_vars[FNAME]=" + fname + "&merge_vars[LNAME]=" + lname + "&merge_vars[MMERGE3]=" + PUday + "&double_optin=false&send_welcome=false"
                    Dim FwebAddr As New Uri(webAddr)
                    Dim httpWebRequest = DirectCast(WebRequest.Create(FwebAddr), HttpWebRequest)
                    httpWebRequest.ContentType = "application/json"
                    Dim httpResponse = DirectCast(httpWebRequest.GetResponse(), HttpWebResponse)
                    Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                        Dim val = streamReader.ReadToEnd()
                    End Using
                Catch ex As Exception
                End Try
            End If
        Catch ex As Exception
            Literal0.Text += "<br /><br />" + ex.Message + "<br />" + ex.StackTrace + "<br />" + webAddr
        End Try

    End Sub

    Protected Sub gv1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gv1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ck1 As CheckBox = DirectCast(e.Row.Cells(4).Controls(0), CheckBox)
            Dim ck2 As CheckBox = DirectCast(e.Row.Cells(5).Controls(0), CheckBox)
            Dim ck3 As CheckBox = DirectCast(e.Row.Cells(6).Controls(0), CheckBox)
            UpdMailChimp(e.Row.Cells(2).Text, ck1.Checked, ck2.Checked, ck3.Checked, e.Row.Cells(0).Text, e.Row.Cells(1).Text, e.Row.Cells(3).Text)
            i += 1
            Literal0.Text += "<br />" + i.ToString + " completed."
        End If
    End Sub
    Protected Sub gv2_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gv2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ck1 As CheckBox = DirectCast(e.Row.Cells(4).Controls(0), CheckBox)
            Dim ck2 As CheckBox = DirectCast(e.Row.Cells(5).Controls(0), CheckBox)
            Dim ck3 As CheckBox = DirectCast(e.Row.Cells(6).Controls(0), CheckBox)
            UpdMailChimp(e.Row.Cells(2).Text, ck1.Checked, ck2.Checked, ck3.Checked, e.Row.Cells(0).Text, e.Row.Cells(1).Text, e.Row.Cells(3).Text)
            i += 1
            Literal0.Text += "<br />" + i.ToString + " completed."
        End If
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Literal0.Text = ""
    End Sub
End Class
