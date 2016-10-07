Imports System

Namespace Apress.Toolkit.SearchEngine

    <Serializable()> _
    Public Class SearchHitCollection
        Inherits System.Collections.CollectionBase

        Public Sub Add(ByVal item As SearchHit)
            List.Add(item)
        End Sub

        Public Sub Remove(ByVal index As Integer)
            If index > Count - 1 Or index < 0 Then
                Throw New ArgumentException( _
                    "No item at the specified index.")
            Else
                List.RemoveAt(index)
            End If
        End Sub

        Default Public Property Item(ByVal index As Integer) As SearchHit
            Get
                Return DirectCast(List.Item(index), SearchHit)
            End Get
            Set(ByVal value As SearchHit)
                List.Item(index) = value
            End Set
        End Property

    End Class
End Namespace