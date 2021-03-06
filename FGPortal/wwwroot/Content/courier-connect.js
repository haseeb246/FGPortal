(function (window, $) {
    var coreLib = function () {
        var self = this;

        // Functions
        function init() {
            if ('/Route/List /App/DisplayOrders /Route/Map /App/RouteDetails'.includes(window.location.pathname)) { $('.nav > li > a[href="/"]').parent().addClass('active'); }
            $('.nav > li > a[href="/Route/List"]').parent().addClass('active');
            $('.nav > li > a[href="' + window.location.pathname + '"]').parent().addClass('active');
        }

        init();
    }
    var helpersLib = function () {
        var self = this;

        // Public Functions
        self.isNullOrEmpty = isNullOrEmpty;
        self.get2dArrayItem = get2dArrayItem;
        self.validURL = validURL;
        self.isLocalhost = isLocalhost;
        self.setCookie = setCookie;
        self.isIntersect = isIntersect;
        self.isIntersectAny = isIntersectAny;

        // Functions
        function isNullOrEmpty(obj) {
            return obj === null || obj === undefined || obj === '';
        }
        function get2dArrayItem(array, where, what) {
            for (var i = 0; i < array.length; i++) {
                if (array[i][where] === what) {
                    return array[i];
                }
            }
        }
        function validURL(str) {
            return /^(?:(?:https?|ftp):\/\/)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})))(?::\d{2,5})?(?:\/\S*)?$/.test(str);
        }
        function isLocalhost() {
            return location.hostname === 'localhost';
        }
        function setCookie(name, value) {
            $.post('../Ajax/SetCookie', { name, value });
        }
        function isIntersect(input, find) {
            var results = [];

            for (var i = 0; i < find.length; i++) { if (find[i] === '') { find.splice(i, 1); } }
            for (var j = 0; j < input.length; j++) { if (find.indexOf(input[j]) !== -1) { results.push(input[j]); } }

            return results.length === find.length;
        }
        function isIntersectAny(input, find) {
            var results = [];

            for (var i = 0; i < find.length; i++) { if (find[i] === '') { find.splice(i, 1); } }
            for (var j = 0; j < input.length; j++) { if (find.indexOf(input[j]) !== -1) { results.push(input[j]); } }

            return results.length > 0;
        }
    }
    var googleMapLib = function () {
        var self = this;

        // Private Variables
        var _googleMapsAPIKey = 'AIzaSyAfkCg4V-cn03lKmI1qu9wsDzdZ5aJCeGs';
        var _map = null;
        var _mapDOMElementID = 'map';
        var _markers = [];
        var _openInfoWindows = [];
        var _viewableStates = [];

        // Public Functions
        self.getMapObject = getMapObject;
        self.createMap = createMap;
        self.setMarkers = setMarkers;
        self.centerMarkerOnMap = centerMarkerOnMap;
        self.showMarkersByClass = showMarkersByClass;
        self.showMarkersByClasses = showMarkersByClasses;
        self.showMarkersByClassesForStateFilter = showMarkersByClassesForStateFilter;
        self.hideMarkersByClass = hideMarkersByClass;
        self.hideMarkersByClasses = hideMarkersByClasses;
        self.showAllMarkers = showAllMarkers;
        self.hideAllMarkers = hideAllMarkers;
        self.zoomToFit = zoomToFit;
        self.zoomToFitStateMarkers = zoomToFitStateMarkers;
        self.clearViewableStates = clearViewableStates;

        function init() {
            createMap();
        }
        function createMap(mapOptions) {
            _map = null;
            _markers = [];
            _openInfoWindows = [];

            var mapElement = document.getElementById(_mapDOMElementID);
            if (!FGPortal.helpers.isNullOrEmpty(mapElement)) {
                if (FGPortal.helpers.isNullOrEmpty(mapOptions)) {
                    mapOptions = {
                        zoom: 4.3,
                        center: { lat: 38.4987789, lng: -98.3200779 },
                        mapTypeId: google.maps.MapTypeId.ROADMAP
                    };
                }
                _map = new google.maps.Map(mapElement, mapOptions);
            }
        }
        function getMapObject() {
            return _map;
        }
        function ensureMapSetup() {
            if (FGPortal.helpers.isNullOrEmpty(_map)) {
                createMap();
            }
        }
        function setMarkers(markers) {
            ensureMapSetup();

            for (var i = 0; i < markers.length; i++) {

                var marker = new google.maps.Marker({
                    map: _map,
                    position: { lat: Number(markers[i].lat), lng: Number(markers[i].lng) },
                    icon: {
                        url: markers[i].icon,
                        scaledSize: new google.maps.Size(15, 24), // scaled size
                    },
                    info: new google.maps.InfoWindow({
                        disableAutoPan: true,
                        content: markers[i].infoWindowContent,
                        marker: markers[i]
                    })
                });

                marker.info.addListener('closeclick', function () {
                    var index = _openInfoWindows.indexOf(this);
                    if (index > -1) {
                        _openInfoWindows.splice(index, 1);
                    }
                });
                marker.addListener('mouseover', function () {
                    if (!_openInfoWindows.includes(this.info)) {
                        this.info.open(map, this);
                    }
                });
                marker.addListener('mouseout', function () {
                    if (!_openInfoWindows.includes(this.info)) {
                        this.info.close(map, this);
                    }
                });
                marker.addListener('click', function () {
                    if (!FGPortal.helpers.isNullOrEmpty(this.info.marker.state)) {
                        this.info.close();
                        this.setVisible(false);
                        _viewableStates.push('state-' + this.info.marker.state);
                        applyFilter();
                        zoomToFitStateMarkers('state-' + this.info.marker.state)
                    }
                    else if (!FGPortal.helpers.isNullOrEmpty(this.info.marker.url)) {
                        window.open(this.info.marker.url, '_blank');
                    }
                    else {
                        _map.setCenter(this.getPosition());
                        this.info.open(map, this);
                        _openInfoWindows.push(this.info);
                    }
                });

                marker.setVisible(marker.info.marker.visible);

                _markers.push(marker);
            }

            setTimeout(() => { zoomToFit(); }, 300);
        }
        function openInfoWindow(id) {
            for (var i = 0; i < _markers.length; i++) {
                if (_markers[i].info.marker.id === id) {
                    closeAllInfoWindows();
                    var marker = _markers[i];
                    marker.info.open(map, marker);
                    _map.setCenter(marker.getPosition());
                }
            }
        }
        function closeAllInfoWindows() {
            for (var i = 0; i < _markers.length; i++) {
                _markers[i].info.close();
            }
        }
        function centerMarkerOnMap(id) {
            for (var i = 0; i < _markers.length; i++) {
                if (_markers[i].info.marker.id === id) {
                    _map.setCenter(_markers[i].getPosition());
                }
            }
        }
        function showMarkersByClass(name) {
            for (var i = 0; i < _markers.length; i++) {
                if (_markers[i].info.marker.classes.indexOf(name) !== -1)
                    _markers[i].setVisible(true);
            }
        }
        function showMarkersByClasses(classes) {
            for (var i = 0; i < _markers.length; i++) {
                if (FGPortal.helpers.isIntersect(_markers[i].info.marker.classes.split(','), classes))
                    _markers[i].setVisible(true);
            }
        }
        function showMarkersByClassesForStateFilter(classes) {
            for (var i = 0; i < _markers.length; i++) {
                if (FGPortal.helpers.isIntersect(_markers[i].info.marker.classes.split(','), classes)
                    && FGPortal.helpers.isIntersectAny(_markers[i].info.marker.classes.split(','), _viewableStates)) {
                    _markers[i].setVisible(true);
                } else if (_markers[i].info.marker.classes.indexOf('state-marker') === -1) {
                    _markers[i].setVisible(false);
                }
            }
        }
        function hideMarkersByClass(name) {
            for (var i = 0; i < _markers.length; i++) {
                if (_markers[i].info.marker.classes.indexOf(name) !== -1)
                    _markers[i].setVisible(false);

            }
        }
        function hideMarkersByClasses(classes) {
            for (var i = 0; i < _markers.length; i++) {
                if (FGPortal.helpers.isIntersect(_markers[i].info.marker.classes.split(','), classes))
                    _markers[i].setVisible(false);
            }
        }
        function showAllMarkers() {
            for (var i = 0; i < _markers.length; i++) {
                _markers[i].setVisible(true);
            }
        }
        function hideAllMarkers() {
            for (var i = 0; i < _markers.length; i++) {
                _markers[i].setVisible(false);
            }
        }
        function zoomToFit() {
            var bounds = new google.maps.LatLngBounds();
            for (var i = 0; i < _markers.length; i++) { bounds.extend(_markers[i].getPosition()); }
            _map.fitBounds(bounds);
        }
        function zoomToFitStateMarkers(state) {
            var bounds = new google.maps.LatLngBounds();
            for (var i = 0; i < _markers.length; i++) {
                if (FGPortal.helpers.isIntersectAny(_markers[i].info.marker.classes.split(','), [ state ]))
                    bounds.extend(_markers[i].getPosition());
            }
            _map.fitBounds(bounds);
        }
        function clearViewableStates() {
            _viewableStates = [];
        }

        init();
    }
    var aerisLib = function () {
        var self = this;

        // Private Variables
        var _apiID = '';
        var _apiSe = '';

        // Public Functions
        self.applyWeatherOverlay = applyWeatherOverlay;
        self.removeWeatherOverlay = removeWeatherOverlay;

        function init() {
            _apiID = FGPortal.helpers.isLocalhost() ? 'wgE96YE3scTQLKjnqiMsv' : 'GGAsOvzLvM7MkVml3X2m9';
            _apiSe = FGPortal.helpers.isLocalhost() ? 'tlyy22v5uBRBcm8lWeP0Y6ZISPLDVKGWXTJH9kYb' : 'fhQFx6yH6vNCwsE840LTlbo23KFtFHmT86B4CFD4';
		}
        function applyWeatherOverlay(map) {
            removeWeatherOverlay();
            var radar = new google.maps.ImageMapType({
                getTileUrl: function (coord, zoom) {
                    return ['https://maps.aerisapi.com/' + _apiID + '_' + _apiSe + '/radar/', zoom, '/', coord.x, '/', coord.y, '/current.png'].join('');
                },
                tileSize: new google.maps.Size(256, 256)
            });
            map.overlayMapTypes.push(radar);
        }
        function removeWeatherOverlay(map) {
            try {
                map.overlayMapTypes.clear();
            } catch (error) {
                // ...
            }
        }

        init();
    }

    window.FGPortal = new coreLib();
    window.FGPortal.helpers = new helpersLib();
    window.FGPortal.maps = new googleMapLib();
    window.FGPortal.aeris = new aerisLib();
})(window, $);