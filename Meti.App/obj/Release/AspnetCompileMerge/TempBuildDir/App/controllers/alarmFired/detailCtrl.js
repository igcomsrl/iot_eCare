app.controller('alarmFiredDetailCtrl',
    ['$scope',
        'alarmFiredId',
        '$uibModalInstance',
        'alarmFiredService',
        'toastr',
        '$rootScope',
        'onlyRead',
        'responseDispatcherService',
        'geolocationService',
        '$q',
        function ($scope, alarmFiredId, $uibModalInstance, alarmFiredService, toastr, $rootScope, onlyRead, responseDispatcherService, geolocationService, $q) {
            //#region Model
            $scope.alarmFiredModel = null;    
            $scope.isTurnOffBusy = false;
            $scope.isMapVisible = false;
            $scope.onlyRead = onlyRead;
            $scope.getPatientDashboardUrl = geolocationService.getPatientDashboardUrl;
            //#endregion

            //#region Behaviors
            
            $scope.alarmFiredOff = function () {
                //Valido il form
                

                $scope.isTurnOffBusy = true;

                var request = $scope.alarmFiredModel;

                var promise = alarmFiredService.turnOff(request);

                promise.then(function (response) {
                    //var data = response.data;
                    //$scope.processModel.id = data.id;
                    toastr.success($rootScope.validationMessages.saveSuccess);
                    $scope.close();
                }, function (error) {
                    responseDispatcherService.printErrorMessages(error.data.data, toastr);
                    $log.error("Errore in process-UpdateCtrl.Update: " + angular.fromJson(error));
                }).finally(function () {
                    $scope.isTurnOffBusy = false;
                });
            }

            $scope.initPinMap = function () {

                var initPinMapSub = function () {
                    //Se il paziente è caduto, ritorno il punto esatto di caduta
                    if (alarmFiredModel.isFall || alarmFiredModel.isFallNoResponse) {
                        return alarmFiredModel.latlng;
                    }
                    //Altrimenti ritorno la sua posizione di residenza
                    else
                        return alarmFiredModel.processInstance.patient.latlng;
                } 

                var checkPinMap = true;
                while (checkPinMap) {
                    if (alarmFiredModel) {
                        initPinMapSub();
                        checkPinMap = false;
                    }     
                }
      
            }

            $scope.showMap = function () {
                $scope.isMapVisible = true;
            }

            $scope.hideMap = function () {
                $scope.isMapVisible = false;
            }

            var get = function (id) {
                var deferred = $q.defer();

                var request = { "id": id }

                var promise = alarmFiredService.get(request);

                promise.then(function (dto) {
                    $scope.alarmFiredModel = alarmFiredService.alarmFiredDetailModel(dto.data);

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
                get(alarmFiredId).then(function (success) {

                });
            }

            $scope.close = function() {
                $uibModalInstance.close();
            }

            constructor();

            $scope.$on("$destroy", function () {
                
            });
        }]);