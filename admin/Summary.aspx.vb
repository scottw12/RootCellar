Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI.Upload
Imports Telerik.Web.UI
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.html.simpleparser
Imports System.Drawing

Partial Class admin_Summary
    Inherits System.Web.UI.Page
    Dim SqlQuary As String = ""
    Dim Options As String = ""
    Dim dt As New DataTable()
    Private conn As SqlConnection = Nothing
    Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
    Private cmd As SqlCommand = Nothing

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If User.Identity.IsAuthenticated Then
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
            Else
                Response.Redirect("~/login")
            End If
            FillWeekInfo()

            RadProgressArea1.ProgressIndicators = RadProgressArea1.ProgressIndicators And Not ProgressIndicators.SelectedFilesCount
        End If

        RadProgressArea1.Localization.Uploaded = "Total Progress"
        RadProgressArea1.Localization.UploadedFiles = "Progress"
        RadProgressArea1.Localization.CurrentFileName = "Custom progress in action: "

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        Return
    End Sub
    Protected Sub FillWeekInfo()
        dt.Columns.Add("Week")
        dt.Rows.Add(" - Select a Week - ")
        'Create Rows in DataTable
        Dim myDataReader2 As SqlDataReader
        Dim mySqlConnection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
        Dim mySqlCommand2 As SqlCommand
        Dim SDateRange As String = ""
        Dim query As String = "select Sstart, send from seasons where currents='True' order by sstart"
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
                            SDateRange = "(week >= '" + myDataReader2.GetDateTime(0) + "' and week <= '" + myDataReader2.GetDateTime(1) + "')"
                            i += 1
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
        Try
            Using mySqlConnection
                mySqlCommand = New SqlCommand("SELECT DISTINCT Week FROM Weekly where " + SDateRange + " ORDER BY [Week]", mySqlConnection)
                mySqlConnection.Open()

                myDataReader = mySqlCommand.ExecuteReader()

                If myDataReader.HasRows Then
                    Do While myDataReader.Read()
                        If Not myDataReader.GetDateTime(0).Year.ToString = "1900" Then
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
            UpdateProgressContext()
            FillInfo()
        Else
            GridView1.Visible = False
        End If
    End Sub
    Protected Sub FillInfo()
        GridView1.Visible = True
        Dim week As String
        week = WeekList.SelectedValue
        Dim pattern As String = "-(.*?)/"
        Dim replacement As String = "/" & vbCrLf
        Dim rgx As New Regex(pattern, RegexOptions.Singleline)
        week = rgx.Replace(week, replacement)
        week = (DateTime.Parse(week)).ToString.Replace(" 12:00:00 AM", "")
        Dim myDataReader As SqlDataReader
        Dim mySqlCommand As SqlCommand
        For Each Storerow In FillStoreInfo.Rows
            SqlQuary = "Select Count(Case when week='" + week + "' and Weekly.Bounty='True' and Location='" + Storerow(0) + "' and vacation='false' and Weekly.PickupDay='Thursday' Then 1 Else Null End) As BountyPUThu, Count( Case when week='" + week + "' and Weekly.Bounty='True' and Location='" + Storerow(0) + "' and vacation='false' and Weekly.PickupDay='Friday' Then 1 Else Null End) As BountyPUFri, Count( Case when week='" + week + "' and Weekly.Bounty='True' and Location='" + Storerow(0) + "' and vacation='false' and Weekly.PickupDay='Saturday' Then 1 Else Null End) As BountyPUSat, Count( Case when week='" + week + "' and Weekly.Bounty='True' and vacation='false' and pickedup='false' and location='" + Storerow(0) + "' Then 1 Else Null End) As BountyNPU, "
            SqlQuary += "Count(Case when week='" + week + "' and Weekly.Barnyard='True' and Location='" + Storerow(0) + "' and vacation='false' and Weekly.PickupDay='Thursday' Then 1 Else Null End) As BarnyardPUThu, Count( Case when week='" + week + "' and Weekly.Barnyard='True' and Location='" + Storerow(0) + "' and vacation='false' and Weekly.PickupDay='Friday' Then 1 Else Null End) As BarnyardPUFri, Count( Case when week='" + week + "' and Weekly.Barnyard='True' and Location='" + Storerow(0) + "' and vacation='false' and Weekly.PickupDay='Saturday' Then 1 Else Null End) As BarnyardPUSat, Count( Case when week='" + week + "' and Weekly.Barnyard='True' and vacation='false' and pickedup='false' and location='" + Storerow(0) + "' Then 1 Else Null End) As BarnyardNPU, "
            SqlQuary += "Count(Case when week='" + week + "' and Weekly.Ploughman='True' and Location='" + Storerow(0) + "' and vacation='false' and Weekly.PickupDay='Thursday' Then 1 Else Null End) As PloughmanPUThu, Count( Case when week='" + week + "' and Weekly.Ploughman='True' and Location='" + Storerow(0) + "' and vacation='false' and Weekly.PickupDay='Friday' Then 1 Else Null End) As PloughmanPUFri, Count( Case when week='" + week + "' and Weekly.Ploughman='True' and Location='" + Storerow(0) + "' and vacation='false' and Weekly.PickupDay='Saturday' Then 1 Else Null End) As PloughmanPUSat, Count( Case when week='" + week + "' and Weekly.Ploughman='True' and vacation='false' and pickedup='false' and location='" + Storerow(0) + "' Then 1 Else Null End) As PloughmanNPU, "
            SqlQuary += "Count( Case when week='" + week + "' And Weekly.Bounty = 'True' and vacation='true' and location='" + Storerow(0) + "' Then 1 Else Null End) As BountyVac, Count( Case when week='" + week + "' And Weekly.Barnyard = 'true' and vacation='true' and location='" + Storerow(0) + "' Then 1 Else Null End) As BarnVac, Count( Case when week='" + week + "' And Weekly.Ploughman = 'true' and vacation='true' and location='" + Storerow(0) + "' Then 1 Else Null End) As PloVac From weekly INNER JOIN Subscribers on weekly.SubId=Subscribers.SubId Where  Subscribers.Active = 'true'"
            Using mySqlConnection As New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
                mySqlCommand = New SqlCommand(SqlQuary, mySqlConnection)
                mySqlConnection.Open()
                myDataReader = mySqlCommand.ExecuteReader()
                If myDataReader.HasRows Then
                    Dim SubInfo As String = ""
                    Dim sql As String
                    Do While myDataReader.Read()
                        sql = "Update summary set Bounty='" + myDataReader.GetInt32(0).ToString + "' WHERE store='" + Storerow(0) + " Thursday PU'  Update summary set Bounty='" + myDataReader.GetInt32(1).ToString + "' WHERE store= '" + Storerow(0) + " Friday PU' Update summary set Bounty='" + myDataReader.GetInt32(2).ToString + "' WHERE store= '" + Storerow(0) + " Saturday PU' Update summary set bounty='" + myDataReader.GetInt32(3).ToString + "' WHERE store= '" + Storerow(0) + " NPUs'  "
                        sql += "Update summary set Barnyard='" + myDataReader.GetInt32(4).ToString + "' WHERE store='" + Storerow(0) + " Thursday PU'  Update summary set Barnyard='" + myDataReader.GetInt32(5).ToString + "' WHERE store= '" + Storerow(0) + " Friday PU' Update summary set Barnyard='" + myDataReader.GetInt32(6).ToString + "' WHERE store= '" + Storerow(0) + " Saturday PU' Update summary set Barnyard='" + myDataReader.GetInt32(7).ToString + "' WHERE store= '" + Storerow(0) + " NPUs' "
                        sql += "Update summary set Ploughman='" + myDataReader.GetInt32(8).ToString + "' WHERE store='" + Storerow(0) + " Thursday PU'  Update summary set Ploughman='" + myDataReader.GetInt32(9).ToString + "' WHERE store= '" + Storerow(0) + " Friday PU' Update summary set Ploughman='" + myDataReader.GetInt32(10).ToString + "' WHERE store= '" + Storerow(0) + " Saturday PU' Update summary set Ploughman='" + myDataReader.GetInt32(11).ToString + "' WHERE store= '" + Storerow(0) + " NPUs' "
                        sql += "Update summary set Bounty='" + myDataReader.GetInt32(12).ToString + "' WHERE store= '" + Storerow(0) + " Vacation' Update summary set Barnyard='" + myDataReader.GetInt32(13).ToString + "' WHERE store= '" + Storerow(0) + " Vacation' Update summary set Ploughman='" + myDataReader.GetInt32(14).ToString + "' WHERE store= '" + Storerow(0) + " Vacation' Update summary set Total='" + (myDataReader.GetInt32(12) + myDataReader.GetInt32(13) + myDataReader.GetInt32(14)).ToString + "' WHERE store= '" + Storerow(0) + " Vacation'"
                        sql += "Update summary set Bounty='" + (myDataReader.GetInt32(0) + myDataReader.GetInt32(1).ToString + myDataReader.GetInt32(2).ToString).ToString + "' WHERE store='" + Storerow(0) + "' Update summary set Barnyard='" + (myDataReader.GetInt32(4) + myDataReader.GetInt32(5).ToString + myDataReader.GetInt32(6).ToString).ToString + "' WHERE store='" + Storerow(0) + "' Update summary set Ploughman='" + (myDataReader.GetInt32(8) + myDataReader.GetInt32(9).ToString + myDataReader.GetInt32(10).ToString).ToString + "' WHERE store='" + Storerow(0) + "'"
                        sql += "Update summary set Total='" + (myDataReader.GetInt32(0) + myDataReader.GetInt32(1).ToString + myDataReader.GetInt32(2).ToString + myDataReader.GetInt32(4) + myDataReader.GetInt32(5).ToString + myDataReader.GetInt32(6).ToString + myDataReader.GetInt32(8) + myDataReader.GetInt32(9).ToString + myDataReader.GetInt32(10).ToString).ToString + "' WHERE store='" + Storerow(0) + "' Update summary set Total='" + (myDataReader.GetInt32(0) + myDataReader.GetInt32(4).ToString + myDataReader.GetInt32(8).ToString + myDataReader.GetInt32(12)).ToString + "' WHERE store='" + Storerow(0) + " Thursday PU' Update summary set Total='" + (myDataReader.GetInt32(1) + myDataReader.GetInt32(5).ToString + myDataReader.GetInt32(9).ToString + myDataReader.GetInt32(13)).ToString + "' WHERE store='" + Storerow(0) + " Friday PU' Update summary set Total='" + (myDataReader.GetInt32(2) + myDataReader.GetInt32(6).ToString + myDataReader.GetInt32(10).ToString + myDataReader.GetInt32(14)).ToString + "' WHERE store='" + Storerow(0) + " Saturday PU' Update summary set Total='" + (myDataReader.GetInt32(3) + myDataReader.GetInt32(7).ToString + myDataReader.GetInt32(11)).ToString + "' WHERE store='" + Storerow(0) + " NPUs'"
                        conn = New SqlConnection(ConnectionString)
                        conn.Open()
                        cmd = New SqlCommand(sql, conn)
                        cmd.ExecuteNonQuery()
                        cmd.Connection.Close()
                    Loop
                Else
                    Console.WriteLine("No rows found.")
                End If
                myDataReader.Close()
            End Using
        Next
        GridView1.DataBind()
    End Sub
    Function FillStoreInfo() As DataTable
        Dim dtStores As New DataTable()
        dtStores.Columns.Add("Store")
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
                        dtStores.Rows.Add(myDataReader.GetString(0))
                    Loop
                Else
                    Console.WriteLine("No rows found.")
                End If
                myDataReader.Close()
            End Using
        Finally
        End Try
        Return dtStores
    End Function
    Function FillDayInfo() As DataTable
        Dim dtDays As New DataTable()
        dtDays.Columns.Add("PickupDay")
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
                dtDays.Rows.Add(myDataReader.GetString(0))
                    Loop
                Else
                    Console.WriteLine("No rows found.")
                End If
                myDataReader.Close()
            End Using
        Finally
        End Try
        Return dtDays
    End Function

    Protected Sub WeekList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles WeekList.SelectedIndexChanged
        If Not WeekList.SelectedValue = " - Select a Week - " Then
            UpdateProgressContext()
            FillInfo()
        Else
            GridView1.Visible = False
        End If
    End Sub
    Private Sub UpdateProgressContext()
        Const Total As Integer = 67

        Dim progress As RadProgressContext = RadProgressContext.Current
        'progress.Speed = "N/A"

        For i As Integer = 0 To Total - 1
            progress.PrimaryTotal = 1
            progress.PrimaryValue = 1
            progress.PrimaryPercent = 100

            progress.SecondaryTotal = Total
            progress.SecondaryValue = i
            progress.SecondaryPercent = i

            progress.CurrentOperationText = "Step " & i.ToString()

            If Not Response.IsClientConnected Then
                'Cancel button was clicked or the browser was closed, so stop processing
                Exit For
            End If

            progress.TimeEstimated = (Total - i) * 100
            'Stall the current thread for 0.1 seconds
            System.Threading.Thread.Sleep(100)
        Next
    End Sub

    Protected Sub DownloadButton_Click(sender As Object, e As EventArgs) Handles DownloadButton.Click

        Response.Clear()
        Response.Buffer = True
        Dim fileName As String = "Weekly_Report_for_" + WeekList.SelectedValue.ToString
        Response.AddHeader("content-disposition", (Convert.ToString("attachment;filename=") & fileName) + ".xls")
        Response.ContentEncoding = System.Text.Encoding.Unicode
        Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())
        Response.ContentType = "application/vnd.ms-excel"
        Using sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)
            GridView1.Visible = True
            GridView1.RenderControl(hw)
            Response.Output.Write(sw.ToString())
            Response.Flush()
            Response.[End]()
        End Using

    End Sub

    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim box As String = e.Row.Cells(0).Text
            For Each cell As TableCell In e.Row.Cells
                cell.Visible = False
                For Each Dayrow In FillDayInfo.Rows
                    If box.Contains(Dayrow(0)) Then
                        cell.Visible = True
                    End If
                Next
                For Each Storerow In FillStoreInfo.Rows
                    If box = Storerow(0) Then
                        cell.Visible = True
                    End If
                Next
                If box.Contains("NPUs") Then
                    cell.Visible = True
                End If
                If box.Contains("Vacation") Then
                    cell.Visible = True
                End If
            Next
        End If
    End Sub
End Class
