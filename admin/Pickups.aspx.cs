
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Drawing;
using System.Net.Mail;
using System.Configuration;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Web.UI;

partial class admin_Pickups : System.Web.UI.Page
{

    private SqlConnection conn = null;
    string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    private SqlCommand cmd = null;
    string Options = "";
    System.DateTime ThursdayPickup;

    System.DateTime FridayPickup;

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
                if (ds.Tables[0].Rows[0]["WeeklyPickups"].ToString() == "False")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Error();", true);
                    return;
                }

                else
                {
                    FillWeekInfo();
                    FillStoreInfo();
                    FillDayInfo();
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
                            }
                            else if (role == "Employee")
                            {
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
                }
            }
        }
    }
    protected void FillInfo()
    {
        if (!(WeekList.SelectedValue == " - Select a Week - "))
        {
            string week = WeekList.SelectedValue;
            string pattern = "-(.*?)/";
            string replacement = "/" + "\r\n";
            Regex rgx = new Regex(pattern, RegexOptions.Singleline);
            week = rgx.Replace(week, replacement);
            week = (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "");
            string SqlQuary = "SELECT DISTINCT subscribers.firstname1, subscribers.lastname1, subscribers.allergies, weekly.bounty, weekly.barnyard, weekly.ploughman, weekly.paidBounty, weekly.paidBarnyard, weekly.paidPloughman, weekly.pickedup, phone1, subscribers.SubId, weekly.vacation FROM Weekly inner JOIN subscribers ON weekly.SubID=subscribers.SubId where week='" + week + "'" + Options + "and subscribers.active='true' ORDER BY subscribers.LastName1, subscribers.FirstName1";
            //string SqlQuary = "SELECT DISTINCT subscribers.firstname1, subscribers.lastname1, subscribers.allergies, weekly.bounty, weekly.barnyard, weekly.ploughman, weekly.paidBounty, weekly.paidBarnyard, weekly.paidPloughman, weekly.pickedup, phone1, subscribers.SubId, weekly.vacation FROM Weekly inner JOIN subscribers ON weekly.SubID=subscribers.SubId where week='" + week + "'" + Options + "and subscribers.active='true' ORDER BY subscribers.LastName1, subscribers.FirstName1";
            DataTable dt = new DataTable();
            dt.Columns.Add("SubID");
            dt.Columns.Add("FirstName1");
            dt.Columns.Add("LastName1");
            dt.Columns.Add("allergies");
            dt.Columns.Add("bounty");
            dt.Columns.Add("barnyard");
            dt.Columns.Add("ploughman");
            dt.Columns.Add("paidBounty");
            dt.Columns.Add("paidBarnyard");
            dt.Columns.Add("paidPloughman");
            dt.Columns.Add("pickedup");
            dt.Columns.Add("phone");
            dt.Columns.Add("bountyvac");
            dt.Columns.Add("barnyardvac");
            dt.Columns.Add("ploughmanvac");

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
                        bool Bounty = false;
                        bool Barnyard = false;
                        bool Ploughman = false;
                        bool paidBounty = false;
                        bool paidBarnyard = false;
                        bool paidPloughman = false;
                        bool pickedup = false;
                        bool Vacation = false;
                        bool Bountyvac = false;
                        bool Barnyardvac = false;
                        bool Ploughmanvac = false;
                        while (myDataReader.Read())
                        {
                            //If myDataReader.GetBoolean(6) = True Then
                            //    paidBounty = "~/images/apptrue.gif"
                            //Else
                            //    paidBounty = "~/images/appfalse.gif"
                            //End If
                            Bounty = myDataReader.GetBoolean(3);
                            Barnyard = myDataReader.GetBoolean(4);
                            Ploughman = myDataReader.GetBoolean(5);
                            Vacation = myDataReader.GetBoolean(12);
                            if (Bounty == true & Vacation == true)
                            {
                                Bountyvac = true;
                            }
                            else
                            {
                                Bountyvac = false;
                            }
                            if (Barnyard == true & Vacation == true)
                            {
                                Barnyardvac = true;
                            }
                            else
                            {
                                Barnyardvac = false;
                            }
                            if (Ploughman == true & Vacation == true)
                            {
                                Ploughmanvac = true;
                            }
                            else
                            {
                                Ploughmanvac = false;
                            }
                            if (myDataReader.GetBoolean(6) == true)
                            {
                                paidBounty = false;
                            }
                            else if (myDataReader.GetBoolean(6) == false & Bounty == true)
                            {
                                paidBounty = true;
                            }
                            else
                            {
                                paidBounty = false;
                            }
                            if (myDataReader.GetBoolean(7) == true)
                            {
                                paidBarnyard = false;
                            }
                            else if (myDataReader.GetBoolean(7) == false & Barnyard == true)
                            {
                                paidBarnyard = true;
                            }
                            else
                            {
                                paidBarnyard = false;
                            }
                            if (myDataReader.GetBoolean(8) == true)
                            {
                                paidPloughman = false;
                            }
                            else if (myDataReader.GetBoolean(8) == false & Ploughman == true)
                            {
                                paidPloughman = true;
                            }
                            else
                            {
                                paidPloughman = false;
                            }
                            if (myDataReader.GetBoolean(9) == true)
                            {
                                pickedup = false;
                            }
                            else
                            {
                                pickedup = true;
                            }
                            if (Vacation == true)
                            {
                                Bounty = false;
                                Barnyard = false;
                                Ploughman = false;
                                pickedup = false;
                            }
                            dt.Rows.Add(myDataReader.GetInt32(11), myDataReader.GetString(0), myDataReader.GetString(1), myDataReader.GetString(2), Bounty, Barnyard, Ploughman, paidBounty, paidBarnyard, paidPloughman,
                            pickedup, myDataReader.GetString(10), Bountyvac, Barnyardvac, Ploughmanvac);

                            //SqlConnection cn = Constant.Connection();
                            //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM PickupChange WHERE CustomerID='" + myDataReader.GetInt32(11) + "'", cn);
                            //DataSet ds = new DataSet();
                            //da.Fill(ds);
                            //if (ds.Tables[0].Rows.Count > 0)
                            //{
                            //    dt.Rows.Add(myDataReader.GetInt32(11), myDataReader.GetString(0), myDataReader.GetString(1), myDataReader.GetString(2), Bounty, Barnyard, Ploughman, paidBounty, paidBarnyard, paidPloughman, pickedup, myDataReader.GetString(10), Bountyvac, Barnyardvac, Ploughmanvac);
                            //}
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
        }
    }
    protected void FillWeekInfo()
    {
        SqlDataReader myDataReader2 = default(SqlDataReader);
        SqlConnection mySqlConnection2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        SqlCommand mySqlCommand2 = default(SqlCommand);
        string SDateRange = "";
        string query = "select Sstart, send from seasons where currents='true'";
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (mySqlConnection2)
                {
                    mySqlCommand2 = new SqlCommand(query, mySqlConnection2);
                    mySqlConnection2.Open();
                    myDataReader2 = mySqlCommand2.ExecuteReader();
                    if (myDataReader2.HasRows)
                    {
                        while (myDataReader2.Read())
                        {
                            SDateRange = "where week>='" + myDataReader2.GetDateTime(0) + "' and week <= '" + myDataReader2.GetDateTime(1) + "' ";
                        }
                    }
                    myDataReader2.Close();
                }
            }
        }
        finally
        {
        }
        DataTable dt = new DataTable();
        dt.Columns.Add("Week");
        dt.Rows.Add(" - Select a Week - ");
        //Create Rows in DataTable
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        try
        {
            using (mySqlConnection)
            {
                mySqlCommand = new SqlCommand("SELECT DISTINCT Week FROM weekly " + SDateRange + " order by week", mySqlConnection);
                mySqlConnection.Open();

                myDataReader = mySqlCommand.ExecuteReader();

                if (myDataReader.HasRows)
                {
                    while (myDataReader.Read())
                    {
                        if (!(myDataReader.GetDateTime(0).Year.ToString() == "1900"))
                        {
                            dt.Rows.Add(myDataReader.GetDateTime(0).Month.ToString() + "/" + myDataReader.GetDateTime(0).Day.ToString() + "-" + myDataReader.GetDateTime(0).AddDays(1).Day.ToString() + "/" + myDataReader.GetDateTime(0).Year.ToString());
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
        this.WeekList.DataSource = dt;
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
        if (!(WeekList.SelectedValue == " - Select a Week - "))
        {
            FillInfo();
        }
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
    protected void WeekList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!(WeekList.SelectedValue == " - Select a Week - "))
        {
            Literal1.Text = "";
            GridView1.Visible = true;
            FillInfo();
        }
        else
        {
            Literal1.Text = "<h2>Please select a week</h2>";
            GridView1.Visible = false;
        }
        
    }
    protected void StoreList_SelectedIndexChanged(object sender, EventArgs e)
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
                Options = "and (weekly.location = '" + StoreList.SelectedValue + "') and (weekly.PickupDay = '" + PickupDayList.SelectedValue + "')";
            }
            else
            {

                Options = "and (weekly.location = '" + StoreList.SelectedValue + "')";
            }
        }
        else if (!(PickupDayList.SelectedValue == " - Select a Pickup Day - "))
        {
            Options = "and (weekly.PickupDay = '" + PickupDayList.SelectedValue + "')";
        }
        if (NPUCheck.Checked == true)
        {
            Options += " and pickedup='false' and vacation='false'";
        }
        if (!(WeekList.SelectedValue == " - Select a Week - ") & !(StoreList.SelectedValue == " - Select a Store - ") & !(PickupDayList.SelectedValue == " - Select a Pickup Day - "))
        {
            ReminderButton.Visible = true;
        }
        else
        {
            ReminderButton.Visible = false;
        }
        FillInfo();
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
                Options = "and (weekly.PickupDay = '" + PickupDayList.SelectedValue + "') and (weekly.location = '" + StoreList.SelectedValue + "')";
            }
            else
            {
                Options = "and (weekly.PickupDay = '" + PickupDayList.SelectedValue + "')";
            }
        }
        else if (!(StoreList.SelectedValue == " - Select a Store - "))
        {
            Options = "and (weekly.location = '" + StoreList.SelectedValue + "')";
        }
        if (NPUCheck.Checked == true)
        {
            Options += " and pickedup='false' and vacation='false'";
        }
        if (!(WeekList.SelectedValue == " - Select a Week - ") & !(StoreList.SelectedValue == " - Select a Store - ") & !(PickupDayList.SelectedValue == " - Select a Pickup Day - "))
        {
            ReminderButton.Visible = true;
        }
        else
        {
            ReminderButton.Visible = false;
        }
        FillInfo();
    }
    protected void GetPickupDay()
    {
        if (System.DateTime.Today.ToString("dddd") == "Sunday")
        {
            ThursdayPickup = System.DateTime.Today.AddDays(4);
            FridayPickup = System.DateTime.Today.AddDays(5);
        }
        else if (System.DateTime.Today.ToString("dddd") == "Monday")
        {
            ThursdayPickup = System.DateTime.Today.AddDays(3);
            FridayPickup = System.DateTime.Today.AddDays(4);
        }
        else if (System.DateTime.Today.ToString("dddd") == "Tuesday")
        {
            ThursdayPickup = System.DateTime.Today.AddDays(2);
            FridayPickup = System.DateTime.Today.AddDays(3);
        }
        else if (System.DateTime.Today.ToString("dddd") == "Wednesday")
        {
            ThursdayPickup = System.DateTime.Today.AddDays(1);
            FridayPickup = System.DateTime.Today.AddDays(2);
        }
        else if (System.DateTime.Today.ToString("dddd") == "Thursday")
        {
            ThursdayPickup = System.DateTime.Today;
            FridayPickup = System.DateTime.Today.AddDays(1);
        }
        else if (System.DateTime.Today.ToString("dddd") == "Friday")
        {
            ThursdayPickup = System.DateTime.Today.AddDays(6);
            FridayPickup = System.DateTime.Today;
        }
        {
            ThursdayPickup = System.DateTime.Today.AddDays(5);
            FridayPickup = System.DateTime.Today.AddDays(6);
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal BountySub = e.Row.FindControl("BountySub") as Literal;
            Literal BarnyardSub = e.Row.FindControl("BarnyardSub") as Literal;
            Literal PloughmanSub = e.Row.FindControl("PloughmanSub") as Literal;
            Literal BountyVac = e.Row.FindControl("BountyVac") as Literal;
            Literal BarnyardVac = e.Row.FindControl("BarnyardVac") as Literal;
            Literal PloughmanVac = e.Row.FindControl("PloughmanVac") as Literal;
            ImageButton BountypayButton = e.Row.FindControl("btnpayBounty") as ImageButton;
            ImageButton BarnyardpayButton = e.Row.FindControl("btnpayBarnyard") as ImageButton;
            ImageButton PloughmanpayButton = e.Row.FindControl("btnpayPloughman") as ImageButton;
            ImageButton Pickup = e.Row.FindControl("pickup") as ImageButton;
            foreach (TableCell cell in e.Row.Cells)
            {
                cell.BackColor = Color.White;
                if (BountySub.Visible == true & BountypayButton.Visible == false)
                {
                    cell.BackColor = Color.Yellow;
                }
                if (BarnyardSub.Visible == true & BarnyardpayButton.Visible == false)
                {
                    cell.BackColor = Color.Yellow;
                }
                if (PloughmanSub.Visible == true & PloughmanpayButton.Visible == false)
                {
                    cell.BackColor = Color.Yellow;
                }
                if (Pickup.Visible == false)
                {
                    cell.BackColor = Color.Green;
                    Pickup.Visible = false;
                }
                else
                {
                    Pickup.Visible = true;
                }
                if (BountyVac.Visible == true | BarnyardVac.Visible == true | PloughmanVac.Visible == true)
                {
                    cell.BackColor = Color.Red;
                    Pickup.Visible = false;
                    BountypayButton.Visible = false;
                    BarnyardpayButton.Visible = false;
                    PloughmanpayButton.Visible = false;
                }
            }
        }
    }
    protected void GridView1_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        //GridViewRow row = (GridViewRow)e.CommandSource;
        GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
        string SI = GridView1.DataKeys[row.RowIndex].Value.ToString();
        try
        {
            if (e.CommandName == "PayBounty")
            {
                Response.Redirect("pay.aspx?s=" + SI + "&B=Bounty");
            }
            else if (e.CommandName == "PayBarnyard")
            {
                Response.Redirect("pay.aspx?s=" + SI + "&B=Barnyard");
            }
            else if (e.CommandName == "PayPloughman")
            {
                Response.Redirect("pay.aspx?s=" + SI + "&B=Ploughman");
            }
            else if (e.CommandName == "Pickup")
            {
                Response.Redirect("pickup.aspx?s=" + SI + "&week=" + WeekList.SelectedItem.Text);
            }
            else if (e.CommandName == "ViewProduct")
            {
                Response.Redirect("ViewProductDetails.aspx?s=" + SI);
            }
        }
        catch (Exception ex)
        {
        }
    }


    protected void NPUCheck_CheckedChanged(object sender, EventArgs e)
    {
        if (NPUCheck.Checked == true)
        {
            if (!(StoreList.SelectedValue == " - Select a Store - "))
            {
                if (!(PickupDayList.SelectedValue == " - Select a Pickup Day - "))
                {
                    Options = "and (weekly.location = '" + StoreList.SelectedValue + "') and (weekly.PickupDay = '" + PickupDayList.SelectedValue + "') and pickedup='false' and vacation='false'";
                }
                else
                {
                    Options = "and (weekly.location = '" + StoreList.SelectedValue + "') and pickedup='false' and vacation='false'";
                }
            }
            else if (!(PickupDayList.SelectedValue == " - Select a Pickup Day - "))
            {
                Options = "and (weekly.PickupDay = '" + PickupDayList.SelectedValue + "') and pickedup='false' and vacation='false'";
            }
            else
            {
                Options = " and pickedup='false' and vacation='false'";
            }
        }
        else
        {
            if (!(StoreList.SelectedValue == " - Select a Store - "))
            {
                if (!(PickupDayList.SelectedValue == " - Select a Pickup Day - "))
                {
                    Options = "and (weekly.location = '" + StoreList.SelectedValue + "') and (weekly.PickupDay = '" + PickupDayList.SelectedValue + "')";
                }
                else
                {
                    Options = "and (weekly.location = '" + StoreList.SelectedValue + "')";
                }
            }
            else if (!(PickupDayList.SelectedValue == " - Select a Pickup Day - "))
            {
                Options = "and (weekly.PickupDay = '" + PickupDayList.SelectedValue + "')";
            }
            else
            {
                Options = "";
            }
        }

        FillInfo();
    }

    protected void ReminderButton_Click(object sender, EventArgs e)
    {
        try
        {
            string week = WeekList.SelectedValue;
            string pattern = "-(.*?)/";
            string replacement = "/" + "\r\n";
            Regex rgx = new Regex(pattern, RegexOptions.Singleline);
            week = rgx.Replace(week, replacement);
            week = (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "");
            string SqlQuary = "SELECT DISTINCT subscribers.firstname1, subscribers.email1, subscribers.SubId FROM Weekly INNER JOIN subscribers ON weekly.SubID=subscribers.SubId where week='" + week + "' and (weekly.location = '" + StoreList.SelectedValue + "') and (weekly.PickupDay = '" + PickupDayList.SelectedValue + "') and active='true' and vacation='false' and pickedup='false' ORDER BY subscribers.FirstName1";
            DataTable dt = new DataTable();
            dt.Columns.Add("SubID");
            dt.Columns.Add("FirstName1");
            dt.Columns.Add("email");
            SqlDataReader myDataReader = default(SqlDataReader);
            SqlConnection mySqlConnection = default(SqlConnection);
            SqlCommand mySqlCommand = default(SqlCommand);
            mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (mySqlConnection)
            {
                mySqlCommand = new SqlCommand(SqlQuary, mySqlConnection);
                mySqlConnection.Open();
                myDataReader = mySqlCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    string SubInfo = "";
                    string pickedup = "";
                    while (myDataReader.Read())
                    {
                        dt.Rows.Add(myDataReader.GetInt32(2), myDataReader.GetString(0), myDataReader.GetString(1));
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                myDataReader.Close();
            }
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    //MailMessage oMail0 = new MailMessage();
                    //oMail0.From = new MailAddress("Root Cellar <website@rdollc.com>");
                    //oMail0.To.Add(new MailAddress(row["email"].ToString().Replace("'", "").Replace("\"", "").Replace(" ", "")));
                    //oMail0.Subject = "Root Cellar Subscription ";
                    //oMail0.Priority = MailPriority.High;
                    //oMail0.IsBodyHtml = true;
                    //oMail0.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
                    string oMail0Body = "<head><title></title></head>";
                    oMail0Body += "<body>";
                    oMail0Body += "Hello " + row["FirstName1"] + ",<br /><br />";
                    oMail0Body += "This is a friendly reminder that you forgot to pick up your box today. We will be open tomorrow, please come pick up your box at your earliest convenience";
                    oMail0Body += "Thank you!<br /> Root Cellar!";
                    oMail0Body += "</body>";
                    oMail0Body += "</html>";
                    //AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail0.Body, null, "text/html");
                    //oMail0.AlternateViews.Add(htmlView2);
                    //System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
                    //;
                    //
                    //smtpmail2.Send(oMail0);
                    //oMail0 = null;
                    Constant.SendMail(row["email"].ToString(), "Root Cellar Subscription", oMail0Body);
                    i += 1;
                }
                catch (Exception ex)
                {
                }
            }
            Literal1.Text = "<h2>" + i.ToString() + " reminder emails sent</h2>";
        }
        catch (Exception ex)
        {
            Literal1.Text = "We're sorry, there seems to have been an error sending the reminder email";
        }
    }
    public admin_Pickups()
    {
        Load += Page_Load;
    }
}

