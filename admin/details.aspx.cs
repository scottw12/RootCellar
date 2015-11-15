using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using PerceptiveMCAPI;
using PerceptiveMCAPI.Types;
using PerceptiveMCAPI.Methods;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class admin_details : System.Web.UI.Page
{
    protected string SubID, week;
    private SqlConnection conn = null;
    private string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    bool isExisting = false;
    protected void Page_Load(object sender, EventArgs e)
    {

        if ((Request.QueryString["s"] != null))
        {
            if (!string.IsNullOrEmpty(Request.QueryString["s"].ToString()))
            {
                SubID = Request.QueryString["s"].ToString();
                SqlDataReader myDataReader = default(SqlDataReader);
                string query = "SELECT username FROM subscribers Where SubId=@SubId";
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand())
                    {
                        var _with1 = comm;
                        _with1.Connection = conn;
                        _with1.CommandType = CommandType.Text;
                        _with1.CommandText = query;
                        comm.Parameters.Add("@SubID", SqlDbType.VarChar).Value = SubID;
                        conn.Open();
                        myDataReader = comm.ExecuteReader(CommandBehavior.CloseConnection);
                        while ((myDataReader.Read()))
                        {
                            if (!myDataReader.IsDBNull(0))
                            {
                                UsernameLiteral.Text = myDataReader.GetString(0);
                            }
                        }
                    }
                }
            }
        }
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
                        PaymentsPane.Visible = true;
                    }
                    else if (role == "Employee")
                    {
                        PaymentsPane.Visible = false;
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
            FillDayInfo();
            FillStoreInfo();
            FillPaymentInfo();
            FillInfo();
        }
    }


    protected void FillInfo()
    {
        if ((Request.QueryString["s"] != null))
        {
            if (!string.IsNullOrEmpty(Request.QueryString["s"].ToString()))
            {
                SubID = Request.QueryString["s"].ToString();
                week = Request.QueryString["week"].ToString();
                SqlDataReader myDataReader = default(SqlDataReader);
                SqlConnection mySqlConnection = default(SqlConnection);
                SqlCommand mySqlCommand = default(SqlCommand);
                mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                mySqlCommand = new SqlCommand("SELECT Firstname1, Firstname2, lastname1, lastname2, email1, email2, phone1, phone2, address, city, state, zip, allergies, newsletter, vacused, subscribers.bounty, subscribers.barnyard, subscribers.ploughman, subscribers.pickupday, subscribers.store, subscribers.notes, BountyNL, BarnyardNL, PloughmanNL FROM weekly INNER JOIN subscribers ON weekly.SubID=subscribers.SubId Where weekly.SubId= '" + SubID + "'", mySqlConnection);
                try
                {
                    SqlConnection cn = Constant.Connection();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.HomeDeliverySubscriber.*, dbo.Subscribers.* FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId WHERE dbo.HomeDeliverySubscriber.SubId='" + SubID + "'", cn);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        litDelivery.Text = "Delivery Status: " + ds.Tables[0].Rows[0]["Request"].ToString();
                    }
                    else
                    {
                        litDelivery.Text = "Delivery Status: None";
                    }

                    SqlDataAdapter da2 = new SqlDataAdapter("SELECT dbo.PurchaseProduct.*, dbo.PurchaseProductDetails.* FROM dbo.PurchaseProduct INNER JOIN dbo.PurchaseProductDetails ON dbo.PurchaseProduct.BuyID = dbo.PurchaseProductDetails.BuyId WHERE dbo.PurchaseProduct.SubscriberID='" + SubID + "' and dbo.PurchaseProduct.Week='" + week + "'", cn);
                    DataSet ds2 = new DataSet();
                    da2.Fill(ds2);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gvProducts.DataSource = ds2.Tables[0];
                        gvProducts.DataBind();
                    }
                    else
                    {
                        gvProducts.DataSource = null;
                        gvProducts.DataBind();
                    }


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
                        if (myDataReader.GetBoolean(15) == true)
                        {
                            BoxButton.Items.FindByText("Bounty - $35.00").Selected = true;
                        }
                        else
                        {
                            GridView1.Columns[2].Visible = false;
                        }
                        if (myDataReader.GetBoolean(16) == true)
                        {
                            BoxButton.Items.FindByText("Barnyard - $35.00").Selected = true;
                        }
                        else
                        {
                            GridView1.Columns[3].Visible = false;
                        }
                        if (myDataReader.GetBoolean(17) == true)
                        {
                            BoxButton.Items.FindByText("Ploughman - $35.00").Selected = true;
                        }
                        else
                        {
                            GridView1.Columns[4].Visible = false;
                        }
                        PickupDayList.SelectedValue = myDataReader.GetString(18);
                        StoreList.SelectedValue = myDataReader.GetString(19);
                        HeaderLiteral.Text = myDataReader.GetString(0) + " " + myDataReader.GetString(2) + "'s ";
                        if (!string.IsNullOrEmpty(myDataReader.GetString(20)))
                        {
                            Literal2.Text = myDataReader.GetString(20);
                        }
                        else
                        {
                            Literal2.Text = "No permanent notes for this subscriber";
                        }
                        BountyNL.Checked = myDataReader.GetBoolean(21);
                        BarnyardNL.Checked = myDataReader.GetBoolean(22);
                        PloughmanNL.Checked = myDataReader.GetBoolean(23);
                    }
                }
                catch (Exception ex)
                {
                    Literal1.Text = ex.Message + "<br />" + ex.StackTrace;
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
    protected void FillDayInfo()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PickupDay");
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
    //protected void FillPaymentInfo()
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
    //                        SDateRange = "and ((week>='" + myDataReader2.GetDateTime(0) + "' and week <= '" + myDataReader2.GetDateTime(1) + "') Or Week = '1/1/1900') ";
    //                    }
    //                }
    //                myDataReader2.Close();
    //            }
    //        }
    //    }
    //    finally
    //    {
    //    }
    //    SqlDataReader myDataReader = default(SqlDataReader);
    //    SqlConnection mySqlConnection = default(SqlConnection);
    //    SqlCommand mySqlCommand = default(SqlCommand);
    //    mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
    //    string SqlQuary = "SELECT SubId, Week, PaidBounty, PaidBarnyard, PaidPloughman FROM Weekly where subID='" + SubID + "' " + SDateRange + " ORDER BY [Week]";
    //    DataTable dt = new DataTable();
    //    dt.Columns.Add("SubId");
    //    dt.Columns.Add("Week");
    //    dt.Columns.Add("PaidBounty");
    //    dt.Columns.Add("PaidBarnyard");
    //    dt.Columns.Add("PaidPloughman");

    //    try
    //    {
    //        using (mySqlConnection)
    //        {
    //            mySqlCommand = new SqlCommand(SqlQuary, mySqlConnection);
    //            mySqlConnection.Open();
    //            myDataReader = mySqlCommand.ExecuteReader();
    //            if (myDataReader.HasRows)
    //            {
    //                string SubInfo = "";
    //                string paid = "";
    //                string pickedup = "";
    //                string vacation = "";
    //                while (myDataReader.Read())
    //                {
    //                    string week = (myDataReader.GetDateTime(1).Month.ToString() + "/" + myDataReader.GetDateTime(1).Day.ToString() + "-" + myDataReader.GetDateTime(1).AddDays(1).Day.ToString() + "/" + myDataReader.GetDateTime(1).Year.ToString());
    //                    if (week == "1/1-2/1900")
    //                    {
    //                        week = "Deposit";
    //                    }
    //                    dt.Rows.Add(myDataReader.GetInt32(0), week, myDataReader.GetBoolean(2), myDataReader.GetBoolean(3), myDataReader.GetBoolean(4));
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
    //    GridView1.DataSource = dt;
    //    GridView1.DataBind();
    //}

    //by Soham
    protected void FillPaymentInfo()
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
            Session["IsPaidCheckbox"] = ds4;
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
        int count = ds3.Tables[0].Rows.Count;

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


        //gvProduct.DataSource = dt2;
        //gvProduct.DataBind();
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
      
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
       
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

    protected void Button3_Click(object sender, EventArgs e)
    {
        
    }
    private void UpdMailChimp(string email, bool Bounty, bool Barnyard, bool Ploughman)
    {
        string webAddr = "";
        try
        {
            if (Bounty == true)
            {
                try
                {
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=f310b8a278&email[email]=" + email1.Text.Trim() + "&merge_vars[FNAME]=" + firstname1.Text.Trim() + "&merge_vars[LNAME]=" + lastname1.Text.Trim() + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim() + "&double_optin=false&send_welcome=false";
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
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/unsubscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=f310b8a278&email[email]=" + email1.Text.Trim() + "&merge_vars[FNAME]=" + firstname1.Text.Trim() + "&merge_vars[LNAME]=" + lastname1.Text.Trim() + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim() + "&double_optin=false&send_welcome=false";
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
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=1ad43508d8&email[email]=" + email1.Text.Trim() + "&merge_vars[FNAME]=" + firstname1.Text.Trim() + "&merge_vars[LNAME]=" + lastname1.Text.Trim() + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim() + "&double_optin=false&send_welcome=false";
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
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/unsubscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=1ad43508d8&email[email]=" + email1.Text.Trim() + "&merge_vars[FNAME]=" + firstname1.Text.Trim() + "&merge_vars[LNAME]=" + lastname1.Text.Trim() + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim() + "&double_optin=false&send_welcome=false";
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
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=078a386ef9&email[email]=" + email1.Text.Trim() + "&merge_vars[FNAME]=" + firstname1.Text.Trim() + "&merge_vars[LNAME]=" + lastname1.Text.Trim() + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim() + "&double_optin=false&send_welcome=false";
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
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/unsubscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=078a386ef9&email[email]=" + email1.Text.Trim() + "&merge_vars[FNAME]=" + firstname1.Text.Trim() + "&merge_vars[LNAME]=" + lastname1.Text.Trim() + "&merge_vars[MMERGE3]=" + PickupDayList.SelectedValue.Trim() + "&double_optin=false&send_welcome=false";
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
            Literal1.Text = "We're sorry, there seems to have been an error.";
            //Literal1.Text = ex.Message + "<br />" + ex.StackTrace + "<br />" + webAddr
        }

    }

    protected void PassResetButton_Click(object sender, EventArgs e)
    {
        try
        {
            Literal1.Text = "";
            if (!string.IsNullOrEmpty(TextBox3.Text))
            {
                SubID = Request.QueryString["s"].ToString();
                string username = "";
                SqlDataReader myDataReader = default(SqlDataReader);
                string query = "SELECT username FROM subscribers Where SubId=@SubId";

                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand())
                    {
                        var _with4 = comm;
                        _with4.Connection = conn;
                        _with4.CommandType = CommandType.Text;
                        _with4.CommandText = query;
                        comm.Parameters.Add("@SubID", SqlDbType.VarChar).Value = SubID;
                        conn.Open();
                        myDataReader = comm.ExecuteReader(CommandBehavior.CloseConnection);
                        while ((myDataReader.Read()))
                        {
                            if (!myDataReader.IsDBNull(0))
                            {
                                username = myDataReader.GetString(0);
                            }
                        }

                    }
                }
                MembershipUser myObject = Membership.GetUser(username);
                myObject.IsApproved = true;
                dynamic newpassword = TextBox3.Text.Trim();
                string generatedpassword = myObject.ResetPassword();
                myObject.ChangePassword(generatedpassword, newpassword);
                if (CheckBox1.Checked == true)
                {
                    //MailMessage oMail0 = new MailMessage();
                    //oMail0.From = new MailAddress("Root Cellar <website@rootcellarboxes.com>");
                    //oMail0.To.Add(new MailAddress(email1.Text));
                    //oMail0.Subject = "Root Cellar Password Reset ";
                    //oMail0.Priority = MailPriority.High;
                    //oMail0.IsBodyHtml = true;
                    //oMail0.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
                    string oMail0Body = "<head><title></title></head>";
                    oMail0Body += "<body>";
                    oMail0Body += "Hello " + firstname1.Text.Replace("'", "").Replace("\"", "").Replace(" ", "") + ",<br /><br />";
                    oMail0Body += "Our staff has reset your account password.<br /><br />You can now login to <a href='http://www.rootcellarboxes.com/login'>http://www.rootcellarboxes.com/login</a> using:<br />";
                    oMail0Body += "Username: " + username + "<br />";
                    oMail0Body += "Password: " + newpassword + "<br /><br />";
                    oMail0Body += "Please feel free to give us a call (573) 443-5055 or reply to this email if you have any additional questions.<br /><br /> ";
                    oMail0Body += "Root Cellar Team";
                    oMail0Body += "</body>";
                    oMail0Body += "</html>";
                    //AlternateView htmlView = AlternateView.CreateAlternateViewFromString(oMail0.Body, null, "text/html");
                    //oMail0.AlternateViews.Add(htmlView);
                    //SmtpClient smtpmail0 = new SmtpClient("relay-hosting.secureserver.net");
                    //;
                    //smtpmail0.UseDefaultCredentials = true;
                    //smtpmail0.Send(oMail0);
                    //oMail0 = null;
                    Constant.SendMail(email1.Text.Trim(), "Root Cellar Password Reset", oMail0Body);
                }
                Literal1.Text = "<h2>Subscriber's passsword has been reset!</h2>";
            }
        }
        catch (Exception ex)
        {
            //Literal1.Text = ex.Message + "<br />" + ex.StackTrace
        }
    }
    public bool ChangeUserName(string NewUserName, string OldUserName)
    {
        bool IsSuccsessful = false;
        SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        SqlCommand cmdChangeUserName = new SqlCommand();
        var _with5 = cmdChangeUserName;
        _with5.CommandText = "dbo.Membership_ChangeUserName";
        _with5.CommandType = System.Data.CommandType.StoredProcedure;
        _with5.Connection = myConn;
        _with5.Parameters.Add("@OldUserName", System.Data.SqlDbType.NVarChar);
        _with5.Parameters.Add("@NewUserName", System.Data.SqlDbType.NVarChar);
        cmdChangeUserName.Parameters["@OldUserName"].Value = OldUserName;
        cmdChangeUserName.Parameters["@NewUserName"].Value = NewUserName;
        try
        {
            myConn.Open();
            cmdChangeUserName.ExecuteNonQuery();
            myConn.Close();
            IsSuccsessful = true;
        }
        catch (Exception ex)
        {
            IsSuccsessful = false;
        }
        return IsSuccsessful;
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        string newUsername = TextBox1.Text.Trim();
        if (newUsername.Length < 5)
        {
            Literal1.Text = string.Format("The username {0} is too short. Please use at least 5 characters.", newUsername);
            return;
        }
        //Does this username already exist?
        MembershipUser usr = Membership.GetUser(newUsername);
        if (usr != null)
        {
            Literal1.Text = string.Format("The username {0} is already being used by someone else. Please try entering a different username.", newUsername);
            return;
        }
        string username = UsernameLiteral.Text;
        if (ChangeUserName(newUsername, username) == true)
        {
            UsernameLiteral.Text = newUsername;
            Literal1.Text = "<h2>Subscriber's username has been changed!</h2>";
            if (CheckBox2.Checked == true)
            {
                //MailMessage oMail0 = new MailMessage();
                //oMail0.From = new MailAddress("Root Cellar <website@rootcellarboxes.com>");
                //oMail0.To.Add(new MailAddress(email1.Text));
                //oMail0.Subject = "Root Cellar Password Reset ";
                //oMail0.Priority = MailPriority.High;
                //oMail0.IsBodyHtml = true;
                //oMail0.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
                string oMail0Body = "<head><title></title></head>";
                oMail0Body += "<head><title></title></head>";
                oMail0Body += "<body>";
                oMail0Body += "Hello " + firstname1.Text.Replace("'", "").Replace("\"", "").Replace(" ", "") + ",<br /><br />";
                oMail0Body += "Our staff has changed your account username.<br /><br />You can now login to <a href='http://www.rootcellarboxes.com/login'>http://www.rootcellarboxes.com/login</a> using:<br />";
                oMail0Body += "Username: " + newUsername + "<br />";
                oMail0Body += "Please feel free to give us a call (573) 443-5055 or reply to this email if you have any additional questions.<br /><br /> ";
                oMail0Body += "Root Cellar Team";
                oMail0Body += "</body>";
                oMail0Body += "</html>";
                //AlternateView htmlView = AlternateView.CreateAlternateViewFromString(oMail0.Body, null, "text/html");
                //oMail0.AlternateViews.Add(htmlView);
                //SmtpClient smtpmail0 = new SmtpClient("relay-hosting.secureserver.net");
                //;
                //smtpmail0.UseDefaultCredentials = true;
                //smtpmail0.Send(oMail0);
                //oMail0 = null;
                Constant.SendMail(email1.Text.Trim(), "Root Cellar Password Reset", oMail0Body);
            }
        }
        else
        {
            Literal1.Text = "<h2>There was a problem updating the subscriber's username.</h2>";
        }
    }

    protected void Button3_Click1(object sender, EventArgs e)
    {
        Literal1.Text = "";
        if ((Request.QueryString["s"] != null))
        {
            if (!string.IsNullOrEmpty(Request.QueryString["s"].ToString()))
            {
                SubID = Request.QueryString["s"].ToString();
                string query = "Update subscribers set pickupday=@pickupday, store=@store, bounty=@bounty, barnyard=@barnyard, ploughman=@ploughman, active=@active Where subId=@SubId";
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand())
                    {
                        var _with2 = comm;
                        _with2.Connection = conn;
                        _with2.CommandType = CommandType.Text;
                        _with2.CommandText = query;
                        comm.Parameters.Add("@pickupday", SqlDbType.Text).Value = PickupDayList.SelectedValue;
                        _with2.Parameters.Add("@store", SqlDbType.VarChar).Value = StoreList.SelectedValue;
                        _with2.Parameters.Add("@SubId", SqlDbType.Int).Value = SubID;
                        _with2.Parameters.Add("@bounty", SqlDbType.Bit).Value = BoxButton.Items.FindByText("Bounty - $35.00").Selected;
                        _with2.Parameters.Add("@barnyard", SqlDbType.Bit).Value = BoxButton.Items.FindByText("Barnyard - $35.00").Selected;
                        _with2.Parameters.Add("@ploughman", SqlDbType.Bit).Value = BoxButton.Items.FindByText("Ploughman - $35.00").Selected;
                        if (BoxButton.Items.FindByText("Bounty - $35.00").Selected == false & BoxButton.Items.FindByText("Barnyard - $35.00").Selected == false & BoxButton.Items.FindByText("Ploughman - $35.00").Selected == false)
                        {
                            _with2.Parameters.Add("@active", SqlDbType.Bit).Value = false;
                        }
                        else
                        {
                            _with2.Parameters.Add("@active", SqlDbType.Bit).Value = true;
                        }
                        try
                        {
                            conn.Open();
                            comm.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            Literal1.Text = "Were sorry, there was an error";
                        }
                    }
                }
                SqlConnection mySqlConnection = default(SqlConnection);
                mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                query = "Update Weekly set bounty=@bounty, barnyard=@barnyard, ploughman=@ploughman, PickupDay=@PickupDay, Location=@Location where SubId=@SubId ";
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand())
                    {
                        var _with3 = comm;
                        _with3.Connection = conn;
                        _with3.CommandType = CommandType.Text;
                        _with3.CommandText = query;
                        comm.Parameters.Add("@SubId", SqlDbType.Int).Value = SubID;
                        _with3.Parameters.Add("@bounty", SqlDbType.Bit).Value = BoxButton.Items.FindByText("Bounty - $35.00").Selected;
                        _with3.Parameters.Add("@barnyard", SqlDbType.Bit).Value = BoxButton.Items.FindByText("Barnyard - $35.00").Selected;
                        _with3.Parameters.Add("@ploughman", SqlDbType.Bit).Value = BoxButton.Items.FindByText("Ploughman - $35.00").Selected;

                        _with3.Parameters.Add("@PickupDay", SqlDbType.VarChar).Value = PickupDayList.SelectedValue;
                        _with3.Parameters.Add("@Location", SqlDbType.VarChar).Value = StoreList.SelectedValue;
                        try
                        {
                            conn.Open();
                            comm.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            Literal1.Text = "Were sorry, there was an error";
                        }
                    }
                }
                Literal1.Text = "<h2>Subscriber Updated!</h2>";
            }
        }
    }
    protected void Button1_Click1(object sender, EventArgs e)
    {
        Literal1.Text = "";
        if ((Request.QueryString["s"] != null))
        {
            if (!string.IsNullOrEmpty(Request.QueryString["s"].ToString()))
            {
                SubID = Request.QueryString["s"].ToString();
                string query = "Update subscribers set FirstName1=@FirstName1, LastName1=@LastName1, Email1=@Email1, phone1=@phone1, FirstName2=@FirstName2, LastName2=@LastName2, Email2=@Email2, phone2=@phone2, Address=@Address, City=@City, State=@State, Zip=@Zip, Allergies=@Allergies, BountyNL=@BountyNL, BarnyardNL=@BarnyardNL, PloughmanNL=@PloughmanNL Where subId=@SubId";
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
                        _with1.Parameters.Add("@SubId", SqlDbType.Int).Value = SubID;
                        _with1.Parameters.Add("@BountyNL", SqlDbType.Bit).Value = BountyNL.Checked;
                        _with1.Parameters.Add("@BarnyardNL", SqlDbType.Bit).Value = BarnyardNL.Checked;
                        _with1.Parameters.Add("@PloughmanNL", SqlDbType.Bit).Value = PloughmanNL.Checked;
                        try
                        {
                            conn.Open();
                            comm.ExecuteNonQuery();
                            Literal1.Text = "<h2>Subscriber Updated!</h2>";
                            UpdMailChimp(email1.Text, BountyNL.Checked, BarnyardNL.Checked, PloughmanNL.Checked);
                            if (!string.IsNullOrEmpty(email2.Text))
                            {
                                UpdMailChimp(email2.Text, BountyNL.Checked, BarnyardNL.Checked, PloughmanNL.Checked);
                            }
                        }
                        catch (SqlException ex)
                        {
                            Literal1.Text = "Were sorry, there was an error";
                        }
                    }
                }
            }
        }
    }
    protected void Button2_Click1(object sender, EventArgs e)
    {
        foreach (GridViewRow Weekrow in GridView1.Rows)
        {
            CheckBox BountyPaid = Weekrow.FindControl("BountyPaidCheck") as CheckBox;
            CheckBox BarnyardPaid = Weekrow.FindControl("BarnyardPaidCheck") as CheckBox;
            CheckBox PloughmanPaid = Weekrow.FindControl("PloughmanPaidCheck") as CheckBox;
            string week = Weekrow.Cells[1].Text;
            if (!(week == "Deposit"))
            {
                string pattern = "-(.*?)/";
                string replacement = "/" + "\r\n";
                Regex rgx = new Regex(pattern, RegexOptions.Singleline);
                week = rgx.Replace(week, replacement);
            }
            else
            {
                week = "1/1/1900";

            }

            week = (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "");
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string sql = "update weekly set PaidBounty='" + BountyPaid.Checked.ToString() + "', PaidBarnyard='" + BarnyardPaid.Checked.ToString() + "', PaidPloughman='" + PloughmanPaid.Checked.ToString() + "' where SubID='" + Request.QueryString["s"].ToString() + "' and week='" + week + "'";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            //if week == Deposit  and is true make sure that the subscribers table active filed is set to true...
            if (week == "1/1/1900") {
                if (BountyPaid.Checked == true || BarnyardPaid.Checked == true || PloughmanPaid.Checked == true) {
                    string sql1 = "update subscribers set active=-1 where SubID='" + Request.QueryString["s"].ToString()  + "'";
                    conn.Open();
                    SqlCommand cmd1 = new SqlCommand(sql1);
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Connection = conn;
                    cmd1.ExecuteNonQuery();
                    cmd1.Connection.Close();
                }
            }
        }
        Literal1.Text = "<h2>Payment Updated</h2>";
        FillInfo();

    }
    protected void OnCheckedChanged(Object sender, EventArgs e)
    {
       // tableUPanel.Update();
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
        if (days * 35 == 0)
        {
           // Price.Text = "$0.00";
        }
        else
        {
            //Price.Text = "" + (days * 35).ToString("C2");
        }

    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddtionalProduct")
        {
            //gvProducts.Visible = true;
            // string Currentweek = e.Row.Cells[1].Text;
           int SubId = int.Parse(Request.QueryString["s"]);
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
                gvProducts.Visible = false;
            }
            gvProducts.Visible = true;
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