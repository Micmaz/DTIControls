<%@ Control  Language="vb" AutoEventWireup="false" CodeBehind="ChromeMenu.ascx.vb" Inherits="DTIAdminPanel.ChromeMenu" %>
<%@ Register Assembly="DTIControls" Namespace="DTIAdminPanel" TagPrefix="cc1" %>

<div class="chromestyle" id="chromemenu">
    <ul>
	    <cc1:MenuItem ID="menuitem5" runat="server">
		    <li><a href="##link##"<hasChildren> rel="dropmenu##Id##"</hasChildren>>##Name##</a></li>
		    <hasChildren>
		    <div id="dropmenu##Id##" class="dropmenudiv">
			    <cc1:MenuItem ID="menuitem6" runat="server">
			    <a href="##link##">##Name##</a>					
			    </cc1:MenuItem>
		    </div>
		    </hasChildren>
	    </cc1:MenuItem>
    </ul>
</div>
<script type="text/javascript" language="javascript">//<![CDATA[
     cssdropdown.startchrome("chromemenu");
//]]></script> 


