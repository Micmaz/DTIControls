<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="plumb.aspx.vb" Inherits="JqueryUIControlsTest.plumb" %>

<%@ Register assembly="JqueryUIControls" namespace="JqueryUIControls" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style>
    .window { border:1px solid #346789;
box-shadow: 2px 2px 19px #aaa;
   -o-box-shadow: 2px 2px 19px #aaa;
   -webkit-box-shadow: 2px 2px 19px #aaa;
   -moz-box-shadow: 2px 2px 19px #aaa;
-moz-border-radius:0.5em;
border-radius:0.5em;
opacity:0.8;
filter:alpha(opacity=80);
width:5em; height:5em;
line-height:5em;
text-align:center;
z-index:20; position:absolute;
background-color:#eeeeef;
color:black;
font-family:helvetica;padding:0.5em;
font-size:0.9em;}
.window:hover {

box-shadow: 2px 2px 19px #444;
   -o-box-shadow: 2px 2px 19px #444;
   -webkit-box-shadow: 2px 2px 19px #444;
   -moz-box-shadow: 2px 2px 19px #444;
    opacity:0.6;
filter:alpha(opacity=60);

}

.active {
	border:1px dotted green;
}
.hover {
	border:1px dotted red;
}

#div1 { top:14em;left:2em;}
#div2 { top:15em; left:63em;}
#window3 { top:37em;left:38em; }
#window4 { top:5em; left:28em;}
._jsPlumb_connector { z-index:4; }
._jsPlumb_endpoint { z-index:21;cursor:pointer; }
.hl { border:3px solid red; }
#debug { position:absolute; background-color:black; color:red; z-index:5000 }

.aLabel {
 	background-color:white; 
	padding:0.4em; 
	font:12px sans-serif; 
	color:#444;
	z-index:21;
	border:1px dotted gray;
	opacity:0.8;
	filter:alpha(opacity=80);
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
            <cc1:ThemePicker useCookie="true" ID="ThemePicker1" runat="server">
        </cc1:ThemePicker>    
    <div>
    
        <cc1:jsPlumb ID="jsPlumb1" runat="server">
        <div class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix window" id="div1">Test1</div>
        <div class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix window" id="div2">Test2</div>
        </cc1:jsPlumb>
    
    </div>
    </form>
</body>
</html>
