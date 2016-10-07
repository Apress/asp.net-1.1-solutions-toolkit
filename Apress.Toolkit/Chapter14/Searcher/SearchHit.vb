Imports System

Namespace Apress.Toolkit.SearchEngine

    <Serializable()> _
    Public Class SearchHit
        Inherits IndexEntry

        Private _rank As Integer
        Private _percent As Decimal

        Public ReadOnly Property Rank() As Integer
            Get
                Return _rank
            End Get

        End Property

        Public ReadOnly Property Percent() As Decimal
            Get
                Return _percent
            End Get
        End Property

        Public Sub New(ByVal page As IndexEntry, _
          ByVal rank As Integer, ByVal percent As Decimal)
            MyBase.New(page.Url)
            _rank = rank
            _percent = percent
            Me.Description = page.Description
            Me.Keywords = page.Keywords
            Me.Title = page.Title
        End Sub

    End Class

End Namespace

