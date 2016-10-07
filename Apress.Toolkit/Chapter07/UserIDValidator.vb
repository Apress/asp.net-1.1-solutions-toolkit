'********************************************
' This class validates a user id with GUID
' format against a fictious database. It 
' shows the extensibility available in our 
' validation controls.
'********************************************

Imports Microsoft.VisualBasic
Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Text.RegularExpressions

Namespace Apress.Toolkit.Validation

    Public Class UserIDValidator
        Inherits BaseValidationProvider

        Public Overrides Function Validate(ByVal validator As ValidatorControl, ByVal value As String) As Boolean
            'Assumes the connection string is placed in the Web.config file.
            Dim cn As SqlConnection = New SqlConnection(ConfigurationSettings.AppSettings("connection"))
            Dim cmd As SqlCommand = New SqlCommand( _
               "SELECT UserID FROM [User] WHERE UserId='" + value + "'", cn)
            cn.Open()
            'If Read() returns true it means a row was found.
            Return cmd.ExecuteReader(CommandBehavior.CloseConnection).Read()
        End Function

        Public Overrides Function HasScriptForChange() As Boolean
            Return True
        End Function

        Public Overrides Function HasScriptForKeyPress() As Boolean
            Return True
        End Function

        Public Overrides Function GetScriptForChange(ByVal validator As ValidatorControl) As String
            '---- Expected output ----
            '	if (!/^[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12}$/.test(sender.Text)) e.Cancel = true;
            '	if (e.Cancel) this.IsValid = false;	
            '	else this.IsValid = true;
            '-------------------------
            Dim sb As New StringBuilder
            sb.Append(ControlChars.Tab)
            sb.Append("if (!/^[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12}$/.test(sender.Text)) e.Cancel = true;")
            sb.Append(ControlChars.NewLine).Append(ControlChars.Tab)
            sb.Append("if (e.Cancel) this.IsValid = false;")
            sb.Append(ControlChars.NewLine)
            sb.Append(ControlChars.Tab)
            sb.Append("else this.IsValid = true;")
            Return sb.ToString()
        End Function

        Public Overrides Function GetScriptForKeyPress(ByVal validator As ValidatorControl) As String
            '---- Expected output ----
            '	if (!(Char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == "-") && !e.IsControl) e.Handled = true;
            '	if (e.IsControl) return;	
            '	if (e.Handled) this.IsValid = false;	
            '	else this.IsValid = true;
            '-------------------------
            Dim sb As New StringBuilder
            sb.Append(ControlChars.Tab)
            sb.Append("if (!(Char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == "" - "") && !e.IsControl) e.Handled = true;")
            sb.Append(ControlChars.NewLine).Append(ControlChars.Tab)
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