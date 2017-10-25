        var myMaps = new Array();
        var mapCounter = 0;
        
        function mapObject(addr, address_title, map_div_id, directions_div_id, client_id, map_index, dir_width, dir_height){
            var parent = $("#" + map_div_id).parent().parent();
            var parentWidth = parent.innerWidth();
            var parentHeight = parent.innerWidth();
            var ele = $("#" + map_div_id);
            if (ele.outerWidth() > parentWidth) {
                ele.width(parentWidth);
                ele.parent().width(parentWidth);
            };
            if (ele.outerHeight() > parentHeight) {
                ele.height(parentHeight);
                ele.parent().height(parentHeight);
            };
            this.address = addr;
            this.map = new GMap2(document.getElementById(map_div_id));
            this.mapId = map_div_id;
            this.dirId = directions_div_id;
            this.gdir = new GDirections(this.map, document.getElementById(directions_div_id));
            this.geocoder = new GClientGeocoder();
            this.clientId = client_id;
            this.to_htmls = null;
            this.from_htmls = null;
            this.marker = null;
            this.index = map_index;
            this.dirWidth = dir_width;
            this.dirHeight = dir_height;
            
            if (address_title > '') {
                this.title = "<strong>" + address_title + "</strong><br />";
            }
            else {
                this.title = "";
            }
        }

        function initialize(address, addressTitle, map_div_id, directions_div_id, client_id, dir_width, dir_height) {
          if (GBrowserIsCompatible()) {
            var myMapObject = new mapObject(address, addressTitle, map_div_id, directions_div_id, client_id, mapCounter, dir_width, dir_height)
            myMapObject.map.setUIToDefault();
            showAddress(myMapObject);
            myMaps[mapCounter] = myMapObject;
            mapCounter++; 
            myMapObject.map.checkResize();
          }          
          else {
             $("#" + map_div_id).width(0);
             $("#" + map_div_id).height(0);
          }
        }
        
    String.prototype.trim = function () {
        return this.replace(/^\s*/, "").replace(/\s*$/, "");
    }

        function showAddress(myMapObject) {
        
        //var i = gmarkers.length;

          if (myMapObject.geocoder) {
            myMapObject.geocoder.getLocations(
              myMapObject.address,
              function(locations) {
                if (!locations) {
                  //alert(address + " not found");
                } else {
                  var place = locations.Placemark[0];
                  var point = new GLatLng(place.Point.coordinates[1],place.Point.coordinates[0]);
                  var street;
                  var city;
                  var state;
                  var zip;
                  if(place.AddressDetails.Country.AdministrativeArea.SubAdministrativeArea) {
                    street = place.AddressDetails.Country.AdministrativeArea.SubAdministrativeArea.Locality.Thoroughfare.ThoroughfareName;
                    city = place.AddressDetails.Country.AdministrativeArea.SubAdministrativeArea.Locality.LocalityName;
                    zip = place.AddressDetails.Country.AdministrativeArea.SubAdministrativeArea.Locality.PostalCode.PostalCodeNumber;
                  }
                  else {
                    street = place.AddressDetails.Country.AdministrativeArea.Locality.Thoroughfare.ThoroughfareName;
                    city = place.AddressDetails.Country.AdministrativeArea.Locality.LocalityName;
                    zip = place.AddressDetails.Country.AdministrativeArea.Locality.PostalCode.PostalCodeNumber;
                  }
                  state = place.AddressDetails.Country.AdministrativeArea.AdministrativeAreaName;
                  
                  myMapObject.map.setCenter(point, 13);
                  myMapObject.marker = new GMarker(point);
                  myMapObject.map.addOverlay(myMapObject.marker);
                  myMapObject.marker.openInfoWindowHtml(myMapObject.title + street + "<br />" + city + ", " + state + "  " + zip + '<br>Directions: <a href="javascript:tohere('+myMapObject.index+')" style="color:Blue; font-weight: bold; text-decoration: underline;">To here<\/a> - <a href="javascript:fromhere('+myMapObject.index+')" style="color:Blue; font-weight: bold; text-decoration: underline;">From here<\/a>');
                  GEvent.addListener(myMapObject.marker, "click", function() {
                    myMapObject.marker.openInfoWindowHtml(myMapObject.title + street + "<br />" + city + ", " + state + "  " + zip + '<br>Directions: <a href="javascript:tohere('+myMapObject.index+')" style="color:Blue; font-weight: bold; text-decoration: underline;">To here<\/a> - <a href="javascript:fromhere('+myMapObject.index+')" style="color:Blue; font-weight: bold; text-decoration: underline;">From here<\/a>');
                    });  
                  GEvent.addListener(myMapObject.gdir, "error", function() {
                      var code = myMapObject.gdir.getStatus().code;                    
                      if (code == 602) {
                        alert("Address could not be found!");
                      }         
                    });            
                }
                // The info window version with the "to here" form open
            myMapObject.to_htmls = '<br>Directions: <b>To here<\/b> - <a href="javascript:fromhere('+myMapObject.index+')">From here<\/a>' +
               '<br>Start address:<form action="javascript:getDirections('+myMapObject.index+')">' +
               '<input type="text" SIZE=40 MAXLENGTH=40 name="saddr" id="saddr_' + myMapObject.clientId + 
               '" value="" /><br><INPUT value="Get Directions" TYPE="SUBMIT"><br>Walk <input type="checkbox"' +
               ' name="walk" id="walk_' + myMapObject.clientId + '" /> &nbsp; Avoid Highways <input type="checkbox"' +
               ' name="highways" id="highways_' + myMapObject.clientId + '" /><input type="hidden" id="daddr_' + 
               myMapObject.clientId + '" value="'+name+"@"+ point.lat() + ',' + point.lng() + '"/>';
            // The info window version with the "from here" form open
            myMapObject.from_htmls = '<br>Directions: <a href="javascript:tohere('+myMapObject.index+')">To here<\/a> - <b>From here<\/b>' +
               '<br>End address:<form action="javascript:getDirections('+myMapObject.index+')">' +
               '<input type="text" SIZE=40 MAXLENGTH=40 name="daddr" id="daddr_' + myMapObject.clientId + 
               '" value="" /><br><INPUT value="Get Directions" TYPE="SUBMIT"><br>Walk <input type="checkbox"' +
               ' name="walk" id="walk_' + myMapObject.clientId + '" /> &nbsp; Avoid Highways <input type="checkbox"' +
               ' name="highways" id="highways_' + myMapObject.clientId + '" /><input type="hidden" id="saddr_' + 
               myMapObject.clientId + '" value="'+name+"@"+ point.lat() + ',' + point.lng() + '"/>';
              }
            );
          }
          

        }
        
        function getDirections(map_index) {
            // ==== Set up the walk and avoid highways options ====
            var opts = {};
            var myMapObject = myMaps[map_index];
            //var code = null;
            if (document.getElementById("walk_" + myMapObject.clientId).checked) {
               opts.travelMode = G_TRAVEL_MODE_WALKING;
            }
            if (document.getElementById("highways_" + myMapObject.clientId).checked) {
               opts.avoidHighways = true;
            }
            // ==== set the start and end locations ====
            var saddr = document.getElementById("saddr_" + myMapObject.clientId).value
            var daddr = document.getElementById("daddr_" + myMapObject.clientId).value
            //gdir.clear();
            myMapObject.gdir.load("from: "+saddr+" to: "+daddr, opts);
            //alert(document.getElementById(myMapObject.dirId));
            //code = gdir.getStatus().code;
            //if(code == 602){
                //document.getElementById("directionsError").Visible = true;
            //    alert("Address could not be found!");
            //    code = 0;
            //}
            
            document.getElementById(myMapObject.dirId).style.height = myMapObject.dirHeight + "px"; 
            document.getElementById(myMapObject.dirId).style.width = myMapObject.dirWidth + "px";
            hs.htmlExpand(null, {maincontentId: myMapObject.dirId, width: myMapObject.dirWidth+50, height: myMapObject.dirHeight+50});
          }

        // functions that open the directions forms
          function tohere(map_index) {
            var myMapObject = myMaps[map_index];
            myMapObject.marker.openInfoWindowHtml(myMapObject.to_htmls);
          }
          function fromhere(map_index) {
            var myMapObject = myMaps[map_index];
            myMapObject.marker.openInfoWindowHtml(myMapObject.from_htmls);
          }
          
