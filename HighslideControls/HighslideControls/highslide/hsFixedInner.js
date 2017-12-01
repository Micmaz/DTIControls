try {
	//a13fe4e6f3a0bf746da103090dd47568
	parent.window.hs.outlineType = 'rounded-white';
	parent.window.hs.graphicsDir = '~/res/BaseClasses/Scripts.aspx?f=HighslideControls/';
	parent.window.hs.showCredits = false;
	parent.window.hs.wrapperClassName = 'draggable-header';
	parent.window.hs.zIndexCounter = 20000;

	parent.window.hs.onActivate = function () {
		var theForm = document.forms[0];
		if (theForm) theForm.appendChild(parent.window.hs.container);
	}
} catch (err) { }

// Highslide fixed popup mod for inner frames. Requires the "Events" component.
if(window.parent && window.parent.hs)
if (!window.parent.hs.ie || window.parent.hs.uaVersion > 6) window.parent.hs.extend ( window.parent.hs.Expander.prototype, {
	    fix: function(on) {
	    if(this.custom) if (this.custom.Fixed) { 
		    var sign = on ? -1 : 1,
			    stl = this.wrapper.style;

		    if (!on) window.parent.hs.getPageSize(); // recalculate scroll positions

		    window.parent.hs.setStyles (this.wrapper, {
			    position: on ? 'fixed' : 'absolute',
			    zoom: 1, // IE7 hasLayout bug,
			    left: (parseInt(stl.left) + sign * window.parent.hs.page.scrollLeft) +'px',
			    top: (parseInt(stl.top) + sign * window.parent.hs.page.scrollTop) +'px'
		    });

		    if (this.outline) {
			    stl = this.outline.table.style;
			    window.parent.hs.setStyles (this.outline.table, {
				    position: on ? 'fixed' : 'absolute',
				    zoom: 1, // IE7 hasLayout bug,
				    left: (parseInt(stl.left) + sign * window.parent.hs.page.scrollLeft) +'px',
				    top: (parseInt(stl.top) + sign * window.parent.hs.page.scrollTop) +'px'
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

