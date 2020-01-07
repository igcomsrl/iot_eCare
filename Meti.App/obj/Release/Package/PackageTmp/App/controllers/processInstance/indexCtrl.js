app.controller('processInstance-indexCtrl',
    ['$scope',
        '$log',
        'toastr',
        "responseDispatcherService",
        'processInstanceService',
        '$q',
        'NgTableParams',
        'ngTableService',
        '$state',
        'registryService',
        'processService',
        '$uibModal',
        'geolocationService',
        function ($scope, $log, toastr, responseDispatcherService, processInstanceService, $q, NgTableParams, ngTableService, $state, registryService, processService, $uibModal, geolocationService) {
            //#region Model
            $scope.model = {
                name: null,
                doctor: null,
                patient: null,
                doctorList: [],
                patientList: [],
                processList: [],
                isCtrlReady: false,
                isFetchBusy: false
            };
            //#endregion

            $scope.$watchGroup(['model.doctor', 'model.patient', 'model.process'],
                function (newValues, oldValues, scope) {
                    $scope.model.processInstanceList = [];
                });

            //#region Behaviors

            $scope.clearFilter = function () {
                $scope.model.name = null;
                $scope.model.doctor = null;
                $scope.model.patient = null;
                $scope.model.process = null;
                $scope.model.isFetchBusy = false;
                $scope.model.processInstanceList = [];
            }

            $scope.fetch = function (startRowIndex, maxRowIndex, orderByProperty, orderByType) {
                $scope.model.isFetchBusy = true;

                var deferred = $q.defer();

                var request =
                {
                    'name': $scope.model.name,
                    'doctorId': $scope.model.doctor ? $scope.model.doctor.id : '',
                    'patientId': $scope.model.patient ? $scope.model.patient.id : '',
                    'processId': $scope.model.process? $scope.model.process.id : '',
                    "startRowIndex": startRowIndex,
                    "maxRowIndex": maxRowIndex,
                    "orderByProperty": orderByProperty || "",
                    "orderByType": orderByType || ""
                };

                processInstanceService.fetch(request)
                    .then(function (response) {
                        var data = response.data;

                        $scope.model.processInstanceList = [];
                        angular.forEach(data.data, function (dto) {
                            var model = processInstanceService.processInstanceIndexModel(dto);
                            $scope.model.processInstanceList.push(model);
                        });

                        deferred.resolve({ "models": $scope.model.processInstanceList, "count": data.count });
                    }, function (error) {
                        //Loggo gli errori
                        $log.warn("Errore in fetch: " + JSON.stringify(error));
                    }).finally(function () {
                        $scope.model.isFetchBusy = false;
                    });

                return deferred.promise;
            }

            $scope.navigateUpdate = function (item) {
                if (!item) {
                    $log.error("Item is null!");
                    return;
                }

                $state.go('shell.processInstance-update', {'id': item.id});
            }

            //$scope.delete = function (item) {
            //    if (!item) {
            //        $log.error("Item is null!");
            //        return;
            //    }

            //    //Definisco la richiesta
            //    var request = { 'id': item.id }

            //    //Eseguo la richiesta al server
            //    var promise = processInstanceService.delete(request);

            //    //Valuto la risposta
            //    promise.then(function (response) {
            //        //var index = $scope.model.processInstanceList.indexOf(item);
            //        //$scope.model.processInstanceList.splice(index, 1);
            //        $scope.processInstanceListNgTable.reload();
            //        toastr.success($rootScope.validationMessages.deleteSuccess);
            //    }, function (error) {
            //        var data = error.data.data;
            //        responseDispatcherService.printErrorMessages(data, toastr);
            //        $log.error("Cancellazione entity fallita in processInstance-indexCtrl" + JSON.stringify(error));
            //    }).finally(function () {
            //    });
            //}

            $scope.openGeolocation = function (processInstanceId) {
                var deferred = $q.defer();

                var request = { "processInstanceId": processInstanceId };

                var promise = geolocationService.getProcessInstanceGeo(request);
                
                promise.then(function (dto) {
                    
                    var geoLocationModel = geolocationService.geoLocationModel(dto.data);
                    
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

                }, function (error) {
                    //Visualizzo gli errori
                    responseDispatcherService.printErrorMessages(error.data.data, toastr);
                    //Loggo gli errori
                    $log.error("Errore in editCtrl: " + JSON.stringify(error));
                    deferred.reject(error);
                });

                return deferred.promise;
            }

            var fetchEssentialData = function () {
                var deferred = $q.defer();

                var request = {};

                processInstanceService.fetchEssentialData(request)
                    .then(function (response) {
                        var data = response ? response.data : {};

                        $scope.model.doctorList = Enumerable.from(data.doctorList).select(function (x) { return registryService.registryDetailModel(x) }).toArray();
                        $scope.model.patientList = Enumerable.from(data.patientList).select(function (x) { return registryService.registryDetailModel(x) }).toArray();
                        $scope.model.processList = Enumerable.from(data.processList).select(function (x) { return processService.processUpdateModel(x) }).toArray();

                        deferred.resolve(response);
                    }, function (error) {
                        $log.error("Errore in fetchEssentialData: " + JSON.stringify(error));
                        deferred.reject(error);
                    }).finally(function () {
                    });

                return deferred.promise;
            }
            //#endregion

            $scope.processInstanceListNgTable = new NgTableParams({
                page: 1,
                sorting: { "InsertDate": "asc" }
            },
                {
                    getData: function (params) {
                        var paginationIndexs = ngTableService.buildPaginatorIndexs(params);
                        return $scope.fetch(paginationIndexs.startRowIndex, paginationIndexs.maxRowIndex, paginationIndexs.orderByProperty, paginationIndexs.orderByType)
                            .then(function (data) {
                                params.total(data.count);
                                return data.models;
                            });
                    }
                });

            var constructor = function () {
                $scope.model.isCtrlReady = false;

                var essentialDataPromises = [];
                essentialDataPromises.push(fetchEssentialData());

                $q.all(essentialDataPromises).then(function () {
                    $scope.model.isCtrlReady = true;
                    //$scope.fetch();
                });
            };

            constructor();
        }]);