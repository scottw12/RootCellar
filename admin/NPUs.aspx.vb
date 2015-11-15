Imports System.Data.SqlClient
Imports System.Data

Partial Class admin_NPUs
    Inherits System.Web.UI.Page

    Dim Options As String = ""
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
            SqlDataSource1.SelectCommand = "SELECT [LastName1], [FirstName1], [phone1], [Bounty], [BarnYard], [Ploughman], [PickupDay], [Store] FROM [Subscribers] Where Active='true' ORDER BY [LastName1], [FirstName1]"
        End If
    End Sub

    Protected Sub StoreList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles StoreList.SelectedIndexChanged
        If Not StoreList.SelectedValue = "" Then
            If Not PickupDay.SelectedValue = "" Then
                Options = "and (Store = '" + StoreList.SelectedValue + "') and (PickupDay = '" + PickupDay.SelectedValue + "')"
            Else
                Options = "and (Store = '" + StoreList.SelectedValue + "')"
            End If
        ElseIf Not PickupDay.SelectedValue = "" Then
            Options = "and (PickupDay = '" + PickupDay.SelectedValue + "')"
        End If
        LoadGrid()
    End Sub

    Protected Sub PickupDay_SelectedIndexChanged(sender As Object, e As EventArgs) Handles PickupDay.SelectedIndexChanged
        If Not PickupDay.SelectedValue = "" Then
            If Not StoreList.SelectedValue = "" Then
                Options = "and (PickupDay = '" + PickupDay.SelectedValue + "') and (Store = '" + StoreList.SelectedValue + "')"
            Else
                Options = "and (PickupDay = '" + PickupDay.SelectedValue + "')"
            End If
        ElseIf Not StoreList.SelectedValue = "" Then
            Options = "and (Store = '" + StoreList.SelectedValue + "')"
        End If
        LoadGrid()
    End Sub

    Protected Sub LoadGrid()
        SqlDataSource1.SelectCommand = "SELECT [LastName1], [FirstName1], [phone1], [Bounty], [BarnYard], [Ploughman], [PickupDay], [Store] FROM Subscribers WHERE (Active = 'True') " + Options + " ORDER BY [LastName1], [FirstName1]"
        GridView1.DataBind()
    End Sub
End Class
