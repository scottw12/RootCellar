using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class admin_HomeDeliveryPendingRequest : System.Web.UI.Page
{
    SqlConnection cn = Constant.Connection();
    static int HomeDeliveryRecordID;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SqlConnection cn = Constant.Connection();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM AllowAccess where UserID='" + Session[Constant.UserID].ToString() + "'", cn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Delivery"].ToString() != "True")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Error();", true);
                    return;
                }

                else
                {
                    BindHomeDeliveryDetails();
                }
            }

        }
    }
    /// <summary>
    /// Home Delivery Details
    /// </summary>
    private void BindHomeDeliveryDetails()
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.Subscribers.Username, dbo.Subscribers.SubId, dbo.HomeDeliverySubscriber.HomeDeliveryRecordID, dbo.HomeDeliverySubscriber.Request FROM dbo.Subscribers INNER JOIN dbo.HomeDeliverySubscriber ON dbo.Subscribers.SubId = dbo.HomeDeliverySubscriber.SubId", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {   
            gvHomeDelivery.DataSource = ds.Tables[0];
            gvHomeDelivery.DataBind();
        }
    }
    /// <summary>
    /// Approved Or Denied
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvHomeDelivery_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //if (e.CommandName == "Delete1")//Denied
        //{
        //    int index = Convert.ToInt32(e.CommandArgument);
        //    SqlConnection cn = Constant.Connection();
        //    cn.Open();
        //    SqlCommand cmd = new SqlCommand("Update HomeDeliverySubscriber set Request='Denied' where HomeDeliveryRecordID=" + index + "", cn);
        //    cmd.ExecuteNonQuery();
        //    BindHomeDeliveryDetails();
        //}

        if (e.CommandName == "Edit1")//Approved Request
        {
            int index = Convert.ToInt32(e.CommandArgument);
            SqlConnection cn = Constant.Connection();
            cn.Open();
            //SqlCommand cmd = new SqlCommand("Update HomeDeliverySubscriber set Request='Approved' where HomeDeliveryRecordID=" + index + "", cn);            
            //cmd.ExecuteNonQuery();
            //BindHomeDeliveryDetails();
            //string HomeDeliveryRecordID = EncryptDecrypt.EncryptPassword();
            Response.Redirect("HomeDeliveryViewRecord.aspx?HomeDeliveryRecordID=" + index.ToString());
        }
    }
}