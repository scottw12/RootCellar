using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class products : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    
    protected void Bounty_click(object sender, EventArgs e)
    {
        Response.Redirect("subscribe?B=Bounty");
    }
    protected void Barnyard_click(object sender, EventArgs e)
    {
        Response.Redirect("subscribe?B=Barnyard");
    }
    protected void Ploughman_click(object sender, EventArgs e)
    {
        Response.Redirect("subscribe?B=Ploughman");
    }
}