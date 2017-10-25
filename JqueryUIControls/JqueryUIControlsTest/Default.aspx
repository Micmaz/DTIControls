<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="JqueryUIControlsTest._Default" %>
<%@ Register Assembly="JqueryUIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <%--<script type='text/javascript' src='js/jquery.scrollTo-min.js'></script>--%>
    <%--<link href="style.css" rel="stylesheet" type="text/css" />--%>
        <style type="text/css" >

    </style>

</head>
<body class="ui-widget1">


    <script type="text/javascript" language="javascript">





/*   $(function(){
   $('<div></div').load("http://www.google.com", function() {
    var container = $(this);
    container.dialog({})
    }); 
    }); */
</script>
<script type="text/javascript" language="javascript">
function checkall(checked){
   
       $( "#cb1").prop('checked', checked);
       $( "#cb2").prop('checked', checked);
       $( "#cb3").prop('checked', checked);
       $( "#cb4").prop('checked', checked);
      
        $('input:checkbox').checkbox();
      }
$(function () {
    $("#clickme").click(function () { alert('hi'); });

    //var not1 = chromeNotificationHTML("Default.aspx?renderNotify=0", function() { alert("clicked") });

  });

  /*
function openDialogHelper(item, elementID, URL) {
    item.html($("<iframe id='" + elementID + "_iframe' frameborder='0' " +
            "marginWidth='0' marginHeight='0' ALLOWTRANSPARENCY='true' scrolling='auto' " +
            "width='100%' height='100%' background='transparent' style='display:none;' " +
            "src='" + URL + "' />"));
    $('#' + elementID + '_iframe').load(function () {
        $(this)[0].style.display = 'block';
        //$(this).hide().fadeIn(500);
        //$(this).contents().find('body').show().css('display', 'block');
        $(this).get(0).contentWindow.window.onbeforeunload = function () {
            $('#' + elementID + '_iframe').fadeOut(500, function () {
                $(this).css('display', 'none');
            });
        };
    });
}

function createDlg(URL, title, height, width, modal, id, Buttons) {
    if (window != window.top) {
        parent.createDlg(URL, title, height, width, modal, id, Buttons);
        return;
    }
    if (!title) title = "";
    if (!width) width = 400;
    if (!height) height = 400;
    if (!id) id = "dynamicDiv_" + URL.hashCode();

    if ($('#' + id).length == 0) {
        $($('form')[0]).append("<div id='" + id + "' Title='" + title + "' style='height:" + height + "px;width:" + width + "px;'></div>");
        //openDialogHelper($('#' + id), id, URL);
        $('#' + id).dialog({
            autoOpen: false,
            open: function (event, ui) {
                openDialogHelper($(this), id, URL);  //Leave it in here this is called even befor the animation has finished.
                $('#' + id + '_iframe').load(function () { addButtons($('#' + id), Buttons); });
            },
            buttons: {},
            modal: modal,
            width: width,
            height: height,
            show: "fade",
            hide: "fade"
        });
    }
    //$('#' + id).appendTo($('form:first'));
    $('#' + id).dialog('open').dialogExtend({ 'dblclick': 'maximize' });
}
*/
</script>
    <form id="form1" runat="server">
     <cc1:ThemePicker useCookie="true" DefaultTheme="dot_luv" Visible="true" ID="ThemePicker1" runat="server"/>

    <cc1:Dialog ID="Dialog2" OpenerText="open Dialog" OpenerType="Button" runat="server">
        <span id="clickme">Click me!</span>
         <cc1:colorpicker color="#66ff99" runat="server" minView="false" ID="colorpicker2"></cc1:colorpicker>
    </cc1:Dialog>
    <asp:RadioButton Text="test1" ID="RadioButton1" runat="server" /><br />
    <asp:RadioButton Text="test2" ID="RadioButton2" runat="server" /><br />
    <asp:RadioButton Text="test2" ID="RadioButton3" runat="server" /><br />
    <asp:RadioButton Text="test2" ID="RadioButton4" runat="server" /><br />
    <asp:RadioButton Text="test2" ID="RadioButton5" runat="server" /><br />
    <div>
      <cc1:Uploader ID="testul" runat="server" />
        Masked Textbox: <cc1:maskedTextbox ID="tbMasked" runat="server" maskPreset="Phone"></cc1:maskedTextbox>
        <a href="javascript:void(0)" onclick="requestNotifyPermissions();">permissions</a>
        <a href="javascript:void(0)" onclick="createDlg('ContentFrame.aspx?asd=1');">dlg Link1</a>
        <a href="javascript:void(0)" onclick="createDlg('ContentFrame.aspx','title',null,null,true,null,{'#Button1':function(){alert('hi');return true;},'test':function(){alert('hi');return true;}});">dlg Link</a>
        <a href="javascript:void(0)" onclick="addButtons('#dynamicDiv_-1097950085',{'#Button1':function(){alert('hi');return true;},'test':function(){alert('hi');return true;}});">Add buttons</a>
        
    checkall <input onclick="checkall($(this).prop('checked'));" type="checkbox" id="cball" name="cball" value="" />
    <input type="checkbox" id="cb1" value="" />
    <input type="checkbox" id="cb2" value="" />
    <input type="checkbox" id="cb3" value="" />
    <input type="checkbox" id="cb4" value="" />        
        <asp:Button ID="Button1" runat="server" Text="Button" /><asp:CheckBox ID="CheckBox1"
            runat="server" />
        <a href="" onclick="createDialogURL('SliderTest.aspx',500,500,'test1aaaa','testTitle');return false;">dynamic</a>&nbsp;
        <a href="" onclick="$('#dlg1').dialog( 'open' );return false;">open</a>&nbsp;
        <a href="" onclick="$('#dlg2').dialog( 'open' );return false;">open2</a>&nbsp;
        <a href="AutoCompleteTest.aspx?id=4">AutoCompleteTest.aspx</a><cc1:AjaxCall 
            ID="timerCall1" runat="server"/>
        <asp:Panel ID="pnl1" runat="server">
        stuff here
        </asp:Panel>
        <cc1:AjaxCall renderControlsBack="true" ID="ajax1" javascriptCallTimer="5000" runat="server"/>
&nbsp;<cc1:InfoDiv id="InfoDiv1" runat="server">
        test Some Info
        </cc1:InfoDiv>
        <input type="button" onclick="Notify.requestPermission(); return false;" value="Set Permissions" />
        <asp:TextBox BackColor="Red" BorderColor="Red" ID="TextBox1" Text="AAA" runat="server"></asp:TextBox>
        <cc1:AjaxCall renderControlsBack="true" ID="JsAjaxCall1" jsReturnFunction="returncall" runat="server"></cc1:AjaxCall>
        <input id="Button2" type="button" onclick="JsAjaxCall1('send stuff');" 
            value="AjaxCall, send message to other browsers" />
            
<script type="text/javascript" language="javascript">
    function returncall(msg) {
        //jqalert(msg);
$('button, input:submit, input:button, input:reset').button();
};
</script>


        <asp:DropDownList ID="DropDownList1" runat="server">
        </asp:DropDownList>
        <br />
        Date picker: <cc1:DatePicker value="" ID="DatePicker1" runat="server"></cc1:DatePicker>
        <br />
        <cc1:InfoDiv isError="true" id="ErrorDiv1" runat="server">
        test an error<br /><br /><br />asdfasdf
        </cc1:InfoDiv>
        &nbsp;
        <cc1:Dialog ID="dlg1" showEffect="highlight" Title="test" url="contentframe.aspx" runat="server" openerText="openme1"></cc1:Dialog>&nbsp;
        <cc1:Dialog ID="dlg2" showEffect="highlight" Title="test" url="contentframe2.aspx" runat="server" openerText="openme2"></cc1:Dialog>&nbsp;
        <cc1:Dialog ID="Dialog1" Title="test" runat="server" AutoOpen="true">Stuff in here</cc1:Dialog>&nbsp;
        <a href="javascript:void();" onclick="$('#Dialog3').dialog( 'open' );">Click link</a>
        <cc1:Dialog ID="Dialog3" Title="test2"  runat="server">More Stuff in here</cc1:Dialog>&nbsp;
        <cc1:Accordion ID="Accordion1" runat="server">            
            <Panes>
                <cc1:AccordionPane ID="AccordionPane1" runat="server" Header="test">
                    test
                </cc1:AccordionPane>
                <cc1:AccordionPane ID="AccordionPane2" runat="server" Header="test2">
                    test
                </cc1:AccordionPane>
            </Panes>
        </cc1:Accordion>
        
        </div>
        <input type="button" value="Test jqalert" onclick="jqalert('test', 'test title')" />
        <input type="button" value="Test jqConfirm" onclick="jqconfirm('test', 'test title', jqalert('ok'), jqalert('not ok'))" />
        <cc1:colorpicker color="#66ff99" runat="server" minView="false" ID="colorpicker1"></cc1:colorpicker>

        
        
         ComboBox:
                 <cc1:ComboBox ID="ComboBox1" runat="server">
            <asp:ListItem>test</asp:ListItem>
            <asp:ListItem>Test2</asp:ListItem>
            <asp:ListItem>test3</asp:ListItem>
            <asp:ListItem>asdf</asp:ListItem>
            <asp:ListItem>1234</asp:ListItem>
        </cc1:ComboBox>
        <br />
                <cc1:ProgressBar ID="ProgressBar1" Value="46" runat="server"/>
        <br />
    <div>
        <cc1:TimePicker id="TimePicker1" runat="server" HourGrid="5" MinuteGrid="10" StepMinute="5"></cc1:TimePicker>
        <asp:Button ID="Button4" runat="server" Text="Button" />
        <cc1:TimePicker id="TimePicker2" runat="server" ShowTimeAndDate="true" ButtonImageOnly="true" MaxDate="0"></cc1:TimePicker>
        <br />
        <cc1:TimePicker ID="tpTimeIn" runat="server" Clock24Hour="true"></cc1:TimePicker>
        </div>
        <br />

            <div>
     <cc1:ToolTip ID="ToolTip3" OpenEffect="fadeIn" runat="server" Text="Mouseover Link">
    bare minium
    </cc1:ToolTip>
    
    <br /><asp:Label ID="Label1" runat="server">Mouse Over this too. </asp:Label>Other content
    <br />Other contentOther contentOther contentOther contentOther content


     <cc1:ToolTip ID="ToolTip7" runat="server"  TargetControlID="Label1" Sticky="true" title="My Title" ClosePosition="title" CloseText="X" Arrows="true" >
        Test TEst TEst
       <asp:Button ID="Button5" runat="server" Text="Button" />
    </cc1:ToolTip>

    </div>


         <cc1:Tabs ID="tabs1" runat="server">
        
         <Tabs>
            <cc1:Tab id="tab1" runat="server" title="test1">
                <span>hhhh</span>test1
                <asp:Button ID="Button3" runat="server" Text="Button" />
            </cc1:Tab>
            <cc1:Tab ID="tab2" runat="server" Title="test2">
                test 2
            </cc1:Tab>
         </Tabs>
         </cc1:Tabs>
        <br />
                <cc1:Sortable ID="Sortable2" runat="server">
            <div>test</div>
            <div>test</div>
        </cc1:Sortable>
        <br />
                <div id="tpBody">
            <span style="float:left">01</span>
            <cc1:Slider id="slHrs" runat="server" Value="6" Min="1" Max="12" style="margin-left:8px;margin-top:4px;margin-bottom:20px;width:180px;float:left"></cc1:Slider>         
            <span>12</span>
            <br style="clear:both;" />
            <span style="float:left">00</span>
            <cc1:Slider id="slMin" runat="server" Max="55" Step="5" style="margin-left:8px;margin-top:4px;width:180px;float:left"></cc1:Slider>
            <span>55</span>
            <br style="clear:both;" />
        </div>
        <br />
        <cc1:Calendar MonthViewLimit="3" ajaxLoad="true" runat="server" DefaultView="month" ID="cal1"></cc1:Calendar> 

        <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
        Date picker: <cc1:DatePicker value="" ID="DatePicker2" runat="server"></cc1:DatePicker>
        <br /><br /><br /><br /><br /><br /><br /><br /><br />
        <br /><br /><br />
    </form>
</body>
</html>
