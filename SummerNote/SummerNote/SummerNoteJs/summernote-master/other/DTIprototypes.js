Array.prototype.remove = function(val) {
    var index = this.indexOf(val);
    if(this.indexOf(val) != -1)this.splice(index, 1);
};

String.prototype.trim = function () {
    return this.replace(/^\s*/, "").replace(/\s*$/, "");
}

if(!Array.indexOf){
  Array.prototype.indexOf = function(obj){
      for(var i=0; i<this.length; i++){
          if(this[i]==obj){
              return i;
          }
      }
      return -1;
  }
}

if(!Array.lastIndexOf){
  Array.prototype.lastIndexOf = function(obj){
      for(var i=this.length; i>=0; i--){
          if(this[i]==obj){
              return i;
          }
      }
      return -1;
  }
}

String.prototype.endsWith = function(pattern){
  var d = this.length - pattern.length;
  return d >= 0 && this.indexOf(pattern,d) === d;
};

String.prototype.startsWith = function(pattern){
  return !this.lastIndexOf(pattern,0);
};


jQuery.expr[':'].regex = function(elem, index, match) {
    var matchParams = match[3].split(','),
        validLabels = /^(data|css):/,
        attr = {
            method: matchParams[0].match(validLabels) ? 
                        matchParams[0].split(':')[0] : 'attr',
            property: matchParams.shift().replace(validLabels,'')
        },
        regexFlags = 'ig',
        regex = new RegExp(matchParams.join('').replace(/^\s+|\s+$/g,''), regexFlags);
    return regex.test(jQuery(elem)[attr.method](attr.property));
}

jQuery.fn.encHTML = function() { 
  return this.each(function(){ 
    var me   = jQuery(this); 
    var html = me.html(); 
    me.html(html.replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;')); 
  }); 
}; 
 
jQuery.fn.decHTML = function() { 
  return this.each(function(){ 
    var me   = jQuery(this); 
    var html = me.html(); 
    me.html(html.replace(/&amp;/g,'&').replace(/&lt;/g,'<').replace(/&gt;/g,'>')); 
  }); 
}; 
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
function IsNumeric(input)
{
   return (input - 0) == input && input.length > 0;
};
