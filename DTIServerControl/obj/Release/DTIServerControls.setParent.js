function setParentContent(){
    //parent.$("#" + parentID).replaceWith($("#"+ clientID).val()); 
    //$("#"+ clientID).val("");
    var parentitm = parent.$("#" + parentID());
    var ClientItm = $('#clientCtrl').contents().find('#CONTENTS').children().first();
    var openlink = ClientItm.find("img.serverctrlEditor").parent();
    openlink.replaceWith(parentitm.find("img.serverctrlEditor").parent());    
    ClientItm.attr("id",parentID());
    parentitm.replaceWith(ClientItm);
    unfreezeParent();
}

function getClient(){
    return $('#clientCtrl').contents().find('#CONTENTS').html();
    //return $('#clientCtrl').contents().find('#CONTENTS').children().first().html();
}

function parentID(){
return getParameterByName("parentID");
}

function freezeParent(){
 var pID = parentID();
 //eval("parent.freeze_" + pID + "()"); 
 eval("parent.FreezeIt('" + parentID() + "')"); 
}
function unfreezeParent(){
 eval("parent.unFreezeIt('" + parentID() + "')"); 
}


function getParameterByName( name )
{
  name = name.replace(/[\[]/,"\\\[").replace(/[\]]/,"\\\]");
  var regexS = "[\\?&]"+name+"=([^&#]*)";
  var regex = new RegExp( regexS );
  var results = regex.exec( window.location.href );
  if( results == null )
    return "";
  else
    return decodeURIComponent(results[1].replace(/\+/g, " "));
}

