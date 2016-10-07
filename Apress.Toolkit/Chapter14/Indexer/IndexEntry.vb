Imports System


Namespace Apress.Toolkit.SearchEngine

    <Serializable()> _
    Public Class IndexEntry
        Private _url As String
        Private _title As String
        Private _description As String
        Private _keywords As String

        Public Property Url() As String
            Get
                Return Me._url
            End Get
            Set(ByVal value As String)
                Me._url = value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal value As String)
                Me._title = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return Me._description
            End Get
            Set(ByVal value As String)
                Me._description = value
            End Set
        End Property

        Public Property Keywords() As String
            Get
                Return Me._keywords
            End Get
            Set(ByVal value As String)
                Me._keywords = value
            End Set
        End Property

        Public Sub New(ByVal url As String)
            Me.Url = url
        End Sub

    End Class
End Namespace

