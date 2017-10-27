<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Company.aspx.cs" Inherits="GPS.Company" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerdata" runat="server">
    <link href="Styles/company.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
      
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentdata" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center">
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel3"
            RenderMode="Inline" Style="height: 100%; z-index: -555">
            <asp:UpdatePanel ID="updatePanelGrid" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="PanelGrid" runat="server">
                        <table class="style1">
                            <tr>
                                <td class="style2">
                                    <asp:Label ID="Label1" runat="server" Text="Company Details"></asp:Label>
                                    &nbsp;
                                    <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add New" CssClass="button" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    <asp:Label ID="lblmsg" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    <telerik:RadGrid ID="CompGrid" runat="server" AllowFilteringByColumn="true" AllowPaging="True"
                                        AllowSorting="True" AutoGenerateColumns="false" PageSize="10" Skin="Windows7"
                                        Width="99%" OnItemCommand="CompGrid_ItemCommand" OnPageIndexChanged="CompGrid_PageIndexChanged"
                                        OnPageSizeChanged="CompGrid_PageSize" OnSortCommand="CompGrid_SortCommand">
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings EnablePostBackOnRowClick="false">
                                            <Selecting AllowRowSelect="True" />
                                            <%-- <ClientEvents OnRowMouseOver="grdUsers_RowMouseOver" />--%>
                                        </ClientSettings>
                                        <MasterTableView DataKeyNames="ID">
                                            <Columns>
                                                <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ToolTip="View" ID="btnView" CssClass="editImg" CausesValidation="false"
                                                            ImageUrl="~/images/Iconedit.png" CommandName="EditGrid" CommandArgument='<%# Bind("ID") %>'
                                                            runat="server" Style="float: left;" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:ImageButton ToolTip="Delete" ID="btnDelete" CssClass="deleteImg" CausesValidation="false"
                                                            CommandName="DeleteGrid" CommandArgument='<%# Bind("ID") %>' runat="server" Style="float: right;"
                                                            OnClientClick="return confirm('Do you want to delete this record?');" ImageUrl="~/images/Icondelete.png" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="Name" HeaderStyle-HorizontalAlign="Center" HeaderText="Company"
                                                    ItemStyle-HorizontalAlign="left" ShowFilterIcon="true" AllowFiltering="true"
                                                    AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" UniqueName="ProductName">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Website" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderText="Website" ItemStyle-HorizontalAlign="left" ShowFilterIcon="true"
                                                    FilterControlWidth="85%" AllowFiltering="true" UniqueName="qty">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Email" HeaderStyle-HorizontalAlign="Center" HeaderText="Email"
                                                    AllowFiltering="true" ItemStyle-HorizontalAlign="left" ShowFilterIcon="true"
                                                    FilterControlWidth="85%" UniqueName="Price">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Contact" DataType="System.String" HeaderText="Contact"
                                                    ItemStyle-HorizontalAlign="right" ShowFilterIcon="true" AllowFiltering="true"
                                                    UniqueName="Contact" AutoPostBackOnFilter="True" CurrentFilterFunction="Contains">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Address" HeaderText="Address" ItemStyle-HorizontalAlign="left"
                                                    AllowFiltering="false" ShowFilterIcon="false" UniqueName="Address">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Created On" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderText="Created On" ItemStyle-HorizontalAlign="left" ShowFilterIcon="false"
                                                    AllowFiltering="false" UniqueName="CreatedOn">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Expiry On" HeaderText="Expires On" ItemStyle-HorizontalAlign="left"
                                                    AllowFiltering="false" ShowFilterIcon="false" UniqueName="Expiry">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn DataField="totalLicense" HeaderText="License" ItemStyle-HorizontalAlign="right"
                                                    AllowFiltering="false" ShowFilterIcon="false" UniqueName="totalLicense">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn DataField="usedLicense" HeaderText="Used License" ItemStyle-HorizontalAlign="right"
                                                    AllowFiltering="false" ShowFilterIcon="false" UniqueName="usedLicense">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn DataField="remainingLicnese" HeaderText="Remaining Licnese" ItemStyle-HorizontalAlign="right"
                                                    AllowFiltering="false" ShowFilterIcon="false" UniqueName="remainingLicnese">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Center"
                                                    AllowFiltering="false" ShowFilterIcon="false" UniqueName="Status">
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                            <PagerStyle AlwaysVisible="true" />
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnsave" />
                    <asp:PostBackTrigger ControlID="btnCancel" />
                </Triggers>
            </asp:UpdatePanel>
        </telerik:RadAjaxPanel>
        <asp:UpdatePanel ID="updatePanelAdd" runat="server">
            <ContentTemplate>
                <asp:Panel ID="PanelCompAdd" runat="server" Visible="false">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Company Name"></asp:Label>
                                <font color="red">*</font>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCompName" runat="server" TabIndex="1">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCompName"
                                    ErrorMessage="Enter Company Name" Font-Size="Small" ForeColor="Red" ValidationGroup="save"
                                    Display="Dynamic"></asp:RequiredFieldValidator>
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
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="Address"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtAddress" runat="server" TabIndex="2">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <%--<td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAddress"
                    ErrorMessage="Enter Address" Font-Size="Small" ValidationGroup="save" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>--%>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Email ID"></asp:Label>
                                <font color="red">*</font>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtemail" runat="server" TabIndex="3">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtemail"
                                    ErrorMessage="Enter Email ID" Font-Size="Small" ForeColor="Red" ValidationGroup="save"
                                    Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtemail"
                                    ErrorMessage="Enter Valid Email" Font-Size="Small" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ForeColor="Red" ValidationGroup="save" Display="Dynamic"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr id="trUserName1" runat="server">
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
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
                                &nbsp;
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtUserName" runat="server" TabIndex="4">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RFVUserName" runat="server" ControlToValidate="txtUserName"
                                    ErrorMessage="Enter User Name" Font-Size="Small" ForeColor="Red" ValidationGroup="save"
                                    Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr id="tr2" runat="server">
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
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
                                &nbsp;
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtUserID" runat="server" TabIndex="4">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtUserID"
                                    ErrorMessage="Enter User ID" Font-Size="Small" ForeColor="Red" ValidationGroup="save"
                                    Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr id="trPassword1" runat="server">
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
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
                                &nbsp;
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtPassword" runat="server" TextMode="Password" TabIndex="5">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RFVPassword" runat="server" ControlToValidate="txtPassword"
                                    ErrorMessage="Enter Password" Font-Size="Small" ForeColor="Red" ValidationGroup="save"
                                    Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtPassword"
                                    ErrorMessage="Password must be atleast 6 or more Characters long." ForeColor="Red"
                                    ValidationExpression="[a-zA-Z0-9]{6,50}"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr id="trConfirmPassword1" runat="server">
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
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
                                &nbsp;
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtConfirmPwd" runat="server" TextMode="Password" TabIndex="6">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RFVConfirmPwd" runat="server" ControlToValidate="txtConfirmPwd"
                                    ErrorMessage="Enter Confirm Password" Font-Size="Small" ForeColor="Red" ValidationGroup="save"
                                    Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CMPConfirmPwd" runat="server" ControlToValidate="txtConfirmPwd"
                                    ControlToCompare="txtPassword" ErrorMessage="Confirm Password Not match" Font-Size="Small"
                                    ForeColor="Red" ValidationGroup="save" Display="Dynamic"></asp:CompareValidator>
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
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label12" runat="server" Text="Website"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtwebsite" runat="server" TabIndex="7">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtwebsite"
                    ErrorMessage="Enter Website" Font-Size="Small" ForeColor="Red" ValidationGroup="save" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtwebsite"
                                    ErrorMessage="Enter Valid Website" Font-Size="Small" ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?"
                                    ForeColor="Red" ValidationGroup="save" Display="Dynamic"></asp:RegularExpressionValidator>
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
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="Contact No"></asp:Label>
                                <font color="red">*</font>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtContact" runat="server" TabIndex="8">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtContact"
                                    ErrorMessage="Enter Contact No" Font-Size="Small" ForeColor="Red" ValidationGroup="save"
                                    Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtContact"
                                    ErrorMessage="Enter Valid Contact Number" Font-Size="Small" ValidationExpression="\d+"
                                    ForeColor="Red" ValidationGroup="save" Display="Dynamic"></asp:RegularExpressionValidator>
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
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label15" runat="server" Text="Expiry On"></asp:Label>
                                <font color="red">*</font>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="expiryDate" runat="server" TabIndex="10">
                                </telerik:RadDatePicker>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="expiryDate"
                                    ErrorMessage="Please select Expiry Date" Font-Size="Small" ForeColor="Red" ValidationGroup="save"
                                    Display="Dynamic"></asp:RequiredFieldValidator>
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
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblOrgLimit" runat="server" Text="Organisation Limit"></asp:Label>
                                <font color="red">*</font>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtOrganisationLimit" runat="server" TabIndex="11" Text="999999">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RFVOrgLimit" runat="server" ControlToValidate="txtOrganisationLimit"
                                    ErrorMessage="Enter Organisation Limit" Font-Size="Small" ForeColor="Red" ValidationGroup="save"
                                    Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="REVOrgLimit" runat="server" ControlToValidate="txtOrganisationLimit"
                                    ErrorMessage="Enter Valid Organisation Limit" Font-Size="Small" ValidationExpression="\d+"
                                    ForeColor="Red" ValidationGroup="save" Display="Dynamic"></asp:RegularExpressionValidator>
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
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCarrierLimit" runat="server" Text="Carriers Limit"></asp:Label>
                                <font color="red">*</font>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCarriersLimit" runat="server" TabIndex="12" Text="999999">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RFVCarriersLimit" runat="server" ControlToValidate="txtCarriersLimit"
                                    ErrorMessage="Enter Carriers Limit" Font-Size="Small" ForeColor="Red" ValidationGroup="save"
                                    Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="REVCarriersLimit" runat="server" ControlToValidate="txtCarriersLimit"
                                    ErrorMessage="Enter Valid Carriers Limit" Font-Size="Small" ValidationExpression="\d+"
                                    ForeColor="Red" ValidationGroup="save" Display="Dynamic"></asp:RegularExpressionValidator>
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
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="Logo"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:FileUpload ID="imgUpload" runat="server" TabIndex="14" />
                                &nbsp; &nbsp; &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Image ID="Image1" runat="server" Width="50px" Height="50px" />
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
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                    ShowSummary="False" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" TabIndex="15" Text="Save"
                                    ValidationGroup="save" CssClass="button" />
                                &nbsp;
                                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" TabIndex="16"
                                    Text="Back" CssClass="button" />
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
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
        </asp:UpdatePanel>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel3" runat="server" Skin="Transparent"
            Style="z-index: -5555;">
        </telerik:RadAjaxLoadingPanel>
    </div>
</asp:Content>
