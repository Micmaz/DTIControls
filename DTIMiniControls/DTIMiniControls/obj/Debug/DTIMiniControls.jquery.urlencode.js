/**  
    This takes a select but only allows for the first object to perform
    the task
    Dependencies: base64.js
*/
(function($){	
	var pairs = {};
	var encoded = false;
	var registered = false;
		
	$.extend({
		urlEncode: function(input) {
		    var str=input;
		    str=escape(str);
            str=str.replace(new RegExp('\\+','g'),'%2B');
            return str.replace(new RegExp('%20','g'),'+');
		},
		urlDecode: function(input) {
		    var str=input;
            str=str.replace(new RegExp('\\+','g'),' ');
            return unescape(str);
		},
	    urlAddWatch: function(_src, _dest)
        {
            if(!registered && $('form').length){
                $('form').submit(function() {
                    $.urlEncodePairs();
                    return true;
                });
                registered = true;
            }
            var src = {};
            var dest = {};
            if($('#' + _src).length && $('#' + _dest).length){
                var src = $('#' + _src)[0];
                var dest = $('#' + _dest)[0];
            }
            else{
                return false;
            }
            pairs['#' + _src] = dest;
        },
        urlEncodePairs: function()
        {
            if(!encoded){
                $.each(pairs, function(src, dest){
                    if($(src)[0].tagName.toLowerCase() == 'input' || $(src)[0].tagName.toLowerCase() == 'textarea'){
                        $(dest).val($.urlEncode($(src).val().replace('…', '&hellip;')));
                    }
                    else{
                        $(dest).val($.urlEncode($(src).html().replace('…', '&hellip;')));
                    }
                });
                encoded = true;
            }
        }
	});
})(jQuery);