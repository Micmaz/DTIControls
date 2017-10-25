function addvalueToAutoComplete(elementID,autocompleteName,parmName,checked){
    var src=$("#"+autocompleteName).autocomplete('option','source');
    src=addvalueToSrc(elementID,parmName,src);
    $("#"+autocompleteName).autocomplete('option','source',src);
}

function addvalueToSrc(elementID,parmName,src,checked){
        if(!parmName)parmName=elementID;
        if(!checked)checked=false;
        src+="&";
        var re = new RegExp("\&"+parmName+"\=.*?\&","g");
        src=src.replace(re,"&");
        if (!checked)
            src += parmName + "=" + escapestr($("#" + elementID).val());
        else
            src+=parmName+"="+$("#"+elementID).prop("checked");
        return src;
    }

    function htmlEncode(value) {
        return $('<div/>').text(value).html();
    }

    function htmlDecode(value) {
        return $('<div/>').html(value).text();
    }

    function escapestr(str) {
        //return htmlEncode(str);
        str = encodeURIComponent(str);

        //str=str.replace(new RegExp('\\+','g'),'%2B'); 
        //str=str.replace(new RegExp('\\/','g'),'%2F');
        //str = str.replace(new RegExp('%20', 'g'), '+');
        str = encodeURIComponent(str);
        return str;
    }