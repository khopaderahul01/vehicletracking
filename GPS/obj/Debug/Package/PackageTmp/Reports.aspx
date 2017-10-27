<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Reports" CodeBehind="Reports.aspx.cs" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerdata" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentdata" runat="Server">
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
                <telerik:RadDock ID="reports" Title="Reports" runat="server" Width="100%" Skin="Transparent"
                    EnableAnimation="True" EnableRoundedCorners="true" Collapsed="false" DefaultCommands="ExpandCollapse"
                    Height="600px">
                    <TitlebarTemplate>
                        <asp:Image ID="Image6" runat="server" ImageUrl="~/icons/reports.png" Height="24px"
                            Width="80px" />
                    </TitlebarTemplate>
                    <ContentTemplate>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chk_0" Text="Enable 0" Visible="false" runat="server" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="dd_report" runat="server" OnSelectedIndexChanged="dd_report_SelectedIndexChanged"
                                                AutoPostBack="true">
                                                <asp:ListItem Value="kmTravld">KM Travelled</asp:ListItem>
                                                <asp:ListItem Value="boldstop">Bold Stop</asp:ListItem>
                                                <asp:ListItem Value="worktime">Working Time</asp:ListItem>
                                                <asp:ListItem Value="idle">Idle Time</asp:ListItem>
                                                <asp:ListItem Value="fuel">Fuel Monitoring</asp:ListItem>
                                                <asp:ListItem Value="temp">Temperature Report</asp:ListItem>
                                                <%--<asp:ListItem Value="ignSeq">Ignition ON/OFF Sequence</asp:ListItem> --%>
                                                <asp:ListItem Value="summarry">Daily Summary</asp:ListItem>
                                                <asp:ListItem Value="speed">Speed</asp:ListItem>
                                                <asp:ListItem Value="overSpeed">Over Speeding</asp:ListItem>
                                                <asp:ListItem Value="geoFencing">Geofencing</asp:ListItem>
                                                <asp:ListItem Value="fenceBKMT">Fence based KmT</asp:ListItem>
                                                <asp:ListItem Value="ignition">Ignition ON/OFF</asp:ListItem>
                                                <asp:ListItem Value="activity">Detailed activity</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label runat="server" ID="lblStopage" Text="Stoppage Time(minutes):" Visible="false"></asp:Label>
                                            <asp:Label ID="lblSpeed" runat="server" Text="Minimum Speed" Visible="false"></asp:Label>
                                            <asp:TextBox ID="txt_boldStop" runat="server" Visible="false" Width="42px"></asp:TextBox>
                                            <telerik:RadButton ID="btngenRpt" runat="server" OnClick="btngenRpt_Click" Text="Genrate" />
                                            <telerik:RadButton ID="btnshowchat" runat="server" Visible="false" Text="View Chart"
                                                OnClick="btnshowchat_Click" />
                                            <telerik:RadButton ID="btnexport" runat="server" Text="Export To Excel" ButtonType="LinkButton"
                                                Visible="false" Target="_blank" NavigateUrl="~/ExportExcel.aspx" />
                                            <telerik:RadButton ID="btnexport_pdf" AutoPostBack="true" ButtonType="LinkButton"
                                                runat="server" Text="Export To PDF" Visible="false" NavigateUrl="~/ExportPdf.aspx" />
                                        </td>
                                    </tr>
                                </table>
                                <table align="center" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="center" colspan="9">
                                            <asp:Label ID="lblRptName" runat="server" Font-Bold="True">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblmsg" runat="server" ForeColor="Red" Visible="false" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblKM" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Green"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel4" runat="server">
                                                <ProgressTemplate>
                                                    <table>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Label ID="msglbl" runat="server" Text="Loading"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/ajax-loader (1).gif" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanelReportGrid" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadGrid ID="gv_Report" runat="server" AllowSorting="True" GridLines="None" EnableViewState="false" ViewStateMode="Disabled"
                                                Skin="Transparent" CellSpacing="0" AutoGenerateColumns="true" OnSortCommand="gv_Report_SortCommand">
                                                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default" EnableImageSprites="True">
                                                </HeaderContextMenu>
                                            </telerik:RadGrid>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanelCharts" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table align="center" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblGeoVisit" runat="server" Text="Geofences not visited:, " Font-Bold="true"
                                                ForeColor="Red" Visible="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="9">
                                            <asp:DataList ID="datalist_day" Width="700px" runat="server" BackColor="White" BorderColor="#E7E7FF"
                                                BorderStyle="None" BorderWidth="1px" CellPadding="5" GridLines="Horizontal" Visible="false"
                                                EnableViewState="false">
                                                <AlternatingItemStyle BackColor="#F7F7F7" />
                                                <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                                                <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                                                <ItemStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                                                <ItemTemplate>
                                                    <table cellpadding="3" cellspacing="3" width="700px">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblst_time" runat="server" Text="Start Time:"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lbldt_on" runat="server" Text='<%# Bind("dateOn")%>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblst_loc" runat="server" Text="Start Location:"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblst_location" runat="server" Text='<%# Bind("st_loc") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblst_time1" runat="server" Text="Stop time:"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lbldt_off" runat="server" Text='<%# Bind("dateOff") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblstop_loc" runat="server" Text="Stop Location:"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblstop_location" runat="server" Text='<%# Bind("end_loc") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblmax_sp" runat="server" Text="Max Speed:"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblmax_speed" runat="server" Text='<%# Bind("MaxSpeed") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblkm" runat="server" Text="Kms Travelled"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblkm_travel" runat="server" Text='<%# Bind("KMTravelINMinutes") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <telerik:RadChart ID="pieChart" runat="server" DefaultType="Pie" Visible="false"
                                                Height="600px" Width="600px" EnableViewState="false">
                                                <PlotArea>
                                                    <XAxis MaxValue="5" MinValue="1" Step="1">
                                                    </XAxis>
                                                    <YAxis MaxValue="25" Step="5">
                                                    </YAxis>
                                                    <YAxis2 MaxValue="5" MinValue="1" Step="1">
                                                    </YAxis2>
                                                </PlotArea>
                                                <ChartTitle>
                                                    <Appearance Dimensions-Margins="4%, 10px, 14px, 6%">
                                                        <FillStyle MainColor="">
                                                        </FillStyle>
                                                    </Appearance>
                                                    <TextBlock Text="Geo Pie">
                                                        <Appearance TextProperties-Color="White" TextProperties-Font="Verdana, 14pt">
                                                        </Appearance>
                                                    </TextBlock>
                                                </ChartTitle>
                                            </telerik:RadChart>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Chart ID="Chart1" EnableViewState="false" runat="server" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)"
                                                ViewStateMode="Disabled" Width="700px" Visible="false">
                                                <Series>
                                                    <asp:Series Name="Series1" Legend="Legend1">
                                                    </asp:Series>
                                                </Series>
                                                <ChartAreas>
                                                    <asp:ChartArea Name="ChartArea1">
                                                        <Area3DStyle Enable3D="false" />
                                                    </asp:ChartArea>
                                                </ChartAreas>
                                            </asp:Chart>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Panel ID="panel_linechart" runat="server" Visible="false">
                                                <asp:Chart ID="Chart2" runat="server" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)"
                                                    EnableViewState="false" Width="700px">
                                                    <Legends>
                                                        <asp:Legend Name="DefaultLegend" Docking="Top" />
                                                    </Legends>
                                                    <Series>
                                                        <asp:Series Name="Fuel" ChartType="Line" ChartArea="ChartArea1">
                                                        </asp:Series>
                                                        <asp:Series Name="Speed" ChartType="Line" ChartArea="ChartArea1">
                                                        </asp:Series>
                                                    </Series>
                                                    <ChartAreas>
                                                        <asp:ChartArea Name="ChartArea1">
                                                        </asp:ChartArea>
                                                    </ChartAreas>
                                                </asp:Chart>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Panel ID="panel_temp" runat="server" Visible="false">
                                                <asp:Chart ID="Chart3" runat="server" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)"
                                                    EnableViewState="false" Width="700px">
                                                    <Legends>
                                                        <asp:Legend Name="DefaultLegend" Docking="Top" />
                                                    </Legends>
                                                    <Series>
                                                        <asp:Series Name="Temprature" ChartType="Line" ChartArea="ChartArea1" YValuesPerPoint="2">
                                                        </asp:Series>
                                                    </Series>
                                                    <ChartAreas>
                                                        <asp:ChartArea Name="ChartArea1">
                                                        </asp:ChartArea>
                                                    </ChartAreas>
                                                </asp:Chart>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>
        </telerik:RadPane>
    </telerik:RadSplitter>
</asp:Content>
