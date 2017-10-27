<%@ Page Title="" Language="C#" MasterPageFile="~/orgAdmin.Master" AutoEventWireup="true" CodeBehind="userRegAdd.aspx.cs" Inherits="GPS.userReg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerdata" runat="server">
    <link href="Styles/org.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentdata" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    
    <div id="MainContainerDiv" style="height: 630px; width: 100%;">
        <!------------------------------------------------------------------------------------------------------------------------------>
        <div style="height: 100%;">
        
            <telerik:RadSplitter ID="RadSplitter1" runat="server" Width="100%" Height="100%"
                BorderStyle="Ridge" Skin="Transparent">
                <telerik:RadPane ID="leftpane" runat="server" Width="36px" SkinID="Transparent">
                    <telerik:RadSlidingZone ID="leftSliderZone" runat="server" SlideDirection="right"
                        Width="36px" SkinID="Transparent" DockedPaneId="leftSlider">
                        <telerik:RadSlidingPane ID="leftSlider" Title="Carriers" runat="server" Width="200px"
                            Height="70px" SkinID="Transparent" Scrolling="None" IconUrl="~/icons/leftSlider.png">
                         
                                    <div style="height:100%">
                                        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel3"
                                            RenderMode="Inline" style="height:100%" >
                                            <asp:UpdatePanel ID="UpdatePanelCarListBox" runat="server" UpdateMode="Conditional" style="height:100%">
                                                <ContentTemplate>
                                                  
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
                                                        Width="100%" Height="90%" CheckBoxes="True" OnItemCheck="car_listbox_ItemCheck"
                                                        Skin="Web20" EnableViewState="true" ViewStateMode="Enabled">
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
                         
                            <table align="left" cellpadding="5" cellspacing="5">
                                <tr>
                                    <td style="width: 150px">
                                        <span><b>User Details</b></span> &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td style="width: 200px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr id="trUserName" runat="server">
                                    <td>
                                        <asp:Label ID="lblUserName" runat="server" Text="Display Name"></asp:Label>
                                        <font color="red">*</font>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtUserName" runat="server" TabIndex="1">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RFVUserName" runat="server" ControlToValidate="txtUserName"
                                            ErrorMessage="Enter User Name" Font-Size="Small" ForeColor="#CC3300" ValidationGroup="save"
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr id="trUserID" runat="server">
                                    <td>
                                        <asp:Label ID="Label14" runat="server" Text="User ID"></asp:Label>
                                        <font color="red">*</font>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtUserID" runat="server" TabIndex="2">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtUserID"
                                            ErrorMessage="Enter User ID" Font-Size="Small" ForeColor="#CC3300" ValidationGroup="save"
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr id="trPassword" runat="server">
                                    <td>
                                        <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>
                                        <font color="red">*</font>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtPassword" runat="server" TextMode="Password" 
                                            TabIndex="3">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtPassword"
                                            ErrorMessage="Password must be atleast 6 or more Characters long." ForeColor="Red"
                                            ValidationExpression="[a-zA-Z0-9]{6,50}"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="RFVPassword" runat="server" ControlToValidate="txtPassword"
                                            ErrorMessage="Enter Password" Font-Size="Small" ForeColor="#CC3300" ValidationGroup="save"
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr id="trConfirmPassword" runat="server">
                                    <td>
                                        <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password"></asp:Label>
                                        <font color="red">*</font>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtConfirmPwd" runat="server" TextMode="Password" 
                                            TabIndex="4">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RFVConfirmPwd" runat="server" ControlToValidate="txtConfirmPwd"
                                            ErrorMessage="Enter Confirm Password" Font-Size="Small" ForeColor="#CC3300" ValidationGroup="save"
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CMPConfirmPwd" runat="server" ControlToValidate="txtConfirmPwd"
                                            ControlToCompare="txtPassword" ErrorMessage="Confirm Password Not match" ForeColor="#CC3300"
                                            Font-Size="Small" ValidationGroup="save" Display="Dynamic"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblEmailId" runat="server" Text="Email Id"></asp:Label>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtEmailId" runat="server" TabIndex="5">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtEmailId"
                            ErrorMessage="Please enter email-id" Font-Size="Small" ForeColor="#CC3300"
                            ValidationGroup="save" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEmailId"
                                            ErrorMessage="Enter Valid Email" Font-Size="Small" ForeColor="#CC3300" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                            ValidationGroup="save" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblExpiry" runat="server" Text="Expiry On"></asp:Label>
                                        <font color="red">*</font>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="ExpiryDate" runat="server" TabIndex="6">
                                        </telerik:RadDatePicker>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ExpiryDate"
                                            ErrorMessage="select Expiry date" Font-Size="Small" ForeColor="#CC3300" ValidationGroup="save"
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ValidationSummary ID="Summary" runat="server" ForeColor="#CC3300" ShowMessageBox="false"
                                            ShowSummary="False" ValidationGroup="save" />
                                    </td>
                                    <td>
                                    </td>
                                    <td  style="padding-top: 30px">
                                        <asp:Button ID="btnInsert" runat="server" OnClick="btnInsert_Click" Style="height: 26px"
                                            Text="Save" ValidationGroup="save" CssClass="button" TabIndex="7" />
                                        &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Back" CausesValidation="False"
                                            OnClick="btnCancel_Click" CssClass="button" TabIndex="8" />
                                        &nbsp;
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="#3333FF"></asp:Label>
                                    </td>
                                    <td style="padding-top: 30px">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </telerik:RadPane>
                    </telerik:RadSplitter>
                </telerik:RadPane>
            </telerik:RadSplitter>
        </div>
    </div>
</asp:Content>
