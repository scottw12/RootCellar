Imports System.Data.SqlClient
Imports System.Data
Imports System.Net
Imports System.Net.Mail

Partial Class account_success
    Inherits System.Web.UI.Page
    Private conn As SqlConnection = Nothing
    Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
    Private cmd As SqlCommand = Nothing
    Dim OrderID As String = ""
    Dim status As String = ""
    Dim SubID As String = ""
    Dim Username As String = ""
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            status = Request.Form("Status")
            OrderID = Request.Form("Order_id")
            If status.Contains("ok") Then
                'Call a function to save transaction details into database
                'Constant.SaveTranDetails()

                Dim myDataReader As SqlDataReader
                Dim mySqlConnection As SqlConnection
                Dim mySqlCommand As SqlCommand
                mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
                Dim SqlQuary As String = "Select Username, P1Date, P1Box, P2Date, P2Box, P3Date,P3Box, P4Date, P4Box, P5Date, P5Box, P6Date, P6Box, P7Date, P7Box, P8Date, P8Box, P9Date, P9Box, P10Date, P10Box, P11Date, P11Box, P12Date, P12Box, P13Date, P13Box, P14Date, P14Box, P15Date, P15Box, P16Date, P16Box, P17Date, P17Box, P18Date, P18Box, P19Date, P19Box, P20Date, P20Box, P21Date, P21Box, P22Date, P22Box, P23Date, P23Box, P24Date, P24Box, P25Date, P25Box from TempOrders Where OrderID='" + OrderID + "'"
                Dim dt As New DataTable()
                dt.Columns.Add("Username")
                dt.Columns.Add("PBox")
                dt.Columns.Add("PDate")
                mySqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
                Using mySqlConnection
                    mySqlCommand = New SqlCommand(SqlQuary, mySqlConnection)
                    mySqlConnection.Open()
                    myDataReader = mySqlCommand.ExecuteReader()
                    If myDataReader.HasRows Then
                        Do While myDataReader.Read()
                            Username = myDataReader.GetString(0)
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(2), myDataReader.GetDateTime(1))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(4), myDataReader.GetDateTime(3))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(6), myDataReader.GetDateTime(5))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(8), myDataReader.GetDateTime(7))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(10), myDataReader.GetDateTime(9))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(12), myDataReader.GetDateTime(11))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(14), myDataReader.GetDateTime(13))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(16), myDataReader.GetDateTime(15))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(18), myDataReader.GetDateTime(17))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(20), myDataReader.GetDateTime(19))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(22), myDataReader.GetDateTime(21))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(24), myDataReader.GetDateTime(23))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(26), myDataReader.GetDateTime(25))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(28), myDataReader.GetDateTime(27))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(30), myDataReader.GetDateTime(29))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(32), myDataReader.GetDateTime(31))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(34), myDataReader.GetDateTime(33))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(36), myDataReader.GetDateTime(35))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(38), myDataReader.GetDateTime(37))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(40), myDataReader.GetDateTime(39))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(42), myDataReader.GetDateTime(41))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(44), myDataReader.GetDateTime(43))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(46), myDataReader.GetDateTime(45))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(48), myDataReader.GetDateTime(47))
                            dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(50), myDataReader.GetDateTime(49))
                        Loop
                    Else
                        Console.WriteLine("No rows found.")
                    End If
                    myDataReader.Close()
                End Using
                Dim myDataReader2 As SqlDataReader
                Dim mySqlConnection2 As SqlConnection
                Dim mySqlCommand2 As SqlCommand
                mySqlConnection2 = New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
                Using mySqlConnection2
                    mySqlCommand2 = New SqlCommand("SELECT SubID FROM subscribers Where Username= '" + Username + "'", mySqlConnection2)
                    Try
                        mySqlConnection2.Open()
                        myDataReader2 = mySqlCommand2.ExecuteReader(CommandBehavior.CloseConnection)
                        Do While (myDataReader2.Read())
                            SubID = myDataReader2.GetInt32(0)
                        Loop
                    Finally
                        If (mySqlConnection2.State = ConnectionState.Open) Then
                            mySqlConnection2.Close()
                        End If
                    End Try
                End Using
                Dim i As Integer = 1
                For Each row As DataRow In dt.Rows
                    If Not row("PBox") = "" Then
                        Dim week As String = row("PDate")
                        Dim pattern As String = "-(.*?)/"
                        Dim replacement As String = "/" & vbCrLf
                        Dim rgx As New Regex(pattern, RegexOptions.Singleline)
                        week = rgx.Replace(week, replacement)
                        week = (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "")
                        SetPaid(i, week, row("PBox"))
                        i += 1
                    End If
                Next row
                DeleteTemp()
                Literal1.Text = "<H2>Thank you</h2><h4>You payment has been processed successfully!</h4> "

            End If
        Catch ex As Exception
            If Not ex.Message.Contains("Object reference not set to an instance of an object") Then
                Dim oMail1 As MailMessage = New MailMessage()
                oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
                oMail1.To.Add(New MailAddress("scottw@jkmcomm.com"))
                oMail1.Subject = "Root Cellar Error"
                oMail1.Priority = MailPriority.High
                oMail1.IsBodyHtml = True
                oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
                oMail1.Body &= "<head><title></title></head>"
                oMail1.Body &= "<body>"
                oMail1.Body &= "Error in 'Success': " + Username + "<br /><br />"
                oMail1.Body &= ex.Message + "<br /><br />" + ex.StackTrace
                oMail1.Body &= "</body>"
                oMail1.Body &= "</html>"
                Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
                oMail1.AlternateViews.Add(htmlView2)
                Dim smtpmail2 As New SmtpClient()
                smtpmail2.Send(oMail1)
                oMail1 = Nothing
                Literal1.Text = "We're sorry, there seems to have been an error"
            End If
        End Try
    End Sub

    Private Sub SetPaid(i As Integer, Bdate As String, Box As String)
        Dim query As String = ""
        Try
            query = "UPDATE weekly SET Paid" + Box + "='true' WHERE week='" + Bdate + "' and SubID='" + SubID + "'"
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
            query = "Update subscribers set active='true' where subid='" + SubID + "' "
            Using conn2 As New SqlConnection(ConnectionString)
                Using comm2 As New SqlCommand()
                    With comm2
                        .Connection = conn2
                        .CommandType = CommandType.Text
                        .CommandText = query
                    End With
                    conn2.Open()
                    comm2.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Try

                Dim oMail1 As MailMessage = New MailMessage()
                oMail1.From = New MailAddress("Root Cellar <website@rootcellarboxes.com>")
                oMail1.To.Add(New MailAddress("scottw@jkmcomm.com"))
                oMail1.Subject = "Root Cellar Error"
                oMail1.Priority = MailPriority.High
                oMail1.IsBodyHtml = True
                oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >"
                oMail1.Body &= "<head><title></title></head>"
                oMail1.Body &= "<body>"
                oMail1.Body &= "Error in 'Success': " + Username + "<br /><br />"
                oMail1.Body &= ex.Message + "<br /><br />" + ex.StackTrace
                oMail1.Body &= " " + Username + "<br /><br />"
                oMail1.Body &= "Query: " + query + "<br /><br />"
                oMail1.Body &= "i: " + i.ToString + "<br /><br />Bdate: " + Bdate + "<br /><br />Box: " + Box
                oMail1.Body &= "</body>"
                oMail1.Body &= "</html>"
                Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
                oMail1.AlternateViews.Add(htmlView2)
                Dim smtpmail2 As New System.Net.Mail.SmtpClient
                smtpmail2.Send(oMail1)
                oMail1 = Nothing
            Catch ex2 As Exception

            End Try

            Literal1.Text = "We're sorry, there seems to have been an error"
        End Try
    End Sub
    Private Sub DeleteTemp()
        Try
            Dim query2 As String = "DELETE FROM TempOrders WHERE OrderID=@OrderID"
            Using conn As New SqlConnection(ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query2
                        comm.Parameters.Add("@OrderID", SqlDbType.VarChar).Value = OrderID
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                End Using
            End Using
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
            oMail1.Body &= "Error in 'Success': " + Username + "<br /><br />"
            oMail1.Body &= ex.Message + "<br /><br />" + ex.StackTrace
            oMail1.Body &= "</body>"
            oMail1.Body &= "</html>"
            Dim htmlView2 As AlternateView = AlternateView.CreateAlternateViewFromString(oMail1.Body, Nothing, "text/html")
            oMail1.AlternateViews.Add(htmlView2)
            Dim smtpmail2 As New System.Net.Mail.SmtpClient
            smtpmail2.Send(oMail1)
            oMail1 = Nothing
        End Try
    End Sub


End Class
