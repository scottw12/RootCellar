using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

using PerceptiveMCAPI;
using PerceptiveMCAPI.Types;
using PerceptiveMCAPI.Methods;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        var webAddr = "https://www.salesvu.com/townvu/api/index.php?request=%7B%22api_key%22%3A%226991d5c4211be3c66e69913b721250ac%22%2C%22action%22%3A%22create_order%22%2C%22store_id%22%3A%2234800%22%2C%22online_customer_id%221%22C2%22%7D";
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
        httpWebRequest.ContentType = "application/json";
         var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var val = streamReader.ReadToEnd();
            Label2.Text += "Request: " + webAddr+"<Br />";
            Label2.Text += "Result: " + val+"<Br />";
        }

    }


    //protected void Button1_Click1(object sender, EventArgs e)
    //{
    //    Literal1.Text = "";
    //    try
    //    {
    //        string storeID = "";
    //        //if (StoreList.SelectedValue == "Downtown Columbia")
    //        //{
    //            storeID = "34798";
    //        //}
    //        //else if (StoreList.SelectedValue == "Jefferson City")
    //        //{
    //        //    storeID = "34800";
    //        //}
    //        string url = "https://www.salesvu.com/townvu/api/index.php?request=";
    //        string webAddr = "";
    //        webAddr += "{'api_key':'6991d5c4211be3c66e69913b721250ac',";
    //        webAddr += "'action':'create_order',";
    //        webAddr += "'store_id':'" + storeID + "',";
    //        webAddr += "'online_customer_id':'1'";

    //        string prodDetails = "";
    //        //foreach (GridViewRow Weekrow in GridView1.Rows)
    //        //{
    //        //    string week = Weekrow.Cells(1).Text;
    //        //    CheckBox BountyPaid = Weekrow.FindControl("BountyPaidCheck") as CheckBox;
    //        //    if (BountyPaid.Enabled == true & BountyPaid.Checked == true)
    //        //    {
    //        //        if (string.IsNullOrEmpty(prodDetails))
    //        //        {
    //        //            prodDetails += ",'order_details':[";
    //        //        }
    //        //        else
    //        //        {
    //                    prodDetails += ",";
    //                // }
    //                prodDetails += "{'product_id':2,";
    //                prodDetails += "'unit_id':0,";
    //                prodDetails += "'quantity':1,";
    //                prodDetails += "'notes':[{";
    //                prodDetails += "'comment':'DBs+Bounty+payment+for+8/21-22/2014'";
    //                prodDetails += "}]}]";
    //                webAddr += prodDetails;
               
    //        webAddr += "}";
    //        webAddr = HttpUtility.UrlEncode(webAddr);
    //        webAddr = webAddr.Replace("%27", "%22");
    //        webAddr = url + webAddr;

    //        Literal1.Text += "<br /><br />Request: " + webAddr;
    //        Uri FwebAddr = new Uri(webAddr);
    //        dynamic httpWebRequest = (HttpWebRequest)WebRequest.Create(FwebAddr);
    //        httpWebRequest.ContentType = "application/json";
    //        dynamic httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
    //        using (streamReader == new StreamReader(httpResponse.GetResponseStream()))
    //        {
    //            dynamic val = streamReader.ReadToEnd();
    //            string pattern = "{(.*?)order_id\":";
    //            string replacement = "";
    //            Regex rgx = new Regex(pattern, RegexOptions.Singleline);
    //            val = rgx.Replace(val, replacement);
    //            pattern = ",(.*?)}}";
    //            replacement = "";
    //            Regex rgx2 = new Regex(pattern, RegexOptions.Singleline);
    //            val = rgx2.Replace(val, replacement);
    //            Literal1.Text += "<br /><br />Result: " + val + "<Br />";
    //            GetURL(val, storeID);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Literal1.Text += ex.Message + "<br /><br />Error:" + ex.StackTrace;
    //    }
    //}
    //public string GetURL(string OrderID, string StoreID)
    //{
    //    try
    //    {
    //        string url = "https://www.salesvu.com/townvu/api/index.php?request=";
    //        string webAddr = "";
    //        webAddr += "{'api_key':'6991d5c4211be3c66e69913b721250ac',";
    //        webAddr += "'action':'get_payment_url',";
    //        webAddr += "'store_id':'" + StoreID + "',";
    //        webAddr += "'online_customer_id':1,";
    //        webAddr += "'order_id':" + OrderID + ",";
    //        webAddr += "'return_url':'http://www.rootcellarboxes.com/success/',";
    //        webAddr += "'platform':'PC'";
    //        webAddr += "}";
    //        webAddr = HttpUtility.UrlEncode(webAddr);
    //        webAddr = webAddr.Replace("%27", "%22");
    //        webAddr = url + webAddr;
    //        Literal1.Text += "<br /><br />Request2: " + webAddr;
    //        Uri FwebAddr = new Uri(webAddr);
    //        dynamic httpWebRequest = (HttpWebRequest)WebRequest.Create(FwebAddr);
    //        httpWebRequest.ContentType = "application/json";
    //        dynamic httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
    //        using (streamReader == new StreamReader(httpResponse.GetResponseStream()))
    //        {
    //            dynamic val = streamReader.ReadToEnd();
    //            //Dim pattern As String = "{(.*?)order_id"":"
    //            //Dim replacement As String = ""
    //            //Dim rgx As New Regex(pattern, RegexOptions.Singleline)
    //            //val = rgx.Replace(val, replacement)
    //            //pattern = ",(.*?)}}"
    //            //replacement = ""
    //            //Dim rgx2 As New Regex(pattern, RegexOptions.Singleline)
    //            //val = rgx2.Replace(val, replacement)
    //            Literal1.Text += "<br /><br />Result2: " + val + "<Br />";
    //            return val;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Literal1.Text += ex.Message + "<br /><br />Error2:" + ex.StackTrace;
    //        return "";
    //    }
    //    return "";
    //}

    
}