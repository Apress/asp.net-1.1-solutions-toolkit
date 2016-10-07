<%@ Import Namespace="Apress.Toolkit.SearchEngine" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="search.aspx.vb" Inherits="index" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>search</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<P>
				<asp:TextBox id="txtQuery" runat="server"></asp:TextBox>
				<asp:ListBox id="lstSearchType" runat="server" Rows="1">
					<asp:ListItem Value="MatchAll">Match All</asp:ListItem>
					<asp:ListItem Value="MatchAny" Selected="True">Match Any</asp:ListItem>
					<asp:ListItem Value="MatchAdvanced">Advanced Search</asp:ListItem>
				</asp:ListBox>
				<asp:ListBox id="lstPhraseMatch" runat="server" Rows="1">
					<asp:ListItem Value="SinglePhrase">Match whole phrase</asp:ListItem>
					<asp:ListItem Value="None" Selected="True">Match individual words</asp:ListItem>
					<asp:ListItem Value="InlineQuotes">Match quoted phrases</asp:ListItem>
				</asp:ListBox>
				<asp:Button id="cmdSearch" runat="server" Text="Search"></asp:Button></P>
			<P>
				<asp:DataList id="ListMatches" runat="server">
					<ItemTemplate>
						<b>
							<%# CType(Container.DataItem, SearchHit).Title %>
							<br>
						</b><font size="1"><b>Description: </b>
							<%# CType(Container.DataItem, SearchHit).Description %>
							<br>
							<b>Keywords: </b>
							<%# CType(Container.DataItem, SearchHit).Keywords %>
							<br>
							<b>URL: </b><u>
								<%# CType(Container.DataItem, SearchHit).Url %>
							</u>
							<br>
							<b>Percent Match: </b>
							<%# CType(Container.DataItem, SearchHit).Percent %>
							% &nbsp; <b>Rank: </b>
							<%# CType(Container.DataItem, SearchHit).Rank %>
						</font>
						<br>
					</ItemTemplate>
				</asp:DataList></P>
		</form>
	</body>
</HTML>
