using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class customer_Vacation : System.Web.UI.Page
{
    static string VacationID = string.Empty;
    static string UserID = string.Empty, CustomerEmail = string.Empty, Address = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        UserID = Session[Constant.UserID].ToString();
        if (!IsPostBack)
        {
            RadDatePicker1.MinDate = DateTime.Today.Date;            
            txtContactNumber.Text = string.Empty;
            txtDeliveryBoy.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtVacation.Text = string.Empty;
            RadDatePicker1.SelectedDate = null;
            if (Request.QueryString["VID"]!=null)
            {
                LoadDetails();
            }
        }
    }
    /// <summary>
    /// Load Details Of Users
    /// </summary>
    private void LoadDetails()
    {
        VacationID = EncryptDecrypt.DecryptPassword(Request.QueryString["VID"]);
        SqlConnection cn = Constant.Connection();
        SqlDataAdapter da = new SqlDataAdapter("Select * from VacationDetails where VID=" + VacationID + "", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtVacation.Text = ds.Tables[0].Rows[0]["Vacation"].ToString();
            RadDatePicker1.SelectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["VacationDate"]);
            txtDeliveryBoy.Text = ds.Tables[0].Rows[0]["DeliveryBoy"].ToString();
            ddlAddress.SelectedValue = ds.Tables[0].Rows[0]["Address"].ToString();
            txtContactNumber.Text = ds.Tables[0].Rows[0]["ContactNumber"].ToString();
            txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
        }
    }
    /// <summary>
    /// Add Vacation Details To Database
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddVacation_Click(object sender, EventArgs e)
    {
        try
        {
            Address = ddlAddress.SelectedValue;
            if (Request.QueryString["VID"] == null)
            {
                //Save New Vacation
                
                CustomerEmail= txtEmail.Text.Trim();
                SqlConnection cn = Constant.Connection();
                cn.Open();
                SqlCommand cmd = new SqlCommand("Insert Into VacationDetails Values (@Vacation,@VacationDate,@DeliveryBoy,@Address,@ContactNumber,@Email,@CustomerID)", cn);
                cmd.Parameters.AddWithValue("@Vacation", txtVacation.Text.Trim());
                cmd.Parameters.AddWithValue("@VacationDate", RadDatePicker1.DbSelectedDate);
                cmd.Parameters.AddWithValue("@DeliveryBoy", txtDeliveryBoy.Text.Trim());
                cmd.Parameters.AddWithValue("@Address", ddlAddress.SelectedValue);
                cmd.Parameters.AddWithValue("@ContactNumber", txtContactNumber.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@CustomerID", Session[Constant.UserID].ToString());
                cmd.ExecuteNonQuery();
                cn.Close();
                Notification();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Save();", true);
            }
            else
            {
                //Update Existing Record

                CustomerEmail = txtEmail.Text.Trim();
                SqlConnection cn = Constant.Connection();
                cn.Open();
                SqlCommand cmd = new SqlCommand("Update VacationDetails set Vacation=@Vacation, VacationDate=@VacationDate, DeliveryBoy=@DeliveryBoy, Address=@Address, ContactNumber=@ContactNumber, Email=@Email, CustomerID=@CustomerID Where VID=" + VacationID + "", cn);
                cmd.Parameters.AddWithValue("@Vacation", txtVacation.Text.Trim());
                cmd.Parameters.AddWithValue("@VacationDate", RadDatePicker1.DbSelectedDate);
                cmd.Parameters.AddWithValue("@DeliveryBoy", txtDeliveryBoy.Text.Trim());
                cmd.Parameters.AddWithValue("@Address", ddlAddress.SelectedValue);
                cmd.Parameters.AddWithValue("@ContactNumber", txtContactNumber.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@CustomerID", Session[Constant.UserID].ToString());
                cmd.ExecuteNonQuery();
                NotificationForUpdate();
                cn.Close();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Update();", true);
            }
        }
        catch (Exception err)
        {

        }
        
    }
    /// <summary>
    /// Mail Notification To Cusomer,Admin and Employee
    /// </summary>
    private static void Notification()
    {
        SqlConnection cn = Constant.Connection();       

        SqlDataAdapter da = new SqlDataAdapter("Select * from Userinfo where UserId='" + UserID + "'", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        
        string BodyForAdmin = "Respected Admin, New vacation will be added. Please check details";
        string BodyForEmployee = "Respected Sir, You are added new vacation";
        string BodyForCustomer = "Respected Sir, Vacation will be added and you have to pickup the bucket from" + Address + " shop";

        //Constant.SendMail(Constant.AdminMailId, "New Vacation Added", BodyForAdmin);
        //Constant.SendMail(ds.Tables[0].Rows[0]["Email"].ToString(), "New Vacation Added", BodyForEmployee);
        Constant.SendMail(CustomerEmail, "New Vacation Added", BodyForCustomer);
    }

    /// <summary>
    /// Mail for update Notification Customer,Admin and Employee
    /// </summary>
    private static void NotificationForUpdate()
    {
        SqlConnection cn = Constant.Connection();

        SqlDataAdapter da = new SqlDataAdapter("Select * from Userinfo where UserId=" + UserID + "", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);

        string BodyForAdmin = "Respected Admin, New vacation has been updated. Please check details";
        string BodyForEmployee = "Respected Sir, Added vacation has been updated";
        string BodyForCustomer = "Respected Sir, Vacation has been updated and you have to pickup the bucket from" + Address + " shop";

        //Constant.SendMail(Constant.AdminMailId, "Vacation Update", BodyForAdmin);
        //Constant.SendMail(ds.Tables[0].Rows[0]["Email"].ToString(), "Vacation Update", BodyForEmployee);
        Constant.SendMail(CustomerEmail, "Vacation Update", BodyForCustomer);
    }
}