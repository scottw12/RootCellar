using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class admin_Products : System.Web.UI.Page
{
    static string FName = string.Empty, FPath = string.Empty, ProductID=string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //DateTime x = Convert.ToDateTime(DateTime.Now.ToShortDateString());
        if (!IsPostBack )
        {
            BindProduct();
            if (Request.QueryString["ProductID"]!=null)
                LoadProduct();
        }
    }

    private void LoadProduct()
    {
        ProductID = EncryptDecrypt.DecryptPassword(Request.QueryString["ProductID"].ToString());
        SqlConnection cn = Constant.Connection();
        SqlDataAdapter da = new SqlDataAdapter("Select * From ProductDetails where ProductID=" + ProductID + "", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtProducts.Text = ds.Tables[0].Rows[0]["ProductName"].ToString();
            txtPDescription.Text = ds.Tables[0].Rows[0]["ProductDescription"].ToString();
            txtPPrice.Text = ds.Tables[0].Rows[0]["ProductPrice"].ToString();
            
            if (ds.Tables[0].Rows[0]["DC"].ToString() == "True")//DowntownColumbia
            {
                cbDowntownColumbia.Checked = true;
                DCThu.Enabled = true;
                DCFri.Enabled = true;
                DCHomeDelivery.Enabled = true;
                DCHomeDelivery0.Enabled = true;

                if (ds.Tables[0].Rows[0]["DCThu"].ToString() ==  "True")
                    DCThu.Checked = true;
                if (ds.Tables[0].Rows[0]["DCFri"].ToString() ==  "True")
                    DCFri.Checked = true;

                if (ds.Tables[0].Rows[0]["DCHD"].ToString() == "True")
                {
                    DCHomeDelivery.Checked = true;
                    DCPaid.Enabled = true;
                    
                }
                if (ds.Tables[0].Rows[0]["DCHDFri"].ToString() == "True")
                {
                    DCHomeDelivery0.Checked = true;
                    DCPaid0.Enabled = true;
                    
                }

                if (ds.Tables[0].Rows[0]["DCP"].ToString() == "True")
                {
                    DCPaid.Checked = true;
                    DCCharges.Enabled = true;
                    DCCharges.Text = ds.Tables[0].Rows[0]["DCCharges"].ToString();
                }
                if (ds.Tables[0].Rows[0]["DCPFri"].ToString() == "True")
                {
                    DCPaid0.Checked = true;
                    DCCharges0.Enabled = true;
                    DCCharges0.Text = ds.Tables[0].Rows[0]["DCChargesFri"].ToString();
                }
            }
            else
                cbDowntownColumbia.Checked = false;

            if (ds.Tables[0].Rows[0]["JC"].ToString() ==  "True")//JeffersonCity
            {
                cbJeffersonCity.Checked = true;
                JCThu.Enabled = true;
                JSFri.Enabled = true;
                JCHomeDelivery.Enabled = true;
                JCHomeDelivery0.Enabled = true;

                if (ds.Tables[0].Rows[0]["JCThu"].ToString() ==  "True")
                    JCThu.Checked = true;
                if (ds.Tables[0].Rows[0]["JCFri"].ToString() ==  "True")
                    JSFri.Checked = true;

                if (ds.Tables[0].Rows[0]["JCHD"].ToString() == "True")
                {
                    JCHomeDelivery.Checked = true;
                    JCPaid.Enabled = true;
                    
                }
                if (ds.Tables[0].Rows[0]["JCHDFri"].ToString() == "True")
                {
                    JCHomeDelivery0.Checked = true;
                    JCPaid0.Enabled = true;
                    
                }

                if (ds.Tables[0].Rows[0]["JCP"].ToString() == "True")
                {
                    JCPaid.Checked = true;
                    JCCharges.Text = ds.Tables[0].Rows[0]["JCCharges"].ToString();
                    JCCharges.Enabled = true;
                }
                if (ds.Tables[0].Rows[0]["JCPFri"].ToString() == "True")
                {
                    JCPaid0.Checked = true;
                    JCCharges0.Text = ds.Tables[0].Rows[0]["JCChargesFri"].ToString();
                    JCCharges0.Enabled = true;
                }
            }
            else
                cbJeffersonCity.Checked = false;

            if (ds.Tables[0].Rows[0]["DHSS"].ToString() ==  "True")//DHSS
            {
                cbDHSS.Checked = true;
                DHSSThu.Enabled = true;
                DHSSFri.Enabled = true;
                DHSSHomeDelivery.Enabled = true;
                DHSSHomeDelivery0.Enabled = true;

                if (ds.Tables[0].Rows[0]["DHSSThu"].ToString() ==  "True")
                    DHSSThu.Checked = true;
                if (ds.Tables[0].Rows[0]["DHSSFri"].ToString() ==  "True")
                    DHSSFri.Checked = true;

                if (ds.Tables[0].Rows[0]["DHSSHD"].ToString() == "True")
                {
                    DHSSHomeDelivery.Checked = true;
                    DHSSPaid.Enabled = true;
                    
                }
                if (ds.Tables[0].Rows[0]["DHSSHDFri"].ToString() == "True")
                {
                    DHSSHomeDelivery0.Checked = true;
                    DHSSPaid0.Enabled = true;
                    
                }

                if (ds.Tables[0].Rows[0]["DHSSP"].ToString() == "True")
                {
                    DHSSPaid.Checked = true;
                    DHSSCharges.Text = ds.Tables[0].Rows[0]["DHSSCharges"].ToString();
                    DHSSCharges.Enabled = true;
                }
                if (ds.Tables[0].Rows[0]["DHSSPFri"].ToString() == "True")
                {
                    DHSSPaid0.Checked = true;
                    DHSSCharges0.Text = ds.Tables[0].Rows[0]["DHSSChargesFri"].ToString();
                    DHSSCharges0.Enabled = true;
                }
            }
            else
                cbDHSS.Checked = false;

            if (ds.Tables[0].Rows[0]["MN"].ToString() ==  "True")//MizzouNorth
            {
                cbMizzouNorth.Checked = true;
                MNThu.Enabled = true;
                MNFri.Enabled = true;
                MNHomeDelivery.Enabled = true;
                MNHomeDelivery0.Enabled = true;

                if (ds.Tables[0].Rows[0]["MNThu"].ToString() ==  "True")
                    MNThu.Checked = true;
                if (ds.Tables[0].Rows[0]["MNFri"].ToString() ==  "True")
                    MNFri.Checked = true;

                if (ds.Tables[0].Rows[0]["MNHD"].ToString() == "True")
                {
                    MNHomeDelivery.Checked = true;
                    MNPaid.Enabled = true;
                    

                }
                if (ds.Tables[0].Rows[0]["MNHDFri"].ToString() == "True")
                {
                    MNHomeDelivery0.Checked = true;
                    MNPaid0.Enabled = true;
                    
                }

                if (ds.Tables[0].Rows[0]["MNP"].ToString() == "True")
                {
                    MNPaid.Checked = true;
                    MNCharges.Text = ds.Tables[0].Rows[0]["MNCharges"].ToString();
                    MNCharges.Enabled = true;
                }
                if (ds.Tables[0].Rows[0]["MNPFri"].ToString() == "True")
                {
                    MNPaid0.Checked = true;
                    MNCharges0.Text = ds.Tables[0].Rows[0]["MNChargesFri"].ToString();
                    MNCharges0.Enabled = true;
                }
            }
            else
                cbMizzouNorth.Checked = false;


            if (ds.Tables[0].Rows[0]["QUA"].ToString() ==  "True")//QUARTERDECK
            {
                cbQUARTERDECK.Checked = true;
                QThu.Enabled = true;
                QFri.Enabled = true;
                QHomeDelivery.Enabled = true;
                QHomeDelivery0.Enabled = true;

                if (ds.Tables[0].Rows[0]["QUAThu"].ToString() ==  "True")
                    QThu.Checked = true;
                if (ds.Tables[0].Rows[0]["QUAFri"].ToString() ==  "True")
                    QFri.Checked = true;

                if (ds.Tables[0].Rows[0]["QHD"].ToString() == "True")
                {
                    QHomeDelivery.Checked = true;
                    QPaid.Enabled = true;
                   
                }
                if (ds.Tables[0].Rows[0]["QHDFri"].ToString() == "True")
                {
                    QHomeDelivery0.Checked = true;
                    QPaid0.Enabled = true;
                   

                }

                if (ds.Tables[0].Rows[0]["QP"].ToString() == "True")
                {
                    QPaid.Checked = true;
                    QCharges.Enabled = true;
                    QCharges.Text = ds.Tables[0].Rows[0]["QCharges"].ToString();
                }
                if (ds.Tables[0].Rows[0]["QPFri"].ToString() == "True")
                {
                    QPaid0.Checked = true;
                    QCharges0.Enabled = true;
                    QCharges0.Text = ds.Tables[0].Rows[0]["QChargesFri"].ToString();
                }
            }
            else
                cbQUARTERDECK.Checked = false;


            if (ds.Tables[0].Rows[0]["UH"].ToString() ==  "True")//UniversityHospital
            {
                cbUniversityHospital.Checked = true;
                UHThu.Enabled = true;
                UHFri.Enabled = true;
                UHHomeDelivery.Enabled = true;
                UHHomeDelivery0.Enabled = true;

                if (ds.Tables[0].Rows[0]["UHThu"].ToString() ==  "True")
                    UHThu.Checked = true;
                if (ds.Tables[0].Rows[0]["UHFri"].ToString() ==  "True")
                    UHFri.Checked = true;

                if (ds.Tables[0].Rows[0]["UHHD"].ToString() == "True")
                {
                    UHHomeDelivery.Checked = true;
                    UHPaid.Enabled = true;
                    
                }
                if (ds.Tables[0].Rows[0]["UHHDFri"].ToString() == "True")
                {
                    UHHomeDelivery0.Checked = true;
                    UHPaid0.Enabled = true;
                    
                }
                if (ds.Tables[0].Rows[0]["UHP"].ToString() == "True")
                {
                    UHPaid.Checked = true;
                    UHCharges.Text = ds.Tables[0].Rows[0]["UHCharges"].ToString();
                    UHCharges.Enabled = true;
                }
                if (ds.Tables[0].Rows[0]["UHPFri"].ToString() == "True")
                {
                    UHPaid0.Checked = true;
                    UHCharges0.Text = ds.Tables[0].Rows[0]["UHChargesFri"].ToString();
                    UHCharges0.Enabled = true;
                }
            }
            else
                cbUniversityHospital.Checked = false;

            if (ds.Tables[0].Rows[0]["UMH"].ToString() ==  "True")//UMHeinkel
            {
                cbUMHeinkel.Checked = true;
                UMHThu.Enabled = true;
                UMHFri.Enabled = true;
                UMHHomeDelivery.Enabled = true;
                UMHHomeDelivery0.Enabled = true;

                if (ds.Tables[0].Rows[0]["UMHThu"].ToString() ==  "True")
                    UMHThu.Checked = true;
                if (ds.Tables[0].Rows[0]["UMHFri"].ToString() ==  "True")
                    UMHFri.Checked = true;

                if (ds.Tables[0].Rows[0]["UMHHD"].ToString() == "True")
                {
                    UMHHomeDelivery.Checked = true;
                    UMHPaid.Enabled = true;
                    
                }
                if (ds.Tables[0].Rows[0]["UMHHDFri"].ToString() == "True")
                {
                    UMHHomeDelivery0.Checked = true;
                    UMHPaid0.Enabled = true;
                   
                }

                if (ds.Tables[0].Rows[0]["UMHP"].ToString() == "True")
                {
                    UMHPaid.Checked = true;
                    UMHCharges.Text = ds.Tables[0].Rows[0]["UMHCharges"].ToString();
                    UMHCharges.Enabled = true;
                }
                if (ds.Tables[0].Rows[0]["UMHPFri"].ToString() == "True")
                {
                    UMHPaid0.Checked = true;
                    UMHCharges0.Text = ds.Tables[0].Rows[0]["UMHChargesFri"].ToString();
                    UMHCharges0.Enabled = true;
                }
            }
            else
                cbUMHeinkel.Checked = false;

            if (ds.Tables[0].Rows[0]["PaymentType"].ToString() ==  "True")//PaymentType
                rblPayment.SelectedIndex = 0;
            else
                rblPayment.SelectedIndex = 1;
        }
    }

    private void BindProduct()
    {
        SqlConnection cn = Constant.Connection();
        SqlDataAdapter da = new SqlDataAdapter("Select * from ProductDetails order by ProductID desc", cn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvProduct.DataSource = ds.Tables[0];
            gvProduct.DataBind();
        }
        else
        {
            gvProduct.Visible = false;
        }
    }
    /// <summary>
    /// Upload Image Files
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Image Uploaded Sucessfully')", true);
            }
        }
        else        
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select The File')", true);
    }

    /// <summary>
    /// Add/Update Data into Database
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ProductID == string.Empty)//Save Code
        {
            SqlConnection cn = Constant.Connection();
            cn.Open();
            SqlCommand cmd = new SqlCommand("Insert Into ProductDetails Values (@ProductName,@ProductDescription,@ProductImage,@ImagePath,@ProductPrice,@DC,@DCThu,@DCFri,@JC,@JCThu,@JCFri,@DHSS,@DHSSThu,@DHSSFri,@MN,@MNThu,@MNFri,@QUA,@QUAThu,@QUAFri,@UH,@UHThu,@UHFri,@UMH,@UMHThu,@UMHFri,@PaymentType,@DCHD,@DCP,@DCCharges,@JCHD,@JCP,@JCCharges,@DHSSHD,@DHSSP,@DHSSCharges,@MNHD,@MNP,@MNCharges,@QHD,@QP,@QCharges,@UHHD,@UHP,@UHCharges,@UMHHD,@UMHP,@UMHCharges,@DCHDFri,@DCPFri,@DCChargesFri,@JCHDFri,@JCPFri,@JCChargesFri,@DHSSHDFri,@DHSSPFri,@DHSSChargesFri,@MNHDFri,@MNPFri,@MNChargesFri,@QHDFri,@QPFri,@QChargesFri,@UHHDFri,@UHPFri,@UHChargesFri,@UMHHDFri,@UMHPFri,@UMHChargesFri)", cn);
            cmd.Parameters.AddWithValue("@ProductName", txtProducts.Text.Trim());
            cmd.Parameters.AddWithValue("@ProductDescription", txtPDescription.Text.Trim());
            cmd.Parameters.AddWithValue("@ProductImage", FName);
            cmd.Parameters.AddWithValue("@ImagePath", FPath);
            cmd.Parameters.AddWithValue("@ProductPrice", txtPPrice.Text.Trim());
            if (cbDowntownColumbia.Checked)
            {
                if (DCFri.Checked || DCThu.Checked)
                    cmd.Parameters.AddWithValue("@DC", true);
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Atleast One Day For Downtown Columbia')", true);
                    return;
                }
            }
            else
                cmd.Parameters.AddWithValue("@DC", false);
            if (DCThu.Checked)
                cmd.Parameters.AddWithValue("@DCThu", true);
            else
                cmd.Parameters.AddWithValue("@DCThu", false);
            if (DCFri.Checked)
                cmd.Parameters.AddWithValue("@DCFri", true);
            else
                cmd.Parameters.AddWithValue("@DCFri", false);


            if (cbJeffersonCity.Checked)
            {
                if (JCThu.Checked || JSFri.Checked)
                    cmd.Parameters.AddWithValue("@JC", true);
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Atleast One Day For Jefferson City')", true);
                    return;
                }
            }
            else
                cmd.Parameters.AddWithValue("@JC", false);
            if (JCThu.Checked)
                cmd.Parameters.AddWithValue("@JCThu", true);
            else
                cmd.Parameters.AddWithValue("@JCThu", false);
            if (JSFri.Checked)
                cmd.Parameters.AddWithValue("@JCFri", true);
            else
                cmd.Parameters.AddWithValue("@JCFri", false);


            if (cbDHSS.Checked)
            {
                if (DHSSFri.Checked || DHSSThu.Checked)
                    cmd.Parameters.AddWithValue("@DHSS", true);
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Atleast One Day For DHSS')", true);
                    return;
                }
            }
            else
                cmd.Parameters.AddWithValue("@DHSS", false);
            if (DHSSThu.Checked)
                cmd.Parameters.AddWithValue("@DHSSThu", true);
            else
                cmd.Parameters.AddWithValue("@DHSSThu", false);
            if (DHSSFri.Checked)
                cmd.Parameters.AddWithValue("@DHSSFri", true);
            else
                cmd.Parameters.AddWithValue("@DHSSFri", false);
            if (cbMizzouNorth.Checked)
            {
                if (MNFri.Checked || MNThu.Checked)
                    cmd.Parameters.AddWithValue("@MN", true);
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Atleast One Day For Mizzou North')", true);
                    return;
                }
            }
            else
                cmd.Parameters.AddWithValue("@MN", false);
            if (MNThu.Checked)
                cmd.Parameters.AddWithValue("@MNThu", true);
            else
                cmd.Parameters.AddWithValue("@MNThu", false);
            if (MNFri.Checked)
                cmd.Parameters.AddWithValue("@MNFri", true);
            else
                cmd.Parameters.AddWithValue("@MNFri", false);

            if (cbQUARTERDECK.Checked)
            {
                if (QFri.Checked || QThu.Checked)
                    cmd.Parameters.AddWithValue("@QUA", true);
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Atleast One Day For QUARTERDECK')", true);
                    return;
                }
            }
            else
                cmd.Parameters.AddWithValue("@QUA", false);
            if (QThu.Checked)
                cmd.Parameters.AddWithValue("@QUAThu", true);
            else
                cmd.Parameters.AddWithValue("@QUAThu", false);

            if (QFri.Checked)
                cmd.Parameters.AddWithValue("@QUAFri", true);
            else
                cmd.Parameters.AddWithValue("@QUAFri", false);

            if (cbUniversityHospital.Checked)
            {
                if (UHThu.Checked || UHFri.Checked)
                    cmd.Parameters.AddWithValue("@UH", true);
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Atleast One Day For University Hospital/School of Medicine')", true);
                    return;
                }
            }
            else
                cmd.Parameters.AddWithValue("@UH", false);
            if (UHThu.Checked)
                cmd.Parameters.AddWithValue("@UHThu", true);
            else
                cmd.Parameters.AddWithValue("@UHThu", false);

            if (UHFri.Checked)
                cmd.Parameters.AddWithValue("@UHFri", true);
            else
                cmd.Parameters.AddWithValue("@UHFri", false);

            if (cbUMHeinkel.Checked)
            {
                if (UMHFri.Checked || UMHThu.Checked)
                    cmd.Parameters.AddWithValue("@UMH", true);
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Atleast One Day For UM Heinkel')", true);
                    return;
                }
            }
            else
                cmd.Parameters.AddWithValue("@UMH", false);

            if (UMHThu.Checked)
                cmd.Parameters.AddWithValue("@UMHThu", true);
            else
                cmd.Parameters.AddWithValue("@UMHThu", false);

            if (UMHFri.Checked)
                cmd.Parameters.AddWithValue("@UMHFri", true);
            else
                cmd.Parameters.AddWithValue("@UMHFri", false);


            if (rblPayment.SelectedIndex == 0)
                cmd.Parameters.AddWithValue("@PaymentType", true);
            else
                cmd.Parameters.AddWithValue("@PaymentType", false);

            /***************Changes***********/
            if (DCHomeDelivery.Checked)
            {
                cmd.Parameters.AddWithValue("@DCHD", true);
                if (DCPaid.Checked)
                {
                    cmd.Parameters.AddWithValue("@DCP", true);
                    if (DCCharges.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@DCCharges", DCCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For Downtown Columbia')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DCP", false);
                    cmd.Parameters.AddWithValue("@DCCharges", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@DCHD", false);
                cmd.Parameters.AddWithValue("@DCP", false);
                cmd.Parameters.AddWithValue("@DCCharges", DBNull.Value);
            }
            //Friday
            if (DCHomeDelivery0.Checked)
            {
                cmd.Parameters.AddWithValue("@DCHDFri", true);
                if (DCPaid0.Checked)
                {
                    cmd.Parameters.AddWithValue("@DCPFri", true);
                    if (DCCharges0.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@DCChargesFri", DCCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For Downtown Columbia')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DCPFri", false);
                    cmd.Parameters.AddWithValue("@DCChargesFri", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@DCHDFri", false);
                cmd.Parameters.AddWithValue("@DCPFri", false);
                cmd.Parameters.AddWithValue("@DCChargesFri", DBNull.Value);
            }
            
            if (JCHomeDelivery.Checked)
            {
                cmd.Parameters.AddWithValue("@JCHD", true);
                if (JCPaid.Checked)
                {
                    cmd.Parameters.AddWithValue("@JCP", true);
                    if (JCCharges.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@JCCharges", JCCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For Jefferson City')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@JCP", false);
                    cmd.Parameters.AddWithValue("@JCCharges", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@JCHD", false);
                cmd.Parameters.AddWithValue("@JCP", false);
                cmd.Parameters.AddWithValue("@JCCharges", DBNull.Value);
            }
            //Fri
            if (JCHomeDelivery0.Checked)
            {
                cmd.Parameters.AddWithValue("@JCHDFri", true);
                if (JCPaid0.Checked)
                {
                    cmd.Parameters.AddWithValue("@JCPFri", true);
                    if (JCCharges0.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@JCChargesFri", JCCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For Jefferson City')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@JCPFri", false);
                    cmd.Parameters.AddWithValue("@JCChargesFri", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@JCHDFri", false);
                cmd.Parameters.AddWithValue("@JCPFri", false);
                cmd.Parameters.AddWithValue("@JCChargesFri", DBNull.Value);
            }

            if (DHSSHomeDelivery.Checked)
            {
                cmd.Parameters.AddWithValue("@DHSSHD", true);
                if (DHSSPaid.Checked)
                {
                    cmd.Parameters.AddWithValue("@DHSSP", true);
                    if (DHSSCharges.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@DHSSCharges", DHSSCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For DHSS')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DHSSP", false);
                    cmd.Parameters.AddWithValue("@DHSSCharges", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@DHSSHD", false);
                cmd.Parameters.AddWithValue("@DHSSP", false);
                cmd.Parameters.AddWithValue("@DHSSCharges", DBNull.Value);
            }
            //Fri
            if (DHSSHomeDelivery0.Checked)
            {
                cmd.Parameters.AddWithValue("@DHSSHDFri", true);
                if (DHSSPaid0.Checked)
                {
                    cmd.Parameters.AddWithValue("@DHSSPFri", true);
                    if (DHSSCharges0.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@DHSSChargesFri", DHSSCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For DHSS')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DHSSPFri", false);
                    cmd.Parameters.AddWithValue("@DHSSChargesFri", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@DHSSHDFri", false);
                cmd.Parameters.AddWithValue("@DHSSPFri", false);
                cmd.Parameters.AddWithValue("@DHSSChargesFri", DBNull.Value);
            }

            if (MNHomeDelivery.Checked)
            {
                cmd.Parameters.AddWithValue("@MNHD", true);
                if (MNPaid.Checked)
                {
                    cmd.Parameters.AddWithValue("@MNP", true);
                    if (MNCharges.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@MNCharges", MNCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For Mizzou North')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MNP", false);
                    cmd.Parameters.AddWithValue("@MNCharges", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@MNHD", false);
                cmd.Parameters.AddWithValue("@MNP", false);
                cmd.Parameters.AddWithValue("@MNCharges", DBNull.Value);
            }
            //Fri
            if (MNHomeDelivery0.Checked)
            {
                cmd.Parameters.AddWithValue("@MNHDFri", true);
                if (MNPaid0.Checked)
                {
                    cmd.Parameters.AddWithValue("@MNPFri", true);
                    if (MNCharges0.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@MNChargesFri", MNCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For Mizzou North')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MNPFri", false);
                    cmd.Parameters.AddWithValue("@MNChargesFri", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@MNHDFri", false);
                cmd.Parameters.AddWithValue("@MNPFri", false);
                cmd.Parameters.AddWithValue("@MNChargesFri", DBNull.Value);
            }

            if (QHomeDelivery.Checked)
            {
                cmd.Parameters.AddWithValue("@QHD", true);
                if (QPaid.Checked)
                {
                    cmd.Parameters.AddWithValue("@QP", true);
                    if (QCharges.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@QCharges", QCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For QUARTERDECK')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@QP", false);
                    cmd.Parameters.AddWithValue("@QCharges", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@QHD", false);
                cmd.Parameters.AddWithValue("@QP", false);
                cmd.Parameters.AddWithValue("@QCharges", DBNull.Value);
            }
            //Fri
            if (QHomeDelivery0.Checked)
            {
                cmd.Parameters.AddWithValue("@QHDFri", true);
                if (QPaid0.Checked)
                {
                    cmd.Parameters.AddWithValue("@QPFri", true);
                    if (QCharges0.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@QChargesFri", QCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For QUARTERDECK')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@QPFri", false);
                    cmd.Parameters.AddWithValue("@QChargesFri", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@QHDFri", false);
                cmd.Parameters.AddWithValue("@QPFri", false);
                cmd.Parameters.AddWithValue("@QChargesFri", DBNull.Value);
            }

            if (UHHomeDelivery.Checked)
            {
                cmd.Parameters.AddWithValue("@UHHD", true);
                if (UHPaid.Checked)
                {
                    cmd.Parameters.AddWithValue("@UHP", true);
                    if (UHCharges.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@UHCharges", UHCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For University Hospital/School of Medicine')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@UHP", false);
                    cmd.Parameters.AddWithValue("@UHCharges", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@UHHD", false);
                cmd.Parameters.AddWithValue("@UHP", false);
                cmd.Parameters.AddWithValue("@UHCharges", DBNull.Value);
            }
            //Fri
            if (UHHomeDelivery0.Checked)
            {
                cmd.Parameters.AddWithValue("@UHHDFri", true);
                if (UHPaid0.Checked)
                {
                    cmd.Parameters.AddWithValue("@UHPFri", true);
                    if (UHCharges0.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@UHChargesFri", UHCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For University Hospital/School of Medicine')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@UHPFri", false);
                    cmd.Parameters.AddWithValue("@UHChargesFri", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@UHHDFri", false);
                cmd.Parameters.AddWithValue("@UHPFri", false);
                cmd.Parameters.AddWithValue("@UHChargesFri", DBNull.Value);
            }

            if (UMHHomeDelivery.Checked)
            {
                cmd.Parameters.AddWithValue("@UMHHD", true);
                if (UMHPaid.Checked)
                {
                    cmd.Parameters.AddWithValue("@UMHP", true);
                    if (UMHCharges.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@UMHCharges", UMHCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For UM Heinkel')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@UMHP", false);
                    cmd.Parameters.AddWithValue("@UMHCharges", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@UMHHD", false);
                cmd.Parameters.AddWithValue("@UMHP", false);
                cmd.Parameters.AddWithValue("@UMHCharges", DBNull.Value);
            }
            //Fri
            if (UMHHomeDelivery0.Checked)
            {
                cmd.Parameters.AddWithValue("@UMHHDFri", true);
                if (UMHPaid0.Checked)
                {
                    cmd.Parameters.AddWithValue("@UMHPFri", true);
                    if (UMHCharges0.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@UMHChargesFri", UMHCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For UM Heinkel')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@UMHPFri", false);
                    cmd.Parameters.AddWithValue("@UMHChargesFri", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@UMHHDFri", false);
                cmd.Parameters.AddWithValue("@UMHPFri", false);
                cmd.Parameters.AddWithValue("@UMHChargesFri", DBNull.Value);
            }



            cmd.ExecuteNonQuery();
            cn.Close();
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Save();", true);
        }
        else//Update Code
        {
            SqlConnection cn = Constant.Connection();
            cn.Open();
            SqlCommand cmd = new SqlCommand("Update ProductDetails Set ProductName=@ProductName,ProductDescription=@ProductDescription,ProductImage=@ProductImage,ImagePath=@ImagePath,ProductPrice=@ProductPrice,DC=@DC,DCThu=@DCThu,DCFri=@DCFri,JC=@JC,JCThu=@JCThu,JCFri=@JCFri,DHSS=@DHSS,DHSSThu=@DHSSThu,DHSSFri=@DHSSFri,MN=@MN,MNThu=@MNThu,MNFri=@MNFri,QUA=@QUA,QUAThu=@QUAThu,QUAFri=@QUAFri,UH=@UH,UHThu=@UHThu,UHFri=@UHFri,UMH=@UMH,UMHThu=@UMHThu,UMHFri=@UMHFri,PaymentType=@PaymentType,DCHD=@DCHD,DCP=@DCP,DCCharges=@DCCharges,JCHD=@JCHD,JCP=@JCP,JCCharges=@JCCharges,DHSSHD=@DHSSHD,DHSSP=@DHSSP,DHSSCharges=@DHSSCharges,MNHD=@MNHD,MNP=@MNP,MNCharges=@MNCharges,QHD=@QHD,QP=@QP,QCharges=@QCharges,UHHD=@UHHD,UHP=@UHP,UHCharges=@UHCharges,UMHHD=@UMHHD,UMHP=@UMHP,UMHCharges=@UMHCharges where ProductID='" + ProductID + "'", cn);
            cmd.Parameters.AddWithValue("@ProductName", txtProducts.Text.Trim());
            cmd.Parameters.AddWithValue("@ProductDescription", txtPDescription.Text.Trim());
            cmd.Parameters.AddWithValue("@ProductImage", FName);
            cmd.Parameters.AddWithValue("@ImagePath", FPath);
            cmd.Parameters.AddWithValue("@ProductPrice", txtPPrice.Text.Trim());
            if (cbDowntownColumbia.Checked)
            {
                if (DCFri.Checked || DCThu.Checked)
                    cmd.Parameters.AddWithValue("@DC", true);
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Atleast One Day For Downtown Columbia')", true);
                    return;
                }
            }
            else
                cmd.Parameters.AddWithValue("@DC", false);
            if (DCThu.Checked)
                cmd.Parameters.AddWithValue("@DCThu", true);
            else
                cmd.Parameters.AddWithValue("@DCThu", false);
            if (DCFri.Checked)
                cmd.Parameters.AddWithValue("@DCFri", true);
            else
                cmd.Parameters.AddWithValue("@DCFri", false);


            if (cbJeffersonCity.Checked)
            {
                if (JCThu.Checked || JSFri.Checked)
                    cmd.Parameters.AddWithValue("@JC", true);
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Atleast One Day For Jefferson City')", true);
                    return;
                }
            }
            else
                cmd.Parameters.AddWithValue("@JC", false);
            if (JCThu.Checked)
                cmd.Parameters.AddWithValue("@JCThu", true);
            else
                cmd.Parameters.AddWithValue("@JCThu", false);
            if (JSFri.Checked)
                cmd.Parameters.AddWithValue("@JCFri", true);
            else
                cmd.Parameters.AddWithValue("@JCFri", false);


            if (cbDHSS.Checked)
            {
                if (DHSSFri.Checked || DHSSThu.Checked)
                    cmd.Parameters.AddWithValue("@DHSS", true);
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Atleast One Day For DHSS')", true);
                    return;
                }
            }
            else
                cmd.Parameters.AddWithValue("@DHSS", false);
            if (DHSSThu.Checked)
                cmd.Parameters.AddWithValue("@DHSSThu", true);
            else
                cmd.Parameters.AddWithValue("@DHSSThu", false);
            if (DHSSFri.Checked)
                cmd.Parameters.AddWithValue("@DHSSFri", true);
            else
                cmd.Parameters.AddWithValue("@DHSSFri", false);
            if (cbMizzouNorth.Checked)
            {
                if (MNFri.Checked || MNThu.Checked)
                    cmd.Parameters.AddWithValue("@MN", true);
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Atleast One Day For Mizzou North')", true);
                    return;
                }
            }
            else
                cmd.Parameters.AddWithValue("@MN", false);
            if (MNThu.Checked)
                cmd.Parameters.AddWithValue("@MNThu", true);
            else
                cmd.Parameters.AddWithValue("@MNThu", false);
            if (MNFri.Checked)
                cmd.Parameters.AddWithValue("@MNFri", true);
            else
                cmd.Parameters.AddWithValue("@MNFri", false);

            if (cbQUARTERDECK.Checked)
            {
                if (QFri.Checked || QThu.Checked)
                    cmd.Parameters.AddWithValue("@QUA", true);
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Atleast One Day For QUARTERDECK')", true);
                    return;
                }
            }
            else
                cmd.Parameters.AddWithValue("@QUA", false);
            if (QThu.Checked)
                cmd.Parameters.AddWithValue("@QUAThu", true);
            else
                cmd.Parameters.AddWithValue("@QUAThu", false);

            if (QFri.Checked)
                cmd.Parameters.AddWithValue("@QUAFri", true);
            else
                cmd.Parameters.AddWithValue("@QUAFri", false);

            if (cbUniversityHospital.Checked)
            {
                if (UHThu.Checked || UHFri.Checked)
                    cmd.Parameters.AddWithValue("@UH", true);
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Atleast One Day For University Hospital/School of Medicine')", true);
                    return;
                }
            }
            else
                cmd.Parameters.AddWithValue("@UH", false);
            if (UHThu.Checked)
                cmd.Parameters.AddWithValue("@UHThu", true);
            else
                cmd.Parameters.AddWithValue("@UHThu", false);

            if (UHFri.Checked)
                cmd.Parameters.AddWithValue("@UHFri", true);
            else
                cmd.Parameters.AddWithValue("@UHFri", false);

            if (cbUMHeinkel.Checked)
            {
                if (UMHFri.Checked || UMHThu.Checked)
                    cmd.Parameters.AddWithValue("@UMH", true);
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Atleast One Day For UM Heinkel')", true);
                    return;
                }
            }
            else
                cmd.Parameters.AddWithValue("@UMH", false);

            if (UMHThu.Checked)
                cmd.Parameters.AddWithValue("@UMHThu", true);
            else
                cmd.Parameters.AddWithValue("@UMHThu", false);

            if (UMHFri.Checked)
                cmd.Parameters.AddWithValue("@UMHFri", true);
            else
                cmd.Parameters.AddWithValue("@UMHFri", false);


            if (rblPayment.SelectedIndex == 0)
                cmd.Parameters.AddWithValue("@PaymentType", true);
            else
                cmd.Parameters.AddWithValue("@PaymentType", false);

            /***************Changes***********/
            if (DCHomeDelivery.Checked)
            {
                cmd.Parameters.AddWithValue("@DCHD", true);
                if (DCPaid.Checked)
                {
                    cmd.Parameters.AddWithValue("@DCP", true);
                    if (DCCharges.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@DCCharges", DCCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For Downtown Columbia')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DCP", false);
                    cmd.Parameters.AddWithValue("@DCCharges", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@DCHD", false);
                cmd.Parameters.AddWithValue("@DCP", false);
                cmd.Parameters.AddWithValue("@DCCharges", DBNull.Value);
            }
            //Friday
            if (DCHomeDelivery0.Checked)
            {
                cmd.Parameters.AddWithValue("@DCHDFri", true);
                if (DCPaid0.Checked)
                {
                    cmd.Parameters.AddWithValue("@DCPFri", true);
                    if (DCCharges0.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@DCChargesFri", DCCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For Downtown Columbia')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DCPFri", false);
                    cmd.Parameters.AddWithValue("@DCChargesFri", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@DCHDFri", false);
                cmd.Parameters.AddWithValue("@DCPFri", false);
                cmd.Parameters.AddWithValue("@DCChargesFri", DBNull.Value);
            }

            if (JCHomeDelivery.Checked)
            {
                cmd.Parameters.AddWithValue("@JCHD", true);
                if (JCPaid.Checked)
                {
                    cmd.Parameters.AddWithValue("@JCP", true);
                    if (JCCharges.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@JCCharges", JCCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For Jefferson City')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@JCP", false);
                    cmd.Parameters.AddWithValue("@JCCharges", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@JCHD", false);
                cmd.Parameters.AddWithValue("@JCP", false);
                cmd.Parameters.AddWithValue("@JCCharges", DBNull.Value);
            }
            //Fri
            if (JCHomeDelivery0.Checked)
            {
                cmd.Parameters.AddWithValue("@JCHDFri", true);
                if (JCPaid0.Checked)
                {
                    cmd.Parameters.AddWithValue("@JCPFri", true);
                    if (JCCharges0.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@JCChargesFri", JCCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For Jefferson City')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@JCPFri", false);
                    cmd.Parameters.AddWithValue("@JCChargesFri", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@JCHDFri", false);
                cmd.Parameters.AddWithValue("@JCPFri", false);
                cmd.Parameters.AddWithValue("@JCChargesFri", DBNull.Value);
            }

            if (DHSSHomeDelivery.Checked)
            {
                cmd.Parameters.AddWithValue("@DHSSHD", true);
                if (DHSSPaid.Checked)
                {
                    cmd.Parameters.AddWithValue("@DHSSP", true);
                    if (DHSSCharges.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@DHSSCharges", DHSSCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For DHSS')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DHSSP", false);
                    cmd.Parameters.AddWithValue("@DHSSCharges", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@DHSSHD", false);
                cmd.Parameters.AddWithValue("@DHSSP", false);
                cmd.Parameters.AddWithValue("@DHSSCharges", DBNull.Value);
            }
            //Fri
            if (DHSSHomeDelivery0.Checked)
            {
                cmd.Parameters.AddWithValue("@DHSSHDFri", true);
                if (DHSSPaid0.Checked)
                {
                    cmd.Parameters.AddWithValue("@DHSSPFri", true);
                    if (DHSSCharges0.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@DHSSChargesFri", DHSSCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For DHSS')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DHSSPFri", false);
                    cmd.Parameters.AddWithValue("@DHSSChargesFri", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@DHSSHDFri", false);
                cmd.Parameters.AddWithValue("@DHSSPFri", false);
                cmd.Parameters.AddWithValue("@DHSSChargesFri", DBNull.Value);
            }

            if (MNHomeDelivery.Checked)
            {
                cmd.Parameters.AddWithValue("@MNHD", true);
                if (MNPaid.Checked)
                {
                    cmd.Parameters.AddWithValue("@MNP", true);
                    if (MNCharges.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@MNCharges", MNCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For Mizzou North')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MNP", false);
                    cmd.Parameters.AddWithValue("@MNCharges", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@MNHD", false);
                cmd.Parameters.AddWithValue("@MNP", false);
                cmd.Parameters.AddWithValue("@MNCharges", DBNull.Value);
            }
            //Fri
            if (MNHomeDelivery0.Checked)
            {
                cmd.Parameters.AddWithValue("@MNHDFri", true);
                if (MNPaid0.Checked)
                {
                    cmd.Parameters.AddWithValue("@MNPFri", true);
                    if (MNCharges0.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@MNChargesFri", MNCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For Mizzou North')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MNPFri", false);
                    cmd.Parameters.AddWithValue("@MNChargesFri", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@MNHDFri", false);
                cmd.Parameters.AddWithValue("@MNPFri", false);
                cmd.Parameters.AddWithValue("@MNChargesFri", DBNull.Value);
            }

            if (QHomeDelivery.Checked)
            {
                cmd.Parameters.AddWithValue("@QHD", true);
                if (QPaid.Checked)
                {
                    cmd.Parameters.AddWithValue("@QP", true);
                    if (QCharges.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@QCharges", QCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For QUARTERDECK')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@QP", false);
                    cmd.Parameters.AddWithValue("@QCharges", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@QHD", false);
                cmd.Parameters.AddWithValue("@QP", false);
                cmd.Parameters.AddWithValue("@QCharges", DBNull.Value);
            }
            //Fri
            if (QHomeDelivery0.Checked)
            {
                cmd.Parameters.AddWithValue("@QHDFri", true);
                if (QPaid0.Checked)
                {
                    cmd.Parameters.AddWithValue("@QPFri", true);
                    if (QCharges0.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@QChargesFri", QCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For QUARTERDECK')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@QPFri", false);
                    cmd.Parameters.AddWithValue("@QChargesFri", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@QHDFri", false);
                cmd.Parameters.AddWithValue("@QPFri", false);
                cmd.Parameters.AddWithValue("@QChargesFri", DBNull.Value);
            }

            if (UHHomeDelivery.Checked)
            {
                cmd.Parameters.AddWithValue("@UHHD", true);
                if (UHPaid.Checked)
                {
                    cmd.Parameters.AddWithValue("@UHP", true);
                    if (UHCharges.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@UHCharges", UHCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For University Hospital/School of Medicine')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@UHP", false);
                    cmd.Parameters.AddWithValue("@UHCharges", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@UHHD", false);
                cmd.Parameters.AddWithValue("@UHP", false);
                cmd.Parameters.AddWithValue("@UHCharges", DBNull.Value);
            }
            //Fri
            if (UHHomeDelivery0.Checked)
            {
                cmd.Parameters.AddWithValue("@UHHDFri", true);
                if (UHPaid0.Checked)
                {
                    cmd.Parameters.AddWithValue("@UHPFri", true);
                    if (UHCharges0.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@UHChargesFri", UHCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For University Hospital/School of Medicine')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@UHPFri", false);
                    cmd.Parameters.AddWithValue("@UHChargesFri", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@UHHDFri", false);
                cmd.Parameters.AddWithValue("@UHPFri", false);
                cmd.Parameters.AddWithValue("@UHChargesFri", DBNull.Value);
            }

            if (UMHHomeDelivery.Checked)
            {
                cmd.Parameters.AddWithValue("@UMHHD", true);
                if (UMHPaid.Checked)
                {
                    cmd.Parameters.AddWithValue("@UMHP", true);
                    if (UMHCharges.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@UMHCharges", UMHCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For UM Heinkel')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@UMHP", false);
                    cmd.Parameters.AddWithValue("@UMHCharges", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@UMHHD", false);
                cmd.Parameters.AddWithValue("@UMHP", false);
                cmd.Parameters.AddWithValue("@UMHCharges", DBNull.Value);
            }
            //Fri
            if (UMHHomeDelivery0.Checked)
            {
                cmd.Parameters.AddWithValue("@UMHHDFri", true);
                if (UMHPaid0.Checked)
                {
                    cmd.Parameters.AddWithValue("@UMHPFri", true);
                    if (UMHCharges0.Text != string.Empty)
                        cmd.Parameters.AddWithValue("@UMHChargesFri", UMHCharges.Text);
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Insert Charges For UM Heinkel')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@UMHPFri", false);
                    cmd.Parameters.AddWithValue("@UMHChargesFri", DBNull.Value);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@UMHHDFri", false);
                cmd.Parameters.AddWithValue("@UMHPFri", false);
                cmd.Parameters.AddWithValue("@UMHChargesFri", DBNull.Value);
            }

            cmd.ExecuteNonQuery();
            cn.Close();
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fail", "Update();", true);
        }
    }
    protected void cbDowntownColumbia_CheckedChanged(object sender, EventArgs e)
    {
        if (cbDowntownColumbia.Checked)
        {
            DCFri.Enabled = true;
            DCThu.Enabled = true;
            DCHomeDelivery.Enabled = true;
            DCHomeDelivery0.Enabled = true;
           
        }
        else
        {
            DCThu.Checked = false;
            DCFri.Checked = false;
            DCFri.Enabled = false;
            DCThu.Enabled = false;
            DCHomeDelivery.Enabled = false;
            DCHomeDelivery0.Enabled = false;
            
        }
    }
    protected void cbJeffersonCity_CheckedChanged(object sender, EventArgs e)
    {
        if (cbJeffersonCity.Checked)
        {
            JCThu.Enabled = true;
            JSFri.Enabled = true;
            JCHomeDelivery.Enabled = true;
            JCHomeDelivery0.Enabled = true;  
        }
        else
        {
            JCThu.Checked = false;
            JSFri.Checked = false;
            JCThu.Enabled = false;
            JSFri.Enabled = false;
            JCHomeDelivery.Enabled = false;
            JCHomeDelivery0.Enabled = false;  
        }
    }
    protected void cbDHSS_CheckedChanged(object sender, EventArgs e)
    {
        if (cbDHSS.Checked)
        {
            DHSSThu.Enabled = true;
            DHSSFri.Enabled = true;
            DHSSHomeDelivery.Enabled = true;
            DHSSHomeDelivery0.Enabled = true;            
        }
        else
        {
            DHSSThu.Checked = false;
            DHSSFri.Checked = false;
            DHSSThu.Enabled = false;
            DHSSFri.Enabled = false;
            DHSSHomeDelivery.Enabled = false;
            DHSSHomeDelivery0.Enabled = false;
            
        }
    }
    protected void cbMizzouNorth_CheckedChanged(object sender, EventArgs e)
    {
        if (cbMizzouNorth.Checked)
        {
            MNThu.Enabled = true;
            MNFri.Enabled = true;
            MNHomeDelivery.Enabled = true;
            MNHomeDelivery0.Enabled = true;
            
        }
        else
        {
            MNThu.Checked = false;
            MNFri.Checked = false;
            MNThu.Enabled = false;
            MNFri.Enabled = false;
            MNHomeDelivery.Enabled = false;
            MNHomeDelivery.Enabled = false;
            
        }
    }
    protected void cbQUARTERDECK_CheckedChanged(object sender, EventArgs e)
    {
        if (cbQUARTERDECK.Checked)
        {
            QThu.Enabled = true;
            QFri.Enabled = true;
            QHomeDelivery.Enabled = true;
            QHomeDelivery0.Enabled = true;
        }
        else
        {
            QThu.Checked = false;
            QFri.Checked = false;
            QThu.Enabled = false;
            QFri.Enabled = false;
            QHomeDelivery.Enabled = false;
            QHomeDelivery0.Enabled = false;           
        }
    }
    protected void cbUniversityHospital_CheckedChanged(object sender, EventArgs e)
    {
        if (cbUniversityHospital.Checked)
        {
            UHThu.Enabled = true;
            UHFri.Enabled = true;
            UHHomeDelivery.Enabled = true;
            UHHomeDelivery0.Enabled = true;
        }
        else
        {
            UHThu.Checked = false;
            UHFri.Checked = false;
            UHThu.Enabled = false;
            UHFri.Enabled = false;
            UHHomeDelivery.Enabled = false;
            UHHomeDelivery0.Enabled = false;
        }
    }
    protected void cbUMHeinkel_CheckedChanged(object sender, EventArgs e)
    {
        if (cbUMHeinkel.Checked)
        {
            UMHThu.Enabled = true;
            UMHFri.Enabled = true;
            UMHHomeDelivery.Enabled = true;
            UMHHomeDelivery0.Enabled = true;
        }
        else
        {
            UMHThu.Checked = false;
            UMHFri.Checked = false;
            UMHThu.Enabled = false;
            UMHFri.Enabled = false;
            UMHHomeDelivery.Enabled = true;
            UMHHomeDelivery0.Enabled = true;
        }
    }
    /// <summary>
    /// Edit Or Delete Product
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvProduct_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete1")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            SqlConnection cn = Constant.Connection();
            SqlCommand cmd = new SqlCommand("Delete From ProductDetails where ProductID=" + index + "", cn);
            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
            Response.Redirect("~/Admin/Products.aspx");
        }

        if (e.CommandName == "Edit1")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            Response.Redirect("~/Admin/Products.aspx?ProductID=" + EncryptDecrypt.EncryptPassword(index.ToString()));
        }
    }

    protected void DCHomeDelivery_CheckedChanged(object sender, EventArgs e)
    {
        if (DCHomeDelivery.Checked)        
            DCPaid.Enabled = true;        
        else
            DCPaid.Enabled = false;
    }
    protected void JCHomeDelivery_CheckedChanged(object sender, EventArgs e)
    {
        if (JCHomeDelivery.Checked)
            JCPaid.Enabled = true;
        else
            JCPaid.Enabled = false;
    }
    protected void DHSSHomeDelivery_CheckedChanged(object sender, EventArgs e)
    {
        if (DHSSHomeDelivery.Checked)
            DHSSPaid.Enabled = true;
        else
            DHSSPaid.Enabled = false;
    }
    protected void MNHomeDelivery_CheckedChanged(object sender, EventArgs e)
    {
        if (MNHomeDelivery.Checked)
            MNPaid.Enabled = true;
        else
            MNPaid.Enabled = false;
    }
    protected void QHomeDelivery_CheckedChanged(object sender, EventArgs e)
    {
        if (QHomeDelivery.Checked)
            QPaid.Enabled = true;
        else
            QPaid.Enabled = false;
    }
    protected void UHHomeDelivery_CheckedChanged(object sender, EventArgs e)
    {
        if (UHHomeDelivery.Checked)
            UHPaid.Enabled = true;
        else
            UHPaid.Enabled = false;
    }
    protected void UMHHomeDelivery_CheckedChanged(object sender, EventArgs e)
    {
        if (UMHHomeDelivery.Checked)
            UMHPaid.Enabled = true;
        else
            UMHPaid.Enabled = false;
    }
    protected void DCPaid_CheckedChanged(object sender, EventArgs e)
    {
        if (DCPaid.Checked)
            DCCharges.Enabled = true;
        else
            DCCharges.Enabled = false;
    }
    protected void JCPaid_CheckedChanged(object sender, EventArgs e)
    {
        if (JCPaid.Checked)
            JCCharges.Enabled = true;
        else
            JCCharges.Enabled = false;
    }
    protected void DHSSPaid_CheckedChanged(object sender, EventArgs e)
    {
        if (DHSSPaid.Checked)
            DHSSCharges.Enabled = true;
        else
            DHSSCharges.Enabled = false;
    }
    protected void MNPaid_CheckedChanged(object sender, EventArgs e)
    {
        if (MNPaid.Checked)
            MNCharges.Enabled = true;
        else
            MNCharges.Enabled = false;
    }
    protected void QPaid_CheckedChanged(object sender, EventArgs e)
    {
        if (QPaid.Checked)
            QCharges.Enabled = true;
        else
            QCharges.Enabled = false;
    }
    protected void UHPaid_CheckedChanged(object sender, EventArgs e)
    {
        if (UHPaid.Checked)
            UHCharges.Enabled = true;
        else
            UHCharges.Enabled = false;
    }
    protected void UMHPaid_CheckedChanged(object sender, EventArgs e)
    {
        if (UMHPaid.Checked)
            UMHCharges.Enabled = true;
        else
            UMHCharges.Enabled = false;
    }
    protected void DCHomeDelivery0_CheckedChanged(object sender, EventArgs e)
    {

        if (DCHomeDelivery0.Checked)
            DCPaid0.Enabled = true;
        else
            DCPaid0.Enabled = false;
    }
    protected void JCHomeDelivery0_CheckedChanged(object sender, EventArgs e)
    {
        if (JCHomeDelivery0.Checked)
            JCPaid0.Enabled = true;
        else
            JCPaid0.Enabled = false;
    }
    protected void DHSSHomeDelivery0_CheckedChanged(object sender, EventArgs e)
    {
        if (DHSSHomeDelivery0.Checked)
            DHSSPaid0.Enabled = true;
        else
            DHSSPaid0.Enabled = false;
    }
    protected void MNHomeDelivery0_CheckedChanged(object sender, EventArgs e)
    {
        if (MNHomeDelivery0.Checked)
            MNPaid0.Enabled = true;
        else
            MNPaid0.Enabled = false;
    }
    protected void QHomeDelivery0_CheckedChanged(object sender, EventArgs e)
    {
        if (QHomeDelivery0.Checked)
            QPaid0.Enabled = true;
        else
            QPaid0.Enabled = false;
    }
    protected void UHHomeDelivery0_CheckedChanged(object sender, EventArgs e)
    {
        if (UHHomeDelivery0.Checked)
            UHPaid0.Enabled = true;
        else
            UHPaid0.Enabled = false;
    }
    protected void UMHHomeDelivery0_CheckedChanged(object sender, EventArgs e)
    {
        if (UMHHomeDelivery0.Checked)
            UMHPaid0.Enabled = true;
        else
            UMHPaid0.Enabled = false;
    }
    protected void DCPaid0_CheckedChanged(object sender, EventArgs e)
    {
        if (DCPaid0.Checked)
            DCCharges0.Enabled = true;
        else
            DCCharges0.Enabled = false;
    }
    protected void JCPaid0_CheckedChanged(object sender, EventArgs e)
    {
        if (JCPaid0.Checked)
            JCCharges0.Enabled = true;
        else
            JCCharges0.Enabled = false;
    }
    protected void DHSSPaid0_CheckedChanged(object sender, EventArgs e)
    {
        if (DHSSPaid0.Checked)
            DHSSCharges0.Enabled = true;
        else
            DHSSCharges0.Enabled = false;
    }
    protected void MNPaid0_CheckedChanged(object sender, EventArgs e)
    {
        if (MNPaid0.Checked)
            MNCharges0.Enabled = true;
        else
            MNCharges0.Enabled = false;
    }
    protected void QPaid0_CheckedChanged(object sender, EventArgs e)
    {
        if (QPaid0.Checked)
            QCharges0.Enabled = true;
        else
            QCharges0.Enabled = false;
    }
    protected void UHPaid0_CheckedChanged(object sender, EventArgs e)
    {
        if (UHPaid0.Checked)
            UHCharges0.Enabled = true;
        else
            UHCharges0.Enabled = false;
    }
    protected void UMHPaid0_CheckedChanged(object sender, EventArgs e)
    {
        if (UMHPaid0.Checked)
            UMHCharges0.Enabled = true;
        else
            UMHCharges0.Enabled = false;
    }
}