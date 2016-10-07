Public Class ImageMap
    Inherits System.Web.UI.Page
    Protected WithEvents ImageMap1 As Apress.Toolkit.ImageMap
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label

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
        'Put user code to initialize the page here
    End Sub

    Private Sub ImageMap1_RegionClicked(ByVal sender As System.Object, ByVal e As Apress.Toolkit.ImageMapEventArgs) Handles ImageMap1.RegionClicked
        Label1.Text = "You clicked region " & e.RegionID
    End Sub
End Class
