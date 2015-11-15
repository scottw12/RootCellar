using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using Telerik.Web.UI;
using System.Configuration;

public partial class account_ManagePickup : System.Web.UI.Page
{
    static int SubId;
    public SqlConnection cn = Constant.Connection();
    string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillWeekInfoVacation();
            FillDayInfo();
            BindDropdown();
            DataSet ds = Constant.Store();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlStore.DataSource = ds.Tables[0];
                ddlStore.DataTextField = "store";
                ddlStore.DataValueField = "store";
                ddlStore.DataBind();
            }
        }
    }

    private void BindDropdown()
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers where Username='" + Membership.GetUser().ToString() + "'", cn);
        DataSet ds2 = new DataSet();
        da.Fill(ds2);

        SqlDataAdapter da2 = new SqlDataAdapter("SELECT * FROM PickupChange WHERE CustomerID='" + ds2.Tables[0].Rows[0]["SubId"].ToString() + "'", cn);
        DataSet ds3 = new DataSet();
        da2.Fill(ds3);
        if (ds3.Tables[0].Rows.Count > 0)
        {
            gvPickupChange.DataSource = ds3.Tables[0];
            gvPickupChange.DataBind();
        }
        else
        {
            gvPickupChange.DataSource = null;
            gvPickupChange.DataBind();
        }
    }
    /// <summary>
    /// Week List
    /// </summary>
    protected void FillWeekInfoVacation()
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

        this.ddlWeek.DataSource = dt;
        this.ddlWeek.DataTextField = "Week";
        this.ddlWeek.DataValueField = "Week";
        this.ddlWeek.DataBind();

    }
    /// <summary>
    /// Pickup Day 
    /// </summary>
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
        PickupDayList.DataSource = dt;
        PickupDayList.DataTextField = "PickupDay";
        PickupDayList.DataValueField = "PickupDay";
        PickupDayList.DataBind();
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (PickupDayList.SelectedValue==" - Select a Pickup Day - ")
        {
            ScriptManager.RegisterClientScriptBlock(this,this.GetType(),"al", "alert('Please select pickupday')", true);
            return;
        }
        if (ddlWeek.SelectedValue == " - Select a Week - ")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "al", "alert('Please select Week')", true);
            return;
        }
        //DateTime SelectedDate = Convert.ToDateTime(DatePicker.SelectedDate);

        //if (SelectedDate.DayOfWeek.ToString() != "Thursday" && SelectedDate.DayOfWeek.ToString() != "Friday")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "al", "alert('Please Select Only Thursday\\'s or Friday\\'s Date')", true);
        //    DatePicker.SelectedDate = null;
        //    return;
        //}
        
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers where Username='" + Membership.GetUser().ToString() + "'", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);

        SqlDataAdapter da2 = new SqlDataAdapter("SELECT PickupDate FROM PickupChange where CustomerID='" + ds.Tables[0].Rows[0]["SubId"].ToString() + "'", cn);
        DataSet ds2 = new DataSet();
        da2.Fill(ds2);
        for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
        {
            if (ddlWeek.SelectedValue.ToString() == ds2.Tables[0].Rows[i][0].ToString())
            {
                ScriptManager.RegisterStartupScript(this,this.GetType(),"dup","alert('Duplicate rorecords not allowed')",true);
                return;
            }
           
        }
            //if (ds.Tables[0].Rows.Count>0)
            //{
            //    //DatePicker.SelectedDate=ds.ta
            //}
            cn.Open();
        SqlCommand cmd2 = new SqlCommand("Insert into PickupChange values(@CustomerID,@PickupDate,@PickupPoint,@PickupDay)", cn);
        cmd2.Parameters.AddWithValue("@CustomerID", ds.Tables[0].Rows[0]["SubId"].ToString());
        cmd2.Parameters.AddWithValue("@PickupDate", ddlWeek.SelectedValue);
        cmd2.Parameters.AddWithValue("@PickupPoint", ddlStore.SelectedValue);
        cmd2.Parameters.AddWithValue("@PickupDay", PickupDayList.SelectedValue);
        cmd2.ExecuteNonQuery();

        cn.Close();
        if (cbPermanent.Checked)
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand("update Subscribers set Store=@Store,PickupDay=@PickupDay Where SubId=@SubId", cn);
            cmd.Parameters.AddWithValue("@Store", ddlStore.SelectedValue);
            cmd.Parameters.AddWithValue("@SubId", ds.Tables[0].Rows[0]["SubId"].ToString());
            cmd.Parameters.AddWithValue("@PickupDay", PickupDayList.SelectedValue);
            cmd.ExecuteNonQuery();
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Update();", true);
        }
        else
        {
            
            string BodyForAdmin = "Respected Admin, Customer changes his pickup point. Please check details";
            string BodyForEmployee = "Respected Sir, Customer changes his pickup point";
            string BodyForCustomer = "Respected Sir, you changed pickup point. New pickup point will be'" + ddlStore.SelectedValue + "'";
            //Constant.SendMail(Constant.AdminMailId, "New Vacation Added", BodyForAdmin);
            //Constant.SendMail(ds.Tables[0].Rows[0]["Email"].ToString(), "New Vacation Added", BodyForEmployee);
            //Constant.SendMail("Sonalic@custom-soft.com", "Pickup Point Changed", BodyForCustomer);
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Save();", true);

        }
    }
    /// <summary>
    /// Delete Record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvPickupChange_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete1")
        {
            string SbId = e.CommandArgument.ToString();
            SqlCommand cmd = new SqlCommand("DELETE FROM PickupChange WHERE PickupID='" + SbId + "'", cn);
            cn.Open();
            cmd.ExecuteNonQuery();
            BindDropdown();
        }
    }
}