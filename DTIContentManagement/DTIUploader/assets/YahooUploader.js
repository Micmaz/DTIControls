	// Custom URL for the uploader swf file (same folder).
	YAHOO.widget.Uploader.SWFURL = "~/res/BaseClasses/Scripts.aspx?d=&f=DTIUploader/uploader.swf";
	
	var myUploaders = {};
	var uploaderCounter = 0;
	
	function uploaderObject(uploaderOverlayId, uploadPath, dataTableContainerId, selectFilesButtonId, fileFilter, redirectURL, errorMessageId){
	    var that = this;
	
        // Instantiate the uploader and write it to its placeholder div.
        this.uploader = new YAHOO.widget.Uploader(uploaderOverlayId);
        
        // Variable for holding the filelist.
	    this.fileList;
	    
	    // When the Flash layer is clicked, the "Browse" dialog is invoked.
	    // The click event handler allows you to do something else if you need to.
	    this.handleClick = function () {
	    }
    	
	    // When contentReady event is fired, you can call methods on the uploader.
	    this.handleContentReady = function () {
	        // Allows the uploader to send log messages to trace, as well as to YAHOO.log
		    that.uploader.setAllowLogging(true);
    		
		    // Allows multiple file selection in "Browse" dialog.
		    that.uploader.setAllowMultipleFiles(true);
    		
		    // New set of file filters.
		    var ff;
		    if (fileFilter == 'All') {
		        ff = new Array({description:"All", extensions:"*.*"});
		    }
		    else if(fileFilter == 'Images') {
		        ff = new Array({description:"Images", extensions:"*.jpg;*.png;*.gif;"}); //*.exe;*.zip;*.mp3
		    }
		    else if(fileFilter == 'Videos') {
		        ff = new Array({description:"Videos", extensions:"*.avi;*.mov;*.mpg;*.wmv;"});
		    }
		    else if(fileFilter == 'ImagesAndVideo') {
		        ff = new Array({description:"Images and Videos", extensions:"*.avi;*.mov;*.mpg;*.wmv;*.jpg;*.png;*.gif;"});
		    }
		        		                   
		    // Apply new set of file filters to the uploader.
		    that.uploader.setFileFilters(ff);
	    }

	    // Actually uploads the files. In this case,
	    // uploadAll() is used for automated queueing and upload 
	    // of all files on the list.
	    // You can manage the queue on your own and use "upload" instead,
	    // if you need to modify the properties of the request for each
	    // individual file.
	    this.upload = function () {
	        if (that.fileList != null) {
		        that.uploader.setSimUploadLimit(1);
		        that.uploader.uploadAll(uploadPath, "POST", null, "Filedata");
	        }	
	    }
    	
	    // Fired when the user selects files in the "Browse" dialog
	    // and clicks "Ok".
	    this.onFileSelect = function (event) {
	        $("#uploadFilesLink").css("visibility","visible");
	        //$("#uploadFilesLink").fadeIn(400);
	        if('fileList' in event && event.fileList != null) {
			    that.fileList = event.fileList;
			    that.createDataTable(that.fileList, dataTableContainerId);    
		    }
		}
    	
	    this.handleClearFiles = function () {
	        $("#uploadFilesLink").css("visibility","hidden");
	        that.uploader.clearFileList();
	        fileID = null;
        	
	        var dataTableContainer = document.getElementById(dataTableContainerId);
	        dataTableContainer.innerHTML = "";
	    }

        // Do something on each file's upload start.
	    this.onUploadStart = function (event) {
	    //alert('started');
    	
	    }
    	
	    // Do something on each file's upload progress event.
	    this.onUploadProgress = function (event) {
		    rowNum = that.fileIdHash[event["id"]];
		    prog = Math.round(100*(event["bytesLoaded"]/event["bytesTotal"]));
		    progbar = "<div style='height:5px;width:100px;background-color:#ccc;'><div style='height:5px;background-color:#049F07;width:" + prog + "px;'></div></div>";
		    mess = "Processing..."
		    that.singleSelectDataTable.updateRow(rowNum, {name: that.dataArr[rowNum]["name"], size: that.dataArr[rowNum]["size"], progress: progbar, message: mess});	
	    }
    	
	    // Do something when each file's upload is complete.
	    this.onUploadComplete = function (event) {
		    rowNum = that.fileIdHash[event["id"]];
		    prog = Math.round(100*(event["bytesLoaded"]/event["bytesTotal"]));
		    progbar = "<div style='height:5px;width:100px;background-color:#ccc;'><div style='height:5px;background-color:#049F07;width:100px;'></div></div>";
		    mess = "Complete"
		    that.singleSelectDataTable.updateRow(rowNum, {name: that.dataArr[rowNum]["name"], size: that.dataArr[rowNum]["size"], progress: progbar, message: mess});
		    //if (event["id"] == that.dataArr[that.dataArr.length - 1].id && redirectURL != "") {
                ////setTimeout(function() {window.location = redirectURL;}, 3000);
                //window.location = redirectURL;
            //}
		}
    	
	    // Do something if a file upload throws an error.
	    // (When uploadAll() is used, the Uploader will
	    // attempt to continue uploading.
	    this.onUploadError = function (event) {
            document.getElementById(errorMessageId).style.display = 'block';
	    }
    	
    	
	    // Do something if an upload is cancelled.
	    this.onUploadCancel = function (event) {
            //uploader.cancel(event["id"]);
           }
    	
	    // Do something when data is received back from the server.
	    this.onUploadResponse = function (event) {
	        if (event["id"] == that.dataArr[that.dataArr.length - 1].id && redirectURL != "") {
                window.location = redirectURL;
            }
	    }
	    
	    this.createDataTable = function (entries, dataTableContainerId) {
	      rowCounter = 0;
	      that.fileIdHash = {};
	      that.dataArr = [];
	      for(var i in entries) {
	         var entry = entries[i];
		     entry["progress"] = "<div style='height:5px;width:100px;background-color:#CCC;'></div>";
		     entry["message"] = ""
		     that.dataArr.unshift(entry);
	      }
    	
	      for (var j = 0; j < that.dataArr.length; j++) {
	        that.fileIdHash[that.dataArr[j].id] = j;
	      }
    	
	        var myColumnDefs = [
	            {key:"name", label: "File Name", sortable:false},
	     	    {key:"size", label: "Size", sortable:false},
	     	    {key:"progress", label: "Upload progress", sortable:false},
	     	    {key:"message", label: "", sortable:false}
	        ];

	      myDataSource = new YAHOO.util.DataSource(that.dataArr);
	      myDataSource.responseType = YAHOO.util.DataSource.TYPE_JSARRAY;
          myDataSource.responseSchema = {
              fields: ["id","name","created","modified","type", "size", "progress", "message"]
          };

	      that.singleSelectDataTable = new YAHOO.widget.DataTable(dataTableContainerId,
	               myColumnDefs, myDataSource, {
	                   caption:"",
	                   selectionMode:"single"
	               });
        }
        
        this.initDOM = function () { 
            var uiLayer = YAHOO.util.Dom.getRegion(selectFilesButtonId);
            var overlay = YAHOO.util.Dom.get(uploaderOverlayId);
            YAHOO.util.Dom.setStyle(overlay, 'width', uiLayer.right-uiLayer.left + "px");
            YAHOO.util.Dom.setStyle(overlay, 'height', uiLayer.bottom-uiLayer.top + "px");
        }
    }     
    
    function initialize(uploaderOverlayId, uploadPath, dataTableContainerId, selectFilesButtonId, uploadFilesId, fileFilter, clearFilesId, redirectURL, errorMessageId) {
        var myUploaderObject = new uploaderObject(uploaderOverlayId, uploadPath, dataTableContainerId, selectFilesButtonId, fileFilter, redirectURL, errorMessageId);
        
        // Add event listeners to various events on the uploader.
	    // Methods on the uploader should only be called once the 
	    // contentReady event has fired.	
	    myUploaderObject.uploader.addListener('contentReady', myUploaderObject.handleContentReady);
	    myUploaderObject.uploader.addListener('fileSelect', myUploaderObject.onFileSelect);
	    myUploaderObject.uploader.addListener('uploadStart', myUploaderObject.onUploadStart);
	    myUploaderObject.uploader.addListener('uploadProgress', myUploaderObject.onUploadProgress);
	    myUploaderObject.uploader.addListener('uploadCancel', myUploaderObject.onUploadCancel);
	    myUploaderObject.uploader.addListener('uploadComplete', myUploaderObject.onUploadComplete);
	    myUploaderObject.uploader.addListener('uploadCompleteData', myUploaderObject.onUploadResponse);
	    myUploaderObject.uploader.addListener('uploadError', myUploaderObject.onUploadError);
        myUploaderObject.uploader.addListener('click', myUploaderObject.handleClick);
        
        YAHOO.util.Event.onDOMReady(myUploaderObject.initDOM);
        
        document.getElementById(uploadFilesId).onclick = myUploaderObject.upload;
        document.getElementById(clearFilesId).onclick = myUploaderObject.handleClearFiles;
    }
