using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class admin_HomeDeliveryViewRecord : System.Web.UI.Page
{
    static string HomeDeliveryRecordID = string.Empty;
    SqlConnection cn = Constant.Connection();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdown();
            if (Request.QueryString["HomeDeliveryRecordID"] != null)
            {
                LoadDeliveryData();
            }
        }

    }
    /// <summary>
    /// Load Delivery Data of Subscriber
    /// </summary>
    private void LoadDeliveryData()
    {
        //HomeDeliveryRecordID = EncryptDecrypt.DecryptPassword(Request.QueryString["HomeDeliveryRecordID"].ToString());
        HomeDeliveryRecordID = Request.QueryString["HomeDeliveryRecordID"].ToString();
        SqlDataAdapter da = new SqlDataAdapter("Select * from HomeDeliverySubscriber where HomeDeliveryRecordID='" + HomeDeliveryRecordID + "'", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlBestTime.SelectedValue = ds.Tables[0].Rows[0]["BestTime"].ToString();
            lblRequest.Text = ds.Tables[0].Rows[0]["Request"].ToString();
            lblBestTime.Text = ddlBestTime.SelectedItem.Text;
            lblSelectdLocation.Text = ds.Tables[0].Rows[0]["Location"].ToString();
            txtDeliveryAddress.Text = ds.Tables[0].Rows[0]["DeliveryAddress"].ToString();
            txtSpeInstr.Text = ds.Tables[0].Rows[0]["SpecialInstruction"].ToString();
            txtCharges.Text = ds.Tables[0].Rows[0]["Charges"].ToString();
        }
    }
    /// <summary>
    /// Load Best Time  DropDownList
    /// </summary>
    private void LoadDropdown()
    {
        SqlConnection cn2 = Constant.Connection();
        SqlDataAdapter da2 = new SqlDataAdapter("select HomeDeliveryId, Convert(nvarchar(50),[Day])+'-'+Convert(nvarchar(50),[StartTime],100)+'-'+Convert(nvarchar(50),[EndTime],100) AS Date FROM HomeDelivery", cn2);
        DataSet ds2 = new DataSet();
        da2.Fill(ds2);
        ddlBestTime.DataSource = ds2.Tables[0];
        ddlBestTime.DataTextField = "Date";
        ddlBestTime.DataValueField = "HomeDeliveryId";
        ddlBestTime.DataBind();
    }
    /// <summary>
    /// Request Approved and Send Mail
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        cn.Open();
        //SqlCommand cmd = new SqlCommand("Update HomeDeliverySubscriber set Request='Approved',LPDay=@LPDay,LPStatTime=@LPStatTime,LPEndTime=@LPEndTime, Charges=@Charges where HomeDeliveryRecordID=" + HomeDeliveryRecordID + "", cn);
        SqlCommand cmd = new SqlCommand("Update HomeDeliverySubscriber set Request='Approved',Charges=@Charges where HomeDeliveryRecordID=" + HomeDeliveryRecordID + "", cn);
        //cmd.Parameters.AddWithValue("@LPDay",ddlStartDay.SelectedValue);
        //cmd.Parameters.AddWithValue("@LPStatTime",rtpStartTime.SelectedTime);
        //cmd.Parameters.AddWithValue("@LPEndTime",rtpEndTime.SelectedTime);
        cmd.Parameters.AddWithValue("@Charges",txtCharges.Text);
        cmd.ExecuteNonQuery();
        //string body = "Approved";
        Constant.SendMail(Constant.AdminMailId, "Approved Mail", txtBody.Text);
        ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Save();", true);
        
    }
    protected void btnDeny_Click(object sender, EventArgs e)
    {
        cn.Open();
        SqlCommand cmd = new SqlCommand("Update HomeDeliverySubscriber set Request='Denied' where HomeDeliveryRecordID=" + HomeDeliveryRecordID + "", cn);
        //cmd.Parameters.AddWithValue("@LPDay", ddlStartDay.SelectedValue);
        //cmd.Parameters.AddWithValue("@LPStatTime", rtpStartTime.SelectedTime);
        //cmd.Parameters.AddWithValue("@LPEndTime", rtpEndTime.SelectedTime);
        Constant.SendMail(Constant.AdminMailId, "Denied Mail", txtBody.Text);
        cmd.ExecuteNonQuery();
        ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Update();", true);
        
    }
}