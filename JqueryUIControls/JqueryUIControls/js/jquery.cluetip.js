/*!
 * jQuery clueTip plugin v1.1.2
 *
 * Date: Fri Mar 25 10:16:31 2011 EDT
 * Requires: jQuery v1.3+
 *
 * Copyright 2010, Karl Swedberg
 * Dual licensed under the MIT and GPL licenses:
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.gnu.org/licenses/gpl.html
 *
 * Full list of options/settings can be found at the bottom of this file and at http://plugins.learningjquery.com/cluetip/
 * Examples can be found at http://plugins.learningjquery.com/cluetip/demo/
 *
*/

(function($) {


  $.cluetip = {
    version: '1.1.2',

    /* clueTip setup
     *  the setup options are applied each time .cluetip() is called,
     *  BUT only if <div id="cluetip"> is not already in the document
    */
    setup: {
      // method to be used for inserting the clueTip into the DOM.
      // Permitted values are 'appendTo', 'prependTo', 'insertBefore', and 'insertAfter'
      insertionType: 'appendTo',
      // element in the DOM the plugin will reference when inserting the clueTip.
      insertionElement: 'form'
    },

    /*
     * clueTip options
     *
     * each one can be explicitly overridden by changing its value.
     * for example: $.cluetip.defaults.width = 200;
     *         or: $.fn.cluetip.defaults.width = 200; // for compatibility with previous clueTip versions
     * would change the default width for all clueTips to 200.
     *
     * each one can also be overridden by passing an options map to the cluetip method.
     * for example: $('a.example').cluetip({width: 200});
     * would change the default width to 200 for clueTips invoked by a link with class of "example"
     *
    */
    defaults: {
      width:            275,      // The width of the clueTip
      height:           'auto',   // The height of the clueTip
      cluezIndex:       97,       // Sets the z-index style property of the clueTip
      positionBy:       'auto',   // Sets the type of positioning: 'auto', 'mouse','bottomTop', 'fixed'
      topOffset:        15,       // Number of px to offset clueTip from top of invoking element
      leftOffset:       15,       // Number of px to offset clueTip from left of invoking element
      local:            false,    // Whether to use content from the same page for the clueTip's body
      localPrefix:      null,     // string to be prepended to the tip attribute if local is true
      localIdSuffix:    null,     // string to be appended to the cluetip content element's id if local is true
      hideLocal:        true,     // If local option is set to true, this determines whether local content
                                  // to be shown in clueTip should be hidden at its original location
      attribute:        'rel',    // the attribute to be used for fetching the clueTip's body content
      titleAttribute:   'title',  // the attribute to be used for fetching the clueTip's title
      splitTitle:       '',       // A character used to split the title attribute into the clueTip title and divs
                                  // within the clueTip body. more info below [6]
      escapeTitle:      false,    // whether to html escape the title attribute
      showTitle:        true,     // show title bar of the clueTip, even if title attribute not set
      cluetipClass:     'default',// class added to outermost clueTip div in the form of 'cluetip-' + clueTipClass.
      hoverClass:       '',       // class applied to the invoking element onmouseover and removed onmouseout
      waitImage:        true,     // whether to show a "loading" img, which is set in jquery.cluetip.css
      cursor:           'help',
      arrows:           false,    // if true, displays arrow on appropriate side of clueTip
      dropShadow:       true,     // set to false if you don't want the drop-shadow effect on the clueTip
      dropShadowSteps:  6,        // adjusts the size of the drop shadow
      sticky:           false,    // keep visible until manually closed
      mouseOutClose:    false,    // close when clueTip is moused out
      activation:       'hover',  // set to 'click' to force user to click to show clueTip
                                  // set to 'focus' to show on focus of a form element and hide on blur
      clickThrough:     true,    // if true, and activation is not 'click', then clicking on link will take user to the link's href,
                                  // even if href and tipAttribute are equal
      tracking:         false,    // if true, clueTip will track mouse movement (experimental)
      delayedClose:     0,        // close clueTip on a timed delay (experimental)
      closePosition:    'top',    // location of close text for sticky cluetips; can be 'top' or 'bottom' or 'title'
      closeText:        'Close',  // text (or HTML) to to be clicked to close sticky clueTips
      truncate:         0,        // number of characters to truncate clueTip's contents. if 0, no truncation occurs

      // effect and speed for opening clueTips
      fx: {
                        open:       'show', // can be 'show' or 'slideDown' or 'fadeIn'
                        openSpeed:  ''
      },

      // settings for when hoverIntent plugin is used
      hoverIntent: {
                        sensitivity:  3,
                        interval:     50,
                        timeout:      0
      },

      // short-circuit function to run just before clueTip is shown.
      onActivate:       function(e) {return true;},
      // function to run just after clueTip is shown.
      onShow:           function(ct, ci){},
      // function to run just after clueTip is hidden.
      onHide:           function(ct, ci){},
      // whether to cache results of ajax request to avoid unnecessary hits to server
      ajaxCache:        true,

      // process data retrieved via xhr before it's displayed
      ajaxProcess:      function(data) {
                          data = data.replace(/<(script|style|title)[^<]+<\/(script|style|title)>/gm, '').replace(/<(link|meta)[^>]+>/g,'');
                          return data;
      },

      // can pass in standard $.ajax() parameters. Callback functions, such as beforeSend,
      // will be queued first within the default callbacks.
      // The only exception is error, which overrides the default
      ajaxSettings: {
                        // error: function(ct, ci) { /* override default error callback */ },
                        // beforeSend: function(ct, ci) { /* called first within default beforeSend callback */ },
                        dataType: 'html'
      },
      debug: false

    }
  };
  var $cluetip, $cluetipInner, $cluetipOuter, $cluetipTitle, $cluetipArrows, $cluetipWait, $dropShadow, imgCount,
      standardClasses = 'ui-widget ui-widget-content ui-cluetip';


  $.fn.cluetip = function(js, options) {
    if (typeof js == 'object') {
      options = js;
      js = null;
    }
    if (js == 'destroy') {
      $(document).unbind('.cluetip');
      $('#cluetip').remove();
      $.removeData(this, 'title');
      $.removeData(this, 'cluetip');
      return this.unbind('.cluetip');
    }

    // merge per-call options with defaults
    options = $.extend(true, {}, $.cluetip.defaults, options || {});

    /** =create cluetip divs **/
    var insertionType = (/appendTo|prependTo|insertBefore|insertAfter/).test(options.insertionType) ? options.insertionType : 'appendTo',
        insertionElement = options.insertionElement || 'form';

    if (!$('#cluetip').length) {
      $(['<div id="cluetip">',
        '<div id="cluetip-outer" class="ui-cluetip-outer">',
          '<h3 id="cluetip-title" class="ui-widget-header ui-cluetip-header"></h3>',
          '<div id="cluetip-inner" class="ui-widget-content ui-cluetip-content"></div>',
        '</div>',
        '<div id="cluetip-extra"></div>',
        '<div id="cluetip-arrows" class="cluetip-arrows"></div>',
      '</div>'].join(''))
      [insertionType](insertionElement).hide();

      var cluezIndex = +options.cluezIndex;

      $cluetip = $('#cluetip').css({position: 'absolute'});
      $cluetipOuter = $('#cluetip-outer').css({position: 'relative', zIndex: cluezIndex});
      $cluetipInner = $('#cluetip-inner');
      $cluetipTitle = $('#cluetip-title');
      $cluetipArrows = $('#cluetip-arrows');
      $cluetipWait = $('<div id="cluetip-waitimage"></div>')
        .css({position: 'absolute'}).insertBefore($cluetip).hide();
    }
    var cluetipPadding = (parseInt($cluetip.css('paddingLeft'),10)||0) + (parseInt($cluetip.css('paddingRight'),10)||0);


    this.each(function(index) {
      var link = this,
          $link = $(this),
          // support metadata plugin (v1.0 and 2.0)
          opts = $.extend(true, {}, options, $.metadata ? $link.metadata() : $.meta ? $link.data() : {}),
          // start out with no contents (for ajax activation)
          cluetipContents = false,
          isActive = false,
          closeOnDelay = 0,
          tipAttribute = $link.attr(opts.attribute),
          ctClass = opts.cluetipClass;

      cluezIndex = +opts.cluezIndex;
      $link.data('cluetip', {title: link.title, zIndex: cluezIndex});

      if (!tipAttribute && !opts.splitTitle && !js) {
        return true;
      }
      // if hideLocal is set to true, on DOM ready hide the local content that will be displayed in the clueTip
      if (opts.local && opts.localPrefix) {tipAttribute = opts.localPrefix + tipAttribute;}
      if (opts.local && opts.hideLocal && tipAttribute) { $(tipAttribute + ':first').hide(); }
      var tOffset = parseInt(opts.topOffset, 10), lOffset = parseInt(opts.leftOffset, 10);
      // vertical measurement variables
      var tipHeight, wHeight,
          defHeight = isNaN(parseInt(opts.height, 10)) ? 'auto' : (/\D/g).test(opts.height) ? opts.height : opts.height + 'px';
      var sTop, linkTop, posY, tipY, mouseY, baseline;
      // horizontal measurement variables
      var tipInnerWidth = parseInt(opts.width, 10) || 275,
          tipWidth = tipInnerWidth + cluetipPadding + opts.dropShadowSteps,
          linkWidth = this.offsetWidth,
          linkLeft, posX, tipX, mouseX, winWidth;

      // parse the title
      var tipParts;
      var tipTitle = (opts.attribute != 'title') ? $link.attr(opts.titleAttribute) : '';
      if (opts.splitTitle) {
        if (tipTitle == undefined) {tipTitle = '';}
        tipParts = tipTitle.split(opts.splitTitle);
        tipTitle = tipParts.shift();
      }
      if (opts.escapeTitle) {
        tipTitle = tipTitle.replace(/&/g,'&amp;').replace(/>/g,'&gt;').replace(/</g,'&lt;');
      }

      var localContent;
      function returnFalse() { return false; }

/***************************************
* ACTIVATION
****************************************/

//activate clueTip
    var activate = function(event) {
      var continueOn = opts.onActivate($link);
      if (continueOn === false) {
        return false;
      }
      isActive = true;
      $cluetip.removeClass().css({width: tipInnerWidth});
      if (tipAttribute == $link.attr('href')) {
        $link.css('cursor', opts.cursor);
      }
      if (opts.hoverClass) {
        $link.addClass(opts.hoverClass);
      }
      linkTop = posY = $link.offset().top;
      linkLeft = $link.offset().left;
      mouseX = event.pageX;
      mouseY = event.pageY;
      if (link.tagName.toLowerCase() != 'area') {
        sTop = $(document).scrollTop();
        winWidth = $(window).width();
      }
// position clueTip horizontally
      if (opts.positionBy == 'fixed') {
        posX = linkWidth + linkLeft + lOffset;
        $cluetip.css({left: posX});
      } else {
        posX = (linkWidth > linkLeft && linkLeft > tipWidth)
          || linkLeft + linkWidth + tipWidth + lOffset > winWidth
          ? linkLeft - tipWidth - lOffset
          : linkWidth + linkLeft + lOffset;
        if (link.tagName.toLowerCase() == 'area' || opts.positionBy == 'mouse' || linkWidth + tipWidth > winWidth) { // position by mouse
          if (mouseX + 20 + tipWidth > winWidth) {
            $cluetip.addClass(' cluetip-' + ctClass);
            posX = (mouseX - tipWidth - lOffset) >= 0 ? mouseX - tipWidth - lOffset - parseInt($cluetip.css('marginLeft'),10) + parseInt($cluetipInner.css('marginRight'),10) :  mouseX - (tipWidth/2);
          } else {
            posX = mouseX + lOffset;
          }
        }
        var pY = posX < 0 ? event.pageY + tOffset : event.pageY;
        $cluetip.css({
          left: (posX > 0 && opts.positionBy != 'bottomTop') ? posX : (mouseX + (tipWidth/2) > winWidth) ? winWidth/2 - tipWidth/2 : Math.max(mouseX - (tipWidth/2),0),
          zIndex: $link.data('cluetip').zIndex
        });
        $cluetipArrows.css({zIndex: $link.data('cluetip').zIndex+1});
      }
        wHeight = $(window).height();

/***************************************
* load a string from cluetip method's first argument
***************************************/
      if (js) {
        if (typeof js == 'function') {
          js = js.call(link);
        }
        $cluetipInner.html(js);
        cluetipShow(pY);
      }
/***************************************
* load the title attribute only (or user-selected attribute).
* clueTip title is the string before the first delimiter
* subsequent delimiters place clueTip body text on separate lines
***************************************/

      else if (tipParts) {
        var tpl = tipParts.length;
        $cluetipInner.html(tpl ? tipParts[0] : '');
        if (tpl > 1) {
          for (var i=1; i < tpl; i++){
            $cluetipInner.append('<div class="split-body">' + tipParts[i] + '</div>');
          }
        }
        cluetipShow(pY);
      }
/***************************************
* load external file via ajax
***************************************/

      else if (!opts.local && tipAttribute.indexOf('#') !== 0) {
        if (/\.(jpe?g|tiff?|gif|png)(?:\?.*)?$/i.test(tipAttribute)) {
          $cluetipInner.html('<img src="' + tipAttribute + '" alt="' + tipTitle + '" />');
          cluetipShow(pY);
        } else {
          var optionBeforeSend = opts.ajaxSettings.beforeSend,
              optionError = opts.ajaxSettings.error,
              optionSuccess = opts.ajaxSettings.success,
              optionComplete = opts.ajaxSettings.complete;
          var ajaxSettings = {
            cache: false, // force requested page not to be cached by browser
            url: tipAttribute,
            beforeSend: function(xhr) {
              if (optionBeforeSend) {optionBeforeSend.call(link, xhr, $cluetip, $cluetipInner);}
              $cluetipOuter.children().empty();
              if (opts.waitImage) {
                $cluetipWait
                .css({top: mouseY+20, left: mouseX+20, zIndex: $link.data('cluetip').zIndex-1})
                .show();
              }
            },
            error: function(xhr, textStatus) {
              if (isActive) {
                if (optionError) {
                  optionError.call(link, xhr, textStatus, $cluetip, $cluetipInner);
                } else {
                  $cluetipInner.html('<i>sorry, the contents could not be loaded</i>');
                }
              }
            },
            success: function(data, textStatus) {
              cluetipContents = opts.ajaxProcess.call(link, data);
              if (isActive) {
                if (optionSuccess) {optionSuccess.call(link, data, textStatus, $cluetip, $cluetipInner);}
                $cluetipInner.html(cluetipContents);
              }
            },
            complete: function(xhr, textStatus) {
              if (optionComplete) {optionComplete.call(link, xhr, textStatus, $cluetip, $cluetipInner);}
              var imgs = $cluetipInner[0].getElementsByTagName('img');
              imgCount = imgs.length;
              for (var i=0, l = imgs.length; i < l; i++) {
                if (imgs[i].complete) {
                  imgCount--;
                }
              }
              if (imgCount && !$.browser.opera) {
                $(imgs).bind('load error', function() {
                  imgCount--;
                  if (imgCount<1) {
                    $cluetipWait.hide();
                    if (isActive) { cluetipShow(pY); }
                  }
                });
              } else {
                $cluetipWait.hide();
                if (isActive) { cluetipShow(pY); }
              }
            }
          };
          var ajaxMergedSettings = $.extend(true, {}, opts.ajaxSettings, ajaxSettings);

          $.ajax(ajaxMergedSettings);
        }

/***************************************
* load an element from the same page
***************************************/
      } else if (opts.local) {

        var $localContent = $(tipAttribute + (/#\S+$/.test(tipAttribute) ? '' : ':eq(' + index + ')')).clone(true).show();
        if (opts.localIdSuffix) {
          $localContent.attr('id', $localContent[0].id + opts.localIdSuffix);
        }
        $cluetipInner.html($localContent);
        cluetipShow(pY);
      }
    };

// get dimensions and options for cluetip and prepare it to be shown
    var cluetipShow = function(bpY) {
      $cluetip.addClass('cluetip-' + ctClass);
      if (opts.truncate) {
        var $truncloaded = $cluetipInner.text().slice(0,opts.truncate) + '...';
        $cluetipInner.html($truncloaded);
      }

      function doNothing() {}; //empty function

      tipTitle ? $cluetipTitle.show().html(tipTitle) : (opts.showTitle) ? $cluetipTitle.show().html('&nbsp;') : $cluetipTitle.hide();
      if (opts.sticky) {
        var $closeLink = $('<div id="cluetip-close"><a href="#">' + opts.closeText + '</a></div>');
        (opts.closePosition == 'bottom') ? $closeLink.appendTo($cluetipInner) : (opts.closePosition == 'title') ? $closeLink.prependTo($cluetipTitle) : $closeLink.prependTo($cluetipInner);
        $closeLink.bind('click.cluetip', function() {
          cluetipClose();
          return false;
        });
        if (opts.mouseOutClose) {
          $cluetip.bind('mouseleave.cluetip', function() {
            cluetipClose();
          });
        } else {
          $cluetip.unbind('mouseleave.cluetip');
        }
      }
// now that content is loaded, finish the positioning
      var direction = '';
      $cluetipOuter.css({zIndex: $link.data('cluetip').zIndex, overflow: defHeight == 'auto' ? 'visible' : 'auto', height: defHeight});
      tipHeight = defHeight == 'auto' ? Math.max($cluetip.outerHeight(),$cluetip.height()) : parseInt(defHeight,10);
      tipY = posY;
      baseline = sTop + wHeight;
      if (opts.positionBy == 'fixed') {
        tipY = posY - opts.dropShadowSteps + tOffset;
      } else if ( (posX < mouseX && Math.max(posX, 0) + tipWidth > mouseX) || opts.positionBy == 'bottomTop') {
        if (posY + tipHeight + tOffset > baseline && mouseY - sTop > tipHeight + tOffset) {
          tipY = mouseY - tipHeight - tOffset;
          direction = 'top';
        } else {
          tipY = mouseY + tOffset;
          direction = 'bottom';
        }
      } else if ( posY + tipHeight + tOffset > baseline ) {
        tipY = (tipHeight >= wHeight) ? sTop : baseline - tipHeight - tOffset;
      } else if ($link.css('display') == 'block' || link.tagName.toLowerCase() == 'area' || opts.positionBy == "mouse") {
        tipY = bpY - tOffset;
      } else {
        tipY = posY - opts.dropShadowSteps;
      }
      if (direction == '') {
        posX < linkLeft ? direction = 'left' : direction = 'right';
      }
      // add classes
      var dynamicClasses = ' clue-' + direction + '-' + ctClass + ' cluetip-' + ctClass;
      if (ctClass == 'rounded') {
        dynamicClasses += ' ui-corner-all';
      }
      $cluetip.css({top: tipY + 'px'}).attr({'className': standardClasses + dynamicClasses});
      // set up arrow positioning to align with element
      if (opts.arrows) {
        var bgY = (posY - tipY - opts.dropShadowSteps);
        $cluetipArrows.css({top: (/(left|right)/.test(direction) && posX >=0 && bgY > 0) ? bgY + 'px' : /(left|right)/.test(direction) ? 0 : ''}).show();
      } else {
        $cluetipArrows.hide();
      }

// (first hide, then) ***SHOW THE CLUETIP***
      // handle dropshadow divs first
      $dropShadow = createDropShadows(opts);
      if ($dropShadow && $dropShadow.length) {
        $dropShadow.hide().css({height: tipHeight, width: tipInnerWidth, zIndex: $link.data('cluetip').zIndex-1}).show();
      }

      $cluetip.hide()[opts.fx.open](opts.fx.openSpeed || 0);
      if ($.fn.bgiframe) { $cluetip.bgiframe(); }
      // delayed close (not fully tested)
      if (opts.delayedClose > 0) {
        closeOnDelay = setTimeout(cluetipClose, opts.delayedClose);
      }
      // trigger the optional onShow function
      opts.onShow.call(link, $cluetip, $cluetipInner);
    };

/***************************************
   =INACTIVATION
-------------------------------------- */
    var inactivate = function(event) {
      isActive = false;
      $cluetipWait.hide();
      if (!opts.sticky || (/click|toggle/).test(opts.activation) ) {
        cluetipClose();
        clearTimeout(closeOnDelay);
      }
      if (opts.hoverClass) {
        $link.removeClass(opts.hoverClass);
      }
    };
// close cluetip and reset some things
    var cluetipClose = function() {
      $cluetipOuter
      .parent().hide().removeClass();
      opts.onHide.call(link, $cluetip, $cluetipInner);
      $link.removeClass('cluetip-clicked');
      if (tipTitle) {
        $link.attr(opts.titleAttribute, tipTitle);
      }
      $link.css('cursor','');
      if (opts.arrows) {
        $cluetipArrows.css({top: ''});
      }
    };

    $(document).bind('hideCluetip', function(e) {
      cluetipClose();
    });
/***************************************
   =BIND EVENTS
-------------------------------------- */
  // activate by click
      if ( (/click|toggle/).test(opts.activation) ) {
        $link.bind('click.cluetip', function(event) {
          if ($cluetip.is(':hidden') || !$link.is('.cluetip-clicked')) {
            activate(event);
            $('.cluetip-clicked').removeClass('cluetip-clicked');
            $link.addClass('cluetip-clicked');
          } else {
            inactivate(event);
          }
          return false;
        });
  // activate by focus; inactivate by blur
      } else if (opts.activation == 'focus') {
        $link.bind('focus.cluetip', function(event) {
          $link.attr('title','');
          activate(event);
        });
        $link.bind('blur.cluetip', function(event) {
          $link.attr('title', $link.data('thisInfo').title);
          inactivate(event);
        });
  // activate by hover
      } else {
        // clicking is returned false if clickThrough option is set to false
        $link[opts.clickThrough ? 'unbind' : 'bind']('click.cluetip', returnFalse);
        //set up mouse tracking
        var mouseTracks = function(evt) {
          if (opts.tracking == true) {
            var trackX = posX - evt.pageX;
            var trackY = tipY ? tipY - evt.pageY : posY - evt.pageY;
            $link.bind('mousemove.cluetip', function(evt) {
              $cluetip.css({left: evt.pageX + trackX, top: evt.pageY + trackY });
            });
          }
        };
        if ($.fn.hoverIntent && opts.hoverIntent) {
          $link.hoverIntent({
            sensitivity: opts.hoverIntent.sensitivity,
            interval: opts.hoverIntent.interval,
            over: function(event) {
              activate(event);
              mouseTracks(event);
            },
            timeout: opts.hoverIntent.timeout,
            out: function(event) {inactivate(event); $link.unbind('mousemove.cluetip');}
          });
        } else {
          $link.bind('mouseenter.cluetip', function(event) {
            activate(event);
            mouseTracks(event);
          })
          .bind('mouseleave.cluetip', function(event) {
            inactivate(event);
            $link.unbind('mousemove.cluetip');
          });
        }

        $link.bind('mouseover.cluetip', function(event) {
          $link.attr('title','');
        }).bind('mouseleave.cluetip', function(event) {
          $link.attr('title', $link.data('cluetip').title);
        });
      }
    });

    /** =private functions
    ************************************************************/
    /** =create dropshadow divs **/

    function createDropShadows(options, newDropShadow) {
      var dropShadowSteps = (options.dropShadow && options.dropShadowSteps) ? +options.dropShadowSteps : 0;
      if ($.support.boxShadow) {
        var dsOffsets = dropShadowSteps === 0 ? '0 0 ' : '1px 1px ';
        $('#cluetip').css($.support.boxShadow, dsOffsets + dropShadowSteps + 'px rgba(0,0,0,0.5)');
        return false;
      }
      var oldDropShadow = $('#cluetip .cluetip-drop-shadow');
      if (dropShadowSteps == oldDropShadow.length) {
        return oldDropShadow;
      }
      oldDropShadow.remove();
      var dropShadows = [];
      for (var i=0; i < dropShadowSteps;) {
        dropShadows[i++] = '<div style="top:' + i + 'px;left:' + i + 'px;"></div>';
      }

      newDropShadow = $(dropShadows.join(''))
      .css({
        position: 'absolute',
        backgroundColor: '#000',
        zIndex: cluezIndex -1,
        opacity: 0.1
      })
      .addClass('cluetip-drop-shadow')
      .prependTo('#cluetip');
      return newDropShadow;

    }

    return this;
  };

  (function() {
    $.support = $.support || {};
    // check support for CSS3 properties (currently only boxShadow)
    var div = document.createElement('div'),
        divStyle = div.style,
        styleProps = ['boxShadow'],
        prefixes = ['moz', 'Moz', 'webkit', 'o'];

    for (var i=0, sl = styleProps.length; i < sl; i++) {
      var prop = styleProps[i],
          uProp = prop.charAt(0).toUpperCase() + prop.slice(1);

      if ( typeof divStyle[ prop ] !== 'undefined' ) {
        $.support[ prop ] = prop;
      } else {
        for (var j=0, pl = prefixes.length; j < pl; j++) {

          if (typeof divStyle[ prefixes[j] + uProp ] !== 'undefined') {
            $.support[ prop ] = prefixes[j] + uProp;
            break;
          }
        }
      }
    }
    div = null;
  })();

  $.fn.cluetip.defaults = $.cluetip.defaults;

})(jQuery);


/*!
* hoverIntent r7 // 2013.03.11 // jQuery 1.9.1+
* http://cherne.net/brian/resources/jquery.hoverIntent.html
*
* You may use hoverIntent under the terms of the MIT license.
* Copyright 2007, 2013 Brian Cherne
*/
(function (e) { e.fn.hoverIntent = function (t, n, r) { var i = { interval: 100, sensitivity: 7, timeout: 0 }; if (typeof t === "object") { i = e.extend(i, t) } else if (e.isFunction(n)) { i = e.extend(i, { over: t, out: n, selector: r }) } else { i = e.extend(i, { over: t, out: t, selector: n }) } var s, o, u, a; var f = function (e) { s = e.pageX; o = e.pageY }; var l = function (t, n) { n.hoverIntent_t = clearTimeout(n.hoverIntent_t); if (Math.abs(u - s) + Math.abs(a - o) < i.sensitivity) { e(n).off("mousemove.hoverIntent", f); n.hoverIntent_s = 1; return i.over.apply(n, [t]) } else { u = s; a = o; n.hoverIntent_t = setTimeout(function () { l(t, n) }, i.interval) } }; var c = function (e, t) { t.hoverIntent_t = clearTimeout(t.hoverIntent_t); t.hoverIntent_s = 0; return i.out.apply(t, [e]) }; var h = function (t) { var n = jQuery.extend({}, t); var r = this; if (r.hoverIntent_t) { r.hoverIntent_t = clearTimeout(r.hoverIntent_t) } if (t.type == "mouseenter") { u = n.pageX; a = n.pageY; e(r).on("mousemove.hoverIntent", f); if (r.hoverIntent_s != 1) { r.hoverIntent_t = setTimeout(function () { l(n, r) }, i.interval) } } else { e(r).off("mousemove.hoverIntent", f); if (r.hoverIntent_s == 1) { r.hoverIntent_t = setTimeout(function () { c(n, r) }, i.timeout) } } }; return this.on({ "mouseenter.hoverIntent": h, "mouseleave.hoverIntent": h }, i.selector) } })(jQuery)