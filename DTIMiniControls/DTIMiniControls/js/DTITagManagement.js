var currentTagsArray = new Array();
var popularTagsArray = new Array();
var separatorArray = new Array();
var currIdArray = new Array();

$(document).ready(function(){
    for (var currDivId in currentTagsArray)
        for(var index in currentTagsArray[currDivId]) 
            if(isInteger(index) && currentTagsArray[currDivId][index] != '') 
                addCurrentTag(currentTagsArray[currDivId][index], currIdArray[currDivId], true);
    for (var popDivId in popularTagsArray) 
        for(var tag in popularTagsArray[popDivId]) 
            if(isInteger(tag)) addPopularTag(popularTagsArray[popDivId][tag], popDivId);
    $("div[id$='DTICurrTags'] > * > .DTICancelButton").on("click", function(){
        var i = 0;
        for(i=0;i<currentTagsArray.length;i++) {
            if(currIdArray[i] == $(this).closest("div[id$='DTICurrTags']").attr("id")) {
                currentTagsArray[i].remove($(this).next().text());
                $(this).parent().remove();
                break;
            }
        }
    });
    $("div[id$='DTIPopularTags'] > .DTIPopularTag").click(function(){
        addCurrentTag($(this).text(), $(this).parent().parent().prev().attr("id"), false);
    });
 });

function addCurrentTag(tag, addToDivId, staticAdd){
    
    if (staticAdd){
        $("#" + addToDivId).append("<div class=\"DTICurrentTag\"><div class=\"DTICancelButton\">X</div><div class=\"DTITagText\">" + tag + "</div></div>");
    }
    else {
        for (var index in currIdArray) 
            if (currIdArray[index] == addToDivId)
                if(jQuery.inArray(tag, currentTagsArray[index]) < 0) {
                    currentTagsArray[index].push(tag);
                    $("#" + addToDivId).append("<div class=\"DTICurrentTag\"><div class=\"DTICancelButton\">X</div><div class=\"DTITagText\">" + tag + "</div></div>");
                }
    }
}  
      
function addPopularTag(tag, addToDivId){
        $("#" + addToDivId).append("<div class=\"DTIPopularTag\">" + tag + "</div>");
}

function isInteger(s) {
  return (s.toString().search(/^-?[0-9]+$/) == 0);
}

function prepareCurrentTags(){
    var currDivId = 0;
    for (currDivId = 0; currDivId < currentTagsArray.length; currDivId++) {
        var tags = '';
        var index = 0;
        for (index = 0; index < currentTagsArray[currDivId].length; index++) {
            if(currentTagsArray[currDivId][index].toString().trim() != '') {
                tags = tags + separatorArray[currIdArray[currDivId]] + currentTagsArray[currDivId][index];
            }
        }
        $("#" + currIdArray[currDivId] + " input[id$='hfTags']").attr("value",tags.substring(1));
    }
    return true;
}
