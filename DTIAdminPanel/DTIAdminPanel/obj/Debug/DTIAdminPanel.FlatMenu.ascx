<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="FlatMenu.ascx.vb" Inherits="DTIAdminPanel.FlatMenu" %>
<%@ Register Assembly="DTIAdminPanel" Namespace="DTIAdminPanel" TagPrefix="cc1" %>

<%--<cc1:MenuItem ID="menuitem12" name="Login" link="/Login.aspx" runat="server"></cc1:MenuItem>
<cc1:MenuItem ID="menuitem1" name="Default" link="/Default.aspx" AdminOnly="true" runat="server"></cc1:MenuItem>
--%>
    <ul>
	    <cc1:MenuItem ID="menuitem1" runat="server">
		    <li><a href="##link##">##Name##</a>	</li>
		    <hasChildren>
		        <cc1:MenuItem ID="menuitem2" runat="server">
		            <li><a href="##link##">##Name##</a></li>
            		    <hasChildren>
                            <cc1:MenuItem ID="menuitem3" runat="server">
                                <li><a href="##link##">##Name##</a></li>
            		                <hasChildren>
                                        <cc1:MenuItem ID="menuitem4" runat="server">
                                            <li><a href="##link##">##Name##</a></li>
            		                            <hasChildren>
                                                    <cc1:MenuItem ID="menuitem5" runat="server">
                                                        <li><a href="##link##">##Name##</a></li>					
                                                    </cc1:MenuItem>
                                                </hasChildren>					
                                        </cc1:MenuItem>
                                    </hasChildren>					
                            </cc1:MenuItem>
                        </hasChildren>	                        				
		        </cc1:MenuItem>
		    </hasChildren>
	    </cc1:MenuItem>
    </ul>