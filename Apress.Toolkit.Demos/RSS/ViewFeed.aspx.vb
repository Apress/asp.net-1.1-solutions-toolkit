Imports Apress.Toolkit

Public Class ViewFeed
    Inherits System.Web.UI.Page
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataList1 As System.Web.UI.WebControls.DataList

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
            Try
                Dim Rss As New RssReader
                Dim Channel As RssChannel
                Channel = Rss.GetChannel(Request.QueryString("url"))

                DataList1.DataSource = Channel.Items
                DataList1.DataBind()
            Catch Err As Exception
                ' Invalid feed.
                lblError.Text = "Invalid feed format.<br><br>"
                lblError.Text &= Err.Message
            End Try
        End If
    End Sub

End Class