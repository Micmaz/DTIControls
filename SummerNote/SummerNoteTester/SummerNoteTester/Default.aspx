<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="ckeditTester._Default"%> 
<%@ Register Assembly="SummerNote" Namespace="SummerNote" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">

		<script>
			function setupToolbar(toolbar) {
				//toolbar.css("display", "none");
				//toolbar.css("position", "fixed");
				toolbar.css("top", "0px");
				toolbar.css("left", "0px");
				toolbar.css("width", "100%");

				if ($("#lowerbtn").length == 0) {
					var lowerbtn = $("<img id='lowerbtn' style='position:absolute;bottom:0;left:50%;width:59px;' class='lowerbtn' src='~/res/BaseClasses/Scripts.aspx?d=&f=ckEditor/extendup.gif'/>")
					$(toolbar).append(lowerbtn);
					var toolheight = $('#topToolbar').outerHeight(true) - 10;
					lowerbtn.toggle(
						function () {
							$(this).attr("src", "~/res/BaseClasses/Scripts.aspx?d=&f=ckEditor/extend.gif");
							//$(toolbar).animate({ "top": "-=" + toolheight + "px" }, "slow");
							$('#topToolbar').animate({ "top": "-=" + toolheight + "px" }, "slow");
						},
						function () {
							$(this).attr("src", "~/res/BaseClasses/Scripts.aspx?d=&f=ckEditor/extendup.gif");
							if ($('#topToolbar').css("top").replace("px", "") < 0)
								$('#topToolbar').animate({ "top": "+=" + toolheight + "px" }, "slow");

							//if ($(toolbar).css("top").replace("px", "") < 0)
							//    $(toolbar).animate({ "top": "+=" + toolheight + "px" }, "slow");
						});
				}
				$(toolbar).append($("#lowerbtn"));
			}

			var reconnect_count = 0;
			function Reconnect() {
				reconnect_count++;
				window.status = 'Session keepalive sent: ' + reconnect_count.toString() + ' time(s)';
				var img = new Image(1, 1);
				img.src = '~/res/Summernote/keepAlive.aspx';
			}

			function keepAlive() {
				if (reconnect_count == 0) {
					Reconnect();
					window.setInterval('Reconnect()', 120000);
				}
			}

			$(function () {
				keepAlive();
			})

		</script>
    <div>
    <div style="padding-top: 150px; padding-left: 50px;">
<div style="width:600px;background-color:Yellow;">
before<br />
        <cc1:SummerNote ID="SummerNote1" runat="server" ToolbarMode="PageTop">
        </cc1:SummerNote>
    after
    <br />
</div>
        <cc1:SummerNote ID="SummerNote2" runat="server" Height="200px" Width="200px" DivCssClass="divClass" ToolbarMode="PageTop">
        </cc1:SummerNote>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
		<asp:Button ID="Button1" runat="server" Text="Button" />
		Contents:<br />
		<asp:Literal ID="Literal1" runat="server"></asp:Literal>
    </div>
    </div>
    </form>
</body>
</html>