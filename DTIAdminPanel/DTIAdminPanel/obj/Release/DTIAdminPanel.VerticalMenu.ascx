<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="VerticalMenu.ascx.vb" Inherits="DTIAdminPanel.VerticalMenu" %>
<%@ Register Assembly="DTIAdminPanel" Namespace="DTIAdminPanel" TagPrefix="cc1" %>

<div class="verticalmenu" id="verticalmenu">
	<ul>
	    <cc1:MenuItem ID="menuitem1" runat="server">
	        <li><a href="##link##">##Name##</a>
	        <hasChildren>
	    		<ul class="verticalSubMenu">
	    		    <cc1:MenuItem ID="menuitem2" runat="server">
		            <li><a href="##link##">##Name##</a></li>
		            </cc1:MenuItem>
		        </ul>
	        </hasChildren>
	        </li>
	    </cc1:MenuItem>
	</ul>
</div>