<%@ Register TagPrefix="cc" Namespace="Apress.Toolkit" Assembly="Apress.Toolkit" %>
<%@ Page Language="VB" Debug="true" %>
<HTML>
	<HEAD>
		<script runat="server">

    Private Sub Page_Load(ByVal sender As System.Object, _
                          ByVal e As System.EventArgs) Handles MyBase.Load
      StrawPoll.XmlFile = Server.MapPath("PollDetails.xml")
      StrawPoll.TableStyle.BorderColor = System.Drawing.Color.Red
      StrawPoll.TableStyle.BorderWidth = New Unit(2)
      StrawPoll.QuestionStyle.ForeColor = System.Drawing.Color.Gray
    End Sub

		</script>
	</HEAD>
	<body>
		<form runat="server">
			<P>
				<cc:XmlPoll id="StrawPoll" runat="server"></cc:XmlPoll></P>
				<br><br>
				<asp:HyperLink id="HyperLink1" runat="server" NavigateUrl="PollResult.aspx">Result</asp:HyperLink></P>
		</form>
	</body>
</HTML>
