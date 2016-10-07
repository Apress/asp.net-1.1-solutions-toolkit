Imports System
Imports System.ComponentModel
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports System.Data
Imports System.Collections
Imports System.Text
Imports System.IO

Namespace Apress.Toolkit

    Public Class DataNavigatorButtonEventArgs
        Inherits CancelEventArgs

        Public Enum ButtonType
            NextButton
            PrevButton
            SaveButton
            CancelButton
        End Enum

        Private _DataSet As DataSet
        Private _ButtonType As ButtonType

        Friend Sub New(ByVal ds As DataSet, ByVal buttontype As ButtonType)
            _DataSet = ds
            _ButtonType = buttontype
        End Sub

        Public ReadOnly Property ButtonClickType() As ButtonType
            Get
                Return _ButtonType
            End Get
        End Property

        Public Property DataSet() As DataSet
            Get
                Return _DataSet
            End Get

            Set(ByVal Value As DataSet)
                _DataSet = Value
            End Set
        End Property
    End Class
End Namespace
