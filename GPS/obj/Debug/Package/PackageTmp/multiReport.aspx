<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="multiReport" CodeBehind="multiReport.aspx.cs" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerdata" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentdata" runat="Server">
    <script type="text/javascript">
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
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Width="100%" Height="100%"
        BorderStyle="Ridge" Skin="Transparent">
        <telerik:RadPane ID="leftpane" runat="server" Width="36px" SkinID="Transparent">
            <telerik:RadSlidingZone ID="leftSliderZone" runat="server" SlideDirection="right"
                Width="36px" SkinID="Transparent" DockedPaneId="leftSlider">
                <telerik:RadSlidingPane ID="leftSlider" runat="server" Width="250px" EnableAjaxSkinRendering="true"
                   Height="100%"   SkinID="Transparent" EnableTheming="true" Scrolling="None" Title="Carriers" IconUrl="~/icons/leftSlider.png">
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
                  
                    <telerik:RadTabStrip ID="vehicleFleet" runat="server" Skin="Transparent" MultiPageID="vehicleFleetMPage"
                        SelectedIndex="0">
                        <Tabs>
                            <telerik:RadTab runat="server" Text="Carrier" PageViewID="vehicles" Selected="True">
                            </telerik:RadTab>
                            <telerik:RadTab runat="server" Text="Group" PageViewID="fleets">
                            </telerik:RadTab>
                        </Tabs>
                    </telerik:RadTabStrip>
                    <telerik:RadMultiPage ID="vehicleFleetMPage" runat="server" SelectedIndex="0" Style="height: 100%"> 
                        <telerik:RadPageView ID="vehicles" runat="server"  Style="height: 100%" Height="100%">
                           
                            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel3" runat="server" Skin="Transparent">
                            </telerik:RadAjaxLoadingPanel>
                            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel3"
                                RenderMode="Inline" Style="height: 100%; z-index: -555">
                                <asp:UpdatePanel ID="UpdatePanelCarListBox" runat="server" UpdateMode="Conditional"  style="height: 90%">
                                    <ContentTemplate>
                                        <asp:Timer ID="TimerListBox" runat="server" Interval="1000" OnTick="TimerListBox_Tick">
                                        </asp:Timer>
                                        <telerik:RadListBox ID="car_listbox" runat="server" AutoPostBack="false" SelectionMode="Multiple"
                                            Width="100%" Height="90%" CheckBoxes="True" OnClientItemChecked="onItemChecked"
                                            Skin="Transparent" EnableViewState="true" ViewStateMode="Enabled">
                                        </telerik:RadListBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </telerik:RadAjaxPanel>
                           
                        </telerik:RadPageView>
                        <telerik:RadPageView ID="fleets" runat="server" Style="height: 100%" Height="100%"> 
                         
                            <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="RadAjaxLoadingPanel3"
                                RenderMode="Inline" Style="height: 100%; z-index: -555">
                                <asp:UpdatePanel ID="UpdatePanelFleetListBox" runat="server" UpdateMode="Always"  style="height: 90%">
                                    <ContentTemplate>
                                        <telerik:RadListBox ID="RadListBoxFleet" runat="server" AutoPostBack="false" SelectionMode="Multiple"
                                            Width="100%" Height="90%" CheckBoxes="True" OnClientItemChecked="onItemChecked"
                                            Skin="Transparent" EnableViewState="true" ViewStateMode="Enabled">
                                        </telerik:RadListBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </telerik:RadAjaxPanel>
                           
                        </telerik:RadPageView>
                    </telerik:RadMultiPage>
                   
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
                    Height="620px">
                    <TitlebarTemplate>
                        <asp:Image ID="Image6" runat="server" ImageUrl="~/icons/reports.png" Height="24px"
                            Width="80px" />
                    </TitlebarTemplate>
                    <ContentTemplate>
                        <asp:UpdatePanel ID="UpdatePanel14" runat="server" EnableViewState="true" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="dd_reportMultiple" runat="server" OnSelectedIndexChanged="dd_reportMultiple_SelectedIndexChanged"
                                                AutoPostBack="true">
                                                <asp:ListItem Value="kmTravld">KM Travelled</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label runat="server" ID="lblStopageMultiple" Text="Stoppage Time(minutes):"
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblSpeedMultiple" runat="server" Text="Minimum Speed" Visible="false"></asp:Label>
                                            <asp:TextBox ID="txt_boldStopMultiple" runat="server" Visible="false" Width="42px"></asp:TextBox>
                                            <telerik:RadButton ID="btngenRptMultiple" runat="server" OnClick="btngenRptMultiple_Click"
                                                Text="Genrate" />
                                            <telerik:RadButton ID="btnshowchatMultiple" runat="server" Visible="false" Text="View Chart" />
                                            <telerik:RadButton ID="btnexportMultiple" runat="server" Text="Export To Excel" ButtonType="LinkButton"
                                                Visible="false" Target="_blank" NavigateUrl="~/ExportToExcelMultiple.aspx" />
                                            <telerik:RadButton ID="btnexport_pdfMultiple" AutoPostBack="true" ButtonType="LinkButton"
                                                runat="server" Text="Export To PDF" Visible="false" NavigateUrl="~/ExportToPDFMultiple.aspx" />
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
                                </table>
                                <table align="center" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="center" colspan="9">
                                            <asp:Label ID="lblRptNameMultiple" runat="server" Font-Bold="True">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Label ID="lblmsgMultiple" runat="server" ForeColor="Red" Visible="false" Font-Size="Medium"></asp:Label>
                                <asp:Label ID="lblKMMultiple" runat="server" Style="text-align: center" Font-Bold="true"
                                    Font-Size="Medium" ForeColor="Green"></asp:Label>
                                <asp:UpdateProgress ID="UpdateProgress5" AssociatedUpdatePanelID="UpdatePanel14"
                                    runat="server">
                                    <ProgressTemplate>
                                        <asp:Image ID="Image1Multiple" runat="server" ImageUrl="~/images/ajax-loader (1).gif" /><asp:Label
                                            ID="msglblMultiple" runat="server" Text="Loading"></asp:Label></ProgressTemplate>
                                </asp:UpdateProgress>
                                <asp:UpdatePanel ID="UpdatePanel15" runat="server" EnableViewState="true" ChildrenAsTriggers="true">
                                    <ContentTemplate>
                                        <telerik:RadGrid ID="gv_ReportMultiple" runat="server" AllowSorting="True" Skin="Transparent"
                                            ClientSettings-AllowExpandCollapse="true" Width="99%" OnSortCommand="gv_Report_SortCommand"
                                            CellSpacing="0" GridLines="None" EnableViewState="true">
                                            <MasterTableView EnableColumnsViewState="false">
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel16" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblGeoVisitMultiple" runat="server" Text="Geofences not visited:, "
                                            Font-Bold="true" ForeColor="Red" Visible="true"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <table>
                                    <tr>
                                        <td>
                                            <telerik:RadChart ID="pieChartMultiple" runat="server" DefaultType="Pie" Visible="false"
                                                Height="600px" Width="600px">
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
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Chart ID="Chart1Multiple" runat="server" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)"
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
                                </table>
                                <asp:Panel ID="panel2" runat="server" Visible="false">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Chart ID="Chart2Multiple" runat="server" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)"
                                                    ViewStateMode="Disabled" Width="700px">
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
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="panel3" runat="server" Visible="false">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Chart ID="Chart3Multiple" runat="server" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)"
                                                    ViewStateMode="Disabled" Width="700px">
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
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>
        </telerik:RadPane>
    </telerik:RadSplitter>
</asp:Content>
