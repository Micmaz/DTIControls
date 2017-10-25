(function($){
    $.fn.dtimenu = function(options){
        var defaults = {
            sensitivity:    3,
            interval:       200,
            showSpeed:      200,
            hideSpeed:      300,
            overCallback:   function(subMenu, parentMenu){},
            outCallback:    function(subMenu, parentMenu){},
            timeout:        350
        };
        var options = $.extend(defaults, options);
        return this.each(function(){
            //this should take a div tag id or a ul id
            var obj = $(this);
            if(obj[0].tagName.toLowerCase() != "ul"){
                //find the ul
                obj = $('ul:eq(0)', obj);
                if(obj[0] === undefined){return false;}
            }
            $('a:eq(0)', obj).addClass('subfolderstyle');
            $('ul', obj).css('left', function(){
                return $($(this).parent('li'))[0].offsetWidth + 'px';
            });
            $('ul ul', obj).each(function(){
                $(this).css('left', function(){
                    return $($(this).parent('li'))[0].offsetWidth + 'px';
                });
            });
            $('li', obj).hoverIntent({
                sensitivity: options.sensitivity,
                interval: options.interval,
                over: function(){
                    $('ul', this).show(options.showSpeed);
                    options.overCallback($('ul', this), $(this));
                },
                out: function(){
                    $('ul', this).hide(options.hideSpeed);
                    options.outCallback($('ul', this), $(this));
                },
                timeout: options.timeout
            });
            $('ul', obj).hide();
        });
    };
})(jQuery);