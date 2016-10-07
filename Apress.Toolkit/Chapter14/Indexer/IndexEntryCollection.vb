Option Explicit On 

Imports System

Namespace Apress.Toolkit.SearchEngine
    <Serializable()> _
    Public Class IndexEntryCollection
        Inherits System.Collections.CollectionBase

        Public Sub Add(ByVal item As IndexEntry)
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

        Default Public Property Item(ByVal index As Integer) As IndexEntry
            Get
                Return DirectCast(List.Item(index), IndexEntry)
            End Get
            Set(ByVal value As IndexEntry)
                List.Item(index) = value
            End Set
        End Property

    End Class
End Namespace