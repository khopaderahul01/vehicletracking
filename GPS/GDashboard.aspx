<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" Inherits="GDashboard" Codebehind="GDashboard.aspx.cs" %>
<%@ Register TagPrefix="dotnet" Namespace="dotnetCHARTING" Assembly="dotnetCHARTING" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerdata" Runat="Server">

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
  </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentdata" Runat="Server">
  
   
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
                
  <asp:UpdatePanel ID="UpdatePanel10" runat="server">
    <contenttemplate>
        <asp:Timer ID="Timer1" runat="server" Interval="20000" OnTick="Timer1_Tick" >
        </asp:Timer>
        
    </contenttemplate>
  </asp:UpdatePanel>
   
  
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        connectionstring="<%$ ConnectionStrings:DatabaseConnectionString %>" 
        selectcommand="Prc_CarrierLastLoc_Fetch_Grid" 
        selectcommandtype="StoredProcedure">
        <selectparameters>
            <asp:SessionParameter DefaultValue="0" Name="role" 
                SessionField="carrierString" Type="String" />
        </selectparameters>
                    
    </asp:SqlDataSource>
                

<!------------------------------------------------------------------------------------------------------------------------------>
     
    <div style="height: 100%">
      <%--  <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" />--%>
       <%-- <telerik:RadDockLayout ID="RadDockLayout1" runat="server">          
                --%>
            <telerik:RadSplitter ID="RadSplitter1" runat="server" Width="100%" Height="600px"
                BorderStyle="Ridge" Skin="Transparent" >

              
                <telerik:RadPane ID="leftpane" runat="server" Width="36px" SkinID="Transparent"  >
                    <telerik:RadSlidingZone ID="leftSliderZone" runat="server" SlideDirection="right" Width="36px" SkinID="Transparent" >
                        <telerik:RadSlidingPane ID="leftSlider" runat="server"  Width="250px" EnableAjaxSkinRendering="true" 
                            SkinID="Transparent"  EnableTheming="true" Scrolling="None" Title="Carriers" IconUrl="~/icons/leftSlider.png"  >                            
                            <telerik:RadDockZone ID="RadDockZoneLeft" runat="server" Orientation="Vertical" Height="100%"
                                Width="98%" >
                                <telerik:RadDock ID="date" Title="Date Select" ForeColor="BlueViolet" runat="server"
                                    Width="100%" Skin="Transparent" EnableAnimation="True" EnableRoundedCorners="True"
                                    Index="0" Tag="" DefaultCommands="ExpandCollapse">
                                    <TitlebarTemplate>
                                        <asp:Image ID="dateLogo" runat="server" ImageUrl="~/icons/date.png" Height="26px"
                                            Width="63px" />
                                    </TitlebarTemplate>
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td class="style3">
                                                    <telerik:RadComboBox ID="txtVehName" runat="server" Skin="WebBlue" EmptyMessage="Select Carrier"
                                                        MarkFirstMatch="True" EnableLoadOnDemand="True">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                  <td>
                                                                <telerik:RadDateTimePicker ID="dateFrom" runat="server" Culture="en-US" EnableViewState="false" width="200px">
                                                                    <TimeView ID="TimeView1"  runat="server" CellSpacing="-1">
                                                                    </TimeView>
                                                                    <TimePopupButton HoverImageUrl="" ImageUrl="" />
                                                                    <Calendar ID="Calendar1" runat="server"  UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                                                    </Calendar>
                                                                    <DateInput ID="DateInput1" runat="server"  EmptyMessage="From Date" >
                                                                    </DateInput>
                                                                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                                </telerik:RadDateTimePicker>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadDateTimePicker ID="dateTo" runat="server" Culture="en-US" EnableViewState="false" width="200px">
                                                                    <TimeView ID="TimeView2" runat="server" CellSpacing="-1">
                                                                    </TimeView>
                                                                    <TimePopupButton HoverImageUrl="" ImageUrl="" />
                                                                    <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                                                    </Calendar>
                                                                    <DateInput ID="DateInput2" runat="server" EmptyMessage="To Date">
                                                                    </DateInput>
                                                                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                                </telerik:RadDateTimePicker>
                                                            </td>
                                            </tr>
                                        </table>
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="DateBoxError" runat="server" Text="" Style="color: Red; font-weight: bolder"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </telerik:RadDock>
                                <telerik:RadDock ID="carrier" Title="Carrier List" runat="server" Width="100%" Height="400px"
                                    Skin="Transparent" EnableAnimation="True" EnableRoundedCorners="true" DefaultCommands="ExpandCollapse">
                                    <TitlebarTemplate>
                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/icons/vehiclelist.png" Height="26px"
                                            Width="103px" />
                                    </TitlebarTemplate>
                                    <ContentTemplate>                                       
                                        <telerik:RadTabStrip ID="vehicleFleet" runat="server" Skin="Transparent" MultiPageID="vehicleFleetMPage"
                                            SelectedIndex="0">
                                            <Tabs>
                                                <telerik:RadTab runat="server" Text="Carrier" PageViewID="vehicles" Selected="True">
                                                </telerik:RadTab>
                                                <telerik:RadTab runat="server" Text="Group" PageViewID="fleets">
                                                </telerik:RadTab>
                                            </Tabs>
                                        </telerik:RadTabStrip>
                                        <telerik:RadMultiPage ID="vehicleFleetMPage" runat="server" SelectedIndex="0">
                                            <telerik:RadPageView ID="vehicles" runat="server">
                                                <div>
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <telerik:RadListBox ID="car_listbox" runat="server" AutoPostBack="True" SelectionMode="Multiple"
                                                                Width="100%" Height="320px" CheckBoxes="True" OnItemCheck="car_listbox_ItemCheck"
                                                                Skin="Web20" ViewStateMode="Enabled">
                                                            </telerik:RadListBox>
                                                            <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" runat="server"
                                                                style="width: 100%; text-align: center">
                                                                <ProgressTemplate>
                                                                    <asp:Image ID="ImageGif" runat="server" ImageUrl="~/images/ajax-loader.gif" /><asp:Label
                                                                        ID="msglblLoading" runat="server" Text="Loading" Style="vertical-align: middle"></asp:Label>
                                                                    <br />
                                                                </ProgressTemplate>
                                                            </asp:UpdateProgress>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </telerik:RadPageView>
                                            <telerik:RadPageView ID="fleets" runat="server">
                                                <div >
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <telerik:RadListBox ID="RadListBoxFleet" runat="server" AutoPostBack="True" SelectionMode="Multiple"
                                                                Width="100%" Height="320px" CheckBoxes="True" OnItemCheck="ListboxFleet_ItemCheck"
                                                                Skin="Web20" ViewStateMode="Enabled">
                                                            </telerik:RadListBox>
                                                            <asp:UpdateProgress ID="UpdateProgress3" AssociatedUpdatePanelID="UpdatePanel7" runat="server"
                                                                style="width: 100%; text-align: center">
                                                                <ProgressTemplate>
                                                                    <asp:Image ID="ImageGif1" runat="server" ImageUrl="~/images/ajax-loader.gif" /><asp:Label
                                                                        ID="msglblLoading1" runat="server" Text="Loading" Style="vertical-align: middle"></asp:Label></ProgressTemplate>
                                                            </asp:UpdateProgress>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </telerik:RadPageView>
                                        </telerik:RadMultiPage>
                                    </ContentTemplate>
                                </telerik:RadDock>
                                 <telerik:RadDock ID="ControlPanel" Title="Control Panel" runat="server"
                                    Width="100%" Skin="Transparent" EnableAnimation="True" EnableRoundedCorners="true"
                                    DefaultCommands="ExpandCollapse" Collapsed="false">
                                    <TitlebarTemplate>
                                        <asp:Image ID="Image7" runat="server" ImageUrl="~/icons/alert.png" Height="24px" Width="70px" />                                                                               
                                    </TitlebarTemplate>
                                    <ContentTemplate>                                        
                                            <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                <ContentTemplate>
                                                    <telerik:RadListBox ID="TikerContent"   Width="100%"   runat="server" 
                                                        EnableViewState="False" ViewStateMode="Inherit" BackColor="#3399FF">
                                                        <buttonsettings renderbuttontext="True" transferbuttons="All" />
                                                    </telerik:RadListBox>                                                    
                                                </ContentTemplate>
                                            </asp:UpdatePanel>   
                                        <table >
                                            <tr>
                                                <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <td>
                                                            <asp:CheckBox ID="autoRefresh" Text="Auto Refresh" AutoPostBack="True" Checked="true"
                                                                runat="server" OnCheckedChanged="autoRefresh_CheckedChanged" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="liveFollow" Text="Carrier Follow" AutoPostBack="True" Checked="false"
                                                                runat="server" OnCheckedChanged="liveFollow_CheckedChanged" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkfilter" Text="Filter Stops" AutoPostBack="True" Checked="true"
                                                                runat="server" />
                                                        </td>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </tr>
                                            </table>     <table >
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <telerik:RadButton ID="oldTrack" runat="server" Text="Trace" ToolTip="Plots Track on Google Map Between Specified date range."
                                                                OnClick="oldTrack_Click">
                                                            </telerik:RadButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:UpdateProgress ID="UpdateProgress4" AssociatedUpdatePanelID="UpdatePanel9" runat="server">
                                                        <ProgressTemplate>
                                                            <asp:Image ID="ImageGif2" runat="server" ImageUrl="~/images/ajax-loader.gif" /><asp:Label
                                                                ID="msglblLoading2" runat="server" Text="Loading" Style="vertical-align: middle"></asp:Label></ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </td>
                                            </tr>
                                            <%--                                            
                                                <td>
                                                    <telerik:RadBinaryImage ID="RadBinaryImage1" runat="server" ImageUrl="~/icons/car_icon3big.png"
                                                        Height="18px" />
                                                </td>
                                                <td>
                                                    <p>
                                                        :Bold Stop</p>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <telerik:RadBinaryImage ID="RadBinaryImage2" Height="18px" runat="server" ImageUrl="~/icons/car_icon3.png" />
                                                </td>
                                                <td>
                                                    <p>
                                                        :Stop.</p>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <telerik:RadBinaryImage ID="RadBinaryImage3" runat="server" ImageUrl="~/icons/car_icon4.png"
                                                        Height="18px" />
                                                </td>
                                                <td style="float: right; text-align: center">
                                                    <p>
                                                        :Normal Track point.</p>
                                                </td>
                                            </tr>--%>
                                        </table>                                       
                                    </ContentTemplate>
                                    
                                </telerik:RadDock>
                                <%--
                                <telerik:RadDock AutoPostBack="true" ID="updates" Title="Alerts Tiker" runat="server"
                                    Width="100%" Skin="Transparent" EnableAnimation="True" EnableRoundedCorners="true"
                                    DefaultCommands="ExpandCollapse" Collapsed="false">
                                    <TitlebarTemplate>
                                        <asp:Image ID="Image7" runat="server" ImageUrl="~/icons/alert.png" Height="24px" Width="70px" />                                                                               
                                    </TitlebarTemplate>
                                    <ContentTemplate>                                        
                                            <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                <ContentTemplate>
                                                    <telerik:RadListBox ID="TikerContent"   Width="100%"   runat="server" 
                                                        EnableViewState="False" ViewStateMode="Inherit" BackColor="#3399FF">
                                                        <buttonsettings renderbuttontext="True" transferbuttons="All" />
                                                    </telerik:RadListBox>                                                    
                                                </ContentTemplate>
                                            </asp:UpdatePanel>                                          
                                    </ContentTemplate>
                                </telerik:RadDock>
                           
                                <telerik:RadDock AutoPostBack="true" ID="RadDock1" Title="Random" runat="server"
                                    Width="100%" Skin="Transparent" EnableAnimation="True" EnableRoundedCorners="true"
                                    DefaultCommands="ExpandCollapse" Collapsed="false">                                    
                                    <ContentTemplate>                                        
                                                                           
                                    </ContentTemplate>
                                </telerik:RadDock>
                                --%>
                            </telerik:RadDockZone>
                            
                        </telerik:RadSlidingPane>
                         <%--<telerik:RadSlidingPane ID="RadSlidingPane1" Title="GeoFencing" runat="server" 
                            Width="220px" SkinID="Transparent" Scrolling="None">
                            <div style="padding: 5px">
                                <ul>
                                    <li><a href="Geofencing.aspx"><span>Circular Geofencing </span></a></li>
                                    <li><a href="SqureGeofencing.aspx"><span>Square Geofencing </span></a></li>
                                    <li><a href="PolyGeofencing.aspx"><span>Polygon Geofencing </span></a></li>
                                </ul>
                            </div>
                            
                        </telerik:RadSlidingPane>--%>
                    </telerik:RadSlidingZone>
                </telerik:RadPane>                
                <telerik:RadSplitBar ID="RadSplitBar1" runat="server" CollapseMode="Both" BackColor="Maroon"
                    BorderColor="Maroon" BorderStyle="Ridge">
                </telerik:RadSplitBar>               
                <telerik:RadPane ID="middlePane" runat="server"  Scrolling="None">                
                    <telerik:RadDockZone ID="RadDockZoneRight" runat="server" Width="99%">
                        <telerik:RadDock ID="track" Title="Live Track" runat="server" Width="100%" Skin="Transparent"
                            Height="390"  EnableAnimation="True" EnableRoundedCorners="true"
                             DefaultCommands="ExpandCollapse">
                            <TitlebarTemplate>
                                <asp:Image ID="Image3" runat="server" ImageUrl="~/icons/livetrack.png" Height="24px"
                                    Width="95px" />
                            </TitlebarTemplate>
                            <ContentTemplate>  
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <telerik:RadSplitter ID="RadSplitter2" runat="server" Width="100%" BorderStyle="Ridge"
                                            Skin="Transparent" Orientation="Vertical" ResizeWithParentPane="false"  Height="355" >
                                            <telerik:RadPane ID="RadPane3" runat="server" Width="100%"  Scrolling="None"  Height="365px">
                                                <div align="center">
                                                    <artem:GoogleMap ID="ReplayMap" runat="server" Enabled="true" EnablePanControl="true"
                                                        Center-Latitude="18.52"
                                                        Center-Longitude="73.857" MapType="Roadmap" Height="360px" Width="100%" Zoom="13"
                                                        EnableOverviewMapControl="True" MapTypeControlOptions-Position="BottomCenter"
                                                        ScaleControlOptions-Position="TopLeft" EnableReverseGeocoding="true" EnableViewState="true"
                                                        Style="float: left">
                                                    </artem:GoogleMap>
                                                </div>
                                            </telerik:RadPane>
                                            <telerik:RadSplitBar ID="RadSplitBar4" runat="server" CollapseMode="Both" BackColor="Maroon"
                                                BorderColor="Maroon" BorderStyle="Ridge">
                                            </telerik:RadSplitBar>
                                            <telerik:RadPane ID="RadPane15" runat="server" Width="100%" Scrolling="None" Height="365px">
                                                <table align="center" >
                                                    <tr >                                                        
                                                        <td style="margin-left:0; margin-right:0">
                                                            <dotnet:Chart ID="dashboard" runat="server" Mentor="false" EnableViewState="true"
                                                                ViewStateMode="Enabled" MarginBottom="0" MarginLeft="0" MarginRight="0" MarginTop="0"/>
                                                        </td>
                                                        <td style="margin-left:0; margin-right:0">
                                                            <dotnet:Chart ID="Chart1" runat="server" Width="100%" Mentor="false" EnableViewState="true"
                                                                ViewStateMode="Enabled" MarginBottom="0" MarginLeft="0" MarginRight="0" MarginTop="0"
                                                                Margin="0" />

                                                                  <dotnet:Chart ID="Chart" runat="server" />
                                                            <td>
                                                            
                                                    </tr>
                                                </table>
                                            </telerik:RadPane>
                                        </telerik:RadSplitter>
                                    </ContentTemplate>
                            </asp:UpdatePanel>
                                <telerik:RadAjaxLoadingPanel  ID="RadAjaxLoadingPanel1" runat="server" Skin="Transparent">
                                </telerik:RadAjaxLoadingPanel>

                            </ContentTemplate>
                        </telerik:RadDock>
                        
                        <telerik:RadDock ID="DashBoardDock" Title="DashBoard" runat="server" Width="100%" Skin="Transparent"
                            EnableAnimation="True" EnableRoundedCorners="true" Resizable="True" DefaultCommands="ExpandCollapse">
                            <TitlebarTemplate>
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/icons/dashboard.png" Height="24px"
                                    Width="95px" />
                            </TitlebarTemplate>
                            <ContentTemplate>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                    <ContentTemplate>                                        
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </telerik:RadDock>     
                    </telerik:RadDockZone>
                </telerik:RadPane>
               <%-- <telerik:RadSplitBar ID="RadSplitBar2" runat="server" CollapseMode="Both" BackColor="Maroon"
                    BorderColor="Maroon" BorderStyle="Ridge">
                </telerik:RadSplitBar>
                <telerik:RadPane ID="RadPane1" runat="server" Width="22px">
                    <telerik:RadSlidingZone ID="RadSlidingZone1" runat="server" SlideDirection="Left">
                        <telerik:RadSlidingPane ID="RadSlidingPane1" Title="GeoFencing" runat="server" 
                            Width="220px" SkinID="Transparent" Scrolling="None">
                            <div style="padding: 5px">
                                <ul>
                                    <li><a href="Geofencing.aspx"><span>Circular Geofencing </span></a></li>
                                    <li><a href="SqureGeofencing.aspx"><span>Square Geofencing </span></a></li>
                                    <li><a href="PolyGeofencing.aspx"><span>Polygon Geofencing </span></a></li>
                                </ul>
                            </div>
                            
                        </telerik:RadSlidingPane>
                    </telerik:RadSlidingZone>
                </telerik:RadPane>--%>
            </telerik:RadSplitter>

        <%--</telerik:RadDockLayout>--%>  
    </div>
    </div>

    </asp:Content>
