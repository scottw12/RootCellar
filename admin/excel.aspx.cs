using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI.Calendar.Utils;

public partial class admin_excel : System.Web.UI.Page
{


    private SqlConnection conn = null;
    private string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    private SqlCommand cmd = null;
    string password = "";
    string Username = "";
    string useremail = "";
    string firstname1 = "";
    string firstname2 = "";
    string lastname1 = "";
    string lastname2 = "";
    int i1 = 0;
    int i2 = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
            string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
            string FolderPath = ConfigurationManager.AppSettings["FolderPath"];

            string FilePath = Server.MapPath(FolderPath + FileName);
            FileUpload1.SaveAs(FilePath);
            Import_To_Grid(FilePath, Extension, rbHDR.SelectedItem.Text);
        }
    }


    protected void btnUpload_Click(object sender, System.EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
            string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
            string FolderPath = ConfigurationManager.AppSettings["FolderPath"];

            string FilePath = Server.MapPath(FolderPath + FileName);
            FileUpload1.SaveAs(FilePath);
            Import_To_Grid(FilePath, Extension, rbHDR.SelectedItem.Text);
        }
    }


    private void Import_To_Grid(string FilePath, string Extension, string isHDR)
    {
        string conStr = "";
        switch (Extension)
        {
            case ".xls":
                //Excel 97-03
                conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                break; // TODO: might not be correct. Was : Exit Select

                break;
            case ".xlsx":
                //Excel 07
                conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                break; // TODO: might not be correct. Was : Exit Select

                break;
        }
        conStr = string.Format(conStr, FilePath, isHDR);

        OleDbConnection connExcel = new OleDbConnection(conStr);
        OleDbCommand cmdExcel = new OleDbCommand();
        OleDbDataAdapter oda = new OleDbDataAdapter();
        DataTable dt = new DataTable();

        cmdExcel.Connection = connExcel;

        //Get the name of First Sheet
        connExcel.Open();
        DataTable dtExcelSchema = default(DataTable);
        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
        connExcel.Close();

        //Read Data from First Sheet
        connExcel.Open();
        cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
        oda.SelectCommand = cmdExcel;
        oda.Fill(dt);
        connExcel.Close();

        //Bind Data to GridView
        GridView1.Caption = Path.GetFileName(FilePath);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        DBInsert();

    }

    protected void PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
        string FileName = GridView1.Caption;
        string Extension = Path.GetExtension(FileName);
        string FilePath = Server.MapPath(FolderPath + FileName);

        Import_To_Grid(FilePath, Extension, rbHDR.SelectedItem.Text);
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
    }
    public bool DBInsert()
    {

        foreach (GridViewRow row in GridView1.Rows)
        {
            if (!string.IsNullOrEmpty(row.Cells[1].Text) | !(row.Cells[1].Text == "&nbsp;"))
            {

                try
                {
                    string query = "INSERT INTO subscribers (FirstName1, LastName1, Email1, phone1, FirstName2, LastName2, Email2, phone2, Address, City, State, Zip, Allergies, vacUsed, Newsletter, Enrolled, Referred, Notes, pickupday, store, bounty, barnyard, ploughman, username, active) VALUES (@FirstName1, @LastName1, @Email1, @phone1, @FirstName2, @LastName2, @Email2, @phone2, @Address, @City, @State, @Zip, @Allergies, @vacUsed, @Newsletter, @Enrolled, @Referred, @Notes, @pickupday, @store, @bounty, @barnyard, @ploughman, @Username, 'true') ";
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        using (SqlCommand comm = new SqlCommand())
                        {
                            var _with1 = comm;
                            _with1.Connection = conn;
                            _with1.CommandType = CommandType.Text;
                            _with1.CommandText = query;
                            string pattern = "(.*?)/(.*?)";
                            string replacement = "$1" + "\r\n";
                            Regex rgx = new Regex(pattern, RegexOptions.Singleline);
                            firstname1 = rgx.Replace(row.Cells[2].Text, replacement);
                            replacement = "$2" + "\r\n";
                            firstname2 = rgx.Replace(row.Cells[2].Text, replacement);
                            replacement = "$1" + "\r\n";
                            lastname1 = rgx.Replace(row.Cells[1].Text, replacement);
                            replacement = "$2" + "\r\n";
                            lastname2 = rgx.Replace(row.Cells[1].Text, replacement);


                            if (firstname1 == firstname2)
                            {
                                firstname2 = "";
                                lastname2 = "";
                            }
                            else
                            {
                                firstname1 = firstname1.Replace(firstname2.Trim(), "") + "\r\n";
                            }

                            comm.Parameters.Add("@FirstName1", SqlDbType.VarChar).Value = firstname1;
                            _with1.Parameters.Add("@LastName1", SqlDbType.VarChar).Value = lastname1;
                            _with1.Parameters.Add("@Email1", SqlDbType.VarChar).Value = row.Cells[12].Text;
                            _with1.Parameters.Add("@phone1", SqlDbType.VarChar).Value = row.Cells[11].Text;
                            _with1.Parameters.Add("@FirstName2", SqlDbType.VarChar).Value = firstname2;
                            _with1.Parameters.Add("@LastName2", SqlDbType.VarChar).Value = lastname2;
                            _with1.Parameters.Add("@Email2", SqlDbType.VarChar).Value = "";
                            _with1.Parameters.Add("@phone2", SqlDbType.VarChar).Value = "";
                            _with1.Parameters.Add("@Address", SqlDbType.VarChar).Value = row.Cells[13].Text;
                            _with1.Parameters.Add("@City", SqlDbType.VarChar).Value = row.Cells[14].Text;
                            _with1.Parameters.Add("@State", SqlDbType.VarChar).Value = row.Cells[15].Text;
                            _with1.Parameters.Add("@Zip", SqlDbType.VarChar).Value = row.Cells[16].Text;
                            _with1.Parameters.Add("@Allergies", SqlDbType.VarChar).Value = row.Cells[17].Text;
                            _with1.Parameters.Add("@vacUsed", SqlDbType.Int).Value = 0;
                            _with1.Parameters.Add("@Newsletter", SqlDbType.Bit).Value = false;
                            _with1.Parameters.Add("@Enrolled", SqlDbType.SmallDateTime).Value = System.DateTime.Now;
                            _with1.Parameters.Add("@Referred", SqlDbType.VarChar).Value = "";
                            _with1.Parameters.Add("@Notes", SqlDbType.Text).Value = "";
                            if (row.Cells[10].Text == "T")
                            {
                                _with1.Parameters.Add("@pickupday", SqlDbType.Text).Value = "Thursday";
                            }
                            else if (row.Cells[10].Text == "F")
                            {
                                _with1.Parameters.Add("@pickupday", SqlDbType.Text).Value = "Friday";
                            }
                            else
                            {
                                _with1.Parameters.Add("@pickupday", SqlDbType.Text).Value = "Friday";
                            }
                            _with1.Parameters.Add("@store", SqlDbType.Text).Value = "Downtown Columbia";
                            _with1.Parameters.Add("@username", SqlDbType.Text).Value = firstname1 + "." + lastname1;
                            if (row.Cells[8].Text == "1")
                            {
                                _with1.Parameters.Add("@barnyard", SqlDbType.Bit).Value = "True";
                            }
                            else
                            {
                                _with1.Parameters.Add("@barnyard", SqlDbType.Bit).Value = "False";
                            }
                            if (row.Cells[7].Text == "1")
                            {
                                _with1.Parameters.Add("@bounty", SqlDbType.Bit).Value = "True";
                            }
                            else
                            {
                                _with1.Parameters.Add("@bounty", SqlDbType.Bit).Value = "False";
                            }
                            if (row.Cells[9].Text == "1")
                            {
                                _with1.Parameters.Add("@ploughman", SqlDbType.Bit).Value = "True";
                            }
                            else
                            {
                                _with1.Parameters.Add("@ploughman", SqlDbType.Bit).Value = "False";
                            }

                            conn.Open();
                            comm.ExecuteNonQuery();
                            i1 += 1;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Literal1.Text += "<br />" + ex.Message + "<br /><br />" + ex.StackTrace;
                }
            }
        }
        Literal1.Text += "<br />" + "Step1 Complete " + i1.ToString() + " records!";
        DBInsert2();
        return true;
    }
    public bool DBInsert2()
    {
        int SubId = 0;
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        foreach (GridViewRow row in GridView1.Rows)
        {
            if (!string.IsNullOrEmpty(row.Cells[1].Text) | !(row.Cells[1].Text == "&nbsp;"))
            {
                string pattern = "(.*?)/(.*?)";
                string replacement = "$1" + "\r\n";
                Regex rgx = new Regex(pattern, RegexOptions.Singleline);
                firstname1 = rgx.Replace(row.Cells[2].Text, replacement);
                replacement = "$2" + "\r\n";
                firstname2 = rgx.Replace(row.Cells[2].Text, replacement);
                replacement = "$1" + "\r\n";
                lastname1 = rgx.Replace(row.Cells[1].Text, replacement);
                replacement = "$2" + "\r\n";
                lastname2 = rgx.Replace(row.Cells[1].Text, replacement);

                if (firstname1 == firstname2)
                {
                    firstname2 = "";
                    lastname2 = "";
                }
                else
                {
                    firstname1 = firstname1.Replace(firstname2.Trim(), "") + "\r\n";
                }
                mySqlCommand = new SqlCommand("SELECT SubID FROM subscribers Where FirstName1= '" + firstname1 + "' and address='" + row.Cells[13].Text + "'", mySqlConnection);
                try
                {
                    mySqlConnection.Open();
                    myDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    while ((myDataReader.Read()))
                    {
                        SubId = myDataReader.GetInt32(0);
                        string query = "INSERT INTO Weekly (SubId, bounty, barnyard, ploughman, PickupDay, Location, Vacation, PaidBounty, PaidBarnyard, PaidPloughman, Pickedup, Notes, Week) VALUES (@SubId, @bounty, @barnyard, @ploughman, @PickupDay, @Location, 'False', @PaidBounty, @PaidBarnyard, @PaidPloughman, 'False', '', @Week) ";
                        using (SqlConnection conn2 = new SqlConnection(ConnectionString))
                        {
                            using (SqlCommand comm2 = new SqlCommand())
                            {
                                var _with2 = comm2;
                                _with2.Connection = conn2;
                                _with2.CommandType = CommandType.Text;
                                _with2.CommandText = query;
                                comm2.Parameters.Add("@SubId", SqlDbType.Int).Value = SubId;
                                if (row.Cells[8].Text == "1")
                                {
                                    _with2.Parameters.Add("@barnyard", SqlDbType.Bit).Value = "True";
                                    _with2.Parameters.Add("@paidbarnyard", SqlDbType.Bit).Value = "True";
                                }
                                else
                                {
                                    _with2.Parameters.Add("@barnyard", SqlDbType.Bit).Value = "False";
                                    _with2.Parameters.Add("@paidbarnyard", SqlDbType.Bit).Value = "False";
                                }
                                if (row.Cells[7].Text == "1")
                                {
                                    _with2.Parameters.Add("@bounty", SqlDbType.Bit).Value = "True";
                                    _with2.Parameters.Add("@paidbounty", SqlDbType.Bit).Value = "True";
                                }
                                else
                                {
                                    _with2.Parameters.Add("@bounty", SqlDbType.Bit).Value = "False";
                                    _with2.Parameters.Add("@paidbounty", SqlDbType.Bit).Value = "False";
                                }
                                if (row.Cells[9].Text == "1")
                                {
                                    _with2.Parameters.Add("@ploughman", SqlDbType.Bit).Value = "True";
                                    _with2.Parameters.Add("@paidploughman", SqlDbType.Bit).Value = "True";
                                }
                                else
                                {
                                    _with2.Parameters.Add("@ploughman", SqlDbType.Bit).Value = "False";
                                    _with2.Parameters.Add("@paidploughman", SqlDbType.Bit).Value = "False";
                                }
                                if (row.Cells[10].Text == "T")
                                {
                                    _with2.Parameters.Add("@pickupday", SqlDbType.Text).Value = "Thursday";
                                }
                                else if (row.Cells[10].Text == "F")
                                {
                                    _with2.Parameters.Add("@pickupday", SqlDbType.Text).Value = "Friday";
                                }
                                else
                                {
                                    _with2.Parameters.Add("@pickupday", SqlDbType.Text).Value = "Friday";
                                }
                                _with2.Parameters.Add("@Location", SqlDbType.Text).Value = "Downtown Columbia";
                                _with2.Parameters.Add("@Week", SqlDbType.SmallDateTime).Value = "1/1/1900";
                                try
                                {
                                    conn2.Open();
                                    comm2.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                {
                                    Literal1.Text += "<br />" + ex.Message + "<br /><br />" + ex.StackTrace;
                                }
                            }
                        }
                        DateTime startDate = DateTime.Now;
                        DateTime endDate = Convert.ToDateTime("12/31/2018");
                        TimeSpan diff = endDate - startDate;
                        int days = diff.Days;
                        for (int i = 0; i <= days; i++)
                        {
                            dynamic testDate = startDate.AddDays(i);
                            //switch (testDate.DayOfWeek)
                            if (testDate.DayOfWeek == DayOfWeek.Thursday)
                            {
                                //case DayOfWeek.Thursday:
                                query = "INSERT INTO Weekly (SubId, bounty, barnyard, ploughman, PickupDay, Location, Vacation, PaidBounty, PaidBarnyard, PaidPloughman, Pickedup, Notes, Week) VALUES (@SubId, @bounty, @barnyard, @ploughman, @PickupDay, @Location, 'False', @PaidBounty, @PaidBarnyard, @PaidPloughman, 'False', '', @Week) ";
                                using (SqlConnection conn2 = new SqlConnection(ConnectionString))
                                {
                                    using (SqlCommand comm2 = new SqlCommand())
                                    {
                                        var _with3 = comm2;
                                        _with3.Connection = conn2;
                                        _with3.CommandType = CommandType.Text;
                                        _with3.CommandText = query;
                                        comm2.Parameters.Add("@SubId", SqlDbType.Int).Value = SubId;
                                        if (row.Cells[8].Text == "1")
                                        {
                                            _with3.Parameters.Add("@barnyard", SqlDbType.Bit).Value = "True";
                                        }
                                        else
                                        {
                                            _with3.Parameters.Add("@barnyard", SqlDbType.Bit).Value = "False";
                                        }
                                        if (row.Cells[7].Text == "1")
                                        {
                                            _with3.Parameters.Add("@bounty", SqlDbType.Bit).Value = "True";
                                        }
                                        else
                                        {
                                            _with3.Parameters.Add("@bounty", SqlDbType.Bit).Value = "False";
                                        }
                                        if (row.Cells[9].Text == "1")
                                        {
                                            _with3.Parameters.Add("@ploughman", SqlDbType.Bit).Value = "True";
                                        }
                                        else
                                        {
                                            _with3.Parameters.Add("@ploughman", SqlDbType.Bit).Value = "False";
                                        }
                                        if (row.Cells[10].Text == "T")
                                        {
                                            _with3.Parameters.Add("@pickupday", SqlDbType.Text).Value = "Thursday";
                                        }
                                        else if (row.Cells[10].Text == "F")
                                        {
                                            _with3.Parameters.Add("@pickupday", SqlDbType.Text).Value = "Friday";
                                        }
                                        else
                                        {
                                            _with3.Parameters.Add("@pickupday", SqlDbType.Text).Value = "Friday";
                                        }
                                        _with3.Parameters.Add("@Location", SqlDbType.Text).Value = "Downtown Columbia";
                                        _with3.Parameters.Add("@PaidBounty", SqlDbType.VarChar).Value = false;
                                        _with3.Parameters.Add("@PaidBarnyard", SqlDbType.VarChar).Value = false;
                                        _with3.Parameters.Add("@PaidPloughman", SqlDbType.VarChar).Value = false;
                                        _with3.Parameters.Add("@Week", SqlDbType.SmallDateTime).Value = testDate.ToShortDateString();
                                        try
                                        {
                                            conn2.Open();
                                            comm2.ExecuteNonQuery();
                                        }
                                        catch (SqlException ex)
                                        {
                                            Literal1.Text += "<br />" + ex.Message + "<br /><br />" + ex.StackTrace;
                                        }
                                    }
                                }
                                //break; // TODO: might not be correct. Was : Exit Select
                                //break;
                            }
                        }
                        i2 += 1;
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
        }
        Literal1.Text += "<br />" + "Step2 Complete " + i2.ToString() + " records!";
        return true;
    }

}