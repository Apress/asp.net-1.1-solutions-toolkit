Imports Apress.Toolkit.SearchEngine

Public Class search
    Inherits System.Web.UI.Page
    Protected WithEvents Indexer As Apress.Toolkit.SearchEngine.SearchIndexer
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.Button
    Protected WithEvents cmdLoad As System.Web.UI.WebControls.Button
    Protected WithEvents ListIndex As System.Web.UI.WebControls.DataList
    Protected WithEvents Util As Apress.Toolkit.SearchEngine.IndexFileUtility

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Indexer = New Apress.Toolkit.SearchEngine.SearchIndexer
        Me.Util = New Apress.Toolkit.SearchEngine.IndexFileUtility

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Dim Urls(7) As String
        Urls(0) = "http://www.asptoday.com"
        Urls(1) = "http://www.csharptoday.com"
        Urls(2) = "http://msdn.microsoft.com"
        Urls(3) = "http://www.ebay.com"
        Urls(4) = "http://www.w3.org"
        Urls(5) = "http://www.amazon.com"
        Urls(6) = "http://www.pcmag.com"
        Urls(7) = "http://www.motleyfool.com"

        Dim Index As IndexEntryCollection = Indexer.IndexUrls(Urls)
        ListIndex.DataSource = Index
        ListIndex.DataBind()

        Util.Save(Index, Request.PhysicalApplicationPath & _
            "index.bin", True)
    End Sub

    Private Sub cmdLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLoad.Click
        Dim Index As IndexEntryCollection
        Index = Util.Load(Request.PhysicalApplicationPath & "index.bin")
        ListIndex.DataSource = Index
        ListIndex.DataBind()
    End Sub
End Class
