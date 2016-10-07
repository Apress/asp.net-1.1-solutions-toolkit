<%@ Register TagPrefix="cc" Namespace="Apress.Toolkit" Assembly="Apress.Toolkit" %>
<%@ Page trace="false" Language="vb" AutoEventWireup="false" Codebehind="ImageMap.aspx.vb" Inherits="ImageMap"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
		<HEAD>
				<title>ImageMap</title>
				<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
				<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
				<meta content="JavaScript" name="vs_defaultClientScript">
				<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		</HEAD>
		<body MS_POSITIONING="FlowLayout">
				<form id="Form1" method="post" runat="server">
						<cc:imagemap id="ImageMap1" runat="server" AbsoluteImagePath="C:\cati-kitty.jpg" ImageSrc="cati-kitty.jpg"
								ImageMap="183,110,93,83;312,136,90,102"></cc:imagemap><BR>
						<BR>
						<asp:label id="Label1" runat="server"></asp:label></form>
		</body>
</HTML>
