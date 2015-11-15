using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;


public partial class admin_NPUs : System.Web.UI.Page
{
    string Options = "";

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
            SqlDataSource1.SelectCommand = "SELECT [LastName1], [FirstName1], [phone1], [Bounty], [BarnYard], [Ploughman], [PickupDay], [Store] FROM [Subscribers] Where Active='true' ORDER BY [LastName1], [FirstName1]";
        }

    }

    protected void StoreList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(StoreList.SelectedValue))
        {
            if (!string.IsNullOrEmpty(PickupDay.SelectedValue))
            {
                Options = "and (Store = '" + StoreList.SelectedValue + "') and (PickupDay = '" + PickupDay.SelectedValue + "')";
            }
            else
            {
                Options = "and (Store = '" + StoreList.SelectedValue + "')";
            }
        }
        else if (!string.IsNullOrEmpty(PickupDay.SelectedValue))
        {
            Options = "and (PickupDay = '" + PickupDay.SelectedValue + "')";
        }
        LoadGrid();
    }

    protected void PickupDay_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(PickupDay.SelectedValue))
        {
            if (!string.IsNullOrEmpty(StoreList.SelectedValue))
            {
                Options = "and (PickupDay = '" + PickupDay.SelectedValue + "') and (Store = '" + StoreList.SelectedValue + "')";
            }
            else
            {
                Options = "and (PickupDay = '" + PickupDay.SelectedValue + "')";
            }
        }
        else if (!string.IsNullOrEmpty(StoreList.SelectedValue))
        {
            Options = "and (Store = '" + StoreList.SelectedValue + "')";
        }
        LoadGrid();
    }

    protected void LoadGrid()
    {
        SqlDataSource1.SelectCommand = "SELECT [LastName1], [FirstName1], [phone1], [Bounty], [BarnYard], [Ploughman], [PickupDay], [Store] FROM Subscribers WHERE (Active = 'True') " + Options + " ORDER BY [LastName1], [FirstName1]";
        GridView1.DataBind();
    }
}