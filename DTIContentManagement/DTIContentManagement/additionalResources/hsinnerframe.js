var hs = {
    expand: function (element, params, custom) {
        parent.window.focus(); // to allow keystroke listening
        parent.window.iframe = getIframe(this);
        //alert(parent.window.location.href);
        //alert(parent.window.iframe);
        return parent.window.hs.expand(element, params, custom);
    },
    htmlExpand: function (element, params, custom) {
        parent.window.focus(); // to allow keystroke listening
        parent.window.iframe = getIframe(this);
        return parent.window.hs.htmlExpand(element, params, custom);
    },
}

function getIframe(el)
{
var myTop;
if (window.frameElement) {
myTop = window.frameElement;
} else if (window.top) {
myTop = window.top;
var myURL = location.href;
var iFs = myTop.document.getElementsByTagName('iframe');
var x, i = iFs.length;
while ( i-- ){
x = iFs[i];
if (x.src && x.src == myURL){
myTop = x;
break;
}
}
}
if (myTop){
return myTop;
} else {
return null;
}
}
