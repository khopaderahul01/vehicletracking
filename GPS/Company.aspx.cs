using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace GPS
{
    public partial class Company : System.Web.UI.Page
    {
        SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["role"] == null))
            {
                Response.Redirect("Default.aspx");
            }
            else if (int.Parse(Session["role"].ToString()) > 1)
            {
                Response.Redirect("Default.aspx");
            }
            if (!IsPostBack)
            {
                Function_LoadCompGrid();

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
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Function_ClearControls();
            btnSave.Text = "Save";
            txtCarriersLimit.Text = "99999";
            txtOrganisationLimit.Text = "99999";
            PanelGrid.Visible = false;
            PanelCompAdd.Visible = true;
            trUserName.Visible = true;
            trPassword.Visible = true;
            trConfirmPassword.Visible = true;
            trUserName1.Visible = true;
            trPassword1.Visible = true;
            trConfirmPassword1.Visible = true;
            trUserID.Visible = true;
            Image1.Visible = false;
        }

        protected void Function_LoadCompGrid()
        {
            try
            {
                btnAdd.Visible = true;
                SqlCommand cmd = new SqlCommand("sp_Company_select", Connection);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                dap.Fill(DS);

                if (DS != null)
                {
                    if (DS.Tables[0].Rows.Count != 0)
                    {
                        CompGrid.Visible = true;
                        CompGrid.DataSource = DS;
                        CompGrid.DataBind();
                    }

                    else
                    {
                        CompGrid.Visible = false;
                        lblmsg.Text = "No Records Found..";
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void CompGrid_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                int compid = 0;
                if (e.CommandName == "EditGrid")
                {
                    Function_ClearControls();
                    PanelCompAdd.Visible = true;
                    compid = Convert.ToInt32(e.CommandArgument);
                    Session["compid"] = Convert.ToInt32(e.CommandArgument);
                    PanelGrid.Visible = false;
                    trUserID.Visible = false;
                    trUserName.Visible = false;
                    trPassword.Visible = false;
                    trConfirmPassword.Visible = false;
                    trPassword1.Visible = false;
                    trConfirmPassword1.Visible = false;
                    Image1.Visible = true;
                    LoadCompanyDataForEdit(compid);
                    btnSave.Text = "Update";
                }

                if (e.CommandName == "DeleteGrid")
                {
                    compid = Convert.ToInt32(e.CommandArgument);
                    Function_DeleteCompany(compid, 1);
                }

                else if (e.CommandName == "Filter")
                {
                    Function_LoadCompGrid();
                }
            }
            catch (Exception ex)
            {

            }

        }

        protected void CompGrid_PageSize(object sender, Telerik.Web.UI.GridPageSizeChangedEventArgs e)
        {
            PanelCompAdd.Visible = false;
            PanelGrid.Visible = true;
            Function_LoadCompGrid();
        }

        protected void CompGrid_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
        {
            Function_LoadCompGrid();
        }

        protected void CompGrid_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            PanelCompAdd.Visible = false;
            PanelGrid.Visible = true;
            Function_LoadCompGrid();
        }

        protected void Function_DeleteCompany(int compId, int task)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("mts_Company_delete", Connection);
                cmd.Parameters.Add("@CompId", SqlDbType.Int).Value = compId;
                cmd.Parameters.Add("@task", SqlDbType.Int).Value = task;
                cmd.CommandType = CommandType.StoredProcedure;
                Connection.Open();
                int i = cmd.ExecuteNonQuery();
                if (task == 1)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Company Deleted Successfully!!');</script>");
                }
                Function_LoadCompGrid();
                Connection.Close();
            }
            catch (Exception)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Error While Deleting Company!!');</script>");
            }
        }

        protected void LoadCompanyDataForEdit(int compId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spCommonSelectStmt", Connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "Company";
                cmd.Parameters.Add("@Condition", SqlDbType.NVarChar).Value = "companyID=" + compId;

                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                dap.Fill(DS);

                if (DS != null)
                {
                    if (DS.Tables[0].Rows.Count != 0)
                    {
                        btnSave.Text = "Update";
                        btnCancel.Text = "Back";
                        txtCompName.Text = DS.Tables[0].Rows[0]["companyName"].ToString();
                        txtAddress.Text = DS.Tables[0].Rows[0]["companyAddr"].ToString();
                        txtContact.Text = DS.Tables[0].Rows[0]["companyContact"].ToString();
                        txtemail.Text = DS.Tables[0].Rows[0]["companyEmail"].ToString();
                        txtwebsite.Text = DS.Tables[0].Rows[0]["companyWebsite"].ToString();
                        txtOrganisationLimit.Text = DS.Tables[0].Rows[0]["OrganisationLimit"].ToString();
                        txtCarriersLimit.Text = DS.Tables[0].Rows[0]["CarrierLimit"].ToString();
                        expiryDate.SelectedDate = Convert.ToDateTime(DS.Tables[0].Rows[0]["expiryOn"].ToString());
                        Image1.ImageUrl = "~/" + DS.Tables[0].Rows[0]["logo"].ToString();
                        Image1.Width = 50;
                        Image1.Height = 50;

                        ViewState["logo"] = DS.Tables[0].Rows[0]["logo"].ToString();

                    }
                }

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Error while loading company information!!');</script>");
            }
        }
    
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.Text == "Save")
            {
                Submit(1);
            }

            else
            {
                Submit(2);
            }
        }

        protected void Submit(int task)
        {
            if (task == 1)
            {
                string userExist = Function_CheckUser();
                if (userExist == "UserExist")
                {
                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('UserID already exists!!. Please choose diffrent UserID');</script>");
                }
                else if (Company_Mgmt(task))
                {
                    if (Function_CreateCompanyUser())
                    {
                        Function_ClearControls();
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Company Details Saved Successfully');</script>");
                        Function_LoadCompGrid();
                    }
                    else
                    {
                        Function_DeleteCompany(int.Parse(Session["InsertedComapanyID"].ToString()), 2);
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Error Saving Company details');</script>");
                    }
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Error Saving Company details');</script>");
                }
            }
            else if (task == 2)
            {
                if (Company_Mgmt(task))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Company Details Updated Successfully');</script>");
                    Function_LoadCompGrid();
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Error Saving Company');</script>");
                }
            }
        }

        protected bool Company_Mgmt(int task)
        {
            Image img = new Image();

            try
            {
                if (imgUpload.PostedFile != null)//Checking for valid file
                {
                    string StrImageName = imgUpload.PostedFile.FileName.Substring(imgUpload.PostedFile.FileName.LastIndexOf("\\") + 1);
                    string StrImageType = imgUpload.PostedFile.ContentType;
                    int IntImageSize = imgUpload.PostedFile.ContentLength;


                    //Checking for the length of the file.If Length is 0 then the file is not uploaded.
                    if (IntImageSize <= 0)
                    {
                        //Image uploading Failed.
                        // Response.Write("Uploading of Image" + StrImageName + "failed");
                    }
                    else
                    {
                        //Image Uploading Success.

                        imgUpload.PostedFile.SaveAs(Server.MapPath("~/UploadedImages/" + StrImageName));
                    }

                    img.ImageUrl = ("UploadedImages/" + StrImageName);
                }

                if (imgUpload.HasFile == false)
                {
                    if (btnSave.Text == "Update")
                    {
                        img.ImageUrl = ViewState["logo"].ToString();
                    }
                    else
                    {
                        img.ImageUrl = ("UploadedImages/Default.jpg");
                    }
                }

                {
                    SqlCommand cmd = new SqlCommand("mts_Company_ins", Connection);
                    cmd.Parameters.Add("@companyName", SqlDbType.NVarChar).Value = txtCompName.Text;
                    cmd.Parameters.Add("@companyWebsite", SqlDbType.NVarChar).Value = txtwebsite.Text;
                    cmd.Parameters.Add("@companyEmail", SqlDbType.NVarChar).Value = txtemail.Text;
                    cmd.Parameters.Add("@companyContact", SqlDbType.BigInt).Value = txtContact.Text;
                    cmd.Parameters.Add("@companyAddr", SqlDbType.NVarChar).Value = txtAddress.Text;

                    cmd.Parameters.Add("@createdOn", SqlDbType.Date).Value = DateTime.Now.Date;

                    if (expiryDate.SelectedDate == null)
                    {
                        cmd.Parameters.Add("@expiryOn", SqlDbType.Date).Value = DateTime.Now.Date;
                    }
                    else
                    {
                        cmd.Parameters.Add("@expiryOn", SqlDbType.Date).Value = expiryDate.SelectedDate;
                    }

                    cmd.Parameters.Add("@logo", SqlDbType.NVarChar).Value = img.ImageUrl;
                    cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = "Active";

                    cmd.Parameters.Add("@OrgLimit", SqlDbType.Int).Value = txtOrganisationLimit.Text;
                    cmd.Parameters.Add("@CarrierLimit", SqlDbType.Int).Value = txtCarriersLimit.Text;
                    cmd.Parameters.Add("@UsersLimit", SqlDbType.Int).Value = 999999;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = DBNull.Value;

                    cmd.Parameters.Add("@Task", SqlDbType.Int).Value = task;
                    if (Session["compid"] != null)
                    {
                        cmd.Parameters.Add("@CompId", SqlDbType.Int).Value = int.Parse(Session["compid"].ToString());
                    }
                    else
                    {
                        cmd.Parameters.Add("@CompId", SqlDbType.Int).Value = 0;
                    }


                    cmd.CommandType = CommandType.StoredProcedure;
                    Connection.Open();
                    SqlDataAdapter dap = new SqlDataAdapter(cmd);
                    DataSet DS = new DataSet();
                    dap.Fill(DS);


                    Session["InsertedComapanyID"] = DS.Tables[0].Rows[0][0].ToString();
                    Connection.Close();

                    PanelGrid.Visible = true;
                    PanelCompAdd.Visible = false;

                }
                return true;
            }
            catch
            {
                return false;
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
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('UserID already Exists. Please select another UserID');</script>");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return userExist;
        }

        protected bool Function_CreateCompanyUser()
        {
            try
            {
                if (txtPassword.Text.Length < 6)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Password must be atleast 7 Character long..');</script>");
                    return false;
                }
                else
                {
                    SqlCommand cmd1 = new SqlCommand("mts_Users_ins", Connection);

                    cmd1.Parameters.Add("@loginID", SqlDbType.NVarChar).Value = txtUserID.Text;
                    cmd1.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = txtUserName.Text;
                    cmd1.Parameters.Add("@Email", SqlDbType.NVarChar).Value = txtemail.Text;
                    cmd1.Parameters.Add("@LoginPwd", SqlDbType.NVarChar).Value = txtPassword.Text;
                    cmd1.Parameters.Add("@Details", SqlDbType.NVarChar).Value = "";
                    cmd1.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = Session["loginid"].ToString();
                    cmd1.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = DateTime.Now.Date;
                    cmd1.Parameters.Add("@expiresOn", SqlDbType.DateTime).Value = expiryDate.SelectedDate;

                    cmd1.Parameters.Add("@Role", SqlDbType.Int).Value = 10;
                    cmd1.Parameters.Add("@fk_OrgID", SqlDbType.Int).Value = DBNull.Value;
                    cmd1.Parameters.Add("@fk_CompanyID", SqlDbType.Int).Value = Session["InsertedComapanyID"];

                    cmd1.CommandType = CommandType.StoredProcedure;
                    Connection.Open();
                    int i = cmd1.ExecuteNonQuery();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text == "Back")
            {
                PanelCompAdd.Visible = false;
                PanelGrid.Visible = true;
                btnAdd.Visible = true;
                //Function_LoadCompGrid();
            }
        }

        protected void Function_ClearControls()
        {
            txtCompName.Text = "";
            txtwebsite.Text = "";
            txtemail.Text = "";
            txtContact.Text = "";
            txtAddress.Text = "";
            txtOrganisationLimit.Text = "";
            txtCarriersLimit.Text = "";
            txtUserName.Text = "";
            txtUserID.Text = "";
            txtPassword.Text = "";
            txtConfirmPwd.Text = "";
            expiryDate.Clear();
        }
        
    }
}