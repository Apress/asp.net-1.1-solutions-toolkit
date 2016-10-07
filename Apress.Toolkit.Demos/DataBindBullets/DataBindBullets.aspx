<%@ Register TagPrefix="cc" Namespace="Apress.Toolkit" Assembly="Apress.Toolkit" %>
<%@ Page Trace="false" Language="vb" AutoEventWireup="false" Codebehind="DataBindBullets.aspx.vb" Inherits="DataBindBullets"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
		<HEAD>
				<title>DataBindBullets</title>
				<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
				<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
				<meta content="JavaScript" name="vs_defaultClientScript">
				<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		</HEAD>
		<body MS_POSITIONING="GridLayout">
				<form id="Form1" method="post" runat="server">
						<H1>My Company&nbsp;Goals&nbsp;</H1>
						<P><cc:databindbulletlist id="DataBindBulletList1" style="Z-INDEX: 101; LEFT: 36px; POSITION: absolute; TOP: 71px"
										runat="server" DisplayOrientation="Vertical" ColumnCount="1" Width="207px" Font-Names="Arial" Font-Size="Medium"></cc:databindbulletlist></P>
				</form>
		</body>
</HTML>
