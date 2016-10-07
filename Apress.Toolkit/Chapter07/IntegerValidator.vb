Imports Microsoft.VisualBasic
Imports System
Imports System.Text

Namespace Apress.Toolkit.Validation

    Public Class IntegerValidator
        Inherits BaseValidationProvider

        Public Overrides Function Validate(ByVal validator As ValidatorControl, ByVal value As String) As Boolean
            Try
                Convert.ToInt32(value)
                Return True
            Catch
                Return False
            End Try
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
            '	if (parseInt(sender.Text) != sender.Text) e.Cancel = true;
            '	if (e.Cancel) this.IsValid = false;	
            '	else this.IsValid = true;
            '-------------------------
            Dim sb As New StringBuilder
            sb.Append(ControlChars.Tab)
            sb.Append("if (parseInt(sender.Text) != sender.Text) e.Cancel = true;")
            sb.Append(ControlChars.NewLine).Append(ControlChars.Tab)
            sb.Append("if (e.Cancel) this.IsValid = false;").Append(ControlChars.NewLine)
            sb.Append(ControlChars.Tab)
            sb.Append("else this.IsValid = true;")
            Return sb.ToString()
        End Function

        Public Overrides Function GetScriptForKeyPress( _
            ByVal validator As ValidatorControl) As String
            '---- Expected output ----
            'if (!Char.IsDigit(e.KeyChar) && !e.IsControl) e.Handled = true;
            'if (e.IsControl) return;	
            'if (e.Handled) this.IsValid = false;	
            'else this.IsValid = true;
            '-------------------------
            Dim sb As New StringBuilder
            sb.Append(ControlChars.Tab)
            sb.Append("if (!Char.IsDigit(e.KeyChar) && !e.IsControl) e.Handled = true;")
            sb.Append(ControlChars.NewLine).Append(ControlChars.Tab)
            'Don't change valid state if it's a control key.
            sb.Append("if (e.IsControl) return;")
            sb.Append(ControlChars.NewLine).Append(ControlChars.Tab)
            'Set the IsValid flag event we are canceling the event. 
            'This way the user gets a hint of what's wrong with the key.
            sb.Append("if (e.Handled) this.IsValid = false;")
            sb.Append(ControlChars.NewLine)
            sb.Append(ControlChars.Tab)
            sb.Append("else this.IsValid = true;")
            Return sb.ToString()
        End Function
    End Class

End Namespace