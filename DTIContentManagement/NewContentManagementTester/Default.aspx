<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="NewContentManagementTester._Default" %>
<%@ Register Assembly="DTIControls" Namespace="DTIContentManagement" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="~/res/BaseClasses/Scripts.aspx?d=&f=DTIContentManagement/jquery.lightbox-0.5.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function calllightbox(elm, cat) {
            $(elm).find('.' + cat + ' a.lightbox').fancybox();
        }
    </script>
<%--    <script src="DTIckEditorLOCAL.js" type="text/javascript"></script>--%>
</head>
<body>
    <form id="form1" runat="server">
    <style type="text/css">.hoverArea>a{background-color:Gray;}</style>
    <div>
        <asp:CheckBox runat="server" ID="cbEdit" AutoPostBack="true" Text="Edit" /><br /><br />
		<br /><br />
             <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>       
        
        <br /><br />
        <cc1:EditPanel runat="server" id="editPanel1" contenttype="maaaaaa"></cc1:EditPanel>
<%--        <cc1:EditPanel runat="server" id="editPanel3" contenttype="newEPtest11a">

				
        </cc1:EditPanel>
        <br /><br />

        <input type="button" value="show" onclick="$(this).next().fadeToggle();" />
        <div style="height:300px;width:400px;display:none;margin-left: 300px;">
        <cc1:EditPanel runat="server" id="editPanel2" contenttype="newEPtest1" ToolbarMode="Normal" ></cc1:EditPanel>
--%>
</div>

    </div>



    </form>
</body>
</html>

