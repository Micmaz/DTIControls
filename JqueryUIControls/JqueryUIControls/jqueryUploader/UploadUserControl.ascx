<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UploadUserControl.ascx.vb" Inherits="JqueryUIControls.UploadUserControl" %>
<link href="~/res/BaseClasses/Scripts.aspx?f=JqueryUIControls/style.css" rel="stylesheet" />
        <div id="upload" class="uploadPanel"><div id="currentfiles" style="display:none"><%=fileList %></div>
       <%-- <form id="Form1" method="post" action="upload.php" enctype="multipart/form-data">--%>
			<div id="drop">
				Drop Here

				<a>Browse</a>
				<input type="file" name="upl" multiple />
			</div>

			<ul>
				<!-- The file uploads will be shown here -->
			</ul>
        		<script src="~/res/BaseClasses/Scripts.aspx?f=JqueryUIControls/jquery.knob.js"></script>
		<!-- jQuery File Upload Dependencies -->
		<script src="~/res/BaseClasses/Scripts.aspx?f=JqueryUIControls/jquery.iframe-transport.js"></script>
		<script src="~/res/BaseClasses/Scripts.aspx?f=JqueryUIControls/jquery.fileupload.js"></script>
		<!-- Our main JS file -->
		<script src="~/res/BaseClasses/Scripts.aspx?f=JqueryUIControls/script.js"></script>
		</div>