using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPSTrackingBLL;
using System.Data;
using System.IO;


namespace GPS
{

    public partial class _Default : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnLogin_Click(object sender, ImageClickEventArgs e)
        {
            cls_User obj_usr = new cls_User();
            DataSet ds = new DataSet();
            ds = obj_usr.fn_Login(txtbLoginName.Text, txtbPassword.Text, 2);           
            if (ds.Tables[0].Rows.Count > 0)
            {
                DateTime expiresOn = DateTime.Parse(ds.Tables[0].Rows[0]["expiresOn"].ToString());

                TimeSpan span = expiresOn - DateTime.Now;
                double i=span.TotalSeconds;
                if(expiresOn<DateTime.Now)
                {
                    lblmsg.Text = "User account is not active..";
                }
                else
                {
                    Session["UserName"] = ds.Tables[0].Rows[0]["UserName"].ToString();
                    Session["loginid"] = ds.Tables[0].Rows[0]["ID"].ToString();
                    Session["userID"] = ds.Tables[0].Rows[0]["loginID"].ToString();
                    Session["role"] = ds.Tables[0].Rows[0]["Role"].ToString();


                    if (ds.Tables[0].Rows[0]["Role"].ToString() == "1") // 1 is role id of superAdmin
                    {
                        Session["fk_CompanyID"] = 0;
                        Session["fk_OrgID"] = 0;                
                        Response.Redirect("dashboard.aspx");                        
                    }
                    else if (ds.Tables[0].Rows[0]["Role"].ToString() == "10") //10 is  role id of company admin 
                    {
                        Session["fk_CompanyID"] = ds.Tables[0].Rows[0]["fk_CompanyID"].ToString();
                        Session["fk_OrgID"] =0;
                        Response.Redirect("dashboard.aspx");
                    }
                    else if (ds.Tables[0].Rows[0]["Role"].ToString() == "20") // 20 is role id of orgadmin user
                    {
                        Session["fk_CompanyID"] = 0;
                        Session["fk_OrgID"] = ds.Tables[0].Rows[0]["fk_OrgID"].ToString();
                        Response.Redirect("dashboard.aspx");
                    }
                    else if (ds.Tables[0].Rows[0]["Role"].ToString() == "30") // 30 is role id of normal user
                    {           
                        Session["fk_CompanyID"] = 0;
                        Session["fk_OrgID"] = ds.Tables[0].Rows[0]["fk_OrgID"].ToString();
                        Response.Redirect("dashboard.aspx");
                    }
                    else if (ds.Tables[0].Rows[0]["Role"].ToString() == "50") // 30 is role id of normal user
                    {
                        Session["fk_CompanyID"] = ds.Tables[0].Rows[0]["ID"].ToString();
                        Session["fk_OrgID"] = ds.Tables[0].Rows[0]["fk_OrgID"].ToString();
                        Response.Redirect("dashboard.aspx");
                    }
                }
               
               
            }
            else
                lblmsg.Text = "Wrong Credentials..";
        }
        protected void lnkbtnforgotpass_Click(object sender, EventArgs e)
        {
            Response.Redirect("ForgotPassword.aspx");
        }
    }
}