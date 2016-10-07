Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Text

Namespace Apress.Toolkit.Validation

    <DefaultProperty("ErrorMessage"), ToolboxData("<{0}:ValidatorControl runat=server></{0}:ValidatorControl>")> _
    Public Class ValidatorControl
        Inherits Label
        Implements IValidator

#Region "Script handling"
        'Contains global types and functions to use by client scripts.
        Shared ValidatorLibScript As String


        'Keys to use for registering scripts.
        'Using the type's FullName ensures uniqueness.
        Shared GlobalKey As String = GetType(ValidatorControl).FullName
        Shared CultureKey As String = GlobalKey + "Culture"

        'Keeps a list of IValidationProvider instances already loaded.
        Shared LoadedValidators As New Hashtable

        'Shared constructor to extract the javascript library from the assembly.
        Shared Sub New()
            Dim asm As [Assembly] = [Assembly].GetExecutingAssembly()

            ' We check for null just in case this is called at design-time.
            If Not asm Is Nothing Then
                Dim resource As String = "ValidatorLib.js"
                Dim stm As Stream = asm.GetManifestResourceStream(resource)

                If Not stm Is Nothing Then
                    Try
                        Dim reader As StreamReader = New StreamReader(stm)
                        ValidatorLibScript = reader.ReadToEnd()
                        reader.Close()
                    Catch e As Exception
                        Throw New ApplicationException( _
                         String.Format("Couldn't extract {0} for {1}.", resource, GlobalKey, e))
                    Finally
                        CType(stm, IDisposable).Dispose()
                    End Try
                End If
            End If
        End Sub

        Private Sub InitializeScripts()
            If Not RenderScripts() Then Return

            Dim sb As StringBuilder

            'Culture information goes first, as it can be used anywhere in the lib.
            'Register common startup script block (i.e. culture information).
            If Not Page.IsClientScriptBlockRegistered(CultureKey) Then
                sb = New StringBuilder
                sb.Append("<script language=""javascript"">")
                sb.Append(ControlChars.NewLine)
                CreateStartupCultureInfo(sb)
                sb.Append("</script>")
                Page.RegisterClientScriptBlock(CultureKey, sb.ToString())
            End If

            'Register type lib script block (i.e. client .NET types and global event handlers).
            If Not Page.IsClientScriptBlockRegistered(GlobalKey) Then
                Page.RegisterClientScriptBlock(GlobalKey, ValidatorLibScript)
            End If

            Dim val As IValidationProvider = GetValidator()
            Dim key As String = Me.TypeName.Replace(".", "")

            'Output common client code for the IValidationProvider type.
            If Not Page.IsStartupScriptRegistered(key) Then
                sb = New StringBuilder
                sb.Append("<script language=""javascript"">")
                sb.Append(ControlChars.NewLine)

                'Emit the client code as specified by the interface implementation.
                If val.HasScriptForChange() Then
                    'function ChangeXXX(sender, e) { [Validator emited code] }
                    sb.Append("function Change").Append(key)
                    sb.Append("(sender, e)").Append(ControlChars.NewLine)
                    sb.Append("{").Append(ControlChars.NewLine)
                    sb.Append(val.GetScriptForChange(Me))
                    sb.Append(ControlChars.NewLine).Append("}")
                    sb.Append(ControlChars.NewLine)

                    'Extend the client javascript object with the new method. 
                    'ApressToolkitValidator.prototype.Change[key] = Change[key];
                    sb.Append("WroxToolkitValidator.prototype.Change")
                    sb.Append(key).Append(" = Change").Append(key)
                    sb.Append(";").Append(ControlChars.NewLine)
                    sb.Append(ControlChars.NewLine)
                End If

                'Emit the client code as specified by the interface implementation.
                If val.HasScriptForKeyPress() Then
                    'function KeyPressXXX(sender, e) { [Validator emited code] }
                    sb.Append("function KeyPress").Append(key)
                    sb.Append("(sender, e)").Append(ControlChars.NewLine)
                    sb.Append("{").Append(ControlChars.NewLine)
                    sb.Append(val.GetScriptForKeyPress(Me))
                    sb.Append(ControlChars.NewLine).Append("}")
                    sb.Append(ControlChars.NewLine)

                    'Extend the client javascript object with the new method. 
                    'ApressToolkitValidator.prototype.KeyPress[key] = KeyPress[key];
                    sb.Append("WroxToolkitValidator.prototype.KeyPress")
                    sb.Append(key).Append(" = KeyPress").Append(key)
                    sb.Append(";").Append(ControlChars.NewLine)
                    sb.Append(ControlChars.NewLine)
                End If

                sb.Append("</script>")

                'Only register if there is client code and if we are able to attach
                'the client side event handlers (IAttributeAccessor).
                If (val.HasScriptForChange() Or val.HasScriptForKeyPress()) And _
                 TypeOf GetControlToValidate() Is IAttributeAccessor Then
                    Page.RegisterStartupScript(key, sb.ToString())
                Else
                    'Register as empty to avoid checking again for this page.
                    Page.RegisterStartupScript(key, String.Empty)
                End If
            End If

            'Output initialization code for the client side validation object.
            sb = New StringBuilder
            sb.Append("<script language=""javascript"">")
            sb.Append(ControlChars.NewLine)
            sb.Append("AddValidator(")
            sb.Append("""").Append(Me.ControlToValidate).Append(""",")
            sb.Append("""").Append(Me.ErrorMessage).Append(""",")
            sb.Append("""").Append(Me.Display.ToString()).Append(""",")
            sb.Append(Me.IsValid.ToString().ToLower()).Append(",")
            sb.Append(val.HasScriptForKeyPress().ToString().ToLower()).Append(",")
            sb.Append(val.HasScriptForChange().ToString().ToLower()).Append(",")
            sb.Append("""").Append(Me.ClientID).Append(""",")
            sb.Append("""").Append(key).Append(""");")
            sb.Append(ControlChars.NewLine).Append("</script>")
            Page.RegisterStartupScript(Me.ClientID, sb.ToString())

        End Sub

        'Client script is emited for browsers with W3CDom/EcmaScript support.
        Private Function RenderScripts() As Boolean
            Return (Context.Request.Browser.EcmaScriptVersion.Major >= 1 And _
             Context.Request.Browser.EcmaScriptVersion.Minor >= 2 And _
             Context.Request.Browser.W3CDomVersion.Major >= 1)
        End Function

        Private Sub CreateStartupCultureInfo(ByRef builder As StringBuilder)
            Dim n As String = ControlChars.NewLine

            builder.Append(n).Append("var CultureInfo;").Append(n)
            DumpProperties(builder, String.Empty, "CultureInfo", Culture)
            DumpProperties(builder, "CultureInfo", "DateTimeFormat", Culture.DateTimeFormat)
            DumpProperties(builder, "CultureInfo", "NumberFormat", Culture.NumberFormat)
            DumpProperties(builder, "CultureInfo", "TextInfo", Culture.TextInfo)
        End Sub

        'Dumps the whole object's properties as a javascript object's properties.
        'If the varName is empty, creates a new instance variable.
        Private Sub DumpProperties(ByRef builder As StringBuilder, ByRef varName As String, ByRef propName As String, ByVal value As Object)
            Dim props As PropertyInfo()
            Dim prop As PropertyInfo
            Dim n As String = ControlChars.NewLine

            builder.Append(n).Append("function ").Append(propName).Append("() {").Append(n)

            props = value.GetType().GetProperties(BindingFlags.Instance Or BindingFlags.Public)

            For Each prop In props
                If prop.PropertyType.IsValueType Then
                    If prop.PropertyType Is GetType(Char) Or prop.PropertyType.IsEnum Then
                        builder.Append(ControlChars.Tab).Append("this.").Append(prop.Name).Append(" = """)
                        builder.Append(prop.GetValue(value, Nothing)).Append(""";").Append(n)
                    ElseIf prop.PropertyType Is GetType(Boolean) Then
                        builder.Append(ControlChars.Tab).Append("this.").Append(prop.Name).Append(" = ")
                        builder.Append(prop.GetValue(value, Nothing).ToString().ToLower()).Append(";").Append(n)
                    Else
                        builder.Append(ControlChars.Tab).Append("this.").Append(prop.Name).Append(" = ")
                        builder.Append(prop.GetValue(value, Nothing)).Append(";").Append(n)
                    End If
                ElseIf prop.PropertyType Is GetType(String) Then
                    builder.Append(ControlChars.Tab).Append("this.").Append(prop.Name).Append(" = """)
                    builder.Append(prop.GetValue(value, Nothing)).Append(""";").Append(n)
                ElseIf prop.PropertyType.IsArray Then
                    'Can we output array values?
                End If
            Next

            builder.Append("}").Append(n)
            If varName <> String.Empty Then builder.Append(varName).Append(".")
            builder.Append(propName)
            builder.Append(" = new ").Append(propName).Append("();").Append(n)
        End Sub

#End Region

#Region "Properties"
        Dim _isvalid As Boolean = True
        Dim _assembly As String = String.Empty
        Dim _type As String = String.Empty
        Dim _control As String
        Dim _display As ValidatorDisplay = ValidatorDisplay.Static
        Dim _cultureaware As Boolean = True
        Dim _info As CultureInfo

        <Description("How the validator is displayed."), _
        Category("Appearance"), DefaultValue(ValidatorDisplay.Static)> _
        Public Property Display() As ValidatorDisplay
            Get
                Return _display
            End Get
            Set(ByVal Value As ValidatorDisplay)
                _display = Value
            End Set
        End Property

        <Description("The message to display when control's value is invalid."), _
        Category("Appearance"), Browsable(False)> _
        Public Property ErrorMessage() As String Implements IValidator.ErrorMessage
            Get
                Return MyBase.Text
            End Get
            Set(ByVal Value As String)
                MyBase.Text = Value
            End Set
        End Property

        <Description("Make the control aware of client culture when validating values."), _
        Category("Appearance")> _
        Public Property CultureAware() As Boolean
            Get
                Return _cultureaware
            End Get
            Set(ByVal Value As Boolean)
                _cultureaware = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Property Culture() As CultureInfo
            Get
                Return _info
            End Get
            Set(ByVal Value As CultureInfo)
                _info = Value
            End Set
        End Property

        <Browsable(False), _
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
        Public Property IsValid() As Boolean Implements IValidator.IsValid
            Get
                Return _isvalid
            End Get
            Set(ByVal Value As Boolean)
                _isvalid = Value
            End Set
        End Property

        <Description("The name of the assembly with the classes " + _
         "implementing the validation code. Use the name as it appears " + _
         "in the References folder."), _
         Category("Validator"), DefaultValue("")> _
        Property AssemblyName() As String
            Get
                Return _assembly
            End Get

            Set(ByVal Value As String)
                _assembly = Value
            End Set
        End Property

        <TypeConverter(GetType(ValidatorTypeConverter)), _
        Description("The class implementing the validation code."), _
        Category("Validator"), DefaultValue("")> _
        Property TypeName() As String
            Get
                Return _type
            End Get

            Set(ByVal Value As String)
                _type = Value
            End Set
        End Property

        <DefaultValue(""), _
        Description("The ID of the control to validate."), _
        TypeConverter(GetType(ValidatedControlConverter)), _
        Category("Behavior")> _
        Public Property ControlToValidate() As String
            Get
                Return _control
            End Get
            Set(ByVal Value As String)
                _control = Value
            End Set
        End Property

        <Browsable(False), _
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
        Public Overrides Property Enabled() As Boolean
            Get
                Return True
            End Get
            Set(ByVal Value As Boolean)
                'Do nothing. Our validator can't be disabled. Remove it from the 
                'page from the page if you don't want it (as it should always be)!
            End Set
        End Property
#End Region

        Public Sub New()
            Me.ForeColor = System.Drawing.Color.Red
        End Sub

        'Validates the control with the supplied validator class.
        Public Sub Validate() Implements IValidator.Validate
            Dim val As IValidationProvider

            Me.IsValid = True
            val = GetValidator()

            Try
                Me.IsValid = val.Validate(Me, GetControlValue())
            Catch e As Exception
                'Validators shouldn't throw exceptions!
                Page.Trace.Warn("Apress.Toolkit.Validation", _
                 String.Format("Validator {0}.{1} threw an exception.", _
                 Me.AssemblyName, Me.TypeName), e)
                Me.IsValid = False
            End Try
        End Sub

        'Trap our target control's prerender to add the event handlers.
        Private Sub OnTargetPreRender(ByVal sender As Object, ByVal e As EventArgs)
            Dim val As IValidationProvider = GetValidator()
            Dim ctl As IAttributeAccessor

            If (TypeOf sender Is IAttributeAccessor) Then
                ctl = CType(sender, IAttributeAccessor)

                If val.HasScriptForChange() Then
                    ctl.SetAttribute("onchange", "return ValidateOnChange(event);")
                    'Add to onblur just to clear error messages shown in keypress.
                    'ctl.SetAttribute("onblur", "return ValidateOnChange(event);")
                End If

                If val.HasScriptForKeyPress() Then
                    ctl.SetAttribute("onkeypress", "return ValidateOnKeyPress(event);")
                End If
            End If
        End Sub


#Region "Helper methods"

        'Retrieves an instance of the selected validator object.
        Private Function GetValidator() As IValidationProvider
            If Me.AssemblyName = String.Empty Then
                Throw New ArgumentException("AssemblyName property not set.")
            End If

            Dim val As IValidationProvider

            Console.Out.WriteLine("heyyy")
            Console.Out.WriteLine(Me.TypeName)
            'Retrieve a preloaded instance or instantiate the validation class.
            If Not LoadedValidators.ContainsKey(Me.TypeName) Then
                val = CType(Activator.CreateInstance(Me.AssemblyName, Me.TypeName).Unwrap(), IValidationProvider)
                LoadedValidators.Add(Me.TypeName, val)
            Else
                val = CType(LoadedValidators(Me.TypeName), IValidationProvider)
            End If

            Return val
        End Function

        'Retrieves the instance of the control to validate.
        Private Function GetControlToValidate() As Control
            'Verify control to validate
            If Me.ControlToValidate = String.Empty Then
                Throw New ArgumentException( _
                 String.Format("{0}.ControlToValidate not set.", Me.ID))
            End If

            Dim ctl As Control = Page.FindControl(Me.ControlToValidate)
            If ctl Is Nothing Then
                Throw New ArgumentException( _
                 String.Format("Control to validate {0} wasn't found.", Me.ControlToValidate))
            End If

            Return ctl
        End Function

        'Retrieves the current value of control to validate.
        Private Function GetControlValue() As String
            Dim ctl As Control = GetControlToValidate()

            'Retrieve the property that serves as the validation source value.
            Dim val As ValidationPropertyAttribute
            val = CType(TypeDescriptor.GetAttributes(ctl).Item(GetType(ValidationPropertyAttribute)), ValidationPropertyAttribute)

            If val Is Nothing Then
                Throw New ArgumentException( _
                 String.Format("Control to validate {0} isn't a valid source for validation.", Me.ControlToValidate))
            End If

            'Finally return the property value.
            Dim prop As PropertyDescriptor
            prop = TypeDescriptor.GetProperties(ctl).Item(val.Name)
            Return CType(prop.GetValue(ctl), String)
        End Function

#End Region

#Region "Control method overrides"

        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
            MyBase.OnInit(e)
            'Add this validator to the collection on the page.
            Page.Validators.Add(Me)
            'Set the appropriate culture.
            If Not CultureAware Then
                Culture = CultureInfo.CurrentCulture
            Else
                'Pick the first language chosen by the client.
                Culture = CultureInfo.CreateSpecificCulture(context.Request.UserLanguages(0))
            End If
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            InitializeScripts()
            MyBase.OnLoad(e)
            'Subscribe to add the attributes to the target control.
            AddHandler GetControlToValidate().PreRender, New EventHandler(AddressOf OnTargetPreRender)
        End Sub

        Protected Overrides Sub AddAttributesToRender(ByVal writer As System.Web.UI.HtmlTextWriter)
            'Only add the visibility attributes if we're not at Design-time
            If CType(Me, IComponent).Site Is Nothing Then
                Select Case Me.Display
                    Case ValidatorDisplay.None
                        Me.Style.Add("display", "none")
                    Case ValidatorDisplay.Dynamic
                        If Not IsValid Then
                            Me.Style.Add("display", "inline")
                        Else
                            Me.Style.Add("display", "none")
                        End If
                    Case ValidatorDisplay.Static
                        If Not IsValid Then
                            Me.Style.Add("visibility", "visible")
                        Else
                            Me.Style.Add("visibility", "hidden")
                        End If
                End Select
            End If

            MyBase.AddAttributesToRender(writer)
        End Sub

        Protected Overrides Sub OnUnload(ByVal e As System.EventArgs)
            'Remove ourselves from the collection.
            Page.Validators.Remove(Me)
            MyBase.OnUnload(e)
        End Sub

#End Region

    End Class

End Namespace