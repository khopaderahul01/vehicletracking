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
    public partial class Carriers : System.Web.UI.Page
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
                trConfirmPassword.Visible = false;
                trPassword.Visible = false;
                trUserID.Visible = false;
                trUserName.Visible = false;
                trExpiry.Visible = false;
                trEmailID.Visible = false;
                trExistingUser.Visible = false;
                lblmsg.Text = "";
                btnAdd.Visible = true;
                Function_LoadCarGrid();
                showLimits();
            }
        }

        protected void showLimits()
        {
            int role = int.Parse(Session["role"].ToString());
            if (role == 20)
            {
                SqlCommand cmd = new SqlCommand("mts_Organisation_GetLicenseDetails", Connection);
                cmd.Parameters.Add("@orgID", SqlDbType.Int).Value = int.Parse(Session["fk_OrgID"].ToString());
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                dap.Fill(DS);
                string licenseUsed = DS.Tables[0].Rows[0][0].ToString();
                string licenseLimit = DS.Tables[0].Rows[0][1].ToString();
                //lblCarrierLimitTop.Text = string.Format("License Used: ({0}/{1})", licenseUsed, licenseLimit);
                lblCarrierLimitTop.Text = string.Format("License Used:<b> <font style=\"color: Red\">  {0} / {1}</font></b>    Available: <b><font style=\"color: Red\"> {2}</font></b>", licenseUsed, licenseLimit, (int.Parse(licenseLimit) - int.Parse(licenseUsed)));
            }
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
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Function_ClearControls();
                Function_LoadOrganisation(0);
                Function_LoadCarrierType(0);
                Function_LoadTimeZone(0);
                Function_LoadFleet(0);
                TrUserType.Visible = true;
                btnsave.Text = "Save";
                txtImeNo.Enabled = true;
                if (Session["role"].ToString() == "1" || Session["role"].ToString() == "10")
                {
                    PanelGrid.Visible = false;
                    PanelCarAdd.Visible = true;
                }
                else
                {
                    // organisationtr1.Visible = organisationtr2.Visible = false;
                    if (allowdToAddCarrier(int.Parse(Session["fk_OrgID"].ToString())))
                    {
                        PanelGrid.Visible = false;
                        PanelCarAdd.Visible = true;
                    }
                    else
                    {
                        string Msg = "Sorry!! Carrier Limit Reached.. To add Carrier please contact your company";
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(updatePanelmain, updatePanelmain.GetType(), "showAlert", "showAlert('" + Msg + "');", true);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected bool allowdToAddCarrier(int orgID)
        {
            SqlCommand cmd = new SqlCommand("mts_checkCarrierLimit", Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@fk_OrgID", SqlDbType.NVarChar).Value = orgID;
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


        protected void Function_LoadCarGrid()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("mts_Select_Carriers", Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                if (Session["role"].ToString() == "1")
                {
                    cmd.Parameters.Add("@role", SqlDbType.NVarChar).Value = int.Parse(Session["role"].ToString());
                    cmd.Parameters.Add("@fk_CompanyID", SqlDbType.NVarChar).Value = DBNull.Value;
                    cmd.Parameters.Add("@fk_OrgID", SqlDbType.NVarChar).Value = DBNull.Value;
                }
                else if (Session["role"].ToString() == "10")
                {
                    cmd.Parameters.Add("@role", SqlDbType.NVarChar).Value = int.Parse(Session["role"].ToString());
                    cmd.Parameters.Add("@fk_CompanyID", SqlDbType.NVarChar).Value = int.Parse(Session["fk_CompanyID"].ToString());
                    cmd.Parameters.Add("@fk_OrgID", SqlDbType.NVarChar).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@role", SqlDbType.NVarChar).Value = int.Parse(Session["role"].ToString());
                    cmd.Parameters.Add("@fk_CompanyID", SqlDbType.NVarChar).Value = DBNull.Value;
                    cmd.Parameters.Add("@fk_OrgID", SqlDbType.NVarChar).Value = int.Parse(Session["fk_OrgID"].ToString());
                }


                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                dap.Fill(DS);
                if (DS != null)
                {
                    if (DS.Tables[0].Rows.Count != 0)
                    {
                        CarGrid.Visible = true;
                        CarGrid.DataSource = DS.Tables[0];
                        CarGrid.DataBind();


                        if (int.Parse(Session["role"].ToString()) != 1)
                        {                         
                            Telerik.Web.UI.GridColumn gd = CarGrid.Columns.FindByUniqueName("Status");
                            gd.Visible = false;

                        }
                        else
                        {
                            Telerik.Web.UI.GridColumn gd = CarGrid.Columns.FindByUniqueName("StatusCol");
                            gd.Visible = true;
                        }


                    }
                    else
                    {
                        CarGrid.Visible = false;
                        lblmsg.Text = "No Records Found..";
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }



        protected void CarGrid_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            int carId = 0;
            if (e.CommandName == "EditGrid")
            {
                PanelGrid.Visible = false;
                TrUserType.Visible = false;
                carId = Convert.ToInt32(e.CommandArgument);

                lblmsg.Text = "";
                PanelCarAdd.Visible = true;

                Function_LoadCarrierData(carId);
                Session["CarId"] = carId;
            }

            if (e.CommandName == "DeleteGrid")
            {
                carId = Convert.ToInt32(e.CommandArgument);
                Function_DeleteCarrier(carId);
            }
            else if (e.CommandName == "Filter")
            {
                Function_LoadCarGrid();
            }
        }


        protected void CarGrid_pageSize(object sender, Telerik.Web.UI.GridPageSizeChangedEventArgs e)
        {
            Function_LoadCarGrid();
            PanelCarAdd.Visible = false;
            PanelGrid.Visible = true;

        }

        protected void CarGrid_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
        {
            Function_LoadCarGrid();
        }

        protected void CarGrid_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            Function_LoadCarGrid();
            PanelCarAdd.Visible = false;
            PanelGrid.Visible = true;
        }


        protected void Function_DeleteCarrier(int carId)
        {
            SqlCommand cmd = new SqlCommand("mts_Carrier_delete", Connection);
            cmd.Parameters.Add("@CarID", SqlDbType.Int).Value = carId;
            cmd.CommandType = CommandType.StoredProcedure;
            Connection.Open();
            int i = cmd.ExecuteNonQuery();
            Function_LoadCarGrid();
        }

        protected void Function_LoadCarrierData(int carid)
        {
            try
            {
                {

                    SqlCommand cmd = new SqlCommand("spCommonSelectStmt", Connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "carrier";
                    cmd.Parameters.Add("@Condition", SqlDbType.NVarChar).Value = "carrierID=" + carid;
                    SqlDataAdapter dap = new SqlDataAdapter(cmd);
                    DataSet DS = new DataSet();
                    dap.Fill(DS);

                    if (DS != null)
                    {
                        if (DS.Tables[0].Rows.Count != 0)
                        {
                            btnsave.Text = "Update";
                            btnCancel.Text = "Back";


                            txtCarName.Text = DS.Tables[0].Rows[0]["carrierName"].ToString();
                            txtImeNo.Text = DS.Tables[0].Rows[0]["deviceImei"].ToString();
                            txtGsmNo.Text = DS.Tables[0].Rows[0]["gsmNumber"].ToString();
                            txtSimNo.Text = DS.Tables[0].Rows[0]["simServiceType"].ToString();

                            txtAPNNames.Text = DS.Tables[0].Rows[0]["apnName"].ToString();
                            txtAvg.Text = DS.Tables[0].Rows[0]["VehicleAverage"].ToString();

                            txtChasiss.Text = DS.Tables[0].Rows[0]["VehicleChassisNumber"].ToString();

                            int CompId = Convert.ToInt32(DS.Tables[0].Rows[0]["companyFId"].ToString());


                            int OrgId = Convert.ToInt32(DS.Tables[0].Rows[0]["orgFId"].ToString());
                            Function_LoadOrganisation(OrgId);

                            int CarTypeId = Convert.ToInt32(DS.Tables[0].Rows[0]["carrierTypeFId"].ToString());
                            Function_LoadCarrierType(CarTypeId);

                            int TimeZoneId = Convert.ToInt32(DS.Tables[0].Rows[0]["zoneFId"].ToString());
                            Function_LoadTimeZone(TimeZoneId);

                            //int FleetId = Convert.ToInt32(DS.Tables[0].Rows[0]["fleetFid"].ToString());
                            //Function_LoadFleet(FleetId);



                            ddlDin2Logic.SelectedValue = DS.Tables[0].Rows[0]["din2Logic"].ToString();

                            txtFual.Text = DS.Tables[0].Rows[0]["VehicleFuelCapacity"].ToString();
                            ddlDin1Logic.Text = DS.Tables[0].Rows[0]["digIgnitionUsed"].ToString();

                            txtOverSpeed.Text = DS.Tables[0].Rows[0]["overSpeedThreshold"].ToString();
                            txtpass.Text = DS.Tables[0].Rows[0]["apnPassword"].ToString();
                            txtRunningg.Text = DS.Tables[0].Rows[0]["VehicleRunning"].ToString();
                            txtServiceDet.Text = DS.Tables[0].Rows[0]["LastServicingDetails"].ToString();

                            if (DS.Tables[0].Rows[0]["LastServicingDate"].ToString() == "")
                            {
                                LastServiceDate.SelectedDate = DateTime.Now.Date;
                            }
                            else
                            {
                                LastServiceDate.SelectedDate = Convert.ToDateTime(DS.Tables[0].Rows[0]["LastServicingDate"].ToString());
                            }
                            txtImeNo.Enabled = false;

                            txtTyres.Text = DS.Tables[0].Rows[0]["VehicleTyreCount"].ToString();
                            txtapnname.Text = DS.Tables[0].Rows[0]["apnUserName"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void Function_LoadTimeZone(int id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spCommonSelectStmt", Connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "timeZone";
                cmd.Parameters.Add("@Condition", SqlDbType.NVarChar).Value = " ";

                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                dap.Fill(DS);

                if (DS != null)
                {
                    if (DS.Tables[0].Rows.Count != 0)
                    {
                        ddlZone.DataSource = DS;
                        ddlZone.DataValueField = "zoneId";
                        ddlZone.DataTextField = "zone";
                        ddlZone.DataBind();

                        if (id == 0)
                        {
                            ddlZone.SelectedValue = DS.Tables[0].Rows[0]["zone"].ToString();
                        }

                        else
                        {
                            //string name = selectId(id, "timeZone", "zoneId");
                            ddlZone.SelectedValue = id.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void Function_LoadFleet(int id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("mts_Fleet_select", Connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@role", SqlDbType.Int).Value = int.Parse(Session["role"].ToString());
                cmd.Parameters.Add("@compID", SqlDbType.Int).Value = int.Parse(Session["fk_CompanyID"].ToString());
                cmd.Parameters.Add("@orgID", SqlDbType.Int).Value = int.Parse(Session["fk_OrgID"].ToString());



                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                dap.Fill(DS);

                if (DS != null)
                {
                    if (DS.Tables[0].Rows.Count != 0)
                    {
                        ddlfleet.DataSource = DS;
                        ddlfleet.DataValueField = "fleetID";
                        ddlfleet.DataTextField = "fleetName";
                        ddlfleet.DataBind();

                        if (id == 0)
                        {
                            ddlfleet.SelectedValue = DS.Tables[0].Rows[0]["fleetName"].ToString();
                        }
                        else
                        {
                            //string name = selectId(id, "fleet", "fleetID");
                            ddlfleet.SelectedValue = id.ToString();
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }




        protected void Function_LoadOrganisation(int id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spCommonSelectStmt", Connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "organisation";

                if (Session["role"].ToString() == "1")
                {
                    cmd.Parameters.Add("@Condition", SqlDbType.NVarChar).Value = "";
                }
                else if (Session["role"].ToString() == "10")
                {

                    cmd.Parameters.Add("@Condition", SqlDbType.NVarChar).Value = "companyFID=" + Session["fk_CompanyID"].ToString();
                }
                else
                {
                    organisationtr1.Visible = organisationtr2.Visible = false;
                    return;
                }
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                dap.Fill(DS);

                if (DS != null)
                {
                    if (DS.Tables[0].Rows.Count != 0)
                    {

                        ddlOrgNm.DataSource = DS;
                        ddlOrgNm.DataValueField = "orgId";
                        ddlOrgNm.DataTextField = "orgName";
                        ddlOrgNm.DataBind();
                        ddlOrgNm.SelectedValue = id.ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void Function_LoadCarrierType(int id)
        {
            //for loading carrier

            try
            {
                SqlCommand cmd = new SqlCommand("spCommonSelectStmt", Connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "trackType";
                cmd.Parameters.Add("@Condition", SqlDbType.NVarChar).Value = " ";

                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                dap.Fill(DS);

                if (DS != null)
                {
                    if (DS.Tables[0].Rows.Count != 0)
                    {
                        ddlCarrier.DataSource = DS;
                        ddlCarrier.DataValueField = "trackTypeID";
                        ddlCarrier.DataTextField = "trackType";
                        ddlCarrier.DataBind();

                        if (id == 0)
                        {
                            ddlCarrier.SelectedValue = DS.Tables[0].Rows[0]["trackType"].ToString();
                        }
                        else
                        {
                            //string name = selectId(id, "trackType", "trackTypeID");
                            ddlCarrier.SelectedValue = id.ToString();
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnsave_Click1(object sender, EventArgs e)
        {
            if (btnsave.Text == "Save")
            {
                int orgID = 0;
                if (Session["role"].ToString() == "1" || Session["role"].ToString() == "10")
                {
                    orgID = int.Parse(ddlOrgNm.SelectedValue);
                }
                else
                {
                    orgID = int.Parse(Session["fk_OrgID"].ToString());
                }

                if (allowdToAddCarrier(orgID))
                {
                    string statusIMEI = Function_CheckIMEI(); //check IMEI Exist
                    if ((statusIMEI == "Exist"))
                    {
                        string Msg = "IMEI Number is already registered!";
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(updatePanelmain, updatePanelmain.GetType(), "showAlert", "showAlert('" + Msg + "');", true);
                        //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('IMEI Number is already registered');</script>");
                    }
                    else
                    {
                        lblIconError.Text = "";
                        if (NewUser.Checked)
                        {
                            string userExist = Function_CheckUser();
                            if (userExist == "UserExist")
                            {
                            }
                            else if (SaveUser())
                            {
                                int userID = int.Parse(Session["InsertedUserID"].ToString());
                                if (carrier_mgmt(1, userID))
                                {
                                    PanelCarAdd.Visible = false;
                                    PanelGrid.Visible = true;
                                    Function_LoadCarGrid();
                                    string Msg = "Save Successfull!!!";
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(updatePanelmain, updatePanelmain.GetType(), "showAlert", "showAlert('" + Msg + "');", true);
                                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Save Successfull!!!');</script>");
                                    Session["InsertedUserID"] = null;
                                    Function_ClearControls();
                                }
                                else
                                {
                                    string Msg = "Error Saving Carrier Details!";
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(updatePanelmain, updatePanelmain.GetType(), "showAlert", "showAlert('" + Msg + "');", true);
                                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Error Saving Carrier Details!');</script>");
                                    DeleteUser(2);
                                }

                            }
                            else
                            {
                                string Msg = "Error Saving User Details!";
                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(updatePanelmain, updatePanelmain.GetType(), "showAlert", "showAlert('" + Msg + "');", true);
                                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Error Saving User Details!');</script>");                               
                            }
                        }
                        else if (Existing.Checked)
                        {
                            int userID = 0;
                            try
                            {
                                userID = int.Parse(ddlUser.SelectedValue);
                            }
                            catch
                            {
                                userID = 0;
                            }
                            if (carrier_mgmt(1, userID))
                            {
                                PanelCarAdd.Visible = false;
                                PanelGrid.Visible = true;
                                Function_LoadCarGrid();
                                string Msg = "Save Successfull!!!";
                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(updatePanelmain, updatePanelmain.GetType(), "showAlert", "showAlert('" + Msg + "');", true);
                                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Save Successfull!!!');</script>");
                                Session["InsertedUserID"] = null;
                                Function_ClearControls();
                            }
                            else
                            {
                                string Msg = "Error Saving Carrier Details!";
                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(updatePanelmain, updatePanelmain.GetType(), "showAlert", "showAlert('" + Msg + "');", true);
                                // Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Error Saving Carrier Details!');</script>");
                                DeleteUser(2);
                            }
                        }
                        else
                        {
                            if (carrier_mgmt(1, 0))
                            {
                                PanelCarAdd.Visible = false;
                                PanelGrid.Visible = true;
                                Function_LoadCarGrid();
                                string Msg = "Save Successfull!!!";
                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(updatePanelmain, updatePanelmain.GetType(), "showAlert", "showAlert('" + Msg + "');", true);
                                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Save Successfull!!!');</script>");
                                Session["InsertedUserID"] = null;
                                Function_ClearControls();
                            }
                            else
                            {
                                string Msg = "Error Saving Carrier Details!";
                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(updatePanelmain, updatePanelmain.GetType(), "showAlert", "showAlert('" + Msg + "');", true);
                                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Error Saving Carrier Details!');</script>");
                                DeleteUser(2);
                            }
                        }
                    }
                }
                else
                {
                    string Msg = "Sorry!! Carrier Limit Reached.. To add Carrier please contact your company";
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(updatePanelmain, updatePanelmain.GetType(), "showAlert", "showAlert('" + Msg + "');", true);
                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('" + Msg + "');</script>");
                }
            }
            else
            {
                if (carrier_mgmt(2, 0))
                {
                    PanelGrid.Visible = false;
                    PanelCarAdd.Visible = false;
                    PanelGrid.Visible = true;
                    Function_LoadCarGrid();
                }
                else
                {
                    string Msg = "Error While saving Data!";
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(updatePanelmain, updatePanelmain.GetType(), "showAlert", "showAlert('" + Msg + "');", true);
                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Error While saving Data!');</script>");
                }

            }
        }
        protected void DeleteUser(int task)
        {
            int userID = int.Parse(Session["InsertedUserID"].ToString());
            SqlCommand cmd1 = new SqlCommand("mts_User_Delete", Connection);
            cmd1.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
            cmd1.Parameters.Add("@task", SqlDbType.Int).Value = task;
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
            cmd1.ExecuteNonQuery();
            Connection.Close();
        }
        protected bool SaveUser()
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

                cmd1.Parameters.Add("@Role", SqlDbType.Int).Value = 50;
                cmd1.Parameters.Add("@fk_OrgID", SqlDbType.Int).Value = int.Parse(Session["fk_OrgID"].ToString());
                cmd1.Parameters.Add("@fk_CompanyID", SqlDbType.Int).Value = DBNull.Value;

                cmd1.CommandType = CommandType.StoredProcedure;
                Connection.Open();
                SqlDataAdapter dap = new SqlDataAdapter(cmd1);
                DataSet DS = new DataSet();
                dap.Fill(DS);
                Session["InsertedUserID"] = DS.Tables[0].Rows[0][0].ToString();
                Connection.Close();

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
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
                        string Msg = "UserID  already Exists please select another UserID";
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(updatePanelmain, updatePanelmain.GetType(), "showAlert", "showAlert('" + Msg + "');", true);
                        //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('UserID  already Exists please select another UserID');</script>");

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return userExist;

        }
        protected string Function_CheckIMEI()
        {
            string statusIMEI = "";
            try
            {
                Int64 imei = 0;

                SqlCommand cmd = new SqlCommand("spCommonSelectStmt", Connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "carrier";
                cmd.Parameters.Add("@Condition", SqlDbType.NVarChar).Value = " ";

                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                dap.Fill(DS);

                if (DS != null)
                {
                    if (DS.Tables[0].Rows.Count != 0)
                    {
                        for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
                        {
                            imei = Convert.ToInt64(DS.Tables[0].Rows[i]["deviceImei"].ToString());

                            if (imei == Convert.ToInt64(txtImeNo.Text))
                            {
                                statusIMEI = "Exist";
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return statusIMEI;

        }


        protected string selectId(int val, String tablename, String key)
        {
            string name = "";
            try
            {
                SqlCommand cmd = new SqlCommand("spCommonSelectStmt", Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = tablename;
                cmd.Parameters.Add("@Condition", SqlDbType.NVarChar).Value = key + "=" + val;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                dap.Fill(DS);

                if (DS != null)
                {
                    if (DS.Tables[0].Rows.Count != 0)
                    {
                        name = DS.Tables[0].Rows[0][0].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return name;
        }

        protected bool carrier_mgmt(int task, int UserID)
        {
            lblmsg.Text = "";

            try
            {
                int CarID = Convert.ToInt32(Session["CarId"]);

                SqlCommand cmd = new SqlCommand("mts_Carrier_ins", Connection);

                cmd.Parameters.Add("@carrierName", SqlDbType.NVarChar).Value = txtCarName.Text;
                cmd.Parameters.Add("@userID", SqlDbType.NVarChar).Value = UserID;

                int carriertypeid = Convert.ToInt32(ddlCarrier.SelectedItem.Value);

                cmd.Parameters.Add("@carrierTypeFId", SqlDbType.Int).Value = carriertypeid;

                cmd.Parameters.Add("@deviceImei", SqlDbType.BigInt).Value = txtImeNo.Text;

                cmd.Parameters.Add("@gsmNumber", SqlDbType.BigInt).Value = txtGsmNo.Text;
                int orgid = 0;
                if (Session["role"].ToString() == "20")
                {
                    orgid = int.Parse(Session["fk_OrgID"].ToString());
                }
                else
                {
                    orgid = Convert.ToInt32(ddlOrgNm.SelectedItem.Value);
                }
                cmd.Parameters.Add("@orgFId", SqlDbType.BigInt).Value = orgid;

                cmd.Parameters.Add("@simServiceType", SqlDbType.NVarChar).Value = txtSimNo.Text;

                int zoneid = Convert.ToInt32(ddlZone.SelectedItem.Value);

                cmd.Parameters.Add("@zoneFId", SqlDbType.Int).Value = zoneid;

                if (txtChasiss.Text == "")
                {
                    cmd.Parameters.Add("@VehicleChassisNumber", SqlDbType.NVarChar).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@VehicleChassisNumber", SqlDbType.NVarChar).Value = txtChasiss.Text;
                }

                if (txtRunningg.Text == "")
                {
                    cmd.Parameters.Add("@VehicleRunning", SqlDbType.Int).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@VehicleRunning", SqlDbType.Int).Value = txtRunningg.Text;
                }

                if (txtFual.Text == "")
                {
                    cmd.Parameters.Add("@VehicleFuelCapacity", SqlDbType.Int).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@VehicleFuelCapacity", SqlDbType.Int).Value = txtFual.Text;
                }

                if (txtAvg.Text == "")
                {
                    cmd.Parameters.Add("@VehicleAverage", SqlDbType.Int).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@VehicleAverage", SqlDbType.Int).Value = txtAvg.Text;
                }

                if (txtTyres.Text == "")
                {
                    cmd.Parameters.Add("@VehicleTyreCount", SqlDbType.Int).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@VehicleTyreCount", SqlDbType.Int).Value = txtTyres.Text;
                }

                if (LastServiceDate.SelectedDate == null)
                {
                    cmd.Parameters.Add("@LastServicingDate", SqlDbType.DateTime).Value = DateTime.Now.Date;
                }
                else
                {
                    cmd.Parameters.Add("@LastServicingDate", SqlDbType.DateTime).Value = LastServiceDate.SelectedDate;
                }

                if (txtServiceDet.Text == "")
                {
                    cmd.Parameters.Add("@LastServicingDetails", SqlDbType.NVarChar).Value = DBNull.Value;
                }

                else
                {
                    cmd.Parameters.Add("@LastServicingDetails", SqlDbType.NVarChar).Value = txtServiceDet.Text;
                }

                if (txtAPNNames.Text == "")
                {

                    cmd.Parameters.Add("@apnName", SqlDbType.NVarChar).Value = DBNull.Value;
                }

                else
                {
                    cmd.Parameters.Add("@apnName", SqlDbType.NVarChar).Value = txtAPNNames.Text;
                }

                if (txtapnname.Text == "")
                {
                    cmd.Parameters.Add("@apnUserName", SqlDbType.NVarChar).Value = DBNull.Value;
                }

                else
                {
                    cmd.Parameters.Add("@apnUserName", SqlDbType.NVarChar).Value = txtapnname.Text;
                }

                if (txtpass.Text == "")
                {
                    cmd.Parameters.Add("@apnPassword", SqlDbType.NVarChar).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@apnPassword", SqlDbType.NVarChar).Value = txtpass.Text;
                }


                if (txtOverSpeed.Text == "")
                {
                    cmd.Parameters.Add("@overSpeedThreshold", SqlDbType.Float).Value = DBNull.Value;
                }

                else
                {
                    cmd.Parameters.Add("@overSpeedThreshold", SqlDbType.Float).Value = txtOverSpeed.Text;
                }


                cmd.Parameters.Add("@din2Logic", SqlDbType.TinyInt).Value = ddlDin2Logic.SelectedValue;

                try
                {
                    int fleetid = Convert.ToInt32(ddlfleet.SelectedItem.Value);
                    cmd.Parameters.Add("@fleetFid", SqlDbType.Int).Value = DBNull.Value;
                }
                catch
                {
                    cmd.Parameters.Add("@fleetFid", SqlDbType.Int).Value = DBNull.Value;
                }


                cmd.Parameters.Add("@digIgnitionUsed", SqlDbType.TinyInt).Value = ddlDin1Logic.SelectedValue;

                cmd.Parameters.Add("@Status", SqlDbType.Char).Value = 'A';
                cmd.Parameters.Add("@Task", SqlDbType.Int).Value = task;


                if (Session["CarId"] != null)
                {
                    cmd.Parameters.Add("@CarID", SqlDbType.Int).Value = CarID;
                }

                else
                {
                    cmd.Parameters.Add("@CarID", SqlDbType.Int).Value = DBNull.Value;
                }


                cmd.CommandType = CommandType.StoredProcedure;
                Connection.Open();
                int i = cmd.ExecuteNonQuery();
                if (task == 1)
                {

                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Save Successfull!!!');</script>");
                    showLimits();
                    Connection.Close();
                    Function_ClearControls();
                    return true;
                }
                else
                    if (task == 2)
                    {
                        //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Update Successfull!!!');</script>");
                        showLimits();
                        Connection.Close();
                        Function_ClearControls();
                        return true;
                    }


                Connection.Close();
                Function_ClearControls();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Error While saving Data!');</script>");
            }
        }


        //protected string Function_CheckLimit()
        //{
        //    string status = "";
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("sp_getTotalCount", Connection);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = ddlOrgNm.SelectedValue;

        //        SqlDataAdapter dap = new SqlDataAdapter(cmd);
        //        DataSet DS = new DataSet();
        //        dap.Fill(DS);

        //        if (DS.Tables[1] != null)
        //        {
        //            if (DS.Tables[1].Rows.Count != 0)
        //            {
        //                int TotCarrier = Convert.ToInt32(DS.Tables[1].Rows[0]["Count"].ToString());
        //                ViewState["TotalCarrier"] = TotCarrier;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return status;
        //}



        protected void Function_ClearControls()
        {
            txtAPNNames.Text = "";
            txtAvg.Text = "";
            txtChasiss.Text = "";
            txtFual.Text = "";
            txtGsmNo.Text = "";
            txtCarName.Text = "";
            txtpass.Text = "";
            txtRunningg.Text = "";
            txtServiceDet.Text = "";
            txtSimNo.Text = "";
            txtOverSpeed.Text = "";
            txtTyres.Text = "";
            txtapnname.Text = "";
            txtImeNo.Text = "";
            ddlZone.Text = "";
            ddlOrgNm.SelectedIndex = 0;
            ddlCarrier.Text = "";
            LastServiceDate.SelectedDate = null;
            txtUserID.Text = "";
            txtUserName.Text = "";
            txtpass.Text = "";
            txtConfirmPwd.Text = "";
            txtEmailId.Text = "";
            txtImeNo.Text = "";
            txtCarName.Text = "";
            txtGsmNo.Text = "";
            txtapnname.Text = "";
            txtOverSpeed.Text = "";
            Existing.Checked = false;
            NewUser.Checked = false;

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text == "Back")
            {
                PanelGrid.Visible = false;
                PanelCarAdd.Visible = false;
                PanelGrid.Visible = true;
                Function_LoadCarGrid();
            }
        }

        protected void ddlOrgNm_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (allowdToAddCarrier(int.Parse(e.Value.ToString())))
            {
            }
            else
            {
                string Msg = "Sorry!! Carrier Limit Reached for this organisation.. To add Carrier please contact your company";
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "showAlert", "showAlert('" + Msg + "');", true);

            }
        }
        protected void LoadDdlUsers()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spCommonSelectStmt", Connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "users";

                if (Session["role"].ToString() == "1")
                {
                    cmd.Parameters.Add("@Condition", SqlDbType.NVarChar).Value = "role=50";
                }
                else if (Session["role"].ToString() == "10")
                {

                    cmd.Parameters.Add("@Condition", SqlDbType.NVarChar).Value = "fk_CompanyID=" + Session["fk_CompanyID"].ToString() + "and role=50";
                }
                else
                {
                    cmd.Parameters.Add("@Condition", SqlDbType.NVarChar).Value = "fk_OrgID=" + Session["fk_OrgID"].ToString() + "and role=50";
                }
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                dap.Fill(DS);

                if (DS != null)
                {
                    if (DS.Tables[0].Rows.Count != 0)
                    {

                        ddlUser.DataSource = DS;
                        ddlUser.DataValueField = "ID";
                        ddlUser.DataTextField = "loginID";
                        ddlUser.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void NewUser_CheckedChanged(object sender, EventArgs e)
        {
            trConfirmPassword.Visible = true;
            trPassword.Visible = true;
            trUserID.Visible = true;
            trUserName.Visible = true;
            trExpiry.Visible = true;
            trEmailID.Visible = true;
            trExistingUser.Visible = false;
        }

        protected void Existing_CheckedChanged(object sender, EventArgs e)
        {
            trConfirmPassword.Visible = false;
            trPassword.Visible = false;
            trUserID.Visible = false;
            trUserName.Visible = false;
            trExpiry.Visible = false;
            trEmailID.Visible = false;
            trExistingUser.Visible = true;
            LoadDdlUsers();
        }
    }
}