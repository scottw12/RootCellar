using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Configuration;

public partial class customer_ProductPay : System.Web.UI.Page
{
    static Double TotalPrice = 0.00;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            FillPaidInfo();
            ViewState["CurrentRecord"] = "Yes";
            TotalPrice = 0.00;
            if (Request.QueryString["View"] != null && Session["SelectedProducts"] == null)
            {
                btnPayment.Enabled = false;
                btnAddMoreItems.Enabled = false;
                btnPayInStore.Enabled = false;
                return;
            }
            else
            {
                btnPayment.Enabled = true;
                btnAddMoreItems.Enabled = true;
                btnPayInStore.Enabled = true;
            }
            if (Request.QueryString["ProductId"] != null)
            {

                DataTable SelectedProducts = Session["SelectedProducts"] as DataTable;
                btnAddMoreItems.Enabled = false;

                if (Request.QueryString["Week"] != null)
                {
                    //if (Request.QueryString["This"] != null && Session["SelectedProducts"] == null)
                    //    ddlWeeks.Enabled = true;
                    //else
                    //    ddlWeeks.Enabled = false;

                    ddlWeeks.SelectedValue = Request.QueryString["Week"].ToString();
                    IEnumerable<DataRow> dtRow = from myRow in SelectedProducts.AsEnumerable()
                                                 where myRow.Field<string>("Week") == ddlWeeks.SelectedValue.ToString()
                                                 select myRow;

                    if (dtRow.Count<DataRow>() > 0)
                    {
                        DataTable dtTemp = dtRow.CopyToDataTable();
                        gvSelectedProduct.DataSource = dtTemp;
                        gvSelectedProduct.DataBind();
                    }
                }
                else
                {
                    ddlWeeks.Enabled = true;
                    gvSelectedProduct.DataSource = SelectedProducts;
                    gvSelectedProduct.DataBind();
                }
                lblTotal.Text = TotalPrice.ToString();
            }
            else
            {

                Session["SelectedProductsPayment"] = Session["SelectedProducts"];
                Session["SPPayment"] = Session["SelectedProducts"];
                Session["SelectedProductsPayment"] = Session["addCard"];
                Session["SPPayment"] = Session["addCard"];
                DataTable SelectedProducts = (DataTable)Session["SelectedProducts"];
                // DataTable SelectedProducts = (DataTable)Session["addCard"];
                if (SelectedProducts.Rows.Count > 0)
                {
                    ddlWeeks.Enabled = true;
                    if (Request.QueryString["Week"] != null)
                    {
                        //if (Request.QueryString["This"] != null && Session["SelectedProducts"] == null)
                        //    ddlWeeks.Enabled = true;
                        //else
                        //    ddlWeeks.Enabled = false;

                        ddlWeeks.SelectedValue = Request.QueryString["Week"].ToString();
                        IEnumerable<DataRow> dtRow = from myRow in SelectedProducts.AsEnumerable()
                                                     where myRow.Field<string>("Week") == ddlWeeks.SelectedValue.ToString()
                                                     select myRow;

                        if (dtRow.Count<DataRow>() > 0)
                        {
                            DataTable dtTemp = dtRow.CopyToDataTable();
                            gvSelectedProduct.DataSource = dtTemp;
                            gvSelectedProduct.DataBind();
                        }
                    }
                    else
                    {
                        gvSelectedProduct.DataSource = SelectedProducts;
                        gvSelectedProduct.DataBind();
                    }
                    lblTotal.Text = TotalPrice.ToString();
                }
                if (Request.QueryString["MsgProduct"].ToString() != string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Product " + Request.QueryString["MsgProduct"].ToString() + " are not available at this time.');", true);
                }
            }
        }
    }
    string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    protected void FillPaidInfo()
    {
        try
        {
            SqlConnection cn = Constant.Connection();

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM subscribers Where Username= '" + Membership.GetUser().ToString() + "'", cn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            string UserID = ds.Tables[0].Rows[0]["SubID"].ToString();

            SqlDataReader myDataReader2 = default(SqlDataReader);
            SqlConnection mySqlConnection2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand mySqlCommand2 = default(SqlCommand);
            string SDateRange = "";
            string query = "select Sstart, send from seasons where currents='true'";
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
                            SDateRange = " and week >= '" + myDataReader2.GetDateTime(0) + "' and week <= '" + myDataReader2.GetDateTime(1) + "' ";
                        }
                    }
                    myDataReader2.Close();
                }
            }
            string SqlQuary = "SELECT SubId, Week, PaidBounty, PaidBarnyard, PaidPloughman, bounty, barnyard, ploughman FROM Weekly where subID='" + UserID.ToString() + "' and ((week='1/1/1900') or (week>='" + System.DateTime.Today.AddDays(-1) + "'" + SDateRange + ")) ORDER BY [Week]";
            DataTable dt = new DataTable();
            dt.Columns.Add("SubId");
            dt.Columns.Add("Week");
            dt.Columns.Add("PaidBounty");
            dt.Columns.Add("PaidBarnyard");
            dt.Columns.Add("PaidPloughman");
            SqlDataReader myDataReader = default(SqlDataReader);
            SqlConnection mySqlConnection = default(SqlConnection);
            SqlCommand mySqlCommand = default(SqlCommand);

            mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (mySqlConnection)
            {
                mySqlCommand = new SqlCommand(SqlQuary, mySqlConnection);
                mySqlConnection.Open();
                myDataReader = mySqlCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    string SubInfo = "";
                    string paid = "";
                    string pickedup = "";
                    string vacation = "";
                    int i = 0;
                    while (myDataReader.Read())
                    {
                        string week = "";
                        if (myDataReader.GetDateTime(1).ToString().Contains("1900"))
                        {
                            week = "Deposit";
                        }
                        else
                        {
                            week = (myDataReader.GetDateTime(1).Month.ToString() + "/" + myDataReader.GetDateTime(1).Day.ToString() + "-" + myDataReader.GetDateTime(1).AddDays(1).Day.ToString() + "/" + myDataReader.GetDateTime(1).Year.ToString());
                        }
                        if ((!myDataReader.IsDBNull(2) & !myDataReader.IsDBNull(3) & !myDataReader.IsDBNull(4)) && week != "Deposit")
                        {
                            dt.Rows.Add(myDataReader.GetInt32(0), week, myDataReader.GetBoolean(2), myDataReader.GetBoolean(3), myDataReader.GetBoolean(4));

                        }
                        else
                        {
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                myDataReader.Close();
            }


            ddlWeeks.DataSource = dt.DefaultView.ToTable(true, "Week");
            ddlWeeks.DataValueField = "Week";
            ddlWeeks.DataTextField = "Week";
            ddlWeeks.DataBind();
            ddlWeeks.Items.Insert(0, "Select Week");
        }
        catch (Exception ex)
        {

        }

    }
    protected void gvSelectedProduct_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label ProductId = e.Row.FindControl("ProductId") as Label;
            Label lblSRNO = e.Row.FindControl("lblSRNO") as Label;

            Label Total = e.Row.FindControl("lblTotalPrice") as Label;
            Label Quantity = e.Row.FindControl("lblQuantity") as Label;
            TextBox txtQuantity = e.Row.FindControl("txtQuantity") as TextBox;
            Double Price = Convert.ToDouble(e.Row.Cells[3].Text.TrimStart('$'));
            //int Quantity = Convert.ToInt32(e.Row.Cells[4].Text);
            if (Request.QueryString["ProductId"] != null)
            {
                if (Request.QueryString["ProductId"].ToString() == lblSRNO.Text)
                {
                    Quantity.Visible = false;
                    txtQuantity.Visible = true;
                    txtQuantity.Text = Quantity.Text;
                    Total.Text = (Price * Convert.ToInt32(Quantity.Text)).ToString("0.00");
                    TotalPrice = TotalPrice + Convert.ToDouble(Total.Text);
                }
                else
                {
                    Total.Text = (Price * Convert.ToInt32(Quantity.Text)).ToString("0.00");
                    TotalPrice = TotalPrice + Convert.ToDouble(Total.Text);
                }
            }
            else
            {
                Total.Text = (Price * Convert.ToInt32(Quantity.Text)).ToString("0.00");
                TotalPrice = TotalPrice + Convert.ToDouble(Total.Text);
            }
        }
    }
    protected void btnPayment_Click(object sender, EventArgs e)
    {
        if (ViewState["CurrentRecord"].ToString() == "No")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "al", "alert('Payment has been done for this week.'); ", true);
            return;
        }
        else
        {
            TotalPrice = 0;
            DataTable dtCompleteRecords = (DataTable)Session["SelectedProducts"];
            gvSelectedProduct.DataSource = dtCompleteRecords;
            gvSelectedProduct.DataBind();

            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add(new System.Data.DataColumn("ProductId", typeof(int)));
            dt.Columns.Add(new System.Data.DataColumn("ProductName", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("ProductPrice", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("Quantity", typeof(int)));
            dt.Columns.Add(new System.Data.DataColumn("TotalPrice", typeof(double)));

            foreach (GridViewRow row in gvSelectedProduct.Rows)
            {
                Label ProductId = (Label)row.FindControl("ProductId");
                string ProductName = row.Cells[2].Text;
                string ProductPrice = row.Cells[3].Text;
                Label Quantity = row.FindControl("lblQuantity") as Label;
                //string Quantity = row.Cells[4].Text;
                Label lblTotalPrice = (Label)row.FindControl("lblTotalPrice");
                dr = dt.NewRow();
                dr[0] = ProductId.Text;
                dr[1] = ProductName.ToString();
                dr[2] = ProductPrice.ToString();
                dr[3] = Quantity.Text;
                dr[4] = lblTotalPrice.Text;
                dt.Rows.Add(dr);
            }
            DataTable dt_TotalProduct = new DataTable();
            DataRow dr_totalPrice;
            DataTable SelectedProducts = (DataTable)Session["SelectedProducts"];
            dt_TotalProduct.Columns.Add(new System.Data.DataColumn("Week", typeof(string)));
            dt_TotalProduct.Columns.Add(new System.Data.DataColumn("Price", typeof(string)));
            dt_TotalProduct.Columns.Add(new System.Data.DataColumn("Link", typeof(string)));
            for (int i = 0; i < SelectedProducts.Rows.Count; i++)
            {
                dr_totalPrice = dt_TotalProduct.NewRow();
                dr_totalPrice[0] = SelectedProducts.Rows[i][5];
                dr_totalPrice[1] = dt.Rows[i][4];
                dr_totalPrice[2] = null;
                dt_TotalProduct.Rows.Add(dr_totalPrice);
            }
            Session["TotalProduct"] = dt_TotalProduct;

            Session["SPPayment"] = dt;
            Response.Redirect("~/account/Default.aspx?TotalPrice=" + TotalPrice);
        }
    }
    protected void btnAddMoreItems_Click(object sender, EventArgs e)
    {
        if (ViewState["CurrentRecord"].ToString() == "No")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "al", "alert('Payment has been done for this week.'); ", true);
            return;
        }
        Response.Redirect("~/Account/Default.aspx?Page1='Page1'");
    }

    /// <summary>
    /// added by harshal to delete record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvSelectedProduct_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (ViewState["CurrentRecord"].ToString() == "No")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "al", "alert('Payment has been done for this week. You can not modify this product.'); ", true);
            return;
        }

        if (e.CommandName == "Delete1")
        {
            DataTable SelectedProducts = (DataTable)Session["SelectedProducts"];
            if (SelectedProducts.Rows.Count > 0)
            {
                TotalPrice = 0.00;
                SelectedProducts.Rows.RemoveAt(Convert.ToInt32(e.CommandArgument) - 1);
                Session["SelectedProducts"] = SelectedProducts;
                gvSelectedProduct.DataSource = SelectedProducts;
                gvSelectedProduct.DataBind();
                lblTotal.Text = TotalPrice.ToString();
            }
        }
        else if (e.CommandName == "Update1")
        {
            SqlConnection cn = Constant.Connection();

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM subscribers Where Username= '" + Membership.GetUser().ToString() + "'", cn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            string UserID = ds.Tables[0].Rows[0]["SubID"].ToString();
            SqlDataAdapter da2 = new SqlDataAdapter("SELECT * FROM PurchaseProduct WHERE [SubscriberID]=" + UserID + " AND [Week]='" + ddlWeeks.SelectedValue + "'", cn);
            DataSet dsCheck = new DataSet();
            da2.Fill(dsCheck);
            if (dsCheck.Tables[0].Rows.Count > 0 && ddlWeeks.SelectedValue.ToString() != "Select Week")
            {
                if (Convert.ToBoolean(dsCheck.Tables[0].Rows[0]["OnlineHome"]) == true)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "al", "alert('This Weeks Order Has Been Submitted for Online Payment.')", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "al", "alert('This Weeks Order Has Been Submitted for Payment In Store.')", true);
                    return;
                }

            }
            GridViewRow row = gvSelectedProduct.Rows[Convert.ToInt32(e.CommandArgument)];

            Label ProductId = row.FindControl("ProductId") as Label;
            Label lblSRNO = row.FindControl("lblSRNO") as Label;

            Label Total = row.FindControl("lblTotalPrice") as Label;
            Label Quantity = row.FindControl("lblQuantity") as Label;
            TextBox txtQuantity = row.FindControl("txtQuantity") as TextBox;
            Double Price = Convert.ToDouble(row.Cells[3].Text.TrimStart('$'));
            HiddenField hfId = row.FindControl("hfId") as HiddenField;

            //if (Request.QueryString["ProductId"] != null)
            //{
            //    if (Request.QueryString["ProductId"].ToString() == lblSRNO.Text)
            //    {
            Quantity.Text = txtQuantity.Text;
            //txtQuantity.Visible = false;
            //Quantity.Visible = true;
            Total.Text = Convert.ToString(Price * Convert.ToInt32(Quantity.Text));
            //Response.Redirect("/account/Default?TotalPrice=" + Total.Text);

            DataTable SelectedProducts = (DataTable)Session["SelectedProducts"];
            for (int i = 1; i <= SelectedProducts.Rows.Count; i++)
            {

                if (SelectedProducts.Rows[i - 1]["Id"].ToString() == hfId.Value)
                {
                    SelectedProducts.Rows[i - 1]["Quantity"] = Quantity.Text;
                }


            }
            SelectedProducts.AcceptChanges();
            Session["SelectedProducts"] = SelectedProducts;
            IEnumerable<DataRow> dtRow = from myRow in SelectedProducts.AsEnumerable()
                                         where myRow.Field<string>("Week") == ddlWeeks.SelectedValue.ToString()
                                         select myRow;

            Label2.Text = "";

            if (dtRow.Count<DataRow>() > 0)
            {
                ViewState["CurrentRecord"] = "Yes";
                DataTable dtTemp = dtRow.CopyToDataTable();
                gvSelectedProduct.DataSource = dtTemp;
                gvSelectedProduct.DataBind();
            }
            else
            {
                ViewOldRecords();
            }
            //gvSelectedProduct.DataSource = SelectedProducts;
            //gvSelectedProduct.DataBind();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "al", "alert('Record updated successfully.')", true);
            return;
        }
    }
    /// <summary>
    /// Pay Products In Store
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPayInStore_Click(object sender, EventArgs e)
    {
        if (ViewState["CurrentRecord"].ToString() == "No")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "al", "alert('Payment has been done for this week.'); ", true);
            return;
        }
        try
        {
            Session["SelectedProductsPayment"] = Session["SelectedProducts"];
            #region Old Code
            //if (Session["SelectedProductsPayment"] != null)
            //{
            //    SqlConnection cn = Constant.Connection();
            //    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers where Username= '" + Membership.GetUser().ToString() + "'", cn);
            //    DataSet ds = new DataSet();
            //    da.Fill(ds);
            //    cn.Open();
            //    SqlCommand cmd = new SqlCommand("Insert into PurchaseProduct values(@SubscriberID,@PurchaseDate,@OnlineHome,@Store,@PickupDay,@Week,@PaymentMode,@IsPaid);SELECT CAST(scope_identity() AS int)", cn);
            //    cmd.Parameters.AddWithValue("@SubscriberID", Convert.ToInt32(ds.Tables[0].Rows[0]["SubId"]));
            //    cmd.Parameters.AddWithValue("@PurchaseDate", DateTime.Now);
            //    cmd.Parameters.AddWithValue("@OnlineHome", false);
            //    cmd.Parameters.AddWithValue("@Store", ds.Tables[0].Rows[0]["Store"].ToString());
            //    cmd.Parameters.AddWithValue("@PickupDay", ds.Tables[0].Rows[0]["PickupDay"].ToString());
            //    cmd.Parameters.AddWithValue("@PaymentMode", "Store");
            //    cmd.Parameters.AddWithValue("@IsPaid", "UnPaid");

            //    cmd.Parameters.AddWithValue("@Week", Session["ProductWeek"].ToString());
            //    int PurchaseProductID = (int)cmd.ExecuteScalar();

            //    cn.Close();
            //    cn.Open();

            //    DataSet dsSeleted = new DataSet();
            //    DataTable Newdt = (DataTable)Session["SelectedProductsPayment"];
            //    DataTable CopyNewdt = Newdt.Copy();
            //    dsSeleted.Tables.Add(CopyNewdt);
            //    // New Added By Harshal For Subtracting Total Quantity of product
            //    for (int i = 0; i < dsSeleted.Tables[0].Rows.Count; i++)
            //    {
            //        SqlDataAdapter da_Qua = new SqlDataAdapter("SELECT * FROM ProductDetailsNew WHERE ProductID='" + Convert.ToInt32(dsSeleted.Tables[0].Rows[i]["ProductID"]) + "'", cn);
            //        DataSet ds_Qua = new DataSet();
            //        da_Qua.Fill(ds_Qua);
            //        cn.Close();
            //        cn.Open();
            //        SqlCommand cmd_Qua = new SqlCommand("UPDATE ProductDetailsNew SET Quantity='" + (Convert.ToInt32(ds_Qua.Tables[0].Rows[0]["Quantity"]) - Convert.ToInt32(dsSeleted.Tables[0].Rows[i]["Quantity"])) + "' WHERE ProductID='" + Convert.ToInt32(dsSeleted.Tables[0].Rows[i]["ProductID"]) + "'", cn);
            //        //cmd_Qua.Parameters.AddWithValue("@Quantity", (Convert.ToInt32(ds_Qua.Tables[0].Rows[0]["Quantity"]) - Convert.ToInt32(dsSeleted.Tables[0].Rows[i]["Quantity"])));
            //        cmd_Qua.ExecuteNonQuery();
            //        cn.Close();
            //    }
            //    //
            //    for (int i = 0; i < dsSeleted.Tables[0].Rows.Count; i++)
            //    {
            //        SqlCommand cmd2 = new SqlCommand("Insert into PurchaseProductDetails values(@BuyId,@SubscriberID,@ProductID,@ProductName,@Price,@Quantity,@PaymentMode,@IsPaid)", cn);
            //        cmd2.Parameters.AddWithValue("@BuyId", PurchaseProductID);
            //        cmd2.Parameters.AddWithValue("@SubscriberID", Convert.ToInt32(ds.Tables[0].Rows[0]["SubId"]));
            //        cmd2.Parameters.AddWithValue("@ProductID", Convert.ToInt32(dsSeleted.Tables[0].Rows[i]["ProductID"]));
            //        cmd2.Parameters.AddWithValue("@ProductName", Convert.ToString(dsSeleted.Tables[0].Rows[i]["ProductName"]));
            //        cmd2.Parameters.AddWithValue("@Price", Convert.ToDouble(dsSeleted.Tables[0].Rows[i]["ProductPrice"]));
            //        cmd2.Parameters.AddWithValue("@Quantity", Convert.ToInt32(dsSeleted.Tables[0].Rows[i]["Quantity"]));
            //        cmd2.Parameters.AddWithValue("@PaymentMode", "Store");
            //        cmd2.Parameters.AddWithValue("@IsPaid", "UnPaid");
            //        cn.Open();
            //        cmd2.ExecuteNonQuery();
            //        cn.Close();

            //    }
            //    cn.Close();

            //} 
            #endregion
            #region Save To Store


            if (Session["SelectedProductsPayment"] != null)
            {
                SqlConnection cn = Constant.Connection();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Subscribers where Username= '" + Membership.GetUser().ToString() + "'", cn);
                DataSet ds = new DataSet();
                da.Fill(ds);

                DataTable dtSelPro = ((DataTable)Session["SelectedProducts"]).DefaultView.ToTable(true, "Week");

                for (int j = 1; j <= dtSelPro.Rows.Count; j++)
                {
                    cn.Open();
                    bool OnlineHome = false;
                    SqlCommand cmd = new SqlCommand("Insert into PurchaseProduct values(@SubscriberID,@PurchaseDate,@OnlineHome,@Store,@PickupDay,@Week,@PaymentMode,@IsPaid);SELECT CAST(scope_identity() AS int)", cn);
                    cmd.Parameters.AddWithValue("@SubscriberID", Convert.ToInt32(ds.Tables[0].Rows[0]["SubId"]));
                    cmd.Parameters.AddWithValue("@PurchaseDate", DateTime.Now);

                    cmd.Parameters.AddWithValue("@OnlineHome", false);//Home Delivery + Online Payment                       
                    cmd.Parameters.AddWithValue("@Store", ds.Tables[0].Rows[0]["Store"].ToString());
                    cmd.Parameters.AddWithValue("@PickupDay", ds.Tables[0].Rows[0]["PickupDay"].ToString());
                    cmd.Parameters.AddWithValue("@PaymentMode", "Store");
                    cmd.Parameters.AddWithValue("@IsPaid", "UnPaid");
                    cmd.Parameters.AddWithValue("@Week", dtSelPro.Rows[j - 1]["Week"]);
                    int PurchaseProductID = (int)cmd.ExecuteScalar();
                    Session["ProductWeek"] = null;
                    cn.Close();
                    cn.Open();

                    DataSet dsSeleted = new DataSet();
                    DataTable Newdt = (DataTable)Session["SelectedProducts"];
                    DataTable CopyNewdt = Newdt.Copy();
                    dsSeleted.Tables.Add(CopyNewdt);

                    for (int i = 0; i < dsSeleted.Tables[0].Rows.Count; i++)
                    {
                        if (dtSelPro.Rows[j - 1]["Week"] == dsSeleted.Tables[0].Rows[0]["Week"].ToString())
                        {
                            // New Added By Harshal For Subtracting Total Quantity of product
                            SqlDataAdapter da_Qua = new SqlDataAdapter("SELECT * FROM ProductDetailsNew WHERE ProductID='" + Convert.ToInt32(dsSeleted.Tables[0].Rows[j - 1]["ProductID"]) + "'", cn);
                            DataSet ds_Qua = new DataSet();
                            da_Qua.Fill(ds_Qua);
                            cn.Close();
                            cn.Open();
                            SqlCommand cmd_Qua = new SqlCommand("UPDATE ProductDetailsNew SET Quantity='" + (Convert.ToInt32(ds_Qua.Tables[0].Rows[0]["Quantity"]) - Convert.ToInt32(dsSeleted.Tables[0].Rows[j - 1]["Quantity"])) + "' WHERE ProductID='" + Convert.ToInt32(dsSeleted.Tables[0].Rows[j - 1]["ProductID"]) + "'", cn);
                            cmd_Qua.ExecuteNonQuery();
                            cn.Close();

                            SqlCommand cmd2 = new SqlCommand("Insert into PurchaseProductDetails values(@BuyId,@SubscriberID,@ProductID,@ProductName,@Price,@Quantity,@PaymentMode,@IsPaid)", cn);
                            cmd2.Parameters.AddWithValue("@BuyId", PurchaseProductID);
                            cmd2.Parameters.AddWithValue("@SubscriberID", Convert.ToInt32(ds.Tables[0].Rows[0]["SubId"]));
                            cmd2.Parameters.AddWithValue("@ProductID", Convert.ToInt32(dsSeleted.Tables[0].Rows[i]["ProductID"]));
                            cmd2.Parameters.AddWithValue("@ProductName", Convert.ToString(dsSeleted.Tables[0].Rows[i]["ProductName"]));
                            cmd2.Parameters.AddWithValue("@Price", Convert.ToDouble(dsSeleted.Tables[0].Rows[i]["ProductPrice"]));
                            cmd2.Parameters.AddWithValue("@Quantity", Convert.ToInt32(dsSeleted.Tables[0].Rows[i]["Quantity"]));
                            //cmd2.Parameters.AddWithValue("@PaymentMode", rblPayment.SelectedValue);
                            cmd2.Parameters.AddWithValue("@PaymentMode", "Store");
                            cmd2.Parameters.AddWithValue("@IsPaid", "UnPaid");

                            cn.Open();
                            cmd2.ExecuteNonQuery();
                            cn.Close();

                        }
                    }
                    cn.Close();
                }
            }

            #endregion
            #region Invoice

            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "al", "alert('Your order has been submitted please make payment at your weekly pick up.'); ", true);

            //using (StringWriter sw = new StringWriter())
            //{
            //    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            //    {
            //        gvSelectedProduct.AllowPaging = false;
            //        gvSelectedProduct.RenderControl(hw);
            //        StringReader sr = new StringReader(sw.ToString());
            //        Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
            //        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

            //        PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            //        pdfDoc.Open();
            //        pdfDoc.Add(new Paragraph("You have to pay $" + lblTotal.Text + " "));
            //        pdfDoc.Add(new Paragraph(" "));
            //        pdfDoc.Add(new Paragraph(" "));
            //        htmlparser.Parse(sr);

            //        pdfDoc.Close();

            //        Response.ContentType = "application/pdf";
            //        Response.AddHeader("content-disposition", "attachment;filename=Invoice.pdf");
            //        Response.Cache.SetCacheability(HttpCacheability.NoCache);                   
            //        Response.Write(pdfDoc);                    
            //        Response.End();
            //    }
            //} 
            #endregion

        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            Session["SelectedProductsPayment"] = null;
            DataTable dt = Session["SelectedProducts"] as DataTable;
            Session["SelectedProducts"] = null;
            dt.Clear();
            Session["SelectedProducts"] = dt;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "al", "alert('Your order has been submitted please make payment at your weekly pick up.'); window.location='../account/Default.aspx';", true);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    protected void btnInvoice_Click(object sender, EventArgs e)
    {
        if (ViewState["CurrentRecord"].ToString() == "No")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "al", "alert('Payment has been done for this week.'); ", true);
            return;
        }
        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            {
                gvSelectedProduct.AllowPaging = false;
                gvSelectedProduct.RenderControl(hw);
                StringReader sr = new StringReader(sw.ToString());
                Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                pdfDoc.Add(new Paragraph("You have to pay $" + lblTotal.Text + " "));
                pdfDoc.Add(new Paragraph(" "));
                pdfDoc.Add(new Paragraph(" "));
                htmlparser.Parse(sr);

                pdfDoc.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Invoice.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
            }
        }
        Session["SelectedProductsPayment"] = null;
        Session["SelectedProducts"] = null;
    }
    protected void ddlWeeks_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable SelectedProducts = Session["SelectedProducts"] as DataTable;
        if (SelectedProducts.Rows.Count > 0)
        {
            IEnumerable<DataRow> dtRow = from myRow in SelectedProducts.AsEnumerable()
                                         where myRow.Field<string>("Week") == ddlWeeks.SelectedValue.ToString()
                                         select myRow;

            Label2.Text = "";

            if (dtRow.Count<DataRow>() > 0)
            {
                ViewState["CurrentRecord"] = "Yes";
                DataTable dtTemp = dtRow.CopyToDataTable();
                gvSelectedProduct.DataSource = dtTemp;
                gvSelectedProduct.DataBind();
            }
            else
            {
                ViewOldRecords();
            }
        }
        else
        {
            ViewOldRecords();
        }
    }

    private void ViewOldRecords()
    {
        ViewState["CurrentRecord"] = "No";



        SqlConnection cn = Constant.Connection();
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM subscribers Where Username= '" + Membership.GetUser().ToString() + "'", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        string UserID = ds.Tables[0].Rows[0]["SubID"].ToString();

        SqlDataAdapter daProductDetails = new SqlDataAdapter("SELECT dbo.PurchaseProduct.*, dbo.PurchaseProductDetails.* FROM dbo.PurchaseProduct INNER JOIN dbo.PurchaseProductDetails ON dbo.PurchaseProduct.BuyID = dbo.PurchaseProductDetails.BuyId WHERE dbo.PurchaseProduct.SubscriberID='" + UserID.ToString() + "' AND dbo.PurchaseProduct.Week='" + ddlWeeks.SelectedValue + "'", cn);
        DataSet dsProductDetails = new DataSet();
        daProductDetails.Fill(dsProductDetails);

        if (dsProductDetails.Tables[0].Rows.Count > 0)
        {
            DataTable dtOlderProduct = new DataTable();
            dtOlderProduct.Columns.AddRange(new DataColumn[6] { new DataColumn("Id", typeof(int)),
                            new DataColumn("ProductId", typeof(int)),
                            new DataColumn("ProductName", typeof(string)),
                            new DataColumn("ProductPrice",typeof(double)),
                            new DataColumn("Quantity",typeof(string)),
                            new DataColumn("Week",typeof(string))});



            for (int i = 0; i < dsProductDetails.Tables[0].Rows.Count; i++)
            {
                DataRow dr = dtOlderProduct.NewRow();
                dr["Id"] = dsProductDetails.Tables[0].Rows.Count;
                dr["ProductId"] = Convert.ToInt32(dsProductDetails.Tables[0].Rows[i]["PurchaseProductID"]);
                dr["ProductName"] = Convert.ToString(dsProductDetails.Tables[0].Rows[i]["ProductName"]);
                dr["ProductPrice"] = Convert.ToDouble(dsProductDetails.Tables[0].Rows[i]["Price"]);
                dr["Quantity"] = Convert.ToString(dsProductDetails.Tables[0].Rows[i]["Quantity"]);
                dr["Week"] = Convert.ToString(dsProductDetails.Tables[0].Rows[i]["Week"]);
                dtOlderProduct.Rows.Add(dr);
            }
            gvSelectedProduct.DataSource = dtOlderProduct;
            gvSelectedProduct.DataBind();
            if (dsProductDetails.Tables[0].Rows[0]["PaymentMode"].ToString() != "Online")
                Label2.Text = "This order has been submitted for in store payment.";
            else
                Label2.Text = "Online Payment Successful";
        }
        else
        {
            gvSelectedProduct.DataSource = null;
            gvSelectedProduct.DataBind();
        }
    }
}