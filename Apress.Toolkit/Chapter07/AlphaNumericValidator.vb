Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Text.RegularExpressions

Namespace Apress.Toolkit.Validation

    Public Class AlphaNumericValidator
        Inherits BaseValidationProvider

        Public Overrides Function Validate(ByVal validator As ValidatorControl, ByVal value As String) As Boolean
            Return Regex.IsMatch(value, "^[\w,\s]*$")
        End Function

        Public Overrides Function HasScriptForChange() As Boolean
            Return True
        End Function

        Public Overrides Function HasScriptForKeyPress() As Boolean
            Return True
        End Function

        Public Overrides Function GetScriptForChange(ByVal validator As ValidatorControl) As String
            '---- Expected output ----
            '	if (!/^[\w,\s]*$/.test(sender.Text)) e.Cancel = true;
            '	if (e.Cancel) this.IsValid = false;	
            '	else this.IsValid = true;
            '-------------------------
            Dim sb As New StringBuilder
            sb.Append(ControlChars.Tab)
            sb.Append("if (!/^[\w,\s]*$/.test(sender.Text)) e.Cancel = true;")
            sb.Append(ControlChars.NewLine).Append(ControlChars.Tab)
            sb.Append("if (e.Cancel) this.IsValid = false;").Append(ControlChars.NewLine)
            sb.Append(ControlChars.Tab)
            sb.Append("else this.IsValid = true;")
            Return sb.ToString()
        End Function

        Public Overrides Function GetScriptForKeyPress(ByVal validator As ValidatorControl) As String
            '---- Expected output ----
            '	if (!Char.IsLetterOrDigit(e.KeyChar) && !e.IsControl) e.Handled = true;
            '	if (e.IsControl) return;	
            '	if (e.Handled) this.IsValid = false;	
            '	else this.IsValid = true;
            '-------------------------
            Dim sb As New StringBuilder
            sb.Append(ControlChars.Tab)
            sb.Append("if (!Char.IsLetterOrDigit(e.KeyChar) && !e.IsControl) e.Handled = true;")
            sb.Append(ControlChars.NewLine).Append(ControlChars.Tab)
            'Don't change valid state if it's a control key.
            sb.Append("if (e.IsControl) return;")
            sb.Append(ControlChars.NewLine).Append(ControlChars.Tab)
            'Show the message even if the key isn't output, to provide a hint.
            sb.Append("if (e.Handled) this.IsValid = false;").Append(ControlChars.NewLine)
            sb.Append(ControlChars.Tab)
            sb.Append("else this.IsValid = true;")
            Return sb.ToString()
        End Function
    End Class

End Namespace
