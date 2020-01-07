app.controller('alarm-editCtrl',
    ['$scope',
        '$log',
        "responseDispatcherService",
        'alarmService',
        '$uibModalInstance',
        '$q',
        'alarmModel',
        'toastr',
        'utilsService',
        '$rootScope',
        '$uibModal',
        'showDevices',
        'processInstanceId',
        function ($scope, $log, responseDispatcherService, alarmService, $uibModalInstance, $q, alarmModel, toastr, utilsService, $rootScope, $uibModal, showDevices, processInstanceId) {
            //#region Model
            $scope.title = '';
            $scope.alarmModel = null;
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
                $scope.alarmModel = null;
                $scope.isCtrlReady = false;
                $scope.isSaveBusy = false;
            }

            var fetchEssentialData = function () {
                var deferred = $q.defer();

                var request = {};

                alarmService.fetchEssentialData(request)
                    .then(function (response) {
                        var data = response ? response.data : {};

                        $scope.alarmColorList = Enumerable.from(data.alarmColorList).select(function (x) { return utilsService.createItemDtoModel(x) }).toArray();

                        deferred.resolve(response);
                    }, function (error) {
                        $log.error("Errore in fetchEssentialData: " + JSON.stringify(error));
                        deferred.reject(error);
                    }).finally(function () {
                        $scope.isCtrlReady = true;
                    });

                return deferred.promise;
            }

            //var get = function (id) {
            //    var deferred = $q.defer();

            //    var request = { "id": id }

            //    var promise = alarmService.get(request);

            //    promise.then(function (dto) {
            //        $scope.alarmModel = alarmService.alarmEditModel(dto.data);

            //        deferred.resolve(dto);
            //    }, function (error) {
            //        //Visualizzo gli errori
            //        responseDispatcherService.printErrorMessages(error.data.data, toastr);
            //        //Loggo gli errori
            //        $log.error("Errore in editCtrl: " + JSON.stringify(error));
            //        deferred.reject(error);
            //    });

            //    return deferred.promise;
            //}

            $scope.deleteAlarmMetric = function (index) {
                $scope.alarmModel.alarmMetrics.splice(index, 1);
            }

            $scope.openAlarmMetricEdit = function (item) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'App/templates/alarmMetric/edit.html',
                    controller: 'alarmMetric-editCtrl',
                    backdrop: 'static',
                    size: 'sm',
                    resolve: {
                        "alarmMetricModel": function () {
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

                $scope.alarmMetricIndex = $scope.alarmModel.alarmMetrics.indexOf(item);
                modalInstance.result.then(function (result) {
                    if (!result)
                        return;
                    if ($scope.alarmMetricIndex > -1) {
                        $scope.alarmModel.alarmMetrics[$scope.alarmMetricIndex] = result.model;

                    } else {
                        $scope.alarmModel.alarmMetrics.push(result.model);
                    }
                });

            }

            $scope.save = function () {
                //Valido il form
                if (!$scope.alarmEditForm.$valid) {
                    toastr.error($rootScope.validationMessages.formErrors);
                    return;
                }

                $uibModalInstance.close({ "model": $scope.alarmModel, "isEdit": $scope.isEdit });

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

                if (alarmModel) {
                    $scope.alarmModel = angular.copy(alarmModel);
                    $scope.isEdit = true;
                    $scope.title = 'Modifica Allarme';
                } else {
                    $scope.alarmModel = alarmService.alarmEditModel({});
                    $scope.title = 'Crea Allarme';
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