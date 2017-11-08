try {
	//a13fe4e6f3a0bf746da103090dd47568
	hs.outlineType = 'rounded-white';
	hs.graphicsDir = '~/res/BaseClasses/Scripts.aspx?f=HighslideControls/';
	hs.showCredits = false;
	hs.wrapperClassName = 'draggable-header';
	hs.zIndexCounter = 20000;

	hs.onActivate = function () {
		var theForm = document.forms[0];
		if (theForm) theForm.appendChild(hs.container);
	}
} catch (err) { }
// Highslide fixed popup mod. Requires the "Events" component.
if (!hs.ie || hs.uaVersion > 6) hs.extend ( hs.Expander.prototype, {
	fix: function (on) {
		var index_highest = 0;
		if (index_highest == 0) {
			$("*").each(function () {
				// always use a radix when using parseInt
				var index_current = parseInt($(this).css("zIndex"), 10);
				if (index_current > index_highest) {
					index_highest = index_current;
				}
			});
		}
		$(".highslide-container").css("z-index", index_highest + 1);
	if(this.custom) if (this.custom.Fixed) { 
		var sign = on ? -1 : 1,
			stl = this.wrapper.style;
		if (!on) hs.getPageSize(); // recalculate scroll positions

		hs.setStyles (this.wrapper, {
			position: on ? 'fixed' : 'absolute',
			zoom: 1, // IE7 hasLayout bug,
			left: (parseInt(stl.left) + sign * hs.page.scrollLeft) +'px',
			top: (parseInt(stl.top) + sign * hs.page.scrollTop) + 'px',
			zindex: index_highest
		});

		if (this.outline) {
			stl = this.outline.table.style;
			hs.setStyles (this.outline.table, {
				position: on ? 'fixed' : 'absolute',
				zoom: 1, // IE7 hasLayout bug,
				left: (parseInt(stl.left) + sign * hs.page.scrollLeft) +'px',
				top: (parseInt(stl.top) + sign * hs.page.scrollTop) +'px'
			});

		}
		this.fixed = on; // flag for use on dragging
		}
	},
	onAfterExpand: function() {
    	this.fix(true); // fix the popup to viewport coordinates
	},

	onBeforeClose: function() {
		this.fix(false); // unfix to get the animation right
	},

    onDrop: function() {
    	this.fix(true); // fix it again after dragging
	},

	onDrag: function(sender, args) {
		if (this.fixed) { // only unfix it on the first drag event
			this.fix(false);
		}
	}

});
