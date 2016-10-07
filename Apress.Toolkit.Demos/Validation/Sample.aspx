<%@ Register TagPrefix="cc" Namespace="Apress.Toolkit.Validation" Assembly="Apress.Toolkit" %>
<%@ Page language="vb" Codebehind="Sample.aspx.vb" AutoEventWireup="false" Inherits="Sample" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML xmlns:xslt="http://www.w3.org/1999/XSL/Transform">
	<HEAD>
		<title>Apress Toolkit Validation</title>
	</HEAD>
	<body style="FONT-SIZE: smaller; FONT-FAMILY: Tahoma, Verdana, Times New Roman" ms_positioning="FlowLayout">
		<form id="Sample" method="post" runat="server">
			<p>
				<table id="Table1" style="BORDER-RIGHT: 1px outset; BORDER-TOP: 1px outset; FONT-SIZE: x-small; BORDER-LEFT: 1px outset; BORDER-BOTTOM: 1px outset; FONT-FAMILY: Tahoma; HEIGHT: 122px; BACKGROUND-COLOR: #f0f1f6"
					cellspacing="0" cellpadding="0" border="0">
					<tr>
						<td>Name:</td>
						<td><asp:textbox id="txtName" runat="server" font-size="Smaller" font-names="Tahoma"></asp:textbox></td>
						<td><cc:validatorcontrol id="valName" runat="server" errormessage="Only letters are allowed." assemblyname="Apress.Toolkit"
								typename="Apress.Toolkit.Validation.AlphaValidator" controltovalidate="txtName" display="Dynamic"> Only letters are allowed.</cc:validatorcontrol></td>
					</tr>
					<tr>
						<td>Age:</td>
						<td><asp:textbox id="txtAge" runat="server" font-size="Smaller" font-names="Tahoma"></asp:textbox></td>
						<td>
							<p><cc:validatorcontrol id="valAge" runat="server" errormessage="The age has to be an integer value." assemblyname="Apress.Toolkit"
									typename="Apress.Toolkit.Validation.IntegerValidator" controltovalidate="txtAge" display="Dynamic"
									width="208px">The age has to be an integer value.</cc:validatorcontrol></p>
						</td>
					</tr>
					<tr>
						<td>ZipCode:</td>
						<td><asp:textbox id="txtZip" runat="server" font-size="Smaller" font-names="Tahoma"></asp:textbox></td>
						<td><cc:validatorcontrol id="valZip" runat="server" errormessage="Enter only alphanumeric characters." assemblyname="Apress.Toolkit"
								typename="Apress.Toolkit.Validation.AlphaNumericValidator" controltovalidate="txtZip" display="Dynamic"
								width="208px">Enter only alphanumeric characters.</cc:validatorcontrol></td>
					</tr>
					<tr>
						<td>Account Total:</td>
						<td><asp:textbox id="txtAccount" runat="server" font-size="Smaller" font-names="Tahoma"></asp:textbox></td>
						<td><cc:validatorcontrol id="valAccount" runat="server" errormessage="Enter a valid currency number." assemblyname="Apress.Toolkit"
								typename="Apress.Toolkit.Validation.CurrencyValidator" controltovalidate="txtAccount" display="Dynamic"
								width="181px">Enter a valid currency number.</cc:validatorcontrol></td>
					</tr>
					<tr>
						<td style="HEIGHT: 20px">Points Earned:</td>
						<td style="HEIGHT: 20px"><asp:textbox id="txtEarned" runat="server" font-size="Smaller" font-names="Tahoma"></asp:textbox></td>
						<td style="HEIGHT: 20px"><cc:validatorcontrol id="valCredits" runat="server" errormessage="Enter a positive number." assemblyname="Apress.Toolkit"
								typename="Apress.Toolkit.Validation.PositiveNumericValidator" controltovalidate="txtEarned" display="Dynamic" width="231px"> Enter a positive number.</cc:validatorcontrol></td>
					</tr>
					<tr>
						<td>Points Missed:</td>
						<td><asp:textbox id="txtMissed" runat="server" font-size="Smaller" font-names="Tahoma"></asp:textbox></td>
						<td><cc:validatorcontrol id="valDebits" runat="server" errormessage="Enter a negative number." assemblyname="Apress.Toolkit"
								typename="Apress.Toolkit.Validation.NegativeNumericValidator" controltovalidate="txtMissed" display="Dynamic"
								width="152px">Enter a negative number.</cc:validatorcontrol></td>
					</tr>
				</table>
			</p>
			<p>Microsoft Required Validator:
				<asp:textbox id="txtRequired" runat="server"></asp:textbox>&nbsp;<asp:requiredfieldvalidator id="valRequired" runat="server" errormessage="This field is required" controltovalidate="txtRequired"></asp:requiredfieldvalidator></p>
			<p>Enter you personal code:
				<asp:textbox id="txtUserID" runat="server" width="272px">436EA455-BABD-4ca2-9D30-7B4F4608A068</asp:textbox>&nbsp;<cc:validatorcontrol id="Validatorcontrol2" runat="server" errormessage="Enter a valid user id." assemblyname="Apress.Toolkit"
					typename="Apress.Toolkit.Validation.PositiveNumericValidator" controltovalidate="txtUserID" width="360px">The user id is either invalid or doesn't exist for the application.</cc:validatorcontrol></p>
			<p><asp:button id="btnSubmit" runat="server" text="Submit"></asp:button></p>
			<p>
				<asp:validationsummary id="txtSummary" runat="server"></asp:validationsummary></p>
		</form>
		<script language="javascript">
<!--


//-->
		</script>
	</body>
</HTML>
