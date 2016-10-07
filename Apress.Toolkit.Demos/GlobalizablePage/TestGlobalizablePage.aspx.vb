Imports System.Resources

Public Class TestGlobalizablePage
    Inherits Apress.Toolkit.GlobalizablePage
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Calendar1 As System.Web.UI.WebControls.Calendar

    Public Overrides Function InitSupportedCultures() As String()
        Dim langs() As String = {"en", "fr", "de", "es"}
        Return langs
    End Function

    Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label1.Text = SR.WelcomeText
        Label2.Text = SR.GoodbyeText
    End Sub
End Class

