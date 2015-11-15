using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            Label2.Text += "Result: " + val+"<Br />";
        }

    }
}