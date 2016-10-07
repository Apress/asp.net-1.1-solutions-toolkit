Imports System.Xml

Namespace Apress.Toolkit
    Public Class RssReader
        Inherits System.ComponentModel.Component

        Public Function GetChannel(ByVal channelUrl As String) As RssChannel

            ' Load the XML document from the specified URL.
            Dim Document As New XmlDocument
            Dim Node As XmlNode, Nodes As XmlNodeList
            Document.Load(channelUrl)

            ' Create and configure the Channel.
            Dim Channel As New RssChannel

            ' Point to 'rss' element to get the version.
            Node = Document.GetElementsByTagName("rss")(0)
            Channel.DocumentVersion = Node.Attributes("version").Value

            ' Point to 'channel' element.
            Node = Document.GetElementsByTagName("channel")(0)
            Channel.Title = Node.Item("title").InnerText
            Channel.Link = Node.Item("link").InnerText
            Channel.Description = Node.Item("description").InnerText

            ' Set the image.
            Node = Document.GetElementsByTagName("image")(0)
            If Not (Node Is Nothing) Then
                Channel.Image.Title = Node.Item("title").InnerText
                Channel.Image.PictureUrl = Node.Item("url").InnerText
                Channel.Image.Link = Node.Item("link").InnerText
            End If

            ' Get all <item> nodes.
            Nodes = Document.GetElementsByTagName("item")
            For Each Node In Nodes
                Dim Item As New RssItem
                Item.Title = Node.Item("title").InnerText
                Item.Link = Node.Item("link").InnerText
                If Not (Node.Item("description") Is Nothing) Then
                    Item.Description = Node.Item("description").InnerText
                End If
                Channel.Items.Add(Item)
            Next

            Return Channel

        End Function

    End Class
End Namespace
