<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="NewContentManagementTester._Default" %>
<%@ Register Assembly="DTIContentManagement" Namespace="DTIContentManagement" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:CheckBox runat="server" ID="cbEdit" AutoPostBack="true" Text="Edit" /><br /><br /><br /><br />
        <cc1:EditPanel runat="server" id="editPanel1" contenttype="newEPtest"></cc1:EditPanel>
        <cc1:EditPanel runat="server" id="editPanel2" contenttype="newEPtest1"></cc1:EditPanel>
    </div>

    			<div contenteditable="true">
				<p>
					Lorem ipsum dolor sit amet enim. Etiam ullamcorper. Suspendisse a pellentesque dui, non felis. Maecenas malesuada elit lectus felis, malesuada ultricies.
				</p>
				<p>
					Curabitur et ligula. Ut molestie a, ultricies porta urna. Vestibulum commodo volutpat a, convallis ac, laoreet enim. Phasellus fermentum in, dolor. Pellentesque facilisis. Nulla imperdiet sit amet magna. Vestibulum dapibus, mauris nec malesuada fames ac.
				</p>
			</div>

    </form>
</body>
</html>

