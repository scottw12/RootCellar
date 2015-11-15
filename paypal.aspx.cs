using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using System.Net;


public partial class paypal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Post back to either sandbox or live
        string strSandbox = "https://www.sandbox.paypal.com/cgi-bin/webscr";
        // string strLive = "https://www.paypal.com/cgi-bin/webscr";
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strSandbox);

        //Set values for the request back
        req.Method = "POST";
        req.ContentType = "application/x-www-form-urlencoded";
        byte[] param = Request.BinaryRead(HttpContext.Current.Request.ContentLength);
        string strRequest = Encoding.ASCII.GetString(param);
        strRequest += "&cmd=_notify-validate";
        req.ContentLength = strRequest.Length;

        //for proxy
        //WebProxy proxy = new WebProxy(new Uri("http://url:port#"));
        //req.Proxy = proxy;

        //Send the request to PayPal and get the response
        StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
        streamOut.Write(strRequest);
        streamOut.Close();
        StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
        string strResponse = streamIn.ReadToEnd();
        streamIn.Close();

        if (strResponse == "VERIFIED")
        {
            //UPDATE YOUR DATABASE

            //TextWriter txWriter = new StreamWriter(Server.MapPath("../uploads/") + Session["orderID"].ToString() + ".txt");
            //txWriter.WriteLine(strResponse);
            //txWriter.Close();

            //check the payment_status is Completed
            //check that txn_id has not been previously processed
            //check that receiver_email is your Primary PayPal email
            //check that payment_amount/payment_currency are correct
            //process payment
        }
        else if (strResponse == "INVALID")
        {
            //UPDATE YOUR DATABASE

            //TextWriter txWriter = new StreamWriter(Server.MapPath("../uploads/") + Session["orderID"].ToString() + ".txt");
            //txWriter.WriteLine(strResponse);
            ////log for manual investigation
            //txWriter.Close();
        }
        else
        {  //UPDATE YOUR DATABASE

            //TextWriter txWriter = new StreamWriter(Server.MapPath("../uploads/") + Session["orderID"].ToString() + ".txt");
            //txWriter.WriteLine("Invalid");
            ////log response/ipn data for manual investigation
            //txWriter.Close();
        }
    }
}
