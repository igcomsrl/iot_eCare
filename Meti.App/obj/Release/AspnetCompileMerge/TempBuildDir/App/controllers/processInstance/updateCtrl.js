app.controller('processInstance-updateCtrl',
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
        '$stateParams',       
        function ($scope, $log, responseDispatcherService, processInstanceService, $q, toastr, utilsService, $rootScope, registryService, $state, $window, $uibModal, processService, $stateParams) {
            //#region Model
            $scope.processInstanceModel = null;
            $scope.doctorList = [];
            $scope.referencePersonList = [];
            $scope.patientList = [];
            $scope.processList = [];
            $scope.isCtrlReady = false;
            $scope.isSaveBusy = false;
            //#endregion

            //$scope.doctorKendoOptions = {
            //    dataSource: $scope.doctorList,
            //    dataTextField: "comboText",
            //    dataValueField: "id",
            //    valuePrimitive: true,
            //    autoBind: false
            //}

            //#region Behaviors
            var clear = function () {
                $scope.processInstanceModel = null;
                $scope.isCtrlReady = false;
                $scope.isUpdateBusy = false;
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
                        $scope.referencePersonList = Enumerable.from(data.referencePersonList).select(function (x) { return registryService.registryDetailModel(x) }).toArray();
                        $scope.patientList = Enumerable.from(data.patientList).select(function (x) { return registryService.registryDetailModel(x) }).toArray();
                        $scope.processList = Enumerable.from(data.processList).select(function (x) { return processService.processUpdateModel(x) }).toArray();

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
                $scope.processInstanceModel.process.parameters.splice(index, 1);
            }

            $scope.deleteprocessInstanceMacro = function (index) {
                $scope.processInstanceModel.process.processInstanceMacros.splice(index, 1);
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
                            return $scope.processInstanceModel.id;
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

            $scope.update = function () {
                //Valido il form
                if (!$scope.processInstanceUpdateForm.$valid) {
                    toastr.error($rootScope.validationMessages.formErrors);
                    return;
                }

                if (!$scope.processInstanceModel.doctors || $scope.processInstanceModel.doctors.length == 0) {
                    toastr.error("Errore di validazione, selezionare almeno un Dottore");
                    return;
                }

                $scope.isUpdateBusy = true;

                $scope.processInstanceModel.process.processType = 2;//Instance

                var request = {
                    'id': $scope.processInstanceModel.id,
                    'name': $scope.processInstanceModel.name,
                    'doctors': $scope.processInstanceModel.doctors,
                    'patient': $scope.processInstanceModel.patient ? $scope.processInstanceModel.patient.id: '',
                    'referencePersons': $scope.processInstanceModel.referencePersons,
                    'process': $scope.processInstanceModel.process,
                    'isEnabled': $scope.processInstanceModel.isEnabled
                }

                var promise = processInstanceService.update(request);

                promise.then(function (response) {
                    var data = response.data;
                    //$scope.processInstanceModel.id = data.id;
                    toastr.success($rootScope.validationMessages.saveSuccess);
                    $window.history.back()
                }, function (error) {
                    responseDispatcherService.printErrorMessages(error.data.data, toastr);
                    $log.error("Errore in processInstance-updateCtrl.update: " + angular.fromJson(error));
                }).finally(function () {
                    $scope.isUpdateBusy = false;
                });
            }

            var get = function (id) {
                var deferred = $q.defer();

                var request = { "id": id }

                var promise = processInstanceService.get(request);

                promise.then(function (dto) {
                    $scope.processInstanceModel = processInstanceService.processInstanceUpdateModel(dto.data);
                    
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

                $scope.processInstanceModel = processInstanceService.processInstanceUpdateModel({});

                $q.all(essentialDataPromises).then(function () {
                    $scope.isCtrlReady = true;
                });
            }

            constructor();

            $scope.$on("$destroy", function () {
                clear();
            });
        }]);