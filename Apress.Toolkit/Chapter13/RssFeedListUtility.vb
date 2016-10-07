Imports System
Imports System.Xml

Namespace Apress.Toolkit

    Public Class RssFeedListReader
        Inherits System.ComponentModel.Component

        Public Function GetFeedList(ByVal feedUri As String) As RssFeedItemCollection

            ' Load the XML document from the specified URL.
            Dim Document As New XmlDocument
            Dim Node As XmlNode, Nodes As XmlNodeList
            Document.Load(feedUri)

            ' Create and configure the feed list.
            Dim Feeds As New RssFeedItemCollection

            ' Get all 'channel' nodes.
            Nodes = Document.GetElementsByTagName("channel")
            Dim x As Integer
            For Each Node In Nodes

                Dim Feed As New RssFeedItem
                Feed.Title = Node.Item("title").InnerText
                Feed.Link = Node.Item("link").InnerText
                Feed.Description = Node.Item("description").InnerText
                Feeds.Add(Feed)

            Next

            Return Feeds

        End Function

    End Class


    Public Class RssFeedItem

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

    Public Class RssFeedItemCollection
        Inherits System.Collections.CollectionBase

        Public Sub Add(ByVal item As RssFeedItem)
            List.Add(item)
        End Sub

        Public Sub Remove(ByVal index As Integer)
            If index > Count - 1 Or index < 0 Then
                Throw New ArgumentException("No item at the specified index.")
            Else
                List.RemoveAt(index)
            End If
        End Sub

        Default Public Property Item(ByVal index As Integer) As RssFeedItem
            Get
                Return CType(List.Item(index), RssFeedItem)
            End Get
            Set(ByVal Value As RssFeedItem)
                list.Item(index) = Value
            End Set
        End Property

    End Class
End Namespace
