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

function addLoadEvent(func) {
  var oldonload = window.onload;
  if (typeof window.onload != 'function') {
    window.onload = func;
  } else {
    window.onload = function() {
      if (oldonload) {
        oldonload();
      }
      func();
    }
  }
};
