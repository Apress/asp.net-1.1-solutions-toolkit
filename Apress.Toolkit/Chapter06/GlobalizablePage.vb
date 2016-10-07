Imports System
Imports System.Web.UI
Imports System.Globalization
Imports System.Threading
Imports System.Text.RegularExpressions

Namespace Apress.Toolkit

    Public Class GlobalizablePage
        Inherits Page

        Private _culture As CultureInfo = CultureInfo.CurrentUICulture
        Private _underlyingUICulture As CultureInfo _
                                  = CultureInfo.CurrentUICulture
        Private _underlyingCulture As CultureInfo _
                                  = CultureInfo.CurrentCulture
        Private _supportedCultures As String() _
                                  = InitSupportedCultures()

        Private ReadOnly cultureSpecRegEx As String _
                                  = "[a-zA-Z]{2,3}(-[a-zA-Z]{2})?"

        Public ReadOnly Property SelectedCulture() As CultureInfo
            Get
                Return _culture
            End Get
        End Property

        Public ReadOnly Property SupportedCultures() As String()
            Get
                Return _supportedCultures
            End Get
        End Property

        Public Overridable Function IsCultureSupported( _
                         ByVal culture As CultureInfo) As Boolean
            Return Array.IndexOf(SupportedCultures, _
                         culture.TwoLetterISOLanguageName) >= 0
        End Function

        Public Overridable Function InitSupportedCultures() _
                                                        As String()
            Dim a() As String _
                      = {_underlyingCulture.TwoLetterISOLanguageName}
            Return a
        End Function

        Protected NotOverridable Overrides Sub OnInit( _
                                     ByVal e As System.EventArgs)
            Dim requestedLang As String
            Dim cultureFinder As New Regex(cultureSpecRegEx)

            For Each requestedLang In Request.UserLanguages
                Try
                    Dim requestedCulture As CultureInfo
                    Dim cultureSpec As String _
           = cultureFinder.Match(requestedLang).Captures(0).Value
                    requestedCulture _
           = CultureInfo.CreateSpecificCulture(cultureSpec)
                    If IsCultureSupported(requestedCulture) Then
                        _culture = requestedCulture
                        Thread.CurrentThread.CurrentUICulture _
                                                 = requestedCulture
                        Thread.CurrentThread.CurrentCulture _
                                                 = requestedCulture
                        Exit For
                    End If
                Catch ex As Exception
                End Try
            Next

            MyBase.OnInit(e)
        End Sub

        Protected Overrides Sub OnUnload(ByVal e As System.EventArgs)
            MyBase.OnUnload(e)
            Thread.CurrentThread.CurrentCulture = _underlyingCulture
            Thread.CurrentThread.CurrentUICulture = _underlyingUICulture
        End Sub
    End Class
End Namespace
