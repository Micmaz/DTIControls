var iframe;

/**
 * Override to also look for elements in the iframe
 */
hs.$ = function (id) {
    //hs.zIndexCounter = 20000;
	var iDoc = iframe.contentDocument || iframe.contentWindow.document;

	if (document.getElementById(id)) return document.getElementById(id);
	else if (iDoc.getElementById(id)) {
		// workaround for IE's missing importNode():
		iEl = iDoc.getElementById(id);
		return hs.createElement(iEl.tagName, { className: iEl.className, innerHTML: iEl.innerHTML } );
	}
	return null;
};

/**
 * Override to look for anchors within the iframe
 */
hs.Expander.prototype.getAdjacentAnchor = function(op) {
	
	var iDoc = iframe.contentDocument || iframe.contentWindow.document;

	var aAr = iDoc.getElementsByTagName('A'), hsAr = {}, activeI = -1, j = 0;
	for (i = 0; i < aAr.length; i++) {
		if (hs.isHsAnchor(aAr[i]) && aAr[i].className.indexOf('highslide') > -1 && ((this.slideshowGroup == hs.getParam(aAr[i], 'slideshowGroup')))) {
			hsAr[j] = aAr[i];
			if (aAr[i] == this.a) activeI = j;
			j++;
		}
	}
	return hsAr[activeI + op];
};

/**
 * Override to index anchors in the iframe
 */
hs.updateAnchors = function() {
	var el, els, all = [], images = [], htmls = [], groups = {}, re;


	var iDoc = iframe.contentDocument || iframe.contentWindow.document;

	for (var i = 0; i < hs.openerTagNames.length; i++) { /// loop through tag names
		els = iDoc.getElementsByTagName(hs.openerTagNames[i]);
		for (var j = 0; j < els.length; j++) { /// loop through each element
			el = els[j];
			re = hs.isHsAnchor(el);
			if (re) {
				hs.push(all, el);
				/// images
				if (re[0] == 'hs.expand') hs.push(images, el);
				/// htmls
				else if (re[0] == 'hs.htmlExpand') hs.push(htmls, el);
				/// groupwise
				var g = hs.getParam(el, 'slideshowGroup') || 'none';
				if (!groups[g]) groups[g] = [];
				hs.push(groups[g], el);
			}
		}
	}
	hs.anchors = { all: all, groups: groups, images: images, htmls: htmls };
	return hs.anchors;

};

/**
 * Override to add the offset of the iframe itself
 */
hs.getPosition = function(el)	{
	var parent = el;
	var p = { x: parent.offsetLeft, y: parent.offsetTop };
	while (parent.offsetParent)	{
		parent = parent.offsetParent;
		p.x += parent.offsetLeft;
		p.y += parent.offsetTop;
		if (parent != document.body && parent != document.documentElement) {
			p.x -= parent.scrollLeft;
			p.y -= parent.scrollTop;
		}
	}

    // add the offset of the iframe itself
	if (!/IFRAME/.test(el.tagName)) {
		var iframePos = hs.getPosition(iframe);
		p.x += iframePos.x;
		p.y += iframePos.y;
	}

	return p;
};

////function buildZMax() {
////    $('*').each(function() {
////      var cur = parseInt($(this).css('zIndex'));
////      zmax = cur > zmax ? $(this).css('zIndex') : zmax;
////    });
////    return zmax;
////  }

//function buildZMax() {
//    var maxZ = Math.max.apply(null,$.map($('body > *'), function(e,n){
//           if($(e).css('position')=='absolute')
//                return parseInt($(e).css('z-index'))||1 ;
//           })
//    );
//    return maxZ
//}