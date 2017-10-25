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
    'dialogHelper': function (context) {

    var createCustDialog =  function (title,body,callbackFunction,dialogButtonText) {
    // invoke insertText method with 'hello' on editor module.
      if(!dialogButtonText) {dialogButtonText = 'Ok';}
      var ui = $.summernote.ui;
      var $editor = context.layoutInfo.editor;
      var options = context.options;
      var $container = options.dialogsInBody ? $(document.body) : $editor;
      var footer = '<button href="#" class="btn btn-primary ext-iframediag-btn">'+dialogButtonText+'</button>';
      var $dialog =  ui.dialog({
        title: title,
        fade: options.dialogsFade,
        body: body,
        footer: footer
      }).render().appendTo($container);

      var openDialog = function () {
        return $.Deferred(function (deferred) {
          var $dialogBtn = $dialog.find('.ext-iframediag-btn');
          
          ui.onDialogShown($dialog, function () {
            context.triggerEvent('dialog.shown');

            $dialogBtn
              .click(function (event) {
                      event.preventDefault();
                      if(callbackFunction){
                        callbackFunction(context);
                      }
                      deferred.resolve({ action: ' Iframe dialog OK clicked...' });
              });
          });

          ui.onDialogHidden($dialog, function () {
            $dialogBtn.off('click');
            if (deferred.state() === 'pending') {
              deferred.reject();
            }
            $dialog.remove();
          });
          ui.showDialog($dialog);
        });
      };
          
          openDialog()
            .then(function () {
            // [workaround] hide dialog before restore range for IE range focus
            ui.hideDialog($dialog);
          })
          .fail(function () {
            context.invoke('editor.restoreRange');
          });
    };

    var createIframeDialog = function (title,iframeLocation,dialogButtonText) {
      var diagID = 'diag'+Math.round(Math.random()*1000000);
       var body = '<div class="form-group row-fluid">' +
          '<iframe id="'+diagID+'" src="'+iframeLocation+'" width="100%" height="100%" border="0" style="border: 0;" />' +
          '</div>';
      var callbackfunc = function (context){
           if(document.getElementById(diagID).contentWindow.frameFunction){
                          document.getElementById(diagID).contentWindow.frameFunction(context,context.invoke('createRange'));
            }
      };
      createCustDialog(title,body,callbackfunc,dialogButtonText);
    };

  var makeButton = function (buttonContents,tooltip,clickFunction){
      return function(){
          var button = $.summernote.ui.button({
            contents: buttonContents,
            tooltip: tooltip,
            click: clickFunction
          });
        return button.render();
      };
  };

  var makeIframeButton = function (buttonContents,tooltip,iframeURL){
    return function(context){
          var button = $.summernote.ui.button({
            contents: buttonContents,
            tooltip: tooltip,
              click: function () {
                $.summernote.plugins.dialogHelper(context).createIframeDialog(tooltip,iframeURL);
            }
          });
        return button.render();
    };
  };

  return {'createIframeDialog':createIframeDialog,
      'createCustDialog':createCustDialog,
      'makeIframeButton':makeIframeButton,
      'makeButton':makeButton
  };
  }
});
})
);
