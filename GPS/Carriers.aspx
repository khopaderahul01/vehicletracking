we<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master"
    CodeBehind="Carriers.aspx.cs" Inherits="GPS.Carriers" %>
    
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerdata" runat="server">
    <link href="Styles/carriers.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function showAlert(message) {
            alert(message);
        }
      
    </script>
    <style type="text/css">
        .style1
        {
            width: 192px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentdata" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <div>
            <asp:Label ID="lblCarrierLimitTop" runat="server" Text=""></asp:Label>
        </div>
        <div>
            <asp:Label ID="lblOrgLimitTop" runat="server" Text=""></asp:Label>
        </div>
    </div>
    <div align="center">
        <telerik:radajaxpanel id="RadAjaxPanel1" runat="server" loadingpanelid="RadAjaxLoadingPanel3"
            rendermode="Inline" style="height: 100%; z-index: -555">
        <asp:UpdatePanel ID="updatePanelGrid" runat="server">
            <ContentTemplate>
                <asp:Panel ID="PanelGrid" runat="server">
                    <table cellpadding="0" cellspacing="0" id="Table1">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Carriers Details"></asp:Label>
                                &nbsp;&nbsp;
                                <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add New" CssClass="button" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                                <asp:Label ID="lblmsg" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <%--<asp:ImageButton ID="btnAdd_Car" runat ="server" ImageUrl ="~/images/add.gif" 
                        />--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:radgrid id="CarGrid" runat="server" allowfilteringbycolumn="true" allowpaging="True"
                                    allowsorting="True" autogeneratecolumns="false" pagesize="10" skin="Windows7"
                                    width="100%" onitemcommand="CarGrid_ItemCommand" onpageindexchanged="CarGrid_PageIndexChanged"
                                    onpagesizechanged="CarGrid_pageSize" onsortcommand="CarGrid_SortCommand">
                                                <GroupingSettings CaseSensitive="false" />
                                                <ClientSettings EnablePostBackOnRowClick="false">
                                                   <%-- <Selecting AllowRowSelect="false" />--%>
                                                    <%--<ClientEvents OnRowMouseOver="grdUsers_RowMouseOver" />--%>
                                                </ClientSettings>
                                                <MasterTableView DataKeyNames="ID" TableLayout="Auto">
                                                    <Columns>
                                                        <telerik:GridTemplateColumn AllowFiltering="false">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton ToolTip="View" ID="btnView" CssClass="editImg" ImageUrl="~/images/Iconedit.png"
                                                                    CausesValidation="false" CommandName="EditGrid" CommandArgument='<%# Bind("ID") %>'
                                                                    runat="server" Style="float: left;" />
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:ImageButton ToolTip="Delete" ID="btnDelete" CssClass="deleteImg" CausesValidation="false"
                                                                    ImageUrl="~/images/Icondelete.png" CommandName="DeleteGrid" CommandArgument='<%# Bind("ID") %>'
                                                                    runat="server" Style="float: right;" OnClientClick="return confirm('Do you want to delete this record?');" />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>                                                       
                                                        <telerik:GridBoundColumn DataField="Name" HeaderStyle-HorizontalAlign="Center" HeaderText="Name"
                                                            ItemStyle-HorizontalAlign="left" ShowFilterIcon="true" UniqueName="ProductName"
                                                            AllowFiltering="true" AutoPostBackOnFilter="True" CurrentFilterFunction="Contains">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Track Type" HeaderStyle-HorizontalAlign="Center"
                                                            HeaderText="Carrier Type" ItemStyle-HorizontalAlign="left" ShowFilterIcon="true"
                                                            UniqueName="qty" AllowFiltering="true" AutoPostBackOnFilter="True" CurrentFilterFunction="Contains">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="IMEI" HeaderStyle-HorizontalAlign="Center" HeaderText="IMEI"
                                                            ItemStyle-HorizontalAlign="Right" ShowFilterIcon="true" UniqueName="Price" AllowFiltering="true"
                                                            AutoPostBackOnFilter="True" CurrentFilterFunction="Contains">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="GSM No" HeaderText="GSM No" ItemStyle-HorizontalAlign="Right"
                                                            ShowFilterIcon="false" UniqueName="A" AllowFiltering="false">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Organisation Name" HeaderStyle-HorizontalAlign="Center"
                                                            HeaderText="Organisation" ItemStyle-HorizontalAlign="left" ShowFilterIcon="true"
                                                            UniqueName="B" AllowFiltering="true" AutoPostBackOnFilter="True" CurrentFilterFunction="Contains">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Zone Name" HeaderText="Time Zone" ItemStyle-HorizontalAlign="Center"
                                                            ShowFilterIcon="false" UniqueName="C" AllowFiltering="false" >
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Center"
                                                            ShowFilterIcon="false" UniqueName="Status" AllowFiltering="false">
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                    <PagerStyle AlwaysVisible="true" />
                                                </MasterTableView>
                                            </telerik:radgrid>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
             <Triggers>
                <asp:PostBackTrigger ControlID="btnsave"  />
                <asp:PostBackTrigger ControlID="btnCancel" />                
            </Triggers>
        </asp:UpdatePanel>
              </telerik:radajaxpanel> 
        <asp:UpdatePanel ID="updatePanelAdd" runat="server">
            <ContentTemplate>
                <asp:Panel ID="PanelCarAdd" runat="server" Visible="false" Style="width: 100%;">
                    <table width="100%" align="center">
                        <tr>
                            <td>
                                <b>
                                    <asp:Label ID="lblCARDetail" Text="Carrier Detail" runat="server"></asp:Label></b>&nbsp;
                            </td>
                        </tr>
                    </table>
                    <table style="float: left; width: 41%;">
                        <tr>
                            <td id="organisationtr2" runat="server">
                                <asp:Label ID="lblOrgNm" runat="server" Text="Organization"></asp:Label>
                                <font style="color: Red">*</font>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td id="organisationtr1" runat="server">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <telerik:radcombobox id="ddlOrgNm" runat="server" autopostback="true" onselectedindexchanged="ddlOrgNm_SelectedIndexChanged">
                                            </telerik:radcombobox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td class="style1">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlOrgNm"
                                    Display="Dynamic" ErrorMessage="please select Organisaton" ForeColor="#CC3300"
                                    SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr id="TrUserType" runat="server">
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="User Type"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:RadioButton ID="Existing" Text="Existing" AutoPostBack="true" runat="server"
                                    GroupName="checkboxes" OnCheckedChanged="Existing_CheckedChanged" />
                                &nbsp; &nbsp; &nbsp;
                                <asp:RadioButton ID="NewUser" Text="New User" AutoPostBack="true" runat="server"
                                    GroupName="checkboxes" OnCheckedChanged="NewUser_CheckedChanged" />
                            </td>
                        </tr>
                        <tr id="trExistingUser" runat="server">
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="User"></asp:Label>
                                <font style="color: Red">*</font>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:radcombobox id="ddlUser" runat="server" tabindex="2">
                                    </telerik:radcombobox>
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
                                <telerik:radtextbox id="txtUserName" runat="server" tabindex="1">
                                    </telerik:radtextbox>
                            </td>
                            <td class="style1">
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
                                <telerik:radtextbox id="txtUserID" runat="server" tabindex="2">
                                    </telerik:radtextbox>
                            </td>
                            <td class="style1">
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
                                <telerik:radtextbox id="txtPassword" runat="server" textmode="Password" tabindex="3">
                                    </telerik:radtextbox>
                            </td>
                            <td class="style1">
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
                                <telerik:radtextbox id="txtConfirmPwd" runat="server" textmode="Password" tabindex="4">
                                    </telerik:radtextbox>
                            </td>
                            <td class="style1">
                                <asp:RequiredFieldValidator ID="RFVConfirmPwd" runat="server" ControlToValidate="txtConfirmPwd"
                                    ErrorMessage="Enter Confirm Password" Font-Size="Small" ForeColor="#CC3300" ValidationGroup="save"
                                    Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CMPConfirmPwd" runat="server" ControlToValidate="txtConfirmPwd"
                                    ControlToCompare="txtPassword" ErrorMessage="Confirm Password Not match" ForeColor="#CC3300"
                                    Font-Size="Small" ValidationGroup="save" Display="Dynamic"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr id="trEmailID" runat="server">
                            <td>
                                <asp:Label ID="lblEmailId" runat="server" Text="Email Id"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:radtextbox id="txtEmailId" runat="server" tabindex="5">
                                    </telerik:radtextbox>
                            </td>
                            <td class="style1">
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtEmailId"
                ErrorMessage="Please enter email-id" Font-Size="Small" ForeColor="#CC3300"
                ValidationGroup="save" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEmailId"
                                    ErrorMessage="Enter Valid Email" Font-Size="Small" ForeColor="#CC3300" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ValidationGroup="save" Display="Dynamic"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr id="trExpiry" runat="server">
                            <td>
                                <asp:Label ID="lblExpiry" runat="server" Text="Expiry On"></asp:Label>
                                <font color="red">*</font>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:raddatepicker id="ExpiryDate" runat="server" tabindex="6">
                                    </telerik:raddatepicker>
                            </td>
                            <td class="style1">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ExpiryDate"
                                    ErrorMessage="select Expiry date" Font-Size="Small" ForeColor="#CC3300" ValidationGroup="save"
                                    Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCarrierName" runat="server" Text="Carrier Name"></asp:Label>
                                <font style="color: Red">*</font>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:radtextbox id="txtCarName" runat="server" tabindex="1">
                                    </telerik:radtextbox>
                            </td>
                            <td class="style1">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCarName"
                                    Display="Dynamic" ErrorMessage="please enter carrier name" ForeColor="#CC3300"
                                    SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDeviceModel" runat="server" Text="Device Type"></asp:Label>
                                <font style="color: Red">*</font>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <%--   <asp:DropDownList ID="ddlCarrier" runat="server">
</asp:DropDownList>--%>
                                <telerik:radcombobox id="ddlCarrier" runat="server" tabindex="2">
                                    </telerik:radcombobox>
                            </td>
                            <td class="style1">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlCarrier"
                                    Display="Dynamic" ErrorMessage="please select Device Type" ForeColor="#CC3300"
                                    SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDeviceIMEI" runat="server" Text="Device IMEI"></asp:Label>
                                <font style="color: Red">*</font>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:radtextbox id="txtImeNo" runat="server" tabindex="3">
                                    </telerik:radtextbox>
                                <br />
                                <asp:Label ID="lblIMEIError" runat="server" ForeColor="#CC3300"></asp:Label>
                            </td>
                            <td class="style1">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtImeNo"
                                    Display="Dynamic" ErrorMessage="please enter IMEI number" ForeColor="#CC3300"
                                    SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server"
                                    ControlToValidate="txtImeNo" ErrorMessage="Please Enter only Number" ForeColor="#CC3300"
                                    ValidationExpression="\d+" ValidationGroup="save" Display="Dynamic"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblGSMNumber" runat="server" Text="GSM Number"></asp:Label>
                                <font style="color: Red">*</font>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:radtextbox id="txtGsmNo" runat="server" tabindex="4">
                                    </telerik:radtextbox>
                            </td>
                            <td class="style1">
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server"
                                    ControlToValidate="txtGsmNo" Display="Dynamic" ErrorMessage="Enter only number"
                                    ForeColor="#CC3300" SetFocusOnError="True" ValidationExpression="\d+" ValidationGroup="save"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtGsmNo"
                                    Display="Dynamic" ErrorMessage="please enter GSM number " ForeColor="#CC3300"
                                    SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblAPNUserName" runat="server" Text="APN Name"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <%--  <asp:DropDownList ID="ddlOrgName" runat="server">
</asp:DropDownList>--%>
                                <telerik:radtextbox id="txtAPNNames" runat="server" tabindex="5">
                                    </telerik:radtextbox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSimsService" runat="server" Text="Sim Service"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:radcombobox id="txtSimNo" runat="server" emptymessage="Select type" tabindex="6">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="Prepaid" Selected="true" />
                                            <telerik:RadComboBoxItem runat="server" Text="Postpaid" />
                                        </Items>
                                    </telerik:radcombobox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblZoneName" runat="server" Text="Zone Name"></asp:Label>
                                <font style="color: Red">*</font>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <%-- <asp:DropDownList ID="ddlZone" runat="server">
</asp:DropDownList>--%>
                                <telerik:radcombobox id="ddlZone" runat="server" tabindex="7">
                                    </telerik:radcombobox>
                            </td>
                            <td class="style1">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlZone"
                                    Display="Dynamic" ErrorMessage="Please Select Zone" ForeColor="#CC3300" ValidationGroup="save"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr id="zoneTr" runat="server" visible="false">
                            <td>
                                <asp:Label ID="lblFleet" runat="server" Text="Fleet"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:radcombobox id="ddlfleet" runat="server" tabindex="8">
                                    </telerik:radcombobox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblOverSpeed" runat="server" Text="Over Speed Threshold"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:radtextbox id="txtOverSpeed" runat="server" tabindex="9">
                                    </telerik:radtextbox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbldin2Logic" runat="server" Text="Digital Input 2 Logic"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:radcombobox id="ddlDin1Logic" runat="server" tabindex="10">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Value="1" Text="Positive" Selected="true" />
                                            <telerik:RadComboBoxItem runat="server" Value="0" Text="Negative" />
                                        </Items>
                                    </telerik:radcombobox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbldigital" runat="server" Text="Digital 2 For Ignition"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:radcombobox id="ddlDin2Logic" runat="server" tabindex="11">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Value="1" Text="Not Used" Selected="true" />
                                            <telerik:RadComboBoxItem runat="server" Value="0" Text="Used" />
                                        </Items>
                                    </telerik:radcombobox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblIconError" runat="server" ForeColor="Red"></asp:Label>
                                <br />
                                <asp:Label ID="lblLimitError" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="Summary" runat="server" ForeColor="#CC3300" ShowMessageBox="True"
                                    ShowSummary="False" ValidationGroup="save" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btnsave" runat="server" OnClick="btnsave_Click1" Style="height: 26px"
                                    Text="Save" TabIndex="21" ValidationGroup="save" CssClass="button" />
                                &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Back" CausesValidation="False"
                                    OnClick="btnCancel_Click" CssClass="button" TabIndex="22" />
                                <br />
                                <asp:Label ID="lblMessage" runat="server" ForeColor="#3333FF"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table style="float: left; width: 25%;">
                        <tr>
                            <td>
                                <asp:Label ID="lblVehChNo" runat="server" Text="Vehicle Chassis Number"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:radtextbox id="txtChasiss" runat="server" tabindex="12">
                                    </telerik:radtextbox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblVehRun" runat="server" Text="Vehicle Running"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:radtextbox id="txtRunningg" runat="server" tabindex="13">
                                    </telerik:radtextbox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblVehFual" runat="server" Text="Vehicle Fuel Capacity"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:radtextbox id="txtFual" runat="server" tabindex="14">
                                    </telerik:radtextbox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblVehAvg" runat="server" Text="Vehicle Average"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:radtextbox id="txtAvg" runat="server" tabindex="15">
                                    </telerik:radtextbox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblVehTyre" runat="server" Text="Vehicle Tyre Count"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:radtextbox id="txtTyres" runat="server" tabindex="16">
                                    </telerik:radtextbox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblLastServiceDate" runat="server" Text="Last Servicing Date"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:raddatepicker id="LastServiceDate" runat="server" tabindex="17">
                                        <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                        </Calendar>
                                        <DateInput DateFormat="dd-MMM-yy" DisplayDateFormat="dd-MMM-yy" LabelWidth="40%"
                                            TabIndex="17">
                                        </DateInput>
                                        <DatePopupButton HoverImageUrl="" ImageUrl="" TabIndex="17" />
                                    </telerik:raddatepicker>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblServiceDetails" runat="server" Text="Last Servicing Details"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:radtextbox id="txtServiceDet" runat="server" tabindex="18">
                                    </telerik:radtextbox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblApnUName" runat="server" Text="Apn user name"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <telerik:radtextbox id="txtapnname" runat="server" tabindex="19">
                                    </telerik:radtextbox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblPwd" runat="server" Text="Apn Password"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <telerik:radtextbox id="txtpass" runat="server" tabindex="20">
                                    </telerik:radtextbox>
                            </td>
                        </tr>
                    </table>
                    <asp:UpdatePanel ID="updatePanelmain" runat="server" style="width: 100%;" UpdateMode="Always">
                        <ContentTemplate>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </ContentTemplate>
           
        </asp:UpdatePanel>
       
        <telerik:radajaxloadingpanel id="RadAjaxLoadingPanel3" runat="server" skin="Transparent"
            style="z-index: -5555;">
       </telerik:radajaxloadingpanel>
    </div>
</asp:Content>
