Option Strict On

Imports System
Imports System.ComponentModel
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Design
Imports System.Drawing.Design
Imports System.Drawing
Imports System.Windows.Forms.Design
Imports System.Windows.Forms
Imports System.Collections
Imports Microsoft.VisualBasic

Namespace Apress.Toolkit

    Public Class ImageMapEventArgs
        Inherits EventArgs

        Private _RegionID As String
        Public ReadOnly Property RegionID() As String
            Get
                Return _RegionID
            End Get
        End Property

        Public Sub New(ByVal RegionID As String)
            MyBase.New()
            _RegionID = RegionID
        End Sub
    End Class

    <ToolboxData("<{0}:ImageMap runat=server></{0}:ImageMap>"), _
     DefaultEvent("RegionClicked")> _
  Public Class ImageMap
        Inherits Web.UI.Control
        Implements IPostBackEventHandler, INamingContainer

        Public Event RegionClicked(ByVal sender As Object, _
                                   ByVal e As ImageMapEventArgs)

        Private _imagePath As String = String.Empty
        Private _imgTag As New HtmlImage
        Private _fullPathToImage As String = String.Empty
        Private _imageMapProperties As New ImageMapProperties("")

        <Editor(GetType(ImageUrlEditor), GetType(UITypeEditor)), _
    Description("Relative path to Image file.")> _
    Public Property ImageSrc() As String
            Get
                Return _imagePath
            End Get

            Set(ByVal value As String)
                _imagePath = value
                ChildControlsCreated = False
            End Set
        End Property

        <Editor(GetType(ImageMapEditor), GetType(UITypeEditor))> _
    Public Property ImageMap() As String
            Get
                Return _imageMapProperties.ToString()
            End Get

            Set(ByVal value As String)
                _imageMapProperties = New ImageMapProperties(value)
                ChildControlsCreated = False
            End Set
        End Property

        <Description("Absolute path to image")> _
    Public Property AbsoluteImagePath() As String
            Get
                Return _fullPathToImage
            End Get

            Set(ByVal Value As String)
                _fullPathToImage = Value
            End Set
        End Property

        Protected Overrides Sub CreateChildControls()
            Controls.Clear()

            _imgTag.ID = "Img"
            _imgTag.Src = _imagePath
            Dim id As String = Me.ClientID
            _imgTag.Attributes.Add("usemap", "#" & id & "Map")
            Controls.Add(_imgTag)

            Dim map As New HtmlControls.HtmlGenericControl("map")
            map.ID = "mapa"
            map.Attributes.Add("name", id & "Map")
            map.Attributes.Add("id", id & "Map")

            Dim numRegions As Integer = _
                      _imageMapProperties.RectCoords.Count

            If numRegions > 0 Then
                ' we estimate the size of our string builder
                ' to preserve resources for memory allocations
                Dim r As Rectangle
                Dim counter As Integer
                For counter = 0 To numRegions - 1
                    Dim area As New HtmlGenericControl("area")
                    area.ID = "area" + counter.ToString()
                    area.Attributes.Add("shape", "rect")
                    r = CType( _
                         _imageMapProperties.RectCoords(counter), _
                         Rectangle)
                    Dim coordString As String = r.Left & "," & _
                                                r.Top & "," & _
                                                r.Right & "," & _
                                                r.Bottom
                    area.Attributes.Add("coords", coordString)
                    Dim jscriptString As String = "javascript:" & _
                               Page.GetPostBackEventReference(Me, _
                               (counter + 1).ToString())
                    area.Attributes.Add("href", jscriptString)
                    map.Controls.Add(area)
                Next
                Controls.Add(map)
            End If

        End Sub

        Public Sub RaisePostBackEvent(ByVal eventArgument As String) _
             Implements IPostBackEventHandler.RaisePostBackEvent
            RaiseEvent RegionClicked(Me, _
                             New ImageMapEventArgs(eventArgument))
        End Sub

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            If (Me.Context Is Nothing) Then
                writer.Write(String.Format("<img src='{0}'>", ImageSrc))
            Else
                MyBase.Render(writer)
            End If
        End Sub
    End Class

End Namespace