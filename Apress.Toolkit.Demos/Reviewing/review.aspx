<%@ Register TagPrefix="uc1" TagName="ReviewerForm" Src="ReviewerForm.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="review.aspx.vb" Inherits="review"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
		<HEAD>
				<title>Post a Review</title>
				<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
				<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
				<meta name="vs_defaultClientScript" content="JavaScript">
				<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		</HEAD>
		<body MS_POSITIONING="FlowLayout">
				<form id="Form1" method="post" runat="server" style="FONT-SIZE: 16px; FONT-FAMILY: verdana">
						<P>Product Review:<BR>
								<BR>
								&nbsp;
								<uc1:ReviewerForm id="ReviewerForm1" runat="server" ConnectionString="data source=local;initial catalog=ReviewsDB;user=sa;pwd=;persist security info=False;workstation id=(Local);packet size=4096"
										ProductID="1"></uc1:ReviewerForm></P>
						<P></P>
				</form>
		</body>
</HTML>
