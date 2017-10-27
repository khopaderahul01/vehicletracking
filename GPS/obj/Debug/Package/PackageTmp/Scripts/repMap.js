
var map;

var lat = [];
var long = [];
var title;
var speedlocal;
var timelocal;
var maxSpeedlocal;
var anglelocal;
var din1local;
var din2local;
var lbsLocationlocal;
var addresslocal;
var total;
var carrierNameGlobal;
var iterator = 0;
var stopFlag = 0;
var timer;
var marker;
var DrawStart = false;
var mlength = 0;
var firstFlag = 0;
var mileage = 0;
var maxSpeedGlobal;
var carrierTypeFIDGlobal;

function initialize() {
    var mapOptions = {
        zoom: 16,
        center: new google.maps.LatLng(18.53, 74),
        mapTypeId: google.maps.MapTypeId.ROADMAP
    }
    var canvasName = "map_canvas";


    map = new google.maps.Map(document.getElementById(canvasName),
            mapOptions);


    return map;
}



function plotTrack(carrierName, latitude, longitude, speed, time, maxSpeed, angle, din1, din2, lbsLocation, address, maxSpeed, carrierTypeFID) {


    initialize();

    clearTimeout(timer);
    var i = 0;
    lat = latitude.slice();
    long = longitude.slice();
    title = carrierName;
    timelocal = time.slice();
    anglelocal = angle.slice();
    din1local = din1.slice();
    din2local = din2.slice();
    lbsLocationlocal = lbsLocation.slice();
    maxSpeedlocal = maxSpeed;
    addresslocal = address;
    speedlocal = speed.slice();
    carrierNameGlobal = carrierName;
    maxSpeedGlobal = maxSpeed;
    carrierTypeFIDGlobal = carrierTypeFID;
    iterator = 0;
    total = lat.length;
    ClearPlayList();
    PlayClick(2);
}


function plotStop() {
    var cnt = 0;

    var temp = iterator;
    do {
        iterator++;
        cnt++;
        if (iterator >= total) {

            break;
        }
    } while (speedlocal[iterator] == 0);

    var spans = document.getElementById("tbodyCurrent").getElementsByTagName("span");
    var tablePlayBack = document.getElementById("tablePlayBack");
    var content = "<strong>Stop</strong> <br/>Time:From: " + timelocal[temp] + "&nbsp;&nbsp;&nbsp;To:" + timelocal[iterator - 1] + "<br/>Speed:0<br/>Addess:" + addresslocal[temp];


    spans[0].innerHTML = timelocal[temp];
    spans[1].innerHTML = speedlocal[temp];
    spans[3].innerHTML = addresslocal[temp];
    spans[2].innerHTML = ExplainAngle(anglelocal[temp]);

    Addstop(content, temp);
    stopPath(temp - 1, temp);
    stopPath(temp, iterator);
}

function stopPath(start, end) {
    var flightPlanCoordinates = [
            new google.maps.LatLng(lat[start], long[start]),
            new google.maps.LatLng(lat[end], long[end])
          ];
    var flightPath = new google.maps.Polyline({
        path: flightPlanCoordinates,
        strokeColor: "#FF0000",
        strokeOpacity: 1.0,
        strokeWeight: 2
    });

    flightPath.setMap(map);
}
function plotTower() {
    var latlnglocal = new google.maps.LatLng(lat[iterator], long[iterator]);
    var pathIco = "icons/tower.png";
    var content = "<strong>Tower</strong> <br/>Time: " + timelocal[iterator] + "<br/>Addess:" + addresslocal[iterator];

    map.panTo(latlnglocal);

    var bpoint = new google.maps.Marker({
        position: latlnglocal,
        map: map,
        draggable: false,
        icon: pathIco
    });

    var infowindow = new google.maps.InfoWindow({
        content: content
    });


    google.maps.event.addListener(bpoint, 'click', function () {
        infowindow.open(map, bpoint);
    });

    return null;
}


function PlayClick(index) {
    var btn1 = document.getElementById("btnSuspend");
    var btn2 = document.getElementById("btnPlay");
    var btn3 = document.getElementById("btnStop");
    


    if (index == 0)//Data being queried
    {

        btn1.disabled = true;
        btn2.disabled = true;
        btn3.disabled = true;


       
    }
    if (index == 1)//Pause
    {
        clearTimeout(timer);


        btn1.disabled = true;
        btn2.disabled = false;
        btn3.disabled = false;
       
    }
    else if (index == 2)//play
    {

        if (stopFlag == 1) {
            mileage = 0;
            ClearPlayList();
            initialize();
            iterator = 0;
            stopFlag = 0;
        }


        btn1.disabled = false;
        btn2.disabled = true;
        btn3.disabled = false;
      

        DrawLine();

    }
    else if (index == 3)//Stop
    {
        firstFlag = 0;
        clearTimeout(timer);
        stopFlag = 1;


        btn1.disabled = true;
        btn2.disabled = false;
        btn3.disabled = true;
      

    }
}

function Calc(lat1, lon1, lat2, lon2) {

    try {
        var R = 6376.5; // km
        if (typeof (Number.prototype.toRad) === "undefined") {
            Number.prototype.toRad = function () {
                return this * Math.PI / 180;
            }
        }
        var dLat = (lat2 - lat1).toRad();
        var dLon = (lon2 - lon1).toRad();
        var lat1 = lat1.toRad();
        var lat2 = lat2.toRad();

        var a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
                        Math.sin(dLon / 2) * Math.sin(dLon / 2) * Math.cos(lat1) * Math.cos(lat2);
        var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
        var d = R * c;
        return d;
    }
    catch (e) {
        return 0;
    }
}

function GetVehIcon(vehicle, Angle) {
    var iconName;

    iconName = "car";

    if (speedlocal[iterator] >= maxSpeedGlobal) {

        vehicle = carrierTypeFIDGlobal + "/orange"
    }
    else if (speedlocal[iterator] <= 3) {
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
    return vehicle + "/" + iconName + "0.png";
}


function ExplainAngle(Angle) {
    if (Angle == null)
        return 'NA'
    if (Angle > 350 || Angle < 10)
        return 'North';

    if (Angle >= 10 && Angle <= 80)
        return 'Northeast';

    if (Angle > 80 && Angle < 100)
        return 'East';

    if (Angle >= 100 && Angle <= 170)
        return 'Southeast';

    if (Angle > 170 && Angle < 190)
        return 'South';

    if (Angle >= 190 && Angle <= 260)
        return 'Southwest';

    if (Angle > 260 && Angle < 280)
        return 'West';

    if (Angle >= 280 && Angle <= 350)
        return 'Northwest';
    return "";
}

function ClearPlayList() {
    var table = document.getElementById("tablePlayBack")
    while (table.rows.length > 0) {
        table.deleteRow(0);
    }
}

function Addstop(description, temp) {

    var latlnglocal = new google.maps.LatLng(lat[temp], long[temp]);
    var pathIco = "icons/car_icon3big.png";

    map.panTo(latlnglocal);

    var bpoint = new google.maps.Marker({
        position: latlnglocal,
        map: map,
        draggable: false,
        icon: pathIco
    });

    var infowindow = new google.maps.InfoWindow({
        content: description
    });


    google.maps.event.addListener(bpoint, 'click', function () {
        infowindow.open(map, bpoint);
    });

    return null;

}
function getSize(basePath) {
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
function AddPoint(description, Angle) {

    var latlnglocal = new google.maps.LatLng(lat[iterator], long[iterator]);
    var pathIco = "icons/" + GetVehIcon("car", Angle);
    // alert(pathIco);
    var size = getSize(carrierTypeFIDGlobal);
    var image = new google.maps.MarkerImage(pathIco,
              new google.maps.Size(size, size),
              new google.maps.Point(0, 0),
              new google.maps.Point(size / 2, size / 2));

    if (!DrawStart) {

        map.panTo(latlnglocal);

        var bpoint = new google.maps.Marker({
            position: latlnglocal,
            map: map,
            draggable: false

        });
        DrawStart = true;
        return null;
    }
    else if (firstFlag == 0) {
        map.panTo(latlnglocal);

        var bpoint = new google.maps.Marker({
            position: latlnglocal,
            map: map,
            draggable: false,
            icon: image

        });
        drawPath();
        firstFlag = 1;
        return bpoint;
    }
    else {
        map.panTo(latlnglocal);

        drawPath();
        marker.setPosition(latlnglocal);
        marker.setIcon(image);
        return marker;


    }
}
function drawPath() {

    var flightPlanCoordinates = [
            new google.maps.LatLng(lat[iterator - 1], long[iterator - 1]),
            new google.maps.LatLng(lat[iterator], long[iterator])
          ];
    var flightPath = new google.maps.Polyline({
        path: flightPlanCoordinates,
        strokeColor: "#FF0000",
        strokeOpacity: 1.0,
        strokeWeight: 2
    });

    flightPath.setMap(map);
}

function ShowPoint(lat, lng, content) {
    var latlng = new google.maps.LatLng(lat, lng);


    var bpoint = new google.maps.Marker({
        position: latlng,
        map: map,
        draggable: false
    });

    var infowindow = new google.maps.InfoWindow({
        content: content
    });
    infowindow.open(map, bpoint);
    google.maps.event.addListener(bpoint, 'click', function () {
        infowindow.open(map, bpoint);
    });
    //map.setCenter(latlng);           
}
