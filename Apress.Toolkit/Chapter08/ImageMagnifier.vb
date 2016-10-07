Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D

Namespace Apress.Toolkit

    Public Class ImageMagnifier
        Inherits System.Web.UI.WebControls.ImageButton

        Private _tempPath, _thumbPath, _fileName As String
        ' Zoom factor  
        Private _zoomFactor As Integer = 2

        Public Property TempPath() As String
            Get
                Return Me._tempPath
            End Get
            Set(ByVal Value As String)
                Me._tempPath = Value
            End Set
        End Property

        Protected Overrides Sub OnLoad(ByVal e As EventArgs)
            ' Call the Base classes OnLoad method
            MyBase.OnLoad(e)

            ' Set the local path to the temporary directory
            Me._thumbPath = Me.Page.MapPath(Me.TempPath) + System.IO.Path.DirectorySeparatorChar

            ' Set a unique file name based on the SessionID 
            ' and Current time
            Me._fileName = Me.Context.Session.SessionID + DateTime.Now.Ticks.ToString() + ".jpg"

            ' If the ViewState value does not exist, 
            ' we are loading the control for the first time

            If ViewState("originalImageURL") Is Nothing Then

                ' Call method to load image for the first time
                Me.ProcessOriginalImage()

            End If
        End Sub

        Protected Overridable Sub ProcessOriginalImage()

            ' Save the path to the User specified original image
            ViewState("originalImageURL") = Me.ImageUrl

            Dim orgImage As System.Drawing.Image = Nothing
            Dim thumbImage As System.Drawing.Image = Nothing

            Try
                ' Build Image from the user specified path
                orgImage = System.Drawing.Image.FromFile(Me.Page.MapPath(Me.ImageUrl))


                Dim myCallback As New System.Drawing.Image.GetThumbnailImageAbort(AddressOf ThumbnailCallback)
                ' Get a Thumbnail of the original image, of the size 
                ' specified by the user
                thumbImage = orgImage.GetThumbnailImage(Convert.ToInt32(Me.Width.Value), Convert.ToInt32(Me.Height.Value), myCallback, System.IntPtr.Zero)

                ' Save the Height and Width of the thumbnail image in 		    ' ViewState
                ViewState("newWidth") = thumbImage.Width
                ViewState("newHeight") = thumbImage.Height

                ' Save the thumbnail to the temporary location
                thumbImage.Save(Me._thumbPath + Me._fileName, ImageFormat.Jpeg)

            Catch e As Exception
                ' Re-throw the exception
                Throw New Exception("An exception occurred while creating the ImageMagnifier Control", e)
            Finally
                ' Never forget to release the resources
                orgImage.Dispose()
                thumbImage.Dispose()
            End Try

            ' Set the ImageUrl to the thumbnail image created
            Me.ImageUrl = Me.TempPath + "/" + Me._fileName
        End Sub

        Public Function ThumbnailCallback() As Boolean
            Return False
        End Function

        Protected Overrides Sub OnClick(ByVal e As ImageClickEventArgs)
            Me.ShowZoomImage(e.X, e.Y)
        End Sub

        Protected Overridable Sub ShowZoomImage(ByVal coordX As Integer, ByVal coordY As Integer)
            Dim orgImage As System.Drawing.Image = Nothing
            Dim thumbImage As System.Drawing.Image = Nothing
            Dim clipImage As System.Drawing.Bitmap = Nothing
            Dim clipBrush As System.Drawing.TextureBrush = Nothing
            Dim imgGraphics As System.Drawing.Graphics = Nothing
            Try
                ' Retrieve the Original user specified image
                ' The path of the original Image is stored in ViewState
                orgImage = System.Drawing.Image.FromFile(Me.Page.MapPath(ViewState("originalImageURL").ToString()))

                ' Create Height and Width for the new thumbnail,
                ' Applying 2x Zoom, based on the current thumbnail image
                Dim newWidth As Integer = Integer.Parse(ViewState("newWidth").ToString()) * Me._zoomFactor
                Dim newHeight As Integer = Integer.Parse(ViewState("newHeight").ToString()) * Me._zoomFactor



                If newWidth < orgImage.Width And newHeight < orgImage.Height Then

                    Dim myCallback As New System.Drawing.Image.GetThumbnailImageAbort(AddressOf ThumbnailCallback)

                    thumbImage = orgImage.GetThumbnailImage(newWidth, newHeight, myCallback, System.IntPtr.Zero)
                Else
                    thumbImage = orgImage
                End If


                Dim imgControlWidth As Integer = Convert.ToInt32(Me.Width.Value)
                Dim imgControlHeight As Integer = Convert.ToInt32(Me.Height.Value)


                ' Retrieve the Previous Clip Rectangle start co-ordinates 
                Dim prevX As Integer = 0
                Dim prevY As Integer = 0

                If Not (ViewState("rectX") Is Nothing) And Not (ViewState("rectY") Is Nothing) Then
                    ' Parse the values from ViewState
                    prevX = Integer.Parse(ViewState("rectX").ToString())
                    prevY = Integer.Parse(ViewState("rectY").ToString())
                End If

                ' Get the clipping path
                ' Map the X & Y coordinates on the newly created thumbnail 
                ' based  on the point where the user clicked
                Dim y As Integer = CInt(thumbImage.Height * (coordY + prevY) / Integer.Parse(ViewState("newHeight").ToString()))
                Dim x As Integer = CInt(thumbImage.Width * (coordX + prevX) / Integer.Parse(ViewState("newWidth").ToString()))

                Dim rectX As Integer = 0
                Dim rectY As Integer = 0

                ' Temporary helper variables
                Dim halfWidth As Integer = CInt(imgControlWidth / 2)
                Dim halfHeight As Integer = CInt(imgControlHeight / 2)

                ' Calculate the Start point of the clipping rectangle

                If ((thumbImage.Width - x) < halfWidth) Then
                    rectX = x - (imgControlWidth - (thumbImage.Width - x))
                Else
                    If x < halfWidth Then
                        rectX = 0
                    Else
                        rectX = x - halfWidth
                    End If
                End If

                If (thumbImage.Height - y) < halfHeight Then
                    rectY = y - (imgControlHeight - (thumbImage.Height - y))
                Else
                    If y < halfHeight Then
                        rectY = 0
                    Else
                        rectY = y - halfHeight
                    End If
                End If

                ' Save the Rectangle start co-ordinates
                ViewState("rectX") = rectX
                ViewState("rectY") = rectY

                ' Create a new Rectangle object
                Dim clipRect As New Rectangle(rectX, rectY, imgControlWidth, imgControlHeight)
                ' Create a blank Bitmap of the size specified by the user 
                clipImage = New System.Drawing.Bitmap(imgControlWidth, imgControlHeight)

                ' Create a new TextureBrush and select the clip part from 		    '	the thumbnail
                clipBrush = New TextureBrush(thumbImage, clipRect)

                ' Create a Graphics object for the blank bitmap
                imgGraphics = Graphics.FromImage(clipImage)

                ' Paint the selected clip on the Graphics of the 
                ' blank bitmap
                imgGraphics.FillRectangle(clipBrush, 0, 0, imgControlWidth, imgControlHeight)

                ' Save the bitmap at the specified temporary path
                clipImage.Save(Me._thumbPath + Me._fileName, ImageFormat.Jpeg)

                ' Save the thumbnail images values
                ViewState("newWidth") = thumbImage.Width
                ViewState("newHeight") = thumbImage.Height

            Catch exc As Exception
                ' Re-throw the exception
                Throw New Exception("An exception occurred while creating the ImageMagnifer Control", exc)
            Finally
                ' Never forget to release the resources
                clipBrush.Dispose()
                imgGraphics.Dispose()
                clipImage.Dispose()
                orgImage.Dispose()
                thumbImage.Dispose()
            End Try
            ' Set the ImageUrl to the thumbnail image created
            Me.ImageUrl = Me.TempPath + "/" + Me._fileName
        End Sub

    End Class
End Namespace

