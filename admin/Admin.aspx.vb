Imports System.Data
Imports System.Net.Mail
Imports System.Data.SqlClient

Partial Class admin_Admin
    Inherits System.Web.UI.Page

    Private conn As SqlConnection = Nothing
    Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
    Private cmd As SqlCommand = Nothing
    Dim Agent As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim myDataReader As SqlDataReader
        'Dim mySqlConnection As SqlConnection
        'Dim mySqlCommand As SqlCommand
        'mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        'mySqlCommand = New SqlCommand("SELECT Role FROM userinfo Where Username= '" + Membership.GetUser().ToString + "'", mySqlConnection)
        'Try
        '    mySqlConnection.Open()
        '    myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        '    ' Always call Read before accessing data.
        '    Do While (myDataReader.Read())
        '        Dim role As String = myDataReader.GetString(0)
        '        If role = "Admin" Then
        '            AccordionCtrl.Visible = True
        '        Else
        '            Response.Redirect("default")
        '        End If
        '    Loop
        'Finally
        '    ' Close the connection when done with it.
        '    If (mySqlConnection.State = ConnectionState.Open) Then
        '        mySqlConnection.Close()
        '    End If
        'End Try
        'If Not Page.IsPostBack Then
        '    LoadSeasons()
        '    LoadExcluded()
        '    Loadactiveboxes()
        '    LoadMultiple()
        'End If
    End Sub
    Protected Sub AddUser()
        Dim FName As String = firstname.Text
        FName = FName.Replace(" ", "")
        Dim LName As String = lastname.Text
        LName = LName.Replace(" ", "")
        Dim Username As String = FName + "." + LName
        Dim SecretQ As String = "Contact General Manager to reset password"
        Dim SecretA As String = "ergwergkqejfqeoufwqeofiheowfqpkoadmvnwo"
        Dim createStatus As MembershipCreateStatus
        Dim newUser As MembershipUser = _
        Membership.CreateUser(Username, password.Text, _
        email.Text, SecretQ, _
        SecretA, True, _
        createStatus)
        Select Case createStatus
            Case MembershipCreateStatus.Success
                Label1.Text = "The user '" + Username + "' has been created!"
                ' Add Userinfo.
                Dim sql As String
                Dim AEmail As String = email.Text
                Dim ARole As String
                If Admin.Checked = True Then
                    ARole = "Admin"
                Else
                    ARole = "Employee"
                End If
                Dim myObject As MembershipUser = Membership.GetUser(Username)
                Dim uid As String = myObject.ProviderUserKey.ToString()
                conn = New SqlConnection(ConnectionString)
                conn.Open()
                sql = "INSERT INTO UserInfo (UserId, FirstName, LastName, Username, Email, Role, firstlast, isapproved) VALUES ('" + uid + "', '" + FName + "', '" + LName + "', '" + Username + "', '" + AEmail + "', '" + ARole + "', '" + LName + ", " + FName + "', 'true')"
                cmd = New SqlCommand(sql, conn)
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
                Label1.Text = "The user '" + Username + "' has been created!"
                firstname.Text = ""
                lastname.Text = ""
                password.Text = ""
                email.Text = ""
                Admin.Checked = False
                Standard.Checked = False
                Exit Select
            Case MembershipCreateStatus.DuplicateUserName
                Label1.Text = "There already exists a user with this username."
                Exit Select
            Case MembershipCreateStatus.DuplicateEmail
                Label1.Text = "There already exists a user with this email address."
                Exit Select
            Case MembershipCreateStatus.InvalidEmail
                Label1.Text = "There email address you provided in invalid."
                Exit Select
            Case MembershipCreateStatus.InvalidAnswer
                Label1.Text = "There security answer was invalid."
                Exit Select
            Case MembershipCreateStatus.InvalidPassword
                Label1.Text = "The password you provided is invalid. It must be seven characters long and have at least one non-alphanumeric character."
                Exit Select
            Case Else
                Label1.Text = "There was an unknown error; the user account was NOT created."
                Exit Select
        End Select
        GridView2.DataBind()
        GridView3.DataBind()
        GridView4.DataBind()
        GridView5.DataBind()

    End Sub
    Sub DaySelect(obj As Object, e As DayRenderEventArgs)
        If e.Day.IsWeekend Then
            e.Day.IsSelectable = False
        End If
        If e.Day.Date.ToString("dddd") = "Thursday" Then
            e.Day.IsSelectable = True
        Else
            e.Day.IsSelectable = False
        End If
        If e.Day.Date < Date.Today Then
            e.Day.IsSelectable = False
        End If
    End Sub
    Protected Sub Button6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button6.Click
        AddUser()
    End Sub
    Sub edit_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        CheckBox1.Checked = False
        CheckBox2.Checked = False
        Label2.Text = ""
        Dim uid, FName, AEmail, ARole As String
        Dim strQuery As String = ""
        Dim row As GridViewRow = GridView2.SelectedRow
        Dim AN As String = row.Cells(0).Text.ToString
        Try
            strQuery = "SELECT UserId, FirstName, LastName, Email, Role FROM Userinfo WHERE UserName =@AN"
            Dim cmd As SqlCommand = New SqlCommand(strQuery)
            cmd.Parameters.Add("@AN", SqlDbType.VarChar).Value = AN
            Dim dt As DataTable = GetData(cmd)
            uid = dt.Rows(0)("UserId").ToString()
            FName = dt.Rows(0)("FirstName").ToString()
            AEmail = dt.Rows(0)("Email").ToString()
            ARole = dt.Rows(0)("Role").ToString()
            Agent = AN
            Panel1.Visible = True
            TextBox1.Text = FName
            TextBox2.Text = dt.Rows(0)("LastName").ToString()
            Label21.Text = Agent
            TextBox4.Text = AEmail
            If ARole = "Employee" Then
                CheckBox1.Checked = True
            ElseIf ARole = "Admin" Then
                CheckBox2.Checked = True
            End If
            GridView2.DataBind()
            GridView3.DataBind()
            GridView4.DataBind()
            GridView5.DataBind()

        Catch ex As Exception
            Label2.Text = "An error has occured."
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("site@blaneywings.com")
            oMail1.To.Add(New MailAddress("scottw@jkmcomm.com"))
            oMail1.Subject = "Root Cellar - ERROR Users"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "URL: " + Request.Url.ToString + "<br />"
            oMail1.Body &= "Referer: " + Request.ServerVariables("HTTP_REFERER").ToString + "<br />"
            oMail1.Body &= "IP: " + Request.ServerVariables("REMOTE_HOST").ToString + "<br />"
            oMail1.Body &= "Error Message: " + ex.ToString() + "<br />"
            oMail1.Body &= "Form Values: " + "<br />"
            For Each s As String In Request.Form.AllKeys
                If s <> "__VIEWSTATE" Then
                    oMail1.Body &= (s & ":") + Request.Form(s) + "<br />"
                End If
            Next
            oMail1.Body &= "Error Stack: " + ex.StackTrace + "<br />"
            oMail1.Body &= "</body>"
            oMail1.Body &= "</html>"
            Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
            oMail1.AlternateViews.Add(htmlView2)
            Dim smtpmail2 As New SmtpClient("relay-hosting.secureserver.net")
            smtpmail2.EnableSsl = False
            smtpmail2.UseDefaultCredentials = True
            smtpmail2.Send(oMail1)
            oMail1 = Nothing
        End Try

    End Sub
    Public Function GetData(ByVal cmd As SqlCommand) As DataTable
        Dim dt As New DataTable
        Dim strConnString As String = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString()
        Dim con As New SqlConnection(strConnString)
        Dim sda As New SqlDataAdapter
        cmd.CommandType = CommandType.Text
        cmd.Connection = con
        Try
            con.Open()
            sda.SelectCommand = cmd
            sda.Fill(dt)
            Return dt
        Catch ex As Exception
            Response.Write(ex.Message)
            Return Nothing
        Finally
            con.Close()
            sda.Dispose()
            con.Dispose()
        End Try
    End Function
    Public Function GetData2(ByVal cmd2 As SqlCommand) As DataTable
        Dim dl As New DataTable
        Dim strConnString As String = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString()
        Dim con2 As New SqlConnection(strConnString)
        Dim sda2 As New SqlDataAdapter
        cmd2.CommandType = CommandType.Text
        cmd2.Connection = con2
        Try
            con2.Open()
            sda2.SelectCommand = cmd2
            sda2.Fill(dl)
            Return dl
        Catch ex As Exception
            Response.Write(ex.Message)
            Return Nothing
        Finally
            con2.Close()
            sda2.Dispose()
            con2.Dispose()
        End Try
    End Function
    Protected Sub Button7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button7.Click
        Try
            Dim myObject As MembershipUser = Membership.GetUser(Label21.Text)
            Dim uid As String = myObject.ProviderUserKey.ToString()
            Dim ARole As String = ""
            If CheckBox1.Checked = True Then
                ARole = "Employee"
            ElseIf CheckBox2.Checked = True Then
                ARole = "Admin"
            End If
            Dim query As String = "Update UserInfo set FirstName=@Firstname, LastName=@Lastname, Email=@email, Role=@role WHERE UserId='{" + uid + "}' update aspnet_Membership set IsApproved='True', Islockedout='false' WHERE UserId='{" + uid + "}'"
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        comm.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = TextBox1.Text
                        .Parameters.Add("@LastName", SqlDbType.VarChar).Value = TextBox2.Text
                        .Parameters.Add("@email", SqlDbType.VarChar).Value = TextBox4.Text
                        .Parameters.Add("@role", SqlDbType.VarChar).Value = ARole
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                End Using
            End Using
            If Not TextBox3.Text = "" Then
                myObject.IsApproved = True
                'myObject.IsLockedOut = False
                Dim newpassword = TextBox3.Text
                Dim generatedpassword As String = myObject.ResetPassword()
                myObject.ChangePassword(generatedpassword, newpassword)
            End If
            Label2.Text = "The user has been edited. "
            GridView2.DataBind()
            GridView3.DataBind()
            GridView4.DataBind()
            GridView5.DataBind()

        Catch ex As Exception
            Label2.Text = "An error has occured"
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("site@blaneywings.com")
            oMail1.To.Add(New MailAddress("scottw@jkmcomm.com"))
            oMail1.Subject = "Root Cellar - ERROR Users"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "URL: " + Request.Url.ToString + "<br />"
            oMail1.Body &= "Referer: " + Request.ServerVariables("HTTP_REFERER").ToString + "<br />"
            oMail1.Body &= "IP: " + Request.ServerVariables("REMOTE_HOST").ToString + "<br />"
            oMail1.Body &= "Error Message: " + ex.ToString() + "<br />"
            oMail1.Body &= "Form Values: " + "<br />"
            For Each s As String In Request.Form.AllKeys
                If s <> "__VIEWSTATE" Then
                    oMail1.Body &= (s & ":") + Request.Form(s) + "<br />"
                End If
            Next
            oMail1.Body &= "Error Stack: " + ex.StackTrace + "<br />"
            oMail1.Body &= "</body>"
            oMail1.Body &= "</html>"
            Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
            oMail1.AlternateViews.Add(htmlView2)
            Dim smtpmail2 As New SmtpClient("relay-hosting.secureserver.net")
            smtpmail2.EnableSsl = False
            smtpmail2.UseDefaultCredentials = True
            smtpmail2.Send(oMail1)
            oMail1 = Nothing
        End Try

    End Sub
    Sub delete_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim strQuery As String = ""
            Dim row As GridViewRow = GridView3.SelectedRow
            Membership.DeleteUser(row.Cells(0).Text.ToString)
            conn = New SqlConnection(ConnectionString)
            conn.Open()
            Dim sql As String = "DELETE FROM Userinfo WHERE Username='" + row.Cells(0).Text.ToString + "'"
            cmd = New SqlCommand(sql, conn)
            cmd.ExecuteNonQuery()
            cmd.Connection.Close()
            Label3.Text = row.Cells(0).Text.ToString + " has been deleted"
            GridView2.DataBind()
            GridView3.DataBind()
            GridView4.DataBind()
            GridView5.DataBind()
        Catch ex As Exception
            Label3.Text = "An error has occured"
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("site@blaneywings.com")
            oMail1.To.Add(New MailAddress("scottw@jkmcomm.com"))
            oMail1.Subject = "Root Cellar - ERROR Users"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "URL: " + Request.Url.ToString + "<br />"
            oMail1.Body &= "Referer: " + Request.ServerVariables("HTTP_REFERER").ToString + "<br />"
            oMail1.Body &= "IP: " + Request.ServerVariables("REMOTE_HOST").ToString + "<br />"
            oMail1.Body &= "Error Message: " + ex.ToString() + "<br />"
            oMail1.Body &= "Form Values: " + "<br />"
            For Each s As String In Request.Form.AllKeys
                If s <> "__VIEWSTATE" Then
                    oMail1.Body &= (s & ":") + Request.Form(s) + "<br />"
                End If
            Next
            oMail1.Body &= "Error Stack: " + ex.StackTrace + "<br />"
            oMail1.Body &= "</body>"
            oMail1.Body &= "</html>"
            Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
            oMail1.AlternateViews.Add(htmlView2)
            Dim smtpmail2 As New SmtpClient("relay-hosting.secureserver.net")
            smtpmail2.EnableSsl = False
            smtpmail2.UseDefaultCredentials = True
            smtpmail2.Send(oMail1)
            oMail1 = Nothing
        End Try
    End Sub
    Sub freeze_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Label4.Text = ""
        Label5.Text = ""
        Try
            Dim strQuery As String = ""
            Dim row As GridViewRow = GridView4.SelectedRow
            Dim ANumber As String = row.Cells(0).Text.ToString
            Dim myObject As MembershipUser = Membership.GetUser(ANumber)
            Dim uid As String = myObject.ProviderUserKey.ToString()
            conn = New SqlConnection(ConnectionString)
            conn.Open()
            Dim sql As String = "update aspnet_Membership set IsApproved='False' WHERE UserId='{" + uid + "}'"
            Dim sql2 As String = "update userinfo set isapproved='False' WHERE UserId='{" + uid + "}'"
            cmd = New SqlCommand(sql, conn)
            cmd.ExecuteNonQuery()
            cmd = New SqlCommand(sql2, conn)
            cmd.ExecuteNonQuery()
            cmd.Connection.Close()
            Label4.Text = row.Cells(0).Text.ToString + "'s account has been frozen"
            GridView2.DataBind()
            GridView3.DataBind()
            GridView4.DataBind()
            GridView5.DataBind()
        Catch ex As Exception
            Label4.Text = "An error has occured"
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("site@blaneywings.com")
            oMail1.To.Add(New MailAddress("scottw@jkmcomm.com"))
            oMail1.Subject = "Root Cellar - ERROR Users"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "URL: " + Request.Url.ToString + "<br />"
            oMail1.Body &= "Referer: " + Request.ServerVariables("HTTP_REFERER").ToString + "<br />"
            oMail1.Body &= "IP: " + Request.ServerVariables("REMOTE_HOST").ToString + "<br />"
            oMail1.Body &= "Error Message: " + ex.ToString() + "<br />"
            oMail1.Body &= "Form Values: " + "<br />"
            For Each s As String In Request.Form.AllKeys
                If s <> "__VIEWSTATE" Then
                    oMail1.Body &= (s & ":") + Request.Form(s) + "<br />"
                End If
            Next
            oMail1.Body &= "Error Stack: " + ex.StackTrace + "<br />"
            oMail1.Body &= "</body>"
            oMail1.Body &= "</html>"
            Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
            oMail1.AlternateViews.Add(htmlView2)
            Dim smtpmail2 As New SmtpClient("relay-hosting.secureserver.net")
            smtpmail2.EnableSsl = False
            smtpmail2.UseDefaultCredentials = True
            smtpmail2.Send(oMail1)
            oMail1 = Nothing
        End Try
    End Sub
    Sub unfreeze_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Label4.Text = ""
        Label5.Text = ""
        Try
            Dim strQuery As String = ""
            Dim row As GridViewRow = GridView5.SelectedRow
            Dim ANumber As String = row.Cells(0).Text.ToString
            Dim myObject As MembershipUser = Membership.GetUser(ANumber)
            Dim uid As String = myObject.ProviderUserKey.ToString()
            conn = New SqlConnection(ConnectionString)
            conn.Open()
            Dim sql As String = "update aspnet_Membership set IsApproved='true' WHERE UserId='{" + uid + "}'"
            Dim sql2 As String = "update userinfo set isapproved='true' WHERE UserId='{" + uid + "}'"
            cmd = New SqlCommand(sql, conn)
            cmd.ExecuteNonQuery()
            cmd = New SqlCommand(sql2, conn)
            cmd.ExecuteNonQuery()
            cmd.Connection.Close()
            Label4.Text = row.Cells(0).Text.ToString + "'s account is now active"
            GridView2.DataBind()
            GridView3.DataBind()
            GridView4.DataBind()
            GridView5.DataBind()
        Catch ex As Exception
            Label4.Text = "An error has occured"
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("site@blaneywings.com")
            oMail1.To.Add(New MailAddress("scottw@jkmcomm.com"))
            oMail1.Subject = "Root Cellar - ERROR Users"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "URL: " + Request.Url.ToString + "<br />"
            oMail1.Body &= "Referer: " + Request.ServerVariables("HTTP_REFERER").ToString + "<br />"
            oMail1.Body &= "IP: " + Request.ServerVariables("REMOTE_HOST").ToString + "<br />"
            oMail1.Body &= "Error Message: " + ex.ToString() + "<br />"
            oMail1.Body &= "Form Values: " + "<br />"
            For Each s As String In Request.Form.AllKeys
                If s <> "__VIEWSTATE" Then
                    oMail1.Body &= (s & ":") + Request.Form(s) + "<br />"
                End If
            Next
            oMail1.Body &= "Error Stack: " + ex.StackTrace + "<br />"
            oMail1.Body &= "</body>"
            oMail1.Body &= "</html>"
            Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
            oMail1.AlternateViews.Add(htmlView2)
            Dim smtpmail2 As New SmtpClient("relay-hosting.secureserver.net")
            smtpmail2.EnableSsl = False
            smtpmail2.UseDefaultCredentials = True
            smtpmail2.Send(oMail1)
            oMail1 = Nothing
        End Try
    End Sub
    Sub Storedelete_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim strQuery As String = ""
            Dim row As GridViewRow = GridView1.SelectedRow
            Dim query As String = "DELETE FROM stores WHERE store=@store1 DELETE from summary where store=@store1 DELETE from summary where store=@store2 DELETE from summary where store=@store3 DELETE from summary where store=@store4 DELETE from summary where store=@store5 DELETE from summary where store=@store6"
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        comm.Parameters.Add("@store1", SqlDbType.VarChar).Value = row.Cells(0).Text.ToString
                        comm.Parameters.Add("@store2", SqlDbType.VarChar).Value = row.Cells(0).Text.ToString + " Thursday PU"
                        comm.Parameters.Add("@store3", SqlDbType.VarChar).Value = row.Cells(0).Text.ToString + " Friday PU"
                        comm.Parameters.Add("@store4", SqlDbType.VarChar).Value = row.Cells(0).Text.ToString + " Saturday PU"
                        comm.Parameters.Add("@store5", SqlDbType.VarChar).Value = row.Cells(0).Text.ToString + " NPUs"
                        comm.Parameters.Add("@store6", SqlDbType.VarChar).Value = row.Cells(0).Text.ToString + " Vacation"
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                End Using
            End Using

            Label6.Text = row.Cells(0).Text.ToString + " has been deleted"
            GridView1.DataBind()
        Catch ex As Exception
            Label6.Text = "An error has occured"
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("site@blaneywings.com")
            oMail1.To.Add(New MailAddress("scottw@jkmcomm.com"))
            oMail1.Subject = "Root Cellar - ERROR Users"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "URL: " + Request.Url.ToString + "<br />"
            oMail1.Body &= "Referer: " + Request.ServerVariables("HTTP_REFERER").ToString + "<br />"
            oMail1.Body &= "IP: " + Request.ServerVariables("REMOTE_HOST").ToString + "<br />"
            oMail1.Body &= "Error Message: " + ex.ToString() + "<br />"
            oMail1.Body &= "Form Values: " + "<br />"
            For Each s As String In Request.Form.AllKeys
                If s <> "__VIEWSTATE" Then
                    oMail1.Body &= (s & ":") + Request.Form(s) + "<br />"
                End If
            Next
            oMail1.Body &= "Error Stack: " + ex.StackTrace + "<br />"
            oMail1.Body &= "</body>"
            oMail1.Body &= "</html>"
            Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
            oMail1.AlternateViews.Add(htmlView2)
            Dim smtpmail2 As New SmtpClient("relay-hosting.secureserver.net")
            smtpmail2.EnableSsl = False
            smtpmail2.UseDefaultCredentials = True
            smtpmail2.Send(oMail1)
            oMail1 = Nothing
        End Try
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Not NewStore.Text = "" Then
            Dim query As String = "Insert into stores (store) values (@store1) Insert into summary (store) values (@store1) Insert into summary (store) values (@store2) Insert into summary (store) values (@store3) Insert into summary (store) values (@store4) Insert into summary (store) values (@store5) Insert into summary (store) values (@store6)"
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        comm.Parameters.Add("@store1", SqlDbType.VarChar).Value = NewStore.Text
                        comm.Parameters.Add("@store2", SqlDbType.VarChar).Value = NewStore.Text + " Thursday PU"
                        comm.Parameters.Add("@store3", SqlDbType.VarChar).Value = NewStore.Text + " Friday PU"
                        comm.Parameters.Add("@store4", SqlDbType.VarChar).Value = NewStore.Text + " Saturday PU"
                        comm.Parameters.Add("@store5", SqlDbType.VarChar).Value = NewStore.Text + " NPUs"
                        comm.Parameters.Add("@store6", SqlDbType.VarChar).Value = NewStore.Text + " Vacation"
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                End Using
            End Using
            NewStore.Text = ""
            Label6.Text = NewStore.Text + " has been added"
            GridView1.DataBind()
        Else
            Label6.Text = "Please enter a store name!"
        End If
    End Sub
    Sub Pickupdaydelete_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim row As GridViewRow = GridView6.SelectedRow
            Dim query As String = "DELETE FROM pickupdays WHERE pickupday=@pickupday"
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        comm.Parameters.Add("@pickupday", SqlDbType.VarChar).Value = row.Cells(0).Text.ToString
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                End Using
            End Using
            Label7.Text = row.Cells(0).Text.ToString + " has been deleted"
            GridView6.DataBind()
        Catch ex As Exception
            Label7.Text = "An error has occured"
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("site@blaneywings.com")
            oMail1.To.Add(New MailAddress("scottw@jkmcomm.com"))
            oMail1.Subject = "Root Cellar - ERROR Users"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "URL: " + Request.Url.ToString + "<br />"
            oMail1.Body &= "Referer: " + Request.ServerVariables("HTTP_REFERER").ToString + "<br />"
            oMail1.Body &= "IP: " + Request.ServerVariables("REMOTE_HOST").ToString + "<br />"
            oMail1.Body &= "Error Message: " + ex.ToString() + "<br />"
            oMail1.Body &= "Form Values: " + "<br />"
            For Each s As String In Request.Form.AllKeys
                If s <> "__VIEWSTATE" Then
                    oMail1.Body &= (s & ":") + Request.Form(s) + "<br />"
                End If
            Next
            oMail1.Body &= "Error Stack: " + ex.StackTrace + "<br />"
            oMail1.Body &= "</body>"
            oMail1.Body &= "</html>"
            Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
            oMail1.AlternateViews.Add(htmlView2)
            Dim smtpmail2 As New SmtpClient("relay-hosting.secureserver.net")
            smtpmail2.EnableSsl = False
            smtpmail2.UseDefaultCredentials = True
            smtpmail2.Send(oMail1)
            oMail1 = Nothing
        End Try
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Not NewPickupDay.Text = "" Then
            Dim query As String = "Insert into pickupdays (pickupday) values (@pickupday)"
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        comm.Parameters.Add("@pickupday", SqlDbType.VarChar).Value = NewPickupDay.Text
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                End Using
            End Using
            Label7.Text = NewPickupDay.Text + " has been added"
            GridView6.DataBind()
        Else
            Label7.Text = "Please enter a Pickup Day!"
        End If
    End Sub
    Private Sub LoadSeasons()
        Dim dt As New DataTable()
        dt.Columns.Add("SID")
        dt.Columns.Add("name")
        dt.Columns.Add("currents")
        dt.Columns.Add("sstart")
        dt.Columns.Add("send")
        dt.Columns.Add("enroll")
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        mySqlCommand = New SqlCommand("SELECT sid, name, currents, sstart, send, enroll FROM seasons order by sid", mySqlConnection)
        Try
            mySqlConnection.Open()
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Do While (myDataReader.Read())
                If myDataReader.HasRows Then
                    dt.Rows.Add(myDataReader.GetInt32(0), myDataReader.GetString(1), myDataReader.GetBoolean(2), (myDataReader.GetDateTime(3).ToString.Replace(" 12:00:00 AM", "")), (myDataReader.GetDateTime(4).ToString.Replace(" 12:00:00 AM", "")), myDataReader.GetBoolean(5))
                End If
            Loop
        Finally
            ' Close the connection when done with it.
            If (mySqlConnection.State = ConnectionState.Open) Then
                mySqlConnection.Close()
            End If
        End Try
        GridView7.DataSource = dt
        GridView7.DataBind()
    End Sub
    Private Sub Loadactiveboxes()
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        mySqlCommand = New SqlCommand("SELECT bounty, barnyard, ploughman FROM activeboxes", mySqlConnection)
        Try
            mySqlConnection.Open()
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Do While (myDataReader.Read())
                If myDataReader.HasRows Then
                    If myDataReader.GetBoolean(0) = True Then
                        BountyActive.Checked = True
                    Else
                        BountyActive.Checked = False
                    End If
                    If myDataReader.GetBoolean(1) = True Then
                        BarnyardActive.Checked = True
                    Else
                        BarnyardActive.Checked = False
                    End If
                    If myDataReader.GetBoolean(2) = True Then
                        PloughmanActive.Checked = True
                    Else
                        PloughmanActive.Checked = False
                    End If
                End If
            Loop
        Finally
            If (mySqlConnection.State = ConnectionState.Open) Then
                mySqlConnection.Close()
            End If
        End Try
    End Sub
    Protected Sub GridViewSample_RowEditing(sender As Object, e As GridViewEditEventArgs)
        GridView7.EditIndex = e.NewEditIndex
        Literal1.Text = ""
        LoadSeasons()
    End Sub
    Protected Sub GridViewSample_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        Try
            Literal1.Text = ""
            Dim SIDLit As Literal = DirectCast(GridView7.Rows(e.RowIndex).FindControl("SIDLit"), Literal)
            Dim SNameBox As TextBox = DirectCast(GridView7.Rows(e.RowIndex).FindControl("SeasonName"), TextBox)
            Dim SNameLit As Literal = DirectCast(GridView7.Rows(e.RowIndex).FindControl("Snamelit"), Literal)
            Dim SCurrent As CheckBox = DirectCast(GridView7.Rows(e.RowIndex).FindControl("SeasonCurrent2"), CheckBox)
            Dim Senroll As CheckBox = DirectCast(GridView7.Rows(e.RowIndex).FindControl("Seasonenroll2"), CheckBox)
            Dim SStart As Calendar = DirectCast(GridView7.Rows(e.RowIndex).FindControl("Calendar1"), Calendar)
            Dim SLit As Literal = DirectCast(GridView7.Rows(e.RowIndex).FindControl("slit"), Literal)
            Dim SEnd As Calendar = DirectCast(GridView7.Rows(e.RowIndex).FindControl("Calendar2"), Calendar)
            Dim ELit As Literal = DirectCast(GridView7.Rows(e.RowIndex).FindControl("elit"), Literal)
            Dim query As String = "Update seasons set name=@name, CurrentS=@CurrentS, SStart=@Sstart, SEnd=@SEnd, enroll=@enroll where SID=@sidlit"
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        comm.Parameters.Add("@SIDlit", SqlDbType.VarChar).Value = SIDLit.Text
                        comm.Parameters.Add("@name", SqlDbType.VarChar).Value = SNameBox.Text
                        comm.Parameters.Add("@CurrentS", SqlDbType.Bit).Value = SCurrent.Checked.ToString
                        comm.Parameters.Add("@SStart", SqlDbType.Date).Value = SStart.SelectedDate.ToString
                        comm.Parameters.Add("@SEnd", SqlDbType.Date).Value = SEnd.SelectedDate.ToString
                        comm.Parameters.Add("@enroll", SqlDbType.Bit).Value = Senroll.Checked.ToString
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                End Using
            End Using
            GridView7.EditIndex = -1
            LoadSeasons()
            If SCurrent.Checked = True Then
                MoveSubscribers()
            Else
                Literal1.Text = "Season updated successfully! "
            End If
        Catch ex As Exception
            Literal1.Text = "We're sorry, there was an error"
        End Try


    End Sub
    Protected Sub GridViewSample_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        GridView7.EditIndex = -1
        Literal1.Text = ""
        LoadSeasons()
    End Sub
    Protected Sub GridViewSample_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim SIDLit As Literal = DirectCast(GridView7.Rows(e.RowIndex).FindControl("SIDLit"), Literal)
        Dim query As String = "Delete from seasons where SID=@SID"
        Using conn As New SqlConnection(ConnectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    comm.Parameters.Add("@SID", SqlDbType.Int).Value = SIDLit.Text
                End With
                conn.Open()
                comm.ExecuteNonQuery()
            End Using
        End Using
        GridView7.EditIndex = -1
        LoadSeasons()
        Literal1.Text = "Season deleted successfully!"
    End Sub
    Protected Sub GridViewSample_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName.Equals("Insert") Then
            Dim SNameBox As TextBox = DirectCast(GridView7.FooterRow.FindControl("NewSeasonName"), TextBox)
            Dim SCurrent As CheckBox = DirectCast(GridView7.FooterRow.FindControl("NewSeasonCurrent"), CheckBox)
            Dim SStart As Calendar = DirectCast(GridView7.FooterRow.FindControl("NewCalendar1"), Calendar)
            Dim SEnd As Calendar = DirectCast(GridView7.FooterRow.FindControl("NewCalendar2"), Calendar)
            Dim Senroll As CheckBox = DirectCast(GridView7.FooterRow.FindControl("NewSeasonenroll"), CheckBox)
            Dim query As String = "Insert Into seasons (name, CurrentS, SStart, SEnd, enroll) Values (@name, @CurrentS, @Sstart, @SEnd, @enroll)"
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        comm.Parameters.Add("@name", SqlDbType.VarChar).Value = SNameBox.Text
                        comm.Parameters.Add("@CurrentS", SqlDbType.Bit).Value = SCurrent.Checked.ToString
                        comm.Parameters.Add("@SStart", SqlDbType.Date).Value = SStart.SelectedDate.ToString
                        comm.Parameters.Add("@SEnd", SqlDbType.Date).Value = SEnd.SelectedDate.ToString
                        comm.Parameters.Add("@enroll", SqlDbType.Bit).Value = Senroll.Checked.ToString
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                End Using
            End Using
            GridView7.EditIndex = -1
            LoadSeasons()
            Literal1.Text = "Season added successfully!"
        End If
    End Sub

    Private Sub MoveSubscribers()
        Try
            Dim dt As New DataTable()
            dt.Columns.Add("SID")
            Dim myDataReader As SqlDataReader
            Dim mySqlConnection As SqlConnection
            Dim mySqlCommand As SqlCommand
            mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
            Dim query As String = ""
            Dim CurrActive As Integer = 0
            '----------------------------------------
            mySqlCommand = New SqlCommand("SELECT COUNT(SubID) AS subscribers FROM subscribers WHERE active='true'", mySqlConnection)
            mySqlConnection.Open()
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Do While (myDataReader.Read())
                If myDataReader.HasRows Then
                    CurrActive = myDataReader.GetInt32(0)
                End If
            Loop
            ' Close the connection when done with it.
            If (mySqlConnection.State = ConnectionState.Open) Then
                mySqlConnection.Close()
            End If
            '----------------------------------------
            query += "Update subscribers set active='false', vacused='0'"
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                End Using
            End Using
            '----------------------------------------
            mySqlCommand = New SqlCommand("SELECT distinct subid FROM weekly where week='1/1/1900' and ((bounty='true' and paidbounty='true') or (barnyard='true' and paidbarnyard='true') or (ploughman='true' and paidploughman='true'))", mySqlConnection)
                mySqlConnection.Open()
                myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                Do While (myDataReader.Read())
                    If myDataReader.HasRows Then
                        dt.Rows.Add(myDataReader.GetInt32(0))
                    End If
                Loop
                If (mySqlConnection.State = ConnectionState.Open) Then
                    mySqlConnection.Close()
                End If
            Dim added As Integer = 0
            For Each row As DataRow In dt.Rows
                query += "Update subscribers set active='true' where subid='" + row("SID") + "' "
                added += 1
            Next
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                End Using
            End Using
            Literal1.Text = "Season added/updated successfully!<br />" + (CurrActive - added).ToString + " subscribers were moved to inactive."
        Catch ex As Exception
            Literal1.Text = "We're sorry, there was an error"
        End Try

    End Sub
    Private Sub LoadExcluded()
        Dim dt As New DataTable()
        dt.Columns.Add("EID")
        dt.Columns.Add("EDate")
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        mySqlCommand = New SqlCommand("SELECT eid, edate FROM excluded order by edate", mySqlConnection)
        Try
            mySqlConnection.Open()
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Do While (myDataReader.Read())
                If myDataReader.HasRows Then
                    dt.Rows.Add(myDataReader.GetInt32(0), (myDataReader.GetDateTime(1).ToString.Replace(" 12:00:00 AM", "")))
                End If
            Loop
        Finally
            ' Close the connection when done with it.
            If (mySqlConnection.State = ConnectionState.Open) Then
                mySqlConnection.Close()
            End If
        End Try
        GridView8.DataSource = dt
        GridView8.DataBind()
    End Sub
    Protected Sub GridView8_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim EIDLit As Literal = DirectCast(GridView8.Rows(e.RowIndex).FindControl("EIDLit"), Literal)
        Dim query As String = "Delete from excluded where EID=@EID"
        Using conn As New SqlConnection(ConnectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    comm.Parameters.Add("@EID", SqlDbType.Int).Value = EIDLit.Text
                End With
                conn.Open()
                comm.ExecuteNonQuery()
            End Using
        End Using
        GridView8.EditIndex = -1
        LoadExcluded()
        Literal2.Text = "Excluded date deleted successfully!"
    End Sub
    Protected Sub GridView8_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName.Equals("Insert") Then
            Try
                Dim EDate As Calendar = DirectCast(GridView8.FooterRow.FindControl("NewEDate"), Calendar)
            Dim query As String = "Insert Into excluded (EDate) Values (@EDate)"
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        comm.Parameters.Add("@edate", SqlDbType.VarChar).Value = EDate.SelectedDate
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                End Using
            End Using
            Dim query2 As String = "delete from weekly where week=@EDate"
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query2
                        comm.Parameters.Add("@edate", SqlDbType.VarChar).Value = EDate.SelectedDate
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                End Using
            End Using
            GridView8.EditIndex = -1
            LoadExcluded()
                Literal2.Text = "Date Removed successfully!"
            Catch ex As Exception
                Literal1.Text = "Were sorry, there was an error"
            End Try

        End If
    End Sub

    Protected Sub ActiveBoxButton_Click(sender As Object, e As EventArgs) Handles ActiveBoxButton.Click
        Dim query As String = "update activeboxes set bounty=@bounty, barnyard=@barnyard, ploughman=@ploughman"
        Using conn As New SqlConnection(ConnectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    comm.Parameters.Add("@bounty", SqlDbType.Bit).Value = BountyActive.Checked.ToString
                    comm.Parameters.Add("@barnyard", SqlDbType.Bit).Value = BarnyardActive.Checked.ToString
                    comm.Parameters.Add("@ploughman", SqlDbType.Bit).Value = PloughmanActive.Checked.ToString
               End With
                conn.Open()
                comm.ExecuteNonQuery()
            End Using
        End Using
        LoadActiveBoxes()
        Literal3.Text = "<span style='color:red;'>Your active box's have been updated!</span>"
    End Sub
    Private Sub LoadMultiple()
        Dim dt As New DataTable()
        dt.Columns.Add("FirstName1")
        dt.Columns.Add("LastName1")
        dt.Columns.Add("SubID")
        dt.Columns.Add("Count")
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        mySqlCommand = New SqlCommand("Select subscribers.FirstName1, subscribers.LastName1, weekly.SubId, Count(Case When weekly.Week = '1/1/1900' Then 1 Else Null End) As Count From weekly Inner Join subscribers On subscribers.SubId = weekly.SubId where active='true' Group By subscribers.FirstName1, subscribers.LastName1, weekly.SubId Order By Count Desc", mySqlConnection)
        Try
            mySqlConnection.Open()
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Do While (myDataReader.Read())
                If myDataReader.HasRows Then
                    dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(1), ("http://rootcellarboxes.com/admin/Details?s=" + myDataReader.GetInt32(2).ToString), myDataReader.GetInt32(3).ToString)
                End If
            Loop
        Finally
            ' Close the connection when done with it.
            If (mySqlConnection.State = ConnectionState.Open) Then
                mySqlConnection.Close()
            End If
        End Try
        GridView10.DataSource = dt
        GridView10.DataBind()
    End Sub
    Protected Sub GridView10_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim Count As Literal = TryCast(e.Row.FindControl("Count"), Literal)
                For Each cell As TableCell In e.Row.Cells
                    If Not Convert.ToInt32(Count.Text) > 1 Then
                        e.Row.Visible = False
                    End If
                Next
            End If
        Catch ex As Exception
            Literal1.Text = ex.Message + ex.StackTrace
        End Try
    End Sub
End Class
