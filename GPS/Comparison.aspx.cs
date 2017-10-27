using System;
using System.Data;
using GPSTrackingBLL;
using System.Threading;
using Telerik.Charting;







public partial class Comparison : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
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
    protected void upUpdatePanel_PreRender(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            if (Request["__EVENTTARGET"] == UpdatePanelCarListBox.ClientID && Request.Form["__EVENTARGUMENT"] == "aaaa")
            {
                bindControls();
                UpdatePanelCarListBox.Update();                
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
            
           // ds = obj_carrier.fn_CarrierLastLoc_Fetch(Convert.ToInt32(Session["role"].ToString()), Convert.ToInt32(Session["task"].ToString()));
            car_listbox.DataSource = ds.Tables[0];
            car_listbox.DataTextField = "carrierName";
            car_listbox.DataValueField = "carrierId";
            car_listbox.DataBind();
            car_listbox.SelectedIndex = 2;           
            count = ds.Tables[0].Rows.Count;
        }
        catch (Exception e)
        {
            cls_fileHandling fh = new cls_fileHandling();
            fh.appendToFile("~/logs/error.txt", DateTime.Now + ": " + e.Message);
            fh.appendToFile("~/logs/stackTrace.txt", DateTime.Now + ": " + e.StackTrace);
           
            
        }
    }
    protected void RadButton1_Click(object sender, EventArgs e)
    {
        fillKmChart(Convert.ToDateTime(dateFrom.SelectedDate.ToString()), Convert.ToDateTime(dateTo.SelectedDate.ToString()));
    }
    protected void TimerTickChartKm(object sender, EventArgs e)
    {
        
        TimerChartKm.Enabled = false;       
        
    }

    protected void fillKmChart(DateTime startdt, DateTime enddt)   
    {
        cls_Reports rpt=new cls_Reports();
        
        //try
        //{ 
            DataTable selectedVehiles = getSelectedVehicles();
            DataSet ds = rpt.GetKmReportForComparison(selectedVehiles, startdt, enddt);
            ChartTitle title = new ChartTitle();
            ChartKm.ChartTitle.TextBlock.Text = "KMs Travelled from: " + startdt.Date.ToShortDateString() + " to:" + enddt.Date.ToShortDateString();
              
            ChartKm.Appearance.BarOverlapPercent =0;
            //ChartKm.Appearance.BarWidthPercent = 80;
            ChartKm.PlotArea.Appearance.Dimensions.Margins.Left = Telerik.Charting.Styles.Unit.Percentage(0);
            ChartKm.PlotArea.Appearance.Dimensions.Margins.Bottom = Telerik.Charting.Styles.Unit.Percentage(0);
            ChartKm.PlotArea.Appearance.Dimensions.Margins.Right = Telerik.Charting.Styles.Unit.Percentage(0);

            ChartKm.PlotArea.XAxis.Clear();
            ChartKm.PlotArea.XAxis.AutoScale = true;
            ChartKm.SeriesOrientation=ChartSeriesOrientation.Horizontal;
            ChartKm.Series.Add(new ChartSeries("Carriers"));
            ChartKm.DataSource = ds.Tables[ds.Tables.Count - 1];
            ChartKm.Series.Clear();
            ChartKm.PlotArea.XAxis.DataLabelsColumn = "carrierName"; 
            

            //for (int x = 0; x < ds.Tables[ds.Tables.Count-1].Rows.Count; x ++)
            //{
            //    ChartSeries series = new ChartSeries(ds.Tables[ds.Tables.Count - 1].Rows[x]["carrierName"].ToString());
            //    ChartKm.AddChartSeries(series);
            //    ChartSeriesItem item = new ChartSeriesItem(Math.Round((double)ds.Tables[ds.Tables.Count - 1].Rows[x]["totalDistance"]));
            //    item.Name = ds.Tables[ds.Tables.Count - 1].Rows[x]["carrierName"].ToString();
            //    ChartKm.Series[x].Items.Add(item);
            //    ChartKm.serie
            //}
            ChartKm.DataBind();

            ChartkmDayWise.ChartTitle.TextBlock.Text = "DateWise KMs Travelled from: " + startdt.Date.ToShortDateString() + " to:" + enddt.Date.ToShortDateString();
            
            ChartkmDayWise.PlotArea.Appearance.Dimensions.Paddings.Left = Telerik.Charting.Styles.Unit.Percentage(0);
            ChartkmDayWise.PlotArea.Appearance.Dimensions.Paddings.Bottom = Telerik.Charting.Styles.Unit.Percentage(0);
            ChartkmDayWise.PlotArea.Appearance.Dimensions.Paddings.Right = Telerik.Charting.Styles.Unit.Percentage(0);

            ChartkmDayWise.Series.Clear();
            ChartkmDayWise.PlotArea.XAxis.Clear();
            ChartkmDayWise.PlotArea.XAxis.AutoScale = true;
            for (int x = 0; x < ds.Tables[1].Rows.Count; x++)
            {
                ChartkmDayWise.PlotArea.XAxis.AddItem(((DateTime)ds.Tables[1].Rows[x]["date"]).Date.ToShortDateString());
                ChartkmDayWise.PlotArea.XAxis.DataLabelsColumn = "date";
            }

            ChartSeries currentSeries = null;

            for (int x = 0; x < ds.Tables.Count-1; x=x+2)
            {
                currentSeries = ChartkmDayWise.CreateSeries(ds.Tables[x].Rows[0]["carrierName"].ToString(), System.Drawing.Color.Empty, System.Drawing.Color.Empty, ChartSeriesType.Line);
                currentSeries.Appearance.LabelAppearance.Shadow.Color = System.Drawing.Color.White; 
                
                //Border.Color = System.Drawing.Color.Black;
                

                //currentSeries.Appearance.ShowLabels = false;                              
            }
            int m = 0;
            for (int l = 1; l < ds.Tables.Count - 1; l = l + 2)
            {
                for (int x = 0; x < ds.Tables[1].Rows.Count; x++)
                {
                    ChartkmDayWise.Series[m].AddItem((double)(ds.Tables[l].Rows[x]["distance"]));
                    
                }
                m++;
            }

            //ChartkmDayWise.ChartTitle.TextBlock.Text = "KMs Travelled from: " + startdt.Date.ToShortDateString() + " to:" + enddt.Date.ToShortDateString();

            //ChartkmDayWise.Appearance.BarOverlapPercent = 0;
            
            //ChartkmDayWise.PlotArea.XAxis.Clear();
            //ChartkmDayWise.PlotArea.XAxis.AutoScale = true;
            //ChartkmDayWise.SeriesOrientation = ChartSeriesOrientation.Vertical;
            //ChartkmDayWise.Series.Add(new ChartSeries("Carriers"));
            //ChartkmDayWise.DataSource = ds.Tables[ds.Tables.Count - 1];
            //ChartkmDayWise.Series.Clear();
            //ChartkmDayWise.PlotArea.XAxis.DataLabelsColumn = "carrierName";
            //ChartkmDayWise.DataBind();



            UpdatePanelChartKm.Update();

        //}
        //catch (Exception e)
        //{
        //    cls_fileHandling fh = new cls_fileHandling();
        //    fh.appendToFile("~/logs/error.txt", DateTime.Now + ": " + e.Message);
        //    fh.appendToFile("~/logs/stackTrace.txt", DateTime.Now + ": " + e.StackTrace);
        //}

    }
    public DataTable getSelectedVehicles()
    {
        int[] arr;
        int[] carrierId = new int[200];
        string fleetString = string.Empty;       
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