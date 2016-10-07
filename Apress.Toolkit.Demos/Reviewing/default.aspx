<%@ Register TagPrefix="uc1" TagName="ReviewerForm" Src="ReviewerForm.ascx" %>
<%@ Page Language="VB" %>
<%@ Register TagPrefix="cc" Namespace="Apress.Toolkit" Assembly="Apress.Toolkit" %>
<HTML>
		<HEAD>
		</HEAD>
		<body>
				<form runat="server">
						<P>
						<P><cc:lastreview id="LastReview1" runat="server" ConnectionString="data source=work1;initial catalog=ReviewsDB;user=sa;pwd=sa;persist security info=False;workstation id=(Local);packet size=4096"
										MaxChars="200" ProductID="1"></cc:lastreview></P>
						<P>
								<asp:HyperLink id="HyperLink1" runat="server" Target="_blank" NavigateUrl="review.aspx" Font-Size="XX-Small"
										Font-Names="Verdana,Arial">Post a Review</asp:HyperLink>
						</P>
				</form>
		</body>
</HTML>
