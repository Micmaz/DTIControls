<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RotatorStart.aspx.vb" Inherits="DTIContentManagementTester.RotatorStart" %>

<%@ Register Assembly="ckEditor" Namespace="ckEditor" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Rotator Interface Development</title>
    <link rel="stylesheet" type="text/css" href="/css/editrotator.css" />
    <script type="text/javascript" src="/js/editrotator.js" language="javascript"></script>
    <script type="text/javascript" src="/js/jquery.cycle.all.2.72.js" language="javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function(){
            $('.outer').editrotator();
            
            //alert('hi');
            
            $('.outer').cycle({ 
                fx:     'scrollHorz', 
                timeout: 0,
                next:   '.outer ~ div > .editrotatornext',
                prev:   '.outer ~ div > .editrotatorprev'
            });
        });  
        
        function showHTML(){
            $('#htmlshow').html($('.outer').html());
            $('#htmlshow > div').show();
        }
    </script>
    <style type="text/css">
        .editrotatorwrapper{
            border:1px solid red;
        }
        .editrotatornav{
            border:1px solid blue;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <cc1:ckEditor ID="EditPanel1" runat="server">
        </cc1:ckEditor>    
<div class="outer">
<div class="rotator">
	<div>

		Vetsuite</div>
	<div>
		<img src="/images/logoroundededges.gif"></div>
	<p>
		Vetsuite is a content management web application used by thousands of veterinarian offices nationwide.<br>
		<a class="link1" href="/res/DTILoggedInControl/Page.aspx?pagename=vetsuite"> Read more...</a></p>
</div>
<div class="rotator">
	<div>
		EDC LAN Locking System</div>
	<div>
		<img src="/images/edclogo.gif"></div>
	<p>
		This is a full scale software and hardware access control system for hospitals nationwide. Currently over 500 units of the hardware are in hospitals.<br>
		<a class="link1" href="/res/DTILoggedInControl/Page.aspx?pagename=edc"> Read more...</a></p>

</div>
<div class="rotator" id="yumyum">
	<div>
		Powervet</div>
	<div>
		<img src="/images/pvlogo.gif"></div>
	<p>
		Powervet is a e-commerce site used by veterinarian offices to order pet pharmaceuticals.<br>
		<a class="link1" href="/res/DTILoggedInControl/Page.aspx?pagename=powervet"> Read more...</a></p>

</div>
<div class="rotator">
	<div>
		RemindMyPet</div>
	<div>
		<img src="/images/rmplogoround.gif"></div>
	<p>
		RemindMyPet is a service provided to help everyday pet owners remember to administer medications to their pets.<br>
		<a class="link1" href="/res/DTILoggedInControl/Page.aspx?pagename=remindmypet"> Read more...</a></p>

</div>
<div class="rotator">
	<div>
		COPE NC</div>
	<div>
		<img src="/images/copelogo.gif"></div>
	<p>
		COPE NC is a start up venture to begin providing psychological aid to online patients.<br>
		<a class="link1" href="/res/DTILoggedInControl/Page.aspx?pagename=cope"> Read more...</a></p>

</div>
<div class="rotator">
	<div>
		Vet Retriever</div>
	<div>
		<img src="/images/VetRetrieverLogo.gif"></div>
	<p>
		Vet Retriever helps online visitors find local veterinarian offices.<br>
		<a class="link1" href="/res/DTILoggedInControl/Page.aspx?pagename=vetretriever"> Read more...</a></p>

</div>
<div class="rotator">
	<div>
		QuizPoll</div>
	<div>
		<img src="/images/quizpolllogo.gif"></div>
	<p>
		Quizpoll is a web application that allows users to develop custom quizzes and records answers from quiz takers.<br>
		<a class="link1" href="/res/DTILoggedInControl/Page.aspx?pagename=quizpoll"> Read more...</a></p>

</div>


    </div>
    <input type="button" value="Show" onclick="javascript:showHTML();"/>
    <div id="htmlshow"></div>
    </form>
</body>
</html>
