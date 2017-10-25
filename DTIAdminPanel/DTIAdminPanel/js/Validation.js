// JScript File
function requiredField(id,displayID){
    var x = $('#'+id).val();
    if (x==null || x==""){
        $('#' + displayID).css("visibility", "visible");
        return false;
    }else{
        $('#' + displayID).css("visibility", "hidden");
         return true;
    }
}

function EmailField(id,displayID){
    var x = $('#'+id).val();
    var atpos=x.indexOf("@");
    var dotpos=x.lastIndexOf(".");
    if (atpos<1 || dotpos<atpos+2 || dotpos+2>=x.length){
        $('#' + displayID).css("visibility", "visible");
        return false;
    }else{
        $('#' + displayID).css("visibility", "none");
         return true;
    }
}

function compareField(id,comparid,displayID){
    var x = $('#'+id).val();
    var x2 = $('#'+comparid).val();
    if (x != x2){
        $('#' + displayID).css("visibility", "visible");
        return false;
    }else{
        $('#' + displayID).css("visibility", "hidden");
         return true;
    }
}