Public Class DataBindBullets
    Inherits System.Web.UI.Page
    Protected WithEvents DataBindBulletList1 As Apress.Toolkit.DataBindBulletList

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
    If Not IsPostBack Then
      Dim Source As New ArrayList()
            Source.Add("More pay")
            Source.Add("Less working hours")
            Source.Add("Longer vacations")
      DataBindBulletList1.DataSource = Source

      DataBindBulletList1.DataBind()
    End If

  End Sub

End Class
