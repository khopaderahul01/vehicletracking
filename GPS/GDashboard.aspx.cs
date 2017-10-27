using System;
using System.Data;
using GPSTrackingBLL;
using System.Globalization;
using Artem.Google.UI;
using dotnetCHARTING;
using System.Drawing;

public partial class GDashboard : System.Web.UI.Page
{
   
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
            bindControls();
            initMap();
            BindCarrierData();

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
            return true;
            //else
            //{
            //    System.TimeSpan span = dt1.Subtract(dt);
            //    int days = span.Days;
            //    days = days + 1;
            //    //if (days > 7)
            //    //{
            //    //    if (dd_report.SelectedValue == "geoFencing" || dd_report.SelectedValue == "kmTravld")
            //    //    {
            //    //        DateBoxError.Text = "";
            //    //        return true;
            //    //    }
            //    //    lblmsg.Visible = true;
            //    //    lblmsg.Text = "Select date range less than 8 days";
            //    //    lblmsg.Text = "Tracking can not Support for more than 7 days" + "<br/>" + "Contact Support";
            //    //    return false;
            //    //}
            //    //else
            //    //{
            //    //    DateBoxError.Text = "";
            //    //    return true;
            //    //}
            //}
        }
        catch (Exception)
        {
            DateBoxError.Text = "Wrong dates Selected!!";
            return false;
        }
    }
    
   
    protected void initMap()
    {
        ReplayMap.EnableOverviewMapControl = true;
        ReplayMap.OverviewMapControlOptions.Opened = true;
    }
    protected void MapLaod(DataSet ds)
    {
        //try
        //{
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
                        GP1.Icon = new MarkerImage().Url = "icons/car_icon4.png";
                    }
                    else
                    {
                        GP1.Icon = new MarkerImage().Url = "icons/car_icon3.png";
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
        int mapdisplay = 390;
        double dist = (6371 * Math.Acos(Math.Sin(latMin / 57.2958) * Math.Sin(latMax / 57.2958) +
                    (Math.Cos(latMin / 57.2958) * Math.Cos(latMax / 57.2958) * Math.Cos((longMax / 57.2958) - (longMin / 57.2958)))));

        zoom = Math.Floor(8 - Math.Log(1.6446 * dist / Math.Sqrt(2 * (mapdisplay * mapdisplay))) / Math.Log(2));

        if (arraycount == 1 || ((latMin == latMax) && (longMin == longMax)))
        {
            zoom = 11;
        }
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
        lat = ((latMax + latMin) / 2).ToString();
        lng = ((longMax + longMin) / 2).ToString();
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
    protected void BindCarrierData()
    {
        try
        {
            cls_Carriers obj_carrier = new cls_Carriers();
            DataSet ds = new DataSet();
            ds = obj_carrier.fn_CarrierLastLoc_Fetch(Convert.ToInt32(Session["role"].ToString()), Convert.ToInt32(Session["fk_CompanyID"].ToString()), Convert.ToInt32(Session["fk_OrgID"].ToString()));
            car_listbox.DataSource = ds;
            car_listbox.DataTextField = "carrierName";
            car_listbox.DataValueField = "carrierId";
            car_listbox.DataBind();
            car_listbox.SelectedIndex = 0;


            cls_Carriers obj_fleet = new cls_Carriers();
            DataSet dsFleet = new DataSet();
            dsFleet = obj_fleet.fn_FleetLastLoc_Fetch(Convert.ToInt32(Session["role"].ToString()), Convert.ToInt32(Session["task"].ToString()));
            RadListBoxFleet.DataSource = dsFleet;
            RadListBoxFleet.DataTextField = "fleetName";
            RadListBoxFleet.DataValueField = "fleetID";
            RadListBoxFleet.DataBind();
            RadListBoxFleet.SelectedIndex = 0;


            int count = ds.Tables[0].Rows.Count;
            MapLaod(generateArrayIndex());
        }
        catch (Exception e)
        {
            cls_fileHandling fh = new cls_fileHandling();
            fh.appendToFile("~/logs/error.txt", DateTime.Now + ": " + e.Message);
            fh.appendToFile("~/logs/stackTrace.txt", DateTime.Now + ": " + e.StackTrace);
        }
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

        //foreach(int index in fleetarr)             
        //{
        //    fleetString=fleetString+RadListBoxFleet.Items[index].Value+",";
        //}
        //if (fleetString.Length > 0)
        //{
        //    fleetString = fleetString.Substring(0, fleetString.Length - 1);
        //}
        cls_Carriers obj_carrier = new cls_Carriers();
        ds = obj_carrier.fn_fetchLastLocation(carrierdt, fleetdt);



        //DataSet ds = new DataSet();
        //ds = obj_carrier.Prc_CarrierLastLoc_FetchByString(fleetString);

        //foreach (int index in arr)
        //{
        //    string carrier_id = car_listbox.Items[index].Value;
        //    carrierId[arraycount] = Int32.Parse(car_listbox.Items[index].Value);
        //    arraycount++;
        //}
        //for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
        //{
        //    carrierId[arraycount] = (int)ds.Tables[0].Rows[k]["carrierFID"];
        //    arraycount++;
        //}        
        //string str = generateString(carrierId);
        //Session["carrierString"] = str;
        //Session["carrierIdArray"] = carrierId; 
        //return carrierId;
        return ds;
    }
    public DataSet getSelectedVehicles()
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
        ds.Tables.Add(carrierdt);
        ds.Tables.Add(fleetdt);
        return ds;
    }
    protected void car_listbox_ItemCheck(object sender, Telerik.Web.UI.RadListBoxItemEventArgs e)
    {
        DataSet ds = generateArrayIndex();
        if (!liveFollow.Checked)
        {
            MapLaod(ds);
            if(ds.Tables[0].Rows.Count>0)
                generateDashboard(ds.Tables[0].Rows[0]);
        }
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        dt.PrimaryKey = new System.Data.DataColumn[] { dt.Columns[0] };
        dt.Merge(ds.Tables[1]);
        //RadGrid1.DataSource = dt;
        //RadGrid1.Rebind();
    }
    protected void ListboxFleet_ItemCheck(object sender, Telerik.Web.UI.RadListBoxItemEventArgs e)
    {
        DataSet ds = generateArrayIndex();
        if (!liveFollow.Checked)
        {
            MapLaod(ds);
        }
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        dt.PrimaryKey = new System.Data.DataColumn[] { dt.Columns[0] };
        dt.Merge(ds.Tables[1]);
        //RadGrid1.DataSource = dt;
        //RadGrid1.Rebind();
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
            //RadGrid1.DataSource = dt;
            //RadGrid1.Rebind();
            if (!liveFollow.Checked)
            {
                ReplayMap.DisableClear = true;
                //int[] carrierId = generateArrayIndex();
                MapLaod(ds);
                if (ds.Tables[0].Rows.Count > 0)
                     generateDashboard(ds.Tables[0].Rows[0]);

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
            Timer1.Enabled = false;
            autoRefresh.Checked = false;
            DataSet ds = new System.Data.DataSet();
            cls_Reports obj_report = new cls_Reports();
            obj_report.carrierId = Convert.ToInt32(txtVehName.SelectedItem.Value);
            obj_report.dateStart = dateFrom.SelectedDate.ToString();
            obj_report.dateEnd = dateTo.SelectedDate.ToString();
            ds = obj_report.fn_Trackcarrier(obj_report);
            plotTrack(ds);
        }
    }
    void plotTrack(DataSet ds)
    {
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
                        GP1.Icon = new MarkerImage().Url = "icons/car_icon3.png";
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
                        GP1.Icon = new MarkerImage().Url = "icons/car_icon2.png";
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
                        GP1.Icon = new MarkerImage().Url = "icons/car_icon4.png";
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
                            GP1.Icon = new MarkerImage().Url = "icons/car_icon3.png";
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
                            GP1.Icon = new MarkerImage().Url = "icons/car_icon3big.png";
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
                        GP1.Icon = new MarkerImage().Url = "icons/car_icon2.png";
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
                        GP1.Icon = new MarkerImage().Url = "icons/car_icon4.png";
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
            DateBoxError.Text = "Please select a Vehicle to follow!";
            liveFollow.Checked = false;
        }
    }
    protected void autoRefresh_CheckedChanged(object sender, EventArgs e)
    {
        if (autoRefresh.Checked == false)
        {
            Timer1.Enabled = false;
            DataSet ds = generateArrayIndex();
            MapLaod(ds);
        }
        else
        {
            Timer1.Enabled = true;
            DataSet ds = generateArrayIndex();
            MapLaod(ds);
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
            MapLaod(ds);
        }
    }
    protected void generateDashboard(DataRow dr)
    {
        // This sample demonstrates using absolute positioning of several gauges on a single chart.

        dashboard.TempDirectory = "~/images";
        dashboard.Debug = true;
        dashboard.Palette = new Color[] { Color.FromArgb(49, 255, 49), Color.FromArgb(255, 255, 0), Color.FromArgb(255, 99, 49), Color.FromArgb(0, 156, 255) };
        dashboard.Type = ChartType.Gauges;

        // Chart.Size = "360";
        dashboard.Height = ReplayMap.Height;
        dashboard.Width = System.Web.UI.WebControls.Unit.Pixel(295);
        dashboard.OverlapFooter = true;

        // Title Box Customization

        dashboard.TitleBox.Label.Color = Color.White;
        dashboard.LegendBox.Position = LegendBoxPosition.None;

        dashboard.TitleBox.Position = TitleBoxPosition.None;
        dashboard.TitleBox.Label.Shadow.Color = Color.FromArgb(105, 0, 0, 0);
        dashboard.TitleBox.Label.Shadow.Depth = 2;
        dashboard.TitleBox.Background.ShadingEffectMode = ShadingEffectMode.Eight;
        dashboard.TitleBox.Background.Color = Color.FromArgb(100, 80, 180);

        dashboard.ShadingEffectMode = ShadingEffectMode.Four;
        dashboard.DefaultSeries.Background.Color = Color.White;
        dashboard.LegendBox.Background = new Background(Color.White, Color.FromArgb(229, 233, 236), 45);











        Chart1.TempDirectory = "~/images";
        Chart1.Debug = true;
        Chart1.Palette = new Color[] { Color.FromArgb(49, 255, 49), Color.FromArgb(255, 255, 0), Color.FromArgb(255, 99, 49), Color.FromArgb(0, 156, 255) };
        Chart1.Type = ChartType.Gauges;

        // Chart.Size = "360";
        Chart1.Height = System.Web.UI.WebControls.Unit.Pixel(160);
        Chart1.Width = System.Web.UI.WebControls.Unit.Pixel(80);
        Chart1.OverlapFooter = true;

        // Title Box Customization

        Chart1.TitleBox.Label.Color = Color.White;
        Chart1.LegendBox.Position = LegendBoxPosition.None;

        Chart1.TitleBox.Position = TitleBoxPosition.None;
        Chart1.TitleBox.Label.Shadow.Color = Color.FromArgb(105, 0, 0, 0);
        Chart1.TitleBox.Label.Shadow.Depth = 2;
        Chart1.TitleBox.Background.ShadingEffectMode = ShadingEffectMode.Eight;
        Chart1.TitleBox.Background.Color = Color.FromArgb(100, 80, 180);

        Chart1.ShadingEffectMode = ShadingEffectMode.Four;
        Chart1.DefaultSeries.Background.Color = Color.White;
        Chart1.LegendBox.Background = new Background(Color.White, Color.FromArgb(229, 233, 236), 45);
        // *DYNAMIC DATA NOTE* 
        // This sample uses random data to populate the chart. To populate 
        // a chart with database data see the following resources:
        // - Help File > Getting Started > Data Tutorials
        // - DataEngine Class in the help file      
        // - Sample: features/DataEngine.aspx

        SeriesCollection mySC = new SeriesCollection();

        // Setup Gauges

       


        Series s = new Series(dr["carrierName"].ToString() + "Speed");
        Element e = new Element("Element");
        e.YValue = Convert.ToDouble(dr["speed"]);
        s.Elements.Add(e);
        mySC.Add(s);


        mySC[0].YAxis = new Axis();
        mySC[0].GaugeType = GaugeType.Circular;
        mySC[0].YAxis.SweepAngle = 180;
        mySC[0].YAxis.OrientationAngle = 270;
        mySC[0].Name = "";
        mySC[0].YAxis.Minimum = 0;
        mySC[0].YAxis.Maximum = 150;
        mySC[0].YAxis.Interval = 20;
        mySC[0].YAxis.Markers.Add(new AxisMarker("", new Background(Color.Green), 0, 60));
        mySC[0].YAxis.Markers.Add(new AxisMarker("", new Background(Color.Yellow), 60, 100));
        mySC[0].YAxis.Markers.Add(new AxisMarker("", new Background(Color.Red), 100, 150));
        mySC[0].YAxis.DefaultTick.Label.Font = new Font("Agency FB", 10, FontStyle.Bold);

        s = new Series(dr["carrierName"].ToString() + "Fuel");
        e = new Element("Element");
        try
        {
            e.YValue = Convert.ToDouble(dr["FuelCounter"]);
        }
        catch
        {
            e.YValue = 0;
        }
        s.Elements.Add(e);
        mySC.Add(s);


        mySC[1].YAxis = new Axis();
        mySC[1].GaugeType = GaugeType.Circular;
        mySC[1].YAxis.SweepAngle = 90;
        mySC[1].YAxis.OrientationAngle = 270;
        mySC[1].YAxis.Minimum = 0;
        mySC[1].YAxis.Maximum = 100;
        mySC[1].YAxis.Interval = 25;
        mySC[1].YAxis.LabelOverrides.Add(new LabelOverride("0", "E"));
        mySC[1].YAxis.LabelOverrides.Add(new LabelOverride("100", "F"));
        mySC[1].YAxis.ClearValues = true;
        mySC[1].YAxis.LabelMarker = new ElementMarker("~/Images/fuel.gif", 30);
        mySC[1].YAxis.Markers.Clear();
        mySC[1].YAxis.Markers.Add(new AxisMarker("", new Background(Color.Red), 75, 100));
        mySC[1].YAxis.DefaultTick.Label.Font = new Font("Arial", 14, FontStyle.Bold);
        mySC[1].Name = "";


        s = new Series(dr["carrierName"].ToString() + "Speed");
        e = new Element("Element");
        try
        {
            e.YValue = (100 - Convert.ToDouble(dr["pcbTemp"]) / 10);
        }
        catch
        {
            e.YValue = 0;
        }
        s.Elements.Add(e);
        mySC.Add(s);



        mySC[2].YAxis = new Axis();
        mySC[2].GaugeType = GaugeType.Circular;
        mySC[2].YAxis.SweepAngle = 90;
        mySC[2].YAxis.OrientationAngle = 0;
        mySC[2].YAxis.Minimum = 0;
        mySC[2].YAxis.Maximum = 100;
        mySC[2].YAxis.Interval = 25;
        mySC[2].YAxis.LabelOverrides.Add(new LabelOverride("0", "H"));
        mySC[2].YAxis.LabelOverrides.Add(new LabelOverride("100", "C"));
        mySC[2].YAxis.ClearValues = true;
        mySC[2].YAxis.LabelMarker = new ElementMarker("~/Images/icon_temp.png", 30);
        mySC[2].YAxis.Markers.Clear();
        mySC[2].YAxis.Markers.Add(new AxisMarker("", new Background(Color.Red), 0, 50));
        mySC[2].YAxis.DefaultTick.Label.Font = new Font("Arial", 14, FontStyle.Bold);
        mySC[2].Name = "";

        s = new Series(dr["carrierName"].ToString() + "Speed");
        e = new Element("Element");
        try
        {
            e.YValue = Convert.ToDouble(dr["Odometer"]);
        }
        catch
        {
            e.YValue = 0;
        }
        s.Elements.Add(e);
        mySC.Add(s);


        mySC[3].Type = ChartType.Gauges;
        mySC[3].GaugeType = GaugeType.DigitalReadout;
        mySC[3].YAxis = new Axis();
        mySC[3].XAxis = new Axis();
        mySC[3].DefaultElement.SmartLabel.Font = new Font("Arial", 12, FontStyle.Bold);
        mySC[3].GaugeBorderBox.Background.Color = Color.FromArgb(200, Color.Black);
        mySC[3].Name = "";
        // mySC[3].YAxis.ClearValues = true;
        mySC[3].XAxis.ClearValues = true;

        //

        s = new Series(dr["carrierName"].ToString() + "Speed");
        e = new Element("Element");
        try
        {           
            e.YValue = Convert.ToDouble(dr["BatteryVoltage"]) / 1000;
        }
        catch
        {
            e.YValue = 0;
        }
        s.Elements.Add(e);
        //mySC.Add(s);

        Series batterySeries = s;

        //battery
        
        batterySeries.YAxis = new Axis();
        batterySeries.YAxis.Maximum = 100;
        batterySeries.YAxis.Minimum = 0;
        batterySeries.YAxis.Interval = 20;

        batterySeries.GaugeType = GaugeType.Vertical;
        

        batterySeries.GaugeLinearStyle = GaugeLinearStyle.Normal;
        batterySeries.Name = "Battery";
        batterySeries.Type = SeriesType.BarSegmented;
        batterySeries.Background.Color = Color.White;
        batterySeries.YAxis.Orientation = Orientation.TopRight;// Label.Alignment = StringAlignment.Center;




        


        //mySC[3].YAxis = new Axis();
        //mySC[3].GaugeType = GaugeType.Vertical;
        //mySC[3].GaugeLinearStyle = GaugeLinearStyle.Thermometer;

        //// Progress bars
        //mySC[4].YAxis = new Axis();
        //mySC[4].GaugeType = GaugeType.Horizontal;
        //mySC[4].Type = SeriesType.BarSegmented;

        //mySC[5].YAxis = new Axis();
        //mySC[5].GaugeType = GaugeType.Horizontal;
        //mySC[5].Type = SeriesType.BarSegmented;

        //// Bars
        //mySC[6].YAxis = new Axis();
        //mySC[6].GaugeType = GaugeType.Bars;
        //mySC[6].Palette = Chart.Palette;
        //mySC[6].DefaultElement.LegendEntry.SortOrder = 3;

        LegendEntry le = new LegendEntry("Series 7", "");
        le.SortOrder = 2;
        le.PaddingTop = 9;
        le.DividerLine.Color = Color.Black;
        le.LabelStyle.Font = new Font("Arial", 8, FontStyle.Bold);
        dashboard.LegendBox.ExtraEntries.Add(le);

        // Specify Absolute Positions for each gauge
        mySC[0].GaugeBorderBox.Position = new Rectangle(10, 10, 250, 200);
        mySC[1].GaugeBorderBox.Position = new Rectangle(10, 200, 140, 140);
        mySC[2].GaugeBorderBox.Position = new Rectangle(140, 200, 140, 140);
        mySC[3].GaugeBorderBox.Position = new Rectangle(((20 + 200) / 2) - 25, 0 +80, 100, 20);
       
        //mySC[5].GaugeBorderBox.Position = new Rectangle(20, 120, 150, 50);
        //mySC[6].GaugeBorderBox.Position =  new Rectangle(330, 40, 130, 130);

        // Add the random data.


        dashboard.SeriesCollection.Add(mySC);

       batterySeries.GaugeBorderBox.Position = new Rectangle(10, 10, 50, 120);
       Chart1.SeriesCollection.Add(batterySeries);




       Chart.TempDirectory = "~/images";
       Chart.Height = System.Web.UI.WebControls.Unit.Pixel(160);
       Chart.Width = System.Web.UI.WebControls.Unit.Pixel(80);
       
       Chart.Title = "Tank Status";
       Chart.Debug = true;
       Chart.Mentor = false;
       Chart.Palette = new Color[] { Color.Aqua, Color.White, Color.FromArgb(255, 99, 49), Color.FromArgb(0, 156, 255) };

       Chart.TitleBox.ClearColors();
       Chart.Use3D = true;
       Chart.YAxis.Scale = Scale.Normal;
       
       Chart.DefaultSeries.Type = SeriesType.Cylinder;
        
       Chart.LegendBox.Visible = false;
       Chart.Depth = 30;

       Chart.DefaultElement.Transparency = 15;

       Chart.ChartArea.ClearColors();
       Chart.YAxis.ClearValues = true;
       //Chart.XAxis.ClearValues = true;
       Chart.YAxis.Line.Color = Color.Transparent;
       Chart.XAxis.Line.Color = Color.Transparent;
       Chart.XAxis.Orientation = dotnetCHARTING.Orientation.Top;
       Chart.XAxis.TickLabelPadding = 7;
       Chart.XAxis.DefaultTick.Label.Font = new Font("Arial", 11, FontStyle.Bold);
       Annotation an = new Annotation("%YValue");
       an.Orientation = dotnetCHARTING.Orientation.Top;
       an.Orientation = dotnetCHARTING.Orientation.Top;
       an.DefaultCorner = BoxCorner.Square;
       an.Label.Color = Color.White;
       an.Label.Font = new Font("Arial", 9, FontStyle.Bold);
       an.Line.Color = Color.White;
       an.Line.Width = 2;
       an.CornerSize = 10;
       an.Background = new Background(Color.FromArgb(248, 186, 0), Color.FromArgb(219, 96, 0), 90);
       an.Shadow.Color = Color.FromArgb(100, Color.LightGray);
       

       // *DYNAMIC DATA NOTE* 
       // This sample uses random data to populate the chart. To populate 
       // a chart with database data see the following resources:
       // - Use the getLiveData() method using the dataEngine to query a database.
       // - Help File > Getting Started > Data Tutorials
       // - DataEngine Class in the help file      
       // - Sample: features/DataEngine.aspx

      

       s = new Series(dr["carrierName"].ToString() + "Speed");
       e = new Element("Element");
       try
       {
           e.YValue = Convert.ToDouble(dr["BatteryVoltage"]) / 1000;
       }
       catch
       {
           e.YValue = 0;
       }
       s.Elements.Add(e);
       //mySC.Add(s);

       batterySeries = s;
       batterySeries[0].Annotation =an;
       

       //mySC[1][0].Annotation = new Annotation("%YValue",an);
      
       // Add the random data.
       batterySeries.GaugeBorderBox.Position = new Rectangle(10, 10, 50, 120);
       Chart.SeriesCollection.Add(s);





    }

   
    SeriesCollection getRandomData()
    {
        Random myR = new Random(1);
        SeriesCollection SC = new SeriesCollection();
        int a = 0, b = 0, c = 2;
        for (a = 1; a < 5; a++)
        {
            c = 2;
            if (a == 7) c = 5;
            Series s = new Series("Series " + a.ToString());
            for (b = 1; b < c; b++)
            {
                Element e = new Element("Element " + b.ToString());
                e.YValue = myR.Next(50);
                s.Elements.Add(e);
            }
            SC.Add(s);
        }
        return SC;
    }

  
}