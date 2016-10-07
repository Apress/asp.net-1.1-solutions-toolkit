Imports System.Data.OleDb

Public Class TestChart
    Inherits Apress.Toolkit.Chart

    Private Sub Page_Load(ByVal sender As System.Object, _
         ByVal e As System.EventArgs) Handles MyBase.Load
        ConnectionString = "Provider=SQLOLEDB.1;" & _
                           "Persist Securit y Info=False;User ID=sa;pwd=sa;" & _
                           "Initial Catalog=Northwind;Data Source=."

        ' Load data source
        Dim p1 As New OleDbParameter("@CategoryName", _
           OleDbType.VarChar, 15)
        Dim p2 As New OleDbParameter("@OrdYear", OleDbType.VarChar, 4)

        p1.Value = "Beverages"

        Dim arParams As New ArrayList
        arParams.Add(p1)
        arParams.Add(p2)

        Me.DataSource = LoadDataSourceBySP("SalesByCategory", arParams)

        Background = Color.White
        ChartType = Apress.Toolkit.Chart.ChartTypeValue.Pie

        Title = "Sales By Category - Beverages"
        Height = 400
        Width = 650
        FontSize = 10

    End Sub
End Class
