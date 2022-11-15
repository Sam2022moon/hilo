<!--
 * Project:	    A-05 : HI-LO
 * Author:	    Hoang Phuc Tran - 8789102
                Bumsu Yi - 8110678
 * Date:		November 14, 2022
 * Description: This is a page with post-backs for the hi-lo game. 
-->
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="A05_HiloGame.Default" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/CSS_Styles/Styles.css" rel="stylesheet" type="text/css" />
    <title>Hi-Lo Game</title>
</head>

<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="showTxt" runat="server" Text="Hi, Enter your username for the game" CssClass="questionStyle"/><br/>
            <asp:TextBox ID="inputTxt"  CssClass="labelStyle" runat="server"/>
            <asp:Button ID="submission" PostBackUrl="~/Default.aspx" runat="server" Text="Submit" CssClass="labelStyle" CausesValidation="true"/><br/>
            <asp:RequiredFieldValidator ID="validator" runat="server" ControlToValidate="inputTxt" ErrorMessage="---your input is invalid because you entered white space or blank, please enter your user name!---." CssClass="errorMsgStyle"></asp:RequiredFieldValidator>
            <asp:ValidationSummary ValidationGroup="validation" runat="server"/>
        </div>
    </form>
</body>
</html>
