Imports System
Imports System.ComponentModel
Imports System.Web.UI
Imports System.Reflection
Imports System.Data
Imports System.Data.SqlClient

Namespace Apress.Toolkit

    <ToolboxData("<{0}:LastReview runat=server></{0}:LastReview>")> Public Class LastReview
        Inherits System.Web.UI.WebControls.WebControl
        Implements IPostBackEventHandler

        Dim _connection As String
        Dim _renderAllReviews As Boolean = False

        <Bindable(True), Category("Appearance"), DefaultValue("")> Property ConnectionString() As String
            Get
                Return _connection
            End Get

            Set(ByVal Value As String)
                _connection = Value
            End Set
        End Property

        Dim _maxchars As Integer

        <Bindable(True), Category("Appearance"), DefaultValue("200")> Property MaxChars() As Integer
            Get
                Return _maxchars
            End Get

            Set(ByVal Value As Integer)
                _maxchars = Value
            End Set
        End Property

        Dim _productid As Integer

        Public Property ProductID() As Integer
            Get
                Return _productid
            End Get

            Set(ByVal Value As Integer)
                _productid = Value
            End Set
        End Property

        Protected Overrides Sub Render(ByVal output As System.Web.UI.HtmlTextWriter)
            If (_renderAllReviews) Then
                RenderAllReviews(output)
            Else
                RenderLastReview(output)
            End If
        End Sub

        Sub RaisePostBackEvent(ByVal eventArgument As String) Implements IPostBackEventHandler.RaisePostBackEvent
            If eventArgument = "GetAllReviews" Then
                _renderAllReviews = True
            End If
        End Sub

        Sub RenderLastReview(ByVal output As HtmlTextWriter)
            Dim strHTML As String
            strHTML = "<TABLE BORDER='0' CELLPADDING='0' CELLSPACING='0' WIDTH='100%'>" & _
                        "<TR>" & _
                            "<TD WIDTH='13' BGCOLOR='#7a97c6' ALIGN='left'><IMG SRC='images/triangle.gif' BORDER='0'><BR>" & _
                            "</TD>" & _
                            "<TD BGCOLOR='#7a97c6'><STRONG><FONT face='Verdana' color='#ffffff' size='1'>Last review</FONT></STRONG><BR>" & _
                            "</TD>" & _
                        "</TR>" & _
                    "</TABLE>" & _
                    "<TABLE BORDER='0' CELLPADDING='0' CELLSPACING='0' WIDTH='100%'>" & _
                        "<TR>" & _
                            "<TD BGCOLOR='#ffffff' align='middle'>" & _
                                "<TABLE cellpadding='1' cellspacing='1' border='0'>" & _
                                    "<TBODY>" & _
                                        "<TR>" & _
                                        "</TR>" & _
                                        "<TR>" & _
                                            "<TD>" & _
                                                "<FONT FACE='Verdana, Arial' SIZE='1'><B>Author:</B> [Author]<BR>" & _
                                                "<B>Review:</B> [Review] </FONT>" & _
                                            "</TD>" & _
                                            "<TD>" & _
                                                "<IMG SRC='[ImageSrc]' BORDER='0' ALT='[Vote]'>" & _
                                            "</TD>" & _
                                        "</TR>" & _
                                        "<TR>" & _
                                            "<TD><FONT FACE='Verdana, Arial' SIZE='1'><A href=""" & _
                                                    "Javascript:[Link]" & _
                                                    ";"">See all reviews...</A></FONT></TD>" & _
                                        "</TR>" & _
                                    "</TBODY>" & _
                                "</TABLE>" & _
                            "</TD>" & _
                        "</TR>" & _
                    "</TABLE>"

            Dim ds As DataSet
            Dim dbutil As New ReviewerDB
            dbutil.ConnectionString = ConnectionString
            dbutil.ProductID = ProductID
            ds = dbutil.GetLastReview

            If Not ds Is Nothing Then
                Dim row As DataRow
                Dim strReview As String
                For Each row In ds.Tables(0).Rows
                    ' Replace placeholder with author
                    strHTML = strHTML.Replace("[Author]", _
 row("author").ToString())

                    ' Replace placeholder with review content
                    strReview = row("review").ToString()
                    If strReview.Length > MaxChars Then
                        strReview = strReview.Remove(MaxChars - 3, strReview.Length - MaxChars) & "..."
                    End If
                    strHTML = strHTML.Replace("[Review]", strReview)

                    ' Replace placeholder with image source and ALT
                    Select Case row("rating")
                        Case "1"
                            strHTML = strHTML.Replace("[ImageSrc]", "images/10star.gif")
                            strHTML = strHTML.Replace("[Vote]", "10 stars")
                        Case "2"
                            strHTML = strHTML.Replace("[ImageSrc]", "images/20star.gif")
                            strHTML = strHTML.Replace("[Vote]", "20 stars")
                        Case "3"
                            strHTML = strHTML.Replace("[ImageSrc]", "images/30star.gif")
                            strHTML = strHTML.Replace("[Vote]", "30 stars")
                        Case "4"
                            strHTML = strHTML.Replace("[ImageSrc]", "images/40star.gif")
                            strHTML = strHTML.Replace("[Vote]", "40 stars")
                        Case "5"
                            strHTML = strHTML.Replace("[ImageSrc]", "images/50star.gif")
                            strHTML = strHTML.Replace("[Vote]", "50 stars")
                    End Select

                    ' Replace placeholder with Link to show all reviews
                    strHTML = strHTML.Replace("[Link]", Page.GetPostBackEventReference(Me, "GetAllReviews"))

                Next

                output.Write(strHTML)
            Else
                output.Write("No reviews for the specified product were found.")
            End If
        End Sub

        Sub RenderAllReviews(ByVal output As HtmlTextWriter)
            Dim strHTML As String = ""
            Dim ds As DataSet
            Dim dbutil As New ReviewerDB
            dbutil.ConnectionString = ConnectionString
            dbutil.ProductID = ProductID
            ds = dbutil.GetAllReviews
            If Not ds Is Nothing Then
                Dim row As DataRow
                strHTML = "<font face='arial'><h2>What people are saying about this product</h2><hr>"
                Dim strTABLE As String

                For Each row In ds.Tables(0).Rows
                    strTABLE = strTABLE & "<table width='100%' border='0' cellpadding='0'><tr><td width='60%'><b>[Author]</b> writes:<br>[Review]</td><td width='20%' " & _
"align='center'>[Date]</td><td width='20%' align='right'>Rating:<img src='[ImageSrc]'></td></tr>"
                    strTABLE = strTABLE.Replace("[Author]", row("author").ToString)
                    strTABLE = strTABLE.Replace("[Review]", row("review").ToString)
                    strTABLE = strTABLE.Replace("[Date]", CType(row("ReviewDate"), Date).ToShortDateString())

                    Select Case row("rating")
                        Case "1"
                            strTABLE = strTABLE.Replace("[ImageSrc]", "images/10star.gif")
                        Case "2"
                            strTABLE = strTABLE.Replace("[ImageSrc]", "images/20star.gif")
                        Case "3"
                            strTABLE = strTABLE.Replace("[ImageSrc]", "images/30star.gif")
                        Case "4"
                            strTABLE = strTABLE.Replace("[ImageSrc]", "images/40star.gif")
                        Case "5"
                            strTABLE = strTABLE.Replace("[ImageSrc]", "images/50star.gif")
                    End Select
                    strTABLE = strTABLE & "</table><hr>"
                Next

                strHTML = strHTML & strTABLE & "</font>"
            End If
            output.Write(strHTML)
        End Sub

    End Class

End Namespace
