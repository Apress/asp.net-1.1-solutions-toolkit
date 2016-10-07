Imports Apress.Toolkit.SearchEngine

Public Class index
    Inherits System.Web.UI.Page
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.Button
    Protected WithEvents cmdLoad As System.Web.UI.WebControls.Button
    Protected WithEvents txtQuery As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents ListMatches As System.Web.UI.WebControls.DataList
    Protected WithEvents lstSearchType As System.Web.UI.WebControls.ListBox
    Protected WithEvents lstPhraseMatch As System.Web.UI.WebControls.ListBox
    Protected WithEvents Util As Apress.Toolkit.SearchEngine.IndexFileUtility
    Protected WithEvents Searcher As Apress.Toolkit.SearchEngine.SearchParser
    Protected WithEvents ListIndex As System.Web.UI.WebControls.DataList

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Util = New Apress.Toolkit.SearchEngine.IndexFileUtility
        Me.Searcher = New Apress.Toolkit.SearchEngine.SearchParser
        '
        'Searcher
        '
        Me.Searcher.Index = Nothing
        Me.Searcher.PhraseMatch = Apress.Toolkit.SearchEngine.PhraseMatch.InlineQuotes
        Me.Searcher.SearchType = Apress.Toolkit.SearchEngine.SearchType.MatchAny
        Me.Searcher.TreatSingleCharAsNoise = True

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Dim index As IndexEntryCollection
        index = Util.Load(Request.PhysicalApplicationPath & "index.bin")

        Searcher.PhraseMatch = CType(lstPhraseMatch.SelectedIndex, PhraseMatch)
        Searcher.SearchType = CType(lstSearchType.SelectedIndex, SearchType)

        Searcher.Index = index

        Dim Matches As SearchHitCollection = Searcher.Search(txtQuery.Text)

        ListMatches.DataSource = Matches
        ListMatches.DataBind()

    End Sub
End Class
