<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="replayOnEarth" CodeBehind="replayOnEarth.aspx.cs" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerdata" runat="Server">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script src="http://earth-api-samples.googlecode.com/svn/trunk/lib/kmldomwalk.js"
        type="text/javascript"> </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentdata" runat="Server">
    <script type="text/javascript">
        google.load("earth", "1");

        var ge = null;
        var newpath;
        function init(path) {
            newpath = path
            google.earth.createInstance('map3d', initCallback, failureCallback);
        }
        function initialize() {
            google.earth.createInstance('map3d', initCB, failureCB);
        }
        function initCB(instance) {
            ge = instance;
            ge.getWindow().setVisibility(true);
        }
        function failureCB(errorCode) {
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




        function enterTour() {
            //  alert('starting tour');
            if (!tour) {
                alert('No tour found!');
                return;
            }
            ge.getTourPlayer().setTour(tour);
        }

        function playTour() {
            ge.getTourPlayer().play();
        }
        function pauseTour() {
            ge.getTourPlayer().pause();
        }
        function resetTour() {
            ge.getTourPlayer().reset();
        }
        function exitTour() {
            ge.getTourPlayer().setTour(null);
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
                <telerik:RadSlidingPane ID="leftSlider" runat="server" Width="250px" Title="Carriers"
                    IconUrl="~/icons/leftSlider.png">
                    <table align="left">
                        <tr>
                            <td>
                                <telerik:RadDateTimePicker ID="dateFrom" runat="server" Culture="en-US" EnableViewState="false"
                                    Width="200px">
                                    <TimeView ID="TimeView1" runat="server" CellSpacing="-1">
                                    </TimeView>
                                    <TimePopupButton HoverImageUrl="" ImageUrl="" />
                                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                        ViewSelectorText="x">
                                    </Calendar>
                                    <DateInput ID="DateInput1" runat="server" EmptyMessage="From Date">
                                    </DateInput>
                                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                </telerik:RadDateTimePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadDateTimePicker ID="dateTo" runat="server" Culture="en-US" EnableViewState="false"
                                    Width="200px">
                                    <TimeView ID="TimeView2" runat="server" CellSpacing="-1">
                                    </TimeView>
                                    <TimePopupButton HoverImageUrl="" ImageUrl="" />
                                    <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                        ViewSelectorText="x">
                                    </Calendar>
                                    <DateInput ID="DateInput2" runat="server" EmptyMessage="To Date">
                                    </DateInput>
                                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                </telerik:RadDateTimePicker>
                            </td>
                        </tr>
                    </table>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="DateBoxError" runat="server" Text="" Style="color: Red; font-weight: bolder"></asp:Label></ContentTemplate>
                    </asp:UpdatePanel>
                   
                        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel3" runat="server" Skin="Transparent">
                        </telerik:RadAjaxLoadingPanel>
                        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel3"
                            RenderMode="Inline"  Style="height: 100%; z-index: -555">
                            <asp:UpdatePanel ID="UpdatePanelCarListBox" runat="server" UpdateMode="Conditional"   style="height: 90%">
                                <ContentTemplate>
                                    <asp:Timer ID="TimerListBox" runat="server" Interval="1000" OnTick="TimerListBox_Tick">
                                    </asp:Timer>
                                    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">
                                        <script type="text/javascript">
            //<![CDATA[
                                            var listbox;
                                            var filterTextBox;

                                            function pageLoad() {
                                                listbox = $find("<%= car_listbox.ClientID %>");
                                                filterTextBox = document.getElementById("<%= TextBox1.ClientID %>");
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

                                                                                       //]]>
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
                                    <telerik:RadListBox ID="car_listbox" runat="server" AutoPostBack="false" SelectionMode="Single"
                                        Width="100%" Height="90%" CheckBoxes="false" Skin="Transparent" EnableViewState="true"
                                        ViewStateMode="Enabled" BackColor="White" ForeColor="Blue">
                                    </telerik:RadListBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </telerik:RadAjaxPanel>
                   
                </telerik:RadSlidingPane>
            </telerik:RadSlidingZone>
        </telerik:RadPane>
        <telerik:RadSplitBar ID="RadSplitBar1" runat="server" CollapseMode="Both" BackColor="Maroon"
            BorderColor="Maroon" BorderStyle="Ridge" EnableViewState="false">
        </telerik:RadSplitBar>
        <telerik:RadPane ID="middlePane" runat="server" Width="100%" Height="100%" Scrolling="None">
            <telerik:RadDockZone ID="RadDockZoneRight" runat="server" Height="100%" Width="99%">
                <telerik:RadDock ID="track" Title="Replay Track" runat="server" Width="100%" Skin="Transparent"
                    EnableAnimation="True" EnableRoundedCorners="true" Resizable="True" DefaultCommands="ExpandCollapse"
                    EnableViewState="false" Height="100%">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <table align="left">
                                        <tr>
                                            <td align="left" class="style2" colspan="9">
                                                <asp:Button ID="btnLinePath" runat="server" OnClick="btnLinePath_Click" Text="Line Path"
                                                    ToolTip="shows only the path traversed by the Carrier." />
                                                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Normal Track"
                                                    ToolTip="works with low speed internet connection" />
                                                <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Advanced Track"
                                                    ToolTip="Require High speed internet connection" />
                                                <asp:Button ID="btnDownload" runat="server" OnClick="Button4_Click" Text="Download"
                                                    ToolTip="Click to download the Tour." />
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <div id="controls">
                                                    <input id="btnentertour" runat="server" onclick="enterTour()" type="button" value="Enter Tour" />
                                                    <input id="btnplaytour" runat="server" onclick="playTour()" type="button" value="Play Tour" />
                                                    <input id="btnpausetour" runat="server" onclick="pauseTour()" type="button" value="Pause Tour" />
                                                    <input id="btnresettour" runat="server" onclick="resetTour()" type="button" value="Stop/Reset Tour" />
                                                    <input id="btnexisttour" runat="server" onclick="exitTour()" type="button" value="Exit Tour" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <div id="map3d" style="height: 90%; width: 100%;">
                        </div>
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>
        </telerik:RadPane>
    </telerik:RadSplitter>
</asp:Content>
