Imports System.Data
Imports System.Data.SqlClient

Partial Class admin_Notes
    Inherits System.Web.UI.Page
    Private conn As SqlConnection = Nothing
    Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
    Private cmd As SqlCommand = Nothing

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            WeekList.Visible = False
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
            NotesPanel.Visible = False
            FillSubscriberInfo()
        End If

    End Sub
    
    Protected Sub FillSubscriberInfo()
        Dim dt As New DataTable()

        dt.Columns.Add("SubID")
        dt.Columns.Add("Subscriber")
        dt.Rows.Add("0", " - Select a Subscriber - ")
        'Create Rows in DataTable
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        Try
            Using mySqlConnection
                mySqlCommand = New SqlCommand("SELECT Distinct weekly.SubID, subscribers.Lastname1, subscribers.Firstname1 FROM Weekly INNER JOIN subscribers ON weekly.SubID=subscribers.SubId where subscribers.active='true' order by subscribers.lastname1, subscribers.firstname1", mySqlConnection)
                mySqlConnection.Open()

                myDataReader = mySqlCommand.ExecuteReader()

                If myDataReader.HasRows Then
                    Dim count As Integer = myDataReader.FieldCount
                    Dim SubInfo As String = ""
                    Do While myDataReader.Read()
                        SubInfo = myDataReader.GetString(1) + ", " + myDataReader.GetString(2)
                        dt.Rows.Add(myDataReader.GetInt32(0), SubInfo)
                    Loop

                Else
                    Console.WriteLine("No rows found.")
                End If

                myDataReader.Close()
            End Using
        Finally
        End Try
        Me.SubscriberList.DataSource = dt
        Me.SubscriberList.DataTextField = "Subscriber"
        Me.SubscriberList.DataValueField = "SubID"
        Me.SubscriberList.DataBind()
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
                        dt.Rows.Add(myDataReader.GetDateTime(0).Month.ToString + "/" + myDataReader.GetDateTime(0).Day.ToString + "-" + myDataReader.GetDateTime(0).AddDays(1).Day.ToString + "/" + myDataReader.GetDateTime(0).Year.ToString)
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

    End Sub

    Protected Sub FillNotes()
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        Dim week As String = ""
        If NoteType.Items.FindByText("Weekly").Selected = True Then
            week = WeekList.SelectedValue
            Dim pattern As String = "-(.*?)/"
            Dim replacement As String = "/" & vbCrLf
            Dim rgx As New Regex(pattern, RegexOptions.Singleline)
            week = rgx.Replace(week, replacement)
            week = (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "")
            mySqlCommand = New SqlCommand("SELECT notes FROM weekly Where week='" + week + "' and SubId= '" + SubscriberList.SelectedValue + "'", mySqlConnection)
        Else
            mySqlCommand = New SqlCommand("SELECT notes FROM subscribers Where SubId= '" + SubscriberList.SelectedValue + "'", mySqlConnection)
        End If
        Try
            mySqlConnection.Open()
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Do While (myDataReader.Read())
                CurrNotesLiteral.Text = myDataReader.GetString(0)
            Loop
        Finally
            If (mySqlConnection.State = ConnectionState.Open) Then
                mySqlConnection.Close()
            End If
        End Try
    End Sub
    Protected Sub SubscriberList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles SubscriberList.SelectedIndexChanged
        Literal1.Text = ""
        If Not SubscriberList.SelectedValue = "0" Then
            NotesPanel.Visible = True
        Else
            NotesPanel.Visible = False
        End If
        WeekList.Visible = False
        NoteType.ClearSelection()
        FillNotes()
    End Sub

    Protected Sub NoteType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles NoteType.SelectedIndexChanged
        If NoteType.Items.FindByText("Weekly").Selected = True Then
            FillWeekInfo()
            WeekList.Visible = True
            FillNotes()
        Else
            WeekList.Visible = False
            FillNotes()
        End If
    End Sub

    Protected Sub submit_Click(sender As Object, e As EventArgs) Handles submit.Click
        Dim note As String
        If Not NoteBox.Text = "" Then
            If Not CurrNotesLiteral.Text = "" Then
                note = CurrNotesLiteral.Text + "<br /><br />" + NoteBox.Text
            Else
                note = NoteBox.Text
            End If
            note = note + "<br />Added by " + Membership.GetUser().ToString + " on " + Date.Now.ToShortDateString.ToString + "<br /><br />"
            Dim query As String = ""
            Dim week As String = ""
            Dim weekset As Date
            If NoteType.Items.FindByText("Weekly").Selected = True Then

                week = WeekList.SelectedValue
                Dim pattern As String = "-(.*?)/"
                Dim replacement As String = "/" & vbCrLf
                Dim rgx As New Regex(pattern, RegexOptions.Singleline)
                week = rgx.Replace(week, replacement)
                week = (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "")
                weekset = DateTime.Parse(week)
                query = "Update weekly set notes=@note WHERE week=@week and SubId=@subID"
                Using conn As New SqlConnection(ConnectionString)
                    Using comm As New SqlCommand()
                        With comm
                            .Connection = conn
                            .CommandType = CommandType.Text
                            .CommandText = query
                            comm.Parameters.Add("@note", SqlDbType.VarChar).Value = note
                            .Parameters.Add("@week", SqlDbType.DateTime).Value = weekset
                            .Parameters.Add("@subID", SqlDbType.Int).Value = SubscriberList.SelectedValue
                        End With
                        conn.Open()
                        comm.ExecuteNonQuery()
                    End Using
                End Using
            Else
                query = "Update subscribers set notes=@note WHERE SubId=@subID"
                Using conn As New SqlConnection(ConnectionString)
                    Using comm As New SqlCommand()
                        With comm
                            .Connection = conn
                            .CommandType = CommandType.Text
                            .CommandText = query
                            comm.Parameters.Add("@note", SqlDbType.VarChar).Value = note
                            .Parameters.Add("@subID", SqlDbType.Int).Value = SubscriberList.SelectedValue
                        End With
                        conn.Open()
                        comm.ExecuteNonQuery()
                    End Using
                End Using
            End If
            
            Literal1.Text = "<h2>Subscriber's note has been added!</h2>"
            NotesPanel.Visible = False
            NoteBox.Text = ""
            SubscriberList.ClearSelection()
            NoteType.ClearSelection()
            FillNotes()
        Else
            Literal1.Text = "<h2>No new note was entered</h2>"
        End If

    End Sub

    Protected Sub WeekList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles WeekList.SelectedIndexChanged
        FillNotes()
    End Sub
End Class
