app.controller('process-updateCtrl',
    ['$scope',
        '$log',
        "responseDispatcherService",
        'processService',
        '$q',
        'toastr',
        'utilsService',
        '$rootScope',
        'registryService',
        '$state',
        '$window',
        '$uibModal',
        '$stateParams',        
        function ($scope, $log, responseDispatcherService, processService, $q, toastr, utilsService, $rootScope, registryService, $state, $window, $uibModal, $stateParams) {
            //#region Model
            $scope.processModel = null;
            //$scope.doctorList = false;
            //$scope.patientList = false;
            $scope.isCtrlReady = false;
            $scope.isSaveBusy = false;
            //#endregion

            //#region Behaviors
            var clear = function () {
                $scope.processModel = null;
                $scope.isCtrlReady = false;
                $scope.isUpdateBusy = false;
            }

            $scope.navigateBack = function () {
                $window.history.back();
            }

            var fetchEssentialData = function () {
                var deferred = $q.defer();

                var request = {};

                processService.fetchEssentialData(request)
                    .then(function (response) {
                        var data = response ? response.data : {};

                        //$scope.doctorList = Enumerable.from(data.doctorList).select(function (x) { return registryService.registryDetailModel(x) }).toArray();
                        //$scope.patientList = Enumerable.from(data.patientList).select(function (x) { return registryService.registryDetailModel(x) }).toArray();

                        deferred.resolve(response);
                    }, function (error) {
                        $log.error("Errore in fetchEssentialData: " + JSON.stringify(error));
                        deferred.reject(error);
                    }).finally(function () {
                        $scope.isCtrlReady = true;
                    });

                return deferred.promise;
            }

            $scope.deleteParameter = function (index) {
                $scope.processModel.parameters.splice(index, 1);
            }

            $scope.deleteProcessMacro = function (index) {
                $scope.processModel.processMacros.splice(index, 1);
            }

            
            //$scope.openProcessMacroEdit = function (item) {
            //    var modalInstance = $uibModal.open({
            //        animation: true,
            //        templateUrl: 'App/templates/processMacro/edit.html',
            //        controller: 'processMacro-editCtrl',
            //        backdrop: 'static',
            //        size: 'lg',
            //        resolve: {
            //            "processMacroModel": function () {
            //                return item;
            //            }
            //        }
            //    });

            //    $scope.processModelIndex = $scope.processModel.processMacros.indexOf(item);
            //    modalInstance.result.then(function (result) {
            //        if (!result)
            //            return;

            //        if ($scope.processModelIndex > -1) {
            //            $scope.processModel.processMacros[$scope.processModelIndex] = result.model;
                        
            //        } else {
            //            $scope.processModel.processMacros.push(result.model);
            //        }
            //    });
            //}

            $scope.openParameterEdit = function (item) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'App/templates/parameter/edit.html',
                    controller: 'parameter-editCtrl',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        "parameterModel": function () {
                            return item;
                        },
                        "showDevices": function () {
                            return false;
                        },
                        "processInstanceId": function () {
                            return null;
                        }
                    }
                });

                $scope.parameterIndex = $scope.processModel.parameters.indexOf(item);
                modalInstance.result.then(function (result) {
                    if (!result)
                        return;
                    if ($scope.parameterIndex > -1) {
                        $scope.processModel.parameters[$scope.parameterIndex] = result.model;

                    } else {
                        $scope.processModel.parameters.push(result.model);
                    }
                });
            }

            $scope.update = function () {
                //Valido il form
                if (!$scope.processUpdateForm.$valid) {
                    toastr.error($rootScope.validationMessages.formErrors);
                    return;
                }

                $scope.isUpdateBusy = true;

                var request = {
                    'id': $scope.processModel.id,
                    'name': $scope.processModel.name,
                    'parameters': $scope.processModel.parameters,
                    'processMacros': $scope.processModel.processMacros,
                    'isEnabled': $scope.processModel.isEnabled
                }

                var promise = processService.update(request);

                promise.then(function (response) {
                    //var data = response.data;
                    //$scope.processModel.id = data.id;
                    toastr.success($rootScope.validationMessages.saveSuccess);
                    $window.history.back()
                }, function (error) {
                    responseDispatcherService.printErrorMessages(error.data.data, toastr);
                    $log.error("Errore in process-UpdateCtrl.Update: " + angular.fromJson(error));
                }).finally(function () {
                    $scope.isUpdateBusy = false;
                });
            }

            var get = function (id) {
                var deferred = $q.defer();

                var request = { "id": id }

                var promise = processService.get(request);

                promise.then(function (dto) {
                    $scope.processModel = processService.processUpdateModel(dto.data);

                    deferred.resolve(dto);
                }, function (error) {
                    //Visualizzo gli errori
                    responseDispatcherService.printErrorMessages(error.data, toastr);
                    //Loggo gli errori
                    $log.error("Errore in editCtrl: " + JSON.stringify(error));
                    deferred.reject(error);
                });

                return deferred.promise;
            }

            //#endregion

            var constructor = function () {
                $scope.isCtrlReady = false;
                var essentialDataPromises = [];
                essentialDataPromises.push(fetchEssentialData());
                essentialDataPromises.push(get($stateParams.id));

                $scope.processModel = processService.processUpdateModel({});

                $q.all(essentialDataPromises).then(function () {
                    $scope.isCtrlReady = true;
                });
            }

            constructor();

            $scope.$on("$destroy", function () {
                clear();
            });
        }]);