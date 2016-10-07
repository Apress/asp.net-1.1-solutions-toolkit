Imports System.IO

Namespace Apress.Toolkit.SearchEngine

    Public Interface IIndexer
        Function IndexPage(ByVal pageContent As Stream, _
                           ByVal url As String) As IndexEntry
    End Interface

End Namespace