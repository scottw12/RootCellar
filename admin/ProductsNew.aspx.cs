using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Telerik.Web.UI;
using System.Configuration;
using System.Text;

public partial class admin_ProductsNew : System.Web.UI.Page
{
    static string FName = string.Empty, FPath = string.Empty, ProductID = string.Empty;
    static int i = 0;
    string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    /************SalesVu************/
    string APIKey = "a662c77bd1c244eb3440a3aa9dedc5bb";
    string url = "https://www.salesvu.com/townvu/api/index.php?request=";

    protected void Page_Load(object sender, EventArgs e)
    {
        i = 0;
        SqlConnection cn = Constant.Connection();
        if (!IsPostBack)
        {
            FillWeekInfo();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM AllowAccess where UserID='" + Session[Constant.UserID].ToString() + "'", cn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["AddProducts"].ToString() != "True")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Error();", true);
                    return;
                }

                else
                {
                    //SqlConnection cn = Constant.Connection();
                    FillRepeater(cn);
                    FillGridview();
                    if (Request.QueryString["ProductID"] != null)
                    {
                        LoadData(cn);
                    }
                }
            }
        }

    }

    private void LoadData(SqlConnection cn)
    {
       
        ProductID = Request.QueryString["ProductID"].ToString();
       
        SqlDataAdapter da2 = new SqlDataAdapter("SELECT [StoreID],[IsActive],[ProductID],[Store],[StartDay],[EndDay],Convert(nvarchar(50),[StartTime],100) as [StartTime],Convert(nvarchar(50),[EndTime],100) as [EndTime],[StartDayNo],[EndDayNo],[HomeDelivery]FROM [dbo].[ProductStoreDetails] where ProductID='" + ProductID + "'", cn);
        DataSet ds2 = new DataSet();
        da2.Fill(ds2);

        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProductDetailsNew where ProductID='" + ProductID + "'", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            rpStoreInfoEdit.Visible = true;
            rpStoreInfo.Visible = false;
            txtProducts.Text = ds.Tables[0].Rows[0]["ProductName"].ToString();
            txtPDescription.Text = ds.Tables[0].Rows[0]["ProductDescription"].ToString();
            txtPPrice.Text = ds.Tables[0].Rows[0]["ProductPrice"].ToString();
            if (ds.Tables[0].Rows[0]["PayOnline"].ToString() == "True")
                cbPayOnline.Checked = true;
            if (ds.Tables[0].Rows[0]["PayInShop"].ToString() == "True")
                cbInShop.Checked = true;

            //ddlWeek.SelectedValue = ds.Tables[0].Rows[0]["Week"].ToString();
            string strValue = ds.Tables[0].Rows[0]["Week"].ToString();
            string[] Week = strValue.Split(',');
            
            foreach (RadComboBoxItem item1 in ddlWeek.Items)
            {
                foreach (var item2 in Week)
                {
                    if (item1.Text==item2)
                    {
                        item1.Checked = true;
                    }
                }
            }
            txtQuantity.Text = ds.Tables[0].Rows[0]["Quantity"].ToString();

            Session["TempEdit"] = ds2.Tables[0];
            rpStoreInfoEdit.DataSource = ds2.Tables[0];
            rpStoreInfoEdit.DataBind();
            Session["TempEdit"] = null;

        }
    }
    protected void FillWeekInfo()
    {

        SqlDataReader myDataReader2 = default(SqlDataReader);
        SqlConnection mySqlConnection2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        SqlCommand mySqlCommand2 = default(SqlCommand);
        string SDateRange = "";
        string query = "select Sstart, send from seasons where currents='true'";
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (mySqlConnection2)
                {
                    mySqlCommand2 = new SqlCommand(query, mySqlConnection2);
                    mySqlConnection2.Open();
                    myDataReader2 = mySqlCommand2.ExecuteReader();
                    if (myDataReader2.HasRows)
                    {
                        while (myDataReader2.Read())
                        {
                            SDateRange = "where week>'" + myDataReader2.GetDateTime(0) + "' and week <= '" + myDataReader2.GetDateTime(1) + "' ";
                        }
                    }
                    myDataReader2.Close();
                }
            }
        }
        finally
        {
        }
        DataTable dt = new DataTable();
        dt.Columns.Add("Week");
        //dt.Rows.Add(" - Select a Week - ");
        //Create Rows in DataTable
        SqlDataReader myDataReader = default(SqlDataReader);
        SqlConnection mySqlConnection = default(SqlConnection);
        SqlCommand mySqlCommand = default(SqlCommand);
        mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        try
        {
            using (mySqlConnection)
            {
                mySqlCommand = new SqlCommand("SELECT DISTINCT Week FROM weekly " + SDateRange + " order by week", mySqlConnection);
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
                    dt.Rows.RemoveAt(1);
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
        this.ddlWeek.DataSource = dt;
        this.ddlWeek.DataTextField = "Week";
        this.ddlWeek.DataValueField = "Week";
        this.ddlWeek.DataBind();
    }

    private void FillGridview()
    {
        SqlConnection cn = Constant.Connection();
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProductDetailsNew ORDER BY ProductName", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvProduct.Visible = true;
            gvProduct.DataSource = ds.Tables[0];
            gvProduct.DataBind();
        }
        else
        {
            gvProduct.Visible = false;
        }
    }

    private void FillRepeater(SqlConnection cn)
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Stores", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            rpStoreInfo.DataSource = ds.Tables[0];
            rpStoreInfo.DataBind();
        }
    }
    protected void btnUploadImage_Click(object sender, EventArgs e)
    {
        if (fuPImage.HasFile)
        {
            string[] validFileTypes = { "bmp", "gif", "png", "jpg", "jpeg" };
            string ext = System.IO.Path.GetExtension(fuPImage.PostedFile.FileName);
            bool isValidFile = false;
            for (int i = 0; i < validFileTypes.Length; i++)
            {
                if (ext == "." + validFileTypes[i])
                {
                    isValidFile = true;
                    break;
                }
            }
            if (!isValidFile)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Upload Only Image Files')", true);
                return;
            }
            else
            {
                FName = Path.GetFileName(fuPImage.PostedFile.FileName);
                fuPImage.PostedFile.SaveAs(Server.MapPath("~/admin/ProductsImage/") + FName);
                FPath = "~/ProductsImage/" + FName;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Image Uploaded Successfully')", true);
            }
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select The File')", true);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlConnection cn = Constant.Connection();
        int ProductID;
        string webAddr = "";
        bool checkForList = false;
        var sb = new StringBuilder();
        var collection = ddlWeek.CheckedItems;
        foreach (var item in collection)
            if (sb.ToString() == string.Empty)
                sb.Append(item.Text);
            else
                sb.Append("," + item.Text);
        if (sb.ToString()==string.Empty)
        {
             ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Atleast One Week')", true);
            return;
        }
        if (!(cbInShop.Checked || cbPayOnline.Checked))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Atleast One Payment Type')", true);
            return;
        }
        if (Request.QueryString["ProductID"] == null)
        {
            foreach (RepeaterItem item in rpStoreInfo.Items)
            {
                CheckBox cbStoreName = (CheckBox)item.FindControl("cbStoreName");
                RadTimePicker rtpStartTime = (RadTimePicker)item.FindControl("rtpStartTime");
                RadTimePicker rtpEndTime = (RadTimePicker)item.FindControl("rtpEndTime");
                DropDownList ddlStartDay = (DropDownList)item.FindControl("ddlStartDay");
                DropDownList ddlEndDay = (DropDownList)item.FindControl("ddlEndDay");

                if (cbStoreName.Checked)
                {
                    if (Convert.ToInt32(ddlStartDay.SelectedValue) > Convert.ToInt32(ddlEndDay.SelectedValue))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Start day must be greater than End day.')", true);
                        return;
                    }
                    checkForList = true;
                    if (rtpStartTime.SelectedTime == null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select start time')", true);
                        return;
                    }
                    if (rtpEndTime.SelectedTime == null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select end time')", true);
                        return;
                    }
                }
            }
            if (checkForList == false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select atleast one store')", true);
                return;
            }
            try
            {
                //Add data into ProductDetailsNew  

                cn.Open();

                string query = "Insert into ProductDetailsNew values (@ProductName,@ProductDescription,@ProductImage,@ImagePath,@ProductPrice,@PayOnline,@PayInShop,@Week,@Quantity);SELECT CAST(scope_identity() AS int)";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@ProductName", txtProducts.Text);
                cmd.Parameters.AddWithValue("@ProductDescription", txtPDescription.Text);
                cmd.Parameters.AddWithValue("@ProductImage", FName);
                cmd.Parameters.AddWithValue("@ImagePath", FPath);
                cmd.Parameters.AddWithValue("@ProductPrice", txtPPrice.Text);
                if (cbPayOnline.Checked)
                    cmd.Parameters.AddWithValue("@PayOnline", true);
                else
                    cmd.Parameters.AddWithValue("@PayOnline", false);
                if (cbInShop.Checked)
                    cmd.Parameters.AddWithValue("@PayInShop", true);
                else
                    cmd.Parameters.AddWithValue("@PayInShop", false);
                cmd.Parameters.AddWithValue("@Week", sb.ToString());
                cmd.Parameters.AddWithValue("@Quantity", txtQuantity.Text);

                ProductID = (int)cmd.ExecuteScalar();

                foreach (RepeaterItem item in rpStoreInfo.Items)
                {
                    //Add data into ProductStoreDetails
                    CheckBox cbStoreName = (CheckBox)item.FindControl("cbStoreName");
                    DropDownList ddlStartDay = (DropDownList)item.FindControl("ddlStartDay");
                    DropDownList ddlEndDay = (DropDownList)item.FindControl("ddlEndDay");

                    RadTimePicker rtpStartTime = (RadTimePicker)item.FindControl("rtpStartTime");
                    RadTimePicker rtpEndTime = (RadTimePicker)item.FindControl("rtpEndTime");

                    CheckBox cbThu = (CheckBox)item.FindControl("cbThu");
                    CheckBox cbFri = (CheckBox)item.FindControl("cbFri");
                    CheckBox cbHomeDelivery = (CheckBox)item.FindControl("cbHomeDelivery");

                    if (cbStoreName.Checked)
                    {

                        SqlCommand cmd2 = new SqlCommand("Insert into ProductStoreDetails([IsActive],[ProductID],[Store],[StartDay],[EndDay],[StartTime],[EndTime],[StartDayNo],[EndDayNo],[HomeDelivery]) values (@IsActive,@ProductID,@Store,@StartDay,@EndDay,@StartTime,@EndTime,@StartDayNo,@EndDayNo,@HomeDelivery)", cn);
                        cmd2.Parameters.AddWithValue("@IsActive", true);
                        cmd2.Parameters.AddWithValue("@ProductID", ProductID);
                        cmd2.Parameters.AddWithValue("@Store", cbStoreName.Text);
                        cmd2.Parameters.AddWithValue("@StartDay", ddlStartDay.SelectedItem.Text);
                        cmd2.Parameters.AddWithValue("@EndDay", ddlEndDay.SelectedItem.Text);
                        cmd2.Parameters.AddWithValue("@StartTime", rtpStartTime.SelectedTime);
                        cmd2.Parameters.AddWithValue("@EndTime", rtpEndTime.SelectedTime);
                        cmd2.Parameters.AddWithValue("@StartDayNo", ddlStartDay.SelectedValue);
                        cmd2.Parameters.AddWithValue("@EndDayNo", ddlEndDay.SelectedValue);
                        cmd2.Parameters.AddWithValue("@HomeDelivery", true);
                        cmd2.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand cmd2 = new SqlCommand("Insert into ProductStoreDetails([ProductID],[Store]) values (@ProductID,@Store)", cn);
                        cmd2.Parameters.AddWithValue("@ProductID", ProductID);
                        cmd2.Parameters.AddWithValue("@Store", cbStoreName.Text);
                        cmd2.ExecuteNonQuery();
                    }
                }
                /*************SalesVu*****************/
                //webAddr += "{'api_key':'" + APIKey + "',";
                //webAddr += "'action':'add_product',";
                //webAddr += "'store_id':'" + StoreID + "',";
                //webAddr += "'online_customer_id':'1'";
                //webAddr += "'order_id':'____'";
                //webAddr += "'order_details':[{";
                //webAddr += "'product_id':'____'";

                ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Save();", true);
            }
            catch (Exception err)
            {
            }

           

        }
        else //Update
        {
            foreach (RepeaterItem item in rpStoreInfoEdit.Items)
            {
                CheckBox cbStoreName = (CheckBox)item.FindControl("cbStoreName");
                RadTimePicker rtpStartTime = (RadTimePicker)item.FindControl("rtpStartTime");
                RadTimePicker rtpEndTime = (RadTimePicker)item.FindControl("rtpEndTime");
                DropDownList ddlStartDay = (DropDownList)item.FindControl("ddlStartDay");
                DropDownList ddlEndDay = (DropDownList)item.FindControl("ddlEndDay");

                if (cbStoreName.Checked)
                {
                    if (Convert.ToInt32(ddlStartDay.SelectedValue) > Convert.ToInt32(ddlEndDay.SelectedValue))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Start day must be greater than End day.')", true);
                        return;
                    }
                    checkForList = true;
                    if (rtpStartTime.SelectedTime == null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select start time')", true);
                        return;
                    }
                    if (rtpEndTime.SelectedTime == null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select end time')", true);
                        return;
                    }
                }
            }
            if (checkForList == false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select atleast one store')", true);
                return;
            }
            try
            {
                //Update data into ProductDetailsNew        
                ProductID = Convert.ToInt32(Request.QueryString["ProductID"]);

                cn.Open();
                string query = "Update ProductDetailsNew set ProductName=@ProductName,ProductDescription=@ProductDescription,ProductImage=@ProductImage,ImagePath=@ImagePath,ProductPrice=@ProductPrice,PayOnline=@PayOnline,PayInShop=@PayInShop,Week=@Week,Quantity=@Quantity where ProductID='" + ProductID + "'";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@ProductName", txtProducts.Text);
                cmd.Parameters.AddWithValue("@ProductDescription", txtPDescription.Text);
                cmd.Parameters.AddWithValue("@ProductImage", FName);
                cmd.Parameters.AddWithValue("@ImagePath", FPath);
                cmd.Parameters.AddWithValue("@ProductPrice", txtPPrice.Text);
                if (cbPayOnline.Checked)
                    cmd.Parameters.AddWithValue("@PayOnline", true);
                else
                    cmd.Parameters.AddWithValue("@PayOnline", false);
                if (cbInShop.Checked)
                    cmd.Parameters.AddWithValue("@PayInShop", true);
                else
                    cmd.Parameters.AddWithValue("@PayInShop", false);
                //cmd.Parameters.AddWithValue("@Week", ddlWeek.SelectedValue);
                cmd.Parameters.AddWithValue("@Week", sb.ToString());
                cmd.Parameters.AddWithValue("@Quantity", txtQuantity.Text);

                cmd.ExecuteScalar();

                foreach (RepeaterItem item in rpStoreInfoEdit.Items)
                {
                    //Add data into ProductStoreDetails
                    Label lblStoreID = (Label)item.FindControl("lblStoreID");
                    CheckBox cbStoreName = (CheckBox)item.FindControl("cbStoreName");
                    DropDownList ddlStartDay = (DropDownList)item.FindControl("ddlStartDay");
                    DropDownList ddlEndDay = (DropDownList)item.FindControl("ddlEndDay");

                    RadTimePicker rtpStartTime = (RadTimePicker)item.FindControl("rtpStartTime");
                    RadTimePicker rtpEndTime = (RadTimePicker)item.FindControl("rtpEndTime");

                    CheckBox cbThu = (CheckBox)item.FindControl("cbThu");
                    CheckBox cbFri = (CheckBox)item.FindControl("cbFri");
                    CheckBox cbHomeDelivery = (CheckBox)item.FindControl("cbHomeDelivery");

                    if (cbStoreName.Checked)
                    {
                        SqlCommand cmd2 = new SqlCommand("Update ProductStoreDetails Set IsActive=@IsActive, StartDay=@StartDay,EndDay=@EndDay,StartTime=@StartTime,EndTime=@EndTime,StartDayNo=@StartDayNo,EndDayNo=@EndDayNo where StoreID='" + lblStoreID.Text + "'", cn);
                        cmd2.Parameters.AddWithValue("@IsActive", true);
                        cmd2.Parameters.AddWithValue("@StartDay", ddlStartDay.SelectedItem.Text);
                        cmd2.Parameters.AddWithValue("@EndDay", ddlEndDay.SelectedItem.Text);
                        cmd2.Parameters.AddWithValue("@StartTime", rtpStartTime.SelectedTime);
                        cmd2.Parameters.AddWithValue("@EndTime", rtpEndTime.SelectedTime);
                        cmd2.Parameters.AddWithValue("@StartDayNo", ddlStartDay.SelectedValue);
                        cmd2.Parameters.AddWithValue("@EndDayNo", ddlEndDay.SelectedValue);
                        cmd2.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand cmd2 = new SqlCommand("Update ProductStoreDetails Set IsActive=@IsActive, StartDay=@StartDay,EndDay=@EndDay,StartTime=@StartTime,EndTime=@EndTime,StartDayNo=@StartDayNo,EndDayNo=@EndDayNo where StoreID='" + lblStoreID.Text + "'", cn);
                        cmd2.Parameters.AddWithValue("@IsActive", false);
                        cmd2.Parameters.AddWithValue("@StartDay", DBNull.Value);
                        cmd2.Parameters.AddWithValue("@EndDay", DBNull.Value);
                        cmd2.Parameters.AddWithValue("@StartTime", DBNull.Value);
                        cmd2.Parameters.AddWithValue("@EndTime", DBNull.Value);
                        cmd2.Parameters.AddWithValue("@StartDayNo", DBNull.Value);
                        cmd2.Parameters.AddWithValue("@EndDayNo", DBNull.Value);
                        cmd2.ExecuteNonQuery();
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Update();", true);
            }
            catch (Exception err)
            {
            }
        }
        FillGridview();
    }
    protected void gvProduct_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete1")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            SqlConnection cn = Constant.Connection();
            SqlCommand cmd = new SqlCommand("Delete From ProductStoreDetails where ProductID=" + index + "", cn);
            SqlCommand cmd2 = new SqlCommand("Delete From ProductDetailsNew where ProductID=" + index + "", cn);
            cn.Open();
            cmd.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
            cn.Close();
            Response.Redirect("~/Admin/ProductsNew.aspx");
        }

        if (e.CommandName == "Edit1")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            Response.Redirect("~/Admin/ProductsNew.aspx?ProductID=" + index);
        }
    }
    protected void rpStoreInfoEdit_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //if (Request.QueryString["ProductID"] != null)
        //{
        //SqlConnection cn = Constant.Connection();
        ////ProductID = EncryptDecrypt.DecryptPassword(Request.QueryString["ProductID"].ToString());
        //ProductID = Request.QueryString["ProductID"].ToString();
        //SqlDataAdapter da2 = new SqlDataAdapter("SELECT * FROM ProductStoreDetails where ProductID='" + ProductID + "'", cn);
        //DataSet ds2 = new DataSet();
        //da2.Fill(ds2);
        //rpStoreInfoEdit.DataSource = ds2.Tables[0];
        //rpStoreInfoEdit.DataBind();

        //}
        RepeaterItem item = e.Item;
        if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
        {
            Label lblStoreID = (Label)e.Item.FindControl("lblStoreID");
            DropDownList ddlStartDay = (DropDownList)e.Item.FindControl("ddlStartDay");
            DropDownList ddlEndDay = (DropDownList)e.Item.FindControl("ddlEndDay");
            RadTimePicker rtpStartTime = (RadTimePicker)e.Item.FindControl("rtpStartTime");
            RadTimePicker rtpEndTime = (RadTimePicker)e.Item.FindControl("rtpEndTime");
            CheckBox cbStoreName = (CheckBox)e.Item.FindControl("cbStoreName");

            DataTable dt = (DataTable)Session["TempEdit"];
            //DataSet ds=new DataSet();
            //ds.Tables.Add(dt);
            if (dt.Rows[i]["IsActive"].ToString() == "True")
            {
                cbStoreName.Checked = true;
                ddlStartDay.SelectedValue = dt.Rows[i]["StartDayNo"].ToString();
                ddlEndDay.SelectedValue = dt.Rows[i]["EndDayNO"].ToString();
                rtpStartTime.SelectedTime = Convert.ToDateTime(dt.Rows[i]["StartTime"]).TimeOfDay;
                rtpEndTime.SelectedTime = Convert.ToDateTime(dt.Rows[i]["EndTime"]).TimeOfDay;
            }
            else
            {

            }
            i++;
        }
    }
    protected void gvProduct_Sorting(object sender, GridViewSortEventArgs e)
    {
        FillGridview();
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING);
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING);
        }
    }
    private void SortGridView(string sortExpression, string direction)
    {

        DataTable dt = gvProduct.DataSource as DataTable;
            DataView dv = new DataView(dt);
            dv.Sort = sortExpression + direction;

            gvProduct.DataSource = dv;
            gvProduct.DataBind();
      

    }
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set
        {
            ViewState["sortDirection"] = value;
        }
    }
    protected void gvProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProduct.PageIndex = e.NewPageIndex;
        gvProduct.DataBind();
       
    }
}