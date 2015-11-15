using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class admin_ViewProductDetails : System.Web.UI.Page
{
    static int SubId;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["s"]!=null)
            {
                SubId=int.Parse(Request.QueryString["s"]);
                SqlConnection cn = Constant.Connection();
                SqlDataAdapter da = new SqlDataAdapter("SELECT dbo.PurchaseProduct.*, dbo.PurchaseProductDetails.* FROM dbo.PurchaseProduct INNER JOIN dbo.PurchaseProductDetails ON dbo.PurchaseProduct.BuyID = dbo.PurchaseProductDetails.BuyId WHERE dbo.PurchaseProduct.SubscriberID='" + SubId + "'", cn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvProducts.DataSource = ds.Tables[0];
                    gvProducts.DataBind();
                }
                else
                {
                    gvProducts.DataSource = ds.Tables[0];
                    gvProducts.DataBind();
                }
               
            }
        }
    }
}