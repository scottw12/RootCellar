Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Data
Imports PerceptiveMCAPI.Methods
Imports PerceptiveMCAPI.Types
Imports PerceptiveMCAPI
Imports System.Net
Imports System.IO

Partial Class subscribe
    Inherits System.Web.UI.Page

    Dim Box As String = ""
    Private conn As SqlConnection = Nothing
    Private ConnectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ToString
    Private cmd As SqlCommand = Nothing
    Dim password As String = ""
    Dim Username As String = ""
    Dim useremail As String = ""
    Dim RedirURL As String = ""
    '****************** SalesVu *********************
    Dim isDev As Boolean
    '***** Development *******
   ' Dim APIKey As String = "cdab5266e335a4b4a11661198393ff9d"
    'Dim StoreID As String = "1668"
    'Dim url As String = "https://dev.salesvu.com/townvu/api/index.php?request="
    '***** Production *******
    Dim APIKey As String = "a662c77bd1c244eb3440a3aa9dedc5bb"
    Dim StoreID As String = "34798"
    Dim url As String = "https://www.salesvu.com/townvu/api/index.php?request="
    '****************** SalesVu *********************
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        '  Membership.DeleteUser("test.jkm")
        '****************** SalesVu *********************
        'isDev = True
        '****************** SalesVu *********************
        If Not Page.IsPostBack Then
            BarnyardNL.Visible = False
            BountyNL.Visible = False
            PloughmanNL.Visible = False
            If Not (Request.QueryString("B") Is Nothing) Then
                If Request.QueryString("B").ToString() <> "" Then
                    If Request.QueryString("B").ToString() = "Barnyard" Then
                        Box = "Barnyard"
                        BarnyardBox.Checked = True
                        BarnyardNL.Visible = True
                    ElseIf Request.QueryString("B").ToString() = "Bounty" Then
                        Box = "Bounty"
                        BountyBox.Checked = True
                        BountyNL.Visible = True
                    ElseIf Request.QueryString("B").ToString() = "Ploughman" Then
                        Box = "Ploughman"
                        PloughmanBox.Checked = True
                        PloughmanNL.Visible = True
                    End If
                End If
            End If
            FillInfo()
            FillDayInfo()
            FillStoreInfo()
            Loadactiveboxes()
            'Price.Text = "$0.00"
        End If
    End Sub
    Protected Sub FillInfo()
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
        'GridView1.Columns(1).Visible = False
        'GridView1.Columns(2).Visible = False
        'GridView1.Columns(3).Visible = False
        Dim SqlQuary As String = "SELECT DISTINCT Week FROM Weekly where week>='" + Date.Today.AddDays(-1) + "'" + SDateRange + "ORDER BY [Week]"
        Dim dt As New DataTable()
        dt.Columns.Add("Week")
        dt.Columns.Add("PaidBounty")
        dt.Columns.Add("PaidBarnyard")
        dt.Columns.Add("PaidPloughman")
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        dt.Rows.Add("Deposit", "True", "True", "True")
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
        'GridView1.DataSource = dt
        'GridView1.DataBind()
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
    Private Sub Loadactiveboxes()
        Try
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
                        If myDataReader.GetBoolean(0) = False Then
                            BountyBox.Enabled = False
                            BountyActive.Text = "<span style='color:red;'>  No longer enrolling for the current season!</span>"
                        End If
                        If myDataReader.GetBoolean(1) = False Then
                            BarnyardBox.Enabled = False
                            BarnyardActive.Text = "<span style='color:red;'>  No longer enrolling for the current season!</span>"
                        End If
                        If myDataReader.GetBoolean(2) = False Then
                            PloughmanBox.Enabled = False
                            PloughmanActive.Text = "<span style='color:red;'>  No longer enrolling for the current season!</span>"
                            Literal3.Text = "."
                        End If
                    End If
                Loop
            Finally
                If (mySqlConnection.State = ConnectionState.Open) Then
                    mySqlConnection.Close()
                End If
            End Try
            mySqlCommand = New SqlCommand("SELECT name FROM seasons where enroll='true'", mySqlConnection)
            Try
                mySqlConnection.Open()
                myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                Do While (myDataReader.Read())
                    If myDataReader.HasRows Then
                        CurrentSeason.Visible = True 'False
                        NextSeason.Visible = False 'True
                        Literal3.Text += myDataReader.GetString(0)
                    End If
                Loop
            Finally
                If (mySqlConnection.State = ConnectionState.Open) Then
                    mySqlConnection.Close()
                End If
            End Try
        Catch ex As Exception

        End Try
        
    End Sub
    Protected Sub StoreList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles StoreList.SelectedIndexChanged
        If StoreList.SelectedValue = "Jefferson City" Then
            If PickupDayList.SelectedValue = "Friday" Then
                PickupDayList.SelectedValue = "Thursday"
                PUDLiteral.Text = "Pickups are only available on Thursday's at the Jefferson City Location"
            Else
                PUDLiteral.Text = ""
            End If
        ElseIf StoreList.SelectedValue = "DHSS (Employee Only)" Then
            If PickupDayList.SelectedValue = "Friday" Then
                PickupDayList.SelectedValue = "Thursday"
                PUDLiteral.Text = "Pickups are only available on Thursday's at the DHSS Location"
            Else
                PUDLiteral.Text = ""
            End If
        ElseIf StoreList.SelectedValue = "Mizzou North (Employee Only)" Then
            If PickupDayList.SelectedValue = "Thursday" Then
                PickupDayList.SelectedValue = "Friday"
                PUDLiteral.Text = "Pickups are only available on Friday's at the Mizzou North Location"
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
                'PickupDayList.SelectedValue = "Thursday"
                'PUDLiteral.Text = "Pickups are only available on Thursday's at the Jefferson City Location"
            ElseIf StoreList.SelectedValue = "DHSS (Employee Only)" Then
                PickupDayList.SelectedValue = "Thursday"
                PUDLiteral.Text = "Pickups are only available on Thursday's at the DHSS Location"
            Else
                PUDLiteral.Text = ""
            End If
        ElseIf PickupDayList.SelectedValue = "Thursday" Then
            If StoreList.SelectedValue = "Mizzou North (Employee Only)" Then
                PickupDayList.SelectedValue = "Friday"
                PUDLiteral.Text = "Pickups are only available on Friday's at the Mizzou North Location"
            Else
                PUDLiteral.Text = ""
            End If
        Else
            PUDLiteral.Text = ""
        End If
    End Sub
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Literal1.Text = ""
        If BountyBox.Checked = False And BarnyardBox.Checked = False And PloughmanBox.Checked = False And NewBountyBox.Checked = False And NewBarnyardBox.Checked = False And NewPloughmanBox.Checked = False Then
            Literal1.Text = "<span style='color:red;'>Please select at least one box </span>"
        Else
            If CreateUser() = True Then
                SendEmail()
                If DBInsert() = True Then
                    If Payment() = False Then
                        Literal1.Text = "<span style='color:red;'><h3> There was a problem processing your payment. </h3>Please login to your new account and try again. An email has been sent to you which contains your login information.</span>"
                    End If
                End If
                If UpdMailChimp(email1.Text, BountyNL.Checked, BarnyardNL.Checked, PloughmanNL.Checked) = False Then
                End If
                Literal0.Text += "<span style='color:green;'><h2>Your account has been created, but is not yet active.</h2></span><h3>Please click <a href='" + RedirURL.Replace("+http", "http") + "' target='_blank' >here</a> to be redirected to our payment gateway. Here you can submit payment for your order and finalize your account.</h3><br /><br /><br /> "
                AppPanel.Visible = False
            Else
                'Literal1.Text = "<span style='color:red;'><h3> There was a problem creating your account. Please check all of the info and try again later.</h3></span>"
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
            'Dim oMail1 As MailMessage = New MailMessage()
            'oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
            'oMail1.To.Add(New MailAddress("scottw@jkmcomm.com"))
            'oMail1.Subject = "Root Cellar Error"
            'oMail1.Priority = MailPriority.High
            'oMail1.IsBodyHtml = True
            'oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            'oMail1.Body &= "<head><title></title></head>"
            'oMail1.Body &= "<body>"
            'oMail1.Body &= "Error creating user1: " + Username + "<br /><br />"
            'oMail1.Body &= ex.Message + "<br /><br />" + ex.StackTrace
            'oMail1.Body &= "</body>"
            'oMail1.Body &= "</html>"
            'Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
            'oMail1.AlternateViews.Add(htmlView2)
            'Dim smtpmail2 As New System.Net.Mail.SmtpClient
            'smtpmail2.Send(oMail1)
            'oMail1 = Nothing
            Literal1.Text = "We're sorry, there seems to have been an error"
            Literal1.Text = ex.Message + "<br />" + ex.StackTrace
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
        oMail0.Body &= "Welcome to the Root Cellar!<br /> The Username and Password for our new online subscription management tool is below. Here you will be able to schedule vacation weeks, provide advance payment online and view your subscription information.<br /><br />You can login to <a href='http://rdo.rootcellarboxes.com/login'>http://rdo.rootcellarboxes.com/login</a> using:<br />"
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
        Dim htmlView As AlternateView = AlternateView.CreateAlternateViewFromString(oMail0.Body, Nothing, "text/html")
        oMail0.AlternateViews.Add(htmlView)
        Dim smtpmail0 As New System.Net.Mail.SmtpClient
        Dim oMail1 As MailMessage = New MailMessage()
        oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
        oMail1.To.Add(New MailAddress("rootcellarmo@gmail.com"))
        oMail1.Subject = "New Root Cellar Subscriber"
        oMail1.Priority = MailPriority.High
        oMail1.IsBodyHtml = True
        oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
        oMail1.Body &= "<head><title></title></head>"
        oMail1.Body &= "<body>"
        oMail1.Body &= "Hello Jake,<br /><br />"
        oMail1.Body &= "" + firstname1.Text.Replace("'", "").Replace("""", "").Replace(" ", "") + " " + lastname1.Text.Replace("'", "").Replace("""", "").Replace(" ", "") + " has created a new account with the following login:<br /><br />"
        oMail1.Body &= "Username: " + Username + "<br />"
        oMail1.Body &= "Passowrd: " + password + "<br /><br />"
        oMail1.Body &= "</body>"
        oMail1.Body &= "</html>"
        Dim htmlView1 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
        oMail1.AlternateViews.Add(htmlView1)
        Dim smtpmail2 As New System.Net.Mail.SmtpClient
        Try
            smtpmail0.Send(oMail0)
            smtpmail2.Send(oMail1)
        Catch ex As Exception
            Dim oMail2 As MailMessage = New MailMessage()
            oMail2.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
            oMail2.To.Add(New MailAddress("scottw@jkmcomm.com"))
            oMail2.Subject = "Root Cellar Error"
            oMail2.Priority = MailPriority.High
            oMail2.IsBodyHtml = True
            oMail2.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail2.Body &= "<head><title></title></head>"
            oMail2.Body &= "<body>"
            oMail2.Body &= "Error sending email user2: " + Username + "<br /><br />"
            oMail2.Body &= ex.Message + "<br /><br />" + ex.StackTrace
            oMail2.Body &= "</body>"
            oMail2.Body &= "</html>"
            Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail2.Body, Nothing, "text/html")
            oMail2.AlternateViews.Add(htmlView2)
            smtpmail0.Send(oMail2)
            oMail2 = Nothing
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
                        If BarnyardBox.Checked = True Or NewBarnyardBox.Checked = True Then
                            .Parameters.Add("@barnyard", SqlDbType.Bit).Value = True
                        Else
                            .Parameters.Add("@barnyard", SqlDbType.Bit).Value = False
                        End If
                        If BountyBox.Checked = True Or NewBountyBox.Checked = True Then
                            .Parameters.Add("@bounty", SqlDbType.Bit).Value = True
                        Else
                            .Parameters.Add("@bounty", SqlDbType.Bit).Value = False
                        End If
                        If PloughmanBox.Checked = True Or NewPloughmanBox.Checked = True Then
                            .Parameters.Add("@ploughman", SqlDbType.Bit).Value = True
                        Else
                            .Parameters.Add("@ploughman", SqlDbType.Bit).Value = False
                        End If
                        Dim active As Boolean = False
                        If NewBarnyardBox.Checked = True Or NewBountyBox.Checked = True Or NewPloughmanBox.Checked = True Then
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
                                        .Parameters.Add("@PaidBounty", SqlDbType.Bit).Value = "False"
                                        .Parameters.Add("@PaidBarnyard", SqlDbType.Bit).Value = "False"
                                        .Parameters.Add("@PaidPloughman", SqlDbType.Bit).Value = "False"
                                        .Parameters.Add("@Week", SqlDbType.SmallDateTime).Value = "1/1/1900"
                                    End With
                                    Try
                                        conn2.Open()
                                        comm2.ExecuteNonQuery()
                                    Catch ex As SqlException
                                        Dim oMail1 As MailMessage = New MailMessage()
                                        oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
                                        oMail1.To.Add(New MailAddress("scottw@jkmcomm.com"))
                                        oMail1.Subject = "Root Cellar Error"
                                        oMail1.Priority = MailPriority.High
                                        oMail1.IsBodyHtml = True
                                        oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
                                        oMail1.Body &= "<head><title></title></head>"
                                        oMail1.Body &= "<body>"
                                        oMail1.Body &= "Error creating user3: " + Username + "<br /><br />"
                                        oMail1.Body &= ex.Message + "<br /><br />" + ex.StackTrace
                                        oMail1.Body &= "</body>"
                                        oMail1.Body &= "</html>"
                                        Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
                                        oMail1.AlternateViews.Add(htmlView2)
                                        Dim smtpmail2 As New System.Net.Mail.SmtpClient
                                        smtpmail2.Send(oMail1)
                                        oMail1 = Nothing
                                        Literal0.Text = "We're sorry, there seems to have been an error."
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
                                                        Dim oMail1 As MailMessage = New MailMessage()
                                                        oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
                                                        oMail1.To.Add(New MailAddress("scottw@jkmcomm.com"))
                                                        oMail1.Subject = "Root Cellar Error"
                                                        oMail1.Priority = MailPriority.High
                                                        oMail1.IsBodyHtml = True
                                                        oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
                                                        oMail1.Body &= "<head><title></title></head>"
                                                        oMail1.Body &= "<body>"
                                                        oMail1.Body &= "Error creating user4: " + Username + "<br /><br />"
                                                        oMail1.Body &= ex.Message + "<br /><br />" + ex.StackTrace
                                                        oMail1.Body &= "</body>"
                                                        oMail1.Body &= "</html>"
                                                        Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
                                                        oMail1.AlternateViews.Add(htmlView2)
                                                        Dim smtpmail2 As New System.Net.Mail.SmtpClient
                                                        smtpmail2.Send(oMail1)
                                                        oMail1 = Nothing
                                                        Literal0.Text = "We're sorry, there seems to have been an error."
                                                        Return False
                                                    End Try
                                                End Using
                                            End Using
                                        End If
                                        Exit Select
                                End Select
                            Next
                        Loop

                    Catch ex As SqlException
                        Dim oMail1 As MailMessage = New MailMessage()
                        oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
                        oMail1.To.Add(New MailAddress("scottw@jkmcomm.com"))
                        oMail1.Subject = "Root Cellar Error"
                        oMail1.Priority = MailPriority.High
                        oMail1.IsBodyHtml = True
                        oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
                        oMail1.Body &= "<head><title></title></head>"
                        oMail1.Body &= "<body>"
                        oMail1.Body &= "Error creating user5: " + Username + "<br /><br />"
                        oMail1.Body &= ex.Message + "<br /><br />" + ex.StackTrace
                        oMail1.Body &= "</body>"
                        oMail1.Body &= "</html>"
                        Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
                        oMail1.AlternateViews.Add(htmlView2)
                        Dim smtpmail2 As New System.Net.Mail.SmtpClient
                        smtpmail2.Send(oMail1)
                        oMail1 = Nothing

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
            Dim smtpmail2 As New System.Net.Mail.SmtpClient
            Literal1.Text = "We're sorry, there seems to have been an error"
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
            oMail1.To.Add(New MailAddress("scottw@jkmcomm.com"))
            oMail1.Subject = "Root Cellar Error"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "Error creating user6: " + Username + "<br /><br />"
            oMail1.Body &= ex.Message + "<br /><br />" + ex.StackTrace
            oMail1.Body &= "</body>"
            oMail1.Body &= "</html>"
            Dim htmlView1 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
            oMail1.AlternateViews.Add(htmlView1)
            smtpmail2.Send(oMail1)
            oMail1 = Nothing
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
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
            oMail1.To.Add(New MailAddress("scottw@jkmcomm.com"))
            oMail1.Subject = "Root Cellar Error"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "Error creating user7: " + Username + "<br /><br />"
            oMail1.Body &= ex.Message + "<br /><br />" + ex.StackTrace
            oMail1.Body &= "</body>"
            oMail1.Body &= "</html>"
            Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
            oMail1.AlternateViews.Add(htmlView2)
            Dim smtpmail2 As New System.Net.Mail.SmtpClient
            smtpmail2.Send(oMail1)
            oMail1 = Nothing

            Return False
            Literal0.Text = "We're sorry, there seems to have been an error."
        End Try
    End Function

    Protected Sub OnBoxChanged(sender As Object, e As EventArgs)
        If BountyBox.Checked = False And NewBountyBox.Checked = False Then
            'GridView1.Columns(1).Visible = False
            BountyNL.Visible = False
            'For Each Weekrow As GridViewRow In GridView1.Rows
            '    If Weekrow.Cells(0).Text = "Deposit" Then
            '        Dim BountyPaid As CheckBox = TryCast(Weekrow.FindControl("BountyPaidCheck"), CheckBox)
            '        BountyPaid.Checked = False
            '        BountyPaid.Enabled = False
            '    End If
            'Next
        Else
            'GridView1.Columns(1).Visible = True
            BountyNL.Visible = True
            'For Each Weekrow As GridViewRow In GridView1.Rows
            '    If Weekrow.Cells(0).Text = "Deposit" Then
            '        Dim BountyPaid As CheckBox = TryCast(Weekrow.FindControl("BountyPaidCheck"), CheckBox)
            '        BountyPaid.Checked = True
            '        BountyPaid.Enabled = False
            '    End If
            'Next
        End If
        If BarnyardBox.Checked = False And NewBarnyardBox.Checked = False Then
            'GridView1.Columns(2).Visible = False
            BarnyardNL.Visible = True
            'For Each Weekrow As GridViewRow In GridView1.Rows
            '    If Weekrow.Cells(0).Text = "Deposit" Then
            '        Dim BarnyardPaid As CheckBox = TryCast(Weekrow.FindControl("BarnyardPaidCheck"), CheckBox)
            '        BarnyardPaid.Checked = False
            '        BarnyardPaid.Enabled = False
            '    End If
            'Next
        Else
            'GridView1.Columns(2).Visible = True
            BarnyardNL.Visible = True
            'For Each Weekrow As GridViewRow In GridView1.Rows
            '    If Weekrow.Cells(0).Text = "Deposit" Then
            '        Dim BarnyardPaid As CheckBox = TryCast(Weekrow.FindControl("BarnyardPaidCheck"), CheckBox)
            '        BarnyardPaid.Checked = True
            '        BarnyardPaid.Enabled = False
            '    End If
            'Next
        End If
        If PloughmanBox.Checked = False And NewPloughmanBox.Checked = False Then
            'GridView1.Columns(3).Visible = False
            PloughmanNL.Visible = False
            'For Each Weekrow As GridViewRow In GridView1.Rows
            '    If Weekrow.Cells(0).Text = "Deposit" Then
            '        Dim PloughmanPaid As CheckBox = TryCast(Weekrow.FindControl("PloughmanPaidCheck"), CheckBox)
            '        PloughmanPaid.Checked = False
            '        PloughmanPaid.Enabled = False
            '    End If
            'Next
        Else
            'GridView1.Columns(3).Visible = True
            PloughmanNL.Visible = True
            'For Each Weekrow As GridViewRow In GridView1.Rows
            '    If Weekrow.Cells(0).Text = "Deposit" Then
            '        Dim PloughmanPaid As CheckBox = TryCast(Weekrow.FindControl("PloughmanPaidCheck"), CheckBox)
            '        PloughmanPaid.Checked = True
            '        PloughmanPaid.Enabled = False
            '    End If
            'Next
        End If
        UpdatePanel1.Update()
        Dim days As Integer = 0
        'For Each Weekrow As GridViewRow In GridView1.Rows
        '    Dim BountyPaid As CheckBox = TryCast(Weekrow.FindControl("BountyPaidCheck"), CheckBox)
        '    If BountyPaid.Checked = True Then
        '        days += 1
        '    End If
        '    Dim BarnyardPaid As CheckBox = TryCast(Weekrow.FindControl("BarnyardPaidCheck"), CheckBox)
        '    If BarnyardPaid.Checked = True Then
        '        days += 1
        '    End If
        '    Dim PloughmanPaid As CheckBox = TryCast(Weekrow.FindControl("PloughmanPaidCheck"), CheckBox)
        '    If PloughmanPaid.Checked = True Then
        '        days += 1
        '    End If
        'Next
        'If days * 33 = 0 Then
        '    Price.Text = "$0.00"
        'Else
        '    Price.Text = "" + (days * 33).ToString("C2")
        'End If
    End Sub
    Function UpdMailChimp(email As String, Bounty As Boolean, Barnyard As Boolean, Ploughman As Boolean) As Boolean
        Try
            Dim webAddr As String = ""
            If Bounty = True Then
                webAddr += "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=f310b8a278&email[email]=" + email1.Text.Trim + "&merge_vars[FNAME]=" + firstname1.Text.Trim + "&merge_vars[LNAME]=" + lastname1.Text.Trim + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim + "&double_optin=false&send_welcome=false"
                Dim FwebAddr As New Uri(webAddr)
                Dim httpWebRequest = DirectCast(WebRequest.Create(FwebAddr), HttpWebRequest)
                httpWebRequest.ContentType = "application/json"
                Dim httpResponse = DirectCast(httpWebRequest.GetResponse(), HttpWebResponse)
                Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                    Dim val = streamReader.ReadToEnd()
                End Using
            End If
            If Barnyard = True Then
                webAddr += "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=1ad43508d8&email[email]=" + email1.Text.Trim + "&merge_vars[FNAME]=" + firstname1.Text.Trim + "&merge_vars[LNAME]=" + lastname1.Text.Trim + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim + "&double_optin=false&send_welcome=false"
                Dim FwebAddr As New Uri(webAddr)
                Dim httpWebRequest = DirectCast(WebRequest.Create(FwebAddr), HttpWebRequest)
                httpWebRequest.ContentType = "application/json"
                Dim httpResponse = DirectCast(httpWebRequest.GetResponse(), HttpWebResponse)
                Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                    Dim val = streamReader.ReadToEnd()
                End Using
            End If
            If Ploughman = True Then
                webAddr += "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=078a386ef9&email[email]=" + email1.Text.Trim + "&merge_vars[FNAME]=" + firstname1.Text.Trim + "&merge_vars[LNAME]=" + lastname1.Text.Trim + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim + "&double_optin=false&send_welcome=false"
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
            Return False
        End Try

    End Function
    Function Payment() As Boolean
        Dim webAddr As String = ""
        Dim val1 As String = ""
        Dim val2 As String = ""
        Dim bounty As String = ""
        Dim barnyard As String = ""
        Dim ploughman As String = ""
        Try
            Literal0.Text = ""
            Dim query1 As String = "INSERT INTO TempOrders (Username, "
            Dim query2 As String = "VALUES ('" + Username + "', "
            Dim Q As Integer = 0
            If isDev = False Then
                If StoreList.SelectedValue = "Downtown Columbia" Then
                    StoreID = "34798"
                    APIKey = "a662c77bd1c244eb3440a3aa9dedc5bb"
                    bounty = "9"
                    barnyard = "10"
                    ploughman = "11"
                ElseIf StoreList.SelectedValue = "Jefferson City" Then
                    StoreID = "34800"
                    APIKey = "9fedad4964460d40d5de103b706cb054"
                    bounty = "85"
                    barnyard = "86"
                    ploughman = "87"
                ElseIf StoreList.SelectedValue = "DHSS (Employee Only)" Then
                    StoreID = "34800"
                    APIKey = "9fedad4964460d40d5de103b706cb054"
                    bounty = "85"
                    barnyard = "86"
                    ploughman = "87"
                ElseIf StoreList.SelectedValue = "Mizzou North (Employee Only)" Then
                    StoreID = "34798"
                    APIKey = "a662c77bd1c244eb3440a3aa9dedc5bb"
                    bounty = "9"
                    barnyard = "10"
                    ploughman = "11"
                ElseIf StoreList.SelectedValue = "QUARTERDECK (Employee Only)" Then
                    StoreID = "34798"
                    APIKey = "a662c77bd1c244eb3440a3aa9dedc5bb"
                    bounty = "9"
                    barnyard = "10"
                    ploughman = "11"
                ElseIf StoreList.SelectedValue = "University Hospital/School of Medicine (Employee Only)" Then
                    StoreID = "34798"
                    APIKey = "a662c77bd1c244eb3440a3aa9dedc5bb"
                    bounty = "9"
                    barnyard = "10"
                    ploughman = "11"
                ElseIf StoreList.SelectedValue = "CRMC" Then
                    StoreID = "34800"
                    APIKey = "9fedad4964460d40d5de103b706cb054"
                    bounty = "85"
                    barnyard = "86"
                    ploughman = "87"
                ElseIf StoreList.SelectedValue = "CRMC Southwest" Then
                    StoreID = "34800"
                    APIKey = "9fedad4964460d40d5de103b706cb054"
                    bounty = "85"
                    barnyard = "86"
                    ploughman = "87"
                Else
                    bounty = "9"
                    barnyard = "10"
                    ploughman = "11"
                End If
            End If
            webAddr += "{'api_key':'" + APIKey + "',"
            webAddr += "'action':'create_order',"
            webAddr += "'store_id':'" + StoreID + "',"
            webAddr += "'online_customer_id':'1'"

            Dim prodDetails As String = ""
            Dim week As String = "1/1/1900"
            If BountyBox.Checked Or NewBountyBox.Checked Then
                Q += 1
                If prodDetails = "" Then
                    prodDetails = ",'order_details':["
                Else
                    prodDetails = ","
                End If
                prodDetails += "{'product_id':" + bounty + ","
                prodDetails += "'selling_price': '35.00',"
                prodDetails += "'unit_id':0,"
                prodDetails += "'quantity':1,"
                prodDetails += "'notes':[{"

                prodDetails += "'comment':'" + Username + "+Bounty+Deposit'"
                prodDetails += "}]}"
                webAddr += prodDetails
                query1 += "P" + Q.ToString + "Date, P" + Q.ToString + "Box, "
                query2 += "'" + week + "', 'Bounty', "
            End If
            If BarnyardBox.Checked = True Or NewBarnyardBox.Checked Then
                Q += 1
                If prodDetails = "" Then
                    prodDetails = ",'order_details':["
                Else
                    prodDetails = ","
                End If
                prodDetails += "{'product_id':" + barnyard + ","
                prodDetails += "'selling_price': '35.00',"
                prodDetails += "'unit_id':0,"
                prodDetails += "'quantity':1,"
                prodDetails += "'notes':[{"
                prodDetails += "'comment':'" + Username + "+Barnyard+Deposit'"
                prodDetails += "}]}"
                webAddr += prodDetails
                query1 += "P" + Q.ToString + "Date, P" + Q.ToString + "Box, "
                query2 += "'" + week + "', 'Barnyard', "
            End If
            If PloughmanBox.Checked = True Or NewPloughmanBox.Checked Then
                Q += 1
                If prodDetails = "" Then
                    prodDetails = ",'order_details':["
                Else
                    prodDetails = ","
                End If
                prodDetails += "{'product_id':" + ploughman + ","
                prodDetails += "'selling_price': '35.00',"
                prodDetails += "'unit_id':0,"
                prodDetails += "'quantity':1,"
                prodDetails += "'notes':[{"
                prodDetails += "'comment':'" + Username + "+Ploughman+Deposit'"
                prodDetails += "}]}"
                webAddr += prodDetails
                query1 += "P" + Q.ToString + "Date, P" + Q.ToString + "Box, "
                query2 += "'" + week + "', 'Ploughman', "
            End If
            webAddr += "]}"
            webAddr = HttpUtility.UrlEncode(webAddr)
            webAddr = webAddr.Replace("%27", "%22")
            webAddr = url + webAddr

            Dim FwebAddr As New Uri(webAddr)
            Dim httpWebRequest = DirectCast(WebRequest.Create(FwebAddr), HttpWebRequest)
            httpWebRequest.ContentType = "application/json"
            Dim httpResponse = DirectCast(httpWebRequest.GetResponse(), HttpWebResponse)
            Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                val1 = streamReader.ReadToEnd()
                Dim pattern As String = "{(.*?)order_id"":"
                Dim replacement As String = ""
                Dim rgx As New Regex(pattern, RegexOptions.Singleline)
                val2 = rgx.Replace(val1, replacement)
                pattern = ",(.*?)}}"
                replacement = ""
                Dim rgx2 As New Regex(pattern, RegexOptions.Singleline)
                val2 = rgx2.Replace(val2, replacement)
                RedirURL = GetURL(val2, StoreID)
                RedirURL = Server.UrlEncode(RedirURL).Replace("%0d%0a", "").Replace("%3a", ":").Replace("%5c%2f", "/").Replace("%3f", "?").Replace("%3d", "=").Replace("%22", """").Replace("%2c", ",").Replace("%3a", ":")
                query1 += "OrderID) "
                query2 += val2 + ")"
            End Using
            Dim query = query1 + query2
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
            Dim i As Integer = 1
            Dim NullFix As String = ""
            Do While i < 26
                NullFix += "update tempOrders set P" + i.ToString + "Box='' where P" + i.ToString + "Box is null update tempOrders set P" + i.ToString + "Date='1/1/1900' where P" + i.ToString + "Date is null "
                i += 1
            Loop
            Dim conn2 As New SqlConnection(ConnectionString)
            conn2.Open()
            Using UpdateCommand As New SqlCommand(NullFix, conn2)
                UpdateCommand.ExecuteNonQuery()
            End Using
            conn2.Close()

            Return True
        Catch ex As Exception
            Literal0.Text = "We're sorry, there seems to have been an error."
            Dim smtpmail2 As New System.Net.Mail.SmtpClient
            Literal1.Text = "We're sorry, there seems to have been an error"
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
            oMail1.To.Add(New MailAddress("scottw@jkmcomm.com"))
            oMail1.Subject = "Root Cellar Error"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "Error creating user9: " + Username + "<br /><br />"
            oMail1.Body &= ex.Message + "<br /><br />" + ex.StackTrace + "<br /><br />"
            oMail1.Body &= "WebAddr: " + webAddr + "<br />"
            oMail1.Body &= "Val1: " + val1 + "<br />"
            oMail1.Body &= "Val2: " + val2 + "<br />"
            oMail1.Body &= "Store:" + StoreList.SelectedValue + "<br />"
            oMail1.Body &= "</body>"
            oMail1.Body &= "</html>"
            Dim htmlView1 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
            oMail1.AlternateViews.Add(htmlView1)

            smtpmail2.Send(oMail1)
            oMail1 = Nothing
            Return False
        End Try
    End Function
    Function GetURL(OrderID As String, StoreID As String) As String
        Try
            Dim webAddr As String = ""
            webAddr += "{'api_key':'" + APIKey + "',"
            webAddr += "'action':'get_payment_url',"
            webAddr += "'store_id':'" + StoreID + "',"
            webAddr += "'online_customer_id':1,"
            webAddr += "'order_id':" + OrderID + ","
            webAddr += "'return_url':'http://www.rootcellarboxes.com/success',"
            webAddr += "'platform':'PC'"
            webAddr += "}"
            webAddr = HttpUtility.UrlEncode(webAddr)
            webAddr = webAddr.Replace("%27", "%22")
            webAddr = url + webAddr
            Dim FwebAddr As New Uri(webAddr)
            Dim httpWebRequest = DirectCast(WebRequest.Create(FwebAddr), HttpWebRequest)
            httpWebRequest.ContentType = "application/json"
            Dim httpResponse = DirectCast(httpWebRequest.GetResponse(), HttpWebResponse)
            Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                Dim val = streamReader.ReadToEnd()
                Dim Status As String = val
                Dim pURL As String = val
                Dim pattern As String = "{(.*?)status"":"""
                Dim replacement As String = ""
                Dim rgx As New Regex(pattern, RegexOptions.Singleline)
                Status = rgx.Replace(Status, replacement)
                pattern = """,(.*?)}}"
                replacement = ""
                Dim rgx2 As New Regex(pattern, RegexOptions.Singleline)
                Status = rgx2.Replace(Status, replacement)
                If Status.Contains("ok") Then
                    pattern = "{(.*?)https"
                    replacement = "https"
                    Dim rgx3 As New Regex(pattern, RegexOptions.Singleline)
                    pURL = rgx3.Replace(pURL, replacement)
                    pattern = """}}"
                    replacement = ""
                    Dim rgx4 As New Regex(pattern, RegexOptions.Singleline)
                    pURL = rgx4.Replace(pURL, replacement)
                End If
                Return pURL
            End Using
        Catch ex As Exception
            Literal0.Text = "We're sorry, there seems to have been an error."
            Dim smtpmail2 As New System.Net.Mail.SmtpClient
            Literal1.Text = "We're sorry, there seems to have been an error"
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
            oMail1.To.Add(New MailAddress("scottw@jkmcomm.com"))
            oMail1.Subject = "Root Cellar Error"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "Error creating user10: " + Username + "<br /><br />"
            oMail1.Body &= ex.Message + "<br /><br />" + ex.StackTrace
            oMail1.Body &= "</body>"
            oMail1.Body &= "</html>"
            Dim htmlView1 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
            oMail1.AlternateViews.Add(htmlView1)
            smtpmail2.Send(oMail1)
            oMail1 = Nothing
            Return ""
        End Try
        Return ""
    End Function
End Class
