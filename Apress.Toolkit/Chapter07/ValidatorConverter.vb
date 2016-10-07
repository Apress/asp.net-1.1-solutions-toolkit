'********************************************
' This class provides design-time advantages
' to developers using ASP.NET Web Matrix or
' VS .NET, loading the list of referenced
' types implementing the IValidationProvider 
' interface.
'********************************************

Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Reflection


Namespace Apress.Toolkit.Validation

    Public Class ValidatorTypeConverter
        Inherits TypeConverter

        'Tell the IDE we want to pass the valid values.
        Public Overloads Overrides Function GetStandardValuesSupported( _
         ByVal context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

        'Tell the IDE that only values from the list are valid.
        Public Overloads Overrides Function GetStandardValuesExclusive( _
         ByVal context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

        'Loads the list of types implementing our validation interface
        Public Overloads Overrides Function GetStandardValues( _
         ByVal context As ITypeDescriptorContext) As StandardValuesCollection

            Dim val As ValidatorControl = CType(context.Instance, ValidatorControl)

            If val.AssemblyName = String.Empty Then Return Nothing

            'Get a reference to the service that handles types references.
            Dim svc As ITypeResolutionService = _
             CType(context.GetService(GetType(ITypeResolutionService)), ITypeResolutionService)
            If svc Is Nothing Then Return Nothing

            Dim name As New AssemblyName
            name.Name = val.AssemblyName
            Dim asm As [Assembly] = svc.GetAssembly(name)
            If asm Is Nothing Then Return Nothing

            'Finally load the types.
            Dim list As System.Collections.ArrayList = New System.Collections.ArrayList
            Dim types() As Type = asm.GetTypes()
            Dim t As Type

            For Each t In types
                'Ensure the type implements our interface, and add it to the list.
                If Not t.IsAbstract And t.FindInterfaces([Module].FilterTypeName, "IValidationProvider").Length <> 0 Then
                    list.Add(t.FullName)
                End If
            Next

            Return New StandardValuesCollection(list)
        End Function

    End Class

End Namespace