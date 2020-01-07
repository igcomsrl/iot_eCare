app.controller('mapCtrl',
    ['$scope',
        '$log',
        "responseDispatcherService",
        '$q',
        'toastr',
        '$rootScope',
        '$uibModal',
        'geolocationService',
        function ($scope, $log, responseDispatcherService, $q, toastr, $rootScope, $uibModal, geolocationService) {
            //#region Model
            $scope.model =
            {
                geoLocationList: new kendo.data.DataSource({ data: [] }),
                geoLocationSelected: null,
                centerMap: [41.8734262, 12.4607732],//Roma,
                isDetailActive: true,
                showOnlySelf: false
            }

            //#endregion
            
            //#region Behaviors

            $scope.$watch(function () {
                return $scope.model.geoLocationSelected;
            }, function () {
                var geoList = [];
                var latlng = null;

                if ($scope.model.geoLocationSelected === null) {
                    geoList = $scope.model.geoLocationList;
                    latlng = $scope.model.centerMap;
                }
                else {
                    geoList.push($scope.model.geoLocationSelected);
                    if ($scope.model.geoLocationSelected.latlngLast)
                        $scope.model.geoLocationSelected.latlng = $scope.model.geoLocationSelected.latlngLast;
                    latlng = $scope.model.geoLocationSelected.latlng;
                    //latlng = $scope.model.geoLocationSelected.latlng;
                }
                createMap(geoList, latlng);
            });
            

            $scope.markerOnClick = function (e) {
                if (!$scope.model.isDetailActive) return;

                var geoLocationModel = geolocationService.geoLocationModel(e.marker.dataItem);

                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'App/templates/geolocation/markerDetail.html',
                    controller: 'markerDetailCtrl',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        "geoLocationId": function () {
                            return geoLocationModel.processInstanceId;
                        }
                    }
                });
                
                modalInstance.result.then(function (result) {
                    if (!result)
                        return;
                    
                });
            }

            function createMap(geoLocationList, center) {
                $("#map").kendoMap({
                    center: center ? center: $scope.model.centerMap,
                    zoom: 9,
                    layers: [
                        {
                            type: 'bing',
                            imagerySet: 'road',
                            //key: 'AqaPuZWytKRUA8Nm5nqvXHWGL8BDCXvK8onCl2PkC581Zp3T_fYAQBiwIphJbRAK'
                            key: 'AmgngVNiAhATq1fjpLbV7JEfwljFOPSaiqh6RquLiPc_oMqsB5v76SUwQ7Ozkhnn'
                            //https://www.bingmapsportal.com/Application
                        },
                        {
                            type: 'marker',
                            shape: "customTarget",
                            dataSource: geoLocationList,
                            locationField: 'latlng',
                            titleField: 'comboText',
                            tooltip: {
                                autoHide:true,
                                animation: {
                                    open: {
                                        effects: "fade:in",
                                        duration: 1000
                                    }
                                }
                            }
                        }
                    ],
                    markerClick: $scope.markerOnClick,
                    markerCreated: function (e) {

                        var latPin = e.marker.dataItem.latlng[0].toFixed(4);
                        var lngPin = e.marker.dataItem.latlng[1].toFixed(4);
                        var latCenterMap = $scope.model.centerMap[0].toFixed(4);
                        var lngCenterMap = $scope.model.centerMap[1].toFixed(4);

                        var pinTarget = "k-i-marker-pin-target";
                        if (latPin === latCenterMap && lngPin === lngCenterMap) {
                            var blinkMe = "blink_me";
                            e.marker.options.shape = pinTarget + ' ' + blinkMe;
                        } else if ($scope.model.showOnlySelf) {
                            var hidePin = "hide-pin";
                            e.marker.options.shape = pinTarget + ' ' + hidePin;
                        }

                        //if (JSON.stringify(e.marker.dataItem.latlng) === JSON.stringify($scope.model.centerMap)) {
                        //    var blinkMe = "blink_me";
                        //    e.marker.options.shape = pinTarget + ' ' + blinkMe;
                        //} else if ($scope.model.showOnlySelf) {
                        //    var hidePin = "hide-pin";
                        //    e.marker.options.shape = pinTarget + ' ' + hidePin;
                        //}
                    }
                });
            }
            

            var fetchProcessInstanceGeo = function (id) {
                var deferred = $q.defer();

                var request = {};

                var promise = geolocationService.fetchProcessInstanceGeo(request);

                var results = [];
                promise.then(function (dto) {

                    angular.forEach(dto.data, function(item) {
                        var geoLocationModel = geolocationService.geoLocationSwiftModel(item);
                        results.push(geoLocationModel);

                        if (geoLocationModel.latLast && geoLocationModel.lngLast)
                        {
                            geoLocationModel.lat = geoLocationModel.latLast;
                            geoLocationModel.lng = geoLocationModel.lngLast;
                            geoLocationModel.latlng = geoLocationModel.latlngLast;
                        }
                            

                        if (geolocationService.hasValidCoordinateInMap(geoLocationModel))
                            $scope.model.geoLocationList.add(geoLocationModel);
                    });

                    createMap($scope.model.geoLocationList);
                    deferred.resolve(dto);

                }, function (error) {
                    //Visualizzo gli errori
                    responseDispatcherService.printErrorMessages(error.data.data, toastr);
                    //Loggo gli errori
                    $log.error("Errore in editCtrl: " + JSON.stringify(error));
                    deferred.reject(error);
                });

                return deferred.promise;
            }

            //#endregion

            var constructor = function () {
                $scope.isCtrlReady = true;               
                fetchProcessInstanceGeo();
            }

            constructor();

            $scope.$on("$destroy", function () {
                
            });
        }]);