using System;
using System.Web.UI.WebControls;
using GPSTrackingBLL;
using System.Data;
using System.Globalization;
using Artem.Google.UI;
using Telerik.Charting;
using Telerik.Web.UI;



public partial class multiReport : System.Web.UI.Page
{
    
    CultureInfo culturenew = new CultureInfo("en-US");
    DataTable km_table = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((Session["role"] == null))
        {
            Response.Redirect("Default.aspx");
        }
        if (!IsPostBack)
        {
            lblGeoVisitMultiple.Visible = false;
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
    protected void TimerListBox_Tick(object sender, EventArgs e)
    {
        bindControls();
        UpdatePanelCarListBox.Update();
        TimerListBox.Enabled = false;
    }
    #region reports
    #region Multiplereports
    protected void dd_reportMultiple_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblSpeedMultiple.Visible = false;
        txt_boldStopMultiple.Visible = false;
        lblKMMultiple.Visible = false;
        btnshowchatMultiple.Visible = false;
        btnexportMultiple.Visible = false;
        btnexport_pdfMultiple.Visible = false;
        lblKMMultiple.Visible = false;
        lblmsgMultiple.Visible = false;
        Chart1Multiple.Visible = false;
        Chart2Multiple.Visible = false;
        Chart3Multiple.Visible = false;

        if (dd_reportMultiple.SelectedValue == "boldstop")
        {
            lblStopageMultiple.Visible = true;
            txt_boldStopMultiple.Visible = true;
        }
        else if (dd_reportMultiple.SelectedValue == "speed")
        {
            lblSpeedMultiple.Visible = true;
            txt_boldStopMultiple.Visible = true;
        }
        else
        {
            lblStopageMultiple.Visible = false;
            txt_boldStopMultiple.Visible = false;
        }
        pieChartMultiple.Visible = false;

    }
    protected void btngenRptMultiple_Click(object sender, EventArgs e)
    {
        //try
        //{

        lblmsgMultiple.Text = "";
        bool valid = datevalidate();

        if (valid == true)
        {
            btnexportMultiple.Visible = true;
            btnexport_pdfMultiple.Visible = true;

            if (dd_reportMultiple.SelectedValue == "overSpeed")
            {
                Load_ReportMultiple("overSpeed", 1);
            }

            else if (dd_reportMultiple.SelectedValue == "kmTravld")
            {
                Load_ReportMultiple("rpt_prc_KM", 1);
            }
            else if (dd_reportMultiple.SelectedValue == "geoFencing")
            {
                Load_ReportMultiple("rpt_prc_geoFencing", 1);
            }
            else if (dd_reportMultiple.SelectedValue == "worktime")
            {
                Load_ReportMultiple("rpt_prc_WorkTime", 1);
            }
            else if (dd_reportMultiple.SelectedValue == "idle")
            {
                Load_ReportMultiple("rpt_prc_idle", 1);
            }
            else if (dd_reportMultiple.SelectedValue == "summarry")
            {
                Load_ReportMultiple("rpt_prc_Daily_Summary", 0);
            }
        }


        //}
        //catch (Exception ex)
        //{
        //}
    }
    private void Load_ReportMultiple(string rptNAme, int onOff)
    {
        //try
        //{

        cls_Reports rpt = new cls_Reports();
        Session["ExportFileNameMultiple"] = dd_reportMultiple.SelectedItem.Text;
        rpt.dateStart = dateFrom.SelectedDate.ToString();
        rpt.dateEnd = dateTo.SelectedDate.ToString();

        //if (rptNAme == "rpt_prc_geoFencing")
        //{
        //    report_geofence(rpt);
        //}
        //else if (rptNAme == "rpt_prc_Daily_Summary")
        //{
        //    report_summary(rpt);
        //}
        //else if (rptNAme == "rpt_prc_idle")
        //{
        //    report_idleTime(rpt);
        //}
        //else 
        if (rptNAme == "rpt_prc_KM")
        {
            report_kmTravelledMultiple(rpt);
        }
        //else if (rptNAme == "rpt_prc_WorkTime")
        //{
        //    report_workTime(rpt);
        //}

        Session["headerStringMultiple"] = lblRptNameMultiple.Text;

        gv_ReportMultiple.MasterTableView.FilterExpression = String.Empty;
        gv_ReportMultiple.MasterTableView.SortExpressions.Clear();
        gv_ReportMultiple.MasterTableView.GroupByExpressions.Clear();
        gv_ReportMultiple.MasterTableView.ClearSelectedItems();
        gv_ReportMultiple.MasterTableView.ClearEditItems();
        gv_ReportMultiple.DataBind();
        gv_ReportMultiple.Visible = true;
        Session["reportMultiple"] = gv_ReportMultiple.DataSource;

        //}

        //catch (Exception ex)
        //{

        //}

    }
    protected void report_kmTravelledMultiple(cls_Reports rpt)
    {
        DateTime startdt = Convert.ToDateTime(dateFrom.SelectedDate.ToString(), culturenew); // + " " + ddl_HH.SelectedItem.Text + ":" + ddl_MM.SelectedItem.Text + ":" + ddl_SS.SelectedItem.Text);
        DateTime enddt = Convert.ToDateTime(dateTo.SelectedDate.ToString(), culturenew); //+ " " + ddl_HH_end.SelectedItem.Text + ":" + ddl_MM_end.SelectedItem.Text + ":" + ddl_SS_end.SelectedItem.Text);

        btnshowchatMultiple.Visible = true;
        lblmsgMultiple.Text = "";
        try
        {
            lblRptNameMultiple.Text = "KILOMETER TRAVEL REPORT FOR SELECTED VEHICLES From" + " " + rpt.dateStart + " " + "To" + " " + rpt.dateEnd;
            km_table.Columns.Add(new DataColumn("Vehicles/Day"));
            DateTime tempstartDate = startdt;
            while (tempstartDate < enddt)
            {
                km_table.Columns.Add(new DataColumn(tempstartDate.Date.ToShortDateString()));
                tempstartDate = tempstartDate.AddDays(1);
            }
            km_table.Columns.Add("Total KMS");
            lblKM.Text = "";
            DataSet fleet = getSelectedVehicles();
            DataSet ds = rpt.fn_prc_KMtravelMultiple(rpt, fleet.Tables[0], fleet.Tables[1]);
            double km = 0;
            double dayKm = 0;
            if (ds.Tables[0].Rows.Count > 1)
            {
                DataTable carriers = ds.Tables[0].DefaultView.ToTable(true, "carrierFId", "carrierName");
                for (int i = 0; i < carriers.Rows.Count; i++)
                {
                    DataRow dr = km_table.NewRow();
                    tempstartDate = startdt;
                    DateTime tempEndDate = startdt.AddDays(1);
                    TimeSpan span = enddt - startdt;
                    if (span.Days > 1)
                    {
                        km = dayKm = calculateKm(ds.Tables[0].Select(string.Format("time > \'{0}\' and time<\'{1}\' and carrierFId={2}", tempstartDate, tempstartDate.AddDays(1).Date, carriers.Rows[i]["carrierFId"]), "time asc"));
                        dr[tempstartDate.ToShortDateString()] = Math.Round(dayKm, 0);
                        tempstartDate = tempstartDate.AddDays(1).Date;
                        while (tempstartDate < enddt)
                        {
                            dayKm = calculateKm(ds.Tables[0].Select(string.Format("time > \'{0}\' and time<\'{1}\'  and carrierFId={2}", tempstartDate, tempstartDate.AddDays(1).Date, carriers.Rows[i]["carrierFId"]), "time asc"));
                            dr[tempstartDate.ToShortDateString()] = Math.Round(dayKm, 0);
                            km = km + dayKm;
                            tempstartDate = tempstartDate.AddDays(1);
                        }
                    }
                    else
                    {
                        km = calculateKm(ds.Tables[0].Select(string.Format("time > \'{0}\' and time<\'{1}\' and carrierFId={2}", startdt, enddt, carriers.Rows[i]["carrierFId"]), "time asc"));
                        dr[startdt.ToShortDateString()] = Math.Round(km, 0);
                    }
                    dr["Vehicles/Day"] = carriers.Rows[i]["carrierName"];
                    dr["Total KMS"] = Math.Round(km, 0);
                    km_table.Rows.Add(dr);
                }
            }
            gv_ReportMultiple.DataSource = km_table;
            // gv_ReportMultiple.Rebind();
            ViewState["tbl_KMMultiple"] = km_table;
            //lblKM.Text = "Total Kilometer Travel is:" + " " + Math.Round(km, 0) + "  KM";
            //lblKM.Visible = true;                
        }
        catch (Exception e)
        {
            cls_fileHandling fh = new cls_fileHandling();
            fh.appendToFile("~/logs/error.txt", DateTime.Now + ": " + e.Message);
            fh.appendToFile("~/logs/stackTrace.txt", DateTime.Now + ": " + e.StackTrace);
        }

    }
    //protected DataRow generateDataRowKMTravelled()
    //{
    //    DateTime tempstartDate = startdt;
    //    DateTime tempEndDate = startdt.AddDays(1);
    //    TimeSpan span = enddt - startdt;
    //    if (span.Days > 1)
    //    {
    //        km = dayKm = calculateKm(ds.Tables[0].Select(string.Format("time > \'{0}\' and time<\'{1}\'", tempstartDate, tempstartDate.AddDays(1).Date), "time asc"));
    //        km_table.Rows.Add(tempstartDate.ToShortDateString(), Math.Round(dayKm, 2));
    //        tempstartDate = tempstartDate.AddDays(1).Date;
    //        while (tempstartDate < enddt)
    //        {
    //            dayKm = calculateKm(ds.Tables[0].Select(string.Format("time > \'{0}\' and time<\'{1}\'", tempstartDate, tempstartDate.AddDays(1).Date), "time asc"));
    //            km_table.Rows.Add(tempstartDate.ToShortDateString(), Math.Round(dayKm, 2));
    //            km = km + dayKm;
    //            tempstartDate = tempstartDate.AddDays(1);
    //        }
    //    }
    //    else
    //    {
    //        km = calculateKm(ds.Tables[0].Select(string.Format("time > \'{0}\' and time<\'{1}\'", startdt, enddt), "time asc"));
    //        km_table.Rows.Add(startdt.ToShortDateString(), Math.Round(km, 2));
    //    }                            
    //}
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
    protected double toRad(double Value)
    {
        /** Converts numeric degrees to radians */
        return Value * Math.PI / 180;
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
    protected void gv_Report_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        //gv_Report.DataSource = Session["report"];
        //gv_Report.Rebind();// Session["report"]
    }
   
}