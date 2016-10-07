Imports System
Imports System.Xml

Namespace Apress.Toolkit

    ' All classes are based on the most widely used version, RSS 0.91.
    Public Class RssChannel

        Private _DocumentVersion As String
        Private _Title As String
        Private _Link As String
        Private _Description As String

        Private _Image As New RssImage
        Private _Items As New RssItemCollection

        Public Property Items() As RssItemCollection
            Get
                Return _Items
            End Get
            Set(ByVal Value As RssItemCollection)
                _Items = Value
            End Set
        End Property

        Public Property Image() As RssImage
            Get
                Return _Image
            End Get
            Set(ByVal Value As RssImage)
                _Image = Value
            End Set
        End Property

        Public Property DocumentVersion() As String
            Get
                Return _DocumentVersion
            End Get
            Set(ByVal Value As String)
                _DocumentVersion = Value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal Value As String)
                _Title = Value
            End Set
        End Property

        Public Property Link() As String
            Get
                Return _Link
            End Get
            Set(ByVal Value As String)
                _Link = Value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal Value As String)
                Value = _Description
            End Set
        End Property

    End Class

    Public Class RssImage

        Private _PictureUrl As String
        Private _Title As String
        Private _Link As String

        Public Property PictureUrl() As String
            Get
                Return _PictureUrl
            End Get
            Set(ByVal Value As String)
                _PictureUrl = Value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal Value As String)
                _Title = Value
            End Set
        End Property

        Public Property Link() As String
            Get
                Return _Link
            End Get
            Set(ByVal Value As String)
                _Link = Value
            End Set
        End Property

    End Class

    Public Class RssItem

        Private _Title As String
        Private _Link As String
        Private _Description As String

        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal Value As String)
                _Title = Value
            End Set
        End Property

        Public Property Link() As String
            Get
                Return _Link
            End Get
            Set(ByVal Value As String)
                _Link = Value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal Value As String)
                _Description = Value
            End Set
        End Property

    End Class

    Public Class RssItemCollection
        Inherits System.Collections.CollectionBase

        Public Sub Add(ByVal item As RssItem)
            List.Add(item)
        End Sub

        Public Sub Remove(ByVal index As Integer)
            If index > Count - 1 Or index < 0 Then
                Throw New ArgumentException("No item at the specified index.")
            Else
                List.RemoveAt(index)
            End If
        End Sub

        Default Public Property Item(ByVal index As Integer) As RssItem
            Get
                Return CType(List.Item(index), RssItem)
            End Get
            Set(ByVal Value As RssItem)
                list.Item(index) = Value
            End Set
        End Property

    End Class
End Namespace
