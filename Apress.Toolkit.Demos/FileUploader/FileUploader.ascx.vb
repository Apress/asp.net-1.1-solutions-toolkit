Imports System.Web.Mail
Imports System.Web.Mail.SmtpMail
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions


Public Class FileUploader2
    Inherits System.Web.UI.UserControl

    Public Enum Save
        ToFile
        ToDB
        ViaEmail
    End Enum

    Public Enum AuthType
        Windows
        SQLServer
    End Enum
    Private Const MAXFILESIZE As Long = 2097152
    Protected WithEvents UploadNow As System.Web.UI.WebControls.Button
    Protected WithEvents FileToUpload As System.Web.UI.HtmlControls.HtmlInputFile
    Private Const SMTPSERVER As String = "localhost"

    Protected Sub FileUploader_Load(ByVal sender As Object, _
    ByVal e As EventArgs) Handles MyBase.Load
        If Page.Application("FileUploaderLock") Is Nothing Then
            Page.Application.Add("FileUploaderLock", New Object)
        End If
    End Sub
    Dim _uploadAction As Save
    Public Property UploadAction() As Save
        Get
            Return _uploadAction
        End Get

        Set(ByVal value As Save)
            _uploadAction = value
        End Set
    End Property

    Dim _authenticationType As AuthType
    Public Property AuthenticationType() As AuthType
        Get
            Return _authenticationType
        End Get

        Set(ByVal value As AuthType)
            _authenticationType = value
        End Set
    End Property

    Dim _pathForFile As String
    Public Property PathForFile() As String
        Get
            Return _pathForFile
        End Get

        Set(ByVal value As String)
            If value.IndexOfAny(Path.InvalidPathChars) = -1 Then
                _pathForFile = value
            Else
                Throw New ArgumentException("Invalid path name")
            End If
        End Set
    End Property

    Dim _serverName As String
    Public Property ServerName() As String
        Get
            Return _serverName
        End Get

        Set(ByVal value As String)
            If value.IndexOf(" ") = -1 Then
                _serverName = value
            Else
                Throw New ArgumentException( _
                    "ServerName cannot contain any spaces")
            End If
        End Set
    End Property

    Dim _databaseName As String
    Public Property DatabaseName() As String
        Get
            Return _databaseName
        End Get

        Set(ByVal value As String)
            If value.IndexOf(" ") = -1 Then
                _databaseName = value
            Else
                Throw New ArgumentException( _
                    "DatabaseName cannot contain any spaces")
            End If
        End Set
    End Property

    Dim _userId As String
    Public Property UserId() As String
        Get
            Return _userId
        End Get

        Set(ByVal value As String)
            If value.IndexOf(" ") = -1 Then
                _userId = value
            Else
                Throw New ArgumentException( _
                    "UserId cannot contain any spaces")
            End If
        End Set
    End Property

    Dim _password As String
    Public Property Password() As String
        Get
            Return _password
        End Get

        Set(ByVal value As String)
            If value.IndexOf(" ") = -1 Then
                _password = value
            Else
                Throw New ArgumentException( _
                    "Password cannot contain any spaces")
            End If
        End Set
    End Property

    Private Const EMAILPATTERN As String = _
        "[^@()<>;, \x00-\x20]+@[a-z]([a-z0-9-]+\.)+[a-z]{2,4}"

    Dim _emailFromAddress As String
    Public Property EMailFromAddress() As String
        Get
            Return _emailFromAddress
        End Get

        Set(ByVal value As String)
            If Regex.IsMatch( _
                value, "^" & EMAILPATTERN & "$", _
                RegexOptions.IgnoreCase Or _
                RegexOptions.ExplicitCapture) Then
                _emailFromAddress = value
            Else
                Throw New ArgumentException("Not a valid e-mail address")
            End If
        End Set
    End Property

    Dim _emailToAddress As String
    Public Property EmailToAddress() As String
        Get
            Return _emailToAddress
        End Get

        Set(ByVal value As String)
            If Regex.IsMatch( _
                value, "^(" & EMAILPATTERN & "(;\s*" & _
                EMAILPATTERN & ")*)$", RegexOptions.IgnoreCase Or _
                RegexOptions.ExplicitCapture) Then
                _emailToAddress = value
            Else
                Throw New ArgumentException("Must be an (optionally) " & _
                    "semicolon separated list of valid addresses")
            End If
        End Set
    End Property

    Dim _emailSubjectLine As String
    Public Property EmailSubjectLine() As String
        Get
            Return _emailSubjectLine
        End Get

        Set(ByVal value As String)
            _emailSubjectLine = value.TrimEnd()
        End Set
    End Property

    Dim _emailBodyContent As String
    Public Property EmailBodyContent() As String
        Get
            Return _emailBodyContent
        End Get

        Set(ByVal value As String)
            _emailBodyContent = value
        End Set
    End Property

    Sub UploadNow_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Not FileToUpload.PostedFile Is Nothing Then
            Dim fileLen As Integer = FileToUpload.PostedFile.ContentLength
            If fileLen > 0 And fileLen < MAXFILESIZE Then

                Select Case Me.UploadAction

                    Case Save.ToFile
                        Try
                            FileToUpload.PostedFile.SaveAs(Me.PathForFile + "\" + path.GetFileName(FileToUpload.Value))
                        Catch ex As Exception
                            ReportErrorMessage(ex)
                        End Try

                    Case Save.ToDB
                        Try
                            Dim uploadFileStream As Stream
                            uploadFileStream = FileToUpload.PostedFile.InputStream
                            Dim incomingFile(fileLen) As Byte
                            uploadFileStream.Read(incomingFile, 0, fileLen)
                            SaveToDB(incomingFile, FileToUpload.PostedFile.FileName)
                        Catch ex As Exception
                            ReportErrorMessage(ex)
                        End Try

                    Case Save.ViaEmail
                        Dim isLocked As Boolean
                        Dim fileName As String
                        Try
                            Dim path As String = FileToUpload.PostedFile.FileName
                            Dim index As Integer
                            index = path.LastIndexOf("\"c)
                            If index = -1 Then
                                index = path.LastIndexOf("/"c)
                            End If
                            fileName = path.Substring(index + 1)

                            SyncLock Application("FileUploaderLock")
                                isLocked = True
                                If File.Exists(fileName) Then
                                    index = 1
                                    While File.Exists(index.ToString("D3") & fileName)
                                        index += 1
                                    End While
                                    fileName = index.ToString("D3") & fileName
                                End If
                                File.Create(fileName).Close()
                            End SyncLock
                            isLocked = False
                            FileToUpload.PostedFile.SaveAs(fileName)
                            SendEmail(fileName)

                        Catch ex As Exception
                            ReportErrorMessage(ex)
                            If isLocked Then
                                System.Threading.Monitor.Exit( _
                                    Application("FileUploaderLock"))
                            End If
                        Finally
                            If Not fileName Is Nothing Then
                                File.Delete(fileName)
                            End If
                        End Try
                End Select

            Else
                If fileLen = 0 Then
                    ReportErrorMessage(New ApplicationException( _
                        "File is empty"))
                Else
                    ReportErrorMessage(New ApplicationException( _
                        "File is larger than " & _
                        (MAXFILESIZE - 1).ToString() & " bytes long"))
                End If
            End If
        Else
            ReportErrorMessage(New ApplicationException( _
                "No File specified to upload"))
        End If
    End Sub

    Private Sub ReportErrorMessage(ByVal ex As Exception)
        Dim str As String
        Dim exMsg As String
        exMsg = ex.Message.Replace("'", """")
        str = String.Format("<script language='javascript'>{0}</script>", "alert ('An error has been reported:\n" & exMsg & "');")
        Page.RegisterStartupScript(Me.UniqueID, str)
    End Sub

    Private Sub SaveToDB(ByVal fileContents() As Byte, ByVal fileName As String)
        Dim ConnString As New StringBuilder
        ConnString.Append("Data Source=" & ServerName)
        ConnString.Append(";initial catalog=" & DatabaseName)
        ConnString.Append(";")
        If AuthenticationType = AuthType.Windows Then
            ConnString.Append("Integrated Security=SSPI;")
        Else
            ConnString.Append("uid=" & UserId)
            ConnString.Append(";pwd=" & Password & ";")
        End If

        Dim Conn As SqlConnection
        Dim SqlQuery As New StringBuilder
        SqlQuery.Append("INSERT INTO Categories (CategoryName, Description, Picture) ")
        SqlQuery.Append("VALUES('")
        SqlQuery.Append(HttpUtility.UrlEncode(fileName))
        SqlQuery.Append("','Testing ASP.NET FileUploader', @ImageFile)")

        Dim SqlCmd As SqlCommand

        Try
            Conn = New SqlConnection(ConnString.ToString())
            SqlCmd = New SqlCommand(SqlQuery.ToString(), Conn)
            SqlCmd.Parameters.Add(New SqlParameter("@ImageFile", _
                SqlDbType.Image)).Value = fileContents
            Conn.Open()
            SqlCmd.ExecuteNonQuery()
        Finally
            If Not Conn Is Nothing Then
                Conn.Close()
                Conn.Dispose()
            End If
            If Not SqlCmd Is Nothing Then
                SqlCmd.Dispose()
            End If
        End Try
    End Sub

    Private Sub SendEmail(ByVal fileName As String)
        Dim emailMessage As New MailMessage
        With emailMessage
            .From = Me.EMailFromAddress
            .To = Me.EmailToAddress
            .Subject = Me.EmailSubjectLine
            .Body = Me.EmailBodyContent

            Dim MailAttach As New MailAttachment(fileName)

            .Attachments.Add(MailAttach)
        End With

        Try
            System.Threading.Monitor.Enter(GetType(SmtpMail))
            SmtpMail.SmtpServer = SMTPSERVER
            SmtpMail.Send(emailMessage)
        Finally
            System.Threading.Monitor.Exit(GetType(SmtpMail))
        End Try
    End Sub
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub UploadNow_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles UploadNow.Click

    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If Not (MyBase.Page Is Nothing) Then
            MyBase.Page.VerifyRenderingInServerForm(Me)
        End If
        MyBase.Render(writer)
    End Sub

End Class

