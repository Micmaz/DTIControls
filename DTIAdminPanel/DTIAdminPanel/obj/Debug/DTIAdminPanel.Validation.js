// JScript File
function requiredField(id,displayID){
    var x = $('#'+id).val();
    if (x==null || x==""){
        $('#'+displayID).css("display","inline");
        return false;
    }else{
         $('#'+displayID).css("display","none");
         return true;
    }
}

function EmailField(id,displayID){
    var x = $('#'+id).val();
    var atpos=x.indexOf("@");
    var dotpos=x.lastIndexOf(".");
    if (atpos<1 || dotpos<atpos+2 || dotpos+2>=x.length){
        $('#'+displayID).css("display","inline");
        return false;
    }else{
         $('#'+displayID).css("display","none");
         return true;
    }
}

function compareField(id,comparid,displayID){
    var x = $('#'+id).val();
    var x2 = $('#'+comparid).val();
    if (x != x2){
        $('#'+displayID).css("display","inline");
        return false;
    }else{
         $('#'+displayID).css("display","none");
         return true;
    }
}