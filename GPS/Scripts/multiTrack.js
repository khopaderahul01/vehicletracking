var map = new Array();
var markerGlobal = new Array();
function initialize(count) {
    var mapOptions = {
        zoom: 16,
        center: new google.maps.LatLng(18.53, 74),
        mapTypeId: google.maps.MapTypeId.ROADMAP
    }
    var canvasName = "map_canvas" + count;


    map[count - 1] = new google.maps.Map(document.getElementById(canvasName),
            mapOptions);
}

function setZoom(count, zoomLevel) {

    map[count - 1].setZoom(zoomLevel);
}




function placeMarker(count, content, title, lat, long, desc, angle, speed, basePath) {

    initialize(count);

    addMarker(count, content, title, lat, long, desc, angle, speed, basePath);


}


function addMarker(count, content, title, lat, long, desc, Angle, speed, basePath) {

    var infowindow = new google.maps.InfoWindow({
        content: content
    });
    document.getElementById("mapDiv" + count).innerHTML = desc;
    var myLatlng = new google.maps.LatLng(lat, long);


    map[count - 1].setCenter(new google.maps.LatLng(lat, long));
    var pathIco = "icons/" + GetVehIcon("car", Angle, speed, basePath);

    var size = getSize(basePath);
    var image = new google.maps.MarkerImage(pathIco, new google.maps.Size(size, size),
                new google.maps.Point(0, 0),
                new google.maps.Point(size / 2, size / 2)
                );



    if (markerGlobal[count - 1] != null) {

        if (markerGlobal[count - 1].getPosition() != myLatlng) {
            markerGlobal[count - 1].setIcon();
        }

        markerGlobal[count - 1] = new google.maps.Marker({
            position: myLatlng,
            map: map[count - 1],
            draggable: false,
            title: title,
            icon: image,
            animation: google.maps.Animation.DROP
        });


        google.maps.event.addListener(markerGlobal[count - 1], 'click', function () {
            infowindow.open(map[count - 1], markerGlobal[count - 1]);
        });
    }
    else {

        var marker = new google.maps.Marker({
            position: myLatlng,
            map: map[count - 1],
            draggable: false,
            title: title,
            icon: image,
            animation: google.maps.Animation.DROP
        });
        markerGlobal[count - 1] = marker;
        google.maps.event.addListener(marker, 'click', function () {
            infowindow.open(map[count - 1], marker);
        });

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
        alert("Script Error(5).. " + err.Message);
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
        alert("Script Error(9).. " + err.Message);
    }
}

function addLine(count, lat1, long1, lat2, long2) {
    var flightPlanCoordinates = [
            new google.maps.LatLng(lat1, long1),
            new google.maps.LatLng(lat2, long2)
          ];
    var flightPath = new google.maps.Polyline({
        path: flightPlanCoordinates,
        strokeColor: "#FF0000",
        strokeOpacity: 1.0,
        strokeWeight: 2
    });

    flightPath.setMap(map[count - 1]);

}


function showAlert(message) {
    alert(message);
}