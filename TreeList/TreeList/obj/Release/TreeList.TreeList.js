// JScript File
//apply the form submit event
$(document).ready(function(){
    $("form").submit(function() {
        var i;
        var treeids = $.find("input.treelist");
        for (i=0; i < treeids.length; i++)
        {
            elId = treeids[i].id;
            try{
                elId = elId.replace("_treelist", "");
                var selId = "#" + elId + "_hidden";
                var str = $("#" + elId).html();
//                $(selId).val(encode64(str));
                
                var reg = /\<a.*?\<\/a/gi;                
                var rId = /id\=(.*?)\s/i;
                var rParId = /parent\=\"(.*?)\"/i;
                var rTxt = /ins\>(.*?)\<\/a/i;
                var rVal = /key\=\"(.*?)\"/i;
                var myArray = str.match(reg);                
                var j;
                for (j = 0; j < myArray.length; j++) 
                {                        
                    var st = myArray[j];
                    var Id = rId.exec(st)[1];
                    var ParentId  = rParId.exec(st)[1];
                    var Value  = rVal.exec(st)[1];
                    var Text  = rTxt.exec(st)[1];
                    //var CheckState = arr[5];
                    //var Selected = arr[6];
                    //var Expanded = arr[7];
                    //var NodeType = arr[8];
                    
                    $(selId).val($(selId).val() + "[" + Id + ":," + ParentId + ":," + Value + ":," + Text + ":,false:,false:,false:,default]");
                } 
            }
            catch(err){
            }
        }
    });
});

var keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
 
function encode64(input) {
     input = escape(input);
     var output = "";
     var chr1, chr2, chr3 = "";
     var enc1, enc2, enc3, enc4 = "";
     var i = 0;
 
     do {
        chr1 = input.charCodeAt(i++);
        chr2 = input.charCodeAt(i++);
        chr3 = input.charCodeAt(i++);
 
        enc1 = chr1 >> 2;
        enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
        enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
        enc4 = chr3 & 63;
 
        if (isNaN(chr2)) {
           enc3 = enc4 = 64;
        } else if (isNaN(chr3)) {
           enc4 = 64;
        }
 
        output = output +
           keyStr.charAt(enc1) +
           keyStr.charAt(enc2) +
           keyStr.charAt(enc3) +
           keyStr.charAt(enc4);
        chr1 = chr2 = chr3 = "";
        enc1 = enc2 = enc3 = enc4 = "";
     } while (i < input.length);
 
     return output;
  }
  
  
function selectNodes(data, hid){
    var key = data.args[0].getAttribute("key")
    $(hid).val(key + ",");
}

