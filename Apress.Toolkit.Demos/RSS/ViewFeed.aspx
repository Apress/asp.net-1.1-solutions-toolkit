<%@ Import Namespace="Apress.Toolkit" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ViewFeed.aspx.vb" Inherits="ViewFeed" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
		<HEAD>
				<title>RSS Articles in this Feed</title>
				<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
				<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
				<meta content="JavaScript" name="vs_defaultClientScript">
				<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		</HEAD>
		<body>
				<form id="Form1" method="post" runat="server">
						<asp:datalist id="DataList1" runat="server" Font-Size="X-Small" Font-Names="Verdana" BorderWidth="1px"
								GridLines="Both" CellPadding="4" BackColor="White" BorderStyle="None" BorderColor="#CC9966"
								Width="500px" Height="269px">
								<ItemStyle ForeColor="#330099" BackColor="White"></ItemStyle>
								<ItemTemplate>
										<b>
												<%# CType(Container.DataItem, RssItem).Title %>
										</b>
										<br>
										<font size="1">
												<%# CType(Container.DataItem, RssItem).Description %>
										</font>
										<br>
										<a href="<%# CType(Container.DataItem, RssItem).Link %>">View</a><br>
								</ItemTemplate>
						</asp:datalist>&nbsp;<BR>
						<asp:label id="lblError" runat="server" Font-Size="Medium" Font-Names="Verdana" EnableViewState="False"
								Font-Bold="True" ForeColor="Maroon"></asp:label></form>
		</body>
</HTML>
