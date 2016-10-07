Namespace Apress.Toolkit.Validation

    Public MustInherit Class BaseValidationProvider
        Implements IValidationProvider

        Public Overridable Function Validate(ByVal validator As ValidatorControl, _
        ByVal value As String) As Boolean Implements IValidationProvider.Validate
            Return True
        End Function

        Public Overridable Function HasScriptForChange() As Boolean _
        Implements IValidationProvider.HasScriptForChange
            Return False
        End Function

        Public Overridable Function HasScriptForKeyPress() As Boolean _
        Implements IValidationProvider.HasScriptForKeyPress
            Return False
        End Function

        Public Overridable Function GetScriptForChange(ByVal validator As ValidatorControl) As String _
        Implements IValidationProvider.GetScriptForChange
            Return String.Empty
        End Function

        Public Overridable Function GetScriptForKeyPress(ByVal validator As ValidatorControl) As String _
        Implements IValidationProvider.GetScriptForKeyPress
            Return String.Empty
        End Function

    End Class

End Namespace