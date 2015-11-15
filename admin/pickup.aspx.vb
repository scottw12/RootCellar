Imports System.Data.SqlClient
Imports System.Data

Partial Class admin_pickup
    Inherits System.Web.UI.Page
    Private conn As SqlConnection = Nothing
    Private ConnectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ToString
    Private cmd As SqlCommand = Nothing
    Protected SubID As String
    Dim littxt As String = ""
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            step0.Visible = False
            FillWeekInfo()

            If Not (Request.QueryString("s") Is Nothing) Then
                If Request.QueryString("s").ToString() <> "" Then
                    FillInfo()
                Else
                    literal1.Text = "NO SUBSCRIBER SELECTED! CHANGES WILL NOT BE SAVED"
                End If
            Else
                literal1.Text = "NO SUBSCRIBER SELECTED! CHANGES WILL NOT BE SAVED"
            End If
               literal1.Text = littxt
            step2.Visible = False
            step3.Visible = False
        End If
    End Sub
    Protected Sub FillInfo()
        SubID = Request.QueryString("s").ToString()

        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        Dim week As String = ""
        Try

            If Not WeekList.SelectedValue = " - Select a Week - " Then
                step1.Visible = True
                week = WeekList.SelectedValue
                Dim pattern As String = "-(.*?)/"
                Dim replacement As String = "/" & vbCrLf
                Dim rgx As New Regex(pattern, RegexOptions.Singleline)
                week = rgx.Replace(week, replacement)
                week = "and weekly.week='" + (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "") + "'"
            Else
                step1.Visible = False
                week = ""
                Exit Sub
            End If
        Catch ex As Exception
            literal1.Text = "week:" + week
        End Try

        mySqlCommand = New SqlCommand("SELECT Firstname1, Lastname1, vacused, weekly.bounty, weekly.barnyard, weekly.ploughman, allergies, weekly.notes, weekly.vacation, weekly.paidBounty, weekly.paidBarnyard, weekly.paidPloughman, subscribers.notes FROM Weekly INNER JOIN subscribers ON weekly.SubID=subscribers.SubId Where weekly.SubID= '" + SubID + "'" + week, mySqlConnection)
        Try
            mySqlConnection.Open()
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Do While (myDataReader.Read())
                Dim Subscriptions As String = "<br /> Current Subscriptions:<br /><b> "
                Dim paid As String = ""
                If myDataReader.GetBoolean(3) = True Then
                    Subscriptions += "Bounty<br />"
                    If Not myDataReader.GetBoolean(9) = True Then
                        paid = "Bounty "
                    End If
                End If
                If myDataReader.GetBoolean(4) = True Then
                    Subscriptions += "Barnyard<br />"
                    If Not myDataReader.GetBoolean(10) = True Then
                        paid = "Barnyard "
                    End If
                End If
                If myDataReader.GetBoolean(5) = True Then
                    Subscriptions += "Ploughman<br />"
                    If Not myDataReader.GetBoolean(11) = True Then
                        paid = "Ploughman "
                    End If
                End If
                littxt = "In store pickup for <b>" + myDataReader.GetString(0) + " " + myDataReader.GetString(1) + "</b>" + Subscriptions + "</b>"
                If myDataReader.GetString(6) = "" Then
                    literal2.Text = "No Allergies on file"
                Else
                    literal2.Text = "<b>ALLEGIC to " + myDataReader.GetString(6) + "</b>"
                End If
                If myDataReader.GetString(7) = "" Then
                    literal3.Text = "None"
                Else
                    literal3.Text = myDataReader.GetString(7)
                End If
                If Not myDataReader.GetString(12) = "" Then
                    literal3.Text += "<br /><br /><b>Permanent Notes</b><br />" + myDataReader.GetString(12)
                End If
                If myDataReader.GetBoolean(8) = True Then
                    step0.Visible = True
                    step1.Visible = False
                Else
                    step0.Visible = False
                    step1.Visible = True
                End If
                If paid = "" Then
                    literal4.Text = "<h2><span style='color:green;'>PAID </span></h2>"
                Else
                    literal4.Text = "<h2><span style='color:red;'>NOT PAID </span></h2><br />The following boxes have not been paid<br />" + paid
                End If

            Loop
        Finally
            If (mySqlConnection.State = ConnectionState.Open) Then
                mySqlConnection.Close()
            End If
        End Try
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
    Protected Sub Yes1_Click(sender As Object, e As EventArgs) Handles Yes1.Click
        step1.Visible = False
        step2.Visible = True
        If literal4.Text.Contains("<span style='color:red;'>NOT PAID </span>") Then
            step2b.Visible = True
        Else
            step2b.Visible = False
            step3.Visible = True
        End If
    End Sub

    Protected Sub PaidButton_Click(sender As Object, e As EventArgs) Handles PaidButton.Click
        step2.Visible = False
        step3.Visible = True
        conn = New SqlConnection(ConnectionString)
        conn.Open()
        Dim week As String = ""
        If Not WeekList.SelectedValue = " - Select a Week - " Then
            week = WeekList.SelectedValue
            Dim pattern As String = "-(.*?)/"
            Dim replacement As String = "/" & vbCrLf
            Dim rgx As New Regex(pattern, RegexOptions.Singleline)
            week = rgx.Replace(week, replacement)
            week = "and week='" + (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "") + "'"
        Else
            week = ""
        End If
        Dim sql As String = "update weekly set paidbounty='True', paidbarnyard='True', paidploughman='True' where SubID='" + Request.QueryString("s").ToString() + "' " + week
        Dim cmd As New SqlCommand(sql)
        cmd.CommandType = CommandType.Text
        cmd.Connection = conn
        cmd.ExecuteNonQuery()
        literal5.Text = "Payment Recorded"
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
    Protected Sub Complete_Click(sender As Object, e As EventArgs) Handles Complete.Click
        conn = New SqlConnection(ConnectionString)
        conn.Open()
        Dim week As String = ""
        If Not WeekList.SelectedValue = " - Select a Week - " Then
            week = WeekList.SelectedValue
            Dim pattern As String = "-(.*?)/"
            Dim replacement As String = "/" & vbCrLf
            Dim rgx As New Regex(pattern, RegexOptions.Singleline)
            week = rgx.Replace(week, replacement)
            week = "and week='" + (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "") + "'"
        Else
            week = ""
        End If
        Dim sql As String = "update weekly set pickedup='true' where SubID='" + Request.QueryString("s").ToString() + "' " + week
        Dim cmd As New SqlCommand(sql)
        cmd.CommandType = CommandType.Text
        cmd.Connection = conn
        cmd.ExecuteNonQuery()
        Response.Redirect("~/admin/pickups")
    End Sub
    Function GetPickupDay() As Integer
        Dim daysAdd As Integer
        If Date.Today.ToString("dddd") = "Sunday" Then
            daysAdd = 6
        ElseIf Date.Today.ToString("dddd") = "Monday" Then
            daysAdd = 5
        ElseIf Date.Today.ToString("dddd") = "Tuesday" Then
            daysAdd = 4
        ElseIf Date.Today.ToString("dddd") = "Wednesday" Then
            daysAdd = 3
        ElseIf Date.Today.ToString("dddd") = "Thursday" Then
            daysAdd = 2
        ElseIf Date.Today.ToString("dddd") = "Friday" Then
            daysAdd = 1
        ElseIf Date.Today.ToString("dddd") = "Saturday" Then
            daysAdd = 0
        End If
        Return daysAdd
    End Function

    Protected Sub WeekList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles WeekList.SelectedIndexChanged
        If Not WeekList.SelectedValue = " - Select a Week - " Then
            step1.Visible = True
            FillInfo()
        Else
            step1.Visible = False
        End If
    End Sub
End Class
