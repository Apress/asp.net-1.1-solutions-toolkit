Public Class Sample
  Inherits System.Web.UI.Page
  Protected WithEvents txtName As System.Web.UI.WebControls.TextBox
    Protected WithEvents valName As Apress.Toolkit.Validation.ValidatorControl
    Protected WithEvents txtAge As System.Web.UI.WebControls.TextBox
    Protected WithEvents valAge As Apress.Toolkit.Validation.ValidatorControl
    Protected WithEvents txtZip As System.Web.UI.WebControls.TextBox
    Protected WithEvents valZip As Apress.Toolkit.Validation.ValidatorControl
    Protected WithEvents txtAccount As System.Web.UI.WebControls.TextBox
    Protected WithEvents valAccount As Apress.Toolkit.Validation.ValidatorControl
    Protected WithEvents txtEarned As System.Web.UI.WebControls.TextBox
    Protected WithEvents valCredits As Apress.Toolkit.Validation.ValidatorControl
    Protected WithEvents txtMissed As System.Web.UI.WebControls.TextBox
    Protected WithEvents valDebits As Apress.Toolkit.Validation.ValidatorControl
    Protected WithEvents txtRequired As System.Web.UI.WebControls.TextBox
    Protected WithEvents valRequired As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtUserID As System.Web.UI.WebControls.TextBox
    Protected WithEvents Validatorcontrol2 As Apress.Toolkit.Validation.ValidatorControl
  Protected WithEvents btnSubmit As System.Web.UI.WebControls.Button
  Protected WithEvents txtSummary As System.Web.UI.WebControls.ValidationSummary

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

End Class
