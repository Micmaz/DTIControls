// JScript File
var borderstyle
function editorOn(divid){
$('#'+divid).parent().parent().find(' >*:last-child img').css('visibility','hidden'); 
borderstyle = $('#'+divid).parent().parent().css('border');
$('#'+divid).parent().parent().css('border','')
}

function editorOff(divid){
$('#'+divid).parent().parent().find(' >*:last-child img').css('visibility',''); 
$('#'+divid).parent().parent().css('border',borderstyle);
}