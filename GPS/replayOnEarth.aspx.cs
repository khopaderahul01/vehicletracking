using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPSTrackingBLL;
using System.Data;
using System.Globalization;
using System.IO;
using Ionic.Zip;

public partial class replayOnEarth : System.Web.UI.Page
{
    string path = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((Session["role"] == null))
        {
            Response.Redirect("Default.aspx");
        }
        if (!IsPostBack)
        {
            btnentertour.Visible = false;
            btnexisttour.Visible = false;
            btnpausetour.Visible = false;
            btnplaytour.Visible = false;
            btnresettour.Visible = false;
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "MyKey", "initialize();", true);
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
    protected void bindControls()
    {
        try
        {
            cls_Carriers obj_carrier = new cls_Carriers();

            DataSet ds = new DataSet();
            ds = obj_carrier.fn_CarrierLastLoc_Fetch(Convert.ToInt32(Session["role"].ToString()), Convert.ToInt32(Session["fk_CompanyID"].ToString()), Convert.ToInt32(Session["fk_OrgID"].ToString()));

            int count = ds.Tables[0].Rows.Count;
            int val = Convert.ToInt32(ds.Tables[0].Rows[0]["carrierId"].ToString());

            //ds = obj_carrier.fn_CarrierLastLoc_Fetch(Convert.ToInt32(Session["role"].ToString()), Convert.ToInt32(Session["task"].ToString()));
            car_listbox.DataSource = ds.Tables[0];
            car_listbox.DataTextField = "carrierName";
            car_listbox.DataValueField = "carrierId";
            car_listbox.DataBind();
            car_listbox.SelectedIndex = 0;

            // count = ds.Tables[0].Rows.Count;
            //MapLoad(generateArrayIndex());
            // UpdatePanelReplayMap.Update();

        }
        catch (Exception e)
        {
            cls_fileHandling fh = new cls_fileHandling();
            fh.appendToFile("~/logs/error.txt", DateTime.Now + ": " + e.Message);
            fh.appendToFile("~/logs/stackTrace.txt", DateTime.Now + ": " + e.StackTrace);

        }
    }
    protected void TimerListBox_Tick(object sender, EventArgs e)
    {
        bindControls();
        UpdatePanelCarListBox.Update();
        TimerListBox.Enabled = false;
    }
   
    public int getSelectedCarrierID()
    {
        int[] arr;
        int carrierId = 0;
        arr = car_listbox.GetSelectedIndices();
        carrierId = Int32.Parse(car_listbox.Items[arr[0]].Value);
        return carrierId;
    }
    protected void btnLinePath_Click(object sender, EventArgs e)
    {
        try
        {
            bool valid = datevalidate();
            if (valid == true)
            {
                createLinePath();
                btnentertour.Visible = true;
                btnexisttour.Visible = true;
                btnpausetour.Visible = true;
                btnplaytour.Visible = true;
                btnresettour.Visible = true;
            }

        }
        catch (Exception ex)
        {
            cls_fileHandling fh = new cls_fileHandling();
            fh.appendToFile("~/logs/error.txt", DateTime.Now + ": " + ex.Message);
            fh.appendToFile("~/logs/stackTrace.txt", DateTime.Now + ": " + ex.StackTrace);
        }
    }

    public void createLinePath()
    {
        try
        {
            DataSet ds = new System.Data.DataSet();
            DataSet ds2 = new System.Data.DataSet();
            cls_Reports obj_report = new cls_Reports();
            obj_report.carrierId = Convert.ToInt32(getSelectedCarrierID());
            obj_report.dateStart = dateFrom.SelectedDate.ToString();
            obj_report.dateEnd = dateTo.SelectedDate.ToString();
            ds = obj_report.fn_Trackcarrier(obj_report);
            ds2 = obj_report.fn_CarrierName(obj_report);
            string carrierName = ds2.Tables[0].Rows[0]["carrierName"].ToString();

            DateTime date = DateTime.Now;
            path = "~/KML/" + obj_report.carrierId + " " + date.Year + " " + date.Month + " " + date.Day + " " + date.Hour + " " + date.Minute + " " + date.Second + " " + date.Millisecond + ".kml";
            createRequiredPath("~/KML/");
            path = ViewState["path"].ToString() + obj_report.carrierId + " " + date.Year + " " + date.Month + " " + date.Day + " " + date.Hour + " " + date.Minute + " " + date.Second + " " + date.Millisecond + ".kml";

            Createkml createkml = new Createkml();
            createkml.createKmlpath(ds, path, obj_report.carrierId, carrierName);
            createKmz(path);
            Uri url = Request.Url;
            string temp = url.ToString();
            string str = Request.AppRelativeCurrentExecutionFilePath;
            str = str.Substring(1);
            path = temp.Substring(0, temp.IndexOf(str));
            path = path + "/KML/" + obj_report.carrierId + " " + date.Year + " " + date.Month + " " + date.Day + " " + date.Hour + " " + date.Minute + " " + date.Second + " " + date.Millisecond + ".kmz";
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "MyKey", "init('" + path + "');", true);
        }
        catch (Exception e)
        {
            cls_fileHandling fh = new cls_fileHandling();
            fh.appendToFile("~/logs/error.txt", DateTime.Now + ": " + e.Message);
            fh.appendToFile("~/logs/stackTrace.txt", DateTime.Now + ": " + e.StackTrace);
        }
    }
    protected void createRequiredPath(string path)
    {
        path = Server.MapPath("~/KML/").ToString();

        ViewState["path"] = path;
        if (!Directory.Exists(path.ToString()))
        {
            // Create the directory.
            path = path.Replace(" ", " ");
            Directory.CreateDirectory(path);
        }
    }
    protected bool datevalidate()
    {
        try
        {
            string str = dateFrom.SelectedDate.ToString();
            if (dateFrom.SelectedDate.ToString() == "" || dateTo.SelectedDate.ToString() == "")
            {
                DateBoxError.Text = "Date Fields cannot be kept empty!!";
                return false;
            }
            CultureInfo culture = new CultureInfo("en-US");
            DateTime dt = Convert.ToDateTime(dateFrom.SelectedDate.ToString(), culture);// txtTimeStart.Text;
            DateTime dt1 = Convert.ToDateTime(dateTo.SelectedDate.ToString(), culture);

            if (dt > dt1)
            {
                DateBoxError.Text = "Not valid date range";
                return false;
            }
            else 
            {
                return true;
            }
        }
        catch (Exception)
        {
            DateBoxError.Text = "Wrong dates Selected!!";
            return false;
        }
    }
    public void createKmz(string path)
    {
        ZipFile kmz = new ZipFile();
        kmz.AddFile(path);
        path = path.Replace("kml", "kmz");
        kmz.Save(path);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            bool valid = datevalidate();
            if (valid == true)
            {
                DataSet ds = new System.Data.DataSet();
                DataSet ds2 = new System.Data.DataSet();
                cls_Reports obj_report = new cls_Reports();
                obj_report.carrierId = Convert.ToInt32(getSelectedCarrierID());
                obj_report.dateStart = dateFrom.SelectedDate.ToString();
                obj_report.dateEnd = dateTo.SelectedDate.ToString();
                ds = obj_report.fn_Trackcarrier(obj_report);
                ds2 = obj_report.fn_CarrierName(obj_report);
                string carrierName = ds2.Tables[0].Rows[0]["carrierName"].ToString();

                DateTime date = DateTime.Now;
                path = "~/KML/" + obj_report.carrierId + " " + date.Year + " " + date.Month + " " + date.Day + " " + date.Hour + " " + date.Minute + " " + date.Second + " " + date.Millisecond + ".kml";
                createRequiredPath("~/KML/");
                path = ViewState["path"].ToString() + obj_report.carrierId + " " + date.Year + " " + date.Month + " " + date.Day + " " + date.Hour + " " + date.Minute + " " + date.Second + " " + date.Millisecond + ".kml";

                Createkml createkml = new Createkml();
                createkml.createKml(ds, path, obj_report.carrierId, carrierName);
                createKmz(path);
                Uri url = Request.Url;
                string temp = url.ToString();
                string str = Request.AppRelativeCurrentExecutionFilePath;
                str = str.Substring(1);
                path = temp.Substring(0, temp.IndexOf(str));
                path = path + "/KML/" + obj_report.carrierId + " " + date.Year + " " + date.Month + " " + date.Day + " " + date.Hour + " " + date.Minute + " " + date.Second + " " + date.Millisecond + ".kmz";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "MyKey", "init('" + path + "');", true);
                btnentertour.Visible = true;
                btnexisttour.Visible = true;
                btnpausetour.Visible = true;
                btnplaytour.Visible = true;
                btnresettour.Visible = true;
            }

        }
        catch (Exception ex)
        {
            cls_fileHandling fh = new cls_fileHandling();
            fh.appendToFile("~/logs/error.txt", DateTime.Now + ": " + ex.Message);
            fh.appendToFile("~/logs/stackTrace.txt", DateTime.Now + ": " + ex.StackTrace);
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        try
        {
            bool valid = datevalidate();
            if (valid == true)
            {
                create();
                btnentertour.Visible = true;
                btnexisttour.Visible = true;
                btnpausetour.Visible = true;
                btnplaytour.Visible = true;
                btnresettour.Visible = true;
            }
        }
        catch (Exception)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "MyKey", "UnknownError();", true);
        }
    }
    public void create()
    {
        try
        {
            DataSet ds = new System.Data.DataSet();
            DataSet ds2 = new System.Data.DataSet();
            cls_Reports obj_report = new cls_Reports();
            obj_report.carrierId = Convert.ToInt32(getSelectedCarrierID());
            obj_report.dateStart = dateFrom.SelectedDate.ToString();
            obj_report.dateEnd = dateTo.SelectedDate.ToString();
            ds = obj_report.fn_Trackcarrier(obj_report);
            ds2 = obj_report.fn_CarrierName(obj_report);
            string carrierName = ds2.Tables[0].Rows[0]["carrierName"].ToString();
            DateTime date = DateTime.Now;
            path = "~/KML/" + obj_report.carrierId + " " + date.Year + " " + date.Month + " " + date.Day + " " + date.Hour + " " + date.Minute + " " + date.Second + " " + date.Millisecond + ".kml";
            createRequiredPath("~/KML/");
            path = ViewState["path"].ToString() + obj_report.carrierId + " " + date.Year + " " + date.Month + " " + date.Day + " " + date.Hour + " " + date.Minute + " " + date.Second + " " + date.Millisecond + ".kml";
            Createkml createkml = new Createkml();
            createkml.createKmlAdv(ds, path, obj_report.carrierId, carrierName);
            createKmz(path);
            Uri url = Request.Url;
            string temp = url.ToString();
            string str = Request.AppRelativeCurrentExecutionFilePath;
            str = str.Substring(1);
            path = temp.Substring(0, temp.IndexOf(str));
            path = path + "/KML/" + obj_report.carrierId + " " + date.Year + " " + date.Month + " " + date.Day + " " + date.Hour + " " + date.Minute + " " + date.Second + " " + date.Millisecond + ".kmz";
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "MyKey", "init('" + path + "');", true);
        }
        catch (Exception e)
        {
            cls_fileHandling fh = new cls_fileHandling();
            fh.appendToFile("~/logs/error.txt", DateTime.Now + ": " + e.Message);
            fh.appendToFile("~/logs/stackTrace.txt", DateTime.Now + ": " + e.StackTrace);
        }

    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        bool valid = datevalidate();
        if (valid == true)
        {
            if (path == "")
            {
                create();
            }
            Response.Redirect(path);
        }
        else
        {

        }
    }
   
   
}