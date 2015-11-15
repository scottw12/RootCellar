Imports System.Data.SqlClient
Imports System.Data
Imports PerceptiveMCAPI
Imports PerceptiveMCAPI.Types
Imports PerceptiveMCAPI.Methods
Imports System.Net
Imports System.IO
Imports System.Net.Mail

Partial Class admin_details
    Inherits System.Web.UI.Page

    Protected SubID As String
    Private conn As SqlConnection = Nothing
    Private ConnectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ToString
    Dim isExisting As Boolean = "False"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (Request.QueryString("s") Is Nothing) Then
            If Request.QueryString("s").ToString() <> "" Then
                SubID = Request.QueryString("s").ToString()
                Dim myDataReader As SqlDataReader
                Dim query As String = "SELECT username FROM subscribers Where SubId=@SubId"
                Using conn As New SqlConnection(ConnectionString)
                    Using comm As New SqlCommand()
                        With comm
                            .Connection = conn
                            .CommandType = CommandType.Text
                            .CommandText = query
                            comm.Parameters.Add("@SubID", SqlDbType.VarChar).Value = SubID
                        End With
                        conn.Open()
                        myDataReader = comm.ExecuteReader(CommandBehavior.CloseConnection)
                        Do While (myDataReader.Read())
                            If Not myDataReader.IsDBNull(0) Then
                                UsernameLiteral.Text = myDataReader.GetString(0)
                            End If
                        Loop
                    End Using
                End Using
            End If
        End If
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
                        PaymentsPane.Visible = True
                    ElseIf role = "Employee" Then
                        PaymentsPane.Visible = False
                    Else
                        Response.Redirect("~/account/")
                    End If
                Loop
            Finally
                If (mySqlConnection.State = ConnectionState.Open) Then
                    mySqlConnection.Close()
                End If
            End Try
            FillDayInfo()
            FillStoreInfo()
            FillPaymentInfo()
            FillInfo()
        End If
    End Sub
    Protected Sub FillInfo()

        If Not (Request.QueryString("s") Is Nothing) Then
            If Request.QueryString("s").ToString() <> "" Then
                SubID = Request.QueryString("s").ToString()
                Dim myDataReader As SqlDataReader
                Dim mySqlConnection As SqlConnection
                Dim mySqlCommand As SqlCommand
                mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
                mySqlCommand = New SqlCommand("SELECT Firstname1, Firstname2, lastname1, lastname2, email1, email2, phone1, phone2, address, city, state, zip, allergies, newsletter, vacused, subscribers.bounty, subscribers.barnyard, subscribers.ploughman, subscribers.pickupday, subscribers.store, subscribers.notes, BountyNL, BarnyardNL, PloughmanNL FROM weekly INNER JOIN subscribers ON weekly.SubID=subscribers.SubId Where weekly.SubId= '" + SubID + "'", mySqlConnection)
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
                        If myDataReader.GetBoolean(15) = True Then
                            BoxButton.Items.FindByText("Bounty - $35.00").Selected = True
                        Else
                            GridView1.Columns(2).Visible = False
                        End If
                        If myDataReader.GetBoolean(16) = True Then
                            BoxButton.Items.FindByText("Barnyard - $35.00").Selected = True
                        Else
                            GridView1.Columns(3).Visible = False
                        End If
                        If myDataReader.GetBoolean(17) = True Then
                            BoxButton.Items.FindByText("Ploughman - $35.00").Selected = True
                        Else
                            GridView1.Columns(4).Visible = False
                        End If
                        PickupDayList.SelectedValue = myDataReader.GetString(18)
                        StoreList.SelectedValue = myDataReader.GetString(19)
                        HeaderLiteral.Text = myDataReader.GetString(0) + " " + myDataReader.GetString(2) + "'s "
                        If Not myDataReader.GetString(20) = "" Then
                            Literal2.Text = myDataReader.GetString(20)
                        Else
                            Literal2.Text = "No permanent notes for this subscriber"
                        End If
                        BountyNL.Checked = myDataReader.GetBoolean(21)
                        BarnyardNL.Checked = myDataReader.GetBoolean(22)
                        PloughmanNL.Checked = myDataReader.GetBoolean(23)
                        'isExisting = myDataReader.GetBoolean(24)
                    Loop
                Catch ex As Exception
                    Literal1.Text = ex.Message + "<br />" + ex.StackTrace
                Finally
                    If (mySqlConnection.State = ConnectionState.Open) Then
                        mySqlConnection.Close()
                    End If
                End Try

            End If
        End If
    End Sub
    Protected Sub FillDayInfo()
        Dim dt As New DataTable()
        dt.Columns.Add("PickupDay")
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
    Protected Sub FillPaymentInfo()
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
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        Dim SqlQuary As String = "SELECT SubId, Week, PaidBounty, PaidBarnyard, PaidPloughman FROM Weekly where subID='" + SubID + "' " + SDateRange + " ORDER BY [Week]"
        Dim dt As New DataTable()
        dt.Columns.Add("SubId")
        dt.Columns.Add("Week")
        dt.Columns.Add("PaidBounty")
        dt.Columns.Add("PaidBarnyard")
        dt.Columns.Add("PaidPloughman")

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
        Literal1.Text = ""
        If Not (Request.QueryString("s") Is Nothing) Then
            If Request.QueryString("s").ToString() <> "" Then
                SubID = Request.QueryString("s").ToString()
                Dim query As String = "Update subscribers set FirstName1=@FirstName1, LastName1=@LastName1, Email1=@Email1, phone1=@phone1, FirstName2=@FirstName2, LastName2=@LastName2, Email2=@Email2, phone2=@phone2, Address=@Address, City=@City, State=@State, Zip=@Zip, Allergies=@Allergies, BountyNL=@BountyNL, BarnyardNL=@BarnyardNL, PloughmanNL=@PloughmanNL Where subId=@SubId"
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
                            .Parameters.Add("@SubId", SqlDbType.Int).Value = SubID
                            .Parameters.Add("@BountyNL", SqlDbType.Bit).Value = BountyNL.Checked
                            .Parameters.Add("@BarnyardNL", SqlDbType.Bit).Value = BarnyardNL.Checked
                            .Parameters.Add("@PloughmanNL", SqlDbType.Bit).Value = PloughmanNL.Checked
                        End With
                        Try
                            conn.Open()
                            comm.ExecuteNonQuery()
                            Literal1.Text = "<h2>Subscriber Updated!</h2>"
                            UpdMailChimp(email1.Text, BountyNL.Checked, BarnyardNL.Checked, PloughmanNL.Checked)
                            If Not email2.Text = "" Then
                                UpdMailChimp(email2.Text, BountyNL.Checked, BarnyardNL.Checked, PloughmanNL.Checked)
                            End If
                        Catch ex As SqlException
                            Literal1.Text = "Were sorry, there was an error"
                        End Try
                    End Using
                End Using
            End If
        End If
    End Sub
    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        For Each Weekrow As GridViewRow In GridView1.Rows
            Dim BountyPaid As CheckBox = TryCast(Weekrow.FindControl("BountyPaidCheck"), CheckBox)
            Dim BarnyardPaid As CheckBox = TryCast(Weekrow.FindControl("BarnyardPaidCheck"), CheckBox)
            Dim PloughmanPaid As CheckBox = TryCast(Weekrow.FindControl("PloughmanPaidCheck"), CheckBox)
            Dim week As String = Weekrow.Cells(1).Text
            If Not week = "Deposit" Then
                Dim pattern As String = "-(.*?)/"
                Dim replacement As String = "/" & vbCrLf
                Dim rgx As New Regex(pattern, RegexOptions.Singleline)
                week = rgx.Replace(week, replacement)
            Else
                week = "1/1/1900"
            End If

            week = (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "")
            conn = New SqlConnection(ConnectionString)
            conn.Open()
            Dim sql As String = "update weekly set PaidBounty='" + BountyPaid.Checked.ToString + "', PaidBarnyard='" + BarnyardPaid.Checked.ToString + "', PaidPloughman='" + PloughmanPaid.Checked.ToString + "' where SubID='" + Request.QueryString("s").ToString() + "' and week='" + week + "'"
            Dim cmd As New SqlCommand(sql)
            cmd.CommandType = CommandType.Text
            cmd.Connection = conn
            cmd.ExecuteNonQuery()
        Next
        Literal1.Text = "<h2>Payment Updated</h2>"
        FillInfo()

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

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Literal1.Text = ""
        If Not (Request.QueryString("s") Is Nothing) Then
            If Request.QueryString("s").ToString() <> "" Then
                SubID = Request.QueryString("s").ToString()
                Dim query As String = "Update subscribers set pickupday=@pickupday, store=@store, bounty=@bounty, barnyard=@barnyard, ploughman=@ploughman, active=@active Where subId=@SubId"
                Using conn As New SqlConnection(ConnectionString)
                    Using comm As New SqlCommand()
                        With comm
                            .Connection = conn
                            .CommandType = CommandType.Text
                            .CommandText = query
                            comm.Parameters.Add("@pickupday", SqlDbType.Text).Value = PickupDayList.SelectedValue
                            .Parameters.Add("@store", SqlDbType.VarChar).Value = StoreList.SelectedValue
                            .Parameters.Add("@SubId", SqlDbType.Int).Value = SubID
                            .Parameters.Add("@bounty", SqlDbType.Bit).Value = BoxButton.Items.FindByText("Bounty - $35.00").Selected
                            .Parameters.Add("@barnyard", SqlDbType.Bit).Value = BoxButton.Items.FindByText("Barnyard - $35.00").Selected
                            .Parameters.Add("@ploughman", SqlDbType.Bit).Value = BoxButton.Items.FindByText("Ploughman - $35.00").Selected
                            If BoxButton.Items.FindByText("Bounty - $35.00").Selected = False And BoxButton.Items.FindByText("Barnyard - $35.00").Selected = False And BoxButton.Items.FindByText("Ploughman - $35.00").Selected = False Then
                                .Parameters.Add("@active", SqlDbType.Bit).Value = False
                            Else
                                .Parameters.Add("@active", SqlDbType.Bit).Value = True
                            End If
                        End With
                        Try
                            conn.Open()
                            comm.ExecuteNonQuery()
                        Catch ex As SqlException
                            Literal1.Text = "Were sorry, there was an error"
                        End Try
                    End Using
                End Using
                Dim mySqlConnection As SqlConnection
                mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
                query = "Update Weekly set bounty=@bounty, barnyard=@barnyard, ploughman=@ploughman, PickupDay=@PickupDay, Location=@Location where SubId=@SubId "
                Using conn As New SqlConnection(ConnectionString)
                    Using comm As New SqlCommand()
                        With comm
                            .Connection = conn
                            .CommandType = CommandType.Text
                            .CommandText = query
                            comm.Parameters.Add("@SubId", SqlDbType.Int).Value = SubID
                            .Parameters.Add("@bounty", SqlDbType.Bit).Value = BoxButton.Items.FindByText("Bounty - $35.00").Selected
                            .Parameters.Add("@barnyard", SqlDbType.Bit).Value = BoxButton.Items.FindByText("Barnyard - $35.00").Selected
                            .Parameters.Add("@ploughman", SqlDbType.Bit).Value = BoxButton.Items.FindByText("Ploughman - $35.00").Selected

                            .Parameters.Add("@PickupDay", SqlDbType.VarChar).Value = PickupDayList.SelectedValue
                            .Parameters.Add("@Location", SqlDbType.VarChar).Value = StoreList.SelectedValue
                        End With
                        Try
                            conn.Open()
                            comm.ExecuteNonQuery()
                        Catch ex As SqlException
                            Literal1.Text = "Were sorry, there was an error"
                        End Try
                    End Using
                End Using
                Literal1.Text = "<h2>Subscriber Updated!</h2>"
            End If
        End If
    End Sub
    Private Sub UpdMailChimp(email As String, Bounty As Boolean, Barnyard As Boolean, Ploughman As Boolean)
        Dim webAddr As String = ""
        Try
            If Bounty = True Then
                Try
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=0a27dd543a&email[email]=" + email1.Text.Trim + "&merge_vars[FNAME]=" + firstname1.Text.Trim + "&merge_vars[LNAME]=" + lastname1.Text.Trim + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim + "&double_optin=false&send_welcome=false"
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
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/unsubscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=0a27dd543a&email[email]=" + email1.Text.Trim + "&merge_vars[FNAME]=" + firstname1.Text.Trim + "&merge_vars[LNAME]=" + lastname1.Text.Trim + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim + "&double_optin=false&send_welcome=false"
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
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=2335ec6f51&email[email]=" + email1.Text.Trim + "&merge_vars[FNAME]=" + firstname1.Text.Trim + "&merge_vars[LNAME]=" + lastname1.Text.Trim + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim + "&double_optin=false&send_welcome=false"
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
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/unsubscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=2335ec6f51&email[email]=" + email1.Text.Trim + "&merge_vars[FNAME]=" + firstname1.Text.Trim + "&merge_vars[LNAME]=" + lastname1.Text.Trim + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim + "&double_optin=false&send_welcome=false"
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
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=4801343502&email[email]=" + email1.Text.Trim + "&merge_vars[FNAME]=" + firstname1.Text.Trim + "&merge_vars[LNAME]=" + lastname1.Text.Trim + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim + "&double_optin=false&send_welcome=false"
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
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/unsubscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=4801343502&email[email]=" + email1.Text.Trim + "&merge_vars[FNAME]=" + firstname1.Text.Trim + "&merge_vars[LNAME]=" + lastname1.Text.Trim + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim + "&double_optin=false&send_welcome=false"
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
            Literal1.Text = "We're sorry, there seems to have been an error."
            'Literal1.Text = ex.Message + "<br />" + ex.StackTrace + "<br />" + webAddr
        End Try

    End Sub

    Protected Sub PassResetButton_Click(sender As Object, e As EventArgs) Handles PassResetButton.Click
        Try
            Literal1.Text = ""
            If Not TextBox3.Text = "" Then
                SubID = Request.QueryString("s").ToString()
                Dim username As String = ""
                Dim myDataReader As SqlDataReader
                Dim query As String = "SELECT username FROM subscribers Where SubId=@SubId"

                Using conn As New SqlConnection(ConnectionString)
                    Using comm As New SqlCommand()
                        With comm
                            .Connection = conn
                            .CommandType = CommandType.Text
                            .CommandText = query
                            comm.Parameters.Add("@SubID", SqlDbType.VarChar).Value = SubID
                        End With
                        conn.Open()
                        myDataReader = comm.ExecuteReader(CommandBehavior.CloseConnection)
                        Do While (myDataReader.Read())
                            If Not myDataReader.IsDBNull(0) Then
                                username = myDataReader.GetString(0)
                            End If
                        Loop

                    End Using
                End Using
                Dim myObject As MembershipUser = Membership.GetUser(username)
                myObject.IsApproved = True
                Dim newpassword = TextBox3.Text.Trim
                Dim generatedpassword As String = myObject.ResetPassword()
                myObject.ChangePassword(generatedpassword, newpassword)
                If CheckBox1.Checked = True Then
                    Dim oMail0 As MailMessage = New MailMessage()
                    oMail0.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
                    oMail0.To.Add(New MailAddress(email1.Text))
                    oMail0.Subject = "Root Cellar Password Reset "
                    oMail0.Priority = MailPriority.High
                    oMail0.IsBodyHtml = True
                    oMail0.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
                    oMail0.Body &= "<head><title></title></head>"
                    oMail0.Body &= "<body>"
                    oMail0.Body &= "Hello " + firstname1.Text.Replace("'", "").Replace("""", "").Replace(" ", "") + ",<br /><br />"
                    oMail0.Body &= "Our staff has reset your account password.<br /><br />You can now login to <a href='http://www.rootcellarboxes.com/login'>http://www.rootcellarboxes.com/login</a> using:<br />"
                    oMail0.Body &= "Username: " + username + "<br />"
                    oMail0.Body &= "Password: " + newpassword + "<br /><br />"
                    oMail0.Body &= "Please feel free to give us a call (573) 443-5055 or reply to this email if you have any additional questions.<br /><br /> "
                    oMail0.Body &= "Root Cellar Team"
                    oMail0.Body &= "</body>"
                    oMail0.Body &= "</html>"
                    Dim htmlView As AlternateView = AlternateView.CreateAlternateViewFromString(oMail0.Body, Nothing, "text/html")
                    oMail0.AlternateViews.Add(htmlView)
                    Dim smtpmail0 As New SmtpClient("relay-hosting.secureserver.net")
                    smtpmail0.EnableSsl = False
                    smtpmail0.UseDefaultCredentials = True
                    smtpmail0.Send(oMail0)
                    oMail0 = Nothing
                End If
                Literal1.Text = "<h2>Subscriber's passsword has been reset!</h2>"
            End If
        Catch ex As Exception
            'Literal1.Text = ex.Message + "<br />" + ex.StackTrace
        End Try
    End Sub
    Public Function ChangeUserName(ByVal NewUserName As String, OldUserName As String) As Boolean
        Dim IsSuccsessful As Boolean = False
        Dim myConn As New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ToString)
        Dim cmdChangeUserName As New SqlCommand()
        With cmdChangeUserName
            .CommandText = "dbo.Membership_ChangeUserName"
            .CommandType = Data.CommandType.StoredProcedure
            .Connection = myConn
            .Parameters.Add("@OldUserName", Data.SqlDbType.NVarChar)
            .Parameters.Add("@NewUserName", Data.SqlDbType.NVarChar)
        End With
        cmdChangeUserName.Parameters("@OldUserName").Value = OldUserName
        cmdChangeUserName.Parameters("@NewUserName").Value = NewUserName
        Try
            myConn.Open()
            cmdChangeUserName.ExecuteNonQuery()
            myConn.Close()
            IsSuccsessful = True
        Catch ex As Exception
            IsSuccsessful = False
        End Try
        Return IsSuccsessful
    End Function
    
    Protected Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim newUsername As String = TextBox1.Text.Trim()
        If newUsername.Length < 5 Then
            Literal1.Text = String.Format("The username {0} is too short. Please use at least 5 characters.", newUsername)
            Exit Sub
        End If
        'Does this username already exist?
        Dim usr As MembershipUser = Membership.GetUser(newUsername)
        If usr IsNot Nothing Then
            Literal1.Text = String.Format("The username {0} is already being used by someone else. Please try entering a different username.", newUsername)
            Exit Sub
        End If
        Dim username As String = UsernameLiteral.Text
        If ChangeUserName(newUsername, username) = True Then
            UsernameLiteral.Text = newUsername
            Literal1.Text = "<h2>Subscriber's username has been changed!</h2>"
            If CheckBox2.Checked = True Then
                Dim oMail0 As MailMessage = New MailMessage()
                oMail0.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
                oMail0.To.Add(New MailAddress(email1.Text))
                oMail0.Subject = "Root Cellar Password Reset "
                oMail0.Priority = MailPriority.High
                oMail0.IsBodyHtml = True
                oMail0.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
                oMail0.Body &= "<head><title></title></head>"
                oMail0.Body &= "<body>"
                oMail0.Body &= "Hello " + firstname1.Text.Replace("'", "").Replace("""", "").Replace(" ", "") + ",<br /><br />"
                oMail0.Body &= "Our staff has changed your account username.<br /><br />You can now login to <a href='http://www.rootcellarboxes.com/login'>http://www.rootcellarboxes.com/login</a> using:<br />"
                oMail0.Body &= "Username: " + newUsername + "<br />"
                oMail0.Body &= "Please feel free to give us a call (573) 443-5055 or reply to this email if you have any additional questions.<br /><br /> "
                oMail0.Body &= "Root Cellar Team"
                oMail0.Body &= "</body>"
                oMail0.Body &= "</html>"
                Dim htmlView As AlternateView = AlternateView.CreateAlternateViewFromString(oMail0.Body, Nothing, "text/html")
                oMail0.AlternateViews.Add(htmlView)
                Dim smtpmail0 As New SmtpClient("relay-hosting.secureserver.net")
                smtpmail0.EnableSsl = False
                smtpmail0.UseDefaultCredentials = True
                smtpmail0.Send(oMail0)
                oMail0 = Nothing
            End If
        Else
            Literal1.Text = "<h2>There was a problem updating the subscriber's username.</h2>"
        End If
    End Sub
End Class
