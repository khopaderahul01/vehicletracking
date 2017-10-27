using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPS
{
    public partial class orgAdmin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["role"] == null))
            {
                Response.Redirect("Default.aspx");
            }


            if (!IsPostBack)
            {
                lbluser.Text = Session["UserName"].ToString();
                // imglogo.ImageUrl = Session["orglogourl"].ToString();
            }
        }


        protected void lbtnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Default.aspx");
        }
    }
}