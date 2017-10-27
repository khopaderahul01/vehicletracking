using System;
using System.Data;
using GPSTrackingBLL;
using System.Data.SqlClient;
using System.Configuration;

namespace GPS
{
    public partial class userEditNormal : System.Web.UI.Page
    {
        SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (int.Parse(Session["role"].ToString()) != 20)
            {
                Response.Redirect("Default.aspx");
            }
            // UpdatePanelCarListBox.Update();
            if (!IsPostBack)
            {
                bindControls();
                ViewState["dataTableCarriers"] = null;
                LoadData();
            }
        }
        protected void LoadData()
        {
            int userID =(int) Session["EditUserID"];
            SqlCommand cmd = new SqlCommand("mts_userNormalSelectForEdit", Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@userID", SqlDbType.Int).Value = "ID=" + userID;
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            dap.Fill(ds);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count != 0)
                {
                   
                    txtUserName.Text = ds.Tables[0].Rows[0]["UserName"].ToString();
                    txtUserID.Text = ds.Tables[0].Rows[0]["loginID"].ToString();
                    txtPassword.Text = "";
                    txtConfirmPwd.Text = "";
                    txtOldPassword.Text = "";
                    txtEmailId.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                    ExpiryDate.SelectedDate = DateTime.Parse(ds.Tables[0].Rows[0]["expiresOn"].ToString());                                
                }
                for (int i = 0; i < ds.Tables[1].Rows.Count;i++ )
                {

                    car_listbox.SelectedValue = ds.Tables[1].Rows[i]["carrierID"].ToString();
                }

            }



        }
        protected void bindControls()
        {
            try
            {
                cls_Carriers obj_carrier = new cls_Carriers();

                DataSet ds = new DataSet();
                ds = obj_carrier.fn_CarrierFromCarriers_Fetch(Convert.ToInt32(Session["role"].ToString()), Convert.ToInt32(Session["fk_CompanyID"].ToString()), Convert.ToInt32(Session["fk_OrgID"].ToString()));

                int count = ds.Tables[0].Rows.Count;
                int val = Convert.ToInt32(ds.Tables[0].Rows[0]["carrierId"].ToString());

                // ds = obj_carrier.fn_CarrierLastLoc_Fetch(Convert.ToInt32(Session["role"].ToString()), Convert.ToInt32(Session["task"].ToString()));
                car_listbox.DataSource = ds.Tables[0];
                car_listbox.DataTextField = "carrierName";
                car_listbox.DataValueField = "carrierId";
                car_listbox.DataBind();
                car_listbox.SelectedIndex = 0;

            }
            catch (Exception e)
            {
                cls_fileHandling fh = new cls_fileHandling();
                fh.appendToFile("~/logs/error.txt", DateTime.Now + ": " + e.Message);
                fh.appendToFile("~/logs/stackTrace.txt", DateTime.Now + ": " + e.StackTrace);
            }
        }
       

        protected void car_listbox_ItemCheck(object sender, Telerik.Web.UI.RadListBoxItemEventArgs e)
        {
            DataTable dt = generateArrayIndex();
            ViewState["dataTableCarriers"] = dt;
        }


        protected void btnInsert_Click(object sender, EventArgs e)
        {

            DataTable dt = (DataTable)ViewState["dataTableCarriers"];
            if (dt != null)
            {
                if (dt.Rows.Count == 0)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Please select atleast one carrier!!');</script>");
                }
                else
                {
                    string userExist = Function_CheckUser();
                    if (userExist == "UserExist")
                    {
                    }
                    else if (SaveUser())
                    {
                        if (saveRefrencesToUsers(dt))
                        {
                            Function_ClearControls();
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('User Details Saved Successfully');</script>");
                        }
                        else
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('Error Saving User Details');</script>");
                            DeleteUser(2);
                        }
                    }
                }
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('please select atleast one device!!');</script>");
            }


        }
        protected void Function_ClearControls()
        {
            txtConfirmPwd.Text = "";
            txtConfirmPwd.Text = "";
            txtUserID.Text = "";
            txtUserName.Text = "";
            txtEmailId.Text = "";
            ExpiryDate.SelectedDate = null;
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

        protected bool saveRefrencesToUsers(DataTable dt)
        {
            try
            {
                int userID = int.Parse(Session["InsertedUserID"].ToString());
                cls_Carriers obj = new cls_Carriers();
                obj.fn_carrierUsers_ins(dt, userID);
                return true;
            }
            catch
            {
                return false;
            }





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
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "btn", "<script type = 'text/javascript'>alert('UserID  already Exists please select another UserID');</script>");

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return userExist;

        }

        public DataTable generateArrayIndex()
        {
            int[] arr;
            DataTable carrierdt = new DataTable();
            carrierdt.Columns.Add("carrierId");
            arr = car_listbox.GetCheckedIndices();



            foreach (int index in arr)
            {
                carrierdt.Rows.Add(car_listbox.Items[index].Value);
            }
            return carrierdt;
        }
    }
}