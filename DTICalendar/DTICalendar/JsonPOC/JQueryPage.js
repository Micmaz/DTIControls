// JScript File
$(document).ready(function() {
  // Add the page method call as an onclick handler for the div.
  $(".Today").click(function() {
    var _this = this;
    $.ajax({
      type: "POST",
      url: "##URLKey##?JsonTest=" + $(_this).attr("id"),
      data: "{when:'Today'}",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function(msg) {
        // Replace the div's content with the page method's return.
        $(_this).text(msg);
      },
      error: function(msg){
        var i = 0;
      }
    });
  });
$(".Yesterday").click(function() {
    var _this = this;
    $.ajax({
      type: "POST",
      url: "##URLKey##?JsonTest=" + $(_this).attr("id"),
      data: "{when:'Yesterday'}",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function(msg) {
        // Replace the div's content with the page method's return.
        $(_this).text(msg);
      },
      error: function(msg){
        var i = 0;
      }
    });
  });
$(".Tomorrow").click(function() {
    var _this = this;
    $.ajax({
      type: "POST",
      url: "##URLKey##?JsonTest=" + $(_this).attr("id"),
      data: "{when:'Tomorrow'}",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function(msg) {
        // Replace the div's content with the page method's return.
        $(_this).text(msg);
      },
      error: function(msg){
        var i = 0;
      }
    });
  });
});