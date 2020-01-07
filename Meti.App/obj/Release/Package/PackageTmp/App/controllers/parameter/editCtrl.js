app.controller('parameter-editCtrl',
    ['$scope',
        '$log',
        "responseDispatcherService",
        'parameterService',
        '$uibModalInstance',
        '$q',
        'parameterModel',
        'toastr',
        'utilsService',
        '$rootScope',
        '$uibModal',
        'deviceService',
        'showDevices',
        'processInstanceId',
        function ($scope, $log, responseDispatcherService, parameterService, $uibModalInstance, $q, parameterModel, toastr, utilsService, $rootScope, $uibModal, deviceService, showDevices, processInstanceId) {
            //#region Model
            $scope.title = '';
            $scope.parameterModel = null;
            $scope.frequencyTypeList = [];
            $scope.deviceList = [];
            $scope.isCtrlReady = false;
            $scope.isSaveBusy = false;
            $scope.isEdit = false;
            $scope.showDevices = showDevices;

            //#endregion

            //#region Behaviors

            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            };

            var clear = function () {
                $scope.parameterModel = null;
                $scope.isCtrlReady = false;
                $scope.isSaveBusy = false;
            }

            var fetchEssentialData = function () {
                var deferred = $q.defer();

                var request = {
                    "deviceId": ''
                };

                parameterService.fetchEssentialData(request)
                    .then(function (response) {
                        var data = response ? response.data : {};

                        $scope.frequencyTypeList = Enumerable.from(data.frequencyTypeList).select(function (x) { return utilsService.createItemDtoModel(x) }).toArray();
                        //if ($scope.showDevices) {
                        //    $scope.deviceList = Enumerable.from(data.deviceList).select(function (x) { return deviceService.deviceEditModel(x) }).toArray();
                        //    if ($scope.parameterModel.device) {
                        //        $scope.deviceList.push($scope.parameterModel.device);
                        //    }
                        //}
                        

                        deferred.resolve(response);
                    }, function (error) {
                        $log.error("Errore in fetchEssentialData: " + JSON.stringify(error));
                        deferred.reject(error);
                    }).finally(function () {
                        $scope.isCtrlReady = true;
                    });

                return deferred.promise;
            }

            $scope.deleteAlarm = function(index) {
                $scope.parameterModel.alarms.splice(index, 1);
            }

            $scope.openAlarmEdit = function (item) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'App/templates/alarm/edit.html',
                    controller: 'alarm-editCtrl',
                    backdrop: 'static',
                    size: 'md',
                    resolve: {
                        "alarmModel": function () {
                            return item;
                        },
                        "showDevices": function () {
                            return $scope.showDevices;
                        },
                        "processInstanceId": function () {
                            return processInstanceId;
                        }
                    }
                });

                $scope.alarmIndex = $scope.parameterModel.alarms.indexOf(item);
                modalInstance.result.then(function (result) {
                    if (!result)
                        return;
                    if ($scope.alarmIndex > -1) {
                        $scope.parameterModel.alarms[$scope.alarmIndex] = result.model;

                    } else {
                        $scope.parameterModel.alarms.push(result.model);
                    }
                });
             
            }
            
            $scope.save = function () {
                //Valido il form
                if (!$scope.parameterEditForm.$valid) {
                    toastr.error($rootScope.validationMessages.formErrors);
                    return;
                }
                
                $uibModalInstance.close({ "model": $scope.parameterModel, "isEdit": $scope.isEdit });

                closeModal();
            }

            var closeModal = function() {
                $uibModalInstance.close();
                clear();
            }
            //#endregion

            var constructor = function () {
                $scope.isCtrlReady = false;
                var essentialDataPromises = [];

                if (parameterModel) {
                    $scope.parameterModel = angular.copy(parameterModel);
                    //essentialDataPromises.push(get(parameterId));
                    $scope.isEdit = true;
                    $scope.title = 'Modifica Parametro';
                } else {
                    $scope.parameterModel = parameterService.parameterCreateModel({});
                    $scope.title = 'Crea Parametro';
                }

                essentialDataPromises.push(fetchEssentialData());

                $q.all(essentialDataPromises).then(function () {
                    $scope.isCtrlReady = true;
                });
            }

            constructor();

            //$scope.$on("$destroy", function () {
            //    clear();
            //});
        }]);