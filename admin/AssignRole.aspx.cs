using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class admin_AssignRole : System.Web.UI.Page
{
    static string UserID = string.Empty;
    static bool Flag = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["Id"]!=null)
            {
                UserID = Request.QueryString["Id"].ToString();
                SqlConnection cn = Constant.Connection();
                SqlDataAdapter da = new SqlDataAdapter("Select * from AllowAccess where UserID='" + UserID + "'", cn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Flag = true;
                    if (ds.Tables[0].Rows[0]["CurrentSubscribers"].ToString() == "True")
                        cbCS.Checked = true;
                    if (ds.Tables[0].Rows[0]["CreateNewSubscriber"].ToString() == "True")
                        cbCNS.Checked = true;
                    if (ds.Tables[0].Rows[0]["SubscriberNotes"].ToString() == "True")
                        cbSN.Checked = true;
                    if (ds.Tables[0].Rows[0]["WeeklyPickups"].ToString() == "True")
                        cbWP.Checked = true;
                    if (ds.Tables[0].Rows[0]["WeeklySummary"].ToString() == "True")
                        cbWS.Checked = true;
                    if (ds.Tables[0].Rows[0]["Admin"].ToString() == "True")
                        cbAdmin.Checked = true;
                    if (ds.Tables[0].Rows[0]["ViewReport"].ToString() == "True")
                        cbVR.Checked = true;
                    if (ds.Tables[0].Rows[0]["AddProducts"].ToString() == "True")
                        cbAddProduct.Checked = true;
                    if (ds.Tables[0].Rows[0]["Delivery"].ToString() == "True")
                        cbDelivery.Checked = true;
                    if (ds.Tables[0].Rows[0]["DeliveryTime"].ToString() == "True")
                        cbDeliveryTime.Checked = true;
                }
            }
        }

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SqlConnection cn = Constant.Connection();
        if (Flag == false)
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand("Insert into AllowAccess values(@UserID,@CurrentSubscribers,@CreateNewSubscriber,@SubscriberNotes,@WeeklyPickups,@WeeklySummary,@Admin,@ViewReport,@AddProducts,@Delivery,@DeliveryTime)", cn);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            if (cbCS.Checked)
                cmd.Parameters.AddWithValue("@CurrentSubscribers", true);
            else
                cmd.Parameters.AddWithValue("@CurrentSubscribers", false);
            if (cbCNS.Checked)
                cmd.Parameters.AddWithValue("@CreateNewSubscriber", true);
            else
                cmd.Parameters.AddWithValue("@CreateNewSubscriber", false);

            if (cbSN.Checked)
                cmd.Parameters.AddWithValue("@SubscriberNotes", true);
            else
                cmd.Parameters.AddWithValue("@SubscriberNotes", false);
            if (cbWP.Checked)
                cmd.Parameters.AddWithValue("@WeeklyPickups", true);
            else
                cmd.Parameters.AddWithValue("@WeeklyPickups", false);
            if (cbWS.Checked)
                cmd.Parameters.AddWithValue("@WeeklySummary", true);
            else
                cmd.Parameters.AddWithValue("@WeeklySummary", false);
            if (cbAdmin.Checked)
                cmd.Parameters.AddWithValue("@Admin", true);
            else
                cmd.Parameters.AddWithValue("@Admin", false);
            if (cbVR.Checked)
                cmd.Parameters.AddWithValue("@ViewReport", true);
            else
                cmd.Parameters.AddWithValue("@ViewReport", false);
            
            if (cbAddProduct.Checked)
                cmd.Parameters.AddWithValue("@AddProducts", true);
            else
                cmd.Parameters.AddWithValue("@AddProducts", false);
            if (cbDelivery.Checked)
                cmd.Parameters.AddWithValue("@Delivery", true);
            else
                cmd.Parameters.AddWithValue("@Delivery", false);
            if (cbDeliveryTime.Checked)
                cmd.Parameters.AddWithValue("@DeliveryTime", true);
            else
                cmd.Parameters.AddWithValue("@DeliveryTime", false);


            cmd.ExecuteNonQuery();
            cn.Close();
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Save();", true);
        }
        else//Update
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand("update AllowAccess set CurrentSubscribers=@CurrentSubscribers,CreateNewSubscriber=@CreateNewSubscriber,SubscriberNotes=@SubscriberNotes,WeeklyPickups=@WeeklyPickups,WeeklySummary=@WeeklySummary,Admin=@Admin,ViewReport=@ViewReport,AddProducts=@AddProducts,Delivery=@Delivery,DeliveryTime=@DeliveryTime where UserId='" + UserID + "'", cn);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            if (cbCS.Checked)
                cmd.Parameters.AddWithValue("@CurrentSubscribers", true);
            else
                cmd.Parameters.AddWithValue("@CurrentSubscribers", false);
            if (cbCNS.Checked)
                cmd.Parameters.AddWithValue("@CreateNewSubscriber", true);
            else
                cmd.Parameters.AddWithValue("@CreateNewSubscriber", false);

            if (cbSN.Checked)
                cmd.Parameters.AddWithValue("@SubscriberNotes", true);
            else
                cmd.Parameters.AddWithValue("@SubscriberNotes", false);
            if (cbWP.Checked)
                cmd.Parameters.AddWithValue("@WeeklyPickups", true);
            else
                cmd.Parameters.AddWithValue("@WeeklyPickups", false);
            if (cbWS.Checked)
                cmd.Parameters.AddWithValue("@WeeklySummary", true);
            else
                cmd.Parameters.AddWithValue("@WeeklySummary", false);
            if (cbAdmin.Checked)
                cmd.Parameters.AddWithValue("@Admin", true);
            else
                cmd.Parameters.AddWithValue("@Admin", false);
            if (cbVR.Checked)
                cmd.Parameters.AddWithValue("@ViewReport", true);
            else
                cmd.Parameters.AddWithValue("@ViewReport", false);
            if (cbAddProduct.Checked)
                cmd.Parameters.AddWithValue("@AddProducts", true);
            else
                cmd.Parameters.AddWithValue("@AddProducts", false);
            if (cbDelivery.Checked)
                cmd.Parameters.AddWithValue("@Delivery", true);
            else
                cmd.Parameters.AddWithValue("@Delivery", false);
            if (cbDeliveryTime.Checked)
                cmd.Parameters.AddWithValue("@DeliveryTime", true);
            else
                cmd.Parameters.AddWithValue("@DeliveryTime", false);
            cmd.ExecuteNonQuery();
            cn.Close();
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Save();", true);
        }
    }
}