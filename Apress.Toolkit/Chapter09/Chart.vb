Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Data
Imports System.Data.OleDb
Imports System
Imports System.Collections

Namespace Apress.Toolkit

    Public Class Chart
        Inherits System.Web.UI.Page

        Public Enum ChartTypeValue
            Pie
            Histogram
        End Enum

        Private _type As ChartTypeValue

        Public Property ChartType() As ChartTypeValue
            Get
                Return _type
            End Get
            Set(ByVal Value As ChartTypeValue)
                _type = Value
            End Set
        End Property

        Private _connString As String
        Private _background As Color = Color.White
        Private _height As Integer
        Private _width As Integer
        Private _fontSize As Integer
        Private _title As String
        Private _ds As DataSet

        Public Property Height() As Integer
            Get
                Return _height
            End Get
            Set(ByVal Value As Integer)
                _height = Value
            End Set
        End Property

        Public Property Width() As Integer
            Get
                Return _width
            End Get
            Set(ByVal Value As Integer)
                _width = Value
            End Set
        End Property

        Public Property FontSize() As Integer
            Get
                Return _fontSize
            End Get
            Set(ByVal Value As Integer)
                _fontSize = Value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal Value As String)
                _title = Value
            End Set
        End Property

        Public Property ConnectionString() As String
            Get
                Return _connString
            End Get
            Set(ByVal Value As String)
                _connString = Value
            End Set
        End Property

        Public Property Background() As Color
            Get
                Return _background
            End Get
            Set(ByVal Value As Color)
                _background = Value
            End Set
        End Property

        Public Property DataSource() As DataSet
            Get
                Return _ds
            End Get
            Set(ByVal Value As DataSet)
                _ds = Value
            End Set
        End Property

        Private Sub DrawChart(ByVal title As String, _
         ByVal iColumn As Integer, ByVal iLabel As Integer)

            Response.ContentType = "image/gif"

            ' Create BMP canvas
            Dim bmp As New Bitmap(Width, Height)
            Dim objGraphic As Graphics = Graphics.FromImage(bmp)
            objGraphic.FillRectangle(New SolidBrush(Background), 0, 0, _
               Width, Height)

            If Not _ds Is Nothing Then
                ' Calculate the total
                CalculateTotal(_ds, 1)

                ' Generate random colors
                Dim I As Integer
                Dim colors As New ArrayList
                Dim rnd As New Random
                For I = 0 To _ds.Tables(0).Rows.Count - 1
                    colors.Add(New SolidBrush(Color.FromArgb(rnd.Next(255), _
                                                             rnd.Next(255), _
                                                             rnd.Next(255))))
                Next

                Dim iChartWidth As Integer = CInt(Width / 2)
                Dim iChartHeight As Integer = Height

                Dim iTotalColumnPos As Integer = _ds.Tables(0).Columns.Count - 1

                Select Case ChartType

                    Case ChartTypeValue.Pie

                        ' Check maximum pie width available
                        Dim iPieWidth As Integer = iChartWidth
                        If iChartWidth > iChartHeight Then
                            iPieWidth = iChartHeight
                        End If

                        ' Draw pie
                        Dim degree As Single
                        Dim pieRect As New Rectangle(0, 0, (iPieWidth - 10), _
                                                           (iPieWidth - 10))

                        For I = 0 To _ds.Tables(0).Rows.Count - 1
                            objGraphic.FillPie(CType(colors(I), Brush), pieRect, _
                                degree, CSng(CInt(_ds.Tables(0).Rows(I)(iColumn)) / _
                                CInt(_ds.Tables(0).Rows(I)(iTotalColumnPos)) * 360))
                            degree = degree + _
                                CSng((CInt(_ds.Tables(0).Rows(I)(iColumn)) / _
                                CInt(_ds.Tables(0).Rows(I)(iTotalColumnPos)) * 360))
                        Next

                    Case ChartTypeValue.Histogram

                        ' Find number of columns in chart
                        Dim noColumns As New Integer
                        noColumns = _ds.Tables(0).Rows.Count

                        ' Find max column height
                        Dim maxHeight As Integer = 0
                        For I = 0 To _ds.Tables(0).Rows.Count - 1
                            If (CInt(_ds.Tables(0).Rows(I)(iColumn)) > maxHeight) Then
                                maxHeight = CInt(_ds.Tables(0).Rows(I)(iColumn))
                            End If
                        Next

                        ' Draw histogram
                        Dim histogramRect As New Rectangle
                        histogramRect.Width = CInt((iChartWidth / noColumns)) - 1
                        For I = 0 To _ds.Tables(0).Rows.Count - 1
                            Dim dbl As Double
                            histogramRect.X = CInt(I * _
                                           (iChartWidth / _ds.Tables(0).Rows.Count))
                            dbl = 1 - CInt(_ds.Tables(0).Rows(I)(iColumn)) / maxHeight
                            histogramRect.Y = CInt(iChartHeight * dbl)
                            dbl = CInt(_ds.Tables(0).Rows(I)(iColumn)) / maxHeight
                            histogramRect.Height = CInt(iChartHeight * dbl)
                            objGraphic.FillRectangle(CType(colors(I), Brush), histogramRect)
                        Next
                End Select

                DrawLegend(objGraphic, colors, title, iLabel)

                bmp.Save(Page.Response.OutputStream, ImageFormat.Gif)
            End If

            bmp.Dispose()
            objGraphic.Dispose()
        End Sub

        Private Sub CalculateTotal(ByRef ds As DataSet, _
               ByVal iColumn As Integer)

            ' Add a column that will contain the total
            Dim cTotal As New DataColumn("Total")
            cTotal.Expression = "Sum(" & _
                                ds.Tables(0).Columns(iColumn).ColumnName & ")"
            cTotal.DataType = Type.GetType("System.Single")
            ds.Tables(0).Columns.Add(cTotal)
        End Sub

        Private Sub DrawLegend(ByRef objGraphics As _
                               Graphics, ByRef colors As ArrayList, _
                               ByVal title As String, _
                               ByVal iColumn As Integer)


            Dim I As Integer
            Dim f As New Font("Tahoma", FontSize)
            Dim fTitle As New Font("Arial", FontSize, FontStyle.Bold)

            Dim solidBlack As New SolidBrush(Color.Black)
            Dim iLegendWidth As Integer = CInt(Width / 2)
            Dim iLegendHeight As Integer = Height
            Dim rect As New Rectangle
            rect.X = iLegendWidth
            rect.Y = 0
            rect.Height = iLegendWidth - 1
            rect.Width = iLegendWidth - 1

            objGraphics.DrawRectangle(New Pen(Color.Black, 1), rect)
            objGraphics.DrawString(title, fTitle, solidBlack, _
                                                        (iLegendWidth + 1), 0)

            For I = 0 To _ds.Tables(0).Rows.Count - 1
                rect.X = iLegendWidth + 5
                rect.Y = 30 + f.Height * I
                rect.Height = 10
                rect.Width = 10
                objGraphics.FillRectangle(CType(colors(I), Brush), rect)

                objGraphics.DrawString(_ds.Tables(0).Rows(I)(iColumn).ToString(), _
                      f, solidBlack, iLegendWidth + 20, _
                      30 + f.Height * I - 3)
            Next

            Dim iTotalColumnPos As Integer = _ds.Tables(0).Columns.Count - 1
            objGraphics.DrawString("Total: " & _
                            _ds.Tables(0).Rows(0)(iTotalColumnPos).ToString(), _
                            f, solidBlack, iLegendWidth, _
                           (iLegendWidth + 1) - f.Height - 5)

        End Sub

        Protected Overrides Sub Render( _
                          ByVal writer As System.Web.UI.HtmlTextWriter)
            DrawChart(Title, 1, 0)
        End Sub

        Public Function LoadDataSourceBySELECT(ByVal SQL As String) _
             As DataSet


            Dim dbConn As New OleDbConnection(ConnectionString)


            Dim da As New OleDbDataAdapter(SQL, dbConn)
            Dim ds As New DataSet

            Try
                da.Fill(ds)

                'CalculateTotal(ds, 1)
                Return ds
            Catch e As Exception
                Return Nothing
            End Try
        End Function

        Public Function LoadDataSourceBySP( _
                                ByVal storedProcedure As String, _
          ByVal arrayParams As ArrayList) As DataSet

            Dim dbConn As New OleDbConnection(ConnectionString)

            Dim dbComm As New OleDbCommand
            dbComm.CommandText = storedProcedure
            dbComm.CommandType = CommandType.StoredProcedure

            Dim I As Integer
            For I = 0 To arrayParams.Count - 1
                dbComm.Parameters.Add(arrayParams(I))
            Next

            dbComm.Connection = dbConn

            Dim da As New OleDbDataAdapter(dbComm)
            Dim ds As New DataSet

            Try
                dbConn.Open()
                da.Fill(ds)

                Return ds

            Catch e As Exception
                Return Nothing
            Finally
                If dbConn.State = ConnectionState.Open Then
                    dbConn.Close()
                End If
            End Try

        End Function

    End Class

End Namespace
