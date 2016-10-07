Imports System
Imports System.Web.UI
Imports System.Data
Imports System.IO
Imports System.Web.UI.WebControls
Imports System.Web.Caching
Imports System.Web

Namespace Apress.Toolkit

    Public Class VoteEventArgs
        Inherits System.EventArgs

        Private _voteValue As String
        Public Property VoteValue() As String
            Get
                Return Me._voteValue
            End Get

            Set(ByVal value As String)
                Me._voteValue = value
            End Set
        End Property

        Private _voteText As String
        Property VoteText() As String
            Get
                Return Me._voteText
            End Get

            Set(ByVal value As String)
                Me._voteText = value
            End Set
        End Property

        Sub New(ByVal voteText As String, ByVal voteValue As String)
            Me._voteValue = voteValue
            Me._voteText = voteText
        End Sub
    End Class
End Namespace