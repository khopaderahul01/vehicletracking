using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace GPS
{
    public partial class userMgmt : System.Web.UI.Page
    {

        SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ToString());
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["role"] == null))
            {
                Response.Redirect("Default.aspx");
            }
            if (!IsPostBack)
            {
                FetchUsers();
                lblPWDLength.Text = "";            
            }
        }
        void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["role"] != null)
            {
                int role = int.Parse(Session["role"].ToString());
                if (role == 10) //10 is  role id of company admin 
                {
                    this.MasterPageFile = "companyAdmin.Master";
                }
                else if (role == 20) // 20 is role id of orgadmin user
                {
                    this.MasterPageFile = "orgAdmin.Master";
                }
                else if (role == 50) // 30 is role id of normal user
                {
                    this.MasterPageFile = "NormalUser.Master";
                }
            }
        }
        private void FetchUsers()
        {

            try
            {               
                {
                    SqlCommand cmd = new SqlCommand("mts_User_Select", Connection);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@role", SqlDbType.NVarChar).Value = int.Parse(Session["role"].ToString());
                    cmd.Parameters.Add("@fk_CompanyID", SqlDbType.NVarChar).Value = int.Parse(Session["fk_CompanyID"].ToString());
                    cmd.Parameters.Add("@fk_OrgID", SqlDbType.NVarChar).Value = int.Parse(Session["fk_OrgID"].ToString());
                    if (int.Parse(Session["role"].ToString()) == 50)
                    {
                        cmd.Parameters.Add("@userID", SqlDbType.NVarChar).Value = int.Parse(Session["userID"].ToString());
                    }
                    else
                    {
                        cmd.Parameters.Add("@userID", SqlDbType.NVarChar).Value = 0;
                    }
                    SqlDataAdapter dap = new SqlDataAdapter(cmd);
                    DataSet DS = new DataSet();
                    dap.Fill(DS);

                    if (DS != null)
                    {
                        if (DS.Tables[0].Rows.Count != 0)
                        {
                            UserGrid.Visible = true;
                            UserGrid.DataSource = DS;
                            UserGrid.DataBind();
                        }

                        else
                        {
                            UserGrid.Visible = false;
                            lblUserPermission.Text = "No Records Found..";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        //protected void Fetch_AllCompanyUsers()
        //{
        //    SqlCommand cmd = new SqlCommand("spCommonSelectStmt", Connection);
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "Company";
        //    cmd.Parameters.Add("@Condition", SqlDbType.NVarChar).Value = "UserID=" + UserInfo.UserID;

        //    SqlDataAdapter dap = new SqlDataAdapter(cmd);
        //    DataSet DS = new DataSet();
        //    dap.Fill(DS);
        //    int CompanyId = 0;
        //    if (DS != null)
        //    {
        //        if (DS.Tables[0].Rows.Count != 0)
        //        {
        //            CompanyId = Convert.ToInt32(DS.Tables[0].Rows[0]["companyID"].ToString());
        //        }
        //    }


        //    SqlCommand cmd2 = new SqlCommand("spCommonSelectStmt", Connection);
        //    cmd2.CommandType = CommandType.StoredProcedure;

        //    cmd2.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "organisation";
        //    cmd2.Parameters.Add("@Condition", SqlDbType.NVarChar).Value = "companyFId=" + CompanyId;

        //    SqlDataAdapter dap2 = new SqlDataAdapter(cmd2);
        //    DataSet DS2 = new DataSet();
        //    dap2.Fill(DS2);
        //    int orgID = 0;
        //    if (DS2 != null)
        //    {
        //        if (DS2.Tables[0].Rows.Count != 0)
        //        {
        //            for (int i = 0; i < DS2.Tables[0].Rows.Count; i++)
        //            {
        //                orgID = Convert.ToInt32(DS2.Tables[0].Rows[i]["orgId"].ToString());
        //                //bind_Users(orgID);

        //                SqlCommand cmd1 = new SqlCommand("sp_Users_select", Connection);
        //                cmd1.CommandType = CommandType.StoredProcedure;

        //                cmd1.Parameters.Add("@orgID", SqlDbType.NVarChar).Value = orgID;

        //                SqlDataAdapter dap1 = new SqlDataAdapter(cmd1);
        //                DataSet DS1 = new DataSet();
        //                dap1.Fill(DS1);

        //                if (DS1 != null)
        //                {
        //                    if (DS1.Tables[0].Rows.Count != 0)
        //                    {

        //                        UserGrid.DataSource = DS1.Tables[0];

        //                        UserGrid.DataBind();

        //                    }

        //                    else
        //                    {

        //                    }

        //                }
        //            }
        //        }
        //    }


        //}

        private void bind_Users(int orgID)
        {
            SqlCommand cmd1 = new SqlCommand("sp_Users_select", Connection);
            cmd1.CommandType = CommandType.StoredProcedure;

            cmd1.Parameters.Add("@orgID", SqlDbType.NVarChar).Value = orgID;

            SqlDataAdapter dap1 = new SqlDataAdapter(cmd1);
            DataSet DS1 = new DataSet();
            dap1.Fill(DS1);

            if (DS1 != null)
            {
                if (DS1.Tables[0].Rows.Count != 0)
                {

                    UserGrid.DataSource = DS1.Tables[0];

                    UserGrid.DataBind();

                }

                else
                {

                }

            }


        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkOldPassword(txtUserID.Text, txtOldPassword.Text))
                {
                    SqlCommand cmd1 = new SqlCommand("[mts_Users_update]", Connection);
                    cmd1.Parameters.Add("@loginID", SqlDbType.NVarChar).Value = txtUserID.Text;
                    cmd1.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = txtDisplayName.Text;
                    cmd1.Parameters.Add("@Email", SqlDbType.NVarChar).Value = txtEmail.Text;
                    cmd1.Parameters.Add("@LoginPwd", SqlDbType.NVarChar).Value = txtpassword.Text;
                    cmd1.Parameters.Add("@Details", SqlDbType.NVarChar).Value = "";
                    cmd1.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = "";
                    cmd1.Parameters.Add("@expiryon", SqlDbType.DateTime).Value = expiryDate.SelectedDate;
                    cmd1.Parameters.Add("@Role", SqlDbType.Int).Value = 9;
                    cmd1.Parameters.Add("@fk_OrgID", SqlDbType.Int).Value = 0;
                    cmd1.Parameters.Add("@UserId", SqlDbType.Int).Value = DBNull.Value;


                    cmd1.CommandType = CommandType.StoredProcedure;
                    Connection.Open();
                    int i = cmd1.ExecuteNonQuery();
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('User updated Successfully!!');</script>");
                    lblPWDLength.Text = "";
                    txtDisplayName.Text = "";
                    txtpassword.Text = "";
                    txtEmail.Text = "";
                    txtconfirmpassword.Text = "";
                    lblUserPermission.Text = "";
                    lblMsg.Text = "";
                    expiryDate.SelectedDate = null;
                    FetchUsers();
                    panel_userRegister.Visible = false;
                    PanelGrid.Visible = true;
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Wrong password!!');</script>");                   
                }

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Error while updating user Information!!');</script>");                   
            }

        }

        protected bool checkOldPassword(string userID,string password)
        {
            SqlCommand cmd = new SqlCommand("spCommonSelectStmt", Connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "Users";
            cmd.Parameters.Add("@Condition", SqlDbType.NVarChar).Value = "loginID='" + userID + "' and LoginPwd='"+password+"'";

            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            DataSet DS = new DataSet();
            dap.Fill(DS);

            if (DS != null)
            {
                if (DS.Tables[0].Rows.Count != 0)
                {
                    return true;
                }
            }
            return false;
        }
        protected string Function_CheckEmail()
        {
            string status = "";
            try
            {
                SqlCommand cmd = new SqlCommand("spCommonSelectStmt", Connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "Users";
                cmd.Parameters.Add("@Condition", SqlDbType.NVarChar).Value = "Email='" + txtEmail.Text + "'";

                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                dap.Fill(DS);

                if (DS != null)
                {
                    if (DS.Tables[0].Rows.Count != 0)
                    {
                        status = "Exist";
                        // lblEmailAdd.Text = "Email address is already registered please enter another email address";
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Email address is already registered please enter another email address');</script>");

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }
         
        protected void btnback_Click(object sender, EventArgs e)
        {
            PanelGrid.Visible = true;
            panel_userRegister.Visible = false;
        }

        protected void UserGrid_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            int userID = 0;
            if (e.CommandName == "EditGrid")
            {
                PanelGrid.Visible = false;               
                userID = Convert.ToInt32(e.CommandArgument);
                Function_LoadUserData(userID);
                Session["userIDtoEdit"] = userID;
            }

            if (e.CommandName == "DeleteGrid")
            {
              
            }
            else
            {
                FetchUsers();
            }
           //FetchUsers();
        }
        
        protected void Function_LoadUserData(int userID)
        {
            try
            {                   
                    
                SqlCommand cmd = new SqlCommand("spCommonSelectStmt", Connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "users";
                cmd.Parameters.Add("@Condition", SqlDbType.NVarChar).Value = "ID=" + userID;

                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
               
                dap.Fill(ds);
               
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        if (ds.Tables[0].Rows[0]["role"].ToString() == "50")
                        {
                            Session["EditUserID"] = userID;
                            Response.Redirect("userEditNormal.aspx");
                        }
                        lblPWDLength.Text = "";
                        txtDisplayName.Text = ds.Tables[0].Rows[0]["UserName"].ToString();
                        txtUserID.Text = ds.Tables[0].Rows[0]["loginID"].ToString();
                        txtpassword.Text = "";
                        txtconfirmpassword.Text = "";
                        txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();                       
                        expiryDate.SelectedDate = DateTime.Parse(ds.Tables[0].Rows[0]["expiresOn"].ToString());
                        lblUserPermission.Text = "";
                        panel_userRegister.Visible = true;
                        lblMsg.Text = "";    
                    }
                }                      

            }
            catch (Exception ex)
            {
            }    
        }

        protected void UserGrid_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            FetchUsers();
        }

        protected void UserGrid_PageSizeChanged(object sender, Telerik.Web.UI.GridPageSizeChangedEventArgs e)
        {
            FetchUsers();
        }

        protected void UserGrid_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
        {
            FetchUsers();
        }
    }
}