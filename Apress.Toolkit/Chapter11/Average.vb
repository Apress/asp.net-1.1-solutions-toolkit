Imports System
Imports System.ComponentModel
Imports System.Web.UI
Imports System.Reflection
Imports System.Data
Imports System.Data.SqlClient

Namespace Apress.Toolkit

    <ToolboxData("<{0}:Average runat=server></{0}:Average>")> Public Class Average
        Inherits System.Web.UI.WebControls.WebControl

        Dim _connection As String

        <Bindable(True), Category("Appearance"), DefaultValue("")> Property ConnectionString() As String
            Get
                Return _connection
            End Get

            Set(ByVal Value As String)
                _connection = Value
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
            Dim ds As DataSet
            Dim dbutil As New ReviewerDB
            dbutil.ConnectionString = ConnectionString
            dbutil.ProductID = ProductID
            Dim average As Single = dbutil.GetAverage

            If average <> 0 Then
                Dim strHTML As String = "<TABLE BORDER='0' CELLPADDING='0' CELLSPACING='0' WIDTH='100%'>" & _
                        "<TR>" & _
                            "<TD BGCOLOR='#ffffff' align='middle'>" & _
                                "<TABLE cellpadding='1' cellspacing='1' border='0'>" & _
                                    "<TBODY>" & _
                                        "<TR>" & _
                                            "<TD>" & _
                                                "<FONT FACE='Verdana, Arial' SIZE='1'><B>Rating:</B>" & _
                                                "</FONT><BR>" & _
                                            "</TD>" & _
                                            "<TD>" & _
                                                "<IMG SRC='[ImageSrc]' BORDER='0' ALT='[Vote]'>" & _
                                            "</TD>" & _
                                        "</TR>" & _
                                    "</TBODY>" & _
                                "</TABLE>" & _
                            "</TD>" & _
                        "</TR>" & _
                    "</TABLE>"

                Select Case CInt(average * 4)
                    Case 4
                        strHTML = strHTML.Replace("[ImageSrc]", "images/10star.gif")
                        strHTML = strHTML.Replace("[Vote]", "1 star")
                    Case 5
                        strHTML = strHTML.Replace("[ImageSrc]", "images/13star.gif")
                        strHTML = strHTML.Replace("[Vote]", "1.25 stars")
                    Case 6
                        strHTML = strHTML.Replace("[ImageSrc]", "images/15star.gif")
                        strHTML = strHTML.Replace("[Vote]", "1.5 stars")
                    Case 7
                        strHTML = strHTML.Replace("[ImageSrc]", "images/17star.gif")
                        strHTML = strHTML.Replace("[Vote]", "1.75 stars")
                    Case 8
                        strHTML = strHTML.Replace("[ImageSrc]", "images/20star.gif")
                        strHTML = strHTML.Replace("[Vote]", "2 stars")
                    Case 9
                        strHTML = strHTML.Replace("[ImageSrc]", "images/23star.gif")
                        strHTML = strHTML.Replace("[Vote]", "2.25 stars")
                    Case 10
                        strHTML = strHTML.Replace("[ImageSrc]", "images/25star.gif")
                        strHTML = strHTML.Replace("[Vote]", "2.5 stars")
                    Case 11
                        strHTML = strHTML.Replace("[ImageSrc]", "images/27star.gif")
                        strHTML = strHTML.Replace("[Vote]", "2.75 stars")
                    Case 12
                        strHTML = strHTML.Replace("[ImageSrc]", "images/30star.gif")
                        strHTML = strHTML.Replace("[Vote]", "3 stars")
                    Case 13
                        strHTML = strHTML.Replace("[ImageSrc]", "images/33star.gif")
                        strHTML = strHTML.Replace("[Vote]", "3.25 stars")
                    Case 14
                        strHTML = strHTML.Replace("[ImageSrc]", "images/35star.gif")
                        strHTML = strHTML.Replace("[Vote]", "3.5 stars")
                    Case 15
                        strHTML = strHTML.Replace("[ImageSrc]", "images/37star.gif")
                        strHTML = strHTML.Replace("[Vote]", "3.75 stars")
                    Case 16
                        strHTML = strHTML.Replace("[ImageSrc]", "images/40star.gif")
                        strHTML = strHTML.Replace("[Vote]", "4 stars")
                    Case 17
                        strHTML = strHTML.Replace("[ImageSrc]", "images/43star.gif")
                        strHTML = strHTML.Replace("[Vote]", "4.25 stars")
                    Case 18
                        strHTML = strHTML.Replace("[ImageSrc]", "images/45star.gif")
                        strHTML = strHTML.Replace("[Vote]", "4.5 stars")
                    Case 19
                        strHTML = strHTML.Replace("[ImageSrc]", "images/47star.gif")
                        strHTML = strHTML.Replace("[Vote]", "4.75 stars")
                    Case 20
                        strHTML = strHTML.Replace("[ImageSrc]", "images/50star.gif")
                        strHTML = strHTML.Replace("[Vote]", "5 stars")
                End Select

                output.Write(strHTML)
            Else
                output.Write("No average for the specified product.")
            End If
        End Sub

    End Class
End Namespace