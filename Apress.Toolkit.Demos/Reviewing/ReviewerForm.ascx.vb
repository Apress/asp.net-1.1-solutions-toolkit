Imports Apress.Toolkit

Public MustInherit Class ReviewerForm
    Inherits System.Web.UI.UserControl
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtName As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents txtReview As System.Web.UI.WebControls.TextBox
    Protected WithEvents rblRating As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents ibSubmit As System.Web.UI.WebControls.ImageButton
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator2 As System.Web.UI.WebControls.RequiredFieldValidator

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


    Dim _connection As String
    Dim _displayThankYouNote As Boolean = False

  Property ConnectionString() As String
    Get
      Return _connection
    End Get

    Set(ByVal Value As String)
      _connection = Value
    End Set
  End Property

  Dim _productid As Integer

  Public Property ProductID() As Integer
    Get
      Return _productid
    End Get

    Set(ByVal Value As Integer)
      _productid = Value
    End Set
  End Property

  Private Sub ibSubmit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibSubmit.Click
        If Page.IsValid Then
            Dim dbcontrol As New ReviewerDB
            dbcontrol.ConnectionString = ConnectionString
            dbcontrol.ProductID = ProductID
            dbcontrol.InsertReview(txtName.Text, txtReview.Text, Integer.Parse(rblRating.SelectedItem.Value))
            _displayThankYouNote = True
        End If
    End Sub


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If (_displayThankYouNote) Then
            RenderThankYouNote(writer)
        Else
            MyBase.Render(writer)
        End If
    End Sub

    Sub RenderThankYouNote(ByVal writer As HtmlTextWriter)
        Dim Str As String = "Thank you for submitting your review! You can <a href='javascript:window.close()'>close</a> this window now"
        writer.Write(String.Format("<html><body><p>{0}</p></body></html>", Str))
    End Sub
End Class
