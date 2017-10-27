<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Organisation.aspx.cs" Inherits="GPS.Organisation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headerdata" runat="server">
    <link href="Styles/org.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentdata" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center">
        <script type="text/javascript">
            function showAlert(message) {
                alert(message);
            }
        </script>
        <div >
            <div>
                <asp:Label ID="lblCarrierLimitTop" runat="server" Text=""></asp:Label>
            </div>
            <div>
                <asp:Label ID="lblOrgLimitTop" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel3"
            RenderMode="Inline" Style="height: 100%; z-index: -555">
            <asp:UpdatePanel ID="updatePanelGrid" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="PanelOrgView" runat="server">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Organisation Details"></asp:Label>
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
                                    <telerik:RadGrid ID="OrgGrid" runat="server" AllowFilteringByColumn="true" AllowPaging="True"
                                        AllowSorting="True" AutoGenerateColumns="false" PageSize="10" Skin="Windows7"
                                        Width="99%" OnItemCommand="OrgGrid_ItemCommand" OnPageIndexChanged="OrgGrid_PageIndexChanged"
                                        OnSortCommand="OrgGrid_SortCommand" OnPageSizeChanged="OrgGrid_PageSize">
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings>
                                            <%--   EnablePostBackOnRowClick="true"   <Selecting AllowRowSelect="false" />--%>
                                            <Selecting AllowRowSelect="true" />
                                            <%-- <ClientEvents OnRowMouseOver="grdUsers_RowMouseOver" />--%>
                                        </ClientSettings>
                                        <MasterTableView DataKeyNames="ID">
                                            <Columns>
                                                <telerik:GridTemplateColumn AllowFiltering="false">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ToolTip="View" ID="btnView" CssClass="editImg" CausesValidation="false"
                                                            ImageUrl="~/images/Iconedit.png" CommandName="editgrid" CommandArgument='<%# Bind("ID") %>'
                                                            runat="server" Style="float: left;" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:ImageButton ToolTip="Delete" ID="btnDelete" CssClass="deleteImg" CausesValidation="false"
                                                            CommandName="deletegrid" CommandArgument='<%# Bind("ID") %>' runat="server" Style="float: right;"
                                                            OnClientClick="return confirm('Do you want to delete this record?');" ImageUrl="~/images/Icondelete.png" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="Name" HeaderStyle-HorizontalAlign="Center" HeaderText="Organisation"
                                                    ItemStyle-HorizontalAlign="Left" ShowFilterIcon="true" UniqueName="ProductName"
                                                    AllowFiltering="true" AutoPostBackOnFilter="True" CurrentFilterFunction="Contains">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="companyName" HeaderText="Company" ItemStyle-HorizontalAlign="Left"
                                                    ShowFilterIcon="true" UniqueName="company" AllowFiltering="true" AutoPostBackOnFilter="True"
                                                    CurrentFilterFunction="Contains">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Contact No" DataType="System.String" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderText="Contact No" ItemStyle-HorizontalAlign="Right" ShowFilterIcon="true"
                                                    AllowFiltering="true" AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                    UniqueName="qty">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Email" HeaderStyle-HorizontalAlign="Center" HeaderText="Email"
                                                    FilterControlWidth="85%" ItemStyle-HorizontalAlign="Left" ShowFilterIcon="true"
                                                    UniqueName="Price" AllowFiltering="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Address" HeaderText="Address" ItemStyle-HorizontalAlign="Left"
                                                    ShowFilterIcon="false" UniqueName="A" AllowFiltering="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Crteated On" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderText="Created On" ItemStyle-HorizontalAlign="Left" ShowFilterIcon="false"
                                                    UniqueName="B" AllowFiltering="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Expiry On" HeaderText="Expires On" ItemStyle-HorizontalAlign="Left"
                                                    ShowFilterIcon="false" UniqueName="C" AllowFiltering="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="website" HeaderText="Website" ItemStyle-HorizontalAlign="Left"
                                                    ShowFilterIcon="true" UniqueName="D" AllowFiltering="true" FilterControlWidth="85%">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="carrierLimit" HeaderText="License" ItemStyle-HorizontalAlign="Right"
                                                    ShowFilterIcon="false" UniqueName="carrierLimit" AllowFiltering="false">
                                                </telerik:GridBoundColumn>
                                                
                                                <telerik:GridBoundColumn DataField="usedLicense" HeaderText="Used License" ItemStyle-HorizontalAlign="Right"
                                                    ShowFilterIcon="false" UniqueName="usedLicense" AllowFiltering="false">
                                                </telerik:GridBoundColumn>
                                                
                                                <telerik:GridBoundColumn DataField="remainingLicnese" HeaderText="Remaining License" ItemStyle-HorizontalAlign="Right"
                                                    ShowFilterIcon="false" UniqueName="remainingLicnese" AllowFiltering="false">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Center"
                                                    ShowFilterIcon="false" UniqueName="StatusCol" AllowFiltering="false">
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
                    <asp:PostBackTrigger ControlID="btnInsert" />
                    <asp:PostBackTrigger ControlID="btnCancel" />
                </Triggers>
            </asp:UpdatePanel>
        </telerik:RadAjaxPanel>
        <asp:UpdatePanel ID="updatePanelAdd" runat="server">
            <ContentTemplate>
                <asp:Panel ID="PanelOrgAdd" runat="server" Visible="false">
                    <table cellpadding="5" cellspacing="5">
                        <tr>
                            <td style="width: 150px">
                                <span><b>Orgnization Details</b></span> &nbsp;
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
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Organisation Name"></asp:Label>
                                <font color="red">*</font>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtOrgName" runat="server" TabIndex="1">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtOrgName"
                                    ErrorMessage="Please enter company name" Font-Size="Small" ForeColor="#CC3300"
                                    ValidationGroup="save" Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr id="trCompany" runat="server">
                            <td>
                                <asp:Label ID="lblCompName" runat="server" Text="Company Name"></asp:Label>
                                <font color="red">*</font>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <telerik:RadComboBox ID="ddlCompName" runat="server" TabIndex="2" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCompName_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlCompName"
                                    ErrorMessage="Please select Company" Font-Size="Small" ForeColor="#CC3300" ValidationGroup="save"
                                    Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblContactNo" runat="server" Text="Contact Number"></asp:Label>
                                <font color="red">*</font>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtContactNo" runat="server" TabIndex="3">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtContactNo"
                                    ErrorMessage="Please enter Contact" Font-Size="Small" ForeColor="#CC3300" ValidationGroup="save"
                                    Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtContactNo"
                                    ErrorMessage="Enter Valid Contact number" Font-Size="Small" ForeColor="#CC3300"
                                    ValidationExpression="\d+" ValidationGroup="save" Display="Dynamic"></asp:RegularExpressionValidator>
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
                                <telerik:RadTextBox ID="txtEmailId" runat="server" TabIndex="4">
                                </telerik:RadTextBox>
                                <asp:Label ID="lblEmailAdd" runat="server" Font-Size="Small" ForeColor="Red"></asp:Label>
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
                        <tr id="trUserName" runat="server">
                            <td>
                                <asp:Label ID="lblUserName" runat="server" Text="Display Name"></asp:Label>
                                <font color="red">*</font>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtUserName" runat="server" TabIndex="4">
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
                                <telerik:RadTextBox ID="txtUserID" runat="server" TabIndex="4">
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
                                <telerik:RadTextBox ID="txtPassword" runat="server" TextMode="Password" TabIndex="5">
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
                                <telerik:RadTextBox ID="txtConfirmPwd" runat="server" TextMode="Password" TabIndex="6">
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
                                <asp:Label ID="lblAddr" runat="server" Text="Address"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtAddress" runat="server" TabIndex="7">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblWebsiteaddr" runat="server" Text="Website"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtWebsiteAdd" runat="server" TabIndex="8">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="Server"
                                    ControlToValidate="txtWebsiteAdd" ValidationExpression="(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"
                                    ErrorMessage="Please enter a valid URL" Display="Dynamic" ForeColor="#CC3300"
                                    Font-Size="Small" />
                            </td>
                        </tr>
                        <tr id="trcarrierLimit" runat="server">
                            <td>
                                <asp:Label ID="lblCarrierLimit" runat="server" Text="Carrier Limit"></asp:Label>
                                <font color="red">*</font>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upPanel" runat="server">
                                    <ContentTemplate>
                                        <telerik:RadTextBox ID="txtCarrierLimit" runat="server" TabIndex="9">
                                        </telerik:RadTextBox>
                                        <asp:Label ID="lblCarrierlicense" runat="server" ForeColor="Red"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RFVCarrierLimit" runat="server" ControlToValidate="txtCarrierLimit"
                                    ErrorMessage="Enter Carrier Limit" Font-Size="Small" ForeColor="#CC3300" ValidationGroup="save"
                                    Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="REVCarrierLimit" runat="server" ControlToValidate="txtCarrierLimit"
                                    ErrorMessage="Enter valid Carrier Limit" Font-Size="Small" ForeColor="#CC3300"
                                    ValidationExpression="\d+" ValidationGroup="save" Display="Dynamic"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="Logo"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:FileUpload ID="imgUpload" runat="server" />
                            </td>
                            <td>
                                <asp:Image ID="logo" runat="server" Visible="False" />
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
                                <asp:Label ID="lblLimitError" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="Summary" runat="server" ForeColor="#CC3300" ShowMessageBox="false"
                                    ShowSummary="False" ValidationGroup="save" />
                            </td>
                            <td>
                            </td>
                            <td class="style4" style="padding-top: 30px">
                                <asp:Button ID="btnInsert" runat="server" OnClick="btnInsert_Click" Style="height: 26px"
                                    Text="Save" ValidationGroup="save" CssClass="button" />
                                &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Back" CausesValidation="False"
                                    OnClick="btnCancel_Click" CssClass="button" />
                                &nbsp;
                                <asp:Label ID="lblMessage" runat="server" ForeColor="#3333FF"></asp:Label>
                            </td>
                            <td style="padding-top: 30px">
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
                            <td class="style4" style="padding-top: 30px">
                                <%--<asp:Label ID="lblcarrier" runat="server" ForeColor="#3333FF" Text="Continue adding Carriers"
                            Visible="False"></asp:Label>
                        <asp:LinkButton ID="lnkCarrier" runat="server" OnClick="lnkCarrier_Click" Visible="False"
                            CausesValidation="False">Click Here</asp:LinkButton>--%>
                            </td>
                            <td style="padding-top: 30px">
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
