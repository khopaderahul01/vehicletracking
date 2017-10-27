using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using GPSTrackingBLL;
using System.IO;
using Ionic.Zip;
using System.Globalization;

using Telerik.Web.UI;
using Telerik.Charting;




public partial class DashBoard : System.Web.UI.Page
{      
    
    string path = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["tikerTime"] = DateTime.Now;
        if ((Session["role"] == null))
        {
            Response.Redirect("Default.aspx");
        }
        if (!IsPostBack)
        {
           
        }
    }

    void Page_PreInit(object sender, EventArgs e)
    {
        if (Session["role"] != null)
        {
            int role=int.Parse(Session["role"].ToString());
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
            txtVehName.DataSource = ds;
            txtVehName.DataTextField = "CarrierName";
            txtVehName.DataValueField = "CarrierId";
            txtVehName.DataBind();
            int count = ds.Tables[0].Rows.Count;
            int val = Convert.ToInt32(ds.Tables[0].Rows[0]["carrierId"].ToString());
            
            car_listbox.DataSource = ds.Tables[0];
            car_listbox.DataTextField = "carrierName";
            car_listbox.DataValueField = "carrierId";
            car_listbox.DataBind();
            car_listbox.SelectedIndex = 0;


            RadListBoxFleet.DataSource = ds.Tables[1];
            RadListBoxFleet.DataTextField = "fleetName";
            RadListBoxFleet.DataValueField = "fleetID";
            RadListBoxFleet.DataBind();
            RadListBoxFleet.SelectedIndex = 0;
            RadListBoxFleet.Items.Insert(0, new RadListBoxItem("Select All", "-1"));
            car_listbox.Items.Insert(0, new RadListBoxItem("Select All", "-1"));
            car_listbox.Items[0].BackColor = System.Drawing.Color.LightBlue;
            RadListBoxFleet.Items[0].BackColor = System.Drawing.Color.LightBlue;


        }
        catch (Exception e)
        {
            cls_fileHandling fh = new cls_fileHandling();
            fh.appendToFile("~/logs/error.txt", DateTime.Now + ": " + e.Message);
            fh.appendToFile("~/logs/stackTrace.txt", DateTime.Now + ": " + e.StackTrace);

        }
    }
    protected bool datevalidate()
    {
        try
        {
            string str = dateFrom.SelectedDate.ToString();
            if (dateFrom.SelectedDate.ToString() == "" || dateTo.SelectedDate.ToString() == "")
            {
                //DateBoxError.Text = "Date Fields cannot be kept empty!!";
                return false;
            }
            CultureInfo culture = new CultureInfo("en-US");
            DateTime dt = Convert.ToDateTime(dateFrom.SelectedDate.ToString(), culture);// txtTimeStart.Text;
            DateTime dt1 = Convert.ToDateTime(dateTo.SelectedDate.ToString(), culture);

            if (dt > dt1)
            {
                //DateBoxError.Text = "Not valid date range";
                return false;
            }
            else
            {
                System.TimeSpan span = dt1.Subtract(dt);
                int days = span.Days;
                days = days + 1;
                if (days > 7)
                {
                   
                    //lblmsg.Visible = true;
                    //lblmsg.Text = "Select date range less than 8 days";
                    //lblmsg.Text = "Tracking can not Support for more than 7 days" + "<br/>" + "Contact Support";
                    return false;
                }
                else
                {
                    //DateBoxError.Text = "";
                    return true;
                }
            }
        }
        catch (Exception)
        {
            //DateBoxError.Text = "Wrong dates Selected!!";
            return false;
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
  
   
    protected void MapLoad(DataSet ds,UpdatePanel panel,bool setCenter)
    {
        try
        {
        
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(panel, panel.GetType(), "clearMarkers", "clearMarkers();", true);
      
        
            if (autoRefresh.Checked == false)
            {
                Timer1.Enabled = false;
            }
            else
            {
                Timer1.Enabled = true;
            }

            string lat = "20";
            string lng = "77";
        

            for (int table = 0; table < 2; table++)
            {
                for (int j = 0; j < ds.Tables[table].Rows.Count; j++)
                {
               
                    lat = ds.Tables[table].Rows[j]["latitude"].ToString();
                    lng = ds.Tables[table].Rows[j]["longitude"].ToString();
                    string address = ds.Tables[table].Rows[j]["address"].ToString();
                    string PlateNo = ds.Tables[table].Rows[j]["carrierName"].ToString();
                    //string lastlogdt = ds.Tables[0].Rows[0]["LogReceivedServerTime"].ToString();
                    string Speed = ds.Tables[table].Rows[j]["speed"].ToString();
                    string D1 = ds.Tables[table].Rows[j]["Din1"].ToString();
                    string Date = ds.Tables[table].Rows[j]["time"].ToString();

              
               
                    firstPlot(ds.Tables[table].Rows[j], j, panel);

               
                }
            }


            if (setCenter)
            {
                string parameters = lat + "','" + lng;
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(panel, panel.GetType(), "setCenter", "setCenter('" + parameters + "');", true);

            }
        }
        catch (Exception)
        {
        }
    }
    public static double Calc(double Lat1, double Long1, double Lat2, double Long2)
    {

        double dDistance = Double.MinValue;
        double dLat1InRad = Lat1 * (Math.PI / 180.0);
        double dLong1InRad = Long1 * (Math.PI / 180.0);
        double dLat2InRad = Lat2 * (Math.PI / 180.0);
        double dLong2InRad = Long2 * (Math.PI / 180.0);

        double dLongitude = dLong2InRad - dLong1InRad;
        double dLatitude = dLat2InRad - dLat1InRad;

        // Intermediate result a.
        double a = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                   Math.Cos(dLat1InRad) * Math.Cos(dLat2InRad) *
                   Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

        // Intermediate result c (great circle distance in Radians).
        double c = 2.0 * Math.Asin(Math.Sqrt(a));

        // Distance.
        // const Double kEarthRadiusMiles = 3956.0;
        const Double kEarthRadiusKms = 6376.5;
        dDistance = kEarthRadiusKms * c;

        return dDistance;
    }

    public DataSet getSelectedVehicles()
    {
        int[] arr;
        int[] fleetarr;
        int[] carrierId = new int[200];
        string fleetString = string.Empty;
        DataTable carrierdt = new DataTable();
        DataTable fleetdt = new DataTable();

        DataSet ds = new DataSet();

        carrierdt.Columns.Add("carrierId");
        fleetdt.Columns.Add("carrierId");


        arr = car_listbox.GetCheckedIndices();
        fleetarr = RadListBoxFleet.GetCheckedIndices();

        foreach (int index in fleetarr)
        {
            fleetdt.Rows.Add(RadListBoxFleet.Items[index].Value);
        }
        foreach (int index in arr)
        {
            carrierdt.Rows.Add(car_listbox.Items[index].Value);
        }
        ds.Tables.Add(carrierdt);
        ds.Tables.Add(fleetdt);
        return ds;
    }
    public DataSet generateArrayIndex()
    {
        int[] arr;
        int[] fleetarr;
        int[] carrierId = new int[200];
        string fleetString = string.Empty;        
        DataTable carrierdt = new DataTable();
        DataTable fleetdt = new DataTable();
        DataSet ds = new DataSet();
        carrierdt.Columns.Add("carrierId");
        fleetdt.Columns.Add("carrierId");
        arr = car_listbox.GetCheckedIndices();
        fleetarr = RadListBoxFleet.GetCheckedIndices();
        foreach (int index in fleetarr)
        {
            fleetdt.Rows.Add(RadListBoxFleet.Items[index].Value);
        }
        foreach (int index in arr)
        {
            carrierdt.Rows.Add(car_listbox.Items[index].Value);
        }
        cls_Carriers obj_carrier = new cls_Carriers();
        ds = obj_carrier.fn_fetchLastLocation(carrierdt, fleetdt);
        return ds;
    }
    protected void car_listbox_ItemCheck(object sender, Telerik.Web.UI.RadListBoxItemEventArgs e)
    {
        DataSet ds = generateArrayIndex();        
        MapLoad(ds, UpdatePanelCarListBox,true);
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        dt.PrimaryKey = new System.Data.DataColumn[] { dt.Columns[0] };
        dt.Merge(ds.Tables[1]);
        RadGrid1.DataSource = dt;
        RadGrid1.Rebind();
        updatePanelDashboardGrid.Update();

    }
    protected void ListboxFleet_ItemCheck(object sender, Telerik.Web.UI.RadListBoxItemEventArgs e)
    {
        DataSet ds = generateArrayIndex();
       
        MapLoad(ds, UpdatePanelFleetListBox, true);
        
            

        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        dt.PrimaryKey = new System.Data.DataColumn[] { dt.Columns[0] };
        dt.Merge(ds.Tables[1]);
        RadGrid1.DataSource = dt;
        RadGrid1.Rebind();
        updatePanelDashboardGrid.Update();

    }

    void firstPlot(DataRow dr, int count, UpdatePanel panel)
    {
        string lat, lng;
        lat = dr["latitude"].ToString();
        lng = dr["longitude"].ToString();
        string address = dr["address"].ToString();
        string PlateNo = dr["carrierName"].ToString();
        string Speed = dr["speed"].ToString();
        string D1 = dr["Din1"].ToString();
        string Date = dr["time"].ToString();
        string angle = dr["angle"].ToString();
        String Info = "Plate No : " + PlateNo + "</br>" + "Speed : " + "" + Speed
            + "</br>" + "Digital Ignition : " + (D1 == "" ? "NA" : (D1 == "0" ? "OFF" : "ON")) + "</br>" + "Date & Time : " + Date + "</br>" + "Address : " + ((address != "") ? address : "Not Availabe");
        string desc = "<strong><font color=\"red\">" + PlateNo + "</font></strong><font color=\"blue\">" + "&nbsp;&nbsp;&nbsp;Time : " + "</font>" + Date + "<font color=\"blue\">" + "&nbsp;&nbsp;&nbsp;Speed : " + "</font>" + Speed + "<font color=\"blue\">" + "&nbsp;&nbsp;&nbsp;Ignition : " + "</font>" + (D1 == "" ? "NA" : (D1 == "0" ? "OFF" : "ON")) + "&nbsp;&nbsp;&nbsp;<font color=\"blue\">Address : " + "</font>" + ((address != "") ? address : "Not Availabe");

        string basePath = dr["carrierTypeFId"].ToString();
        string lbsLocation = dr["lbsLocation"].ToString();
        string parameters = Info + "','" + PlateNo + "','" + lat + "','" + lng + "','" + desc + "','" + angle + "','" + basePath + "','" + Speed + "','" + lbsLocation;
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(panel, panel.GetType(), "placeMarker" + count, "placeMarker('" + parameters + "');", true);
        
        //parameters = map + "','" + 17;
        //System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanelCarListBox, UpdatePanelCarListBox.GetType(), "setZoom", "setZoom('" + parameters + "');", true);
      
    }


    protected void Timer1_Tick(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = generateArrayIndex();
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            dt.PrimaryKey = new System.Data.DataColumn[] { dt.Columns[0] };
            dt.Merge(ds.Tables[1]);
            RadGrid1.DataSource = dt;
            RadGrid1.Rebind();
            updatePanelDashboardGrid.Update();
            
                //ReplayMap.DisableClear = true;
                //int[] carrierId = generateArrayIndex();
            MapLoad(ds, updatePanel1, false);
           
                


                //cls_Carriers obj_carrier = new cls_Carriers();
                //DataSet ds = new DataSet();
                //ds = obj_carrier.tikerDataFetch(Convert.ToInt32(Session["role"].ToString()), Convert.ToInt32(Session["task"].ToString()), DateTime.Now);

                //for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                //{
                //    TikerContent.Items.Add(new RadListBoxItem(ds.Tables[0].Rows[m]["carrierName"].ToString() + ":" + ds.Tables[0].Rows[m]["feed"].ToString() + " at " + ds.Tables[0].Rows[m]["feedTime"].ToString(), ds.Tables[0].Rows[m]["feedId"].ToString()));
                //}
          
            
        }
        catch (Exception ex)
        {
            cls_fileHandling fh = new cls_fileHandling();
            fh.appendToFile("~/logs/error.txt", DateTime.Now + ": " + ex.Message);
            fh.appendToFile("~/logs/stackTrace.txt", DateTime.Now + ": " + ex.StackTrace);
        }
    }
  

    protected void oldTrack_Click(object sender, EventArgs e)
    {
        if (datevalidate())
        {
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel9, UpdatePanel9.GetType(), "clearMarkers", "clearMarkers();", true);
            if (chkPlotLBS.Checked)
            {
                
      
                Timer1.Enabled = false;
                autoRefresh.Checked = false;
                updatePanelControlPanel.Update();
                DataSet ds = new System.Data.DataSet();
                cls_Reports obj_report = new cls_Reports();
                obj_report.carrierId = Convert.ToInt32(txtVehName.SelectedItem.Value);
                obj_report.dateStart = dateFrom.SelectedDate.ToString();
                obj_report.dateEnd = dateTo.SelectedDate.ToString();

                // DateTime dt = (DateTime)dateFrom.SelectedDate;
                ds = obj_report.fn_TrackcarrierWithLBS(obj_report, (DateTime)dateFrom.SelectedDate, (DateTime)dateTo.SelectedDate);
                plotTrack(ds);
                plotLBS(ds);
                
                
            }
            else
            {
                Timer1.Enabled = false;
                autoRefresh.Checked = false;
                updatePanelControlPanel.Update();
                DataSet ds = new System.Data.DataSet();
                cls_Reports obj_report = new cls_Reports();
                obj_report.carrierId = Convert.ToInt32(txtVehName.SelectedItem.Value);
                obj_report.dateStart = dateFrom.SelectedDate.ToString();
                obj_report.dateEnd = dateTo.SelectedDate.ToString();
                ds = obj_report.fn_Trackcarrier(obj_report);
                plotTrack(ds);
               
                
            }

        }
    }
    void plotTrack(DataSet ds)
    {
        //ReplayMap.MapType = (MapType)Session["mapType"];
        double speed;
        int maxSpeed = 75;
        try
        {
            maxSpeed = Convert.ToInt32(ds.Tables[1].Rows[0]["maxSpeed"]);
        }
        catch (Exception)
        {
            maxSpeed = 75;
        }
        string lat = "20";
        string lng = "73";
        Artem.Google.UI.GooglePolyline pl = new Artem.Google.UI.GooglePolyline();
        pl.StrokeColor = System.Drawing.Color.Blue;
        pl.StrokeWeight = 5;      
        if (ds.Tables[0].Rows.Count <= 0)
        {
        }
        else
        {
            DateTime prevTime = (DateTime)ds.Tables[0].Rows[0]["time"];
            double oldspeed = 0;
            double newlat = 0;
            double newlong = 0;

            DateTime newtime = DateTime.Now;
            DateTime oldtime = DateTime.Now;
            int cnt = 0;
            int recordCnt = ds.Tables[0].Rows.Count;
            int m = 0;
            if (chkfilter.Checked == false)
            {
                while (m < recordCnt)
                {
                    speed = (double)ds.Tables[0].Rows[m]["speed"];
                    newlat = (double)ds.Tables[0].Rows[m]["latitude"];
                    newlong = (double)ds.Tables[0].Rows[m]["longitude"];
                    newtime = (DateTime)ds.Tables[0].Rows[m]["time"];

                    if (speed < 1)
                    {
                        lat = newlat.ToString();
                        lng = newlong.ToString();
                        string PlateNo = txtVehName.Text;
                        string Speed = speed.ToString();
                        string Date = newtime.ToString();
                     
                        
                        
                     
                        string Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
                        string desc = Info;
                        string angle = ds.Tables[0].Rows[m]["angle"].ToString();
                        string iconPath = "icons/random/car_icon3big.png";
                       
                        string parameters = Info + "','" + PlateNo + "','" + lat + "','" + lng + "','" + desc + "','" + angle + "','" + iconPath + "','" + iconPath;
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel9, UpdatePanel9.GetType(), "stop" + m, "stop('" + parameters + "');", true);
                      

                    }
                    else if (speed > maxSpeed)
                    {
                        lat = newlat.ToString();
                        lng = newlong.ToString();
                        string PlateNo = txtVehName.Text;
                        string Speed = speed.ToString();
                        string Date = newtime.ToString();
                    
                        string Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
                        string desc = Info;
                        string angle = ds.Tables[0].Rows[m]["angle"].ToString();                        
                        string basePath = ds.Tables[1].Rows[0]["carrierTypeFId"].ToString();
                        string lbsLocation = "0";
                        string parameters = Info + "','" + PlateNo + "','" + lat + "','" + lng + "','" + desc + "','" + angle + "','" + basePath + "','" + Speed + "','" + lbsLocation;
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel9, UpdatePanel9.GetType(), "placeMarker" + m, "placeMarker('" + parameters + "');", true);
                      
                        
                    }
                    else
                    {
                        lat = newlat.ToString();
                        lng = newlong.ToString();
                        string PlateNo = txtVehName.Text;
                        string Speed = speed.ToString();
                        string Date = newtime.ToString();
                       
                        string Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
                        string desc = Info;
                        string angle = ds.Tables[0].Rows[m]["angle"].ToString();
                        string basePath = ds.Tables[1].Rows[0]["carrierTypeFId"].ToString();
                        string lbsLocation = "0";
                        string parameters = Info + "','" + PlateNo + "','" + lat + "','" + lng + "','" + desc + "','" + angle + "','" + basePath + "','" + Speed + "','" + lbsLocation;                        
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel9, UpdatePanel9.GetType(), "placeMarker" + m, "placeMarker('" + parameters + "');", true);
                        
                    }
                    m++;
                }
            }
            else
            {
                while (m < recordCnt)
                {
                    speed = (double)ds.Tables[0].Rows[m]["speed"];
                    newlat = (double)ds.Tables[0].Rows[m]["latitude"];
                    newlong = (double)ds.Tables[0].Rows[m]["longitude"];
                    newtime = (DateTime)ds.Tables[0].Rows[m]["time"];
                    cnt = 0;
                    if (speed <= 2)
                    {
                        m++;
                        if (m >= recordCnt)
                            break;
                        oldspeed = (double)ds.Tables[0].Rows[m]["speed"];
                        while (oldspeed <= 2 && m < recordCnt)
                        {
                            oldspeed = (double)ds.Tables[0].Rows[m]["speed"];
                            m++;
                            cnt++;
                        }
                        if (cnt < 2)
                        {
                            lat = newlat.ToString();
                            lng = newlong.ToString();
                            string PlateNo = txtVehName.Text;
                            string Speed = speed.ToString();
                            string Date = newtime.ToString();
                       

                            string Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
                            string desc = Info;
                            string angle = ds.Tables[0].Rows[m]["angle"].ToString();
                            string iconPath = "icons/random/car_icon3big.png";
                            string parameters = Info + "','" + PlateNo + "','" + lat + "','" + lng + "','" + desc + "','" + angle + "','" + iconPath;
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel9, UpdatePanel9.GetType(), "stop" + m, "stop('" + parameters + "');", true);
                      
                        }
                        else
                        {
                            oldspeed = (double)ds.Tables[0].Rows[m - 2]["speed"];
                            oldtime = (DateTime)ds.Tables[0].Rows[m - 2]["time"];
                            lat = newlat.ToString();
                            lng = newlong.ToString();
                            string PlateNo = txtVehName.Text;
                            string Speed = "0";
                            string Date = newtime.ToString();
                        


                            string Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Time from:" + newtime + "  to:" + oldtime;
                            string desc = Info;
                            string angle = ds.Tables[0].Rows[m - 2]["angle"].ToString();
                            string iconPath = "icons/random/car_icon3big.png";
                            string parameters = Info + "','" + PlateNo + "','" + lat + "','" + lng + "','" + desc + "','" + angle + "','" + iconPath;
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel9, UpdatePanel9.GetType(), "stop" + m, "stop('" + parameters + "');", true);
                      
                        }
                    }
                    else if (speed > maxSpeed)
                    {
                        lat = newlat.ToString();
                        lng = newlong.ToString();
                        string PlateNo = txtVehName.Text;
                        string Speed = speed.ToString();
                        string Date = newtime.ToString();
                    
                        string Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
                        string desc = Info;
                        string angle = ds.Tables[0].Rows[m]["angle"].ToString();
                        string basePath = ds.Tables[1].Rows[0]["carrierTypeFId"].ToString();
                        string lbsLocation = "0";
                        string parameters = Info + "','" + PlateNo + "','" + lat + "','" + lng + "','" + desc + "','" + angle + "','" + basePath + "','" + Speed + "','" + lbsLocation;
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel9, UpdatePanel9.GetType(), "placeMarker" + m, "placeMarker('" + parameters + "');", true);
                       
                    }
                    else
                    {
                        lat = newlat.ToString();
                        lng = newlong.ToString();
                        string PlateNo = txtVehName.Text;
                        string Speed = speed.ToString();
                        string Date = newtime.ToString();
                   

                        string Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
                        string desc = Info;
                        string angle = ds.Tables[0].Rows[m]["angle"].ToString();
                        string basePath = ds.Tables[1].Rows[0]["carrierTypeFId"].ToString();
                        string lbsLocation = "0";
                        string parameters = Info + "','" + PlateNo + "','" + lat + "','" + lng + "','" + desc + "','" + angle + "','" + basePath + "','" + Speed + "','" + lbsLocation;
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel9, UpdatePanel9.GetType(), "placeMarker" + m, "placeMarker('" + parameters + "');", true);                        
                    }
                    m++;
                }
            }
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel9, UpdatePanel9.GetType(), "drawPath" + m, "drawPath();", true);
        }

    }
    void plotLBS(DataSet ds)
    {

        double speed;

        string lat = "20";
        string lng = "73";

        if (ds.Tables[2].Rows.Count <= 0)
        {
        }
        else
        {
            DateTime prevTime = (DateTime)ds.Tables[2].Rows[0]["time"];
            double newlat = 0;
            double newlong = 0;

            DateTime newtime = DateTime.Now;
            DateTime oldtime = DateTime.Now;
            int cnt = 1;
            int recordCnt = ds.Tables[2].Rows.Count;
            int m = 0;


            while (m < recordCnt)
            {
                speed = (double)ds.Tables[2].Rows[m]["speed"];
                newlat = (double)ds.Tables[2].Rows[m]["latitude"];
                newlong = (double)ds.Tables[2].Rows[m]["longitude"];
                newtime = (DateTime)ds.Tables[2].Rows[m]["time"];

                lat = newlat.ToString();
                lng = newlong.ToString();
                string Date = newtime.ToString();

                cnt++;
                string Info = "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Date & Time : " + Date;
                string desc = Info;
                string angle = ds.Tables[2].Rows[m]["angle"].ToString();
                string iconPath = "icons/random/tower.png";
                string parameters = Info + "','Cell Tower','" + lat + "','" + lng + "','" + desc + "','" + angle + "','" + iconPath;
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel9, UpdatePanel9.GetType(), "stop" + m, "stop('" + parameters + "');", true);

                m++;
            }
        }

    }
    
   
    protected void autoRefresh_CheckedChanged(object sender, EventArgs e)
    {
        if (autoRefresh.Checked == false)
        {
            Timer1.Enabled = false;
            DataSet ds = generateArrayIndex();
            MapLoad(ds, updatePanelControlPanel, true);
        }
        else
        {
            Timer1.Enabled = true;
            DataSet ds = generateArrayIndex();
            MapLoad(ds, updatePanelControlPanel, true);
            
            
        }
    }
   

    protected void RadGrid1_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        DataSet ds = generateArrayIndex();
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        dt.PrimaryKey = new System.Data.DataColumn[] { dt.Columns[0] };
        dt.Merge(ds.Tables[1]);
        RadGrid1.DataSource = dt;
        RadGrid1.Rebind();
        updatePanelDashboardGrid.Update();
    }

   


    protected void TimerListBox_Tick(object sender, EventArgs e)
    {
        bindControls();
        UpdatePanelCarListBox.Update();
        UpdatePanelFleetListBox.Update();
        TimerListBox.Enabled = false;
    }
   
}