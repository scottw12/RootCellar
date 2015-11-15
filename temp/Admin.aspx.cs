using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic;
using System.Data;
using System.Diagnostics;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;


public partial class admin_Admin : System.Web.UI.Page
{

    private SqlConnection conn = null;
    string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    private SqlCommand cmd = null;
    string Agent;
    /// <summary>
    /// page Load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        SqlConnection cn = Constant.Connection();
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM AllowAccess where UserID='" + Session[Constant.UserID].ToString() + "'", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        //Literal4.Text = "SELECT * FROM AllowAccess where UserID='" + Session[Constant.UserID].ToString() + "'<br />" + ds.Tables[0].Rows[0]["Admin"].ToString();
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["Admin"].ToString() != "True")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Error();", true);
                return;
                //Response.Redirect("../Admin/Default.aspx", false);
            }

            else
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
                    // Always call Read before accessing data.
                    while ((myDataReader.Read()))
                    {
                        string role = myDataReader.GetString(0);
                        if (role == "Admin")
                        {
                            AccordionCtrl.Visible = true;
                        }
                        else
                        {
                            //Response.Redirect("default");
                            AccordionCtrl.Visible = true;
                        }
                    }
                }
                finally
                {
                    // Close the connection when done with it.
                    if ((mySqlConnection.State == ConnectionState.Open))
                    {
                        mySqlConnection.Close();
                    }
                }
            }
        }
        if (!Page.IsPostBack)
        {
            //SqlConnection cn = Constant.Connection();
            LoadAssignRole(cn);
            LoadSeasons();
            LoadExcluded();
            Loadactiveboxes();
            LoadMultiple();
        }
    }
    /// <summary>
    /// Assign Role
    /// </summary>
    /// <param name="cn"></param>
    private void LoadAssignRole(SqlConnection cn)
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM userinfo", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvAssignRole.DataSource = ds.Tables[0];
            gvAssignRole.DataBind();
        }
    }


    protected void AddUser()
    {
        string FName = firstname.Text;
        FName = FName.Replace(" ", "");
        string LName = lastname.Text;
        LName = LName.Replace(" ", "");
        string Username = FName + "." + LName;
        string SecretQ = "Contact General Manager to reset password";
        string SecretA = "ergwergkqejfqeoufwqeofiheowfqpkoadmvnwo";
        MembershipCreateStatus createStatus = default(MembershipCreateStatus);
        MembershipUser newUser = Membership.CreateUser(Username, password.Text, email.Text, SecretQ, SecretA, true, out createStatus);


        switch (createStatus)
        {
            case MembershipCreateStatus.Success:
                Label1.Text = "The user '" + Username + "' has been created!";
                // Add Userinfo.
                string sql = null;
                string AEmail = email.Text;
                string ARole = null;
                if (Admin.Checked == true)
                {
                    ARole = "Admin";
                }
                else
                {
                    ARole = "Employee";
                }
                MembershipUser myObject = Membership.GetUser(Username);
                string uid = myObject.ProviderUserKey.ToString();
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                sql = "INSERT INTO UserInfo (UserId, FirstName, LastName, Username, Email, Role, firstlast, isapproved) VALUES ('" + uid + "', '" + FName + "', '" + LName + "', '" + Username + "', '" + AEmail + "', '" + ARole + "', '" + LName + ", " + FName + "', 'true')";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                Label1.Text = "The user '" + Username + "' has been created!";
                firstname.Text = "";
                lastname.Text = "";
                password.Text = "";
                email.Text = "";
                Admin.Checked = false;
                Standard.Checked = false;
                break; // TODO: might not be correct. Was : Exit Select


            case MembershipCreateStatus.DuplicateUserName:
                Label1.Text = "There already exists a user with this username.";
                break; // TODO: might not be correct. Was : Exit Select

            case MembershipCreateStatus.DuplicateEmail:
                Label1.Text = "There already exists a user with this email address.";
                break; // TODO: might not be correct. Was : Exit Select

            case MembershipCreateStatus.InvalidEmail:
                Label1.Text = "There email address you provided in invalid.";
                break; // TODO: might not be correct. Was : Exit Select

            case MembershipCreateStatus.InvalidAnswer:
                Label1.Text = "There security answer was invalid.";
                break; // TODO: might not be correct. Was : Exit Select

            case MembershipCreateStatus.InvalidPassword:
                Label1.Text = "The password you provided is invalid. It must be seven characters long and have at least one non-alphanumeric character.";
                break; // TODO: might not be correct. Was : Exit Select

            default:
                Label1.Text = "There was an unknown error; the user account was NOT created.";
                break; // TODO: might not be correct. Was : Exit Select

        }
        GridView2.DataBind();
        GridView3.DataBind();
        GridView4.DataBind();
        GridView5.DataBind();

    }


    public void DaySelect(object obj, DayRenderEventArgs e)
    {
        if (e.Day.IsWeekend)
        {
            e.Day.IsSelectable = false;
        }
        if (e.Day.Date.ToString("dddd") == "Thursday")
        {
            e.Day.IsSelectable = true;
        }
        else
        {
            e.Day.IsSelectable = false;
        }
        if (e.Day.Date < System.DateTime.Today)
        {
            e.Day.IsSelectable = false;
        }
    }

    protected void Button6_Click(object sender, System.EventArgs e)
    {
        AddUser();
    }


    public void edit_SelectedIndexChanged(object sender, EventArgs e)
    {
        CheckBox1.Checked = false;
        CheckBox2.Checked = false;
        Label2.Text = "";
        string uid = null;
        string FName = null;
        string AEmail = null;
        string ARole = null;
        string strQuery = "";
        GridViewRow row = GridView2.SelectedRow;
        string AN = row.Cells[0].Text.ToString();
        try
        {
            strQuery = "SELECT UserId, FirstName, LastName, Email, Role FROM Userinfo WHERE UserName =@AN";
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add("@AN", SqlDbType.VarChar).Value = AN;
            DataTable dt = GetData(cmd);
            uid = dt.Rows[0]["UserId"].ToString();
            FName = dt.Rows[0]["FirstName"].ToString();
            AEmail = dt.Rows[0]["Email"].ToString();
            ARole = dt.Rows[0]["Role"].ToString();
            Agent = AN;
            Panel1.Visible = true;
            TextBox1.Text = FName;
            TextBox2.Text = dt.Rows[0]["LastName"].ToString();
            Label21.Text = Agent;
            TextBox4.Text = AEmail;
            if (ARole == "Employee")
            {
                CheckBox1.Checked = true;
            }
            else if (ARole == "Admin")
            {
                CheckBox2.Checked = true;
            }
            GridView2.DataBind();
            GridView3.DataBind();
            GridView4.DataBind();
            GridView5.DataBind();

        }
        catch (Exception ex)
        {
            Label2.Text = "An error has occured.";
            MailMessage oMail1 = new MailMessage();
            oMail1.From = new MailAddress("site@blaneywings.com");
            oMail1.To.Add(new MailAddress("dbccemtp@gmail.com"));
            oMail1.Subject = "Root Cellar - ERROR Users";
            oMail1.Priority = MailPriority.High;
            oMail1.IsBodyHtml = true;
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            oMail1.Body += "<head><title></title></head>";
            oMail1.Body += "<body>";
            oMail1.Body += "URL: " + Request.Url.ToString() + "<br />";
            oMail1.Body += "Referer: " + Request.ServerVariables["HTTP_REFERER"].ToString() + "<br />";
            oMail1.Body += "IP: " + Request.ServerVariables["REMOTE_HOST"].ToString() + "<br />";
            oMail1.Body += "Error Message: " + ex.ToString() + "<br />";
            oMail1.Body += "Form Values: " + "<br />";
            foreach (string s in Request.Form.AllKeys)
            {
                if (s != "__VIEWSTATE")
                {
                    oMail1.Body += (s + ":") + Request.Form[s] + "<br />";
                }
            }
            oMail1.Body += "Error Stack: " + ex.StackTrace + "<br />";
            oMail1.Body += "</body>";
            oMail1.Body += "</html>";
            AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            oMail1.AlternateViews.Add(htmlView2);
            System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            ;
            
            smtpmail2.Send(oMail1);
            oMail1 = null;
        }

    }


    public DataTable GetData(SqlCommand cmd)
    {
        DataTable dt = new DataTable();
        string strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        SqlConnection con = new SqlConnection(strConnString);
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;
        try
        {
            con.Open();
            sda.SelectCommand = cmd;
            sda.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            return null;
        }
        finally
        {
            con.Close();
            sda.Dispose();
            con.Dispose();
        }
    }



    public DataTable GetData2(SqlCommand cmd2)
    {
        DataTable dl = new DataTable();
        string strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        SqlConnection con2 = new SqlConnection(strConnString);
        SqlDataAdapter sda2 = new SqlDataAdapter();
        cmd2.CommandType = CommandType.Text;
        cmd2.Connection = con2;
        try
        {
            con2.Open();
            sda2.SelectCommand = cmd2;
            sda2.Fill(dl);
            return dl;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            return null;
        }
        finally
        {
            con2.Close();
            sda2.Dispose();
            con2.Dispose();
        }
    }


    protected void Button7_Click(object sender, System.EventArgs e)
    {
        try
        {
            MembershipUser myObject = Membership.GetUser(Label21.Text);
            string uid = myObject.ProviderUserKey.ToString();
            string ARole = "";
            if (CheckBox1.Checked == true)
            {
                ARole = "Employee";
            }
            else if (CheckBox2.Checked == true)
            {
                ARole = "Admin";
            }
            string query = "Update UserInfo set FirstName=@Firstname, LastName=@Lastname, Email=@email, Role=@role WHERE UserId='{" + uid + "}' update aspnet_Membership set IsApproved='True', Islockedout='false' WHERE UserId='{" + uid + "}'";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    var _with1 = comm;
                    _with1.Connection = conn;
                    _with1.CommandType = CommandType.Text;
                    _with1.CommandText = query;
                    comm.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = TextBox1.Text;
                    _with1.Parameters.Add("@LastName", SqlDbType.VarChar).Value = TextBox2.Text;
                    _with1.Parameters.Add("@email", SqlDbType.VarChar).Value = TextBox4.Text;
                    _with1.Parameters.Add("@role", SqlDbType.VarChar).Value = ARole;
                    conn.Open();
                    comm.ExecuteNonQuery();
                }
            }
            if (!string.IsNullOrEmpty(TextBox3.Text))
            {
                myObject.IsApproved = true;
                //myObject.IsLockedOut = False
                dynamic newpassword = TextBox3.Text;
                string generatedpassword = myObject.ResetPassword();
                myObject.ChangePassword(generatedpassword, newpassword);
            }
            Label2.Text = "The user has been edited. ";
            GridView2.DataBind();
            GridView3.DataBind();
            GridView4.DataBind();
            GridView5.DataBind();

        }
        catch (Exception ex)
        {
            Label2.Text = "An error has occured";
            MailMessage oMail1 = new MailMessage();
            oMail1.From = new MailAddress("site@blaneywings.com");
            oMail1.To.Add(new MailAddress("dbccemtp@gmail.com"));
            oMail1.Subject = "Root Cellar - ERROR Users";
            oMail1.Priority = MailPriority.High;
            oMail1.IsBodyHtml = true;
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            oMail1.Body += "<head><title></title></head>";
            oMail1.Body += "<body>";
            oMail1.Body += "URL: " + Request.Url.ToString() + "<br />";
            oMail1.Body += "Referer: " + Request.ServerVariables["HTTP_REFERER"].ToString() + "<br />";
            oMail1.Body += "IP: " + Request.ServerVariables["REMOTE_HOST"].ToString() + "<br />";
            oMail1.Body += "Error Message: " + ex.ToString() + "<br />";
            oMail1.Body += "Form Values: " + "<br />";
            foreach (string s in Request.Form.AllKeys)
            {
                if (s != "__VIEWSTATE")
                {
                    oMail1.Body += (s + ":") + Request.Form[s] + "<br />";
                }
            }
            oMail1.Body += "Error Stack: " + ex.StackTrace + "<br />";
            oMail1.Body += "</body>";
            oMail1.Body += "</html>";
            AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            oMail1.AlternateViews.Add(htmlView2);
            System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            ;
            
            smtpmail2.Send(oMail1);
            oMail1 = null;
        }

    }


    public void delete_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string strQuery = "";
            GridViewRow row = GridView3.SelectedRow;
            Membership.DeleteUser(row.Cells[0].Text.ToString());
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string sql = "DELETE FROM Userinfo WHERE Username='" + row.Cells[0].Text.ToString() + "'";
            cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            Label3.Text = row.Cells[0].Text.ToString() + " has been deleted";
            GridView2.DataBind();
            GridView3.DataBind();
            GridView4.DataBind();
            GridView5.DataBind();
        }
        catch (Exception ex)
        {
            Label3.Text = "An error has occured";
            MailMessage oMail1 = new MailMessage();
            oMail1.From = new MailAddress("site@blaneywings.com");
            oMail1.To.Add(new MailAddress("dbccemtp@gmail.com"));
            oMail1.Subject = "Root Cellar - ERROR Users";
            oMail1.Priority = MailPriority.High;
            oMail1.IsBodyHtml = true;
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            oMail1.Body += "<head><title></title></head>";
            oMail1.Body += "<body>";
            oMail1.Body += "URL: " + Request.Url.ToString() + "<br />";
            oMail1.Body += "Referer: " + Request.ServerVariables["HTTP_REFERER"].ToString() + "<br />";
            oMail1.Body += "IP: " + Request.ServerVariables["REMOTE_HOST"].ToString() + "<br />";
            oMail1.Body += "Error Message: " + ex.ToString() + "<br />";
            oMail1.Body += "Form Values: " + "<br />";
            foreach (string s in Request.Form.AllKeys)
            {
                if (s != "__VIEWSTATE")
                {
                    oMail1.Body += (s + ":") + Request.Form[s] + "<br />";
                }
            }
            oMail1.Body += "Error Stack: " + ex.StackTrace + "<br />";
            oMail1.Body += "</body>";
            oMail1.Body += "</html>";
            AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            oMail1.AlternateViews.Add(htmlView2);
            System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            ;
            
            smtpmail2.Send(oMail1);
            oMail1 = null;
        }
    }

    public void freeze_SelectedIndexChanged(object sender, EventArgs e)
    {
        Label4.Text = "";
        Label5.Text = "";
        try
        {
            string strQuery = "";
            GridViewRow row = GridView4.SelectedRow;
            string ANumber = row.Cells[0].Text.ToString();
            MembershipUser myObject = Membership.GetUser(ANumber);
            string uid = myObject.ProviderUserKey.ToString();
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string sql = "update aspnet_Membership set IsApproved='False' WHERE UserId='{" + uid + "}'";
            string sql2 = "update userinfo set isapproved='False' WHERE UserId='{" + uid + "}'";
            cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand(sql2, conn);
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            Label4.Text = row.Cells[0].Text.ToString() + "'s account has been frozen";
            GridView2.DataBind();
            GridView3.DataBind();
            GridView4.DataBind();
            GridView5.DataBind();
        }
        catch (Exception ex)
        {
            Label4.Text = "An error has occured";
            MailMessage oMail1 = new MailMessage();
            oMail1.From = new MailAddress("site@blaneywings.com");
            oMail1.To.Add(new MailAddress("dbccemtp@gmail.com"));
            oMail1.Subject = "Root Cellar - ERROR Users";
            oMail1.Priority = MailPriority.High;
            oMail1.IsBodyHtml = true;
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            oMail1.Body += "<head><title></title></head>";
            oMail1.Body += "<body>";
            oMail1.Body += "URL: " + Request.Url.ToString() + "<br />";
            oMail1.Body += "Referer: " + Request.ServerVariables["HTTP_REFERER"].ToString() + "<br />";
            oMail1.Body += "IP: " + Request.ServerVariables["REMOTE_HOST"].ToString() + "<br />";
            oMail1.Body += "Error Message: " + ex.ToString() + "<br />";
            oMail1.Body += "Form Values: " + "<br />";
            foreach (string s in Request.Form.AllKeys)
            {
                if (s != "__VIEWSTATE")
                {
                    oMail1.Body += (s + ":") + Request.Form[s] + "<br />";
                }
            }
            oMail1.Body += "Error Stack: " + ex.StackTrace + "<br />";
            oMail1.Body += "</body>";
            oMail1.Body += "</html>";
            AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            oMail1.AlternateViews.Add(htmlView2);
            System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            ;
            
            smtpmail2.Send(oMail1);
            oMail1 = null;
        }
    }



    /* public void delete_SelectedIndexChanged(object sender, EventArgs e)
     {
         try
         {
             string strQuery = "";
             GridViewRow row = GridView3.SelectedRow;
             Membership.DeleteUser(row.Cells[0].Text.ToString());
             conn = new SqlConnection(ConnectionString);
             conn.Open();
             string sql = "DELETE FROM Userinfo WHERE Username='" + row.Cells[0].Text.ToString() + "'";
             cmd = new SqlCommand(sql, conn);
             cmd.ExecuteNonQuery();
             cmd.Connection.Close();
             Label3.Text = row.Cells[0].Text.ToString() + " has been deleted";
             GridView2.DataBind();
             GridView3.DataBind();
             GridView4.DataBind();
             GridView5.DataBind();
         }
         catch (Exception ex)
         {
             Label3.Text = "An error has occured";
             MailMessage oMail1 = new MailMessage();
             oMail1.From = new MailAddress("site@blaneywings.com");
             oMail1.To.Add(new MailAddress("dbccemtp@gmail.com"));
             oMail1.Subject = "Root Cellar - ERROR Users";
             oMail1.Priority = MailPriority.High;
             oMail1.IsBodyHtml = true;
             oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
             oMail1.Body += "<head><title></title></head>";
             oMail1.Body += "<body>";
             oMail1.Body += "URL: " + Request.Url.ToString() + "<br />";
             oMail1.Body += "Referer: " + Request.ServerVariables["HTTP_REFERER".ToString() + "<br />";
             oMail1.Body += "IP: " + Request.ServerVariables["REMOTE_HOST"].ToString() + "<br />";
             oMail1.Body += "Error Message: " + ex.ToString() + "<br />";
             oMail1.Body += "Form Values: " + "<br />";
             foreach (string s in Request.Form.AllKeys)
             {
                 if (s != "__VIEWSTATE")
                 {
                     oMail1.Body += (s + ":") + Request.Form[s] + "<br />";
                 }
             }
             oMail1.Body += "Error Stack: " + ex.StackTrace + "<br />";
             oMail1.Body += "</body>";
             oMail1.Body += "</html>";
             AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
             oMail1.AlternateViews.Add(htmlView2);
             System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
             ;
             
             smtpmail2.Send(oMail1);
             oMail1 = null;
         }
     }

     */
    public void unfreeze_SelectedIndexChanged(object sender, EventArgs e)
    {
        Label4.Text = "";
        Label5.Text = "";
        try
        {
            string strQuery = "";
            GridViewRow row = GridView5.SelectedRow;
            string ANumber = row.Cells[0].Text.ToString();
            MembershipUser myObject = Membership.GetUser(ANumber);
            string uid = myObject.ProviderUserKey.ToString();
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string sql = "update aspnet_Membership set IsApproved='true' WHERE UserId='{" + uid + "}'";
            string sql2 = "update userinfo set isapproved='true' WHERE UserId='{" + uid + "}'";
            cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand(sql2, conn);
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            Label4.Text = row.Cells[0].Text.ToString() + "'s account is now active";
            GridView2.DataBind();
            GridView3.DataBind();
            GridView4.DataBind();
            GridView5.DataBind();
        }
        catch (Exception ex)
        {
            Label4.Text = "An error has occured";
            MailMessage oMail1 = new MailMessage();
            oMail1.From = new MailAddress("site@blaneywings.com");
            oMail1.To.Add(new MailAddress("dbccemtp@gmail.com"));
            oMail1.Subject = "Root Cellar - ERROR Users";
            oMail1.Priority = MailPriority.High;
            oMail1.IsBodyHtml = true;
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            oMail1.Body += "<head><title></title></head>";
            oMail1.Body += "<body>";
            oMail1.Body += "URL: " + Request.Url.ToString() + "<br />";
            oMail1.Body += "Referer: " + Request.ServerVariables["HTTP_REFERER"].ToString() + "<br />";
            oMail1.Body += "IP: " + Request.ServerVariables["REMOTE_HOST"].ToString() + "<br />";
            oMail1.Body += "Error Message: " + ex.ToString() + "<br />";
            oMail1.Body += "Form Values: " + "<br />";
            foreach (string s in Request.Form.AllKeys)
            {
                if (s != "__VIEWSTATE")
                {
                    oMail1.Body += (s + ":") + Request.Form[s] + "<br />";
                }
            }
            oMail1.Body += "Error Stack: " + ex.StackTrace + "<br />";
            oMail1.Body += "</body>";
            oMail1.Body += "</html>";
            AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            oMail1.AlternateViews.Add(htmlView2);
            System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            ;
            
            smtpmail2.Send(oMail1);
            oMail1 = null;
        }
    }

    public void Storedelete_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string strQuery = "";
            GridViewRow row = GridView1.SelectedRow;
            string query = "DELETE FROM stores WHERE store=@store1 DELETE from summary where store=@store1 DELETE from summary where store=@store2 DELETE from summary where store=@store3 DELETE from summary where store=@store4 DELETE from summary where store=@store5 DELETE from summary where store=@store6";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    var _with1 = comm;
                    _with1.Connection = conn;
                    _with1.CommandType = CommandType.Text;
                    _with1.CommandText = query;
                    comm.Parameters.Add("@store1", SqlDbType.VarChar).Value = row.Cells[0].Text.ToString();
                    comm.Parameters.Add("@store2", SqlDbType.VarChar).Value = row.Cells[0].Text.ToString() + " Thursday PU";
                    comm.Parameters.Add("@store3", SqlDbType.VarChar).Value = row.Cells[0].Text.ToString() + " Friday PU";
                    comm.Parameters.Add("@store4", SqlDbType.VarChar).Value = row.Cells[0].Text.ToString() + " Saturday PU";
                    comm.Parameters.Add("@store5", SqlDbType.VarChar).Value = row.Cells[0].Text.ToString() + " NPUs";
                    comm.Parameters.Add("@store6", SqlDbType.VarChar).Value = row.Cells[0].Text.ToString() + " Vacation";
                    conn.Open();
                    comm.ExecuteNonQuery();
                }
            }

            Label6.Text = row.Cells[0].Text.ToString() + " has been deleted";
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            Label6.Text = "An error has occured";
            MailMessage oMail1 = new MailMessage();
            oMail1.From = new MailAddress("site@blaneywings.com");
            oMail1.To.Add(new MailAddress("dbccemtp@gmail.com"));
            oMail1.Subject = "Root Cellar - ERROR Users";
            oMail1.Priority = MailPriority.High;
            oMail1.IsBodyHtml = true;
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            oMail1.Body += "<head><title></title></head>";
            oMail1.Body += "<body>";
            oMail1.Body += "URL: " + Request.Url.ToString() + "<br />";
            oMail1.Body += "Referer: " + Request.ServerVariables["HTTP_REFERER"].ToString() + "<br />";
            oMail1.Body += "IP: " + Request.ServerVariables["REMOTE_HOST"].ToString() + "<br />";
            oMail1.Body += "Error Message: " + ex.ToString() + "<br />";
            oMail1.Body += "Form Values: " + "<br />";
            foreach (string s in Request.Form.AllKeys)
            {
                if (s != "__VIEWSTATE")
                {
                    oMail1.Body += (s + ":") + Request.Form[s] + "<br />";
                }
            }
            oMail1.Body += "Error Stack: " + ex.StackTrace + "<br />";
            oMail1.Body += "</body>";
            oMail1.Body += "</html>";
            AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            oMail1.AlternateViews.Add(htmlView2);
            System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            ;
            
            smtpmail2.Send(oMail1);
            oMail1 = null;
        }
    }



    /*public void unfreeze_SelectedIndexChanged(object sender, EventArgs e)
    {
        Label4.Text = "";
        Label5.Text = "";
        try
        {
            string strQuery = "";
            GridViewRow row = GridView5.SelectedRow;
            string ANumber = row.Cells[0].Text.ToString();
            MembershipUser myObject = Membership.GetUser(ANumber);
            string uid = myObject.ProviderUserKey.ToString();
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string sql = "update aspnet_Membership set IsApproved='true' WHERE UserId='{" + uid + "}'";
            string sql2 = "update userinfo set isapproved='true' WHERE UserId='{" + uid + "}'";
            cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand(sql2, conn);
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            Label4.Text = row.Cells[0].Text.ToString() + "'s account is now active";
            GridView2.DataBind();
            GridView3.DataBind();
            GridView4.DataBind();
            GridView5.DataBind();
        }
        catch (Exception ex)
        {
            Label4.Text = "An error has occured";
            MailMessage oMail1 = new MailMessage();
            oMail1.From = new MailAddress("site@blaneywings.com");
            oMail1.To.Add(new MailAddress("dbccemtp@gmail.com"));
            oMail1.Subject = "Root Cellar - ERROR Users";
            oMail1.Priority = MailPriority.High;
            oMail1.IsBodyHtml = true;
            oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            oMail1.Body += "<head><title></title></head>";
            oMail1.Body += "<body>";
            oMail1.Body += "URL: " + Request.Url.ToString() + "<br />";
            oMail1.Body += "Referer: " + Request.ServerVariables["HTTP_REFERER"].ToString() + "<br />";
            oMail1.Body += "IP: " + Request.ServerVariables["REMOTE_HOST"].ToString() + "<br />";
            oMail1.Body += "Error Message: " + ex.ToString() + "<br />";
            oMail1.Body += "Form Values: " + "<br />";
            foreach (string s in Request.Form.AllKeys)
            {
                if (s != "__VIEWSTATE")
                {
                    oMail1.Body += (s + ":") + Request.Form[s] + "<br />";
                }
            }
            oMail1.Body += "Error Stack: " + ex.StackTrace + "<br />";
            oMail1.Body += "</body>";
            oMail1.Body += "</html>";
            AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            oMail1.AlternateViews.Add(htmlView2);
            System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            ;
            
            smtpmail2.Send(oMail1);
            oMail1 = null;
        }
    }

    */
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(NewStore.Text))
        {
            string query = "Insert into stores (store) values (@store1) Insert into summary (store) values (@store1) Insert into summary (store) values (@store2) Insert into summary (store) values (@store3) Insert into summary (store) values (@store4) Insert into summary (store) values (@store5) Insert into summary (store) values (@store6)";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    var _with1 = comm;
                    _with1.Connection = conn;
                    _with1.CommandType = CommandType.Text;
                    _with1.CommandText = query;
                    comm.Parameters.Add("@store1", SqlDbType.VarChar).Value = NewStore.Text;
                    comm.Parameters.Add("@store2", SqlDbType.VarChar).Value = NewStore.Text + " Thursday PU";
                    comm.Parameters.Add("@store3", SqlDbType.VarChar).Value = NewStore.Text + " Friday PU";
                    comm.Parameters.Add("@store4", SqlDbType.VarChar).Value = NewStore.Text + " Saturday PU";
                    comm.Parameters.Add("@store5", SqlDbType.VarChar).Value = NewStore.Text + " NPUs";
                    comm.Parameters.Add("@store6", SqlDbType.VarChar).Value = NewStore.Text + " Vacation";
                    conn.Open();
                    comm.ExecuteNonQuery();
                }
            }
            NewStore.Text = "";
            Label6.Text = NewStore.Text + " has been added";
            GridView1.DataBind();
        }
        else
        {
            Label6.Text = "Please enter a store name!";
        }
    }


    protected void Button2_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(NewPickupDay.Text))
        {
            string query = "Insert into pickupdays (pickupday) values (@pickupday)";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    var _with1 = comm;
                    _with1.Connection = conn;
                    _with1.CommandType = CommandType.Text;
                    _with1.CommandText = query;
                    comm.Parameters.Add("@pickupday", SqlDbType.VarChar).Value = NewPickupDay.Text;
                    conn.Open();
                    comm.ExecuteNonQuery();
                }
            }
            Label7.Text = NewPickupDay.Text + " has been added";
            GridView6.DataBind();
        }
        else
        {
            Label7.Text = "Please enter a Pickup Day!";
        }
    }
    private void LoadSeasons()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SID");
        dt.Columns.Add("name");
        dt.Columns.Add("currents");
        dt.Columns.Add("SStart");
        dt.Columns.Add("send");
        dt.Columns.Add("enroll");
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        mySqlCommand = new SqlCommand("SELECT sid, name, currents, sstart, send, enroll FROM seasons order by sid", mySqlConnection);
        try
        {
            mySqlConnection.Open();
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            while ((myDataReader.Read()))
            {
                if (myDataReader.HasRows)
                {
                    dt.Rows.Add(myDataReader.GetInt32(0), myDataReader.GetString(1), myDataReader.GetBoolean(2), (myDataReader.GetDateTime(3).ToString().Replace(" 12:00:00 AM", "")), (myDataReader.GetDateTime(4).ToString().Replace(" 12:00:00 AM", "")), myDataReader.GetBoolean(5));
                    int i = dt.Rows.Count;
                    //if (dt.Rows[i][3] != null)
                    //{
                    //    string str = dt.Rows[i][3].ToString();
                    //    string str1 = "";
                    //}
                }
            }
        }
        finally
        {
            // Close the connection when done with it.
            if ((mySqlConnection.State == ConnectionState.Open))
            {
                mySqlConnection.Close();
            }
        }
        GridView7.DataSource = dt;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string dt1 = Convert.ToDateTime(dt.Rows[i][3].ToString()).ToShortDateString();
            DateTime dt2 = Convert.ToDateTime(dt1);
            dt.Rows[i][3] = Convert.ToDateTime(dt.Rows[i][3].ToString()).ToShortDateString();
            //Calendar1.SelectedDate = Convert.ToDateTime(dt.Rows[i][3]);

        }

        GridView7.DataBind();
    }
    private void Loadactiveboxes()
    {
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        mySqlCommand = new SqlCommand("SELECT bounty, barnyard, ploughman FROM activeboxes", mySqlConnection);
        try
        {
            mySqlConnection.Open();
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            while ((myDataReader.Read()))
            {
                if (myDataReader.HasRows)
                {
                    if (myDataReader.GetBoolean(0) == true)
                    {
                        BountyActive.Checked = true;
                    }
                    else
                    {
                        BountyActive.Checked = false;
                    }
                    if (myDataReader.GetBoolean(1) == true)
                    {
                        BarnyardActive.Checked = true;
                    }
                    else
                    {
                        BarnyardActive.Checked = false;
                    }
                    if (myDataReader.GetBoolean(2) == true)
                    {
                        PloughmanActive.Checked = true;
                    }
                    else
                    {
                        PloughmanActive.Checked = false;
                    }
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
    }
    protected void GridViewSample_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView7.EditIndex = e.NewEditIndex;
        Literal1.Text = "";
        LoadSeasons();
    }



    protected void GridViewSample_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            Literal1.Text = "";
            Literal SIDLit = (Literal)GridView7.Rows[e.RowIndex].FindControl("SIDLit");
            TextBox SNameBox = (TextBox)GridView7.Rows[e.RowIndex].FindControl("SeasonName");
            Literal SNameLit = (Literal)GridView7.Rows[e.RowIndex].FindControl("Snamelit");
            CheckBox SCurrent = (CheckBox)GridView7.Rows[e.RowIndex].FindControl("SeasonCurrent2");
            CheckBox Senroll = (CheckBox)GridView7.Rows[e.RowIndex].FindControl("Seasonenroll2");
            Calendar SStart = (Calendar)GridView7.Rows[e.RowIndex].FindControl("Calendar1");
            Literal SLit = (Literal)GridView7.Rows[e.RowIndex].FindControl("slit");
            Calendar SEnd = (Calendar)GridView7.Rows[e.RowIndex].FindControl("Calendar2");
            Literal ELit = (Literal)GridView7.Rows[e.RowIndex].FindControl("elit");
            string query = "Update seasons set name=@name, CurrentS=@CurrentS,";
            if (SStart.SelectedDate.ToString() != "1/1/0001 12:00:00 AM")
            {
                query += " SStart=@Sstart,";
            }
            if (SEnd.SelectedDate.ToString() != "1/1/0001 12:00:00 AM")
            {
                query += "SEnd=@SEnd, ";
            }
            query += "enroll=@enroll where SID=@sidlit";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    var _with1 = comm;
                    _with1.Connection = conn;
                    _with1.CommandType = CommandType.Text;
                    _with1.CommandText = query;
                    //comm.Parameters.Add("@SIDlit", SqlDbType.VarChar).Value = SIDLit.Text;
                    //comm.Parameters.Add("@name", SqlDbType.VarChar).Value = SNameBox.Text;
                    //comm.Parameters.Add("@CurrentS", SqlDbType.Bit).Value = SCurrent.Checked.ToString();
                    //comm.Parameters.Add("@SStart", SqlDbType.Date).Value = SStart.SelectedDate.ToString();
                    //comm.Parameters.Add("@SEnd", SqlDbType.Date).Value = SEnd.SelectedDate.ToString();
                    //comm.Parameters.Add("@enroll", SqlDbType.Bit).Value = Senroll.Checked.ToString();

                    comm.Parameters.AddWithValue("@SIDlit", SIDLit.Text);
                    comm.Parameters.AddWithValue("@name", SNameBox.Text);
                    comm.Parameters.AddWithValue("@CurrentS", SCurrent.Checked.ToString());
                    if (SStart.SelectedDate.ToString() != "1/1/0001 12:00:00 AM")
                    {
                        comm.Parameters.AddWithValue("@SStart", SStart.SelectedDate);
                    }
                    if (SEnd.SelectedDate.ToString() != "1/1/0001 12:00:00 AM")
                    {
                        comm.Parameters.AddWithValue("@SEnd", SEnd.SelectedDate);
                    }
                    comm.Parameters.AddWithValue("@enroll", Senroll.Checked.ToString());

                    conn.Open();
                    comm.ExecuteNonQuery();
                }
            }
            GridView7.EditIndex = -1;
            LoadSeasons();
            if (SCurrent.Checked == true)
            {
                MoveSubscribers();
            }
            else
            {
                Literal1.Text = "Season updated successfully! ";
            }
        }
        catch (Exception ex)
        {
            Literal1.Text = "We're sorry, there was an error";
        }


    }
    protected void GridViewSample_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView7.EditIndex = -1;
        Literal1.Text = "";
        LoadSeasons();
    }
    protected void GridViewSample_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Literal SIDLit = (Literal)GridView7.Rows[e.RowIndex].FindControl("SIDLit");
        string query = "Delete from seasons where SID=@SID";
        using (SqlConnection conn = new SqlConnection(ConnectionString))
        {
            using (SqlCommand comm = new SqlCommand())
            {
                var _with2 = comm;
                _with2.Connection = conn;
                _with2.CommandType = CommandType.Text;
                _with2.CommandText = query;
                comm.Parameters.Add("@SID", SqlDbType.Int).Value = SIDLit.Text;
                conn.Open();
                comm.ExecuteNonQuery();
            }
        }
        GridView7.EditIndex = -1;
        LoadSeasons();
        Literal1.Text = "Season deleted successfully!";
    }
    protected void GridViewSample_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Insert"))
        {
            TextBox SNameBox = (TextBox)GridView7.FooterRow.FindControl("NewSeasonName");
            CheckBox SCurrent = (CheckBox)GridView7.FooterRow.FindControl("NewSeasonCurrent");
            Calendar SStart = (Calendar)GridView7.FooterRow.FindControl("NewCalendar1");
            Calendar SEnd = (Calendar)GridView7.FooterRow.FindControl("NewCalendar2");
            CheckBox Senroll = (CheckBox)GridView7.FooterRow.FindControl("NewSeasonenroll");
            string query = "Insert Into seasons (name, CurrentS, SStart, SEnd, enroll) Values (@name, @CurrentS, @Sstart, @SEnd, @enroll)";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    var _with3 = comm;
                    _with3.Connection = conn;
                    _with3.CommandType = CommandType.Text;
                    _with3.CommandText = query;
                    comm.Parameters.Add("@name", SqlDbType.VarChar).Value = SNameBox.Text;
                    comm.Parameters.Add("@CurrentS", SqlDbType.Bit).Value = SCurrent.Checked.ToString();
                    comm.Parameters.Add("@SStart", SqlDbType.Date).Value = SStart.SelectedDate.ToString();
                    comm.Parameters.Add("@SEnd", SqlDbType.Date).Value = SEnd.SelectedDate.ToString();
                    comm.Parameters.Add("@enroll", SqlDbType.Bit).Value = Senroll.Checked.ToString();
                    conn.Open();
                    comm.ExecuteNonQuery();
                }
            }
            GridView7.EditIndex = -1;
            LoadSeasons();
            Literal1.Text = "Season added successfully!";
        }
    }



    private void MoveSubscribers()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SID");
            SqlDataReader myDataReader = default(SqlDataReader);
            SqlConnection mySqlConnection = default(SqlConnection);
            SqlCommand mySqlCommand = default(SqlCommand);
            mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            string query = "";
            int CurrActive = 0;
            //----------------------------------------
            mySqlCommand = new SqlCommand("SELECT COUNT(SubID) AS subscribers FROM subscribers WHERE active='true'", mySqlConnection);
            mySqlConnection.Open();
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            while ((myDataReader.Read()))
            {
                if (myDataReader.HasRows)
                {
                    CurrActive = myDataReader.GetInt32(0);
                }
            }
            // Close the connection when done with it.
            if ((mySqlConnection.State == ConnectionState.Open))
            {
                mySqlConnection.Close();
            }
            //----------------------------------------
            query += "Update subscribers set active='false', vacused='0'";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    var _with1 = comm;
                    _with1.Connection = conn;
                    _with1.CommandType = CommandType.Text;
                    _with1.CommandText = query;
                    conn.Open();
                    comm.ExecuteNonQuery();
                }
            }
            //----------------------------------------
            mySqlCommand = new SqlCommand("SELECT distinct subid FROM weekly where week='1/1/1900' and ((bounty='true' and paidbounty='true') or (barnyard='true' and paidbarnyard='true') or (ploughman='true' and paidploughman='true'))", mySqlConnection);
            mySqlConnection.Open();
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            while ((myDataReader.Read()))
            {
                if (myDataReader.HasRows)
                {
                    dt.Rows.Add(myDataReader.GetInt32(0));
                }
            }
            if ((mySqlConnection.State == ConnectionState.Open))
            {
                mySqlConnection.Close();
            }
            int added = 0;
            foreach (DataRow row in dt.Rows)
            {
                query += "Update subscribers set active='true' where subid='" + row["SID"] + "' ";
                added += 1;
            }
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    var _with2 = comm;
                    _with2.Connection = conn;
                    _with2.CommandType = CommandType.Text;
                    _with2.CommandText = query;
                    conn.Open();
                    comm.ExecuteNonQuery();
                }
            }
            Literal1.Text = "Season added/updated successfully!<br />" + (CurrActive - added).ToString() + " subscribers were moved to inactive.";
        }
        catch (Exception ex)
        {
            Literal1.Text = "We're sorry, there was an error";
        }

    }
    private void LoadExcluded()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("EID");
        dt.Columns.Add("EDate");
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        mySqlCommand = new SqlCommand("SELECT eid, edate FROM excluded order by edate", mySqlConnection);
        try
        {
            mySqlConnection.Open();
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            while ((myDataReader.Read()))
            {
                if (myDataReader.HasRows)
                {
                    dt.Rows.Add(myDataReader.GetInt32(0), (myDataReader.GetDateTime(1).ToString().Replace(" 12:00:00 AM", "")));
                }
            }
        }
        finally
        {
            // Close the connection when done with it.
            if ((mySqlConnection.State == ConnectionState.Open))
            {
                mySqlConnection.Close();
            }
        }
        GridView8.DataSource = dt;
        GridView8.DataBind();
    }
    protected void GridView8_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Literal EIDLit = (Literal)GridView8.Rows[e.RowIndex].FindControl("EIDLit");
        string query = "Delete from excluded where EID=@EID";
        using (SqlConnection conn = new SqlConnection(ConnectionString))
        {
            using (SqlCommand comm = new SqlCommand())
            {
                var _with3 = comm;
                _with3.Connection = conn;
                _with3.CommandType = CommandType.Text;
                _with3.CommandText = query;
                comm.Parameters.Add("@EID", SqlDbType.Int).Value = EIDLit.Text;
                conn.Open();
                comm.ExecuteNonQuery();
            }
        }
        GridView8.EditIndex = -1;
        LoadExcluded();
        Literal2.Text = "Excluded date deleted successfully!";
    }
    protected void GridView8_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Insert"))
        {
            try
            {
                Calendar EDate = (Calendar)GridView8.FooterRow.FindControl("NewEDate");
                string query = "Insert Into excluded (EDate) Values (@EDate)";
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand())
                    {
                        var _with4 = comm;
                        _with4.Connection = conn;
                        _with4.CommandType = CommandType.Text;
                        _with4.CommandText = query;
                        comm.Parameters.Add("@edate", SqlDbType.VarChar).Value = EDate.SelectedDate;
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                }
                string query2 = "delete from weekly where week=@EDate";
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand())
                    {
                        var _with5 = comm;
                        _with5.Connection = conn;
                        _with5.CommandType = CommandType.Text;
                        _with5.CommandText = query2;
                        comm.Parameters.Add("@edate", SqlDbType.VarChar).Value = EDate.SelectedDate;
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                }
                GridView8.EditIndex = -1;
                LoadExcluded();
                Literal2.Text = "Date Removed successfully!";
            }
            catch (Exception ex)
            {
                Literal1.Text = "Were sorry, there was an error";
            }

        }
    }



    protected void ActiveBoxButton_Click(object sender, EventArgs e)
    {
        string query = "update activeboxes set bounty=@bounty, barnyard=@barnyard, ploughman=@ploughman";
        using (SqlConnection conn = new SqlConnection(ConnectionString))
        {
            using (SqlCommand comm = new SqlCommand())
            {
                var _with1 = comm;
                _with1.Connection = conn;
                _with1.CommandType = CommandType.Text;
                _with1.CommandText = query;
                comm.Parameters.Add("@bounty", SqlDbType.Bit).Value = BountyActive.Checked.ToString();
                comm.Parameters.Add("@barnyard", SqlDbType.Bit).Value = BarnyardActive.Checked.ToString();
                comm.Parameters.Add("@ploughman", SqlDbType.Bit).Value = PloughmanActive.Checked.ToString();
                conn.Open();
                comm.ExecuteNonQuery();
            }
        }
        Loadactiveboxes();
        Literal3.Text = "<span style='color:red;'>Your active box's have been updated!</span>";
    }
    private void LoadMultiple()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("FirstName1");
        dt.Columns.Add("LastName1");
        dt.Columns.Add("SubID");
        dt.Columns.Add("Count");
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        mySqlCommand = new SqlCommand("Select subscribers.FirstName1, subscribers.LastName1, weekly.SubId, Count(Case When weekly.Week = '1/1/1900' Then 1 Else Null End) As Count From weekly Inner Join subscribers On subscribers.SubId = weekly.SubId where active='true' Group By subscribers.FirstName1, subscribers.LastName1, weekly.SubId Order By Count Desc", mySqlConnection);
        try
        {
            mySqlConnection.Open();
            myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            while ((myDataReader.Read()))
            {
                if (myDataReader.HasRows)
                {
                    dt.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(1), ("http://rootcellarboxes.com/admin/Details?s=" + myDataReader.GetInt32(2).ToString()), myDataReader.GetInt32(3).ToString());
                }
            }
        }
        finally
        {
            // Close the connection when done with it.
            if ((mySqlConnection.State == ConnectionState.Open))
            {
                mySqlConnection.Close();
            }
        }
        GridView10.DataSource = dt;
        GridView10.DataBind();
    }
    protected void GridView10_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal Count = e.Row.FindControl("Count") as Literal;
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (!(Convert.ToInt32(Count.Text) > 1))
                    {
                        e.Row.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Literal1.Text = ex.Message + ex.StackTrace;
        }
    }


    protected void PickupDaydelete_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow row = GridView6.SelectedRow;
            string query = "DELETE FROM pickupdays WHERE pickupday=@pickupday";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    var _with2 = comm;
                    _with2.Connection = conn;
                    _with2.CommandType = CommandType.Text;
                    _with2.CommandText = query;
                    comm.Parameters.Add("@pickupday", SqlDbType.VarChar).Value = row.Cells[0].Text.ToString();
                    conn.Open();
                    comm.ExecuteNonQuery();
                }
            }
            Label7.Text = row.Cells[0].Text.ToString() + " has been deleted";
            GridView6.DataBind();
        }
        catch (Exception ex)
        {
            //Label7.Text = "An error has occured";
            //MailMessage oMail1 = new MailMessage();
            //oMail1.From = new MailAddress("site@blaneywings.com");
            //oMail1.To.Add(new MailAddress("dbccemtp@gmail.com"));
            //oMail1.Subject = "Root Cellar - ERROR Users";
            //oMail1.Priority = MailPriority.High;
            //oMail1.IsBodyHtml = true;
            //oMail1.Body = "<html xmlns='http://www.w3.org/1999/xhtml' >";
            //oMail1.Body += "<head><title></title></head>";
            //oMail1.Body += "<body>";
            //oMail1.Body += "URL: " + Request.Url.ToString() + "<br />";
            //oMail1.Body += "Referer: " + Request.ServerVariables["HTTP_REFERER"].ToString() + "<br />";
            //oMail1.Body += "IP: " + Request.ServerVariables["REMOTE_HOST"].ToString() + "<br />";
            //oMail1.Body += "Error Message: " + ex.ToString() + "<br />";
            //oMail1.Body += "Form Values: " + "<br />";
            //foreach (string s in Request.Form.AllKeys)
            //{
            //    if (s != "__VIEWSTATE")
            //    {
            //        oMail1.Body += (s + ":") + Request.Form[s] + "<br />";
            //    }
            //}
            //oMail1.Body += "Error Stack: " + ex.StackTrace + "<br />";
            //oMail1.Body += "</body>";
            //oMail1.Body += "</html>";
            //AlternateView htmlView2 = AlternateView.CreateAlternateViewFromString(oMail1.Body, null, "text/html");
            //oMail1.AlternateViews.Add(htmlView2);
            //System.Net.Mail.SmtpClient smtpmail2 = new System.Net.Mail.SmtpClient();
            //;
            //
            //smtpmail2.Send(oMail1);
            //oMail1 = null;
        }
    }

}