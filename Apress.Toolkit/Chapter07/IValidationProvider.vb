Namespace Apress.Toolkit.Validation

    Public Interface IValidationProvider
        'Function to be called on the server side
        Function Validate(ByVal validator As ValidatorControl, ByVal value As String) As Boolean

        'Tells the validator control whether we provide client scripts
        Function HasScriptForChange() As Boolean
        Function HasScriptForKeyPress() As Boolean

        'Method to output client side script for validating the whole 
        'value entered on the textbox. The client side javascript 
        'function has the same signature as the CancelEventHandler delegate. 
        'Look at .NET documentation. The function signature is generated
        'automatically by the ValidatorControl class.
        Function GetScriptForChange(ByVal validator As ValidatorControl) As String

        'Method to output client side script for validating each 
        'keystroke entered in the textbox. The client side javascript 
        'function has the same signature as the KeyPressEventHandler delegate. 
        'Look at .NET documentation. The function signature is generated
        'automatically by the ValidatorControl class. We offer KeyCode additionally.
        Function GetScriptForKeyPress(ByVal validator As ValidatorControl) As String
    End Interface

End Namespace