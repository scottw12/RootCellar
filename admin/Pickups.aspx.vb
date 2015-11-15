Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Net.Mail

Partial Class admin_Pickups
    Inherits System.Web.UI.Page

    Private conn As SqlConnection = Nothing
    Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
    Private cmd As SqlCommand = Nothing
    Dim Options As String = ""
    Dim ThursdayPickup As Date
    Dim FridayPickup As Date


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            FillWeekInfo()
            FillStoreInfo()
            FillDayInfo()
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

        End If

    End Sub
    Protected Sub FillInfo()
        If Not WeekList.SelectedValue = " - Select a Week - " Then
            Dim week As String = WeekList.SelectedValue
            Dim pattern As String = "-(.*?)/"
            Dim replacement As String = "/" & vbCrLf
            Dim rgx As New Regex(pattern, RegexOptions.Singleline)
            week = rgx.Replace(week, replacement)
            week = (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "")
            Dim SqlQuary As String = "SELECT DISTINCT subscribers.firstname1, subscribers.lastname1, subscribers.allergies, weekly.bounty, weekly.barnyard, weekly.ploughman, weekly.paidBounty, weekly.paidBarnyard, weekly.paidPloughman, weekly.pickedup, phone1, subscribers.SubId, weekly.vacation FROM Weekly INNER JOIN subscribers ON weekly.SubID=subscribers.SubId where week='" + week + "'" + Options + "and subscribers.active='true' ORDER BY subscribers.LastName1, subscribers.FirstName1"
            Dim dt As New DataTable()
            dt.Columns.Add("SubID")
            dt.Columns.Add("FirstName1")
            dt.Columns.Add("LastName1")
            dt.Columns.Add("allergies")
            dt.Columns.Add("bounty")
            dt.Columns.Add("barnyard")
            dt.Columns.Add("ploughman")
            dt.Columns.Add("paidBounty")
            dt.Columns.Add("paidBarnyard")
            dt.Columns.Add("paidPloughman")
            dt.Columns.Add("pickedup")
            dt.Columns.Add("phone")
            dt.Columns.Add("bountyvac")
            dt.Columns.Add("barnyardvac")
            dt.Columns.Add("ploughmanvac")

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
                        Dim Bounty As Boolean
                        Dim Barnyard As Boolean
                        Dim Ploughman As Boolean
                        Dim paidBounty As Boolean
                        Dim paidBarnyard As Boolean
                        Dim paidPloughman As Boolean
                        Dim pickedup As Boolean
                        Dim Vacation As Boolean
                        Dim Bountyvac As Boolean
                        Dim Barnyardvac As Boolean
                        Dim Ploughmanvac As Boolean
                        Do While myDataReader.Read()
                            'If myDataReader.GetBoolean(6) = True Then
                            '    paidBounty = "~/images/apptrue.gif"
                            'Else
                            '    paidBounty = "~/images/appfalse.gif"
                            'End If
                            Bounty = myDataReader.GetBoolean(3)
                            Barnyard = myDataReader.GetBoolean(4)
                            Ploughman = myDataReader.GetBoolean(5)
                            Vacation = myDataReader.GetBoolean(12)
                            If Bounty = True And Vacation = True Then
                                Bountyvac = True
                            Else
                                Bountyvac = False
                            End If
                            If Barnyard = True And Vacation = True Then
                                Barnyardvac = True
                            Else
                                Barnyardvac = False
                            End If
                            If Ploughman = True And Vacation = True Then
                                Ploughmanvac = True
                            Else
                                Ploughmanvac = False
                            End If
                            If myDataReader.GetBoolean(6) = True Then
                                paidBounty = False
                            ElseIf myDataReader.GetBoolean(6) = False And Bounty = True Then
                                paidBounty = True
                            Else
                                paidBounty = False
                            End If
                            If myDataReader.GetBoolean(7) = True Then
                                paidBarnyard = False
                            ElseIf myDataReader.GetBoolean(7) = False And Barnyard = True Then
                                paidBarnyard = True
                            Else
                                paidBarnyard = False
                            End If
                            If myDataReader.GetBoolean(8) = True Then
                                paidPloughman = False
                            ElseIf myDataReader.GetBoolean(8) = False And Ploughman = True Then
                                paidPloughman = True
                            Else
                                paidPloughman = False
                            End If
                            If myDataReader.GetBoolean(9) = True Then
                                pickedup = False
                            Else
                                pickedup = True
                            End If
                            If Vacation = True Then
                                Bounty = False
                                Barnyard = False
                                Ploughman = False
                                pickedup = False
                            End If
                            dt.Rows.Add(myDataReader.GetInt32(11), myDataReader.GetString(0), myDataReader.GetString(1), myDataReader.GetString(2), Bounty, Barnyard, Ploughman, paidBounty, paidBarnyard, paidPloughman, pickedup, myDataReader.GetString(10), Bountyvac, Barnyardvac, Ploughmanvac)
                        Loop
                    Else
                        Console.WriteLine("No rows found.")
                    End If
                    myDataReader.Close()
                End Using
            Finally
            End Try
            GridView1.DataSource = dt
            GridView1.DataBind()
        End If
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
                            SDateRange = "where week>='" + myDataReader2.GetDateTime(0) + "' and week <= '" + myDataReader2.GetDateTime(1) + "' "
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
                mySqlCommand = New SqlCommand("SELECT DISTINCT Week FROM weekly " + SDateRange + " order by week", mySqlConnection)
                mySqlConnection.Open()

                myDataReader = mySqlCommand.ExecuteReader()

                If myDataReader.HasRows Then
                    Do While myDataReader.Read()
                        If Not myDataReader.GetDateTime(0).Year = "1900" Then
                            dt.Rows.Add(myDataReader.GetDateTime(0).Month.ToString + "/" + myDataReader.GetDateTime(0).Day.ToString + "-" + myDataReader.GetDateTime(0).AddDays(1).Day.ToString + "/" + myDataReader.GetDateTime(0).Year.ToString)
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
        Dim SelDate As DateTime = Date.Now
        If Date.Now.DayOfWeek = DayOfWeek.Sunday Then
            SelDate = SelDate.AddDays(4)
        ElseIf Date.Now.DayOfWeek = DayOfWeek.Monday Then
            SelDate = SelDate.AddDays(3)
        ElseIf Date.Now.DayOfWeek = DayOfWeek.Tuesday Then
            SelDate = SelDate.AddDays(2)
        ElseIf Date.Now.DayOfWeek = DayOfWeek.Wednesday Then
            SelDate = SelDate.AddDays(1)
        ElseIf Date.Now.DayOfWeek = DayOfWeek.Thursday Then
        ElseIf Date.Now.DayOfWeek = DayOfWeek.Friday Then
            SelDate = SelDate.AddDays(-1)
        ElseIf Date.Now.DayOfWeek = DayOfWeek.Saturday Then
            SelDate = SelDate.AddDays(-2)
        End If
        WeekList.SelectedValue = SelDate.Month.ToString + "/" + SelDate.Day.ToString + "-" + SelDate.AddDays(1).Day.ToString + "/" + SelDate.Year.ToString
        If Not WeekList.SelectedValue = " - Select a Week - " Then
            FillInfo()
        End If
    End Sub
    Protected Sub FillStoreInfo()
        Dim dt As New DataTable()
        dt.Columns.Add("Store")
        dt.Rows.Add(" - Select a Store - ")
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
    Protected Sub WeekList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles WeekList.SelectedIndexChanged
        If Not WeekList.SelectedValue = " - Select a Week - " Then
            Literal1.Text = ""
            GridView1.Visible = True
            FillInfo()
        Else
            Literal1.Text = "<h2>Please select a week</h2>"
            GridView1.Visible = False
        End If
        If Not WeekList.SelectedValue = " - Select a Week - " And Not StoreList.SelectedValue = " - Select a Store - " And Not PickupDayList.SelectedValue = " - Select a Pickup Day - " Then
            ReminderButton.Visible = True
        Else
            ReminderButton.Visible = False
        End If
    End Sub
    Protected Sub StoreList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles StoreList.SelectedIndexChanged
        If Not StoreList.SelectedValue = " - Select a Store - " Then
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
            If Not PickupDayList.SelectedValue = " - Select a Pickup Day - " Then
                Options = "and (weekly.location = '" + StoreList.SelectedValue + "') and (weekly.PickupDay = '" + PickupDayList.SelectedValue + "')"
            Else
                Options = "and (weekly.location = '" + StoreList.SelectedValue + "')"
            End If
        ElseIf Not PickupDayList.SelectedValue = " - Select a Pickup Day - " Then
            Options = "and (weekly.PickupDay = '" + PickupDayList.SelectedValue + "')"
        End If
        If NPUCheck.Checked = True Then
            Options += " and pickedup='false' and vacation='false'"
        End If
        If Not WeekList.SelectedValue = " - Select a Week - " And Not StoreList.SelectedValue = " - Select a Store - " And Not PickupDayList.SelectedValue = " - Select a Pickup Day - " Then
            ReminderButton.Visible = True
        Else
            ReminderButton.Visible = False
        End If
        FillInfo()
    End Sub

    Protected Sub PickupDay_SelectedIndexChanged(sender As Object, e As EventArgs) Handles PickupDayList.SelectedIndexChanged
        If Not PickupDayList.SelectedValue = " - Select a Pickup Day - " Then
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
            If Not StoreList.SelectedValue = " - Select a Store - " Then
                Options = "and (weekly.PickupDay = '" + PickupDayList.SelectedValue + "') and (weekly.location = '" + StoreList.SelectedValue + "')"
            Else
                Options = "and (weekly.PickupDay = '" + PickupDayList.SelectedValue + "')"
            End If
        ElseIf Not StoreList.SelectedValue = " - Select a Store - " Then
            Options = "and (weekly.location = '" + StoreList.SelectedValue + "')"
        End If
        If NPUCheck.Checked = True Then
            Options += " and pickedup='false' and vacation='false'"
        End If
        If Not WeekList.SelectedValue = " - Select a Week - " And Not StoreList.SelectedValue = " - Select a Store - " And Not PickupDayList.SelectedValue = " - Select a Pickup Day - " Then
            ReminderButton.Visible = True
        Else
            ReminderButton.Visible = False
        End If
        FillInfo()
    End Sub
    Protected Sub GetPickupDay()
        If Date.Today.ToString("dddd") = "Sunday" Then
            ThursdayPickup = Date.Today.AddDays(4)
            FridayPickup = Date.Today.AddDays(5)
        ElseIf Date.Today.ToString("dddd") = "Monday" Then
            ThursdayPickup = Date.Today.AddDays(3)
            FridayPickup = Date.Today.AddDays(4)
        ElseIf Date.Today.ToString("dddd") = "Tuesday" Then
            ThursdayPickup = Date.Today.AddDays(2)
            FridayPickup = Date.Today.AddDays(3)
        ElseIf Date.Today.ToString("dddd") = "Wednesday" Then
            ThursdayPickup = Date.Today.AddDays(1)
            FridayPickup = Date.Today.AddDays(2)
        ElseIf Date.Today.ToString("dddd") = "Thursday" Then
            ThursdayPickup = Date.Today
            FridayPickup = Date.Today.AddDays(1)
        ElseIf Date.Today.ToString("dddd") = "Friday" Then
            ThursdayPickup = Date.Today.AddDays(6)
            FridayPickup = Date.Today
        ElseIf Date.Today.ToString("dddd") = "Saturday" Then
            ThursdayPickup = Date.Today.AddDays(5)
            FridayPickup = Date.Today.AddDays(6)
        End If
    End Sub

    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim BountySub As Literal = TryCast(e.Row.FindControl("BountySub"), Literal)
            Dim BarnyardSub As Literal = TryCast(e.Row.FindControl("BarnyardSub"), Literal)
            Dim PloughmanSub As Literal = TryCast(e.Row.FindControl("PloughmanSub"), Literal)
            Dim BountyVac As Literal = TryCast(e.Row.FindControl("BountyVac"), Literal)
            Dim BarnyardVac As Literal = TryCast(e.Row.FindControl("BarnyardVac"), Literal)
            Dim PloughmanVac As Literal = TryCast(e.Row.FindControl("PloughmanVac"), Literal)
            Dim BountypayButton As ImageButton = TryCast(e.Row.FindControl("btnpayBounty"), ImageButton)
            Dim BarnyardpayButton As ImageButton = TryCast(e.Row.FindControl("btnpayBarnyard"), ImageButton)
            Dim PloughmanpayButton As ImageButton = TryCast(e.Row.FindControl("btnpayPloughman"), ImageButton)
            Dim Pickup As ImageButton = TryCast(e.Row.FindControl("pickup"), ImageButton)
            For Each cell As TableCell In e.Row.Cells
                cell.BackColor = Color.White
                If BountySub.Visible = True And BountypayButton.Visible = False Then
                    cell.BackColor = Color.Yellow
                End If
                If BarnyardSub.Visible = True And BarnyardpayButton.Visible = False Then
                    cell.BackColor = Color.Yellow
                End If
                If PloughmanSub.Visible = True And PloughmanpayButton.Visible = False Then
                    cell.BackColor = Color.Yellow
                End If
                If Pickup.Visible = False Then
                    cell.BackColor = Color.Green
                    Pickup.Visible = False
                Else
                    Pickup.Visible = True
                End If
                If BountyVac.Visible = True Or BarnyardVac.Visible = True Or PloughmanVac.Visible = True Then
                    cell.BackColor = Color.Red
                    Pickup.Visible = False
                    BountypayButton.Visible = False
                    BarnyardpayButton.Visible = False
                    PloughmanpayButton.Visible = False
                End If
            Next
        End If
    End Sub
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
        Dim SI As String = GridView1.DataKeys(row.RowIndex).Value
        Try
            If e.CommandName = "PayBounty" Then
                Response.Redirect("pay?s=" + SI + "&B=Bounty")
            ElseIf e.CommandName = "PayBarnyard" Then
                Response.Redirect("pay?s=" + SI + "&B=Barnyard")
            ElseIf e.CommandName = "PayPloughman" Then
                Response.Redirect("pay?s=" + SI + "&B=Ploughman")
            ElseIf e.CommandName = "Pickup" Then
                Response.Redirect("pickup?s=" + SI)
            End If
        Catch ex As Exception
        End Try
    End Sub


    Protected Sub NPUCheck_CheckedChanged(sender As Object, e As EventArgs) Handles NPUCheck.CheckedChanged
        If NPUCheck.Checked = True Then
            If Not StoreList.SelectedValue = " - Select a Store - " Then
                If Not PickupDayList.SelectedValue = " - Select a Pickup Day - " Then
                    Options = "and (weekly.location = '" + StoreList.SelectedValue + "') and (weekly.PickupDay = '" + PickupDayList.SelectedValue + "') and pickedup='false' and vacation='false'"
                Else
                    Options = "and (weekly.location = '" + StoreList.SelectedValue + "') and pickedup='false' and vacation='false'"
                End If
            ElseIf Not PickupDayList.SelectedValue = " - Select a Pickup Day - " Then
                Options = "and (weekly.PickupDay = '" + PickupDayList.SelectedValue + "') and pickedup='false' and vacation='false'"
            Else
                Options = " and pickedup='false' and vacation='false'"
            End If
        Else
            If Not StoreList.SelectedValue = " - Select a Store - " Then
                If Not PickupDayList.SelectedValue = " - Select a Pickup Day - " Then
                    Options = "and (weekly.location = '" + StoreList.SelectedValue + "') and (weekly.PickupDay = '" + PickupDayList.SelectedValue + "')"
                Else
                    Options = "and (weekly.location = '" + StoreList.SelectedValue + "')"
                End If
            ElseIf Not PickupDayList.SelectedValue = " - Select a Pickup Day - " Then
                Options = "and (weekly.PickupDay = '" + PickupDayList.SelectedValue + "')"
            Else
                Options = ""
            End If
        End If

        FillInfo()
    End Sub

    Protected Sub ReminderButton_Click(sender As Object, e As EventArgs) Handles ReminderButton.Click
        Try
            Dim week As String = WeekList.SelectedValue
            Dim pattern As String = "-(.*?)/"
            Dim replacement As String = "/" & vbCrLf
            Dim rgx As New Regex(pattern, RegexOptions.Singleline)
            week = rgx.Replace(week, replacement)
            week = (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "")
            Dim SqlQuary As String = "SELECT DISTINCT subscribers.firstname1, subscribers.email1, subscribers.SubId FROM Weekly INNER JOIN subscribers ON weekly.SubID=subscribers.SubId where week='" + week + "' and (weekly.location = '" + StoreList.SelectedValue + "') and (weekly.PickupDay = '" + PickupDayList.SelectedValue + "') and active='true' and vacation='false' and pickedup='false' ORDER BY subscribers.FirstName1"
            Dim dt As New DataTable()
            dt.Columns.Add("SubID")
            dt.Columns.Add("FirstName1")
            dt.Columns.Add("email")
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
                    Dim pickedup As String = ""
                    Do While myDataReader.Read()
                        dt.Rows.Add(myDataReader.GetInt32(2), myDataReader.GetString(0), myDataReader.GetString(1))
                    Loop
                Else
                    Console.WriteLine("No rows found.")
                End If
                myDataReader.Close()
            End Using
            Dim i As Integer = 0
            For Each row As DataRow In dt.Rows
                Try
                    Dim oMail0 As MailMessage = New MailMessage()
                    oMail0.From = New MailAddress("Root Cellar <website@rdollc.com>")
                    oMail0.To.Add(New MailAddress(row("email").Text.Replace("'", "").Replace("""", "").Replace(" ", "")))
                    oMail0.Subject = "Root Cellar Subscription "
                    oMail0.Priority = MailPriority.High
                    oMail0.IsBodyHtml = True
                    oMail0.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
                    oMail0.Body &= "<head><title></title></head>"
                    oMail0.Body &= "<body>"
                    oMail0.Body &= "Hello " + row("FirstName1") + ",<br /><br />"
                    oMail0.Body &= "This is a friendly reminder that you forgot to pick up your box today. We will be open tomorrow, please come pick up your box at your earliest convenience"
                    oMail0.Body &= "Thank you!<br /> Root Cellar!"
                    oMail0.Body &= "</body>"
                    oMail0.Body &= "</html>"
                    Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail0.Body, Nothing, "text/html")
                    oMail0.AlternateViews.Add(htmlView2)
                    Dim smtpmail2 As New SmtpClient("relay-hosting.secureserver.net")
                    smtpmail2.EnableSsl = False
                    smtpmail2.UseDefaultCredentials = True
                    smtpmail2.Send(oMail0)
                    oMail0 = Nothing
                    i += 1
                Catch ex As Exception
                End Try
            Next row
            Literal1.Text = "<h2>" + i.ToString + " reminder emails sent</h2>"
        Catch ex As Exception
            Literal1.Text = "We're sorry, there seems to have been an error sending the reminder email"
        End Try
    End Sub
End Class
