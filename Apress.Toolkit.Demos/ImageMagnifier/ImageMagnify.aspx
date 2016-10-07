<%@ Page Language="VB" %>
<%@ Register TagPrefix="cc" Namespace="Apress.Toolkit" Assembly="Apress.Toolkit" %>
<HTML>
		<HEAD>
		</HEAD>
		<body>
				<form runat="server" ID="Form1">
						<h1>Image Magnifier Test Page
						</h1>
						<h1>
								<cc:ImageMagnifier id="ImageMagnifier1" runat="server" Height="252px" Width="240px" TempPath="images"
										ImageUrl="photo.jpg"></cc:ImageMagnifier>
						</h1>
				</form>
		</body>
</HTML>
