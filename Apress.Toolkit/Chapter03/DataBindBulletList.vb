Imports System
Imports System.ComponentModel
Imports System.Web.UI
Imports System.Drawing.Design
Imports System.Web.UI.HTMLControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.Design
Imports System.Collections

Namespace Apress.Toolkit

    <ToolboxData("<{0}:DataBindBulletList runat=""server"" />")> _
  Public Class DataBindBulletList
        Inherits WebControl
        Implements INamingContainer

        Private Class GenericEnumerator
            Implements IEnumerator

            Private _enumerator As IEnumerator
            Private sync As New Object

            Sub New(ByVal enumerator As IDictionaryEnumerator)
                Dim newCollection As New ArrayList
                While enumerator.MoveNext()
                    newCollection.Add(enumerator.Value)
                End While
                Me._enumerator = newCollection.GetEnumerator()
            End Sub

            Sub New(ByVal enumerator As IEnumerator)
                Me._enumerator = enumerator
            End Sub

            Public ReadOnly Property Current() As Object _
                   Implements IEnumerator.Current
                Get
                    SyncLock sync
                        Return _enumerator.Current
                    End SyncLock
                End Get
            End Property

            Public Function MoveNext() As Boolean _
                    Implements IEnumerator.MoveNext
                SyncLock sync
                    Return _enumerator.MoveNext()
                End SyncLock
            End Function

            Public Sub Reset() Implements IEnumerator.Reset
                SyncLock sync
                    _enumerator.Reset()
                End SyncLock
            End Sub
        End Class

        Public Enum DisplayOrientationType
            Vertical
            Horizontal
        End Enum

        Private _DataSource As IEnumerable

        <Category("ApressToolkit"), _
         Description("The relative path to an image file within the virtual directory to use for the bullet."), _
         Editor(GetType(System.Web.UI.Design.ImageUrlEditor), GetType(UITypeEditor))> _
        Public Property ImageForBullet() As String
            Get
                Dim obj As Object = ViewState.Item("ImageForBullet")
                If Not (obj Is Nothing) Then
                    Return CType(obj, String)
                End If
                Return String.Empty
            End Get
            Set(ByVal value As String)
                ViewState.Item("ImageForBullet") = value
                Me.ChildControlsCreated = False
            End Set
        End Property

        Private Property Bullets() As String
            Get
                Dim obj As Object = ViewState.Item("Bullets")
                If Not (obj Is Nothing) Then
                    Return CType(obj, String)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Item("Bullets") = value
                Me.ChildControlsCreated = False
            End Set
        End Property

        <Category("ApressToolkit"), _
         Description("The Data source to display in the list.")> _
        Public Property DataSource() As IEnumerable
            Get
                Return _DataSource
            End Get
            Set(ByVal value As IEnumerable)
                _DataSource = value
                Me.ChildControlsCreated = False
            End Set
        End Property

        <Category("ApressToolkit"), _
         Description("States wheter the list will be displayed Horizontally or Vertically. Only Valid if ColumnCount is 1.")> _
        Public Property DisplayOrientation() As DisplayOrientationType
            Get
                Dim obj As Object = ViewState.Item("DisplayOrientation")
                If Not (obj Is Nothing) Then
                    Return CType(obj, DisplayOrientationType)
                Else
                    Return DisplayOrientationType.Vertical
                End If
            End Get
            Set(ByVal value As DisplayOrientationType)
                ViewState.Item("DisplayOrientation") = value
                Me.ChildControlsCreated = False
            End Set
        End Property

        <Category("ApressToolkit"), _
         Description("The number of columns to be displayed in the list.")> _
       Public Property ColumnCount() As Integer
            Get
                Dim obj As Object = ViewState.Item("ColumnCount")
                If Not (obj Is Nothing) Then
                    Return CType(obj, Integer)
                Else
                    Return 1
                End If
            End Get
            Set(ByVal value As Integer)
                If value < 1 Then
                    Throw New ArgumentException("Value must be 1 or greater")
                End If
                ViewState.Item("ColumnCount") = value
                Me.ChildControlsCreated = False
                Me.EnsureChildControls()
            End Set
        End Property

        Private Sub CreateDesignTimeRendering()
            Dim _htmltable As New HtmlTable
            Dim tr As HtmlTableRow
            Dim tc As HtmlTableCell
            Dim Counter As Integer
            Dim ColumnCounter As Integer
            Dim totalItems As Integer

            If ColumnCount > 1 Then
                For Counter = 1 To 3
                    tr = New HtmlTableRow
                    For ColumnCounter = 1 To Me.ColumnCount
                        tc = BuildTableCell(String.Format( _
                            "(Row {0}, Column {1})", Counter, ColumnCounter))
                        tr.Cells.Add(tc)
                    Next
                    _htmltable.Rows.Add(tr)
                Next
            ElseIf Me.DisplayOrientation = _
                    DisplayOrientationType.Horizontal Then
                tr = New HtmlTableRow
                For Counter = 1 To 3
                    tc = BuildTableCell(String.Format( _
                            "(Row 1, Column {0})", _
                            Counter))
                    tr.Cells.Add(tc)
                Next
                _htmltable.Rows.Add(tr)
            Else
                For Counter = 1 To 3
                    tr = New HtmlTableRow
                    tc = BuildTableCell(String.Format("(Row {0}, Column 1)", _
                    Counter))
                    tr.Cells.Add(tc)
                    _htmltable.Rows.Add(tr)
                Next
            End If
        End Sub

        Protected Overrides Sub OnDataBinding(ByVal e As EventArgs)
            Dim Enumerator As IEnumerator = _
                New GenericEnumerator(_DataSource.GetEnumerator())
            Dim items() As String
            Dim c As Integer = 0


            Dim myitems As String = String.Empty

            While Enumerator.MoveNext()
                myitems = myitems + CType(Enumerator.Current, String) + "^"
            End While

            If myitems <> String.Empty Then
                myitems = myitems.Substring(0, myitems.Length - 1)
            End If

            Bullets = myitems

            Me.ChildControlsCreated = False
            Return

            items = New String(c) {}
            Enumerator.Reset()
            c = 0
            While Enumerator.MoveNext
                items(c) = CType(Enumerator.Current, String)
            End While
        End Sub

        Private Function GetDesignTimeItems() As String()
            Dim items(3 * Me.ColumnCount) As String
            Dim c As Integer = 0
            For c = 0 To Me.ColumnCount * 3
                items(c) = "Bullet " + c.ToString()
            Next
            Return items
        End Function


        Protected Overrides Sub CreateChildControls()
            Controls.Clear()

            Dim _htmltable As New HtmlTable
            Dim tr As HtmlTableRow
            Dim tc As HtmlTableCell
            Dim Counter As Integer
            Dim ColumnCounter As Integer
            Dim items() As String = Nothing
            Dim sep(1) As Char
            sep(0) = "^"c


            ' If there are bullets to show
            If Not (Bullets = Nothing) Then
                ' get them by spliting the original string using
                ' ^ as a separator char
                items = Bullets.Split(sep)
            Else
                ' if we are at run-time just return (the control will produce no output)
                If Not (Me.Context Is Nothing) Then
                    Return
                    ' if we are at design-time, get a design-time set of bullets
                Else
                    items = GetDesignTimeItems()
                End If
            End If


            If Me.ColumnCount = 1 Then
                If Me.DisplayOrientation = _
                       DisplayOrientationType.Horizontal Then
                    tr = New HtmlTableRow
                    Dim c As Integer
                    For c = 0 To items.Length - 1
                        tc = BuildTableCell(items(c))
                        tr.Controls.Add(tc)
                    Next
                    _htmltable.Rows.Add(tr)
                Else
                    Dim c As Integer
                    For c = 0 To items.Length - 1
                        tr = New HtmlTableRow
                        tc = BuildTableCell(items(c))
                        tr.Cells.Add(tc)
                        _htmltable.Rows.Add(tr)
                    Next
                End If
            Else
                Dim Enumerator As IEnumerator = items.GetEnumerator()
                For Counter = 0 To CInt(items.Length / ColumnCount)
                    tr = New HtmlTableRow
                    For ColumnCounter = 1 To ColumnCount
                        If Enumerator.MoveNext() Then
                            tc = BuildTableCell(Enumerator.Current.ToString())
                            tr.Cells.Add(tc)
                        Else
                            Exit For
                        End If
                    Next
                    If tr.Cells.Count > 0 Then _htmltable.Rows.Add(tr)
                Next
            End If
            Controls.Add(_htmltable)
        End Sub

        Private Function BuildTableCell(ByVal text As String) _
                As HtmlTableCell
            Dim listTag As String
            If Not Me.ImageForBullet = String.Empty Then
                listTag = "<ul><li style='list-style-image:url(" & _
                Me.ImageForBullet & ");'>"
            Else
                listTag = "<ul><li>"
            End If

            Dim tc As HtmlTableCell = New HtmlTableCell
            tc.NoWrap = True
            Dim cellLabel As Label = New Label
            cellLabel.ForeColor = MyBase.ForeColor
            cellLabel.Text = listTag & (text & "</li></ul>")
            tc.Controls.Add(cellLabel)

            Return tc
        End Function

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            EnsureChildControls()
            MyBase.Render(writer)
        End Sub
    End Class
End Namespace
