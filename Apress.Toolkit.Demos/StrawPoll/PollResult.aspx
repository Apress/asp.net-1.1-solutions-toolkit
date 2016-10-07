<%@ Register TagPrefix="cc" Namespace="Apress.Toolkit" Assembly="Apress.Toolkit" %>
<%@ Page Language="VB" Debug="true" %>
<HTML>
	<HEAD>
		<script runat="server">

    Private Sub Page_Load(ByVal sender As System.Object, _
                          ByVal e As System.EventArgs) Handles MyBase.Load
      StrawPollResult.XmlFile = Server.MapPath("PollDetails.xml")
      StrawPollResult.TableStyle.BorderColor = System.Drawing.Color.Red
      StrawPollResult.TableStyle.BorderWidth = New Unit(2)
      StrawPollResult.ImageSrc = Server.MapPath("bar.gif")
    End Sub

		</script>
	</HEAD>
	<body>
		<form runat="server">
			<P>
				<cc:XmlPollResult id="StrawPollResult" runat="server"></cc:XmlPollResult></P>
				<asp:HyperLink id="HyperLink1" runat="server" NavigateUrl="Poll.aspx">Poll Question</asp:HyperLink></P>
		</form>
	</body>
</HTML>
