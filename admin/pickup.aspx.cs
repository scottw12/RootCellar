using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_pickup : System.Web.UI.Page
{
    private SqlConnection conn = null;
    private string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    private SqlCommand cmd = null;
    protected string SubID, week;
    static int SubId;
    string littxt = "";
    static string Subscriber = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            step0.Visible = false;
            FillWeekInfo();

            if ((Request.QueryString["s"] != null))
            {
                if (!string.IsNullOrEmpty(Request.QueryString["s"].ToString()))
                {
                    FillInfo();
                }
                else
                {
                    literal1.Text = "NO SUBSCRIBER SELECTED! CHANGES WILL NOT BE SAVED";
                }
            }
            else
            {
                literal1.Text = "NO SUBSCRIBER SELECTED! CHANGES WILL NOT BE SAVED";
            }
            literal1.Text = littxt;
            step2.Visible = false;
            step3.Visible = false;
            if (Request.QueryString["s"] != null)
            {
                SubId = int.Parse(Request.QueryString["s"]);
                week = Request.QueryString["week"].ToString();
                SqlConnection cn = Constant.Connection();
                SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.PurchaseProduct.*, dbo.PurchaseProductDetails.* FROM dbo.PurchaseProduct INNER JOIN dbo.PurchaseProductDetails ON dbo.PurchaseProduct.BuyID = dbo.PurchaseProductDetails.BuyId WHERE dbo.PurchaseProduct.SubscriberID='" + SubId + "' and dbo.PurchaseProduct.Week='" + week + "'", cn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvProducts.DataSource = ds.Tables[0];
                    gvProducts.DataBind();
                }
                else
                {
                    gvProducts.DataSource = ds.Tables[0];
                    gvProducts.DataBind();
                }

            }
        }

    }

    protected void FillInfo()
    {
        SubID = Request.QueryString["s"].ToString();
        Subscriber = Request.QueryString["s"].ToString();

        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        string week = "";

        try
        {
            if (!(WeekList.SelectedValue == " - Select a Week - "))
            {
                step1.Visible = true;
                week = WeekList.SelectedValue;
                string pattern = "-(.*?)/";
                string replacement = "/" + "\r\n";
                Regex rgx = new Regex(pattern, RegexOptions.Singleline);
                week = rgx.Replace(week, replacement);
                week = "and weekly.week='" + (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "") + "'";
            }
            else
            {
                step1.Visible = false; 
                week = "";
                return;
            }
        }
        catch (Exception ex)
        {
            literal1.Text = "week:" + week;
        }

        mySqlCommand = new SqlCommand("SELECT Firstname1, Lastname1, vacused, weekly.bounty, weekly.barnyard, weekly.ploughman, allergies, weekly.notes, weekly.vacation, weekly.paidBounty, weekly.paidBarnyard, weekly.paidPloughman, subscribers.notes FROM Weekly INNER JOIN subscribers ON weekly.SubID=subscribers.SubId Where weekly.SubID= '" + SubID + "'" + week, mySqlConnection);
        try
        {
            mySqlConnection.Open();
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            while ((myDataReader.Read()))
            {
                string Subscriptions = "<br /> Current Subscriptions:<br /><b> ";
                string paid = "";
                if (myDataReader.GetBoolean(3) == true)
                {
                    Subscriptions += "Bounty<br />";
                    if (!(myDataReader.GetBoolean(9) == true))
                    {
                        paid = "Bounty ";
                    }
                }
                if (myDataReader.GetBoolean(4) == true)
                {
                    Subscriptions += "Barnyard<br />";
                    if (!(myDataReader.GetBoolean(10) == true))
                    {
                        paid = "Barnyard ";
                    }
                }
                if (myDataReader.GetBoolean(5) == true)
                {
                    Subscriptions += "Ploughman<br />";
                    if (!(myDataReader.GetBoolean(11) == true))
                    {
                        paid = "Ploughman ";
                    }
                }
                littxt = "In store pickup for <b>" + myDataReader.GetString(0) + " " + myDataReader.GetString(1) + "</b>" + Subscriptions + "</b>";
                if (string.IsNullOrEmpty(myDataReader.GetString(6)))
                {
                    literal2.Text = "No Allergies on file";
                }
                else
                {
                    literal2.Text = "<b>ALLEGIC to " + myDataReader.GetString(6) + "</b>";
                }
                if (string.IsNullOrEmpty(myDataReader.GetString(7)))
                {
                    literal3.Text = "None";
                }
                else
                {
                    literal3.Text = myDataReader.GetString(7);
                }
                if (!string.IsNullOrEmpty(myDataReader.GetString(12)))
                {
                    literal3.Text += "<br /><br /><b>Permanent Notes</b><br />" + myDataReader.GetString(12);
                }
                if (myDataReader.GetBoolean(8) == true)
                {
                    step0.Visible = true;
                    step1.Visible = false;
                }
                else
                {
                    step0.Visible = false;
                    step1.Visible = true;
                }
                if (string.IsNullOrEmpty(paid))
                {
                    literal4.Text = "<h2><span style='color:green;'>PAID </span></h2>";
                }
                else
                {
                    literal4.Text = "<h2><span style='color:red;'>NOT PAID </span></h2><br />The following boxes have not been paid<br />" + paid;
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
    protected void Yes1_Click(object sender, EventArgs e)
    {
        step1.Visible = false;
        step2.Visible = true;
        if (literal4.Text.Contains("<span style='color:red;'>NOT PAID </span>"))
        {
            step2b.Visible = true;
        }
        else
        {
            step2b.Visible = false;
            step3.Visible = true;
        }
    }

    protected void PaidButton_Click(object sender, EventArgs e)
    {
        SqlConnection cn = Constant.Connection();
        cn.Open();
        SqlCommand cmd3 = new SqlCommand("Update [dbo].[PurchaseProduct] Set IsPaid='Paid' Where SubscriberID='" + Request.QueryString["s"].ToString() + "'", cn);
        cmd3.ExecuteNonQuery();
        cn.Close();
        cn.Open();
        SqlCommand cmd2 = new SqlCommand("Update [dbo].[PurchaseProductDetails] Set IsPaid='Paid' Where SubscriberID='" + Request.QueryString["s"].ToString() + "'", cn);
        cmd2.ExecuteNonQuery();
        cn.Close();
        step2.Visible = false;
        step3.Visible = true;
        conn = new SqlConnection(ConnectionString);
        conn.Open();
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
        else
        {
            week = "";
        }
        string sql = "update weekly set paidbounty='True', paidbarnyard='True', paidploughman='True' where SubID='" + Request.QueryString["s"].ToString() + "' " + week;
        SqlCommand cmd = new SqlCommand(sql);
        cmd.CommandType = CommandType.Text;
        cmd.Connection = conn;
        cmd.ExecuteNonQuery();
        literal5.Text = "Payment Recorded";
    }

    public void DaySelect(object obj, DayRenderEventArgs e)
    {
        if (e.Day.IsWeekend)
        {
            e.Day.IsSelectable = false;
        }
        if (e.Day.Date.ToString("dddd") == "Saturday")
        {
            e.Day.IsSelectable = true;
        }
        else
        {
            e.Day.IsSelectable = false;
        }
        if (e.Day.Date < System.DateTime.Today)
        {
            e.Day.IsSelectable = false;
        }
    }
    protected void Complete_Click(object sender, EventArgs e)
    {
        conn = new SqlConnection(ConnectionString);
        conn.Open();
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
        else
        {
            week = "";
        }
        string sql = "update weekly set pickedup='true' where SubID='" + Request.QueryString["s"].ToString() + "' " + week;
        SqlCommand cmd = new SqlCommand(sql);
        cmd.CommandType = CommandType.Text;
        cmd.Connection = conn;
        cmd.ExecuteNonQuery();
        /****************New Added By Harshal For Time Tracking***************/
        SqlConnection cn = Constant.Connection();
        SqlDataAdapter da = new SqlDataAdapter("select * from [dbo].[Subscribers] where SubId='" + Subscriber + "'", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        string Customer = ds.Tables[0].Rows[0]["FirstName1"].ToString() + " " + ds.Tables[0].Rows[0]["LastName1"].ToString();


        SqlDataAdapter da2 = new SqlDataAdapter("select * from [dbo].[Userinfo] where UserId='" + Session[Constant.UserID].ToString() + "'", cn);
        DataSet ds2 = new DataSet();
        da2.Fill(ds2);
        string Employee = ds2.Tables[0].Rows[0]["FirstName"].ToString() + " " + ds2.Tables[0].Rows[0]["LastName"].ToString();

        cn.Open();
        SqlCommand cmd2 = new SqlCommand("INSERT INTO PickupTimeTracking VALUES (@PickupTime,@SubId,@EmpId,@Employee,@Customer)", cn);
        cmd2.Parameters.AddWithValue("@PickupTime", DateTime.Now.ToString());
        cmd2.Parameters.AddWithValue("@SubId", ds.Tables[0].Rows[0]["SubId"].ToString());
        cmd2.Parameters.AddWithValue("@EmpId", ds2.Tables[0].Rows[0]["UserId"].ToString());
        cmd2.Parameters.AddWithValue("@Employee", Employee);
        cmd2.Parameters.AddWithValue("@Customer", Customer);
        cmd2.ExecuteNonQuery();
        cn.Close();


        Response.Redirect("~/admin/pickups");
    }
    public int GetPickupDay()
    {
        int daysAdd = 0;
        if (System.DateTime.Today.ToString("dddd") == "Sunday")
        {
            daysAdd = 6;
        }
        else if (System.DateTime.Today.ToString("dddd") == "Monday")
        {
            daysAdd = 5;
        }
        else if (System.DateTime.Today.ToString("dddd") == "Tuesday")
        {
            daysAdd = 4;
        }
        else if (System.DateTime.Today.ToString("dddd") == "Wednesday")
        {
            daysAdd = 3;
        }
        else if (System.DateTime.Today.ToString("dddd") == "Thursday")
        {
            daysAdd = 2;
        }
        else if (System.DateTime.Today.ToString("dddd") == "Friday")
        {
            daysAdd = 1;
        }
        else if (System.DateTime.Today.ToString("dddd") == "Saturday")
        {
            daysAdd = 0;
        }
        return daysAdd;
    }

    protected void WeekList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!(WeekList.SelectedValue == " - Select a Week - "))
        {
            step1.Visible = true;
            FillInfo();
        }
        else
        {
            step1.Visible = false;
        }
    }

}