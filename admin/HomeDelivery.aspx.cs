using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Telerik.Web.UI;

public partial class admin_HomeDelivery : System.Web.UI.Page
{
    SqlConnection cn = Constant.Connection();
    static int HomeDeliveryId;
    static bool isEdit = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SqlConnection cn = Constant.Connection();
            SqlDataAdapter da2 = new SqlDataAdapter("SELECT * FROM AllowAccess where UserID='" + Session[Constant.UserID].ToString() + "'", cn);
            DataSet ds2 = new DataSet();
            da2.Fill(ds2);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                if (ds2.Tables[0].Rows[0]["DeliveryTime"].ToString() != "True")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Error();", true);
                    return;
                }

                else
                {
                    SqlDataAdapter da = new SqlDataAdapter("select * from Stores", cn);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            cblStores.Items.Add(ds.Tables[0].Rows[i]["Store"].ToString());
                        }
                    }
                    BindHomeDeliveryGridview();
                }
            }
        }

    }
    /// <summary>
    /// Bind Gridview
    /// </summary>
    private void BindHomeDeliveryGridview()
    {
        SqlDataAdapter da2 = new SqlDataAdapter("Select * from HomeDelivery", cn);
        DataSet ds2 = new DataSet();
        da2.Fill(ds2);
        if (ds2.Tables[0].Rows.Count > 0)
        {
            gvHomeDelivery.DataSource = ds2.Tables[0];
            gvHomeDelivery.DataBind();
        }
        else
        {

        }
    }
    /// <summary>
    /// Save Home Delivery data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool CheckList = false;
        foreach (ListItem item in cblStores.Items)
        {
            if (item.Selected)
            {
                CheckList = true;
                //        return;
            }
        }
        if (CheckList == false)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select atleast one store')", true);
            return;
        }
        if (isEdit == false)
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand("Insert into HomeDelivery values (@Location,@Day,@StartTime,@EndTime);SELECT CAST(scope_identity() AS int)", cn);
            cmd.Parameters.AddWithValue("@Location", txtLocation.Text.Trim());
            cmd.Parameters.AddWithValue("@Day", ddlDays.SelectedValue);
            cmd.Parameters.AddWithValue("@StartTime", rtpStartTime.SelectedTime);
            cmd.Parameters.AddWithValue("@EndTime", rtpEndTime.SelectedTime);
            HomeDeliveryId = (int)cmd.ExecuteScalar();
            cn.Close();
            cn.Open();
            foreach (ListItem item in cblStores.Items)
            {
                if (item.Selected)
                {
                    SqlCommand cmd2 = new SqlCommand("Insert into HomeDeliveryStore values (@HomeDeliveryId,@StoreName,@Available)", cn);
                    cmd2.Parameters.AddWithValue("@HomeDeliveryId", HomeDeliveryId);
                    cmd2.Parameters.AddWithValue("@StoreName", item.Value);
                    cmd2.Parameters.AddWithValue("@Available", true);
                    cmd2.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand cmd2 = new SqlCommand("Insert into HomeDeliveryStore values (@HomeDeliveryId,@StoreName,@Available)", cn);
                    cmd2.Parameters.AddWithValue("@HomeDeliveryId", HomeDeliveryId);
                    cmd2.Parameters.AddWithValue("@StoreName", item.Value);
                    cmd2.Parameters.AddWithValue("@Available", false);
                    cmd2.ExecuteNonQuery();
                }
            }
            cn.Close();
            BindHomeDeliveryGridview();
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Save();", true);
        }
        else//update
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand("update HomeDelivery set Location=@Location,Day=@Day,StartTime=@StartTime,EndTime=@EndTime where HomeDeliveryId='" + HomeDeliveryId + "'", cn);
            cmd.Parameters.AddWithValue("@Location", txtLocation.Text.Trim());
            cmd.Parameters.AddWithValue("@Day", ddlDays.SelectedValue);
            cmd.Parameters.AddWithValue("@StartTime", rtpStartTime.SelectedTime);
            cmd.Parameters.AddWithValue("@EndTime", rtpEndTime.SelectedTime);
            cmd.ExecuteReader();
            cn.Close();
            cn.Open();
            foreach (ListItem item in cblStores.Items)
            {
                SqlCommand cmd2 = new SqlCommand("Update HomeDeliveryStore set Available=@Available where HomeDeliveryId='" + HomeDeliveryId + "' and StoreName='" + item.Text + "'", cn);
                if (item.Selected)
                    cmd2.Parameters.AddWithValue("@Available", true);
                else
                    cmd2.Parameters.AddWithValue("@Available", false);

                cmd2.ExecuteNonQuery();
            }
            cn.Close();
            BindHomeDeliveryGridview();
            isEdit = false;
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Update();", true);
        }
    }
    /// <summary>
    /// Edit and Delete Delivery Data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvHomeDelivery_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete1")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            SqlConnection cn = Constant.Connection();
            SqlCommand cmd = new SqlCommand("Delete From HomeDelivery where HomeDeliveryId=" + index + "", cn);
            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
            BindHomeDeliveryGridview();
        }

        if (e.CommandName == "Edit1")
        {
            for (int i = cblStores.Items.Count - 1; i >= 0; i--)
            {
                cblStores.Items.RemoveAt(i);
            }
            int index = Convert.ToInt32(e.CommandArgument);
            SqlConnection cn = Constant.Connection();
            SqlDataAdapter da = new SqlDataAdapter("Select HomeDeliveryId,Location,Day,Convert(nvarchar(50),StartTime,100) as StartTime,Convert(nvarchar(50),EndTime,100) as EndTime From HomeDelivery where HomeDeliveryId=" + index + "", cn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtLocation.Text = ds.Tables[0].Rows[0]["Location"].ToString();
                ddlDays.SelectedValue = ds.Tables[0].Rows[0]["Day"].ToString();

                //DateTime st = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartTime"]);
                //TimeSpan tst = st.TimeOfDay;
                rtpStartTime.DbSelectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartTime"]).TimeOfDay;
                rtpEndTime.DbSelectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["EndTime"]).TimeOfDay;
                cn.Open();
                SqlCommand cmd = new SqlCommand("Select * From HomeDeliveryStore where HomeDeliveryId=" + index + "", cn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListItem item = new ListItem();
                    item.Text = dr["StoreName"].ToString();
                    item.Value = dr["StoreName"].ToString();
                    item.Selected = Convert.ToBoolean(dr["Available"]);
                    cblStores.Items.Add(item);
                }
                cn.Close();
            }
            isEdit = true;
            HomeDeliveryId = index;
        }
    }
}