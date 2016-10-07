Imports System
Imports System.Web.UI
Imports System.Data
Imports System.IO
Imports System.Web.UI.WebControls
Imports System.Web.Caching

Namespace Apress.Toolkit
    Public Class XmlPollResult
        Inherits XmlPollBase
        Implements INamingContainer

        Public Property ImageSrc() As String
            Get
                Dim o As Object = ViewState("imageSrc")
                If Not (o Is Nothing) Then
                    Return CType(o, String)
                Else
                    Return String.Empty
                End If
            End Get

            Set(ByVal value As String)
                ViewState("imageSrc") = value
            End Set
        End Property

        Protected Overrides Sub CreateChildControls()
            Controls.Clear()
            Me.LoadXml()

            Dim table As table = New table
            table.ApplyStyle(TableStyle)

            Dim questionLabel As Label = New Label
            questionLabel.ApplyStyle(QuestionStyle)
            questionLabel.Text = Me.xmlSet.Tables("Poll").Rows(0)("Question").ToString()

            Dim headerCell As TableCell = New TableCell
            headerCell.Controls.Add(questionLabel)
            Dim noOfRows As Integer = Me.xmlSet.Tables("PollChoice").Rows.Count
            headerCell.ColumnSpan = 3 ' Changed

            Dim headerRow As TableRow = New TableRow
            headerRow.Cells.Add(headerCell)
            headerRow.ApplyStyle(HeaderItemStyle)
            table.Rows.Add(headerRow)

            Dim TotalVotes As Integer = 0
            Dim pos As Integer
            Dim dr As DataRow

            For Each dr In Me.xmlSet.Tables("PollChoice").Rows
                TotalVotes += CType(dr("Votes"), Integer)
            Next


            For Each dr In Me.xmlSet.Tables("PollChoice").Rows
                Dim pollRow As TableRow = New TableRow

                Dim nameCell As TableCell = New TableCell
                nameCell.Width = Unit.Percentage(50)
                nameCell.Controls.Add(New LiteralControl(dr("Text").ToString()))

                Dim imgCell As TableCell = New TableCell
                imgCell.Width = Unit.Percentage(35)

                Dim voteCount As Integer
                voteCount = CType(dr("Votes"), Integer)
                Dim img As Image = New Image
                img.ImageUrl = Me.ImageSrc
                ' Calculate the % for width
                img.Width = Unit.Percentage((CDbl(voteCount) / _
                        CDbl(TotalVotes)) * 100)
                img.Height = Unit.Parse("15")
                imgCell.Controls.Add(img)

                ' Add the % and number of votes
                Dim valueCell As TableCell = New TableCell
                valueCell.Width = Unit.Percentage(15)
                valueCell.Controls.Add(New LiteralControl( _
                        img.Width.Value.ToString("0.00") & " (" & voteCount.ToString() & ")"))
                pollRow.Cells.Add(nameCell)
                pollRow.Cells.Add(imgCell)
                pollRow.Cells.Add(valueCell)

                pollRow.ApplyStyle(BodyItemStyle)
                table.Rows.Add(pollRow)
            Next
            ' Add footer
            Dim footerRow As TableRow = New TableRow
            Dim footerCell As TableCell = New TableCell
            footerCell.ColumnSpan = 3 ' Changed
            footerCell.Controls.Add(New LiteralControl("Total Votes: " & TotalVotes))

            footerRow.Cells.Add(footerCell)
            footerRow.ApplyStyle(FooterItemStyle)
            table.Rows.Add(footerRow)

            Me.Controls.Add(table)
        End Sub

    End Class
End Namespace
