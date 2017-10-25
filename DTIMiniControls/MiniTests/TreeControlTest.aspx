<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TreeControlTest.aspx.vb" Inherits="MiniTests.TreeControlTest" %>

<%@ Register Assembly="DTIMiniControls" Namespace="DTIMiniControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>DTI Tree Demo</title><script type="text/javascript" src="/res/BaseClasses/Scripts.aspx?d=&amp;f=jQueryLibrary/jquery.min.js">
</script><script type="text/javascript" src="/res/BaseClasses/Scripts.aspx?d=&amp;f=jQueryLibrary/jquery-ui.custom.min.js">
</script><script type="text/javascript" src="/res/BaseClasses/Scripts.aspx?d=&amp;f=jQueryLibrary/DTIprototypes.js">
</script><script type="text/javascript" src="/res/BaseClasses/Scripts.aspx?d=&amp;f=DTIMiniControls/jquery.tree.js">
</script><script type="text/javascript" src="/res/BaseClasses/Scripts.aspx?d=&amp;f=DTIMiniControls/jquery.tree.checkbox.js">
</script><script type="text/javascript" src="/res/BaseClasses/Scripts.aspx?d=&amp;f=DTIMiniControls/jquery.tree.contextmenu.js">
</script>
<link type="text/css" href="http://localhost:3369/res/BaseClasses/Scripts.aspx?f=/res/DTIMiniControls/themes/checkbox/style.css" />
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
    <div id="TreeList1" class="demo">
	
    <ul>
		<li id="TreeList1_1" key="1"><a href="#"><ins>&nbsp;</ins>Root Node 1</a><ul>
			<li id="TreeList1_3" key="2"><a href="#"><ins>&nbsp;</ins>Child Node 1-1</a></li><li id="TreeList1_2" key="3"><a href="#"><ins>&nbsp;</ins>Child Node 1-2</a><ul>
				<li id="TreeList1_4" key="7"><a href="#"><ins>&nbsp;</ins>Grand Child Node 1-1-3</a></li>

			</ul></li><li id="TreeList1_5" key="4"><a href="#"><ins>&nbsp;</ins>Child Node 1-3</a></li>
		</ul></li>
	</ul>
</div><input type="hidden" name="TreeList1_hidden" id="TreeList1_hidden" /><input type="hidden" name="TreeList1_deleted" id="TreeList1_deleted" /><script type="text/javascript">
   $(function () { 
		$("#TreeList1").tree({
			opened : ["TreeList1_1", "TreeList1_2"], 
			ui : {
				theme_name : "checkbox",
				theme_path : "/res/BaseClasses/Scripts.aspx?f=/res/DTIMiniControls/themes"
			},
			plugins : { 
				checkbox : {three_state : true, check_children : true},
			    contextmenu : { 
				    items : { 
				        create:{ label : "Create"},
				        rename:{ label : "Rename"},
				        remove:{ label : "Delete"}
				    }
				}
			},
			rules : {
				use_max_children : false,
                drag_copy: "on",
				multitree :  "all",
				new_node_name : "TreeList1_New"
			},
			callback: {
			    onmove: alertMove,
			    ondelete: deleteYell,
			    oncopy: copyYell
			
			},
			lang : {
				new_node : "New Node"
			},
			types : {
			    "default" : {
				    max_depth : 2,
   				    valid_children : "all"
			    }
			}
		});
	});
</script>
	<input type="button" onclick='showStuff("TreeList1");' value="Traverse" />
    <div style="clear:both; min-height:300px; width:400px; border-style:solid;" id="TreeList1_log">TreeList1:<br /></div>
    </div>
        <div style="float:left; padding: 5px; width:56px;">

            <input type="submit" name="Button1" value="Button" id="Button1" />
        </div>
    <div style="float:left; padding: 5px;">
    <div>
    <div>
    <div id="TreeList2" class="demo">
	
    
</div><input type="hidden" name="TreeList2_hidden" id="TreeList2_hidden" /><input type="hidden" name="TreeList2_deleted" id="TreeList2_deleted" /><script type="text/javascript">
   $(function () { 
		$("#TreeList2").tree({
			opened : [], 
			ui : {
				theme_name : "checkbox",
				theme_path : "/res/BaseClasses/Scripts.aspx?f=/res/DTIMiniControls/themes"
			},
			plugins : { 
				checkbox : {three_state : true, check_children : true},
			    contextmenu : { 
				    items : { 
				        create:{ label : "Create"},
				        rename:{ label : "Rename"},
				        remove:{ label : "Delete"}
				    }
				}
			},
			rules : {
				use_max_children : false,
				multitree :  "all",
				new_node_name : "TreeList2_New"
			},
			callback: {
			    onmove: alertMove,
			    ondelete: deleteYell,
			    oncopy: copyYell
			
			},
			lang : {
				new_node : "New Node"
			},
			types : {
			    "default" : {
				    max_depth : 2,
   				    valid_children : "all"
			    }
			}
		});
	});
</script>
    <input type="button" onclick='showStuff("TreeList2");' value="Traverse" />

	</div>
    </div>
    <div style="clear:both; min-height:300px; width:400px; border-style:solid;" id="TreeList2_log">TreeList2:<br /></div>
    </div>
    </div>

    </form>
</body>
</html>

