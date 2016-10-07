Imports System.Web.UI.WebControls

Imports Apress.Toolkit


Public Class NewsBrowser
    Inherits System.Web.UI.Page
    Protected WithEvents listFeeds As System.Web.UI.WebControls.DataList

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Me.IsPostBack Then
            Dim FeedReader As New RssFeedListReader()
            Dim Feeds As RssFeedItemCollection
            Feeds = FeedReader.GetFeedList("http://weblogs.asp.net/MainFeed.aspx")
            listFeeds.DataSource = Feeds
            listFeeds.DataBind()
        End If
    End Sub


    Private Sub listFeeds_ItemCommand(ByVal source As Object, ByVal e As DataListCommandEventArgs) Handles listFeeds.ItemCommand
        Response.Redirect("ViewFeed.aspx?url=" & Server.UrlEncode(e.CommandName))
    End Sub

End Class
