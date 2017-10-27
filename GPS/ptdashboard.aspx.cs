using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using GPSTrackingBLL;
using System.IO;
using Ionic.Zip;
using System.Globalization;
using Artem.Google.UI;
using Telerik.Web.UI;




public partial class ptdashboard : System.Web.UI.Page
{
   
    int role, task;
    int arraycount = 0;
    double zoom = 13;
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
            Session["mapType"] = MapType.Roadmap;         
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
            txtVehName.DataSource = ds;
            txtVehName.DataTextField = "CarrierName";
            txtVehName.DataValueField = "CarrierId";
            txtVehName.DataBind();
            int count = ds.Tables[0].Rows.Count;
            int val = Convert.ToInt32(ds.Tables[0].Rows[0]["carrierId"].ToString());
            
            //ds = obj_carrier.fn_CarrierLastLoc_Fetch(Convert.ToInt32(Session["role"].ToString()), Convert.ToInt32(Session["task"].ToString()));
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
    
    protected void initMap()
    {
        ReplayMap.EnableOverviewMapControl = true;
        ReplayMap.OverviewMapControlOptions.Opened = true;
    }
    protected void MapLoad(DataSet ds)
    {
        //try
        //{
       
        ReplayMap.MapType =(MapType)Session["mapType"];
        if (autoRefresh.Checked == false)
        {
            Timer1.Enabled = false;
        }
        else
        {
            Timer1.Enabled = true;
        }

        ReplayMap.Zoom = 13;
        if (arraycount <= 0 || arraycount >= 2)
        {
            ReplayMap.Zoom = 5;
        }

        string lat = "20";
        string lng = "77";
        double latMin = 90.0;
        double longMin = 180.0;
        double latMax = 0.0;
        double longMax = 0.0;

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

                Marker GP1 = new Marker();
                GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));

                Location lc = new Location(Convert.ToDouble(lat), Convert.ToDouble(lng));


                GP1.Info = "Plate No : " + PlateNo + "</br>" + "Speed : " + "" + Speed
                    + "</br>" + "Digital Ignition : " + (D1 == "" ? "NA" : D1) + "</br>" + "Date & Time : " + Date + "</br>" + "Address : " + ((address != "") ? address : "Not Availabe");

                GP1.Title = PlateNo;
                if (ds.Tables[table].Rows[j]["codecId"].ToString() == "503")
                {
                    GP1.Icon = new MarkerImage().Url = "icons/personal1.png";
                    if (ds.Tables[table].Rows[j]["lbsLocation"].ToString() == "1")
                    {
                        GoogleCircle c = new GoogleCircle();
                        c.Center = GP1.Position;
                        c.Radius = 1000;
                        c.FillColor = System.Drawing.Color.LightBlue;
                        c.StrokeWeight = 2;
                        ReplayMap.Overlays.Add(c);
                    }
                }
                else
                {
                    if (D1 == "1" || Int32.Parse(Speed) > 2)
                    {
                        GP1.Icon = new MarkerImage().Url = "icons/personal1.png";
                    }
                    else
                    {
                        GP1.Icon = new MarkerImage().Url = "icons/personal1.png";
                    }
                }
                ReplayMap.Markers.Add(GP1);

                if (Convert.ToDouble(lat) > latMax)
                {
                    latMax = Convert.ToDouble(lat);
                }
                if (Convert.ToDouble(lng) > longMax)
                {
                    longMax = Convert.ToDouble(lng);
                }
                if (Convert.ToDouble(lat) < latMin)
                {
                    latMin = Convert.ToDouble(lat);
                }
                if (Convert.ToDouble(lng) < longMin)
                {
                    longMin = Convert.ToDouble(lng);
                }
            }
        }
        int mapdisplay = 360;
        double dist = (6371 * Math.Acos(Math.Sin(latMin / 57.2958) * Math.Sin(latMax / 57.2958) +
                    (Math.Cos(latMin / 57.2958) * Math.Cos(latMax / 57.2958) * Math.Cos((longMax / 57.2958) - (longMin / 57.2958)))));

        zoom = Math.Floor(8 - Math.Log(1.6446 * dist / Math.Sqrt(2 * (mapdisplay * mapdisplay))) / Math.Log(2));

      
        //if (zoom == 11)
        //{
        //    zoom = 14;
        //}
        if ((ds.Tables[0].Rows.Count + ds.Tables[1].Rows.Count) == 1)
        {
            ReplayMap.Zoom = 16;
        }
        else
        {
            ReplayMap.Zoom = (Int32)zoom;
        }
        if (ReplayMap.Markers.Count == 1 || ((latMin == latMax) && (longMin == longMax)))
        {
            zoom = 11;
        }
        else
        {
            lat = ((latMax + latMin) / 2).ToString();
            lng = ((longMax + longMin) / 2).ToString();
        }
        
        ReplayMap.Center = (new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng)));
        ReplayMap.OverviewMapControlOptions.Opened = true;
        //}
        //catch (Exception)
        //{
        //}
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
   
    
    public DataSet generateArrayIndex()
    {
        int[] arr;
        int[] fleetarr;
        int[] carrierId = new int[200];
        string fleetString = string.Empty;
        arraycount = 0;
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
        if (!liveFollow.Checked)
        {
            MapLoad(ds);
            UpdatePanelReplayMap.Update();
        }
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
        if (!liveFollow.Checked)
        {
            MapLoad(ds);
            UpdatePanelReplayMap.Update();

        }
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        dt.PrimaryKey = new System.Data.DataColumn[] { dt.Columns[0] };
        dt.Merge(ds.Tables[1]);
        RadGrid1.DataSource = dt;
        RadGrid1.Rebind();
        updatePanelDashboardGrid.Update();

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
            if (!liveFollow.Checked)
            {
                ReplayMap.DisableClear = true;
                //int[] carrierId = generateArrayIndex();
                MapLoad(ds);
                UpdatePanelReplayMap.Update();
                

                //cls_Carriers obj_carrier = new cls_Carriers();
                //DataSet ds = new DataSet();
                //ds = obj_carrier.tikerDataFetch(Convert.ToInt32(Session["role"].ToString()), Convert.ToInt32(Session["task"].ToString()), DateTime.Now);

                //for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                //{
                //    TikerContent.Items.Add(new RadListBoxItem(ds.Tables[0].Rows[m]["carrierName"].ToString() + ":" + ds.Tables[0].Rows[m]["feed"].ToString() + " at " + ds.Tables[0].Rows[m]["feedTime"].ToString(), ds.Tables[0].Rows[m]["feedId"].ToString()));
                //}
            }
            else
            {
                liveTrackPlay();
            }
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
                UpdatePanelReplayMap.Update();
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
                UpdatePanelReplayMap.Update();
            }

        }
    }
    void plotTrack(DataSet ds)
    {
        ReplayMap.MapType = (MapType)Session["mapType"];
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
        ReplayMap.Markers.Clear();
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
                        Marker GP1 = new Marker();
                        GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));
                        GP1.Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
                        GP1.Title = newtime.TimeOfDay.ToString(); ;
                        GP1.Icon = new MarkerImage().Url = "icons/personal1blue.png";
                        ReplayMap.Markers.Add(GP1);
                        pl.Path.Add(new LatLng(newlat, newlong));
                    }
                    else if (speed > maxSpeed)
                    {
                        lat = newlat.ToString();
                        lng = newlong.ToString();
                        string PlateNo = txtVehName.Text;
                        string Speed = speed.ToString();
                        string Date = newtime.ToString();
                        Marker GP1 = new Marker();
                        GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));
                        GP1.Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
                        GP1.Title = newtime.TimeOfDay.ToString();
                        GP1.Icon = new MarkerImage().Url = "icons/personal1blue.png";
                        ReplayMap.Markers.Add(GP1);
                        pl.Path.Add(new LatLng(newlat, newlong));
                    }
                    else
                    {
                        lat = newlat.ToString();
                        lng = newlong.ToString();
                        string PlateNo = txtVehName.Text;
                        string Speed = speed.ToString();
                        string Date = newtime.ToString();
                        Marker GP1 = new Marker();
                        GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));

                        GP1.Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
                        GP1.Title = newtime.TimeOfDay.ToString();
                        GP1.Icon = new MarkerImage().Url = "icons/personal1blue.png";
                        ReplayMap.Markers.Add(GP1);
                        pl.Path.Add(new LatLng(newlat, newlong));

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
                            Marker GP1 = new Marker();
                            GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));

                            GP1.Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
                            GP1.Title = newtime.TimeOfDay.ToString(); ;
                            GP1.Icon = new MarkerImage().Url = "icons/personal1blue.png";
                            ReplayMap.Markers.Add(GP1);
                            pl.Path.Add(new LatLng(newlat, newlong));
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
                            Marker GP1 = new Marker();
                            GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));

                            GP1.Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Time from:" + newtime + "  to:" + oldtime;
                            GP1.Title = "Stop from " + newtime.TimeOfDay + " to " + oldtime.TimeOfDay;
                            GP1.Icon = new MarkerImage().Url = "icons/personal1blue.png";
                            ReplayMap.Markers.Add(GP1);
                            pl.Path.Add(new LatLng(newlat, newlong));

                        }
                    }
                    else if (speed > maxSpeed)
                    {
                        lat = newlat.ToString();
                        lng = newlong.ToString();
                        string PlateNo = txtVehName.Text;
                        string Speed = speed.ToString();
                        string Date = newtime.ToString();
                        Marker GP1 = new Marker();
                        GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));

                        GP1.Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
                        GP1.Title = newtime.TimeOfDay.ToString();
                        GP1.Icon = new MarkerImage().Url = "icons/personal1blue.png";
                        ReplayMap.Markers.Add(GP1);
                        pl.Path.Add(new LatLng(newlat, newlong));
                    }
                    else
                    {
                        lat = newlat.ToString();
                        lng = newlong.ToString();
                        string PlateNo = txtVehName.Text;
                        string Speed = speed.ToString();
                        string Date = newtime.ToString();
                        Marker GP1 = new Marker();
                        GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));

                        GP1.Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
                        GP1.Title = newtime.TimeOfDay.ToString();
                        GP1.Icon = new MarkerImage().Url = "icons/personal1blue.png";
                        ReplayMap.Markers.Add(GP1);
                        pl.Path.Add(new LatLng(newlat, newlong));

                    }
                    m++;
                }
            }
            ReplayMap.Polylines.Add(pl);
            ReplayMap.Center = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));
        }

    }
    void plotLBS(DataSet ds)
    {
        ReplayMap.MapType = (MapType)Session["mapType"];
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
       // ReplayMap.Markers.Clear();
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
                Marker GP1 = new Marker();
                GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));
                GP1.Info = "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Date & Time : " + Date;
                GP1.Title = cnt.ToString();
                cnt++;
                GP1.Icon = new MarkerImage().Url = "icons/tower.png";
                ReplayMap.Markers.Add(GP1);
                //pl.Path.Add(new LatLng(newlat, newlong));
               
                m++;
            }
         
            //ReplayMap.Polylines.Add(pl);
            ReplayMap.Center = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));
        }

    }
    /*
        void plotTrack(DataSet ds)
        {
            double speed;
            int maxSpeed = 75;
            Artem.Google.UI.GooglePolyline pl = new Artem.Google.UI.GooglePolyline();
            pl.StrokeColor = System.Drawing.Color.Blue;
            pl.StrokeWeight = 5;
            ReplayMap.Markers.Clear();
            if (ds.Tables[0].Rows.Count <= 0)
            {
            }
            else
            {
                DateTime prevTime = (DateTime)ds.Tables[0].Rows[0]["time"];            
                double newlat = 0;
                double newlong = 0;

                DateTime newtime = DateTime.Now;
                DateTime oldtime = DateTime.Now;
                int cnt = 0;
                int recordCnt = ds.Tables[0].Rows.Count;
                int m = 0;
                while (m < recordCnt)
                {
                    speed = (double)ds.Tables[0].Rows[m]["speed"];
                    newlat = (double)ds.Tables[0].Rows[m]["latitude"];
                    newlong = (double)ds.Tables[0].Rows[m]["longitude"];
                    newtime = (DateTime)ds.Tables[0].Rows[m]["time"];
                    cnt = 0;
                    if (speed <=2)
                    {   
                        while (m+1 < recordCnt)
                        {
                            if (Math.Round(newlat, 4) == Math.Round((double)ds.Tables[0].Rows[m+!]["latitude"], 4)&&Math.Round(newlong, 4) == Math.Round((double)ds.Tables[0].Rows[m]["longitude"],4))
                            {
                                 m++;
                                 cnt++;                            
                            }
                            else
                            {
                                break;
                            }                       
                        }
                        if (cnt < 2)
                        {
                            double lat = (double)ds.Tables[0].Rows[m]["latitude"];
                            double lng = (double)ds.Tables[0].Rows[m]["longitude"];
                            string PlateNo = txtVehName.Text;
                            string Speed = speed.ToString();
                            string Date = ds.Tables[0].Rows[m]["time"].ToString();
                            Marker GP1 = new Marker();
                            GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));
                            ViewState["lat"] = lat;
                            ViewState["lng"] = lng;
                            GP1.Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
                            GP1.Title = newtime.TimeOfDay.ToString(); ;
                            GP1.Icon = new MarkerImage().Url = "icons/car_icon3.png";
                            ReplayMap.Markers.Add(GP1);
                            pl.Path.Add(new LatLng(lat,lng));
                        }
                        else
                        {   
                            oldtime = (DateTime)ds.Tables[0].Rows[m - 1]["time"];                        
                            string PlateNo = txtVehName.Text;
                            string Speed = "0";
                            string Date = newtime.ToString();
                            Marker GP1 = new Marker();
                            GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));
                            ViewState["lat"] = lat;
                            ViewState["lng"] = lng;
                            GP1.Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Time from:" + newtime + "  to:" + oldtime;
                            GP1.Title = "Stop from " + newtime.TimeOfDay + " to " + oldtime.TimeOfDay;
                            GP1.Icon = new MarkerImage().Url = "icons/car_icon3big.png";
                            ReplayMap.Markers.Add(GP1);
                            pl.Path.Add(new LatLng(lat, lng));

                        }
                    }
                    else if (speed > maxSpeed)
                    {
                        lat = newlat;
                        lng = newlong;
                        string PlateNo = txtVehName.Text;
                        string Speed = speed.ToString();
                        string Date = newtime.ToString();
                        Marker GP1 = new Marker();
                        GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));
                        ViewState["lat"] = lat;
                        ViewState["lng"] = lng;
                        GP1.Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
                        GP1.Title = newtime.TimeOfDay.ToString();
                        GP1.Icon = new MarkerImage().Url = "icons/car_icon2.png";
                        ReplayMap.Markers.Add(GP1);
                        pl.Path.Add(new LatLng(lat, lng));
                    }
                    else
                    {                    
                        string PlateNo = txtVehName.Text;
                        string Speed = speed.ToString();
                        string Date = newtime.ToString();
                        Marker GP1 = new Marker();
                        GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));
                        ViewState["lat"] = lat;
                        ViewState["lng"] = lng;
                        GP1.Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + newlat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
                        GP1.Title = newtime.TimeOfDay.ToString();
                        GP1.Icon = new MarkerImage().Url = "icons/car_icon4.png";
                        ReplayMap.Markers.Add(GP1);
                        pl.Path.Add(new LatLng(lat, lng));

                    }
                    m++;
                }
                ReplayMap.Polylines.Add(pl);
                ReplayMap.Center = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));
            }

        }
        */
    void liveTrackPlay()
    {

        if (txtVehName.SelectedItem != null)
        {
            DataSet ds = new System.Data.DataSet();
            cls_Carriers obj_carrier = new cls_Carriers();
            string lat;
            string lng;

            ds = obj_carrier.fn_CarrierMtsLoc_FetchBY_CarrierID(Int32.Parse(txtVehName.SelectedItem.Value), (DateTime)Session["followStartTime"]);

            if (ds.Tables[0].Rows.Count == 0)
            {
                ds = obj_carrier.fn_CarrierLastLoc_FetchBY_CarrierID(Int32.Parse(txtVehName.SelectedItem.Value));
            }
            LatLng ltlng = new LatLng();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                lat = ds.Tables[0].Rows[i]["latitude"].ToString();
                lng = ds.Tables[0].Rows[i]["longitude"].ToString();
                string PlateNo = ds.Tables[0].Rows[i]["carrierName"].ToString();
                string Speed = ds.Tables[0].Rows[i]["speed"].ToString();
                string D1 = ds.Tables[0].Rows[i]["Din1"].ToString();
                string Date = ds.Tables[0].Rows[i]["time"].ToString();
                Marker GP1 = new Marker();
                GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));

                GP1.Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed
                + "</br>" + "Digital Ignition : " + D1 + "</br>" + "Date & Time : " + Date;

                GP1.Title = PlateNo;
                if (D1 == "1" || Int32.Parse(Speed) > 2)
                {
                    GP1.Icon = new MarkerImage().Url = "icons/car_icon4.png";
                }
                else
                {
                    GP1.Icon = new MarkerImage().Url = "icons/car_icon3.png";
                }

                ReplayMap.Markers.Add(GP1);


                if (i != 0)
                {
                    Artem.Google.UI.GooglePolyline pl = new Artem.Google.UI.GooglePolyline();
                    pl.StrokeColor = System.Drawing.Color.Blue;
                    pl.StrokeWeight = 5;
                    pl.Path.Add(new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng)));
                    pl.Path.Add(ltlng);
                    ReplayMap.Polylines.Add(pl);
                }
                ltlng = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));
                ReplayMap.Center = (ltlng);
                ReplayMap.OverviewMapControlOptions.Opened = true;
            }
        }
        else
        {
           // Page.ClientScript.RegisterStartupScript(Page.GetType(), "MyKey", "selectError()", true);
            //DateBoxError.Text = "Please select a Vehicle to follow!";
            liveFollow.Checked = false;
            updatePanelControlPanel.Update();
        }
    }
    protected void autoRefresh_CheckedChanged(object sender, EventArgs e)
    {
        if (autoRefresh.Checked == false)
        {
            Timer1.Enabled = false;
            DataSet ds = generateArrayIndex();
            MapLoad(ds);
        }
        else
        {
            Timer1.Enabled = true;
            DataSet ds = generateArrayIndex();
            MapLoad(ds);
            UpdatePanelReplayMap.Update();
        }
    }
    protected void liveFollow_CheckedChanged(object sender, EventArgs e)
    {
        if (liveFollow.Checked)
        {
            Session["followStartTime"] = DateTime.Now;
            liveTrackPlay();
        }
        else
        {
            DataSet ds = generateArrayIndex();
            MapLoad(ds);
            
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

    protected void ReplayMap_MapTypeChanged(object sender, EventArgs e)
    {
        if (ReplayMap.MapType == MapType.Roadmap)
        {
            Session["mapType"] = MapType.Satellite;
        }
        else
        {
            Session["mapType"] = MapType.Roadmap;
        }
    }

   
    protected void actOnCarrierListevent()
    {
        DataSet ds = generateArrayIndex();
        if (!liveFollow.Checked)
        {
            MapLoad(ds);
            UpdatePanelReplayMap.Update();
        }
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