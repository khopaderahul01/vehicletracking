
<%@ Page Language="C#"   MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="ptdashboard" Codebehind="ptdashboard.aspx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerdata" Runat="Server">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

     <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script src="http://earth-api-samples.googlecode.com/svn/trunk/lib/kmldomwalk.js" type="text/javascript"> </script>

 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentdata" Runat="Server" style="width: 100%; height: 100%">
    <script type="text/javascript">



        google.load("earth", "1");

        var ge = null;
        var newpath;
        function init(path) {
            newpath = path
            google.earth.createInstance('map3d', initCallback, failureCallback);
        }

        function onItemChecked(sender, e) {
            var item = e.get_item();
            var items = sender.get_items();
            var checked = item.get_checked();
            var firstItem = sender.getItem(0);
            if (item.get_text() == "Select All") {
                items.forEach(function (itm) { itm.set_checked(checked); });
            }
            else {
                if (sender.get_checkedItems().length == items.get_count() - 1) {
                    firstItem.set_checked(!firstItem.get_checked());
                }
            }
        }

        function initCallback(instance) {
            ge = instance;
            ge.getWindow().setVisibility(true);
            ge.getNavigationControl().setVisibility(ge.VISIBILITY_SHOW);
            google.earth.fetchKml(ge, newpath, function (kmlObject) {
                if (kmlObject) {
                    var flag = false;
                    ge.getFeatures().appendChild(kmlObject);
                    walkKmlDom(kmlObject, function () {
                        if (this.getType() == 'KmlTour') {
                            tour = this;
                            flag = true;
                            return false;
                        }
                    });

                    if (!flag) {
                        alert('No data found for the selected time Period!');
                        return;
                    }
                    ge.getTourPlayer().setTour(tour);
                    ge.getTourPlayer().play();

                }
                else {
                    alert('Error in loading kmz file..')
                }

            });
        }
        function failureCallback() {
            alert('Error While loading data...');
        }




      
        var listbox;
        var filterTextBox;

        function pageLoad() {
            listbox = $find("<%= car_listbox.ClientID %>");
            filterTextBox = document.getElementById("<%= TextBox1.ClientID %>");
            // set on anything but that 
            listbox._getGroupElement().focus();
        }
        function pageLoad2() {
            listbox = $find("<%= RadListBoxFleet.ClientID %>");
            filterTextBox = document.getElementById("<%= TextBox2.ClientID %>");
            // set on anything but that 
            listbox._getGroupElement().focus();
        }

        function OnClientDroppedHandler(sender, eventArgs) {
            // clear emphasis from matching characters
            eventArgs.get_sourceItem().set_text(clearTextEmphasis(eventArgs.get_sourceItem().get_text()));
        }

        function filterList() {
            pageLoad();
            clearListEmphasis();
            createMatchingList();
        }
        function filterList2() {
            pageLoad2();
            clearListEmphasis();
            createMatchingList();
        }

        // clear emphasis from matching characters for entire list
        function clearListEmphasis() {
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

        // clear emphasis from matching characters for given text
        function clearTextEmphasis(text) {
            var re = new RegExp("</{0,1}em>", "gi");
            return text.replace(re, "");
        }

        // hide listboxitems without matching characters
        function createMatchingList() {
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
            
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <!------------------------------------------------------------------------------------------------------------------------------>
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Width="100%" Height="100%"
        BorderStyle="Ridge" Skin="Transparent">
        <telerik:RadPane ID="leftpane" runat="server" Width="36px" SkinID="Transparent">
            <telerik:RadSlidingZone ID="leftSliderZone" runat="server" SlideDirection="right"
                Width="36px" SkinID="Transparent" DockedPaneId="leftSlider">
                <telerik:RadSlidingPane ID="leftSlider" Title="Carriers" runat="server" Width="230px"
                    Height="100%" SkinID="Transparent" Scrolling="None" IconUrl="~/icons/leftSlider.png">
                    <telerik:RadTabStrip ID="vehicleFleet" runat="server" Skin="Transparent" MultiPageID="vehicleFleetMPage"
                        SelectedIndex="0">
                        <Tabs>
                            <telerik:RadTab runat="server" Text="Carrier" PageViewID="vehicles" Selected="True"
                                Height="100%">
                            </telerik:RadTab>
                            <telerik:RadTab runat="server" Text="Group" PageViewID="fleets">
                            </telerik:RadTab>
                        </Tabs>
                    </telerik:RadTabStrip>
                    <telerik:RadMultiPage ID="vehicleFleetMPage" runat="server" SelectedIndex="0" Height="100%">
                        <telerik:RadPageView ID="vehicles" runat="server" Height="100%">
                            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel3" Height="100%">
                                <asp:UpdatePanel ID="UpdatePanelCarListBox" runat="server" UpdateMode="Conditional"
                                    Height="100%">
                                    <ContentTemplate>
                                        <asp:Timer ID="TimerListBox" runat="server" Interval="1000" OnTick="TimerListBox_Tick">
                                        </asp:Timer>
                                        <table align="center" width="100%" sty>
                                            <tr>
                                                <td align="center" width="100%">
                                                    <telerik:RadTextBox ID="TextBox1" runat="server" Width="100%" onkeyup="filterList();"
                                                        EmptyMessage="Type to Search">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <telerik:RadListBox ID="car_listbox" runat="server" AutoPostBack="true" SelectionMode="Multiple"
                                            Width="100%" Style="height: 100%" CheckBoxes="True" OnItemCheck="car_listbox_ItemCheck"
                                            OnClientItemChecked="onItemChecked" Skin="Web20" EnableViewState="true" ViewStateMode="Enabled">
                                        </telerik:RadListBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </telerik:RadAjaxPanel>
                        </telerik:RadPageView>
                        <telerik:RadPageView ID="fleets" runat="server" Height="100%">
                            <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="RadAjaxLoadingPanel3"
                                RenderMode="Inline" Height="100%">
                                <asp:UpdatePanel ID="UpdatePanelFleetListBox" runat="server" UpdateMode="Conditional"
                                    Height="100%">
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
                                            Width="100%" Style="height: 100%" CheckBoxes="True" OnItemCheck="ListboxFleet_ItemCheck"
                                            OnClientItemChecked="onItemChecked" Skin="Web20" EnableViewState="true" ViewStateMode="Enabled">
                                        </telerik:RadListBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </telerik:RadAjaxPanel>
                        </telerik:RadPageView>
                    </telerik:RadMultiPage>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel3" runat="server" Skin="Transparent">
                    </telerik:RadAjaxLoadingPanel>
                </telerik:RadSlidingPane>
            </telerik:RadSlidingZone>
        </telerik:RadPane>
        <telerik:RadSplitBar ID="RadSplitBar1" runat="server" CollapseMode="Both" BackColor="Maroon"
            BorderColor="Maroon" BorderStyle="Ridge" EnableViewState="false">
        </telerik:RadSplitBar>
        <telerik:RadPane ID="middlePane" runat="server" Height="100%" Scrolling="None">
            <telerik:RadDockZone ID="RadDockZoneRight" runat="server" Height="100%" Width="99%">
                <telerik:RadDock ID="track" Title="Live Track" runat="server" Width="100%" Skin="Transparent"
                    EnableAnimation="True" EnableRoundedCorners="true" Resizable="True" DefaultCommands="ExpandCollapse">
                    <TitlebarTemplate>
                        <asp:Image ID="Image3" runat="server" ImageUrl="~/icons/livetrack.png" Height="24px"
                            Width="95px" />
                    </TitlebarTemplate>
                    <ContentTemplate>
                        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Timer ID="Timer1" runat="server" Interval="20000" OnTick="Timer1_Tick">
                                </asp:Timer>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server" Skin="Transparent">
                        </telerik:RadAjaxLoadingPanel>
                        <telerik:RadAjaxPanel ID="RadAjaxPanel4" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
                            RenderMode="Block">
                       
                        <table>
                         </telerik:RadAjaxPanel>
                            <tr style="width: 100%;">
                                <td>
                                    <telerik:RadComboBox ID="txtVehName" runat="server" Skin="WebBlue" EmptyMessage="Select Carrier"
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
                                            <telerik:RadButton ID="oldTrack" runat="server" Text="Trace" ToolTip="Plots Track on Google Map Between Specified date range."
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
                                            <asp:CheckBox ID="liveFollow" Text="Carrier Follow" AutoPostBack="True" Checked="false"
                                                runat="server" OnCheckedChanged="liveFollow_CheckedChanged" />
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
                        <asp:UpdatePanel ID="UpdatePanelReplayMap" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table style="border: thin groove #000000" width="100%">
                                    <tr>
                                        <td>
                                            <artem:GoogleMap ID="ReplayMap" runat="server" Enabled="true" EnablePanControl="true"
                                                Center-Latitude="18.52" Center-Longitude="73.86" MapType="Roadmap" Height="360px"
                                                Width="100%" Zoom="13" EnableOverviewMapControl="True" EnableMapTypeControl="false"
                                                ScaleControlOptions-Position="TopLeft" EnableViewState="false">
                                            </artem:GoogleMap>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock ID="dashboard" Title="Dashboard" runat="server" Width="100%" Skin="Transparent"
                    EnableAnimation="True" EnableRoundedCorners="true" DefaultCommands="ExpandCollapse">
                    <TitlebarTemplate>
                        <asp:Image ID="Image4" runat="server" ImageUrl="~/icons/dashboard.png" Height="24px"
                            Width="99px" />
                    </TitlebarTemplate>
                    <ContentTemplate>
                        <asp:UpdatePanel ID="updatePanelDashboardGrid" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <telerik:RadGrid ID="RadGrid1" runat="server" EnableViewState="true" AllowSorting="True"
                                    GridLines="None" Skin="Transparent" Width="99%" CellSpacing="0" OnSortCommand="RadGrid1_SortCommand">
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
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="CarrierName" HeaderText="Carrier" SortExpression="CarrierName"
                                                UniqueName="CarrierName">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="time" HeaderText="Last Seen" SortExpression="time"
                                                UniqueName="time" DataType="System.DateTime" ReadOnly="True">
                                            </telerik:GridBoundColumn>
                                           
                                            <telerik:GridTemplateColumn DataField="BatteryVoltage" DataType="System.Double" HeaderText="Battery"
                                                SortExpression="BatteryVoltage" UniqueName="BatteryVoltage">
                                                <ItemTemplate>
                                                    <asp:Label ID="BatteryVoltageLabel" runat="server" Text='<%# Eval("BatteryVoltage").ToString().Equals("")||Eval("BatteryVoltage").ToString().Equals("0")?("NA"):(float.Parse(Eval("BatteryVoltage").ToString())).ToString()%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                          
                                            <telerik:GridBoundColumn DataField="sat" DataType="System.Int32" HeaderText="GPS"
                                                SortExpression="sat" UniqueName="sat">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="speed" DataType="System.Double" HeaderText="Speed"
                                                SortExpression="speed" UniqueName="speed">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="address" DataType="System.Double" HeaderText="Address"
                                                SortExpression="address" UniqueName="address">
                                            </telerik:GridBoundColumn>
                                           
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>
        </telerik:RadPane>
    </telerik:RadSplitter>
</asp:Content>
