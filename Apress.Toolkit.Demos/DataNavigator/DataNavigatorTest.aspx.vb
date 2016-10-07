Imports System.Data.SqlClient
Imports Apress.Toolkit



Public Class DataNavTest
    Inherits System.Web.UI.Page
    Protected WithEvents DataNavigator1 As Apress.Toolkit.DataNavigator

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

    Private Sub Page_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load


        If Not (Me.IsPostBack) Then
            ' This logic creates a connection string to the default .NET SDK local instance of MSDE running Northwind.
            Dim ConnString As String = "data source={0};initial catalog=Northwind;integrated security=SSPI;persist security info=False;workstation id={0};packet size=4096"
            '    For a local SQL Server instance running Northwind, use the following line:
            '    Dim ConnString As String = "data source={0};initial catalog=Northwind;integrated security=SSPI;persist security info=False;workstation id={0};packet size=4096"


            Dim HostName As String = Server.MachineName
            ConnString = String.Format(ConnString, HostName)

            Dim cnn As New SqlConnection(ConnString)
            Dim sql As String = "SELECT ProductName, UnitPrice, " & _
                "Discontinued FROM Products"
            'Dim sql As String = "SELECT ShipName," & _
            '"OrderDate FROM Orders"
            Dim da As New SqlDataAdapter
            Dim cmd As New SqlCommand
            Dim ds As New DataSet
            Dim tbl As New DataTable
            Dim DataTypes() As System.TypeCode = _
                {TypeCode.String, TypeCode.String, TypeCode.Boolean}

            With cmd
                .CommandText = sql
                .CommandType = CommandType.Text
                .Connection = cnn
            End With

            Try
                da.MissingSchemaAction = MissingSchemaAction.Add
                da.SelectCommand = cmd
                da.Fill(tbl)
                da.FillSchema(tbl, SchemaType.Mapped)
                ds.Tables.Add(tbl)

            Finally
                If cnn.State <> ConnectionState.Closed Then
                    cnn.Close()
                End If
                cmd.Dispose()
                cmd = Nothing
                da.Dispose()
                da = Nothing
            End Try

            With DataNavigator1
                .DataSource = ds
                '.DataTypes = DataTypes
            End With
        End If


    End Sub


    Private Sub DataNavigator1_ButtonClick(ByVal sender As Object, ByVal e As Apress.Toolkit.DataNavigatorButtonEventArgs) Handles DataNavigator1.ButtonClick
        Select Case e.ButtonClickType
            Case DataNavigatorButtonEventArgs.ButtonType.CancelButton
                ' Add ADO.NET code

            Case DataNavigatorButtonEventArgs.ButtonType.NextButton
                ' Add ADO.NET code

            Case DataNavigatorButtonEventArgs.ButtonType.PrevButton
                ' Add ADO.NET code

            Case DataNavigatorButtonEventArgs.ButtonType.SaveButton
                ' Add ADO.NET code
        End Select

    End Sub
End Class
