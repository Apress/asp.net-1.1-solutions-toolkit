<%@ Control Language="vb" AutoEventWireup="false" Codebehind="FileUploader.ascx.vb" Inherits="FileUploader2" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<p>
		Select a file to upload...
</p>
<p>
		<input id="FileToUpload" style="WIDTH: 281px; HEIGHT: 26px" type="file" size="24" runat="server"
				NAME="FileToUpload">
		<asp:Button id="UploadNow" onclick="UploadNow_Click" runat="server" Height="24px" Width="97px"
				Text="Upload Now"></asp:Button>
</p>
