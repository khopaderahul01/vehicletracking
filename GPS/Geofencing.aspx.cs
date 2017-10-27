using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPSTrackingBLL;
using System.Data;


public partial class demo : System.Web.UI.Page
{
    string[] datalatarray;
    string[] datalongarray;
    string[] latarray;
    string[] longarray;
    DataTable geodata = new DataTable();
    int role, task;

    DataSet ds_geoareaList = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        if ((Session["role"] == null))
        {
            Response.Redirect("Default.aspx");
        }
        if (!IsPostBack)
        {
            role = Convert.ToInt32(Session["role"].ToString());          
            BindCarrierData();
            getcurrentlocload();
            getMapLoad(1);

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
    protected void getcurrentlocload()
    {
        try
        {
            DataSet ds = new System.Data.DataSet();
            cls_Carriers obj_carrier = new cls_Carriers();

            ds = obj_carrier.fn_CarrierLastLoc_FetchBY_CarrierID(Convert.ToInt32(ddlcarrier.SelectedItem.Value));

            ViewState["cu_lat"] = ds.Tables[0].Rows[0]["latitude"].ToString();
            ViewState["cu_long"] = ds.Tables[0].Rows[0]["longitude"].ToString();
            ViewState["cu_speed"] = ds.Tables[0].Rows[0]["speed"].ToString();

           
        }
        catch (Exception ex)
        {
        }
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
    protected void BindCarrierData()
    {
        try
        {
            cls_Carriers obj_carrier = new cls_Carriers();

            DataSet ds = new DataSet();
            ds = obj_carrier.fn_CarrierLastLoc_Fetch(Convert.ToInt32(Session["role"].ToString()), Convert.ToInt32(Session["fk_CompanyID"].ToString()), Convert.ToInt32(Session["fk_OrgID"].ToString()));
            ddlcarrier.DataSource = ds;
            ddlcarrier.DataTextField = "CarrierName";
            ddlcarrier.DataValueField = "CarrierId";
            ddlcarrier.DataBind();

            int count = ds.Tables[0].Rows.Count;
            int val = Convert.ToInt32(ds.Tables[0].Rows[0]["carrierId"].ToString());

        }
        catch (Exception ex)
        {
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lblmsg.Text = "";
            geodata.Columns.Add("Latitude");
            geodata.Columns.Add("Longtitude");
            geodata.Columns.Add("DateTime");

            getMapLoad(3);

            cls_Reports rpt = new cls_Reports();
            DataTable dt_latlong = new DataTable();
            string timestart = "";

            rpt.carrierId = Convert.ToInt32(ddlcarrier.SelectedItem.Value);
            rpt.dateStart = txtDateStart.Text + " " + ddl_HH.SelectedItem.Text + ":" + ddl_MM.SelectedItem.Text + ":" + "00";// txtTimeStart.Text;
            rpt.dateEnd = txtDateEnd.Text + " " + ddl_HH_end.SelectedItem.Text + ":" + ddl_MM_end.SelectedItem.Text + ":" + "00"; // txtTimeEnd.Text;


            if (ddl_HH_end.SelectedItem.Text + ":" + ddl_MM_end.SelectedItem.Text == "24:00")
            {
                timestart = "23:59:59";
                rpt.dateEnd = txtDateEnd.Text + " " + timestart;
            }

            dt_latlong = rpt.rpt_prc_Geo_getdata(rpt);
            int rowcount = dt_latlong.Rows.Count;
            datalatarray = new string[rowcount];
            datalongarray = new string[rowcount];

            DateTime date = new DateTime();

            for (int i = 0; i < rowcount; i++)
            {
                datalatarray[i] = dt_latlong.Rows[i]["lat"].ToString();
                datalongarray[i] = dt_latlong.Rows[i]["long"].ToString();
                date = Convert.ToDateTime(dt_latlong.Rows[i]["DateT"].ToString());
                checkGeoArea(datalatarray[i].ToString(), datalongarray[i].ToString(), date, 1);
            }

            if (lblmsg.Text != ddlcarrier.SelectedItem.Text + " " + "Carrier not in geofencing Area")
            {
                 int row = 0;
                 TimeSpan span;
                 TimeSpan tot = new TimeSpan(0, 0, 0);
                 string date1, date2;
                 DataTable dt = new DataTable();
                 dt = (DataTable)ViewState["tbl_geodata"];
                    //addition of timr
                    while (row < dt.Rows.Count)
                    {
                        date1 = dt.Rows[row]["DateTime"].ToString();
                        row++;
                        if (row < dt.Rows.Count)
                        {
                            date2 = dt.Rows[row]["DateTime"].ToString();
                            span = Convert.ToDateTime(date2).Subtract(Convert.ToDateTime(date1));
                            tot = tot + span;
                        }
                    }

           
                    lblmsg.Text = ddlcarrier.SelectedItem.Text + " " + "Carrier is in geofencing Area" + " " + "For selected Date and Time Span for" + " " + tot;

               }
            Panel_geodata.Visible = true;
            gv_geodata.DataSource = geodata;
            gv_geodata.DataBind();
            ViewState["tbl_data"] = geodata;

        }
        catch (Exception ex)
        {
        }
    }
    protected void get_latlongrange()
    {
        try
        {
            int counter = 0;
            string st_lat = txtlat.Text;
            string st_lng = txtlong.Text;


            string[] words = st_lat.Split(',');
            foreach (string word in words)
            {
                counter++;
            }
            ViewState["cnt"] = counter;
            // lat array
            int i = 0;
            latarray = new string[counter];
            string[] lat = st_lat.Split(',');

            foreach (string word1 in lat)
            {
                latarray[i] = word1.ToString();
                i++;
                counter++;
            }
            ViewState["lat1"] = latarray[0];
            //Long array
            counter = 0;
            int j = 0;
            longarray = new string[Convert.ToInt32(ViewState["cnt"])];
            string[] lng = st_lng.Split(',');

            foreach (string word1 in lng)
            {
                longarray[j] = word1.ToString();
                j++;
                counter++;
            }
            ViewState["long1"] = longarray[0];


        }
        catch (Exception ex)
        {
        }
    }
    protected void checkGeoArea(string lat, string lng, DateTime date, int task)
    {

        try
        {
            lblmsg.Text = "";
            //double meter = CalcDistanceBetween(float.Parse(ViewState["lat1"].ToString()), float.Parse(ViewState["long2"].ToString()),(float.Parse(ViewState["Centerlat"].ToString())), float.Parse(ViewState["centerlng"].ToString()));
            //double KM= (meter/1000);
            double KMstable = CalcDistanceBetween((float.Parse(ViewState["Centerlat"].ToString())), float.Parse(ViewState["centerlng"].ToString()), float.Parse(ViewState["lat1"].ToString()), float.Parse(ViewState["long1"].ToString()));
            double KMnew = CalcDistanceBetween((float.Parse(ViewState["Centerlat"].ToString())), float.Parse(ViewState["centerlng"].ToString()), float.Parse(lat), float.Parse(lng));
            double currkem = Convert.ToDouble(ViewState["Centerradius"].ToString());

            if (KMnew <= KMstable)
            {
                if (task == 1)
                {
                    geodata.Rows.Add(lat, lng, date);
                    ViewState["tbl_geodata"] = geodata;
                }
                this.ClientScript.RegisterStartupScript(this.GetType(), "Some Title", "<script language=\"javaScript\">" + "alert('Carrier is in geofencing Area');" + "<" + "/script>");
                lblmsg.Text = ddlcarrier.SelectedItem.Text + " " + "Carrier is in geofencing Area";
              
            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "Some Title", "<script language=\"javaScript\">" + "alert('Carrier not in geofencing Area');" + "<" + "/script>");
                lblmsg.Text = ddlcarrier.SelectedItem.Text + " " + "Carrier not in geofencing Area";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void insideGeoArea()
    {
        try
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), "Some Title", "<script language=\"javaScript\">" + "alert('Carrier inside geofencing Area');" + "<" + "/script>");

            lblmsg.Text = ddlcarrier.SelectedItem.Text + " " + "Carrier inside  geofencing Area" + " " + "For selected Date and Time Span";
        }
        catch (Exception ex)
        {
        }
    }

    protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {

            gv_geodata.DataSource = ViewState["tbl_data"];
            gv_geodata.PageIndex = e.NewPageIndex;
            gv_geodata.DataBind();

        }
        catch (Exception ex)
        {
        }
    }
    protected void ddlcarrier_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getcurrentlocload();
            getMapLoad(1);
        }
        catch (Exception ex)
        {
        }
    }
    protected void lbtngeo_Click(object sender, EventArgs e)
    {
        try
        {
            getMapLoad(2);
        }
        catch (Exception ex)
        {
        }
    }

    protected void getMapLoad(int task)
    {
        try
        {
            if (task == 1)
            {
                cls_Carriers obj_carrier = new cls_Carriers();
                int carrier = Convert.ToInt32(ddlcarrier.SelectedItem.Value);

                DataTable dt = new DataTable();
                dt = obj_carrier.fn_FetchGofenceData(carrier, 1);

                Double lat = 0;
                Double lng = 0;

                int radius = 0;

                lat = Convert.ToDouble(dt.Rows[0]["latitude"].ToString());
                ViewState["Centerlat"] = lat;

                lng = Convert.ToDouble(dt.Rows[0]["longitude"].ToString());
                ViewState["centerlng"] = lng;

                radius = 1;
                ViewState["Centerradius"] = radius;


                Page.ClientScript.RegisterStartupScript(Page.GetType(), "MyKey", "initialize('" + lat + "', '" + lng + "', '" + radius + "','" + ViewState["cu_lat"].ToString() + "','" + ViewState["cu_long"].ToString() + "','" + ViewState["cu_speed"] + "','"+ddlcarrier.SelectedItem.Text+"');", true);
                //panel_geo.Visible = true;

                GeoareaList();
            
            }
            else if (task == 2)
            {
                DataSet ds = new System.Data.DataSet();
                cls_Carriers obj_carrier = new cls_Carriers();
                ds = obj_carrier.fn_CarrierLastLoc_FetchBY_CarrierID(Convert.ToInt32(ddlcarrier.SelectedItem.Value));

                string lat = ds.Tables[0].Rows[0]["latitude"].ToString();
                string lng = ds.Tables[0].Rows[0]["longitude"].ToString();

                get_latlongrange();

                int i = 0;
                string newcenter = txtcenter.Text;
                string[] newcenterarray = newcenter.Split(',', '(', ')');

                foreach (string word1 in newcenterarray)
                {
                    newcenterarray[i] = word1.ToString();
                    i++;

                }
                string newcenterlat = newcenterarray[1].ToString();
                string newcenterlong = newcenterarray[2].ToString();
                ViewState["Centerlat"] = newcenterlat;
                ViewState["centerlng"] = newcenterlong;

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "MyKey", "initialize('" + newcenterlat + "', '" + newcenterlong + "', '" + txtredius.Text + "','" + ViewState["cu_lat"].ToString() + "','" + ViewState["cu_long"].ToString() + "','" + ViewState["cu_speed"] + "','" + ddlcarrier.SelectedItem.Text + "');", true);


                checkGeoArea(lat, lng, DateTime.Now, 0);
            }
            else if (task == 3)
            {
                DataSet ds = new System.Data.DataSet();
                cls_Carriers obj_carrier = new cls_Carriers();
                ds = obj_carrier.fn_CarrierLastLoc_FetchBY_CarrierID(Convert.ToInt32(ddlcarrier.SelectedItem.Value));

                string lat = ds.Tables[0].Rows[0]["latitude"].ToString();
                string lng = ds.Tables[0].Rows[0]["longitude"].ToString();

                get_latlongrange();

                int i = 0;
                string newcenter = txtcenter.Text;
                string[] newcenterarray = newcenter.Split(',', '(', ')');

                foreach (string word1 in newcenterarray)
                {
                    newcenterarray[i] = word1.ToString();
                    i++;
                }
                string newcenterlat = newcenterarray[1].ToString();
                string newcenterlong = newcenterarray[2].ToString();
                ViewState["Centerlat"] = newcenterlat;
                ViewState["centerlng"] = newcenterlong;

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "MyKey", "initialize('" + newcenterlat + "', '" + newcenterlong + "', '" + txtredius.Text + "','" + ViewState["cu_lat"].ToString() + "','" + ViewState["cu_long"].ToString() + "','" + ViewState["cu_speed"] + "','" + ddlcarrier.SelectedItem.Text + "');", true);
            }
        }
        catch (Exception)
        {
        }
    }

    protected bool datevalidate()
    {
        if (txtenddt.Text != "")
        {
            DateTime dt = Convert.ToDateTime(txtstdt.Text);// txtTimeStart.Text;
            DateTime dt1 = Convert.ToDateTime(txtenddt.Text);
            if (dt > dt1)
            {
                lblmsg.Visible = true;

                lblerrormsg.Text = "Please select valid date range";
                return false;
            }
            else
            {
                return true;
            }
        }
        else { return true; }

    }

    protected void btnsavegeo_Click(object sender, EventArgs e)
    {
        if (txtgeoname.Text == "")
        {
            rfvgeoname.Visible = true;
            return;
        }
        rfvgeoname.Visible = false;
        try
        {
            lblerrormsg.Text = "";
            bool value = datevalidate();
            if (value == true)
            {

                int i = 0;
                string newcenter = txtcenter.Text;
                string[] newcenterarray = newcenter.Split(',', '(', ')');

                foreach (string word1 in newcenterarray)
                {
                    newcenterarray[i] = word1.ToString();
                    i++;

                }
                string newcenterlat = newcenterarray[1].ToString();
                string newcenterlong = newcenterarray[2].ToString();

                cls_GeoFencing obj_geo = new cls_GeoFencing();



                obj_geo.AreaName = txtgeoname.Text;
                obj_geo.IsCircularGF = 1;
                obj_geo.GeoLatitude1 = float.Parse(newcenterlat);
                obj_geo.GeoLongitude1 = float.Parse(newcenterlong);
                obj_geo.Redius = float.Parse(txtredius.Text);
                obj_geo.CarrierID = Convert.ToInt32(ddlcarrier.SelectedItem.Value);

                if (btnsavegeo.Text == "Save Geofence Area")
                {
                    obj_geo.GeofenceId = 0;
                    obj_geo.task = 1;
                }
                else
                {
                    obj_geo.GeofenceId = Convert.ToInt32(ViewState["fid"].ToString());
                    obj_geo.task = 2;
                }

                obj_geo.alertst_time = txtstdt.Text + " " + ddl_HH_geo.SelectedItem.Text + ":" + ddl_MM_geo.SelectedItem.Text + ":" + "00"; // txtTimeStart.Text;;
                obj_geo.alertend_time = txtenddt.Text + " "+ ddl_HH_end .SelectedItem.Text + ":"+ ddl_MM_end.SelectedItem.Text +":"+"00"; // txtTimeEnd.Text;;


                int geo_id = obj_geo.fn_relCarrierGeofence_manage(obj_geo);

                if (btnsavegeo.Text == "Save Geofence Area")
                {
                    this.ClientScript.RegisterStartupScript(this.GetType(), "Some Title", "<script language=\"javaScript\">" + "alert('GeoFence Saved successfully ');" + "<" + "/script>");
                }
                else
                {
                    this.ClientScript.RegisterStartupScript(this.GetType(), "Some Title", "<script language=\"javaScript\">" + "alert('GeoFence Updated successfully ');" + "<" + "/script>");
                    btnsavegeo.Text = "Save Geofence Area";
                }
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "MyKey", "initialize('" + newcenterlat + "', '" + newcenterlong + "', '" + txtredius.Text + "','" + ViewState["cu_lat"].ToString() + "','" + ViewState["cu_long"].ToString() + "','" + ViewState["cu_speed"] + "','" + ddlcarrier.SelectedItem.Text + "');", true);
                GeoareaList();
            }
            
        }
        catch (Exception ex)
        {
        }
    }

    //function for fetching geo area data & assign it to gv_groareaList
    protected void GeoareaList()
    {
        try
        {
            cls_GeoFencing obj_geo = new cls_GeoFencing();
            ds_geoareaList = obj_geo.fn_FetchCiruclarGEo(Convert.ToInt32(ddlcarrier.SelectedItem.Value),1); // task 1 for circular geofence
            gv_geoareaList.DataSource = ds_geoareaList;
            gv_geoareaList.DataBind();
            ViewState["ds_geo"] = ds_geoareaList;
            
                
        }
        catch (Exception ex)
        {
        }
    }

    protected void gv_geoareaList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            lblerrormsg.Text = "";
            DataSet ds = new DataSet();
            cls_GeoFencing obj_geo = new cls_GeoFencing();

            Control ctl = e.CommandSource as Control;
            GridViewRow CurrentRow = ctl.NamingContainer as GridViewRow;
            int row = CurrentRow.RowIndex;
            object objTemp = gv_geoareaList.DataKeys[CurrentRow.RowIndex].Value as object;
            ds = obj_geo.fn_FetchCiruclarGEopoint(Convert.ToInt32(ddlcarrier.SelectedItem.Value), 1, Convert.ToInt32 (objTemp));
            if (objTemp != null)
            {
                string geofId= objTemp.ToString();
                int fenceid = Convert.ToInt32(geofId);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "MyKey", "initialize('" + ds.Tables[0].Rows[0]["centerLat"] + "', '" + ds.Tables[0].Rows[0]["centerLong"] + "', '" + ds.Tables[0].Rows[0]["fenceRadius"] + "','" + ViewState["cu_lat"].ToString() + "','" + ViewState["cu_long"].ToString() + "','" + ViewState["cu_speed"] + "','" + ddlcarrier.SelectedItem.Text + "');", true);

            }
            txtgeoname.Text = ds.Tables[0].Rows[0]["fenceName"].ToString();

            if (e.CommandName == "editgeo")
            {
                btnsavegeo.Text = "Update Geofence Area";
                ViewState["fid"] = (e.CommandArgument);
                this.ClientScript.RegisterStartupScript(this.GetType(), "Some Title", "<script language=\"javaScript\">" + "alert('Please do necessary changes and click on update button ');" + "<" + "/script>");
                
            }
            else if (e.CommandName == "deleteGeo")
            {
                deletegeofence(Convert.ToInt32(e.CommandArgument));
                getcurrentlocload();
                getMapLoad(1);
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void deletegeofence(int geoid)
    {
        try
        {
            cls_GeoFencing obj_geo = new cls_GeoFencing();
            obj_geo.task = 3;
            obj_geo.GeofenceId = geoid;

            //send other field empty
               obj_geo.AreaName ="";
               obj_geo.IsCircularGF = 1;
               obj_geo.GeoLatitude1 = float.Parse("0.0");
               obj_geo.GeoLongitude1 =float.Parse("0.0");
               obj_geo.Redius = float.Parse("0.0");
               obj_geo.CarrierID = 0;
                
               obj_geo.alertst_time = "";
               obj_geo.alertend_time = "";

            obj_geo.fn_relCarrierGeofence_manage(obj_geo);

            GeoareaList();
        }
        catch (Exception ex)
        {
        }
    }

    protected void lbtnshowdata_Click(object sender, EventArgs e)
    {
        try
        {
            panel_geoareaList.Visible = true;
            GeoareaList();
            txtlat.Text = "";
            txtstdt.Text = "";
            txtenddt.Text = "";
            txtgeoname.Text = "";
            lblerrormsg.Text = "";

            getcurrentlocload();
            getMapLoad(1);

        }
        catch (Exception ex)
        {
        }
    }

    protected void lbtnclearmap_Click(object sender, EventArgs e)
    {
        txtlat.Text = "";
        txtstdt.Text = "";
        txtenddt.Text = "";
        txtgeoname.Text = "";
        lblerrormsg.Text = "";
        getMapLoad(1);
    }
}