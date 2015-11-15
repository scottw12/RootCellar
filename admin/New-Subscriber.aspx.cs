using System.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Net.Mail;
using PerceptiveMCAPI;
using PerceptiveMCAPI.Types;
using PerceptiveMCAPI.Methods;
using System.IO;
using System.Net;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Telerik.Web.UI.Calendar.Utils;
using System.Text;
using System.Web.UI;


public partial class admin_New_Subscriber : System.Web.UI.Page
{

    private SqlConnection conn = null;
    private string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    private SqlCommand cmd = null;
    string password = "";
    string Username = "";
    string useremail = "";

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
                if (ds.Tables[0].Rows[0]["CreateNewSubscriber"].ToString() == "False")
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
                    FillInfo();
                    FillDayInfo();
                    FillStoreInfo();
                    Price.Text = "$0.00";
                }
            }
        }
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
    protected void FillDayInfo()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PickupDay");
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


    protected void StoreList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void PickupDayList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        Literal1.Text = "";
        if (BountyBox.Checked == false & BarnyardBox.Checked == false & PloughmanBox.Checked == false)
        {
            Literal1.Text = "<span style='color:red;'>Please select at least one box </span>";
        }
        else
        {
            if (CreateUser() == true)
            {
                if (DBInsert() == true)
                {
                    UpdMailChimp(email1.Text, BountyNL.Checked, BarnyardNL.Checked, PloughmanNL.Checked);
                    if (!string.IsNullOrEmpty(email2.Text))
                    {
                        UpdMailChimp(email2.Text, BountyNL.Checked, BarnyardNL.Checked, PloughmanNL.Checked);
                    }
                    SendEmail();
                    Panel1.Visible = false;
                    Literal0.Text += "<span style='color:green;'><h2>" + firstname1.Text + " " + lastname1.Text + "'s account has been created!</h2></span>";
                }
                else
                {
                    Literal1.Text = "<span style='color:red;'><h2> There was a problem creating an account for " + firstname1.Text + " " + lastname1.Text + ". Please check all of the info and try again.</h2></span>";
                }
            }
        }
    }
    public static bool UserNameExists(string yourName)
    {
        return Membership.GetUser(yourName) != null;
    }
    public bool CreateUser()
    {
        try
        {
            string FName = firstname1.Text.Replace("'", "*1*").Replace("\"", "*2*").Replace(" ", "").Replace("  ", "");
            string LName = lastname1.Text.Replace("'", "*1*").Replace("\"", "*2*").Replace(" ", "").Replace("  ", "");
            Username = FName.Trim() + "." + LName.Trim();
            int i = 0;
            int n = 1;
            while (i == 0)
            {
                if (UserNameExists(Username) == true)
                {
                    Username = Username + n.ToString();
                    n += 1;
                }
                else
                {
                    i += 1;
                }
            }
            string Uemail = email1.Text.Replace("'", "").Replace("\"", "").Replace(" ", "").Replace("  ", "");
            string SecretQ = "Please use the Contact Us page to request a reset password";
            string SecretA = "ergwergkqejfqeoufwqeofiheowfqpkoadmvnwo";
            password = firstname1.Text.Trim() + "TempPsw" + (DateTime.Now.Minute + 1).ToString();
            password.Trim();
            MembershipCreateStatus createStatus = default(MembershipCreateStatus);
            MembershipUser newUser = Membership.CreateUser(Username, password, Uemail, SecretQ, SecretA, true, out createStatus);
            
            switch (createStatus)
            {
                case MembershipCreateStatus.Success:
                    return true;
                    //break; // TODO: might not be correct. Was : Exit Select

                    break;
                case MembershipCreateStatus.DuplicateUserName:
                    Literal1.Text = "<span style='color:red;'>There is already a user with this username.</span>";
                    return false;
                    break; // TODO: might not be correct. Was : Exit Select

                    break;
                case MembershipCreateStatus.DuplicateEmail:
                    Literal1.Text = "<span style='color:red;'>There is already a user with this email address.</span>";
                    return false;
                    break; // TODO: might not be correct. Was : Exit Select

                    break;
                case MembershipCreateStatus.InvalidEmail:
                    Literal1.Text = "<span style='color:red;'>There email address you provided in invalid.</span>";
                    return false;
                    break; // TODO: might not be correct. Was : Exit Select

                    break;
                case MembershipCreateStatus.InvalidAnswer:
                    Literal1.Text = "<span style='color:red;'>There security answer was invalid.</span>";
                    return false;
                    break; // TODO: might not be correct. Was : Exit Select

                    break;
                case MembershipCreateStatus.InvalidPassword:
                    Literal1.Text = "<span style='color:red;'>The password you provided is invalid. It must be seven characters long and have at least one non-alphanumeric character.</span>";
                    return false;
                    break; // TODO: might not be correct. Was : Exit Select

                    break;
                default:
                    Literal1.Text = "<span style='color:red;'>There was an unknown error; the user account was NOT created.</span>";
                    return false;
                    break; // TODO: might not be correct. Was : Exit Select

                    break;
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
            //oMail1.Body += "Error creating user: " + Username + "<br /><br />";
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
            //Literal1.Text = "We're sorry, there seems to have been an error";
            return false;
        }
    }
    protected void SendEmail()
    {
        //MailMessage oMail0 = new MailMessage();
        //oMail0.From = new MailAddress("Root Cellar <qat2015team@gmail.com>");
        //oMail0.To.Add(new MailAddress(email1.Text.Replace("'", "").Replace("\"", "").Replace(" ", "")));
        //oMail0.Subject = "Root Cellar Subscription ";
        //oMail0.Priority = MailPriority.High;
        //oMail0.IsBodyHtml = true;
        //oMail0.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
        string oMail0Body = "<head><title></title></head>";
        oMail0Body += "<body>";
        oMail0Body += "Hello " + firstname1.Text.Replace("'", "").Replace("\"", "").Replace(" ", "") + ",<br /><br />";
        oMail0Body += "Welcome to the Root Cellar!<br /> The Username and Password for our new online subscription management tool is below. Here you will be able to schedule vacation weeks, provide advance payment online and view your subscription information.<br /><br />You can login to <a href='http://www.rootcellarboxes.com/login'>http://www.rootcellarboxes.com/login</a> using:<br />";
        oMail0Body += "Username: " + Username + "<br />";
        oMail0Body += "Passowrd: " + password + "<br /><br />";
        oMail0Body += "We also wanted to highlight a few important details about the Subscription Programs:<br /><br />";
        oMail0Body += "We're excited to have you as our newest subscriber and look forward to getting to know you. ";
        oMail0Body += "<ul><li>Subscription members receive 10% off all purchase in the store (regardless of the day). Make sure to mention to the Sales Associate you’re a subscriber to ensure you get your discount.</li>";
        oMail0Body += "<li>In an effort save a few trees our weekly newsletter is sent to your email Thursday mornings. If you do not receive the newsletter or would like additional email addresses added to the list please contact us at rootcellarmo@gmail.com</li>";
        oMail0Body += "<li>Switching box pick up day (example: Friday to Thursday) to make your life easier is allowed. We do ask that you use this flexibility rarely and let us know as soon as possible. Pick-up hours are 12:00PM – 7:00 PM Thursday and 10:00 AM to 7:00 PM Friday 4:30 PM to 6:00 PM Jefferson City.</li>";
        oMail0Body += "<li>The last opportunity to pick up a box for the week is Saturday at 1:00PM (unless advance arrangements have been made).</li>";
        oMail0Body += "<li>Significant effort goes into harvesting, preparing and packing your box each week. In addition the food items in any box not picked up is often wasted. To ensure that all members receive the best value possible and the program is sustainable, failure to pick up your box may result in the loss of deposit.</li>";
        oMail0Body += "<li>Many of you will travel for work or pleasure during the season. We allow two vacation weeks to provide flexibility for these events. We do ask that you provide us 7 Days notice of your absence. Those vacations can be scheduled online or with a sales associate. </li>";
        oMail0Body += "<li> In an effort to be as sustainable as possible we do reuse and recycle as much as possible. One of the items we reuse the most is the Box your items are packaged in each week. Please Return the box each week to help us cut down on waste. In addition you will need to return milk bottles each week to avoid additional deposits. We also except Weiler egg cartons, Nature Fresh Duck Egg cartons and pint glass Jars.</li>";
        oMail0Body += "<li>Because of the large volume of boxes assembled each week we are unable to substitute items except in cases of food allergies. We keep these allergies on file and pack a substitute item in advance. In addition we have the 'Trading Post' available that allows subscribers to easily exchange items they may not need. </li></ul>";
        oMail0Body += "Please feel free to give us a call (573) 443-5055 or reply to this email if you have any additional questions.<br /><br /> ";
        oMail0Body += "Root Cellar Team";
        oMail0Body += "</body>";
        oMail0Body += "</html>";
        //AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail0.Body, null, "text/html");
        //oMail0.AlternateViews.Add(htmlView2);
        //SmtpClient smtpmail2 = new SmtpClient("smtp.gmail.com");
        ////smtpmail2.UseDefaultCredentials = false;
        //;
        //
        try
        {
            //smtpmail2.Send(oMail0);
            Constant.SendMail(email1.Text.Trim(), "Root Cellar Subscription", oMail0Body);

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
            //oMail1.Body += "Error creating user: " + Username + "<br /><br />";
            //oMail1.Body += ex.Message + "<br /><br />" + ex.StackTrace;
            //oMail1.Body += "</body>";
            //oMail1.Body += "</html>";
            //AlternateView htmlView1 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            //oMail1.AlternateViews.Add(htmlView1);
            //;
            //
            //smtpmail2.Send(oMail1);
            //oMail1 = null;
            //Literal1.Text = "We're sorry, there seems to have been an error";
        }
        //oMail0 = null;
    }
    public bool DBInsert()
    {
        try
        {
            string query = "INSERT INTO subscribers (FirstName1, LastName1, Email1, phone1, FirstName2, LastName2, Email2, phone2, Address, City, State, Zip, Allergies, vacUsed, BountyNL, BarnyardNL, PloughmanNL, Enrolled, Referred, Notes, pickupday, store, bounty, barnyard, ploughman, username, active) VALUES (@FirstName1, @LastName1, @Email1, @phone1, @FirstName2, @LastName2, @Email2, @phone2, @Address, @City, @State, @Zip, @Allergies, @vacUsed, @BountyNL, @BarnyardNL, @PloughmanNL,  @Enrolled, @Referred, @Notes, @pickupday, @store, @bounty, @barnyard, @ploughman, @Username, @active) ";
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
                    _with1.Parameters.Add("@Allergies", SqlDbType.VarChar).Value = allergies.Text;
                    _with1.Parameters.Add("@vacUsed", SqlDbType.Int).Value = 0;
                    _with1.Parameters.Add("@BountyNL", SqlDbType.Bit).Value = BountyNL.Checked;
                    _with1.Parameters.Add("@BarnyardNL", SqlDbType.Bit).Value = BarnyardNL.Checked;
                    _with1.Parameters.Add("@PloughmanNL", SqlDbType.Bit).Value = PloughmanNL.Checked;
                    _with1.Parameters.Add("@Enrolled", SqlDbType.SmallDateTime).Value = System.DateTime.Now.ToShortDateString();
                    _with1.Parameters.Add("@Referred", SqlDbType.VarChar).Value = "";
                    _with1.Parameters.Add("@Notes", SqlDbType.Text).Value = "";
                    _with1.Parameters.Add("@pickupday", SqlDbType.Text).Value = PickupDayList.SelectedValue;
                    _with1.Parameters.Add("@store", SqlDbType.Text).Value = StoreList.SelectedValue;
                    _with1.Parameters.Add("@username", SqlDbType.Text).Value = Username;
                    _with1.Parameters.Add("@barnyard", SqlDbType.Bit).Value = BarnyardBox.Checked;
                    _with1.Parameters.Add("@bounty", SqlDbType.Bit).Value = BountyBox.Checked;
                    _with1.Parameters.Add("@ploughman", SqlDbType.Bit).Value = PloughmanBox.Checked;
                    bool active = true;
                    if (NextYear.Checked == true)
                    {
                        active = false;
                    }
                    _with1.Parameters.Add("@active", SqlDbType.Bit).Value = active;


                    conn.Open();
                    comm.ExecuteNonQuery();
                    int SubId = 0;
                    SqlDataReader myDataReader = default(SqlDataReader);
                    SqlConnection mySqlConnection = default(SqlConnection);
                    SqlCommand mySqlCommand = default(SqlCommand);
                    mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                    mySqlCommand = new SqlCommand("SELECT SubID FROM subscribers Where FirstName1= '" + firstname1.Text + "' and address='" + address.Text + "'", mySqlConnection);
                    try
                    {
                        mySqlConnection.Open();
                        myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                        while ((myDataReader.Read()))
                        {
                            SubId = myDataReader.GetInt32(0);
                            query = "INSERT INTO Weekly (SubId, bounty, barnyard, ploughman, PickupDay, Location, Vacation, PaidBounty, PaidBarnyard, PaidPloughman, Pickedup, Notes, Week) VALUES (@SubId, @bounty, @barnyard, @ploughman, @PickupDay, @Location, 'False', @PaidBounty, @PaidBarnyard, @PaidPloughman, 'False', '', @Week) ";
                            using (SqlConnection conn2 = new SqlConnection(ConnectionString))
                            {
                                using (SqlCommand comm2 = new SqlCommand())
                                {
                                    var _with2 = comm2;
                                    _with2.Connection = conn2;
                                    _with2.CommandType = CommandType.Text;
                                    _with2.CommandText = query;
                                    comm2.Parameters.Add("@SubId", SqlDbType.Int).Value = SubId;
                                    _with2.Parameters.Add("@barnyard", SqlDbType.Bit).Value = BarnyardBox.Checked;
                                    _with2.Parameters.Add("@bounty", SqlDbType.Bit).Value = BountyBox.Checked;
                                    _with2.Parameters.Add("@ploughman", SqlDbType.Bit).Value = PloughmanBox.Checked;
                                    _with2.Parameters.Add("@PickupDay", SqlDbType.VarChar).Value = PickupDayList.SelectedValue;
                                    _with2.Parameters.Add("@Location", SqlDbType.VarChar).Value = StoreList.SelectedValue;
                                    _with2.Parameters.Add("@PaidBounty", SqlDbType.Bit).Value = BountyBox.Checked;
                                    _with2.Parameters.Add("@PaidBarnyard", SqlDbType.Bit).Value = BarnyardBox.Checked;
                                    _with2.Parameters.Add("@PaidPloughman", SqlDbType.Bit).Value = PloughmanBox.Checked;
                                    _with2.Parameters.Add("@Week", SqlDbType.SmallDateTime).Value = "1/1/1900";
                                    try
                                    {
                                        conn2.Open();
                                        comm2.ExecuteNonQuery();
                                    }
                                    catch (SqlException ex)
                                    {
                                        Literal0.Text = "Were sorry, there was an error";
                                        return false;
                                    }
                                }
                            }
                            DateTime startDate = DateTime.Now;
                            DateTime endDate = Convert.ToDateTime("12/31/2018");
                            TimeSpan diff = endDate - startDate;
                            int days = diff.Days;
                            for (int i = 0; i <= days; i++)
                            {
                                DateTime testDate = startDate.AddDays(i);
                                switch (testDate.DayOfWeek)
                                {
                                    case DayOfWeek.Thursday:
                                        if (CheckExcluded(testDate.ToShortDateString()) == false)
                                        {
                                            query = "INSERT INTO Weekly (SubId, bounty, barnyard, ploughman, PickupDay, Location, Vacation, PaidBounty, PaidBarnyard, PaidPloughman, Pickedup, Notes, Week) VALUES (@SubId, @bounty, @barnyard, @ploughman, @PickupDay, @Location, 'False', @PaidBounty, @PaidBarnyard, @PaidPloughman, 'False', '', @Week) ";
                                            using (SqlConnection conn2 = new SqlConnection(ConnectionString))
                                            {
                                                using (SqlCommand comm2 = new SqlCommand())
                                                {
                                                    var _with3 = comm2;
                                                    _with3.Connection = conn2;
                                                    _with3.CommandType = CommandType.Text;
                                                    _with3.CommandText = query;
                                                    comm2.Parameters.Add("@SubId", SqlDbType.Int).Value = SubId;
                                                    _with3.Parameters.Add("@barnyard", SqlDbType.Bit).Value = BarnyardBox.Checked;
                                                    _with3.Parameters.Add("@bounty", SqlDbType.Bit).Value = BountyBox.Checked;
                                                    _with3.Parameters.Add("@ploughman", SqlDbType.Bit).Value = PloughmanBox.Checked;
                                                    _with3.Parameters.Add("@PickupDay", SqlDbType.VarChar).Value = PickupDayList.SelectedValue;
                                                    _with3.Parameters.Add("@Location", SqlDbType.VarChar).Value = StoreList.SelectedValue;
                                                    _with3.Parameters.Add("@PaidBounty", SqlDbType.VarChar).Value = false;
                                                    _with3.Parameters.Add("@PaidBarnyard", SqlDbType.VarChar).Value = false;
                                                    _with3.Parameters.Add("@PaidPloughman", SqlDbType.VarChar).Value = false;
                                                    _with3.Parameters.Add("@Week", SqlDbType.SmallDateTime).Value = testDate.ToShortDateString();
                                                    try
                                                    {
                                                        conn2.Open();

                                                        comm2.ExecuteNonQuery();


                                                    }
                                                    catch (SqlException ex)
                                                    {
                                                        Literal0.Text = "Were sorry, there was an error";
                                                        return false;
                                                    }
                                                }
                                            }
                                        }
                                        break; // TODO: might not be correct. Was : Exit Select

                                        break;
                                }
                            }
                        }
                        using (SqlConnection conn2 = new SqlConnection(ConnectionString))
                        {
                            if (BountyBox.Checked == true)
                            {
                                foreach (GridViewRow Weekrow in GridView1.Rows)
                                {
                                    CheckBox BountyPaid = Weekrow.FindControl("BountyPaidCheck") as CheckBox;
                                    string week = Weekrow.Cells[0].Text;
                                    if (week == "Deposit")
                                    {
                                        week = "1/1/1900";
                                    }
                                    try
                                    {

                                        if (BountyPaid.Checked == true)
                                        {
                                            string pattern = "-(.*?)/";
                                            string replacement = "/" + "\r\n";
                                            Regex rgx = new Regex(pattern, RegexOptions.Singleline);
                                            week = rgx.Replace(week, replacement);
                                            week = (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "");
                                            if (conn2.State == ConnectionState.Open)
                                            {
                                                conn2.Close();
                                            }
                                            conn2.Open();
                                            string sql = "update weekly set PaidBounty='True' where SubID='" + SubId.ToString() + "' and week='" + week + "'";
                                            SqlCommand cmd = new SqlCommand(sql);
                                            cmd.CommandType = CommandType.Text;
                                            cmd.Connection = conn2;
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Literal0.Text = "Were sorry, there was an error";
                                        return false;
                                    }

                                }
                            }
                            if (BarnyardBox.Checked == true)
                            {
                                foreach (GridViewRow Weekrow in GridView1.Rows)
                                {
                                    CheckBox BountyPaid = Weekrow.FindControl("BarnyardPaidCheck") as CheckBox;
                                    if (BountyPaid.Enabled == true & BountyPaid.Checked == true)
                                    {
                                        string week = Weekrow.Cells[0].Text;
                                        if (week == "Deposit")
                                        {
                                            week = "1/1/1900";
                                        }
                                        string pattern = "-(.*?)/";
                                        string replacement = "/" + "\r\n";
                                        Regex rgx = new Regex(pattern, RegexOptions.Singleline);
                                        week = rgx.Replace(week, replacement);
                                        week = (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "");
                                        if (conn2.State == ConnectionState.Open)
                                        {
                                            conn2.Close();
                                        }
                                        conn2.Open();
                                        string sql = "update weekly set PaidBarnyard='True' where SubID='" + SubId.ToString() + "' and week='" + week + "'";
                                        SqlCommand cmd = new SqlCommand(sql);
                                        cmd.CommandType = CommandType.Text;
                                        cmd.Connection = conn2;
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                            if (PloughmanBox.Checked == true)
                            {
                                foreach (GridViewRow Weekrow in GridView1.Rows)
                                {
                                    CheckBox BountyPaid = Weekrow.FindControl("PloughmanPaidCheck") as CheckBox;
                                    if (BountyPaid.Enabled == true & BountyPaid.Checked == true)
                                    {
                                        string week = Weekrow.Cells[0].Text;
                                        if (week == "Deposit")
                                        {
                                            week = "1/1/1900";
                                        }
                                        string pattern = "-(.*?)/";
                                        string replacement = "/" + "\r\n";
                                        Regex rgx = new Regex(pattern, RegexOptions.Singleline);
                                        week = rgx.Replace(week, replacement);
                                        week = (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "");
                                        if (conn2.State == ConnectionState.Open)
                                        {
                                            conn2.Close();
                                        }
                                        conn2.Open();
                                        string sql = "update weekly set PaidPloughman='True' where SubID='" + SubId.ToString() + "' and week='" + week + "'";
                                        SqlCommand cmd = new SqlCommand(sql);
                                        cmd.CommandType = CommandType.Text;
                                        cmd.Connection = conn2;
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }


                    }
                    catch (SqlException ex)
                    {
                        Literal1.Text = "We're sorry, there seems to have been an error";
                        return false;
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
            return true;
        }
        catch (SqlException ex)
        {
            Literal1.Text = "We're sorry, there seems to have been an error";
            return false;
        }


    }
    public bool CheckExcluded(string searchValue)
    {
        try
        {
            DataTable Edt = new DataTable();
            Edt.Columns.Add("EDate");
            string SqlQuary = "SELECT edate FROM excluded where edate>='" + System.DateTime.Today + "' order by edate";
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
                    while (myDataReader.Read())
                    {
                        Edt.Rows.Add(myDataReader.GetDateTime(0));
                    }
                }
                myDataReader.Close();
            }
            foreach (DataRow row in Edt.Rows)
            {
                if (row["EDate"].ToString().Replace(" 12:00:00 AM", "") == searchValue)
                    return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            return false;
            Literal0.Text = "Were sorry, there was an error";
        }
    }
    protected void FillInfo()
    {
        GridView1.Columns[1].Visible = false;
        GridView1.Columns[2].Visible = false;
        GridView1.Columns[3].Visible = false;
        DataTable dt = new DataTable();
        dt.Columns.Add("Week");
        dt.Columns.Add("PaidBounty");
        dt.Columns.Add("PaidBarnyard");
        dt.Columns.Add("PaidPloughman");
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        dt.Rows.Add("Deposit", "True", "True", "True");
        if (NextYear.Checked == false)
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
                                SDateRange = " and week <= '" + myDataReader2.GetDateTime(1) + "' ";
                            }
                        }
                        myDataReader2.Close();
                    }
                }
            }
            finally
            {
            }
            string SqlQuary = "SELECT DISTINCT Week FROM Weekly where week>='" + System.DateTime.Today.AddDays(-1) + "'" + SDateRange + "ORDER BY [Week]";
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
                            string week = (myDataReader.GetDateTime(0).Month.ToString() + "/" + myDataReader.GetDateTime(0).Day.ToString() + "-" + myDataReader.GetDateTime(0).AddDays(1).Day.ToString() + "/" + myDataReader.GetDateTime(0).Year.ToString());
                            dt.Rows.Add(week, "False", "False", "False");
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

        }
        else if (NextYear.Checked == true)
        {
        }
        GridView1.DataSource = dt;
        GridView1.DataBind();
        changeColumns();
    }
    protected void NextChanged(Object sender, EventArgs e)
    {
        FillInfo();
    }
    protected void OnCheckedChanged(Object sender, EventArgs e)
    {
        UpdatePanel1.Update();
        int days = 0;
        foreach (GridViewRow Weekrow in GridView1.Rows)
        {
            CheckBox BountyPaid = Weekrow.FindControl("BountyPaidCheck") as CheckBox;
            if (BountyPaid.Checked == true)
            {
                days += 1;
            }
            CheckBox BarnyardPaid = Weekrow.FindControl("BarnyardPaidCheck") as CheckBox;
            if (BarnyardPaid.Checked == true)
            {
                days += 1;
            }
            CheckBox PloughmanPaid = Weekrow.FindControl("PloughmanPaidCheck") as CheckBox;
            if (PloughmanPaid.Checked == true)
            {
                days += 1;
            }
        }
        if (days * 35 == 0)
        {
            Price.Text = "$0.00";
        }
        else
        {
            Price.Text = "" + (days * 35).ToString("C2");
        }

    }
    protected void OnBoxChanged(object sender, EventArgs e)
    {
        changeColumns();
    }
    protected void changeColumns()
    {
        if (BountyBox.Checked == false)
        {
            GridView1.Columns[1].Visible = false;
            foreach (GridViewRow Weekrow in GridView1.Rows)
            {
                if (Weekrow.Cells[0].Text == "Deposit")
                {
                    CheckBox BountyPaid = Weekrow.FindControl("BountyPaidCheck") as CheckBox;
                    BountyPaid.Checked = false;
                    BountyPaid.Enabled = false;
                }
            }
        }
        else
        {
            GridView1.Columns[1].Visible = true;
            foreach (GridViewRow Weekrow in GridView1.Rows)
            {
                if (Weekrow.Cells[0].Text == "Deposit")
                {
                    CheckBox BountyPaid = Weekrow.FindControl("BountyPaidCheck") as CheckBox;
                    BountyPaid.Checked = true;
                    BountyPaid.Enabled = false;
                }
            }
        }
        if (BarnyardBox.Checked == false)
        {
            GridView1.Columns[2].Visible = false;
            foreach (GridViewRow Weekrow in GridView1.Rows)
            {
                if (Weekrow.Cells[0].Text == "Deposit")
                {
                    CheckBox BarnyardPaid = Weekrow.FindControl("BarnyardPaidCheck") as CheckBox;
                    BarnyardPaid.Checked = false;
                    BarnyardPaid.Enabled = false;
                }
            }
        }
        else
        {
            GridView1.Columns[2].Visible = true;
            foreach (GridViewRow Weekrow in GridView1.Rows)
            {
                if (Weekrow.Cells[0].Text == "Deposit")
                {
                    CheckBox BarnyardPaid = Weekrow.FindControl("BarnyardPaidCheck") as CheckBox;
                    BarnyardPaid.Checked = true;
                    BarnyardPaid.Enabled = false;
                }
            }
        }
        if (PloughmanBox.Checked == false)
        {
            GridView1.Columns[3].Visible = false;
            foreach (GridViewRow Weekrow in GridView1.Rows)
            {
                if (Weekrow.Cells[0].Text == "Deposit")
                {
                    CheckBox PloughmanPaid = Weekrow.FindControl("PloughmanPaidCheck") as CheckBox;
                    PloughmanPaid.Checked = false;
                    PloughmanPaid.Enabled = false;
                }
            }
        }
        else
        {
            GridView1.Columns[3].Visible = true;
            foreach (GridViewRow Weekrow in GridView1.Rows)
            {
                if (Weekrow.Cells[0].Text == "Deposit")
                {
                    CheckBox PloughmanPaid = Weekrow.FindControl("PloughmanPaidCheck") as CheckBox;
                    PloughmanPaid.Checked = true;
                    PloughmanPaid.Enabled = false;
                }
            }
        }
        UpdatePanel1.Update();
        int days = 0;
        foreach (GridViewRow Weekrow in GridView1.Rows)
        {
            CheckBox BountyPaid = Weekrow.FindControl("BountyPaidCheck") as CheckBox;
            if (BountyPaid.Checked == true)
            {
                days += 1;
            }
            CheckBox BarnyardPaid = Weekrow.FindControl("BarnyardPaidCheck") as CheckBox;
            if (BarnyardPaid.Checked == true)
            {
                days += 1;
            }
            CheckBox PloughmanPaid = Weekrow.FindControl("PloughmanPaidCheck") as CheckBox;
            if (PloughmanPaid.Checked == true)
            {
                days += 1;
            }
        }
        if (days * 35 == 0)
        {
            Price.Text = "$0.00";
        }
        else
        {
            Price.Text = "" + (days * 35).ToString("C2");
        }
    }
    public bool UpdMailChimp(string email, bool Bounty, bool Barnyard, bool Ploughman)
    {
        try
        {
            string webAddr = "";
           
            if (Bounty == true)
            {
                webAddr += "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=f310b8a278&email[email]=" + email1.Text.Trim() + "&merge_vars[FNAME]=" + firstname1.Text.Trim() + "&merge_vars[LNAME]=" + lastname1.Text.Trim() + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim() + "&double_optin=false&send_welcome=false";
                Uri FwebAddr = new Uri(webAddr);

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(FwebAddr);
                // dynamic httpWebRequest = (HttpWebRequest)WebRequest.Create(FwebAddr);
                httpWebRequest.ContentType = "application/json";
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                // dynamic httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    dynamic val = streamReader.ReadToEnd();
                }
            }
            if (Barnyard == true)
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
            if (Ploughman == true)
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
            return true;
        }
        catch (Exception ex)
        {
            StringBuilder oMail1 = new StringBuilder();
            //MailMessage oMail1 = new MailMessage();
            // oMail1.From = new MailAddress("Root Cellar <qat2015team@gmail.com>");
            // oMail1.To.Add(new MailAddress("nitint@custom-soft.com"));

            oMail1.Append("<html xmlns='http://www.w3.org/1999/xhtml' >");
            oMail1.Append("<head><title></title></head>");
            oMail1.Append("<body>");
            oMail1.Append("Error Updating MailChimp user: " + Username + "<br /><br />");
            oMail1.Append(ex.Message + "<br /><br />" + ex.StackTrace);
            oMail1.Append("</body>");
            oMail1.Append("</html>");
            Constant.SendMail("pradipc@custom-soft.com", "Subjet", Convert.ToString(oMail1));
            // AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            //  oMail1.AlternateViews.Add(htmlView2);
            // SmtpClient smtpmail2 = new SmtpClient("smtp.gmail.com");
            // ;
            //  
            // smtpmail2.Send(oMail1);
            // oMail1 = null;
            return false;
        }

    }


    /****************New Events*****************/
    protected void StoreList_SelectedIndexChanged1(object sender, EventArgs e)
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
    }
    protected void PickupDayList_SelectedIndexChanged1(object sender, EventArgs e)
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
    }
}