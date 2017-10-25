(function($) {
  $.fn.dtiGallery = function(options) {
    options = $.extend({
      ajaxControlFunction: null,
      component_type: 'Gallery',
      content_type: '',
      effect: "slide",
      effectSpeed: 500,
      backInDirection: "left",
      backOutDirection: "right",
      forwardInDirection: "right",
      forwardOutDirection: "left",
      controlHolderClass: "Control_Holder",
      totalPagesClass: "Pages_Label",
      currentPageTextboxClass: "Page_Textbox",
      thumbContainerClass: "Gallery_Holder",
      buttonContainerClass: "Gallery_Button_Div",
      firstButtonClass: "First_Button",
      prevButtonClass: "Back_Button",
      nextButtonClass: "Forward_Button",
      lastButtonClass: "Last_Button",
      pageButtonClass: "Page_Button",
      searchContainerClass: "Search_Holder",
      searchTextBoxClass: "MediaSearchTextBox",
      sortTextBoxClass: "MediaSortTextBox",
      mediaSearchButtonClass: "MediaSearchButton",
      uploadLinksContainerClass: "Upload_Links",
      freezeFunc: null,
      unfreezeFunc: null,
      isInnerFrame: false,
      returnResultsOnEmptySearch: true,
      additionalCallbacks: {}
    }, options);

  $(this).each(function() {
        obj = $(this);
        disabled = false;
        holder = obj.children('.' + options.controlHolderClass);
        thumbHolder = holder.children('.' + options.thumbContainerClass);
        buttonHolder = holder.children('.' + options.buttonContainerClass);
        currentPageTextBox = buttonHolder.children('.' + options.currentPageTextboxClass);
        totalPageHolder = buttonHolder.children('.' + options.totalPagesClass + '[id$="lblGalleryTotalPages"]');
        totalPages = parseInt(totalPageHolder.html().trim());
        searchHolder = holder.children('.' + options.searchContainerClass);
        searchTextBox = searchHolder.find('.' + options.searchTextBoxClass);
        sortTextBox = searchHolder.find('.' + options.sortTextBoxClass);
        oldPageNumber = 0;
        currPageNumber = 1;
        frozen = false;
        searchText = function() {
            if(searchTextBox && searchTextBox.val()) {
                return searchTextBox.val();
            } else {return '';}
        }
        sortText = function() {
            if(sortTextBox && sortTextBox.val()) {
                return sortTextBox.val();
            } else {return '';}
        }
        getPage = function(pageNum) {
            return thumbHolder.children('[id$="page' + pageNum + '"]:first')
        }
        havePage = function(pageNum) {
            if(pageNum == oldPageNumber) {
                return true;
            } else {
                var pageCount = getPage(pageNum).length
                return pageCount > 0;
            }
        }
        loadPage = function(pageNum, callback, useFreeze) {
            if(!havePage(pageNum)) {
                if(useFreeze && options.freezeFunc) {frozen = true; options.freezeFunc();}
                var found = false;
                var divString = '<div id="' + obj.attr('id') + '_page' + pageNum + '" class="Gallery_Page" style="display:none;position:relative;top:0px;"></div>'
                $.each(thumbHolder.children(), function() {
                    if($(this).attr('class') == "ui-effects-wrapper") {
                        if(parseInt($(this).find('[id*="_page"]').attr('id').substring($(this).find('[id*="_page"]').attr('id').indexOf('_page') + 5)) > pageNum) {
                            found = true;
                            $(this).before(divString);
                            return false;
                        }
                    }
                    else if(parseInt($(this).attr('id').substring($(this).attr('id').indexOf('_page') + 5)) > pageNum) {
                        found = true;
                        $(this).before(divString);
                        return false;
                    }
                });
                if(!found) {
                    thumbHolder.append(divString);
                }
                options.ajaxControlFunction(getPage(pageNum),'loadPage', { 
                    componentType: options.component_type,
                    contentType: options.content_type,
                    searchQuery: searchText(),
                    searchSort: sortText(),
                    pageNumber: pageNum,
                    galleryId: obj.attr("id"),
                    isInnerFrame: options.isInnerFrame,
                    returnResultsOnEmptySearch: options.returnResultsOnEmptySearch
                }, function() {
                    if(callback) {callback(getPage(pageNum), function() {
                        makeAdditionalCalls(getPage(pageNum).attr('id'));
                    });}
                });
            }
            else if(callback) {callback(getPage(pageNum));}
        }
        makeAdditionalCalls = function(id) {
            for( var i = 0; i < options.additionalCallbacks.length; i++) {
                options.additionalCallbacks[i]('#' + id);
            }
        }
        loadAdjacentPages = function() {
            if(currPageNumber + 1 <= totalPages) {loadPage(currPageNumber + 1, function(page, callback) {if(callback) {callback();}});}
            if(currPageNumber - 1 > 0) {loadPage(currPageNumber - 1, function(page, callback) {if(callback) {callback();}});}
        }        
        movePageOut = function(page, direction, callback) {
            page.hide(options.effect, { direction: direction }, options.effectSpeed, callback);
        }
        movePageIn = function(page, direction, callback) {
            page.show(options.effect, { direction: direction }, options.effectSpeed, callback);
        }
        changePageInURL = function() {
           window.location.hash = "#" + $.query.load(window.location.hash).set(obj.attr('id') + 'page', currPageNumber);
        }
        setTotalPages = function(page) {
            totalPages = parseInt(page.children('.totalPagesDiv').html().trim());
            totalPageHolder.html(totalPages);
            if (totalPages < 2) {
                buttonHolder.fadeOut();
            }
            else {
                buttonHolder.fadeIn();
            }
        }
        if ($.query.load(window.location.hash).get(obj.attr('id') + 'page')) {
            if($.query.load(window.location.hash).get(obj.attr('id') + 'page') != currPageNumber) {
                currentPageTextBox.val($.query.load(window.location.hash).get(obj.attr('id') + 'page'));
            }
        }
        if ($.query.load(window.location.hash).get(obj.attr('id') + 'q')) {
            if($.query.load(window.location.hash).get(obj.attr('id') + 'q') != searchText()) {
                searchTextBox.val($.query.load(window.location.hash).get(obj.attr('id') + 'q'));
            }
        }
        if ($.query.load(window.location.hash).get(obj.attr('id') + 's')) {
            if($.query.load(window.location.hash).get(obj.attr('id') + 's') != sortText()) {
                sortTextBox.val($.query.load(window.location.hash).get(obj.attr('id') + 's').replace('+', ' '));
            }
        }
        searchTextBox.change(function() {
            if ($(this).val() === '' && $.query.load(window.location.hash).get(obj.attr('id') + 'q') !== '') {
                window.location.hash = "#" + $.query.load(window.location.hash).remove(obj.attr('id') + 'q');
            } else {
                window.location.hash = "#" + $.query.load(window.location.hash).set(obj.attr('id') + 'q', $(this).val());
            }
        });
        buttonHolder.children('.' + options.firstButtonClass).click(function() {
            if(!disabled) {
                disabled = true;
                oldPageNumber = currPageNumber;
                movePageOut(getPage(currPageNumber), options.backOutDirection, function() {
                    currPageNumber = 1;
                    currentPageTextBox.val(currPageNumber);
                    changePageInURL();
                    loadPage(1, function(page, callback) {
                        page.find('img').batchImageLoad({loadingCompleteCallback: function() {
                            if(frozen && options.unfreezeFunc) {frozen = false; options.unfreezeFunc();}
                            movePageIn(page, options.backInDirection, function() {
                                if(callback) {callback();}
                                disabled = false;
                                if(totalPages > 1) {loadPage(2, function(page, callback) {if(callback) {callback();}});}
                            });
                        }});
                    }, true);
                });
            }
            return false;
        });
        buttonHolder.children('.' + options.prevButtonClass).click(function() {
            if(!disabled && currPageNumber > 1) {
                disabled = true;
                oldPageNumber = currPageNumber;
                movePageOut(getPage(currPageNumber), options.backOutDirection, function() {
                    currentPageTextBox.val(--currPageNumber);
                    changePageInURL();
                    loadPage(currPageNumber, function(page, callback) {
                        page.find('img').batchImageLoad({loadingCompleteCallback: function() {
                            if(frozen && options.unfreezeFunc) {frozen = false; options.unfreezeFunc();}
                            movePageIn(page, options.backInDirection, function() {
                                if(callback) {callback();}
                                disabled = false;
                                if(currPageNumber - 1 > 0) {loadPage(currPageNumber - 1, function(page, callback) {if(callback) {callback();}});}
                                if(currPageNumber + 1 <= totalPages) {loadPage(currPageNumber + 1, function(page, callback) {if(callback) {callback();}});}
                            });
                        }});
                    }, true);
                });
                
            }
            return false;
        });
        buttonHolder.children('.' + options.nextButtonClass).click(function() {
            if(!disabled && currPageNumber < totalPages) {
                //if(options.freezeFunc) {frozen = true; options.freezeFunc();}
                disabled = true;
                oldPageNumber = currPageNumber;
                movePageOut(getPage(currPageNumber), options.forwardOutDirection, function() {
                    currentPageTextBox.val(++currPageNumber);
                    changePageInURL();
                    loadPage(currPageNumber, function(page, callback) {
                        page.find('img').batchImageLoad({loadingCompleteCallback: function() {
                            if(frozen && options.unfreezeFunc) {frozen = false; options.unfreezeFunc();}
                            movePageIn(page, options.forwardInDirection, function() {
                                if(callback) {callback();}
                                disabled = false;
                                loadAdjacentPages();
                            });
					    }});
                        
                    }, true);
                });
            }
            return false;
        });
        buttonHolder.children('.' + options.lastButtonClass).click(function() {
            if(!disabled) {
                disabled = true;
                oldPageNumber = currPageNumber;
                movePageOut(getPage(currPageNumber), options.forwardOutDirection, function() {
                    currPageNumber = totalPages;
                    currentPageTextBox.val(currPageNumber);            
                    changePageInURL();
                    loadPage(currPageNumber, function(page, callback) {
                        page.find('img').batchImageLoad({loadingCompleteCallback: function() {
                            if(frozen && options.unfreezeFunc) {frozen = false; options.unfreezeFunc();}
                            movePageIn(page, options.forwardInDirection, function() {
                                if(callback) {callback();}
                                disabled = false;
                                if(currPageNumber > 1) {loadPage(currPageNumber-1, function(page, callback) {if(callback) {callback();}});}
                            });
                        }});
                    }, true);
                });
            }
            return false;
        });
        buttonHolder.children('.' + options.pageButtonClass).click(function() {
            if(!disabled) {
                disabled = true;
                if(IsNumeric(currentPageTextBox.val())) {
                    currPageNumber = parseInt(currentPageTextBox.val());
                } else {
                    currPageNumber = oldPageNumber;
                    currentPageTextBox.val(currPageNumber);
                } 
                if(currPageNumber <= totalPages && currPageNumber > 0) {
                    currentPage = thumbHolder.children(":visible");
                    oldPageNumber = parseInt($(currentPage).attr('id').substring($(this).attr('id').indexOf('_page') + 5))
                    currentPage.fadeOut(options.effectSpeed, function() {
                        changePageInURL();
                        loadPage(parseInt(currentPageTextBox.val()), function(page, callback) {
                            page.find('img').batchImageLoad({loadingCompleteCallback: function() {
                                if(frozen && options.unfreezeFunc) {frozen = false; options.unfreezeFunc();}
                                page.fadeIn(options.effectSpeed, function() {
                                    if(callback) {callback();}
                                    disabled = false;
                                    loadAdjacentPages();
                                });
                            }});
                        }, true);
                    });  
                }
            }
            return false;
        });
        searchHolder.find('.' + options.mediaSearchButtonClass).click(function() {
            if(!disabled) {
                window.location.hash = "#" + $.query.load(window.location.hash).set(obj.attr('id') + 's', sortTextBox.val());
                disabled = true;
                oldPageNumber = 0;
                getPage(currPageNumber).fadeOut(options.effectSpeed, function() {
                    currentPageTextBox.val(1);
                    currPageNumber = 1;
                    changePageInURL();
                    thumbHolder.empty();            
                    loadPage(1, function(page, callback) {
                        setTotalPages(page);
                        page.find('img').batchImageLoad({loadingCompleteCallback: function() {
                            if(frozen && options.unfreezeFunc) {frozen = false; options.unfreezeFunc();}
                            page.fadeIn(options.effectSpeed, function() {
                                if(callback) {callback();}
                                disabled = false;
                                if(totalPages > 1) {loadPage(2, function(page, callback) {if(callback) {callback();}});}
                            });
                        }});
                    }, true);
                });
            }
            return false;
        });
        if(IsNumeric(currentPageTextBox.val())) {
            currPageNumber = parseInt(currentPageTextBox.val());
        } else {
            currPageNumber = 1;
            currentPageTextBox.val(currPageNumber);
        } 
        loadPage(currPageNumber, function(page, callback) {
            setTotalPages(page);
            changePageInURL();
            page.find('img').batchImageLoad({loadingCompleteCallback: function() {
                if(frozen && options.unfreezeFunc) {frozen = false; options.unfreezeFunc();}
                page.fadeIn(options.effectSpeed, function() {if(callback) {callback();} loadAdjacentPages();});
            }});
        }, true);
    });
  }
})(jQuery);