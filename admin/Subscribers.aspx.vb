Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing

Partial Class admin_Subscribers
    Inherits System.Web.UI.Page
    Dim options As String = ""
    Dim active As String = "active='true'"
    Dim Weeksdt As New DataTable()
    Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
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
                        Session("Role") = "Admin"
                    ElseIf role = "Employee" Then
                        Session("Role") = "Employee"
                    Else
                        Response.Redirect("~/account/")
                    End If
                Loop
            Finally
                If (mySqlConnection.State = ConnectionState.Open) Then
                    mySqlConnection.Close()
                End If
            End Try
            FillWeekInfo()
            FillDayInfo()
            FillStoreInfo()
            FillInfo()

        End If

    End Sub
    Protected Sub FillInfo()
        Dim week As String
        If Not WeekList.SelectedValue = " - Select a Week - " Then
            week = WeekList.SelectedValue
            Dim pattern As String = "-(.*?)/"
            Dim replacement As String = "/" & vbCrLf
            Dim rgx As New Regex(pattern, RegexOptions.Singleline)
            week = rgx.Replace(week, replacement)
            If Not options = "" Then
                week = "and weekly.week='" + (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "") + "' and "
            Else
                week = "and weekly.week='" + (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "") + "'"
            End If

        Else
            If Not options = "" Then
                week = "and "
            Else
                week = ""
            End If
        End If
        'Dim SqlQuary As String = "SELECT DISTINCT subscribers.SubId, subscribers.firstname1, subscribers.lastname1, subscribers.firstname2, subscribers.lastname2, Weekly.paidbounty, Weekly.paidbarnyard, Weekly.paidploughman, Weekly.pickedup, Weekly.vacation, weekly.bounty, weekly.barnyard, weekly.ploughman FROM Weekly INNER JOIN subscribers ON weekly.SubID=subscribers.SubId where active='true' " + week + options + " ORDER BY LastName1, FirstName1"
        Dim SqlQuary As String = "SELECT DISTINCT subscribers.SubId, subscribers.firstname1, subscribers.lastname1, subscribers.firstname2, subscribers.lastname2 FROM Weekly INNER JOIN subscribers ON weekly.SubID=subscribers.SubId where " + active + " " + week + options + " ORDER BY LastName1, FirstName1"
        Dim dt As New DataTable()
        dt.Columns.Add("SubId")
        dt.Columns.Add("FirstName1")
        dt.Columns.Add("LastName1")
        dt.Columns.Add("FirstName2")
        dt.Columns.Add("LastName2")
        'dt.Columns.Add("paidbounty")
        'dt.Columns.Add("paidbarnyard")
        'dt.Columns.Add("paidploughman")
        'dt.Columns.Add("pickedup")
        'dt.Columns.Add("vacation")
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
                    Dim paid As String = ""
                    Dim pickedup As String = ""
                    Dim vacation As String = ""
                    Dim i As Integer = 0
                    Dim j As Integer = 0
                    Do While myDataReader.Read()
                        'If myDataReader.GetBoolean(10) = True Then
                        '    i += 1
                        'End If
                        'If myDataReader.GetBoolean(11) = True Then
                        '    i += 1
                        'End If
                        'If myDataReader.GetBoolean(12) = True Then
                        '    i += 1
                        'End If
                        'If myDataReader.GetBoolean(5) = True Then
                        '    j += 1
                        'End If
                        'If myDataReader.GetBoolean(6) = True Then
                        '    j += 1
                        'End If
                        'If myDataReader.GetBoolean(7) = True Then
                        '    j += 1
                        'End If
                        'If i = j Then
                        '    paid = ""
                        'Else
                        '    paid = "~/images/Pay.png"
                        'End If
                        'If myDataReader.GetBoolean(8) = True Then
                        '    pickedup = ""
                        'Else
                        '    pickedup = "~/images/Pickup.png"
                        'End If
                        'If myDataReader.GetBoolean(9) = True Then
                        '    vacation = ""
                        'Else
                        '    vacation = "~/images/Vacation.png"
                        'End If
                        'dt.Rows.Add(myDataReader.GetInt32(0), myDataReader.GetString(1), myDataReader.GetString(2), myDataReader.GetString(3), myDataReader.GetString(4), paid, paid, paid, pickedup, vacation)
                        dt.Rows.Add(myDataReader.GetInt32(0), myDataReader.GetString(1), myDataReader.GetString(2), myDataReader.GetString(3), myDataReader.GetString(4))
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
        tableUPanel.Update()
    End Sub
    Protected Sub FillWeekInfo()
        Weeksdt.Columns.Add("Week")
        Weeksdt.Rows.Add(" - Select a Week - ")
        Dim myDataReader2 As SqlDataReader
        Dim mySqlConnection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        Dim mySqlCommand2 As SqlCommand
        Dim SDateRange As String = ""
        Dim query As String = "select Sstart, send from seasons order by sstart"
        Try
            Using conn As New SqlConnection(ConnectionString)
                Using mySqlConnection2
                    mySqlCommand2 = New SqlCommand(query, mySqlConnection2)
                    mySqlConnection2.Open()
                    myDataReader2 = mySqlCommand2.ExecuteReader()
                    Dim i As Integer = 0
                    If myDataReader2.HasRows Then
                        Do While myDataReader2.Read()
                                If Not i = 0 Then
                                    SDateRange += " or "
                                End If
                                SDateRange += "(week >= '" + myDataReader2.GetDateTime(0) + "' and week <= '" + myDataReader2.GetDateTime(1) + "')"
                            i += 1
                        Loop
                        FillWeekInfo2(SDateRange)
                    End If
                    myDataReader2.Close()
                End Using
            End Using
        Finally
        End Try
        Me.WeekList.DataSource = Weeksdt
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
    End Sub
    Sub FillWeekInfo2(daterange As String)
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        Try
            Using mySqlConnection
                mySqlCommand = New SqlCommand("SELECT DISTINCT Week FROM Weekly where " + daterange + " ORDER BY [Week]", mySqlConnection)
                mySqlConnection.Open()

                myDataReader = mySqlCommand.ExecuteReader()

                If myDataReader.HasRows Then
                    Do While myDataReader.Read()
                        If Not myDataReader.GetDateTime(0).Year = "1900" Then
                            Weeksdt.Rows.Add(myDataReader.GetDateTime(0).Month.ToString + "/" + myDataReader.GetDateTime(0).Day.ToString + "-" + myDataReader.GetDateTime(0).AddDays(1).Day.ToString + "/" + myDataReader.GetDateTime(0).Year.ToString)
                        End If
                    Loop
                Else
                    Console.WriteLine("No rows found.")
                End If
                myDataReader.Close()
            End Using
        Finally
        End Try
        Return


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
    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ID As String = GridView1.DataKeys(e.Row.RowIndex).Values(0).ToString()
            Dim myDataReader As SqlDataReader
            Dim mySqlConnection As SqlConnection
            Dim mySqlCommand As SqlCommand
            Dim week As String = ""
            If Not WeekList.SelectedValue = " - Select a Week - " Then
                week = WeekList.SelectedValue
                Dim pattern As String = "-(.*?)/"
                Dim replacement As String = "/" & vbCrLf
                Dim rgx As New Regex(pattern, RegexOptions.Singleline)
                week = rgx.Replace(week, replacement)
                week = "and week='" + (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "") + "'"
            End If
            mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
            mySqlCommand = New SqlCommand("SELECT weekly.paid, weekly.pickedup, weekly.vacation, subscribers.vacUsed FROM Weekly INNER JOIN subscribers ON weekly.SubID=subscribers.SubId Where subscribers.SubId= '" + ID + "' " + week + "", mySqlConnection)
            Dim payButton As ImageButton = TryCast(e.Row.FindControl("btnpay"), ImageButton)
            Dim pickupButton As ImageButton = TryCast(e.Row.FindControl("pickedup"), ImageButton)
            Dim vacationButton As ImageButton = TryCast(e.Row.FindControl("btnvac"), ImageButton)
            Dim ForcevacationButton As Button = TryCast(e.Row.FindControl("ForceVac"), Button)
            Dim isVacation As Boolean = False
            Try
                mySqlConnection.Open()
                myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                Do While (myDataReader.Read())
                    'If myDataReader.GetBoolean(0) = True Then
                    '    'payButton.Visible = False
                    'End If
                    If myDataReader.GetBoolean(1) = True Then
                        pickupButton.Visible = True
                    Else
                        pickupButton.Visible = False
                    End If
                    isVacation = myDataReader.GetBoolean(2)
                    If myDataReader.GetInt32(3) > 1 Then
                        vacationButton.Visible = False
                        If Session("Role") = "Admin" Then
                            ForcevacationButton.Visible = True
                        Else
                            ForcevacationButton.Visible = False
                        End If
                    Else
                        ForcevacationButton.Visible = False
                    End If

                Loop
            Finally
                If (mySqlConnection.State = ConnectionState.Open) Then
                    mySqlConnection.Close()
                End If
            End Try
        End If
    End Sub
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Try
            Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            Dim SI As String = GridView1.DataKeys(row.RowIndex).Value
            If e.CommandName = "Pay" Then
                Response.Redirect("pay?s=" + SI)
            ElseIf e.CommandName = "Vacation" Then
                Response.Redirect("Vacation?s=" + SI)
            ElseIf e.CommandName = "ForceVacation" Then
                Response.Redirect("Vacation?s=" + SI + "&F=Y")
            ElseIf e.CommandName = "Details" Then
                Response.Redirect("Details?s=" + SI)
            End If
        Catch ex As Exception
            lit1.Text = ex.StackTrace + "<br />" + ex.StackTrace + "<br />Row: " + GridView1.SelectedIndex.ToString
        End Try
    End Sub

    Protected Sub WeekList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles WeekList.SelectedIndexChanged
        If Not WeekList.SelectedValue = " - Select a Week - " Then
            lit1.Text = ""
            GridView1.Visible = True
            FillInfo()
        Else
            lit1.Text = "<h2>Please select a week</h2>"
            GridView1.Visible = False
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
                options = "(weekly.location = '" + StoreList.SelectedValue + "') and (weekly.PickupDay = '" + PickupDayList.SelectedValue + "')"
            Else
                options = "(weekly.location = '" + StoreList.SelectedValue + "')"
            End If
        ElseIf Not PickupDayList.SelectedValue = " - Select a Pickup Day - " Then
            options = "(weekly.PickupDay = '" + PickupDayList.SelectedValue + "')"
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
                options = "(weekly.PickupDay = '" + PickupDayList.SelectedValue + "') and (weekly.location = '" + StoreList.SelectedValue + "')"
            Else
                options = "(weekly.PickupDay = '" + PickupDayList.SelectedValue + "')"
            End If
        ElseIf Not StoreList.SelectedValue = " - Select a Store - " Then
            options = "(weekly.location = '" + StoreList.SelectedValue + "')"
        End If
        FillInfo()
    End Sub
    
    Protected Sub Showinactive_CheckedChanged(sender As Object, e As EventArgs) Handles Showinactive.CheckedChanged
        If Showinactive.Checked = True Then
            active = "active='false'"
        ElseIf Showinactive.Checked = False Then
            active = "active='true'"
        End If
        FillInfo()
    End Sub
End Class
