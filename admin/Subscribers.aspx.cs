using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_Subscribers : System.Web.UI.Page
{

	string options = "";
	string active = "active='true'";
	DataTable Weeksdt = new DataTable();
	string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            SqlConnection cn = Constant.Connection();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM AllowAccess where UserID='" + Session[Constant.UserID].ToString() + "'", cn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CurrentSubscribers"].ToString() == "False")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Error();", true);
                    return;
                }
                else
                {
                    SqlDataReader myDataReader = default(SqlDataReader);
                    SqlConnection mySqlConnection = default(SqlConnection);
                    SqlCommand mySqlCommand = default(SqlCommand);
                    mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                    mySqlCommand = new SqlCommand("SELECT Role FROM userinfo Where Username= '" + Membership.GetUser().ToString() + "'", mySqlConnection);
                    try
                    {
                        mySqlConnection.Open();
                        myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                        while ((myDataReader.Read()))
                        {
                            string role = myDataReader.GetString(0);
                            if (role == "Admin")
                            {
                                Session["Role"] = "Admin";
                            }
                            else if (role == "Employee")
                            {
                                Session["Role"] = "Employee";
                            }
                            else
                            {
                                Response.Redirect("~/account/");
                            }
                        }
                    }
                    finally
                    {
                        if ((mySqlConnection.State == ConnectionState.Open))
                        {
                            mySqlConnection.Close();
                        }
                    }
                    FillWeekInfo();
                    FillDayInfo();
                    FillStoreInfo();
                    FillInfo();
                }
            }
        }
    }

    protected void FillInfo()
    {
        string week = null;
        if (!(WeekList.SelectedValue == " - Select a Week - "))
        {
            week = WeekList.SelectedValue;
            string pattern = "-(.*?)/";
            string replacement = "/" + "\r\n";
            Regex rgx = new Regex(pattern, RegexOptions.Singleline);
            week = rgx.Replace(week, replacement);
            if (!string.IsNullOrEmpty(options))
            {
                week = "and weekly.week='" + (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "") + "' and ";
            }
            else
            {
                week = "and weekly.week='" + (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "") + "'";
            }

        }
        else
        {
            if (!string.IsNullOrEmpty(options))
            {
                week = "and ";
            }
            else
            {
                week = "";
            }
        }
        //Dim SqlQuary As String = "SELECT DISTINCT subscribers.SubId, subscribers.firstname1, subscribers.lastname1, subscribers.firstname2, subscribers.lastname2, Weekly.paidbounty, Weekly.paidbarnyard, Weekly.paidploughman, Weekly.pickedup, Weekly.vacation, weekly.bounty, weekly.barnyard, weekly.ploughman FROM Weekly INNER JOIN subscribers ON weekly.SubID=subscribers.SubId where active='true' " + week + options + " ORDER BY LastName1, FirstName1"
        string SqlQuary = "SELECT DISTINCT subscribers.SubId, subscribers.firstname1, subscribers.lastname1, subscribers.firstname2, subscribers.lastname2 FROM Weekly INNER JOIN subscribers ON weekly.SubID=subscribers.SubId where " + active + " " + week + options + " ORDER BY LastName1, FirstName1";
        DataTable dt = new DataTable();
        dt.Columns.Add("SubId");
        dt.Columns.Add("FirstName1");
        dt.Columns.Add("LastName1");
        dt.Columns.Add("FirstName2");
        dt.Columns.Add("LastName2");
        //dt.Columns.Add("paidbounty")
        //dt.Columns.Add("paidbarnyard")
        //dt.Columns.Add("paidploughman")
        //dt.Columns.Add("pickedup")
        //dt.Columns.Add("vacation")
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        try
        {
            using (mySqlConnection)
            {
                mySqlCommand = new SqlCommand(SqlQuary, mySqlConnection);
                mySqlConnection.Open();
                myDataReader = mySqlCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    string SubInfo = "";
                    string paid = "";
                    string pickedup = "";
                    string vacation = "";
                    int i = 0;
                    int j = 0;
                    while (myDataReader.Read())
                    {
                        //If myDataReader.GetBoolean(10) = True Then
                        //    i += 1
                        //End If
                        //If myDataReader.GetBoolean(11) = True Then
                        //    i += 1
                        //End If
                        //If myDataReader.GetBoolean(12) = True Then
                        //    i += 1
                        //End If
                        //If myDataReader.GetBoolean(5) = True Then
                        //    j += 1
                        //End If
                        //If myDataReader.GetBoolean(6) = True Then
                        //    j += 1
                        //End If
                        //If myDataReader.GetBoolean(7) = True Then
                        //    j += 1
                        //End If
                        //If i = j Then
                        //    paid = ""
                        //Else
                        //    paid = "~/images/Pay.png"
                        //End If
                        //If myDataReader.GetBoolean(8) = True Then
                        //    pickedup = ""
                        //Else
                        //    pickedup = "~/images/Pickup.png"
                        //End If
                        //If myDataReader.GetBoolean(9) = True Then
                        //    vacation = ""
                        //Else
                        //    vacation = "~/images/Vacation.png"
                        //End If
                        //dt.Rows.Add(myDataReader.GetInt32(0), myDataReader.GetString(1), myDataReader.GetString(2), myDataReader.GetString(3), myDataReader.GetString(4), paid, paid, paid, pickedup, vacation)
                        dt.Rows.Add(myDataReader.GetInt32(0), myDataReader.GetString(1), myDataReader.GetString(2), myDataReader.GetString(3), myDataReader.GetString(4));
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                myDataReader.Close();
            }
        }
        finally
        {
        }
        GridView1.DataSource = dt;
        GridView1.DataBind();
        tableUPanel.Update();
    }
    protected void FillWeekInfo()
    {
        Weeksdt.Columns.Add("Week");
        Weeksdt.Rows.Add(" - Select a Week - ");
        SqlDataReader myDataReader2 = default(SqlDataReader);
        SqlConnection mySqlConnection2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        SqlCommand mySqlCommand2 = default(SqlCommand);
        string SDateRange = "";
        string query = "select Sstart, send from seasons order by sstart";
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (mySqlConnection2)
                {
                    mySqlCommand2 = new SqlCommand(query, mySqlConnection2);
                    mySqlConnection2.Open();
                    myDataReader2 = mySqlCommand2.ExecuteReader();
                    int i = 0;
                    if (myDataReader2.HasRows)
                    {
                        while (myDataReader2.Read())
                        {
                            if (!(i == 0))
                            {
                                SDateRange += " or ";
                            }
                            SDateRange += "(week >= '" + myDataReader2.GetDateTime(0) + "' and week <= '" + myDataReader2.GetDateTime(1) + "')";
                            i += 1;
                        }
                        FillWeekInfo2(SDateRange);
                    }
                    myDataReader2.Close();
                }
            }
        }
        finally
        {
        }
        this.WeekList.DataSource = Weeksdt;
        this.WeekList.DataTextField = "Week";
        this.WeekList.DataValueField = "Week";
        this.WeekList.DataBind();
        DateTime SelDate = System.DateTime.Now;
        if (System.DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
        {
            SelDate = SelDate.AddDays(4);
        }
        else if (System.DateTime.Now.DayOfWeek == DayOfWeek.Monday)
        {
            SelDate = SelDate.AddDays(3);
        }
        else if (System.DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
        {
            SelDate = SelDate.AddDays(2);
        }
        else if (System.DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
        {
            SelDate = SelDate.AddDays(1);
        }
        else if (System.DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
        {
        }
        else if (System.DateTime.Now.DayOfWeek == DayOfWeek.Friday)
        {
            SelDate = SelDate.AddDays(-1);
        }
        else if (System.DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
        {
            SelDate = SelDate.AddDays(-2);
        }
        WeekList.SelectedValue = SelDate.Month.ToString() + "/" + SelDate.Day.ToString() + "-" + SelDate.AddDays(1).Day.ToString() + "/" + SelDate.Year.ToString();
    }
    public void FillWeekInfo2(string daterange)
    {
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        try
        {
            using (mySqlConnection)
            {
                mySqlCommand = new SqlCommand("SELECT DISTINCT Week FROM Weekly where " + daterange + " ORDER BY [Week]", mySqlConnection);
                mySqlConnection.Open();

                myDataReader = mySqlCommand.ExecuteReader();

                if (myDataReader.HasRows)
                {
                    while (myDataReader.Read())
                    {
                        if (!(myDataReader.GetDateTime(0).Year.ToString() == "1900"))
                        {
                            Weeksdt.Rows.Add(myDataReader.GetDateTime(0).Month.ToString() + "/" + myDataReader.GetDateTime(0).Day.ToString() + "-" + myDataReader.GetDateTime(0).AddDays(1).Day.ToString() + "/" + myDataReader.GetDateTime(0).Year.ToString());
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                myDataReader.Close();
            }
        }
        finally
        {
        }
        return;


    }
    protected void FillStoreInfo()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Store");
        dt.Rows.Add(" - Select a Store - ");
        //Create Rows in DataTable
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        try
        {
            using (mySqlConnection)
            {
                mySqlCommand = new SqlCommand("SELECT Store FROM Stores", mySqlConnection);
                mySqlConnection.Open();

                myDataReader = mySqlCommand.ExecuteReader();

                if (myDataReader.HasRows)
                {
                    while (myDataReader.Read())
                    {
                        dt.Rows.Add(myDataReader.GetString(0));
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                myDataReader.Close();
            }
        }
        finally
        {
        }
        this.StoreList.DataSource = dt;
        this.StoreList.DataTextField = "store";
        this.StoreList.DataValueField = "store";
        this.StoreList.DataBind();
    }
    protected void FillDayInfo()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PickupDay");
        dt.Rows.Add(" - Select a Pickup Day - ");
        //Create Rows in DataTable
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        try
        {
            using (mySqlConnection)
            {
                mySqlCommand = new SqlCommand("SELECT PickupDay FROM PickupDays", mySqlConnection);
                mySqlConnection.Open();

                myDataReader = mySqlCommand.ExecuteReader();

                if (myDataReader.HasRows)
                {
                    while (myDataReader.Read())
                    {
                        dt.Rows.Add(myDataReader.GetString(0));
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                myDataReader.Close();
            }
        }
        finally
        {
        }
        this.PickupDayList.DataSource = dt;
        this.PickupDayList.DataTextField = "PickupDay";
        this.PickupDayList.DataValueField = "PickupDay";
        this.PickupDayList.DataBind();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string ID = GridView1.DataKeys[e.Row.RowIndex].Values[0].ToString();
            SqlDataReader myDataReader = default(SqlDataReader);
            SqlConnection mySqlConnection = default(SqlConnection);
            SqlCommand mySqlCommand = default(SqlCommand);
            string week = "";
            if (!(WeekList.SelectedValue == " - Select a Week - "))
            {
                week = WeekList.SelectedValue;
                string pattern = "-(.*?)/";
                string replacement = "/" + "\r\n";
                Regex rgx = new Regex(pattern, RegexOptions.Singleline);
                week = rgx.Replace(week, replacement);
                week = "and week='" + (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "") + "'";
            }
            mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            mySqlCommand = new SqlCommand("SELECT weekly.paid, weekly.pickedup, weekly.vacation, subscribers.vacUsed FROM Weekly INNER JOIN subscribers ON weekly.SubID=subscribers.SubId Where subscribers.SubId= '" + ID + "' " + week + "", mySqlConnection);
            ImageButton payButton = e.Row.FindControl("btnpay") as ImageButton;
            ImageButton pickupButton = e.Row.FindControl("pickedup") as ImageButton;
            ImageButton vacationButton = e.Row.FindControl("btnvac") as ImageButton;
            Button ForcevacationButton = e.Row.FindControl("ForceVac") as Button;
            bool isVacation = false;
            try
            {
                mySqlConnection.Open();
                myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while ((myDataReader.Read()))
                {
                    //If myDataReader.GetBoolean(0) = True Then
                    //    'payButton.Visible = False
                    //End If
                    if (myDataReader.GetBoolean(1) == true)
                    {
                        pickupButton.Visible = true;
                    }
                    else
                    {
                        pickupButton.Visible = false;
                    }
                    isVacation = myDataReader.GetBoolean(2);
                    if (myDataReader.GetInt32(3) > 1)
                    {
                        //vacationButton.Visible = false;
                        
                        if (Session["Role"] == "Admin")
                        {
                            //ForcevacationButton.Visible = true;
                            
                        }
                        else
                        {
                            //ForcevacationButton.Visible = false;
                        }
                    }
                    else
                    {
                        //ForcevacationButton.Visible = false;
                    }

                }
            }
            finally
            {
                if ((mySqlConnection.State == ConnectionState.Open))
                {
                    mySqlConnection.Close();
                }
            }
        }
    }
    protected void GridView1_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
            string SI = GridView1.DataKeys[row.RowIndex].Value.ToString();
            if (e.CommandName == "Pay")
            {
                Response.Redirect("Pay.aspx?s=" + SI + "&week=" + WeekList.SelectedItem.Text);
            }
            else if (e.CommandName == "Vacation")
            {
                Response.Redirect("Vacation.aspx?s=" + SI);
            }
            else if (e.CommandName == "ForceVacation")
            {
                Response.Redirect("Vacation.aspx?s=" + SI + "&F=Y");
            }
            else if (e.CommandName == "Details")
            {
                Response.Redirect("Details.aspx?s=" + SI + "&week=" + WeekList.SelectedItem.Text);
            }
        }
        catch (Exception ex)
        {
            lit1.Text = ex.StackTrace + "<br />" + ex.StackTrace + "<br />Row: " + GridView1.SelectedIndex.ToString();
        }
    }

    protected void WeekList_SelectedIndexChanged(object sender, EventArgs e)
    {
       
    }
    protected void StoreList_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }

    protected void PickupDay_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!(PickupDayList.SelectedValue == " - Select a Pickup Day - "))
        {
            if (PickupDayList.SelectedValue == "Friday")
            {
                if (StoreList.SelectedValue == "Jefferson City")
                {
                    
                }
                else
                {
                    PUDLiteral.Text = "";
                }
            }
            else
            {
                PUDLiteral.Text = "";
            }
            if (!(StoreList.SelectedValue == " - Select a Store - "))
            {
                options = "(weekly.PickupDay = '" + PickupDayList.SelectedValue + "') and (weekly.location = '" + StoreList.SelectedValue + "')";
            }
            else
            {
                options = "(weekly.PickupDay = '" + PickupDayList.SelectedValue + "')";
            }
        }
        else if (!(StoreList.SelectedValue == " - Select a Store - "))
        {
            options = "(weekly.location = '" + StoreList.SelectedValue + "')";
        }
        FillInfo();
    }

    protected void Showinactive_CheckedChanged(object sender, EventArgs e)
    {
        if (Showinactive.Checked == true)
        {
            active = "active='false'";
        }
        else if (Showinactive.Checked == false)
        {
            active = "active='true'";
        }
        FillInfo();
    }

    /**********************Changed*************/
    protected void WeekList_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (!(WeekList.SelectedValue == " - Select a Week - "))
        {
            lit1.Text = "";
            GridView1.Visible = true;
            FillInfo();
        }
        else
        {
            lit1.Text = "<h2>Please select a week</h2>";
            GridView1.Visible = false;
        }
    }
    protected void StoreList_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (!(StoreList.SelectedValue == " - Select a Store - "))
        {
            if (StoreList.SelectedValue == "Jefferson City")
            {
                if (PickupDayList.SelectedValue == "Friday")
                {
                    
                }
                else
                {
                    PUDLiteral.Text = "";
                }
            }
            else
            {
                PUDLiteral.Text = "";
            }
            if (!(PickupDayList.SelectedValue == " - Select a Pickup Day - "))
            {
                options = "(weekly.location = '" + StoreList.SelectedValue + "') and (weekly.PickupDay = '" + PickupDayList.SelectedValue + "')";
            }
            else
            {
                options = "(weekly.location = '" + StoreList.SelectedValue + "')";
            }
        }
        else if (!(PickupDayList.SelectedValue == " - Select a Pickup Day - "))
        {
            options = "(weekly.PickupDay = '" + PickupDayList.SelectedValue + "')";
        }
        FillInfo();
    }
    protected void PickupDayList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}