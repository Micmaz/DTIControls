<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="JqueryUI.ascx.vb" Inherits="DTIAdminPanel.JqueryUI" %>
<%@ Register Assembly="DTIControls" Namespace="DTIAdminPanel" TagPrefix="cc1" %>

<ul class="JqueryUIHorizontal"> 
    <cc1:MenuItem ID="MenuItem1" runat="server">
    <li><a href="##link##">##Name##</a>	
        <hasChildren>
        <ul>
        <cc1:MenuItem ID="MenuItem2" runat="server">
		    <li><a href="##link##">##Name##</a>	</li>
	    </cc1:MenuItem>
	    </ul>
	    </hasChildren>
    </li>
    </cc1:MenuItem>
</ul>