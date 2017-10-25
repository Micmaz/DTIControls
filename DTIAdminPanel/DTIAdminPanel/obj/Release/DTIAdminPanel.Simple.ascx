<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Simple.ascx.vb" Inherits="DTIAdminPanel.Simple" %>
<%@ Register Assembly="DTIAdminPanel" Namespace="DTIAdminPanel" TagPrefix="cc1" %>
<ul>
    <cc1:MenuItem ID="menuitem1" runat="server">
        <li><a href="##link##">##Name##</a></li>
        <hasChildren>
            <ul>
		        <cc1:MenuItem ID="menuitem2" runat="server">
		            <li><a href="##link##">##Name##</a></li>
		            <hasChildren>
                        <ul>
		                    <cc1:MenuItem ID="menuitem3" runat="server">
		                        <li><a href="##link##">##Name##</a></li>
		                            <hasChildren>
                                        <ul>
		                                    <cc1:MenuItem ID="menuitem4" runat="server">
		                                        <li><a href="##link##">##Name##</a></li>
		                                            <hasChildren>
                                                        <ul>
		                                                    <cc1:MenuItem ID="menuitem5" runat="server">
		                                                        <li><a href="##link##">##Name##</a></li>					
		                                                    </cc1:MenuItem>
		                                                </ul>
		                                            </hasChildren>					
		                                    </cc1:MenuItem>
		                                </ul>
		                            </hasChildren>					
		                    </cc1:MenuItem>
		                </ul>
		            </hasChildren>					
		        </cc1:MenuItem>
		    </ul>
		</hasChildren>
    </cc1:MenuItem>
</ul>