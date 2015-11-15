Imports System.IO
Imports System.Data.OleDb
Imports System.Data
Imports System.Data.SqlClient

Partial Class admin_excel
    Inherits System.Web.UI.Page
    Private conn As SqlConnection = Nothing
    Private ConnectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ToString
    Private cmd As SqlCommand = Nothing
    Dim password As String = ""
    Dim Username As String = ""
    Dim useremail As String = ""
    Dim firstname1 As String = ""
    Dim firstname2 As String = ""
    Dim lastname1 As String = ""
    Dim lastname2 As String = ""
    Dim i1 As Integer = 0
    Dim i2 As Integer = 0
    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If FileUpload1.HasFile Then
            Dim FileName As String = Path.GetFileName(FileUpload1.PostedFile.FileName)
            Dim Extension As String = Path.GetExtension(FileUpload1.PostedFile.FileName)
            Dim FolderPath As String = ConfigurationManager.AppSettings("FolderPath")

            Dim FilePath As String = Server.MapPath(FolderPath + FileName)
            FileUpload1.SaveAs(FilePath)
            Import_To_Grid(FilePath, Extension, rbHDR.SelectedItem.Text)
        End If
    End Sub
    Private Sub Import_To_Grid(ByVal FilePath As String, ByVal Extension As String, ByVal isHDR As String)
        Dim conStr As String = ""
        Select Case Extension
            Case ".xls"
                'Excel 97-03
                conStr = ConfigurationManager.ConnectionStrings("Excel03ConString") _
                           .ConnectionString
                Exit Select
            Case ".xlsx"
                'Excel 07
                conStr = ConfigurationManager.ConnectionStrings("Excel07ConString") _
                          .ConnectionString
                Exit Select
        End Select
        conStr = String.Format(conStr, FilePath, isHDR)

        Dim connExcel As New OleDbConnection(conStr)
        Dim cmdExcel As New OleDbCommand()
        Dim oda As New OleDbDataAdapter()
        Dim dt As New DataTable()

        cmdExcel.Connection = connExcel

        'Get the name of First Sheet
        connExcel.Open()
        Dim dtExcelSchema As DataTable
        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
        Dim SheetName As String = dtExcelSchema.Rows(0)("TABLE_NAME").ToString()
        connExcel.Close()

        'Read Data from First Sheet
        connExcel.Open()
        cmdExcel.CommandText = "SELECT * From [" & SheetName & "]"
        oda.SelectCommand = cmdExcel
        oda.Fill(dt)
        connExcel.Close()

        'Bind Data to GridView
        GridView1.Caption = Path.GetFileName(FilePath)
        GridView1.DataSource = dt
        GridView1.DataBind()
        DBInsert()

    End Sub

    Protected Sub PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        Dim FolderPath As String = ConfigurationManager.AppSettings("FolderPath")
        Dim FileName As String = GridView1.Caption
        Dim Extension As String = Path.GetExtension(FileName)
        Dim FilePath As String = Server.MapPath(FolderPath + FileName)

        Import_To_Grid(FilePath, Extension, rbHDR.SelectedItem.Text)
        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataBind()
    End Sub
    Function DBInsert() As Boolean
        
        For Each row As GridViewRow In GridView1.Rows
            If Not row.Cells(1).Text = "" Or Not row.Cells(1).Text = "&nbsp;" Then
                Try

                    Dim query As String = "INSERT INTO subscribers (FirstName1, LastName1, Email1, phone1, FirstName2, LastName2, Email2, phone2, Address, City, State, Zip, Allergies, vacUsed, Newsletter, Enrolled, Referred, Notes, pickupday, store, bounty, barnyard, ploughman, username, active) VALUES (@FirstName1, @LastName1, @Email1, @phone1, @FirstName2, @LastName2, @Email2, @phone2, @Address, @City, @State, @Zip, @Allergies, @vacUsed, @Newsletter, @Enrolled, @Referred, @Notes, @pickupday, @store, @bounty, @barnyard, @ploughman, @Username, 'true') "
                    Using conn As New SqlConnection(ConnectionString)
                        Using comm As New SqlCommand()
                            With comm
                                .Connection = conn
                                .CommandType = CommandType.Text
                                .CommandText = query
                                Dim pattern As String = "(.*?)/(.*?)"
                                Dim replacement As String = "$1" & vbCrLf
                                Dim rgx As New Regex(pattern, RegexOptions.Singleline)
                                firstname1 = rgx.Replace(row.Cells(2).Text, replacement)
                                replacement = "$2" & vbCrLf
                                firstname2 = rgx.Replace(row.Cells(2).Text, replacement)
                                replacement = "$1" & vbCrLf
                                lastname1 = rgx.Replace(row.Cells(1).Text, replacement)
                                replacement = "$2" & vbCrLf
                                lastname2 = rgx.Replace(row.Cells(1).Text, replacement)


                                If firstname1 = firstname2 Then
                                    firstname2 = ""
                                    lastname2 = ""
                                Else
                                    firstname1 = firstname1.Replace(firstname2.Trim, "") & vbCrLf
                                End If

                                comm.Parameters.Add("@FirstName1", SqlDbType.VarChar).Value = firstname1
                                .Parameters.Add("@LastName1", SqlDbType.VarChar).Value = lastname1
                                .Parameters.Add("@Email1", SqlDbType.VarChar).Value = row.Cells(12).Text
                                .Parameters.Add("@phone1", SqlDbType.VarChar).Value = row.Cells(11).Text
                                .Parameters.Add("@FirstName2", SqlDbType.VarChar).Value = firstname2
                                .Parameters.Add("@LastName2", SqlDbType.VarChar).Value = lastname2
                                .Parameters.Add("@Email2", SqlDbType.VarChar).Value = ""
                                .Parameters.Add("@phone2", SqlDbType.VarChar).Value = ""
                                .Parameters.Add("@Address", SqlDbType.VarChar).Value = row.Cells(13).Text
                                .Parameters.Add("@City", SqlDbType.VarChar).Value = row.Cells(14).Text
                                .Parameters.Add("@State", SqlDbType.VarChar).Value = row.Cells(15).Text
                                .Parameters.Add("@Zip", SqlDbType.VarChar).Value = row.Cells(16).Text
                                .Parameters.Add("@Allergies", SqlDbType.VarChar).Value = row.Cells(17).Text
                                .Parameters.Add("@vacUsed", SqlDbType.Int).Value = 0
                                .Parameters.Add("@Newsletter", SqlDbType.Bit).Value = False
                                .Parameters.Add("@Enrolled", SqlDbType.SmallDateTime).Value = Date.Now
                                .Parameters.Add("@Referred", SqlDbType.VarChar).Value = ""
                                .Parameters.Add("@Notes", SqlDbType.Text).Value = ""
                                If row.Cells(10).Text = "T" Then
                                    .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Thursday"
                                ElseIf row.Cells(10).Text = "F" Then
                                    .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Friday"
                                Else
                                    .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Friday"
                                End If
                                .Parameters.Add("@store", SqlDbType.Text).Value = "Downtown Columbia"
                                .Parameters.Add("@username", SqlDbType.Text).Value = firstname1 + "." + lastname1
                                If row.Cells(8).Text = "1" Then
                                    .Parameters.Add("@barnyard", SqlDbType.Bit).Value = "True"
                                Else
                                    .Parameters.Add("@barnyard", SqlDbType.Bit).Value = "False"
                                End If
                                If row.Cells(7).Text = "1" Then
                                    .Parameters.Add("@bounty", SqlDbType.Bit).Value = "True"
                                Else
                                    .Parameters.Add("@bounty", SqlDbType.Bit).Value = "False"
                                End If
                                If row.Cells(9).Text = "1" Then
                                    .Parameters.Add("@ploughman", SqlDbType.Bit).Value = "True"
                                Else
                                    .Parameters.Add("@ploughman", SqlDbType.Bit).Value = "False"
                                End If
                            End With

                            conn.Open()
                            comm.ExecuteNonQuery()
                            i1 += 1
                        End Using
                    End Using
                Catch ex As SqlException
                    Literal1.Text += "<br />" + ex.Message + "<br /><br />" + ex.StackTrace
                End Try
            End If
        Next
        Literal1.Text += "<br />" + "Step1 Complete " + i1.ToString + " records!"

        DBInsert2()
    End Function
    Function DBInsert2() As Boolean
        Dim SubId As Integer = 0
        Dim myDataReader As SqlDataReader
        Dim mySqlConnection As SqlConnection
        Dim mySqlCommand As SqlCommand
        mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        For Each row As GridViewRow In GridView1.Rows
            If Not row.Cells(1).Text = "" Or Not row.Cells(1).Text = "&nbsp;" Then
                Dim pattern As String = "(.*?)/(.*?)"
                Dim replacement As String = "$1" & vbCrLf
                Dim rgx As New Regex(pattern, RegexOptions.Singleline)
                firstname1 = rgx.Replace(row.Cells(2).Text, replacement)
                replacement = "$2" & vbCrLf
                firstname2 = rgx.Replace(row.Cells(2).Text, replacement)
                replacement = "$1" & vbCrLf
                lastname1 = rgx.Replace(row.Cells(1).Text, replacement)
                replacement = "$2" & vbCrLf
                lastname2 = rgx.Replace(row.Cells(1).Text, replacement)

                If firstname1 = firstname2 Then
                    firstname2 = ""
                    lastname2 = ""
                Else
                    firstname1 = firstname1.Replace(firstname2.Trim, "") & vbCrLf
                End If
                mySqlCommand = New SqlCommand("SELECT SubID FROM subscribers Where FirstName1= '" + firstname1 + "' and address='" + row.Cells(13).Text + "'", mySqlConnection)
                Try
                    mySqlConnection.Open()
                    myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                    Do While (myDataReader.Read())
                        SubId = myDataReader.GetInt32(0)
                        Dim query As String = "INSERT INTO Weekly (SubId, bounty, barnyard, ploughman, PickupDay, Location, Vacation, PaidBounty, PaidBarnyard, PaidPloughman, Pickedup, Notes, Week) VALUES (@SubId, @bounty, @barnyard, @ploughman, @PickupDay, @Location, 'False', @PaidBounty, @PaidBarnyard, @PaidPloughman, 'False', '', @Week) "
                        Using conn2 As New SqlConnection(ConnectionString)
                            Using comm2 As New SqlCommand()
                                With comm2
                                    .Connection = conn2
                                    .CommandType = CommandType.Text
                                    .CommandText = query
                                    comm2.Parameters.Add("@SubId", SqlDbType.Int).Value = SubId
                                    If row.Cells(8).Text = "1" Then
                                        .Parameters.Add("@barnyard", SqlDbType.Bit).Value = "True"
                                        .Parameters.Add("@paidbarnyard", SqlDbType.Bit).Value = "True"
                                    Else
                                        .Parameters.Add("@barnyard", SqlDbType.Bit).Value = "False"
                                        .Parameters.Add("@paidbarnyard", SqlDbType.Bit).Value = "False"
                                    End If
                                    If row.Cells(7).Text = "1" Then
                                        .Parameters.Add("@bounty", SqlDbType.Bit).Value = "True"
                                        .Parameters.Add("@paidbounty", SqlDbType.Bit).Value = "True"
                                    Else
                                        .Parameters.Add("@bounty", SqlDbType.Bit).Value = "False"
                                        .Parameters.Add("@paidbounty", SqlDbType.Bit).Value = "False"
                                    End If
                                    If row.Cells(9).Text = "1" Then
                                        .Parameters.Add("@ploughman", SqlDbType.Bit).Value = "True"
                                        .Parameters.Add("@paidploughman", SqlDbType.Bit).Value = "True"
                                    Else
                                        .Parameters.Add("@ploughman", SqlDbType.Bit).Value = "False"
                                        .Parameters.Add("@paidploughman", SqlDbType.Bit).Value = "False"
                                    End If
                                    If row.Cells(10).Text = "T" Then
                                        .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Thursday"
                                    ElseIf row.Cells(10).Text = "F" Then
                                        .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Friday"
                                    Else
                                        .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Friday"
                                    End If
                                    .Parameters.Add("@Location", SqlDbType.Text).Value = "Downtown Columbia"
                                     .Parameters.Add("@Week", SqlDbType.SmallDateTime).Value = "1/1/1900"
                                End With
                                Try
                                    conn2.Open()
                                    comm2.ExecuteNonQuery()
                                Catch ex As SqlException
                                    Literal1.Text += "<br />" + ex.Message + "<br /><br />" + ex.StackTrace
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
                                    query = "INSERT INTO Weekly (SubId, bounty, barnyard, ploughman, PickupDay, Location, Vacation, PaidBounty, PaidBarnyard, PaidPloughman, Pickedup, Notes, Week) VALUES (@SubId, @bounty, @barnyard, @ploughman, @PickupDay, @Location, 'False', @PaidBounty, @PaidBarnyard, @PaidPloughman, 'False', '', @Week) "
                                    Using conn2 As New SqlConnection(ConnectionString)
                                        Using comm2 As New SqlCommand()
                                            With comm2
                                                .Connection = conn2
                                                .CommandType = CommandType.Text
                                                .CommandText = query
                                                comm2.Parameters.Add("@SubId", SqlDbType.Int).Value = SubId
                                                If row.Cells(8).Text = "1" Then
                                                    .Parameters.Add("@barnyard", SqlDbType.Bit).Value = "True"
                                                Else
                                                    .Parameters.Add("@barnyard", SqlDbType.Bit).Value = "False"
                                                End If
                                                If row.Cells(7).Text = "1" Then
                                                    .Parameters.Add("@bounty", SqlDbType.Bit).Value = "True"
                                                Else
                                                    .Parameters.Add("@bounty", SqlDbType.Bit).Value = "False"
                                                End If
                                                If row.Cells(9).Text = "1" Then
                                                    .Parameters.Add("@ploughman", SqlDbType.Bit).Value = "True"
                                                Else
                                                    .Parameters.Add("@ploughman", SqlDbType.Bit).Value = "False"
                                                End If
                                                If row.Cells(10).Text = "T" Then
                                                    .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Thursday"
                                                ElseIf row.Cells(10).Text = "F" Then
                                                    .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Friday"
                                                Else
                                                    .Parameters.Add("@pickupday", SqlDbType.Text).Value = "Friday"
                                                End If
                                                .Parameters.Add("@Location", SqlDbType.Text).Value = "Downtown Columbia"
                                                .Parameters.Add("@PaidBounty", SqlDbType.VarChar).Value = False
                                                .Parameters.Add("@PaidBarnyard", SqlDbType.VarChar).Value = False
                                                .Parameters.Add("@PaidPloughman", SqlDbType.VarChar).Value = False
                                                .Parameters.Add("@Week", SqlDbType.SmallDateTime).Value = testDate.ToShortDateString()
                                            End With
                                            Try
                                                conn2.Open()
                                                comm2.ExecuteNonQuery()
                                            Catch ex As SqlException
                                                Literal1.Text += "<br />" + ex.Message + "<br /><br />" + ex.StackTrace
                                            End Try
                                        End Using
                                    End Using
                                    Exit Select
                            End Select
                        Next
                        i2 += 1
                    Loop
                Finally
                    If (mySqlConnection.State = ConnectionState.Open) Then
                        mySqlConnection.Close()
                    End If
                End Try
            End If
        Next
        Literal1.Text += "<br />" + "Step2 Complete " + i2.ToString + " records!"
    End Function

End Class
