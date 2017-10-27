<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Site.master" CodeBehind="Default.aspx.cs" Inherits="GPS._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headerdata" Runat="Server">
    <script type="text/javascript" language="javascript">
        $(function () {
            $("#tabs").tabs({
        });
    });
	</script>
    <link href="Styles/skin.css" rel="stylesheet" type="text/css" />
    <link href="login-box.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentdata" Runat="Server" >

<div id="login-box">

<asp:label ID="lblhead" runat="server"> <img src="images/sandstechLogo.png" />User Login</asp:label>

<div class="loginBoxFieldsMain">
<asp:Label ID="lblmsg" runat="server"></asp:Label>
<div id="login-box-name">Username:</div>
<div id="login-box-field">            
<asp:TextBox ID="txtbLoginName" cssclass="form-login" runat="server" ></asp:TextBox></div>
<div style="clear:both;"></div>

<div id="login-box-name">Password:</div>
<div id="login-box-field"><asp:TextBox ID="txtbPassword" cssclass="form-login" runat="server" TextMode="Password" ></asp:TextBox></div>
<div style="clear:both;"></div>
<asp:ImageButton ID="btnLogin" runat="server" border="0" ImageUrl="images/loginBtn.png" width="80" height="28" style="border:none;" onclick="btnLogin_Click"/>
</div>

</div>

</asp:Content>