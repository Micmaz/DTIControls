(function($) {

    $.fn.textbox = function(type) {
        return this.each(function() {
            var $this = $(this);
            if ($this.attr("uithemed") != "uithemed") {
                var bg;
				if($this.attr("style") && $this.attr("style").toLowerCase().indexOf("background") != -1 ){
					if ($this.css("background")) bg = $this.css("background");
				}
                $this.addClass('ui-corner-all ui-widget-content');
                $this.attr("uithemed", "uithemed");
                if (bg) $this.css("background", bg);
            }
        });
    };

    $.fn.untextbox = function(type) {
        return this.each(function() {
            var $this = $(this);
            if ($this.attr("uithemed") == "uithemed") {
                $this.removeAttr("uithemed");
                $this.removeClass('ui-corner-all ui-widget-content');
            }
        });
    };

	$.fn.unbutton = function(type) {
        return this.each(function() {
            var $this = $(this);
            //if ($this.attr("uithemed") == "uithemed") {
            //    $this.removeAttr("uithemed");
                $this.removeClass('ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only ui-button-disabled ui-state-disabled');
            //}
        });
    };

    var checkboxchangeFunction = function() {
        $($(this).next().children()[0]).toggleClass("ui-icon-check", $(this).prop('checked'));
        $($(this).next().children()[0]).toggleClass("ui-icon-blank", !$(this).prop('checked'));
        return true;
    };
    $.fn.checkbox = function(type) {
        return this.each(function() {
            var $this = $(this);
            if ($this.attr("uithemed") != "uithemed") {
                var size = "16";
                $this.after("<span class='ui-icon ui-icon-blank' style='display:block;width:" + size + "px;height:" + size + "px;'></span>");
                var parent = $this.next();
                $this.attr("uithemed", "uithemed");
                $this.hide();

                parent.wrap("<span class='ui-state-default ui-corner-all hover' style='width:" + size + ";height:" + size + ";display:inline-block;margin-right:5px;'></span>");
                var parent2 = parent.parent();

                $this.bind('click', checkboxchangeFunction);
                if ($this.is(':disabled')) {
                    parent2.addClass("ui-state-disabled");
                } else {
                    parent2.mouseenter(function(event) {
                        $(this).addClass("ui-state-hover");
                    });
                    parent2.mouseleave(function(event) {
                        $(this).removeClass("ui-state-hover");
                    });
                    parent.click(function(event) {
                        var cb = $(this).parent().prev()[0];
                        cb.click();
                    });
                }
            }
            $this.extend({ updateCheck: checkboxchangeFunction });
            $this.updateCheck();
        });
    };

    $.fn.uncheckbox = function(type) {
        return this.each(function() {
            var $this = $(this);
            if ($this.attr("uithemed") == "uithemed") {
                $this.show();
                $this.removeAttr("uithemed");
                $this.next().remove();
                $this.unbind($.browser.msie ? 'click' : 'change', checkboxchangeFunction);
            }
        });
    };

    var setrb = function(parent, state) {
        var checkedClass = "ui-icon-bullet";
        var uncheckedClass = "ui-icon-radio-on";
        var offset = 4;
        parent.removeClass(checkedClass);
        parent.removeClass(uncheckedClass);
        if (state) { parent.addClass(checkedClass); }
        else { parent.addClass(uncheckedClass); }
        try {
            if (!$.browser.msie) {
                parent.css('background-position', '');
                var backpos = parent.css('background-position').replace('px', '').split(' ');
                if (parent.hasClass(checkedClass)) {
                    parent.css('background-position', (parseInt(backpos[0], 0) - offset + 1) + "px " + (parseInt(backpos[1], 0) - offset) + "px");
                } else {
                    parent.css('background-position', (parseInt(backpos[0], 0) - offset) + "px " + (parseInt(backpos[1], 0) - offset) + "px");
                }
            } else {
                parent.css('background-position-x', '');
                parent.css('background-position-y', '');
                if (parent.hasClass(checkedClass)) {
                    parent.css('background-position-x', (parseInt(parent.css('background-position-x').replace('px', '')) - offset + 1) + "px ");
                    parent.css('background-position-y', (parseInt(parent.css('background-position-y').replace('px', '')) - offset) + "px ");
                } else {
                    parent.css('background-position-x', (parseInt(parent.css('background-position-x').replace('px', '')) - offset) + "px ");
                    parent.css('background-position-y', (parseInt(parent.css('background-position-y').replace('px', '')) - offset) + "px ");
                }

            }
        } catch (err) { }
    }
    var radiolistchangeFunction = function() {
        $(":radio[name=" + $(this).attr("name") + "]").each(function() {
            setrb($($(this).next().children()[0]), this.checked);
        });
    };
    $.fn.radio = function(type) {
        return this.each(function() {
            var $this = $(this);
            if ($this.attr("uithemed") != "uithemed") {
                var size = "9";

                $this.after("<span class='ui-icon ui-icon-radio-on' style='display:block;width:" + size + "px;height:" + size + "px;'></span>");
                var parent = $this.next();
                $this.attr("uithemed", "uithemed");
                $this.hide();

                $this.bind($.browser.msie ? 'click' : 'change', radiolistchangeFunction);

                parent.wrap("<span class='ui-state-default ui-corner-all hover' style='width:" + size + ";height:" + size + ";display:inline-block;margin-right:5px;'></span>");
                var parent2 = parent.parent();
                setrb(parent, this.checked);

                if ($this.is(':disabled')) {
                    parent2.addClass("ui-state-disabled");
                } else {
                    parent.click(function(event) {
                        var cb = $(this).parent().prev()[0];
                        $(cb).click();
                    });

                    parent2.mouseenter(function(event) {
                        $(this).addClass("ui-state-hover");
                    });
                    parent2.mouseleave(function(event) {
                        $(this).removeClass("ui-state-hover");
                    });
                }
            }
        });
    };

    $.fn.unradio = function(type) {
        return this.each(function() {
            var $this = $(this);
            if ($this.attr("uithemed") == "uithemed") {
                $this.show();
                $this.removeAttr("uithemed");
                $this.next().remove();
                $this.unbind($.browser.msie ? 'click' : 'change', radiolistchangeFunction);
            }
        });
    };
})(jQuery);

function addFromParent(select){ 
    var oHead = document.getElementsByTagName("head")[0];   
    if($(select).length >0){
      if($.browser.msie){
        if($(select)[0].tagName == "LINK")
            $(select).attr("href",parent.$(select).attr('href'));
        else if($(select)[0].tagName == "SCRIPT"){
            $(select)[0].text  = parent.$(select).html();
        }else if($(select)[0].tagName == "STYLE"){
            $(select)[0].text  = parent.$(select).html();    
        }else{
            $(select).replaceWith(parent.$(select).clone());
        }
      }else{ $(select).replaceWith(parent.$(select).clone());}
    }else{
        if(!$.browser.msie){ $('head').append(parent.$(select).clone());}
        else
        if(parent.$(select)[0].tagName == "SCRIPT")
        {
            var script   = document.createElement("script");
            script.type  = "text/javascript";
            script.text  = parent.$(select).html();               // use this for inline script
            document.getElementsByTagName("head")[0].appendChild(script);
        }else if(parent.$(select)[0].tagName == "STYLE")
        {
            var script   = document.createElement("style");
            script.type  = "text/css";
            script.text  = parent.$(select).html();               // use this for inline script
            document.getElementsByTagName("head")[0].appendChild(script);
        }else
          $('head').append(parent.$(select).clone());
    }
}
