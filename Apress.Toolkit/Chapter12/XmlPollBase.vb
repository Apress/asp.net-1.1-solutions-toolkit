Imports System
Imports System.Web.UI
Imports System.Data
Imports System.IO
Imports System.Web.UI.WebControls
Imports System.Web.Caching
Imports System.Web

Namespace Apress.Toolkit

    Public Delegate Sub VoteCastEventHandler(ByVal source As Object, _
        ByVal voteDetails As VoteEventArgs)

    Public MustInherit Class XmlPollBase
        Inherits Control

        Protected xmlSet As DataSet

        Private ReadOnly Property CacheString() As String
            Get
                Return Me.UniqueID & "xmlSet"
            End Get
        End Property

        Private _questionStyle As Style = New Style
        Private _tableStyle As TableStyle = New TableStyle
        Private _headerItemStyle As TableItemStyle = New TableItemStyle
        Private _bodyItemStyle As TableItemStyle = New TableItemStyle
        Private _footerItemStyle As TableItemStyle = New TableItemStyle

        Public Property QuestionStyle() As Style
            Get
                Return Me._questionStyle
            End Get

            Set(ByVal value As Style)
                Me._questionStyle = value
            End Set
        End Property

        Public Property TableStyle() As WebControls.TableStyle
            Get
                Return Me._tableStyle
            End Get

            Set(ByVal Value As WebControls.TableStyle)
                Me._tableStyle = Value
                ChildControlsCreated = False
            End Set
        End Property

        Public Property HeaderItemStyle() As TableItemStyle
            Get
                Return Me._headerItemStyle
            End Get

            Set(ByVal value As TableItemStyle)
                Me._headerItemStyle = value
            End Set
        End Property

        Public Property BodyItemStyle() As TableItemStyle
            Get
                Return Me._bodyItemStyle
            End Get

            Set(ByVal value As TableItemStyle)
                Me._bodyItemStyle = value
            End Set
        End Property

        Public Property FooterItemStyle() As TableItemStyle
            Get
                Return Me._footerItemStyle
            End Get

            Set(ByVal value As TableItemStyle)
                Me._footerItemStyle = value
            End Set
        End Property

        Public Property XmlFile() As String
            Get
                If ViewState("xmlFile") Is Nothing Then
                    Return String.Empty
                Else
                    Return CType(ViewState("xmlFile"), String)
                End If
            End Get

            Set(ByVal value As String)
                ViewState("xmlFile") = value
            End Set
        End Property

        Protected Sub LoadXml()
            If Context.Cache(Me.CacheString) Is Nothing Then
                If File.Exists(Me.XmlFile) Then
                    Dim readStream As FileStream
                    Try
                        readStream = New FileStream(Me.XmlFile, FileMode.Open, _
                        FileAccess.Read, FileShare.ReadWrite)
                        Me.xmlSet = New DataSet
                        'SyncLock Me.XmlFile
                        Me.xmlSet.ReadXml(readStream)
                        'End SyncLock 
                        Dim cacheDepend As CacheDependency = _
                                New CacheDependency(Me.XmlFile)
                        Context.Cache.Insert(Me.CacheString, Me.xmlSet, cacheDepend)
                    Finally
                        readStream.Close()
                    End Try
                Else
                    Throw New ArgumentException("The XML data source file does" _
                            & " not exist!")
                End If
            Else
                Me.xmlSet = CType(Context.Cache(CacheString), DataSet)
            End If
        End Sub
    End Class
End Namespace
