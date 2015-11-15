
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;
using Telerik.Web.UI.Calendar.Utils;
using System.Text.RegularExpressions;
using System.Web.UI;

public partial class admin_Notes : System.Web.UI.Page
{
    private SqlConnection conn = null;
    string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    private SqlCommand cmd = null;

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
                if (ds.Tables[0].Rows[0]["SubscriberNotes"].ToString() == "False")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Error();", true);
                    return;
                }

                else
                {
                    WeekList.Visible = false;
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
                    NotesPanel.Visible = false;
                    FillSubscriberInfo();
                }
            }
        }        
    }

    protected void FillSubscriberInfo()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("SubID");
        dt.Columns.Add("Subscriber");
        dt.Rows.Add("0", " - Select a Subscriber - ");
        //Create Rows in DataTable
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        try
        {
            using (mySqlConnection)
            {
                mySqlCommand = new SqlCommand("SELECT Distinct weekly.SubID, subscribers.Lastname1, subscribers.Firstname1 FROM Weekly INNER JOIN subscribers ON weekly.SubID=subscribers.SubId where subscribers.active='true' order by subscribers.lastname1, subscribers.firstname1", mySqlConnection);
                mySqlConnection.Open();

                myDataReader = mySqlCommand.ExecuteReader();

                if (myDataReader.HasRows)
                {
                    int count = myDataReader.FieldCount;
                    string SubInfo = "";
                    while (myDataReader.Read())
                    {
                        SubInfo = myDataReader.GetString(1) + ", " + myDataReader.GetString(2);
                        dt.Rows.Add(myDataReader.GetInt32(0), SubInfo);
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
        this.SubscriberList.DataSource = dt;
        this.SubscriberList.DataTextField = "Subscriber";
        this.SubscriberList.DataValueField = "SubID";
        this.SubscriberList.DataBind();
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
                        dt.Rows.Add(myDataReader.GetDateTime(0).Month.ToString() + "/" + myDataReader.GetDateTime(0).Day.ToString() + "-" + myDataReader.GetDateTime(0).AddDays(1).Day.ToString() + "/" + myDataReader.GetDateTime(0).Year.ToString());
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

    }

    protected void FillNotes()
    {
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        string week = "";
        if (NoteType.Items.FindByText("Weekly").Selected == true)
        {
            week = WeekList.SelectedValue;
            string pattern = "-(.*?)/";
            string replacement = "/" + "\r\n";
            Regex rgx = new Regex(pattern, RegexOptions.Singleline);
            week = rgx.Replace(week, replacement);
            week = (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "");
            mySqlCommand = new SqlCommand("SELECT notes FROM weekly Where week='" + week + "' and SubId= '" + SubscriberList.SelectedValue + "'", mySqlConnection);
        }
        else
        {
            mySqlCommand = new SqlCommand("SELECT notes FROM subscribers Where SubId= '" + SubscriberList.SelectedValue + "'", mySqlConnection);
        }
        try
        {
            mySqlConnection.Open();
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            while ((myDataReader.Read()))
            {
                CurrNotesLiteral.Text = myDataReader.GetString(0);
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
    protected void SubscriberList_SelectedIndexChanged(object sender, EventArgs e)
    {
        Literal1.Text = "";
        if (!(SubscriberList.SelectedValue == "0"))
        {
            NotesPanel.Visible = true;
        }
        else
        {
            NotesPanel.Visible = false;
        }
        WeekList.Visible = false;
        NoteType.ClearSelection();
        FillNotes();
    }

    protected void NoteType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (NoteType.Items.FindByText("Weekly").Selected == true)
        {
            FillWeekInfo();
            WeekList.Visible = true;
            FillNotes();
        }
        else
        {
            WeekList.Visible = false;
            FillNotes();
        }
    }

    protected void submit_Click(object sender, EventArgs e)
    {
        string note = null;
        if (!string.IsNullOrEmpty(NoteBox.Text))
        {
            if (!string.IsNullOrEmpty(CurrNotesLiteral.Text))
            {
                note = CurrNotesLiteral.Text + "<br /><br />" + NoteBox.Text;
            }
            else
            {
                note = NoteBox.Text;
            }
            note = note + "<br />Added by " + Membership.GetUser().ToString() + " on " + System.DateTime.Now.ToShortDateString() + "<br /><br />";
            string query = "";
            string week = "";
            System.DateTime weekset = default(System.DateTime);

            if (NoteType.Items.FindByText("Weekly").Selected == true)
            {
                week = WeekList.SelectedValue;
                string pattern = "-(.*?)/";
                string replacement = "/" + "\r\n";
                Regex rgx = new Regex(pattern, RegexOptions.Singleline);
                week = rgx.Replace(week, replacement);
                week = (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "");
                weekset = DateTime.Parse(week);
                query = "Update weekly set notes=@note,WeekValue=@WeekValue WHERE week=@week and SubId=@subID";
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand())
                    {
                        var _with1 = comm;
                        _with1.Connection = conn;
                        _with1.CommandType = CommandType.Text;
                        _with1.CommandText = query;
                        comm.Parameters.Add("@note", SqlDbType.VarChar).Value = note;
                        comm.Parameters.AddWithValue("@WeekValue", WeekList.SelectedValue);
                        _with1.Parameters.Add("@week", SqlDbType.DateTime).Value = weekset;
                        _with1.Parameters.Add("@subID", SqlDbType.Int).Value = SubscriberList.SelectedValue;
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                query = "Update subscribers set notes=@note WHERE SubId=@subID";
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand())
                    {
                        var _with2 = comm;
                        _with2.Connection = conn;
                        _with2.CommandType = CommandType.Text;
                        _with2.CommandText = query;
                        comm.Parameters.Add("@note", SqlDbType.VarChar).Value = note;
                        _with2.Parameters.Add("@subID", SqlDbType.Int).Value = SubscriberList.SelectedValue;
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                }
            }

            Literal1.Text = "<h2>Subscriber's note has been added!</h2>";
            NotesPanel.Visible = false;
            NoteBox.Text = "";
            SubscriberList.ClearSelection();
            NoteType.ClearSelection();
            FillNotes();
        }
        else
        {
            Literal1.Text = "<h2>No new note was entered</h2>";
        }

    }

    protected void WeekList_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillNotes();
    }

}