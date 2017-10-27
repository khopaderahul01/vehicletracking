<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Comparison" Codebehind="Comparison.aspx.cs" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Charting" tagprefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerdata" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentdata" runat="Server">
    <script type="text/javascript">
        function pageLoad(sender, e) {
            if (!e.get_isPartialLoad()) {
                __doPostBack('<%= UpdatePanelCarListBox.ClientID %>', 'aaaa');               
            }
        }     
    </script>
    <table align="center" width="100%">
        <tr>
            <td>
                <div id="MainContainerDiv" style="height: 630px; width: 100%;">
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    <!------------------------------------------------------------------------------------------------------------------------------>
                    <div>
                        <telerik:RadSplitter ID="RadSplitter1" runat="server" Width="100%" Height="650px"
                            BorderStyle="Ridge" Skin="Transparent" >
                            <telerik:RadPane ID="leftpane" runat="server" Width="36px" SkinID="Transparent">
                                <telerik:RadSlidingZone ID="leftSliderZone" runat="server" SlideDirection="right"
                                    Width="36px" SkinID="Transparent" ClickToOpen="true">
                                    <telerik:RadSlidingPane ID="dateSlider"  runat="server" Width="200px" EnableAjaxSkinRendering="true"
                                        SkinID="Transparent" Title="Carriers" IconUrl="~/icons/leftSlider.png" DockOnOpen="true" TabView="ImageOnly" EnableTheming="true">
                                        <table align="center">
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
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="DateBoxError" runat="server" Text="" Style="color: Red; font-weight: bolder"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </telerik:RadSlidingPane>
                                    <telerik:RadSlidingPane ID="carrier"  runat="server" Width="250px"
                                        EnableAjaxSkinRendering="true" SkinID="Transparent"  DockOnOpen="true" 
                                        Title="Carriers" IconUrl="~/icons/leftSlider.png" TabView="ImageOnly" EnableTheming="true">
                                        <div>
                                            <asp:UpdatePanel ID="UpdatePanelCarListBox" runat="server" UpdateMode="Conditional"
                                                OnPreRender="upUpdatePanel_PreRender">
                                                <ContentTemplate>
                                                    <telerik:RadListBox ID="car_listbox" runat="server" AutoPostBack="true" SelectionMode="Multiple"
                                                        Width="100%" Height="315px" CheckBoxes="True" Skin="Web20" EnableViewState="true"
                                                        ViewStateMode="Enabled" LoadingPanelID="RadAjaxLoadingPanel1">
                                                    </telerik:RadListBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanelCarListBox"
                                                runat="server">
                                                <ProgressTemplate>
                                                    <div style="position: relative; top: -200px; text-align: center;">
                                                        <asp:Image ID="ImageGif" runat="server" ImageUrl="~/images/image_311127.gif" />
                                                        <br />
                                                    </div>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>
                                    </telerik:RadSlidingPane>
                                    <telerik:RadSlidingPane ID="parameterSlider" runat="server" Width="250px"
                                        EnableAjaxSkinRendering="true" SkinID="Transparent"
                                        Title="Carriers" IconUrl="~/icons/leftSlider.png" DockOnOpen="true" TabView="ImageOnly">
                                        <div>
                                        </div>
                                    </telerik:RadSlidingPane>
                                </telerik:RadSlidingZone>
                            </telerik:RadPane>
                            <telerik:RadSplitBar ID="RadSplitBar1" runat="server" CollapseMode="Both" BackColor="Maroon"
                                BorderColor="Maroon" BorderStyle="Ridge" EnableViewState="false">
                            </telerik:RadSplitBar>
                            <telerik:RadPane ID="middlePane" runat="server" Width="100%" Height="100%" Scrolling="None">
                               
                                <div align="center">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <telerik:RadButton ID="RadButton1" Text="Compare" runat="server" OnClick="RadButton1_Click">
                                            </telerik:RadButton>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanelChartKm" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Timer ID="TimerChartKm" runat="server" OnTick="TimerTickChartKm" Interval="2000">
                                        </asp:Timer>
                                        <table align="center">
                                            <tr>
                                                <td align="center">
                                                    <telerik:RadChart ID="ChartKm" Width="590px" AutoLayout="true"  Visible="true" DefaultType="Bar" runat="server">

                                                    </telerik:RadChart>
                                                </td>
                                                <td>
                                                    <telerik:RadChart ID="ChartkmDayWise" Width="590px" Visible="true" runat="server" DefaultType="Line">
                                                    </telerik:RadChart>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanelChartKm"
                                                        runat="server">
                                                        <ProgressTemplate>
                                                            <div style="position: relative; top: -180px; text-align: center;">
                                                                <asp:Image ID="ImageGifa" runat="server" ImageUrl="~/images/image_311127.gif" />
                                                                <br />
                                                            </div>
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </td>
                                                 <td align="center">
                                                    <asp:UpdateProgress ID="UpdateProgress3" AssociatedUpdatePanelID="UpdatePanelChartKm"
                                                        runat="server">
                                                        <ProgressTemplate>
                                                            <div style="position: relative; top: -180px; text-align: center;">
                                                                <asp:Image ID="ImageGifaa" runat="server" ImageUrl="~/images/image_311127.gif" />
                                                                <br />
                                                            </div>
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                       
                            </telerik:RadPane>
                        </telerik:RadSplitter>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
