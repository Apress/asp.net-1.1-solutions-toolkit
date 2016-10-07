Imports System.IO
Imports System.Text.RegularExpressions

Namespace Apress.Toolkit.SearchEngine

    Public Class HtmlMetaTagIndexer
        Implements IIndexer

        Private Const NameRegexPattern As String = "<meta\s+name\s*=\s*(""|')"
        Private Const ContentRegexPattern As String = "\1\s+content\s*=\s*(""|')\s*(?<match>.*?)\s*\2\s*/?>"
        Private Shared TitlePattern As String = "<title>(?<match>.*?)</title>"

        Private Shared MetaKeywords As New Regex(NameRegexPattern & "keywords" & ContentRegexPattern, RegexOptions.IgnoreCase Or RegexOptions.Singleline)
        Private Shared MetaDescription As New Regex(NameRegexPattern & "description" & ContentRegexPattern, RegexOptions.IgnoreCase Or RegexOptions.Singleline)
        Private Shared TitleRegex As New Regex(TitlePattern, RegexOptions.IgnoreCase Or RegexOptions.Singleline)

        Public Function IndexPage(ByVal pageContent As Stream, ByVal url As String) As IndexEntry Implements IIndexer.IndexPage

            Dim Reader As New StreamReader(pageContent)
            Dim Html As String = Reader.ReadToEnd()
            Reader.Close()
            Dim KeywordMatch As Match = MetaKeywords.Match(Html)
            Dim Keywords As String
            If KeywordMatch.Success Then
                Keywords = KeywordMatch.Groups("match").Value
            End If

            If Keywords = String.Empty Then
                ' This page should not be indexed.
                Return Nothing
            End If
            Dim DescriptionMatch As Match = MetaDescription.Match(Html)
            Dim Description As String
            If DescriptionMatch.Success Then
                Description = DescriptionMatch.Groups("match").Value
            End If
            Dim TitleMatch As Match = TitleRegex.Match(Html)
            Dim Title As String
            If TitleMatch.Success Then
                Title = TitleMatch.Groups("match").Value
            End If

            Dim Entry As IndexEntry = New IndexEntry(url)
            Entry.Title = Title
            Entry.Description = Description
            Entry.Keywords = Keywords
            Return Entry

        End Function

    End Class

End Namespace