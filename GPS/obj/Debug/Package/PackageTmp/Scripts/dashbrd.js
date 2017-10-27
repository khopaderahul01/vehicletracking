

function onItemChecked(sender, e) {
    try {
        var item = e.get_item();
        var items = sender.get_items();
        var checked = item.get_checked();
        var firstItem = sender.getItem(0);
        if (item.get_text() == "Select All") {
            items.forEach(function (itm) { itm.set_checked(checked); });
        }
        else {
            if (sender.get_checkedItems().length == items.get_count() - 1) {
                firstItem.set_checked(!firstItem.get_checked());
            }
        }
    }
    catch (err) {
       // alert("Script Error(1).. " + err.Message);
    }
}




var map;
var marker = new Array();
var count = 0;
var flightPath = new Array();
var lineCount = 0;
var circle=new Array();
var circleCount = 0;

function initialize() {
    try {
        var mapOptions =
                 {
                     zoom: 13,
                     center: new google.maps.LatLng(18.52, 73.86),
                     mapTypeId: google.maps.MapTypeId.ROADMAP,
                     overviewMapControl: true,
                     overviewMapControlOptions:
                     {
                         opened: true
                     }
                 }
        var canvasName = "map_canvas";
        map = new google.maps.Map(document.getElementById(canvasName), mapOptions);
        return map;
    }
    catch (err) {
        //alert("Script Error(2).. " + err.Message)
    }
}

window.onload = function () {
    try {
        initialize();
    }
    catch (err) {
        //alert("Script Error(3).. " + err.Message);
    }
}

function clearMarkers() {
    try {
        for (var i = 0; i < this.marker.length; i++) {
            marker[i].setMap(null);
            if (i < circleCount) {
                circle[i].setMap(null);
            }
            if (i < lineCount) {
                flightPath[i].setMap(null);
            }
        }
        marker = new Array();
        count = 0;
        lineCount = 0;
        circleCount = 0;
    }
    catch (err) {
        //alert("Script Error(4).. " + err.Message);
    }
}

function setCenter(lat, long) {
    try {
        map.setCenter(new google.maps.LatLng(lat, long));
        if (this.marker.length > 0) {
            var bounds = new google.maps.LatLngBounds();
            for (var i = 0; i < this.marker.length; i++) {
                bounds.extend(marker[i].position);
                map.fitBounds(bounds);
            }

        }
    }
    catch (err) {
       // alert("Script Error(18).. " + err.Message);
    }
}

function getSize(basePath) {
    try {
        if (basePath == "1") {
            return 38;
        }
        else if (basePath == "2") {
            return 48;
        }
        else if (basePath == "4") {
            return 42;
        }
        else if (basePath == "5") {
            return 32;
        }
    }
    catch (err) {
        //alert("Script Error(5).. " + err.Message);
    }
}

function placeMarker(content, title, lat, long, desc, Angle, basePath, speed,lbsLocation) {
    try {
        
        var pathIco = "icons/" + GetVehIcon("car", Angle, speed, basePath);
        var size = getSize(basePath);
        var image = new google.maps.MarkerImage(pathIco, new google.maps.Size(size, size),
                new google.maps.Point(0, 0),
                new google.maps.Point(size / 2, size / 2)
                );

        var myLatlng = new google.maps.LatLng(lat, long);

       // alert(lbsLocation);

        if (lbsLocation == 1) {
        
            var populationOptions = {
                strokeColor: "#FF0000",
                strokeOpacity: 0.8,
                strokeWeight: 2,
                fillColor: "#FF0000",
                fillOpacity: 0.35,
                map: map,
                center: myLatlng,
                radius: 100
            };
            circle[circleCount] = new google.maps.Circle(populationOptions);
            circleCount++;
        }





        marker[count] = new google.maps.Marker
                ({
                    position: myLatlng,
                    map: map,
                    draggable: false,
                    title: title,
                    icon: image,
                    animation: google.maps.Animation.NONE
                });


        var infowindow = new google.maps.InfoWindow
                 ({
                     content: content
                 });
        var temp = count;
        google.maps.event.addListener(marker[temp], 'click', function () {
            infowindow.open(map, marker[temp]);
        });

        count++;
    }
    catch (err) {
        //alert("Script Error(6).. " + err.Message);
    }
}

function drawPath() {
    try {
        var bounds = new google.maps.LatLngBounds();
        for (var i = 0; i < count - 1; i++) {
            var flightPlanCoordinates = [marker[i].position, marker[i + 1].position];
            flightPath[lineCount] = new google.maps.Polyline
                     ({
                         path: flightPlanCoordinates,
                         strokeColor: "#FF0000",
                         strokeOpacity: 1.0,
                         strokeWeight: 2
                     });
            flightPath[lineCount].setMap(map);
            lineCount++
            bounds.extend(marker[i].position);
        }
        map.fitBounds(bounds);
    }
    catch (err) {
       // alert("Script Error(7).. " + err.Message);
    }
}

function stop(content, title, lat, long, desc, Angle, pathIco) {
    try {
        var image = new google.maps.MarkerImage(pathIco, new google.maps.Size(42, 42),
                new google.maps.Point(0, 0),
                new google.maps.Point(21, 21));
        var myLatlng = new google.maps.LatLng(lat, long);
        marker[count] = new google.maps.Marker
                ({
                    position: myLatlng,
                    map: map,
                    draggable: false,
                    title: title,
                    icon: image,
                    animation: google.maps.Animation.NONE
                });

        var infowindow = new google.maps.InfoWindow
                 ({
                     content: content
                 });
        var temp = count;
        google.maps.event.addListener(marker[temp], 'click', function () {
            infowindow.open(map, marker[temp]);
        });
        count++;
    }
    catch (err) {
       // alert("Script Error(8).. " + err.Message);
    }
}

function GetVehIcon(vehicle, Angle, speed, carrierTypeFIDGlobal) {
    try {
        var iconName;
        iconName = "car";
        if (speed <= 3) {
            vehicle = carrierTypeFIDGlobal + "/red"
        }
        else {
            vehicle = carrierTypeFIDGlobal + "/green"
        }

        if (Angle > 350 || Angle < 10)
            return vehicle + "/" + iconName + "0.png";
        else if (Angle >= 10 && Angle <= 35)
            return vehicle + "/" + iconName + "0_45.png";
        else if (Angle > 35 && Angle < 55)
            return vehicle + "/" + iconName + "45.png";
        else if (Angle >= 55 && Angle <= 80)
            return vehicle + "/" + iconName + "45_90.png";
        else if (Angle > 80 && Angle < 100)
            return vehicle + "/" + iconName + "90.png";
        else if (Angle >= 100 && Angle <= 125)
            return vehicle + "/" + iconName + "90_135.png";
        else if (Angle > 125 && Angle < 145)
            return vehicle + "/" + iconName + "135.png";
        else if (Angle >= 145 && Angle <= 170)
            return vehicle + "/" + iconName + "135_180.png";
        else if (Angle > 170 && Angle < 190)
            return vehicle + "/" + iconName + "180.png";
        else if (Angle >= 190 && Angle <= 215)
            return vehicle + "/" + iconName + "180_225.png";
        else if (Angle > 215 && Angle < 235)
            return vehicle + "/" + iconName + "225.png";
        else if (Angle >= 235 && Angle <= 260)
            return vehicle + "/" + iconName + "225_270.png";
        else if (Angle > 260 && Angle < 280)
            return vehicle + "/" + iconName + "270.png";
        else if (Angle >= 280 && Angle <= 305)
            return vehicle + "/" + iconName + "270_315.png";
        else if (Angle > 305 && Angle < 325)
            return vehicle + "/" + iconName + "315.png";
        else if (Angle >= 325 && Angle <= 350)
            return vehicle + "/" + iconName + "315_350.png";
        return vehicle + "/" + vehicle + "0.png";
    }
    catch (err) {
       // alert("Script Error(9).. " + err.Message);
    }
}
