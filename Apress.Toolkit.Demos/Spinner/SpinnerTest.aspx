<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SpinnerTest.aspx.vb" Inherits="SpinnerTest"%>
<%@ Register TagPrefix="cc" Namespace="Apress.Toolkit.Controls" Assembly="Apress.Toolkit" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>SpinnerTest</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table>
				<tr>
					<td>Percentage:
					</td>
					<td><cc:Spinner id="Spinner1" runat="server" width="66px" buttonsize="XX-Small" BackColor="#FFE0C0"></cc:Spinner></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
