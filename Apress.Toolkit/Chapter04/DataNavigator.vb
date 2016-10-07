Imports System
Imports System.ComponentModel
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports System.Data
Imports System.Collections
Imports System.Text
Imports System.IO


Namespace Apress.Toolkit

    <ToolboxData("<{0}:DataNavigator runat=server></{0}:DataNavigator>")> _
    Public Class DataNavigator
        Inherits System.Web.UI.WebControls.WebControl
        Implements INamingContainer

        Public Event ButtonClick(ByVal sender As Object, _
            ByVal e As DataNavigatorButtonEventArgs)
        Private WithEvents PreviousButton As Button
        Private WithEvents NextButton As Button
        Private WithEvents SaveButton As Button
        Private WithEvents CancelButton As Button

        Public CurrentIndex As Integer

        Public DataSource As DataSet

        Private Function StringToDS(ByVal str As String) As DataSet
            Dim ds As DataSet = New DataSet
            Dim sw As StringReader = New StringReader(str)

            ds.ReadXml(sw)
            Return ds
        End Function

        Public DataTypes As TypeCode()

        Private Sub SetDataTableValues()
            Dim htmltable As htmltable = CType(Controls(0), htmltable)
            Dim tr As HtmlTableRow
            Dim datacol As DataColumn
            Dim colindex As Integer = 0

            For Each tr In htmltable.Rows
                datacol = DataSource.Tables(0).Columns(colindex)
                Dim colType As Type = DataSource.Tables(0).Columns(colindex).DataType
                If (colType Is GetType(Boolean)) Then
                    DataSource.Tables(0).Rows(CurrentIndex)(colindex) = _
                       Math.Abs(CType(CType(tr.Cells(1).Controls(0), _
                       CheckBox).Checked, Integer))
                ElseIf (colType Is GetType(DateTime)) Then
                    DataSource.Tables(0).Rows(CurrentIndex)(colindex) = _
                       CType(tr.Cells(1).Controls(0), Calendar).SelectedDate
                Else
                    DataSource.Tables(0).Rows(CurrentIndex)(colindex) = _
                       CType(tr.Cells(1).Controls(0), TextBox).Text
                End If
                colindex = colindex + 1
            Next
        End Sub

        Private Function GetControl(ByVal Index As Integer) As WebControl
            Dim colType As Type = DataSource.Tables(0).Columns(Index).DataType

            If (colType Is GetType(Boolean)) Then
                Dim chkBox As CheckBox = New CheckBox
                chkBox.Checked = _
                    CType(DataSource.Tables(0).Rows(CurrentIndex) _
                    (DataSource.Tables(0).Columns(Index)), Boolean)
                chkBox.ApplyStyle(ControlStyle)
                Return chkBox
            ElseIf (colType Is GetType(Decimal) Or colType Is GetType(Double) _
                        Or colType Is GetType(Int16) Or colType Is GetType(Int32) _
                        Or colType Is GetType(Int64) Or colType Is GetType(Single) _
                        Or colType Is GetType(UInt16) Or colType Is GetType(UInt32) _
                        Or colType Is GetType(UInt64) Or colType Is GetType(Char) _
                        Or colType Is GetType(String)) Then
                Dim txtBox As TextBox = New TextBox
                If (DataSource.Tables(0).Columns(Index).MaxLength <> -1) Then
                    txtBox.MaxLength = DataSource.Tables(0).Columns(Index).MaxLength
                End If
                txtBox.Text = _
                    DataSource.Tables(0).Rows(CurrentIndex) _
                    (DataSource.Tables(0).Columns(Index)).ToString()
                txtBox.ApplyStyle(ControlStyle)
                Return txtBox
            Else
                ' At this point we're handling a non-supported type
                ' add code to return your favorite control for such types
            End If
        End Function

        Protected Overrides Sub CreateChildControls()

            If DataSource.Tables(0) Is Nothing Then
                Return
            End If

            If DataSource.Tables(0).Rows.Count = 0 Then
                Return
            End If

            Controls.Clear()

            Dim htmltable As HtmlControls.HtmlTable = _
                New HtmlControls.HtmlTable
            Dim tr As HtmlTableRow
            Dim tc As HtmlTableCell
            Dim WebControl As WebControl
            Dim ColumnCounter As Integer

            For ColumnCounter = 0 To DataSource.Tables(0).Columns.Count - 1
                tr = New HtmlTableRow
                htmltable.Rows.Add(tr)
                tc = New HtmlTableCell
                tc.InnerHtml = _
                    DataSource.Tables(0).Columns(ColumnCounter).ColumnName
                tr.Cells.Add(tc)
                tc = New HtmlTableCell
                tr.Cells.Add(tc)
                WebControl = GetControl(ColumnCounter)
                tc.Controls.Add(WebControl)
            Next

            Controls.Add(htmltable)

            htmltable = New htmltable
            tr = New HtmlTableRow
            tc = New HtmlTableCell

            htmltable.Rows.Add(tr)

            PreviousButton = New Button
            With PreviousButton
                .Text = "Previous"
                .CausesValidation = True
                .ApplyStyle(ControlStyle)
                .CommandArgument = CurrentIndex.ToString()
            End With
            tc.Controls.Add(PreviousButton)

            tr.Cells.Add(tc)

            tc = New HtmlTableCell
            NextButton = New Button
            With NextButton
                .Text = "Next"
                .CausesValidation = True
                .ApplyStyle(ControlStyle)
                .CommandArgument = CurrentIndex.ToString()
            End With
            tc.Controls.Add(NextButton)

            tr.Cells.Add(tc)

            tc = New HtmlTableCell
            CancelButton = New Button
            With CancelButton
                .Text = "Cancel"
                .CausesValidation = True
                .ApplyStyle(ControlStyle)
                .CommandArgument = CurrentIndex.ToString()
                .Attributes.Add("onclick", _
          "window.returnValue=window.confirm('Are you sure you want to cancel?');void(0);")
            End With
            tc.Controls.Add(CancelButton)

            tr.Cells.Add(tc)

            tc = New HtmlTableCell
            SaveButton = New Button
            With SaveButton
                .Text = "Save"
                .CausesValidation = True
                .ApplyStyle(ControlStyle)
                .CommandArgument = CurrentIndex.ToString()
            End With

            tc.Controls.Add(SaveButton)

            tr.Cells.Add(tc)

            Controls.Add(htmltable)

        End Sub

        Private Sub PreviousButton_Click(ByVal sender As Object, _
            ByVal e As System.EventArgs) Handles PreviousButton.Click

            If Int32.Parse(PreviousButton.CommandArgument) <= 0 Then
                Return
            End If

            Dim ButtonEventArgs As New DataNavigatorButtonEventArgs(DataSource, _
                DataNavigatorButtonEventArgs.ButtonType.PrevButton)
            RaiseEvent ButtonClick(Me, ButtonEventArgs)

            If Not ButtonEventArgs.Cancel Then
                SetDataTableValues()
                CurrentIndex -= 1
                MyBase.ChildControlsCreated = False
            End If
        End Sub

        Private Sub NextButton_Click(ByVal sender As Object, _
            ByVal e As System.EventArgs) Handles NextButton.Click

            If Int32.Parse(PreviousButton.CommandArgument) >= _
                DataSource.Tables(0).Rows.Count - 1 Then
                Return
            End If

            Dim ButtonEventArgs As New DataNavigatorButtonEventArgs(DataSource, _
                DataNavigatorButtonEventArgs.ButtonType.NextButton)
            RaiseEvent ButtonClick(Me, ButtonEventArgs)

            If Not ButtonEventArgs.Cancel Then
                SetDataTableValues()
                CurrentIndex += 1
                MyBase.ChildControlsCreated = False
            End If
        End Sub

        Private Sub SaveButton_Click(ByVal sender As Object, _
            ByVal e As System.EventArgs) Handles SaveButton.Click

            Dim ButtonClickEventArgs As New _
                DataNavigatorButtonEventArgs(DataSource, _
                DataNavigatorButtonEventArgs.ButtonType.SaveButton)

            SetDataTableValues()

            RaiseEvent ButtonClick(Me, ButtonClickEventArgs)

            If Not ButtonClickEventArgs.Cancel Then
                DataSource.AcceptChanges()
            End If
        End Sub

        Private Sub CancelButton_Click(ByVal sender As Object, _
             ByVal e As System.EventArgs) Handles CancelButton.Click

            DataSource.RejectChanges()
            MyBase.ChildControlsCreated = False

            RaiseEvent ButtonClick(Me, _
                New DataNavigatorButtonEventArgs(DataSource, _
                DataNavigatorButtonEventArgs.ButtonType.CancelButton))
        End Sub



        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            EnsureChildControls()
            MyBase.Render(writer)
        End Sub

        Protected Overrides Function SaveViewState() As Object
            Dim t As Triplet = New Triplet
            t.First = CurrentIndex
            t.Second = DataSource
            t.Third = DataTypes
            Return t
        End Function

        Protected Overrides Sub LoadViewState(ByVal savedState As Object)
            Dim t As Triplet = CType(savedState, Triplet)
            CurrentIndex = CType(t.First, Integer)
            DataSource = CType(t.Second, DataSet)
            DataTypes = CType(t.Third, TypeCode())
        End Sub
    End Class
End Namespace