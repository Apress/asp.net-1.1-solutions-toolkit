Option Strict On

Imports System
Imports System.ComponentModel
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Design
Imports System.Drawing.Design
Imports System.Drawing
Imports System.Windows.Forms.Design
Imports System.Windows.Forms
Imports System.Collections
Imports Microsoft.VisualBasic

Namespace Apress.Toolkit
    Public Class ImageMapProperties
        Private _coords As ArrayList

        Public Sub New(ByVal designerString As String)
            Dim stringRepresentation As String
            If designerString Is Nothing Then
                stringRepresentation = ""
            Else
                stringRepresentation = designerString
            End If

            Dim vals() As String = stringRepresentation.Split(";"c)

            Dim rects As New ArrayList
            Dim coords() As String
            Dim valsLength As Integer = vals.Length
            Dim r As Rectangle
            Dim counter As Integer
            If valsLength > 0 Then
                For counter = 0 To valsLength - 1
                    coords = vals(counter).Split(","c)
                    If coords.Length = 4 Then
                        r = New Rectangle(CInt(coords(0)), CInt(coords(1)), CInt(coords(2)), CInt(coords(3)))
                        rects.Add(r)
                    End If
                Next
            End If
            _coords = rects

        End Sub

        Public Sub New(ByVal rectCoordinates As ArrayList)
            _coords = rectCoordinates
        End Sub

        Public Overrides Function ToString() As String
            If _coords Is Nothing Then Return ""
            Dim rectCoordsCount As Integer = _coords.Count
            Dim counter As Integer
            Dim stringRep As New System.Text.StringBuilder
            If rectCoordsCount > 0 Then
                For counter = 0 To rectCoordsCount - 1
                    Dim r As Rectangle
                    r = CType(_coords(counter), Rectangle)
                    If counter > 0 Then stringRep.Append(";")
                    stringRep.Append(r.X & ",")
                    stringRep.Append(r.Y & ",")
                    stringRep.Append(r.Width & "," & r.Height)
                Next
            End If
            Return stringRep.ToString()
        End Function

        Public Property RectCoords() As ArrayList
            Get
                Return _coords
            End Get

            Set(ByVal value As ArrayList)
                _coords = value
            End Set
        End Property

    End Class

End Namespace
