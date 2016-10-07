<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Upload.aspx.vb" Inherits="Upload2"%>
<%@ Register TagPrefix="cc" TagName="FileUploader" Src="FileUploader.ascx" %>
<HTML>
		<HEAD>
				<title>Demonstration of uploading a file using the FileUploader component </title>
				<script runat="server">
  Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    With FileUploader
      .UploadAction = .Save.ToFile
      .PathForFile = "C:\Test"
      .EmailFromAddress = "postmaster@Apress.com"
      .EmailToAddress = "nonexistent@Apress.com"
    End With
  End Sub
				</script>
		</HEAD>
		<body>
				<form runat="server" ID="Form1">
								<cc:FileUploader id="FileUploader" runat="server"></cc:FileUploader>
				</form>
				<P></P>
		</body>
</HTML>
