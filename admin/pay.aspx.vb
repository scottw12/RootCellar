Imports System.Data.SqlClient
Imports System.Data

Partial Class admin_pay
    Inherits System.Web.UI.Page

    Protected SubID As String
    Private conn As SqlConnection = Nothing
    Private ConnectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ToString

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
            If Not (Request.QueryString("s") Is Nothing) Then
                If Request.QueryString("s").ToString() <> "" Then
                    SubID = Request.QueryString("s").ToString()
                    FillInfo()
                    If Not Page.IsPostBack Then
                        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
                        mySqlCommand = New SqlCommand("SELECT Firstname1, Lastname1, weekly.bounty, weekly.barnyard, weekly.ploughman FROM weekly INNER JOIN subscribers ON weekly.SubID=subscribers.SubId Where weekly.SubID= '" + SubID + "'", mySqlConnection)
                        Try
                            mySqlConnection.Open()
                            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                            Do While (myDataReader.Read())
                                Literal1.Text = "Making subscription payment for <b>" + myDataReader.GetString(0) + " " + myDataReader.GetString(1) + "<br />"
                                If myDataReader.GetBoolean(3) = False Then
                                    GridView1.Columns(3).Visible = False
                                End If
                                If myDataReader.GetBoolean(2) = False Then
                                    GridView1.Columns(2).Visible = False
                                End If
                                If myDataReader.GetBoolean(4) = False Then
                                    GridView1.Columns(4).Visible = False
                                End If
                            Loop
                        Finally
                            If (mySqlConnection.State = ConnectionState.Open) Then
                                mySqlConnection.Close()
                            End If
                        End Try

                    End If
                End If
            End If
            
            Price.Text = "$0.00"
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
                            SDateRange = "and ((week>='" + myDataReader2.GetDateTime(0) + "' and week <= '" + myDataReader2.GetDateTime(1) + "') Or Week = '1/1/1900') "
                        Loop
                    End If
                    myDataReader2.Close()
                End Using
            End Using
        Finally
        End Try
        Dim SqlQuary As String = "SELECT SubId, Week, PaidBounty, PaidBarnyard, PaidPloughman FROM Weekly where subID='" + SubID + "' " + SDateRange + " ORDER BY [Week]"
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
                        Dim week As String = (myDataReader.GetDateTime(1).Month.ToString + "/" + myDataReader.GetDateTime(1).Day.ToString + "-" + myDataReader.GetDateTime(1).AddDays(1).Day.ToString + "/" + myDataReader.GetDateTime(1).Year.ToString)
                        If week = "1/1-2/1900" Then
                            week = "Deposit"
                        End If
                        dt.Rows.Add(myDataReader.GetInt32(0), week, myDataReader.GetBoolean(2), myDataReader.GetBoolean(3), myDataReader.GetBoolean(4))
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
    End Sub
     Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim days As Integer = 0
            For Each Weekrow As GridViewRow In GridView1.Rows
                Dim BountyPaid As CheckBox = TryCast(Weekrow.FindControl("BountyPaidCheck"), CheckBox)
                If BountyPaid.Enabled = True And BountyPaid.Checked = True Then
                    Dim week As String = Weekrow.Cells(1).Text
                    Dim pattern As String = "-(.*?)/"
                    Dim replacement As String = "/" & vbCrLf
                    Dim rgx As New Regex(pattern, RegexOptions.Singleline)
                    week = rgx.Replace(week, replacement)
                    week = (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "")
                    conn = New SqlConnection(ConnectionString)
                    conn.Open()
                    Dim sql As String = "update weekly set PaidBounty='True' where SubID='" + Request.QueryString("s").ToString() + "' and week='" + week + "'"
                    Dim cmd As New SqlCommand(sql)
                    cmd.CommandType = CommandType.Text
                    cmd.Connection = conn
                    cmd.ExecuteNonQuery()
                    days += 1
                End If
            Next
            For Each Weekrow As GridViewRow In GridView1.Rows
                Dim BountyPaid As CheckBox = TryCast(Weekrow.FindControl("BarnyardPaidCheck"), CheckBox)
                If BountyPaid.Enabled = True And BountyPaid.Checked = True Then
                    Dim week As String = Weekrow.Cells(1).Text
                    Dim pattern As String = "-(.*?)/"
                    Dim replacement As String = "/" & vbCrLf
                    Dim rgx As New Regex(pattern, RegexOptions.Singleline)
                    week = rgx.Replace(week, replacement)
                    week = (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "")
                    conn = New SqlConnection(ConnectionString)
                    conn.Open()
                    Dim sql As String = "update weekly set PaidBarnyard='True' where SubID='" + Request.QueryString("s").ToString() + "' and week='" + week + "'"
                    Dim cmd As New SqlCommand(sql)
                    cmd.CommandType = CommandType.Text
                    cmd.Connection = conn
                    cmd.ExecuteNonQuery()
                    days += 1
                End If
            Next
            For Each Weekrow As GridViewRow In GridView1.Rows
                Dim BountyPaid As CheckBox = TryCast(Weekrow.FindControl("PloughmanPaidCheck"), CheckBox)
                If BountyPaid.Enabled = True And BountyPaid.Checked = True Then
                    Dim week As String = Weekrow.Cells(1).Text
                    Dim pattern As String = "-(.*?)/"
                    Dim replacement As String = "/" & vbCrLf
                    Dim rgx As New Regex(pattern, RegexOptions.Singleline)
                    week = rgx.Replace(week, replacement)
                    week = (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "")
                    conn = New SqlConnection(ConnectionString)
                    conn.Open()
                    Dim sql As String = "update weekly set PaidPloughman='True' where SubID='" + Request.QueryString("s").ToString() + "' and week='" + week + "'"
                    Dim cmd As New SqlCommand(sql)
                    cmd.CommandType = CommandType.Text
                    cmd.Connection = conn
                    cmd.ExecuteNonQuery()
                    days += 1
                End If
            Next
            If days = 0 Then
                Literal2.Text = "Please select at least one week to make payment on"
            Else
                Literal1.Text = "Payment Recorded"
                step1.Visible = False
                tableUPanel.Update()
                FillInfo()
            End If
        
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

    Protected Sub OnCheckedChanged(sender As [Object], e As EventArgs)
        tableUPanel.Update()
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
End Class
