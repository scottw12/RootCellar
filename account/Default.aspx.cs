using System.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Net.Mail;
using System.Data.SqlClient;
using Telerik.Web.UI;
using PerceptiveMCAPI;
using PerceptiveMCAPI.Types;
using PerceptiveMCAPI.Methods;
using System.Net;
using System.IO;
using System.Web.Security;
using System.Windows;
using System.Web.UI.WebControls;
using Telerik.Web.UI.Calendar.Utils;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Text;


public partial class account_Default : System.Web.UI.Page
{
    static Double TotalPrice = 0.00, ProductPrice = 0.00, BoxPrice = 0.00, Total = 0.00;
    static string Charges = string.Empty;
    private SqlConnection conn = null;
    string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    private SqlCommand cmd = null;
    int SubId;
    public static bool isEdit = false, isEditVacation = false;
    static string VacationID = string.Empty;
    static string UserID = string.Empty, CustomerEmail = string.Empty, Address = string.Empty;
    public DataTable dt = new DataTable();

    //****************** SalesVu *********************
    bool isDev;
    public static bool isEditHome = false;
    //***** Development *******
    //Dim APIKey As String = "cdab5266e335a4b4a11661198393ff9d"
    //Dim StoreID As String = "1668"
    //Dim url As String = "https://dev.salesvu.com/townvu/api/index.php?request="
    //***** Production *******
    string APIKey = "a662c77bd1c244eb3440a3aa9dedc5bb";
    string StoreID = "34798";
    string url = "https://www.salesvu.com/townvu/api/index.php?request=";
    //****************** SalesVu *********************
    public DataTable SelectedProducts;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Session["SPPayment"] = null;
        isDev = false;
        ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
        scriptManager.RegisterPostBackControl(this.Button2);
        //****************** SalesVu *********************
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["Page"] != null)
            {
                RadTabStrip1.SelectedIndex = 1;
                RadPageView5.Selected = true;
                ddlweekProduct.SelectedValue = Request.QueryString["Page"].ToString();
                DataTable SelectedProducts = (DataTable)Session["SelectedProducts"];
                int j = SelectedProducts.Rows.Count;
                for (int i = 0; i < SelectedProducts.Rows.Count; i++)
                {
                    if (SelectedProducts.Rows[i][4].ToString() == Request.QueryString["Page"].ToString())
                    {
                        SelectedProducts.Rows[i].Delete();
                        i--;

                    }

                }
            }
            if (Request.QueryString["Page1"] != null)
            {
                RadTabStrip1.SelectedIndex = 1;
                RadPageView5.Selected = true;


            }
            if (Request.QueryString["VID"] != null)
            {
                LoadDetails();
            }
            if (Request.QueryString["TotalPrice"] != null)
            {
                Price.Text = Math.Round(Convert.ToDecimal(Request.QueryString["TotalPrice"]), 2).ToString("0.00");
                ProductPrice = Convert.ToDouble(Request.QueryString["TotalPrice"].ToString());
                Total = Convert.ToDouble(Request.QueryString["TotalPrice"].ToString());
                RadTabStrip1.SelectedIndex = 2;
                RadPageView2.Selected = true;

            }
            else
            {
                Price.Text = "0.00";
            }
            SqlDataReader myDataReader = default(SqlDataReader);
            SqlConnection mySqlConnection = default(SqlConnection);
            SqlCommand mySqlCommand = default(SqlCommand);
            mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            mySqlCommand = new SqlCommand("SELECT UserId,Role FROM userinfo Where Username= '" + Membership.GetUser().ToString() + "'", mySqlConnection);
            try
            {
                mySqlConnection.Open();
                myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while ((myDataReader.Read()))
                {
                    string role = myDataReader[1].ToString();
                    string uid = myDataReader[0].ToString();
                    if (role == "Admin")
                    {
                        Session[Constant.UserRole] = role;
                        Session[Constant.UserID] = uid;
                        Response.Redirect("~/admin/");
                    }
                    else if (role == "Employee")
                    {
                        Session[Constant.UserRole] = role;
                        Session[Constant.UserID] = uid;
                        Response.Redirect("~/admin/");
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
            HomeDelivery();
            FillInfo();
            FillPaidInfo();
            FillDayInfo();
            FillStoreInfo();
            //FillWeekInfo();
            //FillWeekInfoVacation();
            //FillVacInfo();
            BindVacation();

            /***********New Added**********/

            BindProducts();
            if (Session["TotalProduct"] != null)
            {
                DataTable dt_totalProduct = Session["TotalProduct"] as DataTable;

                //ddlWeeks.DataSource = dt_totalProduct.DefaultView.ToTable(true, "Week");
                //ddlWeeks.DataValueField = "Week";
                //ddlWeeks.DataTextField = "Week";
                //ddlWeeks.DataBind();
                //ddlWeeks.Items.Insert(0, "Select Week");

                gvTotalProduct.DataSource = dt_totalProduct;
                gvTotalProduct.DataBind();

            }
        }
    }
    /// <summary>
    /// bind all products to repeter
    /// </summary>
    private void BindProducts()
    {
        SqlConnection cn = Constant.Connection();

        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM subscribers Where Username= '" + Membership.GetUser().ToString() + "'", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        UserID = ds.Tables[0].Rows[0]["SubID"].ToString();
        SqlDataAdapter da2 = new SqlDataAdapter("SELECT * FROM PurchaseProduct WHERE [SubscriberID]=" + UserID + " AND [Week]='" + ddlweekProduct.SelectedValue + "'", cn);
        DataSet dsCheck = new DataSet();
        da2.Fill(dsCheck);
        if (dsCheck.Tables[0].Rows.Count > 0 && ddlweekProduct.SelectedValue.ToString() != " - Select a Week - ")
        {
            if (Convert.ToBoolean(dsCheck.Tables[0].Rows[0]["OnlineHome"]) == true)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "al", "alert('This Weeks Order Has Been Submitted for Online Payment.')", true);
                return;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "al", "alert('This Weeks Order Has Been Submitted for Payment In Store.')", true);
                return;
            }

        }


        SqlCommand cmd = new SqlCommand("DisplayProduct", cn);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Store", ds.Tables[0].Rows[0]["Store"].ToString());
        cmd.Parameters.AddWithValue("@Week", ddlweekProduct.SelectedValue);
        SqlDataAdapter da5 = new SqlDataAdapter();
        da5.SelectCommand = cmd;

        DataSet ds5 = new DataSet();
        da5.Fill(ds5);
        if (ds5.Tables[0].Rows.Count > 0)
        {
            rcProducts.DataSource = ds5.Tables[0];
            rcProducts.DataBind();
        }
        else
        {
            if (ddlweekProduct.SelectedValue.ToString() != " - Select a Week - ")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "al", "alert('There is no product for this week.')", true);
            }

        }
        if (Session["SPPayment"] != null)
        {
            DataTable PaypentProduct = (DataTable)Session["SPPayment"];

            for (int i = 0; i < PaypentProduct.Rows.Count; i++)
            {
                foreach (RepeaterItem item in rcProducts.Controls)
                {
                    CheckBox cbAddToCart = item.FindControl("cbAddToCart") as CheckBox;
                    Label ProductID = item.FindControl("ProductID") as Label;
                    Label lblProductName = item.FindControl("lblProductName") as Label;
                    Label lblPrice = item.FindControl("lblPrice") as Label;
                    TextBox Quantity = item.FindControl("txtQuantity") as TextBox;
                    HiddenField hfQuantity = item.FindControl("hfQuantity") as HiddenField;

                    if (ProductID.Text == PaypentProduct.Rows[i]["ProductId"].ToString())
                    {
                        Quantity.Text = PaypentProduct.Rows[i]["Quantity"].ToString();
                        if (PaypentProduct.Rows[i]["ProductPrice"].ToString() != null)
                        {
                            cbAddToCart.Checked = true;
                        }

                    }
                }
            }

        }
    }

    private void HomeDelivery()
    {
        SqlConnection cn2 = Constant.Connection();
        SqlDataAdapter da1 = new SqlDataAdapter("SELECT * FROM Subscribers where Username= '" + Membership.GetUser().ToString() + "'", cn2);
        DataSet ds1 = new DataSet();
        da1.Fill(ds1);


        SqlDataAdapter da2 = new SqlDataAdapter("SELECT dbo.HomeDelivery.HomeDeliveryId, Convert(nvarchar(50),[Day])+'-'+Convert(nvarchar(50),[StartTime],100)+'-'+Convert(nvarchar(50),[EndTime],100) AS Date  FROM dbo.HomeDelivery INNER JOIN dbo.HomeDeliveryStore ON dbo.HomeDelivery.HomeDeliveryId = dbo.HomeDeliveryStore.HomeDeliveryId where dbo.HomeDeliveryStore.Available='True' and dbo.HomeDeliveryStore.StoreName='" + ds1.Tables[0].Rows[0]["Store"].ToString() + "'", cn2);
        DataSet ds2 = new DataSet();
        da2.Fill(ds2);
        ddlBestTime.DataSource = ds2.Tables[0];
        ddlBestTime.DataTextField = "Date";
        ddlBestTime.DataValueField = "Date";
        ddlBestTime.DataBind();

        SqlDataAdapter da3 = new SqlDataAdapter("Select * from HomeDeliverySubscriber where SubId='" + ds1.Tables[0].Rows[0]["SubId"].ToString() + "'", cn2);
        DataSet ds3 = new DataSet();
        da3.Fill(ds3);
        if (ds3.Tables[0].Rows.Count > 0)
        {
            txtDeliveryAddress.Text = ds3.Tables[0].Rows[0]["DeliveryAddress"].ToString();
            ddlLocation.SelectedValue = ds3.Tables[0].Rows[0]["Location"].ToString();
            txtSpecialinstr.Text = ds3.Tables[0].Rows[0]["SpecialInstruction"].ToString();
            ddlBestTime.SelectedValue = ds3.Tables[0].Rows[0]["BestTime"].ToString();
            litMsg.Text = "You Request is: " + ds3.Tables[0].Rows[0]["Request"].ToString();
            isEditHome = true;
        }
        else
            isEditHome = false;
    }


    /// <summary>
    /// Bind Vacation Details to gridview
    /// </summary>
    private void BindVacation()
    {

        SqlConnection cn = Constant.Connection();
        SqlDataAdapter da = new SqlDataAdapter("SELECT SubID FROM subscribers Where Username= '" + Membership.GetUser().ToString() + "'", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);

        SqlDataAdapter da2 = new SqlDataAdapter("SELECT * FROM VacationDetails Where CustomerID= '" + ds.Tables[0].Rows[0]["SubID"].ToString() + "'", cn);
        DataSet ds2 = new DataSet();
        da2.Fill(ds2);
        if (ds2.Tables[0].Rows.Count > 0)
        {
            gvVacation.DataSource = ds2.Tables[0];
            gvVacation.DataBind();
        }
        else
        {
            gvVacation.DataSource = null;
            gvVacation.DataBind();
            gvVacation.Visible = true;
        }

    }
    /// <summary>
    /// Load Details Of Users
    /// </summary>
    private void LoadDetails()
    {
        VacationID = EncryptDecrypt.DecryptPassword(Request.QueryString["VID"]);
        SqlConnection cn = Constant.Connection();
        SqlDataAdapter da = new SqlDataAdapter("Select * from VacationDetails where VID=" + VacationID + "", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {

            WeekList.SelectedValue = ds.Tables[0].Rows[0]["VacationWeek"].ToString();

        }
    }

    protected void FillInfo()
    {
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        mySqlCommand = new SqlCommand("SELECT SubID FROM subscribers Where Username= '" + Membership.GetUser().ToString() + "'", mySqlConnection);
        try
        {
            mySqlConnection.Open();
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            while ((myDataReader.Read()))
            {
                SubId = myDataReader.GetInt32(0);
            }
        }
        finally
        {
            if ((mySqlConnection.State == ConnectionState.Open))
            {
                mySqlConnection.Close();
            }
        }
        mySqlCommand = new SqlCommand("SELECT Firstname1, Firstname2, lastname1, lastname2, email1, email2, phone1, phone2, address, city, state, zip, allergies, BountyNL, BarnyardNL, PloughmanNL, vacused, bounty, barnyard, ploughman, pickupday, store FROM Subscribers Where Username= '" + Membership.GetUser().ToString() + "'", mySqlConnection);
        try
        {
            mySqlConnection.Open();
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            while ((myDataReader.Read()))
            {
                if (!myDataReader.IsDBNull(0))
                {
                    firstname1.Text = myDataReader.GetString(0);
                }
                if (!myDataReader.IsDBNull(1))
                {
                    firstname2.Text = myDataReader.GetString(1);
                }
                if (!myDataReader.IsDBNull(2))
                {
                    lastname1.Text = myDataReader.GetString(2);
                }
                if (!myDataReader.IsDBNull(3))
                {
                    lastname2.Text = myDataReader.GetString(3);
                }
                if (!myDataReader.IsDBNull(4))
                {
                    email1.Text = myDataReader.GetString(4);
                }
                if (!myDataReader.IsDBNull(5))
                {
                    email2.Text = myDataReader.GetString(5);
                }
                if (!myDataReader.IsDBNull(6))
                {
                    phone1.Text = myDataReader.GetString(6);
                }
                if (!myDataReader.IsDBNull(7))
                {
                    phone2.Text = myDataReader.GetString(7);
                }
                if (!myDataReader.IsDBNull(8))
                {
                    address.Text = myDataReader.GetString(8);
                }
                if (!myDataReader.IsDBNull(9))
                {
                    city.Text = myDataReader.GetString(9);
                }
                if (!myDataReader.IsDBNull(10))
                {
                    state.Text = myDataReader.GetString(10);
                }
                if (!myDataReader.IsDBNull(11))
                {
                    zip.Text = myDataReader.GetString(11);
                }
                if (!myDataReader.IsDBNull(12))
                {
                    allergies.Text = myDataReader.GetString(12);
                }
                if (!myDataReader.IsDBNull(13))
                {
                    BountyNL.Checked = myDataReader.GetBoolean(13);
                }
                if (!myDataReader.IsDBNull(14))
                {
                    BarnyardNL.Checked = myDataReader.GetBoolean(14);
                }
                if (!myDataReader.IsDBNull(15))
                {
                    PloughmanNL.Checked = myDataReader.GetBoolean(15);
                }
                if (!myDataReader.IsDBNull(16))
                {
                    //int weeks = myDataReader.GetInt32(16);
                    //weeksLiteral.Text = "You have used <b>" + weeks.ToString() + "</b> of your vacation weeks.";
                    //if (weeks == 2)
                    //{
                    //    weeksLiteral.Text = "You have already used your <b>2</b> vacation weeks.";
                    //    //calpanel.Visible = false;
                    //}
                }
                if (myDataReader.GetBoolean(17) == true)
                {
                    BoxButton.Items.FindByText("Bounty - $35.00").Selected = true;
                    //For Each Weekrow As GridViewRow In GridView1.Rows
                    //    If Weekrow.Cells(0).Text = "Deposit" Then
                    //        Dim BountyPaid As CheckBox = TryCast(Weekrow.FindControl("BountyPaidCheck"), CheckBox)
                    //        BountyPaid.Checked = True
                    //        BountyPaid.Enabled = False
                    //    End If
                    //Next
                }
                else
                {
                    CancelBounty.Visible = false;
                    GridView1.Columns[2].Visible = false;
                    //For Each Weekrow As GridViewRow In GridView1.Rows
                    //    If Weekrow.Cells(0).Text = "Deposit" Then
                    //        Dim BountyPaid As CheckBox = TryCast(Weekrow.FindControl("BountyPaidCheck"), CheckBox)
                    //        BountyPaid.Checked = False
                    //        BountyPaid.Enabled = False
                    //    End If
                    //Next
                }
                if (myDataReader.GetBoolean(18) == true)
                {
                    BoxButton.Items.FindByText("Barnyard - $35.00").Selected = true;
                    //For Each Weekrow As GridViewRow In GridView1.Rows
                    //    If Weekrow.Cells(0).Text = "Deposit" Then
                    //        Dim BarnyardPaid As CheckBox = TryCast(Weekrow.FindControl("BarnyardPaidCheck"), CheckBox)
                    //        BarnyardPaid.Checked = True
                    //        BarnyardPaid.Enabled = False
                    //    End If
                    //Next
                }
                else
                {
                    CancelBarnyard.Visible = false;
                    GridView1.Columns[3].Visible = false;
                    //For Each Weekrow As GridViewRow In GridView1.Rows
                    //    If Weekrow.Cells(0).Text = "Deposit" Then
                    //        Dim BarnyardPaid As CheckBox = TryCast(Weekrow.FindControl("BarnyardPaidCheck"), CheckBox)
                    //        BarnyardPaid.Checked = False
                    //        BarnyardPaid.Enabled = False
                    //    End If
                    //Next
                }
                if (myDataReader.GetBoolean(19) == true)
                {
                    BoxButton.Items.FindByText("Ploughman - $35.00").Selected = true;
                    //For Each Weekrow As GridViewRow In GridView1.Rows
                    //    If Weekrow.Cells(0).Text = "Deposit" Then
                    //        Dim PloughmanPaid As CheckBox = TryCast(Weekrow.FindControl("PloughmanPaidCheck"), CheckBox)
                    //        PloughmanPaid.Checked = True
                    //        PloughmanPaid.Enabled = False
                    //    End If
                    //Next
                }
                else
                {
                    CancelPloughman.Visible = false;
                    GridView1.Columns[4].Visible = false;
                    //For Each Weekrow As GridViewRow In GridView1.Rows
                    //    If Weekrow.Cells(0).Text = "Deposit" Then
                    //        Dim PloughmanPaid As CheckBox = TryCast(Weekrow.FindControl("PloughmanPaidCheck"), CheckBox)
                    //        PloughmanPaid.Checked = False
                    //        PloughmanPaid.Enabled = False
                    //    End If
                    //Next
                }
                PickupDayList.SelectedValue = myDataReader.GetString(20);
                StoreList.SelectedValue = myDataReader.GetString(21);
            }
        }
        catch (Exception ex)
        {
            MailMessage oMail1 = new MailMessage();
            oMail1.From = new MailAddress("Root Cellar <website@rootcellarboxes.com>");
            oMail1.To.Add(new MailAddress("scottw@jkmcomm.com"));
            oMail1.Subject = "Root Cellar Error";
            oMail1.Priority = MailPriority.High;
            oMail1.IsBodyHtml = true;
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            oMail1.Body += "<head><title></title></head>";
            oMail1.Body += "<body>";
            oMail1.Body += "Error in subscriber Default1: " + Membership.GetUser().ToString() + "<br /><br />";
            oMail1.Body += ex.Message + "<br /><br />" + ex.StackTrace;
            oMail1.Body += "</body>";
            oMail1.Body += "</html>";
            AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            oMail1.AlternateViews.Add(htmlView2);
            System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            ;
            
            smtpmail2.Send(oMail1);
            oMail1 = null;
            Literal0.Text = "We're sorry, there seems to have been an error.";
        }
        finally
        {
            if ((mySqlConnection.State == ConnectionState.Open))
            {
                mySqlConnection.Close();
            }
        }
    }
    protected void FillPaidInfo()
    {
        try
        {
            SqlDataReader myDataReader2 = default(SqlDataReader);
            SqlConnection mySqlConnection2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand mySqlCommand2 = default(SqlCommand);
            string SDateRange = "";
            string query = "select Sstart, send from seasons where currents='true'";
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
                            SDateRange = " and week >= '" + myDataReader2.GetDateTime(0) + "' and week <= '" + myDataReader2.GetDateTime(1) + "' ";
                        }
                    }
                    myDataReader2.Close();
                }
            }
            string SqlQuary = "SELECT SubId, Week, PaidBounty, PaidBarnyard, PaidPloughman, bounty, barnyard, ploughman FROM Weekly where subID='" + SubId.ToString() + "' and ((week='1/1/1900') or (week>='" + System.DateTime.Today.AddDays(-1) + "'" + SDateRange + ")) ORDER BY [Week]";
            DataTable dt = new DataTable();
            dt.Columns.Add("SubId");
            dt.Columns.Add("Week");
            dt.Columns.Add("PaidBounty");
            dt.Columns.Add("PaidBarnyard");
            dt.Columns.Add("PaidPloughman");
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
                    string paid = "";
                    string pickedup = "";
                    string vacation = "";
                    int i = 0;
                    while (myDataReader.Read())
                    {
                        string week = "";
                        if (myDataReader.GetDateTime(1).ToString().Contains("1900"))
                        {
                            week = "Deposit";
                        }
                        else
                        {
                            week = (myDataReader.GetDateTime(1).Month.ToString() + "/" + myDataReader.GetDateTime(1).Day.ToString() + "-" + myDataReader.GetDateTime(1).AddDays(1).Day.ToString() + "/" + myDataReader.GetDateTime(1).Year.ToString());
                        }
                        if (!myDataReader.IsDBNull(2) & !myDataReader.IsDBNull(3) & !myDataReader.IsDBNull(4))
                        {
                            dt.Rows.Add(myDataReader.GetInt32(0), week, myDataReader.GetBoolean(2), myDataReader.GetBoolean(3), myDataReader.GetBoolean(4));

                        }
                        else
                        {
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                myDataReader.Close();
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();
            gvProducts.DataSource = dt;
            gvProducts.DataBind();

            //delivery Grid
            gvDelivery.DataSource = dt;
            gvDelivery.DataBind();


            dt.Rows.RemoveAt(0);
            //ddlWeeks.DataSource = dt.DefaultView.ToTable(true, "Week");
            //ddlWeeks.DataValueField = "Week";
            //ddlWeeks.DataTextField = "Week";
            //ddlWeeks.DataBind();
            //ddlWeeks.Items.Insert(0, "Select Week");

            ddlWeek.DataSource = dt;
            ddlWeek.DataTextField = "Week";
            ddlWeek.DataValueField = "Week";
            ddlWeek.DataBind();
            ddlWeek.Items.Insert(0, " - Select a Week - ");

            ddlweekProduct.DataSource = dt;
            ddlweekProduct.DataTextField = "Week";
            ddlweekProduct.DataValueField = "Week";
            ddlweekProduct.DataBind();
            ddlweekProduct.Items.Insert(0, " - Select a Week - ");

            WeekList.DataSource = dt;
            WeekList.DataTextField = "Week";
            WeekList.DataValueField = "Week";
            WeekList.DataBind();
            WeekList.Items.RemoveAt(0);
            WeekList.Items.RemoveAt(0);
            WeekList.Items.Insert(0, " - Select a Week - ");
        }
        catch (Exception ex)
        {
            //MailMessage oMail1 = new MailMessage();
            //oMail1.From = new MailAddress("Root Cellar <website@rootcellarboxes.com>");
            //oMail1.To.Add(new MailAddress("scottw@jkmcomm.com"));
            //oMail1.Subject = "Root Cellar Error";
            //oMail1.Priority = MailPriority.High;
            //oMail1.IsBodyHtml = true;
            //oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            //oMail1.Body += "<head><title></title></head>";
            //oMail1.Body += "<body>";
            //oMail1.Body += "Error in subscriber Default2: " + Membership.GetUser().ToString() + "<br /><br />";
            //oMail1.Body += ex.Message + "<br /><br />" + ex.StackTrace;
            //oMail1.Body += "</body>";
            //oMail1.Body += "</html>";
            //AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            //oMail1.AlternateViews.Add(htmlView2);
            //System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            //;
            //
            //smtpmail2.Send(oMail1);
            //oMail1 = null;
            //Literal0.Text = "We're sorry, there seems to have been an error.";
        }

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
    protected void FillStoreInfo()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Store");
        dt.Rows.Add("");
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
    //protected void FillWeekInfo()
    //{

    //    SqlDataReader myDataReader2 = default(SqlDataReader);
    //    SqlConnection mySqlConnection2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
    //    SqlCommand mySqlCommand2 = default(SqlCommand);
    //    string SDateRange = "";
    //    string query = "select Sstart, send from seasons where currents='true'";
    //    try
    //    {
    //        using (SqlConnection conn = new SqlConnection(ConnectionString))
    //        {
    //            using (mySqlConnection2)
    //            {
    //                mySqlCommand2 = new SqlCommand(query, mySqlConnection2);
    //                mySqlConnection2.Open();
    //                myDataReader2 = mySqlCommand2.ExecuteReader();
    //                if (myDataReader2.HasRows)
    //                {
    //                    while (myDataReader2.Read())
    //                    {
    //                        SDateRange = "where week>'" + myDataReader2.GetDateTime(0) + "' and week <= '" + myDataReader2.GetDateTime(1) + "' ";
    //                    }
    //                }
    //                myDataReader2.Close();
    //            }
    //        }
    //    }
    //    finally
    //    {
    //    }
    //    DataTable dt = new DataTable();
    //    dt.Columns.Add("Week");
    //    dt.Rows.Add(" - Select a Week - ");
    //    //Create Rows in DataTable
    //    SqlDataReader myDataReader = default(SqlDataReader);
    //    SqlConnection mySqlConnection = default(SqlConnection);
    //    SqlCommand mySqlCommand = default(SqlCommand);
    //    mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
    //    try
    //    {
    //        using (mySqlConnection)
    //        {
    //            mySqlCommand = new SqlCommand("SELECT DISTINCT Week FROM weekly " + SDateRange + " order by week", mySqlConnection);
    //            mySqlConnection.Open();

    //            myDataReader = mySqlCommand.ExecuteReader();

    //            if (myDataReader.HasRows)
    //            {
    //                while (myDataReader.Read())
    //                {
    //                    if (!(myDataReader.GetDateTime(0).Year.ToString() == "1900"))
    //                    {
    //                        dt.Rows.Add(myDataReader.GetDateTime(0).Month.ToString() + "/" + myDataReader.GetDateTime(0).Day.ToString() + "-" + myDataReader.GetDateTime(0).AddDays(1).Day.ToString() + "/" + myDataReader.GetDateTime(0).Year.ToString());
    //                    }
    //                }
    //                dt.Rows.RemoveAt(1);
    //            }
    //            else
    //            {
    //                Console.WriteLine("No rows found.");
    //            }
    //            myDataReader.Close();
    //        }
    //    }
    //    finally
    //    {
    //    }
    //    this.WeekList.DataSource = dt;
    //    this.WeekList.DataTextField = "Week";
    //    this.WeekList.DataValueField = "Week";
    //    this.WeekList.DataBind();



    //}
    //protected void FillWeekInfoVacation()
    //{

    //    SqlDataReader myDataReader2 = default(SqlDataReader);
    //    SqlConnection mySqlConnection2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
    //    SqlCommand mySqlCommand2 = default(SqlCommand);
    //    string SDateRange = "";
    //    string query = "select Sstart, send from seasons where currents='true'";
    //    try
    //    {
    //        using (SqlConnection conn = new SqlConnection(ConnectionString))
    //        {
    //            using (mySqlConnection2)
    //            {
    //                mySqlCommand2 = new SqlCommand(query, mySqlConnection2);
    //                mySqlConnection2.Open();
    //                myDataReader2 = mySqlCommand2.ExecuteReader();
    //                if (myDataReader2.HasRows)
    //                {
    //                    while (myDataReader2.Read())
    //                    {
    //                        SDateRange = "where week>'" + myDataReader2.GetDateTime(0) + "' and week <= '" + myDataReader2.GetDateTime(1) + "' ";
    //                    }
    //                }
    //                myDataReader2.Close();
    //            }
    //        }
    //    }
    //    finally
    //    {
    //    }
    //    DataTable dt = new DataTable();
    //    dt.Columns.Add("Week");
    //    dt.Rows.Add(" - Select a Week - ");
    //    //Create Rows in DataTable
    //    SqlDataReader myDataReader = default(SqlDataReader);
    //    SqlConnection mySqlConnection = default(SqlConnection);
    //    SqlCommand mySqlCommand = default(SqlCommand);
    //    mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
    //    try
    //    {
    //        using (mySqlConnection)
    //        {
    //            mySqlCommand = new SqlCommand("SELECT DISTINCT Week FROM weekly " + SDateRange + " order by week", mySqlConnection);
    //            mySqlConnection.Open();

    //            myDataReader = mySqlCommand.ExecuteReader();

    //            if (myDataReader.HasRows)
    //            {
    //                while (myDataReader.Read())
    //                {
    //                    if (!(myDataReader.GetDateTime(0).Year.ToString() == "1900"))
    //                    {
    //                        dt.Rows.Add(myDataReader.GetDateTime(0).Month.ToString() + "/" + myDataReader.GetDateTime(0).Day.ToString() + "-" + myDataReader.GetDateTime(0).AddDays(1).Day.ToString() + "/" + myDataReader.GetDateTime(0).Year.ToString());
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                Console.WriteLine("No rows found.");
    //            }
    //            myDataReader.Close();
    //        }
    //    }
    //    finally
    //    {
    //    }

    //    this.ddlWeek.DataSource = dt;
    //    this.ddlWeek.DataTextField = "Week";
    //    this.ddlWeek.DataValueField = "Week";
    //    this.ddlWeek.DataBind();

    //    this.ddlweekProduct.DataSource = dt;
    //    this.ddlweekProduct.DataTextField = "Week";
    //    this.ddlweekProduct.DataValueField = "Week";
    //    this.ddlweekProduct.DataBind();

    //}

    protected void OnCheckedChanged(Object sender, EventArgs e)
    {
        PriceUPanel.Update();
        int days = 0;

        if (Price.Text == "0.00")
        {
            ProductPrice = 0;
        }

        foreach (GridViewRow Weekrow in GridView1.Rows)
        {
            CheckBox BountyPaid = Weekrow.FindControl("BountyPaidCheck") as CheckBox;
            if (BountyPaid.Enabled == true && BountyPaid.Checked == true)
            {
                days += 1;

            }
            CheckBox BarnyardPaid = Weekrow.FindControl("BarnyardPaidCheck") as CheckBox;
            if (BarnyardPaid.Enabled == true && BarnyardPaid.Checked == true)
            {
                days += 1;

            }
            CheckBox PloughmanPaid = Weekrow.FindControl("PloughmanPaidCheck") as CheckBox;
            if (PloughmanPaid.Enabled == true && PloughmanPaid.Checked == true)
            {
                days += 1;

            }
        }
        if (days * 35 == 0 && Price.Text == "35.00")
        {
            Price.Text = "0.00";
        }
        else
        {
            //Price.Text = "" + (days * 35).ToString("C2");
            if (Price.Text != string.Empty)
            {
                BoxPrice = Convert.ToDouble(days * 35);

                Price.Text = (ProductPrice + BoxPrice).ToString("0.00");
            }
            else
                Price.Text = "" + (days * 35).ToString("0.00");
        }

    }
    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridViewRow row = e.Row;
            try
            {
                Button Delete = e.Row.FindControl("deleteButton") as Button;
                string week = row.Cells[1].Text;
                string pattern = "-(.*?)/";
                string replacement = "/" + "\r\n";//vbcrlf
                Regex rgx = new Regex(pattern, RegexOptions.Singleline);
                week = rgx.Replace(week, replacement);
                week = (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "");
                if (!((DateTime.Parse(week) > System.DateTime.Today.AddDays(7)) == true))
                {
                    Delete.Visible = false;
                }
            }
            catch (Exception ex)
            {
                //MailMessage oMail1 = new MailMessage();
                //oMail1.From = new MailAddress("Root Cellar <website@rootcellarboxes.com>");
                //oMail1.To.Add(new MailAddress("scottw@jkmcomm.com"));
                //oMail1.Subject = "Root Cellar Error";
                //oMail1.Priority = MailPriority.High;
                //oMail1.IsBodyHtml = true;
                //oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
                //oMail1.Body += "<head><title></title></head>";
                //oMail1.Body += "<body>";
                //oMail1.Body += "Error in subscriber Default3: " + Membership.GetUser().ToString() + "<br /><br />";
                //oMail1.Body += ex.Message + "<br /><br />" + ex.StackTrace;
                //oMail1.Body += "</body>";
                //oMail1.Body += "</html>";
                //AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
                //oMail1.AlternateViews.Add(htmlView2);
                //System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
                //;
                //
                //smtpmail2.Send(oMail1);
                //oMail1 = null;

            }
        }
    }
    protected void StoreList_SelectedIndexChanged(object sender, EventArgs e)
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
        else if (StoreList.SelectedValue == "DHSS (Employee Only)")
        {
            if (PickupDayList.SelectedValue == "Friday")
            {
                PickupDayList.SelectedValue = "Thursday";
                PUDLiteral.Text = "Pickups are only available on Thursday's at the DHSS Location";
            }
            else
            {
                PUDLiteral.Text = "";
            }
        }
        else if (StoreList.SelectedValue == "Mizzou North (Employee Only)")
        {
            if (PickupDayList.SelectedValue == "Thursday")
            {
                PickupDayList.SelectedValue = "Friday";
                PUDLiteral.Text = "Pickups are only available on Friday's at the Mizzou North Location";
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
    }
    protected void PickupDayList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (PickupDayList.SelectedValue == "Friday")
        {
            if (StoreList.SelectedValue == "Jefferson City")
            {
                
            }
            else if (StoreList.SelectedValue == "DHSS (Employee Only)")
            {
                PickupDayList.SelectedValue = "Thursday";
                PUDLiteral.Text = "Pickups are only available on Thursday's at the DHSS Location";
            }
            else
            {
                PUDLiteral.Text = "";
            }
        }
        else if (PickupDayList.SelectedValue == "Thursday")
        {
            if (StoreList.SelectedValue == "Mizzou North (Employee Only)")
            {
                PickupDayList.SelectedValue = "Friday";
                PUDLiteral.Text = "Pickups are only available on Friday's at the Mizzou North Location";
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
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "Update subscribers set FirstName1=@FirstName1, LastName1=@LastName1, Email1=@Email1, phone1=@phone1, FirstName2=@FirstName2, LastName2=@LastName2, Email2=@Email2, phone2=@phone2, Address=@Address, City=@City, State=@State, Zip=@Zip, BountyNL=@BountyNL, Barnyard=@BarnyardNL, PloughmanNL=@PloughmanNL Where username=@username";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    var _with1 = comm;
                    _with1.Connection = conn;
                    _with1.CommandType = CommandType.Text;
                    _with1.CommandText = query;
                    comm.Parameters.Add("@FirstName1", SqlDbType.VarChar).Value = firstname1.Text;
                    _with1.Parameters.Add("@LastName1", SqlDbType.VarChar).Value = lastname1.Text;
                    _with1.Parameters.Add("@Email1", SqlDbType.VarChar).Value = email1.Text;
                    _with1.Parameters.Add("@phone1", SqlDbType.VarChar).Value = phone1.Text;
                    _with1.Parameters.Add("@FirstName2", SqlDbType.VarChar).Value = firstname2.Text;
                    _with1.Parameters.Add("@LastName2", SqlDbType.VarChar).Value = lastname2.Text;
                    _with1.Parameters.Add("@Email2", SqlDbType.VarChar).Value = email2.Text;
                    _with1.Parameters.Add("@phone2", SqlDbType.VarChar).Value = phone2.Text;
                    _with1.Parameters.Add("@Address", SqlDbType.VarChar).Value = address.Text;
                    _with1.Parameters.Add("@City", SqlDbType.VarChar).Value = city.Text;
                    _with1.Parameters.Add("@State", SqlDbType.VarChar).Value = state.Text;
                    _with1.Parameters.Add("@Zip", SqlDbType.VarChar).Value = zip.Text;
                    _with1.Parameters.Add("@BountyNL", SqlDbType.Bit).Value = BountyNL.Checked;
                    _with1.Parameters.Add("@BarnyardNL", SqlDbType.Bit).Value = BarnyardNL.Checked;
                    _with1.Parameters.Add("@PloughmanNL", SqlDbType.Bit).Value = PloughmanNL.Checked;
                    _with1.Parameters.Add("@username", SqlDbType.VarChar).Value = Membership.GetUser().ToString();
                    conn.Open();
                    comm.ExecuteNonQuery();
                    Literal0.Text = "Account Updated";
                    UpdMailChimp(email1.Text, BountyNL.Checked, BarnyardNL.Checked, PloughmanNL.Checked);
                    if (!string.IsNullOrEmpty(email2.Text))
                    {
                        UpdMailChimp(email2.Text, BountyNL.Checked, BarnyardNL.Checked, PloughmanNL.Checked);
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            //MailMessage oMail1 = new MailMessage();
            //oMail1.From = new MailAddress("Root Cellar <website@rootcellarboxes.com>");
            //oMail1.To.Add(new MailAddress("scottw@jkmcomm.com"));
            //oMail1.Subject = "Root Cellar Error";
            //oMail1.Priority = MailPriority.High;
            //oMail1.IsBodyHtml = true;
            //oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            //oMail1.Body += "<head><title></title></head>";
            //oMail1.Body += "<body>";
            //oMail1.Body += "Error in subscriber Default4: " + Membership.GetUser().ToString() + "<br /><br />";
            //oMail1.Body += ex.Message + "<br /><br />" + ex.StackTrace;
            //oMail1.Body += "</body>";
            //oMail1.Body += "</html>";
            //AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            //oMail1.AlternateViews.Add(htmlView2);
            //System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            //;
            //
            //smtpmail2.Send(oMail1);
            //oMail1 = null;
            //Literal1.Text = "We're sorry, there seems to have been an error.";
        }
        /***************HomeDeliveryUpdate**********/

    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "Update subscribers set Allergies=@Allergies, store=@store, pickupday=@pickupday Where username=@username";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    var _with2 = comm;
                    _with2.Connection = conn;
                    _with2.CommandType = CommandType.Text;
                    _with2.CommandText = query;
                    comm.Parameters.Add("@Allergies", SqlDbType.VarChar).Value = allergies.Text;
                    _with2.Parameters.Add("@pickupday", SqlDbType.Text).Value = PickupDayList.SelectedValue;
                    _with2.Parameters.Add("@store", SqlDbType.Text).Value = StoreList.SelectedValue;
                    _with2.Parameters.Add("@username", SqlDbType.VarChar).Value = Membership.GetUser().ToString();

                    conn.Open();
                    comm.ExecuteNonQuery();
                    Literal0.Text = "Account Updated";
                }
            }
        }
        catch (SqlException ex)
        {
            MailMessage oMail1 = new MailMessage();
            oMail1.From = new MailAddress("Root Cellar <website@rootcellarboxes.com>");
            oMail1.To.Add(new MailAddress("scottw@jkmcomm.com"));
            oMail1.Subject = "Root Cellar Error";
            oMail1.Priority = MailPriority.High;
            oMail1.IsBodyHtml = true;
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            oMail1.Body += "<head><title></title></head>";
            oMail1.Body += "<body>";
            oMail1.Body += "Error in subscriber Default5: " + Membership.GetUser().ToString() + "<br /><br />";
            oMail1.Body += ex.Message + "<br /><br />" + ex.StackTrace;
            oMail1.Body += "</body>";
            oMail1.Body += "</html>";
            AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            oMail1.AlternateViews.Add(htmlView2);
            System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            ;
            
            smtpmail2.Send(oMail1);
            oMail1 = null;
            Literal1.Text = "We're sorry, there seems to have been an error.";
        }
    }

    protected void Submit_Click(object sender, EventArgs e)
    {
        try
        {
            SqlDataReader myDataReader = default(SqlDataReader);
            SqlConnection mySqlConnection = default(SqlConnection);
            SqlCommand mySqlCommand = default(SqlCommand);
            mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            mySqlCommand = new SqlCommand("SELECT SubID FROM subscribers Where Username= '" + Membership.GetUser().ToString() + "'", mySqlConnection);
            try
            {
                mySqlConnection.Open();
                myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while ((myDataReader.Read()))
                {
                    SubId = myDataReader.GetInt32(0);
                }
            }
            finally
            {
                if ((mySqlConnection.State == ConnectionState.Open))
                {
                    mySqlConnection.Close();
                }
            }

            string sql = "";
            string sql2 = "";
            mySqlCommand = new SqlCommand("SELECT vacused FROM subscribers Where SubID= '" + SubId.ToString() + "'", mySqlConnection);
            if (!(WeekList.SelectedValue == " - Select a Week - "))
            {
                string week = null;
                week = WeekList.SelectedValue;
                string pattern = "-(.*?)/";
                string replacement = "/" + "\r\n";
                Regex rgx = new Regex(pattern, RegexOptions.Singleline);
                week = rgx.Replace(week, replacement);
                week = (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "");
                try
                {
                    mySqlConnection.Open();
                    myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    while ((myDataReader.Read()))
                    {
                        sql = "update subscribers set vacused='" + (myDataReader.GetInt32(0) + 1).ToString() + "' where subid='" + SubId.ToString() + "'";
                        sql2 = "update weekly set vacation='true' where subid='" + SubId.ToString() + "' and week='" + week + "'";
                    }
                }
                finally
                {
                    if ((mySqlConnection.State == ConnectionState.Open))
                    {
                        mySqlConnection.Close();
                    }
                }
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand())
                    {
                        var _with3 = comm;
                        _with3.Connection = conn;
                        _with3.CommandType = CommandType.Text;
                        _with3.CommandText = sql;
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                }
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand())
                    {
                        var _with4 = comm;
                        _with4.Connection = conn;
                        _with4.CommandType = CommandType.Text;
                        _with4.CommandText = sql2;
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                }
                Literal0.Text = "You have successfully scheduled " + WeekList.SelectedValue + " as a vacation week";
                WeekList.ClearSelection();
                FillInfo();
                //FillVacInfo();
            }
            else
            {
                Literal2.Text = "Please select a week";
            }
        }
        catch (Exception ex)
        {
            //MailMessage oMail1 = new MailMessage();
            //oMail1.From = new MailAddress("Root Cellar <website@rootcellarboxes.com>");
            //oMail1.To.Add(new MailAddress("scottw@jkmcomm.com"));
            //oMail1.Subject = "Root Cellar Error";
            //oMail1.Priority = MailPriority.High;
            //oMail1.IsBodyHtml = true;
            //oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            //oMail1.Body += "<head><title></title></head>";
            //oMail1.Body += "<body>";
            //oMail1.Body += "Error in subscriber Default6: " + Membership.GetUser().ToString() + "<br /><br />";
            //oMail1.Body += ex.Message + "<br /><br />" + ex.StackTrace;
            //oMail1.Body += "</body>";
            //oMail1.Body += "</html>";
            //AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            //oMail1.AlternateViews.Add(htmlView2);
            //System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            //;
            //
            //smtpmail2.Send(oMail1);
            //oMail1 = null;
            //Literal2.Text = "We're Sorry, there was an error";
        }


    }

    protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //int weeksUsed = 0;
        //string week = GridView2.Rows[e.RowIndex].Cells[1].Text;
        //string pattern = "-(.*?)/";
        //string replacement = "/" + "\r\n";
        //Regex rgx = new Regex(pattern, RegexOptions.Singleline);
        //week = rgx.Replace(week, replacement);
        //week = (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "");
        //SqlDataReader myDataReader = default(SqlDataReader);
        //SqlConnection mySqlConnection = default(SqlConnection);
        //SqlCommand mySqlCommand = default(SqlCommand);
        //mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //mySqlCommand = new SqlCommand("SELECT SubID, vacused FROM subscribers Where Username= '" + Membership.GetUser().ToString() + "'", mySqlConnection);
        //try
        //{
        //    mySqlConnection.Open();
        //    myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
        //    while ((myDataReader.Read()))
        //    {
        //        SubId = myDataReader.GetInt32(0);
        //        weeksUsed = myDataReader.GetInt32(1);
        //    }
        //}
        //finally
        //{
        //    if ((mySqlConnection.State == ConnectionState.Open))
        //    {
        //        mySqlConnection.Close();
        //    }
        //}
        //try
        //{
        //    string query = "Update weekly set vacation='false' Where SubID=@SubID and week=@week";
        //    using (SqlConnection conn = new SqlConnection(ConnectionString))
        //    {
        //        using (SqlCommand comm = new SqlCommand())
        //        {
        //            var _with5 = comm;
        //            _with5.Connection = conn;
        //            _with5.CommandType = CommandType.Text;
        //            _with5.CommandText = query;
        //            comm.Parameters.Add("@SubID", SqlDbType.Int).Value = SubId;
        //            _with5.Parameters.Add("@week", SqlDbType.DateTime).Value = week;
        //            conn.Open();
        //            comm.ExecuteNonQuery();

        //        }
        //    }
        //}
        //catch (SqlException ex)
        //{
        //    MailMessage oMail1 = new MailMessage();
        //    oMail1.From = new MailAddress("Root Cellar <website@rootcellarboxes.com>");
        //    oMail1.To.Add(new MailAddress("scottw@jkmcomm.com"));
        //    oMail1.Subject = "Root Cellar Error";
        //    oMail1.Priority = MailPriority.High;
        //    oMail1.IsBodyHtml = true;
        //    oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
        //    oMail1.Body += "<head><title></title></head>";
        //    oMail1.Body += "<body>";
        //    oMail1.Body += "Error in subscriber Default7: " + Membership.GetUser().ToString() + "<br /><br />";
        //    oMail1.Body += ex.Message + "<br /><br />" + ex.StackTrace;
        //    oMail1.Body += "</body>";
        //    oMail1.Body += "</html>";
        //    AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
        //    oMail1.AlternateViews.Add(htmlView2);
        //    System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
        //    ;
        //    
        //    smtpmail2.Send(oMail1);
        //    oMail1 = null;
        //    Literal0.Text = "We're sorry, there seems to have been an error.";
        //}
        //if (!(weeksUsed < 1))
        //{
        //    try
        //    {
        //        string query = "Update subscribers set vacused=@vacused Where username=@username";
        //        using (SqlConnection conn = new SqlConnection(ConnectionString))
        //        {
        //            using (SqlCommand comm = new SqlCommand())
        //            {
        //                var _with6 = comm;
        //                _with6.Connection = conn;
        //                _with6.CommandType = CommandType.Text;
        //                _with6.CommandText = query;
        //                comm.Parameters.Add("@vacused", SqlDbType.VarChar).Value = (weeksUsed - 1).ToString();
        //                _with6.Parameters.Add("@username", SqlDbType.VarChar).Value = Membership.GetUser().ToString();
        //                conn.Open();
        //                comm.ExecuteNonQuery();
        //                Literal0.Text = GridView2.Rows[e.RowIndex].Cells[1].Text + " has been removed as a vacation week.";
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        MailMessage oMail1 = new MailMessage();
        //        oMail1.From = new MailAddress("Root Cellar <website@rootcellarboxes.com>");
        //        oMail1.To.Add(new MailAddress("scottw@jkmcomm.com"));
        //        oMail1.Subject = "Root Cellar Error";
        //        oMail1.Priority = MailPriority.High;
        //        oMail1.IsBodyHtml = true;
        //        oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
        //        oMail1.Body += "<head><title></title></head>";
        //        oMail1.Body += "<body>";
        //        oMail1.Body += "Error in subscriber Default8: " + Membership.GetUser().ToString() + "<br /><br />";
        //        oMail1.Body += ex.Message + "<br /><br />" + ex.StackTrace;
        //        oMail1.Body += "</body>";
        //        oMail1.Body += "</html>";
        //        AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
        //        oMail1.AlternateViews.Add(htmlView2);
        //        System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
        //        ;
        //        
        //        smtpmail2.Send(oMail1);
        //        oMail1 = null;
        //        Literal0.Text = "We're sorry, there seems to have been an error.";
        //    }
        //}
        //calpanel.Visible = true;
        //FillInfo();
        //FillVacInfo();
    }

    protected void CancelBounty_Click(object sender, EventArgs e)
    {
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        bool active = true;
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        mySqlCommand = new SqlCommand("SELECT SubID, Barnyard, ploughman FROM subscribers Where Username= '" + Membership.GetUser().ToString() + "'", mySqlConnection);
        try
        {
            mySqlConnection.Open();
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            while ((myDataReader.Read()))
            {
                SubId = myDataReader.GetInt32(0);
                if (myDataReader.GetBoolean(1) == false & myDataReader.GetBoolean(2) == false)
                {
                    active = false;
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
        try
        {
            string query = "Update weekly set bounty='false' Where SubID=@SubID ";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    var _with7 = comm;
                    _with7.Connection = conn;
                    _with7.CommandType = CommandType.Text;
                    _with7.CommandText = query;
                    comm.Parameters.Add("@SubID", SqlDbType.Int).Value = SubId;
                    conn.Open();
                    comm.ExecuteNonQuery();

                }
            }
        }
        catch (SqlException ex)
        {
            MailMessage oMail1 = new MailMessage();
            oMail1.From = new MailAddress("Root Cellar <website@rootcellarboxes.com>");
            oMail1.To.Add(new MailAddress("scottw@jkmcomm.com"));
            oMail1.Subject = "Root Cellar Error";
            oMail1.Priority = MailPriority.High;
            oMail1.IsBodyHtml = true;
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            oMail1.Body += "<head><title></title></head>";
            oMail1.Body += "<body>";
            oMail1.Body += "Error in subscriber Default9: " + Membership.GetUser().ToString() + "<br /><br />";
            oMail1.Body += ex.Message + "<br /><br />" + ex.StackTrace;
            oMail1.Body += "</body>";
            oMail1.Body += "</html>";
            AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            oMail1.AlternateViews.Add(htmlView2);
            System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            ;
            
            smtpmail2.Send(oMail1);
            oMail1 = null;
            Literal0.Text = "We're sorry, there seems to have been an error.";
        }
        try
        {
            string query = "Update subscribers set bounty='false', active=@active Where username=@username";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    var _with8 = comm;
                    _with8.Connection = conn;
                    _with8.CommandType = CommandType.Text;
                    _with8.CommandText = query;
                    comm.Parameters.Add("@username", SqlDbType.VarChar).Value = Membership.GetUser().ToString();
                    _with8.Parameters.Add("@active", SqlDbType.VarChar).Value = active;
                    conn.Open();
                    comm.ExecuteNonQuery();
                    Literal0.Text = "Your subscription has been cancelled.";
                }
            }
        }
        catch (SqlException ex)
        {
            MailMessage oMail1 = new MailMessage();
            oMail1.From = new MailAddress("Root Cellar <website@rootcellarboxes.com>");
            oMail1.To.Add(new MailAddress("scottw@jkmcomm.com"));
            oMail1.Subject = "Root Cellar Error";
            oMail1.Priority = MailPriority.High;
            oMail1.IsBodyHtml = true;
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            oMail1.Body += "<head><title></title></head>";
            oMail1.Body += "<body>";
            oMail1.Body += "Error in subscriber Default10: " + Membership.GetUser().ToString() + "<br /><br />";
            oMail1.Body += ex.Message + "<br /><br />" + ex.StackTrace;
            oMail1.Body += "</body>";
            oMail1.Body += "</html>";
            AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            oMail1.AlternateViews.Add(htmlView2);
            System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            ;
            
            smtpmail2.Send(oMail1);
            oMail1 = null;
            Literal0.Text = "We're sorry, there seems to have been an error.";
        }
        FillInfo();
        FillPaidInfo();
        FillDayInfo();
        FillStoreInfo();
        //FillWeekInfo();
        //FillWeekInfoVacation();
        //FillVacInfo();
        Price.Text = "$0.00";
    }
    protected void CancelBarnyard_Click(object sender, EventArgs e)
    {
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        bool active = true;
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        mySqlCommand = new SqlCommand("SELECT SubID, Bounty, ploughman FROM subscribers Where Username= '" + Membership.GetUser().ToString() + "'", mySqlConnection);
        try
        {
            mySqlConnection.Open();
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            while ((myDataReader.Read()))
            {
                SubId = myDataReader.GetInt32(0);
                if (myDataReader.GetBoolean(1) == false & myDataReader.GetBoolean(2) == false)
                {
                    active = false;
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
        try
        {
            string query = "Update weekly set barnyard='false' Where SubID=@SubID ";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    var _with9 = comm;
                    _with9.Connection = conn;
                    _with9.CommandType = CommandType.Text;
                    _with9.CommandText = query;
                    comm.Parameters.Add("@SubID", SqlDbType.Int).Value = SubId;
                    conn.Open();
                    comm.ExecuteNonQuery();

                }
            }
        }
        catch (SqlException ex)
        {
            MailMessage oMail1 = new MailMessage();
            oMail1.From = new MailAddress("Root Cellar <website@rootcellarboxes.com>");
            oMail1.To.Add(new MailAddress("scottw@jkmcomm.com"));
            oMail1.Subject = "Root Cellar Error";
            oMail1.Priority = MailPriority.High;
            oMail1.IsBodyHtml = true;
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            oMail1.Body += "<head><title></title></head>";
            oMail1.Body += "<body>";
            oMail1.Body += "Error in subscriber Default11: " + Membership.GetUser().ToString() + "<br /><br />";
            oMail1.Body += ex.Message + "<br /><br />" + ex.StackTrace;
            oMail1.Body += "</body>";
            oMail1.Body += "</html>";
            AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            oMail1.AlternateViews.Add(htmlView2);
            System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            ;
            
            smtpmail2.Send(oMail1);
            oMail1 = null;
            Literal0.Text = "We're sorry, there seems to have been an error.";
        }
        try
        {
            string query = "Update subscribers set barnyard='false', active=@active Where username=@username";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    var _with10 = comm;
                    _with10.Connection = conn;
                    _with10.CommandType = CommandType.Text;
                    _with10.CommandText = query;
                    comm.Parameters.Add("@username", SqlDbType.VarChar).Value = Membership.GetUser().ToString();
                    _with10.Parameters.Add("@active", SqlDbType.VarChar).Value = active;
                    conn.Open();
                    comm.ExecuteNonQuery();
                    Literal0.Text = "Your subscription has been cancelled.";
                }
            }
        }
        catch (SqlException ex)
        {
            MailMessage oMail1 = new MailMessage();
            oMail1.From = new MailAddress("Root Cellar <website@rootcellarboxes.com>");
            oMail1.To.Add(new MailAddress("scottw@jkmcomm.com"));
            oMail1.Subject = "Root Cellar Error";
            oMail1.Priority = MailPriority.High;
            oMail1.IsBodyHtml = true;
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            oMail1.Body += "<head><title></title></head>";
            oMail1.Body += "<body>";
            oMail1.Body += "Error in subscriber Default12: " + Membership.GetUser().ToString() + "<br /><br />";
            oMail1.Body += ex.Message + "<br /><br />" + ex.StackTrace;
            oMail1.Body += "</body>";
            oMail1.Body += "</html>";
            AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            oMail1.AlternateViews.Add(htmlView2);
            System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            ;
            
            smtpmail2.Send(oMail1);
            oMail1 = null;
            Literal0.Text = "We're sorry, there seems to have been an error.";
        }
        FillInfo();
        FillPaidInfo();
        FillDayInfo();
        FillStoreInfo();
        //FillWeekInfo();
        //FillWeekInfoVacation();
        //FillVacInfo();
        Price.Text = "$0.00";
    }
    protected void CancelPloughman_Click(object sender, EventArgs e)
    {
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        bool active = true;
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        mySqlCommand = new SqlCommand("SELECT SubID, Bounty, Barnyard FROM subscribers Where Username= '" + Membership.GetUser().ToString() + "'", mySqlConnection);
        try
        {
            mySqlConnection.Open();
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            while ((myDataReader.Read()))
            {
                SubId = myDataReader.GetInt32(0);
                if (myDataReader.GetBoolean(1) == false & myDataReader.GetBoolean(2) == false)
                {
                    active = false;
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
        try
        {
            string query = "Update weekly set ploughman='false' Where SubID=@SubID ";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    var _with11 = comm;
                    _with11.Connection = conn;
                    _with11.CommandType = CommandType.Text;
                    _with11.CommandText = query;
                    comm.Parameters.Add("@SubID", SqlDbType.Int).Value = SubId;
                    conn.Open();
                    comm.ExecuteNonQuery();

                }
            }
        }
        catch (SqlException ex)
        {
            MailMessage oMail1 = new MailMessage();
            oMail1.From = new MailAddress("Root Cellar <website@rootcellarboxes.com>");
            oMail1.To.Add(new MailAddress("scottw@jkmcomm.com"));
            oMail1.Subject = "Root Cellar Error";
            oMail1.Priority = MailPriority.High;
            oMail1.IsBodyHtml = true;
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            oMail1.Body += "<head><title></title></head>";
            oMail1.Body += "<body>";
            oMail1.Body += "Error in subscriber Default13: " + Membership.GetUser().ToString() + "<br /><br />";
            oMail1.Body += ex.Message + "<br /><br />" + ex.StackTrace;
            oMail1.Body += "</body>";
            oMail1.Body += "</html>";
            AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            oMail1.AlternateViews.Add(htmlView2);
            System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            ;
            
            smtpmail2.Send(oMail1);
            oMail1 = null;
            Literal0.Text = "We're sorry, there seems to have been an error.";
        }
        try
        {
            string query = "Update subscribers set ploughman='false', active=@active Where username=@username";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    var _with12 = comm;
                    _with12.Connection = conn;
                    _with12.CommandType = CommandType.Text;
                    _with12.CommandText = query;
                    comm.Parameters.Add("@username", SqlDbType.VarChar).Value = Membership.GetUser().ToString();
                    _with12.Parameters.Add("@active", SqlDbType.VarChar).Value = active;
                    conn.Open();
                    comm.ExecuteNonQuery();
                    Literal0.Text = "Your subscription have been cancelled.";
                }
            }
        }
        catch (SqlException ex)
        {
            MailMessage oMail1 = new MailMessage();
            oMail1.From = new MailAddress("Root Cellar <website@rootcellarboxes.com>");
            oMail1.To.Add(new MailAddress("scottw@jkmcomm.com"));
            oMail1.Subject = "Root Cellar Error";
            oMail1.Priority = MailPriority.High;
            oMail1.IsBodyHtml = true;
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            oMail1.Body += "<head><title></title></head>";
            oMail1.Body += "<body>";
            oMail1.Body += "Error in subscriber Default14: " + Membership.GetUser().ToString() + "<br /><br />";
            oMail1.Body += ex.Message + "<br /><br />" + ex.StackTrace;
            oMail1.Body += "</body>";
            oMail1.Body += "</html>";
            AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            oMail1.AlternateViews.Add(htmlView2);
            System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            ;
            
            smtpmail2.Send(oMail1);
            oMail1 = null;
            Literal0.Text = "We're sorry, there seems to have been an error.";
        }
        FillInfo();
        FillPaidInfo();
        FillDayInfo();
        FillStoreInfo();
        //FillWeekInfo();
        //FillWeekInfoVacation();
        //FillVacInfo();
        Price.Text = "$0.00";
    }
    private void UpdMailChimp(string email, bool Bounty, bool Barnyard, bool Ploughman)
    {
        try
        {
            string webAddr = "";
            if (Bounty == true)
            {
                try
                {
                    webAddr += "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=f310b8a278&email[email]=" + email1.Text.Trim() + "&merge_vars[FNAME]=" + firstname1.Text.Trim() + "&merge_vars[LNAME]=" + lastname1.Text.Trim() + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim() + "&double_optin=false&send_welcome=false";
                    Uri FwebAddr = new Uri(webAddr);
                    dynamic httpWebRequest = (HttpWebRequest)WebRequest.Create(FwebAddr);
                    httpWebRequest.ContentType = "application/json";
                    dynamic httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        dynamic val = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else if (Bounty == false)
            {
                try
                {
                    webAddr += "https://us2.api.mailchimp.com/2.0/lists/unsubscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=f310b8a278&email[email]=" + email1.Text.Trim() + "&merge_vars[FNAME]=" + firstname1.Text.Trim() + "&merge_vars[LNAME]=" + lastname1.Text.Trim() + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim() + "&double_optin=false&send_welcome=false";
                    Uri FwebAddr = new Uri(webAddr);
                    dynamic httpWebRequest = (HttpWebRequest)WebRequest.Create(FwebAddr);
                    httpWebRequest.ContentType = "application/json";
                    dynamic httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        dynamic val = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (Barnyard == true)
            {
                try
                {
                    webAddr += "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=1ad43508d8&email[email]=" + email1.Text.Trim() + "&merge_vars[FNAME]=" + firstname1.Text.Trim() + "&merge_vars[LNAME]=" + lastname1.Text.Trim() + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim() + "&double_optin=false&send_welcome=false";
                    Uri FwebAddr = new Uri(webAddr);
                    dynamic httpWebRequest = (HttpWebRequest)WebRequest.Create(FwebAddr);
                    httpWebRequest.ContentType = "application/json";
                    dynamic httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        dynamic val = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else if (Barnyard == false)
            {
                try
                {
                    webAddr += "https://us2.api.mailchimp.com/2.0/lists/unsubscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=1ad43508d8&email[email]=" + email1.Text.Trim() + "&merge_vars[FNAME]=" + firstname1.Text.Trim() + "&merge_vars[LNAME]=" + lastname1.Text.Trim() + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim() + "&double_optin=false&send_welcome=false";
                    Uri FwebAddr = new Uri(webAddr);
                    dynamic httpWebRequest = (HttpWebRequest)WebRequest.Create(FwebAddr);
                    httpWebRequest.ContentType = "application/json";
                    dynamic httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        dynamic val = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (Ploughman == true)
            {
                try
                {
                    webAddr += "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=078a386ef9&email[email]=" + email1.Text.Trim() + "&merge_vars[FNAME]=" + firstname1.Text.Trim() + "&merge_vars[LNAME]=" + lastname1.Text.Trim() + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim() + "&double_optin=false&send_welcome=false";
                    Uri FwebAddr = new Uri(webAddr);
                    dynamic httpWebRequest = (HttpWebRequest)WebRequest.Create(FwebAddr);
                    httpWebRequest.ContentType = "application/json";
                    dynamic httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        dynamic val = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else if (Ploughman == false)
            {
                try
                {
                    webAddr += "https://us2.api.mailchimp.com/2.0/lists/unsubscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=078a386ef9&email[email]=" + email1.Text.Trim() + "&merge_vars[FNAME]=" + firstname1.Text.Trim() + "&merge_vars[LNAME]=" + lastname1.Text.Trim() + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim() + "&double_optin=false&send_welcome=false";
                    Uri FwebAddr = new Uri(webAddr);
                    dynamic httpWebRequest = (HttpWebRequest)WebRequest.Create(FwebAddr);
                    httpWebRequest.ContentType = "application/json";
                    dynamic httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        dynamic val = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
            MailMessage oMail1 = new MailMessage();
            oMail1.From = new MailAddress("Root Cellar <website@rootcellarboxes.com>");
            oMail1.To.Add(new MailAddress("scottw@jkmcomm.com"));
            oMail1.Subject = "Root Cellar Error";
            oMail1.Priority = MailPriority.High;
            oMail1.IsBodyHtml = true;
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            oMail1.Body += "<head><title></title></head>";
            oMail1.Body += "<body>";
            oMail1.Body += "Error in subscriber Default15: " + Membership.GetUser().ToString() + "<br /><br />";
            oMail1.Body += ex.Message + "<br /><br />" + ex.StackTrace;
            oMail1.Body += "</body>";
            oMail1.Body += "</html>";
            AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            oMail1.AlternateViews.Add(htmlView2);
            System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            ;
            
            smtpmail2.Send(oMail1);
            oMail1 = null;
            //Literal0.Text = "We're sorry, there seems to have been an error."
        }

    }
    /// <summary>
    /// Make a payment Now
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button2_Click1(object sender, EventArgs e)
    {
        if (Price.Text == "0.00")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "al", "alert('Please select week to pay for subscribed box or select additional product.')", true);
            return;
        }
        dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Week", typeof(string)),                           
                            new DataColumn("Box",typeof(string)) });
        foreach (GridViewRow item in GridView1.Rows)
        {
            CheckBox BountyPaidCheck = item.FindControl("BountyPaidCheck") as CheckBox;
            if (BountyPaidCheck.Checked && BountyPaidCheck.Enabled == true)
            {
                dt.Rows.Add(item.Cells[1].ToString(), "Bounty");
            }
            CheckBox BarnyardPaidCheck = item.FindControl("BarnyardPaidCheck") as CheckBox;
            if (BarnyardPaidCheck.Checked && BarnyardPaidCheck.Enabled == true)
            {
                dt.Rows.Add(item.Cells[1].ToString(), "Barnyard");
            }
            CheckBox PloughmanPaidCheck = item.FindControl("PloughmanPaidCheck") as CheckBox;
            if (PloughmanPaidCheck.Checked && PloughmanPaidCheck.Enabled == true)
            {
                dt.Rows.Add(item.Cells[1].Text, "Ploughman");
            }
        }
        foreach (GridViewRow item in gvTotalProduct.Rows)
        {
            CheckBox cbTotalProduct = item.FindControl("cbTotalProduct") as CheckBox;
            if (cbTotalProduct.Checked && cbTotalProduct.Enabled == true)
            {
                dt.Rows.Add(item.Cells[1].ToString(), "Additional_Product");
            }

        }
        /****Box***/
        gvBox.DataSource = dt;
        gvBox.DataBind();


        #region Save To Store


        if (Session["SelectedProductsPayment"] != null)
        {
            SqlConnection cn = Constant.Connection();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers where Username= '" + Membership.GetUser().ToString() + "'", cn);
            DataSet ds = new DataSet();
            da.Fill(ds);

            DataTable dtSelPro = ((DataTable)Session["SelectedProducts"]).DefaultView.ToTable(true, "Week");
            bool WeekChecked = false;
            for (int j = 1; j <= dtSelPro.Rows.Count; j++)
            {
                foreach (GridViewRow drPro in gvProducts.Rows)
                {
                    Label lblWeek = drPro.FindControl("lblWeek") as Label;
                    CheckBox cbProducts = drPro.FindControl("cbProducts") as CheckBox;
                    if (dtSelPro.Rows[j - 1]["Week"].ToString() == lblWeek.Text && cbProducts.Checked)
                    {
                        WeekChecked = true;
                        break;
                    }
                }
                if (WeekChecked == true)
                {
                    cn.Open();
                    bool OnlineHome = false;
                    SqlCommand cmd = new SqlCommand("Insert into PurchaseProductTemp values(@SubscriberID,@PurchaseDate,@OnlineHome,@Store,@PickupDay,@Week,@PaymentMode,@IsPaid);SELECT CAST(scope_identity() AS int)", cn);
                    cmd.Parameters.AddWithValue("@SubscriberID", Convert.ToInt32(ds.Tables[0].Rows[0]["SubId"]));
                    cmd.Parameters.AddWithValue("@PurchaseDate", DateTime.Now);
                    foreach (GridViewRow item in gvDelivery.Rows)
                    {
                        CheckBox cbDelivery = item.FindControl("cbDelivery") as CheckBox;
                        if (cbDelivery.Checked == true && cbDelivery.Enabled == true)
                        {
                            OnlineHome = true;
                            break;
                        }
                    }

                    cmd.Parameters.AddWithValue("@OnlineHome", OnlineHome);//Home Delivery + Online Payment                       
                    cmd.Parameters.AddWithValue("@Store", StoreList.SelectedValue);
                    cmd.Parameters.AddWithValue("@PickupDay", PickupDayList.SelectedValue);
                    cmd.Parameters.AddWithValue("@PaymentMode", "Online");
                    cmd.Parameters.AddWithValue("@IsPaid", "Paid");
                    cmd.Parameters.AddWithValue("@Week", dtSelPro.Rows[j - 1]["Week"]);
                    int PurchaseProductID = (int)cmd.ExecuteScalar();
                    Session["ProductWeek"] = null;
                    cn.Close();
                    cn.Open();

                    DataSet dsSeleted = new DataSet();
                    DataTable Newdt = (DataTable)Session["SelectedProducts"];
                    DataTable CopyNewdt = Newdt.Copy();
                    dsSeleted.Tables.Add(CopyNewdt);

                    for (int i = 0; i < dsSeleted.Tables[0].Rows.Count; i++)
                    {
                        if (dtSelPro.Rows[j - 1]["Week"] == dsSeleted.Tables[0].Rows[0]["Week"].ToString())
                        {
                            // New Added By Harshal For Subtracting Total Quantity of product
                            //SqlDataAdapter da_Qua = new SqlDataAdapter("SELECT * FROM ProductDetailsNew WHERE ProductID='" + Convert.ToInt32(dsSeleted.Tables[0].Rows[j - 1]["ProductID"]) + "'", cn);
                            //DataSet ds_Qua = new DataSet();
                            //da_Qua.Fill(ds_Qua);
                            //cn.Close();
                            //cn.Open();
                            //SqlCommand cmd_Qua = new SqlCommand("UPDATE ProductDetailsNew SET Quantity='" + (Convert.ToInt32(ds_Qua.Tables[0].Rows[0]["Quantity"]) - Convert.ToInt32(dsSeleted.Tables[0].Rows[j - 1]["Quantity"])) + "' WHERE ProductID='" + Convert.ToInt32(dsSeleted.Tables[0].Rows[j - 1]["ProductID"]) + "'", cn);
                            //cmd_Qua.ExecuteNonQuery();
                            //cn.Close();

                            SqlCommand cmd2 = new SqlCommand("Insert into PurchaseProductDetailsTemp values(@BuyId,@SubscriberID,@ProductID,@ProductName,@Price,@Quantity,@PaymentMode,@IsPaid)", cn);
                            cmd2.Parameters.AddWithValue("@BuyId", PurchaseProductID);
                            cmd2.Parameters.AddWithValue("@SubscriberID", Convert.ToInt32(ds.Tables[0].Rows[0]["SubId"]));
                            cmd2.Parameters.AddWithValue("@ProductID", Convert.ToInt32(dsSeleted.Tables[0].Rows[i]["ProductID"]));
                            cmd2.Parameters.AddWithValue("@ProductName", Convert.ToString(dsSeleted.Tables[0].Rows[i]["ProductName"]));
                            cmd2.Parameters.AddWithValue("@Price", Convert.ToDouble(dsSeleted.Tables[0].Rows[i]["ProductPrice"]));
                            cmd2.Parameters.AddWithValue("@Quantity", Convert.ToInt32(dsSeleted.Tables[0].Rows[i]["Quantity"]));
                            //cmd2.Parameters.AddWithValue("@PaymentMode", rblPayment.SelectedValue);
                            cmd2.Parameters.AddWithValue("@PaymentMode", "Online");
                            cmd2.Parameters.AddWithValue("@IsPaid", "Paid");

                            cn.Open();
                            cmd2.ExecuteNonQuery();
                            cn.Close();


                        }
                    }
                    cn.Close();
                }
            }

        #endregion


            //DataTable SelectedProducts = (DataTable)Session["SelectedProductsPayment"];
            //if (SelectedProducts.Rows.Count > 0)
            //{
            //    gvSelectedProduct.DataSource = SelectedProducts;
            //    gvSelectedProduct.DataBind();
            //    lblTotal.Text = Price.Text;
            //}

        }
        #region Invoice

        /************************Genrate Invoice****************/
        //lblTotal.Text=
        //Response.ContentType = "application/pdf";
        //Response.AddHeader("content-disposition", "attachment;filename=Panel.pdf");
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(sw);
        //pnlInvoice.RenderControl(hw);
        //StringReader sr = new StringReader(sw.ToString());
        //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
        //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //pdfDoc.Open();
        //htmlparser.Parse(sr);
        //pdfDoc.Close();
        //Response.Write(pdfDoc);
        //Response.End();
        /*******************************************************/

        #endregion
        Price.Text = "0.0";
        #region Sales Vu


        string val = "";
        string RedirURL = "";
        string webAddr = "";
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        PaymentsLiteral.Text = "";
        string query1 = "INSERT INTO TempOrders (Username, ";
        string query2 = "VALUES ('" + Membership.GetUser().ToString() + "', ";
        string Oweek = "";

        string bounty = "";
        string barnyard = "";
        string ploughman = "";
        try
        {
            int Q = 0;
            if (isDev == false)
            {
                string CurrStore = "";
                mySqlCommand = new SqlCommand("SELECT store FROM Subscribers Where Username= '" + Membership.GetUser().ToString() + "'", mySqlConnection);
                try
                {
                    mySqlConnection.Open();
                    myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    while ((myDataReader.Read()))
                    {
                        if (!myDataReader.IsDBNull(0))
                        {
                            CurrStore = myDataReader.GetString(0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MailMessage oMail1 = new MailMessage();
                    //oMail1.From = new MailAddress("Root Cellar <website@rootcellarboxes.com>");
                    //oMail1.To.Add(new MailAddress("scottw@jkmcomm.com"));
                    //oMail1.Subject = "Root Cellar Error";
                    //oMail1.Priority = MailPriority.High;
                    //oMail1.IsBodyHtml = true;
                    //oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
                    //oMail1.Body += "<head><title></title></head>";
                    //oMail1.Body += "<body>";
                    //oMail1.Body += "Error in subscriber Default: " + Membership.GetUser().ToString() + "<br /><br />";
                    //oMail1.Body += ex.Message + "<br /><br />" + ex.StackTrace;
                    //oMail1.Body += "</body>";
                    //oMail1.Body += "</html>";
                    //AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
                    //oMail1.AlternateViews.Add(htmlView2);
                    //System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
                    //;
                    //
                    //smtpmail2.Send(oMail1);
                    //oMail1 = null;
                    //CurrStore = StoreList.SelectedValue;
                }
                if (StoreList.SelectedValue == "Downtown Columbia")
                {
                    StoreID = "34798";
                    APIKey = "a662c77bd1c244eb3440a3aa9dedc5bb";
                    bounty = "9";
                    barnyard = "10";
                    ploughman = "11";
                }
                else if (StoreList.SelectedValue == "Jefferson City")
                {
                    StoreID = "34800";
                    APIKey = "9fedad4964460d40d5de103b706cb054";
                    bounty = "85";
                    barnyard = "86";
                    ploughman = "87";
                }
                else if (StoreList.SelectedValue == "DHSS (Employee Only)")
                {
                    StoreID = "34800";
                    APIKey = "9fedad4964460d40d5de103b706cb054";
                    bounty = "85";
                    barnyard = "86";
                    ploughman = "87";
                }
                else if (StoreList.SelectedValue == "Mizzou North (Employee Only)")
                {
                    StoreID = "34798";
                    APIKey = "a662c77bd1c244eb3440a3aa9dedc5bb";
                    bounty = "9";
                    barnyard = "10";
                    ploughman = "11";
                }
                else if (StoreList.SelectedValue == "QUARTERDECK (Employee Only)")
                {
                    StoreID = "34798";
                    APIKey = "a662c77bd1c244eb3440a3aa9dedc5bb";
                    bounty = "9";
                    barnyard = "10";
                    ploughman = "11";
                }
                else if (StoreList.SelectedValue == "University Hospital/School of Medicine (Employee Only)")
                {
                    StoreID = "34798";
                    APIKey = "a662c77bd1c244eb3440a3aa9dedc5bb";
                    bounty = "9";
                    barnyard = "10";
                    ploughman = "11";
                }
                else if (StoreList.SelectedValue == "CRMC")
                {
                    StoreID = "34800";
                    APIKey = "9fedad4964460d40d5de103b706cb054";
                    bounty = "85";
                    barnyard = "86";
                    ploughman = "87";
                }
                else if (StoreList.SelectedValue == "CRMC Southwest")
                {
                    StoreID = "34800";
                    APIKey = "9fedad4964460d40d5de103b706cb054";
                    bounty = "85";
                    barnyard = "86";
                    ploughman = "87";
                }
                else
                {
                    bounty = "9";
                    barnyard = "10";
                    ploughman = "11";
                }
            }
            webAddr += "{'api_key':'" + APIKey + "',";
            webAddr += "'action':'create_order',";
            webAddr += "'store_id':'" + StoreID + "',";
            webAddr += "'online_customer_id':'1'";

            string prodDetails = "";
            foreach (GridViewRow Weekrow in GridView1.Rows)
            {
                string week = Weekrow.Cells[1].Text;
                if (!(week == "Deposit"))
                {
                    Oweek = week;
                    string pattern = "-(.*?)/";
                    string replacement = "/" + "\r\n";
                    Regex rgx = new Regex(pattern, RegexOptions.Singleline);
                    Oweek = rgx.Replace(Oweek, replacement);
                    Oweek = (DateTime.Parse(Oweek)).ToString().Replace(" 12:00:00 AM", "");
                }
                else
                {
                    Oweek = "1/1/1900";
                }
                CheckBox BountyPaid = Weekrow.FindControl("BountyPaidCheck") as CheckBox;
                if (BountyPaid.Enabled == true & BountyPaid.Checked == true)
                {
                    Q += 1;
                    if (string.IsNullOrEmpty(prodDetails))
                    {
                        prodDetails = ",'order_details':[";
                    }
                    else
                    {
                        prodDetails = ",";
                    }
                    prodDetails += "{'product_id':" + bounty + ",";
                    prodDetails += "'selling_price': '35.00',";
                    prodDetails += "'unit_id':0,";
                    prodDetails += "'quantity':1,";
                    prodDetails += "'notes':[{";
                    if (week == "Deposit")
                    {
                        prodDetails += "'comment':'" + Membership.GetUser().ToString() + "+Bounty+Deposit'";
                    }
                    else
                    {
                        prodDetails += "'comment':'" + Membership.GetUser().ToString() + "+Bounty+payment+for+" + week + "'";
                    }
                    prodDetails += "}]}";
                    webAddr += prodDetails;
                    query1 += "P" + Q.ToString() + "Date, P" + Q.ToString() + "Box, ";
                    query2 += "'" + Oweek + "', 'Bounty', ";
                }
                CheckBox BarnyardPaid = Weekrow.FindControl("BarnyardPaidCheck") as CheckBox;
                if (BarnyardPaid.Enabled == true & BarnyardPaid.Checked == true)
                {
                    Q += 1;
                    if (string.IsNullOrEmpty(prodDetails))
                    {
                        prodDetails = ",'order_details':[";
                    }
                    else
                    {
                        prodDetails = ",";
                    }

                    prodDetails += "{'product_id':" + barnyard + ",";
                    prodDetails += "'selling_price': '35.00',";
                    prodDetails += "'unit_id':0,";
                    prodDetails += "'quantity':1,";
                    prodDetails += "'notes':[{";
                    if (week == "Deposit")
                    {
                        prodDetails += "'comment':'" + Membership.GetUser().ToString() + "+Barnyard+Deposit'";
                    }
                    else
                    {
                        prodDetails += "'comment':'" + Membership.GetUser().ToString() + "+Barnyard+payment+for+" + week + "'";
                    }
                    prodDetails += "}]}";
                    webAddr += prodDetails;
                    query1 += "P" + Q.ToString() + "Date, P" + Q.ToString() + "Box, ";
                    query2 += "'" + Oweek + "', 'Barnyard', ";
                }
                CheckBox PloughmanPaid = Weekrow.FindControl("PloughmanPaidCheck") as CheckBox;
                if (PloughmanPaid.Enabled == true & PloughmanPaid.Checked == true)
                {
                    Q += 1;
                    if (string.IsNullOrEmpty(prodDetails))
                    {
                        prodDetails = ",'order_details':[";
                    }
                    else
                    {
                        prodDetails = ",";
                    }
                    prodDetails += "{'product_id':" + ploughman + ",";
                    prodDetails += "'selling_price': '35.00',";
                    prodDetails += "'unit_id':0,";
                    prodDetails += "'quantity':1,";
                    prodDetails += "'notes':[{";
                    if (week == "Deposit")
                    {
                        prodDetails += "'comment':'" + Membership.GetUser().ToString() + "+Ploughman+Deposit'";
                    }
                    else
                    {
                        prodDetails += "'comment':'" + Membership.GetUser().ToString() + "+Ploughman+payment+for+" + week + "'";
                    }
                    prodDetails += "}]}";
                    webAddr += prodDetails;
                    query1 += "P" + Q.ToString() + "Date, P" + Q.ToString() + "Box, ";
                    query2 += "'" + Oweek + "', 'Ploughman', ";
                }

            }
            //gvTotalProduct;
            foreach (GridViewRow Weekrow in gvTotalProduct.Rows)
            {
                string week = Weekrow.Cells[0].Text;
                if (!(week == "Deposit"))
                {
                    Oweek = week;
                    string pattern = "-(.*?)/";
                    string replacement = "/" + "\r\n";
                    Regex rgx = new Regex(pattern, RegexOptions.Singleline);
                    Oweek = rgx.Replace(Oweek, replacement);
                    Oweek = (DateTime.Parse(Oweek)).ToString().Replace(" 12:00:00 AM", "");
                }
                else
                {
                    Oweek = "1/1/1900";
                }

                CheckBox cbTotalProduct = Weekrow.FindControl("cbTotalProduct") as CheckBox;
                if (cbTotalProduct.Enabled == true & cbTotalProduct.Checked == true)
                {
                    Q += 1;
                    if (string.IsNullOrEmpty(prodDetails))
                    {
                        prodDetails = ",'order_details':[";
                    }
                    else
                    {
                        prodDetails = ",";
                    }
                    prodDetails += "{'product_id':" + ploughman + ",";
                    prodDetails += "'selling_price':" + cbTotalProduct.Text + ",";
                    prodDetails += "'unit_id':0,";
                    prodDetails += "'quantity':1,";
                    prodDetails += "'notes':[{";
                    if (week == "Deposit")
                    {
                        prodDetails += "'comment':'" + Membership.GetUser().ToString() + "+Ploughman+Deposit'";
                    }
                    else
                    {
                        prodDetails += "'comment':'" + Membership.GetUser().ToString() + "+Additional_Product +payment+for+" + week + "'";
                    }
                    prodDetails += "}]}";
                    webAddr += prodDetails;
                    query1 += "P" + Q.ToString() + "Date, P" + Q.ToString() + "Box, ";
                    query2 += "'" + Oweek + "', 'Additional_Product', ";
                }

            }
            webAddr += "]}";
            webAddr = HttpUtility.UrlEncode(webAddr);
            webAddr = webAddr.Replace("%27", "%22");
            webAddr = url + webAddr;
            if (Q > 0)
            {
                //PaymentsLiteral.Text += "Request " + webAddr + "<br /><br />"
                Uri FwebAddr = new Uri(webAddr);
                dynamic httpWebRequest = (HttpWebRequest)WebRequest.Create(FwebAddr);
                httpWebRequest.ContentType = "application/json";
                dynamic httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    val = streamReader.ReadToEnd();
                    //PaymentsLiteral.Text += "Result " + val + "<br /><br />"
                    string pattern = "{(.*?)order_id\":";
                    string replacement = "";
                    Regex rgx = new Regex(pattern, RegexOptions.Singleline);
                    val = rgx.Replace(val, replacement);
                    pattern = ",(.*?)}}";
                    replacement = "";
                    Regex rgx2 = new Regex(pattern, RegexOptions.Singleline);
                    val = rgx2.Replace(val, replacement);
                    RedirURL = GetURL(val, StoreID);
                    //PaymentsLiteral.Text += "<br /><br />" + RedirURL + "<br /><br />"
                    RedirURL = Server.UrlEncode(RedirURL).Replace("%0d%0a", "").Replace("%3a", ":").Replace("%5c%2f", "/").Replace("%3f", "?").Replace("%3d", "=").Replace("%22", "\"").Replace("%2c", ",").Replace("%3a", ":");
                    //PaymentsLiteral.Text += "<br /><br />(" + RedirURL + ")" + "<br /><br />"
                    query1 += "OrderID) ";
                    query2 += val + ")";
                }
                dynamic query = query1 + query2;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand())
                    {
                        var _with13 = comm;
                        _with13.Connection = conn;
                        _with13.CommandType = CommandType.Text;
                        _with13.CommandText = query;
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                }
                int i = 1;
                string NullFix = "";
                while (i < 26)
                {
                    NullFix += "update tempOrders set P" + i.ToString() + "Box='' where P" + i.ToString() + "Box is null update tempOrders set P" + i.ToString() + "Date='1/1/1900' where P" + i.ToString() + "Date is null ";
                    i += 1;
                }
                SqlConnection conn2 = new SqlConnection(ConnectionString);
                conn2.Open();
                using (SqlCommand UpdateCommand = new SqlCommand(NullFix, conn2))
                {
                    UpdateCommand.ExecuteNonQuery();
                }
                conn2.Close();
                RedirURL = RedirURL.ToString().TrimStart('+');
                Response.Redirect(RedirURL, false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                PaymentsLiteral.Text = "<h2><span style='color: red;'>Please select at least one box/week.</span></h2>";
            }
            PriceUPanel.Update();
        #endregion

        }
        catch (Exception ex)
        {
            MailMessage oMail1 = new MailMessage();
            oMail1.From = new MailAddress("Root Cellar <website@rootcellarboxes.com>");
            oMail1.To.Add(new MailAddress("scottw@jkmcomm.com"));
            oMail1.Subject = "Root Cellar Error";
            oMail1.Priority = MailPriority.High;
            oMail1.IsBodyHtml = true;
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            oMail1.Body += "<head><title></title></head>";
            oMail1.Body += "<body>";
            oMail1.Body += "Error in subscriber Default16: " + Membership.GetUser().ToString() + "<br /><br />";
            oMail1.Body += ex.Message + "<br /><br />" + ex.StackTrace + "<br /><br />";
            try
            {
                oMail1.Body += "WebAddr:" + webAddr + "<br /><br />";
                oMail1.Body += "GetURL:" + GetURL(val, StoreID) + "<br /><br />";
                oMail1.Body += "RedirURL:" + RedirURL + "<br /><br />";
            }
            catch (Exception ex2)
            {
                oMail1.Body += "EX2:" + ex.Message + "<br /><br />" + ex.StackTrace + "<br /><br />";
            }

            oMail1.Body += "</body>";
            oMail1.Body += "</html>";
            AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            oMail1.AlternateViews.Add(htmlView2);
            System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            ;
            
            smtpmail2.Send(oMail1);
            oMail1 = null;
            PaymentsLiteral.Text = "We're sorry, there seems to have been an error.";
        }
    }
    public string GetURL(string OrderID, string StoreID)
    {
        try
        {
            string webAddr = "";
            webAddr += "{'api_key':'" + APIKey + "',";
            webAddr += "'action':'get_payment_url',";
            webAddr += "'store_id':'" + StoreID + "',";
            webAddr += "'online_customer_id':1,";
            webAddr += "'order_id':" + OrderID + ",";
            webAddr += "'return_url':'http://www.rootcellarboxes.com/success',";
            webAddr += "'platform':'PC'";
            webAddr += "}";
            webAddr = HttpUtility.UrlEncode(webAddr);
            webAddr = webAddr.Replace("%27", "%22");
            webAddr = url + webAddr;
            //PaymentsLiteral.Text += "Request " + webAddr + "<br /><br />"
            Uri FwebAddr = new Uri(webAddr);
            dynamic httpWebRequest = (HttpWebRequest)WebRequest.Create(FwebAddr);
            httpWebRequest.ContentType = "application/json";
            dynamic httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                dynamic val = streamReader.ReadToEnd();
                //PaymentsLiteral.Text += "Result " + val + "<br /><br />"
                string Status = val;
                string pURL = val;
                string pattern = "{(.*?)status\":\"";
                string replacement = "";
                Regex rgx = new Regex(pattern, RegexOptions.Singleline);
                Status = rgx.Replace(Status, replacement);
                pattern = "\",(.*?)}}";
                replacement = "";
                Regex rgx2 = new Regex(pattern, RegexOptions.Singleline);
                Status = rgx2.Replace(Status, replacement);
                if (Status.Contains("ok"))
                {
                    pattern = "{(.*?)https";
                    replacement = "https";
                    Regex rgx3 = new Regex(pattern, RegexOptions.Singleline);
                    pURL = rgx3.Replace(pURL, replacement);
                    pattern = "\"}}";
                    replacement = "";
                    Regex rgx4 = new Regex(pattern, RegexOptions.Singleline);
                    pURL = rgx4.Replace(pURL, replacement);
                }
                return pURL;
            }
            PriceUPanel.Update();
        }
        catch (Exception ex)
        {
            MailMessage oMail1 = new MailMessage();
            oMail1.From = new MailAddress("Root Cellar <website@rootcellarboxes.com>");
            oMail1.To.Add(new MailAddress("scottw@jkmcomm.com"));
            oMail1.Subject = "Root Cellar Error";
            oMail1.Priority = MailPriority.High;
            oMail1.IsBodyHtml = true;
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            oMail1.Body += "<head><title></title></head>";
            oMail1.Body += "<body>";
            oMail1.Body += "Error in subscriber Default17: " + Membership.GetUser().ToString() + "<br /><br />";
            oMail1.Body += ex.Message + "<br /><br />" + ex.StackTrace;
            oMail1.Body += "</body>";
            oMail1.Body += "</html>";
            AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            oMail1.AlternateViews.Add(htmlView2);
            System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            ;
            
            smtpmail2.Send(oMail1);
            oMail1 = null;
            PaymentsLiteral.Text = "We're sorry, there seems to have been an error.";
            return "";
        }
        return "";
    }

    protected void RadTabStrip1_TabClick(object sender, RadTabStripEventArgs e)
    {
        PaymentsLiteral.Text = "";
        Literal0.Text = "";
        MailPanel1.Update();
    }

    protected void Button4_Click(object sender, EventArgs e)
    {

        try
        {
            if (!string.IsNullOrEmpty(NewPassBox1.Text))
            {
                if (!string.IsNullOrEmpty(NewPassBox2.Text))
                {
                    if (NewPassBox1.Text.Trim() == NewPassBox2.Text.Trim())
                    {
                        if (NewPassBox1.Text.Length > 5)
                        {
                            MembershipUser myObject = Membership.GetUser();
                            string generatedpassword = myObject.ResetPassword();
                            myObject.IsApproved = true;
                            myObject.ChangePassword(generatedpassword, NewPassBox1.Text.Trim());
                            PassLiteral.Text = "<span style='color:green;'>Your password has been changed!</span>";
                        }
                        else
                        {
                            PassLiteral.Text = "<span style='color:red;'>Your password must be at least 6 characters.</span>";
                        }
                    }
                    else
                    {
                        PassLiteral.Text = "<span style='color:red;'>Your passwords do not match.</span>";
                    }
                }
                else
                {
                    PassLiteral.Text = "<span style='color:red;'>Please confirm your new password.</span>";
                }
            }
            else
            {
                PassLiteral.Text = "<span style='color:red;'>Please enter a new password.</span>";
            }
        }
        catch (Exception ex)
        {
            PassLiteral.Text = "We're sorry, there was a problem resetting your password.";
        }
    }
    /// <summary>
    /// New Added: List of all Products
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rcProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        //Label lblPrice = rcProducts.FindControl("lblPrice") as Label;
        //Label lblProductName = rcProducts.FindControl("lblProductName") as Label;
        //List<Constant.Cart> AllCart = new List<Constant.Cart>();
        //Constant.Cart obj=new Constant.Cart();
        //obj.ProductID=Convert.ToInt32( e.CommandArgument);
        //obj.Price=Convert.ToInt32(lblPrice.Text);
        //obj.ProductName=lblProductName.Text;
        //AllCart.Add(obj);
        SqlConnection cn = Constant.Connection();
        SqlDataAdapter da = new SqlDataAdapter("Select * from ProductDetailsNew where ProductID='" + e.CommandArgument + "'", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        //AllCart = new List<Constant.Cart>();
        Constant.Cart obj = new Constant.Cart();
        TextBox Quantity = rcProducts.FindControl("txtQuantity") as TextBox;
        obj.ProductID = Convert.ToInt32(ds.Tables[0].Rows[0]["ProductID"]);
        obj.Price = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[0]["ProductPrice"]), 2);
        obj.ProductName = Convert.ToString(ds.Tables[0].Rows[0]["ProductName"]);
        obj.Quantity = Convert.ToInt32(Quantity.Text);
        //AllCart.Add(obj);
    }
    /// <summary>
    /// View All Products
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddCart_Click(object sender, EventArgs e)
    {
        //Session.RemoveAll();
        SqlConnection cn = Constant.Connection();
        bool CheckCart = false;
        string MsgProduct = string.Empty;

        SqlCommand cmd = new SqlCommand("BuyDisplayProduct", cn);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Store", StoreList.SelectedItem.Text);
        cmd.Parameters.AddWithValue("@Week", ddlweekProduct.SelectedValue);
        SqlDataAdapter da5 = new SqlDataAdapter();
        da5.SelectCommand = cmd;
        DataSet ds5 = new DataSet();
        da5.Fill(ds5);
        int DataTableId = 1;

        try
        {
            foreach (RepeaterItem item in rcProducts.Controls)
            {
                CheckBox cbAddToCart = item.FindControl("cbAddToCart") as CheckBox;
                if (cbAddToCart.Checked)
                {
                    CheckCart = true;
                    Label ProductID = item.FindControl("ProductID") as Label;
                    Label lblProductName = item.FindControl("lblProductName") as Label;
                    Label lblPrice = item.FindControl("lblPrice") as Label;
                    TextBox Quantity = item.FindControl("txtQuantity") as TextBox;
                    HiddenField hfQuantity = item.FindControl("hfQuantity") as HiddenField;

                    if (Quantity.Text == string.Empty)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Insert Quantity for " + lblProductName.Text + "')", true);
                        return;
                    }
                    else if (hfQuantity.Value == "" || Convert.ToInt32(hfQuantity.Value.ToString()) < Convert.ToInt32(Quantity.Text))
                    {
                        String Msg = "Only " + hfQuantity.Value + " Quantity Remaining For " + lblProductName.Text + ".";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + Msg + "')", true);
                        return;
                    }

                    //Code For To Select Only Avilable Products
                    //for (int i = 0; i < ds5.Tables[0].Rows.Count; i++)
                    //{
                    //    if (ds5.Tables[0].Rows[i]["ProductID"].ToString() != ProductID.Text)
                    //    {
                    //        cbAddToCart.Checked = false;
                    //        if (MsgProduct == string.Empty)
                    //        {
                    //            MsgProduct = lblProductName.Text;
                    //            ds5.Tables[0].Rows.RemoveAt(i);
                    //            break;
                    //        }
                    //        else
                    //        {
                    //            MsgProduct += "," + lblProductName.Text;
                    //            //ds5.Tables[0].Rows[i].Delete();
                    //            ds5.Tables[0].Rows.RemoveAt(i);
                    //            break;
                    //        }
                    //    }
                    //}

                }
            }

            foreach (RepeaterItem item in rcProducts.Controls)
            {
                CheckBox cbAddToCart = item.FindControl("cbAddToCart") as CheckBox;
                if (cbAddToCart.Checked)
                {
                    CheckCart = true;
                    Label ProductID = item.FindControl("ProductID") as Label;
                    Label lblProductName = item.FindControl("lblProductName") as Label;
                    Label lblPrice = item.FindControl("lblPrice") as Label;
                    TextBox Quantity = item.FindControl("txtQuantity") as TextBox;
                    HiddenField hfQuantity = item.FindControl("hfQuantity") as HiddenField;
                    // DataTable SelectedProducts = (DataTable)Session["addCard"];
                    DataTable SelectedProducts = (DataTable)Session["SelectedProducts"];
                    DataRow dr = SelectedProducts.NewRow();

                    dr["Id"] = SelectedProducts.Rows.Count + 1;

                    dr["ProductId"] = Convert.ToInt32(ProductID.Text);
                    dr["ProductName"] = Convert.ToString(lblProductName.Text);
                    dr["ProductPrice"] = Convert.ToDouble(Math.Round(Convert.ToDouble(lblPrice.Text), 2).ToString());
                    dr["Quantity"] = Convert.ToString(Quantity.Text);
                    dr["Week"] = Convert.ToString(ddlweekProduct.SelectedValue);
                    SelectedProducts.Rows.Add(dr);
                    Session["SelectedProducts"] = SelectedProducts;
                    Session["addCard"] = SelectedProducts;
                    Session["ProductWeek"] = ddlweekProduct.SelectedValue;
                }
            }
            if (CheckCart == false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Atleast One Product');", true);
                return;
            }
            //CheckCart = false;
            //foreach (RepeaterItem item in rcProducts.Controls)
            //{
            //    CheckBox cbAddToCart = item.FindControl("cbAddToCart") as CheckBox;
            //    if (cbAddToCart.Checked)
            //    {
            //        CheckCart = true;
            //        break;
            //    }
            //}
            //if (CheckCart == false)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Product " + MsgProduct + " are not available at this time.');", true);
            //    Session.Abandon();
            //    return;

            //}

            Response.Redirect("~/customer/ProductPay.aspx?Week=" + ddlweekProduct.SelectedValue + "&MsgProduct=" + MsgProduct);
        }
        catch (Exception er)
        {
        }
    }

    /// <summary>
    /// Add Vacation
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddVacation_Click(object sender, EventArgs e)
    {
        SqlConnection cn = Constant.Connection();
        SqlDataAdapter da = new SqlDataAdapter("SELECT SubID FROM subscribers Where Username= '" + Membership.GetUser().ToString() + "'", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        SqlDataAdapter da3 = new SqlDataAdapter("SELECT * FROM VacationDetails Where CustomerID= '" + ds.Tables[0].Rows[0]["SubID"].ToString() + "' and VacationWeek='" + WeekList.SelectedValue + "'", cn);
        DataSet ds3 = new DataSet();
        da3.Fill(ds3);
        if (ds3.Tables[0].Rows.Count == 0)
        {
            SqlDataAdapter da2 = new SqlDataAdapter("SELECT * FROM VacationDetails Where CustomerID= '" + ds.Tables[0].Rows[0]["SubID"].ToString() + "'", cn);
            DataSet ds2 = new DataSet();
            if (isEditVacation == false)
            {
                //Save New Vacation 
                da2.Fill(ds2);
                if (ds2.Tables[0].Rows.Count < 2)
                {
                    try
                    {
                        cn.Open();
                        SqlCommand cmd = new SqlCommand("Insert Into VacationDetails Values (@CustomerID,@VacationWeek,@VacationAddedDate,@VacationAddedBy)", cn);
                        cmd.Parameters.AddWithValue("@CustomerID", ds.Tables[0].Rows[0]["SubID"].ToString());
                        cmd.Parameters.AddWithValue("@VacationWeek", WeekList.SelectedValue);
                        cmd.Parameters.AddWithValue("@VacationAddedDate", DateTime.Now.ToShortDateString());
                        cmd.Parameters.AddWithValue("@VacationAddedBy", "Customer Initials");

                        cmd.ExecuteNonQuery();
                        cn.Close();

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Vacation Added Successfully')", true);

                        Notification();


                    }
                    catch (Exception err)
                    {

                    }

                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('You have already used 2 vacation.')", true);
            }
            else
            {
                //Update Existing Record
                cn.Open();
                SqlCommand cmd = new SqlCommand("Update VacationDetails set VacationWeek=@VacationWeek,VacationAddedDate=@VacationAddedDate,VacationAddedBy=@VacationAddedBy Where VID=" + VacationID + "", cn);
                cmd.Parameters.AddWithValue("@VacationWeek", WeekList.SelectedValue);
                cmd.Parameters.AddWithValue("@VacationAddedDate", DateTime.Now.ToShortDateString());
                cmd.Parameters.AddWithValue("@VacationAddedBy", "Customer Initials");
                cmd.ExecuteNonQuery();

                cn.Close();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Vacation Updated Successfully')", true);
                //ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Update();", true);
                NotificationForUpdate();
                isEditVacation = false;
            }

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Vacation is already added for selected week.')", true);
            return;
        }
        BindVacation();
    }
    /// <summary>
    /// Mail Notification To Cusomer,Admin and Employee
    /// </summary>
    private static void Notification()
    {
        SqlConnection cn = Constant.Connection();

        SqlDataAdapter da = new SqlDataAdapter("select * from [dbo].[Subscribers]  where SubId='" + UserID + "'", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);

        string BodyForAdmin = "Respected Admin, New vacation will be added. Please check details";
        string BodyForEmployee = "Respected Sir, You are added new vacation";
        string BodyForCustomer = "Respected Sir, Vacation will be added";

        Constant.SendMail(Constant.AdminMailId, "New Vacation Added", BodyForAdmin);
        Constant.SendMail(ds.Tables[0].Rows[0]["Email1"].ToString(), "New Vacation Added", BodyForEmployee);
        //Constant.SendMail(CustomerEmail, "New Vacation Added", BodyForCustomer);
    }

    /// <summary>
    /// Mail for update Notification Customer,Admin and Employee
    /// </summary>
    private static void NotificationForUpdate()
    {
        SqlConnection cn = Constant.Connection();

        SqlDataAdapter da = new SqlDataAdapter("select * from [dbo].[Subscribers] where SubId=" + UserID + "", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);

        string BodyForAdmin = "Respected Admin, New vacation has been updated. Please check details";
        string BodyForEmployee = "Respected Sir, Added vacation has been updated";
        string BodyForCustomer = "Respected Sir, Vacation has been updated";

        Constant.SendMail(Constant.AdminMailId, "Vacation Update", BodyForAdmin);
        Constant.SendMail(ds.Tables[0].Rows[0]["Email1"].ToString(), "Vacation Update", BodyForEmployee);
        //Constant.SendMail(CustomerEmail, "Vacation Update", BodyForCustomer);
    }


    /// <summary>
    /// Edit And Delete Operation
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvVacation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete1")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            SqlConnection cn = Constant.Connection();
            SqlCommand cmd = new SqlCommand("Delete From VacationDetails where VID=" + index + "", cn);
            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
            BindVacation();
        }

        if (e.CommandName == "Edit1")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            //Response.Redirect("~/account/Default.aspx?VID=" + EncryptDecrypt.EncryptPassword(index.ToString()));
            SqlConnection cn = Constant.Connection();
            SqlDataAdapter da = new SqlDataAdapter("Select * from VacationDetails where VID=" + index + "", cn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                WeekList.SelectedValue = ds.Tables[0].Rows[0]["VacationWeek"].ToString();
                VacationID = Convert.ToString(index);
                isEditVacation = true;

            }

        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }


    /***********New 2************/
    //protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName == "Delete1")
    //    {
    //        int index = Convert.ToInt32(e.CommandArgument);
    //        SqlConnection cn = Constant.Connection();
    //        SqlCommand cmd = new SqlCommand("Delete From VacationDetails where VID=" + index + "", cn);
    //        cn.Open();
    //        cmd.ExecuteNonQuery();
    //        cn.Close();
    //        BindVacation();
    //    }

    //    //if (e.CommandName == "Edit1")
    //    //{
    //    //    int index = Convert.ToInt32(e.CommandArgument);
    //    //    Response.Redirect("~/account/Default.aspx?VID=" + EncryptDecrypt.EncryptPassword(index.ToString()));
    //    //}
    //}
    /// <summary>
    /// Request for Home Delivery
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRequest_Click(object sender, EventArgs e)
    {
        if (litMsg.Text == "You Request is: Pending")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('You Request is pending. Please Wait for Administration Replay')", true);
            return;
        }
        else
        {
            SqlConnection cn = Constant.Connection();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers where Username= '" + Membership.GetUser().ToString() + "'", cn);
            DataSet ds = new DataSet();
            da.Fill(ds);

            SqlDataAdapter da3 = new SqlDataAdapter("Select * from HomeDeliverySubscriber where SubId='" + ds.Tables[0].Rows[0]["SubId"].ToString() + "'", cn);
            DataSet ds3 = new DataSet();
            da3.Fill(ds3);
            if (ds3.Tables[0].Rows.Count > 0)
                isEditHome = true;

            else
                isEditHome = false;


            if (isEditHome != true)
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("Insert into HomeDeliverySubscriber values(@SubId,@DeliveryAddress,@BestTime,@Location,@SpecialInstruction,@Request,@Charges)", cn);
                    cmd.Parameters.AddWithValue("@SubId", ds.Tables[0].Rows[0]["SubId"].ToString());
                    cmd.Parameters.AddWithValue("@DeliveryAddress", txtDeliveryAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@BestTime", ddlBestTime.SelectedValue);
                    cmd.Parameters.AddWithValue("@Location", ddlLocation.SelectedValue);
                    cmd.Parameters.AddWithValue("@SpecialInstruction", txtSpecialinstr.Text.Trim());
                    cmd.Parameters.AddWithValue("@Request", "Pending");
                    cmd.Parameters.AddWithValue("@Charges", DBNull.Value);

                    cmd.ExecuteNonQuery();
                    cn.Close();
                    isEditHome = false;
                    litMsg.Text = "You Request is: Pending";
                }
                catch (SqlException ex)
                {

                }
            }
            else//Update
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("Update HomeDeliverySubscriber set DeliveryAddress=@DeliveryAddress,BestTime=@BestTime,Location=@Location,SpecialInstruction=@SpecialInstruction, Request=@Request where SubId='" + ds.Tables[0].Rows[0]["SubId"].ToString() + "'", cn);

                    cmd.Parameters.AddWithValue("@DeliveryAddress", txtDeliveryAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@BestTime", ddlBestTime.SelectedValue);
                    cmd.Parameters.AddWithValue("@Location", ddlLocation.SelectedValue);
                    cmd.Parameters.AddWithValue("@SpecialInstruction", txtSpecialinstr.Text.Trim());
                    cmd.Parameters.AddWithValue("@Request", "Pending");
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    litMsg.Text = "You Request is: Pending";
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Request has been send to admin.')", true);
                }
                catch (SqlException ex)
                {

                }
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Request has been send to admin.')", true);
        }
    }


    protected void rptSelectedProduct_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }
    protected void cbProduct_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cbAddToCart = (CheckBox)sender;
        if (cbAddToCart.Checked)
            ProductPrice = ProductPrice + Convert.ToDouble(cbAddToCart.ToolTip);
        else
            ProductPrice = ProductPrice - Convert.ToDouble(cbAddToCart.ToolTip);
        Price.Text = (ProductPrice + BoxPrice).ToString("0.00");
        Session["SPPayment"] = null;
    }
    /// <summary>
    /// Home Delivery Checked Changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cbHomeDelivery_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cbHomeDelivery = (CheckBox)sender;

        int count = 0;
        bool week = false;
        SqlConnection cn = Constant.Connection();
        SqlDataAdapter da = new SqlDataAdapter("Select * from subscribers Where Username= '" + Membership.GetUser().ToString() + "'", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);

        SqlDataAdapter da2 = new SqlDataAdapter("Select * from HomeDeliverySubscriber Where SubId= '" + ds.Tables[0].Rows[0]["SubId"].ToString() + "'", cn);
        DataSet ds2 = new DataSet();
        da2.Fill(ds2);
        if (cbHomeDelivery.Checked)
        {

            if (ds2.Tables[0].Rows.Count > 0)
            {
                if (ds2.Tables[0].Rows[0]["Request"].ToString() != "Approved")
                {
                    cbHomeDelivery.Checked = false;
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Your request for home delivery is not aproved.')", true);
                    //return;
                }
                else
                {
                    Charges = ds2.Tables[0].Rows[0]["Charges"].ToString();
                    foreach (GridViewRow item in GridView1.Rows)
                    {
                        CheckBox PloughmanPaidCheck = item.FindControl("PloughmanPaidCheck") as CheckBox;
                        CheckBox BountyPaidCheck = item.FindControl("BountyPaidCheck") as CheckBox;
                        CheckBox BarnyardPaidCheck = item.FindControl("BarnyardPaidCheck") as CheckBox;
                        if (PloughmanPaidCheck.Checked || BountyPaidCheck.Checked || BarnyardPaidCheck.Checked)
                        {
                            count = count + 1;
                        }

                    }
                    foreach (GridViewRow item2 in gvTotalProduct.Rows)
                    {
                        CheckBox cbTotalProduct = item2.FindControl("cbTotalProduct") as CheckBox;
                        if (cbTotalProduct.Checked && cbHomeDelivery.Checked)
                        {
                            foreach (GridViewRow item in GridView1.Rows)
                            {
                                if (item2.Cells[0].Text == item.Cells[1].Text)
                                {
                                    CheckBox PloughmanPaidCheck = item.FindControl("PloughmanPaidCheck") as CheckBox;
                                    CheckBox BountyPaidCheck = item.FindControl("BountyPaidCheck") as CheckBox;
                                    CheckBox BarnyardPaidCheck = item.FindControl("BarnyardPaidCheck") as CheckBox;
                                    if (PloughmanPaidCheck.Checked || BountyPaidCheck.Checked || BarnyardPaidCheck.Checked)
                                    {
                                        week = true;
                                    }
                                    //return;
                                }

                            }
                            if (week == false)
                            {
                                count = count + 1;
                                break;
                            }
                        }
                    }

                    Double Total = Convert.ToDouble(Price.Text);
                    Total = Total + (Convert.ToDouble(Charges) * count);
                    ProductPrice = ProductPrice + Convert.ToDouble(Charges);
                    Price.Text = Total.ToString("0.00");
                    // Price.Text = ProductPrice.ToString();
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Home Delivery Charges $" + (Convert.ToDouble(Charges) * count) + " Has Been Added.')", true);
                    return;
                }
            }
            else
            {
                cbHomeDelivery.Checked = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Your Home Delivery Details From Account Info.')", true);
                return;
            }

        }
        else
        {
            Charges = ds2.Tables[0].Rows[0]["Charges"].ToString();
            foreach (GridViewRow item in GridView1.Rows)
            {
                CheckBox PloughmanPaidCheck = item.FindControl("PloughmanPaidCheck") as CheckBox;
                CheckBox BountyPaidCheck = item.FindControl("BountyPaidCheck") as CheckBox;
                CheckBox BarnyardPaidCheck = item.FindControl("BarnyardPaidCheck") as CheckBox;
                if (PloughmanPaidCheck.Checked || BountyPaidCheck.Checked || BarnyardPaidCheck.Checked)
                {
                    count = count + 1;
                }

            }
            foreach (GridViewRow item2 in gvTotalProduct.Rows)
            {
                CheckBox cbTotalProduct = item2.FindControl("cbTotalProduct") as CheckBox;
                if (cbTotalProduct.Checked && !cbHomeDelivery.Checked)
                {
                    foreach (GridViewRow item in GridView1.Rows)
                    {
                        if (item2.Cells[0].Text == item.Cells[1].Text)
                        {
                            week = true;
                            //return;
                        }
                    }
                    if (week == false)
                    {
                        count = count + 1;
                        break;
                    }
                }
            }

            Double Total = Convert.ToDouble(Price.Text);
            Total = Total - (Convert.ToDouble(Charges) * count);

            ProductPrice = ProductPrice - Convert.ToDouble(Charges);
            Price.Text = Total.ToString("0.00");
            //Price.Text = ProductPrice.ToString();


            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Home Delivery Charges has been removed')", true);
            return;
        }

    }


    protected void btnPayment_Click(object sender, EventArgs e)
    {

    }
    protected void gvSelectedProduct_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Double Price = Convert.ToDouble(e.Row.Cells[3].Text);
            int Quantity = Convert.ToInt32(e.Row.Cells[4].Text);
            Label Total = e.Row.FindControl("lblTotalPrice") as Label;
            Total.Text = Convert.ToString(Price * Quantity);
            TotalPrice = TotalPrice + Convert.ToDouble(Total.Text);
        }
    }
    protected void rblPayment_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (rblPayment.SelectedValue == "Store")
        //{
        //    cbHomeDelivery.Enabled = false;
        //    Session["PaymentMode"] = "Store";
        //}
        //else
        //{
        //    cbHomeDelivery.Enabled = true;
        //    Session["PaymentMode"] = "Online";
        //}
    }

    /// <summary>
    /// Pay Products In Store
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPayInStore_Click(object sender, EventArgs e)
    {
        if (Session["SelectedProductsPayment"] != null)
        {
            SqlConnection cn = Constant.Connection();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers where Username= '" + Membership.GetUser().ToString() + "'", cn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cn.Open();
            SqlCommand cmd = new SqlCommand("Insert into PurchaseProduct values(@SubscriberID,@PurchaseDate,@OnlineHome,@Store,@PickupDay,@Week,@PaymentMode,@IsPaid);SELECT CAST(scope_identity() AS int)", cn);
            cmd.Parameters.AddWithValue("@SubscriberID", Convert.ToInt32(ds.Tables[0].Rows[0]["SubId"]));
            cmd.Parameters.AddWithValue("@PurchaseDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@OnlineHome", true);
            cmd.Parameters.AddWithValue("@Store", StoreList.SelectedValue);
            cmd.Parameters.AddWithValue("@PickupDay", PickupDayList.SelectedValue);
            cmd.Parameters.AddWithValue("@PaymentMode", "Store");
            cmd.Parameters.AddWithValue("@IsPaid", "UnPaid");
            //if (ddlWeek.SelectedItem.Text == " - Select a Week - ")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "al", "alert('Please Select Week')", true);
            //    return;
            //}
            //cmd.Parameters.AddWithValue("@Week", ddlWeek.SelectedValue);
            cmd.Parameters.AddWithValue("@Week", Session["ProductWeek"].ToString());
            int PurchaseProductID = (int)cmd.ExecuteScalar();

            cn.Close();
            cn.Open();

            DataSet dsSeleted = new DataSet();
            DataTable Newdt = (DataTable)Session["SelectedProductsPayment"];
            DataTable CopyNewdt = Newdt.Copy();
            dsSeleted.Tables.Add(CopyNewdt);
            // New Added By Harshal For Subtracting Total Quantity of product
            for (int i = 0; i < dsSeleted.Tables[0].Rows.Count; i++)
            {
                SqlDataAdapter da_Qua = new SqlDataAdapter("SELECT * FROM ProductDetailsNew WHERE ProductID='" + Convert.ToInt32(dsSeleted.Tables[0].Rows[i]["ProductID"]) + "'", cn);
                DataSet ds_Qua = new DataSet();
                da_Qua.Fill(ds_Qua);
                cn.Close();
                cn.Open();
                SqlCommand cmd_Qua = new SqlCommand("UPDATE ProductDetailsNew SET Quantity='" + (Convert.ToInt32(ds_Qua.Tables[0].Rows[0]["Quantity"]) - Convert.ToInt32(dsSeleted.Tables[0].Rows[i]["Quantity"])) + "' WHERE ProductID='" + Convert.ToInt32(dsSeleted.Tables[0].Rows[i]["ProductID"]) + "'", cn);
                //cmd_Qua.Parameters.AddWithValue("@Quantity", (Convert.ToInt32(ds_Qua.Tables[0].Rows[0]["Quantity"]) - Convert.ToInt32(dsSeleted.Tables[0].Rows[i]["Quantity"])));
                cmd_Qua.ExecuteNonQuery();
                cn.Close();
            }
            //
            for (int i = 0; i < dsSeleted.Tables[0].Rows.Count; i++)
            {
                SqlCommand cmd2 = new SqlCommand("Insert into PurchaseProductDetails values(@BuyId,@SubscriberID,@ProductID,@ProductName,@Price,@Quantity,@PaymentMode,@IsPaid)", cn);
                cmd2.Parameters.AddWithValue("@BuyId", PurchaseProductID);
                cmd2.Parameters.AddWithValue("@SubscriberID", Convert.ToInt32(ds.Tables[0].Rows[0]["SubId"]));
                cmd2.Parameters.AddWithValue("@ProductID", Convert.ToInt32(dsSeleted.Tables[0].Rows[i]["ProductID"]));
                cmd2.Parameters.AddWithValue("@ProductName", Convert.ToString(dsSeleted.Tables[0].Rows[i]["ProductName"]));
                cmd2.Parameters.AddWithValue("@Price", Convert.ToDouble(dsSeleted.Tables[0].Rows[i]["ProductPrice"]));
                cmd2.Parameters.AddWithValue("@Quantity", Convert.ToInt32(dsSeleted.Tables[0].Rows[i]["Quantity"]));
                cmd2.Parameters.AddWithValue("@PaymentMode", "Store");
                cmd2.Parameters.AddWithValue("@IsPaid", "UnPaid");
                cn.Open();
                cmd2.ExecuteNonQuery();
                cn.Close();

            }
            cn.Close();
            DataTable SelectedProducts = (DataTable)Session["SelectedProductsPayment"];
            if (SelectedProducts.Rows.Count > 0)
            {
                gvSelectedProduct.DataSource = SelectedProducts;
                gvSelectedProduct.DataBind();
                lblTotal.Text = TotalPrice.ToString();
            }
        }
        Session["SelectedProductsPayment"] = null;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "al", "alert('Products added to cart Successfully. PLease Pickup Your Product From Neareast Store.')", true);
        return;
    }

    protected void ddlweekProduct_TextChanged(object sender, EventArgs e)
    {

        BindProducts();
    }
    protected void gvTotalProduct_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Edit1")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument); // Get the current row
            string Week = Convert.ToString(gvTotalProduct.Rows[rowIndex].Cells[0].Text);

            Response.Redirect("~/customer/ProductPay.aspx?MsgProduct=" + "&ProductId=" + (Convert.ToInt32(e.CommandArgument) + 1).ToString() + "&Week=" + Week);
            //+"&Week="+
        }

    }
    protected void cbTotalProduct_CheckedChanged(object sender, EventArgs e)
    {

        CheckBox cbTotalProduct = (CheckBox)sender;
        if (cbTotalProduct.Checked)
        {
            ProductPrice = ProductPrice + Convert.ToDouble(cbTotalProduct.Text);
            Total = Total + Convert.ToDouble(cbTotalProduct.Text);
        }
        else
        {
            ProductPrice = ProductPrice - Convert.ToDouble(cbTotalProduct.Text);
            Total = Total - Convert.ToDouble(cbTotalProduct.Text);
        }
        Price.Text = (ProductPrice + BoxPrice).ToString("0.00");
    }

    protected void lbTotalProduct_Click(object sender, EventArgs e)
    {
        //Get the button that raised the event
        LinkButton btn = (LinkButton)sender;

        //Get the row that contains this button
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;

        //Get rowindex
        int rowindex = gvr.RowIndex;
        string str = "";
        //Response.Redirect("~/account/Default.aspx?VID=" + EncryptDecrypt.EncryptPassword(index.ToString()));
        if (Session["TotalProduct"] != null)
        {
            DataTable dt_totalProduct = Session["TotalProduct"] as DataTable;


            str = dt_totalProduct.Rows[rowindex][0].ToString();


        }
        if (Session["SPPayment"] != null)
        {
            Response.Redirect("~/account/Default.aspx");
        }
        Response.Redirect("~/account/Default.aspx?Page=" + str);

    }
    protected void gvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblWeek = e.Row.FindControl("lblWeek") as Label;
            CheckBox cbProducts = e.Row.FindControl("cbProducts") as CheckBox;
            if (lblWeek.Text == "Deposit")
            {
                cbProducts.Enabled = false;
                return;
            }
            SqlConnection cn2 = Constant.Connection();
            SqlDataAdapter da1 = new SqlDataAdapter("SELECT dbo.PurchaseProduct.*, dbo.Subscribers.* FROM dbo.PurchaseProduct INNER JOIN dbo.Subscribers ON dbo.PurchaseProduct.SubscriberID = dbo.Subscribers.SubId where dbo.PurchaseProduct.PaymentMode='Online' and dbo.Subscribers.Username= '" + Membership.GetUser().ToString() + "' and dbo.PurchaseProduct.Week ='" + lblWeek.Text + "'", cn2);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                cbProducts.Checked = true;
                cbProducts.Enabled = false;
            }
            else
            {
                cbProducts.Checked = false;
                cbProducts.Enabled = true;
            }
            DataTable SelectedProducts = Session["SelectedProducts"] as DataTable;
            //DataSet ds = new DataSet();
            //ds.Tables.Add(SelectedProducts);
            if (SelectedProducts != null)
            {
                double CbTextValue = 0.00;
                for (int i = 1; i <= SelectedProducts.Rows.Count; i++)
                {
                    if (SelectedProducts.Rows[i - 1]["Week"].ToString() == lblWeek.Text)
                    {
                        CbTextValue = CbTextValue + (Convert.ToDouble(SelectedProducts.Rows[i - 1]["ProductPrice"]) * Convert.ToDouble(SelectedProducts.Rows[i - 1]["Quantity"]));
                        cbProducts.Checked = true;
                    }
                }
                if (CbTextValue == 0.00)
                {
                    cbProducts.Enabled = false;
                }
                else
                    cbProducts.Text = "$" + CbTextValue.ToString("0.00");
            }
        }
    }

    protected void ddlWeeks_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DataTable SelectedProducts = Session["TotalProduct"] as DataTable;
        //if (SelectedProducts != null)
        //{
        //    if (SelectedProducts.Rows.Count > 0)
        //    {
        //        IEnumerable<DataRow> dtRow = from myRow in SelectedProducts.AsEnumerable()
        //                                     where myRow.Field<string>("Week") == ddlWeeks.SelectedValue.ToString()
        //                                     select myRow;

        //        if (dtRow != null)
        //        {
        //            DataTable dtTemp = dtRow.CopyToDataTable();
        //            gvTotalProduct.DataSource = dtTemp;
        //            gvTotalProduct.DataBind();
        //        }
        //        else
        //        {
        //            gvTotalProduct.DataSource = SelectedProducts;
        //            gvTotalProduct.DataBind();
        //        }
        //    }
        //}
        //else
        //    ddlWeeks.SelectedValue = "Select Week";
    }

    /// <summary>
    /// view current orders
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCurrentOrders_Click(object sender, EventArgs e)
    {

        Response.Redirect("~/customer/ProductPay.aspx?Week=" + ddlweekProduct.SelectedValue + "&MsgProduct=" + "&view=");
    }
    protected void gvDelivery_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblWeek = e.Row.FindControl("lblWeek") as Label;
            CheckBox cbDelivery = e.Row.FindControl("cbDelivery") as CheckBox;
            if (lblWeek.Text == "Deposit")
            {
                cbDelivery.Enabled = false;
                return;
            }
            SqlConnection cn2 = Constant.Connection();
            SqlDataAdapter da1 = new SqlDataAdapter("SELECT dbo.PurchaseProduct.*, dbo.Subscribers.* FROM dbo.PurchaseProduct INNER JOIN dbo.Subscribers ON dbo.PurchaseProduct.SubscriberID = dbo.Subscribers.SubId where dbo.Subscribers.Username= '" + Membership.GetUser().ToString() + "' and dbo.PurchaseProduct.Week ='" + lblWeek.Text + "'", cn2);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToBoolean(ds1.Tables[0].Rows[0]["OnlineHome"]) == true)
                {
                    cbDelivery.Checked = true;
                }
                else
                    cbDelivery.Checked = false;

                cbDelivery.Enabled = false;
            }
            DataTable SelectedProducts = Session["SelectedProducts"] as DataTable;
            if (SelectedProducts != null)
            {
                double CbTextValue = 0.00;
                for (int i = 1; i <= SelectedProducts.Rows.Count; i++)
                {
                    if (SelectedProducts.Rows[i - 1]["Week"].ToString() == lblWeek.Text)
                    {
                        cbDelivery.Enabled = true;
                    }
                }


            }
        }
    }
    protected void cbDelivery_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cbDelivery = (CheckBox)sender;
        SqlConnection cn = Constant.Connection();
        SqlDataAdapter da = new SqlDataAdapter("Select * from subscribers Where Username= '" + Membership.GetUser().ToString() + "'", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);

        SqlDataAdapter da2 = new SqlDataAdapter("Select * from HomeDeliverySubscriber Where SubId= '" + ds.Tables[0].Rows[0]["SubId"].ToString() + "'", cn);
        DataSet ds2 = new DataSet();
        da2.Fill(ds2);
        if (ds2.Tables[0].Rows.Count == 0)
        {
            cbDelivery.Checked = false;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Your request for home delivery is not aproved.')", true);
            return;
        }
        else if (ds2.Tables[0].Rows[0]["Request"].ToString() != "Approved")
        {
            cbDelivery.Checked = false;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Your request for home delivery is not aproved.')", true);
            return;
        }
        if (cbDelivery.Checked)
        {

            #region Old Code
            //if (ds2.Tables[0].Rows.Count > 0)
            //{
            //    if (ds2.Tables[0].Rows[0]["Request"].ToString() != "Approved")
            //    {
            //        cbDelivery.Checked = false;
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Your request for home delivery is not aproved.')", true);
            //        return;
            //    }
            //    else
            //    {
            //        foreach (GridViewRow item in gvTotalProduct.Rows)
            //        {
            //            string Week = Convert.ToString(item.Cells[0].Text);
            //            CheckBox cbHomeDelivery = item.FindControl("cbHomeDelivery") as CheckBox;
            //            cbHomeDelivery.Checked = cbDelivery.Checked;

            //            if (cbDelivery.ToolTip == Week && cbDelivery.Enabled == true)
            //            {
            //                cbHomeDelivery_CheckedChanged(cbHomeDelivery, e);
            //            }
            //        }
            //    }
            //} 
            #endregion
            ProductPrice = Convert.ToDouble(ds2.Tables[0].Rows[0]["Charges"]) + Convert.ToDouble(ProductPrice);
        }
        else
        {
            ProductPrice = Convert.ToDouble(ProductPrice) - Convert.ToDouble(ds2.Tables[0].Rows[0]["Charges"]);
        }
        Price.Text = (ProductPrice + BoxPrice).ToString("0.00");
    }
    protected void rcProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lblPrice = e.Item.FindControl("lblPrice") as Label;
        if (lblPrice != null)
        {
            lblPrice.Text = Math.Round(Convert.ToDecimal(lblPrice.Text), 2).ToString("0.00");
        }
    }
    protected void cbProducts_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cbProducts = sender as CheckBox;
       
        if (cbProducts.Checked)
        {
           // ProductPrice = Convert.ToDouble(cbProducts.Text.TrimStart('$')) + Convert.ToDouble(ProductPrice)
        }
        else
        {
            ProductPrice = Convert.ToDouble(ProductPrice) - Convert.ToDouble(cbProducts.Text.TrimStart('$'));
        }
        Price.Text = (ProductPrice + BoxPrice).ToString("0.00");
    }
}