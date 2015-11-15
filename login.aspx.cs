using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["addCard"] = null;
        Session["SelectedProductsPayment"] = null;
        Session["SelectedProducts"] = null;
        Session["SPPayment"] = null;
        Session.RemoveAll();
        DataTable SelectedProducts = new DataTable();
        SelectedProducts.Columns.Add("Id", typeof(int));
        SelectedProducts.Columns.Add("ProductId", typeof(int));
        SelectedProducts.Columns.Add("ProductName", typeof(string));
        SelectedProducts.Columns.Add("ProductPrice", typeof(double));
        SelectedProducts.Columns.Add("Quantity", typeof(string));
        SelectedProducts.Columns.Add("Week", typeof(string));
        Session["SelectedProducts"] = SelectedProducts;
        Session["addCard"] = SelectedProducts;
    }
}