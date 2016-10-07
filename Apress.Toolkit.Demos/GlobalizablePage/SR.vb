Imports System.Resources
Imports System.Globalization

Friend NotInheritable Class SR

    Private resources As ResourceManager
    Private Shared singleton As SR
    Private Shared SyncRoot As New Object

    Shared Sub New()
        SR.singleton = Nothing
    End Sub

    Friend Sub New()
        Me.resources = New ResourceManager("SR", MyBase.GetType.Module.Assembly)
    End Sub

    Private Shared Function GetSingleton() As SR
        If SR.singleton Is Nothing Then
            SyncLock (SyncRoot)
                If SR.singleton Is Nothing Then
                    SR.singleton = New SR
                End If
            End SyncLock
        End If
        Return SR.singleton
    End Function

    Public Shared Function GetString(ByVal name As String) As String
        Dim res As SR = SR.GetSingleton
        If (res Is Nothing) Then
            Return Nothing
        End If
        Return res.resources.GetString(name)
    End Function

    Public Shared ReadOnly Property WelcomeText() As String
        Get
            Return SR.GetString("WelcomeText")
        End Get
    End Property

    Public Shared ReadOnly Property GoodbyeText() As String
        Get
            Return SR.GetString("GoodbyeText")
        End Get
    End Property

End Class
