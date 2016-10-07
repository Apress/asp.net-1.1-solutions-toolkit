Imports System
Imports System.Web.UI
Imports System.Data
Imports System.IO
Imports System.Web.UI.WebControls
Imports System.Web.Caching
Imports System.Web

Namespace Apress.Toolkit

    Public Class XmlPoll
        Inherits XmlPollBase
        Implements INamingContainer

        Private Enum RenderView
            Poll = 1
            ThankYouNote = 2
            DuplicateVote = 3
        End Enum

        Public Event VoteCast As VoteCastEventHandler
        Private optList As RadioButtonList
        Private _optionStyle As Style = New Style
        Private _buttonStyle As Style = New Style
        Private _renderView As RenderView = RenderView.Poll

        Public Property OptionStyle() As Style
            Get
                Return _optionStyle
            End Get

            Set(ByVal value As Style)
                _optionStyle = value
            End Set
        End Property

        Property ButtonStyle() As Style
            Get
                Return _buttonStyle
            End Get

            Set(ByVal value As Style)
                _buttonStyle = value
            End Set
        End Property

        Protected Overrides Sub CreateChildControls()
            If (_renderView = RenderView.DuplicateVote) Then
                Dim lbl As Label = New Label
                lbl.Text = "You've already voted in this poll."
                Controls.Add(lbl)
                Return
            ElseIf (_renderView = RenderView.ThankYouNote) Then
                Dim lbl As Label = New Label
                lbl.Text = "Thank you for voting!"
                Controls.Add(lbl)
                Return
            End If

            Me.LoadXml()

            Dim table As New table
            table.ApplyStyle(Me.TableStyle)

            Dim headerRow As TableRow = New TableRow

            Dim questionLabel As Label = New Label
            questionLabel.ApplyStyle(QuestionStyle)
            questionLabel.Text = Me.xmlSet.Tables("Poll").Rows(0)("Question").ToString()

            Dim headerCell As TableCell = New TableCell
            headerCell.Controls.Add(questionLabel)

            headerRow.Cells.Add(headerCell)
            headerRow.ApplyStyle(HeaderItemStyle)

            table.Rows.Add(headerRow)

            Dim bodyRow As TableRow = New TableRow

            optList = New RadioButtonList
            With optList
                .ApplyStyle(OptionStyle)
                .DataSource = Me.xmlSet.Tables("PollChoice")
                .DataTextField = "Text"
                .DataValueField = "Value"
                .DataBind()
                .SelectedIndex = 0
            End With

            Dim bodyCell As TableCell = New TableCell
            bodyCell.Controls.Add(optList)

            bodyRow.Cells.Add(bodyCell)
            bodyRow.ApplyStyle(BodyItemStyle)
            table.Rows.Add(bodyRow)

            Dim pollButton As Button = New Button
            pollButton.ApplyStyle(ButtonStyle)
            pollButton.Text = "Vote!"

            AddHandler pollButton.Click, AddressOf Me.PollButton_Click

            Dim footerRow As TableRow = New TableRow
            Dim footerCell As TableCell = New TableCell

            footerCell.Controls.Add(pollButton)
            footerRow.Cells.Add(footerCell)
            footerRow.ApplyStyle(FooterItemStyle)
            table.Rows.Add(footerRow)

            Me.Controls.Add(table)
        End Sub

        Private Sub PollButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            If Context.Request.Browser.Cookies Then
                Dim cookieKey As String = Me.UniqueID
                Dim cookieValue As Integer = _
                Me.xmlSet.Tables("Poll").Rows(0)("Question").GetHashCode()

                Dim pollCookie As HttpCookie = _
                        Context.Request.Cookies.Item("pollCookie")
                If pollCookie Is Nothing Then
                    AddVote(cookieKey, CStr(cookieValue))
                Else
                    If Not pollCookie.Values(cookieKey) = CStr(cookieValue) Then
                        AddVote(cookieKey, CStr(cookieValue))
                    Else
                        SetRenderView(RenderView.DuplicateVote)
                    End If
                End If
            End If
        End Sub


        Private Sub AddVote(ByVal cookieKey As String, ByVal cookieValue As String)
            ' Update the xml file
            SaveXml(cookieKey, CStr(cookieValue))
            ' Set flag in cookie
            WriteCookie(cookieKey, CStr(cookieValue))
            ' Set appropiate rendering
            SetRenderView(RenderView.ThankYouNote)
            ' Fire event
            Me.OnVoteCast(New VoteEventArgs(Me.optList.SelectedItem.Text, _
            Me.optList.SelectedItem.Value))
        End Sub

        Private Sub SetRenderView(ByVal renderView As RenderView)
            _renderView = renderView
            ChildControlsCreated = False
        End Sub

        Private Sub WriteCookie(ByVal cookieKey As String, ByVal cookieValue As String)
            Dim pollCookie As HttpCookie = Context.Request.Cookies("pollCookie")
            If (pollCookie Is Nothing) Then
                pollCookie = New HttpCookie("pollCookie")
            End If
            pollCookie.Values.Add(cookieKey, cookieValue)
            pollCookie.Expires = DateTime.MaxValue
            Context.Response.Cookies.Add(pollCookie)
        End Sub

        Private Sub SaveXml(ByVal cookieKey As String, ByVal cookieValue As String)
            Dim selRow As DataRow() = _
                    Me.xmlSet.Tables("PollChoice").Select("Value = " & _
                    Me.optList.SelectedItem.Value)

            Dim votes As Integer = CType(selRow(0)("Votes"), Integer)
            selRow(0)("Votes") = votes + 1

            Me.xmlSet.AcceptChanges()

            Dim writeStream As FileStream
            Try
                writeStream = New FileStream(Me.XmlFile, FileMode.Truncate, _
                        FileAccess.Write, FileShare.ReadWrite)
                SyncLock Me.XmlFile
                    Me.xmlSet.WriteXml(writeStream)
                End SyncLock
            Finally
                If Not writeStream Is Nothing Then
                    writeStream.Close()
                End If
            End Try
        End Sub

        Protected Overridable Sub OnVoteCast(ByVal voteDetails As Apress.Toolkit.VoteEventArgs)
            RaiseEvent VoteCast(Me, voteDetails)
        End Sub


        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            If Context.Request Is Nothing Then
                writer.Write("Poll Control")
            Else
                MyBase.Render(writer)
            End If

        End Sub
    End Class
End Namespace
