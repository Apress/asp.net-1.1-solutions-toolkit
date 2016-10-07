Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Reflection
Imports System.Text
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace Apress.Toolkit.Controls
    Public Enum SpinnerAlign
        Left
        Right
    End Enum

    Public Enum ValueAlign
        Center
        Left
        Right
    End Enum

    <ToolboxData("<{0}:Spinner runat=""server"" width=""80px"" buttonsize=""XX-Small"" />")> _
    Public Class Spinner
        Inherits WebControl
        Implements INamingContainer

        Private Shared Scripts As String
        Private Shared StartupScriptFormat As String = _
    "<script language=""JavaScript"">t = document.getElementById(""{0}""); t.step = {1}; t.original = {2}; t.max = {3}; t.min = {4};</script>"

        Private _value As Integer = 0
        Private _inc As Integer = 1
        Private _size As FontUnit
        Private _max As Integer = 100
        Private _min As Integer = 0
        Private _align As SpinnerAlign = SpinnerAlign.Right
        Private _textalign As ValueAlign = ValueAlign.Left

        Shared Sub New()
            Dim asm As System.Reflection.Assembly = _
                System.Reflection.Assembly.GetExecutingAssembly()

            ' We check for null just in case the variable is called at
            ' design-time.
            If Not asm Is Nothing Then
                'Just for clarity define multiple variables.
                Dim resource As String = "SpinnerLib.js"
                Dim stm As Stream = asm.GetManifestResourceStream(resource)
                If stm Is Nothing Then Return

                Try
                    Dim reader As StreamReader = New StreamReader(stm)
                    Scripts = reader.ReadToEnd()
                    reader.Close()
                Finally
                    If Not stm Is Nothing Then stm.Close()
                End Try
            End If
        End Sub

        <Bindable(True), Category("Data"), DefaultValue(1)> _
        Property Increment() As Integer
            Get
                Return _inc
            End Get

            Set(ByVal Value As Integer)
                _inc = Value
            End Set
        End Property

        <Bindable(True), Category("Appearance"), DefaultValue(0), _
        Description("The initial value for the control.")> _
        Property Value() As Integer
            Get
                Return _value
            End Get

            Set(ByVal Value As Integer)
                _value = Value
                ChildControlsCreated = False
            End Set
        End Property

        <Bindable(True), Category("Data"), DefaultValue(100), _
        Description("The maximum value allowed for the control.")> _
        Property Maximum() As Integer
            Get
                Return _max
            End Get

            Set(ByVal Value As Integer)
                _max = Value
            End Set
        End Property

        <Bindable(True), Category("Data"), DefaultValue(0), _
        Description("The minimum value allowed for the control.")> _
        Property Minimum() As Integer
            Get
                Return _min
            End Get

            Set(ByVal Value As Integer)
                _min = Value
            End Set
        End Property

        <Bindable(True), Category("Layout"), DefaultValue(ValueAlign.Left), _
        Description("Alignment of the value inside the control's textbox.")> _
        Property TextAlign() As ValueAlign
            Get
                Return _textalign
            End Get

            Set(ByVal Value As ValueAlign)
                If Not [Enum].IsDefined(GetType(ValueAlign), Value) Then
                    Throw New ArgumentException
                End If

                ChildControlsCreated = False
                _textalign = Value
            End Set
        End Property

        <Bindable(True), Category("Layout"), _
        DefaultValue(SpinnerAlign.Right), _
        Description("Alignment of the Up/Down button controls.")> _
        Property ButtonAlign() As SpinnerAlign
            Get
                Return _align
            End Get

            Set(ByVal Value As SpinnerAlign)
                If Not [Enum].IsDefined(GetType(SpinnerAlign), Value) Then
                    Throw New ArgumentException
                End If

                ChildControlsCreated = False
                _align = Value
            End Set
        End Property

        <Bindable(True), Category("Appearance")> _
        Property ButtonSize() As FontUnit
            Get
                Return _size
            End Get

            Set(ByVal Value As FontUnit)
                ChildControlsCreated = False
                _size = Value
            End Set
        End Property

        Protected Overrides Sub CreateChildControls()
            Dim tb As New Table
            Dim tc As TableCell

            tb.CellPadding = 0
            tb.CellSpacing = 0

            'Create the table hierarchy first.
            tb.Rows.Add(New TableRow)
            tb.Rows.Add(New TableRow)
            tb.Rows(0).Cells.Add(New TableCell)
            tb.Rows(0).Cells.Add(New TableCell)
            tb.Rows(1).Cells.Add(New TableCell)

            'Clear the previous layout.
            MyBase.Controls.Clear()
            MyBase.Controls.Add(tb)

            ' Create the textbox and add it to the appropriate cell ASAP,
            ' so we get the ClientID property set, which is needed by the 
            ' code for the up/down "buttons" that change its value 
            ' on the client side.
            Dim txt As New TextBox
            txt.ID = "txtValue"
            txt.Attributes.Add("onkeypress", "return KeyPressed(event, this);")
            txt.Attributes.Add("onkeyup", "return KeyUp(this);")
            txt.Attributes.Add("onchange", "return SpinnerChanged(this);")
            txt.Attributes.Add("onpaste", "return SpinnerChanged(this);")

            'Apply the style set to the control to the textbox.
            txt.ApplyStyle(Me.ControlStyle)

            'Remove textbox borders, which will appear in the table cell instead
            txt.Style.Add("BORDER-TOP", "none")
            txt.Style.Add("BORDER-RIGHT", "none")
            txt.Style.Add("BORDER-LEFT", "none")
            txt.Style.Add("BORDER-BOTTOM", "none")
            txt.Width = New Unit("100%")
            txt.Text = Me.Value.ToString()

            If ButtonAlign = SpinnerAlign.Left Then
                tc = tb.Rows(0).Cells(1)
            Else
                tc = tb.Rows(0).Cells(0)
            End If

            tc.Style.Add("vertical-align", "middle")

            'Apply the style defined for the whole control to the cell
            tc.ApplyStyle(Me.ControlStyle)

            'Simulate textbox borders on the table cell
            tc.Style.Add("BORDER-TOP", "2px inset")
            tc.Style.Add("BORDER-RIGHT", "silver 1px solid")
            tc.Style.Add("BORDER-LEFT", "2px inset")
            tc.Style.Add("BORDER-BOTTOM", "silver 1px solid")
            tc.Controls.Add(txt)
            tc.RowSpan = 2
            tc.Width = New Unit("100%")


            'Detect if this is IE, to swap the border for the "buttons"
            'when the user clicks them (doesn't look good under NS).
            Dim isIE As Boolean = True

            If Context Is Nothing Then
                isIE = True
            ElseIf Not (context.Request Is Nothing) Then
                If context.Request.Browser.Browser <> "IE" Then
                    isIE = False
                End If
            End If

            If ButtonAlign = SpinnerAlign.Left Then
                tc = tb.Rows(0).Cells(0)
            Else
                tc = tb.Rows(0).Cells(1)
            End If

            'Up "button"
            tc.BackColor = System.Drawing.Color.Gainsboro
            tc.BorderStyle = CType(IIf(isIE, BorderStyle.Outset, BorderStyle.Solid), BorderStyle)
            tc.BorderWidth = New Unit("1px")
            tc.Style.Add("cursor", "pointer")
            tc.Font.Size = _size

            tc.Attributes.Add("onmouseup", "this.style.backgroundColor='Gainsboro';" + _
                IIf(isIE, "this.style.borderStyle='outset';", "").ToString())

            tc.Attributes.Add("onmousedown", "this.style.backgroundColor='WhiteSmoke';" + _
                IIf(isIE, "this.style.borderStyle='inset';", "").ToString())

            tc.Attributes.Add("onmouseout", "this.style.backgroundColor='Gainsboro';" + _
                IIf(isIE, "this.style.borderStyle='outset';", "").ToString())

            tc.Attributes.Add("onclick", "Increment('" + txt.ClientID + "');")

            tc.Text = ChrW(9650) 'represents the "▲" character.

            'Down "button"
            tc = tb.Rows(1).Cells(0)
            tc.BackColor = System.Drawing.Color.Gainsboro
            tc.BorderStyle = CType(IIf(isIE, BorderStyle.Outset, _
                  BorderStyle.Solid), BorderStyle)
            tc.BorderWidth = New Unit("1px")
            tc.Style.Add("cursor", "pointer")
            tc.Font.Size = _size

            tc.Attributes.Add("onmouseup", _
                "this.style.backgroundColor='Gainsboro';" + _
                IIf(isIE, "this.style.borderStyle='outset';", "").ToString())

            tc.Attributes.Add("onmousedown", _
                "this.style.backgroundColor='WhiteSmoke';" + _
                IIf(isIE, "this.style.borderStyle='inset';", "").ToString())

            tc.Attributes.Add("onmouseout", _
                "this.style.backgroundColor='Gainsboro';" + _
                IIf(isIE, "this.style.borderStyle='outset';", "").ToString())

            tc.Attributes.Add("onclick", "Decrement('" + txt.ClientID + "');")

            tc.Text = ChrW(9660) 'represents the "▼" character.
        End Sub

        Protected Overrides Sub AddAttributesToRender( _
            ByVal writer As System.Web.UI.HtmlTextWriter)

            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, _
                Me.Width.ToString())
        End Sub


        Protected Overrides Sub Render( _
            ByVal writer As System.Web.UI.HtmlTextWriter)
            EnsureChildControls()
            MyBase.Render(writer)
        End Sub
        Public Overrides ReadOnly Property Controls() As ControlCollection
            Get
                Me.EnsureChildControls()
                Return Me.Controls
            End Get
        End Property


        Protected Overrides Sub OnLoad(ByVal e As EventArgs)
            EnsureChildControls()


            'Register library.
            If Not Page.IsClientScriptBlockRegistered(Me.GetType().FullName) _
                Then
                Page.RegisterClientScriptBlock(Me.GetType().FullName, Scripts)
            End If

            Page.RegisterStartupScript(Me.ClientID, _
                String.Format( _
                    StartupScriptFormat, Me.FindControl("txtValue").ClientID, _
                    Me.Increment, Me.Value, Me.Maximum, Me.Minimum))
            MyBase.OnLoad(e)
        End Sub

        Public Overrides Property BackColor() As System.Drawing.Color
            Get
                Return MyBase.BackColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                ChildControlsCreated = False
                MyBase.BackColor = Value
            End Set
        End Property
    End Class
End Namespace