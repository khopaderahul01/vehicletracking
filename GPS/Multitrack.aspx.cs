using System;
using System.Data;
using GPSTrackingBLL;





public partial class Multitrack : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        if ((Session["role"] == null))
        {
            Response.Redirect("Default.aspx");
        }
       
        double[] latitude = new double[2];
        latitude[0] = 22.2;

        if (!IsPostBack)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Map");
            dt.Columns.Add("ID").DataType = System.Type.GetType("System.Int32");
            dt.Columns.Add("date").DataType = System.Type.GetType("System.String");
            dt.Columns.Add("lat").DataType = System.Type.GetType("System.String");
            dt.Columns.Add("long").DataType = System.Type.GetType("System.String");
            
            
            dt.Rows.Add(1,-1,null,"","");
            dt.Rows.Add(2, -1, null, "", "");
            dt.Rows.Add(3, -1, null, "", "");
            dt.Rows.Add(4, -1, null, "", "");
            ViewState["mapState"] = dt;
            Map1.Closed = false;
            Map2.Closed = false;
            Map3.Closed = false;
            Map4.Closed = false;
        }
      //  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Tooltify", "initialize('" + 1 + "');initialize('" + 2 + "');initialize('" + 3 + "');initialize('" + 4 + "');", true);

       // Page.ClientScript.RegisterStartupScript(Page.GetType(), "MyKey", "initialize('" + 1 + "');initialize('" + 2 + "');initialize('" + 3 + "');initialize('" + 4 + "');", true);
        //System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanelCarListBox, UpdatePanelCarListBox.GetType(), "MyKey", "initialize('" + 1 + "');", true);
        //System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanelCarListBox, UpdatePanelCarListBox.GetType(), "MyKey1", "initialize('" + 2 + "');", true);
        //System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanelCarListBox, UpdatePanelCarListBox.GetType(), "MyKey2", "initialize('" + 3 + "');", true);
        //System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanelCarListBox, UpdatePanelCarListBox.GetType(), "MyKey3", "initialize('" + 4 + "');", true);
       
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
   
    #region Leftpanel
    
    protected void TimerListBox_Tick(object sender, EventArgs e)
    {
        bindControls();
        UpdatePanelCarListBox.Update();        
        TimerListBox.Enabled = false;
        Timer1.Enabled = true;
        
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
        DataSet ds = generateArrayIndex();        
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        dt.PrimaryKey = new System.Data.DataColumn[] { dt.Columns[0] };
        dt.Merge(ds.Tables[1]);
        
       // //ClientScript.RegisterArrayDeclaration("latitudeArr", "'22.0'");
       //// Page.ClientScript.RegisterStartupScript(Page.GetType(), "MyKey", "placeMarker('" + 20 + "', '" + latitude + "', '" + 74.0 + "', '" + 50 + "', '" + 15 + "');", true);
       // //System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanelCarListBox, UpdatePanelCarListBox.GetType(), "MyKey", "initialize('" + 1 + "');", true);
       // //System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanelCarListBox, UpdatePanelCarListBox.GetType(), "MyKey1", "initialize('" + 2 + "');", true);
       // //System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanelCarListBox, UpdatePanelCarListBox.GetType(), "MyKey2", "initialize('" + 3 + "');", true);
       // //System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanelCarListBox, UpdatePanelCarListBox.GetType(), "MyKey3", "initialize('" + 4 + "');", true);
       // System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanelCarListBox, UpdatePanelCarListBox.GetType(), "drop", "drop('"+1+"');", true);
        plotMaps(dt);
    }

    

    protected void RadReplay_Click(object sender, EventArgs e)
    {
        int[] arr;
        int carrierId = 0;

        arr = car_listbox.GetSelectedIndices();
        carrierId = Int32.Parse(car_listbox.Items[arr[0]].Value);
        double[] latitude = new double[2];
        latitude[0] = 22.2;
        ClientScript.RegisterArrayDeclaration("latitudeArr", "'22.0'");
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "MyKey", "placeMarker('" + 20 + "', '" + latitude + "', '" + 74.0 + "', '" + 50 + "', '" + 15 + "');", true);


    }
    protected void ListboxFleet_ItemCheck(object sender, Telerik.Web.UI.RadListBoxItemEventArgs e)
    {
        DataSet ds = generateArrayIndex();       
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        dt.PrimaryKey = new System.Data.DataColumn[] { dt.Columns[0] };
        dt.Merge(ds.Tables[1]);       
    }
    public DataSet generateArrayIndex()
    {
        int[] arr;
        
        int[] carrierId = new int[200];
        string fleetString = string.Empty;
        DataTable carrierdt = new DataTable();
        DataTable fleetdt = new DataTable();
        DataSet ds = new DataSet();
        carrierdt.Columns.Add("carrierId");
        fleetdt.Columns.Add("carrierId");
        arr = car_listbox.GetCheckedIndices();
        
        foreach (int index in arr)
        {
            carrierdt.Rows.Add(car_listbox.Items[index].Value);
        }
        cls_Carriers obj_carrier = new cls_Carriers();
        ds = obj_carrier.fn_fetchLastLocation(carrierdt, null);
        return ds;
    }
    #endregion

    void plotMaps(DataTable dt)
    {
        if (dt.Rows.Count > 4)
        {
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanelCarListBox, UpdatePanelCarListBox.GetType(), "showAlert", "showAlert('Maximum of 4 vehicles can be selected!!!');", true);
        }
        else
        {
            DataTable mapState = (DataTable)ViewState["mapState"];
            for (int i = 0; i < 4; i++)
            {
                int ID = int.Parse(mapState.Rows[i]["ID"].ToString());
                DataRow[] dr = dt.Select("CarrierId=" + ID);
                if (dr.Length <= 0 && ID != -1)
                {
                    hideMap(i + 1);
                    mapState.Rows[i]["ID"] = -1;
                }
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int carrierID = int.Parse(dt.Rows[i]["CarrierId"].ToString());
                DataRow[] dr = mapState.Select("ID=" + carrierID);
                if (dr.Length <= 0)
                {
                    dr = mapState.Select("ID=-1");
                    int map = int.Parse(dr[0]["Map"].ToString());
                    mapState.Rows[map - 1]["ID"] = carrierID;
                    firstPlot(dt.Rows[i], map, mapState);
                }
            }
        }
    }


    void firstPlot(DataRow dr, int map, DataTable mapState)
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
        string desc = "<strong><font color=\"red\">" + PlateNo + "</font></strong><font color=\"blue\">" + "&nbsp;&nbsp;&nbsp;Time : " + "</font>" + Date + "<font color=\"blue\">" + "&nbsp;&nbsp;&nbsp;Speed : " + "</font>" + Speed + "<font color=\"blue\">" + "&nbsp;&nbsp;&nbsp;Ignition : " + "</font>" + (D1 == "" ? "NA" : (D1 == "0" ? "OFF" : "ON")) + "&nbsp;&nbsp;&nbsp;<font color=\"blue\">Address : " + "</font>" +((address != "") ? address : "Not Availabe");
        string basePath = dr["carrierTypeFId"].ToString();
        string parameters = map + "','" + Info + "','" + PlateNo + "','" + lat + "','" + lng + "','" + desc + "','" + angle + "','" + Speed + "','" + basePath;
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanelCarListBox, UpdatePanelCarListBox.GetType(), "placeMarker", "placeMarker('" + parameters + "');", true);
        showMap(map, PlateNo);
        parameters = map + "','" + 17;
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanelCarListBox, UpdatePanelCarListBox.GetType(), "setZoom", "setZoom('" + parameters + "');", true);
        mapState.Rows[map - 1]["date"] = Date;
        mapState.Rows[map - 1]["lat"] = lat;
        mapState.Rows[map - 1]["long"] = lng;
        

    }

    void showMap(int count, string PlateNo)
    {
        switch (count)
        {
            case 1:
                Map1.Closed = true;
                Map1.Title = PlateNo;
                break;

            case 2:
                Map2.Closed = true;
                Map2.Title = PlateNo;
                break;

            case 3:
                Map3.Closed = true;
                Map3.Title = PlateNo;
                break;

            case 4:
                Map4.Closed = true;
                Map4.Title = PlateNo;
                break;
        }

    }
    void hideMap(int count)
    {
        switch (count)
        {
            case 1:
                Map1.Closed = false;
                break;

            case 2:
                Map2.Closed = false;
                break;

            case 3:
                Map3.Closed = false;
                break;

            case 4:
                Map4.Closed = false;
                break;
        }

    }

    void callScript(String funName, String para)
    {
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanelCarListBox, UpdatePanelCarListBox.GetType(), "drop", "drop('" + 1 + "');", true);
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {       
        cls_Carriers obj_carrier = new cls_Carriers();
        DataSet ds = obj_carrier.fn_fetchLastLocation(getCarrierDt(), null);
        PlotOnTimer(ds.Tables[0]);
     
    }

    void PlotOnTimer(DataTable dt)
    {
        DataTable mapState = (DataTable)ViewState["mapState"];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            int carrierID =  int.Parse(dt.Rows[i]["CarrierId"].ToString());
            DataRow[] dr=mapState.Select("ID="+carrierID);
            if (dr[0]["date"].ToString() != dt.Rows[i]["time"].ToString())
            {
                string lat, lng;
                lat = dt.Rows[i]["latitude"].ToString();
                lng = dt.Rows[i]["longitude"].ToString();
                string address = dt.Rows[i]["address"].ToString();
                string PlateNo = dt.Rows[i]["carrierName"].ToString();
                string Speed = dt.Rows[i]["speed"].ToString();
                string D1 = dt.Rows[i]["Din1"].ToString();
                string Date = dt.Rows[i]["time"].ToString();
                string angle = dt.Rows[i]["angle"].ToString();
                String Info = "Plate No : " + PlateNo + "</br>" + "Speed : " + "" + Speed
                    + "</br>" + "Digital Ignition : " + (D1 == "" ? "NA" : (D1 == "0" ? "OFF" : "ON")) + "</br>" + "Date & Time : " + Date + "</br>" + "Address : " + ((address != "") ? address : "Not Availabe");
                string basePath = dt.Rows[i]["carrierTypeFId"].ToString(); 
                int map = int.Parse(dr[0]["Map"].ToString());
                string desc = "<strong><font color=\"red\">" + PlateNo + "</font></strong><font color=\"blue\">" + "&nbsp;&nbsp;&nbsp;Time : " + "</font>" + Date + "<font color=\"blue\">" + "&nbsp;&nbsp;&nbsp;Speed : " + "</font>" + Speed + "<font color=\"blue\">" + "&nbsp;&nbsp;&nbsp;Ignition : " + "</font>" + (D1 == "" ? "NA" : (D1 == "0" ? "OFF" : "ON")) + "&nbsp;&nbsp;&nbsp;<font color=\"blue\">Address : " + "</font>" + ((address != "") ? address : "Not Availabe");
                string parameters = map + "','" + Info + "','" + PlateNo + "','" + lat + "','" + lng + "','" + desc + "','" + angle + "','" + Speed + "','" + basePath;
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "addMarker", "addMarker('" + parameters + "');", true);
                parameters = map + "','" + mapState.Rows[map - 1]["lat"].ToString() + "','" + mapState.Rows[map - 1]["long"].ToString() + "','" + lat + "','" + lng;
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "addLine", "addLine('" + parameters + "');", true);

                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "showAlert", "showAlert('yes!!!');", true);
               // showMap(map, PlateNo);
               // parameters = map + "','" + 17;
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanelCarListBox, UpdatePanelCarListBox.GetType(), "setZoom", "setZoom('" + parameters + "');", true);
                mapState.Rows[map - 1]["date"] = Date;
                mapState.Rows[map - 1]["lat"] = lat;
                mapState.Rows[map - 1]["long"] = lng;
            }
            else
            { 
            
            }
        }


       

    }
    DataTable getCarrierDt()
    {
        DataTable mapState = (DataTable)ViewState["mapState"];
        DataTable dt = new DataTable();
        DataRow[] dr=mapState.Select("ID<>-1");
        dt.Columns.Add("carrierId");
        for (int i = 0; i < dr.Length; i++)
        {
            dt.Rows.Add(dr[i]["ID"]);
        }
        return dt;
    }
    
}