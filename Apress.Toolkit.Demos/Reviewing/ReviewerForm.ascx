<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ReviewerForm.ascx.vb" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" Inherits="ReviewerForm" %>
<TABLE id="Table1" style="BORDER-RIGHT: black thin double; BORDER-TOP: black thin double; BORDER-LEFT: black thin double; BORDER-BOTTOM: black thin double"
		cellSpacing="10" cellPadding="0" width="300" border="0">
		<TR>
				<TD><asp:label id="Label1" runat="server" Font-Names="Verdana" Font-Bold="True" Font-Size="Smaller">Name</asp:label></TD>
				<TD><asp:textbox id="txtName" runat="server" Width="237px" BorderStyle="Solid" BorderColor="Black"></asp:textbox></TD>
				<TD>
						<P align="left"><asp:label id="Label3" runat="server" Font-Names="Verdana" Font-Bold="True" Font-Size="Smaller">Rating</asp:label></P>
				</TD>
		</TR>
		<TR>
				<TD vAlign="top"><asp:label id="Label2" runat="server" Font-Names="Verdana" Font-Bold="True" Font-Size="Smaller">Review</asp:label></TD>
				<TD><asp:textbox id="txtReview" runat="server" Width="237px" TextMode="MultiLine" Height="225px"
								BorderStyle="Solid" BorderColor="Black" MaxLength="8000"></asp:textbox></TD>
				<TD vAlign="top"><asp:radiobuttonlist id="rblRating" runat="server" Width="119px" Font-Names="Verdana" Font-Size="XX-Small">
								<asp:ListItem Value="5" Selected="True">Excellent</asp:ListItem>
								<asp:ListItem Value="4">Good</asp:ListItem>
								<asp:ListItem Value="3">Fairly good</asp:ListItem>
								<asp:ListItem Value="2">Sufficient</asp:ListItem>
								<asp:ListItem Value="1">Not good</asp:ListItem>
						</asp:radiobuttonlist></TD>
		</TR>
		<TR>
				<TD><asp:imagebutton id="ibSubmit" runat="server" ImageUrl="images/submit.gif"></asp:imagebutton></TD>
				<TD colSpan="3">
						<P align="left">
								<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Font-Names="Verdana" Font-Size="Smaller"
										ErrorMessage="Please, specify your review in the Review textarea..." ControlToValidate="txtReview"></asp:RequiredFieldValidator>
								<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" Font-Names="Verdana" Font-Size="Smaller"
										ErrorMessage="Please, specify your name in the Name textbox..." ControlToValidate="txtName"></asp:RequiredFieldValidator></P>
				</TD>
		</TR>
</TABLE>
