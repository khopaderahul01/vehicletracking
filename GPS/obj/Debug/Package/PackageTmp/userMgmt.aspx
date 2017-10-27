<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="userMgmt.aspx.cs" Inherits="GPS.userMgmt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headerdata" runat="server">
    <link href="Styles/company.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentdata" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="100%" align="center">
        <tr>
            <td  align="center">
                <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel3"
                    RenderMode="Inline" Style="height: 100%; z-index: -555">
                    <asp:UpdatePanel ID="updatePanelGrid" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="PanelGrid" runat="server">
                                <table>
                                    <tr>
                                        <td class="style2">
                                            <asp:Label ID="Label8" runat="server" Text="User Details"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style2">
                                            <asp:Label ID="lblUserPermission" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadGrid ID="UserGrid" runat="server" AllowFilteringByColumn="true" AllowPaging="True"
                                                AllowSorting="True" AutoGenerateColumns="false" PageSize="15" Skin="Windows7"
                                                Width="100%" OnItemCommand="UserGrid_ItemCommand" OnPageIndexChanged="UserGrid_PageIndexChanged"
                                                OnPageSizeChanged="UserGrid_PageSizeChanged" OnSortCommand="UserGrid_SortCommand">
                                                <GroupingSettings CaseSensitive="false" />
                                                <MasterTableView DataKeyNames="ID">
                                                    <Columns>
                                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton ToolTip="View" ID="btnView" CssClass="editImg" CausesValidation="false"
                                                                    ImageUrl="~/images/Iconedit.png" CommandName="EditGrid" CommandArgument='<%# Bind("ID") %>'
                                                                    runat="server" Style="float: left;" />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridBoundColumn DataField="UserName" HeaderStyle-HorizontalAlign="Center"
                                                            AllowFiltering="true" AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                            HeaderText="Display Name" ItemStyle-HorizontalAlign="Left" ShowFilterIcon="true">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="loginID" HeaderStyle-HorizontalAlign="Center"
                                                            HeaderText="UserID" ItemStyle-HorizontalAlign="Left" ShowFilterIcon="true"
                                                            AllowFiltering="true" AutoPostBackOnFilter="True" CurrentFilterFunction="Contains">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Email" HeaderStyle-HorizontalAlign="Center" HeaderText="Email"
                                                            ItemStyle-HorizontalAlign="Left" ShowFilterIcon="true" UniqueName="Email" AllowFiltering="true"
                                                            AutoPostBackOnFilter="True" CurrentFilterFunction="Contains">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="CreatedOn" HeaderStyle-HorizontalAlign="Center"
                                                            HeaderText="Created On" ItemStyle-HorizontalAlign="Left" ShowFilterIcon="false"
                                                            AllowFiltering="false">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="expiresOn" HeaderText="Expires On" ItemStyle-HorizontalAlign="Center"
                                                            AllowFiltering="false" ShowFilterIcon="false">
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
                            <asp:PostBackTrigger ControlID="btnSubmit" />
                            <asp:PostBackTrigger ControlID="btnback" />
                        </Triggers>
                    </asp:UpdatePanel>
                </telerik:RadAjaxPanel>
                <asp:UpdatePanel ID="updatePanelAdd" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="panel_userRegister" runat="server" Visible="false">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text="User ID"></asp:Label>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtUserID" runat="server" Enabled="False">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtUserID"
                                            ErrorMessage="Enter Display name" ValidationGroup="save"></asp:RequiredFieldValidator>
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
                                        <asp:Label ID="Label1" runat="server" Text="Display Name"></asp:Label>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtDisplayName" runat="server">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDisplayName"
                                            ErrorMessage="Enter User name" ValidationGroup="save"></asp:RequiredFieldValidator>
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
                                        <asp:Label ID="Label10" runat="server" Text="Old Password"></asp:Label>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtOldPassword" runat="server" TextMode="Password" MaxLength="15">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtOldPassword"
                                            ErrorMessage="Enter Old password" ValidationGroup="save"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
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
                                        <asp:Label ID="Label2" runat="server" Text="Password"></asp:Label>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtpassword" runat="server" TextMode="Password" MaxLength="15">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtpassword"
                                            ErrorMessage="Password must be atleast 6 or more Characters long." ForeColor="Red"
                                            ValidationExpression="[a-zA-Z0-9]{6,50}"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtpassword"
                                            ErrorMessage="Enter password" ValidationGroup="save"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPWDLength" runat="server" ForeColor="Red"></asp:Label>
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
                                        <asp:Label ID="Label3" runat="server" Text="Confirm Password"></asp:Label>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtconfirmpassword" runat="server" TextMode="Password">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtpassword"
                                            ControlToValidate="txtconfirmpassword" ErrorMessage="Passwords do not match"
                                            ValidationGroup="save"></asp:CompareValidator>
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
                                        <asp:Label ID="Label5" runat="server" Text="Email"></asp:Label>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtEmail" runat="server">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtEmail"
                                            ErrorMessage="Enter Email" ValidationGroup="save"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                                            ErrorMessage="Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                            ValidationGroup="save"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblEmailAdd" runat="server" ForeColor="Red"></asp:Label>
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
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="expiryDate"
                                            ErrorMessage="Please select Expiry Date" Font-Size="Small" ForeColor="Red" ValidationGroup="save"
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                            ShowSummary="False" ValidationGroup="save" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSubmit" runat="server" Text="Update" OnClick="btnSubmit_Click"
                                            ValidationGroup="save" CssClass="button" />
                                        &nbsp; &nbsp;
                                        <asp:Button ID="btnback" runat="server" Text="Back" CausesValidation="false" CssClass="button"
                                            OnClick="btnback_Click" />
                                        &nbsp;
                                        <asp:Label ID="lblMsg" runat="server" ForeColor="#3366FF"></asp:Label>
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
                                    <td>
                                        &nbsp;
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
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel3" runat="server" Skin="Transparent"
                    Style="z-index: -5555;">
                </telerik:RadAjaxLoadingPanel>
            </td>
        </tr>
    </table>
</asp:Content>
