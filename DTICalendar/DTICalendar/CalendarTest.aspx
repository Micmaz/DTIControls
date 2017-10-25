<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CalendarTest.aspx.vb" Inherits="DTICalendar.CalendarTest" %>
<%@ Register Assembly="DTICalendar" Namespace="DTICalendar" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Calendar Test</title>
    <script type="text/javascript" language="javascript">
	$(document).ready(function() {
	        $('#eventEndTime, #eventEndDate').change(function(){validateTime();});
	        $('#eventStartDate, #eventEndDate').datepicker();
	        $('td.fc-state-default').live("mouseover", function(){
	            $(this).css({'border-width':'1px','border-color':'blue'});
	        });
	        $('td.fc-state-default').live("mouseout", function(){
	            $(this).css({'border-width':'1px 0 0 1px', 'border-color':'#CCCCCC'});
	        });
	        $('#eventStartTime, #eventEndTime').timePicker({show24Hours: false, step:15});
			$('#DTICalendar1').fullCalendar({
			    editable: true,
			    header:{
			        left: 'prev,next',
			        center: 'title',
			        right: 'today month,agendaWeek,agendaDay'
			    },
			    events: "CalendarTest.aspx?DTICalendar=DTICalendar1&action=fetchEvents",
			    eventDrop: function(event) {
                    //updateEvent
			    },
			    eventResize: function(event) {
                    //updateEvent
			    },
			    eventAfterRender: function(calEvent, jsEvent, view){
			        if(calEvent.id == 'tempEventExplode'){
			           $(jsEvent).contextMenu({menu: 'addEventMenu'},
				        function(action, el, pos, _calEvent) {
				            switch(action){
				                case "paste": DTIshowCalEditPanel(_calEvent);break;
				            }
				        }, 
				        calEvent
				    );}
			        else{
					$(jsEvent).contextMenu({menu: 'calendarMenu'},
				        function(action, el, pos, _calEvent) {
				            switch(action){
				                case "edit": DTIshowCalEditPanel(_calEvent);break;
				                case "delete": DTIdeleteEvent(_calEvent);break;
				            }
				        }, 
				        calEvent
				    );}
			    },
			    dayClick: function(dayDate, allDay, jsEvent, view){
			        var calDiv = $(this).parents('div.Yesterday');
			        calDiv.fullCalendar('removeEvents', 'tempEventExplode');
			        switch(view.name){
			            case 'month': 
			                calDiv.fullCalendar('renderEvent', {id:'tempEventExplode', title:'New Event [right-click to create]', start:dayDate});
			                break;
			            case 'agendaWeek': 
			                calDiv.fullCalendar('renderEvent', {id:'tempEventExplode', title:'New Event [right-click to create]', start:dayDate, allDay:false});
			                break;
			            case 'agendaDay':
			                calDiv.fullCalendar('renderEvent', {id:'tempEventExplode', title:'New Event [right-click to create]', start:dayDate, allDay:false});
			                break;
			        }
			    },
			    loading: function(bool) {
				    if (bool) $('#loading').show();
				    else $('#loading').hide();
			    }
		    });
	});
	var curCalEvent;
	function DTIshowCalEditPanel(calEvent){
	    curCalEvent = calEvent;
	    var start = curCalEvent.start;
	    var end = curCalEvent.end;
	    if(curCalEvent.id == 'tempEventExplode'){
	        $('#DTICalDeletebtn').hide();
	    }
	    else{
	        $('#DTICalDeletebtn').show();
	    }
	    $('#eventName').val(curCalEvent.title);
	    $('#eventWhere').val(curCalEvent.where);
	    $('#eventDesc').val(curCalEvent.description);
        $('#eventStartDate').val(getDateString(start));
        $('#eventEndDate').val(getDateString(end?end:start));
	    if(curCalEvent.allDay){
    	    $('#eventAllDay').attr('checked', true);
	        $('#eventStartTime').val('');
	        $('#eventEndTime').val('');
	    }
	    else{
    	    $('#eventAllDay').attr('checked', false);
	        $('#eventStartTime').val(getTimeString(start));
	        $('#eventEndTime').val(end?getTimeString(end):'');
	    }
	    $('#eventView').center();
	}
	function DTIcloseCalEditPanel(){
	    $('#eventView').hide();
	    $('.time-picker').hide();
	}
	function DTIsaveEvent(){
	    if(curCalEvent){
	        if(curCalEvent.id == 'tempEventExplode'){
	            //createEvent
	        }
	        else{
	            //updateEvent
	        }
	    }
	    DTIcloseCalEditPanel();
	}
	function DTIdeleteEvent(calEvent){
	    if(calEvent){
	        if(confirm("Delete Event?")){
	            //deleteEvent
	            DTIcloseCalEditPanel();
	        }
	    }
	    else if(curCalEvent){
	        //deleteEvent
	        DTIcloseCalEditPanel();
	    }
	}
	function getTimeString(oTime){
	    var ap = "AM"
	    var hour = oTime.getHours();
	    if (hour>11){ap="PM";}
        if (hour>12){hour=hour-12;}
        if (!hour){hour=12;}
        return zeroPad(hour) + ":" + zeroPad(oTime.getMinutes()) + " " + ap;
	}
	function getDateString(oTime){
	    try{
	        return zeroPad(oTime.getMonth() + 1) + "/" + zeroPad(oTime.getDate()) + "/" + oTime.getFullYear();
	    }
	    catch(err){return '';}
	}
	function zeroPad(val){
        return (val < 10 ? '0' : '') + val;
	}
	function validateTime(){
	    try{
	        var start = makeDateObject($('#eventStartTime').val(), $('#eventStartDate').val());
	        var end = makeDateObject($('#eventEndTime').val(), $('#eventEndDate').val());
	        timeError($('#eventAllDay').attr('checked')?end<start:end<=start);
	    }
	    catch(err){
	    }
	}
	function makeDateObject(oTime, oDate){
	    var newDate = new Date(oDate);
	    var time = oTime.split(':');
	    try{
	        var hour = time[0];
	        var half = time[1].split(' ')[1];
	        var mins = time[1].split(' ')[0];
	        if (half=='AM'){
	            if(hour==12){hour=0;}
	        }
	        else{
	            hour+=12;
	        }
	        newDate.setHours(hour);
	        newDate.setMinutes(mins);
	    }
	    catch(err){
	    }
	    return newDate;
	}
	function timeError(err){
	    if(err){
	        $('#eventEndTime, #eventEndDate').css('background-color', '#FF8F8F');
	    }
	    else{
	        $('#eventEndTime, #eventEndDate').css('background-color', '');
	    }
	}
    </script>
    <style type="text/css">
    #eventView{
        display:none;
        position:absolute;
        background-color:yellow;
        z-index:9;
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div><div id='loading' style='display:none'>loading...</div>
    <cc1:DTICalendar ID="DTICalendar1" runat="server" CssClass="Yesterday">
    </cc1:DTICalendar>
    <ul id="calendarMenu" class="contextMenu">		
            <li class="edit"><a href="#edit">Edit</a></li>
			<li class="delete"><a href="#delete">Delete</a></li>
    </ul>
    <ul id="addEventMenu" class="contextMenu">
            <li class="paste"><a href="#paste">Create Event</a></li>	
    </ul>
    </div>
    <div id="eventView">
        <table>
        <tr>
        <td>Name
        </td>
        <td><input type="text" style="width: 386px" id="eventName"/>
        </td>
        </tr>
        <tr>
        <td>When
        </td>
        <td><input type="text" style="width: 70px" id="eventStartDate"/>
            <input type="text" style="width: 70px" id="eventStartTime"/>
            to
            <input type="text" style="width: 70px;" id="eventEndTime"/>
            <input type="text" style="width: 70px;"  id="eventEndDate"/>
            <input id="eventAllDay" type="checkbox" />All day</td>
        </tr>
        <tr>
        <td>Where
        </td>
        <td><input type="text" style="width: 386px" id="eventWhere"/>
        </td>
        </tr>
        <tr>
        <td style="height: 136px" valign="top">Description
        </td>
        <td style="height: 136px"><textarea style="width: 386px;height:120px;" id="eventDesc" rows=""></textarea>
        </td>
        </tr>
        <tr>
        <td colspan="2" align="right">
            <input type="button" id="DTICalSavebtn" value="Save" onclick="DTIsaveEvent();"/>
            <input type="button" id="DTICalCancelbtn" value="Cancel" onclick="DTIcloseCalEditPanel();"/>
            <input type="button" id="DTICalDeletebtn" value="Delete" onclick="DTIdeleteEvent();"/>
        </td>
        </tr>
        </table>
    </div>
    </form>
</body>
</html>
