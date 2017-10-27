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
using Telerik.Charting;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

using System.Threading;

using System.Text;


public partial class replayOnMap : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((Session["role"] == null))
        {
            Response.Redirect("Default.aspx");
        }
        //dateFrom.SelectedDate = DateTime.Parse("01-Dec-12 12:00:00 AM");
        //dateTo.SelectedDate = DateTime.Parse("02-Dec-12 12:00:00 AM");
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
    public string getSelectedCarrierName()
    {
        int[] arr;
        string carrierName;
        arr = car_listbox.GetSelectedIndices();
        carrierName = car_listbox.Items[arr[0]].Text;
        return carrierName;
    }
    protected void RadReplay_Click(object sender, EventArgs e)
    {
        //int[] arr;
        //int carrierId=0;
        
        //arr = car_listbox.GetSelectedIndices();       
        //carrierId=Int32.Parse(car_listbox.Items[arr[0]].Value);
        //double[] latitude=new double[2];
        //latitude[0] = 22.2;
        //ClientScript.RegisterArrayDeclaration("latitudeArr", "'22.0'");
        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "MyKey", "placeMarker('" + 20 + "', '" + latitude + "', '" + 74.0 + "', '" + 50 + "', '" + 15 + "');", true);
        
        //int[] x = new int[] { 1, 2, 3, 4, 5 };
        //int[] y = new int[] { 1, 2, 3, 4, 5 };

        //string xStr = getArrayString(x); // converts {1,2,3,4,5} to [1,2,3,4,5]
        //string yStr = getArrayString(y);

        //string script = String.Format("test({0},{1})", xStr, yStr);
       
        oldTrack_Click();
       
    }
   
    protected void oldTrack_Click()
    {
        if (datevalidate())
        {
            if (chkPlotLBS.Checked)
            {
                updatePanelControlPanel.Update();
                DataSet ds = new System.Data.DataSet();
                cls_Reports obj_report = new cls_Reports();
                obj_report.carrierId = getSelectedCarrierID();
                obj_report.dateStart = dateFrom.SelectedDate.ToString();
                obj_report.dateEnd = dateTo.SelectedDate.ToString();

                // DateTime dt = (DateTime)dateFrom.SelectedDate;
                ds = obj_report.fn_TrackcarrierWithGPSLBS(obj_report, (DateTime)dateFrom.SelectedDate, (DateTime)dateTo.SelectedDate);
                plotTrack(ds);
                //plotLBS(ds);
            }
            else
            {
               
                updatePanelControlPanel.Update();
                DataSet ds = new System.Data.DataSet();
                cls_Reports obj_report = new cls_Reports();
                obj_report.carrierId = getSelectedCarrierID();
                obj_report.dateStart = dateFrom.SelectedDate.ToString();
                obj_report.dateEnd = dateTo.SelectedDate.ToString();
                ds = obj_report.fn_ReplayOnMap(obj_report);
                plotTrack(ds);
                
            }

        }
    }
   

    protected bool datevalidate()
    {
        try
        {
            string str = dateFrom.SelectedDate.ToString();
            if (dateFrom.SelectedDate.ToString() == "" || dateTo.SelectedDate.ToString() == "")
            {
                
                string error = "Date Fields cannot be kept empty!!";
                string script = String.Format("showAlert('{0}')", error);

                System.Web.UI.ScriptManager.RegisterClientScriptBlock(updatePanelControlPanel, updatePanelControlPanel.GetType(), "showAlert", script, true);
       
                return false;
            }
            CultureInfo culture = new CultureInfo("en-US");
            DateTime dt = Convert.ToDateTime(dateFrom.SelectedDate.ToString(), culture);// txtTimeStart.Text;
            DateTime dt1 = Convert.ToDateTime(dateTo.SelectedDate.ToString(), culture);

            if (dt > dt1)
            {
         
                string error="Not valid date range";
                string script = String.Format("showAlert('{0}')",error);

                System.Web.UI.ScriptManager.RegisterClientScriptBlock(updatePanelControlPanel, updatePanelControlPanel.GetType(), "showAlert", script, true);
       
                return false;
            }
            else
            {
                System.TimeSpan span = dt1.Subtract(dt);
                int days = span.Days;
                days = days + 1;
                if (days > 7)
                {
                    //if (dd_report.SelectedValue == "geoFencing" || dd_report.SelectedValue == "kmTravld")
                    //{
                    //    DateBoxError.Text = "";
                    //    return true;
                    //}
                    //lblmsg.Visible = true;
                    //lblmsg.Text = "Select date range less than 8 days";
                    //lblmsg.Text = "Tracking can not Support for more than 7 days" + "<br/>" + "Contact Support";
                    return false;
                }
                else
                {
                   
                    return true;
                }
            }
        }
        catch (Exception)
        {
           // DateBoxError.Text = "";
            string error = "Wrong dates Selected!!";
            string script = String.Format("showAlert('{0}')",error);

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(updatePanelControlPanel, updatePanelControlPanel.GetType(), "showAlert", script, true);
       
            return false;
        }
    }
   
    
    private string getArrayString(DataTable dt, string colName,bool flag)
    {
        StringBuilder sb = new StringBuilder();
        if (flag == true)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                sb.Append("'"+dt.Rows[i][colName].ToString()+"'" + ",");
            }
            string arrayStr = string.Format("[{0}]", sb.ToString().TrimEnd(','));
            return arrayStr;
        }

        else
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                sb.Append(dt.Rows[i][colName].ToString() + ",");
            }
            string arrayStr = string.Format("[{0}]", sb.ToString().TrimEnd(','));
            return arrayStr;
        }
        
    }
    void plotTrack(DataSet ds)
    {
        string maxSpeed = "75";
        string carrierTypeFID="-1";
     
        maxSpeed = ds.Tables[1].Rows[0]["maxSpeed"].ToString();       
        carrierTypeFID = ds.Tables[1].Rows[0]["carrierTypeFId"].ToString();
        if (maxSpeed == "")
        {
            maxSpeed = "75";
        }
        
        string latitude = getArrayString(ds.Tables[0], "latitude",false); // converts {1,2,3,4,5} to [1,2,3,4,5]
        string longitude = getArrayString(ds.Tables[0], "longitude", false);
        string speed = getArrayString(ds.Tables[0], "speed", false);
        string time = getArrayString(ds.Tables[0], "time", true);
        string address = getArrayString(ds.Tables[0], "address", true);
        string angle = getArrayString(ds.Tables[0], "angle", false);
        string din1 = getArrayString(ds.Tables[0], "Din1", false);
        string din2 = getArrayString(ds.Tables[0], "Din2", false);
        string lbsLocation = getArrayString(ds.Tables[0], "lbsLocation", false);
        
        string script = String.Format("plotTrack('{0}',{1},{2},{3},{4},'{5}',{6},{7},{8},{9},{10},{11},{12})", getSelectedCarrierName(), latitude, longitude, speed, time, maxSpeed, angle, din1, din2, lbsLocation, address,maxSpeed,carrierTypeFID);

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(updatePanelControlPanel, updatePanelControlPanel.GetType(), "plotTrack", script, true);
        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "plotTrack", script, true);
      
       
        //string lat = "20";
        //string lng = "73";
        //Artem.Google.UI.GooglePolyline pl = new Artem.Google.UI.GooglePolyline();
        //pl.StrokeColor = System.Drawing.Color.Blue;
        //pl.StrokeWeight = 5;
        //ReplayMap.Markers.Clear();
        //if (ds.Tables[0].Rows.Count <= 0)
        //{
        //}
        //else
        //{
        //    DateTime prevTime = (DateTime)ds.Tables[0].Rows[0]["time"];
        //    double oldspeed = 0;
        //    double newlat = 0;
        //    double newlong = 0;

        //    DateTime newtime = DateTime.Now;
        //    DateTime oldtime = DateTime.Now;
        //    int cnt = 0;
        //    int recordCnt = ds.Tables[0].Rows.Count;
        //    int m = 0;
        //    if (chkfilter.Checked == false)
        //    {
        //        while (m < recordCnt)
        //        {
        //            speed = (double)ds.Tables[0].Rows[m]["speed"];
        //            newlat = (double)ds.Tables[0].Rows[m]["latitude"];
        //            newlong = (double)ds.Tables[0].Rows[m]["longitude"];
        //            newtime = (DateTime)ds.Tables[0].Rows[m]["time"];

        //            if (speed < 1)
        //            {
        //                lat = newlat.ToString();
        //                lng = newlong.ToString();
        //                string PlateNo = txtVehName.Text;
        //                string Speed = speed.ToString();
        //                string Date = newtime.ToString();
        //                Marker GP1 = new Marker();
        //                GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));
        //                GP1.Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
        //                GP1.Title = newtime.TimeOfDay.ToString(); ;
        //                GP1.Icon = new MarkerImage().Url = "icons/car_icon3.png";
        //                ReplayMap.Markers.Add(GP1);
        //                pl.Path.Add(new LatLng(newlat, newlong));
        //            }
        //            else if (speed > maxSpeed)
        //            {
        //                lat = newlat.ToString();
        //                lng = newlong.ToString();
        //                string PlateNo = txtVehName.Text;
        //                string Speed = speed.ToString();
        //                string Date = newtime.ToString();
        //                Marker GP1 = new Marker();
        //                GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));
        //                GP1.Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
        //                GP1.Title = newtime.TimeOfDay.ToString();
        //                GP1.Icon = new MarkerImage().Url = "icons/car_icon2.png";
        //                ReplayMap.Markers.Add(GP1);
        //                pl.Path.Add(new LatLng(newlat, newlong));
        //            }
        //            else
        //            {
        //                lat = newlat.ToString();
        //                lng = newlong.ToString();
        //                string PlateNo = txtVehName.Text;
        //                string Speed = speed.ToString();
        //                string Date = newtime.ToString();
        //                Marker GP1 = new Marker();
        //                GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));

        //                GP1.Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
        //                GP1.Title = newtime.TimeOfDay.ToString();
        //                GP1.Icon = new MarkerImage().Url = "icons/car_icon4.png";
        //                ReplayMap.Markers.Add(GP1);
        //                pl.Path.Add(new LatLng(newlat, newlong));

        //            }
        //            m++;
        //        }
        //    }
        //    else
        //    {
        //        while (m < recordCnt)
        //        {
        //            speed = (double)ds.Tables[0].Rows[m]["speed"];
        //            newlat = (double)ds.Tables[0].Rows[m]["latitude"];
        //            newlong = (double)ds.Tables[0].Rows[m]["longitude"];
        //            newtime = (DateTime)ds.Tables[0].Rows[m]["time"];
        //            cnt = 0;
        //            if (speed <= 2)
        //            {
        //                m++;
        //                if (m >= recordCnt)
        //                    break;
        //                oldspeed = (double)ds.Tables[0].Rows[m]["speed"];
        //                while (oldspeed <= 2 && m < recordCnt)
        //                {
        //                    oldspeed = (double)ds.Tables[0].Rows[m]["speed"];
        //                    m++;
        //                    cnt++;
        //                }
        //                if (cnt < 2)
        //                {
        //                    lat = newlat.ToString();
        //                    lng = newlong.ToString();
        //                    string PlateNo = txtVehName.Text;
        //                    string Speed = speed.ToString();
        //                    string Date = newtime.ToString();
        //                    Marker GP1 = new Marker();
        //                    GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));

        //                    GP1.Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
        //                    GP1.Title = newtime.TimeOfDay.ToString(); ;
        //                    GP1.Icon = new MarkerImage().Url = "icons/car_icon3.png";
        //                    ReplayMap.Markers.Add(GP1);
        //                    pl.Path.Add(new LatLng(newlat, newlong));
        //                }
        //                else
        //                {
        //                    oldspeed = (double)ds.Tables[0].Rows[m - 2]["speed"];
        //                    oldtime = (DateTime)ds.Tables[0].Rows[m - 2]["time"];
        //                    lat = newlat.ToString();
        //                    lng = newlong.ToString();
        //                    string PlateNo = txtVehName.Text;
        //                    string Speed = "0";
        //                    string Date = newtime.ToString();
        //                    Marker GP1 = new Marker();
        //                    GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));

        //                    GP1.Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Time from:" + newtime + "  to:" + oldtime;
        //                    GP1.Title = "Stop from " + newtime.TimeOfDay + " to " + oldtime.TimeOfDay;
        //                    GP1.Icon = new MarkerImage().Url = "icons/car_icon3big.png";
        //                    ReplayMap.Markers.Add(GP1);
        //                    pl.Path.Add(new LatLng(newlat, newlong));

        //                }
        //            }
        //            else if (speed > maxSpeed)
        //            {
        //                lat = newlat.ToString();
        //                lng = newlong.ToString();
        //                string PlateNo = txtVehName.Text;
        //                string Speed = speed.ToString();
        //                string Date = newtime.ToString();
        //                Marker GP1 = new Marker();
        //                GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));

        //                GP1.Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
        //                GP1.Title = newtime.TimeOfDay.ToString();
        //                GP1.Icon = new MarkerImage().Url = "icons/car_icon2.png";
        //                ReplayMap.Markers.Add(GP1);
        //                pl.Path.Add(new LatLng(newlat, newlong));
        //            }
        //            else
        //            {
        //                lat = newlat.ToString();
        //                lng = newlong.ToString();
        //                string PlateNo = txtVehName.Text;
        //                string Speed = speed.ToString();
        //                string Date = newtime.ToString();
        //                Marker GP1 = new Marker();
        //                GP1.Position = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));

        //                GP1.Info = "Plate No : " + PlateNo + "</br>" + "Latitude : " + lat + "</br>" + "Longitude : " + "" + lng + "</br>" + "Speed : " + "" + Speed + "</br>" + "Date & Time : " + Date;
        //                GP1.Title = newtime.TimeOfDay.ToString();
        //                GP1.Icon = new MarkerImage().Url = "icons/car_icon4.png";
        //                ReplayMap.Markers.Add(GP1);
        //                pl.Path.Add(new LatLng(newlat, newlong));

        //            }
        //            m++;
        //        }
        //    }
        //    ReplayMap.Polylines.Add(pl);
        //    ReplayMap.Center = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));
        //}

    }
    void plotLBS(DataSet ds)
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
                //ReplayMap.Markers.Add(GP1);
                //pl.Path.Add(new LatLng(newlat, newlong));

                m++;
            }

            //ReplayMap.Polylines.Add(pl);
            //ReplayMap.Center = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));
        }

    }


}