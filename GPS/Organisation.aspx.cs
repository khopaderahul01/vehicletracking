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
    public partial class Organisation : System.Web.UI.Page
    {
        SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["role"] == null))
            {
                Response.Redirect("Default.aspx");
            }
            else if (int.Parse(Session["role"].ToString()) > 10)
            {
                Response.Redirect("Default.aspx");
            }
            if (!IsPostBack)
            {
                lblmsg.Text = "";
                btnAdd.Visible = true;
                Function_LoadOrgGrid();
                showLimits();
            }
        }
        protected void showLimits()
        {
            int role = int.Parse(Session["role"].ToString());
            if (role == 10)
            {
                SqlCommand cmd = new SqlCommand("mts_Company_GetLicenseDetails", Connection);
                cmd.Parameters.Add("@companyID", SqlDbType.Int).Value = int.Parse(Session["fk_CompanyID"].ToString());
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                dap.Fill(DS);
                string carrierlicenseUsed = DS.Tables[0].Rows[0][0].ToString();
                string carrierlicenseLimit = DS.Tables[0].Rows[0][1].ToString();
                string orglicenseUsed = DS.Tables[0].Rows[0][2].ToString();
                string orglicenseLimit = DS.Tables[0].Rows[0][3].ToString();
                lblCarrierLimitTop.Text = string.Format("Device License Used:<b>  <font style=\"color: Red\">{0} / {1}</font></b>    Available: <b> <font style=\"color: Red\">{2}</font></b>", carrierlicenseUsed, carrierlicenseLimit, (int.Parse(carrierlicenseLimit) - int.Parse(carrierlicenseUsed)));
                lblOrgLimitTop.Text = string.Format("Organisation License Used:<b> <font style=\"color: Red\"> {0} / {1}</font></b>     Available: <b> <font style=\"color: Red\">{2}</font></b>", orglicenseUsed, orglicenseLimit, (int.Parse(orglicenseLimit) - int.Parse(orglicenseUsed)));
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

        protected void Function_LoadOrgGrid()
        {
            try
            {
                btnAdd.Visible = true;
                SqlCommand cmd = new SqlCommand("mts_Organisation_select", Connection);

                cmd.Parameters.Add("@role", SqlDbType.NVarChar).Value = int.Parse(Session["role"].ToString());

                if (Session["fk_CompanyID"] != null)
                {
                    cmd.Parameters.Add("@fk_CompanyID", SqlDbType.NVarChar).Value = Session["fk_CompanyID"].ToString();
                }
                else
                {
                    cmd.Parameters.Add("@fk_CompanyID", SqlDbType.NVarChar).Value = 0;
                }

                cmd.CommandType = CommandType.StoredProcedure;


                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                dap.Fill(DS);

                if (DS != null)
                {
                    if (DS.Tables[0].Rows.Count != 0)
                    {
                        OrgGrid.Visible = true;
                        OrgGrid.DataSource = DS;
                        OrgGrid.DataBind();
                        if (int.Parse(Session["role"].ToString()) != 1)
                        {
                            Telerik.Web.UI.GridColumn gd = OrgGrid.Columns.FindByUniqueName("company");
                            gd.Visible = false;
                            gd = OrgGrid.Columns.FindByUniqueName("StatusCol");
                            gd.Visible = false;

                        }
                        else
                        {
                            Telerik.Web.UI.GridColumn gd = OrgGrid.Columns.FindByUniqueName("company");
                            gd.Visible = true;
                            gd = OrgGrid.Columns.FindByUniqueName("StatusCol");
                            gd.Visible = true;
                        }
                    }

                    else
                    {
                        OrgGrid.Visible = false;
                        lblmsg.Text = "No Records Found..";
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void OrgGrid_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            int orgId = 0;
            if (e.CommandName == "editgrid")
            {
                orgId = Convert.ToInt32(e.CommandArgument);
                Session["orgID"] = orgId;
                lblmsg.Text = "";
                btnAdd.Visible = false;
                PanelOrgView.Visible = false;
                PanelOrgAdd.Visible = true;
                Function_LoadOrganisation();
                trUserID.Visible = false;
                trUserName.Visible = false;
                trConfirmPassword.Visible = false;
                trPassword.Visible = false;
                ddlCompName.Enabled = true;
                logo.Visible = true;
                btnInsert.Text = "Update";

                if (Session["role"].ToString() == "1")
                { }
                else
                {
                    trCompany.Visible = false;
                }
            }

            if (e.CommandName == "deletegrid")
            {
                orgId = Convert.ToInt32(e.CommandArgument);
                Function_DeleteOrganisation(orgId, 1);
            }
            else if (e.CommandName == "Filter")
            {
                Function_LoadOrgGrid();
            }
        }

        protected void OrgGrid_PageSize(object sender, Telerik.Web.UI.GridPageSizeChangedEventArgs e)
        {
            PanelOrgAdd.Visible = false;
            PanelOrgView.Visible = true;
            Function_LoadOrgGrid();
        }

        protected void OrgGrid_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            PanelOrgAdd.Visible = false;
            PanelOrgView.Visible = true;
            Function_LoadOrgGrid();
        }

        protected void OrgGrid_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
        {
            Function_LoadOrgGrid();
        }

        protected void Function_DeleteOrganisation(int OrgId, int task)
        {

            SqlCommand cmd = new SqlCommand("mts_Organisation_delete", Connection);
            cmd.Parameters.Add("@orgId", SqlDbType.Int).Value = OrgId;
            cmd.Parameters.Add("@task", SqlDbType.Int).Value = task;
            cmd.CommandType = CommandType.StoredProcedure;
            Connection.Open();
            int i = cmd.ExecuteNonQuery();
            if (task == 1)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Organisation Deleted Successfully');</script>");
            }
            Function_LoadOrgGrid();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            txtCarrierLimit.Text = "99999";
            logo.Visible = false;
            try
            {
                trUserID.Visible = true;
                trUserName.Visible = true;
                trConfirmPassword.Visible = true;
                trPassword.Visible = true;
                Function_ClearControls();
                btnInsert.Text = "Save";

                if (Session["role"].ToString() == "1")
                {
                    PanelOrgView.Visible = false;
                    PanelOrgAdd.Visible = true;
                    bindCommpanyGrid();
                    //ddlCompName.Enabled = true;
                   
                    int licenceAvail = getAvailLicence(int.Parse(ddlCompName.SelectedValue));
                    lblCarrierlicense.Text = "Avalable license: " + licenceAvail;
                    Session["Licence"] = licenceAvail;
                    txtCarrierLimit.Text = "" + licenceAvail;
                  

                }
                else
                {
                    if (allowdToAddOrg(int.Parse(Session["fk_CompanyID"].ToString())))
                    {
                        PanelOrgView.Visible = false;
                        PanelOrgAdd.Visible = true;
                        trUserName.Visible = true;
                        trPassword.Visible = true;
                        trConfirmPassword.Visible = true;
                        int licenceAvail = getAvailLicence(int.Parse(Session["fk_CompanyID"].ToString()));
                        lblCarrierlicense.Text = "Available license: " + licenceAvail;
                        Session["Licence"] = licenceAvail;
                        txtCarrierLimit.Text = "" + licenceAvail;
                        trCompany.Visible = false;
                    }
                    else
                    {
                        string Msg = "Sorry!! Organisation Limit Reached.. To add organisation please contact your company";
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('" + Msg + "');</script>");
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        protected void bindCommpanyGrid()
        {
            SqlCommand cmd = new SqlCommand("[mts_Company_select]", Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            DataSet DS = new DataSet();
            dap.Fill(DS);

            if (DS != null)
            {
                if (DS.Tables[0].Rows.Count != 0)
                {
                    ddlCompName.DataSource = DS;
                    ddlCompName.DataValueField = "companyID";
                    ddlCompName.DataTextField = "companyName";
                    ddlCompName.DataBind();
                }
            }
        }

        protected bool allowdToAddOrg(int companyID)
        {
            SqlCommand cmd = new SqlCommand("mts_checkOrgLimit", Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@fk_CompanyID", SqlDbType.NVarChar).Value = companyID;
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            DataSet DS = new DataSet();
            dap.Fill(DS);

            if (DS != null)
            {
                if (DS.Tables[0].Rows.Count != 0)
                {
                    if (DS.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        return true;

                    }
                    else
                    {
                        return false;
                    }

                }
            }
            return false;

        }

        protected void Function_LoadOrganisation()
        {
            try
            {
                if (Session["orgID"] != null)
                {
                    int orgid = Convert.ToInt32(Session["orgID"]);
                    SqlCommand cmd = new SqlCommand("spCommonSelectStmt", Connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "organisation ";
                    cmd.Parameters.Add("@Condition", SqlDbType.NVarChar).Value = "orgId=" + orgid;

                    SqlDataAdapter dap = new SqlDataAdapter(cmd);
                    DataSet DS = new DataSet();
                    dap.Fill(DS);

                    if (DS != null)
                    {
                        if (DS.Tables[0].Rows.Count != 0)
                        {
                            txtOrgName.Text = DS.Tables[0].Rows[0]["orgName"].ToString();
                            txtAddress.Text = DS.Tables[0].Rows[0]["orgAddr"].ToString();
                            txtContactNo.Text = DS.Tables[0].Rows[0]["orgContact"].ToString();
                            txtEmailId.Text = DS.Tables[0].Rows[0]["orgEmail"].ToString();
                            txtWebsiteAdd.Text = DS.Tables[0].Rows[0]["website"].ToString();
                            ExpiryDate.SelectedDate = Convert.ToDateTime(DS.Tables[0].Rows[0]["expiryOn"].ToString());
                            int CompId = Convert.ToInt32(DS.Tables[0].Rows[0]["companyFId"].ToString());
                            Session["oldLicence"] = txtCarrierLimit.Text = DS.Tables[0].Rows[0]["carrierLimit"].ToString();

                            int licenceAvail = getAvailLicence(CompId);
                            lblCarrierlicense.Text = "Value cant be Reduced. Available license: " + (licenceAvail + int.Parse(txtCarrierLimit.Text));
                            Session["Licence"] = licenceAvail;



                            bindCommpanyGrid();
                            ddlCompName.SelectedValue = CompId.ToString();
                            Session["CompID"] = CompId;




                            logo.Visible = true;

                            logo.ImageUrl = "~/" + DS.Tables[0].Rows[0]["logo"].ToString();
                            logo.Width = 50;
                            logo.Height = 50;
                            ViewState["logo"] = DS.Tables[0].Rows[0]["logo"].ToString();

                            btnInsert.Text = "Update";

                            btnCancel.Text = "Back";

                            trUserName.Visible = false;
                            trPassword.Visible = false;
                            trConfirmPassword.Visible = false;

                        }
                    }
                }
            }

            catch (Exception ex)
            {
            }
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            int companyID = 0;

            if (Session["role"].ToString() == "1")
            {
                try
                {
                    companyID = int.Parse(ddlCompName.SelectedValue);
                }
                catch
                {
                    companyID = int.Parse(ddlCompName.Items[0].Value);
                }
            }
            else
            {
                companyID = int.Parse(Session["fk_CompanyID"].ToString());
            }

            if (btnInsert.Text == "Save")
            {
                if (int.Parse(txtCarrierLimit.Text.ToString()) > int.Parse(Session["Licence"].ToString()))
                {
                    string Msg = "Value for carrier Limit must be Less or equal to " + Session["Licence"].ToString();
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('" + Msg + "');</script>");
                }
                else
                {
                    if (allowdToAddOrg(companyID))
                    {
                        lblmsg.Text = "";
                        Submit(1, companyID);
                    }
                    else
                    {
                        string Msg = "Sorry!! Organisation Limit Reached for this company.";
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('" + Msg + "');</script>");
                    }
                }

            }
            else
            {


                if (int.Parse(txtCarrierLimit.Text.ToString()) >  int.Parse(Session["oldLicence"].ToString())+int.Parse(Session["Licence"].ToString()))
                {
                    string Msg = "You have " + Session["Licence"].ToString() + " license left!! ";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('" + Msg + "');</script>");
                }
                else if (int.Parse(txtCarrierLimit.Text.ToString()) < int.Parse(Session["oldLicence"].ToString()))
                {
                    string Msg = "Value Cannot be Reduced!! ";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('" + Msg + "');</script>");
                }
                else
                {
                    lblmsg.Text = "";
                    Submit(2, companyID);
                }
            }
        }
        protected void Submit(int task, int companyID)
        {
            if (task == 1)
            {
                string userExist = Function_CheckUser();
                if (userExist == "UserExist")
                {
                }
                else if (SaveOrg(task, companyID))
                {
                    if (Function_CreateOrganisationUser(companyID))
                    {
                        Function_ClearControls();
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Organisation Details Saved Successfully');</script>");
                        PanelOrgAdd.Visible = false;
                        PanelOrgView.Visible = false;
                        PanelOrgView.Visible = true;
                        showLimits();
                        Function_LoadOrgGrid();
                    }
                    else
                    {
                        Function_DeleteOrganisation(int.Parse(Session["InsertedOrgID"].ToString()), 2);
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Error Saving Organisation');</script>");
                    }
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Error Saving Organisation');</script>");
                }
            }
            else
            {
                Session["updateOrg"] = Convert.ToInt32(Session["orgID"]);
                if (SaveOrg(task, companyID))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Organisation Details Updated Successfully');</script>");
                    Function_ClearControls();
                    PanelOrgAdd.Visible = false;
                    PanelOrgView.Visible = true;
                    showLimits();
                    Function_LoadOrgGrid();
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Error while updating organisation Details');</script>");
                }
            }
        }

        protected string Function_CheckUser()
        {
            string userExist = "";
            try
            {
                SqlCommand cmd = new SqlCommand("spCommonSelectStmt", Connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "Users";
                cmd.Parameters.Add("@Condition", SqlDbType.NVarChar).Value = "loginID='" + txtUserID.Text + "'";

                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                dap.Fill(DS);

                if (DS != null)
                {
                    if (DS.Tables[0].Rows.Count != 0)
                    {
                        userExist = "UserExist";


                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('UserID  already Exists please select another UserID');</script>");

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return userExist;

        }

        protected bool Function_CreateOrganisationUser(int companyID)
        {
            try
            {
                SqlCommand cmd1 = new SqlCommand("mts_Users_ins", Connection);

                cmd1.Parameters.Add("@loginID", SqlDbType.NVarChar).Value = txtUserID.Text;
                cmd1.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = txtUserName.Text;
                cmd1.Parameters.Add("@Email", SqlDbType.NVarChar).Value = txtEmailId.Text;
                cmd1.Parameters.Add("@LoginPwd", SqlDbType.NVarChar).Value = txtPassword.Text;
                cmd1.Parameters.Add("@Details", SqlDbType.NVarChar).Value = "";
                cmd1.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = Session["loginid"].ToString();
                cmd1.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = DateTime.Now.Date;
                cmd1.Parameters.Add("@expiresOn", SqlDbType.DateTime).Value = ExpiryDate.SelectedDate;

                cmd1.Parameters.Add("@Role", SqlDbType.Int).Value = 20;
                cmd1.Parameters.Add("@fk_OrgID", SqlDbType.Int).Value = Session["InsertedOrgID"];
                cmd1.Parameters.Add("@fk_CompanyID", SqlDbType.Int).Value = companyID;

                cmd1.CommandType = CommandType.StoredProcedure;
                Connection.Open();
                int i = cmd1.ExecuteNonQuery();
                Connection.Close();

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        protected bool SaveOrg(int task, int companyID)
        {
            try
            {
                Image img = new Image();
                if (imgUpload.PostedFile != null)//Checking for valid file
                {
                    string StrImageName = imgUpload.PostedFile.FileName.Substring(imgUpload.PostedFile.FileName.LastIndexOf("\\") + 1);
                    string StrImageType = imgUpload.PostedFile.ContentType;
                    int IntImageSize = imgUpload.PostedFile.ContentLength;


                    if (IntImageSize <= 0)
                    {

                    }
                    else
                    {
                        imgUpload.PostedFile.SaveAs(Server.MapPath("~/UploadedImages/" + StrImageName));
                    }

                    img.ImageUrl = ("UploadedImages/" + StrImageName);
                }

                if (imgUpload.HasFile == false)
                {
                    if (task == 2)
                    {
                        img.ImageUrl = ViewState["logo"].ToString();
                    }
                    else
                    {
                        img.ImageUrl = ("UploadedImages/Default.jpg");
                    }
                }

                SqlCommand cmd = new SqlCommand("mts_Organisation_ins", Connection);
                cmd.Parameters.Add("@orgName", SqlDbType.NVarChar).Value = txtOrgName.Text;
                cmd.Parameters.Add("@orgContact", SqlDbType.BigInt).Value = txtContactNo.Text;
                cmd.Parameters.Add("@orgEmail", SqlDbType.NVarChar).Value = txtEmailId.Text;
                cmd.Parameters.Add("@orgAddr", SqlDbType.NVarChar).Value = txtAddress.Text;

                cmd.Parameters.Add("@createdOn", SqlDbType.Date).Value = DateTime.Now.Date;

                if (ExpiryDate.SelectedDate == null)
                {
                    cmd.Parameters.Add("@expiryOn", SqlDbType.Date).Value = DateTime.Now.Date;
                }
                else
                {
                    cmd.Parameters.Add("@expiryOn", SqlDbType.Date).Value = ExpiryDate.SelectedDate;
                }


                cmd.Parameters.Add("@logo", SqlDbType.NVarChar).Value = img.ImageUrl;

                cmd.Parameters.Add("@website", SqlDbType.NVarChar).Value = txtWebsiteAdd.Text;

                cmd.Parameters.Add("@companyFId", SqlDbType.Int).Value = companyID;

                cmd.Parameters.Add("@CarrierLimit", SqlDbType.Int).Value = Convert.ToInt32(txtCarrierLimit.Text);

                cmd.Parameters.Add("@UID", SqlDbType.Int).Value = DBNull.Value;

                cmd.Parameters.Add("@Task", SqlDbType.Int).Value = task;

                cmd.Parameters.Add("@Status", SqlDbType.Char).Value = "A";
                try
                {
                    int orgID = int.Parse(Session["updateOrg"].ToString());
                    cmd.Parameters.Add("@orId", SqlDbType.Int).Value = orgID;
                }
                catch
                {
                    cmd.Parameters.Add("@orId", SqlDbType.Int).Value = DBNull.Value;
                }



                cmd.CommandType = CommandType.StoredProcedure;
                Connection.Open();
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                dap.Fill(DS);


                Session["InsertedOrgID"] = DS.Tables[0].Rows[0][0].ToString();
                Connection.Close();


                return true;


            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected void Function_ClearControls()
        {
            txtAddress.Text = "";
            txtContactNo.Text = "";
            txtEmailId.Text = "";
            txtOrgName.Text = "";
            txtWebsiteAdd.Text = "";
            ExpiryDate.SelectedDate = null;

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                PanelOrgAdd.Visible = false; //addorg               
                PanelOrgView.Visible = false;  //grid
                PanelOrgView.Visible = true;
                btnAdd.Visible = true;
                // Function_LoadOrgGrid();
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlCompName_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (allowdToAddOrg(int.Parse(ddlCompName.SelectedValue)))
            {
                int licenceAvail = getAvailLicence(int.Parse(ddlCompName.SelectedValue));
                lblCarrierlicense.Text = "Avalable license: " + licenceAvail;
                Session["Licence"] = licenceAvail;
                txtCarrierLimit.Text = "" + licenceAvail;
            }
            else
            {
                string Msg = "Sorry!! Organisation Limit Reached for this company.";
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "showAlert", "showAlert('" + Msg + "');", true);
            }
        }

        protected int getAvailLicence(int companyID)
        {
            SqlCommand cmd = new SqlCommand("[mts_Company_AvailCarrierLimit]", Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@companyID", SqlDbType.Int).Value = companyID;
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            DataSet DS = new DataSet();
            dap.Fill(DS);
            if (DS != null)
            {
                if (DS.Tables[0].Rows.Count != 0)
                {
                    return int.Parse(DS.Tables[0].Rows[0][0].ToString());
                }
            }
            return 0;
        }
    }
}