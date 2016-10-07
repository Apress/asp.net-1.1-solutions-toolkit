Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Text.RegularExpressions

Namespace Apress.Toolkit.Validation

    Public Class PositiveNumericValidator
        Inherits BaseValidationProvider

        Private Function BuildPattern(ByRef val As ValidatorControl) As String
            'Builds the following expression, with culture-aware symbols:
            '"^\+?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(\.[0-9][0-9])?$"
            Dim sb As New StringBuilder
            Dim i As Integer

            With val.Culture.NumberFormat
                sb.Append("^\").Append(.PositiveSign)
                sb.Append("?([0-9]{1,3}\").Append(.NumberGroupSeparator)
                sb.Append("([0-9]{3}\").Append(.NumberGroupSeparator)
                sb.Append(")*[0-9]{3}|[0-9]+)(\")
                If (.CurrencyDecimalDigits > 0) Then _
                    sb.Append(.NumberDecimalSeparator)
                'Append the number of digits.
                For i = 1 To .NumberDecimalDigits
                    sb.Append("[0-9]")
                Next
                sb.Append(")?$")
            End With

            Return sb.ToString()
        End Function

        Public Overrides Function Validate(ByVal validator As ValidatorControl, ByVal value As String) As Boolean
            Return Regex.IsMatch(value, BuildPattern(validator))
        End Function

        Public Overrides Function HasScriptForChange() As Boolean
            Return True
        End Function

        Public Overrides Function HasScriptForKeyPress() As Boolean
            Return True
        End Function

        Public Overrides Function GetScriptForChange(ByVal validator As ValidatorControl) As String
            '---- Expected output ----
            '	if (!/[BuildPattern]/.test(sender.Text)) e.Cancel = true;
            '	if (e.Cancel) this.IsValid = false;	
            '	else this.IsValid = true;
            '-------------------------
            Dim sb As New StringBuilder
            sb.Append(ControlChars.Tab)
            sb.Append("if (!/").Append(BuildPattern(validator)).Append("/.test(sender.Text)) e.Cancel = true;")
            sb.Append(ControlChars.NewLine).Append(ControlChars.Tab)
            sb.Append("if (e.Cancel) this.IsValid = false;").Append(ControlChars.NewLine)
            sb.Append(ControlChars.Tab)
            sb.Append("else this.IsValid = true;")
            Return sb.ToString()
        End Function

        Public Overrides Function GetScriptForKeyPress(ByVal validator As ValidatorControl) As String
            '---- Expected output ----
            '	if ((!Char.IsNumber(e.KeyChar) || 
            '      (e.KeyChar == CultureInfo.NumberFormat.NegativeSign) || 
            '      (e.KeyChar == CultureInfo.NumberFormat.PositiveSign && sender.Text.IndexOf(CultureInfo.NumberFormat.PositiveSign) != -1) || 
            '      (e.KeyChar == CultureInfo.NumberFormat.NumberDecimalSeparator && sender.Text.IndexOf(CultureInfo.NumberFormat.NumberDecimalSeparator) != -1)) && 
            '     !e.IsControl) e.Handled = true;
            '	if (e.Handled) this.IsValid = false;	
            '	else this.IsValid = true;
            '-------------------------
            Dim sb As New StringBuilder
            sb.Append(ControlChars.Tab)
            sb.Append("if ((!Char.IsNumber(e.KeyChar) || ")
            sb.Append("(e.KeyChar == CultureInfo.NumberFormat.NegativeSign) || ")
            sb.Append("(e.KeyChar == CultureInfo.NumberFormat.PositiveSign && sender.Text.IndexOf(CultureInfo.NumberFormat.PositiveSign) != -1) || ")
            sb.Append("(e.KeyChar == CultureInfo.NumberFormat.NumberDecimalSeparator && sender.Text.IndexOf(CultureInfo.NumberFormat.NumberDecimalSeparator) != -1)) && ")
            sb.Append("!e.IsControl) e.Handled = true;")
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