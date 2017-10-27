using System;
using System.Web.UI.WebControls;
using GPSTrackingBLL;
using System.Data;
using System.Globalization;
using Artem.Google.UI;
using Telerik.Charting;
using Telerik.Web.UI;



public partial class Reports : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((Session["role"] == null))
        {
            Response.Redirect("Default.aspx");
        }
        if (!IsPostBack)
        {
            lblGeoVisit.Visible = false;
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

            //ds = obj_carrier.fn_CarrierLastLoc_Fetch(Convert.ToInt32(Session["role"].ToString()), Convert.ToInt32(Session["fk_CompanyID"].ToString()), Convert.ToInt32(Session["fk_OrgID"].ToString()));
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
    #region reports
    protected double calculateKm(DataRow[] dt)
    {
        int row = 0;
        double km = 0;
        if (dt.Length > 1)
        {
            DateTime day = (DateTime)dt[0]["time"];
            for (row = 1; row < dt.Length; row++)
            {
                double dist = CalcDistanceBetween((float)Convert.ToDouble(dt[row - 1]["lat"]), (float)Convert.ToDouble(dt[row - 1]["long"]), (float)Convert.ToDouble(dt[row]["lat"]), (float)Convert.ToDouble(dt[row]["long"]));
                km = km + Convert.ToDouble(dist);
            }
            km = km + (double)((Convert.ToDecimal(km) / 100) * 5);
        }
        return km;
    }

    #region Monoreports
    string[] laton;
    string[] longon;
    string[] latoff;
    string[] longoff;
    DataTable km_table = new DataTable();
    CultureInfo culturenew = new CultureInfo("en-US");
    public int getSelectedCarrierID()
    {
        int[] arr;
        int carrierId = 0;
        arr = car_listbox.GetSelectedIndices();
        carrierId = Int32.Parse(car_listbox.Items[arr[0]].Value);
        return carrierId;
    }
    public String getSelectedCarrierName()
    {
        int[] arr;
        string  carrierId = String.Empty;
        arr = car_listbox.GetSelectedIndices();
        carrierId = car_listbox.Items[arr[0]].Text;
        return carrierId;
    }
    protected void btngenRpt_Click(object sender, EventArgs e)
    {
        //try
        //{

        lblmsg.Text = "";
        bool valid = datevalidate();

        if (valid == true)
        {
            //lblmsgval.Text = dd_report.Items[dd_report.SelectedIndex].Text + " Report For " + txtVehName.Items[txtVehName.SelectedIndex].Text + ":";
            btnexport.Visible = true;
            btnexport_pdf.Visible = true;

            if (dd_report.SelectedValue == "fuel")
            {
                Load_Report("rpt_prc_Fual", 0);
            }
            else if (dd_report.SelectedValue == "speed")
            {
                Load_Report("speed", 1);
            }
            else if (dd_report.SelectedValue == "activity")
            {
                Load_Report("activity", 1);

            }
            else if (dd_report.SelectedValue == "overSpeed")
            {
                Load_Report("overSpeed", 1);
            }

            else if (dd_report.SelectedValue == "temp")
            {
                Load_Report("rpt_prc_Temp", 1);
            }
            else if (dd_report.SelectedValue == "kmTravld")
            {
                Load_Report("rpt_prc_KM", 1);
            }
            else if (dd_report.SelectedValue == "geoFencing")
            {
                Load_Report("rpt_prc_geoFencing", 1);
            }
            else if (dd_report.SelectedValue == "worktime")
            {
                Load_Report("rpt_prc_WorkTime", 1);
            }
            else if (dd_report.SelectedValue == "idle")//idle time
            {
                Load_Report("rpt_prc_idle", 1);
            }
            else if (dd_report.SelectedValue == "ignition")//ignition
            {
                Load_Report("rpt_prc_Ignition_OnOff", 0);
            }
            else if (dd_report.SelectedValue == "ignSeq") //ignition sequence
            {
                Load_Report("rpt_prc_Ignition_OnOff_Sequence", 0);
            }
            else if (dd_report.SelectedValue == "summarry")//daily summary
            {
                Load_Report("rpt_prc_Daily_Summary", 0);
            }
            else if (dd_report.SelectedValue == "fenceBKMT")//daily summary
            {
                Load_Report("rpt_prc_fenceBKMT", 0);
            }
            else if (dd_report.SelectedValue == "boldstop") // bold stop 
            {
                if (txt_boldStop.Text == "")
                {
                    lblmsg.Visible = true;
                    lblmsg.Text = "Please Enter Stoppage Time";
                }
                else
                {
                    Load_Report("rpt_prc_Bold_Stop_Summary", 0);
                }
            }
        }


        //}
        //catch (Exception ex)
        //{
        //}

        UpdatePanelReportGrid.Update();
        UpdatePanelCharts.Update();
    }
    protected void GetKM(DateTime st_dt, DateTime ed_dt)
    {
        decimal j = Convert.ToDecimal(0.0);
        //try
        //{
        cls_Reports rpt = new cls_Reports();

        rpt.VehName = getSelectedCarrierName();
        rpt.dateStart = Convert.ToString(st_dt);
        rpt.dateEnd = Convert.ToString(ed_dt);
        rpt.task = 0;


        lblKM.Text = "";
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        rpt.carrierId = Convert.ToInt32(getSelectedCarrierID());


        if (dd_report.SelectedValue == "kmTravld")
        {
            rpt.task = 1;
            ds = rpt.fn_prc_KMtravel(rpt, 1);
        }
        else
        {
            ds = rpt.fn_prc_KMtravel(rpt, 0);
        }


        // ds = rpt.fn_prc_KMtravel(rpt,0);
        dt = ds.Tables[0];

        int count = ds.Tables[0].Rows.Count;
        int row = 0, col = 0;
        float lat1, lat2, long1, long2;
        double km = 0;


        while (row < count)
        {
            //lat1, lon1, lat2, lon2
            //18.5021982,73.9317570,18.5021984,73.9317568
            lat1 = float.Parse(ds.Tables[0].Rows[row][col].ToString());
            col++;
            long1 = float.Parse(ds.Tables[0].Rows[row][col].ToString());
            col--;
            row++;
            if (row < count)
            {
                lat2 = float.Parse(ds.Tables[0].Rows[row][col].ToString());
                col++;
                long2 = float.Parse(ds.Tables[0].Rows[row][col].ToString());
                col--;


                if (dd_report.SelectedValue == "kmTravld")
                {
                    double abc = CalcDistanceBetween(lat1, long1, lat2, long2);

                    km = km + Convert.ToDouble(abc);

                }
                else
                {
                    double abc = CalcDistanceBetween(lat1, long1, lat2, long2);

                    km = km + Convert.ToDouble(abc);
                }



                ViewState["km_Summary"] = km;
                //double summary = Convert.ToDouble(ViewState["summery"]);
                //ViewState["summery"] = summary + km;
            }


        }

        if (dd_report.SelectedValue == "km")
        {

            decimal km_dsc = Convert.ToDecimal(km);
            decimal km_dec = Math.Round(km_dsc, 2);


            string date_start = dateFormate(Convert.ToString(st_dt));
            string date_end = dateFormate(Convert.ToString(ed_dt));

            //km_table.Rows.Add(date_start, OFFtime);
            km_table.Rows.Add(date_start, km_dec);

            gv_Report.DataSource = km_table;
            gv_Report.DataBind();
            ViewState["tbl_KM"] = km_table;

            int cnt = km_table.Rows.Count;
            int i = 0;



            for (i = 0; i < cnt; i++)
            {
                j = j + Convert.ToDecimal(km_table.Rows[i]["KM Travel"].ToString());
            }

        }

        if (dd_report.SelectedValue == "kmTravld")
        {

            decimal km_dsc = Convert.ToDecimal(km);
            decimal km_dec = Math.Round(km_dsc, 2);
            decimal addextra = (km_dec / 100) * 5;

            string date_start = dateFormate(Convert.ToString(st_dt));
            string date_end = dateFormate(Convert.ToString(ed_dt));

            //km_table.Rows.Add(date_start, OFFtime);
            km_table.Rows.Add(date_start, (km_dec + addextra));

            gv_Report.DataSource = km_table;
            gv_Report.DataBind();
            ViewState["tbl_KM"] = km_table;

            int cnt = km_table.Rows.Count;
            int i = 0;



            for (i = 0; i < cnt; i++)
            {
                j = j + Convert.ToDecimal(km_table.Rows[i]["KM Travel"].ToString());
            }

        }

        if (dd_report.SelectedValue == "km")
        {
            lblKM.Text = "Total Kilometer Travel is:" + " " + Convert.ToString(j);
        }

        //}
        //catch (Exception ex)
        //{
        //}
    }
    private string dateFormate(string date)
    {
        DateTime dt = Convert.ToDateTime(date);

        // Get year, month, and day
        int year = dt.Year;
        int month = dt.Month;
        int day = dt.Day;

        string det = month + "/" + day + "/" + year;
        return det;
    }
    protected double CalcDistanceBetween(float lat1, float lon1, float lat2, float lon2)
    {

        //Radius of the earth in:  1.609344 miles,  6371 km  | var R = (6371 / 1.609344);
        double R = 6371; // Radius in Kilometers default
        double dLat = toRad(lat2 - lat1);
        double dLon = toRad(lon2 - lon1);
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(toRad(lat1)) * Math.Cos(toRad(lat2)) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var d = R * c;

        return d;
    }
    protected double toRad(double Value)
    {
        /** Converts numeric degrees to radians */
        return Value * Math.PI / 180;
    }
    public double ConvertToRadians(double angle)
    {
        return (Math.PI / 180) * angle;
    }
    private void Load_Report(string rptNAme, int onOff)
    {
        //try
        //{

        cls_Reports rpt = new cls_Reports();
        Session["ExportFileName"] = dd_report.SelectedItem.Text;
        rpt.VehName = getSelectedCarrierName();
        rpt.dateStart = dateFrom.SelectedDate.ToString();
        rpt.dateEnd = dateTo.SelectedDate.ToString();
        if (rptNAme == "speed")
        {
            report_speed(rpt);
        }
        else if (rptNAme == "overSpeed")
        {
            report_overSpeed(rpt);
        }
        else if (rptNAme == "activity")
        {
            report_activity(rpt);

        }
        else if (rptNAme == "rpt_prc_geoFencing")
        {
            report_geofence(rpt);
        }
        else if (rptNAme == "rpt_prc_fenceBKMT")
        {
            report_geofenceKmTravelled(rpt);
        }
        else if (rptNAme == "rpt_prc_Daily_Summary")
        {
            report_summary(rpt);
        }
        else if (rptNAme == "rpt_prc_idle")
        {
            report_idleTime(rpt);
        }
        else if (rptNAme == "rpt_prc_KM")
        {
            report_kmTravelled(rpt);

        }
        else if (rptNAme == "rpt_prc_Fual")
        {
            report_fuel(rpt, rptNAme);
        }

        else if (rptNAme == "rpt_prc_Ignition_OnOff_Sequence")
        {
            report_ignOnOffSeq(rpt, rptNAme);
        }
        else if (rptNAme == "rpt_prc_Ignition_OnOff")
        {
            report_ignOnOff(rpt, rptNAme);
        }

        else if (rptNAme == "rpt_prc_Bold_Stop_Summary")
        {
            report_boldStop(rpt);
        }

        else if (rptNAme == "rpt_prc_WorkTime")
        {
            report_workTime(rpt);
        }
        else
        {
            report_temprature(rpt);
        }

        Session["headerString"] = lblRptName.Text;

        gv_Report.MasterTableView.FilterExpression = String.Empty;
        gv_Report.MasterTableView.SortExpressions.Clear();
        gv_Report.MasterTableView.GroupByExpressions.Clear();
        gv_Report.MasterTableView.ClearSelectedItems();
        gv_Report.MasterTableView.ClearEditItems();
        gv_Report.DataBind();
        gv_Report.Visible = true;
        Session["report"] = gv_Report.DataSource;

        //}

        //catch (Exception ex)
        //{

        //}

    }
    protected void report_summary(cls_Reports rpt)
    {
        DataTable dt_details = new DataTable();
        //  gv_Report.DataSource = dt_details;
        //gv_Report.DataBind();

        lblmsg.Text = "";
        DateTime startdt = Convert.ToDateTime(dateFrom.SelectedDate.ToString(), culturenew); // + " " + ddl_HH.SelectedItem.Text + ":" + ddl_MM.SelectedItem.Text + ":" + ddl_SS.SelectedItem.Text);
        DateTime enddt = startdt; //.AddDays(1); // Convert.ToDateTime(txtDateEnd.Text); //+ " " + ddl_HH_end.SelectedItem.Text + ":" + ddl_MM_end.SelectedItem.Text + ":" + ddl_SS_end.SelectedItem.Text);
        DateTime rpt_enddate = startdt.AddDays(1);
        TimeSpan span = enddt.Subtract(startdt);



        lblRptName.Text = "DAILY SUMMARY REPORT FOR " + "'" + getSelectedCarrierName() + "'" + " " + "For" + " " + rpt.dateStart;
        rpt.carrierId = Convert.ToInt32(getSelectedCarrierID());
        rpt.dateEnd = rpt_enddate.ToString();
        string st_lat, st_long, end_lat, end_long;
        string startLoc = "", stoploc = "";

        dt_details = rpt.fn_daySummary(rpt);
        if (dt_details.Rows.Count > 0)
        {
            st_lat = dt_details.Rows[0]["latOn"].ToString();
            st_long = dt_details.Rows[0]["longOn"].ToString();
            end_lat = dt_details.Rows[0]["latOff"].ToString();
            end_long = dt_details.Rows[0]["longOff"].ToString();

            GPSTrackingBLL.cls_Location obj = new cls_Location();
            startLoc = obj.GetAddress(st_lat, st_long);
            stoploc = obj.GetAddress(end_lat, end_long);

            dt_details.Columns.Add("KMTravelINMinutes");
            dt_details.Columns.Add("st_loc");
            dt_details.Columns.Add("end_loc");
        }

        double KM = 0.0;
        if (dt_details.Rows.Count > 0)
        {
            while (startdt <= enddt)
            {
                int day = startdt.Day;
                DateTime enddate = startdt.AddDays(1);
                GetKM(startdt, enddate);
                KM = KM + Convert.ToDouble(ViewState["km_Summary"]);
                startdt = startdt.AddDays(1);
            }
        }
        else
        {
            lblmsg.Visible = true;
            lblmsg.Text = "No record found";
        }

        decimal km_dsc = Convert.ToDecimal(KM);
        decimal km_dec = Math.Round(km_dsc, 2);

        foreach (DataRow dr in dt_details.Rows)
        {
            //need to set value to MyRow column
            dr["KMTravelINMinutes"] = km_dec;   // or set it to some other value
            dr["st_loc"] = startLoc;
            dr["end_loc"] = stoploc;
        }

        datalist_day.Visible = true;
        datalist_day.FindControl("datalist_day");
        datalist_day.DataSource = dt_details;
        datalist_day.DataBind();

        gv_Report.Visible = false;
        gv_Report.DataSource = dt_details;
        ViewState["table_DailySummary"] = dt_details;
    }
    protected void report_fuel(cls_Reports rpt, string rptNAme)
    {
        btnshowchat.Visible = true;
        lblmsg.Text = "";
        rpt.carrierId = Convert.ToInt32(getSelectedCarrierID());
        if (chk_0.Checked == true)
            rpt.onOff = 1;
        else
            rpt.onOff = 0;

        rpt.rpt_query = "mts_getFuelRpt";
        lblRptName.Text = "FUEL REPORT FOR " + "'" + getSelectedCarrierName() + "'" + " " + "From" + " " + rpt.dateStart + " " + "To" + " " + rpt.dateEnd;
        DataTable dt = new DataTable();
        dt = rpt.fn_Fuelrpt(rpt);
        ViewState["table_fuel"] = dt;
        gv_Report.DataSource = dt;// rpt.fn_FuelTemp(rpt);
        // ViewState["tbl_Fuel"] = gv_Report.DataSource;
        if (dt.Rows.Count == 0)
        {
            lblmsg.Visible = true;
            lblmsg.Text = "No record found";
        }
        else
        {
            gv_Report.DataSource = dt;
            gv_Report.Rebind();
            gv_Report.Visible = true;
        //    gv_Report.Width = Unit.Percentage(40);
        }
        //gv_Report.Width = Unit.Percentage(40);
    }
    protected void report_kmTravelled(cls_Reports rpt)
    {
        DateTime startdt = Convert.ToDateTime(dateFrom.SelectedDate.ToString(), culturenew); // + " " + ddl_HH.SelectedItem.Text + ":" + ddl_MM.SelectedItem.Text + ":" + ddl_SS.SelectedItem.Text);
        DateTime enddt = Convert.ToDateTime(dateTo.SelectedDate.ToString(), culturenew); //+ " " + ddl_HH_end.SelectedItem.Text + ":" + ddl_MM_end.SelectedItem.Text + ":" + ddl_SS_end.SelectedItem.Text);
        btnshowchat.Visible = true;
        lblmsg.Text = "";
        try
        {
            lblRptName.Text = "KILOMETER TRAVEL REPORT FOR " + "'" + getSelectedCarrierName() + "'" + " " + "From" + " " + rpt.dateStart + " " + "To" + " " + rpt.dateEnd;
            km_table.Columns.Add(new DataColumn("Date"));
            km_table.Columns.Add(new DataColumn("KM Travel"));
            lblKM.Text = "";
            DataSet ds = new DataSet();
            rpt.carrierId = Convert.ToInt32(getSelectedCarrierID());

            if (dd_report.SelectedValue == "kmTravld")
            {
                rpt.task = 1;
                ds = rpt.fn_prc_KMtravel(rpt, 1);
            }
            else
            {
                ds = rpt.fn_prc_KMtravel(rpt, 0);
            }
            double km = 0;
            double dayKm = 0;
            if (ds.Tables[0].Rows.Count > 1)
            {
                DateTime tempstartDate = startdt;
                DateTime tempEndDate = startdt.AddDays(1);
                TimeSpan span = enddt - startdt;
                if (span.Days > 1)
                {
                    km = dayKm = calculateKm(ds.Tables[0].Select(string.Format("time > \'{0}\' and time<\'{1}\'", tempstartDate, tempstartDate.AddDays(1).Date), "time asc"));
                    km_table.Rows.Add(tempstartDate.ToShortDateString(), Math.Round(dayKm, 2));
                    tempstartDate = tempstartDate.AddDays(1).Date;
                    while (tempstartDate < enddt)
                    {
                        dayKm = calculateKm(ds.Tables[0].Select(string.Format("time > \'{0}\' and time<\'{1}\'", tempstartDate, tempstartDate.AddDays(1).Date), "time asc"));
                        km_table.Rows.Add(tempstartDate.ToShortDateString(), Math.Round(dayKm, 2));
                        km = km + dayKm;
                        tempstartDate = tempstartDate.AddDays(1);
                    }
                }
                else
                {
                    km = calculateKm(ds.Tables[0].Select(string.Format("time > \'{0}\' and time<\'{1}\'", startdt, enddt), "time asc"));
                    km_table.Rows.Add(startdt.ToShortDateString(), Math.Round(km, 2));
                }
            }
            gv_Report.DataSource = km_table;
            gv_Report.Rebind();
            gv_Report.Width = Unit.Percentage(40);
            ViewState["tbl_KM"] = km_table;
            lblKM.Text = "Total Kilometer Travel is:" + " " + Math.Round(km, 2) + "  KM";
            lblKM.Visible = true;

        }
        catch (Exception e)
        {
            cls_fileHandling fh = new cls_fileHandling();
            fh.appendToFile("~/logs/error.txt", DateTime.Now + ": " + e.Message);
            fh.appendToFile("~/logs/stackTrace.txt", DateTime.Now + ": " + e.StackTrace);
        }

    }
    protected void report_boldStop(cls_Reports rpt)
    {
        lblmsg.Text = "";
        rpt.carrierId = Convert.ToInt32(getSelectedCarrierID());
        lblRptName.Text = "BOLD STOP REPORT FOR " + "'" + getSelectedCarrierName() + "'" + " " + "From" + " " + rpt.dateStart + " " + "To" + " " + rpt.dateEnd;
        int stopTime = 5;
        if (txt_boldStop.Text != "")
        {
            try
            {
                stopTime = Int32.Parse(txt_boldStop.Text);
                if (stopTime <= 0)
                {
                    lblmsg.Text = "Please Enter a Number greater than 0 in the Stoppage Time...";
                    lblmsg.ForeColor = System.Drawing.Color.Red;
                    lblmsg.Visible = true;
                    return;
                }
            }
            catch (Exception)
            {
                lblmsg.Text = "Please Enter valid Number in the Stoppage Time...";
                lblmsg.ForeColor = System.Drawing.Color.Red;
                lblmsg.Visible = true;
                return;
            }
        }
        DataTable dt = new DataTable();
        dt = rpt.fn_BoldStop(rpt);
        DataTable table_boldresult = new DataTable();
        table_boldresult.Columns.Add("Start time");
        table_boldresult.Columns.Add("End time");
        table_boldresult.Columns.Add("Bold Stop in Minutes");
        table_boldresult.Columns.Add("Location");
        DateTime stopStart;
        DateTime stopEnd;
        TimeSpan span;
        string location;

        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                while (Convert.ToInt32(dt.Rows[i]["speed"]) > 3)
                {
                    i++;
                    if (i >= dt.Rows.Count)
                    {
                        break;
                    }
                }
                if (i >= dt.Rows.Count)
                {
                    break;
                }
                stopStart = (DateTime)dt.Rows[i]["time"];
                location = dt.Rows[i]["address"].ToString();
                while (Convert.ToInt32(dt.Rows[i]["speed"]) <= 3)
                {
                    i++;
                    if (i >= dt.Rows.Count)
                    {
                        break;
                    }
                }
                if (i >= dt.Rows.Count)
                {
                    stopEnd = (DateTime)dt.Rows[dt.Rows.Count - 1]["time"];
                }
                else
                {
                    stopEnd = (DateTime)dt.Rows[i]["time"];
                }

                span = stopEnd - stopStart;
                if (span.Minutes >= stopTime)
                {
                    table_boldresult.Rows.Add(stopStart, stopEnd, span, location);
                }
            }
        }
        else
        {
            lblmsg.Visible = true;
            lblmsg.Text = "No record found";
        }
        ViewState["table_blodstop"] = table_boldresult;
        gv_Report.DataSource = table_boldresult;
        gv_Report.Width = Unit.Percentage(100);

    }
    protected void report_workTime(cls_Reports rpt)
    {
        if (dd_report.SelectedValue == "worktime")
        {
            lblRptName.Text = "WORKING TIME REPORT FOR " + "'" + getSelectedCarrierName() + "'" + " " + "From" + " " + rpt.dateStart + " " + "To" + " " + rpt.dateEnd;
        }
        btnshowchat.Visible = true;
        lblmsg.Text = "";
        DateTime startdt = Convert.ToDateTime(dateFrom.SelectedDate.ToString(), culturenew); // + " " + ddl_HH.SelectedItem.Text + ":" + ddl_MM.SelectedItem.Text + ":" + ddl_SS.SelectedItem.Text);
        DateTime enddt = Convert.ToDateTime(dateTo.SelectedDate.ToString(), culturenew); //+ " " + ddl_HH_end.SelectedItem.Text + ":" + ddl_MM_end.SelectedItem.Text + ":" + ddl_SS_end.SelectedItem.Text);            
        //TimeSpan span = enddt.Subtract(startdt);
        km_table.Columns.Add(new DataColumn("Date"));
        km_table.Columns.Add(new DataColumn("Work Time in Minutes HH:MM"));
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        int row = 0;
        TimeSpan onTime = TimeSpan.Zero;
        TimeSpan dayTime = TimeSpan.Zero;
        rpt.carrierId = Convert.ToInt32(getSelectedCarrierID());
        ds = rpt.fn_WorkTimeNew(rpt);
        Int16 ignitionConnected = Convert.ToInt16(ds.Tables[1].Rows[0]["digIgnitionUsed"]);
        dt = ds.Tables[0];
        DateTime tempDate;
        TimeSpan sum = TimeSpan.Zero;
        if (ignitionConnected == 1 && dt.Rows.Count > 1)
        {
            tempDate = (DateTime)dt.Rows[0]["time"];
            DateTime day = (DateTime)dt.Rows[0]["time"];
            while (row < dt.Rows.Count)
            {
                tempDate = (DateTime)dt.Rows[0]["time"];
                while (Convert.ToInt32(dt.Rows[row]["din1"]) == 0)
                {
                    try
                    {
                        if (((DateTime)dt.Rows[row]["time"]).Date != ((DateTime)dt.Rows[row - 1]["time"]).Date)
                        {
                            //dayTime = dayTime + ((DateTime)dt.Rows[row - 1]["time"]).Subtract(tempDate);
                            km_table.Rows.Add(((DateTime)dt.Rows[row - 1]["time"]).Date.ToShortDateString(), dayTime);
                            sum = sum + dayTime;
                            dayTime = TimeSpan.Zero;
                            day = (DateTime)dt.Rows[row]["time"];
                        }
                    }
                    catch (Exception)
                    { }
                    row++;
                    if (row >= dt.Rows.Count)
                    {
                        break;
                    }
                }
                if (row >= dt.Rows.Count)
                {
                    break;
                }
                tempDate = (DateTime)dt.Rows[row]["time"];
                while (Convert.ToInt32(dt.Rows[row]["din1"]) == 1)
                {
                    try
                    {
                        if (((DateTime)dt.Rows[row]["time"]).Date != ((DateTime)dt.Rows[row - 1]["time"]).Date)
                        {
                            dayTime = dayTime + ((DateTime)dt.Rows[row - 1]["time"]).Subtract(tempDate);
                            km_table.Rows.Add(((DateTime)dt.Rows[row - 1]["time"]).Date.ToShortDateString(), dayTime);
                            sum = sum + dayTime;
                            dayTime = TimeSpan.Zero;
                            day = (DateTime)dt.Rows[row]["time"];
                        }
                    }
                    catch (Exception)
                    { }
                    row++;
                    if (row >= dt.Rows.Count)
                    {
                        break;
                    }
                }
                if (row != 0)
                {
                    onTime = onTime + ((DateTime)dt.Rows[row - 1]["time"]).Subtract(tempDate);
                    dayTime = dayTime + ((DateTime)dt.Rows[row - 1]["time"]).Subtract(tempDate);
                }
                if (row >= dt.Rows.Count)
                {
                    break;
                }
            }
            km_table.Rows.Add(((DateTime)dt.Rows[row - 1]["time"]).Date.ToShortDateString(), onTime.Subtract(sum));
            lblKM.Text = "Total Working Time(HH:MM):" + onTime;
            lblKM.Visible = true;
            gv_Report.Width = Unit.Percentage(40);

        }
        else if (ignitionConnected == 0 && dt.Rows.Count > 1)
        {
            DateTime day = (DateTime)dt.Rows[0]["time"];
            while (row < dt.Rows.Count)
            {
                while (Convert.ToInt32(dt.Rows[row]["speed"]) <= 3)
                {
                    try
                    {
                        if (((DateTime)dt.Rows[row]["time"]).Date != ((DateTime)dt.Rows[row - 1]["time"]).Date)
                        {
                            //dayTime = dayTime + ((DateTime)dt.Rows[row - 1]["time"]).Subtract(tempDate);
                            km_table.Rows.Add(((DateTime)dt.Rows[row - 1]["time"]).Date.ToShortDateString(), dayTime);
                            sum = sum + dayTime;
                            dayTime = TimeSpan.Zero;
                            day = (DateTime)dt.Rows[row]["time"];
                        }
                    }
                    catch (Exception)
                    { }
                    row++;
                    if (row >= dt.Rows.Count)
                    {
                        break;
                    }
                }

                if (row >= dt.Rows.Count)
                {
                    break;
                }
                tempDate = (DateTime)dt.Rows[row]["time"];
                while (Convert.ToInt32(dt.Rows[row]["speed"]) > 3)
                {
                    try
                    {
                        if (((DateTime)dt.Rows[row]["time"]).Date != ((DateTime)dt.Rows[row - 1]["time"]).Date)
                        {
                            dayTime = dayTime + ((DateTime)dt.Rows[row - 1]["time"]).Subtract(tempDate);
                            km_table.Rows.Add(((DateTime)dt.Rows[row - 1]["time"]).Date.ToShortDateString(), dayTime);
                            sum = sum + dayTime;
                            dayTime = TimeSpan.Zero;
                            day = (DateTime)dt.Rows[row]["time"];
                        }
                    }
                    catch (Exception)
                    { }
                    row++;
                    if (row >= dt.Rows.Count)
                    {
                        break;
                    }
                }
                if (row != 0)
                {
                    onTime = onTime + ((DateTime)dt.Rows[row - 1]["time"]).Subtract(tempDate);
                    dayTime = dayTime + ((DateTime)dt.Rows[row - 1]["time"]).Subtract(tempDate);
                }
                if (row >= dt.Rows.Count)
                {
                    break;
                }
            }
            km_table.Rows.Add(((DateTime)dt.Rows[row - 1]["time"]).Date.ToShortDateString(), onTime.Subtract(sum));
            lblKM.Text = "Total Working Time(HH:MM)(approx):" + onTime;
            lblKM.Visible = true;
        }

        gv_Report.DataSource = km_table;
        ViewState["tbl_WorkTime"] = km_table;
        //gv_Report.DataBind();
    }
    protected void report_idleTime(cls_Reports rpt)
    {
        lblRptName.Text = "IDLE TIME REPORT FOR " + "'" + getSelectedCarrierName() + "'" + " " + "From" + " " + rpt.dateStart + " " + "To" + " " + rpt.dateEnd;
        btnshowchat.Visible = true;
        lblmsg.Text = "";
        DateTime startdt = Convert.ToDateTime(dateFrom.SelectedDate.ToString(), culturenew); // + " " + ddl_HH.SelectedItem.Text + ":" + ddl_MM.SelectedItem.Text + ":" + ddl_SS.SelectedItem.Text);
        DateTime enddt = Convert.ToDateTime(dateTo.SelectedDate.ToString(), culturenew); //+ " " + ddl_HH_end.SelectedItem.Text + ":" + ddl_MM_end.SelectedItem.Text + ":" + ddl_SS_end.SelectedItem.Text);            
        km_table.Columns.Add(new DataColumn("Date"));
        km_table.Columns.Add(new DataColumn("Idle Time (HH:MM)"));
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        int row = 0;
        TimeSpan offTime = TimeSpan.Zero;
        TimeSpan dayTime = TimeSpan.Zero;
        rpt.carrierId = Convert.ToInt32(getSelectedCarrierID());
        ds = rpt.fn_WorkTimeNew(rpt);
        Int16 ignitionConnected = Convert.ToInt16(ds.Tables[1].Rows[0]["digIgnitionUsed"]);
        dt = ds.Tables[0];
        DateTime tempDate;
        TimeSpan sum = TimeSpan.Zero;
        if (ignitionConnected == 1 && dt.Rows.Count > 1)
        {
            tempDate = (DateTime)dt.Rows[0]["time"];
            DateTime day = (DateTime)dt.Rows[0]["time"];
            while (row < dt.Rows.Count)
            {
                tempDate = (DateTime)dt.Rows[0]["time"];
                while (Convert.ToInt32(dt.Rows[row]["din1"]) == 0)
                {
                    try
                    {
                        if (((DateTime)dt.Rows[row]["time"]).Date != ((DateTime)dt.Rows[row - 1]["time"]).Date)
                        {
                            //dayTime = dayTime + ((DateTime)dt.Rows[row - 1]["time"]).Subtract(tempDate);
                            km_table.Rows.Add(((DateTime)dt.Rows[row - 1]["time"]).Date.ToShortDateString(), dayTime);
                            sum = sum + dayTime;
                            dayTime = TimeSpan.Zero;
                            day = (DateTime)dt.Rows[row]["time"];
                        }
                    }
                    catch (Exception)
                    { }
                    row++;
                    if (row >= dt.Rows.Count)
                    {
                        break;
                    }
                }
                if (row >= dt.Rows.Count)
                {
                    break;
                }
                tempDate = (DateTime)dt.Rows[0]["time"];
                while (Convert.ToInt32(dt.Rows[row]["din1"]) == 1 && Convert.ToInt32(dt.Rows[row]["speed"]) > 1)
                {
                    try
                    {
                        if (((DateTime)dt.Rows[row]["time"]).Date != ((DateTime)dt.Rows[row - 1]["time"]).Date)
                        {
                            //dayTime = dayTime + ((DateTime)dt.Rows[row - 1]["time"]).Subtract(tempDate);
                            km_table.Rows.Add(((DateTime)dt.Rows[row - 1]["time"]).Date.ToShortDateString(), dayTime);
                            sum = sum + dayTime;
                            dayTime = TimeSpan.Zero;
                            day = (DateTime)dt.Rows[row]["time"];
                        }
                    }
                    catch (Exception)
                    { }
                    row++;
                    if (row >= dt.Rows.Count)
                    {
                        break;
                    }
                }
                if (row >= dt.Rows.Count)
                {
                    break;
                }
                tempDate = (DateTime)dt.Rows[row]["time"];
                while (Convert.ToInt32(dt.Rows[row]["din1"]) == 1 && Convert.ToInt32(dt.Rows[row]["speed"]) <= 1)
                {
                    try
                    {
                        if (((DateTime)dt.Rows[row]["time"]).Date != ((DateTime)dt.Rows[row - 1]["time"]).Date)
                        {
                            dayTime = dayTime + ((DateTime)dt.Rows[row - 1]["time"]).Subtract(tempDate);
                            km_table.Rows.Add(((DateTime)dt.Rows[row - 1]["time"]).Date.ToShortDateString(), dayTime);
                            sum = sum + dayTime;
                            dayTime = TimeSpan.Zero;
                            day = (DateTime)dt.Rows[row]["time"];
                        }
                    }
                    catch (Exception)
                    { }
                    row++;
                    if (row >= dt.Rows.Count)
                    {
                        break;
                    }
                }
                if (row != 0)
                {
                    offTime = offTime + ((DateTime)dt.Rows[row - 1]["time"]).Subtract(tempDate);
                    dayTime = dayTime + ((DateTime)dt.Rows[row - 1]["time"]).Subtract(tempDate);
                }
                if (row >= dt.Rows.Count)
                {
                    break;
                }
            }
            km_table.Rows.Add(((DateTime)dt.Rows[row - 1]["time"]).Date.ToShortDateString(), offTime.Subtract(sum));
            lblKM.Text = "Total Idle Time(HH:MM):" + offTime;
            lblKM.Visible = true;
            gv_Report.DataSource = km_table;
            ViewState["tbl_IdleTime"] = km_table;
            // gv_Report.DataBind();

        }
        else if (ignitionConnected == 0 && dt.Rows.Count > 1)
        {
            lblKM.Text = "Digital Ignition not used for this device.\nReport NA to this device..";
            lblKM.Visible = true;
        }
        gv_Report.Width = Unit.Percentage(30);
    }
    protected void report_activity(cls_Reports rpt)
    {
        rpt.carrierId = Convert.ToInt32(getSelectedCarrierID());
        lblmsg.Text = "";
        lblRptName.Text = lblRptName.Text = "DETAILED ACTIVITY REPORT FOR " + "'" + getSelectedCarrierName() + "'" + " " + "From" + " " + rpt.dateStart + " " + "To" + " " + rpt.dateEnd;
        DataSet ds = rpt.activity(rpt);
        ds.Tables[0].Columns.RemoveAt(1);
        ds.Tables[0].Columns.RemoveAt(0);
        gv_Report.DataSource = ds.Tables[0];
        ViewState["tbl_activity"] = gv_Report.DataSource;
        gv_Report.Width = Unit.Percentage(100);
    }
    protected void report_speed(cls_Reports rpt)
    {
        rpt.carrierId = Convert.ToInt32(getSelectedCarrierID());
        lblmsg.Text = "";
        lblRptName.Text = lblRptName.Text = "SPEED REPORT FOR " + "'" + getSelectedCarrierName() + "'" + " " + "From" + " " + rpt.dateStart + " " + "To" + " " + rpt.dateEnd;
        rpt.speedAbove = Convert.ToInt32(txt_boldStop.Text);
        DataSet ds = new DataSet();
        ds = rpt.speed(rpt);
        gv_Report.DataSource = ds.Tables[0];
        ViewState["tbl_speed"] = ds;
        gv_Report.Width = Unit.Percentage(30);
    }
    protected void report_overSpeed(cls_Reports rpt)
    {
        rpt.carrierId = Convert.ToInt32(getSelectedCarrierID());
        lblmsg.Text = "";
        lblRptName.Text = lblRptName.Text = "OVER SPEEDING REPORT FOR " + "'" + getSelectedCarrierName() + "'" + " " + "From" + " " + rpt.dateStart + " " + "To" + " " + rpt.dateEnd;
        DataSet ds = new DataSet();
        ds = rpt.overSpeed(rpt);
        gv_Report.DataSource = ds.Tables[0];
        ViewState["tbl_overSpeed"] = gv_Report.DataSource;
        gv_Report.Width = Unit.Percentage(40);
    }
    protected void report_geofence(cls_Reports rpt)
    {
        lblRptName.Text = lblRptName.Text = "GEOFENCING REPORT FOR " + "'" + getSelectedCarrierName() + "'" + " " + "From" + " " + rpt.dateStart + " " + "To" + " " + rpt.dateEnd;
        double lat, lon;
        int fenceId, fenceType;
        string fenceName;
        Boolean status = false;
        Point[] pt = new Point[20];
        Point p1 = new Point();
        Point p2 = new Point();
        double radius, dlat, dlon, lat1, lat2, a, c, result, xint = 0;
        int R;
        DateTime inTime;

        DataTable report = new DataTable();
        report.Columns.Add(new DataColumn("GeoFence"));
        report.Columns.Add(new DataColumn("In at"));
        report.Columns.Add(new DataColumn("Out at"));
        report.Columns.Add(new DataColumn("Duration In"));

        DataTable dt = new DataTable();
        DataTable mts = new DataTable();
        cls_GeoFencing geo = new cls_GeoFencing();
        rpt.carrierId = Convert.ToInt32(getSelectedCarrierID());
        dt = geo.fn_FetchCircularGeo(rpt.carrierId, 1);
        dt.Columns.Add("visitFlag");
        dt.Columns["visitFlag"].DefaultValue = false;
        if (dt.Rows.Count > 0)
        {
            mts = rpt.fn_geoFencing(rpt);
            inTime = DateTime.Now;

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                fenceId = (int)dt.Rows[k]["fenceId"];
                fenceName = dt.Rows[k]["fenceName"].ToString();
                fenceType = (int)dt.Rows[k]["fenceType"];
                lat = lon = radius = 0;
                status = false;
                lat = (double)dt.Rows[k]["centerLat"];
                lon = (double)dt.Rows[k]["centerLong"];
                radius = (double)dt.Rows[k]["fenceRadius"];
                status = false;
                dt.Rows[k]["visitFlag"] = false;
                bool flag = false;
                for (int m = 0; m < mts.Rows.Count; m++)
                {

                    R = 6731;
                    dlat = lat - (double)mts.Rows[m]["latitude"];
                    dlat = ConvertToRadians(dlat);
                    dlon = ConvertToRadians(lon - (double)mts.Rows[m]["longitude"]);
                    lat1 = ConvertToRadians((double)mts.Rows[m]["latitude"]);
                    lat2 = ConvertToRadians(lat);

                    a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) + Math.Sin(dlon / 2) * Math.Sin(dlon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
                    c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                    result = R * c;
                    if (result <= radius)     //in geo
                    {
                        if (status == false)
                        {
                            inTime = (DateTime)mts.Rows[m]["time"];
                            flag = true;
                            status = true;
                            dt.Rows[k]["visitFlag"] = true;
                        }
                    }
                    else                        //out geo
                    {
                        if (status == true)
                        {
                            TimeSpan duration = (DateTime)mts.Rows[m]["time"] - inTime;
                            report.Rows.Add(fenceName, inTime, (DateTime)mts.Rows[m]["time"], duration);
                            flag = false;
                        }
                        status = false;
                    }
                }
                if (flag)
                {
                    report.Rows.Add(fenceName, inTime, "-", "-");
                }
            }
            ChartSeries series = new ChartSeries("Geo pie", ChartSeriesType.Pie);

            gv_Report.DataSource = report;
            //gv_Report.DataBind();
            gv_Report.Visible = true;
            ViewState["tbl_geofencing"] = gv_Report.DataSource;
            string name = string.Empty;
            int nameCnt = 0;
            int n = 0;

            for (int l = 0; l < dt.Rows.Count; l++)
            {
                if (Convert.ToBoolean(dt.Rows[l]["visitFlag"]) == false)
                {
                    lblGeoVisit.Visible = true;
                    lblGeoVisit.Text = lblGeoVisit.Text + dt.Rows[l]["fenceName"].ToString() + ", ";
                }
            }
            if (lblGeoVisit.Text.Length > 2)
            {
                lblGeoVisit.Text = lblGeoVisit.Text.Substring(0, lblGeoVisit.Text.Length - 2);
            }
            while (n < report.Rows.Count)
            {
                nameCnt = 0;
                name = report.Rows[n]["GeoFence"].ToString();
                if (name == report.Rows[n]["GeoFence"].ToString())
                {
                    while (name == report.Rows[n]["GeoFence"].ToString())
                    {
                        nameCnt++;
                        n++;
                        if (n >= report.Rows.Count)
                            break;
                    }
                    series.AddItem(nameCnt, name + "(" + nameCnt + ")");

                    //n++;
                }
            }
            pieChart.Clear();
            pieChart.AddChartSeries(series);
            pieChart.Visible = true;
        }
        else
        {
            pieChart.Visible = false;
            lblmsg.Visible = true;
            lblmsg.Text = "No records found";
        }
        gv_Report.Width = Unit.Percentage(80);
    }

    class Fence
    {
        public string fenceName;
        public int fenceId, fenceType;
        public double lat, lon;
        public DateTime time;
    }
    protected void report_geofenceKmTravelled(cls_Reports rpt)
    {
        lblRptName.Text = lblRptName.Text = "KM Travelled between Geofence's REPORT FOR " + "'" + getSelectedCarrierName() + "'" + " " + "From" + " " + rpt.dateStart + " " + "To" + " " + rpt.dateEnd;
        Fence fence = new Fence();
        Fence fenceFrom = new Fence();
        // Fence fenceTo = null;
        Boolean marking = false;
        // Point[] pt = new Point[20];
        // Point p1 = new Point();
        //Point p2 = new Point();
        double radius, dlat, dlon, lat1, lat2, a, c, result;
        int R;

        double km = 0;
        DataTable report = new DataTable();
        report.Columns.Add(new DataColumn("GeoFence from"));
        report.Columns.Add(new DataColumn("Geofence To"));
        report.Columns.Add(new DataColumn("KM"));

        DataTable dt = new DataTable();
        DataTable mts = new DataTable();
        cls_GeoFencing geo = new cls_GeoFencing();
        rpt.carrierId = Convert.ToInt32(getSelectedCarrierID());
        dt = geo.fn_FetchCircularGeo(rpt.carrierId, 1);
        dt.Columns.Add("visitFlag");
        dt.Columns["visitFlag"].DefaultValue = false;
        if (dt.Rows.Count > 0)
        {
            mts = rpt.fn_geoFencing(rpt);

            for (int m = 0; m < mts.Rows.Count; m++)
            {
                if (marking == true)
                {
                    km = km + CalcDistanceBetween((float)Convert.ToDouble(mts.Rows[m - 1]["latitude"]), (float)Convert.ToDouble(mts.Rows[m - 1]["longitude"]), (float)Convert.ToDouble(mts.Rows[m]["latitude"]), (float)Convert.ToDouble(mts.Rows[m]["longitude"]));
                }
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    fence.fenceId = (int)dt.Rows[k]["fenceId"];
                    fence.fenceName = dt.Rows[k]["fenceName"].ToString();
                    fence.fenceType = (int)dt.Rows[k]["fenceType"];
                    fence.lat = (double)dt.Rows[k]["centerLat"];
                    fence.lon = (double)dt.Rows[k]["centerLong"];
                    radius = (double)dt.Rows[k]["fenceRadius"];
                    fence.time = (DateTime)mts.Rows[m]["time"];

                    R = 6731;
                    dlat = fence.lat - (double)mts.Rows[m]["latitude"];
                    dlat = ConvertToRadians(dlat);
                    dlon = ConvertToRadians(fence.lon - (double)mts.Rows[m]["longitude"]);
                    lat1 = ConvertToRadians((double)mts.Rows[m]["latitude"]);
                    lat2 = ConvertToRadians(fence.lat);

                    a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) + Math.Sin(dlon / 2) * Math.Sin(dlon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
                    c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                    result = R * c;

                    if (result <= radius)     //in geo
                    {
                        if (marking == true)
                        {
                            if (fence.fenceId == fenceFrom.fenceId)
                            {
                                continue;
                            }
                            else
                            {
                                double addextra = (km / 100) * 5;
                                report.Rows.Add(fenceFrom.fenceName + "....(" + fenceFrom.time + ")", fence.fenceName + "....(" + fence.time + ")", Math.Round(km + addextra, 1));
                                km = 0;
                                fenceFrom.fenceId = fence.fenceId;
                                fenceFrom.fenceName = fence.fenceName;
                                fenceFrom.time = fence.time;
                            }
                        }
                        else
                        {

                            fenceFrom.fenceId = fence.fenceId;
                            fenceFrom.fenceName = fence.fenceName;
                            fenceFrom.time = (DateTime)mts.Rows[m]["time"];
                            marking = true;
                            km = 0;
                            break;
                        }
                    }
                    else                        //out geo
                    {

                    }
                }
            }

            gv_Report.DataSource = report;
            gv_Report.Visible = true;
            ViewState["tbl_geofencingkm"] = gv_Report.DataSource;
        }
        gv_Report.Width = Unit.Percentage(70);

    }

    protected void report_ignOnOff(cls_Reports rpt, string rptNAme)
    {
        string Gmapurl = "Click Here";
        lblmsg.Text = "";
        int row = 0;
        string LocationON, LocationOff, dateon, dateoff, workingtime;


        rpt.carrierId = Convert.ToInt32(getSelectedCarrierID());
        rpt.rpt_query = rptNAme;

        lblRptName.Text = "IGNITION ON OFF REPORT FOR " + "'" + getSelectedCarrierName() + "'" + " " + "From" + " " + rpt.dateStart + " " + "To" + " " + rpt.dateEnd;
        DataTable dt = new DataTable();
        dt = rpt.fn_Ignition_OnOff(rpt);


        if (dt.Rows.Count == 0)
        {
            lblmsg.Visible = true;
            lblmsg.Text = "No record found";
        }
        int srno = 1;
        int rowcount = dt.Rows.Count;
        DataTable table_getLocation = new DataTable();

        table_getLocation.Columns.Add(new DataColumn("Sr No"));
        table_getLocation.Columns.Add(new DataColumn("Date On"));

        table_getLocation.Columns.Add(new DataColumn("Get start Location"));
        table_getLocation.Columns.Add(new DataColumn("Date Off"));


        table_getLocation.Columns.Add(new DataColumn("Get end Location"));
        table_getLocation.Columns.Add(new DataColumn("Working Time In Minutes"));

        //table_getLocation.Columns.Add(new DataColumn("Location On"));
        //table_getLocation.Columns.Add(new DataColumn("Location Off"));

        laton = new string[rowcount];
        longon = new string[rowcount];
        latoff = new string[rowcount];
        longoff = new string[rowcount];

        while (row < rowcount)
        {
            dateon = dt.Rows[row]["dateOn"].ToString();
            dateoff = dt.Rows[row]["dateOff"].ToString();
            workingtime = dt.Rows[row]["WorkingTime"].ToString();

            if (dt.Rows[row]["latOn"].ToString() != "")
            {
                ViewState["laton"] = float.Parse(dt.Rows[row]["latOn"].ToString());
                laton[row] = dt.Rows[row]["latOn"].ToString();
            }
            else
            {
                ViewState["laton"] = "0.0";
                laton[row] = "0";
            }

            if (dt.Rows[row]["longon"].ToString() != "")
            {
                ViewState["longon"] = float.Parse(dt.Rows[row]["longOn"].ToString());
                longon[row] = dt.Rows[row]["longOn"].ToString();
            }
            else
            {
                ViewState["longon"] = "0.0";
                longon[row] = "0";

            }

            if (dt.Rows[row]["latOff"].ToString() != "")
            {
                ViewState["latoff"] = float.Parse(dt.Rows[row]["latOff"].ToString());
                latoff[row] = dt.Rows[row]["latOff"].ToString();

            }
            else
            {
                ViewState["latoff"] = "0.0";
                latoff[row] = "0";
            }

            if (dt.Rows[row]["longOff"].ToString() != "")
            {
                ViewState["longoff"] = float.Parse(dt.Rows[row]["longOff"].ToString());
                longoff[row] = dt.Rows[row]["longOff"].ToString();
            }
            else
            {
                ViewState["longoff"] = "0.0";
                longoff[row] = "0";
            }

            ViewState["latonarray"] = laton;
            ViewState["longonarray"] = longon;

            ViewState["latoffarray"] = latoff;
            ViewState["longoffarray"] = longoff;

            //getting location of laton &  longon
            GPSTrackingBLL.cls_Location obj = new cls_Location();
            //LocationON = obj.GetAddress(ViewState["laton"].ToString(), ViewState["longon"].ToString());
            // LocationOff = obj.GetAddress(ViewState["latoff"].ToString(), ViewState["longoff"].ToString());
            LocationON = ""; //obj.GetAddress(ViewState["laton"].ToString(), ViewState["longon"].ToString());
            LocationOff = "";// obj.GetAddress(ViewState["latoff"].ToString(), ViewState["longoff"].ToString());

            string locon = ViewState["laton"].ToString() + "," + ViewState["longon"].ToString();
            string locoff = ViewState["latoff"].ToString() + "," + ViewState["longoff"].ToString();

            if (String.IsNullOrEmpty(locon.ToString()) || String.IsNullOrEmpty(locoff.ToString()))
            {
                locon = "Location Not Available";
                locoff = "Location Not Available";


                table_getLocation.Rows.Add(srno, dateon, locon, dateoff, locoff, workingtime);
            }
            else
            {
                string ONlatlong = Gmapurl + locon;
                string OFFlatlong = Gmapurl + locoff;

                //table_getLocation.Rows.Add(srno,dateon,LocationON, LocationOff, dateoff, workingtime);
                table_getLocation.Rows.Add(srno, dateon, Gmapurl, dateoff, Gmapurl, workingtime);
                row++;
                srno++;
            }
        }

        ViewState["tbl_Ig"] = table_getLocation;
        gv_Report.DataSource = table_getLocation;// rpt.fn_FuelTemp(rpt);
        gv_Report.Width = Unit.Percentage(100);
    }
    protected void report_ignOnOffSeq(cls_Reports rpt, string rptNAme)
    {

        string Gmapurl = "Click Here";
        lblmsg.Text = "";
        int row = 0;
        string id, time, speed, din, Location;

        rpt.carrierId = Convert.ToInt32(getSelectedCarrierID());
        rpt.rpt_query = rptNAme;
        lblRptName.Text = "IGNITION ON OFF SEQUENCE REPORT FOR " + "'" + getSelectedCarrierName() + "'" + " " + "From" + " " + rpt.dateStart + " " + "To" + " " + rpt.dateEnd;

        DataTable dt = new DataTable();

        dt = rpt.fn_Ignition_onoffSqe(rpt);
        // ViewState["tbl_Igs"] = dt;

        // gv_Report.DataSource = dt;// rpt.fn_FuelTemp(rpt);

        int rowcount = dt.Rows.Count;
        DataTable table_getLocation = new DataTable();
        table_getLocation.Columns.Add(new DataColumn("Id"));
        table_getLocation.Columns.Add(new DataColumn("Get Location"));

        table_getLocation.Columns.Add(new DataColumn("Time"));
        //table_getLocation.Columns.Add(new DataColumn("speed"));
        table_getLocation.Columns.Add(new DataColumn("Ignition Status"));
        // table_getLocation.Columns.Add(new DataColumn("Get Location"));


        if (dt.Rows.Count == 0)
        {
            lblmsg.Visible = true;
            lblmsg.Text = "No record found";
        }

        laton = new string[rowcount];
        longon = new string[rowcount];



        int srNo = 1;
        while (row < rowcount)
        {
            id = srNo.ToString();// dt.Rows[row]["Id"].ToString();
            time = dt.Rows[row]["time"].ToString();
            speed = dt.Rows[row]["speed"].ToString();
            din = dt.Rows[row]["Din1"].ToString();

            if (dt.Rows[row]["latitude"].ToString() != "")
            {
                ViewState["latitude"] = float.Parse(dt.Rows[row]["latitude"].ToString());
                laton[row] = dt.Rows[row]["latitude"].ToString();
            }
            else
            {
                ViewState["latitude"] = "0.0";
                laton[row] = "0";
            }

            if (dt.Rows[row]["longitude"].ToString() != "")
            {
                ViewState["longitude"] = float.Parse(dt.Rows[row]["longitude"].ToString());
                longon[row] = dt.Rows[row]["longitude"].ToString();
            }
            else
            {
                ViewState["longitude"] = "0.0";
                longon[row] = "0";
            }



            //getting location of laton &  longon
            GPSTrackingBLL.cls_Location obj = new cls_Location();
            // Location = obj.GetAddress(ViewState["latitude"].ToString(), ViewState["longitude"].ToString());

            Location = Gmapurl + ViewState["latitude"].ToString() + "," + ViewState["longitude"].ToString();

            if (String.IsNullOrEmpty(Location.ToString()))
            { Location = "Location Not Available"; }

            table_getLocation.Rows.Add(id, Gmapurl, time, din);
            row++;
            srNo++;
        }

        ViewState["latonarray"] = laton;
        ViewState["longonarray"] = longon;
        ViewState["tbl_Igs"] = table_getLocation;

        gv_Report.DataSource = table_getLocation;// rpt.fn_FuelTemp(rpt);
        gv_Report.Width = Unit.Percentage(100);
    }
    protected void report_temprature(cls_Reports rpt)
    {

        btnshowchat.Visible = true;
        lblmsg.Text = "";
        rpt.carrierId = Convert.ToInt32(getSelectedCarrierID());
        lblRptName.Text = "TEMPERATURE REPORT FOR " + "'" + getSelectedCarrierName() + "'" + " " + "From" + " " + rpt.dateStart + " " + "To" + " " + rpt.dateEnd;
        DataTable dt = new DataTable();
        dt = rpt.fn_Temp(rpt);
        gv_Report.DataSource = dt;
        ViewState["table_temp"] = dt;
        if (dt.Rows.Count == 0)
        {
            lblmsg.Visible = true;
            lblmsg.Text = "No record found";
        }
        gv_Report.Width = Unit.Percentage(40);
    }
    protected void exportOptions_Click(object sender, EventArgs e)
    {
        Session["report"] = ViewState["tbl_KM"];
    }
    protected void btnshowchat_Click(object sender, EventArgs e)
    {
        try
        {

            if (dd_report.SelectedValue == "boldstop")
            {
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["table_DailySummary"];

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);


            }
            else if (dd_report.SelectedValue == "worktime")
            {
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["table_workingtime"];

                showchart("Work Time", "Date", "Working Time In HH:MM");
            }
            else if (dd_report.SelectedValue == "km")
            {
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["tbl_KM"];

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                showchart("Kilo Meter", "Date", "Kilo Meter");
            }
            else if (dd_report.SelectedValue == "kmTravld")
            {
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["tbl_KM"];

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                showchart("Kilo Meter", "Date", "Kilo Meter");

            }
            else if (dd_report.SelectedValue == "fuel")
            {
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["table_fuel"];

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                showchart("Fuel", "Date", "Fuel");

            }
            else if (dd_report.SelectedValue == "ignSeq")
            {
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["tbl_Igs"];

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
            }
            else if (dd_report.SelectedValue == "ignition")
            {
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["tbl_Ig"];

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
            }
            else if (dd_report.SelectedValue == "summarry")
            {
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["table_DailySummary"];

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

            }
            else if (dd_report.SelectedValue == "temp")
            {
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["table_temp"];

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                showchart("Temperature", "Date and Time", "Temperature");

            }
            else if (dd_report.SelectedValue == "speed")
            {
                DataTable dt = new DataTable();

                DataSet ds = (DataSet)ViewState["tbl_speed"];
                ds.Tables.Add(dt);
                showchart("Speed", "Date and Time", "Speed");

            }
            else if (dd_report.SelectedValue == "idle")
            {
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["table_workingtime"];

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                showchart("Idle Time", "Date", "Idle Time in HH:MM");

            }
            else if (dd_report.SelectedValue == "boldstop")
            {
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["table_blodstop"];

                // ConvertDataTableToXML(dt);
                showchart("Bold Stop", "Start Time", "Bold Stop");
            }
        }
        catch (Exception ex)
        {
            cls_fileHandling fh = new cls_fileHandling();
            fh.appendToFile("~/logs/error.txt", DateTime.Now + ": " + ex.Message);
            fh.appendToFile("~/logs/stackTrace.txt", DateTime.Now + ": " + ex.StackTrace);
        }
    }
    protected void dd_report_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblSpeed.Visible = false;
        txt_boldStop.Visible = false;
        lblKM.Visible = false;
        btnshowchat.Visible = false;
        btnexport.Visible = false;
        btnexport_pdf.Visible = false;
        lblKM.Visible = false;
        lblmsg.Visible = false;
        Chart1.Visible = false;
        Chart2.Visible = false;
        Chart3.Visible = false;

        if (dd_report.SelectedValue == "boldstop")
        {
            lblStopage.Visible = true;
            txt_boldStop.Visible = true;
        }
        else if (dd_report.SelectedValue == "speed")
        {
            lblSpeed.Visible = true;
            txt_boldStop.Visible = true;
        }
        else
        {
            lblStopage.Visible = false;
            txt_boldStop.Visible = false;
        }
        pieChart.Visible = false;

    }
    protected void showchart(string rpt, string x, string y)
    {
        //try
        //{

        if (dd_report.SelectedValue == "worktime" || dd_report.SelectedValue == "idle")
        {
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["table_workingtime"];

            Chart1.DataSource = dt;
            Chart1.Height = 400;
            Chart1.Width = 600;

            Chart1.Series[0].XValueMember = dt.Columns[0].ToString();
            Chart1.Series[0].YValueMembers = dt.Columns[1].ToString();

            //Chart1.Series.Add("Series2");
            Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;

            Chart1.Titles.Add("Title1");
            Chart1.Titles["Title1"].Text = rpt + " " + "Chart";
            Chart1.ChartAreas["ChartArea1"].AxisX.Title = x;
            Chart1.ChartAreas["ChartArea1"].AxisY.Title = y;
            Chart1.Visible = true;
        }
        else if (dd_report.SelectedValue == "fuel")
        {
            panel_linechart.Visible = true;
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["table_fuel"];


            Chart2.DataSource = dt;
            Chart2.Height = 400;
            Chart2.Width = 800;

            Chart2.Series[0].XValueMember = dt.Columns[0].ToString();
            Chart2.Series[0].YValueMembers = dt.Columns[1].ToString();
            Chart2.Series[0].Name = "Percentage";

            // Chart2.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            Chart2.ChartAreas["ChartArea1"].AxisY.Interval = 1;


            Chart2.Series[1].XValueMember = dt.Columns[0].ToString();
            Chart2.Series[1].YValueMembers = dt.Columns[2].ToString();
            Chart2.Series[1].Name = "Litres";

            Chart2.Titles.Add("Title1");
            Chart2.Titles["Title1"].Text = rpt + " " + "Chart";
            Chart2.ChartAreas["ChartArea1"].AxisX.Title = x;
            gv_Report.Visible = false;

        }
        else if (dd_report.SelectedValue == "kmTravld" || dd_report.SelectedValue == "kmTravld")
        {

            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["tbl_KM"];


            Chart1.DataSource = dt;
            Chart1.Height = 400;
            Chart1.Width = 600;

            Chart1.Series[0].XValueMember = dt.Columns[0].ToString();
            Chart1.Series[0].YValueMembers = dt.Columns[1].ToString();

            //Chart1.Series.Add("Series2");
            Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;

            Chart1.Titles.Add("Title1");
            Chart1.Titles["Title1"].Text = rpt + " " + "Chart";
            Chart1.ChartAreas["ChartArea1"].AxisX.Title = x;
            Chart1.ChartAreas["ChartArea1"].AxisY.Title = y;
            Chart1.Visible = true;
        }
        else if (dd_report.SelectedValue == "temp")
        {
            panel_temp.Visible = true;
            panel_linechart.Visible = false;

            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["table_temp"];
            panel_temp.Visible = true;
            Chart3.DataSource = dt;
            Chart3.Height = 400;
            Chart3.Width = 900;
            Chart3.Series[0].XValueMember = dt.Columns[0].ToString();
            Chart3.Series[0].YValueMembers = dt.Columns[1].ToString();
            Chart3.ChartAreas["ChartArea1"].AxisX.Interval = 0.2;
            Chart3.Titles.Add("Title1");
            Chart3.Titles["Title1"].Text = rpt + " " + "Chart";
            Chart3.ChartAreas["ChartArea1"].AxisX.Title = x;
            Chart3.ChartAreas["ChartArea1"].AxisY.Title = y;
        }


        else if (dd_report.SelectedValue == "speed")
        {
            panel_temp.Visible = true;
            panel_linechart.Visible = false;

            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["tbl_speed"];
            panel_temp.Visible = true;
            Chart3.DataSource = dt;
            Chart3.Height = 400;
            Chart3.Width = 1000;
            Chart3.Series[0].XValueMember = dt.Columns[0].ToString();
            Chart3.Series[0].YValueMembers = dt.Columns[4].ToString();
            Chart3.ChartAreas["ChartArea1"].AxisX.Interval = 0.2;
            Chart3.Titles.Add("Title1");
            Chart3.Titles["Title1"].Text = rpt + " " + "Chart";
            Chart3.ChartAreas["ChartArea1"].AxisX.Title = x;
            Chart3.ChartAreas["ChartArea1"].AxisY.Title = y;
        }

        UpdatePanelCharts.Update();
        UpdatePanelReportGrid.Update();

        //}
        //catch (Exception ex)
        //{

        //}
    }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           //protected void gridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    //try
    //    //{
    //    string Gmapurl = "http://maps.google.com/?q=";


    //    if (dd_report.SelectedValue == "ignition")
    //    {

    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            //Start Location
    //            var lnk = new HyperLink();
    //            lnk.ID = "lnkFolder";
    //            lnk.Text = e.Row.Cells[2].Text;
    //            lnk.Target = "_blank";

    //            string[] laton_new = (string[])ViewState["latonarray"];
    //            string[] longon_new = (string[])ViewState["longonarray"];
    //            string[] latoff_new = (string[])ViewState["latoffarray"];
    //            string[] longoff_new = (string[])ViewState["longoffarray"];

    //            if (laton_new[count].ToString() == "0" || longon_new[count].ToString() == "0")
    //            {
    //                lnk.Text = "Location not available";
    //            }
    //            else
    //            {
    //                lnk.NavigateUrl = Gmapurl + laton_new[count].ToString() + "," + longon_new[count].ToString();
    //            }
    //            e.Row.Cells[2].Controls.Add(lnk);


    //            // End Location
    //            var lnk1 = new HyperLink();
    //            lnk1.ID = "lnkFolder";
    //            lnk1.Text = e.Row.Cells[4].Text;
    //            lnk1.Target = "_blank";
    //            if (latoff_new[count].ToString() == "0" || longoff_new[count].ToString() == "0")
    //            {
    //                lnk1.Text = "Location not available";
    //            }
    //            else
    //            {
    //                lnk1.NavigateUrl = Gmapurl + latoff_new[count].ToString() + "," + longoff_new[count].ToString();
    //            }

    //            e.Row.Cells[4].Controls.Add(lnk1);
    //            count++;
    //        }
    //    }
    //    if (dd_report.SelectedValue == "ignSeq")
    //    {
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {

    //            //Changing ignition = 1 to ON and ignition = 0 to OFF
    //            if (e.Row.Cells[3].Text.Contains("1"))
    //            {
    //                e.Row.Cells[3].Text = "ON";
    //            }
    //            else
    //            {
    //                e.Row.Cells[3].Text = "OFF";
    //            }

    //            //Location Link
    //            var lnk = new HyperLink();
    //            lnk.ID = "lnkFolder";
    //            lnk.Text = e.Row.Cells[1].Text;
    //            lnk.Target = "_blank";

    //            string[] laton_new = (string[])ViewState["latonarray"];
    //            string[] longon_new = (string[])ViewState["longonarray"];

    //            if (laton_new[count].ToString() == "0" || longon_new[count].ToString() == "0")
    //            {
    //                lnk.Text = "Location not available";
    //            }
    //            else
    //            {
    //                lnk.NavigateUrl = Gmapurl + laton_new[count].ToString() + "," + longon_new[count].ToString();
    //            }

    //            e.Row.Cells[1].Controls.Add(lnk);
    //            count++;
    //        }

    //    }
    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //}
    //}

    //protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    //try
    //    //{
    //    if (dd_report.SelectedValue == "fuel")
    //    {
    //        gv_Report.DataSource = ViewState["table_fuel"];
    //        gv_Report.PageIndex = e.NewPageIndex;
    //        gv_Report.DataBind();
    //    }
    //    else if (dd_report.SelectedValue == "ignSeq")
    //    {
    //        gv_Report.DataSource = ViewState["tbl_Igs"];
    //        //gv_Report.CurrentPageIndex
    //        gv_Report.PageIndex = e.NewPageIndex;
    //        gv_Report.DataBind();
    //    }
    //    else if (dd_report.SelectedValue == "ignition")
    //    {
    //        gv_Report.DataSource = ViewState["tbl_Ig"];
    //        gv_Report.PageIndex = e.NewPageIndex;
    //        gv_Report.DataBind();

    //    }
    //    else if (dd_report.SelectedValue == "boldstop")
    //    {
    //        gv_Report.DataSource = ViewState["table_blodstop"];
    //        gv_Report.PageIndex = e.NewPageIndex;
    //        gv_Report.DataBind();

    //    }
    //    else if (dd_report.SelectedValue == "temp")
    //    {
    //        gv_Report.DataSource = ViewState["table_temp"];
    //        gv_Report.PageIndex = e.NewPageIndex;
    //        gv_Report.DataBind();
    //    }

    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //}
    //}
    protected void gv_Report_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //try
        //{

        if (dd_report.SelectedValue == "boldstop")
        {
            string currentCommand = e.CommandName;
            string currentRowIndex = e.CommandArgument.ToString();
            //string ProductID = GridView1.DataKeys[currentRowIndex].Value;
        }

        //}
        //catch (Exception ex)
        //{
        //}
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
                System.TimeSpan span = dt1.Subtract(dt);
                int days = span.Days;
                days = days + 1;
                if (days > 7)
                {
                    if (dd_report.SelectedValue == "geoFencing" || dd_report.SelectedValue == "kmTravld")
                    {
                        DateBoxError.Text = "";
                        return true;
                    }
                    lblmsg.Visible = true;
                    lblmsg.Text = "Select date range less than 8 days";
                    lblmsg.Text = "Tracking can not Support for more than 7 days" + "<br/>" + "Contact Support";
                    return false;
                }
                else
                {
                    DateBoxError.Text = "";
                    return true;
                }
            }
        }
        catch (Exception)
        {
            DateBoxError.Text = "Wrong dates Selected!!";
            return false;
        }
    }
   
    #endregion
    
    #endregion
    protected void gv_Report_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        gv_Report.DataSource = Session["report"];
        gv_Report.Rebind();// Session["report"]
    }
   

}