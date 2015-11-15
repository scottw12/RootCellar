Imports System
Imports System.Web.UI
Imports Telerik.Web.UI
Imports System.Data.SqlClient
Imports System.Data

Partial Class admin_vacation
    Inherits RadAjaxPage
    Private conn As SqlConnection = Nothing
    Private ConnectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ToString
    Private cmd As SqlCommand = Nothing

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
            If Not (Request.QueryString("s") Is Nothing) Then
                If Request.QueryString("s").ToString() <> "" Then
                    FillWeekInfo()
                    GetDetails()
                Else
                    Literal1.Text = "NO SUBSCRIBER SELECTED! CHANGES WILL NOT BE SAVED"
                End If
            Else
                Literal1.Text = "NO SUBSCRIBER SELECTED! CHANGES WILL NOT BE SAVED"
            End If
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
                        'dt.Rows.Add(myDataReader.GetDateTime(0).ToString.Replace(" 12:00:00 AM", ""))
                        dt.Rows.Add(myDataReader.GetDateTime(0).Month.ToString + "/" + myDataReader.GetDateTime(0).Day.ToString + "-" + myDataReader.GetDateTime(0).AddDays(1).Day.ToString + "/" + myDataReader.GetDateTime(0).Year.ToString)
                        If myDataReader.GetDateTime(0) = Date.Today Or myDataReader.GetDateTime(0) = Date.Today.AddDays(6) Then
                            WeekList.SelectedValue = myDataReader.GetDateTime(0).Month.ToString + "/" + myDataReader.GetDateTime(0).Day.ToString + "-" + myDataReader.GetDateTime(0).AddDays(1).Day.ToString + "/" + myDataReader.GetDateTime(0).Year.ToString
                            GetDetails()
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
    Private Sub UpdDB()
        Try
            Dim myDataReader As SqlDataReader
            Dim mySqlConnection As SqlConnection
            Dim mySqlCommand As SqlCommand
            mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
            mySqlCommand = New SqlCommand("SELECT vacused FROM subscribers Where SubID= '" + Request.QueryString("s").ToString() + "'", mySqlConnection)
            Dim sql As String = ""
            Dim sql2 As String = ""
            Dim SelDate As DateTime = Date.Now
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
                        sql = "update subscribers set vacused='" + (myDataReader.GetInt32(0) + 1).ToString + "' where subid='" + Request.QueryString("s").ToString() + "'"
                        sql2 = "update weekly set vacation='true' where subid='" + Request.QueryString("s").ToString() + "' and week='" + week + "'"
                    Loop
                Finally
                    If (mySqlConnection.State = ConnectionState.Open) Then
                        mySqlConnection.Close()
                    End If
                End Try
                conn = New SqlConnection(ConnectionString)
                conn.Open()
                cmd = New SqlCommand(sql, conn)
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()

                conn = New SqlConnection(ConnectionString)
                conn.Open()
                cmd = New SqlCommand(sql2, conn)
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
                calpanel.Visible = False
                Dim littxt As String = ""
                mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
                mySqlCommand = New SqlCommand("SELECT Firstname1, Lastname1 FROM subscribers Where SubID= '" + Request.QueryString("s") + "'", mySqlConnection)
                Try
                    mySqlConnection.Open()
                    myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                    Do While (myDataReader.Read())
                        littxt = "<h2>Vacation set for " + myDataReader.GetString(0) + " " + myDataReader.GetString(1) + " on " + week + "</h2>"
                    Loop
                Finally
                    If (mySqlConnection.State = ConnectionState.Open) Then
                        mySqlConnection.Close()
                    End If
                End Try
                name.Text = littxt
            Else
                name.Text = "Please select a week"
            End If

        Catch ex As Exception
            Literal1.Text = "Were sorry, there was an error"
        End Try
    End Sub

   Sub GetDetails()
        Dim littxt As String = ""
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        mySqlCommand = New SqlCommand("SELECT Firstname1, Lastname1, vacused FROM subscribers Where SubID= '" + Request.QueryString("s") + "'", mySqlConnection)
        Try
            mySqlConnection.Open()
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Do While (myDataReader.Read())
                If myDataReader.GetInt32(2) = 0 Then
                    littxt = "Setting Vacation dates for " + myDataReader.GetString(0) + " " + myDataReader.GetString(1) + "<br />" + myDataReader.GetString(0) + " has not used any vacation weeks"
                ElseIf myDataReader.GetInt32(2) = 1 Then
                    littxt = "Setting Vacation dates for " + myDataReader.GetString(0) + " " + myDataReader.GetString(1) + "<br />" + myDataReader.GetString(0) + " has used 1 vacation week"
                ElseIf myDataReader.GetInt32(2) > 1 Then
                    littxt = "Setting Vacation dates for " + myDataReader.GetString(0) + " " + myDataReader.GetString(1) + "<br />" + myDataReader.GetString(0) + " has already used " + myDataReader.GetInt32(2).ToString + " vacation weeks!"
                    calpanel.Visible = False
                    If Not (Request.QueryString("F") Is Nothing) Then
                        If Request.QueryString("F").ToString() <> "" Then
                            If Request.QueryString("F") = "Y" And Session("Role") = "Admin" Then
                                calpanel.Visible = True
                            End If
                        End If
                    End If
                End If
            Loop
        Finally
            If (mySqlConnection.State = ConnectionState.Open) Then
                mySqlConnection.Close()
            End If
        End Try
        If littxt = "" Then
            name.Text = "NO SUBSCRIBER SELECTED! CHANGES WILL NOT BE SAVED"
        Else
            name.Text = littxt
        End If
    End Sub

    Protected Sub Submit_Click(sender As Object, e As EventArgs) Handles Submit.Click
        UpdDB()
    End Sub
End Class
