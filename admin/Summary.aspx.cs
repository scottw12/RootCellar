using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using Telerik.Web.UI.Upload;
using Telerik.Web.UI;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Drawing;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

public partial class admin_Summary : System.Web.UI.Page
{
    string SqlQuary = "";
    string Options = "";
    DataTable dt = new DataTable();
    private SqlConnection conn = null;
    string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    private SqlCommand cmd = null;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            SqlConnection cn = Constant.Connection();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM AllowAccess where UserID='" + Session[Constant.UserID].ToString() + "'", cn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["WeeklySummary"].ToString() == "False")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Error();", true);
                    return;
                }

                else
                {
                    if (User.Identity.IsAuthenticated)
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
                    }
                    else
                    {
                        Response.Redirect("~/login");
                    }
                }
            }
            FillWeekInfo();

            //RadProgressArea1.ProgressIndicators = RadProgressArea1.ProgressIndicators && ~ProgressIndicators.SelectedFilesCount;
            RadProgressArea1.ProgressIndicators &= ~ProgressIndicators.SelectedFilesCount;
        }

        RadProgressArea1.Localization.Uploaded = "Total Progress";
        RadProgressArea1.Localization.UploadedFiles = "Progress";
        RadProgressArea1.Localization.CurrentFileName = "Custom progress in action: ";
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }
    protected void FillWeekInfo()
    {
        dt.Columns.Add("Week");
        dt.Rows.Add(" - Select a Week - ");
        //Create Rows in DataTable
        SqlDataReader myDataReader2 = default(SqlDataReader);
        SqlConnection mySqlConnection2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        SqlCommand mySqlCommand2 = default(SqlCommand);
        string SDateRange = "";
        string query = "select Sstart, send from seasons where currents='True' order by sstart";
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (mySqlConnection2)
                {
                    mySqlCommand2 = new SqlCommand(query, mySqlConnection2);
                    mySqlConnection2.Open();
                    myDataReader2 = mySqlCommand2.ExecuteReader();
                    int i = 0;
                    if (myDataReader2.HasRows)
                    {
                        while (myDataReader2.Read())
                        {
                            if (!(i == 0))
                            {
                                SDateRange += " or ";
                            }
                            SDateRange = "(week >= '" + myDataReader2.GetDateTime(0) + "' and week <= '" + myDataReader2.GetDateTime(1) + "')";
                            i += 1;
                        }
                    }
                    myDataReader2.Close();
                }
            }
        }
        finally
        {
        }
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        try
        {
            using (mySqlConnection)
            {
                mySqlCommand = new SqlCommand("SELECT DISTINCT Week FROM Weekly where " + SDateRange + " ORDER BY [Week]", mySqlConnection);
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
        this.WeekList.DataSource = dt;
        this.WeekList.DataTextField = "Week";
        this.WeekList.DataValueField = "Week";
        this.WeekList.DataBind();
        DateTime SelDate = System.DateTime.Now;
        if (System.DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
        {
            SelDate = SelDate.AddDays(4);
        }
        else if (System.DateTime.Now.DayOfWeek == DayOfWeek.Monday)
        {
            SelDate = SelDate.AddDays(3);
        }
        else if (System.DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
        {
            SelDate = SelDate.AddDays(2);
        }
        else if (System.DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
        {
            SelDate = SelDate.AddDays(1);
        }
        else if (System.DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
        {
        }
        else if (System.DateTime.Now.DayOfWeek == DayOfWeek.Friday)
        {
            SelDate = SelDate.AddDays(-1);
        }
        else if (System.DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
        {
            SelDate = SelDate.AddDays(-2);
        }
        WeekList.SelectedValue = SelDate.Month.ToString() + "/" + SelDate.Day.ToString() + "-" + SelDate.AddDays(1).Day.ToString() + "/" + SelDate.Year.ToString();
        if (!(WeekList.SelectedValue == " - Select a Week - "))
        {
            UpdateProgressContext();
            FillInfo();
        }
        else
        {
            GridView1.Visible = false;
        }
    }
    protected void FillInfo()
    {
        GridView1.Visible = true;
        string week = null;
        week = WeekList.SelectedValue;
        string pattern = "-(.*?)/";
        string replacement = "/" + "\r\n";
        Regex rgx = new Regex(pattern, RegexOptions.Singleline);
        week = rgx.Replace(week, replacement);
        week = (DateTime.Parse(week)).ToString().Replace(" 12:00:00 AM", "");
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlCommand mySqlCommand = default(SqlCommand);

        foreach (DataRow Storerow_loopVariable in FillStoreInfo().Rows)
        {
            DataRow Storerow = Storerow_loopVariable;
            SqlQuary = "Select Count(Case when week='" + week + "' and Weekly.Bounty='True' and Location='" + Storerow[0] + "' and vacation='false' and Weekly.PickupDay='Thursday' Then 1 Else Null End) As BountyPUThu, Count( Case when week='" + week + "' and Weekly.Bounty='True' and Location='" + Storerow[0] + "' and vacation='false' and Weekly.PickupDay='Friday' Then 1 Else Null End) As BountyPUFri, Count( Case when week='" + week + "' and Weekly.Bounty='True' and Location='" + Storerow[0] + "' and vacation='false' and Weekly.PickupDay='Saturday' Then 1 Else Null End) As BountyPUSat, Count( Case when week='" + week + "' and Weekly.Bounty='True' and vacation='false' and pickedup='false' and location='" + Storerow[0] + "' Then 1 Else Null End) As BountyNPU, ";
            SqlQuary += "Count(Case when week='" + week + "' and Weekly.Barnyard='True' and Location='" + Storerow[0] + "' and vacation='false' and Weekly.PickupDay='Thursday' Then 1 Else Null End) As BarnyardPUThu, Count( Case when week='" + week + "' and Weekly.Barnyard='True' and Location='" + Storerow[0] + "' and vacation='false' and Weekly.PickupDay='Friday' Then 1 Else Null End) As BarnyardPUFri, Count( Case when week='" + week + "' and Weekly.Barnyard='True' and Location='" + Storerow[0] + "' and vacation='false' and Weekly.PickupDay='Saturday' Then 1 Else Null End) As BarnyardPUSat, Count( Case when week='" + week + "' and Weekly.Barnyard='True' and vacation='false' and pickedup='false' and location='" + Storerow[0] + "' Then 1 Else Null End) As BarnyardNPU, ";
            SqlQuary += "Count(Case when week='" + week + "' and Weekly.Ploughman='True' and Location='" + Storerow[0] + "' and vacation='false' and Weekly.PickupDay='Thursday' Then 1 Else Null End) As PloughmanPUThu, Count( Case when week='" + week + "' and Weekly.Ploughman='True' and Location='" + Storerow[0] + "' and vacation='false' and Weekly.PickupDay='Friday' Then 1 Else Null End) As PloughmanPUFri, Count( Case when week='" + week + "' and Weekly.Ploughman='True' and Location='" + Storerow[0] + "' and vacation='false' and Weekly.PickupDay='Saturday' Then 1 Else Null End) As PloughmanPUSat, Count( Case when week='" + week + "' and Weekly.Ploughman='True' and vacation='false' and pickedup='false' and location='" + Storerow[0] + "' Then 1 Else Null End) As PloughmanNPU, ";
            SqlQuary += "Count( Case when week='" + week + "' And Weekly.Bounty = 'True' and vacation='true' and location='" + Storerow[0] + "' Then 1 Else Null End) As BountyVac, Count( Case when week='" + week + "' And Weekly.Barnyard = 'true' and vacation='true' and location='" + Storerow[0] + "' Then 1 Else Null End) As BarnVac, Count( Case when week='" + week + "' And Weekly.Ploughman = 'true' and vacation='true' and location='" + Storerow[0] + "' Then 1 Else Null End) As PloVac From weekly INNER JOIN Subscribers on weekly.SubId=Subscribers.SubId Where  Subscribers.Active = 'true'";
            using (SqlConnection mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                mySqlCommand = new SqlCommand(SqlQuary, mySqlConnection);
                mySqlConnection.Open();
                myDataReader = mySqlCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    string SubInfo = "";
                    string sql = null;
                    while (myDataReader.Read())
                    {
                        //sql = "Update summary set Bounty='" + myDataReader.GetInt32(0).ToString() + "' WHERE store='" + Storerow[0] + " Thursday PU'  Update summary set Bounty='" + myDataReader.GetInt32(1).ToString() + "' WHERE store= '" + Storerow[0] + " Friday PU' Update summary set Bounty='" + myDataReader.GetInt32(2).ToString() + "' WHERE store= '" + Storerow[0] + " Saturday PU' Update summary set bounty='" + myDataReader.GetInt32(3).ToString() + "' WHERE store= '" + Storerow[0] + " NPUs'  ";
                        //sql += "Update summary set Barnyard='" + myDataReader.GetInt32(4).ToString() + "' WHERE store='" + Storerow[0] + " Thursday PU'  Update summary set Barnyard='" + myDataReader.GetInt32(5).ToString() + "' WHERE store= '" + Storerow[0] + " Friday PU' Update summary set Barnyard='" + myDataReader.GetInt32(6).ToString() + "' WHERE store= '" + Storerow[0] + " Saturday PU' Update summary set Barnyard='" + myDataReader.GetInt32(7).ToString() + "' WHERE store= '" + Storerow[0] + " NPUs' ";
                        //sql += "Update summary set Ploughman='" + myDataReader.GetInt32(8).ToString() + "' WHERE store='" + Storerow[0] + " Thursday PU'  Update summary set Ploughman='" + myDataReader.GetInt32(9).ToString() + "' WHERE store= '" + Storerow[0] + " Friday PU' Update summary set Ploughman='" + myDataReader.GetInt32(10).ToString() + "' WHERE store= '" + Storerow[0] + " Saturday PU' Update summary set Ploughman='" + myDataReader.GetInt32(11).ToString() + "' WHERE store= '" + Storerow[0] + " NPUs' ";
                        //sql += "Update summary set Bounty='" + myDataReader.GetInt32(12).ToString() + "' WHERE store= '" + Storerow[0] + " Vacation' Update summary set Barnyard='" + myDataReader.GetInt32(13).ToString() + "' WHERE store= '" + Storerow[0] + " Vacation' Update summary set Ploughman='" + myDataReader.GetInt32(14).ToString() + "' WHERE store= '" + Storerow[0] + " Vacation' Update summary set Total='" + (myDataReader.GetInt32(12) + myDataReader.GetInt32(13) + myDataReader.GetInt32(14)).ToString() + "' WHERE store= '" + Storerow[0] + " Vacation'";
                        //sql += "Update summary set Bounty='" + (myDataReader.GetInt32(0) + myDataReader.GetInt32(1).ToString() + myDataReader.GetInt32(2).ToString()).ToString() + "' WHERE store='" + Storerow[0] + "' Update summary set Barnyard='" + (myDataReader.GetInt32(4) + myDataReader.GetInt32(5).ToString() + myDataReader.GetInt32(6).ToString()).ToString() + "' WHERE store='" + Storerow[0] + "' Update summary set Ploughman='" + (myDataReader.GetInt32(8) + myDataReader.GetInt32(9).ToString() + myDataReader.GetInt32(10).ToString()).ToString() + "' WHERE store='" + Storerow[0] + "'";
                        //sql += "Update summary set Total='" + (myDataReader.GetInt32(0) + myDataReader.GetInt32(1).ToString() + myDataReader.GetInt32(2).ToString() + myDataReader.GetInt32(4) + myDataReader.GetInt32(5).ToString() + myDataReader.GetInt32(6).ToString() + myDataReader.GetInt32(8) + myDataReader.GetInt32(9).ToString() + myDataReader.GetInt32(10).ToString()).ToString() + "' WHERE store='" + Storerow[0] + "' Update summary set Total='" + (myDataReader.GetInt32(0) + myDataReader.GetInt32(4).ToString() + myDataReader.GetInt32(8).ToString() + myDataReader.GetInt32(12)).ToString() + "' WHERE store='" + Storerow[0] + " Thursday PU' Update summary set Total='" + (myDataReader.GetInt32(1) + myDataReader.GetInt32(5).ToString() + myDataReader.GetInt32(9).ToString() + myDataReader.GetInt32(13)).ToString() + "' WHERE store='" + Storerow[0] + " Friday PU' Update summary set Total='" + (myDataReader.GetInt32(2) + myDataReader.GetInt32(6).ToString() + myDataReader.GetInt32(10).ToString() + myDataReader.GetInt32(14)).ToString() + "' WHERE store='" + Storerow[0] + " Saturday PU' Update summary set Total='" + (myDataReader.GetInt32(3) + myDataReader.GetInt32(7).ToString() + myDataReader.GetInt32(11)).ToString() + "' WHERE store='" + Storerow[0] + " NPUs'";
                        //conn = new SqlConnection(ConnectionString);

                        sql = "Update summary set Bounty='" + myDataReader.GetInt32(0).ToString() + "' WHERE store='" + Storerow[0] + " Thursday PU'  Update summary set Bounty='" + myDataReader.GetInt32(1).ToString() + "' WHERE store= '" + Storerow[0] + " Friday PU' Update summary set Bounty='" + myDataReader.GetInt32(2).ToString() + "' WHERE store= '" + Storerow[0] + " Saturday PU' Update summary set bounty='" + myDataReader.GetInt32(3).ToString() + "' WHERE store= '" + Storerow[0] + " NPUs'  ";
                        sql += "Update summary set Barnyard='" + myDataReader.GetInt32(4).ToString() + "' WHERE store='" + Storerow[0] + " Thursday PU'  Update summary set Barnyard='" + myDataReader.GetInt32(5).ToString() + "' WHERE store= '" + Storerow[0] + " Friday PU' Update summary set Barnyard='" + myDataReader.GetInt32(6).ToString() + "' WHERE store= '" + Storerow[0] + " Saturday PU' Update summary set Barnyard='" + myDataReader.GetInt32(7).ToString() + "' WHERE store= '" + Storerow[0] + " NPUs' ";
                        sql += "Update summary set Ploughman='" + myDataReader.GetInt32(8).ToString() + "' WHERE store='" + Storerow[0] + " Thursday PU'  Update summary set Ploughman='" + myDataReader.GetInt32(9).ToString() + "' WHERE store= '" + Storerow[0] + " Friday PU' Update summary set Ploughman='" + myDataReader.GetInt32(10).ToString() + "' WHERE store= '" + Storerow[0] + " Saturday PU' Update summary set Ploughman='" + myDataReader.GetInt32(11).ToString() + "' WHERE store= '" + Storerow[0] + " NPUs' ";
                        sql += "Update summary set Bounty='" + myDataReader.GetInt32(12).ToString() + "' WHERE store= '" + Storerow[0] + " Vacation' Update summary set Barnyard='" + myDataReader.GetInt32(13).ToString() + "' WHERE store= '" + Storerow[0] + " Vacation' Update summary set Ploughman='" + myDataReader.GetInt32(14).ToString() + "' WHERE store= '" + Storerow[0] + " Vacation' Update summary set Total='" + (Convert.ToInt32(myDataReader.GetInt32(12)) + Convert.ToInt32(myDataReader.GetInt32(13)) + Convert.ToInt32(myDataReader.GetInt32(14))).ToString() + "' WHERE store= '" + Storerow[0] + " Vacation'";
                        sql += "Update summary set Bounty='" + (Convert.ToInt32(myDataReader.GetInt32(0)) + Convert.ToInt32(myDataReader.GetInt32(1)) + Convert.ToInt32(myDataReader.GetInt32(2))).ToString() + "' WHERE store='" + Storerow[0] + "' Update summary set Barnyard='" + (Convert.ToInt32(myDataReader.GetInt32(4)) + Convert.ToInt32(myDataReader.GetInt32(5)) + Convert.ToInt32(myDataReader.GetInt32(6))).ToString() + "' WHERE store='" + Storerow[0] + "' Update summary set Ploughman='" + (Convert.ToInt32(myDataReader.GetInt32(8)) + Convert.ToInt32(myDataReader.GetInt32(9)) + Convert.ToInt32(myDataReader.GetInt32(10))).ToString() + "' WHERE store='" + Storerow[0] + "'";
                        sql += "Update summary set Total='" + (Convert.ToInt32(myDataReader.GetInt32(0)) + Convert.ToInt32(myDataReader.GetInt32(1)) + Convert.ToInt32(myDataReader.GetInt32(2)) + Convert.ToInt32(myDataReader.GetInt32(4)) + Convert.ToInt32(myDataReader.GetInt32(5)) + Convert.ToInt32(myDataReader.GetInt32(6)) + Convert.ToInt32(myDataReader.GetInt32(8)) + Convert.ToInt32(myDataReader.GetInt32(9)) + Convert.ToInt32(myDataReader.GetInt32(10))).ToString() + "' WHERE store='" + Storerow[0] + "' Update summary set Total='" + (Convert.ToInt32(myDataReader.GetInt32(0)) + Convert.ToInt32(myDataReader.GetInt32(4)) + Convert.ToInt32(myDataReader.GetInt32(8)) + Convert.ToInt32(myDataReader.GetInt32(12))).ToString() + "' WHERE store='" + Storerow[0] + " Thursday PU' Update summary set Total='" + (Convert.ToInt32(myDataReader.GetInt32(1)) + Convert.ToInt32(myDataReader.GetInt32(5)) + Convert.ToInt32(myDataReader.GetInt32(9)) + Convert.ToInt32(myDataReader.GetInt32(13))) + "' WHERE store='" + Storerow[0] + " Friday PU' Update summary set Total='" + (Convert.ToInt32(myDataReader.GetInt32(2)) + Convert.ToInt32(myDataReader.GetInt32(6)) + Convert.ToInt32(myDataReader.GetInt32(10)) + Convert.ToInt32(myDataReader.GetInt32(14))) + "' WHERE store='" + Storerow[0] + " Saturday PU' Update summary set Total='" + (Convert.ToInt32(myDataReader.GetInt32(3)) + Convert.ToInt32(myDataReader.GetInt32(7)) + Convert.ToInt32(myDataReader.GetInt32(11))) + "' WHERE store='" + Storerow[0] + " NPUs'";
                        conn = new SqlConnection(ConnectionString);

                        conn.Open();
                        cmd = new SqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                myDataReader.Close();
            }
        }
        //SqlConnection cn = Constant.Connection();
        //SqlDataAdapter da=new SqlDataAdapter(

        GridView1.DataBind();

        

    }
    public DataTable FillStoreInfo()
    {
        DataTable dtStores = new DataTable();
        dtStores.Columns.Add("Store");
        //Create Rows in DataTable
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        try
        {
            using (mySqlConnection)
            {
                mySqlCommand = new SqlCommand("SELECT Store FROM Stores", mySqlConnection);
                mySqlConnection.Open();

                myDataReader = mySqlCommand.ExecuteReader();

                if (myDataReader.HasRows)
                {
                    while (myDataReader.Read())
                    {
                        dtStores.Rows.Add(myDataReader.GetString(0));
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
        return dtStores;
    }
    public DataTable FillDayInfo()
    {
        DataTable dtDays = new DataTable();
        dtDays.Columns.Add("PickupDay");
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
                        dtDays.Rows.Add(myDataReader.GetString(0));
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
        return dtDays;
    }

    protected void WeekList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!(WeekList.SelectedValue == " - Select a Week - "))
        {
            UpdateProgressContext();
            FillInfo();
        }
        else
        {
            GridView1.Visible = false;
        }
    }
    private void UpdateProgressContext()
    {
        const int Total = 67;

        RadProgressContext progress = RadProgressContext.Current;
        //progress.Speed = "N/A"

        for (int i = 0; i <= Total - 1; i++)
        {
            progress.PrimaryTotal = 1;
            progress.PrimaryValue = 1;
            progress.PrimaryPercent = 100;

            progress.SecondaryTotal = Total;
            progress.SecondaryValue = i;
            progress.SecondaryPercent = i;

            progress.CurrentOperationText = "Step " + i.ToString();

            if (!Response.IsClientConnected)
            {
                //Cancel button was clicked or the browser was closed, so stop processing
                break; // TODO: might not be correct. Was : Exit For
            }

            progress.TimeEstimated = (Total - i) * 100;
            //Stall the current thread for 0.1 seconds
            System.Threading.Thread.Sleep(100);
        }
    }


    protected void DownloadButton_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        string fileName = "Weekly_Report_for_" + WeekList.SelectedValue.ToString();
        Response.AddHeader("content-disposition", (Convert.ToString("attachment;filename=") + fileName) + ".xls");
        Response.ContentEncoding = System.Text.Encoding.Unicode;
        Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView1.Visible = true;
            GridView1.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string box = e.Row.Cells[0].Text;
            foreach (TableCell cell in e.Row.Cells)
            {
                cell.Visible = false;
                foreach (DataRow Dayrow_loopVariable in FillDayInfo().Rows)
                {
                    DataRow Dayrow = Dayrow_loopVariable;
                    if (box.Contains(Dayrow[0].ToString()))
                    {
                        cell.Visible = true;
                    }
                }
                foreach (DataRow Storerow_loopVariable in FillStoreInfo().Rows)
                {
                    DataRow Storerow = Storerow_loopVariable;
                    if (box == Storerow[0].ToString())
                    {
                        cell.Visible = true;
                    }
                }
                if (box.Contains("NPUs"))
                {
                    cell.Visible = true;
                }
                if (box.Contains("Vacation"))
                {
                    cell.Visible = true;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Int32 Bounty = Convert.ToInt32(e.Row.Cells[1].Text);
                Int32 Barnyard = Convert.ToInt32(e.Row.Cells[2].Text);
                Int32 Ploughman = Convert.ToInt32(e.Row.Cells[3].Text);
                Int32 Total = Bounty + Barnyard + Ploughman;
                e.Row.Cells[4].Text = Convert.ToString(Total);
            }
        }
    }


}