<%@ Page Language="vb" AutoEventWireup="false" Codebehind="index.aspx.vb" Inherits="search" %>
<%@ Import Namespace="Apress.Toolkit.SearchEngine" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>index</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body>
		<form id="Form2" method="post" runat="server">
			<P>
				<asp:Button id="cmdCreate" runat="server" Text="Create Index"></asp:Button>
				<asp:Button id="cmdLoad" runat="server" Text="Load Index"></asp:Button></P>
			<P>
				<asp:DataList id="ListIndex" runat="server">
					<ItemTemplate>
						<b>
							<%# CType(Container.DataItem, IndexEntry).Title %>
						</b>
						<br>
						<font size="1"><b>Description: </b>
							<%# CType(Container.DataItem, IndexEntry).Description %>
							<br>
							<b>Keywords: </b>
							<%# CType(Container.DataItem, IndexEntry).Keywords %>
							<br>
							<b>URL: </b><u>
								<%# CType(Container.DataItem, IndexEntry).Url %>
							</u></font>
						<br>
					</ItemTemplate>
				</asp:DataList></P>
		</form>
	</body>
</HTML>
