app.controller('processInstance-createCtrl',
    ['$scope',
        '$log',
        "responseDispatcherService",
        'processInstanceService',
        '$q',
        'toastr',
        'utilsService',
        '$rootScope',
        'registryService',
        '$state',
        '$window',
        '$uibModal',
        'processService',
        function ($scope, $log, responseDispatcherService, processInstanceService, $q, toastr, utilsService, $rootScope, registryService, $state, $window, $uibModal, processService) {
            //#region Model
            $scope.processInstanceModel = null;
            $scope.doctorList = [];
            $scope.patientList = [];
            $scope.processList = [];
            $scope.referencePersonList = [];
            $scope.isCtrlReady = false;
            $scope.isSaveBusy = false;
            //#endregion

            //#region Behaviors
            var clear = function () {
                $scope.processInstanceModel = null;
                $scope.isCtrlReady = false;
                $scope.isCreateBusy = false;
            }

            $scope.navigateBack = function () {
                $window.history.back();
            }

            var fetchEssentialData = function () {
                var deferred = $q.defer();

                var request = {};

                processInstanceService.fetchEssentialData(request)
                    .then(function (response) {
                        var data = response ? response.data : {};

                        $scope.doctorList = Enumerable.from(data.doctorList).select(function (x) { return registryService.registryDetailModel(x) }).toArray();
                        $scope.patientList = Enumerable.from(data.patientList).select(function (x) { return registryService.registryDetailModel(x) }).toArray();
                        $scope.processList = Enumerable.from(data.processList).select(function (x) { return processService.processUpdateModel(x) }).toArray();
                        $scope.referencePersonList = Enumerable.from(data.referencePersonList).select(function (x) { return registryService.registryDetailModel(x) }).toArray();

                        deferred.resolve(response);
                    }, function (error) {
                        $log.error("Errore in fetchEssentialData: " + JSON.stringify(error));
                        deferred.reject(error);
                    }).finally(function () {
                        $scope.isCtrlReady = true;
                    });

                return deferred.promise;
            }

            $scope.deleteParameter = function () {
                $scope.processInstanceModel.parameters.splice(index, 1);
            }

            $scope.deleteprocessInstanceMacro = function () {
                $scope.processInstanceModel.processInstanceMacros.splice(index, 1);
            }


            $scope.openProcessInstanceMacroEdit = function (item) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'App/templates/processMacro/edit.html',
                    controller: 'processMacro-editCtrl',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        "processMacroModel": function () {
                            return item;
                        }
                    }
                });

                $scope.processMacroIndex = $scope.processInstanceModel.process.processMacros.indexOf(item);
                modalInstance.result.then(function (result) {
                    if (!result)
                        return;

                    if ($scope.processMacroIndex > -1) {
                        $scope.processInstanceModel.process.processMacros[$scope.processMacroIndex] = result.model;

                    } else {
                        $scope.processInstanceModel.process.processMacros.push(result.model);
                    }
                });
            }

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
                            return true;
                        },
                        "processInstanceId": function () {
                            return null;
                        }
                    }
                });

                $scope.parameterIndex = $scope.processInstanceModel.process.parameters.indexOf(item);
                modalInstance.result.then(function (result) {
                    if (!result)
                        return;
                    if ($scope.parameterIndex > -1) {
                        $scope.processInstanceModel.process.parameters[$scope.parameterIndex] = result.model;

                    } else {
                        $scope.processInstanceModel.process.parameters.push(result.model);
                    }
                });
            }

            $scope.create = function () {
                //Valido il form
                if (!$scope.processInstanceCreateForm.$valid) {
                    toastr.error($rootScope.validationMessages.formErrors);
                    return;
                }

                $scope.isCreateBusy = true;

                $scope.processInstanceModel.process.processType = 2;//Instance

                var request = {
                    'id': $scope.processInstanceModel.id,
                    'name': $scope.processInstanceModel.name,
                    'doctors': $scope.processInstanceModel.doctors,
                    'patient': $scope.processInstanceModel.patient ? $scope.processInstanceModel.patient.id : '',
                    'referencePersons': $scope.processInstanceModel.referencePersons,
                    'process': $scope.processInstanceModel.process,
                    'isEnabled': $scope.processInstanceModel.isEnabled
                }

                var promise = processInstanceService.create(request);

                promise.then(function (response) {
                    var data = response.data;
                    //$scope.processInstanceModel.id = data.id;
                    toastr.success($rootScope.validationMessages.saveSuccess);
                    $window.history.back()
                }, function (error) {
                    responseDispatcherService.printErrorMessages(error.data.data, toastr);
                    $log.error("Errore in processInstance-createCtrl.create: " + angular.fromJson(error));
                }).finally(function () {
                    $scope.isCreateBusy = false;
                });
            }

            //#endregion

            var constructor = function () {
                $scope.isCtrlReady = false;
                var essentialDataPromises = [];
                essentialDataPromises.push(fetchEssentialData());

                $scope.processInstanceModel = processInstanceService.processInstanceCreateModel({});

                $q.all(essentialDataPromises).then(function () {
                    $scope.isCtrlReady = true;
                });
            }

            constructor();

            $scope.$on("$destroy", function () {
                clear();
            });
        }]);