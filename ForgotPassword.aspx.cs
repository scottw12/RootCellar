using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class ForgotPassword : System.Web.UI.Page
{
    SqlConnection cn=Constant.Connection();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSendMail_Click(object sender, EventArgs e)
    {
        //Random random = new Random();
        //string ActivationCode=Convert.ToString(random.Next(1, 100000));
        //cn.Open();
        //SqlCommand cmd=new SqlCommand("Insert into ForgotPassword values(@EmailID,@VarficationCode,@IsActive)",cn);
        //cmd.Parameters.AddWithValue("@EmailID",txtEmail.Text);
        //cmd.Parameters.AddWithValue("@VarficationCode",ActivationCode);
        //cmd.Parameters.AddWithValue("@IsActive",false);
        //cmd.ExecuteNonQuery();
        //string Body = "<a href='http://localhost:3037/ForgotPassword?Email={0}&Key={1}', '" + txtEmail.Text + ", '" + ActivationCode + "'>Click Here To Reset Password</a> ";
        //Constant.SendMail(txtEmail.Text.Trim(),"Reset Password",Body);
        //Response.Redirect("login.aspx");
    }
}