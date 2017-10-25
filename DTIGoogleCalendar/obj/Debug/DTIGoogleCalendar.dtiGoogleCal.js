// JScript File
function _updateEmbed(cId) {
    var source = $("#" + cId + "_iframe").attr("src");
    var base = "/?";
    if ($("#" + cId + "_showTitle:checked").length > 0) {
        source = $.query.load(source).remove("showTitle");
    } else {
        source = $.query.load(source).set("showTitle", 0);
    }
    if ($("#" + cId + "_showNav:checked").length > 0) {
        source = $.query.load(base + source).remove("showNav");
    } else {
        source = $.query.load(base + source).set("showNav", 0);
    }
    if ($("#" + cId + "_showDate:checked").length > 0) {
        source = $.query.load(base + source).remove("showDate");
    } else {
        source = $.query.load(base + source).set("showDate", 0);
    }
    if ($("#" + cId + "_showPrint:checked").length > 0) {
        source = $.query.load(base + source).remove("showPrint");
    } else {
        source = $.query.load(base + source).set("showPrint", 0);
    }
    if ($("#" + cId + "_showTabs:checked").length > 0) {
        source = $.query.load(base + source).remove("showTabs");
    } else {
        source = $.query.load(base + source).set("showTabs", 0);
    }
    if ($("#" + cId + "_showCalendars:checked").length > 0) {
        source = $.query.load(base + source).remove("showCalendars");
    } else {
        source = $.query.load(base + source).set("showCalendars", 0);
    }
    if ($("#" + cId + "_showTz:checked").length > 0) {
        source = $.query.load(base + source).remove("showTz");
    } else {
        source = $.query.load(base + source).set("showTz", 0);
    }
    if ($("#" + cId + "_mode_week:checked").length > 0) {
        source = $.query.load(base + source).set("mode", "WEEK");
    } else if($("#" + cId + "_mode_month:checked").length > 0) {
        source = $.query.load(base + source).remove("mode");
    } else if($("#" + cId + "_mode_agenda:checked").length > 0) {
        source = $.query.load(base + source).set("mode", "AGENDA");
    } else {
        source = $.query.load(base + source).remove("mode");
    }
    $("#" + cId + "_iframe").attr("width", $("#" + cId + "_width").val());
    $("#" + cId).width(parseInt($("#" + cId + "_width").val()));
    var height = $("#" + cId + "_height").val();
    $("#" + cId + "_iframe").attr("height", height);
    $("#" + cId).height(parseInt(height));
    source = $.query.load(base + source).set("height", height);
    source = $.query.load(base + source).set("wkst", $("#" + cId + "_wkst").val());
    source = $.query.load(base + source).set("ctz", $("#" + cId + "_ctz").val());
    if ($("#" + cId + "_title").val() !== "") {
        source = $.query.load(base + source).set("title", $("#" + cId + "_title").val());
    } else {
        source = $.query.load(base + source).remove("title");
    }
    source = $.query.load(base + source).set("src", $("#" + cId + "_calendarId").val());
    $("#" + cId + "_iframe").attr("src", "https://www.google.com/calendar/embed?" + source);
};
function saveGoogleCalendar(cId) {
    var source = $('#' + cId + '_iframe').attr("src");
    var cont_type = $("#" + cId + "_contentType").val();
    var width = $("#" + cId + "_width").val();
    var height = $("#" + cId + "_height").val();
    $.ajax({
      type: "POST",
      url: "~/res/DTIGoogleCalendar/GoogleCalendarEditSettings.aspx/saveGoogleCalendarSettings",
      data: '{"cont_type":"' + cont_type + '","src":"' + source + '","width":"' + width + '","height":"' + height + '"}',
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function(msg) { hs.close(); }
    });
};
function restoreGoogleCalendar(cId) {
    var cont_type = $("#" + cId + "_contentType").val();
    $.ajax({
      type: "POST",
      url: "~/res/DTIGoogleCalendar/GoogleCalendarEditSettings.aspx/restoreGoogleCalendar",
      data: '{"cont_type":"' + cont_type + '"}',
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function(msg) { $("#" + cId + "_iframe").attr("src", msg.d); hs.close();
        var width = parseInt($("#" + cId + "_hiddenWidth").val());
        var height = parseInt($("#" + cId + "_hiddenHeight").val());
        $("#" + cId).width(width);
        $("#" + cId).height(height);
        $("#" + cId + "_iframe").attr("width", width);
        $("#" + cId + "_iframe").attr("height", height);
      }
    });
};
$(document).ready(function () {   
    $.each($("iframe[src*='https://www.google.com/calendar/embed']"), function() {
        var ele = $(this);
        var parent = ele.parent().parent();
        var parentWidth = parent.innerWidth();
        var parentHeight = parent.innerHeight();
        if (ele.outerWidth() > parentWidth) {
            ele.attr("width", parentWidth);
            ele.parent().width(parentWidth);
        }
        if (ele.outerHeight() > parentHeight) {
            ele.attr("height", parentHeight);
            ele.parent().height(parentHeight);
            var source = ele.attr("src");
            ele.attr("src", source + "?" + $.query.load(source).set("height", parentHeight));
        }
        
        var cId = ele.parent().attr("id");
        if ($("#" + cId + "_calendarId").length > 0) {
            var href = $("#" + cId + "_iframe").attr("src")
            $("#" + cId + "_calendarId").val($.query.load(href).get("src"));
            $("#" + cId + "_title").val($.query.load(href).get("title"));
            if ($.query.load(href).get("showTitle") === 0) {
                $("#" + cId + "_showTitle").attr("checked", false);
            }
            if ($.query.load(href).get("showNav") === 0) {
                $("#" + cId + "_showNav").attr("checked", false);
            }
            if ($.query.load(href).get("showDate") === 0) {
                $("#" + cId + "_showDate").attr("checked", false);
            }
            if ($.query.load(href).get("showPrint") === 0) {
                $("#" + cId + "_showPrint").attr("checked", false);
            }
            if ($.query.load(href).get("showTabs") === 0) {
                $("#" + cId + "_showTabs").attr("checked", false);
            }
            if ($.query.load(href).get("showCalendars") === 0) {
                $("#" + cId + "_showCalendars").attr("checked", false);
            }
            if ($.query.load(href).get("showTz") === 0) {
                $("#" + cId + "_showTz").attr("checked", false);
            }
            if ($.query.load(href).get("mode") == "WEEK") {
                $("#" + cId + "_mode_week").attr("checked", true);
                $("#" + cId + "_mode_month").attr("checked", false);
            }
            if ($.query.load(href).get("mode") == "AGENDA") {
                $("#" + cId + "_mode_agenda").attr("checked", true);
                $("#" + cId + "_mode_month").attr("checked", false);
            }
            $("#" + cId + "_width").val($("#" + cId + "_iframe").attr("width"));
            $("#" + cId + "_height").val($("#" + cId + "_iframe").attr("height"));
            $("#" + cId + "_hiddenWidth").val($("#" + cId + "_iframe").attr("width"));
            $("#" + cId + "_hiddenHeight").val($("#" + cId + "_iframe").attr("height"));
            if ($.query.load(href).get("wkst") !== "") {
                $("#" + cId + "_wkst").val($.query.load(href).get("wkst"));
            }
            if ($.query.load(href).get("ctz") !== "") {
                $("#" + cId + "_ctz").val($.query.load(href).get("ctz"));
            } else {
                $("#" + cId + "_ctz").val("America/New_York");
            }
        }
    });
});
