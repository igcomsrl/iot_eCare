app.controller('markerDetailCtrl',
    ['$scope',
        'geoLocationId',
        '$uibModalInstance',
        '$uibModal',
        'geolocationService',
        '$q',
        function ($scope, geoLocationId, $uibModalInstance, $uibModal, geolocationService, $q) {
            //#region Model
            $scope.geoLocationModel = null;
            $scope.getPatientDashboardUrl = geolocationService.getPatientDashboardUrl;

            //#endregion

            //#region Behaviors
            //$scope.toPatientDashboard = function (e) {                

            //    var modalInstance = $uibModal.open({
            //        animation: true,
            //        templateUrl: 'App/templates/geolocation/patientDashboard.html',
            //        controller: 'patientDashboardCtrl',
            //        backdrop: 'static',
            //        size: 'lg',
            //        resolve: {
            //            "processInstanceId": function () {
            //                return geoLocationModel.processInstanceId;
            //            }
            //        }
            //    });

            //    modalInstance.result.then(function (result) {
            //        $scope.close();
            //    });
            //}

            var get = function (id) {
                var deferred = $q.defer();

                var request = { "processInstanceId": id }

                var promise = geolocationService.getProcessInstanceGeo(request);

                promise.then(function (dto) {
                    $scope.geoLocationModel = geolocationService.geoLocationModel(dto.data);

                    deferred.resolve(dto);
                }, function (error) {
                    //Visualizzo gli errori
                    responseDispatcherService.printErrorMessages(error.data, toastr);
                    //Loggo gli errori
                    $log.error("Errore in editCtrl: " + JSON.stringify(error));
                    deferred.resolve(error);
                });

                return deferred.promise;
            }
            //#endregion

            var constructor = function () {
                get(geoLocationId);
            }

            $scope.close = function() {
                $uibModalInstance.close();
            }

            constructor();

            $scope.$on("$destroy", function () {
                
            });
        }]);