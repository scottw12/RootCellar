using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class customer_BuyProduct : System.Web.UI.Page
{
    
    public DataTable SelectedProducts;   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["SelectedProducts"] = null;
            SqlConnection cn = Constant.Connection();
            SqlDataAdapter da = new SqlDataAdapter("select * from ProductDetails", cn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count>0)
            {
                rcProducts.DataSource = ds.Tables[0];
                rcProducts.DataBind();
            }
            SelectedProducts = new DataTable();
            SelectedProducts.Columns.Add("ProductId", typeof(int));
            SelectedProducts.Columns.Add("ProductName", typeof(string));
            SelectedProducts.Columns.Add("ProductPrice", typeof(Double));
            SelectedProducts.Columns.Add("Quantity", typeof(string));
            Session["SelectedProducts"] = SelectedProducts;
        }
        //dr = SelectedProducts.NewRow();
    }
    
    /// <summary>
    /// Add To Cart
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    
    protected void rcProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        //Label lblPrice = rcProducts.FindControl("lblPrice") as Label;
        //Label lblProductName = rcProducts.FindControl("lblProductName") as Label;
        //List<Constant.Cart> AllCart = new List<Constant.Cart>();
        //Constant.Cart obj=new Constant.Cart();
        //obj.ProductID=Convert.ToInt32( e.CommandArgument);
        //obj.Price=Convert.ToInt32(lblPrice.Text);
        //obj.ProductName=lblProductName.Text;
        //AllCart.Add(obj);
        SqlConnection cn =Constant.Connection();
        SqlDataAdapter da = new SqlDataAdapter("Select * from ProductDetails where ProductID='" + e.CommandArgument + "'", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        //AllCart = new List<Constant.Cart>();
        Constant.Cart obj = new Constant.Cart();
        TextBox Quantity = rcProducts.FindControl("txtQuantity") as TextBox;
        obj.ProductID = Convert.ToInt32(ds.Tables[0].Rows[0]["ProductID"]);
        obj.Price = Convert.ToDouble(ds.Tables[0].Rows[0]["ProductPrice"]);
        obj.ProductName = Convert.ToString(ds.Tables[0].Rows[0]["ProductName"]);
        obj.Quantity = Convert.ToInt32(Quantity.Text);
        //AllCart.Add(obj);
    }
    protected void btnAddCart_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (RepeaterItem item in rcProducts.Controls)
            {
                CheckBox cbAddToCart = item.FindControl("cbAddToCart") as CheckBox;
                if (cbAddToCart.Checked)
                {

                    Label ProductID = item.FindControl("ProductID") as Label;
                    Label lblProductName = item.FindControl("lblProductName") as Label;
                    Label lblPrice = item.FindControl("lblPrice") as Label;
                    TextBox Quantity = item.FindControl("txtQuantity") as TextBox;

                    if (Session["SelectedProducts"] == null)
                    {
                        Response.Write("DataTable not exist!");
                        return;
                    }
                    DataTable SelectedProducts = (DataTable)Session["SelectedProducts"];
                    DataRow dr = SelectedProducts.NewRow();


                    dr["ProductId"] = Convert.ToInt32(ProductID.Text);
                    dr["ProductName"] = Convert.ToString(lblProductName.Text);
                    dr["ProductPrice"] = Convert.ToDouble(lblPrice.Text);
                    dr["Quantity"] = Convert.ToString(Quantity.Text);
                    
                    SelectedProducts.Rows.Add(dr);
                    Session["SelectedProducts"] = SelectedProducts;
                    
                }               
            }
            Response.Redirect("~/customer/ProductPay.aspx");
        }
        catch (Exception er)
        {
        }
    }
}