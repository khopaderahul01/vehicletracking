<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" Inherits="demo" Codebehind="Geofencing.aspx.cs" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>



<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headerdata" Runat="Server">
    <script type="text/javascript" language="javascript">
        $(function () {
            $("#tabs").tabs({
        });
    });
	</script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentdata" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    

    <script src="http://maps.google.com/maps?file=api&v=3&key="
      type="text/javascript">
    </script>
    
   <script type="text/javascript">
       /* Developed by: Abhinay Rathore [web3o.blogspot.com] */
       //Global variables 
       var map;
       var bounds = new GLatLngBounds; //Circle Bounds 
      // var map_center = new GLatLng(38.903843, -94.680096);

       var Circle; //Circle object 
       var CirclePoints = []; //Circle drawing points 
       var CircleCenterMarker, CircleResizeMarker;
       var mymarker;
       var circle_moving = false; //To track Circle moving 
       var circle_resizing = false; //To track Circle resizing 
       var radius = 1; //1 km 
       var min_radius = 0.1; //0.5km 
       var max_radius = 15; //5km 

       //Circle Marker/Node icons 
       var redpin = new GIcon(); //Red Pushpin Icon 
       redpin.image = 'icons/icon1.png'; //"http://maps.google.com/mapfiles/ms/icons/red-pushpin.png";
       redpin.iconSize = new GSize(32, 32);
       redpin.iconAnchor = new GPoint(10, 32);
       var bluepin = new GIcon(); //Blue Pushpin Icon 
       bluepin.image = 'icons/icon1.png'; // "http://maps.google.com/mapfiles/ms/icons/blue-pushpin.png";
       bluepin.iconSize = new GSize(32, 32);
       bluepin.iconAnchor = new GPoint(10, 32);

       var imageUrl = new GIcon(); //Blue Pushpin Icon 
       imageUrl.image = 'icons/car_icon3.png'; // "http://maps.google.com/mapfiles/ms/icons/blue-pushpin.png";
  
       imageUrl.iconAnchor = new GPoint(10, 32);

       var cu_lat, cu_long, cu_speed, cu_carriernm;

       function initialize(lt, lng, rds, latitude, longitude, speed, carriernm) { //Initialize Google Map 
           if (GBrowserIsCompatible()) {

               cu_lat = latitude;
               cu_long = longitude;
               cu_speed = speed
               cu_carriernm = carriernm

               radius = rds;
               var lat1 = lt;
               var lng1 = lng;
               var map_center = new GLatLng(lat1, lng1);
               map = new GMap2(document.getElementById("map_canvas")); //New GMap object 
               map.setCenter(map_center);

               var ui = new GMapUIOptions(); //Map UI options 
               ui.maptypes = { normal: true, satellite: true, hybrid: true, physical: false }
               ui.zoom = { scrollwheel: true, doubleclick: true };
               ui.controls = { largemapcontrol3d: true, maptypecontrol: true, scalecontrol: true };
               map.setUI(ui); //Set Map UI options 

               if (cu_speed > 2) {
                    imageUrl.image  = "icons/car_icon4.png";
               }
               else {
                    imageUrl.image  = "icons/car_icon3.png";
               }

                var map_center_new = new GLatLng(cu_lat, cu_long);
              

               addCircleCenterMarker(map_center);
               addCircleResizeMarker(map_center);
               drawCircle(map_center, radius);
               addmarker(map_center_new);

              
           }
       }

       function addmarker(point) {
         
           var markerOptions1 = { icon: imageUrl, draggable: true };
          
          // mymarker = new GMarker(point, markerOptions1);
          
           
          // map.addOverlay(mymarker); //Add marker on the map
         
       }

       // Adds Circle Center marker 
       function addCircleCenterMarker(point) {
         
          var markerOptions = { icon: bluepin, draggable: true };
           CircleCenterMarker = new GMarker(point, markerOptions);

           map.addOverlay(CircleCenterMarker); //Add marker on the map 
      

           GEvent.addListener(CircleCenterMarker, 'dragstart', function () { //Add drag start event 
               circle_moving = true;
           });
           GEvent.addListener(CircleCenterMarker, 'drag', function (point) { //Add drag event 
               drawCircle(point, radius);
           });
           GEvent.addListener(CircleCenterMarker, 'dragend', function (point) { //Add drag end event 
               circle_moving = false;
               drawCircle(point, radius);
           });
       }

       // Adds Circle Resize marker 
       function addCircleResizeMarker(point) {
         
           var resize_icon = new GIcon(redpin);
           resize_icon.maxHeight = 0;
           var markerOptions = { icon: resize_icon, draggable: true };
           CircleResizeMarker = new GMarker(point, markerOptions);
           map.addOverlay(CircleResizeMarker); //Add marker on the map 
        
           GEvent.addListener(CircleResizeMarker, 'dragstart', function () { //Add drag start event 
               circle_resizing = true;
           });
           GEvent.addListener(CircleResizeMarker, 'drag', function (point) { //Add drag event 
               var new_point = new GLatLng(map_center.lat(), point.lng()); //to keep resize marker on horizontal line 
               var new_radius = new_point.distanceFrom(map_center) / 1000; //calculate new radius 
               if (new_radius < min_radius) new_radius = min_radius;
               if (new_radius > max_radius) new_radius = max_radius;
               drawCircle(map_center, new_radius);
           });
           GEvent.addListener(CircleResizeMarker, 'dragend', function (point) { //Add drag end event 
               circle_resizing = false;
               var new_point = new GLatLng(map_center.lat(), point.lng()); //to keep resize marker on horizontal line 
               var new_radius = new_point.distanceFrom(map_center) / 1000; //calculate new radius 
               if (new_radius < min_radius) new_radius = min_radius;
               if (new_radius > max_radius) new_radius = max_radius;
               drawCircle(map_center, new_radius);
           });
       }

       //Draw Circle with given radius and center 
       function drawCircle(center, new_radius) {
           //Circle Drawing Algorithm from: http://koti.mbnet.fi/ojalesa/googlepages/circle.htm 

           //Number of nodes to form the circle 
           var nodes = new_radius * 40;
           if (new_radius < 1) nodes = 40;

           //calculating km/degree 
           var latConv = center.distanceFrom(new GLatLng(center.lat() + 0.1, center.lng())) / 100;
           var lngConv = center.distanceFrom(new GLatLng(center.lat(), center.lng() + 0.1)) / 100;

           CirclePoints = [];
           var step = parseInt(360 / nodes) || 10;
           var counter = 0;

           var lat = new Array(360); // regular array (add an optional integer
           var lng = new Array(360);
           var latdemo = 0;
           var longdemo = 0;

           for (var i = 0; i <= 360; i += step) {
               var cLat = center.lat() + (new_radius / latConv * Math.cos(i * Math.PI / 180));
               var cLng = center.lng() + (new_radius / lngConv * Math.sin(i * Math.PI / 180));

               lat[i] = cLat;
               lng[i] = cLng;
               latdemo = latdemo + ',' + cLat;
               longdemo = longdemo + ',' + cLng;
               var point = new GLatLng(cLat, cLng);
               CirclePoints.push(point);
               counter++;
           }

           for (var i = 0; i <= 1; i ++) {
               document.getElementById('<%=txtlat.ClientID %>').value = lat[0];
               document.getElementById('<%=txtlong.ClientID %>').value = lng[0];
           }

           CircleResizeMarker.setLatLng(CirclePoints[Math.floor(counter / 4)]); //place circle resize marker 
           CirclePoints.push(CirclePoints[0]); //close the circle polygon 
           if (Circle) { map.removeOverlay(Circle); } //Remove existing Circle from Map 
           var fillColor = (circle_resizing || circle_moving) ? 'black' : 'black'; //Set Circle Fill Color
           Circle = new GPolygon(CirclePoints, '#000000', 2, 1, fillColor, 0.2); //New GPolygon object for Circle 
           map.addOverlay(Circle); //Add Circle Overlay on the Map 
           radius = new_radius; //Set global radius 
           map_center = center; //Set global map_center 
           if (!circle_resizing && !circle_moving) { //Fit the circle if it is nor moving or resizing 
               fitCircle();
               //Circle drawing complete trigger function goes here
           }

           document.getElementById('<%=txtredius.ClientID %>').value = radius;
           
       }

       //Fits the Map to Circle bounds 
       function fitCircle() {
           bounds = Circle.getBounds();
           map.setCenter(bounds.getCenter(), map.getBoundsZoomLevel(bounds));
           document.getElementById('<%=txtcenter.ClientID %>').value = bounds.getCenter();
       } 
   </script>

   
       

 
<body> <%--<asp:TextBox ID="txtTimeStart" runat="server"></asp:TextBox>--%>
    <table id="tbl" cellpadding="0" cellspacing="0" width="100%">
    <tr>
    <td style="width:200px;vertical-align:top; float:left"> 
    <table>
    
    <tr><td> <asp:DropDownList AutoPostBack="true"  ID="ddlcarrier" runat="server" 
            onselectedindexchanged="ddlcarrier_SelectedIndexChanged"></asp:DropDownList><br /><br />
                                <asp:LinkButton ID="lbtnshowdata" runat="server" onclick="lbtnshowdata_Click">Show Geofence Area</asp:LinkButton><br /><br />
       <asp:LinkButton ID="lbtnclearmap" runat="server" onclick="lbtnclearmap_Click">Clear Map</asp:LinkButton><br /><br />
             <br/>
             <br />
             <asp:Panel ID="panel_geoareaList"  runat="server" Visible="false">
  <asp:GridView Width="150px" DataKeyNames="fenceId" AutoGenerateColumns="False"  
                     ID="gv_geoareaList" runat="server" 
                     onrowcommand="gv_geoareaList_RowCommand" CellPadding="4" ForeColor="#333333" 
                     GridLines="None">
      <AlternatingRowStyle BackColor="White" />
      <Columns>
          <asp:TemplateField>
              <ItemTemplate>
                  <asp:Button ID="btnEditOne" runat="server" 
                      CommandArgument='<%# Bind("fenceId") %>' CommandName="editgeo" 
                      CssClass="editOneRecord" style="float:left;" ToolTip="Edit" />
                  <asp:Button ID="btndeleteOne" runat="server" 
                      CommandArgument='<%# Bind("fenceId") %>' CommandName="deleteGeo" 
                      CssClass="deleteOneRecord" 
                      OnClientClick="return confirm('Do you want to delete this Geofence?');" 
                      style="float:left;" ToolTip="Delete" />
              </ItemTemplate>
              <ItemStyle Width="50px" />
          </asp:TemplateField>
          <asp:TemplateField HeaderText="Area Name">
              <EditItemTemplate>
                  <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("fenceName") %>'></asp:TextBox>
              </EditItemTemplate>
              <ItemTemplate>
                  <asp:LinkButton ID="LinkButton1" runat="server" Text='<%# Bind("fenceName") %>'></asp:LinkButton>
              </ItemTemplate>
              <HeaderStyle Height="10px" HorizontalAlign="Center" VerticalAlign="Middle" />
              <ItemStyle Height="30px" HorizontalAlign="Left" VerticalAlign="Middle" />
          </asp:TemplateField>
          <%-- <asp:HyperLinkField DataNavigateUrlFields="pk_opr"   DataTextField = "oprname"
                            DataNavigateUrlFormatString="OpprtunitiesDetails.aspx?pk_opr={0}" 
                            HeaderText="Oppertunity"  SortExpression="oprname"/>--%>
      </Columns>
      <EditRowStyle BackColor="#2461BF" />
      <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
      <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
      <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
      <RowStyle BackColor="#EFF3FB" />
      <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
      <SortedAscendingCellStyle BackColor="#F5F7FB" />
      <SortedAscendingHeaderStyle BackColor="#6D95E1" />
      <SortedDescendingCellStyle BackColor="#E9EBEF" />
      <SortedDescendingHeaderStyle BackColor="#4870BE" />
   </asp:GridView>
 </asp:Panel>

  <br/>
             <br />

 <asp:LinkButton ID="lbtngeo" runat="server" Text="Check Current carrier" Visible="false" onclick="lbtngeo_Click" ></asp:LinkButton>
         </td>
    </tr>
 <asp:Panel ID="panel_geo" Visible="false" runat="server">
            <tr>
            <td>
             <asp:Label ID="lblcarrierdata" runat="server" Text="Select date rage for getting carrierdata"></asp:Label>
            </td>
            </tr>
            <tr>
            <td style="width:15px">
                FROM:
            </td>
            </tr>
            <tr>
            <td>
                
                <asp:TextBox ID="txtDateStart" runat="server" style="float:left;" ></asp:TextBox>
               
                <asp:CalendarExtender  ID="txtExpectedDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                Format="MM/dd/yyyy" PopupButtonID="txtDateStart" TargetControlID="txtDateStart">
                </asp:CalendarExtender>

            </td>
            </tr> 
            <tr>
            <td style="vertical-align :middle;">
                <%--<asp:TextBox ID="txtTimeStart" runat="server"></asp:TextBox>--%>
            <span style=" padding-bottom :10px;"> <asp:Label ID="lblhh_start" runat="server" Text="HH:" ></asp:Label> </span> 
             <asp:DropDownList ID="ddl_HH"  runat="server" Width="50px" >
             <asp:ListItem>00</asp:ListItem>
                 <asp:ListItem>01</asp:ListItem>
                 <asp:ListItem>02</asp:ListItem>
                 <asp:ListItem>03</asp:ListItem>
                 <asp:ListItem>04</asp:ListItem>
                 <asp:ListItem>05</asp:ListItem>
                 <asp:ListItem>06</asp:ListItem>
                 <asp:ListItem>07</asp:ListItem>
                 <asp:ListItem>08</asp:ListItem>
                 <asp:ListItem>09</asp:ListItem>
                 <asp:ListItem>10</asp:ListItem>
                 <asp:ListItem>11</asp:ListItem>
                 <asp:ListItem>12</asp:ListItem>
                 <asp:ListItem>13</asp:ListItem>
                 <asp:ListItem>14</asp:ListItem>
                 <asp:ListItem>15</asp:ListItem>
                 <asp:ListItem>16</asp:ListItem>
                 <asp:ListItem>17</asp:ListItem>
                 <asp:ListItem>18</asp:ListItem>
                 <asp:ListItem>19</asp:ListItem>
                 <asp:ListItem>20</asp:ListItem>
                 <asp:ListItem>21</asp:ListItem>
                 <asp:ListItem>22</asp:ListItem>
                 <asp:ListItem>23</asp:ListItem>
                 <asp:ListItem>24</asp:ListItem>
                </asp:DropDownList>
             <asp:Label ID="lblmm_start" runat="server" Text="MM:" ></asp:Label> 
              <asp:DropDownList ID="ddl_MM"  runat="server" Width="50px" >
              <asp:ListItem>00</asp:ListItem>
             <asp:ListItem>01</asp:ListItem>
                 <asp:ListItem>02</asp:ListItem>
                 <asp:ListItem>03</asp:ListItem>
                 <asp:ListItem>04</asp:ListItem>
                 <asp:ListItem>05</asp:ListItem>
                 <asp:ListItem>06</asp:ListItem>
                 <asp:ListItem>07</asp:ListItem>
                 <asp:ListItem>08</asp:ListItem>
                 <asp:ListItem>09</asp:ListItem>
                 <asp:ListItem>10</asp:ListItem>
                 <asp:ListItem>11</asp:ListItem>
                 <asp:ListItem>12</asp:ListItem>
                 <asp:ListItem>13</asp:ListItem>
                 <asp:ListItem>14</asp:ListItem>
                 <asp:ListItem>15</asp:ListItem>
                 <asp:ListItem>16</asp:ListItem>
                 <asp:ListItem>17</asp:ListItem>
                 <asp:ListItem>18</asp:ListItem>
                 <asp:ListItem>19</asp:ListItem>
                 <asp:ListItem>20</asp:ListItem>
                 <asp:ListItem>21</asp:ListItem>
                 <asp:ListItem>22</asp:ListItem>
                 <asp:ListItem>23</asp:ListItem>
                 <asp:ListItem>24</asp:ListItem>

                 <asp:ListItem>24</asp:ListItem>
                 <asp:ListItem>25</asp:ListItem>
                 <asp:ListItem>26</asp:ListItem>
                <asp:ListItem>27</asp:ListItem>
                <asp:ListItem>28</asp:ListItem>
                <asp:ListItem>29</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
                <asp:ListItem>31</asp:ListItem>
                <asp:ListItem>32</asp:ListItem>

                <asp:ListItem>33</asp:ListItem>
                <asp:ListItem>34</asp:ListItem>
                <asp:ListItem>35</asp:ListItem>
                <asp:ListItem>36</asp:ListItem>
                <asp:ListItem>37</asp:ListItem>
                <asp:ListItem>38</asp:ListItem>
                <asp:ListItem>39</asp:ListItem>
                <asp:ListItem>40</asp:ListItem>
                <asp:ListItem>41</asp:ListItem>
                <asp:ListItem>42</asp:ListItem>
                <asp:ListItem>43</asp:ListItem>

                <asp:ListItem>44</asp:ListItem>
                <asp:ListItem>45</asp:ListItem>
                <asp:ListItem>46</asp:ListItem>
                <asp:ListItem>47</asp:ListItem>
                <asp:ListItem>48</asp:ListItem>
                <asp:ListItem>49</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
                <asp:ListItem>51</asp:ListItem>
                <asp:ListItem>52</asp:ListItem>
                <asp:ListItem>53</asp:ListItem>
                <asp:ListItem>54</asp:ListItem>
                <asp:ListItem>55</asp:ListItem>
                <asp:ListItem>56</asp:ListItem>
                <asp:ListItem>57</asp:ListItem>
                <asp:ListItem>58</asp:ListItem>
                <asp:ListItem>59</asp:ListItem>
              

             </asp:DropDownList>
             
              <%--<asp:Label ID="lblss_start" runat="server" Text="SS:" ></asp:Label> 
               <asp:DropDownList ID="ddl_SS"  runat="server" Width="50px">

                  <asp:ListItem>00</asp:ListItem>
                  <asp:ListItem>01</asp:ListItem>
                 <asp:ListItem>02</asp:ListItem>
                 <asp:ListItem>03</asp:ListItem>
                 <asp:ListItem>04</asp:ListItem>
                 <asp:ListItem>05</asp:ListItem>
                 <asp:ListItem>06</asp:ListItem>
                 <asp:ListItem>07</asp:ListItem>
                 <asp:ListItem>08</asp:ListItem>
                 <asp:ListItem>09</asp:ListItem>
                 <asp:ListItem>10</asp:ListItem>
                 <asp:ListItem>11</asp:ListItem>
                 <asp:ListItem>12</asp:ListItem>
                 <asp:ListItem>13</asp:ListItem>
                 <asp:ListItem>14</asp:ListItem>
                 <asp:ListItem>15</asp:ListItem>
                 <asp:ListItem>16</asp:ListItem>
                 <asp:ListItem>17</asp:ListItem>
                 <asp:ListItem>18</asp:ListItem>
                 <asp:ListItem>19</asp:ListItem>
                 <asp:ListItem>20</asp:ListItem>
                 <asp:ListItem>21</asp:ListItem>
                 <asp:ListItem>22</asp:ListItem>
                 <asp:ListItem>23</asp:ListItem>
                 <asp:ListItem>24</asp:ListItem>

                 <asp:ListItem>24</asp:ListItem>
                 <asp:ListItem>25</asp:ListItem>
                 <asp:ListItem>26</asp:ListItem>
                <asp:ListItem>27</asp:ListItem>
                <asp:ListItem>28</asp:ListItem>
                <asp:ListItem>29</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
                <asp:ListItem>31</asp:ListItem>
                <asp:ListItem>32</asp:ListItem>

                <asp:ListItem>33</asp:ListItem>
                <asp:ListItem>34</asp:ListItem>
                <asp:ListItem>35</asp:ListItem>
                <asp:ListItem>36</asp:ListItem>
                <asp:ListItem>37</asp:ListItem>
                <asp:ListItem>38</asp:ListItem>
                <asp:ListItem>39</asp:ListItem>
                <asp:ListItem>40</asp:ListItem>
                <asp:ListItem>41</asp:ListItem>
                <asp:ListItem>42</asp:ListItem>
                <asp:ListItem>43</asp:ListItem>

                <asp:ListItem>44</asp:ListItem>
                <asp:ListItem>45</asp:ListItem>
                <asp:ListItem>46</asp:ListItem>
                <asp:ListItem>47</asp:ListItem>
                <asp:ListItem>48</asp:ListItem>
                <asp:ListItem>49</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
                <asp:ListItem>51</asp:ListItem>
                <asp:ListItem>52</asp:ListItem>
                <asp:ListItem>53</asp:ListItem>
                <asp:ListItem>54</asp:ListItem>
                <asp:ListItem>55</asp:ListItem>
                <asp:ListItem>56</asp:ListItem>
                <asp:ListItem>57</asp:ListItem>
                <asp:ListItem>58</asp:ListItem>
                <asp:ListItem>59</asp:ListItem>
              
             </asp:DropDownList>--%>

            </td>
            </tr>
    <tr>
            <td>
                 <asp:Label ID="lblTo" runat="server" Text="TO" ></asp:Label></td>
    </tr>
    <tr>
            <td>
                <asp:TextBox ID="txtDateEnd" runat="server" style="float:left;"></asp:TextBox>
               
                 <asp:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                Format="MM/dd/yyyy" PopupButtonID="txtDateEnd" TargetControlID="txtDateEnd">
                            </asp:CalendarExtender>
            </td>
</tr>
<tr>
            <td>
                <%--<asp:TextBox ID="txtTimeEnd" runat="server"></asp:TextBox>--%>

                <asp:Label ID="lblhh_end" runat="server" Text="HH:" ></asp:Label>
             <asp:DropDownList ID="ddl_HH_end"  runat="server" Width="50px" >
             <asp:ListItem>00</asp:ListItem>
                 <asp:ListItem>01</asp:ListItem>
                 <asp:ListItem>02</asp:ListItem>
                 <asp:ListItem>03</asp:ListItem>
                 <asp:ListItem>04</asp:ListItem>
                 <asp:ListItem>05</asp:ListItem>
                 <asp:ListItem>06</asp:ListItem>
                 <asp:ListItem>07</asp:ListItem>
                 <asp:ListItem>08</asp:ListItem>
                 <asp:ListItem>09</asp:ListItem>
                 <asp:ListItem>10</asp:ListItem>
                 <asp:ListItem>11</asp:ListItem>
                 <asp:ListItem>12</asp:ListItem>
                 <asp:ListItem>13</asp:ListItem>
                 <asp:ListItem>14</asp:ListItem>
                 <asp:ListItem>15</asp:ListItem>
                 <asp:ListItem>16</asp:ListItem>
                 <asp:ListItem>17</asp:ListItem>
                 <asp:ListItem>18</asp:ListItem>
                 <asp:ListItem>19</asp:ListItem>
                 <asp:ListItem>20</asp:ListItem>
                 <asp:ListItem>21</asp:ListItem>
                 <asp:ListItem>22</asp:ListItem>
                 <asp:ListItem>23</asp:ListItem>
                 <asp:ListItem>24</asp:ListItem>
                </asp:DropDownList>

                <asp:Label ID="lblmm_end" runat="server" Text="MM:" ></asp:Label>
              <asp:DropDownList ID="ddl_MM_end"  runat="server" Width="50px" >
              <asp:ListItem>00</asp:ListItem>
             <asp:ListItem>01</asp:ListItem>
                 <asp:ListItem>02</asp:ListItem>
                 <asp:ListItem>03</asp:ListItem>
                 <asp:ListItem>04</asp:ListItem>
                 <asp:ListItem>05</asp:ListItem>
                 <asp:ListItem>06</asp:ListItem>
                 <asp:ListItem>07</asp:ListItem>
                 <asp:ListItem>08</asp:ListItem>
                 <asp:ListItem>09</asp:ListItem>
                 <asp:ListItem>10</asp:ListItem>
                 <asp:ListItem>11</asp:ListItem>
                 <asp:ListItem>12</asp:ListItem>
                 <asp:ListItem>13</asp:ListItem>
                 <asp:ListItem>14</asp:ListItem>
                 <asp:ListItem>15</asp:ListItem>
                 <asp:ListItem>16</asp:ListItem>
                 <asp:ListItem>17</asp:ListItem>
                 <asp:ListItem>18</asp:ListItem>
                 <asp:ListItem>19</asp:ListItem>
                 <asp:ListItem>20</asp:ListItem>
                 <asp:ListItem>21</asp:ListItem>
                 <asp:ListItem>22</asp:ListItem>
                 <asp:ListItem>23</asp:ListItem>
                 <asp:ListItem>24</asp:ListItem>

                 <asp:ListItem>24</asp:ListItem>
                 <asp:ListItem>25</asp:ListItem>
                 <asp:ListItem>26</asp:ListItem>
                <asp:ListItem>27</asp:ListItem>
                <asp:ListItem>28</asp:ListItem>
                <asp:ListItem>29</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
                <asp:ListItem>31</asp:ListItem>
                <asp:ListItem>32</asp:ListItem>

                <asp:ListItem>33</asp:ListItem>
                <asp:ListItem>34</asp:ListItem>
                <asp:ListItem>35</asp:ListItem>
                <asp:ListItem>36</asp:ListItem>
                <asp:ListItem>37</asp:ListItem>
                <asp:ListItem>38</asp:ListItem>
                <asp:ListItem>39</asp:ListItem>
                <asp:ListItem>40</asp:ListItem>
                <asp:ListItem>41</asp:ListItem>
                <asp:ListItem>42</asp:ListItem>
                <asp:ListItem>43</asp:ListItem>

                <asp:ListItem>44</asp:ListItem>
                <asp:ListItem>45</asp:ListItem>
                <asp:ListItem>46</asp:ListItem>
                <asp:ListItem>47</asp:ListItem>
                <asp:ListItem>48</asp:ListItem>
                <asp:ListItem>49</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
                <asp:ListItem>51</asp:ListItem>
                <asp:ListItem>52</asp:ListItem>
                <asp:ListItem>53</asp:ListItem>
                <asp:ListItem>54</asp:ListItem>
                <asp:ListItem>55</asp:ListItem>
                <asp:ListItem>56</asp:ListItem>
                <asp:ListItem>57</asp:ListItem>
                <asp:ListItem>58</asp:ListItem>
                <asp:ListItem>59</asp:ListItem>
               
             </asp:DropDownList>
             
        <%--    <asp:Label ID="lblss_end" runat="server" Text="SS:" ></asp:Label>
              <asp:DropDownList ID="ddl_SS_end"  runat="server" Width="50px">

                  <asp:ListItem>00</asp:ListItem>
                  <asp:ListItem>01</asp:ListItem>
                 <asp:ListItem>02</asp:ListItem>
                 <asp:ListItem>03</asp:ListItem>
                 <asp:ListItem>04</asp:ListItem>
                 <asp:ListItem>05</asp:ListItem>
                 <asp:ListItem>06</asp:ListItem>
                 <asp:ListItem>07</asp:ListItem>
                 <asp:ListItem>08</asp:ListItem>
                 <asp:ListItem>09</asp:ListItem>
                 <asp:ListItem>10</asp:ListItem>
                 <asp:ListItem>11</asp:ListItem>
                 <asp:ListItem>12</asp:ListItem>
                 <asp:ListItem>13</asp:ListItem>
                 <asp:ListItem>14</asp:ListItem>
                 <asp:ListItem>15</asp:ListItem>
                 <asp:ListItem>16</asp:ListItem>
                 <asp:ListItem>17</asp:ListItem>
                 <asp:ListItem>18</asp:ListItem>
                 <asp:ListItem>19</asp:ListItem>
                 <asp:ListItem>20</asp:ListItem>
                 <asp:ListItem>21</asp:ListItem>
                 <asp:ListItem>22</asp:ListItem>
                 <asp:ListItem>23</asp:ListItem>
                 <asp:ListItem>24</asp:ListItem>

                 <asp:ListItem>24</asp:ListItem>
                 <asp:ListItem>25</asp:ListItem>
                 <asp:ListItem>26</asp:ListItem>
                <asp:ListItem>27</asp:ListItem>
                <asp:ListItem>28</asp:ListItem>
                <asp:ListItem>29</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
                <asp:ListItem>31</asp:ListItem>
                <asp:ListItem>32</asp:ListItem>

                <asp:ListItem>33</asp:ListItem>
                <asp:ListItem>34</asp:ListItem>
                <asp:ListItem>35</asp:ListItem>
                <asp:ListItem>36</asp:ListItem>
                <asp:ListItem>37</asp:ListItem>
                <asp:ListItem>38</asp:ListItem>
                <asp:ListItem>39</asp:ListItem>
                <asp:ListItem>40</asp:ListItem>
                <asp:ListItem>41</asp:ListItem>
                <asp:ListItem>42</asp:ListItem>
                <asp:ListItem>43</asp:ListItem>

                <asp:ListItem>44</asp:ListItem>
                <asp:ListItem>45</asp:ListItem>
                <asp:ListItem>46</asp:ListItem>
                <asp:ListItem>47</asp:ListItem>
                <asp:ListItem>48</asp:ListItem>
                <asp:ListItem>49</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
                <asp:ListItem>51</asp:ListItem>
                <asp:ListItem>52</asp:ListItem>
                <asp:ListItem>53</asp:ListItem>
                <asp:ListItem>54</asp:ListItem>
                <asp:ListItem>55</asp:ListItem>
                <asp:ListItem>56</asp:ListItem>
                <asp:ListItem>57</asp:ListItem>
                <asp:ListItem>58</asp:ListItem>
                <asp:ListItem>59</asp:ListItem>
               
             </asp:DropDownList>--%>
            </td>
            
        </tr>
    <tr><td> <asp:Button id="btnSubmit" Text="Submit" runat="server"  onclick="btnSubmit_Click" /> </td></tr>
    <tr><td><asp:Label ID="lblmsg" runat="server" Text=""></asp:Label></td></tr>
 </asp:Panel>
 
    </table>
        
   </td>
    <td>
        
        <asp:Panel ID="panel_map" runat="server" >
        <table>
        <tr>
        <td> Latitude:</td>
        <td><asp:TextBox ID="txtlat"  runat="server" ></asp:TextBox></td>
        <td> Longitude:</td>
        <td><asp:TextBox ID="txtlong"  runat="server"></asp:TextBox></td>
        </tr>

        <tr>
        <td>CenterLatLong:</td>
        <td><asp:TextBox ID="txtcenter"  runat="server"></asp:TextBox><br /></td>
        <td> radius:</td>
        <td><asp:TextBox ID="txtredius"  runat="server"></asp:TextBox></td>
        
        </tr>

         <tr>
        <td> Enter Goefence area Name:</td>
        <td><asp:TextBox ID="txtgeoname"  runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvgeoname" runat="server" 
                ControlToValidate="txtgeoname" ErrorMessage="Please enter Geofence Area name" 
                ForeColor="#FF3300" ValidationGroup="btnsavegeo"></asp:RequiredFieldValidator>
             </td>
        
        </tr>

        <tr>
        <td>Start time</td>
        <td> <asp:TextBox ID="txtstdt"  runat="server"></asp:TextBox> 
        <asp:CalendarExtender ID="CalendarExtenderstdt" runat="server" CssClass="cal_Theme1"
                                Format="MM/dd/yyyy" PopupButtonID="txtstdt" TargetControlID="txtstdt">
                            </asp:CalendarExtender>

             <asp:Label ID="lblst_HH" runat="server" Text="HH:" ></asp:Label>
             <asp:DropDownList ID="ddl_HH_geo"  runat="server" Width="50px" >
             <asp:ListItem>00</asp:ListItem>
                 <asp:ListItem>01</asp:ListItem>
                 <asp:ListItem>02</asp:ListItem>
                 <asp:ListItem>03</asp:ListItem>
                 <asp:ListItem>04</asp:ListItem>
                 <asp:ListItem>05</asp:ListItem>
                 <asp:ListItem>06</asp:ListItem>
                 <asp:ListItem>07</asp:ListItem>
                 <asp:ListItem>08</asp:ListItem>
                 <asp:ListItem>09</asp:ListItem>
                 <asp:ListItem>10</asp:ListItem>
                 <asp:ListItem>11</asp:ListItem>
                 <asp:ListItem>12</asp:ListItem>
                 <asp:ListItem>13</asp:ListItem>
                 <asp:ListItem>14</asp:ListItem>
                 <asp:ListItem>15</asp:ListItem>
                 <asp:ListItem>16</asp:ListItem>
                 <asp:ListItem>17</asp:ListItem>
                 <asp:ListItem>18</asp:ListItem>
                 <asp:ListItem>19</asp:ListItem>
                 <asp:ListItem>20</asp:ListItem>
                 <asp:ListItem>21</asp:ListItem>
                 <asp:ListItem>22</asp:ListItem>
                 <asp:ListItem>23</asp:ListItem>
                 
                </asp:DropDownList>

             <asp:Label ID="lblst_MM" runat="server" Text="MM:" ></asp:Label> 
              <asp:DropDownList ID="ddl_MM_geo"  runat="server" Width="50px" >
              <asp:ListItem>00</asp:ListItem>
             <asp:ListItem>01</asp:ListItem>
                 <asp:ListItem>02</asp:ListItem>
                 <asp:ListItem>03</asp:ListItem>
                 <asp:ListItem>04</asp:ListItem>
                 <asp:ListItem>05</asp:ListItem>
                 <asp:ListItem>06</asp:ListItem>
                 <asp:ListItem>07</asp:ListItem>
                 <asp:ListItem>08</asp:ListItem>
                 <asp:ListItem>09</asp:ListItem>
                 <asp:ListItem>10</asp:ListItem>
                 <asp:ListItem>11</asp:ListItem>
                 <asp:ListItem>12</asp:ListItem>
                 <asp:ListItem>13</asp:ListItem>
                 <asp:ListItem>14</asp:ListItem>
                 <asp:ListItem>15</asp:ListItem>
                 <asp:ListItem>16</asp:ListItem>
                 <asp:ListItem>17</asp:ListItem>
                 <asp:ListItem>18</asp:ListItem>
                 <asp:ListItem>19</asp:ListItem>
                 <asp:ListItem>20</asp:ListItem>
                 <asp:ListItem>21</asp:ListItem>
                 <asp:ListItem>22</asp:ListItem>
                 <asp:ListItem>23</asp:ListItem>
                 <asp:ListItem>24</asp:ListItem>

               
                 <asp:ListItem>25</asp:ListItem>
                 <asp:ListItem>26</asp:ListItem>
                <asp:ListItem>27</asp:ListItem>
                <asp:ListItem>28</asp:ListItem>
                <asp:ListItem>29</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
                <asp:ListItem>31</asp:ListItem>
                <asp:ListItem>32</asp:ListItem>

                <asp:ListItem>33</asp:ListItem>
                <asp:ListItem>34</asp:ListItem>
                <asp:ListItem>35</asp:ListItem>
                <asp:ListItem>36</asp:ListItem>
                <asp:ListItem>37</asp:ListItem>
                <asp:ListItem>38</asp:ListItem>
                <asp:ListItem>39</asp:ListItem>
                <asp:ListItem>40</asp:ListItem>
                <asp:ListItem>41</asp:ListItem>
                <asp:ListItem>42</asp:ListItem>
                <asp:ListItem>43</asp:ListItem>

                <asp:ListItem>44</asp:ListItem>
                <asp:ListItem>45</asp:ListItem>
                <asp:ListItem>46</asp:ListItem>
                <asp:ListItem>47</asp:ListItem>
                <asp:ListItem>48</asp:ListItem>
                <asp:ListItem>49</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
                <asp:ListItem>51</asp:ListItem>
                <asp:ListItem>52</asp:ListItem>
                <asp:ListItem>53</asp:ListItem>
                <asp:ListItem>54</asp:ListItem>
                <asp:ListItem>55</asp:ListItem>
                <asp:ListItem>56</asp:ListItem>
                <asp:ListItem>57</asp:ListItem>
                <asp:ListItem>58</asp:ListItem>
                <asp:ListItem>59</asp:ListItem>
              

             </asp:DropDownList>
             
             
         </td>
         </tr>
         <tr>
        <td>End Time</td>
        <td><asp:TextBox ID="txtenddt"  runat="server"></asp:TextBox> 
        <asp:CalendarExtender ID="CalendarExtenderendt" runat="server" CssClass="cal_Theme1"
                                Format="MM/dd/yyyy" PopupButtonID="txtenddt" TargetControlID="txtenddt">
                            </asp:CalendarExtender>
         <asp:Label ID="lblen_HH" runat="server" Text="HH:" ></asp:Label>
             <asp:DropDownList ID="ddl_HH_end_geo"  runat="server" Width="50px">
             <asp:ListItem>00</asp:ListItem>
                 <asp:ListItem>01</asp:ListItem>
                 <asp:ListItem>02</asp:ListItem>
                 <asp:ListItem>03</asp:ListItem>
                 <asp:ListItem>04</asp:ListItem>
                 <asp:ListItem>05</asp:ListItem>
                 <asp:ListItem>06</asp:ListItem>
                 <asp:ListItem>07</asp:ListItem>
                 <asp:ListItem>08</asp:ListItem>
                 <asp:ListItem>09</asp:ListItem>
                 <asp:ListItem>10</asp:ListItem>
                 <asp:ListItem>11</asp:ListItem>
                 <asp:ListItem>12</asp:ListItem>
                 <asp:ListItem>13</asp:ListItem>
                 <asp:ListItem>14</asp:ListItem>
                 <asp:ListItem>15</asp:ListItem>
                 <asp:ListItem>16</asp:ListItem>
                 <asp:ListItem>17</asp:ListItem>
                 <asp:ListItem>18</asp:ListItem>
                 <asp:ListItem>19</asp:ListItem>
                 <asp:ListItem>20</asp:ListItem>
                 <asp:ListItem>21</asp:ListItem>
                 <asp:ListItem>22</asp:ListItem>
                 <asp:ListItem>23</asp:ListItem>
                 
                </asp:DropDownList>

                <asp:Label ID="lblen_MM" runat="server" Text="MM:" ></asp:Label>
              <asp:DropDownList ID="ddl_MM_end_geo"  runat="server" Width="50px" >
              <asp:ListItem>00</asp:ListItem>
             <asp:ListItem>01</asp:ListItem>
                 <asp:ListItem>02</asp:ListItem>
                 <asp:ListItem>03</asp:ListItem>
                 <asp:ListItem>04</asp:ListItem>
                 <asp:ListItem>05</asp:ListItem>
                 <asp:ListItem>06</asp:ListItem>
                 <asp:ListItem>07</asp:ListItem>
                 <asp:ListItem>08</asp:ListItem>
                 <asp:ListItem>09</asp:ListItem>
                 <asp:ListItem>10</asp:ListItem>
                 <asp:ListItem>11</asp:ListItem>
                 <asp:ListItem>12</asp:ListItem>
                 <asp:ListItem>13</asp:ListItem>
                 <asp:ListItem>14</asp:ListItem>
                 <asp:ListItem>15</asp:ListItem>
                 <asp:ListItem>16</asp:ListItem>
                 <asp:ListItem>17</asp:ListItem>
                 <asp:ListItem>18</asp:ListItem>
                 <asp:ListItem>19</asp:ListItem>
                 <asp:ListItem>20</asp:ListItem>
                 <asp:ListItem>21</asp:ListItem>
                 <asp:ListItem>22</asp:ListItem>
                 <asp:ListItem>23</asp:ListItem>
                 <asp:ListItem>24</asp:ListItem>

               
                 <asp:ListItem>25</asp:ListItem>
                 <asp:ListItem>26</asp:ListItem>
                <asp:ListItem>27</asp:ListItem>
                <asp:ListItem>28</asp:ListItem>
                <asp:ListItem>29</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
                <asp:ListItem>31</asp:ListItem>
                <asp:ListItem>32</asp:ListItem>

                <asp:ListItem>33</asp:ListItem>
                <asp:ListItem>34</asp:ListItem>
                <asp:ListItem>35</asp:ListItem>
                <asp:ListItem>36</asp:ListItem>
                <asp:ListItem>37</asp:ListItem>
                <asp:ListItem>38</asp:ListItem>
                <asp:ListItem>39</asp:ListItem>
                <asp:ListItem>40</asp:ListItem>
                <asp:ListItem>41</asp:ListItem>
                <asp:ListItem>42</asp:ListItem>
                <asp:ListItem>43</asp:ListItem>

                <asp:ListItem>44</asp:ListItem>
                <asp:ListItem>45</asp:ListItem>
                <asp:ListItem>46</asp:ListItem>
                <asp:ListItem>47</asp:ListItem>
                <asp:ListItem>48</asp:ListItem>
                <asp:ListItem>49</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
                <asp:ListItem>51</asp:ListItem>
                <asp:ListItem>52</asp:ListItem>
                <asp:ListItem>53</asp:ListItem>
                <asp:ListItem>54</asp:ListItem>
                <asp:ListItem>55</asp:ListItem>
                <asp:ListItem>56</asp:ListItem>
                <asp:ListItem>57</asp:ListItem>
                <asp:ListItem>58</asp:ListItem>
                <asp:ListItem>59</asp:ListItem>
               
             </asp:DropDownList>
             
      
       </td>
        </tr>

        <tr>
        <td colspan="4"><asp:Button ID="btnsavegeo" runat="server" 
                Text="Save Geofence Area" onclick="btnsavegeo_Click" 
                ValidationGroup="btnsavegeo" /><br />
            <asp:Label ID="lblerrormsg" runat="server"  Text=""  ForeColor="Red"></asp:Label>
        </td>
        </tr>

        </table>
           
        <input type="hidden" id="ContentType" name="ContentType" />
        <div id="map_canvas" style="width: 800px; height: 600px"></div>
         </asp:Panel>    
         <asp:Panel ID="Panel_geodata" runat="server" Visible="false" >
          <asp:GridView ID="gv_geodata" AllowPaging="true" PageSize="10"  OnPageIndexChanging="gridView_PageIndexChanging" runat="server">
          </asp:GridView>
         </asp:Panel>
           
    </td>
    </tr>

    
    </table>
     
</body>
</asp:Content>
 