Imports System.Data
Imports System.Net.Mail
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports PerceptiveMCAPI
Imports PerceptiveMCAPI.Types
Imports PerceptiveMCAPI.Methods
Imports System.Net
Imports System.IO

Partial Class account_Default
    Inherits RadAjaxPage

    Private conn As SqlConnection = Nothing
    Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
    Private cmd As SqlCommand = Nothing
    Dim SubId As Integer
    '****************** SalesVu *********************
    Dim isDev As Boolean
    '***** Development *******
    'Dim APIKey As String = "cdab5266e335a4b4a11661198393ff9d"
    'Dim StoreID As String = "1668"
    'Dim url As String = "https://dev.salesvu.com/townvu/api/index.php?request="
    '***** Production *******
    Dim APIKey As String = "a662c77bd1c244eb3440a3aa9dedc5bb"
    Dim StoreID As String = "34798"
    Dim url As String = "https://www.salesvu.com/townvu/api/index.php?request="
    '****************** SalesVu *********************
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        '****************** SalesVu *********************
        isDev = False
        '****************** SalesVu *********************
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
                        Response.Redirect("~/admin/")
                    ElseIf role = "Employee" Then
                        Response.Redirect("~/admin/")
                    End If
                Loop
            Finally
                If (mySqlConnection.State = ConnectionState.Open) Then
                    mySqlConnection.Close()
                End If
            End Try
            FillInfo()
            FillPaidInfo()
            FillDayInfo()
            FillStoreInfo()
            FillWeekInfo()
            FillVacInfo()
            Price.Text = "$0.00"

        End If
    End Sub
    Protected Sub FillInfo()
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        mySqlCommand = New SqlCommand("SELECT SubID FROM subscribers Where Username= '" + Membership.GetUser().ToString + "'", mySqlConnection)
        Try
            mySqlConnection.Open()
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Do While (myDataReader.Read())
                SubId = myDataReader.GetInt32(0)
            Loop
        Finally
            If (mySqlConnection.State = ConnectionState.Open) Then
                mySqlConnection.Close()
            End If
        End Try
        mySqlCommand = New SqlCommand("SELECT Firstname1, Firstname2, lastname1, lastname2, email1, email2, phone1, phone2, address, city, state, zip, allergies, BountyNL, BarnyardNL, PloughmanNL, vacused, bounty, barnyard, ploughman, pickupday, store FROM Subscribers Where Username= '" + Membership.GetUser().ToString + "'", mySqlConnection)
        Try
            mySqlConnection.Open()
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Do While (myDataReader.Read())
                If Not myDataReader.IsDBNull(0) Then
                    firstname1.Text = myDataReader.GetString(0)
                End If
                If Not myDataReader.IsDBNull(1) Then
                    firstname2.Text = myDataReader.GetString(1)
                End If
                If Not myDataReader.IsDBNull(2) Then
                    lastname1.Text = myDataReader.GetString(2)
                End If
                If Not myDataReader.IsDBNull(3) Then
                    lastname2.Text = myDataReader.GetString(3)
                End If
                If Not myDataReader.IsDBNull(4) Then
                    email1.Text = myDataReader.GetString(4)
                End If
                If Not myDataReader.IsDBNull(5) Then
                    email2.Text = myDataReader.GetString(5)
                End If
                If Not myDataReader.IsDBNull(6) Then
                    phone1.Text = myDataReader.GetString(6)
                End If
                If Not myDataReader.IsDBNull(7) Then
                    phone2.Text = myDataReader.GetString(7)
                End If
                If Not myDataReader.IsDBNull(8) Then
                    address.Text = myDataReader.GetString(8)
                End If
                If Not myDataReader.IsDBNull(9) Then
                    city.Text = myDataReader.GetString(9)
                End If
                If Not myDataReader.IsDBNull(10) Then
                    state.Text = myDataReader.GetString(10)
                End If
                If Not myDataReader.IsDBNull(11) Then
                    zip.Text = myDataReader.GetString(11)
                End If
                If Not myDataReader.IsDBNull(12) Then
                    allergies.Text = myDataReader.GetString(12)
                End If
                If Not myDataReader.IsDBNull(13) Then
                    BountyNL.Checked = myDataReader.GetBoolean(13)
                End If
                If Not myDataReader.IsDBNull(14) Then
                    BarnyardNL.Checked = myDataReader.GetBoolean(14)
                End If
                If Not myDataReader.IsDBNull(15) Then
                    PloughmanNL.Checked = myDataReader.GetBoolean(15)
                End If
                If Not myDataReader.IsDBNull(16) Then
                    Dim weeks As Integer = myDataReader.GetInt32(16)
                    weeksLiteral.Text = "You have used <b>" + weeks.ToString + "</b> of your vacation weeks."
                    If weeks = 2 Then
                        weeksLiteral.Text = "You have already used your <b>2</b> vacation weeks."
                        calpanel.Visible = False
                    End If
                End If
                If myDataReader.GetBoolean(17) = True Then
                    BoxButton.Items.FindByText("Bounty - $35.00").Selected = True
                    'For Each Weekrow As GridViewRow In GridView1.Rows
                    '    If Weekrow.Cells(0).Text = "Deposit" Then
                    '        Dim BountyPaid As CheckBox = TryCast(Weekrow.FindControl("BountyPaidCheck"), CheckBox)
                    '        BountyPaid.Checked = True
                    '        BountyPaid.Enabled = False
                    '    End If
                    'Next
                Else
                    CancelBounty.Visible = False
                    GridView1.Columns(2).Visible = False
                    'For Each Weekrow As GridViewRow In GridView1.Rows
                    '    If Weekrow.Cells(0).Text = "Deposit" Then
                    '        Dim BountyPaid As CheckBox = TryCast(Weekrow.FindControl("BountyPaidCheck"), CheckBox)
                    '        BountyPaid.Checked = False
                    '        BountyPaid.Enabled = False
                    '    End If
                    'Next
                End If
                If myDataReader.GetBoolean(18) = True Then
                    BoxButton.Items.FindByText("Barnyard - $35.00").Selected = True
                    'For Each Weekrow As GridViewRow In GridView1.Rows
                    '    If Weekrow.Cells(0).Text = "Deposit" Then
                    '        Dim BarnyardPaid As CheckBox = TryCast(Weekrow.FindControl("BarnyardPaidCheck"), CheckBox)
                    '        BarnyardPaid.Checked = True
                    '        BarnyardPaid.Enabled = False
                    '    End If
                    'Next
                Else
                    CancelBarnyard.Visible = False
                    GridView1.Columns(3).Visible = False
                    'For Each Weekrow As GridViewRow In GridView1.Rows
                    '    If Weekrow.Cells(0).Text = "Deposit" Then
                    '        Dim BarnyardPaid As CheckBox = TryCast(Weekrow.FindControl("BarnyardPaidCheck"), CheckBox)
                    '        BarnyardPaid.Checked = False
                    '        BarnyardPaid.Enabled = False
                    '    End If
                    'Next
                End If
                If myDataReader.GetBoolean(19) = True Then
                    BoxButton.Items.FindByText("Ploughman - $35.00").Selected = True
                    'For Each Weekrow As GridViewRow In GridView1.Rows
                    '    If Weekrow.Cells(0).Text = "Deposit" Then
                    '        Dim PloughmanPaid As CheckBox = TryCast(Weekrow.FindControl("PloughmanPaidCheck"), CheckBox)
                    '        PloughmanPaid.Checked = True
                    '        PloughmanPaid.Enabled = False
                    '    End If
                    'Next
                Else
                    CancelPloughman.Visible = False
                    GridView1.Columns(4).Visible = False
                    'For Each Weekrow As GridViewRow In GridView1.Rows
                    '    If Weekrow.Cells(0).Text = "Deposit" Then
                    '        Dim PloughmanPaid As CheckBox = TryCast(Weekrow.FindControl("PloughmanPaidCheck"), CheckBox)
                    '        PloughmanPaid.Checked = False
                    '        PloughmanPaid.Enabled = False
                    '    End If
                    'Next
                End If
                PickupDayList.SelectedValue = myDataReader.GetString(20)
                StoreList.SelectedValue = myDataReader.GetString(21)
            Loop
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
            oMail1.Body &= "Error in subscriber Default1: " + Membership.GetUser().ToString + "<br /><br />"
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
            Literal0.Text = "We're sorry, there seems to have been an error."
        Finally
            If (mySqlConnection.State = ConnectionState.Open) Then
                mySqlConnection.Close()
            End If
        End Try
    End Sub
    Protected Sub FillPaidInfo()
        Try
            Dim myDataReader2 As SqlDataReader
            Dim mySqlConnection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
            Dim mySqlCommand2 As SqlCommand
            Dim SDateRange As String = ""
            Dim query As String = "select Sstart, send from seasons where currents='true'"
            Using conn As New SqlConnection(ConnectionString)
                Using mySqlConnection2
                    mySqlCommand2 = New SqlCommand(query, mySqlConnection2)
                    mySqlConnection2.Open()
                    myDataReader2 = mySqlCommand2.ExecuteReader()
                    If myDataReader2.HasRows Then
                        Do While myDataReader2.Read()
                            SDateRange = " and week >= '" + myDataReader2.GetDateTime(0) + "' and week <= '" + myDataReader2.GetDateTime(1) + "' "
                        Loop
                    End If
                    myDataReader2.Close()
                End Using
            End Using
            Dim SqlQuary As String = "SELECT SubId, Week, PaidBounty, PaidBarnyard, PaidPloughman, bounty, barnyard, ploughman FROM Weekly where subID='" + SubId.ToString + "' and ((week='1/1/1900') or (week>='" + Date.Today.AddDays(-1) + "'" + SDateRange + ")) ORDER BY [Week]"
            Dim dt As New DataTable()
            dt.Columns.Add("SubId")
            dt.Columns.Add("Week")
            dt.Columns.Add("PaidBounty")
            dt.Columns.Add("PaidBarnyard")
            dt.Columns.Add("PaidPloughman")
            Dim myDataReader As SqlDataReader
            Dim mySqlConnection As SqlConnection
            Dim mySqlCommand As SqlCommand

            mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
            Using mySqlConnection
                mySqlCommand = New SqlCommand(SqlQuary, mySqlConnection)
                mySqlConnection.Open()
                myDataReader = mySqlCommand.ExecuteReader()
                If myDataReader.HasRows Then
                    Dim SubInfo As String = ""
                    Dim paid As String = ""
                    Dim pickedup As String = ""
                    Dim vacation As String = ""
                    Dim i As Integer = 0
                    Do While myDataReader.Read()
                        Dim week As String = ""
                        If myDataReader.GetDateTime(1).ToString.Contains("1900") Then
                            week = "Deposit"
                        Else
                            week = (myDataReader.GetDateTime(1).Month.ToString + "/" + myDataReader.GetDateTime(1).Day.ToString + "-" + myDataReader.GetDateTime(1).AddDays(1).Day.ToString + "/" + myDataReader.GetDateTime(1).Year.ToString)
                        End If
                        If Not myDataReader.IsDBNull(2) And Not myDataReader.IsDBNull(3) And Not myDataReader.IsDBNull(4) Then
                            dt.Rows.Add(myDataReader.GetInt32(0), week, myDataReader.GetBoolean(2), myDataReader.GetBoolean(3), myDataReader.GetBoolean(4))
                        Else

                        End If
                    Loop
                Else
                    Console.WriteLine("No rows found.")
                End If

                myDataReader.Close()
            End Using
            GridView1.DataSource = dt
            GridView1.DataBind()
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
            oMail1.Body &= "Error in subscriber Default2: " + Membership.GetUser().ToString + "<br /><br />"
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
            Literal0.Text = "We're sorry, there seems to have been an error."
        End Try

    End Sub
    Protected Sub FillDayInfo()
        Dim dt As New DataTable()
        dt.Columns.Add("PickupDay")
        dt.Rows.Add(" - Select a Pickup Day - ")
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
    Protected Sub FillWeekInfo()
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
                            SDateRange = " and week >= '" + myDataReader2.GetDateTime(0) + "' and week <= '" + myDataReader2.GetDateTime(1) + "' "
                        Loop
                    End If
                    myDataReader2.Close()
                End Using
            End Using
        Finally
        End Try
        Dim dt As New DataTable()
        dt.Columns.Add("Week")
        dt.Rows.Add(" - Select a Week - ")
        'Create Rows in DataTable
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        Try
            Using mySqlConnection
                mySqlCommand = New SqlCommand("SELECT DISTINCT Week FROM Weekly where subID='" + SubId.ToString + "' and ((week='1/1/1900') or (week>='" + Date.Today.AddDays(7) + "'" + SDateRange + ")) ORDER BY [Week]", mySqlConnection)
                mySqlConnection.Open()

                myDataReader = mySqlCommand.ExecuteReader()

                If myDataReader.HasRows Then
                    Do While myDataReader.Read()
                        If Not myDataReader.GetDateTime(0).Year.ToString = "1900" Then
                            dt.Rows.Add(myDataReader.GetDateTime(0).Month.ToString + "/" + myDataReader.GetDateTime(0).Day.ToString + "-" + myDataReader.GetDateTime(0).AddDays(1).Day.ToString + "/" + myDataReader.GetDateTime(0).Year.ToString)
                            If myDataReader.GetDateTime(0) = Date.Today Or myDataReader.GetDateTime(0) = Date.Today.AddDays(6) Then
                                WeekList.SelectedValue = myDataReader.GetDateTime(0).Month.ToString + "/" + myDataReader.GetDateTime(0).Day.ToString + "-" + myDataReader.GetDateTime(0).AddDays(1).Day.ToString + "/" + myDataReader.GetDateTime(0).Year.ToString
                            End If
                        End If
                    Loop
                Else
                    Console.WriteLine("No rows found.")
                End If
                myDataReader.Close()
            End Using
        Finally
        End Try
        Me.WeekList.DataSource = dt
        Me.WeekList.DataTextField = "Week"
        Me.WeekList.DataValueField = "Week"
        Me.WeekList.DataBind()
    End Sub
    Protected Sub FillVacInfo()
        Dim SqlQuary As String = "SELECT DISTINCT SubID, week FROM Weekly where subid='" + SubId.ToString + "' and vacation='true' ORDER BY week"
        Dim dt As New DataTable()
        dt.Columns.Add("SubID")
        dt.Columns.Add("week")
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        Try
            Using mySqlConnection
                mySqlCommand = New SqlCommand(SqlQuary, mySqlConnection)
                mySqlConnection.Open()
                myDataReader = mySqlCommand.ExecuteReader()
                If myDataReader.HasRows Then
                    Dim SubInfo As String = ""
                    Do While myDataReader.Read()
                        dt.Rows.Add(myDataReader.GetInt32(0), myDataReader.GetDateTime(1).Month.ToString + "/" + myDataReader.GetDateTime(1).Day.ToString + "-" + myDataReader.GetDateTime(1).AddDays(1).Day.ToString + "/" + myDataReader.GetDateTime(1).Year.ToString)
                    Loop
                Else
                    Console.WriteLine("No rows found.")
                End If
                myDataReader.Close()
            End Using
        Finally
        End Try
        GridView2.DataSource = dt
        GridView2.DataBind()
    End Sub
    Protected Sub OnCheckedChanged(sender As [Object], e As EventArgs)
        PriceUPanel.Update()
        Dim days As Integer = 0
        For Each Weekrow As GridViewRow In GridView1.Rows
            Dim BountyPaid As CheckBox = TryCast(Weekrow.FindControl("BountyPaidCheck"), CheckBox)
            If BountyPaid.Enabled = True And BountyPaid.Checked = True Then
                days += 1
            End If
            Dim BarnyardPaid As CheckBox = TryCast(Weekrow.FindControl("BarnyardPaidCheck"), CheckBox)
            If BarnyardPaid.Enabled = True And BarnyardPaid.Checked = True Then
                days += 1
            End If
            Dim PloughmanPaid As CheckBox = TryCast(Weekrow.FindControl("PloughmanPaidCheck"), CheckBox)
            If PloughmanPaid.Enabled = True And PloughmanPaid.Checked = True Then
                days += 1
            End If
        Next
        If days * 35 = 0 Then
            Price.Text = "$0.00"
        Else
            Price.Text = "" + (days * 35).ToString("C2")
        End If

    End Sub
    Protected Sub GridView2_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim row As GridViewRow = e.Row
            Try
                Dim Delete As Button = TryCast(e.Row.FindControl("deleteButton"), Button)
                Dim week As String = row.Cells(1).Text
                Dim pattern As String = "-(.*?)/"
                Dim replacement As String = "/" & vbCrLf
                Dim rgx As New Regex(pattern, RegexOptions.Singleline)
                week = rgx.Replace(week, replacement)
                week = (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "")
                If Not (DateTime.Parse(week) > Date.Today.AddDays(7)) = True Then
                    Delete.Visible = False
                End If
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
                oMail1.Body &= "Error in subscriber Default3: " + Membership.GetUser().ToString + "<br /><br />"
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

            End Try
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
                PickupDayList.SelectedValue = "Thursday"
                PUDLiteral.Text = "Pickups are only available on Thursday's at the Jefferson City Location"
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
        Try
            Dim query As String = "Update subscribers set FirstName1=@FirstName1, LastName1=@LastName1, Email1=@Email1, phone1=@phone1, FirstName2=@FirstName2, LastName2=@LastName2, Email2=@Email2, phone2=@phone2, Address=@Address, City=@City, State=@State, Zip=@Zip, BountyNL=@BountyNL, Barnyard=@BarnyardNL, PloughmanNL=@PloughmanNL Where username=@username"
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
                        .Parameters.Add("@BountyNL", SqlDbType.Bit).Value = BountyNL.Checked
                        .Parameters.Add("@BarnyardNL", SqlDbType.Bit).Value = BarnyardNL.Checked
                        .Parameters.Add("@PloughmanNL", SqlDbType.Bit).Value = PloughmanNL.Checked
                        .Parameters.Add("@username", SqlDbType.VarChar).Value = Membership.GetUser().ToString
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    Literal0.Text = "Account Updated"
                    UpdMailChimp(email1.Text, BountyNL.Checked, BarnyardNL.Checked, PloughmanNL.Checked)
                    If Not email2.Text = "" Then
                        UpdMailChimp(email2.Text, BountyNL.Checked, BarnyardNL.Checked, PloughmanNL.Checked)
                    End If
                End Using
            End Using
        Catch ex As SqlException
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
            oMail1.To.Add(New MailAddress("dbccemtp@gmail.com"))
            oMail1.Subject = "Root Cellar Error"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "Error in subscriber Default4: " + Membership.GetUser().ToString + "<br /><br />"
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
            Literal1.Text = "We're sorry, there seems to have been an error."
        End Try
    End Sub
    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            Dim query As String = "Update subscribers set Allergies=@Allergies, store=@store, pickupday=@pickupday Where username=@username"
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        comm.Parameters.Add("@Allergies", SqlDbType.VarChar).Value = allergies.Text
                        .Parameters.Add("@pickupday", SqlDbType.Text).Value = PickupDayList.SelectedValue
                        .Parameters.Add("@store", SqlDbType.Text).Value = StoreList.SelectedValue
                        .Parameters.Add("@username", SqlDbType.VarChar).Value = Membership.GetUser().ToString
                    End With

                    conn.Open()
                    comm.ExecuteNonQuery()
                    Literal0.Text = "Account Updated"
                End Using
            End Using
        Catch ex As SqlException
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
            oMail1.To.Add(New MailAddress("dbccemtp@gmail.com"))
            oMail1.Subject = "Root Cellar Error"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "Error in subscriber Default5: " + Membership.GetUser().ToString + "<br /><br />"
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
            Literal1.Text = "We're sorry, there seems to have been an error."
        End Try
    End Sub

    Protected Sub Submit_Click(sender As Object, e As EventArgs) Handles Submit.Click
        Try
            Dim myDataReader As SqlDataReader
            Dim mySqlConnection As SqlConnection
            Dim mySqlCommand As SqlCommand
            mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
            mySqlCommand = New SqlCommand("SELECT SubID FROM subscribers Where Username= '" + Membership.GetUser().ToString + "'", mySqlConnection)
            Try
                mySqlConnection.Open()
                myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                Do While (myDataReader.Read())
                    SubId = myDataReader.GetInt32(0)
                Loop
            Finally
                If (mySqlConnection.State = ConnectionState.Open) Then
                    mySqlConnection.Close()
                End If
            End Try

            Dim sql As String = ""
            Dim sql2 As String = ""
            mySqlCommand = New SqlCommand("SELECT vacused FROM subscribers Where SubID= '" + SubId.ToString + "'", mySqlConnection)
            If Not WeekList.SelectedValue = " - Select a Week - " Then
                Dim week As String
                week = WeekList.SelectedValue
                Dim pattern As String = "-(.*?)/"
                Dim replacement As String = "/" & vbCrLf
                Dim rgx As New Regex(pattern, RegexOptions.Singleline)
                week = rgx.Replace(week, replacement)
                week = (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "")
                Try
                    mySqlConnection.Open()
                    myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                    Do While (myDataReader.Read())
                        sql = "update subscribers set vacused='" + (myDataReader.GetInt32(0) + 1).ToString + "' where subid='" + SubId.ToString + "'"
                        sql2 = "update weekly set vacation='true' where subid='" + SubId.ToString + "' and week='" + week + "'"
                    Loop
                Finally
                    If (mySqlConnection.State = ConnectionState.Open) Then
                        mySqlConnection.Close()
                    End If
                End Try
                Using conn As New SqlConnection(ConnectionString)
                    Using comm As New SqlCommand()
                        With comm
                            .Connection = conn
                            .CommandType = CommandType.Text
                            .CommandText = sql
                        End With
                        conn.Open()
                        comm.ExecuteNonQuery()
                    End Using
                End Using
                Using conn As New SqlConnection(ConnectionString)
                    Using comm As New SqlCommand()
                        With comm
                            .Connection = conn
                            .CommandType = CommandType.Text
                            .CommandText = sql2
                        End With
                        conn.Open()
                        comm.ExecuteNonQuery()
                    End Using
                End Using
                Literal0.Text = "You have successfully scheduled " + WeekList.SelectedValue + " as a vacation week"
                WeekList.ClearSelection()
                FillInfo()
                FillVacInfo()
            Else
                Literal2.Text = "Please select a week"
            End If
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
            oMail1.Body &= "Error in subscriber Default6: " + Membership.GetUser().ToString + "<br /><br />"
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
            Literal2.Text = "We're Sorry, there was an error"
        End Try


    End Sub

    Protected Sub GridView2_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridView2.RowDeleting
        Dim weeksUsed As Integer = 0
        Dim week As String = GridView2.Rows(e.RowIndex).Cells(1).Text
        Dim pattern As String = "-(.*?)/"
        Dim replacement As String = "/" & vbCrLf
        Dim rgx As New Regex(pattern, RegexOptions.Singleline)
        week = rgx.Replace(week, replacement)
        week = (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "")
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        mySqlCommand = New SqlCommand("SELECT SubID, vacused FROM subscribers Where Username= '" + Membership.GetUser().ToString + "'", mySqlConnection)
        Try
            mySqlConnection.Open()
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Do While (myDataReader.Read())
                SubId = myDataReader.GetInt32(0)
                weeksUsed = myDataReader.GetInt32(1)
            Loop
        Finally
            If (mySqlConnection.State = ConnectionState.Open) Then
                mySqlConnection.Close()
            End If
        End Try
        Try
            Dim query As String = "Update weekly set vacation='false' Where SubID=@SubID and week=@week"
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        comm.Parameters.Add("@SubID", SqlDbType.Int).Value = SubId
                        .Parameters.Add("@week", SqlDbType.DateTime).Value = week
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()

                End Using
            End Using
        Catch ex As SqlException
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
            oMail1.To.Add(New MailAddress("dbccemtp@gmail.com"))
            oMail1.Subject = "Root Cellar Error"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "Error in subscriber Default7: " + Membership.GetUser().ToString + "<br /><br />"
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
            Literal0.Text = "We're sorry, there seems to have been an error."
        End Try
        If Not weeksUsed < 1 Then
            Try
                Dim query As String = "Update subscribers set vacused=@vacused Where username=@username"
                Using conn As New SqlConnection(ConnectionString)
                    Using comm As New SqlCommand()
                        With comm
                            .Connection = conn
                            .CommandType = CommandType.Text
                            .CommandText = query
                            comm.Parameters.Add("@vacused", SqlDbType.VarChar).Value = (weeksUsed - 1).ToString
                            .Parameters.Add("@username", SqlDbType.VarChar).Value = Membership.GetUser().ToString
                        End With
                        conn.Open()
                        comm.ExecuteNonQuery()
                        Literal0.Text = GridView2.Rows(e.RowIndex).Cells(1).Text + " has been removed as a vacation week."
                    End Using
                End Using
            Catch ex As SqlException
                Dim oMail1 As MailMessage = New MailMessage()
                oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
                oMail1.To.Add(New MailAddress("dbccemtp@gmail.com"))
                oMail1.Subject = "Root Cellar Error"
                oMail1.Priority = MailPriority.High
                oMail1.IsBodyHtml = True
                oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
                oMail1.Body &= "<head><title></title></head>"
                oMail1.Body &= "<body>"
                oMail1.Body &= "Error in subscriber Default8: " + Membership.GetUser().ToString + "<br /><br />"
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
                Literal0.Text = "We're sorry, there seems to have been an error."
            End Try
        End If
        calpanel.Visible = True
        FillInfo()
        FillVacInfo()
    End Sub

    Protected Sub CancelBounty_Click(sender As Object, e As EventArgs) Handles CancelBounty.Click
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        Dim active As Boolean = True
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        mySqlCommand = New SqlCommand("SELECT SubID, Barnyard, ploughman FROM subscribers Where Username= '" + Membership.GetUser().ToString + "'", mySqlConnection)
        Try
            mySqlConnection.Open()
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Do While (myDataReader.Read())
                SubId = myDataReader.GetInt32(0)
                If myDataReader.GetBoolean(1) = False And myDataReader.GetBoolean(2) = False Then
                    active = False
                End If
            Loop
        Finally
            If (mySqlConnection.State = ConnectionState.Open) Then
                mySqlConnection.Close()
            End If
        End Try
        Try
            Dim query As String = "Update weekly set bounty='false' Where SubID=@SubID "
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        comm.Parameters.Add("@SubID", SqlDbType.Int).Value = SubId
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()

                End Using
            End Using
        Catch ex As SqlException
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
            oMail1.To.Add(New MailAddress("dbccemtp@gmail.com"))
            oMail1.Subject = "Root Cellar Error"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "Error in subscriber Default9: " + Membership.GetUser().ToString + "<br /><br />"
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
            Literal0.Text = "We're sorry, there seems to have been an error."
        End Try
        Try
            Dim query As String = "Update subscribers set bounty='false', active=@active Where username=@username"
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        comm.Parameters.Add("@username", SqlDbType.VarChar).Value = Membership.GetUser().ToString
                        .Parameters.Add("@active", SqlDbType.VarChar).Value = active
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    Literal0.Text = "Your subscription has been cancelled."
                End Using
            End Using
        Catch ex As SqlException
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
            oMail1.To.Add(New MailAddress("dbccemtp@gmail.com"))
            oMail1.Subject = "Root Cellar Error"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "Error in subscriber Default10: " + Membership.GetUser().ToString + "<br /><br />"
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
            Literal0.Text = "We're sorry, there seems to have been an error."
        End Try
        FillInfo()
        FillPaidInfo()
        FillDayInfo()
        FillStoreInfo()
        FillWeekInfo()
        FillVacInfo()
        Price.Text = "$0.00"
    End Sub
    Protected Sub CancelBarnyard_Click(sender As Object, e As EventArgs) Handles CancelBarnyard.Click
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        Dim active As Boolean = True
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        mySqlCommand = New SqlCommand("SELECT SubID, Bounty, ploughman FROM subscribers Where Username= '" + Membership.GetUser().ToString + "'", mySqlConnection)
        Try
            mySqlConnection.Open()
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Do While (myDataReader.Read())
                SubId = myDataReader.GetInt32(0)
                If myDataReader.GetBoolean(1) = False And myDataReader.GetBoolean(2) = False Then
                    active = False
                End If
            Loop
        Finally
            If (mySqlConnection.State = ConnectionState.Open) Then
                mySqlConnection.Close()
            End If
        End Try
        Try
            Dim query As String = "Update weekly set barnyard='false' Where SubID=@SubID "
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        comm.Parameters.Add("@SubID", SqlDbType.Int).Value = SubId
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()

                End Using
            End Using
        Catch ex As SqlException
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
            oMail1.To.Add(New MailAddress("dbccemtp@gmail.com"))
            oMail1.Subject = "Root Cellar Error"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "Error in subscriber Default11: " + Membership.GetUser().ToString + "<br /><br />"
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
            Literal0.Text = "We're sorry, there seems to have been an error."
        End Try
        Try
            Dim query As String = "Update subscribers set barnyard='false', active=@active Where username=@username"
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        comm.Parameters.Add("@username", SqlDbType.VarChar).Value = Membership.GetUser().ToString
                        .Parameters.Add("@active", SqlDbType.VarChar).Value = active
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    Literal0.Text = "Your subscription has been cancelled."
                End Using
            End Using
        Catch ex As SqlException
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
            oMail1.To.Add(New MailAddress("dbccemtp@gmail.com"))
            oMail1.Subject = "Root Cellar Error"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "Error in subscriber Default12: " + Membership.GetUser().ToString + "<br /><br />"
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
            Literal0.Text = "We're sorry, there seems to have been an error."
        End Try
        FillInfo()
        FillPaidInfo()
        FillDayInfo()
        FillStoreInfo()
        FillWeekInfo()
        FillVacInfo()
        Price.Text = "$0.00"
    End Sub
    Protected Sub CancelPloughman_Click(sender As Object, e As EventArgs) Handles CancelPloughman.Click
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        Dim active As Boolean = True
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        mySqlCommand = New SqlCommand("SELECT SubID, Bounty, Barnyard FROM subscribers Where Username= '" + Membership.GetUser().ToString + "'", mySqlConnection)
        Try
            mySqlConnection.Open()
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Do While (myDataReader.Read())
                SubId = myDataReader.GetInt32(0)
                If myDataReader.GetBoolean(1) = False And myDataReader.GetBoolean(2) = False Then
                    active = False
                End If
            Loop
        Finally
            If (mySqlConnection.State = ConnectionState.Open) Then
                mySqlConnection.Close()
            End If
        End Try
        Try
            Dim query As String = "Update weekly set ploughman='false' Where SubID=@SubID "
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        comm.Parameters.Add("@SubID", SqlDbType.Int).Value = SubId
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()

                End Using
            End Using
        Catch ex As SqlException
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
            oMail1.To.Add(New MailAddress("dbccemtp@gmail.com"))
            oMail1.Subject = "Root Cellar Error"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "Error in subscriber Default13: " + Membership.GetUser().ToString + "<br /><br />"
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
            Literal0.Text = "We're sorry, there seems to have been an error."
        End Try
        Try
            Dim query As String = "Update subscribers set ploughman='false', active=@active Where username=@username"
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        comm.Parameters.Add("@username", SqlDbType.VarChar).Value = Membership.GetUser().ToString
                        .Parameters.Add("@active", SqlDbType.VarChar).Value = active
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    Literal0.Text = "Your subscription have been cancelled."
                End Using
            End Using
        Catch ex As SqlException
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
            oMail1.To.Add(New MailAddress("dbccemtp@gmail.com"))
            oMail1.Subject = "Root Cellar Error"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "Error in subscriber Default14: " + Membership.GetUser().ToString + "<br /><br />"
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
            Literal0.Text = "We're sorry, there seems to have been an error."
        End Try
        FillInfo()
        FillPaidInfo()
        FillDayInfo()
        FillStoreInfo()
        FillWeekInfo()
        FillVacInfo()
        Price.Text = "$0.00"
    End Sub
    Private Sub UpdMailChimp(email As String, Bounty As Boolean, Barnyard As Boolean, Ploughman As Boolean)
        Try
            Dim webAddr As String = ""
            If Bounty = True Then
                Try
                    webAddr += "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=0a27dd543a&email[email]=" + email1.Text.Trim + "&merge_vars[FNAME]=" + firstname1.Text.Trim + "&merge_vars[LNAME]=" + lastname1.Text.Trim + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim + "&double_optin=false&send_welcome=false"
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
                    webAddr += "https://us2.api.mailchimp.com/2.0/lists/unsubscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=0a27dd543a&email[email]=" + email1.Text.Trim + "&merge_vars[FNAME]=" + firstname1.Text.Trim + "&merge_vars[LNAME]=" + lastname1.Text.Trim + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim + "&double_optin=false&send_welcome=false"
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
                    webAddr += "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=2335ec6f51&email[email]=" + email1.Text.Trim + "&merge_vars[FNAME]=" + firstname1.Text.Trim + "&merge_vars[LNAME]=" + lastname1.Text.Trim + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim + "&double_optin=false&send_welcome=false"
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
                    webAddr += "https://us2.api.mailchimp.com/2.0/lists/unsubscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=2335ec6f51&email[email]=" + email1.Text.Trim + "&merge_vars[FNAME]=" + firstname1.Text.Trim + "&merge_vars[LNAME]=" + lastname1.Text.Trim + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim + "&double_optin=false&send_welcome=false"
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
                    webAddr += "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=4801343502&email[email]=" + email1.Text.Trim + "&merge_vars[FNAME]=" + firstname1.Text.Trim + "&merge_vars[LNAME]=" + lastname1.Text.Trim + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim + "&double_optin=false&send_welcome=false"
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
                    webAddr += "https://us2.api.mailchimp.com/2.0/lists/unsubscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=4801343502&email[email]=" + email1.Text.Trim + "&merge_vars[FNAME]=" + firstname1.Text.Trim + "&merge_vars[LNAME]=" + lastname1.Text.Trim + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim + "&double_optin=false&send_welcome=false"
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
            Dim oMail1 As MailMessage = New MailMessage()
            oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
            oMail1.To.Add(New MailAddress("dbccemtp@gmail.com"))
            oMail1.Subject = "Root Cellar Error"
            oMail1.Priority = MailPriority.High
            oMail1.IsBodyHtml = True
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
            oMail1.Body &= "<head><title></title></head>"
            oMail1.Body &= "<body>"
            oMail1.Body &= "Error in subscriber Default15: " + Membership.GetUser().ToString + "<br /><br />"
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
            'Literal0.Text = "We're sorry, there seems to have been an error."
        End Try

    End Sub

    Protected Sub Button2_Click1(sender As Object, e As EventArgs) Handles Button2.Click
        Dim val As String = ""
        Dim RedirURL As String = ""
        Dim webAddr As String = ""
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        PaymentsLiteral.Text = ""
        Dim query1 As String = "INSERT INTO TempOrders (Username, "
        Dim query2 As String = "VALUES ('" + Membership.GetUser().ToString + "', "
        Dim Oweek As String = ""
        Dim bounty As String = ""
        Dim barnyard As String = ""
        Dim ploughman As String = ""
        Try
            Dim Q As Integer = 0
            If isDev = False Then
                Dim CurrStore As String = ""
                mySqlCommand = New SqlCommand("SELECT store FROM Subscribers Where Username= '" + Membership.GetUser().ToString + "'", mySqlConnection)
                Try
                    mySqlConnection.Open()
                    myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                    Do While (myDataReader.Read())
                        If Not myDataReader.IsDBNull(0) Then
                            CurrStore = myDataReader.GetString(0)
                        End If
                    Loop
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
                    oMail1.Body &= "Error in subscriber Default: " + Membership.GetUser().ToString + "<br /><br />"
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
                    CurrStore = StoreList.SelectedValue
                End Try
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
            For Each Weekrow As GridViewRow In GridView1.Rows
                Dim week As String = Weekrow.Cells(1).Text
                If Not week = "Deposit" Then
                    Oweek = week
                    Dim pattern As String = "-(.*?)/"
                    Dim replacement As String = "/" & vbCrLf
                    Dim rgx As New Regex(pattern, RegexOptions.Singleline)
                    Oweek = rgx.Replace(Oweek, replacement)
                    Oweek = (DateTime.Parse(Oweek)).ToString.Replace(" 12:00:00 AM", "")
                Else
                    Oweek = "1/1/1900"
                End If
                Dim BountyPaid As CheckBox = TryCast(Weekrow.FindControl("BountyPaidCheck"), CheckBox)
                If BountyPaid.Enabled = True And BountyPaid.Checked = True Then
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
                    If week = "Deposit" Then
                        prodDetails += "'comment':'" + Membership.GetUser().ToString + "+Bounty+Deposit'"
                    Else
                        prodDetails += "'comment':'" + Membership.GetUser().ToString + "+Bounty+payment+for+" + week + "'"
                    End If
                    prodDetails += "}]}"
                    webAddr += prodDetails
                    query1 += "P" + Q.ToString + "Date, P" + Q.ToString + "Box, "
                    query2 += "'" + Oweek + "', 'Bounty', "
                End If
                Dim BarnyardPaid As CheckBox = TryCast(Weekrow.FindControl("BarnyardPaidCheck"), CheckBox)
                If BarnyardPaid.Enabled = True And BarnyardPaid.Checked = True Then
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
                    If week = "Deposit" Then
                        prodDetails += "'comment':'" + Membership.GetUser().ToString + "+Barnyard+Deposit'"
                    Else
                        prodDetails += "'comment':'" + Membership.GetUser().ToString + "+Barnyard+payment+for+" + week + "'"
                    End If
                    prodDetails += "}]}"
                    webAddr += prodDetails
                    query1 += "P" + Q.ToString + "Date, P" + Q.ToString + "Box, "
                    query2 += "'" + Oweek + "', 'Barnyard', "
                End If
                Dim PloughmanPaid As CheckBox = TryCast(Weekrow.FindControl("PloughmanPaidCheck"), CheckBox)
                If PloughmanPaid.Enabled = True And PloughmanPaid.Checked = True Then
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
                    If week = "Deposit" Then
                        prodDetails += "'comment':'" + Membership.GetUser().ToString + "+Ploughman+Deposit'"
                    Else
                        prodDetails += "'comment':'" + Membership.GetUser().ToString + "+Ploughman+payment+for+" + week + "'"
                    End If
                    prodDetails += "}]}"
                    webAddr += prodDetails
                    query1 += "P" + Q.ToString + "Date, P" + Q.ToString + "Box, "
                    query2 += "'" + Oweek + "', 'Ploughman', "
                End If

            Next
            webAddr += "]}"
            webAddr = HttpUtility.UrlEncode(webAddr)
            webAddr = webAddr.Replace("%27", "%22")
            webAddr = url + webAddr
            If Q > 0 Then
                'PaymentsLiteral.Text += "Request " + webAddr + "<br /><br />"
                Dim FwebAddr As New Uri(webAddr)
                Dim httpWebRequest = DirectCast(WebRequest.Create(FwebAddr), HttpWebRequest)
                httpWebRequest.ContentType = "application/json"
                Dim httpResponse = DirectCast(httpWebRequest.GetResponse(), HttpWebResponse)
                Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                    val = streamReader.ReadToEnd()
                    'PaymentsLiteral.Text += "Result " + val + "<br /><br />"
                    Dim pattern As String = "{(.*?)order_id"":"
                    Dim replacement As String = ""
                    Dim rgx As New Regex(pattern, RegexOptions.Singleline)
                    val = rgx.Replace(val, replacement)
                    pattern = ",(.*?)}}"
                    replacement = ""
                    Dim rgx2 As New Regex(pattern, RegexOptions.Singleline)
                    val = rgx2.Replace(val, replacement)
                    RedirURL = GetURL(val, StoreID)
                    'PaymentsLiteral.Text += "<br /><br />" + RedirURL + "<br /><br />"
                    RedirURL = Server.UrlEncode(RedirURL).Replace("%0d%0a", "").Replace("%3a", ":").Replace("%5c%2f", "/").Replace("%3f", "?").Replace("%3d", "=").Replace("%22", """").Replace("%2c", ",").Replace("%3a", ":")
                    'PaymentsLiteral.Text += "<br /><br />(" + RedirURL + ")" + "<br /><br />"
                    query1 += "OrderID) "
                    query2 += val + ")"
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
                Response.Redirect(RedirURL, False)
                Context.ApplicationInstance.CompleteRequest()
            Else
                PaymentsLiteral.Text = "<h2><span style='color: red;'>Please select at least one box/week.</span></h2>"
            End If
            PriceUPanel.Update()
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
            oMail1.Body &= "Error in subscriber Default16: " + Membership.GetUser().ToString + "<br /><br />"
            oMail1.Body &= ex.Message + "<br /><br />" + ex.StackTrace + "<br /><br />"
            Try
                oMail1.Body &= "WebAddr:" + webAddr + "<br /><br />"
                oMail1.Body &= "GetURL:" + GetURL(val, StoreID) + "<br /><br />"
                oMail1.Body &= "RedirURL:" + RedirURL + "<br /><br />"
            Catch ex2 As Exception
                oMail1.Body &= "EX2:" + ex.Message + "<br /><br />" + ex.StackTrace + "<br /><br />"
            End Try
            
            oMail1.Body &= "</body>"
            oMail1.Body &= "</html>"
            Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
            oMail1.AlternateViews.Add(htmlView2)
            Dim smtpmail2 As New SmtpClient("relay-hosting.secureserver.net")
            smtpmail2.EnableSsl = False
            smtpmail2.UseDefaultCredentials = True
            smtpmail2.Send(oMail1)
            oMail1 = Nothing
            PaymentsLiteral.Text = "We're sorry, there seems to have been an error."
        End Try
    End Sub
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
            'PaymentsLiteral.Text += "Request " + webAddr + "<br /><br />"
            Dim FwebAddr As New Uri(webAddr)
            Dim httpWebRequest = DirectCast(WebRequest.Create(FwebAddr), HttpWebRequest)
            httpWebRequest.ContentType = "application/json"
            Dim httpResponse = DirectCast(httpWebRequest.GetResponse(), HttpWebResponse)
            Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                Dim val = streamReader.ReadToEnd()
                'PaymentsLiteral.Text += "Result " + val + "<br /><br />"
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
            PriceUPanel.Update()
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
            oMail1.Body &= "Error in subscriber Default17: " + Membership.GetUser().ToString + "<br /><br />"
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
            PaymentsLiteral.Text = "We're sorry, there seems to have been an error."
            Return ""
        End Try
        Return ""
    End Function

    Protected Sub RadTabStrip1_TabClick(sender As Object, e As RadTabStripEventArgs) Handles RadTabStrip1.TabClick
        PaymentsLiteral.Text = ""
        Literal0.Text = ""
        MailPanel1.Update()
    End Sub

    Protected Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try

            If Not NewPassBox1.Text = "" Then
                If Not NewPassBox2.Text = "" Then
                    If NewPassBox1.Text.Trim = NewPassBox2.Text.Trim Then
                        If NewPassBox1.Text.Length > 5 Then
                            Dim myObject As MembershipUser = Membership.GetUser()
                            Dim generatedpassword As String = myObject.ResetPassword()
                            myObject.IsApproved = True
                            myObject.ChangePassword(generatedpassword, NewPassBox1.Text.Trim)
                            PassLiteral.Text = "<span style='color:green;'>Your password has been changed!</span>"
                        Else
                            PassLiteral.Text = "<span style='color:red;'>Your password must be at least 6 characters.</span>"
                        End If
                    Else
                        PassLiteral.Text = "<span style='color:red;'>Your passwords do not match.</span>"
                    End If
                    Else
                    PassLiteral.Text = "<span style='color:red;'>Please confirm your new password.</span>"
                    End If
            Else
                PassLiteral.Text = "<span style='color:red;'>Please enter a new password.</span>"
            End If
        Catch ex As Exception
            PassLiteral.Text = "We're sorry, there was a problem resetting your password."
        End Try
    End Sub
End Class
