(function (factory) {
  /* global define */
  if (typeof define === 'function' && define.amd) {
    // AMD. Register as an anonymous module.
    define(['jquery'], factory);
  } else if (typeof module === 'object' && module.exports) {
    // Node/CommonJS
    module.exports = factory(require('jquery'));
  } else {
    // Browser globals
    factory(window.jQuery);
  }
}(function ($) {

  // minimal dialog plugin
  $.extend($.summernote.plugins, {
    /**
     * @param {Object} context - context object has status of editor.
     */
    'minidiag': function (context) {
      var self = this;

      // ui has renders to build ui elements.
      //  - you can create a button with `ui.button`
      var ui = $.summernote.ui;

			var $editor = context.layoutInfo.editor;
			var options = context.options;
			
      // add context menu button
      context.memo('button.minidiag', function () {
        return ui.button({
          contents: '<i class="fa fa-file-o"/>',
          tooltip: 'mini dialog',
          click: context.createInvokeHandler('minidiag.showDialog')
        }).render();
      });

      // This method will be called when editor is initialized by $('..').summernote();
      // You can create elements for plugin]
      self.initialize = function () {
				var $container = options.dialogsInBody ? $(document.body) : $editor;
				var randNum = Math.round(Math.random()*1000000);
				var body = '<div class="form-group row-fluid">' +
						'<iframe id="minidiag" src="iframe.html" width="100%" height="100%" border="0" style="border: 0;" />' +
						'</div>';
				var footer = '<button href="#" class="btn btn-primary ext-minidiag-btn">OK</button>';

				self.$dialog = ui.dialog({
					title: 'minimal dialog title',
					fade: options.dialogsFade,
					body: body,
					footer: footer
				}).render().appendTo($container);
      };

      // This methods will be called when editor is destroyed by $('..').summernote('destroy');
      // You should remove elements on `initialize`.
      self.destroy = function () {
        self.$dialog.remove();
        self.$dialog = null;
      };

			self.showDialog = function () {
				self
					.openDialog()
					.then(function (dialogData) {
						// [workaround] hide dialog before restore range for IE range focus
						ui.hideDialog(self.$dialog);
						context.invoke('editor.restoreRange');
						
						// do something with dialogData
						console.log("dialog returned: ", dialogData)
						// ...
					})
					.fail(function () {
						context.invoke('editor.restoreRange');
					});

			};
			
	    self.openDialog = function () {
				return $.Deferred(function (deferred) {
					var $dialogBtn = self.$dialog.find('.ext-minidiag-btn');
					
					ui.onDialogShown(self.$dialog, function () {
						context.triggerEvent('dialog.shown');

						$dialogBtn
						  .click(function (event) {
                			event.preventDefault();
                			if(document.getElementById('minidiag').contentWindow.frameFunct){
                				document.getElementById('minidiag').contentWindow.frameFunct(context.invoke('createRange'));
                			}
                		deferred.resolve({ action: 'mini dialog OK clicked...' });
              });
					});

					ui.onDialogHidden(self.$dialog, function () {
						$dialogBtn.off('click');

						if (deferred.state() === 'pending') {
							deferred.reject();
						}
					});

					ui.showDialog(self.$dialog);
				});
			};

    }
  });

}));
