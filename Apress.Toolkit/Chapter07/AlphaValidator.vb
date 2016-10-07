Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Text.RegularExpressions

Namespace Apress.Toolkit.Validation

    Public Class AlphaValidator
        Inherits BaseValidationProvider

        Public Overrides Function Validate( _
            ByVal validator As ValidatorControl, _
            ByVal value As String) As Boolean
            Return Regex.IsMatch(value, "^[a-zA-Z_,\s]*$")
        End Function

        Public Overrides Function HasScriptForChange() As Boolean
            Return True
        End Function

        Public Overrides Function HasScriptForKeyPress() As Boolean
            Return True
        End Function

        Public Overrides Function GetScriptForChange( _
            ByVal validator As ValidatorControl) As String
            '---- Expected output ----
            'if (!/^[a-zA-Z_,\s]*$/.test(sender.Text)) e.Cancel = true;
            'if (e.Cancel) this.IsValid = false;	
            'else this.IsValid = true;
            '-------------------------
            Dim sb As New StringBuilder
            sb.Append(ControlChars.Tab)
            sb.Append("if (!/^[a-zA-Z_,\s]*$/.test(sender.Text)) e.Cancel = true;")
            sb.Append(ControlChars.NewLine).Append(ControlChars.Tab)
            sb.Append("if (e.Cancel) this.IsValid = false;")
            sb.Append(ControlChars.NewLine)
            sb.Append(ControlChars.Tab)
            sb.Append("else this.IsValid = true;")
            Return sb.ToString()
        End Function

        Public Overrides Function GetScriptForKeyPress( _
            ByVal validator As ValidatorControl) As String
            '---- Expected output ----
            'if (!Char.IsLetter(e.KeyChar) && !e.IsControl) e.Handled = true;
            'if (e.Handled) this.IsValid = false;	
            'else this.IsValid = true;
            '-------------------------
            Dim sb As New StringBuilder
            sb.Append(ControlChars.Tab)
            sb.Append("if (!Char.IsLetter(e.KeyChar) && !e.IsControl) e.Handled = true;")
            'Don't change valid state if it's a control key.
            sb.Append("if (e.IsControl) return;")
            sb.Append(ControlChars.NewLine).Append(ControlChars.Tab)
            'Show the message even if the key isn't output, to provide a hint.
            sb.Append("if (e.Handled) this.IsValid = false;")
            sb.Append(ControlChars.NewLine)
            sb.Append(ControlChars.Tab)
            sb.Append("else this.IsValid = true;")
            Return sb.ToString()
        End Function
    End Class

End Namespace