
let map;
var myStyle = [
    {
        featureType: "administrative",
        elementType: "all",
        stylers: [
            { visibility: "on" }
        ]
    }, {
        featureType: "landscape.man_made",
        elementType: "all",
        stylers: [
            { visibility: "off" }
        ]
    }, {
        featureType: "poi.attraction",
        elementType: "all",
        stylers: [
            { visibility: "off" }
        ]
    }, {
        featureType: "poi.business",
        elementType: "labels",
        stylers: [
            { visibility: "off" }
        ]
    }, {
        featureType: "poi.government",
        elementType: "all",
        stylers: [
            { visibility: "off" }
        ]
    }, {
        featureType: "poi.medical",
        elementType: "all",
        stylers: [
            { visibility: "on" }
        ]
    }, {
        featureType: "poi.place_of_worship",
        elementType: "all",
        stylers: [
            { visibility: "off" }
        ]
    }, {
        featureType: "poi.school",
        elementType: "all",
        stylers: [
            { visibility: "off" }
        ]
    }, {
        featureType: "poi.sports_complex",
        elementType: "labels",
        stylers: [
            { visibility: "off" }
        ]
    }, {
        featureType: "road",
        elementType: "all",
        stylers: [
            { visibility: "on" }
        ]
    }, {
        featureType: "transit",
        elementType: "all",
        stylers: [
            { visibility: "off" }
        ]
    }
]

function initMap() {
    var map = new google.maps.Map(document.getElementById('map'), {
        mapTypeControlOptions: {
            mapTypeIds: ['mystyle', google.maps.MapTypeId.ROADMAP, google.maps.MapTypeId.TERRAIN]
        },
        center: new google.maps.LatLng(42.662914, 21.165503),
        zoom: 9,
        mapTypeId: 'mystyle'
    });

    map.mapTypes.set('mystyle', new google.maps.StyledMapType(myStyle, { name: 'My Style' }));

    var marker;

    marker = new google.maps.Marker({
        position: { lat: 42.0002, lng: 20.001 },
        map: map,
        title: 'Hospital'
    });

    map.addListener("click", (e) => {
        var lat = e.latLng.lat();
        var lng = e.latLng.lng();
        document.getElementById("latitude").setAttribute('value', lat);
        document.getElementById("longitude").setAttribute('value', lng);
        if (marker && marker.setMap) {
            marker.setMap(null);
        }
        marker = new google.maps.Marker({
            position: { lat: lat, lng: lng },
            map: map,
            title: 'Hospital'
        });

    });
}