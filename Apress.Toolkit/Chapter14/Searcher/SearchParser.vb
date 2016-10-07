Imports System
Imports System.Collections.Specialized

Namespace Apress.Toolkit.SearchEngine

    Public Class SearchParser
        Inherits System.ComponentModel.Component

        Private _searchType As SearchType = SearchType.MatchAny

        Public Property SearchType() As SearchType
            Get
                Return _searchType
            End Get
            Set(ByVal Value As SearchType)
                _searchType = Value
            End Set
        End Property

        Private _phraseMatch As PhraseMatch = PhraseMatch.InlineQuotes

        Public Property PhraseMatch() As PhraseMatch
            Get
                Return _phraseMatch
            End Get
            Set(ByVal Value As PhraseMatch)
                _phraseMatch = Value
            End Set
        End Property

        Private _noiseWords As New StringCollection
        Public ReadOnly Property NoiseWords() As StringCollection
            Get
                Return _noiseWords
            End Get
        End Property

        Private _stripChars As New StringCollection

        Public ReadOnly Property StripChars() As StringCollection
            Get
                Return _stripChars
            End Get
        End Property

        Private _spaceChars As New StringCollection

        Public ReadOnly Property SpaceChars() As StringCollection
            Get
                Return _spaceChars
            End Get
        End Property

        Private _treatSingleCharAsNoise As Boolean = True

        Public Property TreatSingleCharAsNoise() As Boolean
            Get
                Return _treatSingleCharAsNoise
            End Get
            Set(ByVal Value As Boolean)
                _treatSingleCharAsNoise = Value
            End Set
        End Property

        Private _index As IndexEntryCollection

        Public Property Index() As IndexEntryCollection
            Get
                Return _index
            End Get
            Set(ByVal Value As IndexEntryCollection)
                _index = Value
            End Set
        End Property

        Public Sub New()

            Dim Noise() As String = _
              {"the", "a", "an", "or", "and", "to", "of"}
            Dim Strip() As String = _
              {"?", "@", "$", "%", "^", "&", "*", "(", ")"}
            Dim Space() As String = {",", ";", ":", "/", "\"}

            ' We copy the arrays into string collections.
            ' This simplifies the code, because you can check for a
            ' character using the Contains() method, instead of iterating
            ' through the array manually.
            Me.NoiseWords.AddRange(Noise)
            Me.StripChars.AddRange(Strip)
            Me.SpaceChars.AddRange(Space)

        End Sub

        Public Function Search(ByVal query As String) _
          As SearchHitCollection

            ' 1. Strip out special characters.
            query = FilterCharacters(query)

            ' 2. Break the query down into words.
            ' This procedure updates the RequiredWords and OptionalWords
            ' string arrays based on the query and the SearchType.
            ' Note that excluded words are only supported for
            ' SearchType.MatchAdvanced queries.
            Dim RequiredWords, ExcludedWords, OptionalWords _
              As StringCollection

            ParseQuery(query, RequiredWords, ExcludedWords, OptionalWords)

            ' 3. Convert all words to a standard form, and remove noise words.
            CanonicalizeWords(RequiredWords)
            CanonicalizeWords(OptionalWords)
            CanonicalizeWords(ExcludedWords)

            ' 4. Now perform the search. This ranks each page that is a match.
            Dim Matches As SearchHitCollection
            Matches = MatchTerms(RequiredWords, ExcludedWords, OptionalWords)

            Return Matches

        End Function

        Private Function FilterCharacters(ByVal query As String) As String

            Dim StrippedQuery, LastChar As String
            Dim i As Integer

            For i = 0 To query.Length - 1

                If Me.StripChars.Contains(query.Chars(i)) Then
                    ' Do nothing (strip out special characters).

                ElseIf (Me.PhraseMatch <> PhraseMatch.InlineQuotes _
                        And query.Chars(i) = """") Then
                    ' Do nothing (strip out quotations if they aren't used.)

                Else
                    ' Continue with further processing.

                    If Me.SpaceChars.Contains(query.Chars(i)) Then
                        ' Convert space characters to spaces. This takes
                        ' effect even inside phrases in InlineQuotes mode.
                        If LastChar <> " " Then
                            StrippedQuery &= " "
                            LastChar = " "
                        End If

                    Else
                        If LastChar = " " And query.Chars(i) = " " Then
                            ' Duplicate spaces are ignored (collapsed).
                        Else
                            ' The character was not a filter or space
                            ' character. Copy the character as is.
                            StrippedQuery &= query.Chars(i)
                            LastChar = query.Chars(i)
                        End If
                    End If
                End If

            Next

            Return StrippedQuery

        End Function

        Private Sub ParseQuery(ByVal query As String, _
            ByRef requiredWords As StringCollection, _
            ByRef excludedWords As StringCollection, _
            ByRef optionalWords As StringCollection)

            ' Parse query for required, optional, and excluded words.
            requiredWords = New StringCollection
            optionalWords = New StringCollection
            excludedWords = New StringCollection

            ' First, handle the special case (single phrase mode).
            If Me.PhraseMatch = PhraseMatch.SinglePhrase Then
                requiredWords.Add(query)
                Return
            End If

            ' In all other search modes, the query must be examined more
            ' closely.
            Dim InQuotedPhrase, Required, Excluded As Boolean
            Dim CurrentWord As String

            Dim i As Integer
            For i = 0 To query.Length - 1

                ' Special handling at end of string.
                If (query.Chars(i) = " " And Not InQuotedPhrase) _
                  Or (i = query.Length - 1) Then

                    ' Make sure we catch the last character.
                    If i = query.Length - 1 And query.Chars(i) <> " " _
                      And query.Chars(i) <> """" _
                      Then CurrentWord &= query.Chars(i)

                    ' Check for special words in advanced mode.
                    If Me.SearchType = SearchType.MatchAdvanced And _
                      Not InQuotedPhrase And CurrentWord.ToLower = "and" Then

                        ' The next word is required.
                        Required = True

                    ElseIf Me.SearchType = SearchType.MatchAdvanced And _
                      Not InQuotedPhrase And CurrentWord.ToLower = "not" Then

                        ' The next word is excluded.
                        Excluded = True

                    Else
                        ' The word is complete.
                        ' Add it to the appropriate collection.
                        Select Case Me.SearchType

                            Case SearchType.MatchAny
                                optionalWords.Add(CurrentWord)

                            Case SearchType.MatchAll
                                requiredWords.Add(CurrentWord)

                            Case SearchType.MatchAdvanced
                                If Required Then
                                    requiredWords.Add(CurrentWord)
                                ElseIf Excluded Then
                                    excludedWords.Add(CurrentWord)
                                Else
                                    optionalWords.Add(CurrentWord)
                                End If

                                Required = False
                                Excluded = False

                        End Select
                    End If

                    InQuotedPhrase = False
                    CurrentWord = ""

                ElseIf query.Chars(i) = """" And _
                  Me.PhraseMatch = PhraseMatch.InlineQuotes Then
                    ' A quote phrase has started or ended.
                    ' If started, do not complete the word until another
                    ' quote is found.
                    InQuotedPhrase = Not InQuotedPhrase

                Else
                    ' This is an ordinary character.
                    ' Add it to the current word.
                    CurrentWord &= query.Chars(i)

                End If
            Next

        End Sub

        Private Sub CanonicalizeWords(ByVal words As StringCollection)

            Dim WordsToDelete As New StringCollection

            Dim i As Integer
            For i = 0 To words.Count - 1
                If Me.NoiseWords.Contains(words(i)) Then
                    WordsToDelete.Add(words(i))
                ElseIf Me.TreatSingleCharAsNoise And words(i).Length = 1 Then
                    WordsToDelete.Add(words(i))
                Else
                    words(i) = words(i).Trim().ToLower()
                End If
            Next

            Dim Word As String
            For Each Word In WordsToDelete
                words.Remove(Word)
            Next

        End Sub

        Public Function MatchTerms(ByVal RequiredWords As StringCollection, _
            ByVal ExcludedWords As StringCollection, _
            ByVal OptionalWords As StringCollection) As SearchHitCollection

            Dim Hits As New SearchHitCollection

            Dim Page As IndexEntry
            For Each Page In Index
                Dim Keywords As New StringCollection
                Keywords.AddRange(GetKeywordsForPage(Page))

                ' Optionally, this step could be performed when indexing.
                ' However, performing it later ensures the latest 
                ' canonicalization settings always take effect.
                CanonicalizeWords(Keywords)

                Dim Rank, TotalWords As Integer
                TotalWords = RequiredWords.Count + OptionalWords.Count

                Rank = RankPage(RequiredWords, ExcludedWords, _
                                OptionalWords, Keywords)

                If Rank > 0 Then
                    Hits.Add( _
                      New SearchHit(Page, Rank, CDec(Rank / TotalWords * 100)))
                End If
            Next

            Return Hits

        End Function

        Private Function GetKeywordsForPage(ByVal page As IndexEntry) _
            As String()

            Return page.Keywords.Split(","c)

        End Function

        Private Function RankPage(ByVal requiredWords As StringCollection, _
            ByVal excludedWords As StringCollection, _
            ByVal optionalWords As StringCollection, _
            ByVal keywords As StringCollection) As Integer

            Dim Rank As Integer

            ' Check for excluded words.
            ' If any are found, the rank is automatically 0.
            Dim Word As String
            For Each Word In excludedWords
                If keywords.Contains(Word) Then
                    Return 0
                End If
            Next

            ' Check for required words.
            ' If any are not found, the rank is automatically 0.
            For Each Word In requiredWords
                If keywords.Contains(Word) Then
                    Rank += 1
                Else
                    Return 0
                End If
            Next

            ' Check for optional words.
            For Each Word In optionalWords
                If keywords.Contains(Word) Then
                    Rank += 1
                End If
            Next

            Return Rank

        End Function

    End Class
End Namespace