<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="JSFileUploader.ascx.vb" Inherits="DTIUploader.JSFileUploader" %>
<script language="javascript">

function selectFiles(){
        $("#uploadFilesLink").fadeIn(300);
}    


</script>
    <div id="uiElements" style="display:inline;">
		<div id="uploaderContainer" runat="Server">
			<div id="uploaderOverlay" style="position:absolute; z-index:2" runat="server"></div>
			<div id="selectFilesLink" class="selectFilesLink" runat="Server" style="z-index:1"><input type="button" runat="server" class="UploaderButtons" id="selectLink" value="Select Files" onclick="return selectFiles();" /></div>
			<br /><br />
        </div>
        <div id="dataTableContainer" runat="server"></div>
		<div id="uploadFilesLink" class="uploadFilesLink" style="visibility:hidden;"><br /><br /><input type="button" runat="server" class="UploaderButtons" id="uploadLink" onClick="upload(); return false;" value="Upload Files" />
		<input type="button" class="UploaderButtons" onClick="handleClearFiles(); return false;" id="clearLink" runat="server" value="Clear" />
                   </div>
    </div>
    