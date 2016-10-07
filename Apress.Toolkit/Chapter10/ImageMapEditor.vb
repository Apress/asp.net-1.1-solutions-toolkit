Option Strict On

Imports System
Imports System.ComponentModel
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Design
Imports System.Drawing.Design
Imports System.Drawing
Imports System.Windows.Forms.Design
Imports System.Windows.Forms
Imports System.Collections
Imports Microsoft.VisualBasic

Namespace Apress.Toolkit

    Public Class ImageMapEditor
        Inherits UITypeEditor

        Public Overloads Overrides Function GetEditStyle( _
                    ByVal context As ITypeDescriptorContext) _
                                         As UITypeEditorEditStyle
            Return UITypeEditorEditStyle.Modal
        End Function

        Public Overloads Overrides Function EditValue( _
                    ByVal context As ITypeDescriptorContext, _
                    ByVal provider As IServiceProvider, _
                    ByVal value As Object) As Object
            Dim returnvalue As Object = value
            Dim srv As IWindowsFormsEditorService

            If Not provider Is Nothing Then
                srv = CType(provider.GetService( _
                               GetType(IWindowsFormsEditorService)), _
                            IWindowsFormsEditorService)
            End If

            If Not srv Is Nothing Then
                Dim ImageMapEd As New ImageMapEditorForm
                Dim stringRep As String = CType(value, String)
                Dim imgMapProps As New ImageMapProperties(stringRep)
                With ImageMapEd
                    .ImagePath = CType(context.Instance, _
                                    ImageMap).AbsoluteImagePath
                    .Regions = CType(imgMapProps.RectCoords.Clone(), ArrayList)
                    .StartPosition = FormStartPosition.CenterScreen
                    If srv.ShowDialog(ImageMapEd) _
                                    = DialogResult.OK Then
                        Dim newimgMapProps As _
                                    New ImageMapProperties(.Regions)
                        Return newimgMapProps.ToString
                    End If
                End With
                Return imgMapProps.ToString()
            End If
        End Function

    End Class

End Namespace
