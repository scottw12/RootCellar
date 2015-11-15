Imports System.Data.SqlClient
Imports System.Data
Imports System.Net.Mail
Imports PerceptiveMCAPI
Imports PerceptiveMCAPI.Types
Imports PerceptiveMCAPI.Methods
Imports System.IO
Imports System.Net

Partial Class admin_New_Subscriber
    Inherits System.Web.UI.Page

    Private conn As SqlConnection = Nothing
    Private ConnectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ToString
    Private cmd As SqlCommand = Nothing
    Dim password As String = ""
    Dim Username As String = ""
    Dim useremail As String = ""


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            Dim myDataReader As SqlDataReader
            Dim mySqlConnection As SqlConnection
            Dim mySqlCommand As SqlCommand
            mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
            mySqlCommand = New SqlCommand("SELECT Role FROM userinfo Where Username= '" + Membership.GetUser().ToString + "'", mySqlConnection)
            Try
                mySqlConnection.Open()
                myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                Do While (myDataReader.Read())
                    Dim role As String = myDataReader.GetString(0)
                    If role = "Admin" Then
                    ElseIf role = "Employee" Then
                    Else
                        Response.Redirect("~/account/")
                    End If
                Loop
            Finally
                If (mySqlConnection.State = ConnectionState.Open) Then
                    mySqlConnection.Close()
                End If
            End Try
            FillInfo()
            FillDayInfo()
            FillStoreInfo()
            Price.Text = "$0.00"
        End If

    End Sub
    Protected Sub FillStoreInfo()
        Dim dt As New DataTable()
        dt.Columns.Add("Store")
        dt.Rows.Add("")
        'Create Rows in DataTable
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        Try
            Using mySqlConnection
                mySqlCommand = New SqlCommand("SELECT Store FROM Stores", mySqlConnection)
                mySqlConnection.Open()

                myDataReader = mySqlCommand.ExecuteReader()

                If myDataReader.HasRows Then
                    Do While myDataReader.Read()
                        dt.Rows.Add(myDataReader.GetString(0))
                    Loop
                Else
                    Console.WriteLine("No rows found.")
                End If
                myDataReader.Close()
            End Using
        Finally
        End Try
        Me.StoreList.DataSource = dt
        Me.StoreList.DataTextField = "store"
        Me.StoreList.DataValueField = "store"
        Me.StoreList.DataBind()
    End Sub
    Protected Sub FillDayInfo()
        Dim dt As New DataTable()
        dt.Columns.Add("PickupDay")
        dt.Rows.Add("")
        'Create Rows in DataTable
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        Try
            Using mySqlConnection
                mySqlCommand = New SqlCommand("SELECT PickupDay FROM PickupDays", mySqlConnection)
                mySqlConnection.Open()

                myDataReader = mySqlCommand.ExecuteReader()

                If myDataReader.HasRows Then
                    Do While myDataReader.Read()
                        dt.Rows.Add(myDataReader.GetString(0))
                    Loop
                Else
                    Console.WriteLine("No rows found.")
                End If
                myDataReader.Close()
            End Using
        Finally
        End Try
        Me.PickupDayList.DataSource = dt
        Me.PickupDayList.DataTextField = "PickupDay"
        Me.PickupDayList.DataValueField = "PickupDay"
        Me.PickupDayList.DataBind()
    End Sub
    Sub DaySelect(obj As Object, e As DayRenderEventArgs)
        If e.Day.IsWeekend Then
            e.Day.IsSelectable = False
        End If
        If e.Day.Date.ToString("dddd") = "Saturday" Then
            e.Day.IsSelectable = True
        Else
            e.Day.IsSelectable = False
        End If
        If e.Day.Date < Date.Today Then
            e.Day.IsSelectable = False
        End If
    End Sub
    Protected Sub StoreList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles StoreList.SelectedIndexChanged
        If StoreList.SelectedValue = "Jefferson City" Then
            If PickupDayList.SelectedValue = "Friday" Then
                PickupDayList.SelectedValue = "Thursday"
                PUDLiteral.Text = "Pickups are only available on Thursday's at the Jefferson City Location"
            Else
                PUDLiteral.Text = ""
            End If
        Else
            PUDLiteral.Text = ""
        End If
    End Sub
    Protected Sub PickupDayList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles PickupDayList.SelectedIndexChanged
        If PickupDayList.SelectedValue = "Friday" Then
            If StoreList.SelectedValue = "Jefferson City" Then
                PickupDayList.SelectedValue = "Thursday"
                PUDLiteral.Text = "Pickups are only available on Thursday's at the Jefferson City Location"
            Else
                PUDLiteral.Text = ""
            End If
        Else
            PUDLiteral.Text = ""
        End If
    End Sub
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Literal1.Text = ""
        If BountyBox.Checked = False And BarnyardBox.Checked = False And PloughmanBox.Checked = False Then
            Literal1.Text = "<span style='color:red;'>Please select at least one box </span>"
        Else
            If CreateUser() = True Then
                If DBInsert() = True Then
                    UpdMailChimp(email1.Text, BountyNL.Checked, BarnyardNL.Checked, PloughmanNL.Checked)
                    If Not email2.Text = "" Then
                        UpdMailChimp(email2.Text, BountyNL.Checked, BarnyardNL.Checked, PloughmanNL.Checked)
                    End If
                    SendEmail()
                    Panel1.Visible = False
                    Literal0.Text += "<span style='color:green;'><h2>" + firstname1.Text + " " + lastname1.Text + "'s account has been created!</h2></span>"
                Else
                    Literal1.Text = "<span style='color:red;'><h2> There was a problem creating an account for " + firstname1.Text + " " + lastname1.Text + ". Please check all of the info and try again.</h2></span>"
                End If
            End If
        End If
    End Sub
    Public Shared Function UserNameExists(yourName As String) As Boolean
        Return Membership.GetUser(yourName) IsNot Nothing
    End Function
    Function CreateUser() As Boolean
        Try
            Dim FName As String = firstname1.Text.Replace("'", "*1*").Replace("""", "*2*").Replace(" ", "").Replace("  ", "")
            Dim LName As String = lastname1.Text.Replace("'", "*1*").Replace("""", "*2*").Replace(" ", "").Replace("  ", "")
            Username = FName.Trim + "." + LName.Trim
            Dim i As Integer = 0
            Dim n As Integer = 1
            Do While i = 0
                If UserNameExists(Username) = True Then
                    Username = Username + n.ToString
                    n += 1
                Else
                    i += 1
                End If
            Loop
            Dim Uemail As String = email1.Text.Replace("'", "").Replace("""", "").Replace(" ", "").Replace("  ", "")
            Dim SecretQ As String = "Please use the Contact Us page to request a reset password"
            Dim SecretA As String = "ergwergkqejfqeoufwqeofiheowfqpkoadmvnwo"
            password = firstname1.Text.Trim + "TempPsw" + (TimeOfDay.Minute + 1).ToString
            password.Trim()
            Dim createStatus As MembershipCreateStatus
            Dim newUser As MembershipUser = _
            Membership.CreateUser(Username, password, _
            Uemail, SecretQ, _
            SecretA, True, _
            createStatus)
            Select Case createStatus
                Case MembershipCreateStatus.Success
                    Return True
                    Exit Select
                Case MembershipCreateStatus.DuplicateUserName
                    Literal1.Text = "<span style='color:red;'>There is already a user with this username.</span>"
                    Return False
                    Exit Select
                Case MembershipCreateStatus.DuplicateEmail
                    Literal1.Text = "<span style='color:red;'>There is already a user with this email address.</span>"
                    Return False
                    Exit Select
                Case MembershipCreateStatus.InvalidEmail
                    Literal1.Text = "<span style='color:red;'>There email address you provided in invalid.</span>"
                    Return False
                    Exit Select
                Case MembershipCreateStatus.InvalidAnswer
                    Literal1.Text = "<span style='color:red;'>There security answer was invalid.</span>"
                    Return False
                    Exit Select
                Case MembershipCreateStatus.InvalidPassword
                    Literal1.Text = "<span style='color:red;'>The password you provided is invalid. It must be seven characters long and have at least one non-alphanumeric character.</span>"
                    Return False
                    Exit Select
                Case Else
                    Literal1.Text = "<span style='color:red;'>There was an unknown error; the user account was NOT created.</span>"
                    Return False
                    Exit Select
            End Select
        Catch ex As Exception
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
            oMail1.To.Add(New MailAddress("dbccemtp@gmail.com"))
            oMail1.Subject = "Root Cellar Error"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "Error creating user: " + Username + "<br /><br />"
            oMail1.Body &= ex.Message + "<br /><br />" + ex.StackTrace
            oMail1.Body &= "</body>"
            oMail1.Body &= "</html>"
            Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
            oMail1.AlternateViews.Add(htmlView2)
            Dim smtpmail2 As New SmtpClient("relay-hosting.secureserver.net")
            smtpmail2.EnableSsl = False
            smtpmail2.UseDefaultCredentials = True
            smtpmail2.Send(oMail1)
            oMail1 = Nothing
            Literal1.Text = "We're sorry, there seems to have been an error"
            Return False
        End Try
    End Function
    Protected Sub SendEmail()
        Dim oMail0 As MailMessage = New MailMessage()
        oMail0.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
        oMail0.To.Add(New MailAddress(email1.Text.Replace("'", "").Replace("""", "").Replace(" ", "")))
        oMail0.Subject = "Root Cellar Subscription "
        oMail0.Priority = MailPriority.High
        oMail0.IsBodyHtml = True
        oMail0.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
        oMail0.Body &= "<head><title></title></head>"
        oMail0.Body &= "<body>"
        oMail0.Body &= "Hello " + firstname1.Text.Replace("'", "").Replace("""", "").Replace(" ", "") + ",<br /><br />"
        oMail0.Body &= "Welcome to the Root Cellar!<br /> The Username and Password for our new online subscription management tool is below. Here you will be able to schedule vacation weeks, provide advance payment online and view your subscription information.<br /><br />You can login to <a href='http://www.rootcellarboxes.com/login'>http://www.rootcellarboxes.com/login</a> using:<br />"
        oMail0.Body &= "Username: " + Username + "<br />"
        oMail0.Body &= "Passowrd: " + password + "<br /><br />"
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
        Try
            smtpmail2.Send(oMail0)
        Catch ex As Exception
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
            oMail1.To.Add(New MailAddress("dbccemtp@gmail.com"))
            oMail1.Subject = "Root Cellar Error"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "Error creating user: " + Username + "<br /><br />"
            oMail1.Body &= ex.Message + "<br /><br />" + ex.StackTrace
            oMail1.Body &= "</body>"
            oMail1.Body &= "</html>"
            Dim htmlView1 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
            oMail1.AlternateViews.Add(htmlView1)
            smtpmail2.EnableSsl = False
            smtpmail2.UseDefaultCredentials = True
            smtpmail2.Send(oMail1)
            oMail1 = Nothing
            Literal1.Text = "We're sorry, there seems to have been an error"
        End Try
        oMail0 = Nothing
    End Sub
    Function DBInsert() As Boolean
        Try
            Dim query As String = "INSERT INTO subscribers (FirstName1, LastName1, Email1, phone1, FirstName2, LastName2, Email2, phone2, Address, City, State, Zip, Allergies, vacUsed, BountyNL, BarnyardNL, PloughmanNL, Enrolled, Referred, Notes, pickupday, store, bounty, barnyard, ploughman, username, active) VALUES (@FirstName1, @LastName1, @Email1, @phone1, @FirstName2, @LastName2, @Email2, @phone2, @Address, @City, @State, @Zip, @Allergies, @vacUsed, @BountyNL, @BarnyardNL, @PloughmanNL,  @Enrolled, @Referred, @Notes, @pickupday, @store, @bounty, @barnyard, @ploughman, @Username, @active) "
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        comm.Parameters.Add("@FirstName1", SqlDbType.VarChar).Value = firstname1.Text
                        .Parameters.Add("@LastName1", SqlDbType.VarChar).Value = lastname1.Text
                        .Parameters.Add("@Email1", SqlDbType.VarChar).Value = email1.Text
                        .Parameters.Add("@phone1", SqlDbType.VarChar).Value = phone1.Text
                        .Parameters.Add("@FirstName2", SqlDbType.VarChar).Value = firstname2.Text
                        .Parameters.Add("@LastName2", SqlDbType.VarChar).Value = lastname2.Text
                        .Parameters.Add("@Email2", SqlDbType.VarChar).Value = email2.Text
                        .Parameters.Add("@phone2", SqlDbType.VarChar).Value = phone2.Text
                        .Parameters.Add("@Address", SqlDbType.VarChar).Value = address.Text
                        .Parameters.Add("@City", SqlDbType.VarChar).Value = city.Text
                        .Parameters.Add("@State", SqlDbType.VarChar).Value = state.Text
                        .Parameters.Add("@Zip", SqlDbType.VarChar).Value = zip.Text
                        .Parameters.Add("@Allergies", SqlDbType.VarChar).Value = allergies.Text
                        .Parameters.Add("@vacUsed", SqlDbType.Int).Value = 0
                        .Parameters.Add("@BountyNL", SqlDbType.Bit).Value = BountyNL.Checked
                        .Parameters.Add("@BarnyardNL", SqlDbType.Bit).Value = BarnyardNL.Checked
                        .Parameters.Add("@PloughmanNL", SqlDbType.Bit).Value = PloughmanNL.Checked
                        .Parameters.Add("@Enrolled", SqlDbType.SmallDateTime).Value = Date.Now.ToShortDateString
                        .Parameters.Add("@Referred", SqlDbType.VarChar).Value = ""
                        .Parameters.Add("@Notes", SqlDbType.Text).Value = ""
                        .Parameters.Add("@pickupday", SqlDbType.Text).Value = PickupDayList.SelectedValue
                        .Parameters.Add("@store", SqlDbType.Text).Value = StoreList.SelectedValue
                        .Parameters.Add("@username", SqlDbType.Text).Value = Username
                        .Parameters.Add("@barnyard", SqlDbType.Bit).Value = BarnyardBox.Checked
                        .Parameters.Add("@bounty", SqlDbType.Bit).Value = BountyBox.Checked
                        .Parameters.Add("@ploughman", SqlDbType.Bit).Value = PloughmanBox.Checked
                        Dim active As Boolean = True
                        If NextYear.Checked = True Then
                            active = False
                        End If
                        .Parameters.Add("@active", SqlDbType.Bit).Value = active

                    End With

                    conn.Open()
                    comm.ExecuteNonQuery()
                    Dim SubId As Integer = 0
                    Dim myDataReader As SqlDataReader
                    Dim mySqlConnection As SqlConnection
                    Dim mySqlCommand As SqlCommand
                    mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
                    mySqlCommand = New SqlCommand("SELECT SubID FROM subscribers Where FirstName1= '" + firstname1.Text + "' and address='" + address.Text + "'", mySqlConnection)
                    Try
                        mySqlConnection.Open()
                        myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                        Do While (myDataReader.Read())
                            SubId = myDataReader.GetInt32(0)
                            query = "INSERT INTO Weekly (SubId, bounty, barnyard, ploughman, PickupDay, Location, Vacation, PaidBounty, PaidBarnyard, PaidPloughman, Pickedup, Notes, Week) VALUES (@SubId, @bounty, @barnyard, @ploughman, @PickupDay, @Location, 'False', @PaidBounty, @PaidBarnyard, @PaidPloughman, 'False', '', @Week) "
                            Using conn2 As New SqlConnection(ConnectionString)
                                Using comm2 As New SqlCommand()
                                    With comm2
                                        .Connection = conn2
                                        .CommandType = CommandType.Text
                                        .CommandText = query
                                        comm2.Parameters.Add("@SubId", SqlDbType.Int).Value = SubId
                                        .Parameters.Add("@barnyard", SqlDbType.Bit).Value = BarnyardBox.Checked
                                        .Parameters.Add("@bounty", SqlDbType.Bit).Value = BountyBox.Checked
                                        .Parameters.Add("@ploughman", SqlDbType.Bit).Value = PloughmanBox.Checked
                                        .Parameters.Add("@PickupDay", SqlDbType.VarChar).Value = PickupDayList.SelectedValue
                                        .Parameters.Add("@Location", SqlDbType.VarChar).Value = StoreList.SelectedValue
                                        .Parameters.Add("@PaidBounty", SqlDbType.Bit).Value = BountyBox.Checked
                                        .Parameters.Add("@PaidBarnyard", SqlDbType.Bit).Value = BarnyardBox.Checked
                                        .Parameters.Add("@PaidPloughman", SqlDbType.Bit).Value = PloughmanBox.Checked
                                        .Parameters.Add("@Week", SqlDbType.SmallDateTime).Value = "1/1/1900"
                                    End With
                                    Try
                                        conn2.Open()
                                        comm2.ExecuteNonQuery()
                                    Catch ex As SqlException
                                        Literal0.Text = "Were sorry, there was an error"
                                        Return False
                                    End Try
                                End Using
                            End Using
                            Dim startDate As DateTime = DateTime.Now
                            Dim endDate As DateTime = "12/31/2018"
                            Dim diff As TimeSpan = endDate - startDate
                            Dim days As Integer = diff.Days
                            For i As Integer = 0 To days
                                Dim testDate = startDate.AddDays(i)
                                Select Case testDate.DayOfWeek
                                    Case DayOfWeek.Thursday
                                        If CheckExcluded(testDate.ToShortDateString) = False Then
                                            query = "INSERT INTO Weekly (SubId, bounty, barnyard, ploughman, PickupDay, Location, Vacation, PaidBounty, PaidBarnyard, PaidPloughman, Pickedup, Notes, Week) VALUES (@SubId, @bounty, @barnyard, @ploughman, @PickupDay, @Location, 'False', @PaidBounty, @PaidBarnyard, @PaidPloughman, 'False', '', @Week) "
                                            Using conn2 As New SqlConnection(ConnectionString)
                                                Using comm2 As New SqlCommand()
                                                    With comm2
                                                        .Connection = conn2
                                                        .CommandType = CommandType.Text
                                                        .CommandText = query
                                                        comm2.Parameters.Add("@SubId", SqlDbType.Int).Value = SubId
                                                        .Parameters.Add("@barnyard", SqlDbType.Bit).Value = BarnyardBox.Checked
                                                        .Parameters.Add("@bounty", SqlDbType.Bit).Value = BountyBox.Checked
                                                        .Parameters.Add("@ploughman", SqlDbType.Bit).Value = PloughmanBox.Checked
                                                        .Parameters.Add("@PickupDay", SqlDbType.VarChar).Value = PickupDayList.SelectedValue
                                                        .Parameters.Add("@Location", SqlDbType.VarChar).Value = StoreList.SelectedValue
                                                        .Parameters.Add("@PaidBounty", SqlDbType.VarChar).Value = False
                                                        .Parameters.Add("@PaidBarnyard", SqlDbType.VarChar).Value = False
                                                        .Parameters.Add("@PaidPloughman", SqlDbType.VarChar).Value = False
                                                        .Parameters.Add("@Week", SqlDbType.SmallDateTime).Value = testDate.ToShortDateString()
                                                    End With
                                                    Try
                                                        conn2.Open()

                                                        comm2.ExecuteNonQuery()


                                                    Catch ex As SqlException
                                                        Literal0.Text = "Were sorry, there was an error"
                                                        Return False
                                                    End Try
                                                End Using
                                            End Using
                                        End If
                                        Exit Select
                                End Select
                            Next
                        Loop
                        Using conn2 As New SqlConnection(ConnectionString)
                            If BountyBox.Checked = True Then
                                For Each Weekrow As GridViewRow In GridView1.Rows
                                    Dim BountyPaid As CheckBox = TryCast(Weekrow.FindControl("BountyPaidCheck"), CheckBox)
                                    Dim week As String = Weekrow.Cells(0).Text
                                    If week = "Deposit" Then
                                        week = "1/1/1900"
                                    End If
                                    Try
                                        If BountyPaid.Checked = True Then

                                            Dim pattern As String = "-(.*?)/"
                                            Dim replacement As String = "/" & vbCrLf
                                            Dim rgx As New Regex(pattern, RegexOptions.Singleline)
                                            week = rgx.Replace(week, replacement)
                                            week = (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "")
                                            If conn2.State = ConnectionState.Open Then
                                                conn2.Close()
                                            End If
                                            conn2.Open()
                                            Dim sql As String = "update weekly set PaidBounty='True' where SubID='" + SubId.ToString() + "' and week='" + week + "'"
                                            Dim cmd As New SqlCommand(sql)
                                            cmd.CommandType = CommandType.Text
                                            cmd.Connection = conn2
                                            cmd.ExecuteNonQuery()
                                        End If
                                    Catch ex As Exception
                                        Literal0.Text = "Were sorry, there was an error"
                                        Return False
                                    End Try

                                Next
                            End If
                            If BarnyardBox.Checked = True Then
                                For Each Weekrow As GridViewRow In GridView1.Rows
                                    Dim BountyPaid As CheckBox = TryCast(Weekrow.FindControl("BarnyardPaidCheck"), CheckBox)
                                    If BountyPaid.Enabled = True And BountyPaid.Checked = True Then
                                        Dim week As String = Weekrow.Cells(0).Text
                                        If week = "Deposit" Then
                                            week = "1/1/1900"
                                        End If
                                        Dim pattern As String = "-(.*?)/"
                                        Dim replacement As String = "/" & vbCrLf
                                        Dim rgx As New Regex(pattern, RegexOptions.Singleline)
                                        week = rgx.Replace(week, replacement)
                                        week = (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "")
                                        If conn2.State = ConnectionState.Open Then
                                            conn2.Close()
                                        End If
                                        conn2.Open()
                                        Dim sql As String = "update weekly set PaidBarnyard='True' where SubID='" + SubId.ToString() + "' and week='" + week + "'"
                                        Dim cmd As New SqlCommand(sql)
                                        cmd.CommandType = CommandType.Text
                                        cmd.Connection = conn2
                                        cmd.ExecuteNonQuery()
                                    End If
                                Next
                            End If
                            If PloughmanBox.Checked = True Then
                                For Each Weekrow As GridViewRow In GridView1.Rows
                                    Dim BountyPaid As CheckBox = TryCast(Weekrow.FindControl("PloughmanPaidCheck"), CheckBox)
                                    If BountyPaid.Enabled = True And BountyPaid.Checked = True Then
                                        Dim week As String = Weekrow.Cells(0).Text
                                        If week = "Deposit" Then
                                            week = "1/1/1900"
                                        End If
                                        Dim pattern As String = "-(.*?)/"
                                        Dim replacement As String = "/" & vbCrLf
                                        Dim rgx As New Regex(pattern, RegexOptions.Singleline)
                                        week = rgx.Replace(week, replacement)
                                        week = (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "")
                                        If conn2.State = ConnectionState.Open Then
                                            conn2.Close()
                                        End If
                                        conn2.Open()
                                        Dim sql As String = "update weekly set PaidPloughman='True' where SubID='" + SubId.ToString + "' and week='" + week + "'"
                                        Dim cmd As New SqlCommand(sql)
                                        cmd.CommandType = CommandType.Text
                                        cmd.Connection = conn2
                                        cmd.ExecuteNonQuery()
                                    End If
                                Next
                            End If
                        End Using


                    Catch ex As SqlException
                        Literal1.Text = "We're sorry, there seems to have been an error"
                        Return False
                    Finally
                        If (mySqlConnection.State = ConnectionState.Open) Then
                            mySqlConnection.Close()
                        End If
                    End Try
                End Using
            End Using
            Return True
        Catch ex As SqlException
            Literal1.Text = "We're sorry, there seems to have been an error"
            Return False
        End Try


    End Function
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
            Literal0.Text = "Were sorry, there was an error"
        End Try
    End Function
    Protected Sub FillInfo()
        GridView1.Columns(1).Visible = False
        GridView1.Columns(2).Visible = False
        GridView1.Columns(3).Visible = False
        Dim dt As New DataTable()
        dt.Columns.Add("Week")
        dt.Columns.Add("PaidBounty")
        dt.Columns.Add("PaidBarnyard")
        dt.Columns.Add("PaidPloughman")
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        dt.Rows.Add("Deposit", "True", "True", "True")
        If NextYear.Checked = False Then
            Dim myDataReader2 As SqlDataReader
            Dim mySqlConnection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
            Dim mySqlCommand2 As SqlCommand
            Dim SDateRange As String = ""
            Dim query As String = "select Sstart, send from seasons where currents='true'"
            Try
                Using conn As New SqlConnection(ConnectionString)
                    Using mySqlConnection2
                        mySqlCommand2 = New SqlCommand(query, mySqlConnection2)
                        mySqlConnection2.Open()
                        myDataReader2 = mySqlCommand2.ExecuteReader()
                        If myDataReader2.HasRows Then
                            Do While myDataReader2.Read()
                                SDateRange = " and week <= '" + myDataReader2.GetDateTime(1) + "' "
                            Loop
                        End If
                        myDataReader2.Close()
                    End Using
                End Using
            Finally
            End Try
            Dim SqlQuary As String = "SELECT DISTINCT Week FROM Weekly where week>='" + Date.Today.AddDays(-1) + "'" + SDateRange + "ORDER BY [Week]"
            mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
            Try
                Using mySqlConnection
                    mySqlCommand = New SqlCommand(SqlQuary, mySqlConnection)
                    mySqlConnection.Open()
                    myDataReader = mySqlCommand.ExecuteReader()
                    If myDataReader.HasRows Then
                        Dim SubInfo As String = ""
                        Dim paid As String = ""
                        Dim pickedup As String = ""
                        Dim vacation As String = ""
                        Do While myDataReader.Read()
                            Dim week As String = (myDataReader.GetDateTime(0).Month.ToString + "/" + myDataReader.GetDateTime(0).Day.ToString + "-" + myDataReader.GetDateTime(0).AddDays(1).Day.ToString + "/" + myDataReader.GetDateTime(0).Year.ToString)
                            dt.Rows.Add(week, "False", "False", "False")
                        Loop
                    Else
                        Console.WriteLine("No rows found.")
                    End If

                    myDataReader.Close()
                End Using
            Finally
            End Try
        ElseIf NextYear.Checked = True Then

        End If
        GridView1.DataSource = dt
        GridView1.DataBind()
        changeColumns()
    End Sub
    Protected Sub NextChanged(sender As [Object], e As EventArgs)
        FillInfo()
    End Sub
    Protected Sub OnCheckedChanged(sender As [Object], e As EventArgs)
        UpdatePanel1.Update()
        Dim days As Integer = 0
        For Each Weekrow As GridViewRow In GridView1.Rows
            Dim BountyPaid As CheckBox = TryCast(Weekrow.FindControl("BountyPaidCheck"), CheckBox)
            If BountyPaid.Checked = True Then
                days += 1
            End If
            Dim BarnyardPaid As CheckBox = TryCast(Weekrow.FindControl("BarnyardPaidCheck"), CheckBox)
            If BarnyardPaid.Checked = True Then
                days += 1
            End If
            Dim PloughmanPaid As CheckBox = TryCast(Weekrow.FindControl("PloughmanPaidCheck"), CheckBox)
            If PloughmanPaid.Checked = True Then
                days += 1
            End If
        Next
        If days * 33 = 0 Then
            Price.Text = "$0.00"
        Else
            Price.Text = "" + (days * 33).ToString("C2")
        End If

    End Sub
    Protected Sub OnBoxChanged(sender As Object, e As EventArgs)
        changeColumns()
    End Sub
    Protected Sub changeColumns()
        If BountyBox.Checked = False Then
            GridView1.Columns(1).Visible = False
            For Each Weekrow As GridViewRow In GridView1.Rows
                If Weekrow.Cells(0).Text = "Deposit" Then
                    Dim BountyPaid As CheckBox = TryCast(Weekrow.FindControl("BountyPaidCheck"), CheckBox)
                    BountyPaid.Checked = False
                    BountyPaid.Enabled = False
                End If
            Next
        Else
            GridView1.Columns(1).Visible = True
            For Each Weekrow As GridViewRow In GridView1.Rows
                If Weekrow.Cells(0).Text = "Deposit" Then
                    Dim BountyPaid As CheckBox = TryCast(Weekrow.FindControl("BountyPaidCheck"), CheckBox)
                    BountyPaid.Checked = True
                    BountyPaid.Enabled = False
                End If
            Next
        End If
        If BarnyardBox.Checked = False Then
            GridView1.Columns(2).Visible = False
            For Each Weekrow As GridViewRow In GridView1.Rows
                If Weekrow.Cells(0).Text = "Deposit" Then
                    Dim BarnyardPaid As CheckBox = TryCast(Weekrow.FindControl("BarnyardPaidCheck"), CheckBox)
                    BarnyardPaid.Checked = False
                    BarnyardPaid.Enabled = False
                End If
            Next
        Else
            GridView1.Columns(2).Visible = True
            For Each Weekrow As GridViewRow In GridView1.Rows
                If Weekrow.Cells(0).Text = "Deposit" Then
                    Dim BarnyardPaid As CheckBox = TryCast(Weekrow.FindControl("BarnyardPaidCheck"), CheckBox)
                    BarnyardPaid.Checked = True
                    BarnyardPaid.Enabled = False
                End If
            Next
        End If
        If PloughmanBox.Checked = False Then
            GridView1.Columns(3).Visible = False
            For Each Weekrow As GridViewRow In GridView1.Rows
                If Weekrow.Cells(0).Text = "Deposit" Then
                    Dim PloughmanPaid As CheckBox = TryCast(Weekrow.FindControl("PloughmanPaidCheck"), CheckBox)
                    PloughmanPaid.Checked = False
                    PloughmanPaid.Enabled = False
                End If
            Next
        Else
            GridView1.Columns(3).Visible = True
            For Each Weekrow As GridViewRow In GridView1.Rows
                If Weekrow.Cells(0).Text = "Deposit" Then
                    Dim PloughmanPaid As CheckBox = TryCast(Weekrow.FindControl("PloughmanPaidCheck"), CheckBox)
                    PloughmanPaid.Checked = True
                    PloughmanPaid.Enabled = False
                End If
            Next
        End If
        UpdatePanel1.Update()
        Dim days As Integer = 0
        For Each Weekrow As GridViewRow In GridView1.Rows
            Dim BountyPaid As CheckBox = TryCast(Weekrow.FindControl("BountyPaidCheck"), CheckBox)
            If BountyPaid.Checked = True Then
                days += 1
            End If
            Dim BarnyardPaid As CheckBox = TryCast(Weekrow.FindControl("BarnyardPaidCheck"), CheckBox)
            If BarnyardPaid.Checked = True Then
                days += 1
            End If
            Dim PloughmanPaid As CheckBox = TryCast(Weekrow.FindControl("PloughmanPaidCheck"), CheckBox)
            If PloughmanPaid.Checked = True Then
                days += 1
            End If
        Next
        If days * 33 = 0 Then
            Price.Text = "$0.00"
        Else
            Price.Text = "" + (days * 33).ToString("C2")
        End If
    End Sub
    Function UpdMailChimp(email As String, Bounty As Boolean, Barnyard As Boolean, Ploughman As Boolean) As Boolean
        Try
            Dim webAddr As String = ""
            If Bounty = True Then
                webAddr += "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=0a27dd543a&email[email]=" + email1.Text.Trim + "&merge_vars[FNAME]=" + firstname1.Text.Trim + "&merge_vars[LNAME]=" + lastname1.Text.Trim + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim + "&double_optin=false&send_welcome=false"
                Dim FwebAddr As New Uri(webAddr)
                Dim httpWebRequest = DirectCast(WebRequest.Create(FwebAddr), HttpWebRequest)
                httpWebRequest.ContentType = "application/json"
                Dim httpResponse = DirectCast(httpWebRequest.GetResponse(), HttpWebResponse)
                Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                    Dim val = streamReader.ReadToEnd()
                End Using
            End If
            If Barnyard = True Then
                webAddr += "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=2335ec6f51&email[email]=" + email1.Text.Trim + "&merge_vars[FNAME]=" + firstname1.Text.Trim + "&merge_vars[LNAME]=" + lastname1.Text.Trim + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim + "&double_optin=false&send_welcome=false"
                Dim FwebAddr As New Uri(webAddr)
                Dim httpWebRequest = DirectCast(WebRequest.Create(FwebAddr), HttpWebRequest)
                httpWebRequest.ContentType = "application/json"
                Dim httpResponse = DirectCast(httpWebRequest.GetResponse(), HttpWebResponse)
                Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                    Dim val = streamReader.ReadToEnd()
                End Using
            End If
            If Ploughman = True Then
                webAddr += "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=4801343502&email[email]=" + email1.Text.Trim + "&merge_vars[FNAME]=" + firstname1.Text.Trim + "&merge_vars[LNAME]=" + lastname1.Text.Trim + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim + "&double_optin=false&send_welcome=false"
                Dim FwebAddr As New Uri(webAddr)
                Dim httpWebRequest = DirectCast(WebRequest.Create(FwebAddr), HttpWebRequest)
                httpWebRequest.ContentType = "application/json"
                Dim httpResponse = DirectCast(httpWebRequest.GetResponse(), HttpWebResponse)
                Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                    Dim val = streamReader.ReadToEnd()
                End Using
            End If
            Return True
        Catch ex As Exception
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
            oMail1.To.Add(New MailAddress("dbccemtp@gmail.com"))
            oMail1.Subject = "Root Cellar Error"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "Error Updating MailChimp user: " + Username + "<br /><br />"
            oMail1.Body &= ex.Message + "<br /><br />" + ex.StackTrace
            oMail1.Body &= "</body>"
            oMail1.Body &= "</html>"
            Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
            oMail1.AlternateViews.Add(htmlView2)
            Dim smtpmail2 As New SmtpClient("relay-hosting.secureserver.net")
            smtpmail2.EnableSsl = False
            smtpmail2.UseDefaultCredentials = True
            smtpmail2.Send(oMail1)
            oMail1 = Nothing
            Return False
        End Try

    End Function

End Class



