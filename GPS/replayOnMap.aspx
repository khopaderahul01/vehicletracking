<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="replayOnMap" CodeBehind="replayOnMap.aspx.cs" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerdata" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?k&sensor=true&v=3">
    </script>
    <script src="Scripts/repMap.js" type="text/javascript"></script>
    <style type="text/css">
        .tableList, .tableTitle
        {
            background-color: #9EBFE8;
        }
        .tableList td
        {
            background-color: White;
            padding: 2px 2px 2px 2px;
            text-align: center;
        }
        .tabSearch td
        {
            background-color: white;
        }
        #tbodyCurrent td
        {
            text-align: left;
            padding: 3px 3px 3px 3px;
        }
        #tbodyCurrent td span
        {
            color: White;
        }
        .divVehNoF_hover
        {
            font-size: 9pt;
            color: white;
            background-color: #0a246a;
            padding: 2px 2px 2px 2px;
            text-align: left;
        }
        .divVehNoF
        {
            font-size: 9pt;
            padding: 2px 2px 2px 2px;
            text-align: left;
        }
        .tr_hover td
        {
            background-color: #ebeef6;
        }
        .tr td
        {
            background-color: #ffffff;
        }
        .MarkerText
        {
            width: 120px;
            color: blue;
            font-size: 9pt;
            font-weight: bold;
            font-family: ????;
        }
        
        *.horizontal_track
        {
            background-color: #bbb;
            width: 120px;
            line-height: 0px;
            font-size: 0px;
            text-align: left;
            padding: 4px;
            border: 1px solid;
            border-color: #ddd #999 #999 #ddd;
        }
        *.horizontal_slider
        {
            background-color: #666;
            width: 16px;
            height: 8px;
            position: relative;
            z-index: 2;
            line-height: 0;
            margin: 0;
            border: 2px solid;
            border-color: #999 #333 #333 #999;
        }
        *.horizontal_slit
        {
            background-color: #333;
            width: 110px;
            height: 2px;
            margin: 4px 4px 2px 4px;
            line-height: 0;
            position: absolute;
            z-index: 1;
            border: 1px solid;
            border-color: #999 #ddd #ddd #999;
        }
        *.vertical_track
        {
            background-color: #bbb;
            padding: 3px 5px 15px 5px;
            border: 1px solid;
            border-color: #ddd #999 #999 #ddd;
        }
        *.vertical_slider
        {
            background-color: #666;
            width: 18px;
            height: 8px;
            font: 0px;
            text-align: left;
            line-height: 0px;
            position: relative;
            z-index: 1;
            border: 2px solid;
            border-color: #999 #333 #333 #999;
        }
        *.vertical_slit
        {
            background-color: #000;
            width: 2px;
            height: 100px;
            position: absolute;
            margin: 4px 10px 4px 10px;
            padding: 4px 0 1px 0;
            line-height: 0;
            font-size: 0;
            border: 1px solid;
            border-color: #666 #ccc #ccc #666;
        }
        *.display_holder
        {
            background-color: #bbb;
            color: #fff;
            width: 34px;
            height: 20px;
            text-align: right;
            padding: 0;
            border: 1px solid;
            border-color: #ddd #999 #999 #ddd;
        }
        .value_display
        {
            background-color: #bbb;
            color: #333;
            width: 30px;
            margin: 0 2px;
            text-align: right;
            font-size: 8pt;
            font-face: verdana, arial, helvetica, sans-serif;
            font-weight: bold;
            line-height: 12px;
            border: 0;
            cursor: default;
        }
        
        .MyModalPanel
        {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            margin: 0;
            padding: 0;
            opacity: 0.2;
            filter: alpha(opacity=20);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentdata" runat="Server" onload="initialize()">
    <script type="text/javascript">

        function DrawLine() {


            if (iterator == 0) {
                DrawStart = false;
                mlength = 0;
                AddPoint(content, anglelocal[iterator]);
            }
            else if (lbsLocationlocal[iterator] == 1) {
                plotTower();
            }
            else
                if (speedlocal[iterator] <= 3 && speedlocal[iterator + 1] <= 3 && document.getElementById('<%=chkfilter.ClientID%>').checked) {
                    plotStop();
                }
                else {
                    var spans = document.getElementById("tbodyCurrent").getElementsByTagName("span");
                    var tablePlayBack = document.getElementById("tablePlayBack");
                    var content = "Plate no: " + carrierNameGlobal + "<br/>Time:" + timelocal[iterator] + "<br/>Speed:" + speedlocal[iterator] + "<br/>Addess:" + addresslocal[iterator];

                    var tr = document.createElement("tr");
                    tr.onmouseover = function () { this.className = "tr_hover"; };
                    tr.onmouseout = function () { this.className = "tr"; };
                    tr.style.cursor = "pointer";
                    tr.onclick = function () {
                        var newcontent = "Plate no: " + carrierNameGlobal + "<br/>Time:" + this.childNodes[0].innerHTML + "<br/>Speed:" + this.childNodes[2].innerHTML + "<br/>Addess:" + this.childNodes[1].innerHTML;

                        ShowPoint(this.childNodes[1].dataFld.split(',')[0], this.childNodes[1].dataFld.split(',')[1], newcontent);
                    };

                    var td1 = document.createElement("td");
                    td1.innerHTML = timelocal[iterator];
                    td1.style.width = "18%";
                    tr.appendChild(td1);

                    var td2 = document.createElement("td");
                    td2.style.textAlign = "left";
                    td2.dataFld = lat[iterator] + "," + long[iterator];
                    td2.innerHTML = addresslocal[iterator];
                    td2.style.width = "32%";
                    tr.appendChild(td2);

                    var td3 = document.createElement("td");
                    td3.innerHTML = speedlocal[iterator];
                    td3.style.width = "12%";
                    tr.appendChild(td3);

                    var td4 = document.createElement("td");
                    mileage = mileage + Calc(lat[iterator], long[iterator], lat[iterator + 1], long[iterator + 1]);
                    td4.innerHTML = Math.round(mileage * 10) / 10;
                    td4.style.width = "12%";
                    tr.appendChild(td4);

                    var td5 = document.createElement("td");
                    td5.innerHTML = (din1local[iterator] == 0 ? "OFF" : "ON");
                    td5.style.width = "14%";
                    tr.appendChild(td5);

                    tablePlayBack.appendChild(tr);
                    tr.scrollIntoView();
                    spans[0].innerHTML = timelocal[iterator];
                    spans[1].innerHTML = speedlocal[iterator];
                    spans[3].innerHTML = addresslocal[iterator];
                    spans[2].innerHTML = ExplainAngle(anglelocal[iterator]);
                    marker = AddPoint(content, anglelocal[iterator]);

                }


            iterator++;
            var SliderSpe = document.getElementById("SliderSpe");
            var sperate = SliderSpe.value;
            timer = setTimeout(function () {
                if (iterator < total - 1) {
                    DrawLine();
                }
                else {
                    PlayClick(3);
                    firstFlag = 0;
                    alert('Track playback finished!');
                }
            }, 1000 - sperate * 100);
        }


        function OnClientValueChanged(sender, args) {
            document.getElementById("SliderSpe").value = sender.get_value()

        }

        function showAlert(message) {
            alert(message);
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <!------------------------------------------------------------------------------------------------------------------------------>
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Width="100%" Height="100%"
        BorderStyle="Ridge" Skin="Transparent">
        <telerik:RadPane ID="middlePane" runat="server" Height="100%">
            <div style="height: 24px; background-color: #B0B0B0; border: solid 1px #666666;"
                id="divCurrent">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tbody id="tbodyCurrent">
                        <tr>
                            <td style="width: 20%;">
                                Time:<span></span>
                            </td>
                            <td style="width: 10%;">
                                Speed:<span></span>
                            </td>
                            <td style="width: 10%;">
                                Angle:<span></span>
                            </td>
                            <td style="width: 60%;">
                                Address:<span></span>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div id="map_canvas" style="width: 100%; height: 65%">
            </div>
            <div style="height: 30%;" id="divControl">
                <table id="tabBottom" cellpadding="0" style="height: 158px;" cellspacing="0" border="0"
                    width="100%">
                    <tr>
                        <td style="width: 65%; vertical-align: top;" id="tdList">
                            <table cellpadding="0" cellspacing="1" border="0" style="width: 98%; text-align: center"
                                class="tableTitle" id="tableTitle">
                                <tbody>
                                    <tr>
                                        <td class="dgHeader" style="width: 18%">
                                            Time
                                        </td>
                                        <td class="dgHeader" style="width: 32%">
                                            Address<font style="font-weight: normal; color: #666666;">(Click to see map location
                                                point)</font>
                                        </td>
                                        <td class="dgHeader" id="tdSpeed" style="width: 12%">
                                            Speed
                                        </td>
                                        <td class="dgHeader" id="tdMile" style="width: 12%">
                                            KM Travelled
                                        </td>
                                        <td class="dgHeader" style="width: 14%">
                                            Ignition
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <div id="divPlayBack" style="overflow-y: scroll; height: 128px; width: 100%;" class="divScroll">
                                <table cellpadding="0" cellspacing="1" border="0" class="tableList" width="100%">
                                    <tbody id="tablePlayBack">
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadPane>
        <telerik:RadSplitBar ID="RadSplitBar1" runat="server" CollapseMode="Both" BackColor="Maroon"
            BorderColor="Maroon" BorderStyle="Ridge" EnableViewState="false">
        </telerik:RadSplitBar>
        <telerik:RadPane ID="leftpane" runat="server"  Height="100%">
            <telerik:RadSlidingZone ID="leftSliderZone" runat="server" SlideDirection="Left"
                Height="100%" Width="36px"  DockedPaneId="leftSlider">
                <telerik:RadSlidingPane ID="leftSlider" runat="server" Width="280px" 
                    Title="Carriers" Height="100%">
                    <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="RadAjaxLoadingPanel3"
                        RenderMode="Inline">
                        <asp:UpdatePanel ID="updatePanelControlPanel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table align="left">
                                    <tr>
                                        <td>
                                            <telerik:RadDateTimePicker ID="dateFrom" runat="server" Culture="en-US" EnableViewState="false"
                                                Width="170px">
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
                                        <td>
                                            <asp:CheckBox ID="chkfilter" Text="Filter Stops" AutoPostBack="True" Checked="true"
                                                runat="server" />&nbsp;&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadDateTimePicker ID="dateTo" runat="server" Culture="en-US" EnableViewState="false"
                                                Width="170px">
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
                                        <td>
                                            <asp:CheckBox ID="chkPlotLBS" Text="Plot LBS" AutoPostBack="True" Checked="false"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <table align="left" cellpadding="1" cellspacing="1" style="height: 40px;">
                                    <tr>
                                        <td>
                                            <telerik:RadButton ID="btnReplay" runat="server" Text="Search" OnClick="RadReplay_Click">
                                            </telerik:RadButton>
                                        </td>
                                        <td>
                                            <input type="button" style="padding: 2px 2px 2px 2px" disabled id="btnSuspend" onclick="PlayClick(1)"
                                                value='Pause' />
                                        </td>
                                        <td>
                                            <input type="button" style="padding: 2px 2px 2px 2px" disabled id="btnPlay" onclick="PlayClick(2)"
                                                value='Play' />
                                        </td>
                                        <td>
                                            <input type="button" style="padding: 2px 2px 2px 2px" disabled id="btnStop" onclick="PlayClick(3)"
                                                value='Stop' />
                                        </td>
                                    </tr>
                                </table>
                                <table align="left" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="text-align: right">
                                            Playback speed:
                                        </td>
                                        <td>
                                            <telerik:RadSlider ID="Slider" runat="server" MaximumValue="10" Value="5" MinimumValue="1"
                                                Width="150px" OnClientValueChanged="OnClientValueChanged" OnClientLoad="OnClientValueChanged">
                                            </telerik:RadSlider>
                                        </td>
                                        <td>
                                            <input type="text" id="SliderSpe" style="border: none; color: Red; font-weight: bold;
                                                text-align: center; width: 20px" readonly="readonly" value="5" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </telerik:RadAjaxPanel>
                    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel3"
                        RenderMode="Inline" Style="height: 100%; z-index: -555">
                        <asp:UpdatePanel ID="UpdatePanelCarListBox" runat="server" UpdateMode="Conditional"
                            style="height: 100%">
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
                                    Width="100%" Height="70%" CheckBoxes="false" Skin="Transparent" EnableViewState="true"
                                    ViewStateMode="Enabled">
                                </telerik:RadListBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </telerik:RadAjaxPanel>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel3" runat="server" Skin="Transparent">
                    </telerik:RadAjaxLoadingPanel>
                </telerik:RadSlidingPane>
            </telerik:RadSlidingZone>
        </telerik:RadPane>
    </telerik:RadSplitter>
</asp:Content>
