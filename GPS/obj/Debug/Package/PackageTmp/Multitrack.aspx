<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Multitrack" Codebehind="Multitrack.aspx.cs" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headerdata" runat="Server">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
 <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?k&sensor=true&v=3">
    </script>
    <script src="Scripts/multiTrack.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentdata" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Width="100%" Height="100%"
        BorderStyle="Ridge" Skin="Transparent">
        <telerik:RadPane ID="leftpane" runat="server" Width="36px" SkinID="Transparent">
            <telerik:RadSlidingZone ID="leftSliderZone" runat="server" SlideDirection="right"
                Width="36px" SkinID="Transparent" DockedPaneId="leftSlider">
                <telerik:RadSlidingPane ID="leftSlider" Title="Carriers" runat="server" Width="200px"
                    Height="70px" SkinID="Transparent" Scrolling="None" IconUrl="~/icons/leftSlider.png">
                    <div style="height: 100%">
                        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel3"
                            RenderMode="Inline" Style="height: 100%">
                            <asp:UpdatePanel ID="UpdatePanelCarListBox" runat="server" UpdateMode="Conditional"
                                style="height: 85%">
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
                                    <telerik:RadListBox ID="car_listbox" runat="server" AutoPostBack="true" SelectionMode="Multiple"
                                        Width="100%" Height="100%" CheckBoxes="True" OnItemCheck="car_listbox_ItemCheck"
                                        Skin="Transparent" EnableViewState="true" ViewStateMode="Enabled">
                                    </telerik:RadListBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </telerik:RadAjaxPanel>
                    </div>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel3" runat="server" Skin="Transparent">
                    </telerik:RadAjaxLoadingPanel>
                </telerik:RadSlidingPane>
            </telerik:RadSlidingZone>
        </telerik:RadPane>
        <telerik:RadSplitBar ID="RadSplitBar1" runat="server" CollapseMode="Both" BackColor="Maroon"
            BorderColor="Maroon" BorderStyle="Ridge" EnableViewState="false">
        </telerik:RadSplitBar>
        <telerik:RadPane ID="middlePane" runat="server" Height="100%" Scrolling="None">
            <telerik:RadSplitter ID="RadSplitter2" runat="server" Width="100%" Height="100%"
                Skin="Transparent" ResizeWithParentPane="false">
                <telerik:RadPane ID="RadPane1" runat="server" Width="100%" Height="100%" Scrolling="None">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" style="height: 100%">
                        <ContentTemplate>
                            <telerik:RadDockZone ID="playZone" runat="server" Height="100%" Width="99%" Orientation="Horizontal">
                                <telerik:RadDock ID="Map1" ForeColor="BlueViolet" runat="server" Width="50%" Height="50%"
                                    Skin="Transparent" DockHandle="None" DefaultCommands="None">
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Timer ID="Timer1" runat="server" Interval="10000" OnTick="Timer1_Tick" Enabled="false">
                                                </asp:Timer>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <table width="100%" style="height: 95%">
                                            <tr style="height: 100%">
                                                <td style="height: 100%">
                                                    <div id="map_canvas1" style="width: 100%; height: 100%">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="mapDiv1">
                                        </div>
                                    </ContentTemplate>
                                </telerik:RadDock>
                                <telerik:RadDock ID="Map2" ForeColor="BlueViolet" runat="server" Width="50%" Height="50%"
                                    Skin="Transparent" DockHandle="None" DefaultCommands="None">
                                    <ContentTemplate>
                                        <table width="100%" style="height: 95%">
                                            <tr style="height: 100%">
                                                <td style="height: 100%">
                                                    <div id="map_canvas2" style="width: 100%; height: 100%">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="mapDiv2">
                                        </div>
                                    </ContentTemplate>
                                </telerik:RadDock>
                                <telerik:RadDock ID="Map3" ForeColor="BlueViolet" runat="server" Width="50%" Height="50%"
                                    Skin="Transparent" DockHandle="None" DefaultCommands="None">
                                    <ContentTemplate>
                                        <table width="100%" style="height: 95%">
                                            <tr style="height: 100%">
                                                <td style="height: 100%">
                                                    <div id="map_canvas3" style="width: 100%; height: 100%">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="mapDiv3">
                                        </div>
                                    </ContentTemplate>
                                </telerik:RadDock>
                                <telerik:RadDock ID="Map4" ForeColor="BlueViolet" runat="server" Width="50%" Height="50%"
                                    Skin="Transparent" DockHandle="None" DefaultCommands="None">
                                    <ContentTemplate>
                                        <table width="100%" style="height: 95%">
                                            <tr style="height: 100%">
                                                <td style="height: 100%">
                                                    <div id="map_canvas4" style="width: 100%; height: 100%">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="mapDiv4">
                                        </div>
                                    </ContentTemplate>
                                </telerik:RadDock>
                            </telerik:RadDockZone>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </telerik:RadPane>
            </telerik:RadSplitter>
        </telerik:RadPane>
    </telerik:RadSplitter>  
</asp:Content>
