Imports System
Imports System.Net
Imports System.IO

Namespace Apress.Toolkit.SearchEngine

    Public Class SearchIndexer
        Inherits System.ComponentModel.Component

        Private _indexer As IIndexer

        Public ReadOnly Property Indexer() As IIndexer
            Get
                Return _indexer
            End Get
        End Property

        Public Sub New()
            Me.New(New HtmlMetaTagIndexer)
        End Sub

        Public Sub New(ByVal indexer As IIndexer)
            If indexer Is Nothing Then
                Throw New ArgumentNullException("indexer", _
                    "Must pass in a real IIndexer object.")
            Else
                Me._indexer = indexer
            End If
        End Sub

        Public Function IndexUrl(ByVal url As String) As IndexEntry
            Return IndexPage(url)
        End Function

        Private Function IndexPage(ByVal url As String) As IndexEntry

            If url Is Nothing Then
                Throw New ArgumentNullException("url", _
                  "Must pass a string object")
            ElseIf url = String.Empty Then
                Throw New ArgumentException("url", _
                  "Must pass a URL as a string")
            End If

            Dim Request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
            Dim Response As WebResponse = Request.GetResponse()

            Return _indexer.IndexPage(Response.GetResponseStream(), url)

        End Function

        Public Function IndexUrls(ByVal urls() As String) _
                          As IndexEntryCollection

            Dim Index As New IndexEntryCollection

            Dim Url As String
            For Each Url In urls
                Dim Page As IndexEntry = IndexPage(Url)
                If Not (Page Is Nothing) Then Index.Add(Page)
            Next

            Return Index

        End Function

        Public Function IndexSite(ByVal baseUrl As String, _
          ByVal physicalPath As String) As IndexEntryCollection

            If Not baseUrl.EndsWith("/") Then baseUrl &= "/"

            Dim Index As New IndexEntryCollection
            Dim SiteDirectory As New DirectoryInfo(physicalPath)
            Dim File As FileInfo

            For Each File In SiteDirectory.GetFiles()
                If File.Extension.ToLower = ".aspx" Or _
                  File.Extension.ToLower = ".asp" Or _
                  File.Extension.ToLower = ".htm" Or _
                  File.Extension.ToLower = ".html" Then

                    Dim Page As IndexEntry
                    Page = IndexPage(baseUrl & File.Name)

                    If Not (Page Is Nothing) Then
                        Index.Add(Page)
                    End If
                End If
            Next

            Return Index

        End Function

        Public Function UpdateIndex(ByVal currentIndex As IndexEntryCollection) As IndexEntryCollection

            Dim NewIndex As New IndexEntryCollection

            Dim Page As IndexEntry
            For Each Page In currentIndex
                Dim NewPage As IndexEntry = IndexPage(Page.Url)
                If Not (NewPage Is Nothing) Then NewIndex.Add(NewPage)
            Next

            Return NewIndex

        End Function

    End Class


End Namespace