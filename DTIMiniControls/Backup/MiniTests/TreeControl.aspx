<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TreeControl.aspx.vb" Inherits="MiniTests.TreeControl" %>

<%@ Register Assembly="DTIMiniControls" Namespace="DTIMiniControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>DTI Tree Demo</title>
<%--	<script type="text/javascript" src="js/jquery.tree.js"></script>
	<script type="text/javascript" src="js/jquery.tree.checkbox.js"></script>
	<script type="text/javascript" src="js/jquery.tree.contextmenu.js"></script>--%>
	<style type="text/css">
	html, body { margin:0; padding:0; }
	body, td, th, pre, code, select, option, input, textarea { font-family:"Trebuchet MS", Sans-serif; font-size:10pt; }
	#container { width:800px; margin:10px auto; overflow:hidden; }
	.demo { height:200px; width:400px; margin:0; border:solid; font-family:Verdana; font-size:10px; background:white; }
	</style>
	<script type="text/javascript">
	function showStuff(elId){
        $("#" + elId + "_deleted").attr("value", $.tree.reference("#" + elId).settings.data.deleted.join(","));
        $("#" + elId + "_hidden").attr("value", "[" + $.tree.reference("#" + elId).get_nodes().join("], [") + "]");
        $("#" + elId + "_log").html(elId + ":<br /><br />[" + $.tree.reference("#" + elId).get_nodes().join("] <br /> [") + "]");
    }
    </script>
	</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div style="float:left; padding: 5px;">
    <cc1:TreeList ID="TreeList1" runat="server" CssClass="demo" EmbeddedThemePath="/res/DTIMiniControls/themes" CheckStyle="Trinary" MultiTreeEnabled="true" MaxDepth="2" ReadModeOnly="false" Drag_Copy="ctrl" No_Int_Copy="true">
    </cc1:TreeList>
	<input type="button" onclick='showStuff("TreeList1");' value="Traverse" />
    <div style="clear:both; min-height:300px; width:400px; border-style:solid;" id="TreeList1_log">TreeList1:<br /></div>
    </div>
        <div style="float:left; padding: 5px; width:56px;">
            <asp:Button ID="Button1" runat="server" Text="Button" />
        </div>
    <div style="float:left; padding: 5px;">
    <div>
    <div>
    <cc1:TreeList ID="TreeList2" runat="server" CssClass="demo" EmbeddedThemePath="/res/DTIMiniControls/themes" CheckStyle="Trinary" MultiTreeEnabled="true" MaxDepth="2" ReadModeOnly="false">
    </cc1:TreeList>
    <input type="button" onclick='showStuff("TreeList2");' value="Traverse" />
	</div>
    </div>
    <div style="clear:both; min-height:300px; width:400px; border-style:solid;" id="TreeList2_log">TreeList2:<br /></div>
    </div>
    </div>
    </form>
</body>
</html>

