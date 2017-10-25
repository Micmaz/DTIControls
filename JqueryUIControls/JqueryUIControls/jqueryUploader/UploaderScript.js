$(function () {

    var ul = $('#upload ul');

    $('#drop a').click(function () {
        // Simulate a click on the file input button
        // to show the file browser dialog
        $(this).parent().find('input').click();
    });

    // Initialize the jQuery File Upload plugin
    $('#upload').fileupload({

        // This element will accept file drag/drop uploading
        dropZone: $('#drop'),

        // This function is called when a file is added to the queue;
        // either via the browse button, or via drag/drop:
        add: function (e, data) {

            var tpl = $('<li class="working ulfile"><input type="text" value="0" data-width="48" data-height="48"' +
                ' data-fgColor="#0788a5" data-readOnly="1" data-bgColor="#3e4043" /><p></p><span class="delitem"></span></li>');
            var filename = data.files[0].name;
            // Append the file name and file size
            tpl.find('p').append("" + filename + "")
                         .append('<i>' + formatFileSize(data.files[0].size) + '</i>');

            // Add the HTML to the UL element
            data.context = tpl.appendTo(ul);

            // Initialize the knob plugin
            tpl.find('input').knob();

            // Listen for clicks on the cancel icon
            tpl.find('span').click(function () {

                if (tpl.hasClass('working')) {
                    jqXHR.abort();
                    tpl.fadeOut(function () { tpl.remove(); });
                } else {
                    var li = $(this).parent();
                    $.get($(location).attr('href') + "?doRemove=true&removeFile=" + li.text().replace(li.find('i').text(), ''),
                        function (resp) {
                            li.fadeOut(function () { li.remove(); });
                        }
                    );
                }



            });

            // Automatically upload the file once it is added to the queue
            var jqXHR = data.submit();
        },

        progress: function (e, data) {

            // Calculate the completion percentage of the upload
            var progress = parseInt(data.loaded / data.total * 100, 10);

            // Update the hidden input field and trigger a change
            // so that the jQuery knob plugin knows to update the dial
            data.context.find('input').val(progress).change();

            if (progress == 100) {
                var itm = $(data.context);
                itm.removeClass('working');
                setTimeout(function () {
                    itm.find('span').animate({ 'background-position-y': '90%' }, 1000);
                }, 2000);
                data.context.find('span').delay(2000).animate({ 'background-position-y': '90%' }, 1000)
            }
        },

        fail: function (e, data) {
            // Something has gone wrong!
            data.context.addClass('error');
        }

    });


    // Prevent the default action when a file is dropped on the window
    $(document).on('drop dragover', function (e) {
        e.preventDefault();
    });

    // Helper function that formats the file sizes
    function formatFileSize(bytes) {
        if (typeof bytes !== 'number') {
            return '';
        }

        if (bytes >= 1000000000) {
            return (bytes / 1000000000).toFixed(2) + ' GB';
        }

        if (bytes >= 1000000) {
            return (bytes / 1000000).toFixed(2) + ' MB';
        }

        return (bytes / 1000).toFixed(2) + ' KB';
    }

    $("#drop a").button();

    var files = $("#currentfiles").text().split("#");
    // Iterate through each value
    for (var i = 0; i < files.length; i++) {
        if (files[i] != "") {
            var filename = files[i].split(",")[0];
            var filesize = Number(files[i].split(",")[1]);
            var tpl = $('<li class="ulfile"><input type="text" value="100" data-width="48" data-height="48"' +
                    ' data-fgColor="#0788a5" data-readOnly="1" data-bgColor="#3e4043" /><p></p><span></span></li>');

            // Append the file name and file size
            tpl.find('p').append("" + filename + "").append('<i>' + formatFileSize(filesize) + '</i>');
            tpl.appendTo(ul);
            // Initialize the knob plugin
            tpl.find('input').knob();
            // Listen for clicks on the cancel icon
            tpl.find('span').click(function () {
                var li = $(this).parent();
                $.get($(location).attr('href') + "?doRemove=true&removeFile=" + li.text().replace(li.find('i').text(), ''),
                    function (resp) {
                        li.fadeOut(function () { li.remove(); });
                    }
                );
            });
        }

    }
    $('.ulfile').find('span').delay(2000).animate({ 'background-position-y': '90%' }, 1000)
});