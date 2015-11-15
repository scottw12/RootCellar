using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class customer_VacationList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                BindVacation();
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
    /// <summary>
    /// Bind Vacation Details to gridview
    /// </summary>
    private void BindVacation()
    {
        if (Session[Constant.UserRole] == "Admin")
        {
            SqlConnection cn = Constant.Connection();
            SqlDataAdapter da = new SqlDataAdapter("Select * From VacationDetails order by VID desc", cn);
            DataSet ds = new DataSet();           
            da.Fill(ds);
          
            gvVacation.DataSource = ds.Tables[0];
            gvVacation.DataBind();
        }
        else
        {
            SqlConnection cn = Constant.Connection();
            SqlDataAdapter da = new SqlDataAdapter("Select * From VacationDetails where CustomerID='" + Session[Constant.UserID] + "' order by VID desc", cn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            gvVacation.DataSource = ds.Tables[0];
            gvVacation.DataBind();
        }
    }
    /// <summary>
    /// Edit And Delete Operation
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvVacation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete1")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            SqlConnection cn = Constant.Connection();
            SqlCommand cmd = new SqlCommand("Delete From VacationDetails where VID=" + index + "", cn);
            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
            BindVacation();
        }

        if (e.CommandName == "Edit1")
        {

            int index = Convert.ToInt32(e.CommandArgument);
            SqlConnection cn = Constant.Connection();
            SqlDataAdapter da = new SqlDataAdapter("Select * From VacationDetails where VID=" + index + "", cn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count>0)
            {
                if (Convert.ToDateTime(ds.Tables[0].Rows[0]["VacationDate"]) == Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('You Can not edit today record')", true);
                    return;
                }
                else
                {
                    Response.Redirect("~/customer/Vacation.aspx?VID=" + EncryptDecrypt.EncryptPassword(index.ToString()));
                }
            }
        }
    }
    protected void gvVacation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvVacation.PageIndex = e.NewPageIndex;
        BindVacation();
    }
}