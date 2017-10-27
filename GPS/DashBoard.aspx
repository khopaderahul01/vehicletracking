<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master"
    Inherits="DashBoard" CodeBehind="DashBoard.aspx.cs" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerdata" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?k&sensor=true&v=3">
    </script>
    <script src="Scripts/dashbrd.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentdata" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <!------------------------------------------------------------------------------------------------------------------------------>
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Width="100%" Height="100%"
        BorderStyle="Ridge" >

         <telerik:RadPane ID="middlePane" runat="server" Width="100%" Height="100%" Scrolling="None">
            <telerik:RadDockZone ID="RadDockZoneRight" runat="server" Height="100%" Width="99%">
                <telerik:RadDock ID="dashboard" Title="Dashboard" runat="server" Width="100%" 
                    EnableAnimation="True" EnableRoundedCorners="true" DefaultCommands="ExpandCollapse">
                    <ContentTemplate>
                        <telerik:RadAjaxPanel ID="RadAjaxPanel3" runat="server" LoadingPanelID="RadAjaxLoadingPanel3"
                            RenderMode="Inline" Width="100%">
                            <asp:UpdatePanel ID="updatePanelDashboardGrid" runat="server" UpdateMode="Conditional"
                                style="width: 100%">
                                <ContentTemplate>
                                    <table width="100%" align="center">
                                        <tr>
                                            <td>
                                                <telerik:RadGrid ID="RadGrid1" runat="server" EnableViewState="true" AllowSorting="True"
                                                    Style="width: 100%" GridLines="None"  Width="100%" OnSortCommand="RadGrid1_SortCommand">
                                                    <ClientSettings AllowColumnsReorder="True" ReorderColumnsOnClient="True">
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="CarrierId,time">
                                                        <CommandItemSettings ExportToPdfText="Export to Pdf" />
                                                        <RowIndicatorColumn>
                                                            <HeaderStyle Width="20px" />
                                                        </RowIndicatorColumn>
                                                        <ExpandCollapseColumn>
                                                            <HeaderStyle Width="20px" />
                                                        </ExpandCollapseColumn>
                                                        <CommandItemSettings ExportToPdfText="Export to Pdf" />
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="CarrierName" HeaderText="Carrier" SortExpression="CarrierName"
                                                                UniqueName="CarrierName">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="time" HeaderText="Last Seen" SortExpression="time"
                                                                UniqueName="time" DataType="System.DateTime" ReadOnly="True">
                                                            </telerik:GridBoundColumn>
                                                            <%--    
                                        <telerik:GridBoundColumn DataField="latitude" DataType="System.Double" HeaderText="latitude"
                                            SortExpression="latitude" UniqueName="latitude">
                                                        
                                        </telerik:GridBoundColumn>  
                                        <telerik:GridBoundColumn DataField="longitude" DataType="System.Double" HeaderText="longitude"
                                            SortExpression="longitude" UniqueName="longitude">
                                        </telerik:GridBoundColumn>
                                                            --%>
                                                            <telerik:GridTemplateColumn DataField="Din1" DataType="System.Int32" HeaderText="IGN"
                                                                SortExpression="Din1" UniqueName="Din1">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Din1Label" runat="server" Text='<%# Eval("Din1").ToString().Equals("")?"NA":(Eval("carrierTypeFId").ToString().Equals("1")?"NA":(Eval("Din1").ToString().Equals("0")?"OFF":"ON")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn DataField="Din2" DataType="System.Int32" HeaderText="Door"
                                                                SortExpression="Din2" UniqueName="Din2">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Din2Label" runat="server" Text='<%#Eval("Din2").ToString().Equals("")?"NA":(Eval("carrierTypeFId").ToString().Equals("1")?"NA":(Eval("Din2").ToString().Equals("0")?"Close":"Open")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn DataField="BatteryVoltage" DataType="System.Double" HeaderText="Battery"
                                                                SortExpression="BatteryVoltage" UniqueName="BatteryVoltage">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="BatteryVoltageLabel" runat="server" Text='<%# Eval("BatteryVoltage").ToString().Equals("")||Eval("BatteryVoltage").ToString().Equals("0")?("NA"):(float.Parse(Eval("BatteryVoltage").ToString())/1000).ToString()+ " V"%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn DataField="PcbTemp" DataType="System.Double" HeaderText="Temp."
                                                                SortExpression="PcbTemp" UniqueName="PcbTemp">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="PcbTempLabel" runat="server" Text='<%# Eval("PcbTemp").ToString().Equals("")||Eval("PcbTemp").ToString().Equals("0")?("NA"):(float.Parse(Eval("PcbTemp").ToString())/10).ToString()+ " deg"%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn DataField="sat" DataType="System.Int32" HeaderText="GPS"
                                                                SortExpression="sat" UniqueName="sat">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="speed" DataType="System.Double" HeaderText="Speed"
                                                                SortExpression="speed" UniqueName="speed">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn DataField="latitude" DataType="System.Double" HeaderText="latitude."
                                                                SortExpression="latitude" UniqueName="latitude">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="latitudelbl" runat="server" Text='<%# System.Math.Round((Double)Eval("latitude"),2) %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn DataField="longitude" DataType="System.Double" HeaderText="longitude."
                                                                SortExpression="longitude" UniqueName="longitude">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="longitudelbl" runat="server" Text='<%# System.Math.Round((Double)Eval("longitude"),2) %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn DataField="address" DataType="System.Double" HeaderText="Address"
                                                                SortExpression="address" UniqueName="address">
                                                            </telerik:GridBoundColumn>
                                                            <%-- 
                                        <telerik:GridBoundColumn DataField="orgName" HeaderText="Organisation" SortExpression="orgName"
                                            UniqueName="orgName">
                                        </telerik:GridBoundColumn>
                                                   
                                        <telerik:GridButtonColumn HeaderText="Location" Text="Get location" 
                                            UniqueName="column">
                                        </telerik:GridButtonColumn>
                                                            --%>
                                                        </Columns>
                                                        <EditFormSettings>
                                                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                            </EditColumn>
                                                        </EditFormSettings>
                                                    </MasterTableView>
                                                    <FilterMenu EnableImageSprites="False">
                                                    </FilterMenu>
                                                    <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default" EnableImageSprites="True">
                                                    </HeaderContextMenu>
                                                </telerik:RadGrid>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </telerik:RadAjaxPanel>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock ID="track" Title="Live Location" runat="server" Width="100%" 
                    EnableAnimation="True" EnableRoundedCorners="true" Resizable="True" DefaultCommands="ExpandCollapse">
                    <ContentTemplate>
                        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Timer ID="Timer1" runat="server" Interval="5000" OnTick="Timer1_Tick">
                                </asp:Timer>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" >
                        </telerik:RadAjaxLoadingPanel>
                        <telerik:RadAjaxPanel ID="RadAjaxPanel4" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
                            RenderMode="Block">
                            <table style="width: 100%; display: none; background-color: ActiveBorder">
                                <tr>
                                    <td>
                                        <telerik:RadComboBox ID="txtVehName" runat="server"  EmptyMessage="Select Carrier"
                                            MarkFirstMatch="True" EnableLoadOnDemand="True" EnableViewState="true" Filter="Contains">
                                        </telerik:RadComboBox>
                                        &nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <telerik:RadDateTimePicker ID="dateFrom" runat="server" Culture="en-US" EnableViewState="false"
                                            Width="200px">
                                            <DateInput ID="DateInput1" runat="server" EmptyMessage="From Date">
                                            </DateInput>
                                            <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                        </telerik:RadDateTimePicker>
                                        &nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <telerik:RadDateTimePicker ID="dateTo" runat="server" Culture="en-US" EnableViewState="false"
                                            Width="200px">
                                            <DateInput ID="DateInput2" runat="server" EmptyMessage="To Date">
                                            </DateInput>
                                            <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                        </telerik:RadDateTimePicker>
                                        &nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <telerik:RadButton  ID="oldTrack" runat="server" Text="Trace" ToolTip="Plots Track on Google Map Between Specified date range."
                                                    OnClick="oldTrack_Click">
                                                </telerik:RadButton>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="updatePanelControlPanel" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:CheckBox ID="autoRefresh" Text="Auto Refresh" AutoPostBack="True" Checked="true"
                                                    runat="server" OnCheckedChanged="autoRefresh_CheckedChanged" EnableViewState="true" />
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:CheckBox ID="chkfilter" Text="Filter Stops" AutoPostBack="True" Checked="true"
                                                    runat="server" />&nbsp;&nbsp;&nbsp;
                                                <asp:CheckBox ID="chkPlotLBS" Text="Plot LBS Data" AutoPostBack="True" Checked="false"
                                                    runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <div id="map_canvas" style="width: 100%; height: 360px">
                            </div>
                        </telerik:RadAjaxPanel>
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>
        </telerik:RadPane>


      
        <telerik:RadSplitBar ID="RadSplitBar1" runat="server" CollapseMode="Both" BackColor="Maroon"
            BorderColor="Maroon" BorderStyle="Ridge" EnableViewState="false">
        </telerik:RadSplitBar>
         <telerik:RadPane ID="leftpane" runat="server" Width="36px" SkinID="Default">
            <telerik:RadSlidingZone ID="leftSliderZone" runat="server" SlideDirection="left"
                Width="36px" DockedPaneId="leftSlider" >
                <telerik:RadSlidingPane ID="leftSlider" Title="Carriers" runat="server" Width="230px"
                    Height="100%"  Scrolling="None">
                    <telerik:RadTabStrip ID="vehicleFleet" runat="server" MultiPageID="vehicleFleetMPage"
                        SelectedIndex="0">
                        <Tabs>
                            <telerik:RadTab runat="server" Text="Carrier" PageViewID="vehicles" Selected="True">
                            </telerik:RadTab>
                            <telerik:RadTab runat="server" Text="Group" PageViewID="fleets">
                            </telerik:RadTab>
                        </Tabs>
                    </telerik:RadTabStrip>
                    <telerik:RadMultiPage ID="vehicleFleetMPage" runat="server" SelectedIndex="0" Style="height: 100%">
                        <telerik:RadPageView ID="vehicles" runat="server" Style="height: 100%" Height="100%">
                            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel3"
                                RenderMode="Inline" Style="height: 100%; z-index: -555">
                                <asp:UpdatePanel ID="UpdatePanelCarListBox" runat="server" UpdateMode="Conditional"
                                    style="height: 80%">
                                    <ContentTemplate>
                                        <asp:Timer ID="TimerListBox" runat="server" Interval="1000" OnTick="TimerListBox_Tick">
                                        </asp:Timer>
                                        <telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">
                                            <script type="text/javascript">

                                                function OnClientDroppedHandler(sender, eventArgs) {
                                                    try {
                                                        // clear emphasis from matching characters
                                                        eventArgs.get_sourceItem().set_text(clearTextEmphasis(eventArgs.get_sourceItem().get_text()));
                                                    }
                                                    catch (err) {

                                                    }
                                                }

                                                var listbox;
                                                var filterTextBox;

                                                function pageLoad() {
                                                    try {
                                                        listbox = $find("<%= car_listbox.ClientID %>");
                                                        filterTextBox = document.getElementById("<%= TextBox1.ClientID %>");
                                                        // set on anything but that 
                                                        listbox._getGroupElement().focus();
                                                    }
                                                    catch (err) {

                                                    }
                                                }

                                                function filterList() {
                                                    try {
                                                        pageLoad();
                                                        clearListEmphasis();
                                                        createMatchingList();
                                                    }
                                                    catch (err) {

                                                    }
                                                }

                                                function clearListEmphasis() {
                                                    try {
                                                        var re = new RegExp("</{0,1}em>", "gi");
                                                        var items = listbox.get_items();
                                                        var itemText;

                                                        items.forEach
                                                            (
                                                                function (item) {
                                                                    itemText = item.get_text();
                                                                    item.set_text(clearTextEmphasis(itemText));
                                                                }
                                                            )
                                                    }
                                                    catch (err) {

                                                    }
                                                }

                                                function clearTextEmphasis(text) {
                                                    try {
                                                        var re = new RegExp("</{0,1}em>", "gi");
                                                        return text.replace(re, "");
                                                    }
                                                    catch (err) {

                                                    }
                                                }

                                                function createMatchingList() {
                                                    try {
                                                        var items = listbox.get_items();
                                                        var filterText = filterTextBox.value;
                                                        var re = new RegExp(filterText, "i");

                                                        items.forEach
                                                            (
                                                                function (item) {
                                                                    var itemText = item.get_text();

                                                                    if (itemText.toLowerCase().indexOf(filterText.toLowerCase()) != -1) {
                                                                        item.set_text(itemText.replace(re, "<em>" + itemText.match(re) + "</em>"));
                                                                        item.set_visible(true);
                                                                    }
                                                                    else {
                                                                        item.set_visible(false);
                                                                    }
                                                                }
                                                            )
                                                    }
                                                    catch (err) {

                                                    }
                                                }




                                                var listbox2;
                                                var filterTextBox2;

                                                function pageLoad2() {
                                                    try {
                                                        listbox2 = $find("<%= RadListBoxFleet.ClientID %>");
                                                        filterTextBox2 = document.getElementById("<%= TextBox2.ClientID %>");
                                                        // set on anything but that 
                                                        listbox2._getGroupElement().focus();
                                                    }
                                                    catch (err) {

                                                    }
                                                }

                                                function filterList2() {
                                                    try {
                                                        pageLoad2();
                                                        clearListEmphasis2();
                                                        createMatchingList2();
                                                    }
                                                    catch (err) {

                                                    }
                                                }

                                                function clearListEmphasis2() {
                                                    try {
                                                        var re = new RegExp("</{0,1}em>", "gi");
                                                        var items = listbox2.get_items();
                                                        var itemText;

                                                        items.forEach
                                                            (
                                                                function (item) {
                                                                    itemText = item.get_text();
                                                                    item.set_text(clearTextEmphasis2(itemText));
                                                                }
                                                            )
                                                    }
                                                    catch (err) {

                                                    }
                                                }

                                                function clearTextEmphasis2(text) {
                                                    try {
                                                        var re = new RegExp("</{0,1}em>", "gi");
                                                        return text.replace(re, "");
                                                    }
                                                    catch (err) {

                                                    }
                                                }

                                                function createMatchingList2() {
                                                    try {
                                                        var items = listbox2.get_items();
                                                        var filterText = filterTextBox2.value;
                                                        var re = new RegExp(filterText, "i");

                                                        items.forEach
                                                            (
                                                                function (item) {
                                                                    var itemText = item.get_text();

                                                                    if (itemText.toLowerCase().indexOf(filterText.toLowerCase()) != -1) {
                                                                        item.set_text(itemText.replace(re, "<em>" + itemText.match(re) + "</em>"));
                                                                        item.set_visible(true);
                                                                    }
                                                                    else {
                                                                        item.set_visible(false);
                                                                    }
                                                                }
                                                            )
                                                    }
                                                    catch (err) {

                                                    }
                                                }
                                                    


                                            </script>
                                        </telerik:RadCodeBlock>
                                        <table align="center" width="100%">
                                            <tr>
                                                <td align="center" width="100%">
                                                    <telerik:RadTextBox ID="TextBox1" runat="server" Width="100%" onkeyup="filterList();"
                                                        EmptyMessage="Type to Search">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <telerik:RadListBox ID="car_listbox" runat="server" AutoPostBack="true" SelectionMode="Multiple"
                                            Width="100%" Height="100%" CheckBoxes="True" OnItemCheck="car_listbox_ItemCheck"
                                            OnClientItemChecked="onItemChecked"  EnableViewState="true"
                                            ViewStateMode="Enabled">
                                        </telerik:RadListBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </telerik:RadAjaxPanel>
                        </telerik:RadPageView>
                        <telerik:RadPageView ID="fleets" runat="server" Style="height: 100%">
                            <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="RadAjaxLoadingPanel3"
                                RenderMode="Inline" Height="90%" Style="height: 100%; z-index: -555">
                                <asp:UpdatePanel ID="UpdatePanelFleetListBox" runat="server" UpdateMode="Conditional"
                                    style="height: 80%">
                                    <ContentTemplate>
                                        <table align="center" width="100%">
                                            <tr>
                                                <td align="center" width="100%">
                                                    <telerik:RadTextBox ID="TextBox2" Width="100%" runat="server" EmptyMessage="Type to Search"
                                                        onkeyup="filterList2();">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <telerik:RadListBox ID="RadListBoxFleet" runat="server" AutoPostBack="True" SelectionMode="Multiple"
                                            Width="100%" Height="100%" CheckBoxes="True" OnItemCheck="ListboxFleet_ItemCheck"
                                            OnClientItemChecked="onItemChecked" Skin="Transparent" EnableViewState="true"
                                            ViewStateMode="Enabled">
                                        </telerik:RadListBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </telerik:RadAjaxPanel>
                        </telerik:RadPageView>
                    </telerik:RadMultiPage>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel3" runat="server" Skin="Transparent"
                        Style="z-index: -5555;">
                    </telerik:RadAjaxLoadingPanel>
                </telerik:RadSlidingPane>
            </telerik:RadSlidingZone>
        </telerik:RadPane>
    </telerik:RadSplitter>
</asp:Content>
