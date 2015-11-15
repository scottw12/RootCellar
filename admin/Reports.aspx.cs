using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Net;
using System.Net.Mail;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Configuration;
public partial class admin_Reports : System.Web.UI.Page
{
    DataSet Myds = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
        scriptManager.RegisterPostBackControl(this.gvHomeDelivery);

        //btnView_Click+=.......
        if (!IsPostBack)
        {
            SqlConnection cn = Constant.Connection();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM AllowAccess where UserID='" + Session[Constant.UserID].ToString() + "'", cn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ViewReport"].ToString() != "True")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Error();", true);
                    return;
                }

                else
                {
                    FillWeekInfo1();
                    BindDropDown();
                    divAllergies.Visible = true;
                    divNotes.Visible = false;
                    divTimeTracking.Visible = false;
                    divHomeDelivery.Visible = false;
                    divStore.Visible = false;
                    divNPU.Visible = false;
                    divWeeklyReports.Visible = false;
                    divPickupTT.Visible = false;
                    divVacation.Visible = false;
                    BindAllergies();
                    BindNotes();
                    BindTimeTracking();
                    BindHomeDelivery();
                    BindStore();
                    BindNPU();
                    VacationWeekDropdown();
                    StoreDropdown();

                    WeeklyReports();
                    BindVacation();
                    BindPickupTT();

                }
            }
        }
    }

    protected void FillWeekInfo1()
    {

        SqlDataReader myDataReader2 = default(SqlDataReader);
        SqlConnection mySqlConnection2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        SqlCommand mySqlCommand2 = default(SqlCommand);
        string SDateRange = "";
        string query = "select Sstart, send from seasons where currents='true'";
        try
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)) ;
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
                            SDateRange = "where week>'" + myDataReader2.GetDateTime(0) + "' and week <= '" + myDataReader2.GetDateTime(1) + "' ";
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
                    dt.Rows.RemoveAt(1);
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
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
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
                            SDateRange = " and week >= '" + myDataReader2.GetDateTime(0) + "' and week <= '" + myDataReader2.GetDateTime(1) + "' ";
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
                //mySqlCommand = new SqlCommand("SELECT DISTINCT Week FROM Weekly where subID='" + SubId.ToString() + "' and ((week='1/1/1900') or (week>='" + System.DateTime.Today.AddDays(7) + "'" + SDateRange + ")) ORDER BY [Week]", mySqlConnection);
                //mySqlCommand = new SqlCommand("SELECT DISTINCT Week FROM Weekly where subID= 4624 and ((week='1/1/1900') or (week>='" + System.DateTime.Today.AddDays(7) + "'" + SDateRange + ")) ORDER BY [Week]", mySqlConnection);
                //mySqlCommand = new SqlCommand("SELECT DISTINCT Week FROM Weekly where subID= '" + SubId.ToString() + "' " + SDateRange + " ORDER BY [Week]", mySqlConnection);
                mySqlConnection.Open();

                myDataReader = mySqlCommand.ExecuteReader();

                if (myDataReader.HasRows)
                {
                    while (myDataReader.Read())
                    {
                        if (!(myDataReader.GetDateTime(0).Year.ToString() == "1900"))
                        {
                            dt.Rows.Add(myDataReader.GetDateTime(0).Month.ToString() + "/" + myDataReader.GetDateTime(0).Day.ToString() + "-" + myDataReader.GetDateTime(0).AddDays(1).Day.ToString() + "/" + myDataReader.GetDateTime(0).Year.ToString());
                            if (myDataReader.GetDateTime(0) == System.DateTime.Today | myDataReader.GetDateTime(0) == System.DateTime.Today.AddDays(6))
                            {
                                WeekList.SelectedValue = myDataReader.GetDateTime(0).Month.ToString() + "/" + myDataReader.GetDateTime(0).Day.ToString() + "-" + myDataReader.GetDateTime(0).AddDays(1).Day.ToString() + "/" + myDataReader.GetDateTime(0).Year.ToString();
                            }
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

    }
    private void BindPickupTT()
    {
        SqlConnection cn = Constant.Connection();
        DataSet ds = new DataSet();
        if (ddlStore.SelectedValue == "")
        {


            if (ddlPickupDay.SelectedValue == "Select Pickup Day")
            {
                if (txtPikupTT.Text == string.Empty)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [dbo].[PickupTimeTracking]", cn);
                    da.Fill(ds);
                }
                else if (txtPikupTT.Text != string.Empty)
                {
                    string User = txtPikupTT.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [dbo].[PickupTimeTracking] WHERE CUSTOMER like '%" + User + "%'", cn);
                    da.Fill(ds);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvPickupTT.DataSource = ds.Tables[0];
                    gvPickupTT.DataBind();
                }
                else
                {
                    gvPickupTT.DataSource = null;
                    gvPickupTT.DataBind();
                }
            }
            else
            {
                if (txtPikupTT.Text == string.Empty)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.PickupTimeTracking.*, dbo.Subscribers.* FROM dbo.Subscribers INNER JOIN dbo.PickupTimeTracking ON dbo.Subscribers.SubId = dbo.PickupTimeTracking.SubId WHERE Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtPikupTT.Text != string.Empty)
                {
                    string User = txtPikupTT.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.PickupTimeTracking.*, dbo.Subscribers.* FROM dbo.Subscribers INNER JOIN dbo.PickupTimeTracking ON dbo.Subscribers.SubId = dbo.PickupTimeTracking.SubId where dbo.PickupTimeTracking.CUSTOMER like '%" + User + "%' and  Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'", cn);
                    da.Fill(ds);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvPickupTT.DataSource = ds.Tables[0];
                    gvPickupTT.DataBind();
                }
                else
                {
                    gvPickupTT.DataSource = null;
                    gvPickupTT.DataBind();
                }
            }
        }
        else//Store
        {
            if (ddlPickupDay.SelectedValue == "Select Pickup Day")
            {
                if (txtPikupTT.Text == string.Empty)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.PickupTimeTracking.* FROM dbo.Subscribers INNER JOIN dbo.PickupTimeTracking ON dbo.Subscribers.SubId = dbo.PickupTimeTracking.SubId WHERE  dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtPikupTT.Text != string.Empty)
                {
                    string User = txtPikupTT.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.PickupTimeTracking.* FROM dbo.Subscribers INNER JOIN dbo.PickupTimeTracking ON dbo.Subscribers.SubId = dbo.PickupTimeTracking.SubId WHERE  dbo.Subscribers.Store='" + ddlStore.SelectedValue + "' and CUSTOMER like '%" + User + "%'", cn);
                    da.Fill(ds);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvPickupTT.DataSource = ds.Tables[0];
                    gvPickupTT.DataBind();
                }
                else
                {
                    gvPickupTT.DataSource = null;
                    gvPickupTT.DataBind();
                }
            }
            else
            {
                if (txtPikupTT.Text == string.Empty)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.PickupTimeTracking.*, dbo.Subscribers.* FROM dbo.Subscribers INNER JOIN dbo.PickupTimeTracking ON dbo.Subscribers.SubId = dbo.PickupTimeTracking.SubId WHERE Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtPikupTT.Text != string.Empty)
                {
                    string User = txtPikupTT.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.PickupTimeTracking.*, dbo.Subscribers.* FROM dbo.Subscribers INNER JOIN dbo.PickupTimeTracking ON dbo.Subscribers.SubId = dbo.PickupTimeTracking.SubId where dbo.PickupTimeTracking.CUSTOMER like '%" + User + "%' and  Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvPickupTT.DataSource = ds.Tables[0];
                    gvPickupTT.DataBind();
                }
                else
                {
                    gvPickupTT.DataSource = null;
                    gvPickupTT.DataBind();
                }
            }
        }
    }
    /// <summary>
    /// Vacation Report
    /// </summary>
    private void BindVacation()
    {
        SqlConnection cn = Constant.Connection();
        DataSet ds = new DataSet();
        string VacationQuery = "SELECT dbo.VacationDetails.*, dbo.Subscribers.* FROM dbo.VacationDetails INNER JOIN dbo.Subscribers ON dbo.VacationDetails.CustomerID = dbo.Subscribers.SubId";
        string VacationSubQuery = string.Empty;
        if (WeekList.SelectedValue != " - Select a Week - ")
        {
            if (VacationSubQuery != string.Empty)
                VacationSubQuery += " and dbo.VacationDetails.VacationWeek='" + WeekList.SelectedValue + "'";
            else
                VacationSubQuery += " dbo.VacationDetails.VacationWeek='" + WeekList.SelectedValue + "'";
        }
        if (ddlStore.SelectedValue != "")
        {
            if (VacationSubQuery != string.Empty)
                VacationSubQuery += " and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'";
            else
                VacationSubQuery += " dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'";

        }
        if (ddlPickupDay.SelectedValue != "Select Pickup Day")
        {
            if (VacationSubQuery != string.Empty)
                VacationSubQuery += " and dbo.Subscribers.PickupDay='" + ddlStore.SelectedValue + "'";
            else
                VacationSubQuery += " dbo.Subscribers.PickupDay='" + ddlStore.SelectedValue + "'";
        }
        if (txtVacation.Text != string.Empty)
        {
            if (VacationSubQuery != string.Empty)
                VacationSubQuery += " and dbo.Subscribers.Username like '%" + txtVacation.Text + "%'";
            else
                VacationSubQuery += " dbo.Subscribers.Username like '%" + txtVacation.Text + "%'";
        }

        if (VacationSubQuery == string.Empty)
        {
            SqlDataAdapter da = new SqlDataAdapter(VacationQuery + " WHERE dbo.VacationDetails.VacationAddedDate>='" + DateTime.Now.ToShortDateString() + "'", cn);
            da.Fill(ds);
        }
        else
        {
            SqlDataAdapter da = new SqlDataAdapter(VacationQuery + " WHERE " + VacationSubQuery, cn);
            da.Fill(ds);
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVacation.DataSource = ds.Tables[0];
            gvVacation.DataBind();
        }
        else
        {
            gvVacation.DataSource = null;
            gvVacation.DataBind();
        }
        //if (ddlStore.SelectedValue == "")
        //{
        //    if (ddlPickupDay.SelectedValue == "Select Pickup Day")
        //    {
        //        if (txtVacation.Text == string.Empty)
        //        {
        //            SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.VacationDetails.*, dbo.Subscribers.* FROM dbo.VacationDetails INNER JOIN dbo.Subscribers ON dbo.VacationDetails.CustomerID = dbo.Subscribers.SubId WHERE dbo.VacationDetails.VacationAddedDate>='" + DateTime.Now.ToShortDateString() + "'", cn);
        //            da.Fill(ds);
        //        }
        //        else if (txtVacation.Text != string.Empty)
        //        {
        //            string User = txtVacation.Text.Trim();
        //            SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.VacationDetails.*, dbo.Subscribers.* FROM dbo.VacationDetails INNER JOIN dbo.Subscribers ON dbo.VacationDetails.CustomerID = dbo.Subscribers.SubId where Subscribers.Username like '%" + User + "%' and  dbo.VacationDetails.VacationAddedDate>='" + DateTime.Now.ToShortDateString() + "'", cn);
        //            da.Fill(ds);
        //        }

        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            gvVacation.DataSource = ds.Tables[0];
        //            gvVacation.DataBind();
        //        }
        //        else
        //        {
        //            gvVacation.DataSource = null;
        //            gvVacation.DataBind();
        //        }
        //    }
        //    else
        //    {
        //        if (txtVacation.Text == string.Empty)
        //        {
        //            SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.VacationDetails.*, dbo.Subscribers.* FROM dbo.VacationDetails INNER JOIN dbo.Subscribers ON dbo.VacationDetails.CustomerID = dbo.Subscribers.SubId WHERE  Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and  dbo.VacationDetails.VacationAddedDate>='" + DateTime.Now.ToShortDateString() + "'", cn);
        //            da.Fill(ds);
        //        }
        //        else if (txtVacation.Text != string.Empty)
        //        {
        //            string User = txtVacation.Text.Trim();
        //            SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.VacationDetails.*, dbo.Subscribers.* FROM dbo.VacationDetails INNER JOIN dbo.Subscribers ON dbo.VacationDetails.CustomerID = dbo.Subscribers.SubId where Subscribers.Username like '%" + User + "%' and  Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and  dbo.VacationDetails.VacationAddedDate>='" + DateTime.Now.ToShortDateString() + "'", cn);
        //            da.Fill(ds);
        //        }

        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            gvVacation.DataSource = ds.Tables[0];
        //            gvVacation.DataBind();
        //        }
        //        else
        //        {
        //            gvVacation.DataSource = null;
        //            gvVacation.DataBind();
        //        }
        //    }
        //}
        //else//Store
        //{
        //    if (ddlPickupDay.SelectedValue == "Select Pickup Day")
        //    {
        //        if (txtVacation.Text == string.Empty)
        //        {
        //            SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.VacationDetails.*, dbo.Subscribers.* FROM dbo.VacationDetails INNER JOIN dbo.Subscribers ON dbo.VacationDetails.CustomerID = dbo.Subscribers.SubId Where  dbo.Subscribers.Store='" + ddlStore.SelectedValue + "' and  dbo.VacationDetails.VacationAddedDate>='" + DateTime.Now.ToShortDateString() + "'", cn);
        //            da.Fill(ds);
        //        }
        //        else if (txtVacation.Text != string.Empty)
        //        {
        //            string User = txtVacation.Text.Trim();
        //            SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.VacationDetails.*, dbo.Subscribers.* FROM dbo.VacationDetails INNER JOIN dbo.Subscribers ON dbo.VacationDetails.CustomerID = dbo.Subscribers.SubId where Subscribers.Username like '%" + User + "%' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "' and  dbo.VacationDetails.VacationAddedDate>='" + DateTime.Now.ToShortDateString() + "'", cn);
        //            da.Fill(ds);
        //        }

        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            gvVacation.DataSource = ds.Tables[0];
        //            gvVacation.DataBind();
        //        }
        //        else
        //        {
        //            gvVacation.DataSource = null;
        //            gvVacation.DataBind();
        //        }
        //    }
        //    else
        //    {
        //        if (txtVacation.Text == string.Empty)
        //        {
        //            SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.VacationDetails.*, dbo.Subscribers.* FROM dbo.VacationDetails INNER JOIN dbo.Subscribers ON dbo.VacationDetails.CustomerID = dbo.Subscribers.SubId WHERE  Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "' and  dbo.VacationDetails.VacationAddedDate>='" + DateTime.Now.ToShortDateString() + "'", cn);
        //            da.Fill(ds);
        //        }
        //        else if (txtVacation.Text != string.Empty)
        //        {
        //            string User = txtVacation.Text.Trim();
        //            SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.VacationDetails.*, dbo.Subscribers.* FROM dbo.VacationDetails INNER JOIN dbo.Subscribers ON dbo.VacationDetails.CustomerID = dbo.Subscribers.SubId where Subscribers.Username like '%" + User + "%' and  Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "' and  dbo.VacationDetails.VacationAddedDate>='" + DateTime.Now.ToShortDateString() + "'", cn);
        //            da.Fill(ds);
        //        }

        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            gvVacation.DataSource = ds.Tables[0];
        //            gvVacation.DataBind();
        //        }
        //        else
        //        {
        //            gvVacation.DataSource = null;
        //            gvVacation.DataBind();
        //        }
        //    }
        //}
    }


    private void BindDropDown()
    {
        SqlConnection cn2 = Constant.Connection();
        SqlDataAdapter da2 = new SqlDataAdapter("Select * From Stores", cn2);
        DataSet ds2 = new DataSet();
        da2.Fill(ds2);

        ddlStore.DataSource = ds2.Tables[0];
        ddlStore.DataTextField = "Store";
        ddlStore.DataValueField = "Store";
        ddlStore.DataBind();
        ddlStore.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- Select Store -", ""));
    }

    private void WeeklyReports()
    {
        DataTable dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[3] { new DataColumn("TotalBoxes", typeof(int)),
                            new DataColumn("Vacation", typeof(int)),
                            new DataColumn("NPU",typeof(int)) });
        DateTime mondayOfLastWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6);
        DateTime NextDays = mondayOfLastWeek.AddDays(7);
        SqlConnection cn = Constant.Connection();
        DataSet ds = new DataSet();
        DataSet ds2 = new DataSet();
        //SqlDataAdapter da = new SqlDataAdapter("SELECT COUNT(dbo.Weekly.Bounty) as TotalBoxes, COUNT(dbo.Weekly.Barnyard) as Barnyard, COUNT(dbo.Weekly.Ploughman) as Ploughman ,COUNT(dbo.VacationDetails.VacationAddedDate) as Vacation,COUNT(dbo.VacationDetails.VacationAddedDate) as NPU FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID INNER JOIN dbo.Weekly ON dbo.Subscribers.SubId = dbo.Weekly.SubId where dbo.Weekly.Bounty='1' and dbo.Weekly.Ploughman='1' and  dbo.Weekly.Barnyard='1' and   dbo.Weekly.Week>'" + mondayOfLastWeek + "' and   dbo.Weekly.Week<'" + NextDays + "' and   dbo.VacationDetails.VacationAddedDate>'" + mondayOfLastWeek + "' and   dbo.VacationDetails.VacationAddedDate<'" + NextDays + "' ", cn);

        if (ddlBoxes.SelectedValue == "Barnyard")
        {
            //SqlDataAdapter da = new SqlDataAdapter("SELECT COUNT(dbo.Weekly.Barnyard) as TotalBoxes,COUNT(dbo.VacationDetails.VacationAddedDate) as Vacation,COUNT(dbo.VacationDetails.VacationAddedDate) as NPU FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID INNER JOIN dbo.Weekly ON dbo.Subscribers.SubId = dbo.Weekly.SubId where dbo.Weekly.Barnyard='1' and   dbo.Weekly.Week>'" + mondayOfLastWeek + "' and   dbo.Weekly.Week<'" + NextDays + "' and   dbo.VacationDetails.VacationAddedDate>'" + mondayOfLastWeek + "' and   dbo.VacationDetails.VacationAddedDate<'" + NextDays + "' and dbo.Subscribers.Store='"+ddlStore.SelectedValue+"'", cn);
            SqlDataAdapter da = new SqlDataAdapter("SELECT COUNT(dbo.Weekly.Barnyard) as TotalBoxes FROM dbo.Weekly where dbo.Weekly.Barnyard='1' and   dbo.Weekly.Week>'" + mondayOfLastWeek + "' and   dbo.Weekly.Week<'" + NextDays + "' and dbo.Weekly.Location='" + ddlStore.SelectedValue + "'", cn);
            da.Fill(ds);
            //SqlDataAdapter da2 = new SqlDataAdapter("Select COUNT(dbo.VacationDetails.VacationAddedDate) as Vacation,COUNT(dbo.VacationDetails.VacationAddedDate) as NPU From dbo.VacationDetails  where   dbo.VacationDetails.VacationAddedDate>'" + mondayOfLastWeek + "' and  dbo.VacationDetails.VacationAddedDate<'" + NextDays + "'", cn);

            SqlDataAdapter da2 = new SqlDataAdapter("SELECT COUNT(dbo.VacationDetails.VacationAddedDate) as Vacation,COUNT(dbo.VacationDetails.VacationAddedDate) as NPU FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID  where   dbo.VacationDetails.VacationAddedDate>'" + mondayOfLastWeek + "' and  dbo.VacationDetails.VacationAddedDate<'" + NextDays + "' and  dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
            da2.Fill(ds2);
            int TotalBoxes = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalBoxes"]);
            int Vacation = Convert.ToInt32(ds2.Tables[0].Rows[0]["Vacation"]);
            int NPU = Convert.ToInt32(ds2.Tables[0].Rows[0]["NPU"]);
            dt.Rows.Add(TotalBoxes, Vacation, NPU);
        }
        else if (ddlBoxes.SelectedValue == "Ploughman")
        {
            //SqlDataAdapter da = new SqlDataAdapter("SELECT COUNT(dbo.Weekly.Ploughman) as TotalBoxes,COUNT(dbo.VacationDetails.VacationAddedDate) as Vacation,COUNT(dbo.VacationDetails.VacationAddedDate) as NPU FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID INNER JOIN dbo.Weekly ON dbo.Subscribers.SubId = dbo.Weekly.SubId where dbo.Weekly.Ploughman='1' and   dbo.Weekly.Week>'" + mondayOfLastWeek + "' and   dbo.Weekly.Week<'" + NextDays + "' and   dbo.VacationDetails.VacationAddedDate>'" + mondayOfLastWeek + "' and   dbo.VacationDetails.VacationAddedDate<'" + NextDays + "'  and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
            SqlDataAdapter da = new SqlDataAdapter("SELECT COUNT(dbo.Weekly.Ploughman) as TotalBoxes FROM dbo.Weekly where dbo.Weekly.Ploughman='1' and   dbo.Weekly.Week>'" + mondayOfLastWeek + "' and   dbo.Weekly.Week<'" + NextDays + "' and dbo.Weekly.Location='" + ddlStore.SelectedValue + "'", cn);
            da.Fill(ds);
            //SqlDataAdapter da2 = new SqlDataAdapter("Select COUNT(dbo.VacationDetails.VacationAddedDate) as Vacation,COUNT(dbo.VacationDetails.VacationAddedDate) as NPU From dbo.VacationDetails  where   dbo.VacationDetails.VacationAddedDate>'" + mondayOfLastWeek + "' and  dbo.VacationDetails.VacationAddedDate<'" + NextDays + "'", cn);

            SqlDataAdapter da2 = new SqlDataAdapter("SELECT COUNT(dbo.VacationDetails.VacationAddedDate) as Vacation,COUNT(dbo.VacationDetails.VacationAddedDate) as NPU FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID  where   dbo.VacationDetails.VacationAddedDate>'" + mondayOfLastWeek + "' and  dbo.VacationDetails.VacationAddedDate<'" + NextDays + "' and  dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
            da2.Fill(ds2);
            int TotalBoxes = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalBoxes"]);
            int Vacation = Convert.ToInt32(ds2.Tables[0].Rows[0]["Vacation"]);
            int NPU = Convert.ToInt32(ds2.Tables[0].Rows[0]["NPU"]);
            dt.Rows.Add(TotalBoxes, Vacation, NPU);

        }
        else
        {
            string Query = "SELECT COUNT(dbo.Weekly.Bounty) as TotalBoxes FROM dbo.Weekly where dbo.Weekly.Bounty='1' and   dbo.Weekly.Week>'" + mondayOfLastWeek + "' and   dbo.Weekly.Week<'" + NextDays + "' ";
            //SqlDataAdapter da = new SqlDataAdapter("SELECT COUNT(dbo.Weekly.Bounty) as TotalBoxes,COUNT(dbo.VacationDetails.VacationAddedDate) as Vacation,COUNT(dbo.VacationDetails.VacationAddedDate) as NPU FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID INNER JOIN dbo.Weekly ON dbo.Subscribers.SubId = dbo.Weekly.SubId where dbo.Weekly.Bounty='1' and   dbo.Weekly.Week>'" + mondayOfLastWeek + "' and   dbo.Weekly.Week<'" + NextDays + "' and   dbo.VacationDetails.VacationAddedDate>'" + mondayOfLastWeek + "' and   dbo.VacationDetails.VacationAddedDate<'" + NextDays + "'  and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
            if (ddlStore.SelectedItem.Text != "- Select Store -")
                Query += "and dbo.Weekly.Location='" + ddlStore.SelectedValue + "'";    
            SqlDataAdapter da = new SqlDataAdapter(Query, cn);
            da.Fill(ds);
            //SqlDataAdapter da2 = new SqlDataAdapter("Select COUNT(dbo.VacationDetails.VacationAddedDate) as Vacation,COUNT(dbo.VacationDetails.VacationAddedDate) as NPU From dbo.VacationDetails  where   dbo.VacationDetails.VacationAddedDate>'" + mondayOfLastWeek + "' and  dbo.VacationDetails.VacationAddedDate<'" + NextDays + "'", cn);

            string Query1 = "SELECT COUNT(dbo.VacationDetails.VacationAddedDate) as Vacation,COUNT(dbo.VacationDetails.VacationAddedDate) as NPU FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID  where   dbo.VacationDetails.VacationAddedDate>'" + mondayOfLastWeek + "' and  dbo.VacationDetails.VacationAddedDate<'" + NextDays + "'";
            if (ddlStore.SelectedItem.Text != "- Select Store -")
            {
                Query1 += "and  dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'";
            }

            SqlDataAdapter da2 = new SqlDataAdapter(Query1, cn);
            da2.Fill(ds2);
            int TotalBoxes = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalBoxes"]);
            int Vacation = Convert.ToInt32(ds2.Tables[0].Rows[0]["Vacation"]);
            int NPU = Convert.ToInt32(ds2.Tables[0].Rows[0]["NPU"]);
            dt.Rows.Add(TotalBoxes, Vacation, NPU);

        }


        if (dt.Rows.Count > 0)
        {
            gvWeeklyReports.DataSource = dt;
            gvWeeklyReports.DataBind();
        }
        else
        {
            gvWeeklyReports.DataSource = null;
            gvWeeklyReports.DataBind();
        }
    }

    private void NPULastWeekRecord()
    {
        DateTime mondayOfLastWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6);
        DateTime NextDays = mondayOfLastWeek.AddDays(6);
        SqlConnection cn = Constant.Connection();
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.FirstName1, dbo.Subscribers.LastName1, dbo.Subscribers.Username, dbo.VacationDetails.*, dbo.Subscribers.PickupDay, dbo.Subscribers.Bounty, dbo.Subscribers.BarnYard, dbo.Subscribers.Ploughman, dbo.Subscribers.Store FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID Where VacationDetails.VacationAddedDate>'" + mondayOfLastWeek + "' and  VacationDetails.VacationAddedDate<'" + NextDays + "'", cn);
        da.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvNPU.DataSource = ds.Tables[0];
            gvNPU.DataBind();
        }
        else
        {
            gvNPU.DataSource = null;
            gvNPU.DataBind();
        }
    }
    /// <summary>
    /// Store Grid Of Last Week Record
    /// </summary>
    private void StoreLastWeekRecordGrid()
    {
        DateTime mondayOfLastWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6);
        DateTime NextDays = mondayOfLastWeek.AddDays(5);
        SqlConnection cn = Constant.Connection();
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID Where PurchaseProduct.PurchaseDate>'" + mondayOfLastWeek + "' and  PurchaseProduct.PurchaseDate<'" + NextDays + "'", cn);
        da.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvStore.DataSource = ds.Tables[0];
            gvStore.DataBind();
        }
        else
        {
            gvStore.DataSource = null;
            gvStore.DataBind();
        }
    }
    /// <summary>
    /// Home Delivery Grid Of Last Week Record
    /// </summary>
    private void HDLastWeekRecordGrid()
    {
        DateTime mondayOfLastWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6);
        DateTime NextDays = mondayOfLastWeek.AddDays(5);
        SqlConnection cn = Constant.Connection();
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter("SELECT Subscribers.SubId, Subscribers.Username, PurchaseProduct.PurchaseDate FROM Subscribers INNER JOIN PurchaseProduct ON Subscribers.SubId = PurchaseProduct.SubscriberID Where PurchaseProduct.PurchaseDate>'" + mondayOfLastWeek + "' and  PurchaseProduct.PurchaseDate<'" + NextDays + "' and dbo.PurchaseProduct.OnlineHome='1'", cn);
        da.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvHomeDelivery.DataSource = ds.Tables[0];
            gvHomeDelivery.DataBind();
        }
        else
        {
            gvHomeDelivery.DataSource = null;
            gvHomeDelivery.DataBind();
        }
    }
    /// <summary>
    /// Time Tracking Grid Of Last Week Record
    /// </summary>
    private void TTLastWeekRecordGrid()
    {
        DateTime mondayOfLastWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6);
        DateTime NextDays = mondayOfLastWeek.AddDays(5);
        SqlConnection cn = Constant.Connection();
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter("SELECT Subscribers.SubId, Subscribers.Username, PurchaseProduct.PurchaseDate FROM Subscribers INNER JOIN PurchaseProduct ON Subscribers.SubId = PurchaseProduct.SubscriberID Where PurchaseProduct.PurchaseDate>'" + mondayOfLastWeek + "' and  PurchaseProduct.PurchaseDate<'" + NextDays + "'", cn);
        da.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvTimeTracking.DataSource = ds.Tables[0];
            gvTimeTracking.DataBind();
        }
        else
        {
            gvTimeTracking.DataSource = null;
            gvTimeTracking.DataBind();
        }
    }

    private void StoreDropdown()
    {
        //SqlConnection cn = Constant.Connection();
        //SqlDataAdapter da = new SqlDataAdapter("select * From Stores", cn);
        //DataSet ds = new DataSet();
        //da.Fill(ds);
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    ddlStoreNPU.DataSource = ds.Tables[0];
        //    ddlStoreNPU.DataTextField = "Store";
        //    ddlStoreNPU.DataValueField = "Store";
        //    ddlStoreNPU.DataBind();
        //}

    }

    /// <summary>
    /// Bind Weeks to dropdown
    /// </summary>
    private void VacationWeekDropdown()
    {
        SqlConnection cn = Constant.Connection();
        SqlDataAdapter da = new SqlDataAdapter("select distinct VacationWeek From VacationDetails order by VacationWeek", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlWeeks.DataSource = ds.Tables[0];
            ddlWeeks.DataTextField = "VacationWeek";
            ddlWeeks.DataValueField = "VacationWeek";
            ddlWeeks.DataBind();
        }
    }
    /// <summary>
    /// Bind NPU List
    /// </summary>
    private void BindNPU()
    {
        SqlConnection cn = Constant.Connection();
        DataSet ds = new DataSet();

        if (ddlStore.SelectedValue == "")
        {
            if (ddlPickupDay.SelectedValue == "Select Pickup Day" && WeekList.SelectedValue == " - Select a Week - ")
            {
                if (txtNPU.Text == string.Empty)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.FirstName1, dbo.Subscribers.LastName1, dbo.Subscribers.Username, dbo.VacationDetails.*, dbo.Subscribers.PickupDay, dbo.Subscribers.Bounty, dbo.Subscribers.BarnYard, dbo.Subscribers.Ploughman, dbo.Subscribers.Store FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID where dbo.VacationDetails.VacationWeek='" + ddlWeeks.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtNPU.Text != string.Empty)
                {
                    string User = txtNPU.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.FirstName1, dbo.Subscribers.LastName1, dbo.Subscribers.Username, dbo.VacationDetails.*, dbo.Subscribers.PickupDay, dbo.Subscribers.Bounty, dbo.Subscribers.BarnYard, dbo.Subscribers.Ploughman, dbo.Subscribers.Store FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID where dbo.VacationDetails.VacationWeek='" + ddlWeeks.SelectedValue + "' and dbo.Subscribers.Username='" + txtNPU.Text + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvNPU.DataSource = ds.Tables[0];

                    gvNPU.DataBind();
                }
                else
                {
                    gvNPU.DataSource = null;
                    gvNPU.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue != "Select Pickup Day" && WeekList.SelectedValue == " - Select a Week - ")
            {
                if (txtNPU.Text == string.Empty)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.FirstName1, dbo.Subscribers.LastName1, dbo.Subscribers.Username, dbo.VacationDetails.*, dbo.Subscribers.PickupDay, dbo.Subscribers.Bounty, dbo.Subscribers.BarnYard, dbo.Subscribers.Ploughman, dbo.Subscribers.Store FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID where dbo.VacationDetails.VacationWeek='" + ddlWeeks.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "' and  Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtNPU.Text != string.Empty)
                {
                    string User = txtNPU.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.FirstName1, dbo.Subscribers.LastName1, dbo.Subscribers.Username, dbo.VacationDetails.*, dbo.Subscribers.PickupDay, dbo.Subscribers.Bounty, dbo.Subscribers.BarnYard, dbo.Subscribers.Ploughman, dbo.Subscribers.Store FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID where dbo.VacationDetails.VacationWeek='" + ddlWeeks.SelectedValue + "' and dbo.Subscribers.Username='" + txtNPU.Text + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "' and  Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'", cn);
                    da.Fill(ds);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvNPU.DataSource = ds.Tables[0];

                    gvNPU.DataBind();
                }
                else
                {
                    gvNPU.DataSource = null;
                    gvNPU.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue != "Select Pickup Day" && WeekList.SelectedValue != " - Select a Week - ")
            {
                if (txtNPU.Text == string.Empty)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.FirstName1, dbo.Subscribers.LastName1, dbo.Subscribers.Username, dbo.VacationDetails.*, dbo.Subscribers.PickupDay, dbo.Subscribers.Bounty, dbo.Subscribers.BarnYard, dbo.Subscribers.Ploughman, dbo.Subscribers.Store FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID where dbo.VacationDetails.VacationWeek='" + ddlWeeks.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "' and  Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.VacationDetails.VacationWeek='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtNPU.Text != string.Empty)
                {
                    string User = txtNPU.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.FirstName1, dbo.Subscribers.LastName1, dbo.Subscribers.Username, dbo.VacationDetails.*, dbo.Subscribers.PickupDay, dbo.Subscribers.Bounty, dbo.Subscribers.BarnYard, dbo.Subscribers.Ploughman, dbo.Subscribers.Store FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID where dbo.VacationDetails.VacationWeek='" + ddlWeeks.SelectedValue + "' and dbo.Subscribers.Username='" + txtNPU.Text + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "' and  Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'and dbo.VacationDetails.VacationWeek='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvNPU.DataSource = ds.Tables[0];

                    gvNPU.DataBind();
                }
                else
                {
                    gvNPU.DataSource = null;
                    gvNPU.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue == "Select Pickup Day" && WeekList.SelectedValue != " - Select a Week - ")
            {
                if (txtNPU.Text == string.Empty)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.FirstName1, dbo.Subscribers.LastName1, dbo.Subscribers.Username, dbo.VacationDetails.*, dbo.Subscribers.PickupDay, dbo.Subscribers.Bounty, dbo.Subscribers.BarnYard, dbo.Subscribers.Ploughman, dbo.Subscribers.Store FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID where dbo.Subscribers.Store='" + ddlStore.SelectedValue + "' and  Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.VacationDetails.VacationWeek='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtNPU.Text != string.Empty)
                {
                    string User = txtNPU.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.FirstName1, dbo.Subscribers.LastName1, dbo.Subscribers.Username, dbo.VacationDetails.*, dbo.Subscribers.PickupDay, dbo.Subscribers.Bounty, dbo.Subscribers.BarnYard, dbo.Subscribers.Ploughman, dbo.Subscribers.Store FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID where dbo.Subscribers.Username='" + txtNPU.Text + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "' and  Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'and dbo.VacationDetails.VacationWeek='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvNPU.DataSource = ds.Tables[0];

                    gvNPU.DataBind();
                }
                else
                {
                    gvNPU.DataSource = null;
                    gvNPU.DataBind();
                }
            }
        }
        else//Store
        {
            if (ddlPickupDay.SelectedValue == "Select Pickup Day" && WeekList.SelectedValue == " - Select a Week - ")
            {
                if (txtNPU.Text == string.Empty)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.FirstName1, dbo.Subscribers.LastName1, dbo.Subscribers.Username, dbo.VacationDetails.*, dbo.Subscribers.PickupDay, dbo.Subscribers.Bounty, dbo.Subscribers.BarnYard, dbo.Subscribers.Ploughman, dbo.Subscribers.Store FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID where dbo.VacationDetails.VacationWeek='" + ddlWeeks.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtNPU.Text != string.Empty)
                {
                    string User = txtNPU.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.FirstName1, dbo.Subscribers.LastName1, dbo.Subscribers.Username, dbo.VacationDetails.*, dbo.Subscribers.PickupDay, dbo.Subscribers.Bounty, dbo.Subscribers.BarnYard, dbo.Subscribers.Ploughman, dbo.Subscribers.Store FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID where dbo.VacationDetails.VacationWeek='" + ddlWeeks.SelectedValue + "' and dbo.Subscribers.Username='" + txtNPU.Text + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvNPU.DataSource = ds.Tables[0];

                    gvNPU.DataBind();
                }
                else
                {
                    gvNPU.DataSource = null;
                    gvNPU.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue != "Select Pickup Day" && WeekList.SelectedValue == " - Select a Week - ")
            {
                if (txtNPU.Text == string.Empty)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.FirstName1, dbo.Subscribers.LastName1, dbo.Subscribers.Username, dbo.VacationDetails.*, dbo.Subscribers.PickupDay, dbo.Subscribers.Bounty, dbo.Subscribers.BarnYard, dbo.Subscribers.Ploughman, dbo.Subscribers.Store FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID where dbo.VacationDetails.VacationWeek='" + ddlWeeks.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "' and  Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtNPU.Text != string.Empty)
                {
                    string User = txtNPU.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.FirstName1, dbo.Subscribers.LastName1, dbo.Subscribers.Username, dbo.VacationDetails.*, dbo.Subscribers.PickupDay, dbo.Subscribers.Bounty, dbo.Subscribers.BarnYard, dbo.Subscribers.Ploughman, dbo.Subscribers.Store FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID where dbo.VacationDetails.VacationWeek='" + ddlWeeks.SelectedValue + "' and dbo.Subscribers.Username='" + txtNPU.Text + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "' and  Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvNPU.DataSource = ds.Tables[0];

                    gvNPU.DataBind();
                }
                else
                {
                    gvNPU.DataSource = null;
                    gvNPU.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue != "Select Pickup Day" && WeekList.SelectedValue != " - Select a Week - ")
            {
                if (txtNPU.Text == string.Empty)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.FirstName1, dbo.Subscribers.LastName1, dbo.Subscribers.Username, dbo.VacationDetails.*, dbo.Subscribers.PickupDay, dbo.Subscribers.Bounty, dbo.Subscribers.BarnYard, dbo.Subscribers.Ploughman, dbo.Subscribers.Store FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID where dbo.VacationDetails.VacationWeek='" + ddlWeeks.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "' and  Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.VacationDetails.VacationWeek='" + WeekList.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtNPU.Text != string.Empty)
                {
                    string User = txtNPU.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.FirstName1, dbo.Subscribers.LastName1, dbo.Subscribers.Username, dbo.VacationDetails.*, dbo.Subscribers.PickupDay, dbo.Subscribers.Bounty, dbo.Subscribers.BarnYard, dbo.Subscribers.Ploughman, dbo.Subscribers.Store FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID where dbo.VacationDetails.VacationWeek='" + ddlWeeks.SelectedValue + "' and dbo.Subscribers.Username='" + txtNPU.Text + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "' and  Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'and dbo.VacationDetails.VacationWeek='" + WeekList.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvNPU.DataSource = ds.Tables[0];

                    gvNPU.DataBind();
                }
                else
                {
                    gvNPU.DataSource = null;
                    gvNPU.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue == "Select Pickup Day" && WeekList.SelectedValue != " - Select a Week - ")
            {
                if (txtNPU.Text == string.Empty)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.FirstName1, dbo.Subscribers.LastName1, dbo.Subscribers.Username, dbo.VacationDetails.*, dbo.Subscribers.PickupDay, dbo.Subscribers.Bounty, dbo.Subscribers.BarnYard, dbo.Subscribers.Ploughman, dbo.Subscribers.Store FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID where dbo.Subscribers.Store='" + ddlStore.SelectedValue + "' and  Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.VacationDetails.VacationWeek='" + WeekList.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtNPU.Text != string.Empty)
                {
                    string User = txtNPU.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.FirstName1, dbo.Subscribers.LastName1, dbo.Subscribers.Username, dbo.VacationDetails.*, dbo.Subscribers.PickupDay, dbo.Subscribers.Bounty, dbo.Subscribers.BarnYard, dbo.Subscribers.Ploughman, dbo.Subscribers.Store FROM dbo.Subscribers INNER JOIN dbo.VacationDetails ON dbo.Subscribers.SubId = dbo.VacationDetails.CustomerID where dbo.Subscribers.Username='" + txtNPU.Text + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "' and  Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'and dbo.VacationDetails.VacationWeek='" + WeekList.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvNPU.DataSource = ds.Tables[0];

                    gvNPU.DataBind();
                }
                else
                {
                    gvNPU.DataSource = null;
                    gvNPU.DataBind();
                }
            }
        }
    }
    /// <summary>
    /// Bind Store
    /// </summary>
    private void BindStore()
    {
        SqlConnection cn = Constant.Connection();
        DataSet ds = new DataSet();
        if (ddlStore.SelectedValue == "")
        {
            if (ddlPickupDay.SelectedValue == "Select Pickup Day" && WeekList.SelectedValue == " - Select a Week - ")
            {
                if (txtStore.Text == string.Empty && dpStore.SelectedDate == null)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID", cn);
                    da.Fill(ds);
                }
                else if (txtStore.Text != string.Empty && dpStore.SelectedDate == null)
                {
                    string User = txtStore.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%'", cn);
                    da.Fill(ds);
                }
                else if (txtStore.Text == string.Empty && dpStore.SelectedDate != null)
                {
                    DateTime Date = (DateTime)dpStore.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where PurchaseProduct.PurchaseDate = '" + searchdate + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    DateTime Date = (DateTime)dpStore.SelectedDate;
                    string User = txtStore.Text;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and PurchaseProduct.PurchaseDate = '" + searchdate + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvStore.DataSource = ds.Tables[0];
                    gvStore.DataBind();
                }
                else
                {
                    gvStore.DataSource = null;
                    gvStore.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue != "Select Pickup Day" && WeekList.SelectedValue == " - Select a Week - ")
            {
                if (txtStore.Text == string.Empty && dpStore.SelectedDate == null)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  WHERE Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtStore.Text != string.Empty && dpStore.SelectedDate == null)
                {
                    string User = txtStore.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtStore.Text == string.Empty && dpStore.SelectedDate != null)
                {
                    DateTime Date = (DateTime)dpStore.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where PurchaseProduct.PurchaseDate = '" + searchdate + "' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    DateTime Date = (DateTime)dpStore.SelectedDate;
                    string User = txtStore.Text;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and PurchaseProduct.PurchaseDate = '" + searchdate + "' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvStore.DataSource = ds.Tables[0];
                    gvStore.DataBind();
                }
                else
                {
                    gvStore.DataSource = null;
                    gvStore.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue != "Select Pickup Day" && WeekList.SelectedValue != " - Select a Week - ")
            {
                if (txtStore.Text == string.Empty && dpStore.SelectedDate == null)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  WHERE Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + " and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtStore.Text != string.Empty && dpStore.SelectedDate == null)
                {
                    string User = txtStore.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtStore.Text == string.Empty && dpStore.SelectedDate != null)
                {
                    DateTime Date = (DateTime)dpStore.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where PurchaseProduct.PurchaseDate = '" + searchdate + "' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    DateTime Date = (DateTime)dpStore.SelectedDate;
                    string User = txtStore.Text;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and PurchaseProduct.PurchaseDate = '" + searchdate + "' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + " and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvStore.DataSource = ds.Tables[0];
                    gvStore.DataBind();
                }
                else
                {
                    gvStore.DataSource = null;
                    gvStore.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue == "Select Pickup Day" && WeekList.SelectedValue != " - Select a Week - ")
            {
                if (txtStore.Text == string.Empty && dpStore.SelectedDate == null)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  WHERE dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtStore.Text != string.Empty && dpStore.SelectedDate == null)
                {
                    string User = txtStore.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtStore.Text == string.Empty && dpStore.SelectedDate != null)
                {
                    DateTime Date = (DateTime)dpStore.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where PurchaseProduct.PurchaseDate = '" + searchdate + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    DateTime Date = (DateTime)dpStore.SelectedDate;
                    string User = txtStore.Text;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and PurchaseProduct.PurchaseDate = '" + searchdate + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvStore.DataSource = ds.Tables[0];
                    gvStore.DataBind();
                }
                else
                {
                    gvStore.DataSource = null;
                    gvStore.DataBind();
                }
            }
        }
        else//Store
        {
            if (ddlPickupDay.SelectedValue == "Select Pickup Day" && WeekList.SelectedValue == " - Select a Week - ")
            {
                if (txtStore.Text == string.Empty && dpStore.SelectedDate == null)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID WHERE dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtStore.Text != string.Empty && dpStore.SelectedDate == null)
                {
                    string User = txtStore.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtStore.Text == string.Empty && dpStore.SelectedDate != null)
                {
                    DateTime Date = (DateTime)dpStore.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where PurchaseProduct.PurchaseDate = '" + searchdate + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    DateTime Date = (DateTime)dpStore.SelectedDate;
                    string User = txtStore.Text;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and PurchaseProduct.PurchaseDate = '" + searchdate + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvStore.DataSource = ds.Tables[0];
                    gvStore.DataBind();
                }
                else
                {
                    gvStore.DataSource = null;
                    gvStore.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue != "Select Pickup Day" && WeekList.SelectedValue == " - Select a Week - ")
            {
                if (txtStore.Text == string.Empty && dpStore.SelectedDate == null)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  WHERE Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtStore.Text != string.Empty && dpStore.SelectedDate == null)
                {
                    string User = txtStore.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtStore.Text == string.Empty && dpStore.SelectedDate != null)
                {
                    DateTime Date = (DateTime)dpStore.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where PurchaseProduct.PurchaseDate = '" + searchdate + "' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    DateTime Date = (DateTime)dpStore.SelectedDate;
                    string User = txtStore.Text;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and PurchaseProduct.PurchaseDate = '" + searchdate + "' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvStore.DataSource = ds.Tables[0];
                    gvStore.DataBind();
                }
                else
                {
                    gvStore.DataSource = null;
                    gvStore.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue != "Select Pickup Day" && WeekList.SelectedValue != " - Select a Week - ")
            {
                if (txtStore.Text == string.Empty && dpStore.SelectedDate == null)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  WHERE Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtStore.Text != string.Empty && dpStore.SelectedDate == null)
                {
                    string User = txtStore.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtStore.Text == string.Empty && dpStore.SelectedDate != null)
                {
                    DateTime Date = (DateTime)dpStore.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where PurchaseProduct.PurchaseDate = '" + searchdate + "' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    DateTime Date = (DateTime)dpStore.SelectedDate;
                    string User = txtStore.Text;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and PurchaseProduct.PurchaseDate = '" + searchdate + "' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + " and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvStore.DataSource = ds.Tables[0];
                    gvStore.DataBind();
                }
                else
                {
                    gvStore.DataSource = null;
                    gvStore.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue == "Select Pickup Day" && WeekList.SelectedValue != " - Select a Week - ")
            {
                if (txtStore.Text == string.Empty && dpStore.SelectedDate == null)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  WHERE dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtStore.Text != string.Empty && dpStore.SelectedDate == null)
                {
                    string User = txtStore.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtStore.Text == string.Empty && dpStore.SelectedDate != null)
                {
                    DateTime Date = (DateTime)dpStore.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where PurchaseProduct.PurchaseDate = '" + searchdate + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    DateTime Date = (DateTime)dpStore.SelectedDate;
                    string User = txtStore.Text;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.SubId, dbo.Subscribers.Username, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and PurchaseProduct.PurchaseDate = '" + searchdate + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvStore.DataSource = ds.Tables[0];
                    gvStore.DataBind();
                }
                else
                {
                    gvStore.DataSource = null;
                    gvStore.DataBind();
                }
            }
        }
    }
    /// <summary>
    /// Bind Home Delivery
    /// </summary>
    private void BindHomeDelivery()
    {
        SqlConnection cn = Constant.Connection();
        DataSet ds = new DataSet();
        if (ddlStore.SelectedValue == "")
        {
            if (ddlPickupDay.SelectedValue == "Select Pickup Day" && WeekList.SelectedValue == " - Select a Week - ")
            {
                if (txtHD.Text == string.Empty && dpHD.SelectedDate == null)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.HomeDeliverySubscriber.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  WHERE dbo.PurchaseProduct.OnlineHome='1'", cn);
                    da.Fill(ds);
                }
                else if (txtHD.Text != string.Empty && dpHD.SelectedDate == null)
                {
                    string User = txtHD.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.HomeDeliverySubscriber.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  WHERE dbo.PurchaseProduct.OnlineHome='1' and Subscribers.Username like '%" + User + "%'", cn);
                    da.Fill(ds);
                }
                else if (txtHD.Text == string.Empty && dpHD.SelectedDate != null)
                {
                    string User = txtHD.Text.Trim();
                    DateTime Date = (DateTime)dpHD.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.HomeDeliverySubscriber.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  WHERE dbo.PurchaseProduct.OnlineHome='1' and  PurchaseProduct.PurchaseDate = '" + searchdate + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    string User = txtHD.Text.Trim();
                    DateTime Date = (DateTime)dpHD.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.HomeDeliverySubscriber.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  WHERE dbo.PurchaseProduct.OnlineHome='1' and  PurchaseProduct.PurchaseDate = '" + searchdate + "' and Subscribers.Username like '%" + User + "%'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvHomeDelivery.DataSource = ds.Tables[0];
                    ViewState["HomeDelivery"] = ds.Tables[0];
                    gvHomeDelivery.DataBind();
                }
                else
                {
                    ViewState["HomeDelivery"] = null;
                    gvHomeDelivery.DataSource = null;
                    gvHomeDelivery.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue != "Select Pickup Day" && WeekList.SelectedValue == " - Select a Week - ")
            {
                if (txtHD.Text == string.Empty && dpHD.SelectedDate == null)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.HomeDeliverySubscriber.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  WHERE dbo.PurchaseProduct.OnlineHome='1' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtHD.Text != string.Empty && dpHD.SelectedDate == null)
                {
                    string User = txtHD.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.HomeDeliverySubscriber.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  WHERE dbo.PurchaseProduct.OnlineHome='1' and Subscribers.Username like '%" + User + "%'and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtHD.Text == string.Empty && dpHD.SelectedDate != null)
                {
                    string User = txtHD.Text.Trim();
                    DateTime Date = (DateTime)dpHD.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.HomeDeliverySubscriber.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  WHERE dbo.PurchaseProduct.OnlineHome='1' and  PurchaseProduct.PurchaseDate = '" + searchdate + "' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    string User = txtHD.Text.Trim();
                    DateTime Date = (DateTime)dpHD.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.HomeDeliverySubscriber.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  WHERE dbo.PurchaseProduct.OnlineHome='1' and  PurchaseProduct.PurchaseDate = '" + searchdate + "' and Subscribers.Username like '%" + User + "%' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["HomeDelivery"] = ds.Tables[0];
                    gvHomeDelivery.DataSource = ds.Tables[0];
                    gvHomeDelivery.DataBind();
                }
                else
                {
                    ViewState["HomeDelivery"] = null;
                    gvHomeDelivery.DataSource = null;
                    gvHomeDelivery.DataBind();
                }

            }
            else if (ddlPickupDay.SelectedValue == "Select Pickup Day" && WeekList.SelectedValue != " - Select a Week - ")
            {
                if (txtHD.Text == string.Empty && dpHD.SelectedDate == null)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.HomeDeliverySubscriber.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  WHERE dbo.PurchaseProduct.OnlineHome='1' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtHD.Text != string.Empty && dpHD.SelectedDate == null)
                {
                    string User = txtHD.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.HomeDeliverySubscriber.*,dbo.Subscribers.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where dbo.PurchaseProduct.OnlineHome='1' and Subscribers.Username like '%" + User + "%' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtHD.Text == string.Empty && dpHD.SelectedDate != null)
                {
                    string User = txtHD.Text.Trim();
                    DateTime Date = (DateTime)dpHD.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.HomeDeliverySubscriber.*,dbo.Subscribers.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where dbo.PurchaseProduct.OnlineHome='1' and  PurchaseProduct.PurchaseDate = '" + searchdate + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    string User = txtHD.Text.Trim();
                    DateTime Date = (DateTime)dpHD.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.HomeDeliverySubscriber.*,dbo.Subscribers.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where dbo.PurchaseProduct.OnlineHome='1' and  PurchaseProduct.PurchaseDate = '" + searchdate + "' and Subscribers.Username like '%" + User + "%' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["HomeDelivery"] = ds.Tables[0];
                    gvHomeDelivery.DataSource = ds.Tables[0];
                    gvHomeDelivery.DataBind();
                }
                else
                {
                    ViewState["HomeDelivery"] = null;
                    gvHomeDelivery.DataSource = null;
                    gvHomeDelivery.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue != "Select Pickup Day" && WeekList.SelectedValue != " - Select a Week - ")
            {
                if (txtHD.Text == string.Empty && dpHD.SelectedDate == null)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.HomeDeliverySubscriber.*,dbo.Subscribers.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  where dbo.PurchaseProduct.OnlineHome='1' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtHD.Text != string.Empty && dpHD.SelectedDate == null)
                {
                    string User = txtHD.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.HomeDeliverySubscriber.*,dbo.Subscribers.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  where dbo.PurchaseProduct.OnlineHome='1' and Subscribers.Username like '%" + User + "%'and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtHD.Text == string.Empty && dpHD.SelectedDate != null)
                {
                    string User = txtHD.Text.Trim();
                    DateTime Date = (DateTime)dpHD.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.HomeDeliverySubscriber.*,dbo.Subscribers.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  where dbo.PurchaseProduct.OnlineHome='1' and  PurchaseProduct.PurchaseDate = '" + searchdate + "' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    string User = txtHD.Text.Trim();
                    DateTime Date = (DateTime)dpHD.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.HomeDeliverySubscriber.*,dbo.Subscribers.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  where dbo.PurchaseProduct.OnlineHome='1' and  PurchaseProduct.PurchaseDate = '" + searchdate + "' and Subscribers.Username like '%" + User + "%' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["HomeDelivery"] = ds.Tables[0];
                    gvHomeDelivery.DataSource = ds.Tables[0];
                    gvHomeDelivery.DataBind();
                }
                else
                {
                    ViewState["HomeDelivery"] = null;
                    gvHomeDelivery.DataSource = null;
                    gvHomeDelivery.DataBind();
                }
            }
        }
        else//Store
        {
            if (ddlPickupDay.SelectedValue == "Select Pickup Day" && WeekList.SelectedValue == " - Select a Week - ")
            {
                if (txtHD.Text == string.Empty && dpHD.SelectedDate == null)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.HomeDeliverySubscriber.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  WHERE dbo.PurchaseProduct.OnlineHome='1' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtHD.Text != string.Empty && dpHD.SelectedDate == null)
                {
                    string User = txtHD.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.HomeDeliverySubscriber.*,dbo.Subscribers.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  where dbo.PurchaseProduct.OnlineHome='1' and Subscribers.Username like '%" + User + "%' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtHD.Text == string.Empty && dpHD.SelectedDate != null)
                {
                    string User = txtHD.Text.Trim();
                    DateTime Date = (DateTime)dpHD.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*,dbo.HomeDeliverySubscriber.*, dbo.PurchaseProduct.* FROM dbo.HomeDeliverySubscriber INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  where dbo.PurchaseProduct.OnlineHome='1' and  PurchaseProduct.PurchaseDate = '" + searchdate + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    string User = txtHD.Text.Trim();
                    DateTime Date = (DateTime)dpHD.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*,dbo.HomeDeliverySubscriber.*, dbo.PurchaseProduct.* FROM dbo.HomeDeliverySubscriber INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where dbo.PurchaseProduct.OnlineHome='1' and  PurchaseProduct.PurchaseDate = '" + searchdate + "' and Subscribers.Username like '%" + User + "%' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["HomeDelivery"] = ds.Tables[0];
                    gvHomeDelivery.DataSource = ds.Tables[0];
                    gvHomeDelivery.DataBind();
                }
                else
                {
                    ViewState["HomeDelivery"] = null;
                    gvHomeDelivery.DataSource = null;
                    gvHomeDelivery.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue != "Select Pickup Day" && WeekList.SelectedValue == " - Select a Week - ")
            {
                if (txtHD.Text == string.Empty && dpHD.SelectedDate == null)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.HomeDeliverySubscriber.*,dbo.Subscribers.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where dbo.PurchaseProduct.OnlineHome='1' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtHD.Text != string.Empty && dpHD.SelectedDate == null)
                {
                    string User = txtHD.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.HomeDeliverySubscriber.*,dbo.Subscribers.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where dbo.PurchaseProduct.OnlineHome='1' and Subscribers.Username like '%" + User + "%'and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtHD.Text == string.Empty && dpHD.SelectedDate != null)
                {
                    string User = txtHD.Text.Trim();
                    DateTime Date = (DateTime)dpHD.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.HomeDeliverySubscriber.*,dbo.Subscribers.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where dbo.PurchaseProduct.OnlineHome='1' and  PurchaseProduct.PurchaseDate = '" + searchdate + "' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    string User = txtHD.Text.Trim();
                    DateTime Date = (DateTime)dpHD.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.HomeDeliverySubscriber.*,dbo.Subscribers.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where dbo.PurchaseProduct.OnlineHome='1' and  PurchaseProduct.PurchaseDate = '" + searchdate + "' and Subscribers.Username like '%" + User + "%' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["HomeDelivery"] = ds.Tables[0];
                    gvHomeDelivery.DataSource = ds.Tables[0];
                    gvHomeDelivery.DataBind();
                }
                else
                {
                    ViewState["HomeDelivery"] = null;
                    gvHomeDelivery.DataSource = null;
                    gvHomeDelivery.DataBind();
                }

            }
            else if (ddlPickupDay.SelectedValue == "Select Pickup Day" && WeekList.SelectedValue != " - Select a Week - ")
            {
                if (txtHD.Text == string.Empty && dpHD.SelectedDate == null)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.HomeDeliverySubscriber.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID  WHERE dbo.PurchaseProduct.OnlineHome='1' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtHD.Text != string.Empty && dpHD.SelectedDate == null)
                {
                    string User = txtHD.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where dbo.PurchaseProduct.OnlineHome='1' and Subscribers.Username like '%" + User + "%' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtHD.Text == string.Empty && dpHD.SelectedDate != null)
                {
                    string User = txtHD.Text.Trim();
                    DateTime Date = (DateTime)dpHD.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where dbo.PurchaseProduct.OnlineHome='1' and  PurchaseProduct.PurchaseDate = '" + searchdate + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    string User = txtHD.Text.Trim();
                    DateTime Date = (DateTime)dpHD.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where dbo.PurchaseProduct.OnlineHome='1' and  PurchaseProduct.PurchaseDate = '" + searchdate + "' and Subscribers.Username like '%" + User + "%' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["HomeDelivery"] = ds.Tables[0];
                    gvHomeDelivery.DataSource = ds.Tables[0];
                    gvHomeDelivery.DataBind();
                }
                else
                {
                    ViewState["HomeDelivery"] = null;
                    gvHomeDelivery.DataSource = null;
                    gvHomeDelivery.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue != "Select Pickup Day" && WeekList.SelectedValue != " - Select a Week - ")
            {
                if (txtHD.Text == string.Empty && dpHD.SelectedDate == null)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where dbo.PurchaseProduct.OnlineHome='1' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtHD.Text != string.Empty && dpHD.SelectedDate == null)
                {
                    string User = txtHD.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where dbo.PurchaseProduct.OnlineHome='1' and Subscribers.Username like '%" + User + "%'and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else if (txtHD.Text == string.Empty && dpHD.SelectedDate != null)
                {
                    string User = txtHD.Text.Trim();
                    DateTime Date = (DateTime)dpHD.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where dbo.PurchaseProduct.OnlineHome='1' and  PurchaseProduct.PurchaseDate = '" + searchdate + "' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    string User = txtHD.Text.Trim();
                    DateTime Date = (DateTime)dpHD.SelectedDate;
                    string searchdate = Date.ToShortDateString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.PurchaseProduct.* FROM dbo.Subscribers INNER JOIN dbo.PurchaseProduct ON dbo.Subscribers.SubId = dbo.PurchaseProduct.SubscriberID where dbo.PurchaseProduct.OnlineHome='1' and  PurchaseProduct.PurchaseDate = '" + searchdate + "' and Subscribers.Username like '%" + User + "%' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "' and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["HomeDelivery"] = ds.Tables[0];
                    gvHomeDelivery.DataSource = ds.Tables[0];
                    gvHomeDelivery.DataBind();
                }
                else
                {
                    ViewState["HomeDelivery"] = null;
                    gvHomeDelivery.DataSource = null;
                    gvHomeDelivery.DataBind();
                }
            }
        }
    }
    /// <summary>
    /// Bind Time Tracking Grid
    /// </summary>
    private void BindTimeTracking()
    {
        SqlConnection cn = Constant.Connection();
        DataSet ds = new DataSet();

        if (ddlStore.SelectedValue == "")
        {
            if (ddlPickupDay.SelectedValue == "Select Pickup Day" && WeekList.SelectedValue == " - Select a Week - ")
            {
                if (txtTT.Text == string.Empty)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Subscribers.*, PurchaseProduct.* FROM Subscribers INNER JOIN PurchaseProduct ON Subscribers.SubId = PurchaseProduct.SubscriberID", cn);
                    da.Fill(ds);
                }
                else
                {
                    string User = txtTT.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Subscribers.*, PurchaseProduct.* FROM Subscribers INNER JOIN PurchaseProduct ON Subscribers.SubId = PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvTimeTracking.DataSource = ds.Tables[0];
                    gvTimeTracking.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue != "Select Pickup Day" && WeekList.SelectedValue == " - Select a Week - ")
            {
                if (txtTT.Text == string.Empty)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Subscribers.*, PurchaseProduct.* FROM Subscribers INNER JOIN PurchaseProduct ON Subscribers.SubId = PurchaseProduct.SubscriberID WHERE Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    string User = txtTT.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Subscribers.*, PurchaseProduct.* FROM Subscribers INNER JOIN PurchaseProduct ON Subscribers.SubId = PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvTimeTracking.DataSource = ds.Tables[0];
                    gvTimeTracking.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue != "Select Pickup Day" && WeekList.SelectedValue != " - Select a Week - ")
            {
                if (txtTT.Text == string.Empty)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Subscribers.*, PurchaseProduct.* FROM Subscribers INNER JOIN PurchaseProduct ON Subscribers.SubId = PurchaseProduct.SubscriberID WHERE Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    string User = txtTT.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Subscribers.*, PurchaseProduct.* FROM Subscribers INNER JOIN PurchaseProduct ON Subscribers.SubId = PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvTimeTracking.DataSource = ds.Tables[0];
                    gvTimeTracking.DataBind();
                }
                else
                {
                    gvTimeTracking.DataSource = null;
                    gvTimeTracking.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue == "Select Pickup Day" && WeekList.SelectedValue != " - Select a Week - ")
            {
                if (txtTT.Text == string.Empty)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Subscribers.*, PurchaseProduct.* FROM Subscribers INNER JOIN PurchaseProduct ON Subscribers.SubId = PurchaseProduct.SubscriberID WHERE dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    string User = txtTT.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Subscribers.*, PurchaseProduct.* FROM Subscribers INNER JOIN PurchaseProduct ON Subscribers.SubId = PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvTimeTracking.DataSource = ds.Tables[0];
                    gvTimeTracking.DataBind();
                }
                else
                {
                    gvTimeTracking.DataSource = null;
                    gvTimeTracking.DataBind();
                }
            }
        }
        else//Store
        {
            if (ddlPickupDay.SelectedValue == "Select Pickup Day" && WeekList.SelectedValue == " - Select a Week - ")
            {
                if (txtTT.Text == string.Empty)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Subscribers.*, PurchaseProduct.* FROM Subscribers INNER JOIN PurchaseProduct ON Subscribers.SubId = PurchaseProduct.SubscriberID  and Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    string User = txtTT.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Subscribers.*, PurchaseProduct.* FROM Subscribers INNER JOIN PurchaseProduct ON Subscribers.SubId = PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvTimeTracking.DataSource = ds.Tables[0];
                    gvTimeTracking.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue != "Select Pickup Day" && WeekList.SelectedValue == " - Select a Week - ")
            {
                if (txtTT.Text == string.Empty)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Subscribers.*, PurchaseProduct.* FROM Subscribers INNER JOIN PurchaseProduct ON Subscribers.SubId = PurchaseProduct.SubscriberID WHERE Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    string User = txtTT.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Subscribers.*, PurchaseProduct.* FROM Subscribers INNER JOIN PurchaseProduct ON Subscribers.SubId = PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvTimeTracking.DataSource = ds.Tables[0];
                    gvTimeTracking.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue != "Select Pickup Day" && WeekList.SelectedValue != " - Select a Week - ")
            {
                if (txtTT.Text == string.Empty)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Subscribers.*, PurchaseProduct.* FROM Subscribers INNER JOIN PurchaseProduct ON Subscribers.SubId = PurchaseProduct.SubscriberID WHERE Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "' and Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    string User = txtTT.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Subscribers.*, PurchaseProduct.* FROM Subscribers INNER JOIN PurchaseProduct ON Subscribers.SubId = PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "' and Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvTimeTracking.DataSource = ds.Tables[0];
                    gvTimeTracking.DataBind();
                }
                else
                {
                    gvTimeTracking.DataSource = null;
                    gvTimeTracking.DataBind();
                }
            }
            else if (ddlPickupDay.SelectedValue == "Select Pickup Day" && WeekList.SelectedValue != " - Select a Week - ")
            {
                if (txtTT.Text == string.Empty)
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Subscribers.*, PurchaseProduct.* FROM Subscribers INNER JOIN PurchaseProduct ON Subscribers.SubId = PurchaseProduct.SubscriberID WHERE dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "' and Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                else
                {
                    string User = txtTT.Text.Trim();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Subscribers.*, PurchaseProduct.* FROM Subscribers INNER JOIN PurchaseProduct ON Subscribers.SubId = PurchaseProduct.SubscriberID where Subscribers.Username like '%" + User + "%' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "' and Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvTimeTracking.DataSource = ds.Tables[0];
                    gvTimeTracking.DataBind();
                }
                else
                {
                    gvTimeTracking.DataSource = null;
                    gvTimeTracking.DataBind();
                }
            }
        }
    }
    /// <summary>
    /// Bind Notes 
    /// </summary>
    private void BindNotes()
    {
        SqlConnection cn = Constant.Connection();
        DataSet ds = new DataSet();

        string NotesQuery = "SELECT dbo.Subscribers.Username, dbo.Subscribers.SubId, dbo.Subscribers.Notes As PermanentNotes, dbo.Weekly.Notes AS WeeklyNotes, dbo.Weekly.Week FROM dbo.Subscribers INNER JOIN dbo.Weekly ON dbo.Subscribers.SubId = dbo.Weekly.SubId WHERE (dbo.Weekly.Notes != '' and dbo.Subscribers.Notes != '') and dbo.Subscribers.Active='True'";

        if (WeekList.SelectedValue != " - Select a Week - ")
        {
            NotesQuery += " and dbo.Weekly.WeekValue='" + WeekList.SelectedValue + "'";
        }
        if (ddlStore.SelectedValue != "")
        {
            NotesQuery += " and dbo.Subscribers.Store='" + ddlStore.SelectedValue + "'";
        }
        if (ddlPickupDay.SelectedValue != "Select Pickup Day")
        {
            NotesQuery += " and dbo.Subscribers.PickupDay='" + ddlStore.SelectedValue + "'";
        }
        if (txtUserNameNotes.Text != string.Empty)
        {
            NotesQuery += " and dbo.Subscribers.Username like '%" + txtUserNameNotes.Text + "%'";
        }
        SqlDataAdapter da = new SqlDataAdapter(NotesQuery, cn);
        da.Fill(ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvNotes.DataSource = ds.Tables[0];
            gvNotes.DataBind();
        }
        else
        {
            gvNotes.DataSource = null;
            gvNotes.DataBind();
        }


    }
    /// <summary>
    /// Bind Allergies
    /// </summary>
    private void BindAllergies()
    {
        SqlConnection cn = Constant.Connection();
        DataSet ds = new DataSet();

        #region Edit By Harshal


        //if (ddlStore.SelectedValue == "")
        //{
        //    if (ddlPickupDay.SelectedValue == "Select Pickup Day")
        //    {
        //        if (txtCustomerName.Text == string.Empty)
        //        {
        //            //  SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers", cn);
        //            //By Soham
        //            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers where Subscribers.Allergies!='' and Subscribers.Allergies is not null and Subscribers.Allergies!='none' and Subscribers.Allergies!='None' and Subscribers.Allergies!='n/a' and Subscribers.Allergies!='N/A' and Subscribers.Allergies!='&nbsp;' and Subscribers.Active='True'", cn);
        //            da.Fill(ds);
        //        }
        //        else
        //        {
        //            string User = txtCustomerName.Text.Trim();
        //            // SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers where Username like '%" + User + "%' ", cn);
        //            //By Soham
        //            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers where Username like '%" + User + "%' and Subscribers.Allergies!='' and Subscribers.Allergies!='none' and Subscribers.Allergies!='None' and Subscribers.Allergies!='n/a' and Subscribers.Allergies!='N/A' and Subscribers.Allergies is not null and Subscribers.Allergies!='&nbsp;' and Subscribers.Active='True'", cn);
        //            da.Fill(ds);
        //        }
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            gvAllergies.DataSource = ds.Tables[0];
        //            gvAllergies.DataBind();
        //        }
        //    }
        //    else
        //    {
        //        if (txtCustomerName.Text == string.Empty)
        //        {
        //            // SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers WHERE Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'", cn);
        //            //By Soham
        //            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers WHERE Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and Subscribers.Allergies!='' and Subscribers.Allergies!='none' and Subscribers.Allergies!='None' and Subscribers.Allergies!='n/a' and Subscribers.Allergies!='N/A' and Subscribers.Allergies is not null and Subscribers.Allergies!='&nbsp;' and Subscribers.Active='True'", cn);
        //            da.Fill(ds);
        //        }
        //        else
        //        {
        //            string User = txtCustomerName.Text.Trim();
        //            //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers where Username like '%" + User + "%' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'", cn);
        //            //by Soham
        //            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers where Username like '%" + User + "%' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and Subscribers.Allergies!='' and Subscribers.Allergies!='none' and Subscribers.Allergies!='None' and Subscribers.Allergies!='n/a' and Subscribers.Allergies!='N/A' and Subscribers.Allergies is not null and Subscribers.Allergies!='&nbsp;' and Subscribers.Active='True'", cn);
        //            da.Fill(ds);
        //        }
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            gvAllergies.DataSource = ds.Tables[0];
        //            gvAllergies.DataBind();
        //        }
        //    }
        //}
        //else//Sorted by Store
        //{
        //    if (ddlPickupDay.SelectedValue == "Select Pickup Day")
        //    {
        //        if (txtCustomerName.Text == string.Empty)
        //        {
        //            // SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers where Store='" + ddlStore.SelectedValue + "'", cn);
        //            //by Soham
        //            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers where Store='" + ddlStore.SelectedValue + "' and Subscribers.Allergies!='' and Subscribers.Allergies!='none' and Subscribers.Allergies!='None'  and Subscribers.Allergies!='n/a' and Subscribers.Allergies!='N/A' and Subscribers.Allergies is not null  and Subscribers.Allergies!='&nbsp;' and Subscribers.Active='True'", cn);
        //            da.Fill(ds);
        //        }
        //        else
        //        {
        //            string User = txtCustomerName.Text.Trim();
        //            //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers where Username like '%" + User + "%' and Store='" + ddlStore.SelectedValue + "'", cn);
        //            //By soham
        //            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers where Username like '%" + User + "%' and Store='" + ddlStore.SelectedValue + "' and Subscribers.Allergies!='' and Subscribers.Allergies!='none' and Subscribers.Allergies!='None' and Subscribers.Allergies!='n/a' and Subscribers.Allergies!='N/A' and Subscribers.Allergies is not null and Subscribers.Allergies!='&nbsp;' and Subscribers.Active='True'", cn);
        //            da.Fill(ds);
        //        }
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            gvAllergies.DataSource = ds.Tables[0];
        //            gvAllergies.DataBind();
        //        }
        //    }
        //    else
        //    {
        //        if (txtCustomerName.Text == string.Empty)
        //        {
        //            //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers WHERE Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
        //            //By soham
        //            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers WHERE Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and Subscribers.Store='" + ddlStore.SelectedValue + "' and Subscribers.Allergies!='' and Subscribers.Allergies!='none' and Subscribers.Allergies!='None' and Subscribers.Allergies!='n/a' and Subscribers.Allergies!='N/A' and Subscribers.Allergies is not null and Subscribers.Allergies!='&nbsp;' and Subscribers.Active='True'", cn);
        //            da.Fill(ds);
        //        }
        //        else
        //        {
        //            string User = txtCustomerName.Text.Trim();
        //            // SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers where Username like '%" + User + "%' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and Subscribers.Store='" + ddlStore.SelectedValue + "'", cn);
        //            //By Soham
        //            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers where Username like '%" + User + "%' and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "' and Subscribers.Store='" + ddlStore.SelectedValue + "' and Subscribers.Allergies!='' and Subscribers.Allergies!='none' and Subscribers.Allergies!='None' and Subscribers.Allergies!='n/a' and Subscribers.Allergies!='N/A'  and Subscribers.Allergies is not null and Subscribers.Allergies!='&nbsp;' and Subscribers.Active='True'", cn);
        //            da.Fill(ds);
        //        }
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            gvAllergies.DataSource = ds.Tables[0];
        //            gvAllergies.DataBind();
        //        }
        //    }
        //}
        #endregion

        string query = "SELECT * FROM Subscribers WHERE Subscribers.Active='True'";

        if (ddlStore.SelectedValue != "")
        {
            query += " and Store='" + ddlStore.SelectedValue + "'";
        }
        if (ddlPickupDay.SelectedValue != "Select Pickup Day")
        {
            query += " and Subscribers.PickupDay='" + ddlPickupDay.SelectedValue + "'";
        }
        if (txtCustomerName.Text != string.Empty)
        {
            query += " and Username like '%" + txtCustomerName.Text.Trim() + "%'";
        }
        SqlDataAdapter daAl = new SqlDataAdapter(query, cn);
        daAl.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvAllergies.DataSource = ds.Tables[0];
            gvAllergies.DataBind();
        }
    }

    //public override void VerifyRenderingInServerForm(Control control)
    //{
    //    /* Verifies that the control is rendered */
    //}
    protected void btnView_Click(object sender, EventArgs e)
    {
        ExportGridToPDF();
    }
    protected void gvNotes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvNotes.PageIndex = e.NewPageIndex;
        BindNotes();
    }

    protected void gvAllergies_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAllergies.PageIndex = e.NewPageIndex;
        BindAllergies();
    }
    protected void btnSearchCustomer_Click(object sender, EventArgs e)
    {
        BindAllergies();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtCustomerName.Text = string.Empty;
        BindAllergies();
    }
    /// <summary>
    /// Dropdown Select Option
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlReport.SelectedValue == "Allergies")
        {
            divAllergies.Visible = true;
            divNotes.Visible = false;
            divTimeTracking.Visible = false;
            divHomeDelivery.Visible = false;
            divStore.Visible = false;
            divNPU.Visible = false;
            divWeeklyReports.Visible = false;
            divVacation.Visible = false;
            divPickupTT.Visible = false;
        }
        else if (ddlReport.SelectedValue == "Notes")
        {
            divAllergies.Visible = false;
            divNotes.Visible = true;
            divTimeTracking.Visible = false;
            divHomeDelivery.Visible = false;
            divStore.Visible = false;
            divNPU.Visible = false;
            divWeeklyReports.Visible = false;
            divVacation.Visible = false;
            divPickupTT.Visible = false;
        }
        else if (ddlReport.SelectedValue == "Time Tracking")
        {
            divAllergies.Visible = false;
            divNotes.Visible = false;
            divTimeTracking.Visible = true;
            divHomeDelivery.Visible = false;
            divStore.Visible = false;
            divNPU.Visible = false;
            divWeeklyReports.Visible = false;
            divVacation.Visible = false;
            divPickupTT.Visible = false;
        }
        else if (ddlReport.SelectedValue == "Home delivery")
        {
            divAllergies.Visible = false;
            divNotes.Visible = false;
            divTimeTracking.Visible = false;
            divHomeDelivery.Visible = true;
            divStore.Visible = false;
            divNPU.Visible = false;
            divWeeklyReports.Visible = false;
            divVacation.Visible = false;
            divPickupTT.Visible = false;
        }
        else if (ddlReport.SelectedValue == "Store")
        {
            divAllergies.Visible = false;
            divNotes.Visible = false;
            divTimeTracking.Visible = false;
            divHomeDelivery.Visible = false;
            divStore.Visible = true;
            divNPU.Visible = false;
            divWeeklyReports.Visible = false;
            divVacation.Visible = false;
            divPickupTT.Visible = false;
        }
        else if (ddlReport.SelectedValue == "NPU")
        {
            divAllergies.Visible = false;
            divNotes.Visible = false;
            divTimeTracking.Visible = false;
            divHomeDelivery.Visible = false;
            divStore.Visible = false;
            divNPU.Visible = true;
            divWeeklyReports.Visible = false;
            divVacation.Visible = false;
            divPickupTT.Visible = false;
        }
        else if (ddlReport.SelectedValue == "Last Week Report")
        {
            divAllergies.Visible = false;
            divNotes.Visible = false;
            divTimeTracking.Visible = false;
            divHomeDelivery.Visible = false;
            divStore.Visible = false;
            divNPU.Visible = false;
            divWeeklyReports.Visible = true;
            divVacation.Visible = false;
            divPickupTT.Visible = false;
        }
        else if (ddlReport.SelectedValue == "Vacation")
        {
            divAllergies.Visible = false;
            divNotes.Visible = false;
            divTimeTracking.Visible = false;
            divHomeDelivery.Visible = false;
            divStore.Visible = false;
            divNPU.Visible = false;
            divWeeklyReports.Visible = false;
            divVacation.Visible = true;
            divPickupTT.Visible = false;
        }
        else if (ddlReport.SelectedValue == "Pickup Time Tracking")
        {
            divAllergies.Visible = false;
            divNotes.Visible = false;
            divTimeTracking.Visible = false;
            divHomeDelivery.Visible = false;
            divStore.Visible = false;
            divNPU.Visible = false;
            divWeeklyReports.Visible = false;
            divVacation.Visible = false;
            divPickupTT.Visible = true;
        }

    }
    protected void btnSearchNotes_Click(object sender, EventArgs e)
    {
        BindNotes();
    }
    protected void btnClearNotes_Click(object sender, EventArgs e)
    {
        txtUserNameNotes.Text = string.Empty;
        BindNotes();
    }
    protected void gvTimeTracking_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTimeTracking.PageIndex = e.NewPageIndex;
        BindTimeTracking();
    }

    protected void btnSearchHD_Click(object sender, EventArgs e)
    {
        BindHomeDelivery();
    }
    protected void btnCancelHD_Click(object sender, EventArgs e)
    {
        txtHD.Text = string.Empty;
        dpHD.SelectedDate = null;
        BindHomeDelivery();
    }
    protected void gvHomeDelivery_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHomeDelivery.PageIndex = e.NewPageIndex;
        BindHomeDelivery();
    }


    protected void gvStore_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvStore.PageIndex = e.NewPageIndex;
        BindStore();
    }

    protected void btnStoreCancel_Click(object sender, EventArgs e)
    {
        txtStore.Text = string.Empty;
        dpStore.SelectedDate = null;
        BindStore();
    }
    protected void btnStoreFind_Click(object sender, EventArgs e)
    {
        BindStore();
    }

    /// <summary>
    /// Export To PDF
    /// </summary>
    private void ExportGridToPDF()
    {
        try
        {
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    //To Export all pages
                    if (ddlReport.SelectedValue == "Allergies")
                    {
                        gvAllergies.AllowPaging = false;
                        this.BindAllergies();
                        gvAllergies.RenderControl(hw);
                    }
                    else if (ddlReport.SelectedValue == "Notes")
                    {
                        gvNotes.AllowPaging = false;
                        this.BindNotes();
                        gvNotes.RenderControl(hw);
                    }
                    else if (ddlReport.SelectedValue == "Time Tracking")
                    {
                        gvTimeTracking.AllowPaging = false;
                        this.BindTimeTracking();
                        gvTimeTracking.RenderControl(hw);
                    }
                    else if (ddlReport.SelectedValue == "Home delivery")
                    {
                        gvHomeDelivery.AllowPaging = false;
                        this.BindHomeDelivery();
                        gvHomeDelivery.RenderControl(hw);
                    }
                    else if (ddlReport.SelectedValue == "Store")
                    {
                        gvStore.AllowPaging = false;
                        this.BindStore();
                        gvStore.RenderControl(hw);
                    }
                    else if (ddlReport.SelectedValue == "NPU")
                    {
                        gvNPU.AllowPaging = false;
                        this.BindNPU();
                        gvNPU.RenderControl(hw);
                    }
                    else if (ddlReport.SelectedValue == "Last Week Report")
                    {
                        gvWeeklyReports.AllowPaging = false;
                        this.WeeklyReports();
                        gvWeeklyReports.RenderControl(hw);
                    }
                    else if (ddlReport.SelectedValue == "Vacation")
                    {
                        gvVacation.AllowPaging = false;
                        this.BindVacation();
                        gvVacation.RenderControl(hw);
                    }
                    else if (ddlReport.SelectedValue == "Pickup Time Tracking")
                    {
                        gvPickupTT.AllowPaging = false;
                        this.BindPickupTT();
                        gvPickupTT.RenderControl(hw);
                    }

                    StringReader sr = new StringReader(sw.ToString());
                    Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

                    PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    pdfDoc.Open();
                    if (WeekList.SelectedValue == " - Select a Week - ")
                        pdfDoc.Add(new Paragraph("Week:"));
                    else
                        pdfDoc.Add(new Paragraph("Week:" + WeekList.SelectedValue));
                    pdfDoc.Add(new Paragraph("Store:" + ddlStore.SelectedValue));
                    if (ddlPickupDay.SelectedValue == "Select Pickup Day")
                        pdfDoc.Add(new Paragraph("Pickup Day:"));
                    else
                        pdfDoc.Add(new Paragraph("Pickup Day:" + ddlPickupDay.SelectedValue));
                    pdfDoc.Add(new Paragraph(" "));
                    pdfDoc.Add(new Paragraph(" "));
                    htmlparser.Parse(sr);
                    pdfDoc.Close();

                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + ddlReport.SelectedItem.Text + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();
                }
            }
        }
        catch (Exception ex) { }
    }


    protected void btnNPUSearch_Click(object sender, EventArgs e)
    {
        BindNPU();
    }
    protected void btnNPUCancel_Click(object sender, EventArgs e)
    {
        txtNPU.Text = string.Empty;
        BindNPU();
    }
    protected void gvNPU_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvNPU.PageIndex = e.NewPageIndex;
        BindNPU();
    }
    protected void btnLastWeekTT_Click(object sender, EventArgs e)
    {
        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            {
                TTLastWeekRecordGrid();
                gvTimeTracking.AllowPaging = false;

                gvTimeTracking.RenderControl(hw);

                StringReader sr = new StringReader(sw.ToString());
                Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Online Payment Tracking Last Week Record.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
            }
        }
    }
    protected void btnLastWeekHD_Click(object sender, EventArgs e)
    {
        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            {
                HDLastWeekRecordGrid();
                gvHomeDelivery.AllowPaging = false;

                gvHomeDelivery.RenderControl(hw);

                StringReader sr = new StringReader(sw.ToString());
                Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Home Delivery Last Week Record.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
            }
        }
    }
    protected void btnLastWeekStore_Click(object sender, EventArgs e)
    {
        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            {
                StoreLastWeekRecordGrid();
                gvStore.AllowPaging = false;

                gvStore.RenderControl(hw);

                StringReader sr = new StringReader(sw.ToString());
                Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + ddlReport.SelectedItem.Text + ".pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
            }
        }
    }
    protected void btnLastWeekNPU_Click(object sender, EventArgs e)
    {
        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            {
                NPULastWeekRecord();
                gvNPU.AllowPaging = false;

                gvNPU.RenderControl(hw);

                StringReader sr = new StringReader(sw.ToString());
                Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=NPULastWeek.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
            }
        }
    }
    protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindAllergies();
        BindNotes();
        BindTimeTracking();
        BindHomeDelivery();
        BindStore();
        BindNPU();
        BindVacation();
        BindPickupTT();

        WeeklyReports();
    }
    protected void ddlBoxes_SelectedIndexChanged(object sender, EventArgs e)
    {
        WeeklyReports();
    }
    protected void ddlPickupDay_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlReport.SelectedValue == "Allergies")
        {
            BindAllergies();
        }
        else if (ddlReport.SelectedValue == "Notes")
        {
            BindNotes();
        }
        else if (ddlReport.SelectedValue == "Time Tracking")
        {
            BindTimeTracking();
        }
        else if (ddlReport.SelectedValue == "Home delivery")
        {
            BindHomeDelivery();
        }
        else if (ddlReport.SelectedValue == "Store")
        {
            BindStore();
        }
        else if (ddlReport.SelectedValue == "NPU")
        {
            BindNPU();
        }
        else if (ddlReport.SelectedValue == "Vacation")
        {
            BindVacation();
        }
    }
    protected void gvVacation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvVacation.PageIndex = e.NewPageIndex;
        BindVacation();
    }
    protected void btnVactionSearch_Click(object sender, EventArgs e)
    {
        BindVacation();
    }
    protected void btnVacationClear_Click(object sender, EventArgs e)
    {
        txtVacation.Text = string.Empty;
        BindVacation();
    }
    protected void btnPTTClear_Click(object sender, EventArgs e)
    {
        txtPikupTT.Text = string.Empty;
        BindPickupTT();
    }
    protected void btnPTTSearch_Click(object sender, EventArgs e)
    {
        BindPickupTT();
    }
    protected void gvPickupTT_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPickupTT.PageIndex = e.NewPageIndex;
        BindPickupTT();
    }
    protected void WeekList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlReport.SelectedValue == "Allergies")
        {
            BindAllergies();
        }
        else if (ddlReport.SelectedValue == "Notes")
        {
            BindNotes();
        }
        else if (ddlReport.SelectedValue == "Time Tracking")
        {
            BindTimeTracking();
        }
        else if (ddlReport.SelectedValue == "Home delivery")
        {
            BindHomeDelivery();
        }
        else if (ddlReport.SelectedValue == "Store")
        {
            BindStore();
        }
        else if (ddlReport.SelectedValue == "NPU")
        {
            BindNPU();
        }
        else if (ddlReport.SelectedValue == "Vacation")
        {
            BindVacation();
        }
        else if (ddlReport.SelectedValue == "Last Week Report")
        {
            WeeklyReports();
        }
    }
    protected void btnSearchTT_Click(object sender, EventArgs e)
    {
        BindTimeTracking();
    }
    protected void gvHomeDelivery_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "Download")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            SqlConnection cn = Constant.Connection();
            SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.*, dbo.HomeDeliverySubscriber.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId WHERE dbo.Subscribers.SubId='" + index + "'", cn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            gvDELIVERYINFORMATION.DataSource = ds.Tables[0];
            gvDELIVERYINFORMATION.DataBind();

            gvSUBSCRIPTIONINFORMATION.DataSource = ds.Tables[0];
            gvSUBSCRIPTIONINFORMATION.DataBind();

            gvThirdTable.DataSource = ds.Tables[0];
            gvThirdTable.DataBind();

            if (WeekList.SelectedValue == " - Select a Week - ")
            {
                SqlDataAdapter da3 = new SqlDataAdapter("SELECT dbo.PurchaseProduct.*, dbo.PurchaseProductDetails.* FROM dbo.PurchaseProduct INNER JOIN dbo.PurchaseProductDetails ON dbo.PurchaseProduct.BuyID = dbo.PurchaseProductDetails.BuyId WHERE dbo.PurchaseProduct.OnlineHome='1' and dbo.PurchaseProductDetails.SubscriberID='" + index + "'", cn);
                DataSet ds3 = new DataSet();
                da3.Fill(ds3);
                gvADDITIONALITEMS.DataSource = ds3.Tables[0];
            }
            else
            {
                SqlDataAdapter da3 = new SqlDataAdapter("SELECT dbo.PurchaseProduct.*, dbo.PurchaseProductDetails.* FROM dbo.PurchaseProduct INNER JOIN dbo.PurchaseProductDetails ON dbo.PurchaseProduct.BuyID = dbo.PurchaseProductDetails.BuyId WHERE dbo.PurchaseProduct.OnlineHome='1' and dbo.PurchaseProductDetails.SubscriberID='" + index + "' and dbo.PurchaseProduct.Week='" + WeekList.SelectedValue + "'", cn);
                DataSet ds3 = new DataSet();
                da3.Fill(ds3);
                gvADDITIONALITEMS.DataSource = ds3.Tables[0];
            }



            gvADDITIONALITEMS.DataBind();

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Panel.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
            divHomeDeliveryRecord.RenderControl(htmlTextWriter);
            StringReader stringReader = new StringReader(stringWriter.ToString());
            Document Doc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(Doc);

            PdfWriter.GetInstance(Doc, Response.OutputStream);
            Doc.Open();
            Doc.Add(new Paragraph("Week:" + WeekList.SelectedValue));
            Doc.Add(new Paragraph("Store:" + ddlStore.SelectedValue));
            Doc.Add(new Paragraph(" "));
            htmlparser.Parse(stringReader);
            Doc.Close();
            Response.Write(Doc);
            Response.End();
        }

    }
    public override void VerifyRenderingInServerForm(Control control)
    {

    }
    protected void gvHomeDelivery_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //DataSet ds = ViewState["HomeDelivery"] as DataSet;
            ImageButton ImgBounty = e.Row.FindControl("ImgBounty") as ImageButton;
            ImageButton ImgBarnYard = e.Row.FindControl("ImgBarnYard") as ImageButton;
            ImageButton ImgPloughman = e.Row.FindControl("ImgPloughman") as ImageButton;

            HiddenField hfBounty = e.Row.FindControl("hfBounty") as HiddenField;
            HiddenField hfBarnYard = e.Row.FindControl("hfBarnYard") as HiddenField;
            HiddenField hfPloughman = e.Row.FindControl("hfPloughman") as HiddenField;

            if (hfBounty.Value != "False")
            {
                ImgBounty.Visible = false;
            }
            if (hfBarnYard.Value != "False")
            {
                ImgBarnYard.Visible = false;
            }
            if (hfPloughman.Value != "False")
            {
                ImgPloughman.Visible = false;
            }


        }
    }
    protected void gvNotes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[3].Text = Server.HtmlDecode(Convert.ToString(e.Row.Cells[3].Text));
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[4].Text = Server.HtmlDecode(Convert.ToString(e.Row.Cells[4].Text));
        }
    }
    protected void gvThirdTable_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = Server.HtmlDecode(Convert.ToString(e.Row.Cells[0].Text));
        }
    }
    protected void gvSUBSCRIPTIONINFORMATION_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Text = Server.HtmlDecode(Convert.ToString(e.Row.Cells[1].Text));
        }
    }
    protected void gvAllergies_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[3].Text = Server.HtmlDecode(Convert.ToString(e.Row.Cells[3].Text));
        }
    }
    protected void gvVacation_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ImgBounty = e.Row.FindControl("ImgBounty") as ImageButton;
            ImageButton ImgBarnYard = e.Row.FindControl("ImgBarnYard") as ImageButton;
            ImageButton ImgPloughman = e.Row.FindControl("ImgPloughman") as ImageButton;

            HiddenField hfBounty = e.Row.FindControl("hfBounty") as HiddenField;
            HiddenField hfBarnYard = e.Row.FindControl("hfBarnYard") as HiddenField;
            HiddenField hfPloughman = e.Row.FindControl("hfPloughman") as HiddenField;

            if (hfBounty.Value != "False")
            {
                ImgBounty.Visible = false;
            }
            if (hfBarnYard.Value != "False")
            {
                ImgBarnYard.Visible = false;
            }
            if (hfPloughman.Value != "False")
            {
                ImgPloughman.Visible = false;
            }
        }
    }
    protected void gvDELIVERYINFORMATION_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string str = "";

            if (e.Row.Cells[1].Text == "True")
            {
                str += "Bounty";
            }
            if (e.Row.Cells[2].Text == "True")
            {
                if (str == "")
                {
                    str += "BarnYard";
                }
                else
                    str += " ,BarnYard";
            }
            if (e.Row.Cells[3].Text == "True")
            {
                if (str == "")
                    str += "Ploughman";
                else
                    str += " ,Ploughman";
            }
            gvDELIVERYINFORMATION.Columns[1].Visible = false;
            gvDELIVERYINFORMATION.Columns[2].Visible = false;
            gvDELIVERYINFORMATION.Columns[3].Visible = false;
            if (str == "")
            {

            }
            Label Total = e.Row.FindControl("Label1") as Label;
            Total.Text = str;
        }

    }
}