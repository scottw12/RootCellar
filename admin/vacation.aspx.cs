
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.UI;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;
using System.Text.RegularExpressions;

partial class admin_vacation : RadAjaxPage
{
    private SqlConnection conn = null;
    private string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    static string SubID = string.Empty, VacationID = string.Empty;

    static bool isEditVacation = false;
    private SqlCommand cmd = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            FillWeekInfo();
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
            if (Request.QueryString["s"] != null)
            {
                SubID = Request.QueryString["s"].ToString();
                SqlConnection cn = Constant.Connection();
                SqlDataAdapter da = new SqlDataAdapter("Select * from VacationDetails where CustomerID='" + SubID + "'", cn);
                DataSet ds = new DataSet();
                da.Fill(ds);
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
                SqlDataAdapter da2 = new SqlDataAdapter("SELECT * FROM [dbo].[Subscribers] WHERE SubId='" + SubID + "'", cn);
                DataSet ds2 = new DataSet();
                da2.Fill(ds);
                name.Text = "Add Vacation For: " + ds.Tables[0].Rows[0]["FirstName1"].ToString() + " " + ds.Tables[0].Rows[0]["LastName1"].ToString();
            }



            //if (Request.QueryString["s"] != null)
            //{
            //    if (!string.IsNullOrEmpty(Request.QueryString["s"].ToString()))
            //    {
            //        FillWeekInfo();
            //        GetDetails();
            //    }
            //    else
            //    {
            //        Literal1.Text = "NO SUBSCRIBER SELECTED! CHANGES WILL NOT BE SAVED";
            //    }
            //}
            //else
            //{
            //    Literal1.Text = "NO SUBSCRIBER SELECTED! CHANGES WILL NOT BE SAVED";
            //}
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
                    //dt.Rows.RemoveAt(1);
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
    protected void FillWeekInfo1()
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
                mySqlCommand = new SqlCommand("SELECT DISTINCT Week FROM Weekly where subID= 4624 " + SDateRange + " ORDER BY [Week]", mySqlConnection);
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
   // protected void FillWeekInfo()
    //{
        //SqlDataReader myDataReader2 = default(SqlDataReader);
        //SqlConnection mySqlConnection2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //SqlCommand mySqlCommand2 = default(SqlCommand);
        //string SDateRange = "";
        //string query = "select Sstart, send from seasons where currents='true'";
        //try
        //{
        //    using (SqlConnection conn = new SqlConnection(ConnectionString))
        //    {
        //        using (mySqlConnection2)
        //        {
        //            mySqlCommand2 = new SqlCommand(query, mySqlConnection2);
        //            mySqlConnection2.Open();
        //            myDataReader2 = mySqlCommand2.ExecuteReader();
        //            if (myDataReader2.HasRows)
        //            {
        //                while (myDataReader2.Read())
        //                {
        //                    SDateRange = "where week>='" + myDataReader2.GetDateTime(0) + "' and week <= '" + myDataReader2.GetDateTime(1) + "' ";
        //                }
        //            }
        //            myDataReader2.Close();
        //        }
        //    }
        //}
        //finally
        //{
        //}
        //DataTable dt = new DataTable();
        //dt.Columns.Add("Week");
        //dt.Rows.Add(" - Select a Week - ");
        ////Create Rows in DataTable
        //SqlDataReader myDataReader = default(SqlDataReader);
        //SqlConnection mySqlConnection = default(SqlConnection);
        //SqlCommand mySqlCommand = default(SqlCommand);
        //mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //try
        //{
        //    using (mySqlConnection)
        //    {
        //        mySqlCommand = new SqlCommand("SELECT DISTINCT Week FROM weekly " + SDateRange + " order by week", mySqlConnection);
        //        mySqlConnection.Open();

        //        myDataReader = mySqlCommand.ExecuteReader();

        //        if (myDataReader.HasRows)
        //        {
        //            while (myDataReader.Read())
        //            {
        //                //dt.Rows.Add(myDataReader.GetDateTime(0).ToString().Replace(" 12:00:00 AM", ""))
        //                dt.Rows.Add(myDataReader.GetDateTime(0).Month.ToString() + "/" + myDataReader.GetDateTime(0).Day.ToString() + "-" + myDataReader.GetDateTime(0).AddDays(1).Day.ToString() + "/" + myDataReader.GetDateTime(0).Year.ToString());
        //                if (myDataReader.GetDateTime(0) == System.DateTime.Today | myDataReader.GetDateTime(0) == System.DateTime.Today.AddDays(6))
        //                {
        //                    WeekList.SelectedValue = myDataReader.GetDateTime(0).Month.ToString() + "/" + myDataReader.GetDateTime(0).Day.ToString() + "-" + myDataReader.GetDateTime(0).AddDays(1).Day.ToString() + "/" + myDataReader.GetDateTime(0).Year.ToString();
        //                    GetDetails();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("No rows found.");
        //        }
        //        myDataReader.Close();
        //    }
        //}
        //finally
        //{
        //}
        //this.WeekList.DataSource = dt;
        //this.WeekList.DataTextField = "Week";
        //this.WeekList.DataValueField = "Week";
        //this.WeekList.DataBind();

   //}
    private void UpdDB()
    {
        //try
        //{
        //    SqlDataReader myDataReader = default(SqlDataReader);
        //    SqlConnection mySqlConnection = default(SqlConnection);
        //    SqlCommand mySqlCommand = default(SqlCommand);
        //    mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //    mySqlCommand = new SqlCommand("SELECT vacused FROM subscribers Where SubID= '" + Request.QueryString["s"].ToString() + "'", mySqlConnection);
        //    string sql = "";
        //    string sql2 = "";
        //    DateTime SelDate = System.DateTime.Now;
        //    if (!(WeekList.SelectedValue == " - Select a Week - "))
        //    {
        //        string week = null;
        //        week = WeekList.SelectedValue;
        //        string pattern = "-(.*?)/";
        //        string replacement = "/" + "\r\n";
        //        Regex rgx = new Regex(pattern, RegexOptions.Singleline);
        //        week = rgx.Replace(week, replacement);
        //        week = (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "");
        //        try
        //        {
        //            mySqlConnection.Open();
        //            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
        //            while ((myDataReader.Read()))
        //            {
        //                sql = "update subscribers set vacused='" + (myDataReader.GetInt32(0) + 1).ToString() + "' where subid='" + Request.QueryString["s"].ToString() + "'";
        //                sql2 = "update weekly set vacation='true' where subid='" + Request.QueryString["s"].ToString() + "' and week='" + week + "'";
        //            }
        //        }
        //        finally
        //        {
        //            if ((mySqlConnection.State == ConnectionState.Open))
        //            {
        //                mySqlConnection.Close();
        //            }
        //        }
        //        conn = new SqlConnection(ConnectionString);
        //        conn.Open();
        //        cmd = new SqlCommand(sql, conn);
        //        cmd.ExecuteNonQuery();
        //        cmd.Connection.Close();

        //        conn = new SqlConnection(ConnectionString);
        //        conn.Open();
        //        cmd = new SqlCommand(sql2, conn);
        //        cmd.ExecuteNonQuery();
        //        cmd.Connection.Close();
        //        calpanel.Visible = false;
        //        string littxt = "";
        //        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //        mySqlCommand = new SqlCommand("SELECT Firstname1, Lastname1 FROM subscribers Where SubID= '" + Request.QueryString["s"] + "'", mySqlConnection);
        //        try
        //        {
        //            mySqlConnection.Open();
        //            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
        //            while ((myDataReader.Read()))
        //            {
        //                littxt = "<h2>Vacation set for " + myDataReader.GetString(0) + " " + myDataReader.GetString(1) + " on " + week + "</h2>";
        //            }
        //        }
        //        finally
        //        {
        //            if ((mySqlConnection.State == ConnectionState.Open))
        //            {
        //                mySqlConnection.Close();
        //            }
        //        }
        //        name.Text = littxt;
        //    }
        //    else
        //    {
        //        name.Text = "Please select a week";
        //    }

        //}
        //catch (Exception ex)
        //{
        //    Literal1.Text = "Were sorry, there was an error";
        //}
    }

    public void GetDetails()
    {
        //string littxt = "";
        //SqlDataReader myDataReader = default(SqlDataReader);
        //SqlConnection mySqlConnection = default(SqlConnection);
        //SqlCommand mySqlCommand = default(SqlCommand);
        //mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //mySqlCommand = new SqlCommand("SELECT Firstname1, Lastname1, vacused FROM subscribers Where SubID= '" + Request.QueryString["s"] + "'", mySqlConnection);
        //try
        //{
        //    mySqlConnection.Open();
        //    myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
        //    while ((myDataReader.Read()))
        //    {
        //        if (myDataReader.GetInt32(2) == 0)
        //        {
        //            littxt = "Setting Vacation dates for " + myDataReader.GetString(0) + " " + myDataReader.GetString(1) + "<br />" + myDataReader.GetString(0) + " has not used any vacation weeks";
        //        }
        //        else if (myDataReader.GetInt32(2) == 1)
        //        {
        //            littxt = "Setting Vacation dates for " + myDataReader.GetString(0) + " " + myDataReader.GetString(1) + "<br />" + myDataReader.GetString(0) + " has used 1 vacation week";
        //        }
        //        else if (myDataReader.GetInt32(2) > 1)
        //        {
        //            littxt = "Setting Vacation dates for " + myDataReader.GetString(0) + " " + myDataReader.GetString(1) + "<br />" + myDataReader.GetString(0) + " has already used " + myDataReader.GetInt32(2).ToString() + " vacation weeks!";
        //            calpanel.Visible = false;
        //            if ((Request.QueryString["F"] != null))
        //            {
        //                if (!string.IsNullOrEmpty(Request.QueryString["F"].ToString()))
        //                {
        //                    if (Request.QueryString["F"] == "Y" & Session["Role"] == "Admin")
        //                    {
        //                        calpanel.Visible = true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //finally
        //{
        //    if ((mySqlConnection.State == ConnectionState.Open))
        //    {
        //        mySqlConnection.Close();
        //    }
        //}
        //if (string.IsNullOrEmpty(littxt))
        //{
        //    name.Text = "NO SUBSCRIBER SELECTED! CHANGES WILL NOT BE SAVED";
        //}
        //else
        //{
        //    name.Text = littxt;
        //}
    }

    protected void Submit_Click(object sender, EventArgs e)
    {
        //UpdDB();
    }
    public admin_vacation()
    {
        //Load += Page_Load;
    }
    /// <summary>
    /// Add New Vacation for curresnt user
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddNewVacation_Click(object sender, EventArgs e)
    {
        SqlConnection cn = Constant.Connection();
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM dbo.Userinfo WHERE UserId='" + Session[Constant.UserID] + "'", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);

        if (WeekList.SelectedItem.Text==" - Select a Week - ")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Select Week')", true);
            return;
        }
        if (isEditVacation == false)
        {
            
            cn.Open();
            SqlCommand cmd = new SqlCommand("Insert into VacationDetails values(@CustomerID,@VacationWeek,@VacationAddedDate,@VacationAddedBy)", cn);
            cmd.Parameters.AddWithValue("@CustomerID", SubID);
            cmd.Parameters.AddWithValue("@VacationWeek", WeekList.SelectedValue);
            cmd.Parameters.AddWithValue("@VacationAddedDate", DateTime.Now.ToShortDateString());
            cmd.Parameters.AddWithValue("@VacationAddedBy", ds.Tables[0].Rows[0]["Username"].ToString());
            cmd.ExecuteNonQuery();
           
            BindDetatils(cn);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Vacation Added Successfully For Current Subscriber')", true);
 string week = null;
                    week = WeekList.SelectedValue;
                    string pattern = "-(.*?)/";
                    string replacement = "/" + "\r\n";
                    Regex rgx = new Regex(pattern, RegexOptions.Singleline);
                    week = rgx.Replace(week, replacement);
                    week = (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "");
            SqlCommand cmd2 = new SqlCommand("update weekly set vacation='true' where subid='" + SubID + "' and week='" + week + "'",cn);
            cmd2.ExecuteNonQuery();
            cn.Close();
        }
        else
        {
            //update vacation           
            cn.Open();
            SqlCommand cmd = new SqlCommand("Update VacationDetails set VacationWeek=@VacationWeek,VacationAddedDate=@VacationAddedDate,VacationAddedBy=@VacationAddedBy Where VID=" + VacationID + "", cn);            
            cmd.Parameters.AddWithValue("@VacationWeek", WeekList.SelectedValue);
            cmd.Parameters.AddWithValue("@VacationAddedDate", DateTime.Now.ToShortDateString());
            cmd.Parameters.AddWithValue("@VacationAddedBy", ds.Tables[0].Rows[0]["Username"].ToString());
            cmd.ExecuteNonQuery();

            cn.Close();
            BindDetatils(cn);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Vacation Updated Successfully')", true);
            //ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Update();", true);
            //NotificationForUpdate();
            isEditVacation = false;
        }
    }
    /// <summary>
    /// Bind Details of user
    /// </summary>
    /// <param name="cn"></param>
    private void BindDetatils(SqlConnection cn)
    {
        SqlDataAdapter da = new SqlDataAdapter("Select * from VacationDetails where CustomerID='" + SubID + "'", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVacation.DataSource = ds.Tables[0];
            gvVacation.DataBind();
        }
    }
    protected void gvVacation_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete1")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            SqlConnection cn = Constant.Connection();
            SqlCommand cmd = new SqlCommand("Delete From VacationDetails where VID=" + index + "", cn);
            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
            BindDetatils(cn);
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
                isEditVacation = true;
            }
            VacationID = index.ToString();
        }
    }
}
