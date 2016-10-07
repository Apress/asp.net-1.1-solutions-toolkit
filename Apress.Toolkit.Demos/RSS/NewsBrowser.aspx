<%@ Import Namespace="Apress.Toolkit" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="NewsBrowser.aspx.vb" Inherits="NewsBrowser" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>List of RSS Feeds</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
		<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<asp:DataList id="listFeeds" runat="server" BorderColor="#CC9966" BorderStyle="None" BackColor="White" CellPadding="6" GridLines="Both" BorderWidth="1px" Font-Names="Verdana" Font-Size="X-Small" Width="460px">
				<ItemStyle ForeColor="#330099" BackColor="White"></ItemStyle>
				<ItemTemplate>
					<asp:Button CommandName="<%# CType(Container.DataItem, RssFeedItem).Link%>" Text="Select" runat="server" ID="Button1"/>
					<b>
						<%# CType(Container.DataItem, RssFeedItem).Title %>
						<br>
					</b><font size="1">
						<%# CType(Container.DataItem, RssFeedItem).Description %>
					</font>
					<br>
				</ItemTemplate>
			</asp:DataList><BR>
			<BR>
		</form>
	</body>
</HTML>
