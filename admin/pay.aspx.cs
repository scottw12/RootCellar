using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class admin_pay : System.Web.UI.Page
{
    protected string SubID;
    string SDateRange;
    public int SubId;
    public int count;
    public string week;
    private SqlConnection conn = null;
    private string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;



    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
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
            if ((Request.QueryString["s"] != null))
            {
                if (!string.IsNullOrEmpty(Request.QueryString["s"].ToString()))
                {
                    SubID = Request.QueryString["s"].ToString();
                    week = Request.QueryString["week"].ToString();
                    FillInfo();
                    if (!Page.IsPostBack)
                    {
                        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                        mySqlCommand = new SqlCommand("SELECT Firstname1, Lastname1, weekly.bounty, weekly.barnyard, weekly.ploughman FROM weekly INNER JOIN subscribers ON weekly.SubID=subscribers.SubId Where weekly.SubID= '" + SubID + "'", mySqlConnection);
                        try
                        {
                            mySqlConnection.Open();
                            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                            while ((myDataReader.Read()))
                            {
                                Literal1.Text = "Making subscription payment for <b>" + myDataReader.GetString(0) + " " + myDataReader.GetString(1) + "<br />";


                                if (myDataReader.GetBoolean(3) == false)
                                {
                                    GridView1.Columns[3].Visible = false;
                                }
                                if (myDataReader.GetBoolean(2) == false)
                                {
                                    GridView1.Columns[2].Visible = false;
                                }
                                if (myDataReader.GetBoolean(4) == false)
                                {
                                    GridView1.Columns[4].Visible = false;
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
            /**************New added By Harshal************/
            //if (Request.QueryString["s"] != null)
            //{
            //    SubId = int.Parse(Request.QueryString["s"]);
            //    SqlConnection cn = Constant.Connection();
            //    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.PurchaseProduct.*, dbo.PurchaseProductDetails.* FROM dbo.PurchaseProduct INNER JOIN dbo.PurchaseProductDetails ON dbo.PurchaseProduct.BuyID = dbo.PurchaseProductDetails.BuyId WHERE dbo.PurchaseProduct.SubscriberID='" + SubId + "' and dbo.PurchaseProduct.Week='" + week + "'", cn);
            //    DataSet ds = new DataSet();
            //    da.Fill(ds);
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        gvProducts.DataSource = ds.Tables[0];
            //        gvProducts.DataBind();
            //    }
            //    else
            //    {
            //        gvProducts.DataSource = ds.Tables[0];
            //        gvProducts.DataBind();
            //    }

            //}

            Price.Text = "$0.00";
        }
    }
    protected void FillInfo()
    {
        SqlDataReader myDataReader2 = default(SqlDataReader);
        SqlConnection mySqlConnection2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        SqlCommand mySqlCommand2 = default(SqlCommand);
       // string SDateRange = "";
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
                            SDateRange = "and ((week>='" + myDataReader2.GetDateTime(0) + "' and week <= '" + myDataReader2.GetDateTime(1) + "') Or Week = '1/1/1900') ";
                        }
                    }
                    myDataReader2.Close();
                }
            }
        }
        finally
        {
        }
        string SqlQuary = "SELECT SubId, Week, PaidBounty, PaidBarnyard, PaidPloughman FROM Weekly where subID='" + SubID + "' " + SDateRange + " ORDER BY [Week]";
        ViewState["query"] = SqlQuary;
        //Added by Harshal for home delivery column
        SqlConnection cn = Constant.Connection();
        SqlDataAdapter da4 = new SqlDataAdapter("Select * From PurchaseProduct WHERE SubscriberID='" + SubID + "' AND OnlineHome='True' AND IsPaid='Paid'", cn);
        DataSet ds4 = new DataSet();
        da4.Fill(ds4);
        bool HomeDelivery = false;
        if (ds4.Tables[0].Rows.Count > 0)
        {
            Session["HomeDeliveryCheckbox"] = ds4;
            //if (ds4.Tables[0].Rows[0]["Request"].ToString() == "Approved")
            //    HomeDelivery = true;
        }
        else
        {
            Session["HomeDeliveryCheckbox"] = null;
        }

        SqlDataAdapter da5 = new SqlDataAdapter("Select * From PurchaseProduct WHERE SubscriberID='" + SubID + "' AND IsPaid='Paid'", cn);
        DataSet ds5 = new DataSet();
        da5.Fill(ds5);
        if (ds5.Tables[0].Rows.Count > 0)
        {
            Session["IsPaidCheckbox"] = ds5;
        }
        else
        {
            Session["IsPaidCheckbox"] = null;
        }


        //
        DataTable dt2 = new DataTable();
        dt2.Columns.Add("Week");


        SqlDataAdapter da = new SqlDataAdapter("Select * From PurchaseProductDetails where SubscriberID='" + SubID + "'", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        SqlDataAdapter da2 = new SqlDataAdapter("Select Distinct ProductName From PurchaseProductDetails where SubscriberID='" + SubID + "'", cn);
        DataSet ds2 = new DataSet();
        da2.Fill(ds2);

        SqlDataAdapter da3 = new SqlDataAdapter("SELECT * FROM PurchaseProduct where SubscriberID='" + SubID + "'", cn);
        DataSet ds3 = new DataSet();
        da3.Fill(ds3);
        count = ds3.Tables[0].Rows.Count;

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            //dt2.Columns.Add(ds.Tables[0].Rows[i]["ProductName"].ToString());
            ////BoundField bfield = new BoundField();
            ////bfield.HeaderText = ds.Tables[0].Rows[i]["ProductName"].ToString();
            ////bfield.DataField = ds.Tables[0].Rows[i]["ProductName"].ToString();
            ////gvProduct.Columns.Add(bfield);

            //TemplateField tfield = new TemplateField();
            //tfield.HeaderText = ds.Tables[0].Rows[i]["ProductName"].ToString();
            //gvProduct.Columns.Add(tfield);
            //gvProduct.DataBind();
            ////tfield = new TemplateField();
            ////tfield.HeaderText = ds.Tables[0].Rows[i]["ProductName"].ToString();
            ////gvProduct.Columns.Add(tfield);

        }


        DataTable dt = new DataTable();
        dt.Columns.Add("SubId");
        dt.Columns.Add("Week");
        dt.Columns.Add("PaidBounty");
        dt.Columns.Add("PaidBarnyard");
        dt.Columns.Add("PaidPloughman");
        dt.Columns.Add("HomeDelivery");
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
                    while (myDataReader.Read())
                    {
                        string week = (myDataReader.GetDateTime(1).Month.ToString() + "/" + myDataReader.GetDateTime(1).Day.ToString() + "-" + myDataReader.GetDateTime(1).AddDays(1).Day.ToString() + "/" + myDataReader.GetDateTime(1).Year.ToString());
                        if (week == "1/1-2/1900")
                        {
                            week = "Deposit";
                        }
                        dt.Rows.Add(myDataReader.GetInt32(0), week, myDataReader.GetBoolean(2), myDataReader.GetBoolean(3), myDataReader.GetBoolean(4), HomeDelivery);
                        dt2.Rows.Add(week);
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
        ViewState["TempData"] = dt2;
        ViewState["Record"] = ds3;
        GridView1.DataSource = dt;
        GridView1.DataBind();


        gvProduct.DataSource = dt2;
        gvProduct.DataBind();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        int days = 0;
        foreach (GridViewRow Weekrow in GridView1.Rows)
        {
            CheckBox BountyPaid = Weekrow.FindControl("BountyPaidCheck") as CheckBox;
            if (BountyPaid.Enabled == true & BountyPaid.Checked == true)
            {
                string week = Weekrow.Cells[1].Text;
                string pattern = "-(.*?)/";
                string replacement = "/" + "\r\n";
                Regex rgx = new Regex(pattern, RegexOptions.Singleline);
                week = rgx.Replace(week, replacement);
                if (week != "Deposit")
                {
                    week = (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "");
                }
                else
                {
                    week = "1/1/1900";
                }
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                string sql = "update weekly set PaidBounty='True' where SubID='" + Request.QueryString["s"].ToString() + "' and week='" + week + "'";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                days += 1;

                //if week == Deposit  and is true make sure that the subscribers table active filed is set to true...
                if (week == "1/1/1900")
                {
                    if (BountyPaid.Checked == true )
                    {
                        string sql1 = "update subscribers set active=-1 where SubID='" + Request.QueryString["s"].ToString() + "'";
                        SqlCommand cmd1 = new SqlCommand(sql1);
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = conn;
                        cmd1.ExecuteNonQuery();
                        cmd1.Connection.Close();
                    }
                }
            }
        }
        foreach (GridViewRow Weekrow in GridView1.Rows)
        {
            CheckBox BarnyardPaid = Weekrow.FindControl("BarnyardPaidCheck") as CheckBox;
            if (BarnyardPaid.Enabled == true & BarnyardPaid.Checked == true)
            {
                string week = Weekrow.Cells[1].Text;
                string pattern = "-(.*?)/";
                string replacement = "/" + "\r\n";
                Regex rgx = new Regex(pattern, RegexOptions.Singleline);
                week = rgx.Replace(week, replacement);
                if (week != "Deposit")
                {
                    week = (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "");
                }
                else
                {
                    week = "1/1/1900";
                }
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                string sql = "update weekly set PaidBarnyard='True' where SubID='" + Request.QueryString["s"].ToString() + "' and week='" + week + "'";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                days += 1;
                //if week == Deposit  and is true make sure that the subscribers table active filed is set to true...
                if (week == "1/1/1900")
                {
                    if (BarnyardPaid.Checked == true)
                    {
                        string sql1 = "update subscribers set active=-1 where SubID='" + Request.QueryString["s"].ToString() + "'";
                        SqlCommand cmd1 = new SqlCommand(sql1);
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = conn;
                        cmd1.ExecuteNonQuery();
                        cmd1.Connection.Close();
                    }
                }
            }
        }
        foreach (GridViewRow Weekrow in GridView1.Rows)
        {
            CheckBox PloughmanPaid = Weekrow.FindControl("PloughmanPaidCheck") as CheckBox;
            if (PloughmanPaid.Enabled == true & PloughmanPaid.Checked == true)
            {
                string week = Weekrow.Cells[1].Text;
                string pattern = "-(.*?)/";
                string replacement = "/" + "\r\n";
                Regex rgx = new Regex(pattern, RegexOptions.Singleline);
                week = rgx.Replace(week, replacement);
                if (week != "Deposit")
                {
                    week = (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "");
                }
                else
                {
                    week = "1/1/1900";
                }
                    conn = new SqlConnection(ConnectionString);
                conn.Open();
                string sql = "update weekly set PaidPloughman='True' where SubID='" + Request.QueryString["s"].ToString() + "' and week='" + week + "'";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                days += 1;
                //if week == Deposit  and is true make sure that the subscribers table active filed is set to true...
                if (week == "1/1/1900")
                {
                    if (PloughmanPaid.Checked == true)
                    {
                        string sql1 = "update subscribers set active=-1 where SubID='" + Request.QueryString["s"].ToString() + "'";
                      
                        SqlCommand cmd1 = new SqlCommand(sql1);
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = conn;
                        cmd1.ExecuteNonQuery();
                        cmd1.Connection.Close();
                    }
                }
            }
        }
        if (days == 0)
        {
            Literal2.Text = "Please select at least one week to make payment on";
        }
        else
        {
            Literal1.Text = "Payment Recorded";
            step1.Visible = false;
            tableUPanel.Update();
            FillInfo();
        }

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

    protected void OnCheckedChanged(Object sender, EventArgs e)
    {
        tableUPanel.Update();
        int days = 0;
        foreach (GridViewRow Weekrow in GridView1.Rows)
        {
            CheckBox BountyPaid = Weekrow.FindControl("BountyPaidCheck") as CheckBox;
            if (BountyPaid.Enabled == true & BountyPaid.Checked == true)
            {
                days += 1;
            }
            CheckBox BarnyardPaid = Weekrow.FindControl("BarnyardPaidCheck") as CheckBox;
            if (BarnyardPaid.Enabled == true & BarnyardPaid.Checked == true)
            {
                days += 1;
            }
            CheckBox PloughmanPaid = Weekrow.FindControl("PloughmanPaidCheck") as CheckBox;
            if (PloughmanPaid.Enabled == true & PloughmanPaid.Checked == true)
            {
                days += 1;
            }
        }
        //get amount of previous payments fro this season
string  SqlQuery = (string)ViewState["query"];
        SqlConnection cn = Constant.Connection();
        
        SqlDataAdapter daPayment = new SqlDataAdapter(SqlQuery,cn);
        DataSet dsPayment = new DataSet();
        daPayment.Fill(dsPayment);
        int boxPayments = 0;
        if (dsPayment.Tables[0].Rows.Count > 0)
        {
           
            foreach (DataRow item in dsPayment.Tables[0].Rows)
            {
                string seeit1 = item["PaidPloughman"].ToString();
                if (item["PaidBounty"].ToString()== "True" ) { boxPayments += 1; };
                if (item["PaidBarnyard"].ToString() == "True" ) { boxPayments += 1; };
                if (item["PaidPloughman"].ToString() == "True" ) { boxPayments += 1; };
            }

            
        }
        //else
        //{
        //    Session["HomeDeliveryCheckbox"] = null;
        //}






        if (days * 35 == 0)
        {
            Price.Text = "$0.00";
        }
        else
        {
            Price.Text = "" + (days * 35 - (boxPayments*35)).ToString("C2");
        }

    }


    protected void gvProduct_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dt = (DataTable)ViewState["TempData"];
        DataSet ds = (DataSet)ViewState["Record"];
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < dt.Columns.Count; i++)
            {
                CheckBox cb2 = new CheckBox();
                cb2.Enabled = false;
                //txtCountry.ID = "txtCountry";
                //txtCountry.Text = (e.Row.DataItem as DataRowView).Row["Country"].ToString();
                e.Row.Cells[i].Controls.Add(cb2);

                if (ds.Tables[0].Rows[count - 1]["Week"].ToString() == e.Row.Cells[0].Text)
                {
                    cb2.Checked = true;
                }
            }
        }
    }
    /// <summary>
    /// Added By Harshal 
    /// To Show or hide additional Product Table
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddtionalProduct")
        {
            //gvProducts.Visible = true;
            // string Currentweek = e.Row.Cells[1].Text;
            SubId = int.Parse(Request.QueryString["s"]);
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridView1.Rows[index];
            string Currentweek = row.Cells[1].Text;

            SqlConnection cn = Constant.Connection();
            SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.PurchaseProduct.*, dbo.PurchaseProductDetails.* FROM dbo.PurchaseProduct INNER JOIN dbo.PurchaseProductDetails ON dbo.PurchaseProduct.BuyID = dbo.PurchaseProductDetails.BuyId WHERE dbo.PurchaseProduct.SubscriberID='" + SubId + "' and dbo.PurchaseProduct.Week='" + Currentweek + "'", cn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvProducts.DataSource = ds.Tables[0];
                gvProducts.DataBind();
                gvProducts.Visible = true;
            }
            else
            {
                gvProducts.DataSource = null;
                gvProducts.DataBind();
                gvProducts.Visible = true;
            }
        }
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        CheckBox cbHomedelivery = e.Row.FindControl("cbHomedelivery") as CheckBox;
        CheckBox cbPaid = e.Row.FindControl("cbPaid") as CheckBox;
        if (Session["HomeDeliveryCheckbox"] != null)
        {
            DataSet ds = Session["HomeDeliveryCheckbox"] as DataSet;
            //GridViewRow row = e.Row.Cells[2] as GridViewRow;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    if (item["Week"].ToString() == e.Row.Cells[1].Text)
                    {
                        cbHomedelivery.Checked = true;
                        break;
                    }
                    else
                        cbHomedelivery.Checked = false;
                }
            }
        }
        else
        {
            if (cbHomedelivery != null)
            {
                cbHomedelivery.Checked = false;
            }
        }
        if (Session["IsPaidCheckbox"] != null)
        {
            DataSet ds = Session["IsPaidCheckbox"] as DataSet;
            //GridViewRow row = e.Row.Cells[2] as GridViewRow;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    if (item["Week"].ToString() == e.Row.Cells[1].Text && item["IsPaid"].ToString() == "Paid")
                    {
                        cbPaid.Checked = true;
                        break;
                    }
                    else
                        cbPaid.Checked = false;
                }
            }
        }
        else
        {
            if (cbHomedelivery != null)
            {
                cbHomedelivery.Checked = false;
            }
        }
    }
}
