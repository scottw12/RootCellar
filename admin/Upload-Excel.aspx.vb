Imports System.Data
Imports Microsoft.VisualBasic.FileIO
Imports System.Data.SqlClient
Imports System.IO
Imports System.Data.OleDb
Imports System.Net.Mail

Partial Class admin_Upload_Excel
    Inherits System.Web.UI.Page
    Dim Query As String = ""
    Dim i As Integer = 0
    Dim i2 As String = 0
    Private conn As SqlConnection = Nothing
    Private ConnectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ToString
    Private cmd As SqlCommand = Nothing
    Dim password As String = ""
    Dim Username As String = ""
    Dim useremail As String = ""

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Literal3.Text = ""
        Dim FilePath As String = ""
        Try
            If File1.HasFile Then
                Dim FileName As String = Path.GetFileName(File1.PostedFile.FileName)
                Dim Extension As String = Path.GetExtension(File1.PostedFile.FileName)
                FilePath = Server.MapPath("/admin/files/" + FileName)
                File1.SaveAs(FilePath)
                Import_To_Grid(FilePath, Extension, "Yes")
            End If
        Catch ex As Exception
            Literal0.Text = ex.Message + "<br />" + ex.StackTrace + "<br />" + FilePath
        End Try
    End Sub


    Private Sub Import_To_Grid(ByVal FilePath As String, ByVal Extension As String, ByVal isHDR As String)
        Dim conStr As String = ""
        Select Case Extension
            Case ".xls"
                'Excel 97-03
                conStr = ConfigurationManager.ConnectionStrings("Excel03ConString").ConnectionString()
                Exit Select
            Case ".xlsx"
                'Excel 07
                conStr = ConfigurationManager.ConnectionStrings("Excel07ConString").ConnectionString()
                Exit Select
        End Select
        conStr = String.Format(conStr, FilePath, isHDR)
        Dim connExcel As New OleDbConnection(conStr)
        Dim cmdExcel As New OleDbCommand()
        Dim oda As New OleDbDataAdapter()
        Dim dt As New DataTable()
        cmdExcel.Connection = connExcel
        'Get the name of First Sheet
        connExcel.Open()
        Dim dtExcelSchema As DataTable
        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
        Dim SheetName As String = dtExcelSchema.Rows(0)("TABLE_NAME").ToString()
        connExcel.Close()
        'Read Data from First Sheet
        connExcel.Open()
        cmdExcel.CommandText = "SELECT * From [" & SheetName & "]"
        oda.SelectCommand = cmdExcel
        oda.Fill(dt)
        connExcel.Close()
        'Bind Data to GridView
        Gridview1.DataSource = dt
        Gridview1.DataBind()
    End Sub

    Protected Sub Gridview1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles Gridview1.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                i += 1
                Dim row As GridViewRow = e.Row
                'Dim pattern As String = "(^\w*)\s(\w*)"
                'Dim replacement As String = "$1', '$2" & vbCrLf
                'Dim rgx As New Regex(pattern, RegexOptions.Singleline)
                'row.Cells(2).Text = rgx.Replace(row.Cells(2).Text, replacement)
                'row.Cells(3).Text = "'" + row.Cells(3).Text + "'"
                If row.Cells(6).Text = ("1") Then
                    row.Cells(6).Text = "True"
                ElseIf Not row.Cells(6).Text = "True" Then
                    row.Cells(6).Text = "False"
                End If
                If row.Cells(7).Text = ("1") Then
                    row.Cells(7).Text = "True"
                ElseIf Not row.Cells(7).Text = "True" Then
                    row.Cells(7).Text = "False"
                End If
                If row.Cells(8).Text = ("1") Then
                    row.Cells(8).Text = "True"
                ElseIf Not row.Cells(8).Text = "True" Then
                    row.Cells(8).Text = "False"
                End If
                row.Cells(9).Text = "'" + row.Cells(9).Text + "'"
            End If
        Catch ex As Exception
            Literal2.Text += ex.Message + "<br />" + ex.StackTrace + "<br />"
        End Try

    End Sub
    Public Shared Function UserNameExists(yourName As String) As Boolean
        Return Membership.GetUser(yourName) IsNot Nothing
    End Function
    Protected Sub CreateUser()
        Try
            For Each row As GridViewRow In Gridview1.Rows
                i += 1
                Dim FName As String = row.Cells(1).Text.Replace("'", "*1*").Replace("""", "*2*").Replace(" ", "").Replace("  ", "")
                Dim LName As String = row.Cells(0).Text.Replace("'", "*1*").Replace("""", "*2*").Replace(" ", "").Replace("  ", "")
                Username = FName.Trim + "." + LName.Trim
                Dim i4 As Integer = 0
                Dim n As Integer = 1
                Do While i4 = 0
                    If UserNameExists(Username) = True Then
                        Username = Username + n.ToString
                        n += 1
                    Else
                        i4 += 1
                    End If
                Loop
                Dim Uemail As String = row.Cells(11).Text.Replace("'", "").Replace("""", "").Replace(" ", "").Replace("  ", "")
                Dim SecretQ As String = "Please contact our store to request a reset password"
                Dim SecretA As String = "ergwergkqejfqeoufwqeofiheowfqpkoadmvnwo"
                password = row.Cells(1).Text.Trim + "TempPsw" + (TimeOfDay.Minute + 1).ToString
                password.Trim()
                Dim createStatus As MembershipCreateStatus
                Dim newUser As MembershipUser = _
                Membership.CreateUser(Username, password, _
                Uemail, SecretQ, _
                SecretA, True, _
                createStatus)
                Select Case createStatus
                    Case MembershipCreateStatus.Success
                        Try
                            Dim query As String = "INSERT INTO subscribers (FirstName1, LastName1, Email1, phone1, FirstName2, LastName2, Email2, phone2, Address, City, State, Zip, Allergies, vacUsed, BountyNL, BarnyardNL, PloughmanNL, Enrolled, Referred, Notes, pickupday, store, bounty, barnyard, ploughman, username, active) VALUES (@FirstName1, @LastName1, @Email1, @phone1, @FirstName2, @LastName2, @Email2, @phone2, @Address, @City, @State, @Zip, @Allergies, @vacUsed, @BountyNL, @BarnyardNL, @PloughmanNL,  @Enrolled, @Referred, @Notes, @pickupday, @store, @bounty, @barnyard, @ploughman, @Username, @active) "
                            Using conn As New SqlConnection(ConnectionString)
                                Using comm As New SqlCommand()
                                    With comm
                                        .Connection = conn
                                        .CommandType = CommandType.Text
                                        .CommandText = query
                                        comm.Parameters.Add("@FirstName1", SqlDbType.VarChar).Value = row.Cells(1).Text
                                        .Parameters.Add("@LastName1", SqlDbType.VarChar).Value = row.Cells(0).Text
                                        .Parameters.Add("@Email1", SqlDbType.VarChar).Value = row.Cells(11).Text
                                        .Parameters.Add("@phone1", SqlDbType.VarChar).Value = row.Cells(10).Text
                                        .Parameters.Add("@FirstName2", SqlDbType.VarChar).Value = ""
                                        .Parameters.Add("@LastName2", SqlDbType.VarChar).Value = ""
                                        .Parameters.Add("@Email2", SqlDbType.VarChar).Value = ""
                                        .Parameters.Add("@phone2", SqlDbType.VarChar).Value = ""
                                        .Parameters.Add("@Address", SqlDbType.VarChar).Value = row.Cells(12).Text
                                        .Parameters.Add("@City", SqlDbType.VarChar).Value = row.Cells(13).Text
                                        .Parameters.Add("@State", SqlDbType.VarChar).Value = row.Cells(14).Text
                                        .Parameters.Add("@Zip", SqlDbType.VarChar).Value = row.Cells(15).Text
                                        .Parameters.Add("@Allergies", SqlDbType.VarChar).Value = row.Cells(16).Text
                                        .Parameters.Add("@vacUsed", SqlDbType.Int).Value = 0
                                        .Parameters.Add("@BountyNL", SqlDbType.Bit).Value = False
                                        .Parameters.Add("@BarnyardNL", SqlDbType.Bit).Value = False
                                        .Parameters.Add("@PloughmanNL", SqlDbType.Bit).Value = False
                                        .Parameters.Add("@Enrolled", SqlDbType.SmallDateTime).Value = (DateTime.Parse(row.Cells(3).Text.Replace("''", ""))).ToString.Replace(" 12:00:00 AM", "")
                                        .Parameters.Add("@Referred", SqlDbType.VarChar).Value = ""
                                        .Parameters.Add("@Notes", SqlDbType.Text).Value = ""
                                        If row.Cells(9).Text = "''T''" Then
                                            .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Thursday"
                                            .Parameters.Add("@store", SqlDbType.Text).Value = "Downtown Columbia"
                                        ElseIf row.Cells(9).Text = "''F''" Then
                                            .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Friday"
                                            .Parameters.Add("@store", SqlDbType.Text).Value = "Downtown Columbia"
                                        ElseIf row.Cells(9).Text = "''TJC''" Then
                                            .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Thursday"
                                            .Parameters.Add("@store", SqlDbType.Text).Value = "Jefferson City"
                                        ElseIf row.Cells(9).Text = "''DHSS''" Then
                                            .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Thursday"
                                            .Parameters.Add("@store", SqlDbType.Text).Value = "DHSS (Employee Only)"
                                        End If
                                        .Parameters.Add("@username", SqlDbType.Text).Value = Username
                                        .Parameters.Add("@barnyard", SqlDbType.Bit).Value = row.Cells(7).Text
                                        .Parameters.Add("@bounty", SqlDbType.Bit).Value = row.Cells(6).Text
                                        .Parameters.Add("@ploughman", SqlDbType.Bit).Value = row.Cells(8).Text
                                        .Parameters.Add("@active", SqlDbType.Bit).Value = True
                                    End With
                                    conn.Open()
                                    comm.ExecuteNonQuery()
                                    Dim SubId As Integer = 0
                                    Dim myDataReader As SqlDataReader
                                    Dim mySqlConnection As SqlConnection
                                    Dim mySqlCommand As SqlCommand
                                    mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
                                    mySqlCommand = New SqlCommand("SELECT SubID FROM subscribers Where FirstName1= '" + row.Cells(1).Text + "' and address='" + row.Cells(12).Text + "'", mySqlConnection)
                                    Try
                                        mySqlConnection.Open()
                                        myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                                        Do While (myDataReader.Read())
                                            SubId = myDataReader.GetInt32(0)
                                            Literal5.Text += SubId.ToString + ", "

                                            query = "INSERT INTO Weekly (SubId, bounty, barnyard, ploughman, PickupDay, Location, Vacation, PaidBounty, PaidBarnyard, PaidPloughman, Pickedup, Notes, Week) VALUES (@SubId, @bounty, @barnyard, @ploughman, @PickupDay, @store, 'False', @PaidBounty, @PaidBarnyard, @PaidPloughman, 'False', '', @Week) "
                                            Using conn2 As New SqlConnection(ConnectionString)
                                                Using comm2 As New SqlCommand()
                                                    With comm2
                                                        .Connection = conn2
                                                        .CommandType = CommandType.Text
                                                        .CommandText = query
                                                        comm2.Parameters.Add("@SubId", SqlDbType.Int).Value = SubId
                                                        .Parameters.Add("@barnyard", SqlDbType.Bit).Value = row.Cells(7).Text
                                                        .Parameters.Add("@bounty", SqlDbType.Bit).Value = row.Cells(6).Text
                                                        .Parameters.Add("@ploughman", SqlDbType.Bit).Value = row.Cells(8).Text
                                                        If row.Cells(9).Text = "''T''" Then
                                                            .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Thursday"
                                                            .Parameters.Add("@store", SqlDbType.Text).Value = "Downtown Columbia"
                                                        ElseIf row.Cells(9).Text = "''F''" Then
                                                            .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Friday"
                                                            .Parameters.Add("@store", SqlDbType.Text).Value = "Downtown Columbia"
                                                        ElseIf row.Cells(9).Text = "''TJC''" Then
                                                            .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Thursday"
                                                            .Parameters.Add("@store", SqlDbType.Text).Value = "Jefferson City"
                                                        ElseIf row.Cells(9).Text = "''DHSS''" Then
                                                            .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Thursday"
                                                            .Parameters.Add("@store", SqlDbType.Text).Value = "DHSS (Employee Only)"
                                                        End If
                                                        .Parameters.Add("@PaidBounty", SqlDbType.Bit).Value = row.Cells(6).Text
                                                        .Parameters.Add("@PaidBarnyard", SqlDbType.Bit).Value = row.Cells(7).Text
                                                        .Parameters.Add("@PaidPloughman", SqlDbType.Bit).Value = row.Cells(8).Text
                                                        .Parameters.Add("@Week", SqlDbType.SmallDateTime).Value = "1/1/1900"
                                                    End With
                                                    Try
                                                        conn2.Open()
                                                        comm2.ExecuteNonQuery()
                                                    Catch ex As SqlException
                                                        Literal2.Text += "Error Insert into weekly1. Username:" + Username + "<br />" + ex.Message + "<br />" + ex.StackTrace + "<br />"
                                                        Exit Sub
                                                    End Try
                                                End Using
                                            End Using
                                            Dim startDate As DateTime = DateTime.Now
                                            Dim endDate As DateTime = "12/31/2018"
                                            Dim diff As TimeSpan = endDate - startDate
                                            Dim days As Integer = diff.Days
                                            For iO As Integer = 0 To days
                                                Dim testDate = startDate.AddDays(iO)
                                                Select Case testDate.DayOfWeek
                                                    Case DayOfWeek.Thursday
                                                        If CheckExcluded(testDate.ToShortDateString) = False Then
                                                            query = "INSERT INTO Weekly (SubId, bounty, barnyard, ploughman, PickupDay, Location, Vacation, PaidBounty, PaidBarnyard, PaidPloughman, Pickedup, Notes, Week) VALUES (@SubId, @bounty, @barnyard, @ploughman, @PickupDay, @store, 'False', @PaidBounty, @PaidBarnyard, @PaidPloughman, 'False', '', @Week) "
                                                            Using conn2 As New SqlConnection(ConnectionString)
                                                                Using comm2 As New SqlCommand()
                                                                    With comm2
                                                                        .Connection = conn2
                                                                        .CommandType = CommandType.Text
                                                                        .CommandText = query
                                                                        comm2.Parameters.Add("@SubId", SqlDbType.Int).Value = SubId
                                                                        .Parameters.Add("@barnyard", SqlDbType.Bit).Value = row.Cells(7).Text
                                                                        .Parameters.Add("@bounty", SqlDbType.Bit).Value = row.Cells(6).Text
                                                                        .Parameters.Add("@ploughman", SqlDbType.Bit).Value = row.Cells(8).Text
                                                                        If row.Cells(9).Text = "''T''" Then
                                                                            .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Thursday"
                                                                            .Parameters.Add("@store", SqlDbType.Text).Value = "Downtown Columbia"
                                                                        ElseIf row.Cells(9).Text = "''F''" Then
                                                                            .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Friday"
                                                                            .Parameters.Add("@store", SqlDbType.Text).Value = "Downtown Columbia"
                                                                        ElseIf row.Cells(9).Text = "''TJC''" Then
                                                                            .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Thursday"
                                                                            .Parameters.Add("@store", SqlDbType.Text).Value = "Jefferson City"
                                                                        ElseIf row.Cells(9).Text = "''DHSS''" Then
                                                                            .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Thursday"
                                                                            .Parameters.Add("@store", SqlDbType.Text).Value = "DHSS (Employee Only)"
                                                                        End If
                                                                        .Parameters.Add("@PaidBounty", SqlDbType.Bit).Value = False
                                                                        .Parameters.Add("@PaidBarnyard", SqlDbType.Bit).Value = False
                                                                        .Parameters.Add("@PaidPloughman", SqlDbType.Bit).Value = False
                                                                        .Parameters.Add("@Week", SqlDbType.SmallDateTime).Value = testDate.ToShortDateString()
                                                                    End With
                                                                    Try
                                                                        conn2.Open()
                                                                        comm2.ExecuteNonQuery()
                                                                    Catch ex As SqlException
                                                                        Literal2.Text += "Error Insert into weekly2. Username:" + Username + "<br />" + ex.Message + "<br />" + ex.StackTrace + "<br />"
                                                                        Exit Sub
                                                                    End Try
                                                                End Using
                                                            End Using
                                                            i2 += 1

                                                        End If
                                                        Exit Select
                                                End Select
                                            Next
                                        Loop
                                    Catch ex As SqlException
                                        Literal2.Text += "Error Main. Username:" + Username + "<br />" + ex.Message + "<br />" + ex.StackTrace + "<br />"
                                        Exit Sub
                                    Finally
                                        If (mySqlConnection.State = ConnectionState.Open) Then
                                            mySqlConnection.Close()
                                        End If
                                    End Try
                                End Using
                            End Using
                            'SendEmail(row.Cells(11).Text, row.Cells(1).Text, Username, password)
                        Catch ex As SqlException
                            Literal2.Text += "Error Main2. Username:" + Username + "<br />" + ex.Message + "<br />" + ex.StackTrace + "<br />"
                            Exit Sub
                        End Try
                    Case Else
                        Literal2.Text += "Error creating user " + Username + "<br />"
                End Select
            Next
            Literal0.Text = i2.ToString + " of " + i.ToString + "records processed successfully."
        Catch ex As Exception
            Literal2.Text += "Error Main3. Username:" + Username + "<br />" + ex.Message + "<br />" + ex.StackTrace + "<br />"
            Exit Sub
        End Try
        UPanel1.Update()
    End Sub
    'Function SendEmail(email As String, Fname As String, user As String, psw As String) As String
    '    Try
    '        Dim oMail0 As MailMessage = New MailMessage()
    '        oMail0.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
    '        oMail0.To.Add(New MailAddress(email.Replace("'", "").Replace("""", "").Replace(" ", "")))
    '        oMail0.Subject = "Root Cellar Subscription "
    '        oMail0.Priority = MailPriority.High
    '        oMail0.IsBodyHtml = True
    '        oMail0.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
    '        oMail0.Body &= "<head><title></title></head>"
    '        oMail0.Body &= "<body>"
    '        oMail0.Body &= "Hello " + Fname.Replace("'", "").Replace("""", "").Replace(" ", "") + ",<br /><br />"
    '        oMail0.Body &= "Welcome to the Root Cellar!<br /> The Username and Password for our new online subscription management tool is below. Here you will be able to schedule vacation weeks, provide advance payment online and view your subscription information.<br /><br />You can login to <a href='http://www.rootcellarboxes.com/login'>http://www.rootcellarboxes.com/login</a> using:<br />"
    '        oMail0.Body &= "Username: " + user + "<br />"
    '        oMail0.Body &= "Passowrd: " + psw + "<br /><br />"
    '        oMail0.Body &= "We also wanted to highlight a few important details about the Subscription Programs:<br /><br />"
    '        oMail0.Body &= "We're excited to have you as our newest subscriber and look forward to getting to know you. "
    '        oMail0.Body &= "<ul><li>Subscription members receive 10% off all purchase in the store (regardless of the day). Make sure to mention to the Sales Associate you’re a subscriber to ensure you get your discount.</li>"
    '        oMail0.Body &= "<li>In an effort save a few trees our weekly newsletter is sent to your email Thursday mornings. If you do not receive the newsletter or would like additional email addresses added to the list please contact us at rootcellarmo@gmail.com</li>"
    '        oMail0.Body &= "<li>Switching box pick up day (example: Friday to Thursday) to make your life easier is allowed. We do ask that you use this flexibility rarely and let us know as soon as possible. Pick-up hours are 12:00PM – 7:00 PM Thursday and 10:00 AM to 7:00 PM Friday 4:30 PM to 6:00 PM Jefferson City.</li>"
    '        oMail0.Body &= "<li>The last opportunity to pick up a box for the week is Saturday at 1:00PM (unless advance arrangements have been made).</li>"
    '        oMail0.Body &= "<li>Significant effort goes into harvesting, preparing and packing your box each week. In addition the food items in any box not picked up is often wasted. To ensure that all members receive the best value possible and the program is sustainable, failure to pick up your box may result in the loss of deposit.</li>"
    '        oMail0.Body &= "<li>Many of you will travel for work or pleasure during the season. We allow two vacation weeks to provide flexibility for these events. We do ask that you provide us 7 Days notice of your absence. Those vacations can be scheduled online or with a sales associate. </li>"
    '        oMail0.Body &= "<li> In an effort to be as sustainable as possible we do reuse and recycle as much as possible. One of the items we reuse the most is the Box your items are packaged in each week. Please Return the box each week to help us cut down on waste. In addition you will need to return milk bottles each week to avoid additional deposits. We also except Weiler egg cartons, Nature Fresh Duck Egg cartons and pint glass Jars.</li>"
    '        oMail0.Body &= "<li>Because of the large volume of boxes assembled each week we are unable to substitute items except in cases of food allergies. We keep these allergies on file and pack a substitute item in advance. In addition we have the 'Trading Post' available that allows subscribers to easily exchange items they may not need. </li></ul>"
    '        oMail0.Body &= "Please feel free to give us a call (573) 443-5055 or reply to this email if you have any additional questions.<br /><br /> "
    '        oMail0.Body &= "Root Cellar Team"
    '        oMail0.Body &= "</body>"
    '        oMail0.Body &= "</html>"
    '        Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail0.Body, Nothing, "text/html")
    '        oMail0.AlternateViews.Add(htmlView2)
    '        Dim smtpmail2 As New SmtpClient("relay-hosting.secureserver.net")
    '        smtpmail2.EnableSsl = False
    '        smtpmail2.UseDefaultCredentials = True

    '        smtpmail2.Send(oMail0)
    '        oMail0 = Nothing
    '    Catch ex As Exception
    '        Literal2.Text += "Error sending email. Username:" + Username + "<br />" + ex.Message + "<br />" + ex.StackTrace + "<br />"
    '    End Try
    'End Function
    Function CheckExcluded(searchValue As String) As Boolean
        Try
            Dim Edt As New DataTable()
            Edt.Columns.Add("EDate")
            Dim SqlQuary As String = "SELECT edate FROM excluded where edate>='" + Date.Today + "' order by edate"
            Dim myDataReader As SqlDataReader
            Dim mySqlConnection As SqlConnection
            Dim mySqlCommand As SqlCommand
            mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
            Using mySqlConnection
                mySqlCommand = New SqlCommand(SqlQuary, mySqlConnection)
                mySqlConnection.Open()
                myDataReader = mySqlCommand.ExecuteReader()
                If myDataReader.HasRows Then
                    Do While myDataReader.Read()
                        Edt.Rows.Add(myDataReader.GetDateTime(0))
                    Loop
                End If
                myDataReader.Close()
            End Using
            For Each row As DataRow In Edt.Rows
                If row("EDate").ToString.Replace(" 12:00:00 AM", "") = searchValue Then Return True
            Next
            Return False
        Catch ex As Exception
            Return False
            Literal2.Text += ex.Message + "<br />" + ex.StackTrace + "<br />Query: " + Query
        End Try
    End Function

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        CreateUser()
      
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Membership.DeleteUser("Lisa.Eastman")
        '    Membership.DeleteUser("Gary.Baker")
    End Sub
    Protected Sub email()
        Try
            Dim dt As New DataTable()
            dt.Columns.Add("SubId")
            For iO As Integer = 4482 To 4574
                dt.Rows.Add(iO.ToString)
            Next
            Dim username As String = ""
            Dim Fname As String = ""
            Dim Lname As String = ""
            Dim email As String = ""
            Dim myDataReader As SqlDataReader
            For Each row As DataRow In dt.Rows
                username = ""
                Dim query As String = "SELECT username, Firstname1, Lastname1, email1 FROM subscribers Where SubId=@SubId"
                Using conn As New SqlConnection(ConnectionString)
                    Using comm As New SqlCommand()
                        With comm
                            .Connection = conn
                            .CommandType = CommandType.Text
                            .CommandText = query
                            comm.Parameters.Add("@SubID", SqlDbType.VarChar).Value = row(0)
                        End With
                        conn.Open()
                        myDataReader = comm.ExecuteReader(CommandBehavior.CloseConnection)
                        Do While (myDataReader.Read())
                            If Not myDataReader.IsDBNull(0) Then
                                username = myDataReader.GetString(0)
                                Fname = myDataReader.GetString(1)
                                Lname = myDataReader.GetString(2)
                                email = myDataReader.GetString(3)
                            End If
                        Loop

                    End Using
                End Using
                If Not username = "" Then
                    Dim myObject As MembershipUser = Membership.GetUser(username)
                    myObject.IsApproved = True
                    Dim newpassword = row(0) + "TempPsw" + (TimeOfDay.Minute + 1).ToString
                    newpassword.trim()
                    Dim generatedpassword As String = myObject.ResetPassword()
                    myObject.ChangePassword(generatedpassword, newpassword)
                    Try
                        Dim oMail0 As MailMessage = New MailMessage()
                        oMail0.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
                        oMail0.To.Add(New MailAddress(email.Replace("'", "").Replace("""", "").Replace(" ", "")))
                        oMail0.Subject = "Root Cellar Subscription "
                        oMail0.Priority = MailPriority.High
                        oMail0.IsBodyHtml = True
                        oMail0.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
                        oMail0.Body &= "<head><title></title></head>"
                        oMail0.Body &= "<body>"
                        oMail0.Body &= "Hello " + Fname.Replace("'", "").Replace("""", "").Replace(" ", "") + ",<br /><br />"
                        oMail0.Body &= "Welcome to the Root Cellar!<br /> The Username and Password for our new online subscription management tool is below. Here you will be able to schedule vacation weeks, provide advance payment online and view your subscription information.<br /><br />You can login to <a href='http://www.rootcellarboxes.com/login'>http://www.rootcellarboxes.com/login</a> using:<br />"
                        oMail0.Body &= "Username: " + username + "<br />"
                        oMail0.Body &= "Passowrd: " + newpassword + "<br /><br />"
                        oMail0.Body &= "We also wanted to highlight a few important details about the Subscription Programs:<br /><br />"
                        oMail0.Body &= "We're excited to have you as our newest subscriber and look forward to getting to know you. "
                        oMail0.Body &= "<ul><li>Subscription members receive 10% off all purchase in the store (regardless of the day). Make sure to mention to the Sales Associate you’re a subscriber to ensure you get your discount.</li>"
                        oMail0.Body &= "<li>In an effort save a few trees our weekly newsletter is sent to your email Thursday mornings. If you do not receive the newsletter or would like additional email addresses added to the list please contact us at rootcellarmo@gmail.com</li>"
                        oMail0.Body &= "<li>Switching box pick up day (example: Friday to Thursday) to make your life easier is allowed. We do ask that you use this flexibility rarely and let us know as soon as possible. Pick-up hours are 12:00PM – 7:00 PM Thursday and 10:00 AM to 7:00 PM Friday 4:30 PM to 6:00 PM Jefferson City.</li>"
                        oMail0.Body &= "<li>The last opportunity to pick up a box for the week is Saturday at 1:00PM (unless advance arrangements have been made).</li>"
                        oMail0.Body &= "<li>Significant effort goes into harvesting, preparing and packing your box each week. In addition the food items in any box not picked up is often wasted. To ensure that all members receive the best value possible and the program is sustainable, failure to pick up your box may result in the loss of deposit.</li>"
                        oMail0.Body &= "<li>Many of you will travel for work or pleasure during the season. We allow two vacation weeks to provide flexibility for these events. We do ask that you provide us 7 Days notice of your absence. Those vacations can be scheduled online or with a sales associate. </li>"
                        oMail0.Body &= "<li> In an effort to be as sustainable as possible we do reuse and recycle as much as possible. One of the items we reuse the most is the Box your items are packaged in each week. Please Return the box each week to help us cut down on waste. In addition you will need to return milk bottles each week to avoid additional deposits. We also except Weiler egg cartons, Nature Fresh Duck Egg cartons and pint glass Jars.</li>"
                        oMail0.Body &= "<li>Because of the large volume of boxes assembled each week we are unable to substitute items except in cases of food allergies. We keep these allergies on file and pack a substitute item in advance. In addition we have the 'Trading Post' available that allows subscribers to easily exchange items they may not need. </li></ul>"
                        oMail0.Body &= "Please feel free to give us a call (573) 443-5055 or reply to this email if you have any additional questions.<br /><br /> "
                        oMail0.Body &= "Root Cellar Team"
                        oMail0.Body &= "</body>"
                        oMail0.Body &= "</html>"
                        Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail0.Body, Nothing, "text/html")
                        oMail0.AlternateViews.Add(htmlView2)
                        Dim smtpmail2 As New SmtpClient("relay-hosting.secureserver.net")
                        smtpmail2.EnableSsl = False
                        smtpmail2.UseDefaultCredentials = True

                        smtpmail2.Send(oMail0)
                        oMail0 = Nothing

                    Catch ex As Exception
                        Literal2.Text += "Error sending email. Username:" + username + "<br />" + ex.Message + "<br />" + ex.StackTrace + "<br />"
                        Exit Sub
                    End Try
                End If
            Next row
        Catch ex As Exception
            Literal2.Text += "Error 2. Username:" + Username + "<br />" + ex.Message + "<br />" + ex.StackTrace + "<br />"
            Exit Sub
        End Try
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        email()
    End Sub
End Class
