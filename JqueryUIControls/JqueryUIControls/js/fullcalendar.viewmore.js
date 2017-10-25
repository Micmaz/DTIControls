/*
* Copyright (c) 2011 Lyconic, LLC.
*
* Permission is hereby granted, free of charge, to any person obtaining
* a copy of this software and associated documentation files (the
* "Software"), to deal in the Software without restriction, including
* without limitation the rights to use, copy, modify, merge, publish,
* distribute, sublicense, and/or sell copies of the Software, and to
* permit persons to whom the Software is furnished to do so, subject to
* the following conditions:
* 
* The above copyright notice and this permission notice shall be
* included in all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
* MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
* NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
* LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
* OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
* WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

//todo should attach reset to changeview method in certain situations

(function($, undefined) {
    $.fn.limitEvents = function(opts) {
        if (opts.constructor === Number) opts = { maxEvents: opts };
        return this.each(function() {
            var limit = new $.fn.limitEvents.constructor($(this));

            $.extend({ maxEvents: 4 }, opts); //defaults
            $(this).fullCalendar('limitEvents', opts)
        .fullCalendar('rerenderEvents');
        });
    };

    $.fn.limitEvents.constructor = function(calendar) {
        if (!(this instanceof arguments.callee)) return new arguments.callee(calendar);
        var self = this;

        self.calendar = calendar;

        self.calendar.data('fullCalendar').limitEvents = function(opts) {
            self.opts = opts;
            self.observers();
            self.increaseHeight(25);
            self.extendCallbacks();
        }
    };
})(jQuery);

(function($, undefined) {
    this.observers = function() {
        var self = this;

        $(document).mouseup(function(e) {  //deselect when clicking outside of calendar or formbubble
            var $target = $(e.target),
          isFormBubble = $target.parents('.form-bubble').length || $target.hasClass('form-bubble'),
          isInsideOfCalendar = $target.parents('.fc-content').length || $target.hasClass('fc-content');

            if (!isInsideOfCalendar && !isFormBubble) self.calendar.fullCalendar('unselect');
        });

        self.calendar.delegate('.fc-event', 'mousedown', function() { //close currently open form bubbles when user clicks an existing event
            $.fn.formBubble.close();
        });

        self.calendar.delegate('.fc-button-prev, .fc-button-next', 'click', function() {
            resetEventsRangeCounts();
            self.calendar.fullCalendar('rerenderEvents');
        });
    };

    this.increaseHeight = function(height, windowResized, td) {
        var cal = this.calendar,
          cells = td || cal.find('.fc-view-month tbody tr td'),
          fcDayContent = cells.find('.fc-day-content'),
          cellHeight, fcDayContentHeight;

        if (windowResized) fcDayContent.height(1);

        cellHeight = cells.eq(0).height(),
    fcDayContentHeight = cellHeight - cells.eq(0).find('.fc-day-number').height();

        fcDayContent.height(fcDayContentHeight);
    };

    this.extendCallbacks = function() {
        var self = this,
        opt = self.calendar.fullCalendar('getView').calendar.options,
        _eventRender = opt.eventRender,
        _eventDrop = opt.eventDrop,
        _eventResize = opt.eventResize,
        _viewDisplay = opt.viewDisplay,
        _events = opt.events,
        _windowResize = opt.windowResize;

        $.extend(opt, {
            eventRender: function(event, element) {
                var currentView = self.calendar.fullCalendar('getView').name,
                dateFormat = (event.allDay) ? 'MM/dd/yyyy' : 'hh:mmtt',
                startDateLink = $.fullCalendar.formatDate(event.start, dateFormat),
                endDateLink = $.fullCalendar.formatDate(event.end, dateFormat),
                maxEvents = self.opts.maxEvents,
                allEvents = self.calendar.fullCalendar('clientEvents'),
                eventDate = $.fullCalendar.formatDate(event.end || event.start, 'MM/dd/yy'),
                td, viewMoreButton;

                event.element = element;
                event.startDateLink = startDateLink;
                event.endDateLink = endDateLink;

                if (currentView === 'month') {
                    doEventsRangeCount(event, self.calendar); //add event quantity to range for event and day
                    td = getCellFromDate(eventDate, self.calendar);
					td.find('.events-view-more').remove();
                    if (td.data('apptCount') > maxEvents) {
                        //if (!td.find('.events-view-more').length) {
							
                            td.data('viewMore', true);
                            viewMoreButton = $('<input style="font-size: 10px;padding:0px;z-index:100;" type="button" class="events-view-more ui-button ui-widget ui-state-default ui-corner-all viewmore-button" value="" />')
                        .appendTo(td)
                        .click(function() {
                            var viewMoreClick = self.opts.viewMoreClick;

                            if (viewMoreClick && $.isFunction(viewMoreClick)) self.opts.viewMoreClick(td, self.calendar);
                            else viewMore(td, self.calendar); //show events in formBubble overlay

                            return false;
                        });
                            if (!td.data('viewMore')) self.increaseHeight(25, false, td);
                        //}
                        td.find('.events-view-more').val("View All " + td.data('apptCount'));
                        if ($.isFunction(_eventRender)) _eventRender(event, element);
                        return false; //prevents event from being rendered
                    } else {
                        td.data('viewMore', false);
                    }
                }
                if ($.isFunction(_eventRender)) _eventRender(event, element);
                return true; //renders event
            },
            eventDrop: function(event, dayDelta, minuteDelta, allDay, revertFunc) {
                resetEventsRangeCounts();
                if ($.isFunction(_eventDrop)) _eventDrop(event, dayDelta, minuteDelta, allDay, revertFunc);
            },
            eventResize: function(event) {
                resetEventsRangeCounts();
                if ($.isFunction(_eventResize)) _eventResize(event);
            },
            viewDisplay: function(view) {
                $.fn.formBubble.close();
                if (view.name !== 'month') resetEventsRangeCounts();
                if ($.isFunction(_viewDisplay)) _viewDisplay(view);
            },
            events: function(start, end, callback) {
                resetEventsRangeCounts();
                if ($.isFunction(_events)) _events(start, end, callback);
            },
            windowResize: function(view) { //fired AFTER events are rendered
                if ($.isFunction(_windowResize)) _windowResize(view);
                self.increaseHeight(25, true);
                resetEventsRangeCounts();
                self.calendar.fullCalendar('render'); //manually render to avoid layout bug
            }
        });
    };

    function doEventsRangeCount(event, calInstance) {
        var eventStart = event._start,
        eventEnd = event._end || event._start,
        dateRange = expandDateRange(eventStart, eventEnd),
        eventElement = event.element;

        $(dateRange).each(function(i) {
            var td = getCellFromDate($.fullCalendar.formatDate(dateRange[i], 'MM/dd/yy'), calInstance),
                currentCount = (td.data('apptCount') || 0) + 1;

            td.data('apptCount', currentCount);

            if (td.data().appointments === undefined) td.data().appointments = [event];
            else td.data().appointments.push(event);
        });
    }

    function expandDateRange(start, end) {
        var value = new Date(start.getFullYear(), start.getMonth(), start.getDate()),
        values = [];

        end = new Date(end.getFullYear(), end.getMonth(), end.getDate());
        if (value > end) throw "InvalidRange";

        while (value <= end) {
            values.push(value);
            value = new Date(value.getFullYear(), value.getMonth(), value.getDate() + 1);
        }

        return values;
    }

    function resetEventsRangeCounts() {
        $('.fc-view-month td').each(function(i) {
            $(this).find('.events-view-more').remove();
            $.removeData(this, "apptCount");
            $.removeData(this, "appointments");
        });
    }

    function viewMore(day, calInstance) {
        var appointments = day.data('appointments'),
        elemWidth = day.outerWidth() + 1,
        self = this;

        day.formBubble({
            graphics: {
                close: true,
                pointer: false
            },
            offset: {
                x: -elemWidth,
                y: 0
            },
            animation: {
                slide: false,
                speed: 0
            },
            callbacks: {
                onOpen: function() {
                    var bubble = $.fn.formBubble.bubbleObject;

                    bubble.addClass('overlay');
                },
                onClose: function() {
                    calInstance.fullCalendar('unselect');
                }
            },
            content: function() {
                var startDate = getDateFromCell(day, calInstance),
            startDateLabel = $.fullCalendar.formatDate(startDate, 'MMM dd'),
            dayValue = parseInt(day.find('.fc-day-number').text()),
            eventList = $('<ul></ul>').prepend('<li><h5 class="ui-widget-header">' + startDateLabel + '</h5></li>');

                elemWidth = elemWidth - 30;

                $(appointments).each(function() {
                    var apptStartDay = parseInt($.fullCalendar.formatDate(this.start, 'd')), //should be comparing date instead of day (bug with gray dates) <-- fix
                    apptEndDay = parseInt($.fullCalendar.formatDate(this.end, 'd')),
                    event = this.element.clone(false).attr('style', '').css('width', elemWidth);

                    if (apptStartDay < dayValue) $(event).addClass('arrow-left');
                    if (apptEndDay > dayValue) $(event).addClass('arrow-right');

                    event.appendTo(eventList).wrap('<li>');
                });

                eventList.wrap('<div>');
                return eventList.parent('div').html();
            }
        });
    }

    function getCellFromDate(thisDate, calInstance) { //ties events to actual table cells, and also differentiates between "gray" dates and "black" dates
        var start = calInstance.fullCalendar('getView').start,
        end = calInstance.fullCalendar('getView').end,
        td;

        thisDate = new Date(thisDate);

        td = $('.fc-day-number').filter(function() {
            return $(this).text() === $.fullCalendar.formatDate(thisDate, 'd')
        }).closest('td');

        if (thisDate < start) { //date is in last month
            td = td.filter(':first');
        } else if (thisDate >= end) {  //date is in next month
            td = td.filter(':last');
        } else { //date is in this month
            td = td.filter(function() {
                return $(this).hasClass('fc-other-month') === false;
            });
        }

        return td;
    }

    function getDateFromCell(td, calInstance) {
        var cellPos = {
            row: td.parent().parent().children().index(td.parent()),
            col: td.parent().children().index(td)
        };

        return calInstance.fullCalendar('getView').cellDate(cellPos);
    }

}).call(jQuery.fn.limitEvents.constructor.prototype, jQuery);

/*
* FormBubble v0.1.4.5
* Requires jQuery v1.32+
* Created by Scott Greenfield
*
* Copyright 2010, Lyconic, Inc.
* http://www.lyconic.com/
*
* Licensed under the MIT license.
* http://www.opensource.org/licenses/mit-license.php
*
* Most functions can be called programatically enabling you to bind them to your own events.
* These functions must be called *after* the the bubble has been initialized.
*
* Visit http://www.lyconic.com/resources/tools/formbubble for support and the most up to date version.
*
*/
(function($) {
    $.fn.formBubble = function(params) {
        var self = arguments.callee;

        self.p = $.extend({
            alignment: {
                bubble: 'right',
                pointer: 'top-left'
            },
            animation: {
                slide: false,
                speed: 80
            },
            bindings: {
                fadeOnBlur: true,
                fadeOnBlurExceptions: ['.fc-event', '.form-bubble', '.ui-datepicker-calendar', '.ui-datepicker-header', '#jstree-contextmenu'],  //selectors that will not cause the widget to close
                realignOnWindowResize: true
            },
            cache: false,
            callbacks: {
                onOpen: function() { },
                onClose: function() { }
            },
            content: '',
            dataType: 'none',
            graphics: {
                close: true,
                pointer: true
            },
            offset: {
                x: 13,
                y: 3
            },
            unique: true,
            url: 'none'
        }, params);

        return this.each(function() {
            self.init(this);
            if (self.p.url != 'none' && self.p.dataType != 'image') self.ajax();
            self.align(self.bubbleObject, this, self.p.alignment);
            self.open(this);
        });
    };

    $.extend($.fn.formBubble, {
        align: function(bubbleObject, bubbleTarget, alignment) {
            var p = this.p;

            bubbleObject = $(bubbleObject);
            bubbleTarget = $(bubbleTarget);

            if (!$.fn.formBubble.bubbleObject.parents('body').length) return false; //no bubble exists in page yet

            var position = bubbleTarget.offset(),
                top = position.top,
                left = position.left,
                right,
                positionCSS,
                offset = bubbleObject.data('offset') || p.offset,
                hOffset = offset.x,
                vOffset = offset.y;

            if (!alignment) alignment = bubbleObject.data('alignment');

            if (alignment.bubble == 'top') {
                hOffset = hOffset + bubbleObject.outerWidth() / -4;
                vOffset = vOffset + bubbleObject.outerHeight();
            } else if (alignment.bubble === 'right') {
                hOffset = hOffset + bubbleTarget.outerWidth();
                bubbleObject[0].style.right = '';
                bubbleObject.find('.form-bubble-pointer')
                    .removeClass('form-bubble-pointer-top-right')
                    .addClass('form-bubble-pointer-top-left');
            } else if (alignment.bubble == 'left') {
                right = $(window).width() - left - hOffset;
                bubbleObject[0].style.left = '';
                bubbleObject.find('.form-bubble-pointer')
                    .removeClass('form-bubble-pointer-top-left')
                    .addClass('form-bubble-pointer-top-right');
            }

            top = top - vOffset;
            left = left + hOffset;

            if ($.browser.msie && parseInt($.browser.version) <= 7) $.fn.formBubble.browser = 'lte ie7';
            else if (bubbleObject.css("display") != "none") bubbleObject.stop().fadeTo(0, 1);

            positionCSS = (right) ? { 'right': right, 'top': top} : { 'left': left, 'top': top };

            if (p.animation.slide && bubbleObject.css("display") != "none") bubbleObject.stop().animate(positionCSS, p.animation.speed);
            else bubbleObject.css(positionCSS);

            bubbleObject.data({ //set bubble data again with updated information
                target: bubbleTarget,
                alignment: alignment,
                offset: offset
            });
        },
        alignAuto: function(bubbleObject, bubbleTarget) {  //find a way for this to prevent from being fired until the user stops scrolling
            var align = 'right',
                rightSpace = $(window).width() - ($(bubbleTarget).offset().left + $(bubbleTarget).width()),
                leftSpace = $(bubbleTarget).offset().left;

            if (leftSpace > rightSpace) align = 'left';

            $.fn.formBubble.align($.fn.formBubble.bubbleObject, bubbleTarget, {
                bubble: align
            });
        },
        ajax: function() {
            var p = this.p;

            $.ajax({
                beforeSend: function() { $.fn.formBubble.beforeSend(); },
                cache: p.cache,
                type: 'GET',
                url: p.url,
                dataType: p.dataType,
                success: function(data) { $.fn.formBubble.success(data); },
                complete: function(data) { $.fn.formBubble.complete(); }
            });
        },
        beforeSend: function() {
            $($.fn.formBubble.bubbleObject).find('.form-bubble-content')
                .empty()
                .append('<div id="bubble-loading"><img src="/images/loading.gif" alt="Loading..." title="Loading..." /></div>');
        },
        bindings: function(bubbleObject) {
            var p = this.p;

            bubbleObject //close button click and hover state
                .find('.form-bubble-close')
                .hover(function() {
                    $(this).fadeTo(0, .75);
                },
                function() {
                    $(this).fadeTo(0, 1);
                })
                .click(function() {
                    $.fn.formBubble.close(bubbleObject);
                });

            if (!$.fn.formBubble.isBound) { //ensures document-wide events are only bound once
                if (p.bindings.realignOnWindowResize) {
                    $(window).resize(function() {
                        $('.form-bubble').each(function() {
                            var bubble = $(this),
                                target = bubble.data('target');

                            $.fn.formBubble.align(bubble, target);
                            if (p.alignment.bubble === 'auto') $.fn.formBubble.alignAuto(bubble, target);
                        });
                    });
                }

                $(document).click(function(event) {
                    if (event.button === 0 && p.bindings.fadeOnBlur) {
                        var len = p.bindings.fadeOnBlurExceptions.length,
                            doClose = false;

                        for (var i = 0; i < len; ++i) { //loop through close exceptions, determine if click causes bubble to close
                            if ($(event.target).parents(p.bindings.fadeOnBlurExceptions[i]).length || $(event.target).is(p.bindings.fadeOnBlurExceptions[i])) {
                                doClose = false;
                                break;
                            } else {
                                doClose = true; //set it to true... for now
                            }
                        }

                        if (doClose) $.fn.formBubble.close();
                    }
                });
            }

            $.fn.formBubble.isBound = true;
        },
        browser: '',
        close: function(bubbleObject) {
            var p = this.p;

            if (!bubbleObject) bubbleObject = '.form-bubble';
            bubbleObject = $(bubbleObject);

            function remove() {
                bubbleObject.remove();
                p.callbacks.onClose();
            }

            if (bubbleObject.is(':visible') && $.fn.formBubble.browser === 'lte ie7') remove();
            else if (bubbleObject.is(':visible')) bubbleObject.stop().fadeOut(p.animation.speed, function() { remove(); });
        },
        complete: function() {
            $($.fn.formBubble.bubbleObject).find('#bubble-loading').remove();
        },
        destroy: function() { //destroys all formbubbles
            $('.form-bubble').remove();
        },
        init: function(bubbleTarget) {
            var p = this.p;

            bubbleTarget = $(bubbleTarget);

            var bubbleObject = $('<div class="form-bubble ui-widget-content"><div class="form-bubble-content"></div></div>')
                .appendTo('body')
                .data({
                    target: bubbleTarget,
                    alignment: p.alignment,
                    offset: p.offset
                });

            $.fn.formBubble.bubbleObject = bubbleObject;

            if (p.unique) {
                $('.form-bubble.unique').remove(); //close ALL other uniques
                bubbleObject.addClass('unique'); //add class unique to current object
            }

            if (p.graphics.close) bubbleObject.prepend('<div class="form-bubble-close"></div>');
            if (p.graphics.pointer) bubbleObject.append('<div class="form-bubble-pointer form-bubble-pointer-' + p.alignment.pointer + '"></div>');

            $.fn.formBubble.bindings(bubbleObject);
        },
        open: function(bubbleTarget) {
            var p = this.p,
                bubbleObject = $($.fn.formBubble.bubbleObject);

            if (typeof p.content === 'function') p.content = p.content();

            if (p.dataType == 'image') {
                if (bubbleObject.find('.form-bubble-content img').length === 0) {
                    bubbleObject.find('.form-bubble-content').append('<div class="image"><img src="' + p.url + '" /></div>');
                }

                $.fn.formBubble.beforeSend();
                bubbleObject.find('.form-bubble-content').append('<div class="image"><img src="' + p.url + '" /></div>');
                $.fn.formBubble.complete();
            } else if (p.content.length) {
                $.fn.formBubble.content(p.content, bubbleTarget);
            }

            //with auto align, contents must be loaded FIRST so we can measure width to calculate whether or not it will fit
            if (bubbleObject.data('alignment').bubble == 'auto') $.fn.formBubble.alignAuto(bubbleObject, bubbleTarget);

            if (bubbleObject.css("display") === "none") {
                bubbleObject.stop().fadeIn(p.animation.speed, function() {
                    p.callbacks.onOpen();
                });
            }
        },
        success: function(data) {
            var p = this.p;

            var dataValue;

            if (p.dataType == 'json') dataValue = data.html;
            if (p.dataType == 'html') dataValue = data;

            $($.fn.formBubble.bubbleObject).find('.form-bubble-content').append(dataValue);
        },
        content: function(data, bubbleTarget) {
            var bubbleObject = $($.fn.formBubble.bubbleObject);

            if (data == 'targetText') data = $(bubbleTarget).text();

            bubbleObject.find('.form-bubble-content').remove();
            bubbleObject.append('<div class="form-bubble-content">' + data + '</div>');
        }
    });
})(jQuery);
