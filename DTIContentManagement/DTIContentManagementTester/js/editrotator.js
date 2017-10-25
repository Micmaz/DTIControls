(function($){
    $.fn.editrotator = function(options){
        var defaults = {
            divClass:       null,
            vertical:       false,
            customImages:   false,
            staticLoadSize: false,
            staticHeight:   200,
            staticWidth:    200,
            newDivText:     "New Content",
            comboImage:     "EditRotatorImages.png",
            imagesPath:     "/images/",
            prevImg:        "prev.gif",
            nextImg:        "next.gif",
            firstImg:       "first.gif",
            lastImg:        "last.gif",
            swapLeftImg:    "swapLeft.gif",
            swapRightImg:   "swapRight.gif",
            deleteDivImg:   "deleteDiv.gif",
            addDivImg:      "addDiv.gif",
            saveImg:        "save.gif",
            cancelImg:      "cancel.gif"
        };
        var options = $.extend(defaults, options);
        
        function showNext(_button){
             var navDiv = $(_button).parents('div.editrotatornav')[0];
             $(navDiv).find('.editrotatorswapright, .editrotatornext, .editrotatorlast').css('visibility', '');
        }
        
        function hideNext(_button){
             var navDiv = $(_button).parents('div.editrotatornav')[0];
             $(navDiv).find('.editrotatorswapright, .editrotatornext, .editrotatorlast').css('visibility', 'hidden');
        }
        
        function showPrev(_button){
             var navDiv = $(_button).parents('div.editrotatornav')[0];
             $(navDiv).find('.editrotatorswapleft, .editrotatorprev, .editrotatorfirst').css('visibility', '');
        }
        
        function hidePrev(_button){
             var navDiv = $(_button).parents('div.editrotatornav')[0];
             $(navDiv).find('.editrotatorswapleft, .editrotatorprev, .editrotatorfirst').css('visibility', 'hidden');
        }
        
        function destroyWrapper(){
            //dtiCheckCKEDestroy();
        }
        
        function displayDiv(hideDiv, showDiv){
            if($(hideDiv).attr('id') == 'dtiRotatorTemp'){
                $(hideDiv).removeAttr('id')
            }
            if(!$(showDiv).attr('id')){
                $(showDiv).attr('id', 'dtiRotatorTemp')
            }
            $(showDiv).show();
            $(hideDiv).hide();
        }
        
        // the actual fn for effecting a transition
        function cycle(wrapper, curr, next, forward, cb) {
            var $l = $(curr), $n = $(next), $wr = $(wrapper);
            $(curr, next).show();
            var speedIn = 1000, speedOut = 1000;
            //find the biggest dimensions of the two divs and set the wrapper to that
            if(!options.staticLoadSize){
                var _width = ($l.width() > $n.width())?$l.width():$n.width();
                var _height = ($l.height() > $n.height())?$l.height():$n.height();
                $wr.animate({
                    width: _width,
                    height: _height
                });
            }
            var _animation = "scrollRight";
            if(options.vertical){
                if(forward){
                    _animation = "scrollUp";
                }
                else{
                    _animation = "scrollDown";
                }
            }
            else{
                if(forward){
                    _animation = "scrollLeft";
                }
                else{
                    _animation = "scrollRight";
                }                
            }
            var fn = function() {$n.animate(_animation, speedIn, null, cb)};
            $l.animate(_animation, speedOut, null);
            fn();
            //set the wrapper size to the size of the current div
            if(!options.staticLoadSize){
                _width = $n.width();
                _height = $n.height();
                $wr.animate({
                    width: _width,
                    height: _height
                }, 200);
            }
            
        }; 
        
        return this.each(function(){
            $(this).children('div:gt(0)').hide();
            $(this).children('div').addClass('dtiContentEdit');
            $(this).addClass('dtiClickEdit');
            //set up height and width to be the first div
            var _w = options.staticWidth;
            var _h = options.staticHeight;
            if(options.staticLoadSize){
                _w = $(this).children('div:eq(0)').width();
                _h = $(this).children('div:eq(0)').height();
                $(this).children('div').each(function(){
                    if($(this).width() > _w){_w = $(this).width();}
                    if($(this).height() > _h){_h = $(this).height();}
                });
            }
            else{
                var _w = $(this).children('div:eq(0)').width();
                var _h = $(this).children('div:eq(0)').height();
            }
            $(this).css({'overflow': 'hidden', 'height' : _h, 'width' : _w});
            if(!$(this).children('div:eq(0)').attr('id')){
                $(this).children('div:eq(0)').attr('id', 'dtiRotatorTemp')
            }
            //wrap divs with div with navigation at the bottom
            $(this).wrap('<div class="editrotatorwrapper"/>');
            $(this).parent('div.editrotatorwrapper').append('<div class="editrotatornav"/>');
            var navigation = $(this).siblings('div.editrotatornav');
            $(navigation).append(
                '<input type="button" title="First Div" class="editrotatorfirst" style="visibility:hidden;border-width:0px;background:url(' + options.imagesPath + ((options.customImages)?options.firstImg:options.comboImage) + ') no-repeat;" />' + 
                '<input type="button" title="Previous Div" class="editrotatorprev" style="visibility:hidden;border-width:0px;background:url(' + options.imagesPath + ((options.customImages)?options.prevImg:options.comboImage) + ') no-repeat;" />' + 
                '<input type="button" title="Swap With Left Div" class="editrotatorswapleft" style="visibility:hidden;border-width:0px;background:url(' + options.imagesPath + ((options.customImages)?options.swapLeftImg:options.comboImage) + ') no-repeat;" />' + 
                '<input type="button" title="Swap With Right Div" class="editrotatorswapright" style="border-width:0px;background:url(' + options.imagesPath + ((options.customImages)?options.swapRightImg:options.comboImage) + ') no-repeat;" />' + 
                '<input type="button" title="Next Div" class="editrotatornext" style="border-width:0px;background:url(' + options.imagesPath + ((options.customImages)?options.nextImg:options.comboImage) + ') no-repeat;" />' + 
                '<input type="button" title="Last Div" class="editrotatorlast" style="border-width:0px;background:url(' + options.imagesPath + ((options.customImages)?options.lastImg:options.comboImage) + ') no-repeat;" />' +
                '<input type="button" title="Add Div" class="editrotatoradd" style="border-width:0px;background:url(' + options.imagesPath + ((options.customImages)?options.addDivImg:options.comboImage) + ') no-repeat;" />' +
                '<input type="button" title="Delete Div" class="editrotatordel" style="border-width:0px;background:url(' + options.imagesPath + ((options.customImages)?options.delDivImg:options.comboImage) + ') no-repeat;" />' +
                '<input type="button" title="Save" class="editrotatorsave" style="border-width:0px;background:url(' + options.imagesPath + ((options.customImages)?options.saveImg:options.comboImage) + ') no-repeat;" />' +
                '<input type="button" title="Cancel" class="editrotatorcancel" style="border-width:0px;background:url(' + options.imagesPath + ((options.customImages)?options.cancelImg:options.comboImage) + ') no-repeat;" />' +
                '<input type="button" value="Strip" title="Strip" class="editrotatorstrip" />');
            //hide other buttons if this is the only div
            if($(this).children('div').length < 2){
                $(navigation).find('.editrotatornext, .editrotatorswapright, .editrotatorlast').css('visibility', 'hidden');
            }
            if(!$(this).children("div").length){
                $(this).append('<div><p>' + options.newDivText + '</p></div>');
            }
            //add to the jquery data object to hold the index of the current div
            jQuery.data(this, 'currentIndex', 0);
            //wire up click events on the buttons
            $(navigation).find('.editrotatorfirst').click(function(_this){
                destroyWrapper();
                var button = _this.target;
                var wrapperDiv = $(button).parents('div.editrotatorwrapper > div:eq(0)')[0];
                var curIndex = jQuery.data(wrapperDiv, 'currentIndex');
                var divs = $(wrapperDiv).children('div');
                hidePrev(button);
                if(divs.length > 1){
                    showNext(button);
                }
                displayDiv($(divs[curIndex]), $(divs).first());
                jQuery.data(wrapperDiv, 'currentIndex', 0);
            });
            $(navigation).find('.editrotatorprev').click(function(_this){
                destroyWrapper();
                var button = _this.target;
                var wrapperDiv = $(button).parents('div.editrotatorwrapper > div:eq(0)')[0];
                var curIndex = jQuery.data(wrapperDiv, 'currentIndex');
                var divs = $(wrapperDiv).children('div');
                //displayDiv($(divs[curIndex]), $(divs[curIndex - 1]));
                if(!(curIndex - 1)){
                     hidePrev(button);
                }
                showNext(button);
                jQuery.data(wrapperDiv, 'currentIndex', curIndex - 1);
            });
            $(navigation).find('.editrotatorswapleft').click(function(_this){
                destroyWrapper();
                var button = _this.target;
                var wrapperDiv = $(button).parents('div.editrotatorwrapper > div:eq(0)')[0];
                var curIndex = jQuery.data(wrapperDiv, 'currentIndex');
                var divs = $(wrapperDiv).children('div');
                $(divs[curIndex]).swap($(divs[curIndex - 1]));
                if(curIndex < 2){
                    hidePrev(button);
                }
                if(divs.length > 1){
                    showNext(button);
                }
                jQuery.data(wrapperDiv, 'currentIndex', curIndex - 1);
            });
            $(navigation).find('.editrotatorswapright').click(function(_this){
                destroyWrapper();
                var button = _this.target;
                var wrapperDiv = $(button).parents('div.editrotatorwrapper > div:eq(0)')[0];
                var curIndex = jQuery.data(wrapperDiv, 'currentIndex');
                var divs = $(wrapperDiv).children('div');
                $(divs[curIndex]).swap($(divs[curIndex + 1]));
                if(curIndex > divs.length - 3){
                    hideNext(button);
                }
                if(divs.length > 1){
                    showPrev(button);
                }
                jQuery.data(wrapperDiv, 'currentIndex', curIndex + 1);
            });
            $(navigation).find('.editrotatornext').click(function(_this){
                destroyWrapper();
                var button = _this.target;
                var wrapperDiv = $(button).parents('div.editrotatorwrapper > div:eq(0)')[0];
                var curIndex = jQuery.data(wrapperDiv, 'currentIndex');
                var divs = $(wrapperDiv).children('div');
                //cycle(wrapperDiv, divs[curIndex], divs[curIndex + 1], true);
                //displayDiv($(divs[curIndex]), $(divs[curIndex + 1]));
                if(!divs[curIndex + 2]){
                     hideNext(button);
                }
                showPrev(button);
                jQuery.data(wrapperDiv, 'currentIndex', curIndex + 1);
            });
            $(navigation).find('.editrotatorlast').click(function(_this){
                destroyWrapper();
                var button = _this.target;
                var wrapperDiv = $(button).parents('div.editrotatorwrapper > div:eq(0)')[0];
                var curIndex = jQuery.data(wrapperDiv, 'currentIndex');
                var divs = $(wrapperDiv).children('div');
                displayDiv($(divs[curIndex]), $(divs).last());
                hideNext(button);
                if(divs.length > 1){
                    showPrev(button);
                }
                jQuery.data(wrapperDiv, 'currentIndex', divs.length - 1);
            });
            $(navigation).find('.editrotatoradd').click(function(_this){
                destroyWrapper();
                var button = _this.target;
                var wrapperDiv = $(button).parents('div.editrotatorwrapper > div:eq(0)')[0];
                var curIndex = jQuery.data(wrapperDiv, 'currentIndex');
                var divs = $(wrapperDiv).children('div');
                var currDiv = divs[curIndex];
                var newDiv = $(currDiv).clone().insertAfter($(currDiv));
                $(newDiv).html('<p>' + options.newDivText + '</p>');
                $(button).siblings('input.editrotatornext').click();
            });
            $(navigation).find('.editrotatordel').click(function(_this){
                destroyWrapper();
                var button = _this.target;
                var wrapperDiv = $(button).parents('div.editrotatorwrapper > div:eq(0)')[0];
                var curIndex = jQuery.data(wrapperDiv, 'currentIndex');
                var divs = $(wrapperDiv).children('div');
                var currDiv = divs[curIndex];
                //if no siblings reset content
                if(divs.length == 1){
                    $(currDiv).html('<p>' + options.newDivText + '</p>');
                }
                else{
                    var replaceDiv;
                    var nextIndex = curIndex;
                    //find replacement div
                    if(!$(currDiv).next().length){
                        $(currDiv).prev().show();
                        replaceDiv = $(currDiv).prev();
                        nextIndex = curIndex - 1;
                    }
                    else{
                        $(currDiv).next().show();
                        replaceDiv = $(currDiv).next();
                    }
                    displayDiv($(currDiv), $(replaceDiv));
                    $(currDiv).remove();
                    jQuery.data(wrapperDiv, 'currentIndex', nextIndex);
                    if(!nextIndex){
                        hidePrev(button);
                    }
                    var lastDiv = $(divs).last();
                    if(lastDiv[0] === replaceDiv[0]){
                        hideNext(button);
                    }
                }
            });
            $(navigation).find('.editrotatorsave').click(function(_this){
                alert('hi');
            });
            $(navigation).find('.editrotatorcancel').click(function(_this){
                alert('hi');
            });
            $(navigation).find('.editrotatorstrip').click(function(_this){
                var button = _this.target;
                var wrapperDiv = $(button).parents('div.editrotatorwrapper > div:eq(0)')[0];
                var curIndex = jQuery.data(wrapperDiv, 'currentIndex');
                var divs = $(wrapperDiv).children('div');
                divs.removeAttr('style');
                $(wrapperDiv).css('position', '');
            });
        });
    };
})(jQuery);

jQuery.fn.swap = function(b){
    b = jQuery(b)[0];
    var a = this[0];
    var t = a.parentNode.insertBefore(document.createTextNode(''), a);
    b.parentNode.insertBefore(a, b);
    t.parentNode.insertBefore(b, t);
    t.parentNode.removeChild(t);
    return this;
};