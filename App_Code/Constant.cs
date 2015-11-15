using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Data;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;

using System.Data.SqlClient;

/// <summary>
/// Summary description for Constant
/// </summary>
public class Constant
{
    public const string UserID = "UserID";
    public const string UserRole = "UserRole";
    public const string AdminMailId = "sonalic@custom-soft.com";
    public const string TempPurchaseProductID = "TempPurchaseProductID";
    public Constant()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static SqlConnection Connection()
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        return cn;
    }

    public static void SendMail(string UserTo, string Subject, string Body)
    {
        try
        {
            //SmtpClient smtp = new SmtpClient();
            //MailMessage email_msg = new MailMessage();
            //email_msg.To.Add(UserTo);
            //email_msg.From = new MailAddress("qat2015team@gmail.com");

            //AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");

            ////LinkedResource imagelink3 = new LinkedResource(HttpContext.Current.Server.MapPath("~/img/logo.png"), "image/png");
            ////imagelink3.ContentId = "imageId1";
            ////imagelink3.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
            ////htmlView.LinkedResources.Add(imagelink3);

            //smtp.UseDefaultCredentials = false;
            //email_msg.AlternateViews.Add(htmlView);
            //email_msg.Subject = Subject;
            ////email_msg.Body = Body;
            //email_msg.IsBodyHtml = true;
            //smtp.EnableSsl = true;
            ////smtp.Host = "587";
            //smtp.Send(email_msg);



            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("website");
                message.To.Add(new MailAddress(UserTo));
                // message.CC.Add(new MailAddress("qat2015team@gmail.com"));
                message.Subject = Subject;
                message.Body = Body;
                message.IsBodyHtml = true;

                //SmtpClient client = new SmtpClient();
                //client.Host = "smtp.gmail.com";
                //client.Send(message);
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                smtp.EnableSsl = true;
                smtp.Send(message);
            }


        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {

        }
    }

    public static DataSet Store()
    {
        SqlConnection cn = Constant.Connection();
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Stores", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }

    public class Cart
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public Double Price { get; set; }
        public int Quantity { get; set; }
    }
    /// <summary>
    /// save transaction details to database
    /// </summary>
    public static void SaveTranDetails()
    {
        SqlConnection cn = Constant.Connection();
        string QuerySelectTrans = " SELECT dbo.PurchaseProductTemp.SubscriberID, dbo.PurchaseProductTemp.PurchaseDate, dbo.PurchaseProductTemp.OnlineHome, dbo.PurchaseProductTemp.Store, " +
                                  " dbo.PurchaseProductTemp.PickupDay, dbo.PurchaseProductTemp.Week, dbo.PurchaseProductTemp.PaymentMode, dbo.PurchaseProductTemp.IsPaid, " +
                                  " dbo.PurchaseProductDetailsTemp.ProductID, dbo.PurchaseProductDetailsTemp.ProductName, dbo.PurchaseProductDetailsTemp.Price, " +
                                  " dbo.PurchaseProductDetailsTemp.Quantity FROM dbo.PurchaseProductTemp INNER JOIN dbo.PurchaseProductDetailsTemp ON dbo.PurchaseProductTemp.BuyID = dbo.PurchaseProductDetailsTemp.BuyId"+
                                  " WHERE ";

        SqlDataAdapter da_TempTrans = new SqlDataAdapter(QuerySelectTrans, cn);
        DataSet ds_TempTrans = new DataSet();
        da_TempTrans.Fill(ds_TempTrans);
        if (ds_TempTrans.Tables[0].Rows.Count > 0)
        {
            SqlCommand cmd_PurchaseProduct = new SqlCommand("Insert into PurchaseProduct values(@SubscriberID,@PurchaseDate,@OnlineHome,@Store,@PickupDay,@Week,@PaymentMode,@IsPaid);SELECT CAST(scope_identity() AS int)", cn);
            cmd_PurchaseProduct.Parameters.AddWithValue("@SubscriberID", Convert.ToInt32(ds_TempTrans.Tables[0].Rows[0]["SubscriberID"]));
            cmd_PurchaseProduct.Parameters.AddWithValue("@PurchaseDate", DateTime.Now);
            cmd_PurchaseProduct.Parameters.AddWithValue("@OnlineHome", Convert.ToBoolean(ds_TempTrans.Tables[0].Rows[0]["OnlineHome"]));//Home Delivery + Online Payment                       
            cmd_PurchaseProduct.Parameters.AddWithValue("@Store", Convert.ToString(ds_TempTrans.Tables[0].Rows[0]["Store"]));
            cmd_PurchaseProduct.Parameters.AddWithValue("@PickupDay", Convert.ToString(ds_TempTrans.Tables[0].Rows[0]["PickupDay"]));
            cmd_PurchaseProduct.Parameters.AddWithValue("@PaymentMode", "Online");
            cmd_PurchaseProduct.Parameters.AddWithValue("@IsPaid", "Paid");
            cmd_PurchaseProduct.Parameters.AddWithValue("@Week", Convert.ToString(ds_TempTrans.Tables[0].Rows[0]["Week"]));
            cn.Open();
            int PurchaseProductID = (int)cmd_PurchaseProduct.ExecuteScalar();
            cn.Close();

            for (int i = 0; i < ds_TempTrans.Tables[0].Rows.Count; i++)
            {
                // New Added By Harshal For Subtracting Total Quantity of product
                SqlDataAdapter da_Qua = new SqlDataAdapter("SELECT * FROM ProductDetailsNew WHERE ProductID='" + Convert.ToInt32(ds_TempTrans.Tables[0].Rows[i]["ProductID"]) + "'", cn);
                DataSet ds_Qua = new DataSet();
                da_Qua.Fill(ds_Qua);
                cn.Close();
                cn.Open();
                SqlCommand cmd_Qua = new SqlCommand("UPDATE ProductDetailsNew SET Quantity='" + (Convert.ToInt32(ds_Qua.Tables[0].Rows[0]["Quantity"]) - Convert.ToInt32(ds_TempTrans.Tables[0].Rows[i]["Quantity"])) + "' WHERE ProductID='" + Convert.ToInt32(ds_TempTrans.Tables[0].Rows[i]["ProductID"]) + "'", cn);
                cmd_Qua.ExecuteNonQuery();
                cn.Close();

                SqlCommand cmd_PurchaseProductDetails = new SqlCommand("Insert into PurchaseProductDetails values(@BuyId,@SubscriberID,@ProductID,@ProductName,@Price,@Quantity,@PaymentMode,@IsPaid)", cn);
                cmd_PurchaseProductDetails.Parameters.AddWithValue("@BuyId", PurchaseProductID);
                cmd_PurchaseProductDetails.Parameters.AddWithValue("@SubscriberID", Convert.ToInt32(ds_TempTrans.Tables[0].Rows[0]["SubscriberID"]));
                cmd_PurchaseProductDetails.Parameters.AddWithValue("@ProductID", Convert.ToInt32(ds_TempTrans.Tables[0].Rows[i]["ProductID"]));
                cmd_PurchaseProductDetails.Parameters.AddWithValue("@ProductName", Convert.ToString(ds_TempTrans.Tables[0].Rows[i]["ProductName"]));
                cmd_PurchaseProductDetails.Parameters.AddWithValue("@Price", Convert.ToDouble(ds_TempTrans.Tables[0].Rows[i]["Price"]));
                cmd_PurchaseProductDetails.Parameters.AddWithValue("@Quantity", Convert.ToInt32(ds_TempTrans.Tables[0].Rows[i]["Quantity"]));
                cmd_PurchaseProductDetails.Parameters.AddWithValue("@PaymentMode", "Online");
                cmd_PurchaseProductDetails.Parameters.AddWithValue("@IsPaid", "Paid");
                cn.Open();
                cmd_PurchaseProductDetails.ExecuteNonQuery();
                cn.Close();
            }
        }
    }
}