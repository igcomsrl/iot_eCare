app.controller('alarmMetric-editCtrl',
    ['$scope',
        '$log',
        "responseDispatcherService",
        'alarmMetricService',
        '$uibModalInstance',
        '$q',
        'alarmMetricModel',
        'toastr',
        'utilsService',
        '$rootScope',
        'parameterService',
        'deviceService',
        'showDevices',
        'processInstanceId',
        function ($scope, $log, responseDispatcherService, alarmMetricService, $uibModalInstance, $q, alarmMetricModel, toastr, utilsService, $rootScope, parameterService, deviceService, showDevices, processInstanceId) {
            //#region Model
            $scope.title = '';
            $scope.alarmMetricModel = null;
            $scope.isCtrlReady = false;
            $scope.isSaveBusy = false;
            $scope.isEdit = false;
            $scope.showDevices = showDevices;
            $scope.deviceList = [];
            //#endregion

            //#region Behaviors

            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            };



            var clear = function () {
                $scope.alarmMetricModel = null;
                $scope.isCtrlReady = false;
                $scope.isSaveBusy = false;
            }

            var fetchEssentialData = function () {
                var deferred = $q.defer();

                var request = {
                    "processInstanceId": processInstanceId
                };

                parameterService.fetchEssentialData(request)
                    .then(function (response) {
                        var data = response ? response.data : {};

                        if ($scope.showDevices) {
                            $scope.deviceList = Enumerable.from(data.deviceList).select(function (x) { return deviceService.deviceEditModel(x) }).toArray();
                            if ($scope.alarmMetricModel.device) {
                                $scope.deviceList.push($scope.alarmMetricModel.device);
                            }
                        }

                        deferred.resolve(response);
                    }, function (error) {
                        $log.error("Errore in fetchEssentialData: " + JSON.stringify(error));
                        deferred.reject(error);
                    }).finally(function () {
                        $scope.isCtrlReady = true;
                    });

                return deferred.promise;
            }

            $scope.save = function () {
                //Valido il form
                if (!$scope.alarmMetricEditForm.$valid) {
                    toastr.error($rootScope.validationMessages.formErrors);
                    return;
                }

                $uibModalInstance.close({ "model": $scope.alarmMetricModel, "isEdit": $scope.isEdit });

                closeModal();
            }

            var closeModal = function () {
                $uibModalInstance.close();
                clear();
            }
            //#endregion

            var constructor = function () {
                $scope.isCtrlReady = false;
                var essentialDataPromises = [];
                essentialDataPromises.push(fetchEssentialData());

                if (alarmMetricModel) {
                    $scope.alarmMetricModel = angular.copy(alarmMetricModel);
                    $scope.isEdit = true;
                    $scope.title = 'Modifica Metrica';
                } else {
                    $scope.alarmMetricModel = alarmMetricService.alarmMetricEditModel({});
                    $scope.title = 'Crea Metrica';
                }

                $q.all(essentialDataPromises).then(function () {
                    $scope.isCtrlReady = true;
                });
            }

            constructor();

            //$scope.$on("$destroy", function () {
            //    clear();
            //});
        }]);