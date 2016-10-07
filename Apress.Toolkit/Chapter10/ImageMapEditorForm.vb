Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections
Imports System

Public Class ImageMapEditorForm
    Inherits System.Windows.Forms.Form

    Private StartPoint As New Point
    Private ContextPoint As New Point
    Private dragging As Boolean = False
    Private Coords As New ArrayList
    Private rect As Rectangle
    Private p As New Pen(Color.White)
    Private img As Bitmap
    Private imgSource As String

    Public Property ImagePath() As String
        Get
            Return imgSource
        End Get
        Set(ByVal Value As String)
            imgSource = Value
            Try
                Dim bmp As New Bitmap(Value)
                imgSource = Value
                img = bmp
                PictureBox1.Image = bmp
                PictureBox1.Invalidate()
            Catch ex As Exception
                System.Windows.Forms.MessageBox.Show(ex.Message & " " & Value)
            End Try
        End Set
    End Property

    Public Property Regions() As ArrayList
        Get
            Return Coords
        End Get
        Set(ByVal Value As ArrayList)
            Coords = Value
        End Set
    End Property


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ImageMapContextMenu As System.Windows.Forms.ContextMenu
    Friend WithEvents DeleteRect As System.Windows.Forms.MenuItem
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents RectangleSelected As System.Windows.Forms.RadioButton
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.ImageMapContextMenu = New System.Windows.Forms.ContextMenu
        Me.DeleteRect = New System.Windows.Forms.MenuItem
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.RectangleSelected = New System.Windows.Forms.RadioButton
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.ContextMenu = Me.ImageMapContextMenu
        Me.PictureBox1.Location = New System.Drawing.Point(128, 16)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(472, 408)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'ImageMapContextMenu
        '
        Me.ImageMapContextMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.DeleteRect})
        '
        'DeleteRect
        '
        Me.DeleteRect.Index = 0
        Me.DeleteRect.Text = "Delete"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(0, 16)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(104, 40)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Outline Color"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Red
        Me.Label4.Location = New System.Drawing.Point(80, 16)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(16, 16)
        Me.Label4.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Blue
        Me.Label3.Location = New System.Drawing.Point(56, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(16, 16)
        Me.Label3.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(32, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(16, 16)
        Me.Label2.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(8, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(16, 16)
        Me.Label1.TabIndex = 0
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.RectangleSelected)
        Me.GroupBox2.Location = New System.Drawing.Point(0, 72)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(104, 64)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Shape"
        '
        'RectangleSelected
        '
        Me.RectangleSelected.Checked = True
        Me.RectangleSelected.Location = New System.Drawing.Point(8, 24)
        Me.RectangleSelected.Name = "RectangleSelected"
        Me.RectangleSelected.Size = New System.Drawing.Size(80, 24)
        Me.RectangleSelected.TabIndex = 0
        Me.RectangleSelected.TabStop = True
        Me.RectangleSelected.Text = "Rectangle"
        '
        'Button1
        '
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button1.Location = New System.Drawing.Point(464, 432)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(64, 24)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "Apply"
        '
        'Button2
        '
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.Location = New System.Drawing.Point(536, 432)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(64, 23)
        Me.Button2.TabIndex = 4
        Me.Button2.Text = "Cancel"
        '
        'ImageMapEditorForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(610, 463)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ImageMapEditorForm"
        Me.Text = "Image Map Editor"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub PictureBox1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseMove
        If dragging Then
            Dim width As Integer = e.X - StartPoint.X
            Dim height As Integer = e.Y - StartPoint.Y
            rect = New Rectangle(StartPoint.X, StartPoint.Y, width, height)
            PictureBox1.Invalidate()
        End If
    End Sub

    Private Sub PictureBox1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseDown
        If e.Button = MouseButtons.Left Then
            dragging = True
            StartPoint = New Point(e.X, e.Y)
        Else
            dragging = False
        End If

        If e.Button = MouseButtons.Right Then
            'get point of mouse for context menu
            ContextPoint.X = e.X
            ContextPoint.Y = e.Y
        End If
    End Sub

    Private Sub PictureBox1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseUp
        If e.Button = MouseButtons.Left Then
            If dragging Then
                dragging = False
                Coords.Add(rect)
                PictureBox1.Invalidate()
            End If
        End If
    End Sub

    Private Sub PictureBox1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PictureBox1.Paint
        Dim r As Rectangle
        Dim counter As Integer
        Dim g As Graphics = e.Graphics
        For counter = 1 To Coords.Count
            p.DashStyle = Drawing.Drawing2D.DashStyle.Solid
            r = CType(Coords.Item(counter - 1), Rectangle)
            g.DrawRectangle(p, r)
            'draw a label on the rectangle
            g.DrawString(counter.ToString(), New Font("Arial", 12), Brushes.Gray, r.X, r.Y)
        Next
        If dragging Then
            p.DashStyle = Drawing.Drawing2D.DashStyle.Dash
            g.DrawRectangle(p, rect)
        End If
    End Sub

    Private Sub ChangeColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click, Label2.Click, Label3.Click, Label4.Click
        p.Color = CType(sender, Label).BackColor
    End Sub

    Private Sub DeleteRect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DeleteRect.Click
        Dim r As Rectangle
        Dim Counter As Integer
        For Counter = 0 To Coords.Count - 1
            r = CType(Coords.Item(Counter), Rectangle)
            If r.Contains(ContextPoint) Then
                Coords.Remove(r)
                PictureBox1.Invalidate()
                Exit For
            End If
        Next
    End Sub
End Class
