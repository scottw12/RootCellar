
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Web.UI.WebControls;

partial class admin_UpdMC : System.Web.UI.Page
{
    int i = 0;
    private void UpdMailChimp(string email, bool Bounty, bool Barnyard, bool Ploughman, string fname, string lname, string PUday)
    {
        string webAddr = "";
        try
        {
            if (Bounty == true)
            {
                try
                {
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=f310b8a278&email[email]=" + email + "&merge_vars[FNAME]=" + fname + "&merge_vars[LNAME]=" + lname + "&merge_vars[MMERGE3]=" + PUday + "&double_optin=false&send_welcome=false";
                    Uri FwebAddr = new Uri(webAddr);
                    dynamic httpWebRequest = (HttpWebRequest)WebRequest.Create(FwebAddr);
                    httpWebRequest.ContentType = "application/json";
                    dynamic httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        dynamic val = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else if (Bounty == false)
            {
                try
                {
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/unsubscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=f310b8a278&email[email]=" + email + "&merge_vars[FNAME]=" + fname + "&merge_vars[LNAME]=" + lname + "&merge_vars[MMERGE3]=" + PUday + "&double_optin=false&send_welcome=false";
                    Uri FwebAddr = new Uri(webAddr);
                    dynamic httpWebRequest = (HttpWebRequest)WebRequest.Create(FwebAddr);
                    httpWebRequest.ContentType = "application/json";
                    dynamic httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        dynamic val = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (Barnyard == true)
            {
                try
                {
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=1ad43508d8&email[email]=" + email + "&merge_vars[FNAME]=" + fname + "&merge_vars[LNAME]=" + lname + "&merge_vars[MMERGE3]=" + PUday + "&double_optin=false&send_welcome=false";
                    Uri FwebAddr = new Uri(webAddr);
                    dynamic httpWebRequest = (HttpWebRequest)WebRequest.Create(FwebAddr);
                    httpWebRequest.ContentType = "application/json";
                    dynamic httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        dynamic val = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else if (Barnyard == false)
            {
                try
                {
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/unsubscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=1ad43508d8&email[email]=" + email + "&merge_vars[FNAME]=" + fname + "&merge_vars[LNAME]=" + lname + "&merge_vars[MMERGE3]=" + PUday + "&double_optin=false&send_welcome=false";
                    Uri FwebAddr = new Uri(webAddr);
                    dynamic httpWebRequest = (HttpWebRequest)WebRequest.Create(FwebAddr);
                    httpWebRequest.ContentType = "application/json";
                    dynamic httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        dynamic val = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (Ploughman == true)
            {
                try
                {
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/subscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=078a386ef9&email[email]=" + email + "&merge_vars[FNAME]=" + fname + "&merge_vars[LNAME]=" + lname + "&merge_vars[MMERGE3]=" + PUday + "&double_optin=false&send_welcome=false";
                    Uri FwebAddr = new Uri(webAddr);
                    dynamic httpWebRequest = (HttpWebRequest)WebRequest.Create(FwebAddr);
                    httpWebRequest.ContentType = "application/json";
                    dynamic httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        dynamic val = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else if (Ploughman == false)
            {
                try
                {
                    webAddr = "https://us2.api.mailchimp.com/2.0/lists/unsubscribe.json?apikey=0f0230afc9931da21572a4b6a00e5c4e-us2&id=078a386ef9&email[email]=" + email + "&merge_vars[FNAME]=" + fname + "&merge_vars[LNAME]=" + lname + "&merge_vars[MMERGE3]=" + PUday + "&double_optin=false&send_welcome=false";
                    Uri FwebAddr = new Uri(webAddr);
                    dynamic httpWebRequest = (HttpWebRequest)WebRequest.Create(FwebAddr);
                    httpWebRequest.ContentType = "application/json";
                    dynamic httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        dynamic val = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
            Literal0.Text += "<br /><br />" + ex.Message + "<br />" + ex.StackTrace + "<br />" + webAddr;
        }

    }

    protected void gv1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox ck1 = (CheckBox)e.Row.Cells[4].Controls[0];
            CheckBox ck2 = (CheckBox)e.Row.Cells[5].Controls[0];
            CheckBox ck3 = (CheckBox)e.Row.Cells[6].Controls[0];
            UpdMailChimp(e.Row.Cells[2].Text, ck1.Checked, ck2.Checked, ck3.Checked, e.Row.Cells[0].Text, e.Row.Cells[1].Text, e.Row.Cells[3].Text);
            i += 1;
            Literal0.Text += "<br />" + i.ToString() + " completed.";
        }
    }
    protected void gv2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox ck1 = (CheckBox)e.Row.Cells[4].Controls[0];
            CheckBox ck2 = (CheckBox)e.Row.Cells[5].Controls[0];
            CheckBox ck3 = (CheckBox)e.Row.Cells[6].Controls[0];
            UpdMailChimp(e.Row.Cells[2].Text, ck1.Checked, ck2.Checked, ck3.Checked, e.Row.Cells[0].Text, e.Row.Cells[1].Text, e.Row.Cells[3].Text);
            i += 1;
            Literal0.Text += "<br />" + i.ToString() + " completed.";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Literal0.Text = "";
    }
    public admin_UpdMC()
    {
        Load += Page_Load;
    }
}

